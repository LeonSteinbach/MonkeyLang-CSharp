namespace Interpreter
{
	public class Environment
	{
		public Dictionary<string, Object> Store { get; set; }
		public Environment? Outer { get; set; }

		public Environment(Environment? outer = null)
		{
			Store = new Dictionary<string, Object>();
			Outer = outer;
		}

		public Tuple<Object?, bool> Get(string name)
		{
			bool found = Store.TryGetValue(name, out Object? value);
			if (found == false && Outer != null)
				return Outer.Get(name);
			return Tuple.Create(value, found);
		}

		public Object Set(string name, Object value)
		{
			Store[name] = value;
			return value;
		}
	}
}
