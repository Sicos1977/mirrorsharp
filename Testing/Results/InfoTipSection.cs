using System.Collections.Generic;

namespace MirrorSharp.Testing.Results;

public class InfoTipSection(string kind) {
    public string Kind { get; } = kind;
    public IList<InfoTipSectionPart> Parts { get; } = new List<InfoTipSectionPart>();

    public override string ToString() {
        return string.Join("", Parts);
    }
}