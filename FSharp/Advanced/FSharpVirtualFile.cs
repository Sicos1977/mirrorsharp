using System;
using System.Collections.Concurrent;
using MirrorSharp.FSharp.Internal;
using IO = System.IO;

namespace MirrorSharp.FSharp.Advanced;

/// <summary>Represents a virtual (in-memory) file within <see cref="FSharpFileSystem" />.</summary>
public abstract class FSharpVirtualFile : IDisposable {
    private readonly ConcurrentDictionary<string, FSharpVirtualFile> _ownerCollection;
    private ReusableMemoryStreamWrapper? _lastStreamWrapper;

    /// <summary>Gets the path of the virtual file (generated, unique).</summary>
    public string Path { get; }

    internal DateTime LastWriteTime { get; set; }

    private protected FSharpVirtualFile(
        string path,
        ConcurrentDictionary<string, FSharpVirtualFile> ownerCollection
    ) {
        Path = path;
        _ownerCollection = ownerCollection;
    }

    /// <summary>Deregisters the file from the <see cref="FSharpFileSystem" />.</summary>
    public void Dispose() {
        _ownerCollection.TryRemove(Path, out _);
    }

    internal abstract IO.MemoryStream GetStream();

    internal ReusableMemoryStreamWrapper GetStreamWrapper() {
        var stream = GetStream();
        if (stream == _lastStreamWrapper?.InnerStream)
            return _lastStreamWrapper!;

        var wrapper = new ReusableMemoryStreamWrapper(stream, IO.Path.GetFileName(Path));
        _lastStreamWrapper = wrapper;
        return wrapper;
    }
}

internal class FSharpVirtualFile<TGetStreamContext> : FSharpVirtualFile, IDisposable {
    private readonly Func<TGetStreamContext, IO.MemoryStream> _getStream;
    private readonly TGetStreamContext _getStreamContext;

    public FSharpVirtualFile(
        string path,
        Func<TGetStreamContext, IO.MemoryStream> getStream,
        TGetStreamContext getStreamContext,
        ConcurrentDictionary<string, FSharpVirtualFile> ownerCollection
    ) : base(path, ownerCollection) {
        _getStream = getStream;
        _getStreamContext = getStreamContext;
    }

    internal override IO.MemoryStream GetStream() {
        return _getStream(_getStreamContext);
    }
}