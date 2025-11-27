using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using MirrorSharp.Advanced;

namespace MirrorSharp.Internal.Results;

internal class NullCommandResultSender(ArrayPool<byte> bufferPool) : ICommandResultSender, IDisposable {
    private readonly FastUtf8JsonWriter _jsonWriter = new(bufferPool);

    public Task SendJsonMessageAsync(CancellationToken cancellationToken) {
        return cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) : Task.CompletedTask;
    }

    public IFastJsonWriter StartJsonMessage(string messageTypeName) {
        _jsonWriter.Reset();
        _jsonWriter.WriteStartObject();
        return _jsonWriter;
    }

    public void Dispose() {
        _jsonWriter.Dispose();
    }
}