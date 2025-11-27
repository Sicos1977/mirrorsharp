using Microsoft.CodeAnalysis;
using MirrorSharp.Internal.Roslyn;

namespace MirrorSharp.VisualBasic.Internal;

internal class VisualBasicLanguage(MirrorSharpVisualBasicOptions options) : RoslynLanguageBase(LanguageNames.VisualBasic,
    "Microsoft.CodeAnalysis.VisualBasic.Features",
    "Microsoft.CodeAnalysis.VisualBasic.Workspaces",
    options);