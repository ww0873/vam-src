using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Leap.Unity.Query;
using Leap.Unity.Swizzle;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200074E RID: 1870
	public static class Utils
	{
		// Token: 0x06002F88 RID: 12168 RVA: 0x000F740C File Offset: 0x000F580C
		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x000F7434 File Offset: 0x000F5834
		public static void Swap<T>(this IList<T> list, int a, int b)
		{
			T value = list[a];
			list[a] = list[b];
			list[b] = value;
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x000F745F File Offset: 0x000F585F
		public static void Swap<T>(this T[] array, int a, int b)
		{
			Utils.Swap<T>(ref array[a], ref array[b]);
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x000F7474 File Offset: 0x000F5874
		public static void Reverse<T>(this T[] array)
		{
			int num = array.Length / 2;
			int i = 0;
			int num2 = array.Length;
			while (i < num)
			{
				array.Swap(i++, --num2);
			}
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x000F74AC File Offset: 0x000F58AC
		public static void Reverse<T>(this T[] array, int start, int length)
		{
			int num = start + length / 2;
			int i = start;
			int num2 = start + length;
			while (i < num)
			{
				array.Swap(i++, --num2);
			}
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x000F74E4 File Offset: 0x000F58E4
		public static void Shuffle<T>(this IList<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list.Swap(i, UnityEngine.Random.Range(i, list.Count));
			}
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x000F751C File Offset: 0x000F591C
		public static void DoubleCapacity<T>(ref T[] array)
		{
			T[] array2 = new T[array.Length * 2];
			Array.Copy(array, array2, array.Length);
			array = array2;
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x000F7544 File Offset: 0x000F5944
		public static bool AreEqualUnordered<T>(IList<T> a, IList<T> b)
		{
			Dictionary<T, int> dictionary = Pool<Dictionary<T, int>>.Spawn();
			bool result;
			try
			{
				int num = 0;
				foreach (T t in a)
				{
					if (t == null)
					{
						num++;
					}
					else
					{
						int num2;
						if (!dictionary.TryGetValue(t, out num2))
						{
							num2 = 0;
						}
						dictionary[t] = num2 + 1;
					}
				}
				foreach (T t2 in b)
				{
					if (t2 == null)
					{
						num--;
					}
					else
					{
						int num3;
						if (!dictionary.TryGetValue(t2, out num3))
						{
							return false;
						}
						dictionary[t2] = num3 - 1;
					}
				}
				if (num != 0)
				{
					result = false;
				}
				else
				{
					foreach (KeyValuePair<T, int> keyValuePair in dictionary)
					{
						if (keyValuePair.Value != 0)
						{
							return false;
						}
					}
					result = true;
				}
			}
			finally
			{
				dictionary.Clear();
				Pool<Dictionary<T, int>>.Recycle(dictionary);
			}
			return result;
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x000F76FC File Offset: 0x000F5AFC
		public static bool ImplementsInterface(this Type type, Type ifaceType)
		{
			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				if (interfaces[i] == ifaceType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000F7730 File Offset: 0x000F5B30
		public static bool IsActiveRelativeToParent(this Transform obj, Transform parent)
		{
			return obj.gameObject.activeSelf && (obj.parent == null || obj.parent == parent || obj.parent.IsActiveRelativeToParent(parent));
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000F7780 File Offset: 0x000F5B80
		public static List<int> GetSortedOrder<T>(this IList<T> list) where T : IComparable<T>
		{
			Utils.<GetSortedOrder>c__AnonStorey0<T> <GetSortedOrder>c__AnonStorey = new Utils.<GetSortedOrder>c__AnonStorey0<T>();
			<GetSortedOrder>c__AnonStorey.list = list;
			List<int> list2 = new List<int>();
			for (int i = 0; i < <GetSortedOrder>c__AnonStorey.list.Count; i++)
			{
				list2.Add(i);
			}
			list2.Sort(new Comparison<int>(<GetSortedOrder>c__AnonStorey.<>m__0));
			return list2;
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x000F77D8 File Offset: 0x000F5BD8
		public static void ApplyOrdering<T>(this IList<T> list, List<int> ordering)
		{
			List<T> list2 = Pool<List<T>>.Spawn();
			try
			{
				list2.AddRange(list);
				for (int i = 0; i < list.Count; i++)
				{
					list[i] = list2[ordering[i]];
				}
			}
			finally
			{
				list2.Clear();
				Pool<List<T>>.Recycle(list2);
			}
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000F7840 File Offset: 0x000F5C40
		public static string MakeRelativePath(string relativeTo, string path)
		{
			if (string.IsNullOrEmpty(relativeTo))
			{
				throw new ArgumentNullException("relativeTo");
			}
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			Uri uri = new Uri(relativeTo);
			Uri uri2 = new Uri(path);
			if (uri.Scheme != uri2.Scheme)
			{
				return path;
			}
			Uri uri3 = uri.MakeRelativeUri(uri2);
			string text = Uri.UnescapeDataString(uri3.ToString());
			if (uri2.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
			{
				text = text.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			}
			return text;
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x000F78DB File Offset: 0x000F5CDB
		public static string TrimEnd(this string str, int characters)
		{
			return str.Substring(0, Mathf.Max(0, str.Length - characters));
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x000F78F2 File Offset: 0x000F5CF2
		public static string TrimStart(this string str, int characters)
		{
			return str.Substring(Mathf.Min(str.Length, characters));
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x000F7908 File Offset: 0x000F5D08
		public static string Capitalize(this string str)
		{
			char c = str[0];
			if (char.IsLetter(c))
			{
				return char.ToUpper(c) + str.Substring(1);
			}
			return str;
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x000F7944 File Offset: 0x000F5D44
		public static string GenerateNiceName(string value)
		{
			Utils.<GenerateNiceName>c__AnonStorey1 <GenerateNiceName>c__AnonStorey = new Utils.<GenerateNiceName>c__AnonStorey1();
			string text = string.Empty;
			<GenerateNiceName>c__AnonStorey.curr = string.Empty;
			Func<char, bool> func = new Func<char, bool>(<GenerateNiceName>c__AnonStorey.<>m__0);
			Func<char, bool> func2 = new Func<char, bool>(<GenerateNiceName>c__AnonStorey.<>m__1);
			Func<char, bool> func3 = new Func<char, bool>(<GenerateNiceName>c__AnonStorey.<>m__2);
			if (Utils.<>f__am$cache0 == null)
			{
				Utils.<>f__am$cache0 = new Func<char, bool>(Utils.<GenerateNiceName>m__0);
			}
			Func<char, bool> func4 = Utils.<>f__am$cache0;
			Func<char, bool> func5 = null;
			int num = value.Length;
			while (num != 0)
			{
				num--;
				char c = value[num];
				if (func5 == null || !func5(c))
				{
					if (<GenerateNiceName>c__AnonStorey.curr != string.Empty)
					{
						text = " " + <GenerateNiceName>c__AnonStorey.curr.Capitalize() + text;
						<GenerateNiceName>c__AnonStorey.curr = string.Empty;
					}
					if (func2(c))
					{
						func5 = func2;
					}
					else if (func(c))
					{
						func5 = func;
					}
					else if (func3(c))
					{
						func5 = func3;
					}
					else
					{
						if (!func4(c))
						{
							throw new Exception("Unexpected state, no function matched character " + c);
						}
						func5 = func4;
					}
				}
			}
			if (<GenerateNiceName>c__AnonStorey.curr != string.Empty)
			{
				text = <GenerateNiceName>c__AnonStorey.curr.Capitalize() + text;
			}
			text = text.Trim();
			if (text.StartsWith("M ") || text.StartsWith("K "))
			{
				text = text.Substring(2);
			}
			return text.Trim();
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x000F7AF4 File Offset: 0x000F5EF4
		public static string ToArrayString<T>(this IEnumerable<T> enumerable)
		{
			string text = "[" + typeof(T).Name + ": ";
			bool flag = false;
			foreach (T t in enumerable)
			{
				if (flag)
				{
					text += ", ";
				}
				text += t.ToString();
				flag = true;
			}
			text += "]";
			return text;
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x000F7B98 File Offset: 0x000F5F98
		public static int Repeat(int x, int m)
		{
			int num = x % m;
			return (num >= 0) ? num : (num + m);
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x000F7BB9 File Offset: 0x000F5FB9
		public static int Sign(int value)
		{
			if (value == 0)
			{
				return 0;
			}
			if (value > 0)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x000F7BCD File Offset: 0x000F5FCD
		public static Vector2 Perpendicular(this Vector2 vector)
		{
			return new Vector2(vector.y, -vector.x);
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x000F7BE4 File Offset: 0x000F5FE4
		public static Vector3 Perpendicular(this Vector3 vector)
		{
			float num = vector.x * vector.x;
			float num2 = vector.y * vector.y;
			float num3 = vector.z * vector.z;
			float num4 = num3 + num;
			float num5 = num2 + num;
			float num6 = num3 + num2;
			if (num4 > num5)
			{
				if (num4 > num6)
				{
					return new Vector3(-vector.z, 0f, vector.x);
				}
				return new Vector3(0f, vector.z, -vector.y);
			}
			else
			{
				if (num5 > num6)
				{
					return new Vector3(vector.y, -vector.x, 0f);
				}
				return new Vector3(0f, vector.z, -vector.y);
			}
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x000F7CAF File Offset: 0x000F60AF
		public static bool ContainsNaN(this Vector3 v)
		{
			return float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z);
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x000F7CE2 File Offset: 0x000F60E2
		public static bool IsBetween(this float f, float f0, float f1)
		{
			if (f0 > f1)
			{
				Utils.Swap<float>(ref f0, ref f1);
			}
			return f0 <= f && f <= f1;
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x000F7D05 File Offset: 0x000F6105
		public static bool IsBetween(this double d, double d0, double d1)
		{
			if (d0 > d1)
			{
				Utils.Swap<double>(ref d0, ref d1);
			}
			return d0 <= d && d <= d1;
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x000F7D28 File Offset: 0x000F6128
		public static Vector3 TimedExtrapolate(Vector3 a, float aTime, Vector3 b, float bTime, float extrapolatedTime)
		{
			return Vector3.LerpUnclamped(a, b, extrapolatedTime.MapUnclamped(aTime, bTime, 0f, 1f));
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x000F7D44 File Offset: 0x000F6144
		public static Quaternion TimedExtrapolate(Quaternion a, float aTime, Quaternion b, float bTime, float extrapolatedTime)
		{
			return Quaternion.SlerpUnclamped(a, b, extrapolatedTime.MapUnclamped(aTime, bTime, 0f, 1f));
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x000F7D60 File Offset: 0x000F6160
		public static bool NextTuple(IList<int> tuple, int maxValue)
		{
			Utils.<NextTuple>c__AnonStorey2 <NextTuple>c__AnonStorey = new Utils.<NextTuple>c__AnonStorey2();
			<NextTuple>c__AnonStorey.maxValue = maxValue;
			return Utils.NextTuple<int>(tuple, new Func<int, int>(<NextTuple>c__AnonStorey.<>m__0));
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x000F7D8C File Offset: 0x000F618C
		public static bool NextTuple<T>(IList<T> tuple, Func<T, T> nextItem) where T : IComparable<T>
		{
			for (int i = tuple.Count - 1; i >= 0; i--)
			{
				T t = tuple[i];
				T value = nextItem(t);
				tuple[i] = value;
				if (value.CompareTo(t) > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x000F7DE4 File Offset: 0x000F61E4
		public static T[] ClearWithDefaults<T>(this T[] arr)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = default(T);
			}
			return arr;
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x000F7E18 File Offset: 0x000F6218
		public static T[] ClearWith<T>(this T[] arr, T value)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = value;
			}
			return arr;
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x000F7E42 File Offset: 0x000F6242
		public static void EnsureListExists<T>(ref List<T> list)
		{
			if (list == null)
			{
				list = new List<T>();
			}
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x000F7E54 File Offset: 0x000F6254
		public static void EnsureListCount<T>(this List<T> list, int count)
		{
			if (list.Count == count)
			{
				return;
			}
			while (list.Count < count)
			{
				list.Add(default(T));
			}
			while (list.Count > count)
			{
				list.RemoveAt(list.Count - 1);
			}
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x000F7EB0 File Offset: 0x000F62B0
		public static void EnsureListCount<T>(this List<T> list, int count, Func<T> createT, Action<T> deleteT = null)
		{
			while (list.Count < count)
			{
				list.Add(createT());
			}
			while (list.Count > count)
			{
				T obj = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
				if (deleteT != null)
				{
					deleteT(obj);
				}
			}
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x000F7F15 File Offset: 0x000F6315
		public static void Add<T>(this List<T> list, T t0, T t1)
		{
			list.Add(t0);
			list.Add(t1);
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x000F7F25 File Offset: 0x000F6325
		public static void Add<T>(this List<T> list, T t0, T t1, T t2)
		{
			list.Add(t0);
			list.Add(t1);
			list.Add(t2);
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x000F7F3C File Offset: 0x000F633C
		public static void Add<T>(this List<T> list, T t0, T t1, T t2, T t3)
		{
			list.Add(t0);
			list.Add(t1);
			list.Add(t2);
			list.Add(t3);
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x000F7F5B File Offset: 0x000F635B
		public static T FindObjectInHierarchy<T>() where T : UnityEngine.Object
		{
			return Resources.FindObjectsOfTypeAll<T>().Query<T>().Where(new Func<T, bool>(Utils.<FindObjectInHierarchy`1>m__1<T>)).FirstOrDefault<T>();
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000F7F7D File Offset: 0x000F637D
		public static Utils.ChildrenEnumerator GetChildren(this Transform t)
		{
			return new Utils.ChildrenEnumerator(t);
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x000F7F85 File Offset: 0x000F6385
		public static void ResetLocalTransform(this Transform t)
		{
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x000F7FA8 File Offset: 0x000F63A8
		public static void ResetLocalPose(this Transform t)
		{
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x000F7FC0 File Offset: 0x000F63C0
		public static void FindOwnedChildComponents<ComponentType, OwnerType>(OwnerType rootObj, List<ComponentType> ownedComponents, bool includeInactiveObjects = false) where OwnerType : Component
		{
			ownedComponents.Clear();
			Stack<Transform> stack = Pool<Stack<Transform>>.Spawn();
			List<ComponentType> list = Pool<List<ComponentType>>.Spawn();
			try
			{
				stack.Push(rootObj.transform);
				while (stack.Count > 0)
				{
					Transform transform = stack.Pop();
					foreach (Transform transform2 in transform.GetChildren())
					{
						if (transform2.GetComponent<OwnerType>() == null && (includeInactiveObjects || transform2.gameObject.activeInHierarchy))
						{
							stack.Push(transform2);
						}
					}
					list.Clear();
					transform.GetComponents<ComponentType>(list);
					foreach (ComponentType item in list)
					{
						ownedComponents.Add(item);
					}
				}
			}
			finally
			{
				stack.Clear();
				Pool<Stack<Transform>>.Recycle(stack);
				list.Clear();
				Pool<List<ComponentType>>.Recycle(list);
			}
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x000F810C File Offset: 0x000F650C
		public static void LookAwayFrom(this Transform thisTransform, Transform transform)
		{
			thisTransform.rotation = Quaternion.LookRotation(thisTransform.position - transform.position, Vector3.up);
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x000F812F File Offset: 0x000F652F
		public static void LookAwayFrom(this Transform thisTransform, Transform transform, Vector3 upwards)
		{
			thisTransform.rotation = Quaternion.LookRotation(thisTransform.position - transform.position, upwards);
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x000F814E File Offset: 0x000F654E
		public static Vector3 ToVector3(this Vector4 v4)
		{
			return new Vector3(v4.x, v4.y, v4.z);
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x000F816A File Offset: 0x000F656A
		public static Vector3 InLocalSpace(this Vector3 v, Transform t)
		{
			return t.InverseTransformPoint(v);
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x000F8174 File Offset: 0x000F6574
		public static Vector3 ToAngleAxisVector(this Quaternion q)
		{
			float d;
			Vector3 a;
			q.ToAngleAxis(out d, out a);
			return a * d;
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x000F8193 File Offset: 0x000F6593
		public static Quaternion QuaternionFromAngleAxisVector(Vector3 angleAxisVector)
		{
			if (angleAxisVector == Vector3.zero)
			{
				return Quaternion.identity;
			}
			return Quaternion.AngleAxis(angleAxisVector.magnitude, angleAxisVector);
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x000F81B8 File Offset: 0x000F65B8
		public static Quaternion ToNormalized(this Quaternion quaternion)
		{
			float x = quaternion.x;
			float y = quaternion.y;
			float z = quaternion.z;
			float w = quaternion.w;
			float num = Mathf.Sqrt(x * x + y * y + z * z + w * w);
			if (Mathf.Approximately(num, 0f))
			{
				return Quaternion.identity;
			}
			return new Quaternion(x / num, y / num, z / num, w / num);
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x000F8227 File Offset: 0x000F6627
		public static Quaternion FaceTargetWithoutTwist(Vector3 fromPosition, Vector3 targetPosition, bool flip180 = false)
		{
			return Utils.FaceTargetWithoutTwist(fromPosition, targetPosition, Vector3.up, flip180);
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x000F8238 File Offset: 0x000F6638
		public static Quaternion FaceTargetWithoutTwist(Vector3 fromPosition, Vector3 targetPosition, Vector3 upwardDirection, bool flip180 = false)
		{
			Vector3 a = targetPosition - fromPosition;
			return Quaternion.LookRotation((float)((!flip180) ? 1 : -1) * a, upwardDirection);
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x000F8267 File Offset: 0x000F6667
		public static Quaternion Flipped(this Quaternion q)
		{
			return new Quaternion(-q.x, -q.y, -q.z, -q.w);
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x000F8290 File Offset: 0x000F6690
		public static void CompressQuatToBytes(Quaternion quat, byte[] buffer, ref int offset)
		{
			int num = 0;
			float num2 = Mathf.Abs(quat.w);
			float num3 = Mathf.Abs(quat.x);
			float num4 = Mathf.Abs(quat.y);
			float num5 = Mathf.Abs(quat.z);
			float num6 = num3;
			if (num4 > num6)
			{
				num = 1;
				num6 = num4;
			}
			if (num5 > num6)
			{
				num = 2;
				num6 = num5;
			}
			if (num2 > num6)
			{
				num = 3;
			}
			float num7;
			float num8;
			float num9;
			if (quat[num] >= 0f)
			{
				num7 = quat[(num + 1) % 4];
				num8 = quat[(num + 2) % 4];
				num9 = quat[(num + 3) % 4];
			}
			else
			{
				num7 = -quat[(num + 1) % 4];
				num8 = -quat[(num + 2) % 4];
				num9 = -quat[(num + 3) % 4];
			}
			uint num10 = (uint)((uint)num << 30);
			float num11 = Mathf.Clamp01((num7 - -0.70710653f) / 1.4142131f);
			uint num12 = (uint)Mathf.Floor(num11 * 1023f + 0.5f);
			num10 |= (num12 & 1023U) << 20;
			num11 = Mathf.Clamp01((num8 - -0.70710653f) / 1.4142131f);
			num12 = (uint)Mathf.Floor(num11 * 1023f + 0.5f);
			num10 |= (num12 & 1023U) << 10;
			num11 = Mathf.Clamp01((num9 - -0.70710653f) / 1.4142131f);
			num12 = (uint)Mathf.Floor(num11 * 1023f + 0.5f);
			num10 |= (num12 & 1023U);
			BitConverterNonAlloc.GetBytes(num10, buffer, ref offset);
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x000F842C File Offset: 0x000F682C
		public static Quaternion DecompressBytesToQuat(byte[] bytes, ref int offset)
		{
			uint num = BitConverterNonAlloc.ToUInt32(bytes, ref offset);
			int num2 = (int)(num >> 30);
			uint num3 = num >> 20 & 1023U;
			float num4 = num3 / 1023f;
			float num5 = num4 * 1.4142131f + -0.70710653f;
			num3 = (num >> 10 & 1023U);
			num4 = num3 / 1023f;
			float num6 = num4 * 1.4142131f + -0.70710653f;
			num3 = (num & 1023U);
			num4 = num3 / 1023f;
			float num7 = num4 * 1.4142131f + -0.70710653f;
			Quaternion identity = Quaternion.identity;
			float value = Mathf.Sqrt(1f - num5 * num5 - num6 * num6 - num7 * num7);
			identity[num2] = value;
			identity[(num2 + 1) % 4] = num5;
			identity[(num2 + 2) % 4] = num6;
			identity[(num2 + 3) % 4] = num7;
			return identity;
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x000F850E File Offset: 0x000F690E
		public static Matrix4x4 CompMul(Matrix4x4 m, float f)
		{
			return new Matrix4x4(m.GetColumn(0) * f, m.GetColumn(1) * f, m.GetColumn(2) * f, m.GetColumn(3) * f);
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x000F8550 File Offset: 0x000F6950
		public static void IgnoreCollisions(GameObject first, GameObject second, bool ignore = true)
		{
			if (first == null || second == null)
			{
				return;
			}
			List<Collider> list = Pool<List<Collider>>.Spawn();
			list.Clear();
			List<Collider> list2 = Pool<List<Collider>>.Spawn();
			list2.Clear();
			try
			{
				first.GetComponentsInChildren<Collider>(list);
				second.GetComponentsInChildren<Collider>(list2);
				for (int i = 0; i < list.Count; i++)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						if (list[i] != list2[j] && list[i].enabled && list2[j].enabled)
						{
							Physics.IgnoreCollision(list[i], list2[j], ignore);
						}
					}
				}
			}
			finally
			{
				list.Clear();
				Pool<List<Collider>>.Recycle(list);
				list2.Clear();
				Pool<List<Collider>>.Recycle(list2);
			}
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x000F8648 File Offset: 0x000F6A48
		public static Vector3 GetDirection(this CapsuleCollider capsule)
		{
			switch (capsule.direction)
			{
			case 0:
				return Vector3.right;
			case 1:
				return Vector3.up;
			}
			return Vector3.forward;
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x000F8684 File Offset: 0x000F6A84
		public static float GetEffectiveRadius(this CapsuleCollider capsule)
		{
			return capsule.radius * capsule.GetEffectiveRadiusMultiplier();
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x000F8694 File Offset: 0x000F6A94
		public static float GetEffectiveRadiusMultiplier(this CapsuleCollider capsule)
		{
			switch (capsule.direction)
			{
			case 0:
				return capsule.transform.lossyScale.yz().CompMax();
			case 1:
				return capsule.transform.lossyScale.xz().CompMax();
			}
			return capsule.transform.lossyScale.xy().CompMax();
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x000F8718 File Offset: 0x000F6B18
		public static void GetCapsulePoints(this CapsuleCollider capsule, out Vector3 a, out Vector3 b)
		{
			float effectiveRadiusMultiplier = capsule.GetEffectiveRadiusMultiplier();
			Vector3 direction = capsule.GetDirection();
			a = direction * (capsule.height / 2f);
			b = -a;
			a = capsule.transform.TransformPoint(a);
			b = capsule.transform.TransformPoint(b);
			a -= direction * effectiveRadiusMultiplier * capsule.radius;
			b += direction * effectiveRadiusMultiplier * capsule.radius;
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x000F87D0 File Offset: 0x000F6BD0
		public static void SetCapsulePoints(this CapsuleCollider capsule, Vector3 a, Vector3 b)
		{
			capsule.center = Vector3.zero;
			capsule.transform.position = (a + b) / 2f;
			Vector3 direction = capsule.GetDirection();
			Vector3 fromDirection = capsule.transform.TransformDirection(direction);
			Quaternion lhs = Quaternion.FromToRotation(fromDirection, a - capsule.transform.position);
			capsule.transform.rotation = lhs * capsule.transform.rotation;
			float magnitude = capsule.transform.InverseTransformPoint(a).magnitude;
			capsule.height = (magnitude + capsule.radius) * 2f;
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x000F8878 File Offset: 0x000F6C78
		public static void FindColliders<T>(GameObject obj, List<T> colliders, bool includeInactiveObjects = false) where T : Collider
		{
			colliders.Clear();
			Stack<Transform> stack = Pool<Stack<Transform>>.Spawn();
			List<T> list = Pool<List<T>>.Spawn();
			try
			{
				stack.Push(obj.transform);
				while (stack.Count > 0)
				{
					Transform transform = stack.Pop();
					foreach (Transform transform2 in transform.GetChildren())
					{
						if (transform2.GetComponent<Rigidbody>() == null && (includeInactiveObjects || transform2.gameObject.activeSelf))
						{
							stack.Push(transform2);
						}
					}
					list.Clear();
					transform.GetComponents<T>(list);
					foreach (T item in list)
					{
						colliders.Add(item);
					}
				}
			}
			finally
			{
				stack.Clear();
				Pool<Stack<Transform>>.Recycle(stack);
				list.Clear();
				Pool<List<T>>.Recycle(list);
			}
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x000F89B8 File Offset: 0x000F6DB8
		public static Color WithAlpha(this Color color, float alpha)
		{
			return new Color(color.r, color.g, color.b, alpha);
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x000F89D8 File Offset: 0x000F6DD8
		public static Color ParseHtmlColorString(string htmlString)
		{
			Color result;
			if (!ColorUtility.TryParseHtmlString(htmlString, out result))
			{
				throw new ArgumentException("The string [" + htmlString + "] is not a valid color code.  Valid color codes include:\n#RGB\n#RGBA\n#RRGGBB\n#RRGGBBAA\nFor more information, see the documentation for ColorUtility.TryParseHtmlString.");
			}
			return result;
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000F8A0C File Offset: 0x000F6E0C
		public static Color LerpHSV(this Color color, Color towardsColor, float t)
		{
			float num;
			float a;
			float a2;
			Color.RGBToHSV(color, out num, out a, out a2);
			float num2;
			float b;
			float b2;
			Color.RGBToHSV(towardsColor, out num2, out b, out b2);
			if (num - num2 < -0.5f)
			{
				num += 1f;
			}
			if (num - num2 > 0.5f)
			{
				num2 += 1f;
			}
			float h = Mathf.Lerp(num, num2, t) % 1f;
			float s = Mathf.Lerp(a, b, t);
			float v = Mathf.Lerp(a2, b2, t);
			return Color.HSVToRGB(h, s, v);
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000F8A8C File Offset: 0x000F6E8C
		public static float LerpHue(float h0, float h1, float t)
		{
			if (h0 < 0f)
			{
				h0 = 1f - -h0 % 1f;
			}
			if (h1 < 0f)
			{
				h1 = 1f - -h1 % 1f;
			}
			if (h0 > 1f)
			{
				h0 %= 1f;
			}
			if (h1 > 1f)
			{
				h1 %= 1f;
			}
			if (h0 - h1 < -0.5f)
			{
				h0 += 1f;
			}
			if (h0 - h1 > 0.5f)
			{
				h1 += 1f;
			}
			return Mathf.Lerp(h0, h1, t) % 1f;
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x000F8B34 File Offset: 0x000F6F34
		public static void DrawCircle(Vector3 center, Vector3 normal, float radius, Color color, int quality = 32, float duration = 0f, bool depthTest = true)
		{
			Vector3 forward = Vector3.Slerp(normal, -normal, 0.5f);
			Utils.DrawArc(360f, center, forward, normal, radius, color, quality);
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x000F8B64 File Offset: 0x000F6F64
		public static void DrawArc(float arc, Vector3 center, Vector3 forward, Vector3 normal, float radius, Color color, int quality = 32)
		{
			Gizmos.color = color;
			Vector3 normalized = Vector3.Cross(normal, forward).normalized;
			float num = arc / (float)quality;
			Vector3 from = center + forward * radius;
			Vector3 vector = default(Vector3);
			float num2 = 0f;
			while (Mathf.Abs(num2) <= Mathf.Abs(arc))
			{
				float num3 = Mathf.Cos(num2 * 0.017453292f);
				float num4 = Mathf.Sin(num2 * 0.017453292f);
				vector.x = center.x + radius * (num3 * forward.x + num4 * normalized.x);
				vector.y = center.y + radius * (num3 * forward.y + num4 * normalized.y);
				vector.z = center.z + radius * (num3 * forward.z + num4 * normalized.z);
				Gizmos.DrawLine(from, vector);
				from = vector;
				num2 += num;
			}
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x000F8C68 File Offset: 0x000F7068
		public static void DrawCone(Vector3 origin, Vector3 direction, float angle, float height, Color color, int quality = 4, float duration = 0f, bool depthTest = true)
		{
			float num = height / (float)quality;
			for (float num2 = num; num2 <= height; num2 += num)
			{
				Utils.DrawCircle(origin + direction * num2, direction, Mathf.Tan(angle * 0.017453292f) * num2, color, quality * 8, duration, depthTest);
			}
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000F8CB8 File Offset: 0x000F70B8
		public static bool IsCompressible(TextureFormat format)
		{
			return format >= (TextureFormat)0 && Array.IndexOf<TextureFormat>(Utils._incompressibleFormats, format) < 0;
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x000F8CD1 File Offset: 0x000F70D1
		public static float Area(this Rect rect)
		{
			return rect.width * rect.height;
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x000F8CE2 File Offset: 0x000F70E2
		public static Rect Extrude(this Rect r, float margin)
		{
			return new Rect(r.x - margin, r.y - margin, r.width + margin * 2f, r.height + margin * 2f);
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x000F8D19 File Offset: 0x000F7119
		public static Rect PadInner(this Rect r, float padding)
		{
			return r.PadInner(padding, padding, padding, padding);
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000F8D28 File Offset: 0x000F7128
		public static Rect PadInner(this Rect r, float padTop, float padBottom, float padLeft, float padRight)
		{
			float x = r.x + padLeft;
			float y = r.y + padBottom;
			float num = r.width - padRight - padLeft;
			float num2 = r.height - padTop - padBottom;
			if (num < 0f)
			{
				x = r.x + padLeft / (padLeft + padRight) * r.width;
				num = 0f;
			}
			if (num2 < 0f)
			{
				y = r.y + padBottom / (padBottom + padTop) * r.height;
				num2 = 0f;
			}
			return new Rect(x, y, num, num2);
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x000F8DBA File Offset: 0x000F71BA
		public static Rect PadTop(this Rect r, float padding)
		{
			return r.PadInner(padding, 0f, 0f, 0f);
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000F8DD2 File Offset: 0x000F71D2
		public static Rect PadBottom(this Rect r, float padding)
		{
			return r.PadInner(0f, padding, 0f, 0f);
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x000F8DEA File Offset: 0x000F71EA
		public static Rect PadLeft(this Rect r, float padding)
		{
			return r.PadInner(0f, 0f, padding, 0f);
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x000F8E02 File Offset: 0x000F7202
		public static Rect PadRight(this Rect r, float padding)
		{
			return r.PadInner(0f, 0f, 0f, padding);
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x000F8E1A File Offset: 0x000F721A
		public static Rect PadTop(this Rect r, float padding, out Rect marginRect)
		{
			marginRect = r.TakeTop(padding);
			return r.PadTop(padding);
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x000F8E30 File Offset: 0x000F7230
		public static Rect PadBottom(this Rect r, float padding, out Rect marginRect)
		{
			marginRect = r.TakeBottom(padding);
			return r.PadBottom(padding);
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x000F8E46 File Offset: 0x000F7246
		public static Rect PadLeft(this Rect r, float padding, out Rect marginRect)
		{
			marginRect = r.TakeLeft(padding);
			return r.PadLeft(padding);
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x000F8E5C File Offset: 0x000F725C
		public static Rect PadRight(this Rect r, float padding, out Rect marginRect)
		{
			marginRect = r.TakeRight(padding);
			return r.PadRight(padding);
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x000F8E74 File Offset: 0x000F7274
		public static Rect PadTopBottomPercent(this Rect r, float padPercent)
		{
			float num = r.height * padPercent;
			return r.PadInner(num, num, 0f, 0f);
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x000F8EA0 File Offset: 0x000F72A0
		public static Rect PadLeftRightPercent(this Rect r, float padPercent)
		{
			float num = r.width * padPercent;
			return r.PadInner(0f, 0f, num, num);
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x000F8ECC File Offset: 0x000F72CC
		public static Rect PadTopPercent(this Rect r, float padPercent)
		{
			float padding = r.height * padPercent;
			return r.PadTop(padding);
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x000F8EEC File Offset: 0x000F72EC
		public static Rect PadBottomPercent(this Rect r, float padPercent)
		{
			float padding = r.height * padPercent;
			return r.PadBottom(padding);
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x000F8F0A File Offset: 0x000F730A
		public static Rect PadLeftPercent(this Rect r, float padPercent)
		{
			return r.PadLeft(r.width * padPercent);
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x000F8F1B File Offset: 0x000F731B
		public static Rect PadRightPercent(this Rect r, float padPercent)
		{
			return r.PadRight(r.width * padPercent);
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x000F8F2C File Offset: 0x000F732C
		public static Rect TakeTop(this Rect r, float heightFromTop)
		{
			heightFromTop = Mathf.Clamp(heightFromTop, 0f, r.height);
			return new Rect(r.x, r.y + r.height - heightFromTop, r.width, heightFromTop);
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x000F8F67 File Offset: 0x000F7367
		public static Rect TakeBottom(this Rect r, float heightFromBottom)
		{
			heightFromBottom = Mathf.Clamp(heightFromBottom, 0f, r.height);
			return new Rect(r.x, r.y, r.width, heightFromBottom);
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x000F8F98 File Offset: 0x000F7398
		public static Rect TakeLeft(this Rect r, float widthFromLeft)
		{
			widthFromLeft = Mathf.Clamp(widthFromLeft, 0f, r.width);
			return new Rect(r.x, r.y, widthFromLeft, r.height);
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x000F8FC9 File Offset: 0x000F73C9
		public static Rect TakeRight(this Rect r, float widthFromRight)
		{
			widthFromRight = Mathf.Clamp(widthFromRight, 0f, r.width);
			return new Rect(r.x + r.width - widthFromRight, r.y, r.height, widthFromRight);
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x000F9004 File Offset: 0x000F7404
		public static Rect TakeTop(this Rect r, float padding, out Rect theRest)
		{
			theRest = r.PadTop(padding);
			return r.TakeTop(padding);
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x000F901A File Offset: 0x000F741A
		public static Rect TakeBottom(this Rect r, float padding, out Rect theRest)
		{
			theRest = r.PadBottom(padding);
			return r.TakeBottom(padding);
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x000F9030 File Offset: 0x000F7430
		public static Rect TakeLeft(this Rect r, float padding, out Rect theRest)
		{
			theRest = r.PadLeft(padding);
			return r.TakeLeft(padding);
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x000F9046 File Offset: 0x000F7446
		public static Rect TakeRight(this Rect r, float padding, out Rect theRest)
		{
			theRest = r.PadRight(padding);
			return r.TakeRight(padding);
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x000F905C File Offset: 0x000F745C
		public static Rect TakeHorizontal(this Rect r, float lineHeight, out Rect theRest, bool fromTop = true)
		{
			theRest = new Rect(r.x, (!fromTop) ? r.y : (r.y + lineHeight), r.width, r.height - lineHeight);
			return new Rect(r.x, (!fromTop) ? (r.y + r.height - lineHeight) : r.y, r.width, lineHeight);
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x000F90D8 File Offset: 0x000F74D8
		public static void SplitHorizontallyWithLeft(this Rect rect, out Rect left, out Rect right, float leftWidth)
		{
			left = rect;
			left.width = leftWidth;
			right = rect;
			right.x += left.width;
			right.width = rect.width - leftWidth;
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x000F9111 File Offset: 0x000F7511
		public static Utils.HorizontalLineRectEnumerator TakeAllLines(this Rect r, int numLines)
		{
			return new Utils.HorizontalLineRectEnumerator(r, numLines);
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x000F911A File Offset: 0x000F751A
		public static Pose From(this Vector3 position, Pose fromPose)
		{
			return new Pose(position, fromPose.rotation).From(fromPose);
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x000F912F File Offset: 0x000F752F
		public static Pose GetPose(this Rigidbody rigidbody)
		{
			return new Pose(rigidbody.position, rigidbody.rotation);
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x000F9144 File Offset: 0x000F7544
		public static Pose MirroredX(this Pose pose)
		{
			Vector3 position = pose.position;
			Quaternion rotation = pose.rotation;
			return new Pose(new Vector3(-position.x, position.y, position.z), new Quaternion(-rotation.x, rotation.y, rotation.z, -rotation.w).Flipped());
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x000F91AC File Offset: 0x000F75AC
		public static Pose Negated(this Pose pose)
		{
			Vector3 position = pose.position;
			Quaternion rotation = pose.rotation;
			return new Pose(new Vector3(-position.x, -position.y, -position.z), new Quaternion(-rotation.z, -rotation.y, -rotation.z, rotation.w));
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x000F920F File Offset: 0x000F760F
		public static float Map(this float value, float valueMin, float valueMax, float resultMin, float resultMax)
		{
			if (valueMin == valueMax)
			{
				return resultMin;
			}
			return Mathf.Lerp(resultMin, resultMax, (value - valueMin) / (valueMax - valueMin));
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x000F9229 File Offset: 0x000F7629
		public static float MapUnclamped(this float value, float valueMin, float valueMax, float resultMin, float resultMax)
		{
			if (valueMin == valueMax)
			{
				return resultMin;
			}
			return Mathf.LerpUnclamped(resultMin, resultMax, (value - valueMin) / (valueMax - valueMin));
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x000F9243 File Offset: 0x000F7643
		public static Vector2 Map(this Vector2 value, float valueMin, float valueMax, float resultMin, float resultMax)
		{
			return new Vector2(value.x.Map(valueMin, valueMax, resultMin, resultMax), value.y.Map(valueMin, valueMax, resultMin, resultMax));
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000F926C File Offset: 0x000F766C
		public static Vector2 MapUnclamped(this Vector2 value, float valueMin, float valueMax, float resultMin, float resultMax)
		{
			return new Vector2(value.x.MapUnclamped(valueMin, valueMax, resultMin, resultMax), value.y.MapUnclamped(valueMin, valueMax, resultMin, resultMax));
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000F9295 File Offset: 0x000F7695
		public static Vector3 Map(this Vector3 value, float valueMin, float valueMax, float resultMin, float resultMax)
		{
			return new Vector3(value.x.Map(valueMin, valueMax, resultMin, resultMax), value.y.Map(valueMin, valueMax, resultMin, resultMax), value.z.Map(valueMin, valueMax, resultMin, resultMax));
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x000F92CF File Offset: 0x000F76CF
		public static Vector3 MapUnclamped(this Vector3 value, float valueMin, float valueMax, float resultMin, float resultMax)
		{
			return new Vector3(value.x.MapUnclamped(valueMin, valueMax, resultMin, resultMax), value.y.MapUnclamped(valueMin, valueMax, resultMin, resultMax), value.z.MapUnclamped(valueMin, valueMax, resultMin, resultMax));
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x000F930C File Offset: 0x000F770C
		public static Vector4 Map(this Vector4 value, float valueMin, float valueMax, float resultMin, float resultMax)
		{
			return new Vector4(value.x.Map(valueMin, valueMax, resultMin, resultMax), value.y.Map(valueMin, valueMax, resultMin, resultMax), value.z.Map(valueMin, valueMax, resultMin, resultMax), value.w.Map(valueMin, valueMax, resultMin, resultMax));
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x000F9364 File Offset: 0x000F7764
		public static Vector4 MapUnclamped(this Vector4 value, float valueMin, float valueMax, float resultMin, float resultMax)
		{
			return new Vector4(value.x.MapUnclamped(valueMin, valueMax, resultMin, resultMax), value.y.MapUnclamped(valueMin, valueMax, resultMin, resultMax), value.z.MapUnclamped(valueMin, valueMax, resultMin, resultMax), value.w.MapUnclamped(valueMin, valueMax, resultMin, resultMax));
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x000F93BA File Offset: 0x000F77BA
		public static Vector2 Map(float input, float valueMin, float valueMax, Vector2 resultMin, Vector2 resultMax)
		{
			return Vector2.Lerp(resultMin, resultMax, Mathf.InverseLerp(valueMin, valueMax, input));
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x000F93CC File Offset: 0x000F77CC
		public static Vector3 Map(float input, float valueMin, float valueMax, Vector3 resultMin, Vector3 resultMax)
		{
			return Vector3.Lerp(resultMin, resultMax, Mathf.InverseLerp(valueMin, valueMax, input));
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x000F93DE File Offset: 0x000F77DE
		public static Vector4 Map(float input, float valueMin, float valueMax, Vector4 resultMin, Vector4 resultMax)
		{
			return Vector4.Lerp(resultMin, resultMax, Mathf.InverseLerp(valueMin, valueMax, input));
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x000F93F0 File Offset: 0x000F77F0
		public static Vector2 CompMul(this Vector2 A, Vector2 B)
		{
			return new Vector2(A.x * B.x, A.y * B.y);
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x000F9415 File Offset: 0x000F7815
		public static Vector3 CompMul(this Vector3 A, Vector3 B)
		{
			return new Vector3(A.x * B.x, A.y * B.y, A.z * B.z);
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x000F944C File Offset: 0x000F784C
		public static Vector4 CompMul(this Vector4 A, Vector4 B)
		{
			return new Vector4(A.x * B.x, A.y * B.y, A.z * B.z, A.w * B.w);
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x000F949A File Offset: 0x000F789A
		public static Vector2 CompDiv(this Vector2 A, Vector2 B)
		{
			return new Vector2(A.x / B.x, A.y / B.y);
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x000F94BF File Offset: 0x000F78BF
		public static Vector3 CompDiv(this Vector3 A, Vector3 B)
		{
			return new Vector3(A.x / B.x, A.y / B.y, A.z / B.z);
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x000F94F4 File Offset: 0x000F78F4
		public static Vector4 CompDiv(this Vector4 A, Vector4 B)
		{
			return new Vector4(A.x / B.x, A.y / B.y, A.z / B.z, A.w / B.w);
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x000F9542 File Offset: 0x000F7942
		public static float CompSum(this Vector2 v)
		{
			return v.x + v.y;
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x000F9553 File Offset: 0x000F7953
		public static float CompSum(this Vector3 v)
		{
			return v.x + v.y + v.z;
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x000F956C File Offset: 0x000F796C
		public static float CompSum(this Vector4 v)
		{
			return v.x + v.y + v.z + v.w;
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x000F958D File Offset: 0x000F798D
		public static float CompMax(this Vector2 v)
		{
			return Mathf.Max(v.x, v.y);
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x000F95A2 File Offset: 0x000F79A2
		public static float CompMax(this Vector3 v)
		{
			return Mathf.Max(Mathf.Max(v.x, v.y), v.z);
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x000F95C3 File Offset: 0x000F79C3
		public static float CompMax(this Vector4 v)
		{
			return Mathf.Max(Mathf.Max(Mathf.Max(v.x, v.y), v.z), v.w);
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x000F95F0 File Offset: 0x000F79F0
		public static float CompMin(this Vector2 v)
		{
			return Mathf.Min(v.x, v.y);
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x000F9605 File Offset: 0x000F7A05
		public static float CompMin(this Vector3 v)
		{
			return Mathf.Min(Mathf.Min(v.x, v.y), v.z);
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x000F9626 File Offset: 0x000F7A26
		public static float CompMin(this Vector4 v)
		{
			return Mathf.Min(Mathf.Min(Mathf.Min(v.x, v.y), v.z), v.w);
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x000F9653 File Offset: 0x000F7A53
		public static float From(this float thisFloat, float otherFloat)
		{
			return thisFloat - otherFloat;
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x000F9658 File Offset: 0x000F7A58
		public static float To(this float thisFloat, float otherFloat)
		{
			return otherFloat - thisFloat;
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x000F965D File Offset: 0x000F7A5D
		public static float Then(this float thisFloat, float otherFloat)
		{
			return thisFloat + otherFloat;
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x000F9662 File Offset: 0x000F7A62
		public static Vector3 From(this Vector3 thisVector, Vector3 otherVector)
		{
			return thisVector - otherVector;
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x000F966B File Offset: 0x000F7A6B
		public static Vector3 To(this Vector3 thisVector, Vector3 otherVector)
		{
			return otherVector - thisVector;
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x000F9674 File Offset: 0x000F7A74
		public static Vector3 Then(this Vector3 thisVector, Vector3 otherVector)
		{
			return thisVector + otherVector;
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x000F967D File Offset: 0x000F7A7D
		public static Quaternion From(this Quaternion thisQuaternion, Quaternion otherQuaternion)
		{
			return Quaternion.Inverse(otherQuaternion) * thisQuaternion;
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x000F968B File Offset: 0x000F7A8B
		public static Quaternion To(this Quaternion thisQuaternion, Quaternion otherQuaternion)
		{
			return Quaternion.Inverse(thisQuaternion) * otherQuaternion;
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x000F9699 File Offset: 0x000F7A99
		public static Quaternion Then(this Quaternion thisQuaternion, Quaternion otherQuaternion)
		{
			return thisQuaternion * otherQuaternion;
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x000F96A2 File Offset: 0x000F7AA2
		public static Pose From(this Pose thisPose, Pose otherPose)
		{
			return otherPose.inverse * thisPose;
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x000F96B1 File Offset: 0x000F7AB1
		public static Pose To(this Pose thisPose, Pose otherPose)
		{
			return thisPose.inverse * otherPose;
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x000F96C0 File Offset: 0x000F7AC0
		public static Pose Then(this Pose thisPose, Pose otherPose)
		{
			return thisPose * otherPose;
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x000F96C9 File Offset: 0x000F7AC9
		public static Matrix4x4 From(this Matrix4x4 thisMatrix, Matrix4x4 otherMatrix)
		{
			return thisMatrix * otherMatrix.inverse;
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x000F96D8 File Offset: 0x000F7AD8
		public static Matrix4x4 To(this Matrix4x4 thisMatrix, Matrix4x4 otherMatrix)
		{
			return otherMatrix * thisMatrix.inverse;
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x000F96E7 File Offset: 0x000F7AE7
		public static Matrix4x4 Then(this Matrix4x4 thisMatrix, Matrix4x4 otherMatrix)
		{
			return otherMatrix * thisMatrix;
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x000F96F0 File Offset: 0x000F7AF0
		// Note: this type is marked as 'beforefieldinit'.
		static Utils()
		{
			TextureFormat[] array = new TextureFormat[7];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.$field-64254BBA73F82A36110C152F592F58FE1CC4EC30).FieldHandle);
			Utils._incompressibleFormats = array;
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x000F9708 File Offset: 0x000F7B08
		[CompilerGenerated]
		private static bool <GenerateNiceName>m__0(char c)
		{
			return !char.IsDigit(c) && !char.IsLetter(c);
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x000F9723 File Offset: 0x000F7B23
		[CompilerGenerated]
		private static bool <FindObjectInHierarchy<T>(T o) where T : UnityEngine.Object
		{
			return true;
		}

		// Token: 0x0400240C RID: 9228
		private static TextureFormat[] _incompressibleFormats;

		// Token: 0x0400240D RID: 9229
		[CompilerGenerated]
		private static Func<char, bool> <>f__am$cache0;

		// Token: 0x0200074F RID: 1871
		public struct ChildrenEnumerator : IEnumerator<Transform>, IEnumerator, IDisposable
		{
			// Token: 0x0600301B RID: 12315 RVA: 0x000F9726 File Offset: 0x000F7B26
			public ChildrenEnumerator(Transform t)
			{
				this._t = t;
				this._idx = -1;
				this._count = t.childCount;
			}

			// Token: 0x0600301C RID: 12316 RVA: 0x000F9742 File Offset: 0x000F7B42
			public Utils.ChildrenEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x0600301D RID: 12317 RVA: 0x000F974A File Offset: 0x000F7B4A
			public bool MoveNext()
			{
				if (this._idx < this._count)
				{
					this._idx++;
				}
				return this._idx != this._count;
			}

			// Token: 0x170005C6 RID: 1478
			// (get) Token: 0x0600301E RID: 12318 RVA: 0x000F977F File Offset: 0x000F7B7F
			public Transform Current
			{
				get
				{
					return (!(this._t == null)) ? this._t.GetChild(this._idx) : null;
				}
			}

			// Token: 0x170005C5 RID: 1477
			// (get) Token: 0x0600301F RID: 12319 RVA: 0x000F97A9 File Offset: 0x000F7BA9
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06003020 RID: 12320 RVA: 0x000F97B1 File Offset: 0x000F7BB1
			public void Reset()
			{
				this._idx = -1;
				this._count = this._t.childCount;
			}

			// Token: 0x06003021 RID: 12321 RVA: 0x000F97CB File Offset: 0x000F7BCB
			public void Dispose()
			{
			}

			// Token: 0x0400240E RID: 9230
			private Transform _t;

			// Token: 0x0400240F RID: 9231
			private int _idx;

			// Token: 0x04002410 RID: 9232
			private int _count;
		}

		// Token: 0x02000750 RID: 1872
		public struct HorizontalLineRectEnumerator
		{
			// Token: 0x06003022 RID: 12322 RVA: 0x000F97CD File Offset: 0x000F7BCD
			public HorizontalLineRectEnumerator(Rect rect, int numLines)
			{
				this.rect = rect;
				this.numLines = numLines;
				this.index = -1;
			}

			// Token: 0x170005C7 RID: 1479
			// (get) Token: 0x06003023 RID: 12323 RVA: 0x000F97E4 File Offset: 0x000F7BE4
			public float eachHeight
			{
				get
				{
					return this.rect.height / (float)this.numLines;
				}
			}

			// Token: 0x170005C8 RID: 1480
			// (get) Token: 0x06003024 RID: 12324 RVA: 0x000F97F9 File Offset: 0x000F7BF9
			public Rect Current
			{
				get
				{
					return new Rect(this.rect.x, this.rect.y + this.eachHeight * (float)this.index, this.rect.width, this.eachHeight);
				}
			}

			// Token: 0x06003025 RID: 12325 RVA: 0x000F9836 File Offset: 0x000F7C36
			public bool MoveNext()
			{
				this.index++;
				return this.index < this.numLines;
			}

			// Token: 0x06003026 RID: 12326 RVA: 0x000F9854 File Offset: 0x000F7C54
			public Utils.HorizontalLineRectEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x06003027 RID: 12327 RVA: 0x000F985C File Offset: 0x000F7C5C
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x06003028 RID: 12328 RVA: 0x000F9868 File Offset: 0x000F7C68
			public Query<Rect> Query()
			{
				List<Rect> list = Pool<List<Rect>>.Spawn();
				Query<Rect> result;
				try
				{
					foreach (Rect item in this)
					{
						list.Add(item);
					}
					result = new Query<Rect>(list);
				}
				finally
				{
					list.Clear();
					Pool<List<Rect>>.Recycle(list);
				}
				return result;
			}

			// Token: 0x04002411 RID: 9233
			private Rect rect;

			// Token: 0x04002412 RID: 9234
			private int numLines;

			// Token: 0x04002413 RID: 9235
			private int index;
		}

		// Token: 0x02000FAF RID: 4015
		[CompilerGenerated]
		private sealed class <GetSortedOrder>c__AnonStorey0<T> where T : IComparable<T>
		{
			// Token: 0x060074CC RID: 29900 RVA: 0x000F98CC File Offset: 0x000F7CCC
			public <GetSortedOrder>c__AnonStorey0()
			{
			}

			// Token: 0x060074CD RID: 29901 RVA: 0x000F98D4 File Offset: 0x000F7CD4
			internal int <>m__0(int a, int b)
			{
				T t = this.list[a];
				return t.CompareTo(this.list[b]);
			}

			// Token: 0x040068DD RID: 26845
			internal IList<T> list;
		}

		// Token: 0x02000FB0 RID: 4016
		[CompilerGenerated]
		private sealed class <GenerateNiceName>c__AnonStorey1
		{
			// Token: 0x060074CE RID: 29902 RVA: 0x000F9907 File Offset: 0x000F7D07
			public <GenerateNiceName>c__AnonStorey1()
			{
			}

			// Token: 0x060074CF RID: 29903 RVA: 0x000F9910 File Offset: 0x000F7D10
			internal bool <>m__0(char c)
			{
				if (this.curr.Length > 0 && char.IsUpper(this.curr[0]))
				{
					return false;
				}
				if (!char.IsLetter(c))
				{
					return false;
				}
				this.curr = c + this.curr;
				return true;
			}

			// Token: 0x060074D0 RID: 29904 RVA: 0x000F996B File Offset: 0x000F7D6B
			internal bool <>m__1(char c)
			{
				if (!char.IsLetter(c))
				{
					return false;
				}
				if (char.IsLower(c))
				{
					return false;
				}
				this.curr = c + this.curr;
				return true;
			}

			// Token: 0x060074D1 RID: 29905 RVA: 0x000F999F File Offset: 0x000F7D9F
			internal bool <>m__2(char c)
			{
				if (!char.IsDigit(c))
				{
					return false;
				}
				this.curr = c + this.curr;
				return true;
			}

			// Token: 0x040068DE RID: 26846
			internal string curr;
		}

		// Token: 0x02000FB1 RID: 4017
		[CompilerGenerated]
		private sealed class <NextTuple>c__AnonStorey2
		{
			// Token: 0x060074D2 RID: 29906 RVA: 0x000F99C6 File Offset: 0x000F7DC6
			public <NextTuple>c__AnonStorey2()
			{
			}

			// Token: 0x060074D3 RID: 29907 RVA: 0x000F99CE File Offset: 0x000F7DCE
			internal int <>m__0(int i)
			{
				return (i + 1) % this.maxValue;
			}

			// Token: 0x040068DF RID: 26847
			internal int maxValue;
		}
	}
}
