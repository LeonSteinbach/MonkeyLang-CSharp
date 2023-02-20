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
			Console.WriteLine("{");

			for (int i = 0; i < program.Statements.Count; i++)
			{
				PrintNode(program.Statements[i], 1, i < program.Statements.Count - 1);
			}

			Console.WriteLine("}");
		}

		private static void PrintNode(Node node, int level, bool nextTokenExists)
		{
			if (node is LetStatement letStatement)
			{
				PrintIndented(letStatement.GetType().Name + ": {", level);
				PrintIndented("type: " + letStatement.Token.Type, level + 1);
				PrintIndented("name: " + letStatement.Name.Value, level + 1);
				PrintIndented("value: {", level + 1);
				PrintNode(letStatement.Value, level + 2, false);
				PrintIndented("}", level + 1);
				PrintIndented(nextTokenExists ? "}," : "}", level);
			}
			else if (node is ReturnStatement returnStatement)
			{
				PrintIndented(returnStatement.GetType().Name + ": {", level);
				PrintIndented("type: " + returnStatement.Token.Type, level + 1);
				PrintIndented("value: {", level + 1);
				PrintNode(returnStatement.ReturnValue, level + 2, false);
				PrintIndented("}", level + 1);
				PrintIndented(nextTokenExists ? "}," : "}", level);
			}
		}

		private static void PrintIndented(string text, int level, bool newLine = true)
		{
			for (int i = 0; i < level; i++)
				Console.Write("  ");
			Console.Write(text);

			if (newLine)
				Console.WriteLine();
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
