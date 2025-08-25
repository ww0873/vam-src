using System;

namespace Leap.Unity
{
	// Token: 0x0200069C RID: 1692
	public static class BoxedIndexableStructExtensions
	{
		// Token: 0x060028D1 RID: 10449 RVA: 0x000DFC90 File Offset: 0x000DE090
		public static void Recycle<Element, IndexableStruct>(this BoxedIndexableStruct<Element, IndexableStruct> pooledWrapper) where IndexableStruct : struct, IIndexableStruct<Element, IndexableStruct>
		{
			Pool<BoxedIndexableStruct<Element, IndexableStruct>>.Recycle(pooledWrapper);
		}
	}
}
