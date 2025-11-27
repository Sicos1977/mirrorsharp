using System.Reflection;
using Microsoft.CodeAnalysis;

namespace MirrorSharp.Internal;

internal class PreloadedAnalyzerAssemblyLoader(Assembly assembly) : IAnalyzerAssemblyLoader {
    public Assembly LoadFromPath(string fullPath) {
        return assembly;
    }

    public void AddDependencyLocation(string fullPath) {
    }
}