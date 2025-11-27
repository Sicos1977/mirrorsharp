namespace MirrorSharp.Testing.Results;

public class InfoTipSectionPart(string kind, string text) {
    public string Kind { get; } = kind;
    public string Text { get; } = text;

    public override string? ToString() {
        return Text;
    }
}