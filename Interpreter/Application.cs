using System.Diagnostics;

namespace Interpreter
{
	class Application
	{
		public static int Main(String[] args)
		{
			string text = "let a = 123; !(a == -123);";

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
			Environment environment = new Environment();

			Program? program = parser.ParseProgram();
			int elapsedMilliseconds = (int)stopwatch.Elapsed.TotalMilliseconds;

			Console.WriteLine();
			Parser.PrintProgram(program);
			Console.WriteLine();
			Console.WriteLine("Elapsed milliseconds: " + elapsedMilliseconds);

			foreach (string error in parser.Errors)
				Console.WriteLine(error);

			Console.WriteLine();
			Console.WriteLine(Evaluator.Evaluate(program, environment));
			Console.WriteLine();

			Console.ReadKey();

			return 0;
		}
	}
}