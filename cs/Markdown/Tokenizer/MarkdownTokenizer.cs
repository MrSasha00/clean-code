using Markdown.Tokenizer.Parsers;

namespace Markdown.Tokenizer;

public class MarkdownTokenizer : ITokenizer
{
	private static readonly List<ITokenParser> parsers
	= new List<ITokenParser>
	{
		new HeadParser(),
		new BoldParser(),
		new ItalicParser(),
		new TextParser(),
		new ListItemParser()
	};

	public List<Token> Tokenize(string text)
	{
		var context = new TokenizerContext(text);
		var tokens = new List<Token>();

		foreach (var parser in parsers)
		{
			var token = parser.Parse(context);
			if(token is not null)
				tokens.Add(token);
		}

		return tokens;
	}
}