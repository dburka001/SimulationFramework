using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSimulation
{
    /// <summary>
    /// Enumerator for Thread Safe List
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IList{T}" />
    public class ThreadSafeEnumerator<T> : IEnumerator<T>
    {
        private T[] _items;
        private int _idx = -1;

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>
        /// The current item.
        /// </value>
        /// <exception cref="NotImplementedException"></exception>
        public T Current { get { return _items[_idx]; } }

        /// <summary>
        /// Gets the current item.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        object IEnumerator.Current { get { return _items[_idx]; } }

        /// <summary>
        /// Moves the next.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool MoveNext()
        {
            _idx++;
            return _idx < _items.Length;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Reset()
        {
            _idx = -1;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _items = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeEnumerator{T}"/> class.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        public ThreadSafeEnumerator(IEnumerable<T> enumerable)
        {
            _items = enumerable.ToArray();
        }
    }
}
