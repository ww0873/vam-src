using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006C5 RID: 1733
	public class SerializableHashSet<T> : SerializableHashSetBase, ICanReportDuplicateInformation, ISerializationCallbackReceiver, IEnumerable<T>, IEnumerable
	{
		// Token: 0x060029BC RID: 10684 RVA: 0x000E211E File Offset: 0x000E051E
		public SerializableHashSet()
		{
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x060029BD RID: 10685 RVA: 0x000E213C File Offset: 0x000E053C
		public int Count
		{
			get
			{
				return this._set.Count;
			}
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x000E2149 File Offset: 0x000E0549
		public bool Add(T item)
		{
			return this._set.Add(item);
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x000E2157 File Offset: 0x000E0557
		public void Clear()
		{
			this._set.Clear();
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x000E2164 File Offset: 0x000E0564
		public bool Contains(T item)
		{
			return this._set.Contains(item);
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x000E2172 File Offset: 0x000E0572
		public bool Remove(T item)
		{
			return this._set.Remove(item);
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x000E2180 File Offset: 0x000E0580
		public static implicit operator HashSet<T>(SerializableHashSet<T> serializableHashSet)
		{
			return serializableHashSet._set;
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x000E2188 File Offset: 0x000E0588
		public IEnumerator<T> GetEnumerator()
		{
			return this._set.GetEnumerator();
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x000E219A File Offset: 0x000E059A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._set.GetEnumerator();
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x000E21AC File Offset: 0x000E05AC
		public void ClearDuplicates()
		{
			HashSet<T> hashSet = new HashSet<T>();
			int count = this._values.Count;
			while (count-- != 0)
			{
				T item = this._values[count];
				if (hashSet.Contains(item))
				{
					this._values.RemoveAt(count);
				}
				else
				{
					hashSet.Add(item);
				}
			}
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x000E220C File Offset: 0x000E060C
		public List<int> GetDuplicationInformation()
		{
			Dictionary<T, int> dictionary = new Dictionary<T, int>();
			foreach (T t in this._values)
			{
				if (t != null)
				{
					if (dictionary.ContainsKey(t))
					{
						Dictionary<T, int> dictionary2;
						T key;
						(dictionary2 = dictionary)[key = t] = dictionary2[key] + 1;
					}
					else
					{
						dictionary[t] = 1;
					}
				}
			}
			List<int> list = new List<int>();
			foreach (T t2 in this._values)
			{
				if (t2 != null)
				{
					list.Add(dictionary[t2]);
				}
			}
			return list;
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x000E2318 File Offset: 0x000E0718
		public void OnAfterDeserialize()
		{
			this._set.Clear();
			if (this._values != null)
			{
				foreach (T t in this._values)
				{
					if (t != null)
					{
						this._set.Add(t);
					}
				}
			}
			this._values.Clear();
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x000E23A8 File Offset: 0x000E07A8
		public void OnBeforeSerialize()
		{
			if (this._values == null)
			{
				this._values = new List<T>();
			}
			this._values.Clear();
			this._values.AddRange(this);
		}

		// Token: 0x060029C9 RID: 10697 RVA: 0x000E23D7 File Offset: 0x000E07D7
		private bool isNull(object obj)
		{
			return obj == null || (obj is UnityEngine.Object && obj as UnityEngine.Object == null);
		}

		// Token: 0x04002200 RID: 8704
		[SerializeField]
		private List<T> _values = new List<T>();

		// Token: 0x04002201 RID: 8705
		[NonSerialized]
		private HashSet<T> _set = new HashSet<T>();
	}
}
