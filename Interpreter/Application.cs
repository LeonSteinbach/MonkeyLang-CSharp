﻿using System.Diagnostics;

namespace Interpreter
{
	class Application
	{
		public static int Main(String[] args)
		{
			string text = "let a = (1 + 2) * 3 " +
			              "let a = 1 + 2 * 3";

			/*
			Lexer lexer = new Lexer(text);

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