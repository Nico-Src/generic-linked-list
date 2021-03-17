Linked List Implementation: (C) Nico Thuniot 2021

- [x] Generisch
- [x] IEnumerable (Enumerate through elements)
- [x] IEquatable (Override Equals Methods)
- [x] IComparable
- [x] IDisposable
- [x] Thread-Safe:
  Implement Sync-Object for Locks
  in these Sections Task/Threads cannot intersect each other
  Other Options: Immutables
- [x] UnitTests (Teilweise erledigt)

Notizen:

In which order should list be cleared -> Answer GetEnumerator() Disposen


Locks:

lock(syncObject) <- if Thread gets to this point that thread gets exclusive access to the code in the lock statement
{                   other Threads getting to the lock statement will either lock it themself if it isnt already locked 
	...             or it will wait till the locked object in this case "syncObject" is released by another thread
}

Equality Comparer: 
EqualityComparer<T>.Default.Equals(this.Value, other);
EqualityComparer -> Default uses either the default obj.Equals()
or if class implements IEquatable<T> than it will use the custom Equals Method