using System;
using System.Buffers;

namespace MirrorSharp.Internal;

internal struct PooledGrowableArray<T> : IDisposable {
    private readonly ArrayPool<T> _pool;

    public PooledGrowableArray(int initialLength, ArrayPool<T> pool) {
        _pool = pool;
        Array = pool.Rent(initialLength);
    }

    public T[] Array { get; private set; }

    public void Grow(int newLength) {
        if (newLength <= Array.Length)
            return;

        var actualNewLength = Array.Length * (int)Math.Pow(2, Math.Log(Math.Ceiling((double)newLength / Array.Length), 2));
        var newArray = (T[]?)null;
        var oldArray = (T[]?)null;
        try {
            newArray = _pool.Rent(actualNewLength);
            System.Array.Copy(Array, 0, newArray, 0, Array.Length);
            oldArray = Array;
            Array = newArray;
        }
        catch (Exception) {
            if (Array != newArray && newArray != null)
                _pool.Return(newArray);
            throw;
        }
        finally {
            if (Array != oldArray && oldArray != null)
                _pool.Return(oldArray);
        }
    }

    public void Dispose() {
        _pool.Return(Array);
    }
}