namespace Markdown.Tokenizer;

public class Token
{
	public TokenType Type { get; set; }
	public string Content { get; set; } = string.Empty;

	public List<Token>? NestedTokens { get; set; }

	public Token(TokenType type, string content, List<Token>? nestedTokens = null)
	{
		Type = type;
		Content = content;
		NestedTokens = nestedTokens;
	}
}