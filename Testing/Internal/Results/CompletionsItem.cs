using System.Collections.Generic;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace MirrorSharp.Testing.Internal.Results;

internal class CompletionsItem(string displayText) {
    public string DisplayText { get; } = displayText;
    public int? Priority { get; set; }
    public IList<string> Kinds { get; } = new List<string>();
}