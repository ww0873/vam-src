using System;

namespace Leap.Unity
{
	// Token: 0x0200069D RID: 1693
	public struct IndexableStructEnumerator<Element, IndexableStruct> where IndexableStruct : struct, IIndexableStruct<Element, IndexableStruct>
	{
		// Token: 0x060028D2 RID: 10450 RVA: 0x000DFC98 File Offset: 0x000DE098
		public IndexableStructEnumerator(IndexableStruct indexable)
		{
			this.maybeIndexable = new IndexableStruct?(indexable);
			this.index = -1;
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x000DFCAD File Offset: 0x000DE0AD
		public IndexableStructEnumerator<Element, IndexableStruct> GetEnumerator()
		{
			return this;
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x000DFCB8 File Offset: 0x000DE0B8
		public bool MoveNext()
		{
			if (this.maybeIndexable == null)
			{
				return false;
			}
			this.index++;
			int num = this.index;
			IndexableStruct value = this.maybeIndexable.Value;
			return num < value.Count;
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000DFD06 File Offset: 0x000DE106
		public void Reset()
		{
			this.index = -1;
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x060028D6 RID: 10454 RVA: 0x000DFD10 File Offset: 0x000DE110
		public Element Current
		{
			get
			{
				IndexableStruct value = this.maybeIndexable.Value;
				return value[this.index];
			}
		}

		// Token: 0x040021CF RID: 8655
		private IndexableStruct? maybeIndexable;

		// Token: 0x040021D0 RID: 8656
		private int index;
	}
}
