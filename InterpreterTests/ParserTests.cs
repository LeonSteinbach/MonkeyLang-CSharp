namespace InterpreterTests
{
	[TestClass]
	public class ParserTests
	{
		[TestMethod]
		public void TestLetStatement()
		{
			string input1 = "let x = 0;" +
			               "let y = 1;";
			Lexer lexer1 = new Lexer(input1);
			Parser parser1 = new Parser(lexer1);

			Program program1 = parser1.ParseProgram();

			Assert.IsTrue(parser1.Errors.Count == 0);
			Assert.IsTrue(program1.Statements.Count == 2);

			Assert.IsTrue(((LetStatement)program1.Statements[0]).Token.Type == TokenType.LET);
			Assert.IsTrue(((LetStatement)program1.Statements[0]).Token.Literal == "let");
			Assert.IsTrue(((LetStatement)program1.Statements[0]).Name.Value == "x");
			Assert.IsTrue(((LetStatement)program1.Statements[0]).Value == null);

			Assert.IsTrue(((LetStatement)program1.Statements[1]).Token.Type == TokenType.LET);
			Assert.IsTrue(((LetStatement)program1.Statements[1]).Token.Literal == "let");
			Assert.IsTrue(((LetStatement)program1.Statements[1]).Name.Value == "y");
			Assert.IsTrue(((LetStatement)program1.Statements[1]).Value == null);

			string input2 = "let return; let x 1; let = 1; let; let x = 1;";
			Lexer lexer2 = new Lexer(input2);
			Parser parser2 = new Parser(lexer2);

			Program program2 = parser2.ParseProgram();
			Assert.IsTrue(parser2.Errors.Count == 4);
		}

		[TestMethod]
		public void TestReturnStatement()
		{
			string input = "return; return 0; return fn(a, b);";
			Lexer lexer = new Lexer(input);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();

			Assert.IsTrue(parser.Errors.Count == 0);
			Assert.IsTrue(program.Statements.Count == 3);

			Assert.IsTrue(((ReturnStatement)program.Statements[0]).Token.Type == TokenType.RETURN);
			Assert.IsTrue(((ReturnStatement)program.Statements[0]).Token.Literal == "return");
			Assert.IsTrue(((ReturnStatement)program.Statements[0]).ReturnValue == null);

			Assert.IsTrue(((ReturnStatement)program.Statements[1]).Token.Type == TokenType.RETURN);
			Assert.IsTrue(((ReturnStatement)program.Statements[1]).Token.Literal == "return");
			Assert.IsTrue(((ReturnStatement)program.Statements[1]).ReturnValue == null);

			Assert.IsTrue(((ReturnStatement)program.Statements[2]).Token.Type == TokenType.RETURN);
			Assert.IsTrue(((ReturnStatement)program.Statements[2]).Token.Literal == "return");
			Assert.IsTrue(((ReturnStatement)program.Statements[2]).ReturnValue == null);
		}
	}
}