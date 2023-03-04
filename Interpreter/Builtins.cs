namespace Interpreter
{
	public class Builtins
	{
		public static Dictionary<string, Builtin> builtins = new()
		{
			{"len", new Builtin {Function = args =>
			{
				if (args.Length != 1)
					return new Error { Message = $"wrong number of arguments. got ${args.Length} but expected 1." };

				switch (args[0].Type())
				{
					case ObjectType.STRING:
						return new Integer { Value = ((String)args[0]).Value.Length };
					default:
						return new Error { Message = $"argument to `len` not supported, got ${args[0].Type()}" };
				}
			}}}
		};
	}
}
