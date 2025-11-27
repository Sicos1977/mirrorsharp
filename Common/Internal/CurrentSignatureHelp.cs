using MirrorSharp.Internal.Roslyn.Internals;

namespace MirrorSharp.Internal;

internal struct CurrentSignatureHelp(ISignatureHelpProviderWrapper provider, SignatureHelpItemsData items) {
    public ISignatureHelpProviderWrapper Provider { get; } = provider;
    public SignatureHelpItemsData Items { get; } = items;
}