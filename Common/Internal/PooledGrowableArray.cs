using System;
using System.Buffers;

namespace MirrorSharp.Internal;

internal struct PooledGrowableArray<T>(int initialLength, ArrayPool<T> pool) : IDisposable {
    public T[] Array { get; private set; } = pool.Rent(initialLength);

    public void Grow(int newLength) {
        if (newLength <= Array.Length)
            return;

        var actualNewLength = Array.Length * (int)Math.Pow(2, Math.Log(Math.Ceiling((double)newLength / Array.Length), 2));
        var newArray = (T[]?)null;
        var oldArray = (T[]?)null;
        try {
            newArray = pool.Rent(actualNewLength);
            System.Array.Copy(Array, 0, newArray, 0, Array.Length);
            oldArray = Array;
            Array = newArray;
        }
        catch (Exception) {
            if (Array != newArray && newArray != null)
                pool.Return(newArray);
            throw;
        }
        finally {
            if (Array != oldArray && oldArray != null)
                pool.Return(oldArray);
        }
    }

    public void Dispose() {
        pool.Return(Array);
    }
}