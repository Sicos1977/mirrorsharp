using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.SignatureHelp;
using MirrorSharp.Internal.Roslyn.Internals;
using SignatureHelpTriggerReason = Microsoft.CodeAnalysis.SignatureHelp.SignatureHelpTriggerReason;

namespace MirrorSharp.Internal.Roslyn47;

internal class SignatureHelpProviderWrapper(ISignatureHelpProvider provider) : ISignatureHelpProviderWrapper {
    public async Task<SignatureHelpItemsData?> GetItemsAsync(Document document, int position, SignatureHelpTriggerInfoData triggerInfo, SignatureHelpOptionsData options, CancellationToken cancellationToken) {
        // This is quite complicated to implement correctly and is still shifting around.
        // For now we will only allow default options. There is no way to check if user
        // intended something different, but that can be implemented later.
        var mappedOptions = SignatureHelpOptions.Default;
        var mappedTriggerInfo = new SignatureHelpTriggerInfo(
            (SignatureHelpTriggerReason)(int)triggerInfo.TriggerReason,
            triggerInfo.TriggerCharacter
        );

        var items = await provider.GetItemsAsync(
            document, position,
            mappedTriggerInfo,
            mappedOptions, cancellationToken
        ).ConfigureAwait(false);

        if (items == null)
            return null;

        return new SignatureHelpItemsData(
            items.Items.Select(i => new SignatureHelpItemData(
                i.DocumentationFactory,
                i.PrefixDisplayParts,
                i.SeparatorDisplayParts,
                i.SuffixDisplayParts,
                i.Parameters.Select(p => new SignatureHelpParameterData(
                    p.Name,
                    p.DocumentationFactory,
                    p.DisplayParts,
                    p.PrefixDisplayParts,
                    p.SuffixDisplayParts
                )),
                i.Parameters.Length
            )),
            items.ApplicableSpan,
            items.ArgumentIndex,
            items.ArgumentCount,
            items.SelectedItemIndex
        );
    }

    public bool IsRetriggerCharacter(char ch) {
        return provider.IsRetriggerCharacter(ch);
    }

    public bool IsTriggerCharacter(char ch) {
        return provider.IsTriggerCharacter(ch);
    }
}