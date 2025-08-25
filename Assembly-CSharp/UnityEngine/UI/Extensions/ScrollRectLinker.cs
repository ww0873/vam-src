using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000552 RID: 1362
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/ScrollRectLinker")]
	public class ScrollRectLinker : MonoBehaviour
	{
		// Token: 0x060022A5 RID: 8869 RVA: 0x000C5A90 File Offset: 0x000C3E90
		public ScrollRectLinker()
		{
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x000C5A9F File Offset: 0x000C3E9F
		private void Awake()
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
			if (this.controllingScrollRect != null)
			{
				this.controllingScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.MirrorPos));
			}
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x000C5ADC File Offset: 0x000C3EDC
		private void MirrorPos(Vector2 scrollPos)
		{
			if (this.clamp)
			{
				this.scrollRect.normalizedPosition = new Vector2(Mathf.Clamp01(scrollPos.x), Mathf.Clamp01(scrollPos.y));
			}
			else
			{
				this.scrollRect.normalizedPosition = scrollPos;
			}
		}

		// Token: 0x04001CA4 RID: 7332
		public bool clamp = true;

		// Token: 0x04001CA5 RID: 7333
		[SerializeField]
		private ScrollRect controllingScrollRect;

		// Token: 0x04001CA6 RID: 7334
		private ScrollRect scrollRect;
	}
}
