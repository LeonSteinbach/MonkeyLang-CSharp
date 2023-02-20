using System.Xml.Linq;

namespace Interpreter
{
	public interface Node
	{
		public string TokenLiteral();
		public string String(int level, bool nextTokenExists);
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

		public string String(int level, bool nextTokenExists)
		{
			string result = Util.GetIndentedNodeString("{", 0);
			for (int i = 0; i < Statements.Count; i++)
			{
				result += Statements[i].String(1, i < Statements.Count - 1);
			}
			result += Util.GetIndentedNodeString("}", 0);
			return result;
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

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(Token.Type + ": {", level);
			result += Util.GetIndentedNodeString("value: " + Value, level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}

	public class IntegerLiteral : Expression
	{
		public Token Token { get; set; }
		public int Value { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(Token.Type + ": {", level);
			result += Util.GetIndentedNodeString("value: " + Value, level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}

	public class PrefixExpression : Expression
	{
		public Token Token { get; set; }
		public string Operator { get; set; }
		public Expression RightExpression { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(Token.Type + ": {", level);
			result += Util.GetIndentedNodeString("operator: " + Operator, level + 1);
			result += Util.GetIndentedNodeString("right-expression: {", level + 1);
			result += RightExpression?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("}", level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
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

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(Token.Type + ": {", level);
			result += Util.GetIndentedNodeString("name: " + Name.Value, level + 1);
			result += Util.GetIndentedNodeString("value: {", level + 1);
			result += Value?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("}", level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
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

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(Token.Type + ": {", level);
			result += Util.GetIndentedNodeString("value: {", level + 1);
			result += ReturnValue?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("}", level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}

	public class ExpressionStatement : Statement
	{
		public Token Token { get; set; }
		public Expression Expression { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(Token.Type + ": {", level);
			result += Util.GetIndentedNodeString("value: {", level + 1);
			result += Expression?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("}", level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}
}
