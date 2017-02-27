using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;


namespace FSOps {
    public enum ActionTag { Copy, Cut }

    
    public sealed class ClipboardEntry<T> : Tuple<T, ActionTag> where T : FSNode {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Tuple`2"/> class.
        /// </summary>
        /// <param name="item1">The value of the tuple's first component.</param><param name="item2">The value of the tuple's second component.</param>
        public ClipboardEntry (T item1, ActionTag item2) : base (item1, item2) { }
    }


    public sealed class ClipboardStack<T> : IEnumerable<Tuple<T, ActionTag>>, INotifyCollectionChanged where T : FSNode {
        private readonly List<ClipboardEntry<T>> _contents;

        public ClipboardStack () {
            _contents = new List<ClipboardEntry<T>> ();
        }

        public int Count {
            get { return _contents.Count; }
        }

        public Tuple<T, ActionTag> Last {
            get { return _contents.Last (); }
        }

        public void Add (ClipboardEntry<T> item) {
            var idx = IndexOf (item);
            if (idx > -1) {
                if (_contents[idx].Item2 != item.Item2) {
                    var prev = _contents[idx];
                    _contents[idx] = item;
                    OnCollectionChanged (
                        new NotifyCollectionChangedEventArgs (
                            NotifyCollectionChangedAction.Replace, item, prev, idx
                        )
                    );         
                }
            } else {
                _contents.Add (item);
                OnCollectionChanged (new NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction.Add, item));
            }
        }

        public void Remove (ClipboardEntry<T> item) {
            _contents.Remove (item);

            OnCollectionChanged (new NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction.Remove, item));
        }

        public int IndexOf (ClipboardEntry<T> entry) {
            for (int i = 0; i < _contents.Count; i++) {
                if (_contents[i].Item1.FullPath == entry.Item1.FullPath) {
                    return i;
                }
            }
            return -1;
        }

        public void Clear () {
            _contents.Clear ();
            
            OnCollectionChanged (new NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction.Reset));
        }

        public Tuple<T, ActionTag> this [int idx] {
            get { return _contents[idx]; }
        }

        public static ClipboardStack<T> operator+ (ClipboardStack<T> lhs, ClipboardEntry<T> rhs) { 
            lhs.Add (rhs);

            return lhs;
        }

        public static ClipboardStack<T> operator- (ClipboardStack<T> lhs, ClipboardEntry<T> rhs) {
            lhs.Remove (rhs);
            
            return lhs;
        }


        #region Implementation of IEnumerable
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Tuple<T, ActionTag>> GetEnumerator () {
            return _contents.GetEnumerator ();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator () {
            return _contents.GetEnumerator ();
        }
        #endregion


        #region Implementation of INotifyCollectionChanged
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged (NotifyCollectionChangedEventArgs e) {
            if (CollectionChanged != null) {
                //using (BlockReentrancy ()) {
                    CollectionChanged (this, e);
                //}
            }
        }
        #endregion
    }
}
