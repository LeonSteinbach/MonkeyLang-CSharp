using System.Diagnostics;

namespace Interpreter
{
	public class Application
	{
		public static void Main(string[] args)
		{
			string text = "let fib = fn(n) { if (n < 2) { return n; }; fib(n-1) + fib(n-2); }; fib(30);";

			Lexer lexer = new Lexer(text);
			Token token;
			Stopwatch stopwatch = Stopwatch.StartNew();
			do
			{
				token = lexer.NextToken();
				Console.WriteLine(token);
			} while (token.Type != TokenType.EOF);

			Console.WriteLine("Lexer [ms]: " + (int) stopwatch.Elapsed.TotalMilliseconds);

			/*
			Stopwatch stopwatch1 = Stopwatch.StartNew();
			Lexer lexer = new Lexer(text);
			Parser parser = new Parser(lexer);
			Environment environment = new Environment();

			Program program = parser.ParseProgram();
			stopwatch1.Stop();

			Console.WriteLine();
			Parser.PrintProgram(program);
			Console.WriteLine();

			foreach (string error in parser.Errors)
				Console.WriteLine(error);

			Console.WriteLine();
			Stopwatch stopwatch2 = Stopwatch.StartNew();
			Console.WriteLine(Evaluator.Evaluate(program, environment));
			stopwatch2.Stop();

			Console.WriteLine();
			Console.WriteLine("Lexer + Parser [ms]: " + (int)stopwatch1.Elapsed.TotalMilliseconds);
			Console.WriteLine("Evaluator [ms]:      " + (int)stopwatch2.Elapsed.TotalMilliseconds);
			*/
			Console.ReadKey();
		}
	}
}