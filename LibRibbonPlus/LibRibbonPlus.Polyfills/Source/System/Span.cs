#nullable enable
using System.Text;

namespace System;

public ref struct Span<T>
{
    internal T[] _array;
    internal int _start;
    internal int _length;

    public Span(T[]? array)
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

    public Span(T[]? array, int start, int length)
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

    public ref T this[int index] => ref _array[_start + index];

    public int Length => _length;

    public void Clear()
    {
        for (int i = 0; i < _array.Length; i++)
        {
            _array[i] = default!;
        }
    }

    public void CopyTo(Span<T> destination)
    {
        for (int i = 0; i < _array.Length; i++)
        {
            destination[i] = _array[_start + i];
        }
    }

    public override bool Equals(object obj) => throw new NotSupportedException();

    public override int GetHashCode() => throw new NotSupportedException();

    public void Fill(T value)
    {
        for (int i = 0; i < _array.Length; i++)
        {
            _array[_start + i] = value;
        }
    }

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
            return $"System.Span<{typeof(T).Name}>[{_length}]";

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

    public static bool operator ==(Span<T> left, Span<T> right)
    {
        if (left.Length != right.Length)
            return false;

        for (int i = 0; i < left.Length; i++)
        {
            if (!Equals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool operator !=(Span<T> left, Span<T> right) => !(left == right);

    public static implicit operator Span<T>(T[]? array) => new(array);

    public static implicit operator Span<T>(ArraySegment<T> segment)
        => new(segment.Array, segment.Offset, segment.Count);

    public static implicit operator ReadOnlySpan<T>(Span<T> span)
        => new(span._array, span._start, span._length);

    /// <summary>
    ///  From .NET source
    /// </summary>
    public ref struct Enumerator
    {
        private Span<T> _span;
        private int _index;

        internal Enumerator(Span<T> span)
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

        public ref T Current => ref _span[_index];
    }
}
