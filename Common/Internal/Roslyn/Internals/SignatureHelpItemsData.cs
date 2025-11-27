using System.Collections.Generic;
using Microsoft.CodeAnalysis.Text;

namespace MirrorSharp.Internal.Roslyn.Internals;

internal class SignatureHelpItemsData(
    IEnumerable<SignatureHelpItemData> items,
    TextSpan applicableSpan,
    int argumentIndex,
    int argumentCount,
    int? selectedItemIndex) {
    public IEnumerable<SignatureHelpItemData> Items { get; } = items;
    public TextSpan ApplicableSpan { get; } = applicableSpan;
    public int ArgumentIndex { get; } = argumentIndex;
    public int ArgumentCount { get; } = argumentCount;
    public int? SelectedItemIndex { get; } = selectedItemIndex;
}