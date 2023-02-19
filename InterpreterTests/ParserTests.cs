namespace InterpreterTests
{
	[TestClass]
	public class ParserTests
	{
		[TestMethod]
		public void TestLetStatement()
		{
			string input = "let x = 0;" +
			               "let y = 1;";
			Lexer lexer = new Lexer(input);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();

			Assert.IsTrue(program.Statements.Count == 2);

			Assert.IsTrue(((LetStatement)program.Statements[0]).Token.Type == TokenType.LET);
			Assert.IsTrue(((LetStatement)program.Statements[0]).Token.Literal == "let");
			Assert.IsTrue(((LetStatement)program.Statements[0]).Name.Value == "x");
			Assert.IsTrue(((LetStatement)program.Statements[0]).Value == null);

			Assert.IsTrue(((LetStatement)program.Statements[1]).Token.Type == TokenType.LET);
			Assert.IsTrue(((LetStatement)program.Statements[1]).Token.Literal == "let");
			Assert.IsTrue(((LetStatement)program.Statements[1]).Name.Value == "y");
			Assert.IsTrue(((LetStatement)program.Statements[1]).Value == null);
		}
	}
}