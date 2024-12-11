namespace Markdown.Tests.Markdown;

[TestFixture]
public class MarkdownTests
{
	private static readonly VerifySettings Settings = new();
	private static readonly MarkdownRenderer Renderer = new();

	[OneTimeSetUp]
	public void OneTimeSetUp()
	{
		Settings.UseDirectory("snapshots");
	}

	[TestCaseSource(nameof(ItalicTestCases))]
	public string Test_1(string input) => Renderer.Render(input);

	private static TestCaseData[] ItalicTestCases =
	[
		new TestCaseData("# Header").Returns("<h1>Header</h1>"),
		new TestCaseData("\\# Header").Returns("# Header"),
		new TestCaseData("\\\\# Header").Returns("\\<h1>Header</h1>"),
		new TestCaseData("_Italic text_").Returns("<em>Italic text</em>"),
		new TestCaseData("\\_Text_").Returns("_Text_"),
		new TestCaseData("\\\\_Italic text_").Returns("\\<em>Italic text</em>"),
		new TestCaseData("_Italic text").Returns("_Italic text"),
		new TestCaseData("Italic text_").Returns("Italic text_"),
		new TestCaseData("Italic_ text_").Returns("Italic_ text_"),
		new TestCaseData("_Italic _text").Returns("_Italic _text"),
		new TestCaseData("_нач_але").Returns("<em>нач</em>але"),
		new TestCaseData("сер_еди_не").Returns("сер<em>еди</em>не"),
		new TestCaseData("цифры_1_12_3").Returns("цифры_1_12_3"),
		new TestCaseData("кон_це._").Returns("кон<em>це.</em>"),
		new TestCaseData("в ра_зных сл_овах не").Returns("в ра_зных сл_овах не"),
		new TestCaseData("__bold__").Returns("<strong>bold</strong>"),
		new TestCaseData("_Text__").Returns("_Text__"),
		new TestCaseData("__Text_").Returns("__Text_"),
		new TestCaseData("__Italic __text").Returns("__Italic __text"),
		new TestCaseData("__два _один_ может__").Returns("<strong>два <em>один</em> может</strong>"),
		new TestCaseData("_одинарного __двойное__ не_").Returns( "<em>одинарного __двойное__ не</em>")
	];

	private static Task Verify(string target) =>
		Verifier.Verify(target, Settings);

	[Test]
	public void SimpleText_Render_Verify() =>
		Verify(Renderer.Render("Text"));

	[Test]
	public void EscapedCharacter_Render_Verify() =>
		Verify(Renderer.Render(@"\_Text_"));

	[Test]
	public void ItalicText_Render_Verify() =>
		Verify(Renderer.Render("_Italic text_"));

	[Test]
	public void BoldText_Render_Verify() =>
		Verify(Renderer.Render("__Bold text__"));

	[Test]
	public void BoldWithItalicText_Render_Verify() =>
		Verify(Renderer.Render("__Bold _with italic_ text__"));

	[Test]
	public void SimpleHeader_Render_Verify() =>
		Verify(Renderer.Render("# Header"));

	[Test]
	public void TwoHeaders_Render_Verify() =>
		Verify(Renderer.Render("# Header one \n# Header two"));
	//
	// [Test]
	// public void HeaderWithItalic_Render_Verify() =>
	// 	Verify(Renderer.Render("# Header with _italic text_"));
	//
	// [Test]
	// public void HeaderWithBoldAndItalic_Render_Verify() =>
	// 	Verify(Renderer.Render("# Header with _italic_ and __bold__ text"));
	//
	// [Test]
	// public void HeaderWithItalicInBold_Render_Verify() =>
	// 	Verify(Renderer.Render("# Header ___italic_ in bold__ text"));
	//
	// [Test]
	// public void SimpleList_Render_Verify() =>
	// 	Verify(Renderer.Render("- item1\n- item2"));
	//
	// [Test]
	// public void ListWithItalicAndBold_Render_Verify() =>
	// 	Verify(Renderer.Render("- _item1_\n- __item2__"));
}