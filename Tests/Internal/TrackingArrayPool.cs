using System;
using System.Buffers;
using System.Collections.Generic;
using Xunit;

namespace MirrorSharp.Tests.Internal;

public class TrackingArrayPool<T>(ArrayPool<T> inner) : ArrayPool<T> {
    private IDictionary<T[], string>? _rented;

    public override T[] Rent(int minimumLength) {
        var array = inner.Rent(minimumLength);
        _rented?.Add(array, Environment.StackTrace);
        return array;
    }

    public override void Return(T[] array, bool clearArray = false) {
        inner.Return(array);
        _rented?.Remove(array);
    }

    public void StartTracking() {
        _rented = new Dictionary<T[], string>();
    }

    public void AssertAllReturned() {
        Assert.Empty(_rented!.Values);
    }
}