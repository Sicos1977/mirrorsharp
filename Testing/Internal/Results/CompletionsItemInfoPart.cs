namespace MirrorSharp.Testing.Internal.Results;

internal class CompletionsItemInfoPart(string kind, string text) {
    public string Kind { get; } = kind;
    public string Text { get; } = text;

    public override string ToString() {
        return Text;
    }
}