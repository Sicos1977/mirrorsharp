using MirrorSharp.Advanced.EarlyAccess;

namespace MirrorSharp.Internal.Roslyn;

internal class RoslynLanguageDependencies(IRoslynCompilationGuard? guard) {
    public IRoslynCompilationGuard? Guard { get; } = guard;
}