using Markdown.Render;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Nodes;
using Markdown.Tokenizer.Tags;

namespace Markdown;

public class MarkdownRenderer : IMarkdown
{
	public string Render(string markdown)
	{
		var tokenizer = new MarkdownTokenizer();
		var renderer = new HtmlRenderer();
		var tokens = tokenizer.Tokenize(markdown);
		var tree = ToTree(tokens);
		return renderer.Render(tree);
	}

	private Node ToTree(List<Token> tokens)
	{
		Node mainNode = new MainNode();
		Node currentNode = mainNode;
		for (int i = 0; i < tokens.Count; i++)
		{
			if (tokens[i].TagStatus == TagStatus.Broken)
			{
				currentNode.Children.Add(new TextNode{Value = tokens[i].Value});
				continue;
			}

			if (tokens[i] is ItalicTag tag)
			{
				if(tag.TagStatus == TagStatus.Open)
				{
					var node = new ItalicNode();
					currentNode.Children.Add(node);
					node.Parent = currentNode;
					currentNode = node;
					continue;
				}

				if (tag.TagStatus == TagStatus.Closed)
				{
					currentNode = currentNode.Parent;
					continue;
				}
			}

			if (tokens[i] is BoldTag boldTag)
			{
				if(boldTag.TagStatus == TagStatus.Open)
				{
					var node = new BoldNode();
					currentNode.Children.Add(node);
					node.Parent = currentNode;
					currentNode = node;
					continue;
				}

				if (boldTag.TagStatus == TagStatus.Closed)
				{
					currentNode = currentNode.Parent;
					continue;
				}
			}

			if (tokens[i] is HeaderTag)
			{
				var node = new HeaderNode();
				currentNode.Children.Add(node);
				node.Parent = currentNode;
				currentNode = node;
				continue;
			}

			if (tokens[i] is NewLineToken)
			{
				if (currentNode is HeaderNode)
				{
					currentNode = currentNode.Parent;
				}
				continue;
			}

			if (tokens[i] is TextToken textToken)
			{
				currentNode.Children.Add(new TextNode { Value = textToken.Value });
				continue;
			}
		}

		return currentNode.Parent ?? currentNode;
	}
}