using System;
using System.Collections;
using System.Collections.Generic;

class Vector<T> : IList
{
    private T[] _contents;
    private int _count;

    T[] array;
    public Vector()
    {
        _contents = Array.Empty<T>();
    }
    public Vector(int count)
    {
        _contents = new T[count];
        _count = count;
    }

    // IList Members
    
    
    public T Back
    {
        get
        {
            return _contents[_contents.Length - 1];
        }
        set
        {
            _contents[_contents.Length - 1] = value;
        }
    }
    public T Front
    {
        get
        {
            return _contents[0];
        }
        set
        {
            _contents[0] = value;
        }
    }
    public int Add(object? value)
    {
        //this.Add((T)value);
        if (_count < _contents.Length)
        {
            _contents[_count] = (T?)value;
            _count++;

            return _count - 1;
        }

        return -1;
    }
    public T At(int index)
    {
        return _contents[index];
    }
    public void Clear()
    {
        _count = 0;
    }

    public bool Contains(object value)
    {
        //for (int i = 0; i < Count; i++)
        //{
        //    if (_contents[i] == ((T)value))
        //    {
        //        return true;
        //    }
        //}
        return false;
    }

    public int IndexOf(object value)
    {
        //for (int i = 0; i < Count; i++)
        //{
        //    if (_contents[i] == ((T)value))
        //    {
        //        return i;
        //    }
        //}
        return -1;
    }
    public void Insert(int index, object value)
    {
        if ((_count + 1 <= _contents.Length) && (index < Count) && (index >= 0))
        {
            _count++;

            for (int i = Count - 1; i > index; i--)
            {
                _contents[i] = _contents[i - 1];
            }
            _contents[index] = (T)value;
        }
    }
    public void PushBack(T value)
    {
        Array.Resize(ref _contents, _contents.Length + 1);
        _count++;
        _contents[_contents.Length - 1] = value;
        
    }
    public void PushFront(T value)
    {
        Insert(0, value);
    }
    public void Remove(object value)
    {
        RemoveAt(IndexOf(value));
    }
    public void RemoveAt(int index)
    {
        if ((index >= 0) && (index < Count))
        {
            for (int i = index; i < Count - 1; i++)
            {
                _contents[i] = _contents[i + 1];
            }
            _count--;
        }
    }
    public object this[int index]
    {
        get
        {
            return _contents[index];
        }
        set
        {
            _contents[index] = (T?)value;
        }
    }

    // ICollection members.

    public void CopyTo(Array array, int index)
    {
        for (int i = 0; i < Count; i++)
        {
            array.SetValue(_contents[i], index++);
        }
    }

    public int Count
    {
        get
        {
            return _count;
        }
    }

    public bool IsSynchronized
    {
        get
        {
            return false;
        }
        set { }
    }
    public bool IsFixedSize
    {
        get
        {
            return false;
        }
        set { }
    }
    public bool IsReadOnly
    {
        get
        {
            return false;
        }
        set { }
    }
    // Return the current instance since the underlying store is not
    // publicly available.
    public object SyncRoot
    {
        get
        {
            return this;
        }
    }

    // IEnumerable Members

    public IEnumerator GetEnumerator()
    {
        return _contents.GetEnumerator();
        //return GetEnumerator();
        //// Refer to the IEnumerator documentation for an example of
        //// implementing an enumerator.
        //throw new NotImplementedException("ququ), The method or operation is not implemented.");
    }

    
}

