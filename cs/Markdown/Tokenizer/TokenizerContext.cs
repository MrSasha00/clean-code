namespace Markdown.Tokenizer;

public class TokenizerContext
{
	private readonly string text;
	private int position;

	public TokenizerContext(string text)
	{
		this.text = text;
		position = 0;
	}

	public bool IsEnd => position >= text.Length;
	public char Current => text[position];
	public void MoveNext() => position++;

	public string ReadWhile(Func<char, bool> predicate)
	{
		var start = position;
		while (!IsEnd && predicate(Current))
		{
			MoveNext();
		}

		return text.Substring(start, position - start);
	}

	public bool Match(string pattern)
	{
		return text.Substring(position).StartsWith(pattern);
	}

	public int Position => position;
	public void ResetTo(int position) => this.position = position;
}