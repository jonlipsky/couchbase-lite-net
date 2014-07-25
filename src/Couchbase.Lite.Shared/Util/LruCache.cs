/* 
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;

namespace Sharpen
{
    public class LruCache<TKey, TValue> 
        where TValue: class // Ensure we don't do null comparisons to any value types, which can't be null.
    {
        [ThreadStatic]
        static LruCache<TKey, TValue> deflt;

        public static LruCache<TKey, TValue> Default {
            get {
                return deflt ?? (deflt = new LruCache<TKey, TValue> (5));
            }
        }

        int capacity;
        LinkedList<ListValueEntry<TKey, TValue>> list;
        Dictionary<TKey, LinkedListNode<ListValueEntry<TKey, TValue>>> lookup;
        LinkedListNode<ListValueEntry<TKey, TValue>> openNode;

        public LruCache (int capacity)
        {
            this.capacity = capacity;
            this.list = new LinkedList<ListValueEntry<TKey, TValue>>();
            this.lookup = new Dictionary<TKey, LinkedListNode<ListValueEntry<TKey, TValue>>> (capacity + 1);
            this.openNode = new LinkedListNode<ListValueEntry<TKey, TValue>>(new ListValueEntry<TKey, TValue> (default(TKey), default(TValue)));
        }

        public void Put (TKey key, TValue value)
        {
            if (Get(key) == null) {
                this.openNode.Value.ItemKey = key;
                this.openNode.Value.ItemValue = value;
                this.list.AddFirst (this.openNode);
                this.lookup.Add (key, this.openNode);

                if (this.list.Count > this.capacity) {
                    // last node is to be removed and saved for the next addition to the cache
                    this.openNode = this.list.Last;

                    // remove from list & dictionary
                    this.list.RemoveLast();
                    this.lookup.Remove(this.openNode.Value.ItemKey);
                } else {
                    // still filling the cache, create a new open node for the next time
                    this.openNode = new LinkedListNode<ListValueEntry<TKey, TValue>>(new ListValueEntry<TKey, TValue>(default(TKey), default(TValue)));
                }
            }
        }

        public TValue Get (TKey key)
        {
            LinkedListNode<ListValueEntry<TKey, TValue>> node = null;
            if (!this.lookup.TryGetValue (key, out node))
                return default (TValue);
            this.list.Remove (node);
            this.list.AddFirst (node);
            return node.Value.ItemValue;
        }

        public void Evict (TKey key)
        {
            LinkedListNode<ListValueEntry<TKey, TValue>> node = null;
            if (!this.lookup.TryGetValue (key, out node))
                return;
            this.list.Remove (node);
            this.lookup.Remove (node.Value.ItemKey);
            this.openNode = this.list.Last;
        }

        public void EvictAll ()
        {
            this.list.Clear();
            this.lookup.Clear();
            this.openNode = new LinkedListNode<ListValueEntry<TKey, TValue>>(new ListValueEntry<TKey, TValue> (default(TKey), default(TValue)));
        }

        public TValue this[TKey key]
        {
            get { return Get(key); }
            set { Put(key, value); }
        }

        class ListValueEntry<K, V> where K : TKey 
            where V : class, TValue
        {
            private WeakReference itemValue;

            internal K ItemKey;
            internal V ItemValue 
            {
                get { return itemValue.Target as V; }
                set { itemValue = new WeakReference(value); } 
            }

            internal ListValueEntry(K key, V value)
            {
                this.ItemKey = key;
                this.ItemValue = value;
            }
        }
    }
}