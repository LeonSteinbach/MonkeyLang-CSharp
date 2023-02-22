using System.Diagnostics;

namespace Interpreter
{
	class Application
	{
		public static int Main(String[] args)
		{
			string text = "let fib = fn(n) {" +
			              "    if (n < 2) {" +
			              "        return n;" +
			              "    }" +
			              "    fib (n - 1) + fib(n - 2)" +
			              "};" +
			              "fib(35);";

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

			Console.WriteLine();
			Parser.PrintProgram(program);
			Console.WriteLine();

			foreach (string error in parser.Errors)
				Console.WriteLine(error);

			Console.WriteLine();
			Console.WriteLine(Evaluator.Evaluate(program, environment));
			int elapsedMilliseconds = (int)stopwatch.Elapsed.TotalMilliseconds;
			Console.WriteLine();
			Console.WriteLine("Elapsed milliseconds: " + elapsedMilliseconds);

			Console.ReadKey();

			return 0;
		}
	}
}