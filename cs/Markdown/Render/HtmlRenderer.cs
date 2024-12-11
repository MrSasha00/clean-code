using System.Text;
using Markdown.Tokenizer.Nodes;

namespace Markdown.Render;

public class HtmlRenderer : ITokenRenderer
{
	public string Render(Node tokens)
	{
		var sb = new StringBuilder();
		foreach (var token in tokens.Children)
			sb.Append(RenderToken(token));

		return sb.ToString();
	}

	private string? RenderToken(Node node)
	{
		return node switch
		{
			TextNode textNode => textNode.Value,
			HeaderNode => $"<h1>{Render(node)}</h1>",
			ItalicNode => $"<em>{Render(node)}</em>",
			BoldNode => $"<strong>{Render(node)}</strong>",
			_ => throw new Exception($"Unknown token type: {node.GetType()}")
		};
	}
}