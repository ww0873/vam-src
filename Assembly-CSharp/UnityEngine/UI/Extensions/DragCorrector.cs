using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000541 RID: 1345
	[RequireComponent(typeof(EventSystem))]
	[AddComponentMenu("UI/Extensions/DragCorrector")]
	public class DragCorrector : MonoBehaviour
	{
		// Token: 0x06002253 RID: 8787 RVA: 0x000C4BDB File Offset: 0x000C2FDB
		public DragCorrector()
		{
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000C4BF8 File Offset: 0x000C2FF8
		private void Start()
		{
			this.dragTH = this.baseTH * (int)Screen.dpi / this.basePPI;
			EventSystem component = base.GetComponent<EventSystem>();
			if (component)
			{
				component.pixelDragThreshold = this.dragTH;
			}
		}

		// Token: 0x04001C81 RID: 7297
		public int baseTH = 6;

		// Token: 0x04001C82 RID: 7298
		public int basePPI = 210;

		// Token: 0x04001C83 RID: 7299
		public int dragTH;
	}
}
