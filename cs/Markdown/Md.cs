using Markdown.Render;
using Markdown.Tokenizer;

namespace Markdown;

public class Md : IMd
{
	private readonly ITokenizer tokenizer = new MarkdownTokenizer();
	private readonly ITokenRenderer renderer = new HtmlRenderer();

	public string Render(string markdown)
	{
		return renderer.Render(tokenizer.Tokenize(markdown));
	}
}