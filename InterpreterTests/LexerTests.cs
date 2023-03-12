namespace InterpreterTests
{
	[TestClass]
	public class LexerTests
	{
		private void AssertTokensEqual(List<Token> targets, List<Token> results)
		{
			Assert.IsTrue(targets.Count == results.Count);

			for (int i = 0; i < targets.Count; i++)
			{
				Assert.IsTrue(targets[i] == results[i]);
			}
		}

		private void LexString(string input, List<Token> results)
		{
			Lexer lexer = new Lexer(input);

			Token token;
			do
			{
				token = lexer.NextToken();
				results.Add(token);
			} while (token.Type != TokenType.EOF);
		}

		[TestMethod]
		public void TestIdentifierCharacterTokens()
		{
			string input = "fn let true false if else return asdf qwerty abc 1abc";
			List<Token> targets = new List<Token>
			{
				new(TokenType.FUNCTION, "fn"),
				new(TokenType.LET, "let"),
				new(TokenType.TRUE, "true"),
				new(TokenType.FALSE, "false"),
				new(TokenType.IF, "if"),
				new(TokenType.ELSE, "else"),
				new(TokenType.RETURN, "return"),
				new(TokenType.IDENT, "asdf"),
				new(TokenType.IDENT, "qwerty"),
				new(TokenType.IDENT, "abc"),
				new(TokenType.ILLEGAL, "1abc"),
				new(TokenType.EOF, ""),
			};
			List<Token> results = new List<Token>();

			LexString(input, results);
			AssertTokensEqual(targets, results);
		}

		[TestMethod]
		public void TestSignleCharacterTokens()
		{
			string input = ";(){},\"\"";
			List<Token> targets = new List<Token>
			{
				new(TokenType.SEMICOLON, ";"),
				new(TokenType.LPAREN, "("),
				new(TokenType.RPAREN, ")"),
				new(TokenType.LBRACE, "{"),
				new(TokenType.RBRACE, "}"),
				new(TokenType.COMMA, ","),
				new(TokenType.STRING, ""),
				new(TokenType.EOF, ""),
			};
			List<Token> results = new List<Token>();

			LexString(input, results);
			AssertTokensEqual(targets, results);
		}

		[TestMethod]
		public void TestDoubleCharacterTokens()
		{
			string input = "= == ! != + - < > * /";
			List<Token> targets = new List<Token>
			{
				new(TokenType.ASSIGN, "="),
				new(TokenType.EQ, "=="),
				new(TokenType.BANG, "!"),
				new(TokenType.NEQ, "!="),
				new(TokenType.PLUS, "+"),
				new(TokenType.MINUS, "-"),
				new(TokenType.LT, "<"),
				new(TokenType.GT, ">"),
				new(TokenType.ASTERIX, "*"),
				new(TokenType.SLASH, "/"),
				new(TokenType.EOF, ""),
			};
			List<Token> results = new List<Token>();

			LexString(input, results);
			AssertTokensEqual(targets, results);
		}

		[TestMethod]
		public void TestIllegalCharacterTokens()
		{
			string input = "#$%&";
			List<Token> targets = new List<Token>
			{
				new(TokenType.ILLEGAL, "#"),
				new(TokenType.ILLEGAL, "$"),
				new(TokenType.ILLEGAL, "%"),
				new(TokenType.ILLEGAL, "&"),
				new(TokenType.EOF, ""),
			};
			List<Token> results = new List<Token>();

			LexString(input, results);
			AssertTokensEqual(targets, results);
		}

		[TestMethod]
		public void TestDigitCharacterTokens()
		{
			string input = "0 1 2 3 4 5 6 7 8 9 123 999999 0123 00";
			List<Token> targets = new List<Token>
			{
				new(TokenType.INT, "0"),
				new(TokenType.INT, "1"),
				new(TokenType.INT, "2"),
				new(TokenType.INT, "3"),
				new(TokenType.INT, "4"),
				new(TokenType.INT, "5"),
				new(TokenType.INT, "6"),
				new(TokenType.INT, "7"),
				new(TokenType.INT, "8"),
				new(TokenType.INT, "9"),
				new(TokenType.INT, "123"),
				new(TokenType.INT, "999999"),
				new(TokenType.ILLEGAL, "0123"),
				new(TokenType.ILLEGAL, "00"),
				new(TokenType.EOF, ""),
			};
			List<Token> results = new List<Token>();

			LexString(input, results);
			AssertTokensEqual(targets, results);
		}
	}
}