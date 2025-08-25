using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004DD RID: 1245
	public interface IBoxSelectable
	{
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001F77 RID: 8055
		// (set) Token: 0x06001F78 RID: 8056
		bool selected { get; set; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001F79 RID: 8057
		// (set) Token: 0x06001F7A RID: 8058
		bool preSelected { get; set; }

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001F7B RID: 8059
		Transform transform { get; }
	}
}
