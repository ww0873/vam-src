using System;

namespace Leap.Unity
{
	// Token: 0x0200069A RID: 1690
	public interface IIndexableStruct<T, ThisIndexableType> where ThisIndexableType : struct, IIndexableStruct<T, ThisIndexableType>
	{
		// Token: 0x17000515 RID: 1301
		T this[int idx]
		{
			get;
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060028CB RID: 10443
		int Count { get; }
	}
}
