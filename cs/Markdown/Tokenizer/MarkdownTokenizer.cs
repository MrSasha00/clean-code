using System.Text;
using Markdown.Tokenizer.Handlers;
using Markdown.Tokenizer.Tags;
using Token = Markdown.Tokenizer.Tags.Token;

namespace Markdown.Tokenizer;

public class MarkdownTokenizer : ITokenizer
{
	private readonly StringBuilder buffer  = new();
	private List<Token> tags = new();
	private readonly Stack<Token> tagStack = new();
	private readonly List<IHandler> handlers = new()
	{
		new HeaderHandler(),
		new ItalicHandler(),
		new BoldHandler(),
	};

	public List<Token> Tokenize(string text)
	{
		var context = new TokenizerContext(text);
		while (!context.IsEnd)
		{
			if (context.Current == '\n')
			{
				FlushBuffer();
				var token = new NewLineToken();
				tags.Add(token);
				context.Advance();
				continue;
			}
			if (context.Current == ' ')
			{
				if (buffer.Length > 0)
				{
					tags.Add(new TextToken(buffer.ToString()));
					buffer.Clear();
				}
				buffer.Append(context.Current);
				context.Advance();
				continue;
			}
			if (context.Current == '\\')
			{
				FlushBuffer();

				tags.Add(new SlashToken());
				context.Advance();
				continue;
			}

			bool flag = false;
			foreach (var handler in handlers)
			{
				var tag = handler.ProceedSymbol(context);
				if (tag != null)
				{
					if (buffer.Length > 0)
					{
						var token = new TextToken(buffer.ToString());
						tags.Add(token);
						buffer.Clear();
					}

					tags.Add(tag);
					tagStack.Push(tag);
					flag = true;
					break;
				}
			}

			if (flag == false)
			{
				buffer.Append(context.Current);
			}

			context.Advance();
		}

		FlushBuffer();
		ProceedEscaped();
		ProceedInWords();
		ProceedTags();
		return tags;
	}

	private void ProceedInWords()
	{
		for (var i = 0; i < tags.Count; i++)
		{
			var current = tags[i];
			if (current.TagStatus == TagStatus.InWord)
			{
				if (i - 2 >= 0)
				{
					if (tags[i - 1].TokenType == TokenType.String
						&& tags[i - 2].TagStatus == TagStatus.Open)
					{
						current.TagStatus = TagStatus.Closed;
					}
				}

				if (i + 2 < tags.Count)
				{
					if (tags[i + 1].TokenType == TokenType.String)
					{
						if (tags[i + 2].TagStatus == TagStatus.Closed)
						{
							current.TagStatus = TagStatus.Open;
						}
						else if (tags[i + 2].TagStatus == TagStatus.InWord)
						{
							current.TagStatus = TagStatus.Open;
							tags[i + 2].TagStatus = TagStatus.Closed;
						}
					}
				}
			}
		}
	}

	private void ProceedEscaped()
	{
		for (var i = 0; i < tags.Count - 1; i++)
		{
			var current = tags[i];
			var next = tags[i + 1];
			if (current.TokenType is TokenType.Slash && current.TagStatus != TagStatus.Broken)
			{
				if (next is { TokenType: TokenType.Slash })
				{
					current.TagStatus = TagStatus.Escaped;
					next.TagStatus = TagStatus.Broken;
				}
				else if (next is { TagStatus: TagStatus.Open or TagStatus.Closed or TagStatus.Single })
				{
					next.TagStatus = TagStatus.Broken;
					current.TagStatus = TagStatus.Escaped;
				}
			}
		}

		tags = tags.Where(t => t.TagStatus != TagStatus.Escaped).ToList();
	}

	private void ProceedTags()
	{
		var tempStack = new Stack<Token>();

		while (tagStack.Count > 0)
		{
			var current = tagStack.Pop();

			if (current.TagStatus != TagStatus.Broken && current.TagStatus != TagStatus.Single)
			{
				if (tempStack.Count > 0)
				{
					var previousTag = tempStack.Peek();

					if (previousTag.TokenType == current.TokenType)
					{
						if (previousTag.TagStatus == TagStatus.Closed && current.TagStatus == TagStatus.Open)
						{
							tempStack.Pop();
						}
						else
						{
							tempStack.Push(current);
						}
					}
					else
					{
						if (current.TokenType == TokenType.Bold && previousTag.TokenType == TokenType.Italic)
						{
							current.TagStatus = TagStatus.Broken;
						}
						else
						{
							tempStack.Push(current);
						}
					}
				}
				else
				{
					tempStack.Push(current);
				}
			}
		}

		while (tempStack.Count > 0)
		{
			tempStack.Pop().TagStatus = TagStatus.Broken;
		}
	}

	private void FlushBuffer()
	{
		if (buffer.Length > 0)
		{
			tags.Add(new TextToken(buffer.ToString()));
			buffer.Clear();
		}
	}
}