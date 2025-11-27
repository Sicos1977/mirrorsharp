// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace MirrorSharp.Testing.Internal.Results;

internal class ResultChange(int start, int length, string text) {
    public int Start { get; } = start;
    public int Length { get; } = length;
    public string Text { get; } = text;
}