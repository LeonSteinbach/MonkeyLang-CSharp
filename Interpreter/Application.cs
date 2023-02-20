namespace Interpreter
{
	class Application
	{
		public static int Main(String[] args)
		{
			string text = "a + -b * c";

			/*
			Lexer lexer = new Lexer(text);

			Token token;
			do
			{
				token = lexer.NextToken();
				Console.WriteLine(token);
			} while (token.Type != TokenType.EOF);
			*/

			Lexer lexer = new Lexer(text);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();
			Console.WriteLine();
			Parser.PrintProgram(program);

			foreach (string error in parser.Errors)
				Console.WriteLine(error);

			Console.ReadKey();

			return 0;
		}
	}
}