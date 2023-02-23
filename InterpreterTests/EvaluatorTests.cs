namespace InterpreterTests
{
	[TestClass]
	public class EvaluatorTests
	{
		[TestMethod]
		public void TestIntegers()
		{
			string input1 = "1;";
			Object result1 = Evaluator.Evaluate(new Parser(new Lexer(input1)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer) result1).Value == 1);

			string input2 = "-1;";
			Object result2 = Evaluator.Evaluate(new Parser(new Lexer(input2)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer) result2).Value == -1);

			string input3 = "--1;";
			Object result3 = Evaluator.Evaluate(new Parser(new Lexer(input3)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer) result3).Value == 1);
		}

		[TestMethod]
		public void TestBooleans()
		{
			string input1 = "true;";
			Object result1 = Evaluator.Evaluate(new Parser(new Lexer(input1)).ParseProgram(), new Environment());
			Assert.IsTrue(((Boolean) result1).Value == true);

			string input2 = "false;";
			Object result2 = Evaluator.Evaluate(new Parser(new Lexer(input2)).ParseProgram(), new Environment());
			Assert.IsTrue(((Boolean) result2).Value == false);

			string input3 = "!true;";
			Object result3 = Evaluator.Evaluate(new Parser(new Lexer(input3)).ParseProgram(), new Environment());
			Assert.IsTrue(((Boolean) result3).Value == false);

			string input4 = "!false;";
			Object result4 = Evaluator.Evaluate(new Parser(new Lexer(input4)).ParseProgram(), new Environment());
			Assert.IsTrue(((Boolean) result4).Value == true);

			string input5 = "!!false;";
			Object result5 = Evaluator.Evaluate(new Parser(new Lexer(input5)).ParseProgram(), new Environment());
			Assert.IsTrue(((Boolean) result5).Value == false);
		}

		[TestMethod]
		public void TestNull()
		{
			string input1 = "let a = 0;";
			Object result1 = Evaluator.Evaluate(new Parser(new Lexer(input1)).ParseProgram(), new Environment());
			Assert.IsTrue(result1.Type() == ObjectType.NULL);
		}

		[TestMethod]
		public void TestInfixExpressions()
		{
			string input1 = "2 + 3;";
			Object result1 = Evaluator.Evaluate(new Parser(new Lexer(input1)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer) result1).Value == 5);

			string input2 = "2 - 3;";
			Object result2 = Evaluator.Evaluate(new Parser(new Lexer(input2)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer) result2).Value == -1);

			string input3 = "2 * 3;";
			Object result3 = Evaluator.Evaluate(new Parser(new Lexer(input3)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer) result3).Value == 6);

			string input4 = "3 / 2;";
			Object result4 = Evaluator.Evaluate(new Parser(new Lexer(input4)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer) result4).Value == 1);

			string input5 = "(1 == 1) == true;";
			Object result5 = Evaluator.Evaluate(new Parser(new Lexer(input5)).ParseProgram(), new Environment());
			Assert.IsTrue(((Boolean) result5).Value == true);

			string input6 = "(1 != 1) == false;";
			Object result6 = Evaluator.Evaluate(new Parser(new Lexer(input6)).ParseProgram(), new Environment());
			Assert.IsTrue(((Boolean) result6).Value == true);
		}

		[TestMethod]
		public void TestGroupedExpressions()
		{
			string input1 = "(1 + 2) * 3;";
			Object result1 = Evaluator.Evaluate(new Parser(new Lexer(input1)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer) result1).Value == 9);

			string input2 = "((1 + 2) * (3 - -5) / 2) + (1 / 1) - 0;";
			Object result2 = Evaluator.Evaluate(new Parser(new Lexer(input2)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer)result2).Value == 13);
		}

		[TestMethod]
		public void TestLetStatements()
		{
			string input1 = "let a = 1; let b = a * 2; b + a;";
			Object result1 = Evaluator.Evaluate(new Parser(new Lexer(input1)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer)result1).Value == 3);
		}

		[TestMethod]
		public void TestIfStatements()
		{
			string input1 = "if (1 > 0) { if (1 < 0) { 1 } else { 2 } } else { 3 };";
			Object result1 = Evaluator.Evaluate(new Parser(new Lexer(input1)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer)result1).Value == 2);
		}

		[TestMethod]
		public void TestFunctions()
		{
			string input1 = "let fib = fn(n) { if (n < 2) { return n }; fib(n-1) + fib(n-2); }; fib(6);";
			Object result1 = Evaluator.Evaluate(new Parser(new Lexer(input1)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer)result1).Value == 8);

			string input2 = "let getone = fn() { 1; }; getone();";
			Object result2 = Evaluator.Evaluate(new Parser(new Lexer(input2)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer)result2).Value == 1);

			string input3 = "let getnull = fn() { }; getnull();";
			Object result3 = Evaluator.Evaluate(new Parser(new Lexer(input3)).ParseProgram(), new Environment());
			Assert.IsTrue(result3.Type() == ObjectType.NULL);

			string input4 = "let gettwo = fn() { 1; return 2; 3; }; gettwo();";
			Object result4 = Evaluator.Evaluate(new Parser(new Lexer(input4)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer)result4).Value == 2);

			string input5 = "let bar = 1; let foo = fn(x) { let bar = 2; bar + x; }; foo(bar);";
			Object result5 = Evaluator.Evaluate(new Parser(new Lexer(input5)).ParseProgram(), new Environment());
			Assert.IsTrue(((Integer)result5).Value == 3);
		}
	}
}