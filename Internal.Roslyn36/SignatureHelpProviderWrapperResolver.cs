using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Shared.Utilities;
using Microsoft.CodeAnalysis.SignatureHelp;
using MirrorSharp.Internal.Roslyn.Internals;

namespace MirrorSharp.Internal.Roslyn36;

[Export(typeof(ISignatureHelpProviderWrapperResolver))]
[method: ImportingConstructor]
internal class SignatureHelpProviderWrapperResolver([ImportMany] IEnumerable<Lazy<ISignatureHelpProvider, OrderableLanguageMetadata>> allProviders)
    : ISignatureHelpProviderWrapperResolver {
    private readonly IList<Lazy<ISignatureHelpProvider, OrderableLanguageMetadata>> _allProviders = ExtensionOrderer.Order(allProviders);

    public IEnumerable<ISignatureHelpProviderWrapper> GetAllSlow(string languageName) {
        if (languageName == null)
            throw new ArgumentNullException(nameof(languageName));

        return _allProviders
            .Where(l => l.Metadata.Language == languageName)
            .Select(l => new SignatureHelpProviderWrapper(l.Value));
    }
}