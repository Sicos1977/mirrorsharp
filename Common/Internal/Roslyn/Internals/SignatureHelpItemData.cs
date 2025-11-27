using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace MirrorSharp.Internal.Roslyn.Internals;

internal struct SignatureHelpItemData(
    Func<CancellationToken, IEnumerable<TaggedText>> documentationFactory,
    ImmutableArray<TaggedText> prefixDisplayParts,
    ImmutableArray<TaggedText> separatorDisplayParts,
    ImmutableArray<TaggedText> suffixDisplayParts,
    IEnumerable<SignatureHelpParameterData> parameters,
    int parameterCount)
{
    // see FromInternalTypeExpressionSlow

    public Func<CancellationToken, IEnumerable<TaggedText>> DocumentationFactory { get; } = documentationFactory;
    public ImmutableArray<TaggedText> PrefixDisplayParts { get; } = prefixDisplayParts;
    public ImmutableArray<TaggedText> SeparatorDisplayParts { get; } = separatorDisplayParts;
    public ImmutableArray<TaggedText> SuffixDisplayParts { get; } = suffixDisplayParts;
    public IEnumerable<SignatureHelpParameterData> Parameters { get; } = parameters;
    public int ParameterCount { get; } = parameterCount;
}