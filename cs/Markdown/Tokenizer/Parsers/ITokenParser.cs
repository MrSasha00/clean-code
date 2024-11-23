namespace Markdown.Tokenizer.Parsers;

public interface ITokenParser
{
	Token? Parse(TokenizerContext text);
}