using System;
using System.Collections.Generic;

namespace MirrorSharp.Internal;

internal class SelfDebug {
    private readonly LogEntry[] _log = new LogEntry[100];
    private bool _endReached;
    private int _logIndex = -1;

    public void Log(string eventType, string? message, int cursorPosition, string text) {
        _logIndex += 1;
        if (_logIndex >= _log.Length) {
            _logIndex = 0;
            _endReached = true;
        }

        _log[_logIndex] = new LogEntry(DateTimeOffset.Now, eventType, message, cursorPosition, text);
    }

    public IEnumerable<LogEntry> GetLogEntries() {
        if (_endReached)
            for (var i = _logIndex + 1; i < _log.Length; i++)
                yield return _log[i];

        for (var i = 0; i <= _logIndex; i++) yield return _log[i];
    }

    public readonly struct LogEntry(DateTimeOffset dateTime, string eventType, string? message, int cursorPosition, string text) {
        public DateTimeOffset DateTime { get; } = dateTime;
        public string EventType { get; } = eventType;
        public string? Message { get; } = message;
        public int CursorPosition { get; } = cursorPosition;
        public string Text { get; } = text;
    }
}