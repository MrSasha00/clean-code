using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tags;

namespace Markdown.Tests.Tokenizer;

[TestFixture]
public class HeaderHandlerTests
{
	[TestCaseSource(nameof(HeaderTokenSource))]
	public void HeaderTokenizerTests((string input, Token[] tags) testCase)
	{
		var tokenizer = new MarkdownTokenizer();
		var res = tokenizer.Tokenize(testCase.input).ToArray();

		for (var i = 0; i < testCase.tags.Length; i++)
		{
			res[i].Value.Should().Be(testCase.tags[i].Value);
			res[i].TokenType.Should().Be(testCase.tags[i].TokenType);
		}
	}

	private static IEnumerable<(string input, Token[] tags)> HeaderTokenSource()
	{
		yield return ("abc", [new TextToken("abc")]);
		yield return ("# abc", [new HeaderTag(), new TextToken("abc")]);
		yield return ("f# abc", [new TextToken("f#"),new TextToken(" abc")]);
		yield return ("\\# abc", [new SlashToken(), new HeaderTag(), new TextToken("abc")]);
		yield return ("\\\\# abc", [new SlashToken(), new SlashToken(), new HeaderTag(), new TextToken("abc")]);
		yield return ("# abc\n# qwe", [
			new HeaderTag(),
			new TextToken("abc"),
			new NewLineToken(),
			new HeaderTag(),
			new TextToken("qwe")]);
	}
}