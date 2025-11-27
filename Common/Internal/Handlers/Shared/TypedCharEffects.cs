using System.Threading;
using System.Threading.Tasks;
using MirrorSharp.Internal.Results;

namespace MirrorSharp.Internal.Handlers.Shared;

internal class TypedCharEffects(ICompletionSupport completion, ISignatureHelpSupport signatureHelp)
    : ITypedCharEffects {
    public async Task ApplyTypedCharAsync(char @char, WorkSession session, ICommandResultSender sender, CancellationToken cancellationToken) {
        await completion.ApplyTypedCharAsync(@char, session, sender, cancellationToken).ConfigureAwait(false);
        await signatureHelp.ApplyTypedCharAsync(@char, session, sender, cancellationToken).ConfigureAwait(false);
    }
}