using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace HashMap
{
    class HashTable<T, T1> : IDictionary<T, T1>
    {
        private SHA256 _algorithme;
        private int _count;
        private int _arraySize;
        private T[] keys, values;

        int ICollection<T>.Count => _count;

        bool ICollection<T>.IsReadOnly => false;

        ICollection<T> IDictionary<T, T1>.Keys => throw new NotImplementedException();

        ICollection<T1> IDictionary<T, T1>.Values => throw new NotImplementedException();

        int ICollection<KeyValuePair<T, T1>>.Count => throw new NotImplementedException();

        bool ICollection<KeyValuePair<T, T1>>.IsReadOnly => throw new NotImplementedException();

        T1 IDictionary<T, T1>.this[T key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public HashTable() : this(127)
        {
        }

        public HashTable(int size)
        {
            _arraySize = size;
            _algorithme = SHA256.Create();

            keys = new T[_arraySize];
            values = new T[_arraySize];
        }

        int nextPrimeNumber(int current)
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

        int NormalizeIndex(T key)
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

        void ICollection<T>.Add(T item)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        void IDictionary<T, T1>.Add(T key, T1 value)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<T, T1>.ContainsKey(T key)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<T, T1>.Remove(T key)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<T, T1>.TryGetValue(T key, out T1 value)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<T, T1>>.Add(KeyValuePair<T, T1> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<T, T1>>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<T, T1>>.Contains(KeyValuePair<T, T1> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<T, T1>>.CopyTo(KeyValuePair<T, T1>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<T, T1>>.Remove(KeyValuePair<T, T1> item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<T, T1>> IEnumerable<KeyValuePair<T, T1>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}