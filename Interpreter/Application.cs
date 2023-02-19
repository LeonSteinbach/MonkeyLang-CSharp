namespace Interpreter
{
	class Application
	{
		public static int Main(String[] args)
		{
			string text = "let three = 1 + 2; let a = 1; let b = 0;";

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

			Console.ReadKey();

			return 0;
		}
	}
}