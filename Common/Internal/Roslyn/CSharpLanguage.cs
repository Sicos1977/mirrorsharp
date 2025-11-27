using System;
using Microsoft.CodeAnalysis;

namespace MirrorSharp.Internal.Roslyn;

internal class CSharpLanguage(MirrorSharpCSharpOptions options) : RoslynLanguageBase(LanguageNames.CSharp,
    "Microsoft.CodeAnalysis.CSharp.Features",
    "Microsoft.CodeAnalysis.CSharp.Workspaces",
    options) {
    protected override bool ShouldConsiderForHostServices(Type type) {
        return base.ShouldConsiderForHostServices(type)
               // IntelliCode type, not available in normal environments
               && type.FullName != "Microsoft.CodeAnalysis.ExternalAccess.Pythia.PythiaSignatureHelpProvider";
    }
}