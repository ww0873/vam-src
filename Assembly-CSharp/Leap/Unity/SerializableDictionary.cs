using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006C2 RID: 1730
	public class SerializableDictionary<TKey, TValue> : SerializableDictionaryBase, IEnumerable<KeyValuePair<TKey, TValue>>, ICanReportDuplicateInformation, ISerializationCallbackReceiver, ISerializableDictionary, IEnumerable
	{
		// Token: 0x060029A7 RID: 10663 RVA: 0x000E1E12 File Offset: 0x000E0212
		public SerializableDictionary()
		{
		}

		// Token: 0x17000534 RID: 1332
		public TValue this[TKey key]
		{
			get
			{
				return this._dictionary[key];
			}
			set
			{
				this._dictionary[key] = value;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x000E1E58 File Offset: 0x000E0258
		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				return this._dictionary.Keys;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x060029AB RID: 10667 RVA: 0x000E1E65 File Offset: 0x000E0265
		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				return this._dictionary.Values;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x060029AC RID: 10668 RVA: 0x000E1E72 File Offset: 0x000E0272
		public int Count
		{
			get
			{
				return this._dictionary.Count;
			}
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x000E1E7F File Offset: 0x000E027F
		public void Add(TKey key, TValue value)
		{
			this._dictionary.Add(key, value);
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x000E1E8E File Offset: 0x000E028E
		public void Clear()
		{
			this._dictionary.Clear();
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x000E1E9B File Offset: 0x000E029B
		public bool ContainsKey(TKey key)
		{
			return this._dictionary.ContainsKey(key);
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x000E1EA9 File Offset: 0x000E02A9
		public bool ContainsValue(TValue value)
		{
			return this._dictionary.ContainsValue(value);
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x000E1EB7 File Offset: 0x000E02B7
		public bool Remove(TKey key)
		{
			return this._dictionary.Remove(key);
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x000E1EC5 File Offset: 0x000E02C5
		public bool TryGetValue(TKey key, out TValue value)
		{
			return this._dictionary.TryGetValue(key, out value);
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x000E1ED4 File Offset: 0x000E02D4
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this._dictionary.GetEnumerator();
		}

		// Token: 0x060029B4 RID: 10676 RVA: 0x000E1EE6 File Offset: 0x000E02E6
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._dictionary.GetEnumerator();
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x000E1EF8 File Offset: 0x000E02F8
		public static implicit operator Dictionary<TKey, TValue>(SerializableDictionary<TKey, TValue> serializableDictionary)
		{
			return serializableDictionary._dictionary;
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x000E1F00 File Offset: 0x000E0300
		public virtual float KeyDisplayRatio()
		{
			return 0.5f;
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x000E1F08 File Offset: 0x000E0308
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<TKey> list = this._dictionary.Keys.ToList<TKey>();
			List<TValue> list2 = this._dictionary.Values.ToList<TValue>();
			stringBuilder.Append("[");
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.Append("{");
				StringBuilder stringBuilder2 = stringBuilder;
				TKey tkey = list[i];
				stringBuilder2.Append(tkey.ToString());
				stringBuilder.Append(" : ");
				StringBuilder stringBuilder3 = stringBuilder;
				TValue tvalue = list2[i];
				stringBuilder3.Append(tvalue.ToString());
				stringBuilder.Append("}, \n");
			}
			stringBuilder.Remove(stringBuilder.Length - 3, 3);
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x000E1FE0 File Offset: 0x000E03E0
		public void OnAfterDeserialize()
		{
			this._dictionary.Clear();
			if (this._keys != null && this._values != null)
			{
				int num = Mathf.Min(this._keys.Count, this._values.Count);
				for (int i = 0; i < num; i++)
				{
					TKey tkey = this._keys[i];
					TValue value = this._values[i];
					if (tkey != null)
					{
						this._dictionary[tkey] = value;
					}
				}
			}
			this._keys.Clear();
			this._values.Clear();
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x000E208C File Offset: 0x000E048C
		public void OnBeforeSerialize()
		{
			if (this._keys == null)
			{
				this._keys = new List<TKey>();
			}
			if (this._values == null)
			{
				this._values = new List<TValue>();
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this._dictionary)
			{
				this._keys.Add(keyValuePair.Key);
				this._values.Add(keyValuePair.Value);
			}
		}

		// Token: 0x040021FD RID: 8701
		[SerializeField]
		private List<TKey> _keys = new List<TKey>();

		// Token: 0x040021FE RID: 8702
		[SerializeField]
		private List<TValue> _values = new List<TValue>();

		// Token: 0x040021FF RID: 8703
		[NonSerialized]
		private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
	}
}
