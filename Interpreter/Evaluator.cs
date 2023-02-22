namespace Interpreter
{
	public class Evaluator
	{
		private static Boolean TRUE = new() { Value = true };
		private static Boolean FALSE = new() { Value = false };
		private static Null NULL = new();

		public static Object Evaluate(Node node)
		{
			switch (node)
			{
				case Program program:
					return EvaluateProgram(program);
				case ExpressionStatement expressionStatement:
					return Evaluate(expressionStatement.Expression);
				case IntegerLiteral integerLiteral:
					return new Integer { Value = integerLiteral.Value };
				case BooleanLiteral booleanLiteral:
					return booleanLiteral.Value ? TRUE : FALSE;
				case PrefixExpression prefixExpression:
					Object prefixRightValue = Evaluate(prefixExpression.RightExpression);
					return EvaluatePrefixExpression(prefixExpression.Operator, prefixRightValue);
				case InfixExpression infixExpression:
					Object infixLeftValue = Evaluate(infixExpression.LeftExpression);
					if (IsError(infixLeftValue))
						return infixLeftValue;
					Object infixRightValue = Evaluate(infixExpression.RightExpression);
					if (IsError(infixRightValue))
						return infixRightValue;
					return EvaluateInfixExpression(infixExpression.Operator, infixLeftValue, infixRightValue);
				case BlockStatement blockStatement:
					return EvaluateBlockStatement(blockStatement);
				case IfExpression ifExpression:
					return EvaluateIfExpression(ifExpression);
				case ReturnStatement returnStatement:
					Object returnValue = Evaluate(returnStatement.ReturnValue);
					return IsError(returnValue) ? returnValue : new Return { Value = returnValue };
				default:
					return NULL;
			}
		}

		private static bool IsError(Object obj)
		{
			return obj != NULL && obj.Type() == ObjectType.ERROR;
		}

		private static Object EvaluateProgram(Program program)
		{
			Object result = NULL;
			foreach (Statement statement in program.Statements)
			{
				result = Evaluate(statement);

				return result.Type() switch
				{
					ObjectType.RETURN => ((Return)result).Value,
					ObjectType.ERROR => result,
					_ => NULL
				};
			}
			return result;
		}

		private static Object EvaluateBlockStatement(BlockStatement blockStatement)
		{
			Object result = NULL;
			foreach (Statement statement in blockStatement.Statements)
			{
				result = Evaluate(statement);
				if (result.Type() == ObjectType.RETURN || result.Type() == ObjectType.ERROR)
					return result;
			}
			return result;
		}

		private static Object EvaluatePrefixExpression(string opr, Object rightExpression)
		{
			return opr switch
			{
				"!" => EvaluateBangOperatorExpression(rightExpression),
				"-" => EvaluateMinusPrefixOperatorExpression(rightExpression),
				_ => new Error { Message = $"unknown operator: {opr} {rightExpression.Type()}" }
			};
		}

		private static Object EvaluateBangOperatorExpression(Object rightExpression)
		{
			return true switch
			{
				true when rightExpression == TRUE => FALSE,
				true when rightExpression == FALSE => TRUE,
				true when rightExpression == NULL => TRUE,
				_ => FALSE
			};
		}

		private static Object EvaluateMinusPrefixOperatorExpression(Object rightExpression)
		{
			if (rightExpression.Type() != ObjectType.INTEGER)
				return NULL;
			return new Integer { Value = -((Integer) rightExpression).Value };
		}

		private static Object EvaluateInfixExpression(string opr, Object leftExpression, Object rightExpression)
		{
			return true switch
			{
				true when leftExpression.Type() != rightExpression.Type() => new Error { Message = $"type mismatch: {leftExpression.Type()} {opr} {rightExpression.Type()}" },
				true when leftExpression.Type() == ObjectType.INTEGER && rightExpression.Type() == ObjectType.INTEGER =>
					EvaluateIntegerInfixExpression(opr, leftExpression, rightExpression),
				true when opr == "==" => ((Boolean) leftExpression).Value == ((Boolean) rightExpression).Value ? TRUE : FALSE,
				true when opr == "!=" => ((Boolean) leftExpression).Value != ((Boolean) rightExpression).Value ? TRUE : FALSE,
				_ => new Error { Message = $"unknown operator: {leftExpression.Type()} {opr} {rightExpression.Type()}" }
			};
		}

		private static Object EvaluateIntegerInfixExpression(string opr, Object leftExpression, Object rightExpression)
		{
			int left = ((Integer)leftExpression).Value;
			int right = ((Integer)rightExpression).Value;

			return opr switch
			{
				"+" => new Integer { Value = left + right },
				"-" => new Integer { Value = left - right },
				"*" => new Integer { Value = left * right },
				"/" => new Integer { Value = left / right },
				"<" => left < right ? TRUE : FALSE,
				">" => left > right ? TRUE : FALSE,
				"==" => left == right ? TRUE : FALSE,
				"!=" => left != right ? TRUE : FALSE,
				_ => new Error { Message = $"unknown operator: {leftExpression.Type()} {opr} {rightExpression.Type()}" }
			};
		}

		private static Object EvaluateIfExpression(IfExpression ifExpression)
		{
			Object condition = Evaluate(ifExpression.Condition);
			if (IsError(condition))
				return condition;

			return IsTruthy(condition)
				? Evaluate(ifExpression.Consequence)
				: Evaluate(ifExpression.Alternative);
		}

		private static bool IsTruthy(Object obj)
		{
			return true switch
			{
				true when obj == NULL => false,
				true when obj == TRUE => true,
				true when obj == FALSE => false,
				_ => true
			};
		}
	}
}
