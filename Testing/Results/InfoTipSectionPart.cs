namespace MirrorSharp.Testing.Results;

public class InfoTipSectionPart {
    public string Kind { get; }
    public string Text { get; }

    public InfoTipSectionPart(string kind, string text) {
        Kind = kind;
        Text = text;
    }

    public override string? ToString() {
        return Text;
    }
}