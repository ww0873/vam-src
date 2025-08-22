using System;

namespace Leap.Unity
{
	// Token: 0x02000699 RID: 1689
	public struct IndexableEnumerator<Element>
	{
		// Token: 0x060028C5 RID: 10437 RVA: 0x000DFB6E File Offset: 0x000DDF6E
		public IndexableEnumerator(IIndexable<Element> indexable)
		{
			this.indexable = indexable;
			this.index = -1;
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x000DFB7E File Offset: 0x000DDF7E
		public IndexableEnumerator<Element> GetEnumerator()
		{
			return this;
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x000DFB86 File Offset: 0x000DDF86
		public bool MoveNext()
		{
			if (this.indexable == null)
			{
				return false;
			}
			this.index++;
			return this.index < this.indexable.Count;
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x000DFBB6 File Offset: 0x000DDFB6
		public void Reset()
		{
			this.index = -1;
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060028C9 RID: 10441 RVA: 0x000DFBBF File Offset: 0x000DDFBF
		public Element Current
		{
			get
			{
				return this.indexable[this.index];
			}
		}

		// Token: 0x040021CC RID: 8652
		private IIndexable<Element> indexable;

		// Token: 0x040021CD RID: 8653
		private int index;
	}
}
