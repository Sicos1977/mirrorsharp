using Microsoft.IO;
using MirrorSharp.Internal;
using MirrorSharp.Internal.Abstraction;

namespace MirrorSharp.FSharp.Internal;

internal class FSharpLanguage : ILanguage {
    public const string Name = "F#";
    private readonly RecyclableMemoryStreamManager _memoryStreamManager;

    private readonly MirrorSharpFSharpOptions _options;

    public FSharpLanguage(MirrorSharpFSharpOptions options, RecyclableMemoryStreamManager memoryStreamManager) {
        _options = options;
        _memoryStreamManager = memoryStreamManager;
    }

    public ILanguageSessionInternal CreateSession(string text, ILanguageSessionExtensions extensions) {
        return new FSharpSession(text, _options, _memoryStreamManager);
    }

    string ILanguage.Name => Name;
}