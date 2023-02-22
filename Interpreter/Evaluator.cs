namespace Interpreter
{
	public class Evaluator
	{
		private static Boolean TRUE = new() { Value = true };
		private static Boolean FALSE = new() { Value = false };
		private static Null NULL = new();

		public static Object Evaluate(Node node, Environment environment)
		{
			switch (node)
			{
				case Program program:
					return EvaluateProgram(program, environment);
				case ExpressionStatement expressionStatement:
					return Evaluate(expressionStatement.Expression, environment);
				case IntegerLiteral integerLiteral:
					return new Integer { Value = integerLiteral.Value };
				case BooleanLiteral booleanLiteral:
					return booleanLiteral.Value ? TRUE : FALSE;
				case PrefixExpression prefixExpression:
					Object prefixRightValue = Evaluate(prefixExpression.RightExpression, environment);
					return EvaluatePrefixExpression(prefixExpression.Operator, prefixRightValue);
				case InfixExpression infixExpression:
					Object infixLeftValue = Evaluate(infixExpression.LeftExpression, environment);
					if (IsError(infixLeftValue))
						return infixLeftValue;
					Object infixRightValue = Evaluate(infixExpression.RightExpression, environment);
					if (IsError(infixRightValue))
						return infixRightValue;
					return EvaluateInfixExpression(infixExpression.Operator, infixLeftValue, infixRightValue);
				case BlockStatement blockStatement:
					return EvaluateBlockStatement(blockStatement, environment);
				case IfExpression ifExpression:
					return EvaluateIfExpression(ifExpression, environment);
				case ReturnStatement returnStatement:
					Object returnValue = Evaluate(returnStatement.ReturnValue, environment);
					return IsError(returnValue) ? returnValue : new Return { Value = returnValue };
				case LetStatement letStatement:
					Object letValue = Evaluate(letStatement.Value, environment);
					if (IsError(letValue))
						return letValue;
					environment.Set(letStatement.Name.Value, letValue);
					return NULL;
				case Identifier identifier:
					return EvaluateIdentifier(identifier, environment);
				default:
					return NULL;
			}
		}

		private static bool IsError(Object obj)
		{
			return obj != NULL && obj.Type() == ObjectType.ERROR;
		}

		private static Object EvaluateProgram(Program program, Environment environment)
		{
			Object result = NULL;
			foreach (Statement statement in program.Statements)
			{
				result = Evaluate(statement, environment);

				if (result.Type() == ObjectType.RETURN)
					return ((Return) result).Value;
				if (result.Type() == ObjectType.ERROR)
					return result;
			}
			return result;
		}

		private static Object EvaluateBlockStatement(BlockStatement blockStatement, Environment environment)
		{
			Object result = NULL;
			foreach (Statement statement in blockStatement.Statements)
			{
				result = Evaluate(statement, environment);
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

		private static Object EvaluateIfExpression(IfExpression ifExpression, Environment environment)
		{
			Object condition = Evaluate(ifExpression.Condition, environment);
			if (IsError(condition))
				return condition;

			return IsTruthy(condition)
				? Evaluate(ifExpression.Consequence, environment)
				: Evaluate(ifExpression.Alternative, environment);
		}

		private static Object EvaluateIdentifier(Identifier identifier, Environment environment)
		{
			Tuple<Object?, bool> result = environment.Get(identifier.Value);
			return result.Item2 == false ?
				new Error { Message = $"identifier not found: {identifier.Value}" } :
				result.Item1 ?? NULL;
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
