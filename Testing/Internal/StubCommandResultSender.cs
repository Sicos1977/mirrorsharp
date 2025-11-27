using System.Buffers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MirrorSharp.Advanced;
using MirrorSharp.Advanced.EarlyAccess;
using MirrorSharp.Internal;
using MirrorSharp.Internal.Results;

namespace MirrorSharp.Testing.Internal;

internal class StubCommandResultSender(WorkSession session, IConnectionSendViewer? sendViewer = null)
    : ICommandResultSender {
    private readonly FastUtf8JsonWriter _writer = new(ArrayPool<byte>.Shared);

    public string? LastMessageTypeName { get; private set; }
    public string? LastMessageJson { get; private set; }

    public IFastJsonWriter StartJsonMessage(string messageTypeName) {
        LastMessageTypeName = messageTypeName;
        _writer.Reset();
        _writer.WriteStartObject();
        return _writer;
    }

    public async Task SendJsonMessageAsync(CancellationToken cancellationToken) {
        _writer.WriteEndObject();
        if (sendViewer != null)
            await sendViewer.ViewDuringSendAsync(LastMessageTypeName!, _writer.WrittenSegment, session, cancellationToken);
        LastMessageJson = Encoding.UTF8.GetString(_writer.WrittenSegment.Array, _writer.WrittenSegment.Offset, _writer.WrittenSegment.Count);
    }
}