namespace Interpreter
{
	public class Lexer
	{
		private readonly string input;
		private int position;
		private int readPosition;
		private byte currentCharacter;

		public Lexer(string input)
		{
			this.input = input;

			ReadCharacter();
		}

		public void ReadCharacter()
		{
			currentCharacter = readPosition >= input.Length ? (byte) 0 : (byte) input[readPosition];

			position = readPosition;
			readPosition++;
		}

		public byte PeekCharacter()
		{
			return readPosition >= input.Length ? (byte) 0 : (byte) input[readPosition];
		}

		public Token NextToken()
		{
			SkipWhitespace();

			byte nextCharacter = PeekCharacter();

			Token token = currentCharacter switch
			{
				(byte) '=' => ProcessDoubleCharacterToken(TokenType.EQ, TokenType.ASSIGN, '='),
				(byte) '!' => ProcessDoubleCharacterToken(TokenType.NEQ, TokenType.BANG, '='),
				(byte) '+' => new Token(TokenType.PLUS, ((char)currentCharacter).ToString()),
				(byte) '-' => new Token(TokenType.MINUS, ((char)currentCharacter).ToString()),
				(byte) ';' => new Token(TokenType.SEMICOLON, ((char) currentCharacter).ToString()),
				(byte) '(' => new Token(TokenType.LPAREN, ((char) currentCharacter).ToString()),
				(byte) ')' => new Token(TokenType.RPAREN, ((char) currentCharacter).ToString()),
				(byte) ',' => new Token(TokenType.COMMA, ((char) currentCharacter).ToString()),
				(byte) '{' => new Token(TokenType.LBRACE, ((char) currentCharacter).ToString()),
				(byte) '}' => new Token(TokenType.RBRACE, ((char) currentCharacter).ToString()),
				0 => new Token(TokenType.EOF, string.Empty),
				_ => GetIdentifier()
			};

			ReadCharacter();

			return token;
		}

		private Token GetIdentifier()
		{
			Token token;
			if (char.IsLetter((char)currentCharacter))
			{
				string literal = ReadIdentifier();
				token = new Token(Token.LookupIdent(literal), literal);
			}
			else if (char.IsDigit((char)currentCharacter))
			{
				string literal = ReadNumber();
				if (literal.Length > 1 && (literal.StartsWith('0') || literal.Any(char.IsLetter)))
					token = new Token(TokenType.ILLEGAL, literal);
				else
					token = new Token(TokenType.INT, literal);
			}
			else
			{
				token = new Token(TokenType.ILLEGAL, ((char)currentCharacter).ToString());
			}

			return token;
		}

		private Token ProcessDoubleCharacterToken(TokenType doubleCharTokenType, TokenType singleCharTokenType, char currentChar)
		{
			if (PeekCharacter() == currentChar)
			{
				string literal = ((char)currentCharacter).ToString();
				ReadCharacter();
				literal += ((char)currentCharacter).ToString();

				return new Token(doubleCharTokenType, literal);
			}
			else
			{
				return new Token(singleCharTokenType, ((char)currentCharacter).ToString());
			}
		}

		private string ReadIdentifier()
		{
			int oldPosition = position;

			while (char.IsLetter((char)PeekCharacter()) || char.IsDigit((char)PeekCharacter()))
				ReadCharacter();

			return input.Substring(oldPosition, (position - oldPosition) + 1);
		}

		private void SkipWhitespace()
		{
			while (new List<char> {' ', '\t', '\n', '\r'}.Contains((char)currentCharacter))
				ReadCharacter();
		}

		private string ReadNumber()
		{
			int oldPosition = position;

			while (char.IsDigit((char)PeekCharacter()) || char.IsLetter((char)PeekCharacter()))
				ReadCharacter();

			return input.Substring(oldPosition, (position - oldPosition) + 1);
		}
	}
}
