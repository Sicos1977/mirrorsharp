using System.Collections.Immutable;

namespace MirrorSharp.Internal;

internal struct CharArrayString(ImmutableArray<char> chars) {
    public ImmutableArray<char> Chars { get; } = chars;
}