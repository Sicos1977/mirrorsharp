using Microsoft.CodeAnalysis;

namespace MirrorSharp.Advanced.EarlyAccess;

internal interface IRoslynCompilationGuard {
    void ValidateCompilation(Compilation compilation, IRoslynSession session);
}