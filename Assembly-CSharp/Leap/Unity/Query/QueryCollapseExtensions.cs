using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Leap.Unity.Query
{
	// Token: 0x02000709 RID: 1801
	public static class QueryCollapseExtensions
	{
		// Token: 0x06002BAD RID: 11181 RVA: 0x000EB19C File Offset: 0x000E959C
		public static bool All<T>(this Query<T> query, Func<T, bool> predicate)
		{
			bool result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				for (int i = 0; i < querySlice.Count; i++)
				{
					if (!predicate(querySlice[i]))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x000EB20C File Offset: 0x000E960C
		public static bool AllEqual<T>(this Query<T> query)
		{
			bool result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (querySlice.Count <= 1)
				{
					result = true;
				}
				else
				{
					EqualityComparer<T> @default = EqualityComparer<T>.Default;
					T x = querySlice[0];
					for (int i = 1; i < querySlice.Count; i++)
					{
						if (!@default.Equals(x, querySlice[i]))
						{
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x000EB2A4 File Offset: 0x000E96A4
		public static bool Any<T>(this Query<T> query)
		{
			return query.Count<T>() > 0;
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x000EB2B0 File Offset: 0x000E96B0
		public static bool Any<T>(this Query<T> query, Func<T, bool> predicate)
		{
			bool result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				for (int i = 0; i < querySlice.Count; i++)
				{
					if (predicate(querySlice[i]))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x000EB320 File Offset: 0x000E9720
		public static float Average(this Query<float> query)
		{
			float result;
			using (Query<float>.QuerySlice querySlice = query.Deconstruct())
			{
				float num = 0f;
				for (int i = 0; i < querySlice.Count; i++)
				{
					num += querySlice[i];
				}
				result = num / (float)querySlice.Count;
			}
			return result;
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x000EB390 File Offset: 0x000E9790
		public static double Average(this Query<double> query)
		{
			double result;
			using (Query<double>.QuerySlice querySlice = query.Deconstruct())
			{
				double num = 0.0;
				for (int i = 0; i < querySlice.Count; i++)
				{
					num += querySlice[i];
				}
				result = num / (double)querySlice.Count;
			}
			return result;
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x000EB404 File Offset: 0x000E9804
		public static bool Contains<T>(this Query<T> query, T item)
		{
			T[] array;
			int num;
			query.Deconstruct(out array, out num);
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < num; i++)
			{
				if (@default.Equals(item, array[i]))
				{
					ArrayPool<T>.Recycle(array);
					return true;
				}
			}
			ArrayPool<T>.Recycle(array);
			return false;
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x000EB458 File Offset: 0x000E9858
		public static int Count<T>(this Query<T> query)
		{
			int count;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				count = querySlice.Count;
			}
			return count;
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x000EB49C File Offset: 0x000E989C
		public static int Count<T>(this Query<T> query, Func<T, bool> predicate)
		{
			return query.Where(predicate).Count<T>();
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000EB4AC File Offset: 0x000E98AC
		public static int CountUnique<T>(this Query<T> query)
		{
			Query<T>.QuerySlice querySlice = query.Deconstruct();
			HashSet<T> hashSet = Pool<HashSet<T>>.Spawn();
			int count;
			try
			{
				for (int i = 0; i < querySlice.Count; i++)
				{
					hashSet.Add(querySlice[i]);
				}
				count = hashSet.Count;
			}
			finally
			{
				querySlice.Dispose();
				hashSet.Clear();
				Pool<HashSet<T>>.Recycle(hashSet);
			}
			return count;
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x000EB520 File Offset: 0x000E9920
		public static int CountUnique<T, K>(this Query<T> query, Func<T, K> selector)
		{
			return query.Select(selector).CountUnique<K>();
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x000EB530 File Offset: 0x000E9930
		public static T ElementAt<T>(this Query<T> query, int index)
		{
			T result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (index < 0 || index >= querySlice.Count)
				{
					throw new IndexOutOfRangeException(string.Concat(new object[]
					{
						"The index ",
						index,
						" was out of range.  Query only has length of ",
						querySlice.Count
					}));
				}
				result = querySlice[index];
			}
			return result;
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x000EB5C0 File Offset: 0x000E99C0
		public static T ElementAtOrDefault<T>(this Query<T> query, int index)
		{
			T result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (index < 0 || index >= querySlice.Count)
				{
					result = default(T);
				}
				else
				{
					result = querySlice[index];
				}
			}
			return result;
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x000EB628 File Offset: 0x000E9A28
		public static T First<T>(this Query<T> query)
		{
			T result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (querySlice.Count == 0)
				{
					throw new InvalidOperationException("The source Query was empty.");
				}
				result = querySlice[0];
			}
			return result;
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x000EB684 File Offset: 0x000E9A84
		public static T First<T>(this Query<T> query, Func<T, bool> predicate)
		{
			return query.Where(predicate).First<T>();
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x000EB694 File Offset: 0x000E9A94
		public static T FirstOrDefault<T>(this Query<T> query)
		{
			T result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (querySlice.Count == 0)
				{
					result = default(T);
				}
				else
				{
					result = querySlice[0];
				}
			}
			return result;
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x000EB6F4 File Offset: 0x000E9AF4
		public static T FirstOrDefault<T>(this Query<T> query, Func<T, bool> predicate)
		{
			return query.Where(predicate).FirstOrDefault<T>();
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x000EB704 File Offset: 0x000E9B04
		public static Maybe<T> FirstOrNone<T>(this Query<T> query)
		{
			Maybe<T> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (querySlice.Count == 0)
				{
					result = Maybe.None;
				}
				else
				{
					result = Maybe.Some<T>(querySlice[0]);
				}
			}
			return result;
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x000EB768 File Offset: 0x000E9B68
		public static Maybe<T> FirstOrNone<T>(this Query<T> query, Func<T, bool> predicate)
		{
			return query.Where(predicate).FirstOrNone<T>();
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x000EB778 File Offset: 0x000E9B78
		public static T Fold<T>(this Query<T> query, Func<T, T, T> foldFunc)
		{
			T result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (querySlice.Count == 0)
				{
					throw new InvalidOperationException("The source Query was empty.");
				}
				T t = querySlice[0];
				for (int i = 1; i < querySlice.Count; i++)
				{
					t = foldFunc(t, querySlice[i]);
				}
				result = t;
			}
			return result;
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x000EB7FC File Offset: 0x000E9BFC
		public static int IndexOf<T>(this Query<T> query, T t)
		{
			int result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				EqualityComparer<T> @default = EqualityComparer<T>.Default;
				for (int i = 0; i < querySlice.Count; i++)
				{
					if (@default.Equals(t, querySlice[i]))
					{
						return i;
					}
				}
				result = -1;
			}
			return result;
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x000EB870 File Offset: 0x000E9C70
		public static int IndexOf<T>(this Query<T> query, Func<T, bool> predicate)
		{
			int result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				for (int i = 0; i < querySlice.Count; i++)
				{
					if (predicate(querySlice[i]))
					{
						return i;
					}
				}
				result = -1;
			}
			return result;
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x000EB8E0 File Offset: 0x000E9CE0
		public static T Last<T>(this Query<T> query)
		{
			T result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (querySlice.Count == 0)
				{
					throw new InvalidOperationException("The source Query was empty.");
				}
				result = querySlice[querySlice.Count - 1];
			}
			return result;
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x000EB944 File Offset: 0x000E9D44
		public static T Last<T>(this Query<T> query, Func<T, bool> predicate)
		{
			return query.Where(predicate).Last<T>();
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x000EB954 File Offset: 0x000E9D54
		public static T LastOrDefault<T>(this Query<T> query)
		{
			T result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (querySlice.Count == 0)
				{
					result = default(T);
				}
				else
				{
					result = querySlice[querySlice.Count - 1];
				}
			}
			return result;
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x000EB9BC File Offset: 0x000E9DBC
		public static T LastOrDefault<T>(this Query<T> query, Func<T, bool> predicate)
		{
			return query.Where(predicate).LastOrDefault<T>();
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x000EB9CC File Offset: 0x000E9DCC
		public static Maybe<T> LastOrNone<T>(this Query<T> query)
		{
			Maybe<T> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (querySlice.Count == 0)
				{
					result = Maybe.None;
				}
				else
				{
					result = Maybe.Some<T>(querySlice[querySlice.Count - 1]);
				}
			}
			return result;
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x000EBA38 File Offset: 0x000E9E38
		public static Maybe<T> LastOrNone<T>(this Query<T> query, Func<T, bool> predicate)
		{
			return query.Where(predicate).LastOrNone<T>();
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x000EBA46 File Offset: 0x000E9E46
		public static T Max<T>(this Query<T> query) where T : IComparable<T>
		{
			return query.Fold(QueryCollapseExtensions.FoldDelegate<T>.max);
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x000EBA53 File Offset: 0x000E9E53
		public static K Max<T, K>(this Query<T> query, Func<T, K> selector) where K : IComparable<K>
		{
			return query.Select(selector).Max<K>();
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x000EBA61 File Offset: 0x000E9E61
		public static T Min<T>(this Query<T> query) where T : IComparable<T>
		{
			return query.Fold(QueryCollapseExtensions.FoldDelegate<T>.min);
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x000EBA6E File Offset: 0x000E9E6E
		public static K Min<T, K>(this Query<T> query, Func<T, K> selector) where K : IComparable<K>
		{
			return query.Select(selector).Min<K>();
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x000EBA7C File Offset: 0x000E9E7C
		public static T Single<T>(this Query<T> query)
		{
			T result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (querySlice.Count != 1)
				{
					throw new InvalidOperationException("The Query had a count of " + querySlice.Count + " instead of a count of 1.");
				}
				result = querySlice[0];
			}
			return result;
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x000EBAEC File Offset: 0x000E9EEC
		public static T Single<T>(this Query<T> query, Func<T, bool> predicate)
		{
			return query.Where(predicate).Single<T>();
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x000EBAFC File Offset: 0x000E9EFC
		public static T SingleOrDefault<T>(this Query<T> query)
		{
			T result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (querySlice.Count != 1)
				{
					result = default(T);
				}
				else
				{
					result = querySlice[0];
				}
			}
			return result;
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x000EBB5C File Offset: 0x000E9F5C
		public static T SingleOrDefault<T>(this Query<T> query, Func<T, bool> predicate)
		{
			return query.Where(predicate).SingleOrDefault<T>();
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x000EBB6C File Offset: 0x000E9F6C
		public static Maybe<T> SingleOrNone<T>(this Query<T> query)
		{
			Maybe<T> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (querySlice.Count != 1)
				{
					result = Maybe.None;
				}
				else
				{
					result = Maybe.Some<T>(querySlice[0]);
				}
			}
			return result;
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x000EBBD0 File Offset: 0x000E9FD0
		public static Maybe<T> SingleOrNone<T>(this Query<T> query, Func<T, bool> predicate)
		{
			return query.Where(predicate).SingleOrNone<T>();
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x000EBBDE File Offset: 0x000E9FDE
		public static int Sum(this Query<int> query)
		{
			if (QueryCollapseExtensions.<>f__am$cache0 == null)
			{
				QueryCollapseExtensions.<>f__am$cache0 = new Func<int, int, int>(QueryCollapseExtensions.<Sum>m__0);
			}
			return query.Fold(QueryCollapseExtensions.<>f__am$cache0);
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x000EBC03 File Offset: 0x000EA003
		public static float Sum(this Query<float> query)
		{
			if (QueryCollapseExtensions.<>f__am$cache1 == null)
			{
				QueryCollapseExtensions.<>f__am$cache1 = new Func<float, float, float>(QueryCollapseExtensions.<Sum>m__1);
			}
			return query.Fold(QueryCollapseExtensions.<>f__am$cache1);
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x000EBC28 File Offset: 0x000EA028
		public static double Sum(this Query<double> query)
		{
			if (QueryCollapseExtensions.<>f__am$cache2 == null)
			{
				QueryCollapseExtensions.<>f__am$cache2 = new Func<double, double, double>(QueryCollapseExtensions.<Sum>m__2);
			}
			return query.Fold(QueryCollapseExtensions.<>f__am$cache2);
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x000EBC50 File Offset: 0x000EA050
		public static T UniformOrDefault<T>(this Query<T> query)
		{
			return query.UniformOrNone<T>().valueOrDefault;
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x000EBC6C File Offset: 0x000EA06C
		public static Maybe<T> UniformOrNone<T>(this Query<T> query)
		{
			Maybe<T> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				if (querySlice.Count == 0)
				{
					result = Maybe.None;
				}
				else
				{
					Query<T>.QuerySlice querySlice2 = querySlice;
					T t = querySlice2[0];
					EqualityComparer<T> @default = EqualityComparer<T>.Default;
					for (int i = 1; i < querySlice.Count; i++)
					{
						if (!@default.Equals(t, querySlice[i]))
						{
							return Maybe.None;
						}
					}
					result = Maybe.Some<T>(t);
				}
			}
			return result;
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x000EBD1C File Offset: 0x000EA11C
		public static T[] ToArray<T>(this Query<T> query)
		{
			T[] result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				T[] array = new T[querySlice.Count];
				Array.Copy(querySlice.BackingArray, array, querySlice.Count);
				result = array;
			}
			return result;
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x000EBD78 File Offset: 0x000EA178
		public static void FillArray<T>(this Query<T> query, T[] array, int offset = 0)
		{
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				Array.Copy(querySlice.BackingArray, 0, array, offset, querySlice.Count);
			}
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x000EBDC8 File Offset: 0x000EA1C8
		public static List<T> ToList<T>(this Query<T> query)
		{
			List<T> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				List<T> list = new List<T>(querySlice.Count);
				for (int i = 0; i < querySlice.Count; i++)
				{
					list.Add(querySlice[i]);
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x000EBE38 File Offset: 0x000EA238
		public static void FillList<T>(this Query<T> query, List<T> list)
		{
			list.Clear();
			query.AppendList(list);
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x000EBE48 File Offset: 0x000EA248
		public static void AppendList<T>(this Query<T> query, List<T> list)
		{
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				for (int i = 0; i < querySlice.Count; i++)
				{
					list.Add(querySlice[i]);
				}
			}
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000EBEA8 File Offset: 0x000EA2A8
		public static HashSet<T> ToHashSet<T>(this Query<T> query)
		{
			HashSet<T> hashSet = new HashSet<T>();
			query.AppendHashSet(hashSet);
			return hashSet;
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000EBEC3 File Offset: 0x000EA2C3
		public static void FillHashSet<T>(this Query<T> query, HashSet<T> set)
		{
			set.Clear();
			query.AppendHashSet(set);
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x000EBED4 File Offset: 0x000EA2D4
		public static void AppendHashSet<T>(this Query<T> query, HashSet<T> set)
		{
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				for (int i = 0; i < querySlice.Count; i++)
				{
					set.Add(querySlice[i]);
				}
			}
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x000EBF34 File Offset: 0x000EA334
		public static Dictionary<K, V> ToDictionary<T, K, V>(this Query<T> query, Func<T, K> keySelector, Func<T, V> valueSelector)
		{
			Dictionary<K, V> result;
			using (Query<T>.QuerySlice querySlice = query.Deconstruct())
			{
				Dictionary<K, V> dictionary = new Dictionary<K, V>();
				for (int i = 0; i < querySlice.Count; i++)
				{
					dictionary[keySelector(querySlice[i])] = valueSelector(querySlice[i]);
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x000EBFB0 File Offset: 0x000EA3B0
		public static Dictionary<T, V> ToDictionary<T, V>(this Query<T> query, Func<T, V> valueSelector)
		{
			return query.ToDictionary(new Func<T, T>(QueryCollapseExtensions.<ToDictionary`2>m__3<T, V>), valueSelector);
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x000EBFC5 File Offset: 0x000EA3C5
		[CompilerGenerated]
		private static int <Sum>m__0(int a, int b)
		{
			return a + b;
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000EBFCA File Offset: 0x000EA3CA
		[CompilerGenerated]
		private static float <Sum>m__1(float a, float b)
		{
			return a + b;
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000EBFCF File Offset: 0x000EA3CF
		[CompilerGenerated]
		private static double <Sum>m__2(double a, double b)
		{
			return a + b;
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000EBFD4 File Offset: 0x000EA3D4
		[CompilerGenerated]
		private static T <ToDictionary<T, V>(T t)
		{
			return t;
		}

		// Token: 0x04002353 RID: 9043
		[CompilerGenerated]
		private static Func<int, int, int> <>f__am$cache0;

		// Token: 0x04002354 RID: 9044
		[CompilerGenerated]
		private static Func<float, float, float> <>f__am$cache1;

		// Token: 0x04002355 RID: 9045
		[CompilerGenerated]
		private static Func<double, double, double> <>f__am$cache2;

		// Token: 0x0200070A RID: 1802
		private static class FoldDelegate<T> where T : IComparable<T>
		{
			// Token: 0x06002BE6 RID: 11238 RVA: 0x000EBFD7 File Offset: 0x000EA3D7
			// Note: this type is marked as 'beforefieldinit'.
			static FoldDelegate()
			{
			}

			// Token: 0x06002BE7 RID: 11239 RVA: 0x000EBFFB File Offset: 0x000EA3FB
			[CompilerGenerated]
			private static T <max>m__0(T a, T b)
			{
				return (a.CompareTo(b) <= 0) ? b : a;
			}

			// Token: 0x06002BE8 RID: 11240 RVA: 0x000EC018 File Offset: 0x000EA418
			[CompilerGenerated]
			private static T <min>m__1(T a, T b)
			{
				return (a.CompareTo(b) >= 0) ? b : a;
			}

			// Token: 0x04002356 RID: 9046
			public static readonly Func<T, T, T> max = new Func<T, T, T>(QueryCollapseExtensions.FoldDelegate<T>.<max>m__0);

			// Token: 0x04002357 RID: 9047
			public static readonly Func<T, T, T> min = new Func<T, T, T>(QueryCollapseExtensions.FoldDelegate<T>.<min>m__1);
		}
	}
}
