namespace MirrorSharp.Testing.Internal.Results;

internal class CompletionsItemInfoPart {
    public string Kind { get; }
    public string Text { get; }

    public CompletionsItemInfoPart(string kind, string text) {
        Kind = kind;
        Text = text;
    }

    public override string ToString() {
        return Text;
    }
}