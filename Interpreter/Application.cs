using System.Diagnostics;

namespace Interpreter
{
	class Application
	{
		public static int Main(String[] args)
		{
			string text = "let mult = fn(a, b) { a * b };" +
			              "let twice = fn(x) { let factor = 2; return mult(x, factor); };" +
			              "let two = twice(1);" +
			              "let three = 3;" +
			              "if (two < tree) { true } else { false };" +
			              "(a + b) * c";

			/*
			string input = "= == ! != + - < > * /";

			Lexer lexer = new Lexer(input);

			Token token;
			do
			{
				token = lexer.NextToken();
				Console.WriteLine(token);
			} while (token.Type != TokenType.EOF);
			*/

			Stopwatch stopwatch = Stopwatch.StartNew();
			Lexer lexer = new Lexer(text);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();
			int elapsedMilliseconds = (int)stopwatch.Elapsed.TotalMilliseconds;

			Console.WriteLine();
			Parser.PrintProgram(program);
			Console.WriteLine();
			Console.WriteLine("Elapsed milliseconds: " + elapsedMilliseconds);

			foreach (string error in parser.Errors)
				Console.WriteLine(error);

			Console.ReadKey();

			return 0;
		}
	}
}