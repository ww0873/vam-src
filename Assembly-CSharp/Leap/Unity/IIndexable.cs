using System;

namespace Leap.Unity
{
	// Token: 0x02000697 RID: 1687
	public interface IIndexable<T>
	{
		// Token: 0x17000512 RID: 1298
		T this[int idx]
		{
			get;
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x060028C2 RID: 10434
		int Count { get; }
	}
}
