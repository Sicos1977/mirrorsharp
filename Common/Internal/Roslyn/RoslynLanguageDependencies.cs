using MirrorSharp.Advanced.EarlyAccess;

namespace MirrorSharp.Internal.Roslyn;

internal class RoslynLanguageDependencies {
    public IRoslynCompilationGuard? Guard { get; }

    public RoslynLanguageDependencies(IRoslynCompilationGuard? guard) {
        Guard = guard;
    }
}