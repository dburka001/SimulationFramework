using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSimulation
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IList{T}" />
    public class ThreadSafeList<T> : IList<T>
    {
        private readonly List<T> _internalList = new List<T>();
        private readonly object ListLock = new Object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeList{T}"/> class.
        /// </summary>
        public ThreadSafeList() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeList{T}"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public ThreadSafeList(IEnumerable<T> items)
        {
            _internalList.AddRange(items);
        }

        /// <summary>
        /// Gets or sets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                lock (ListLock)
                {
                    return _internalList[index];
                }
            }

            set
            {
                lock (ListLock)
                {
                    _internalList[index] = value;
                }
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                lock (ListLock)
                {
                    return _internalList.Count;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(T item)
        {
            lock (ListLock)
            {
                _internalList.Add(item);
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddRange(IEnumerable<T> items)
        {
            lock (ListLock)
            {
                _internalList.AddRange(items);
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (ListLock)
            {
                _internalList.Clear();
            }
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T item)
        {
            lock (ListLock)
            {
                return _internalList.Contains(item);
            }
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (ListLock)
            {
                _internalList.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            lock (ListLock)
            {
                return new ThreadSafeEnumerator<T>(this);
            }

        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            lock (ListLock)
            {
                return _internalList.IndexOf(item);
            }
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, T item)
        {
            lock (ListLock)
            {
                _internalList.Insert(index, item);
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            lock (ListLock)
            {
                return _internalList.Remove(item);
            }
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            lock (ListLock)
            {
                _internalList.RemoveAt(index);
            }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (ListLock)
            {
                return new ThreadSafeEnumerator<T>(this);
            }

        }
    }
}
