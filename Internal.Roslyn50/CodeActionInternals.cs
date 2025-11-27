using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis.CodeActions;
using MirrorSharp.Internal.Roslyn.Internals;
using CodeActionPriority = MirrorSharp.Internal.Roslyn.Internals.CodeActionPriority;

namespace MirrorSharp.Internal.Roslyn50;

[Shared]
[Export(typeof(ICodeActionInternals))]
internal class CodeActionInternals : ICodeActionInternals
{
    public bool IsInlinable(CodeAction action)
    {
        Argument.NotNull(nameof(action), action);
        return action.IsInlinable;
    }

    public CodeActionPriority GetPriority(CodeAction action)
    {
        Argument.NotNull(nameof(action), action);
        return (CodeActionPriority)(int)action.Priority;
    }

    public ImmutableArray<CodeAction> GetNestedCodeActions(CodeAction action)
    {
        Argument.NotNull(nameof(action), action);
        return action.NestedCodeActions;
    }
}