using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools
{
	// Token: 0x020009D6 RID: 2518
	[Serializable]
	public class SerializableDictionary<TK, TV>
	{
		// Token: 0x06003F85 RID: 16261 RVA: 0x0012F240 File Offset: 0x0012D640
		public SerializableDictionary()
		{
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x0012F25E File Offset: 0x0012D65E
		public void Add(TK key, TV value)
		{
			if (this.keys.Contains(key))
			{
				throw new Exception("Key already added");
			}
			this.keys.Add(key);
			this.values.Add(value);
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x0012F294 File Offset: 0x0012D694
		public TV GetValue(TK key)
		{
			if (!this.keys.Contains(key))
			{
				throw new Exception("Can't found key");
			}
			return this.values[this.keys.IndexOf(key)];
		}

		// Token: 0x04003016 RID: 12310
		[SerializeField]
		private List<TK> keys = new List<TK>();

		// Token: 0x04003017 RID: 12311
		[SerializeField]
		private List<TV> values = new List<TV>();
	}
}
