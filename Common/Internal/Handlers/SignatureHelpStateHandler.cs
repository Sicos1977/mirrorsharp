using System;
using System.Threading;
using System.Threading.Tasks;
using MirrorSharp.Internal.Handlers.Shared;
using MirrorSharp.Internal.Results;

namespace MirrorSharp.Internal.Handlers;

internal class SignatureHelpStateHandler(ISignatureHelpSupport signatureHelp) : ICommandHandler {
    public char CommandId => CommandIds.SignatureHelpState;

    public Task ExecuteAsync(AsyncData data, WorkSession session, ICommandResultSender sender, CancellationToken cancellationToken) {
        var @char = FastConvert.Utf8BytesToChar(data.GetFirst().Span);
        if (@char != 'F')
            // ReSharper disable once HeapView.BoxingAllocation
            throw new FormatException($"Unknown SignatureHelp command '{@char}'.");

        return signatureHelp.ForceSignatureHelpAsync(session, sender, cancellationToken);
    }
}