using System;

namespace Leap.Unity
{
	// Token: 0x0200069B RID: 1691
	public class BoxedIndexableStruct<Element, IndexableStruct> : IIndexable<Element>, IPoolable where IndexableStruct : struct, IIndexableStruct<Element, IndexableStruct>
	{
		// Token: 0x060028CC RID: 10444 RVA: 0x000DFBD2 File Offset: 0x000DDFD2
		public BoxedIndexableStruct()
		{
		}

		// Token: 0x17000517 RID: 1303
		public Element this[int idx]
		{
			get
			{
				if (this.maybeIndexableStruct == null)
				{
					throw new NullReferenceException("PooledIndexableStructWrapper failed to index missing " + typeof(IndexableStruct).Name + "; did you assign its maybeIndexableStruct field?");
				}
				IndexableStruct value = this.maybeIndexableStruct.Value;
				return value[idx];
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x060028CE RID: 10446 RVA: 0x000DFC38 File Offset: 0x000DE038
		public int Count
		{
			get
			{
				if (this.maybeIndexableStruct == null)
				{
					return 0;
				}
				IndexableStruct value = this.maybeIndexableStruct.Value;
				return value.Count;
			}
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x000DFC70 File Offset: 0x000DE070
		public void OnSpawn()
		{
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x000DFC74 File Offset: 0x000DE074
		public void OnRecycle()
		{
			this.maybeIndexableStruct = null;
		}

		// Token: 0x040021CE RID: 8654
		public IndexableStruct? maybeIndexableStruct;
	}
}
