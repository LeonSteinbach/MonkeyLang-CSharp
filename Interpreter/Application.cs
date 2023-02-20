namespace Interpreter
{
	class Application
	{
		public static int Main(String[] args)
		{
			string text = "let a = !123 " +
			              "let b = -asdf";

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
			Parser.PrintProgram(program);

			foreach (string error in parser.Errors)
				Console.WriteLine(error);

			Console.ReadKey();

			return 0;
		}
	}
}