namespace Markdown.Tokenizer.Nodes;

public abstract class Node
{
	public string? Value { get; set; }
	public List<Node> Children { get; } = new();
	public Node? Parent { get; set; }
}