using System;
using System.Collections;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x02000849 RID: 2121
	public class DeserializableList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x060036D8 RID: 14040 RVA: 0x0010C047 File Offset: 0x0010A447
		public DeserializableList()
		{
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060036D9 RID: 14041 RVA: 0x0010C04F File Offset: 0x0010A44F
		public int Count
		{
			get
			{
				return this._Data.Count;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060036DA RID: 14042 RVA: 0x0010C05C File Offset: 0x0010A45C
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return ((ICollection<T>)this._Data).IsReadOnly;
			}
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x0010C069 File Offset: 0x0010A469
		public int IndexOf(T obj)
		{
			return this._Data.IndexOf(obj);
		}

		// Token: 0x17000602 RID: 1538
		public T this[int index]
		{
			get
			{
				return this._Data[index];
			}
			set
			{
				this._Data[index] = value;
			}
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x0010C094 File Offset: 0x0010A494
		public void Add(T item)
		{
			this._Data.Add(item);
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x0010C0A2 File Offset: 0x0010A4A2
		public void Clear()
		{
			this._Data.Clear();
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x0010C0AF File Offset: 0x0010A4AF
		public bool Contains(T item)
		{
			return this._Data.Contains(item);
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x0010C0BD File Offset: 0x0010A4BD
		public void CopyTo(T[] array, int arrayIndex)
		{
			this._Data.CopyTo(array, arrayIndex);
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x0010C0CC File Offset: 0x0010A4CC
		public IEnumerator<T> GetEnumerator()
		{
			return this._Data.GetEnumerator();
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x0010C0DE File Offset: 0x0010A4DE
		public void Insert(int index, T item)
		{
			this._Data.Insert(index, item);
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x0010C0ED File Offset: 0x0010A4ED
		public bool Remove(T item)
		{
			return this._Data.Remove(item);
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x0010C0FB File Offset: 0x0010A4FB
		public void RemoveAt(int index)
		{
			this._Data.RemoveAt(index);
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x0010C109 File Offset: 0x0010A509
		private IEnumerator GetEnumerator1()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x0010C111 File Offset: 0x0010A511
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator1();
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060036E8 RID: 14056 RVA: 0x0010C119 File Offset: 0x0010A519
		[Obsolete("Use IList interface on the DeserializableList object instead.", false)]
		public List<T> Data
		{
			get
			{
				return this._Data;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060036E9 RID: 14057 RVA: 0x0010C121 File Offset: 0x0010A521
		public bool HasNextPage
		{
			get
			{
				return !string.IsNullOrEmpty(this.NextUrl);
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060036EA RID: 14058 RVA: 0x0010C131 File Offset: 0x0010A531
		public bool HasPreviousPage
		{
			get
			{
				return !string.IsNullOrEmpty(this.PreviousUrl);
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x060036EB RID: 14059 RVA: 0x0010C141 File Offset: 0x0010A541
		public string NextUrl
		{
			get
			{
				return this._NextUrl;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x060036EC RID: 14060 RVA: 0x0010C149 File Offset: 0x0010A549
		public string PreviousUrl
		{
			get
			{
				return this._PreviousUrl;
			}
		}

		// Token: 0x0400280E RID: 10254
		protected List<T> _Data;

		// Token: 0x0400280F RID: 10255
		protected string _NextUrl;

		// Token: 0x04002810 RID: 10256
		protected string _PreviousUrl;
	}
}
