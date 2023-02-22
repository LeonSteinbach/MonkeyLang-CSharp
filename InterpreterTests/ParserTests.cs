using Boolean = Interpreter.Boolean;

namespace InterpreterTests
{
	[TestClass]
	public class ParserTests
	{
		[TestMethod]
		public void TestLetStatement()
		{
			string input1 = "let x = 0;" +
							"let y = 1;";
			Lexer lexer1 = new Lexer(input1);
			Parser parser1 = new Parser(lexer1);

			Program program1 = parser1.ParseProgram();

			Assert.IsTrue(parser1.Errors.Count == 0);
			Assert.IsTrue(program1.Statements.Count == 2);

			Assert.IsTrue(((LetStatement)program1.Statements[0]).Token.Type == TokenType.LET);
			Assert.IsTrue(((LetStatement)program1.Statements[0]).Token.Literal == "let");
			Assert.IsTrue(((LetStatement)program1.Statements[0]).Name.Value == "x");

			Assert.IsTrue(((LetStatement)program1.Statements[1]).Token.Type == TokenType.LET);
			Assert.IsTrue(((LetStatement)program1.Statements[1]).Token.Literal == "let");
			Assert.IsTrue(((LetStatement)program1.Statements[1]).Name.Value == "y");

			string input2 = "let x 1; let = 1; let; let x = 1;";
			Lexer lexer2 = new Lexer(input2);
			Parser parser2 = new Parser(lexer2);

			parser2.ParseProgram();
			Assert.IsTrue(parser2.Errors.Count == 5);
		}

		[TestMethod]
		public void TestReturnStatement()
		{
			string input = "return 0; return asdf; return 1 + 2; return fn(a) { return a; };";
			Lexer lexer = new Lexer(input);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();

			Assert.IsTrue(parser.Errors.Count == 0);
			Assert.IsTrue(program.Statements.Count == 4);

			Assert.IsTrue(((ReturnStatement)program.Statements[0]).Token.Type == TokenType.RETURN);
			Assert.IsTrue(((ReturnStatement)program.Statements[0]).Token.Literal == "return");
			Assert.IsTrue(((ReturnStatement)program.Statements[1]).Token.Type == TokenType.RETURN);
			Assert.IsTrue(((ReturnStatement)program.Statements[1]).Token.Literal == "return");
			Assert.IsTrue(((ReturnStatement)program.Statements[2]).Token.Type == TokenType.RETURN);
			Assert.IsTrue(((ReturnStatement)program.Statements[2]).Token.Literal == "return");
		}

		[TestMethod]
		public void TestIdentifiers()
		{
			string input = "a; a123b;";
			Lexer lexer = new Lexer(input);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();

			Assert.IsTrue(parser.Errors.Count == 0);
			Assert.IsTrue(program.Statements.Count == 2);

			Assert.IsTrue(((Identifier)((ExpressionStatement)program.Statements[0]).Expression).Token.Type == TokenType.IDENT);
			Assert.IsTrue(((Identifier)((ExpressionStatement)program.Statements[0]).Expression).Value == "a");
			Assert.IsTrue(((Identifier)((ExpressionStatement)program.Statements[1]).Expression).Token.Type == TokenType.IDENT);
			Assert.IsTrue(((Identifier)((ExpressionStatement)program.Statements[1]).Expression).Value == "a123b");
		}

		[TestMethod]
		public void TestIntegerLiterals()
		{
			string input = "0; 1; 123;";
			Lexer lexer = new Lexer(input);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();

			Assert.IsTrue(parser.Errors.Count == 0);
			Assert.IsTrue(program.Statements.Count == 3);

			Assert.IsTrue(((IntegerLiteral)((ExpressionStatement)program.Statements[0]).Expression).Token.Type == TokenType.INT);
			Assert.IsTrue(((IntegerLiteral)((ExpressionStatement)program.Statements[0]).Expression).Value == 0);
			Assert.IsTrue(((IntegerLiteral)((ExpressionStatement)program.Statements[1]).Expression).Token.Type == TokenType.INT);
			Assert.IsTrue(((IntegerLiteral)((ExpressionStatement)program.Statements[1]).Expression).Value == 1);
			Assert.IsTrue(((IntegerLiteral)((ExpressionStatement)program.Statements[2]).Expression).Token.Type == TokenType.INT);
			Assert.IsTrue(((IntegerLiteral)((ExpressionStatement)program.Statements[2]).Expression).Value == 123);
		}

		[TestMethod]
		public void TestPrefixOperators()
		{
			string input = "!a; !1; !foo(); !!a; -a; -0; -foo(); --a;";
			Lexer lexer = new Lexer(input);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();

			Assert.IsTrue(parser.Errors.Count == 0);
			Assert.IsTrue(program.Statements.Count == 8);

			Assert.IsTrue(((ExpressionStatement)program.Statements[0]).Token.Literal == "!");
			Assert.IsTrue(((ExpressionStatement)program.Statements[0]).Token.Type == TokenType.BANG);
			Assert.IsTrue(((ExpressionStatement)program.Statements[1]).Token.Type == TokenType.BANG);
			Assert.IsTrue(((ExpressionStatement)program.Statements[2]).Token.Type == TokenType.BANG);
			Assert.IsTrue(((ExpressionStatement)program.Statements[3]).Token.Type == TokenType.BANG);
			Assert.IsTrue(((PrefixExpression)((ExpressionStatement)program.Statements[3]).Expression).Token.Type == TokenType.BANG);

			Assert.IsTrue(((ExpressionStatement)program.Statements[4]).Token.Literal == "-");
			Assert.IsTrue(((ExpressionStatement)program.Statements[4]).Token.Type == TokenType.MINUS);
			Assert.IsTrue(((ExpressionStatement)program.Statements[5]).Token.Type == TokenType.MINUS);
			Assert.IsTrue(((ExpressionStatement)program.Statements[6]).Token.Type == TokenType.MINUS);
			Assert.IsTrue(((ExpressionStatement)program.Statements[7]).Token.Type == TokenType.MINUS);
			Assert.IsTrue(((PrefixExpression)((ExpressionStatement)program.Statements[7]).Expression).Token.Type == TokenType.MINUS);
		}

		[TestMethod]
		public void TestInfixOperators()
		{
			string input = "1 + 1; 1 - 1; 1 * 1; 1 / 1; 1 < 1; 1 > 1; 1 == 1; 1 != 1;";
			Lexer lexer = new Lexer(input);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();

			Assert.IsTrue(parser.Errors.Count == 0);
			Assert.IsTrue(program.Statements.Count == 8);

			Assert.IsTrue(((InfixExpression)((ExpressionStatement)(program.Statements[0])).Expression).Operator == "+");
			Assert.IsTrue(((InfixExpression)((ExpressionStatement)(program.Statements[1])).Expression).Operator == "-");
			Assert.IsTrue(((InfixExpression)((ExpressionStatement)(program.Statements[2])).Expression).Operator == "*");
			Assert.IsTrue(((InfixExpression)((ExpressionStatement)(program.Statements[3])).Expression).Operator == "/");
			Assert.IsTrue(((InfixExpression)((ExpressionStatement)(program.Statements[4])).Expression).Operator == "<");
			Assert.IsTrue(((InfixExpression)((ExpressionStatement)(program.Statements[5])).Expression).Operator == ">");
			Assert.IsTrue(((InfixExpression)((ExpressionStatement)(program.Statements[6])).Expression).Operator == "==");
			Assert.IsTrue(((InfixExpression)((ExpressionStatement)(program.Statements[7])).Expression).Operator == "!=");
		}

		[TestMethod]
		public void TestBooleanLiterals()
		{
			string input = "true; false;";
			Lexer lexer = new Lexer(input);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();

			Assert.IsTrue(parser.Errors.Count == 0);
			Assert.IsTrue(program.Statements.Count == 2);

			Assert.IsTrue(((BooleanLiteral)((ExpressionStatement)(program.Statements[0])).Expression).Value == true);
			Assert.IsTrue(((BooleanLiteral)((ExpressionStatement)(program.Statements[1])).Expression).Value == false);
		}

		[TestMethod]
		public void TestGroupedExpressions()
		{
			string input = "(1 + 2) * 3;";
			Lexer lexer = new Lexer(input);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();

			Assert.IsTrue(parser.Errors.Count == 0);
			Assert.IsTrue(program.Statements.Count == 1);

			Assert.IsTrue(((InfixExpression)((ExpressionStatement)(program.Statements[0])).Expression).Operator == "*");
			Assert.IsTrue(((InfixExpression)((InfixExpression)((ExpressionStatement)(program.Statements[0])).Expression).LeftExpression).Operator == "+");
		}

		[TestMethod]
		public void TestIfExpressions()
		{
			string input = "if (a > 0) { return a; }; if (b < 0) { } else { return b; };";
			Lexer lexer = new Lexer(input);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();

			Assert.IsTrue(parser.Errors.Count == 0);
			Assert.IsTrue(program.Statements.Count == 2);

			Assert.IsTrue(((InfixExpression)((IfExpression)((ExpressionStatement)(program.Statements[0])).Expression).Condition).Operator == ">");
			Assert.IsTrue(((IfExpression)((ExpressionStatement)(program.Statements[0])).Expression).Consequence.Statements.Count == 1);
			Assert.IsTrue(((IfExpression)((ExpressionStatement)(program.Statements[0])).Expression).Alternative == null);

			Assert.IsTrue(((InfixExpression)((IfExpression)((ExpressionStatement)(program.Statements[1])).Expression).Condition).Operator == "<");
			Assert.IsTrue(((IfExpression)((ExpressionStatement)(program.Statements[1])).Expression).Consequence.Statements.Count == 0);
			Assert.IsTrue(((IfExpression)((ExpressionStatement)(program.Statements[1])).Expression).Alternative.Statements.Count == 1);
		}

		[TestMethod]
		public void TestFunctionLiterals()
		{
			string input = "fn (a, b) { a + b; };";
			Lexer lexer = new Lexer(input);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();

			Assert.IsTrue(parser.Errors.Count == 0);
			Assert.IsTrue(program.Statements.Count == 1);

			Assert.IsTrue(((FunctionLiteral)((ExpressionStatement)(program.Statements[0])).Expression).Parameters.Count == 2);
			Assert.IsTrue(((FunctionLiteral)((ExpressionStatement)(program.Statements[0])).Expression).Body.Statements.Count == 1);
		}

		[TestMethod]
		public void TestCallExpressions()
		{
			string input = "add(1, 2); foo(3, bar(4, 5));";
			Lexer lexer = new Lexer(input);
			Parser parser = new Parser(lexer);

			Program program = parser.ParseProgram();

			Assert.IsTrue(parser.Errors.Count == 0);
			Assert.IsTrue(program.Statements.Count == 2);

			Assert.IsTrue(((Identifier)((CallExpression)((ExpressionStatement)(program.Statements[0])).Expression).Function).Value == "add");
			Assert.IsTrue(((CallExpression)((ExpressionStatement)(program.Statements[0])).Expression).Arguments.Count == 2);

			Assert.IsTrue(((Identifier)((CallExpression)((ExpressionStatement)(program.Statements[1])).Expression).Function).Value == "foo");
			Assert.IsTrue(((CallExpression)((ExpressionStatement)(program.Statements[1])).Expression).Arguments.Count == 2);
			Assert.IsTrue(((Identifier)((CallExpression)((CallExpression)((ExpressionStatement)(program.Statements[1])).Expression).Arguments[1]).Function).Value == "bar");
		}
	}
}