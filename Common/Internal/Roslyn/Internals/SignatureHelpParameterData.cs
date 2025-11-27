using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace MirrorSharp.Internal.Roslyn.Internals;

internal class SignatureHelpParameterData(
    string name,
    Func<CancellationToken, IEnumerable<TaggedText>> documentationFactory,
    IList<TaggedText> displayParts,
    IList<TaggedText> prefixDisplayParts,
    IList<TaggedText> suffixDisplayParts) {
    public string Name { get; } = name;
    public Func<CancellationToken, IEnumerable<TaggedText>> DocumentationFactory { get; } = documentationFactory;
    public IList<TaggedText> DisplayParts { get; } = displayParts;
    public IList<TaggedText> PrefixDisplayParts { get; } = prefixDisplayParts;
    public IList<TaggedText> SuffixDisplayParts { get; } = suffixDisplayParts;
}