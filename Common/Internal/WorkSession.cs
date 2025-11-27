using System;
using System.Collections.Generic;
using MirrorSharp.Advanced;
using MirrorSharp.Internal.Abstraction;
using MirrorSharp.Internal.Roslyn;

namespace MirrorSharp.Internal;

internal class WorkSession(ILanguage language, IWorkSessionOptions options, ILanguageSessionExtensions extensions)
    : IWorkSession, IDisposable {
    private ILanguageSessionInternal? _languageSession;
    private string _lastText = "";

    public ILanguage Language { get; private set; } = Argument.NotNull(nameof(language), language);

    public ILanguageSessionInternal LanguageSession {
        get {
            EnsureInitialized();
            return _languageSession!;
        }
    }

    public RoslynSession Roslyn => (RoslynSession)LanguageSession;

    public int CursorPosition { get; set; }

    public CurrentCompletion CurrentCompletion { get; } = new();

    public IDictionary<string, string> RawOptionsFromClient { get; } = new Dictionary<string, string>();
    public SelfDebug? SelfDebug { get; } = options.SelfDebugEnabled ? new SelfDebug() : null;

    public void Dispose() {
        _languageSession?.Dispose();
    }

    string IWorkSession.LanguageName => Language.Name;

    public bool IsRoslyn => LanguageSession is RoslynSession;
    IRoslynSession IWorkSession.Roslyn => Roslyn;

    public string GetText() {
        return LanguageSession.GetText();
    }

    public IDictionary<string, object?> ExtensionData { get; } = new Dictionary<string, object?>();

    public void ChangeLanguage(ILanguage language) {
        Argument.NotNull(nameof(language), language);
        if (language == Language)
            return;
        Language = language;

        if (_languageSession != null) {
            _lastText = _languageSession.GetText();
            _languageSession.Dispose();
        }

        _languageSession = null;
    }

    private void Initialize() {
        _languageSession = Language.CreateSession(_lastText, extensions);
    }

    public void ReplaceText(string newText, int start = 0, int? length = null) {
        LanguageSession.ReplaceText(newText, start, length);
    }

    private void EnsureInitialized() {
        if (_languageSession != null)
            return;
        Initialize();
    }
}