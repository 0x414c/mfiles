using System;
using System.Collections;
using System.Collections.Generic;


namespace FSOps {
    class Clipboard : IEnumerable<FSNode> {
        #region Implementation of IEnumerable
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<FSNode> GetEnumerator () {
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator () {
            return GetEnumerator ();
        }
        #endregion
    }
}
