namespace Interpreter
{
	public enum ObjectType
	{
		INTEGER,
		BOOLEAN,
		NULL,
		RETURN,
		ERROR,
	}

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
}
