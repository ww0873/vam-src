using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Leap.Unity.Query
{
	// Token: 0x0200070B RID: 1803
	public static class QueryOperatorExtensions
	{
		// Token: 0x06002BE9 RID: 11241 RVA: 0x000EC038 File Offset: 0x000EA438
		public static Query<T> Concat<T>(this Query<T> query, ICollection<T> collection)
		{
			Query<T> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				T[] array = ArrayPool<T>.Spawn(querySlice.Count + collection.Count);
				Array.Copy(querySlice.BackingArray, array, querySlice.Count);
				collection.CopyTo(array, querySlice.Count);
				result = new Query<T>(array, querySlice.Count + collection.Count);
			}
			return result;
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000EC0BC File Offset: 0x000EA4BC
		public static Query<T> Concat<T>(this Query<T> query, Query<T> other)
		{
			Query<T> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				using (Query<T>.QuerySlice querySlice2 = other.Deconstruct())
				{
					T[] array = ArrayPool<T>.Spawn(querySlice.Count + querySlice2.Count);
					Array.Copy(querySlice.BackingArray, array, querySlice.Count);
					Array.Copy(querySlice2.BackingArray, 0, array, querySlice.Count, querySlice2.Count);
					result = new Query<T>(array, querySlice.Count + querySlice2.Count);
				}
			}
			return result;
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000EC174 File Offset: 0x000EA574
		public static Query<T> Distinct<T>(this Query<T> query)
		{
			T[] array;
			int num;
			query.Deconstruct(out array, out num);
			HashSet<T> hashSet = Pool<HashSet<T>>.Spawn();
			for (int i = 0; i < num; i++)
			{
				hashSet.Add(array[i]);
			}
			Array.Clear(array, 0, array.Length);
			num = 0;
			foreach (T t in hashSet)
			{
				array[num++] = t;
			}
			hashSet.Clear();
			Pool<HashSet<T>>.Recycle(hashSet);
			return new Query<T>(array, num);
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000EC224 File Offset: 0x000EA624
		public static Query<T> OfType<T>(this Query<T> query, Type type)
		{
			Query<T> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				T[] array = ArrayPool<T>.Spawn(querySlice.Count);
				int count = 0;
				for (int i = 0; i < querySlice.Count; i++)
				{
					if (querySlice[i] != null)
					{
						T t = querySlice[i];
						if (type.IsAssignableFrom(t.GetType()))
						{
							array[count++] = querySlice[i];
						}
					}
				}
				result = new Query<T>(array, count);
			}
			return result;
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000EC2D8 File Offset: 0x000EA6D8
		public static Query<T> OrderBy<T, K>(this Query<T> query, Func<T, K> selector) where K : IComparable<K>
		{
			T[] array;
			int num;
			query.Deconstruct(out array, out num);
			QueryOperatorExtensions.FunctorComparer<T, K> functorComparer = QueryOperatorExtensions.FunctorComparer<T, K>.Ascending(selector);
			Array.Sort<T>(array, 0, num, functorComparer);
			functorComparer.Clear();
			return new Query<T>(array, num);
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x000EC310 File Offset: 0x000EA710
		public static Query<T> OrderByDescending<T, K>(this Query<T> query, Func<T, K> selector) where K : IComparable<K>
		{
			T[] array;
			int num;
			query.Deconstruct(out array, out num);
			QueryOperatorExtensions.FunctorComparer<T, K> functorComparer = QueryOperatorExtensions.FunctorComparer<T, K>.Descending(selector);
			Array.Sort<T>(array, 0, num, functorComparer);
			functorComparer.Clear();
			return new Query<T>(array, num);
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x000EC348 File Offset: 0x000EA748
		public static Query<T> Repeat<T>(this Query<T> query, int times)
		{
			if (times < 0)
			{
				throw new ArgumentException("The repetition count must be non-negative.");
			}
			Query<T> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				T[] array = ArrayPool<T>.Spawn(querySlice.Count * times);
				for (int i = 0; i < times; i++)
				{
					Array.Copy(querySlice.BackingArray, 0, array, i * querySlice.Count, querySlice.Count);
				}
				result = new Query<T>(array, querySlice.Count * times);
			}
			return result;
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000EC3E4 File Offset: 0x000EA7E4
		public static Query<T> Reverse<T>(this Query<T> query)
		{
			T[] array;
			int num;
			query.Deconstruct(out array, out num);
			array.Reverse(0, num);
			return new Query<T>(array, num);
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x000EC40C File Offset: 0x000EA80C
		public static Query<K> Select<T, K>(this Query<T> query, Func<T, K> selector)
		{
			Query<K> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				K[] array = ArrayPool<K>.Spawn(querySlice.Count);
				for (int i = 0; i < querySlice.Count; i++)
				{
					array[i] = selector(querySlice[i]);
				}
				result = new Query<K>(array, querySlice.Count);
			}
			return result;
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x000EC490 File Offset: 0x000EA890
		public static Query<K> SelectMany<T, K>(this Query<T> query, Func<T, ICollection<K>> selector)
		{
			Query<K> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				int num = 0;
				for (int i = 0; i < querySlice.Count; i++)
				{
					num += selector(querySlice[i]).Count;
				}
				K[] array = ArrayPool<K>.Spawn(num);
				int num2 = 0;
				for (int j = 0; j < querySlice.Count; j++)
				{
					ICollection<K> collection = selector(querySlice[j]);
					collection.CopyTo(array, num2);
					num2 += collection.Count;
				}
				result = new Query<K>(array, num);
			}
			return result;
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x000EC550 File Offset: 0x000EA950
		public static Query<K> SelectMany<T, K>(this Query<T> query, Func<T, Query<K>> selector)
		{
			Query<K> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				Query<K>.QuerySlice[] array = ArrayPool<Query<K>.QuerySlice>.Spawn(querySlice.Count);
				int num = 0;
				for (int i = 0; i < querySlice.Count; i++)
				{
					array[i] = selector(querySlice[i]).Deconstruct();
					num += array[i].Count;
				}
				K[] array2 = ArrayPool<K>.Spawn(num);
				int num2 = 0;
				for (int j = 0; j < querySlice.Count; j++)
				{
					Array.Copy(array[j].BackingArray, 0, array2, num2, array[j].Count);
					num2 += array[j].Count;
					array[j].Dispose();
				}
				ArrayPool<Query<K>.QuerySlice>.Recycle(array);
				result = new Query<K>(array2, num);
			}
			return result;
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x000EC660 File Offset: 0x000EAA60
		public static Query<T> Skip<T>(this Query<T> query, int toSkip)
		{
			T[] array;
			int num;
			query.Deconstruct(out array, out num);
			int num2 = Mathf.Max(num - toSkip, 0);
			toSkip = num - num2;
			Array.Copy(array, toSkip, array, 0, num2);
			Array.Clear(array, num2, array.Length - num2);
			return new Query<T>(array, num2);
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x000EC6A4 File Offset: 0x000EAAA4
		public static Query<T> SkipWhile<T>(this Query<T> query, Func<T, bool> predicate)
		{
			T[] array;
			int num;
			query.Deconstruct(out array, out num);
			int i;
			for (i = 0; i < num; i++)
			{
				if (!predicate(array[i]))
				{
					break;
				}
			}
			int num2 = num - i;
			Array.Copy(array, i, array, 0, num2);
			Array.Clear(array, num2, array.Length - num2);
			return new Query<T>(array, num2);
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x000EC70C File Offset: 0x000EAB0C
		public static Query<T> Sort<T>(this Query<T> query) where T : IComparable<T>
		{
			T[] array;
			int num;
			query.Deconstruct(out array, out num);
			Array.Sort<T>(array, 0, num);
			return new Query<T>(array, num);
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x000EC734 File Offset: 0x000EAB34
		public static Query<T> SortDescending<T>(this Query<T> query) where T : IComparable<T>
		{
			T[] array;
			int num;
			query.Deconstruct(out array, out num);
			Array.Sort<T>(array, 0, num);
			array.Reverse(0, num);
			return new Query<T>(array, num);
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x000EC764 File Offset: 0x000EAB64
		public static Query<T> Take<T>(this Query<T> query, int toTake)
		{
			T[] array;
			int num;
			query.Deconstruct(out array, out num);
			num = Mathf.Min(num, toTake);
			Array.Clear(array, num, array.Length - num);
			return new Query<T>(array, num);
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x000EC798 File Offset: 0x000EAB98
		public static Query<T> TakeWhile<T>(this Query<T> query, Func<T, bool> predicate)
		{
			T[] array;
			int num;
			query.Deconstruct(out array, out num);
			int i;
			for (i = 0; i < num; i++)
			{
				if (!predicate(array[i]))
				{
					break;
				}
			}
			Array.Clear(array, i, array.Length - i);
			return new Query<T>(array, i);
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000EC7EC File Offset: 0x000EABEC
		public static Query<T> Where<T>(this Query<T> query, Func<T, bool> predicate)
		{
			T[] array;
			int num;
			query.Deconstruct(out array, out num);
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (predicate(array[i]))
				{
					array[num2++] = array[i];
				}
			}
			Array.Clear(array, num2, array.Length - num2);
			return new Query<T>(array, num2);
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x000EC84F File Offset: 0x000EAC4F
		public static Query<T> ValidUnityObjs<T>(this Query<T> query) where T : UnityEngine.Object
		{
			return query.Where(new Func<T, bool>(QueryOperatorExtensions.<ValidUnityObjs`1>m__0<T>));
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x000EC864 File Offset: 0x000EAC64
		public static Query<QueryOperatorExtensions.IndexedValue<T>> WithIndices<T>(this Query<T> query)
		{
			Query<QueryOperatorExtensions.IndexedValue<T>> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				QueryOperatorExtensions.IndexedValue<T>[] array = ArrayPool<QueryOperatorExtensions.IndexedValue<T>>.Spawn(querySlice.Count);
				for (int i = 0; i < querySlice.Count; i++)
				{
					array[i] = new QueryOperatorExtensions.IndexedValue<T>
					{
						index = i,
						value = querySlice[i]
					};
				}
				result = new Query<QueryOperatorExtensions.IndexedValue<T>>(array, querySlice.Count);
			}
			return result;
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000EC900 File Offset: 0x000EAD00
		public static Query<QueryOperatorExtensions.PrevPair<T>> WithPrevious<T>(this Query<T> query, int offset = 1, bool includeStart = false)
		{
			Query<QueryOperatorExtensions.PrevPair<T>> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				int num = (!includeStart) ? Mathf.Max(0, querySlice.Count - offset) : querySlice.Count;
				QueryOperatorExtensions.PrevPair<T>[] array = ArrayPool<QueryOperatorExtensions.PrevPair<T>>.Spawn(num);
				int num2 = 0;
				if (includeStart)
				{
					for (int i = 0; i < Mathf.Min(querySlice.Count, offset); i++)
					{
						array[num2++] = new QueryOperatorExtensions.PrevPair<T>
						{
							value = querySlice[i],
							prev = default(T),
							hasPrev = false
						};
					}
				}
				for (int j = offset; j < querySlice.Count; j++)
				{
					array[num2++] = new QueryOperatorExtensions.PrevPair<T>
					{
						value = querySlice[j],
						prev = querySlice[j - offset],
						hasPrev = true
					};
				}
				result = new Query<QueryOperatorExtensions.PrevPair<T>>(array, num);
			}
			return result;
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000ECA48 File Offset: 0x000EAE48
		public static Query<V> Zip<T, K, V>(this Query<T> query, ICollection<K> collection, Func<T, K, V> selector)
		{
			Query<V> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				int num = Mathf.Min(querySlice.Count, collection.Count);
				V[] array = ArrayPool<V>.Spawn(num);
				K[] array2 = ArrayPool<K>.Spawn(collection.Count);
				collection.CopyTo(array2, 0);
				for (int i = 0; i < num; i++)
				{
					array[i] = selector(querySlice[i], array2[i]);
				}
				ArrayPool<K>.Recycle(array2);
				result = new Query<V>(array, num);
			}
			return result;
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x000ECAF4 File Offset: 0x000EAEF4
		public static Query<V> Zip<T, K, V>(this Query<T> query, Query<K> otherQuery, Func<T, K, V> selector)
		{
			Query<V> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				using (Query<K>.QuerySlice querySlice2 = otherQuery.Deconstruct())
				{
					int num = Mathf.Min(querySlice.Count, querySlice2.Count);
					V[] array = ArrayPool<V>.Spawn(num);
					for (int i = 0; i < num; i++)
					{
						array[i] = selector(querySlice[i], querySlice2[i]);
					}
					result = new Query<V>(array, num);
				}
			}
			return result;
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x000ECBAC File Offset: 0x000EAFAC
		[CompilerGenerated]
		private static bool <ValidUnityObjs<T>(T t) where T : UnityEngine.Object
		{
			UnityEngine.Object x = t;
			return x != null;
		}

		// Token: 0x0200070C RID: 1804
		public struct PrevPair<T>
		{
			// Token: 0x04002358 RID: 9048
			public T value;

			// Token: 0x04002359 RID: 9049
			public T prev;

			// Token: 0x0400235A RID: 9050
			public bool hasPrev;
		}

		// Token: 0x0200070D RID: 1805
		public struct IndexedValue<T>
		{
			// Token: 0x0400235B RID: 9051
			public int index;

			// Token: 0x0400235C RID: 9052
			public T value;
		}

		// Token: 0x0200070E RID: 1806
		private class FunctorComparer<T, K> : IComparer<T> where K : IComparable<K>
		{
			// Token: 0x06002C01 RID: 11265 RVA: 0x000ECBC7 File Offset: 0x000EAFC7
			private FunctorComparer()
			{
			}

			// Token: 0x06002C02 RID: 11266 RVA: 0x000ECBCF File Offset: 0x000EAFCF
			public static QueryOperatorExtensions.FunctorComparer<T, K> Ascending(Func<T, K> functor)
			{
				return QueryOperatorExtensions.FunctorComparer<T, K>.single(functor, 1);
			}

			// Token: 0x06002C03 RID: 11267 RVA: 0x000ECBD8 File Offset: 0x000EAFD8
			public static QueryOperatorExtensions.FunctorComparer<T, K> Descending(Func<T, K> functor)
			{
				return QueryOperatorExtensions.FunctorComparer<T, K>.single(functor, -1);
			}

			// Token: 0x06002C04 RID: 11268 RVA: 0x000ECBE1 File Offset: 0x000EAFE1
			private static QueryOperatorExtensions.FunctorComparer<T, K> single(Func<T, K> functor, int sign)
			{
				if (QueryOperatorExtensions.FunctorComparer<T, K>._single == null)
				{
					QueryOperatorExtensions.FunctorComparer<T, K>._single = new QueryOperatorExtensions.FunctorComparer<T, K>();
				}
				QueryOperatorExtensions.FunctorComparer<T, K>._single._functor = functor;
				QueryOperatorExtensions.FunctorComparer<T, K>._single._sign = sign;
				return QueryOperatorExtensions.FunctorComparer<T, K>._single;
			}

			// Token: 0x06002C05 RID: 11269 RVA: 0x000ECC12 File Offset: 0x000EB012
			public void Clear()
			{
				this._functor = null;
			}

			// Token: 0x06002C06 RID: 11270 RVA: 0x000ECC1C File Offset: 0x000EB01C
			public int Compare(T x, T y)
			{
				int sign = this._sign;
				K k = this._functor(x);
				return sign * k.CompareTo(this._functor(y));
			}

			// Token: 0x0400235D RID: 9053
			[ThreadStatic]
			private static QueryOperatorExtensions.FunctorComparer<T, K> _single;

			// Token: 0x0400235E RID: 9054
			private Func<T, K> _functor;

			// Token: 0x0400235F RID: 9055
			private int _sign;
		}
	}
}
