namespace Markdown.Tokenizer.Tags;

public abstract class Token
{
	public virtual TagStatus TagStatus { get; set; }
	public virtual TokenType TokenType { get; }
	public string Value { get; set; }
}