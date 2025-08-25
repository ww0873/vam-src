using System;
using System.Text;

namespace UnityEngine.EventSystems
{
	// Token: 0x0200096C RID: 2412
	public class OVRPointerEventData : PointerEventData
	{
		// Token: 0x06003C4D RID: 15437 RVA: 0x00124275 File Offset: 0x00122675
		public OVRPointerEventData(EventSystem eventSystem) : base(eventSystem)
		{
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x00124280 File Offset: 0x00122680
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<b>Position</b>: " + base.position);
			stringBuilder.AppendLine("<b>delta</b>: " + base.delta);
			stringBuilder.AppendLine("<b>eligibleForClick</b>: " + base.eligibleForClick);
			stringBuilder.AppendLine("<b>pointerEnter</b>: " + base.pointerEnter);
			stringBuilder.AppendLine("<b>pointerPress</b>: " + base.pointerPress);
			stringBuilder.AppendLine("<b>lastPointerPress</b>: " + base.lastPress);
			stringBuilder.AppendLine("<b>pointerDrag</b>: " + base.pointerDrag);
			stringBuilder.AppendLine("<b>worldSpaceRay</b>: " + this.worldSpaceRay);
			stringBuilder.AppendLine("<b>swipeStart</b>: " + this.swipeStart);
			stringBuilder.AppendLine("<b>Use Drag Threshold</b>: " + base.useDragThreshold);
			return stringBuilder.ToString();
		}

		// Token: 0x04002E3A RID: 11834
		public Ray worldSpaceRay;

		// Token: 0x04002E3B RID: 11835
		public Vector2 swipeStart;
	}
}
