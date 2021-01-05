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
        private SHA256 _algorithme;
        private int _count;
        private int _arraySize;
        private const float _fillRatio = .7f;
        private List<KeyValuePair<T, T1>>[] _data;

        public HashTable() : this(120)
        {
        }

        public HashTable(int size)
        {
            _count = 0;
            _arraySize = getNextPrimeNumber(size);
            _algorithme = SHA256.Create();

            _data = new List<KeyValuePair<T, T1>>[_arraySize];
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

        public ICollection<T> Keys
        {
            get
            {
                List<T> keys = new List<T>();
                foreach (var l in _data)
                    if (l is not null)
                        keys.AddRange(l.Select(kv => kv.Key).ToArray());
                return keys;
            }
        }

        public ICollection<T1> Values
        {
            get
            {
                List<T1> values = new List<T1>();
                foreach (var l in _data)
                    if (l is not null)
                        values.AddRange(l.Select(kv => kv.Value).ToArray());
                return values;
            }
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public void Add(T key, T1 value)
        {
            if (ContainsKey(key))
                throw new InvalidOperationException("Key already exists");

            if (_arraySize * _fillRatio < _count)
            {
                _arraySize = getNextPrimeNumber(_arraySize);
                var tempK = new List<KeyValuePair<T, T1>>[_arraySize];

                foreach (var pair in this)
                {
                    int idx = getIndex(pair.Key);
                    if (tempK[idx] is null)
                    {
                        tempK[idx] = new List<KeyValuePair<T, T1>>() { pair };
                    }
                    else
                    {
                        tempK[idx].Add(pair);
                    }
                }


                _data = tempK;
            }

            int index = getIndex(key);

            if (_data[index] is null)
            {
                _data[index] = new List<KeyValuePair<T, T1>>()
                {
                    new KeyValuePair<T, T1>(key, value)
                };
            }
            else
            {
                _data[index].Add(new KeyValuePair<T, T1>(key, value));
            }

            _count++;
        }

        public void Add(KeyValuePair<T, T1> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _arraySize = 127;
            _count = 0;

            _data = new List<KeyValuePair<T, T1>>[_arraySize];
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
            foreach (var list in _data)
                if (list is not null)
                    foreach (var pair in list)
                        yield return pair;
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
                    break;
                }
            if (sub_index == -1) return false;

            pairs.RemoveAt(sub_index);
            _count--;
            return true;
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
                value = Activator.CreateInstance<T1>();
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int getNextPrimeNumber(int current)
        {
            int count = 0;
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
            BinaryFormatter formatter = new();
            using MemoryStream ms = new();

#pragma warning disable
            formatter.Serialize(ms, (object)key);
#pragma warning restore

            var data = ms.ToArray();
            var hash = _algorithme.ComputeHash(data);
            return Math.Abs(BitConverter.ToInt32(hash, 0) % _arraySize);
        }

    }
}