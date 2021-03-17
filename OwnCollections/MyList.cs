using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace OwnCollections
{
    /// <summary>
    /// Doubly Linked List (C) Nico Thuniot 2021
    /// </summary>
    /// <typeparam name="T"> Data-Type of data stored in the Linked List </typeparam>
    /// T must inherit from IEquatable for some methods to work
    public class MyList<T> : IEnumerable<T> where T : IEquatable<T>
    {
        private readonly object syncLock = new object(); // object to keep everything synced

        // Returns Item Count
        public int Count 
        { 
            get
            {
                lock(syncLock)
                {
                    // Check if List is empty
                    if (Head == null || Tail == null) return 0;
                    int counter = 0;
                    Node<T> currentNode = Head;
                    do
                    {
                        counter++;
                    }
                    while ((currentNode = currentNode.Next) != null);
                    return counter;
                }
            }
        }

        // Head and Tail start out as null
        public Node<T> Head = null;
        public Node<T> Tail = null;

        public MyList()
        {
            
        }

        /// <summary>
        /// Adds Element to the end of the Linked List
        /// </summary>
        /// <param name="item"> Value to add </param>
        public void Add(T item)
        {
            lock(syncLock)
            {
                // Initialize List if Head is null
                // else just add it at the end
                if (this.Head == null)
                {
                    this.Head = new Node<T>(item);
                    this.Tail = this.Head;
                }
                else
                {
                    Node<T> tmp = this.Tail; // Nico
                                             // Add Node after the Tail
                    this.Tail.Next = new Node<T>(item);
                    // Define new Node as Tail
                    this.Tail = this.Tail.Next;
                    // Clear the Next Pointer
                    this.Tail.Next = null;
                    // And readd Pointers
                    this.Tail.Prev = tmp;
                }
            }
        }

        /// <summary>
        /// Checks if Value is in Linked List
        /// </summary>
        /// <param name="item"> value to check for </param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            Node<T> currentNode = Head;
            do
            {
                if (currentNode.Equals(item)) return true;
            } while ((currentNode = currentNode.Next) != null); // do-while loop because otherwise Tail wont be included
            return false;
        }

        /// <summary>
        /// Checks if Node is in Linked List
        /// </summary>
        /// <param name="item"> node to check for </param>
        /// <returns></returns>
        public bool Contains(Node<T> item)
        {
            Node<T> currentNode = Head;
            do
            {
                if (currentNode.Equals(item)) return true;
            } while ((currentNode = currentNode.Next) != null); // do-while loop because otherwise Tail wont be included
            return false;
        }

        /// <summary>
        /// Adds all Items of a given Collection that implements IEnumerable<T>
        /// </summary>
        /// <param name="collection"> Collection to add </param>
        public void AddRange(IEnumerable<T> collection)
        {
            lock(syncLock)
            {
                foreach (var item in collection)
                {
                    this.Add(item);
                }
            }
        }

        /// <summary>
        /// Clears the List
        /// </summary>
        public void Clear()
        {
            // Dispose every Node before clearing the Linked List
            this.GetEnumerator().Dispose();

            this.Head = null;
            this.Tail = null;
        }

        /// <summary>
        /// Removes the first item in the linked list
        /// </summary>
        /// <returns> if item was removed + the removed node </returns>
        public (bool removed, Node<T> removedNode) RemoveFirst()
        {
            lock(syncLock)
            {
                if (Head == null) return (false,null);

                Node<T> node = GetNodeAt(0);
                if(this.Count == 1)
                {
                    this.Clear();
                    return (true, node);
                }
                else if(this.Count == 2)
                {
                    // Only one item in list so Head = Trail
                    Head = Tail;
                    return (true, node);
                }
                else
                {

                    var nextNode = node.Next;
                    // The Next Node will be the new Head so no prev pointer
                    nextNode.Prev = null;
                    // Clear the next pointer of the old Head
                    node.Next = null;
                    // Next Node is the new Head
                    Head = nextNode;
                    return ((node != null) ? true : false, node);
                }
            }
        }

        /// <summary>
        /// Removes the last element of the linked list
        /// </summary>
        /// <returns> if item was removed + the removed node </returns>
        public (bool removed, Node<T> removedNode) RemoveLast()
        {
            lock (syncLock)
            {
                if (Head == null) return (false, null);

                Node<T> node = null;
                if (this.Count > 1) node = GetNodeAt(this.Count - 1);
                else node = GetNodeAt(0);
                if (this.Count == 1)
                {
                    this.Clear();
                    return (true, node);
                }
                else if (this.Count == 2)
                {
                    // Only one item in list so Head = Trail
                    Head = Tail;
                    return (true, node);
                }
                else
                {

                    var prevNode = node.Prev;
                    // The Prev Node will be the new Tail so no next pointer
                    prevNode.Next = null;
                    // Clear the prev pointer of the old Tail
                    node.Prev = null;
                    // Prev Node is the new Tail
                    Tail = prevNode;
                    return ((node != null) ? true : false, node);
                }
            }
        }

        /// <summary>
        /// Removes an item at a given position
        /// </summary>
        /// <param name="pos"> given position </param>
        /// <returns> if item was removed + the removed node </returns>
        public (bool removed, Node<T> removedNode) RemoveAt(int pos)
        {
            lock(syncLock)
            {
                // Check if position is a valid position
                if (Head == null || pos < 0 || pos >= this.Count) throw new IndexOutOfRangeException("Index out of bounds.");
                // If Node has no Neighbour
                if (this.Count < 3)
                {
                    if (this.Count == 1)
                    {
                        var node = this.GetNodeAt(0);
                        this.Clear();
                        return ((node != null) ? true : false, node);
                    }
                    else if (this.Count == 2)
                    {
                        if (pos == 0)
                        {
                            var node = this.GetNodeAt(pos);
                            var nextNode = node.Next;
                            nextNode.Prev = null;
                            nextNode.Next = null;
                            node.Next = null;
                            node.Prev = null;
                            this.Head = this.Tail;
                            return ((node != null) ? true : false, node);
                        }
                        else if (pos == 1)
                        {
                            var node = this.GetNodeAt(pos);
                            var prevNode = node.Prev;
                            prevNode.Prev = null;
                            prevNode.Next = null;
                            node.Next = null;
                            node.Prev = null;
                            this.Tail = this.Head;
                            return ((node != null) ? true : false, node);
                        }
                    }
                }
                else
                {
                    if (pos == 0)
                    {
                        var node = this.GetNodeAt(pos);
                        var nextNode = node.Next;
                        nextNode.Prev = null;
                        node.Next = null;
                        Head = nextNode;
                        return ((node != null) ? true : false, node);
                    }
                    else if (pos == this.Count - 1)
                    {
                        var node = this.GetNodeAt(pos);
                        var prevNode = node.Prev;
                        // Prev Node will be new Tail => delete next pointer
                        prevNode.Next = null;
                        // Delete Pointers of old Tail
                        node.Prev = null;
                        // Prev Node is new Tail
                        Tail = prevNode;
                        return ((node != null) ? true : false, node);
                    }
                    else
                    {
                        var node = this.GetNodeAt(pos);
                        var nextNode = node.Next;
                        var prevNode = node.Prev;
                        // Connect the Next and Prev Node
                        nextNode.Prev = prevNode;
                        prevNode.Next = nextNode;
                        // Clear the Pointers of the removed Node
                        node.Next = null;
                        node.Prev = null;
                        return ((node != null) ? true : false, node);
                    }
                }
            }

            return (false, null);
        }

        public T this[int index]
        {
            get
            {
                lock(syncLock)
                {
                    if (this.Count == 0) return default(T);
                    // Check if Position is a valid position
                    if (index < Count && index >= 0)
                    {
                        int counter;
                        Node<T> currentNode = Head;
                        // while we aren't at the specified position continue to go forward
                        for (counter = 0; counter < index; counter++)
                        {
                            currentNode = currentNode.Next;
                        }
                        return currentNode.Value;
                    }
                    else
                    {
                        throw new IndexOutOfRangeException("Index out of bounds.");
                    }
                }
            }

            set
            {
                lock(syncLock)
                {
                    var node = GetNodeAt(index);
                    if(node != null) node.Value = value;
                }
            }
        }

        // Returns Node at given position
        public Node<T> GetNodeAt(int index)
        {
            lock(syncLock)
            {
                if (this.Count == 0) return null;
                // Check if position is a valid position
                if (index < Count && index >= 0)
                {
                    int counter;
                    Node<T> currentNode = Head;
                    // while we aren't at the specified position continue to go forward
                    for (counter = 0; counter < index; counter++)
                    {
                        currentNode = currentNode.Next;
                    }
                    return currentNode;
                }
                else
                {
                    throw new IndexOutOfRangeException("Index out of bounds.");
                }
            }
        }

        /// <summary>
        /// Helper Method to quickly output List to Console
        /// </summary>
        public void PrintToConsole()
        {
            if(this.Count == 0)
            {
                Console.WriteLine("List is Empty.");
                return;
            }

            Console.WriteLine($"{"Index".PadRight(8)}{"Value".PadRight(12)}{"ID".PadRight(8)}");
            for(int i = 0; i < this.Count; i++)
            {
                var node = GetNodeAt(i);
                Console.WriteLine($"{i.ToString().PadRight(8)}{node.Value.ToString().PadRight(12)}{node.ID.ToString().PadRight(8)}");
            }
        }

        /// <summary>
        /// Helper Method to visualize Connections between Nodes
        /// </summary>
        public void PrintWithConnectionsToConsole()
        {
            if (this.Count == 0)
            {
                Console.WriteLine("List is Empty.");
                return;
            }

            for (int i = 0; i < this.Count; i++)
            {
                // i == 0 and Head + Tail are the same means the Node is Head as well as Trail
                // i == 0 and Head + Tail are different means the Node is only Head
                // i == Count - 1 (Last Node) means the Node is a Trail Node
                // otherwise just a normal Node
                Console.Write($"({((i == 0)?((this.Head == this.Tail)?"Head&Trail":"Head") :(i == this.Count - 1)?"Tail":"")}[{i.ToString()}]{GetNodeAt(i).Value.ToString()})");
                Console.ForegroundColor = ConsoleColor.Green;
                string connection = "";
                if (i != this.Count-1)
                {
                    // Check if pointers are initialized
                    if (GetNodeAt(i + 1).Prev != null) connection += "<";
                    if (GetNodeAt(i).Next != null) connection += "-->";
                    Console.Write(connection);
                }
                Console.ResetColor();
            }
        }

        /// <summary>
        /// For iterating through the linked list as well as initalizing the list with value after the constructor
        /// <code>
        /// MyList myList = new MyList()
        /// {
        ///     "Nico",
        ///     "Nino",
        ///     "Adis"
        /// };
        /// </code>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            Node<T> currentNode = this.Head;
            while (currentNode != null)
            {
                yield return currentNode.Value;
                currentNode = currentNode.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class Node<T> : IDisposable, IEquatable<T>, IComparable<Node<T>> where T : IEquatable<T>
    {
        private bool disposedValue;
        private static int Counter = 0;
        public int ID { get; set; }
        public T Value { get; set; }
        public Node<T> Next { get; set; }
        public Node<T> Prev { get; set; }

        public Node(T Value)
        {
            this.Value = Value;
            this.ID = Counter;
            Counter++;
        }

        public override string ToString()
        {
            return $"{this.ID}:{this.Value}";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
                }

                // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
                // TODO: Große Felder auf NULL setzen
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Check if Values are equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(T other)
        {
            return EqualityComparer<T>.Default.Equals(this.Value, other);
        }

        /// <summary>
        /// Check if two Nodes are exactly equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Node<T> other)
        {
            bool equals = false;
            equals = EqualityComparer<T>.Default.Equals(this.Value, other.Value);
            equals = EqualityComparer<int>.Default.Equals(this.ID, other.ID);
            return equals;
        }

        /// <summary>
        /// Compares two Nodes
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Node<T> other)
        {
            if (other == null) return -1; // not equal
            var result = EqualityComparer<Node<T>>.Default.Equals(this, other);
            return (result == true) ? 0 : -1;
        }
    }
}
