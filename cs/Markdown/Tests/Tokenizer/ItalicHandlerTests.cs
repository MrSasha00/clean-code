using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tags;


namespace Markdown.Tests.Tokenizer;

[TestFixture]
public class ItalicParserTests
{
	[TestCaseSource(nameof(ItalicTokenSource))]
	public void ItalicTokenizerTests((string input, Token[] tags) testCase)
	{
		var tokenizer = new MarkdownTokenizer();
		var res = tokenizer.Tokenize(testCase.input).ToArray();

		for (var i = 0; i < testCase.tags.Length; i++)
		{
			res[i].Value.Should().Be(testCase.tags[i].Value);
			res[i].TokenType.Should().Be(testCase.tags[i].TokenType);
		}
	}

	private static IEnumerable<(string input, Token[] tags)> ItalicTokenSource()
	{
		yield return ("abc", [new TextToken("abc")]);
		yield return ("_abc", [new ItalicTag(TagStatus.Open), new TextToken("abc")]);
		yield return ("abc_", [new TextToken("abc"), new ItalicTag(TagStatus.Closed)]);
		yield return ("a_bc_", [
			new TextToken("a"),
			new ItalicTag(TagStatus.InWord),
			new TextToken("bc"),
			new ItalicTag(TagStatus.Closed)]);
		yield return ("_a_bc", [
			new ItalicTag(TagStatus.Open),
			new TextToken("a"),
			new ItalicTag(TagStatus.InWord),
			new TextToken("bc")]);

		yield return ("_a_bc_", [
			new ItalicTag(TagStatus.Open),
			new TextToken("a"),
			new ItalicTag(TagStatus.InWord),
			new TextToken("bc"),
			new ItalicTag(TagStatus.Closed)]);

		yield return ("_abc_", [
			new ItalicTag(TagStatus.Open),
			new TextToken("abc"),
			new ItalicTag(TagStatus.Closed)]);

		yield return ("\\_abc", [
			new SlashToken(),
			new ItalicTag(TagStatus.Open),
			new TextToken("abc")]);

		yield return ("\\\\_abc", [
			new SlashToken(),
			new SlashToken(),
			new ItalicTag(TagStatus.Open),
			new TextToken("abc")]);
	}
}