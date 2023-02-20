namespace Interpreter
{
	public class Parser
	{
		public Lexer Lexer { get; set; }
		private Token CurrentToken { get; set; }
		private Token PeekToken { get; set; }
		public List<string> Errors { get; set; }

		public Dictionary<TokenType, Delegate> PrefixParseFunctions;
		public Dictionary<TokenType, Delegate> InfixParseFunctions;

		public Parser(Lexer lexer)
		{
			Lexer = lexer;
			Errors = new List<string>();

			PrefixParseFunctions = new Dictionary<TokenType, Delegate>();
			InfixParseFunctions = new Dictionary<TokenType, Delegate>();

			RegisterParseFunctions();

			AdvanceTokens();
		}

		private void RegisterParseFunctions()
		{
			RegisterPrefix(TokenType.IDENT, ParseIdentifier);
			RegisterPrefix(TokenType.INT, ParseIntegerLiteral);
			RegisterPrefix(TokenType.BANG, ParsePrefixExpression);
			RegisterPrefix(TokenType.MINUS, ParsePrefixExpression);
		}

		public static void PrintProgram(Program program)
		{
			Console.WriteLine(program.String(0, program.Statements.Count > 1));
		}

		private void PeekError(TokenType tokenType)
		{
			Errors.Add($"Expected next token to be {tokenType}, got {PeekToken.Type} instead.");
		}

		private void IntegerParseError(string integer)
		{
			Errors.Add($"Could not parse {integer} as integer.");
		}

		private void NoPrefixParseFunctionError(TokenType tokenType)
		{
			Errors.Add($"No prefix parse function for {tokenType}.");
		}

		private void RegisterPrefix(TokenType tokenType, Delegate prefixParseFunction)
		{
			PrefixParseFunctions[tokenType] = prefixParseFunction;
		}

		private void RegisterInfix(TokenType tokenType, Delegate infixParseFunction)
		{
			InfixParseFunctions[tokenType] = infixParseFunction;
		}

		private void AdvanceTokens()
		{
			CurrentToken = PeekToken;
			PeekToken = Lexer.NextToken();
		}

		public Program ParseProgram()
		{
			Program program = new Program();
			AdvanceTokens();

			while (CurrentToken.Type != TokenType.EOF)
			{
				Statement? statement = ParseStatement();
				if (statement != null)
					program.Statements.Add(statement);
				AdvanceTokens();
			}
			return program;
		}

		private Statement? ParseStatement()
		{
			return CurrentToken.Type switch
			{
				TokenType.LET => ParseLetStatement(),
				TokenType.RETURN => ParseReturnStatement(),
				_ => ParseExpressionStatement()
			};
		}

		private LetStatement ParseLetStatement()
		{
			LetStatement letStatement = new LetStatement { Token = CurrentToken };

			if (ExpectPeek(TokenType.IDENT) == false)
				return null;

			letStatement.Name = new Identifier() { Token = CurrentToken, Value = CurrentToken.Literal };

			if (ExpectPeek(TokenType.ASSIGN) == false)
				return null;

			AdvanceTokens();
			letStatement.Value = ParseExpression(Precedence.LOWEST);

			if (CurrentToken.Type == TokenType.SEMICOLON)
				AdvanceTokens();

			return letStatement;
		}

		private ReturnStatement ParseReturnStatement()
		{
			ReturnStatement returnStatement = new ReturnStatement { Token = CurrentToken };

			AdvanceTokens();
			returnStatement.ReturnValue = ParseExpression(Precedence.LOWEST);

			while (CurrentToken.Type != TokenType.SEMICOLON)
				AdvanceTokens();

			return returnStatement;
		}

		private ExpressionStatement ParseExpressionStatement()
		{
			ExpressionStatement expressionStatement = new ExpressionStatement { Token = CurrentToken };

			expressionStatement.Expression = ParseExpression(Precedence.LOWEST);

			if (PeekToken.Type == TokenType.SEMICOLON)
				AdvanceTokens();

			return expressionStatement;
		}

		private Expression ParseExpression(Precedence precedence)
		{
			if (PrefixParseFunctions.ContainsKey(CurrentToken.Type) == false)
			{
				NoPrefixParseFunctionError(CurrentToken.Type);
				return null;
			}

			Delegate prefix = PrefixParseFunctions[CurrentToken.Type];

			Expression leftExpression = (Expression)prefix.DynamicInvoke();
			return leftExpression;
		}

		private Expression ParseIdentifier()
		{
			return new Identifier { Token = CurrentToken, Value = CurrentToken.Literal };
		}

		private Expression ParseIntegerLiteral()
		{
			IntegerLiteral integerLiteral = new IntegerLiteral { Token = CurrentToken };
			int value;
			bool error = int.TryParse(CurrentToken.Literal, out value);
			if (error == false)
			{
				IntegerParseError(CurrentToken.Literal);
				return null;
			}

			integerLiteral.Value = value;
			return integerLiteral;
		}

		private Expression ParsePrefixExpression()
		{
			PrefixExpression prefixExpression = new PrefixExpression {Token = CurrentToken, Operator = CurrentToken.Literal};

			AdvanceTokens();
			prefixExpression.RightExpression = ParseExpression(Precedence.PREFIX);

			return prefixExpression;
		}

		private bool ExpectPeek(TokenType tokenType)
		{
			if (PeekToken.Type == tokenType)
			{
				AdvanceTokens();
				return true;
			}

			PeekError(tokenType);
			return false;
		}
	}

	public class ParserException : Exception
	{
		public ParserException() {}
		public ParserException(string message) : base(message) {}
		public ParserException(string message, Exception inner) : base(message, inner) {}
	}

	public enum Precedence
	{
		LOWEST,
		EQUALS,
		LESSGREATER,
		SUM,
		PRODUCT,
		PREFIX,
		CALL
	}
}
