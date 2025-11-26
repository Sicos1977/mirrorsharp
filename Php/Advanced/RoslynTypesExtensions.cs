extern alias peachpie;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using PeachpieRoslyn = peachpie::Microsoft.CodeAnalysis;

namespace MirrorSharp.Php.Advanced;

/// <summary>Provides the conversion from certain types in the fork of Roslyn used in Peachpie to the standard Roslyn.</summary>
public static class RoslynTypesExtensions {
    public static TextSpan ToStandardRoslyn(this peachpie::Microsoft.CodeAnalysis.Text.TextSpan span) {
        return new TextSpan(span.Start, span.Length);
    }

    public static LinePosition ToStandardRoslyn(this peachpie::Microsoft.CodeAnalysis.Text.LinePosition position) {
        return new LinePosition(position.Line, position.Character);
    }

    public static LinePositionSpan ToStandardRoslyn(this peachpie::Microsoft.CodeAnalysis.Text.LinePositionSpan span) {
        return new LinePositionSpan(
            span.Start.ToStandardRoslyn(),
            span.End.ToStandardRoslyn());
    }

    public static Location ToStandardRoslyn(this PeachpieRoslyn.Location location) {
        return Location.Create(
            location.SourceTree.FilePath,
            location.SourceSpan.ToStandardRoslyn(),
            location.GetLineSpan().Span.ToStandardRoslyn());
    }

    public static DiagnosticSeverity ToStandardRoslyn(this PeachpieRoslyn.DiagnosticSeverity severity) {
        return (DiagnosticSeverity)(int)severity;
    }

    public static Diagnostic ToStandardRoslyn(this PeachpieRoslyn.Diagnostic diagnostic) {
        return Diagnostic.Create(
            diagnostic.Id,
            "Compiler",
            diagnostic.GetMessage(),
            diagnostic.Severity.ToStandardRoslyn(),
            diagnostic.DefaultSeverity.ToStandardRoslyn(),
            false,
            diagnostic.Severity == PeachpieRoslyn.DiagnosticSeverity.Warning ? 1 : 0,
            location: diagnostic.Location.ToStandardRoslyn()
        );
    }
}