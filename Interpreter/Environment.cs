namespace Interpreter
{
	public class Environment
	{
		public Dictionary<string, Object> Store { get; set; }

		public Environment()
		{
			Store = new Dictionary<string, Object>();
		}

		public Tuple<Object?, bool> Get(string name)
		{
			bool error = Store.TryGetValue(name, out Object? value);
			return Tuple.Create(value, error);
		}

		public Object? Set(string name, Object value)
		{
			Store[name] = value;
			return value;
		}
	}
}
