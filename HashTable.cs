using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Linq;

namespace HashMap
{
    class HashTable<T, T1> : IDictionary<T, T1>
    {
        private static SHA256 _algorithme;
        private int _count;
        private int _arraySize;
        private const float _fillRatio = .7f;
        private List<KeyValuePair<T, T1>>[] _data;
        private List<T> _keys;
        private List<T1> _values;

        public HashTable() : this(120)
        {
        }

        public HashTable(int size)
        {
            _count = 0;
            _arraySize = getNextPrimeNumber(size);
            if (_algorithme is null) _algorithme = SHA256.Create();

            _data = new List<KeyValuePair<T, T1>>[_arraySize];
            _keys = new();
            _values = new();
        }

        public T1 this[T key]
        {
            get
            {
                if (!ContainsKey(key)) throw new KeyNotFoundException();
                int index = getIndex(key);
                var pairs = _data[index];
                return pairs.Where(p => p.Key.Equals(key)).FirstOrDefault().Value;
            }
            set
            {
                int index = getIndex(key);
                var pairs = _data[index];
                var targetPair = pairs.Where(p => p.Key.Equals(key)).FirstOrDefault();
                if (targetPair.Key is null) throw new KeyNotFoundException();
                KeyValuePair<T, T1> newPair = new KeyValuePair<T, T1>(targetPair.Key, value);
                pairs.Remove(targetPair);
                pairs.Add(newPair);
            }
        }

        public ICollection<T> Keys => _keys;

        public ICollection<T1> Values => _values;

        public int Count => _count;

        public bool IsReadOnly => false;

        public void Add(T key, T1 value)
        {
            Add(new(key, value));
        }

        public void Add(KeyValuePair<T, T1> item)
        {
            if (ContainsKey(item.Key))
                throw new InvalidOperationException("Key already exists");

            if (_arraySize * _fillRatio < _count)
            {
                _arraySize = getNextPrimeNumber(_arraySize);
                var tempK = new List<KeyValuePair<T, T1>>[_arraySize];

                foreach (var pair in this)
                {
                    int idx = getIndex(pair.Key);
                    if (tempK[idx] is null)
                        tempK[idx] = new();

                    tempK[idx].Add(pair);
                }

                _data = tempK;
            }

            int index = getIndex(item.Key);
            if (_data[index] is null)
                _data[index] = new();

            _data[index].Add(item);
            _keys.Add(item.Key);
            _values.Add(item.Value);
            _count++;
        }

        public void Clear()
        {
            _arraySize = 127;
            _count = 0;

            _data = new List<KeyValuePair<T, T1>>[_arraySize];
            _keys.Clear();
            _values.Clear();
        }

        public bool Contains(KeyValuePair<T, T1> item)
        {
            return _data.Where(l => l.Contains(item)).Count() == 1;
        }

        public bool ContainsKey(T key)
        {
            return Keys.Contains(key);
        }

        public void CopyTo(KeyValuePair<T, T1>[] array, int arrayIndex)
        {
            List<KeyValuePair<T, T1>> pairs = new List<KeyValuePair<T, T1>>();
            foreach (var list in _data) pairs.AddRange(list);

            for (int i = arrayIndex, y = 0; i < array.Length; i++, y++)
            {
                array[i] = pairs[y];
            }
        }

        public IEnumerator<KeyValuePair<T, T1>> GetEnumerator()
        {
            for (int i = 0; i < _keys.Count; i++)
            {
                yield return new KeyValuePair<T, T1>(_keys[i], _values[i]);
            }
        }

        public bool Remove(T key)
        {
            int index = getIndex(key);
            var pairs = _data[index];
            if (pairs is null) return false;

            int sub_index = -1;
            for (int i = 0; i < pairs.Count; i++)
                if (pairs[i].Key.Equals(key))
                {
                    sub_index = i;
                    pairs.RemoveAt(sub_index);
                    _count--;
                    _keys.Remove(pairs[i].Key);
                    _values.Remove(pairs[i].Value);
                    return true;
                }

            return false;

        }

        public bool Remove(KeyValuePair<T, T1> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(T key, [MaybeNullWhen(false)] out T1 value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int getNextPrimeNumber(int current)
        {
            int count;
            do
            {
                current++;
                count = 0;
                for (int i = 2; i <= current; i++)
                {
                    if (current % i == 0) count++;
                }
            } while (count > 1);

            return current;
        }

        int getIndex(T key)
        {
            return Math.Abs(key.GetHashCode() % _arraySize);
        }

    }
}