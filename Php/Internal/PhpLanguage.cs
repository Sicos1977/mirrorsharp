using MirrorSharp.Internal;
using MirrorSharp.Internal.Abstraction;

namespace MirrorSharp.Php.Internal;

internal class PhpLanguage(MirrorSharpPhpOptions options) : ILanguage {
    public const string Name = "PHP";

    public ILanguageSessionInternal CreateSession(string text, ILanguageSessionExtensions extensions) {
        return new PhpSession(text, options);
    }

    string ILanguage.Name => Name;
}