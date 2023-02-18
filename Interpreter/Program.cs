namespace Interpreter
{
	class Program
	{
		public static int Main(String[] args)
		{
			string text = "fn let true false if else return asdf qwerty abc 1abc";//""abc1 def2ghi 3jkl 4 a) 3)";

			Lexer lexer = new Lexer(text);

			Token token;
			do
			{
				token = lexer.NextToken();
				Console.WriteLine(token);
			} while (token.Type != TokenType.EOF);

			Console.ReadKey();

			return 0;
		}
	}
}