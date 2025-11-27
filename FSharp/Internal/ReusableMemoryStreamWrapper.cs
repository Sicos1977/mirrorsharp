using System;
using System.IO;
using System.Threading;

namespace MirrorSharp.FSharp.Internal;

internal class ReusableMemoryStreamWrapper(MemoryStream stream, string name) : Stream {
    private int _inUse = 0;

    internal Stream InnerStream => stream;

    public override bool CanRead => stream.CanRead;
    public override bool CanSeek => stream.CanSeek;
    public override bool CanWrite => stream.CanWrite;
    public override long Length => stream.Length;

    public override long Position {
        get => stream.Position;
        set => stream.Position = value;
    }

    public override void Flush() {
        stream.Flush();
    }

    public override long Seek(long offset, SeekOrigin origin) {
        return stream.Seek(offset, origin);
    }

    public override void SetLength(long value) {
        stream.SetLength(value);
    }

    public override int Read(byte[] buffer, int offset, int count) {
        return stream.Read(buffer, offset, count);
    }

    public override void Write(byte[] buffer, int offset, int count) {
        stream.Write(buffer, offset, count);
    }

    public override void Close() {
        Flush();
        Position = 0;
        _inUse = 0;
    }

    internal ReusableMemoryStreamWrapper Reuse() {
        if (Interlocked.CompareExchange(ref _inUse, 1, 0) == 1)
            throw new InvalidOperationException($"Stream {name} is currently in use, parallel access is not supported.");
        return this;
    }
}