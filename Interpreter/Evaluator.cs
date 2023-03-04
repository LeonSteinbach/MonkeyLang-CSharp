namespace Interpreter
{
	public class Evaluator
	{
		public static readonly Boolean TRUE = new() { Value = true };
		public static readonly Boolean FALSE = new() { Value = false };
		public static readonly Null NULL = new();

		public static Object Evaluate(Node? node, Environment environment)
		{
			switch (node)
			{
				case Program program:
					return EvaluateProgram(program, environment);
				case ExpressionStatement expressionStatement:
					return Evaluate(expressionStatement.Expression, environment);
				case IntegerLiteral integerLiteral:
					return new Integer { Value = integerLiteral.Value };
				case StringLiteral stringLiteral:
					return new String { Value = stringLiteral.Value };
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
				case FunctionLiteral functionLiteral:
					return new Function { Parameters = functionLiteral.Parameters, Body = functionLiteral.Body, Environment = environment };
				case CallExpression callExpression:
					Object callFunction = Evaluate(callExpression.Function, environment);
					if (IsError(callFunction))
						return callFunction;
					List<Object> callArguments = EvaluateExpressions(callExpression.Arguments, environment);
					if (callArguments.Count == 1 && IsError(callArguments[0]))
						return callArguments[0];
					return ApplyFunction(callFunction, callArguments);
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
			foreach (Statement? statement in program.Statements)
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
			foreach (Statement? statement in blockStatement.Statements)
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
				true when leftExpression.Type() == ObjectType.STRING && rightExpression.Type() == ObjectType.STRING =>
					EvaluateStringInfixExpression(opr, leftExpression, rightExpression),
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

		private static Object EvaluateStringInfixExpression(string opr, Object leftExpression, Object rightExpression)
		{
			if (opr != "+")
				return new Error { Message = $"unknown operator: {leftExpression.Type()} {opr} {rightExpression.Type()}" };

			string left = ((String)leftExpression).Value;
			string right = ((String)rightExpression).Value;
			return new String { Value = left + right };
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
			if (result.Item2)
				return result.Item1 ?? NULL;
			bool foundBuiltin = Builtins.builtins.TryGetValue(identifier.Value, out Builtin? builtin);
			if (foundBuiltin && builtin != null)
				return builtin;

			return new Error {Message = $"identifier not found: {identifier.Value}"};
		}

		private static List<Object> EvaluateExpressions(List<Expression?>? expressions, Environment environment)
		{
			List<Object> result = new List<Object>();

			if (expressions == null) return result;
			foreach (var evaluated in expressions.Select(expression => Evaluate(expression, environment)))
			{
				if (IsError(evaluated))
					return new List<Object> {evaluated};
				result.Add(evaluated);
			}

			return result;
		}

		private static Object ApplyFunction(Object function, List<Object> arguments)
		{
			switch (function.Type())
			{
				case ObjectType.FUNCTION:
					Environment extendedEnvironment = ExtendFunctionEnvironment((Function)function, arguments);
					Object evaluated = Evaluate(((Function)function).Body, extendedEnvironment);
					return UnwrapReturnValue(evaluated);
				case ObjectType.BUILTIN:
					return ((Builtin) function).Function(arguments.ToArray());
				default:
					return new Error { Message = $"not a function: {function.Type()}" };
			}
		}

		private static Environment ExtendFunctionEnvironment(Function function, List<Object> arguments)
		{
			Environment environment = new Environment(function.Environment);
			foreach (var item in function.Parameters.Select((parameter, i) => new {i, parameter}))
				environment.Set(item.parameter.Value, arguments[item.i]);
			return environment;
		}

		private static Object UnwrapReturnValue(Object obj)
		{
			return (obj.Type() == ObjectType.RETURN ? ((Return) obj).Value : obj) ?? NULL;
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
