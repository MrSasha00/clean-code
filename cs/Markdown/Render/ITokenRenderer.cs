using Markdown.Tokenizer.Nodes;

namespace Markdown.Render;

public interface ITokenRenderer
{
	string Render(Node tokens);
}