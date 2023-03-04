namespace Interpreter
{
	public class Builtins
	{
		public static Dictionary<string, Builtin> builtins = new()
		{
			{"len", new Builtin {Function = args =>
			{
				if (args.Length != 1)
					return new Error { Message = $"wrong number of arguments. got {args.Length} but expected 1." };

				switch (args[0].Type())
				{
					case ObjectType.ARRAY:
						return new Integer { Value = ((Array)args[0]).Elements.Count };
					case ObjectType.STRING:
						return new Integer { Value = ((String)args[0]).Value.Length };
					default:
						return new Error { Message = $"argument to `len` not supported, got {args[0].Type()}" };
				}
			}}},
			{"first", new Builtin {Function = args =>
			{
				if (args.Length != 1)
					return new Error { Message = $"wrong number of arguments. got {args.Length} but expected 1." };
				if (args[0].Type() != ObjectType.ARRAY)
					return new Error { Message = $"argument to `first` must be ARRAY, got {args[0].Type()}" };

				Array array = (Array) args[0];
				return array.Elements.Count > 0 ? array.Elements[0] : Evaluator.NULL;
			}}},
			{"last", new Builtin {Function = args =>
			{
				if (args.Length != 1)
					return new Error { Message = $"wrong number of arguments. got {args.Length} but expected 1." };
				if (args[0].Type() != ObjectType.ARRAY)
					return new Error { Message = $"argument to `last` must be ARRAY, got {args[0].Type()}" };

				Array array = (Array) args[0];
				int length = array.Elements.Count;
				return length > 0 ? array.Elements[length - 1] : Evaluator.NULL;
			}}},
			{"rest", new Builtin {Function = args =>
			{
				if (args.Length != 1)
					return new Error { Message = $"wrong number of arguments. got {args.Length} but expected 1." };
				if (args[0].Type() != ObjectType.ARRAY)
					return new Error { Message = $"argument to `rest` must be ARRAY, got {args[0].Type()}" };

				Array array = (Array) args[0];
				int length = array.Elements.Count;
				return length > 0 ? new Array {Elements = new List<Object>(array.Elements.Skip(1))} : Evaluator.NULL;
			}}},
			{"push", new Builtin {Function = args =>
			{
				if (args.Length != 2)
					return new Error { Message = $"wrong number of arguments. got {args.Length} but expected 2." };
				if (args[0].Type() != ObjectType.ARRAY)
					return new Error { Message = $"argument to `push` must be ARRAY, got {args[0].Type()}" };

				Array array = (Array) args[0];
				int length = array.Elements.Count;
				List<Object> newElements = array.Elements;
				newElements.Add(args[1]);
				return length > 0 ? new Array {Elements = new List<Object>(newElements)} : Evaluator.NULL;
			}}}
		};
	}
}
