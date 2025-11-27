using MirrorSharp.Advanced;
using MirrorSharp.Advanced.EarlyAccess;

namespace MirrorSharp.Internal;

internal class ImmutableExtensionServices(
    ISetOptionsFromClientExtension? setOptionsFromClient,
    ISlowUpdateExtension? slowUpdate,
    IRoslynSourceTextGuard? roslynSourceTextGuard,
    IRoslynCompilationGuard? roslynCompilationGuard,
    IConnectionSendViewer? connectionSendViewer,
    IExceptionLogger? exceptionLogger)
    : ILanguageSessionExtensions {
    public ISetOptionsFromClientExtension? SetOptionsFromClient { get; } = setOptionsFromClient;
    public ISlowUpdateExtension? SlowUpdate { get; } = slowUpdate;
    public IConnectionSendViewer? ConnectionSendViewer { get; } = connectionSendViewer;
    public IExceptionLogger? ExceptionLogger { get; } = exceptionLogger;

    public IRoslynSourceTextGuard? RoslynSourceTextGuard { get; } = roslynSourceTextGuard;
    public IRoslynCompilationGuard? RoslynCompilationGuard { get; } = roslynCompilationGuard;
}