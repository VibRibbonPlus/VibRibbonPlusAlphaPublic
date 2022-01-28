#nullable enable
using System.Text;

namespace System;

public ref struct ReadOnlySpan<T>
{
    internal T[] _array;
    internal int _start;
    internal int _length;

    public ReadOnlySpan(T[]? array)
    {
        if (array == null)
        {
            _array = Array.Empty<T>();
            _start = 0;
            _length = 0;
        }
        else
        {
            if (!typeof(T).IsValueType && array.GetType() != typeof(T[]))
            {
                throw new ArrayTypeMismatchException(nameof(array));
            }

            _array = array;
            _start = 0;
            _length = array.Length;
        }
    }

    public ReadOnlySpan(T[]? array, int start, int length)
    {
        if (array == null)
        {
            _array = Array.Empty<T>();
            _start = 0;
            _length = 0;
        }
        else
        {
            if (!typeof(T).IsValueType && array.GetType() != typeof(T[]))
            {
                throw new ArrayTypeMismatchException(nameof(array));
            }

            _array = array;
            _start = start;
            _length = length;
        }
    }

    public static Span<T> Empty => new(Array.Empty<T>());

    public bool IsEmpty => _length == 0;

    public ref readonly T this[int index] => ref _array[_start + index];

    public int Length => _length;

    public void CopyTo(Span<T> destination)
    {
        for (int i = 0; i < _array.Length; i++)
        {
            destination[i] = _array[_start + i];
        }
    }

    public override bool Equals(object obj) => throw new NotSupportedException();

    public override int GetHashCode() => throw new NotSupportedException();

    public Enumerator GetEnumerator() => new(this);

    public ref T GetPinnableReference() => throw new NotSupportedException();

    public Span<T> Slice(int start) => new(_array, _start + start, _length - start);

    public Span<T> Slice(int start, int length) => new(_array, _start + start, _length - start - length);

    public T[] ToArray()
    {
        if (_length == 0)
        {
            return Array.Empty<T>();
        }

        var array = new T[_length];
        Array.Copy(_array, _start, array, 0, _length);
        return array;
    }

    public override string ToString()
    {
        if (typeof(T) != typeof(char))
            return $"System.ReadOnlySpan<{typeof(T).Name}>[{_length}]";

        StringBuilder builder = new();

        for (int i = 0; i < _length; i++)
            if (_array[_start + i] is char c)
                builder.Append(c);

        return builder.ToString();
    }

    public bool TryCopyTo(Span<T> destination)
    {
        if (destination.Length < _length)
        {
            return false;
        }

        CopyTo(destination);
        return true;
    }

    public static bool operator ==(ReadOnlySpan<T> left, ReadOnlySpan<T> right)
    {
        if (left.Length != right.Length)
        {
            return false;
        }

        for (int i = 0; i < left.Length; i++)
        {
            if (!Equals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool operator !=(ReadOnlySpan<T> left, ReadOnlySpan<T> right) => !(left == right);

    public static implicit operator ReadOnlySpan<T>(T[]? array) => new(array);

    public static implicit operator ReadOnlySpan<T>(ArraySegment<T> segment)
        => new(segment.Array, segment.Offset, segment.Count);

    public static implicit operator ReadOnlySpan<T>(Span<T> span)
        => new(span._array, span._start, span._length);

    /// <summary>
    ///  From .NET source
    /// </summary>
    public ref struct Enumerator
    {
        private ReadOnlySpan<T> _span;
        private int _index;

        internal Enumerator(ReadOnlySpan<T> span)
        {
            _span = span;
            _index = -1;
        }

        public bool MoveNext()
        {
            int index = _index + 1;
            if (index < _span._length)
            {
                _index = index;
                return true;
            }

            return false;
        }

        public ref readonly T Current => ref _span[_index];
    }
}
