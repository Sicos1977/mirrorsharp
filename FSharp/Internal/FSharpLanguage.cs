using Microsoft.IO;
using MirrorSharp.Internal;
using MirrorSharp.Internal.Abstraction;

namespace MirrorSharp.FSharp.Internal;

internal class FSharpLanguage(MirrorSharpFSharpOptions options, RecyclableMemoryStreamManager memoryStreamManager)
    : ILanguage {
    public const string Name = "F#";

    public ILanguageSessionInternal CreateSession(string text, ILanguageSessionExtensions extensions) {
        return new FSharpSession(text, options, memoryStreamManager);
    }

    string ILanguage.Name => Name;
}