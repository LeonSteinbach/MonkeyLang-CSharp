namespace Interpreter
{
	public static class Util
	{
		public static string GetIndentedNodeString(string text, int level, bool newLine = true)
		{
			string result = string.Empty;
			for (int i = 0; i < level; i++)
				result += "  ";
			result += text;

			if (newLine)
				result += Environment.NewLine;
			return result;
		}
	}
}
