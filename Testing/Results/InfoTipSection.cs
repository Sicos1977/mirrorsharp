using System.Collections.Generic;

namespace MirrorSharp.Testing.Results;

public class InfoTipSection {
    public string Kind { get; }
    public IList<InfoTipSectionPart> Parts { get; } = new List<InfoTipSectionPart>();

    public InfoTipSection(string kind) {
        Kind = kind;
    }

    public override string ToString() {
        return string.Join("", Parts);
    }
}