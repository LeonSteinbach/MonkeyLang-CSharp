namespace Interpreter
{
	public class Parser
	{
		public Lexer Lexer { get; set; }
		private Token CurrentToken { get; set; }
		private Token PeekToken { get; set; }
		public List<string> Errors { get; set; }

		public Parser(Lexer lexer)
		{
			Lexer = lexer;
			Errors = new List<string>();

			AdvanceTokens();
		}

		public static void PrintProgram(Program program)
		{
			Console.WriteLine(program.String(0, program.Statements.Count > 1));
		}

		private void PeekError(TokenType tokenType)
		{
			Errors.Add($"Expected next token to be {tokenType}, got {PeekToken.Type} instead.");
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
				_ => null
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

			// TODO: Parse expressions

			while (CurrentToken.Type != TokenType.SEMICOLON)
				AdvanceTokens();

			return letStatement;
		}

		private ReturnStatement ParseReturnStatement()
		{
			ReturnStatement returnStatement = new ReturnStatement { Token = CurrentToken };

			AdvanceTokens();

			// TODO: Parse expressions

			while (CurrentToken.Type != TokenType.SEMICOLON)
				AdvanceTokens();

			return returnStatement;
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
}
