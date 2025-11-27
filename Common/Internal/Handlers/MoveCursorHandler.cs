using System.Threading;
using System.Threading.Tasks;
using MirrorSharp.Internal.Handlers.Shared;
using MirrorSharp.Internal.Results;

namespace MirrorSharp.Internal.Handlers;

internal class MoveCursorHandler(ISignatureHelpSupport signatureHelp) : ICommandHandler {
    public char CommandId => CommandIds.MoveCursor;

    public Task ExecuteAsync(AsyncData data, WorkSession session, ICommandResultSender sender, CancellationToken cancellationToken) {
        var cursorPosition = FastConvert.Utf8BytesToInt32(data.GetFirst().Span);
        session.CursorPosition = cursorPosition;
        return signatureHelp.ApplyCursorPositionChangeAsync(session, sender, cancellationToken);
    }
}