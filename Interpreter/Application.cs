using System.Diagnostics;

namespace Interpreter
{
	public class Application
	{
		public static void Main(string[] args)
		{
			string text = "len(\"hello world\");";

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

			Program program = parser.ParseProgram();

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
		}
	}
}