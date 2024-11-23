using Markdown.Tokenizer;

namespace Markdown.Render;

public interface ITokenRenderer
{
	string Render(List<Token> tokens);
}