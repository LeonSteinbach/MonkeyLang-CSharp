namespace Interpreter
{
	public enum ObjectType
	{
		INTEGER,
		STRING,
		BOOLEAN,
		NULL,
		RETURN,
		ERROR,
		FUNCTION,
		BUILTIN,
		ARRAY
	}

	public delegate Object BuiltinFunction(params Object[] args);

	public interface Object
	{
		public ObjectType Type();
		public string ToString();
	}

	public class Integer : Object
	{
		public int Value { get; set; }

		public override string ToString()
		{
			return Value.ToString();
		}

		public ObjectType Type()
		{
			return ObjectType.INTEGER;
		}
	}

	public class String : Object
	{
		public string Value { get; set; }

		public override string ToString()
		{
			return Value;
		}

		public ObjectType Type()
		{
			return ObjectType.STRING;
		}
	}

	public class Boolean : Object
	{
		public bool Value { get; set; }

		public override string ToString()
		{
			return Value.ToString();
		}

		public ObjectType Type()
		{
			return ObjectType.BOOLEAN;
		}
	}

	public class Null : Object
	{
		public override string ToString()
		{
			return "null";
		}

		public ObjectType Type()
		{
			return ObjectType.NULL;
		}
	}

	public class Return : Object
	{
		public Object? Value { get; set; }

		public override string ToString()
		{
			return Value?.ToString() ?? string.Empty;
		}

		public ObjectType Type()
		{
			return ObjectType.RETURN;
		}
	}

	public class Error : Object
	{
		public string? Message { get; set; }

		public override string ToString()
		{
			return "ERROR: " + Message;
		}

		public ObjectType Type()
		{
			return ObjectType.ERROR;
		}
	}

	public class Function : Object
	{
		public List<Identifier>? Parameters { get; set; }
		public BlockStatement? Body { get; set; }
		public Environment Environment { get; set; }

		public override string ToString()
		{
			string parametersString = string.Join(", ", Parameters);
			return $"fn({parametersString}) {Body}";
		}

		public ObjectType Type()
		{
			return ObjectType.FUNCTION;
		}
	}

	public class Builtin : Object
	{
		public BuiltinFunction Function { get; set; }

		public override string ToString()
		{
			return "builtin function";
		}

		public ObjectType Type()
		{
			return ObjectType.BUILTIN;
		}
	}

	public class Array : Object
	{
		public List<Object> Elements { get; set; }

		public override string ToString()
		{
			string elementsString = string.Join(", ", Elements);
			return $"[{elementsString}]";
		}

		public ObjectType Type()
		{
			return ObjectType.ARRAY;
		}
	}
}
