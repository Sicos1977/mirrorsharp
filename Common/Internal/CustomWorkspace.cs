using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;

namespace MirrorSharp.Internal;

internal class CustomWorkspace(HostServices host) : Workspace(host, "Custom") {
    public override bool CanOpenDocuments => true;

    /* same as AdHoc */

    public override bool CanApplyChange(ApplyChangesKind feature) {
        return feature == ApplyChangesKind.ChangeDocument
               || feature == ApplyChangesKind.ChangeParseOptions
               || feature == ApplyChangesKind.ChangeCompilationOptions
               || feature == ApplyChangesKind.AddMetadataReference
               || feature == ApplyChangesKind.RemoveMetadataReference;
    }

    public new Solution SetCurrentSolution(Solution solution) {
        return base.SetCurrentSolution(solution);
    }
}