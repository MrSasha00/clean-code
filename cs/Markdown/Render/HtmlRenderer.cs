using System.Text;
using Markdown.Render.Renders;
using Markdown.Tokenizer;

namespace Markdown.Render;

public class HtmlRenderer : ITokenRenderer
{
	private readonly Dictionary<TokenType, ITokenRender> _renders = new()
	{
		{ TokenType.Italic , new ItalicRender() },
		{ TokenType.Bold , new BoldRender() },
		{ TokenType.Header, new HeadRender() },
		{ TokenType.Text, new TextRender() },
		{ TokenType.ItemList, new ItemListRender() }
	};

	public string Render(List<Token> tokens)
	{
		var sb = new StringBuilder();
		foreach (var token in tokens)
		{
			sb.Append(Render(token));
		}

		return sb.ToString();
	}

	private string Render(Token token)
	{
		return _renders[token.Type].Render(token);
	}


}