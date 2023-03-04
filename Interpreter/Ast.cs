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
		public List<Statement?> Statements { get; set; }

		public Program()
		{
			Statements = new List<Statement?>();
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

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			result += Util.GetIndentedNodeString("type: " + Token.Type + ",", level + 1);
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

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			result += Util.GetIndentedNodeString("type: " + Token.Type + ",", level + 1);
			result += Util.GetIndentedNodeString("value: " + Value, level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}

	public class StringLiteral : Expression
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

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			result += Util.GetIndentedNodeString("type: " + Token.Type + ",", level + 1);
			result += Util.GetIndentedNodeString("value: " + Value, level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}

	public class BooleanLiteral : Expression
	{
		public Token Token { get; set; }
		public bool Value { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			result += Util.GetIndentedNodeString("type: " + Token.Type + ",", level + 1);
			result += Util.GetIndentedNodeString("value: " + Value, level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}

	public class PrefixExpression : Expression
	{
		public Token Token { get; set; }
		public string Operator { get; set; }
		public Expression? RightExpression { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			result += Util.GetIndentedNodeString("type: " + Token.Type + ",", level + 1);
			result += Util.GetIndentedNodeString("operator: " + Operator + ",", level + 1);
			result += Util.GetIndentedNodeString("right-expression: {", level + 1);
			result += RightExpression?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("}", level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}

	public class InfixExpression : Expression
	{
		public Token Token { get; set; }
		public string Operator { get; set; }
		public Expression? LeftExpression { get; set; }
		public Expression? RightExpression { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			result += Util.GetIndentedNodeString("type: " + Token.Type + ",", level + 1);
			result += Util.GetIndentedNodeString("operator: " + Operator + ",", level + 1);
			result += Util.GetIndentedNodeString("left-expression: {", level + 1);
			result += LeftExpression?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("},", level + 1);
			result += Util.GetIndentedNodeString("right-expression: {", level + 1);
			result += RightExpression?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("}", level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}

	public class IfExpression : Expression
	{
		public Token Token { get; set; }
		public Expression? Condition { get; set; }
		public BlockStatement? Consequence { get; set; }
		public BlockStatement? Alternative { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			result += Util.GetIndentedNodeString("type: " + Token.Type + ",", level + 1);
			result += Util.GetIndentedNodeString("condition: {", level + 1);
			result += Condition?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("},", level + 1);
			result += Util.GetIndentedNodeString("consequence: {", level + 1);
			result += Consequence?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("},", level + 1);
			result += Util.GetIndentedNodeString("alternative: {", level + 1);
			result += Alternative?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("}", level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}

	public class FunctionLiteral : Expression
	{
		public Token Token { get; set; }
		public List<Identifier>? Parameters { get; set; }
		public BlockStatement? Body { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			result += Util.GetIndentedNodeString("type: " + Token.Type + ",", level + 1);
			result += Util.GetIndentedNodeString("parameters: {", level + 1);
			for (int i = 0; i < Parameters.Count; i++)
			{
				result += Parameters[i].String(level + 2, i < Parameters.Count - 1);
			}
			result += Util.GetIndentedNodeString("},", level + 1);
			result += Util.GetIndentedNodeString("body: {", level + 1);
			result += Body?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("}", level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}

	public class CallExpression : Expression
	{
		public Token Token { get; set; }
		public Expression Function { get; set; }
		public List<Expression?>? Arguments { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			result += Util.GetIndentedNodeString("type: " + Token.Type + ",", level + 1);
			result += Util.GetIndentedNodeString("function: {", level + 1);
			result += Function?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("}", level + 1);
			result += Util.GetIndentedNodeString("arguments: {", level + 1);
			for (int i = 0; i < Arguments.Count; i++)
			{
				result += Arguments[i].String(level + 2, i < Arguments.Count - 1);
			}
			result += Util.GetIndentedNodeString("}", level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}

	public class BlockStatement : Statement
	{
		public Token Token { get; set; }
		public List<Statement?> Statements { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			for (int i = 0; i < Statements.Count; i++)
			{
				result += Statements[i].String(level + 1, i < Statements.Count - 1);
			}
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}

	public class LetStatement : Statement
	{
		public Token Token { get; set; }
		public Identifier Name { get; set; }
		public Expression? Value { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			result += Util.GetIndentedNodeString("type: " + Token.Type + ",", level + 1);
			result += Util.GetIndentedNodeString("name: {", level + 1);
			result += Name?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("},", level + 1);
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
		public Expression? ReturnValue { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			result += Util.GetIndentedNodeString("type: " + Token.Type + ",", level + 1);
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
		public Expression? Expression { get; set; }

		public string TokenLiteral()
		{
			return Token.Literal;
		}

		public string String(int level, bool nextTokenExists)
		{
			string result = string.Empty;

			result += Util.GetIndentedNodeString(GetType().Name + ": {", level);
			result += Util.GetIndentedNodeString("type: " + Token.Type + ",", level + 1);
			result += Util.GetIndentedNodeString("value: {", level + 1);
			result += Expression?.String(level + 2, nextTokenExists);
			result += Util.GetIndentedNodeString("}", level + 1);
			result += Util.GetIndentedNodeString(nextTokenExists ? "}," : "}", level);

			return result;
		}
	}
}
