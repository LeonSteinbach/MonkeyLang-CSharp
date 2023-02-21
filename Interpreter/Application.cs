using System.Diagnostics;

namespace Interpreter
{
	class Application
	{
		public static int Main(String[] args)
		{
			string text = "let func = fn(a, b) {" +
			              "    return fn(c, d) { return a + b + c + d; };" +
			              "}";

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