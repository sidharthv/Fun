using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation.Collections;

namespace CalvinHobbes.Common
{
    /// <summary>
    /// Implementation of IObservableMap that supports reentrancy for use as a default view
    /// model.
    /// </summary>
    public class ObservableDictionary<T, U> : IObservableMap<T, U> where T: class where U: class
    {
        private class ObservableDictionaryChangedEventArgs<V> : IMapChangedEventArgs<V>
        {
            public ObservableDictionaryChangedEventArgs(CollectionChange change, V key)
            {
                this.CollectionChange = change;
                this.Key = key;
            }

            public CollectionChange CollectionChange { get; private set; }
            public V Key { get; private set; }
        }

        private Dictionary<T, U> _dictionary = new Dictionary<T, U>();
        public event MapChangedEventHandler<T, U> MapChanged;

        private void InvokeMapChanged(CollectionChange change, T key)
        {
            var eventHandler = MapChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new ObservableDictionaryChangedEventArgs<T>(change, key));
            }
        }

        public void Add(T key, U value)
        {
            this._dictionary.Add(key, value);
            this.InvokeMapChanged(CollectionChange.ItemInserted, key);
        }

        public void Add(KeyValuePair<T, U> item)
        {
            this.Add(item.Key, item.Value);
        }

        public bool Remove(T key)
        {
            if (this._dictionary.Remove(key))
            {
                this.InvokeMapChanged(CollectionChange.ItemRemoved, key);
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<T, U> item)
        {
            U currentValue;
            if (this._dictionary.TryGetValue(item.Key, out currentValue) &&
                Object.Equals(item.Value, currentValue) && this._dictionary.Remove(item.Key))
            {
                this.InvokeMapChanged(CollectionChange.ItemRemoved, item.Key);
                return true;
            }
            return false;
        }

        public U this[T key]
        {
            get
            {
                return this._dictionary[key];
            }
            set
            {
                this._dictionary[key] = value;
                this.InvokeMapChanged(CollectionChange.ItemChanged, key);
            }
        }

        public void Clear()
        {
            var priorKeys = this._dictionary.Keys.ToArray();
            this._dictionary.Clear();
            foreach (var key in priorKeys)
            {
                this.InvokeMapChanged(CollectionChange.ItemRemoved, key);
            }
        }

        public ICollection<T> Keys
        {
            get { return this._dictionary.Keys; }
        }

        public bool ContainsKey(T key)
        {
            return this._dictionary.ContainsKey(key);
        }

        public bool TryGetValue(T key, out U value)
        {
            return this._dictionary.TryGetValue(key, out value);
        }

        public ICollection<U> Values
        {
            get { return this._dictionary.Values; }
        }

        public bool Contains(KeyValuePair<T, U> item)
        {
            return this._dictionary.Contains(item);
        }

        public int Count
        {
            get { return this._dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<KeyValuePair<T, U>> GetEnumerator()
        {
            return this._dictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._dictionary.GetEnumerator();
        }

        public void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex)
        {
            int arraySize = array.Length;
            foreach (var pair in this._dictionary)
            {
                if (arrayIndex >= arraySize) break;
                array[arrayIndex++] = pair;
            }
        }
    }
}
