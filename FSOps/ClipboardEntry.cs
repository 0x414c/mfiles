using System;

namespace FSOps
{
    public sealed class ClipboardEntry<T> : Tuple<T, ActionTag> where T : FSNode {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Tuple`2"/> class.
        /// </summary>
        /// <param name="item1">The value of the tuple's first component.</param><param name="item2">The value of the tuple's second component.</param>
        public ClipboardEntry (T item1, ActionTag item2) : base (item1, item2) { }
    }
}