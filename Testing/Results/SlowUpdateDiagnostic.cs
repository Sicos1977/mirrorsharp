using System.Collections.Generic;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace MirrorSharp.Testing.Results;

public class SlowUpdateDiagnostic(
    string id,
    string message,
    string severity,
    ResultSpan span) {
    public string Id { get; } = id;
    public string Message { get; } = message;
    public string Severity { get; } = severity;
    public ResultSpan Span { get; } = span;
    public IList<string> Tags { get; } = new List<string>();
    public IList<SlowUpdateDiagnosticAction> Actions { get; } = new List<SlowUpdateDiagnosticAction>();

    public override string ToString() {
        return $"{Severity} {Id}: {Message}";
    }
}