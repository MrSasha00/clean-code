using Markdown.Tokenizer;

namespace Markdown.Render.Renders;

public interface ITokenRender
{
	string Render(Token token);
}