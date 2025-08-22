using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap.Unity.Query
{
	// Token: 0x02000704 RID: 1796
	public struct Query<T>
	{
		// Token: 0x06002B94 RID: 11156 RVA: 0x000EAD6C File Offset: 0x000E916C
		public Query(T[] array, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (count < 0)
			{
				throw new ArgumentException("Count must be non-negative, but was " + count);
			}
			if (count > array.Length)
			{
				throw new ArgumentException(string.Concat(new object[]
				{
					"Count was ",
					count,
					" but the provided array only had a length of ",
					array.Length
				}));
			}
			this._array = array;
			this._count = count;
			this._validator = Query<T>.Validator.Spawn();
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x000EADFE File Offset: 0x000E91FE
		public Query(ICollection<T> collection)
		{
			this._array = ArrayPool<T>.Spawn(collection.Count);
			this._count = collection.Count;
			collection.CopyTo(this._array, 0);
			this._validator = Query<T>.Validator.Spawn();
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x000EAE38 File Offset: 0x000E9238
		public Query(Query<T> other)
		{
			other._validator.Validate();
			this._array = ArrayPool<T>.Spawn(other._count);
			this._count = other._count;
			Array.Copy(other._array, this._array, this._count);
			this._validator = Query<T>.Validator.Spawn();
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000EAE94 File Offset: 0x000E9294
		public Query<K> OfType<K>() where K : T
		{
			this._validator.Validate();
			K[] array = ArrayPool<K>.Spawn(this._count);
			int count = 0;
			for (int i = 0; i < this._count; i++)
			{
				if (this._array[i] is K)
				{
					array[count++] = (K)((object)this._array[i]);
				}
			}
			this.Dispose();
			return new Query<K>(array, count);
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x000EAF1A File Offset: 0x000E931A
		public Query<K> Cast<K>() where K : class
		{
			return this.Select(new Func<T, K>(Query<T>.<Cast`1>m__0<K>));
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x000EAF33 File Offset: 0x000E9333
		public void Dispose()
		{
			this._validator.Validate();
			ArrayPool<T>.Recycle(this._array);
			Query<T>.Validator.Invalidate(this._validator);
			this._array = null;
			this._count = 0;
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x000EAF64 File Offset: 0x000E9364
		public void Deconstruct(out T[] array, out int count)
		{
			this._validator.Validate();
			array = this._array;
			count = this._count;
			Query<T>.Validator.Invalidate(this._validator);
			this._array = null;
			this._count = 0;
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x000EAF9C File Offset: 0x000E939C
		public Query<T>.QuerySlice Deconstruct()
		{
			T[] array;
			int count;
			this.Deconstruct(out array, out count);
			return new Query<T>.QuerySlice(array, count);
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x000EAFBC File Offset: 0x000E93BC
		public Query<T>.Enumerator GetEnumerator()
		{
			this._validator.Validate();
			T[] array;
			int count;
			this.Deconstruct(out array, out count);
			return new Query<T>.Enumerator(array, count);
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x000EAFE5 File Offset: 0x000E93E5
		[CompilerGenerated]
		private static K <Cast<K>(T item) where K : class
		{
			return item as K;
		}

		// Token: 0x04002346 RID: 9030
		private T[] _array;

		// Token: 0x04002347 RID: 9031
		private int _count;

		// Token: 0x04002348 RID: 9032
		private Query<T>.Validator _validator;

		// Token: 0x02000705 RID: 1797
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06002B9E RID: 11166 RVA: 0x000EAFF8 File Offset: 0x000E93F8
			public Enumerator(T[] array, int count)
			{
				this._array = array;
				this._count = count;
				this._nextIndex = 0;
				this.Current = default(T);
			}

			// Token: 0x17000580 RID: 1408
			// (get) Token: 0x06002B9F RID: 11167 RVA: 0x000EB029 File Offset: 0x000E9429
			// (set) Token: 0x06002BA0 RID: 11168 RVA: 0x000EB031 File Offset: 0x000E9431
			public T Current
			{
				[CompilerGenerated]
				get
				{
					return this.<Current>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<Current>k__BackingField = value;
				}
			}

			// Token: 0x1700057F RID: 1407
			// (get) Token: 0x06002BA1 RID: 11169 RVA: 0x000EB03A File Offset: 0x000E943A
			object IEnumerator.Current
			{
				get
				{
					if (this._nextIndex == 0)
					{
						throw new InvalidOperationException();
					}
					return this.Current;
				}
			}

			// Token: 0x06002BA2 RID: 11170 RVA: 0x000EB058 File Offset: 0x000E9458
			public bool MoveNext()
			{
				if (this._nextIndex >= this._count)
				{
					return false;
				}
				this.Current = this._array[this._nextIndex++];
				return true;
			}

			// Token: 0x06002BA3 RID: 11171 RVA: 0x000EB09B File Offset: 0x000E949B
			public void Dispose()
			{
				ArrayPool<T>.Recycle(this._array);
			}

			// Token: 0x06002BA4 RID: 11172 RVA: 0x000EB0A8 File Offset: 0x000E94A8
			public void Reset()
			{
				throw new InvalidOperationException();
			}

			// Token: 0x04002349 RID: 9033
			private T[] _array;

			// Token: 0x0400234A RID: 9034
			private int _count;

			// Token: 0x0400234B RID: 9035
			private int _nextIndex;

			// Token: 0x0400234C RID: 9036
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private T <Current>k__BackingField;
		}

		// Token: 0x02000706 RID: 1798
		public struct QuerySlice : IDisposable
		{
			// Token: 0x06002BA5 RID: 11173 RVA: 0x000EB0AF File Offset: 0x000E94AF
			public QuerySlice(T[] array, int count)
			{
				this.BackingArray = array;
				this.Count = count;
			}

			// Token: 0x17000581 RID: 1409
			public T this[int index]
			{
				get
				{
					return this.BackingArray[index];
				}
			}

			// Token: 0x06002BA7 RID: 11175 RVA: 0x000EB0CD File Offset: 0x000E94CD
			public void Dispose()
			{
				ArrayPool<T>.Recycle(this.BackingArray);
			}

			// Token: 0x0400234D RID: 9037
			public readonly T[] BackingArray;

			// Token: 0x0400234E RID: 9038
			public readonly int Count;
		}

		// Token: 0x02000707 RID: 1799
		private struct Validator
		{
			// Token: 0x06002BA8 RID: 11176 RVA: 0x000EB0DC File Offset: 0x000E94DC
			public void Validate()
			{
				if (this._idValue == 0)
				{
					throw new InvalidOperationException("This Query is not valid, you cannot construct a Query using the default constructor.");
				}
				if (this._idRef == null || this._idRef.value != this._idValue)
				{
					throw new InvalidOperationException("This Query has already been disposed.  A Query can only be used once before it is automatically disposed.");
				}
			}

			// Token: 0x06002BA9 RID: 11177 RVA: 0x000EB12C File Offset: 0x000E952C
			public static Query<T>.Validator Spawn()
			{
				Query<T>.Validator.Id id = Pool<Query<T>.Validator.Id>.Spawn();
				Query<T>.Validator.Id id2 = id;
				int nextId = Query<T>.Validator._nextId;
				Query<T>.Validator._nextId = nextId + 1;
				id2.value = nextId;
				return new Query<T>.Validator
				{
					_idRef = id,
					_idValue = id.value
				};
			}

			// Token: 0x06002BAA RID: 11178 RVA: 0x000EB170 File Offset: 0x000E9570
			public static void Invalidate(Query<T>.Validator validator)
			{
				validator._idRef.value = -1;
				Pool<Query<T>.Validator.Id>.Recycle(validator._idRef);
			}

			// Token: 0x06002BAB RID: 11179 RVA: 0x000EB18B File Offset: 0x000E958B
			// Note: this type is marked as 'beforefieldinit'.
			static Validator()
			{
			}

			// Token: 0x0400234F RID: 9039
			private static int _nextId = 1;

			// Token: 0x04002350 RID: 9040
			private Query<T>.Validator.Id _idRef;

			// Token: 0x04002351 RID: 9041
			private int _idValue;

			// Token: 0x02000708 RID: 1800
			private class Id
			{
				// Token: 0x06002BAC RID: 11180 RVA: 0x000EB193 File Offset: 0x000E9593
				public Id()
				{
				}

				// Token: 0x04002352 RID: 9042
				public int value;
			}
		}
	}
}
