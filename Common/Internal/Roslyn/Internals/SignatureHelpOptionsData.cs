using Microsoft.CodeAnalysis;

namespace MirrorSharp.Internal.Roslyn.Internals {
    internal struct SignatureHelpOptionsData {
        public Project Project { get; }

        private SignatureHelpOptionsData(Project project) {
            Project = project;
        }

        public static SignatureHelpOptionsData From(Project project) => new (project);
    }
}