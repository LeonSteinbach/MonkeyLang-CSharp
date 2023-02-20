namespace Interpreter
{
	public interface Node
	{
		public string TokenLiteral();
	}

	public interface Statement : Node {}

	public interface Expression : Node {}

	public class Program : Node
	{
		public List<Statement> Statements { get; set; }

		public Program()
		{
			Statements = new List<Statement>();
		}

		public string TokenLiteral()
		{
			return Statements.Count > 0 ? Statements[0].TokenLiteral() : string.Empty;
		}
	}

	public class Identifier : Expression
	{
		public Token Token { get; set; }
		public string Value { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}
	}

	public class LetStatement : Statement
	{
		public Token Token { get; set; }
		public Identifier Name { get; set; }
		public Expression Value { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}
	}

	public class ReturnStatement : Statement
	{
		public Token Token { get; set; }
		public Expression ReturnValue { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}
	}
}
