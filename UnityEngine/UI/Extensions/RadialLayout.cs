using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000512 RID: 1298
	[AddComponentMenu("Layout/Extensions/Radial Layout")]
	public class RadialLayout : LayoutGroup
	{
		// Token: 0x060020C7 RID: 8391 RVA: 0x000BC5CB File Offset: 0x000BA9CB
		public RadialLayout()
		{
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000BC5D3 File Offset: 0x000BA9D3
		protected override void OnEnable()
		{
			base.OnEnable();
			this.CalculateRadial();
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x000BC5E1 File Offset: 0x000BA9E1
		public override void SetLayoutHorizontal()
		{
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x000BC5E3 File Offset: 0x000BA9E3
		public override void SetLayoutVertical()
		{
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x000BC5E5 File Offset: 0x000BA9E5
		public override void CalculateLayoutInputVertical()
		{
			this.CalculateRadial();
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x000BC5ED File Offset: 0x000BA9ED
		public override void CalculateLayoutInputHorizontal()
		{
			this.CalculateRadial();
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x000BC5F8 File Offset: 0x000BA9F8
		private void CalculateRadial()
		{
			this.m_Tracker.Clear();
			if (base.transform.childCount == 0)
			{
				return;
			}
			float num = (this.MaxAngle - this.MinAngle) / (float)base.transform.childCount;
			float num2 = this.StartAngle;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
				if (rectTransform != null)
				{
					this.m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.PivotX | DrivenTransformProperties.PivotY);
					Vector3 a = new Vector3(Mathf.Cos(num2 * 0.017453292f), Mathf.Sin(num2 * 0.017453292f), 0f);
					rectTransform.localPosition = a * this.fDistance;
					RectTransform rectTransform2 = rectTransform;
					Vector2 vector = new Vector2(0.5f, 0.5f);
					rectTransform.pivot = vector;
					vector = vector;
					rectTransform.anchorMax = vector;
					rectTransform2.anchorMin = vector;
					num2 += num;
				}
			}
		}

		// Token: 0x04001B58 RID: 7000
		public float fDistance;

		// Token: 0x04001B59 RID: 7001
		[Range(0f, 360f)]
		public float MinAngle;

		// Token: 0x04001B5A RID: 7002
		[Range(0f, 360f)]
		public float MaxAngle;

		// Token: 0x04001B5B RID: 7003
		[Range(0f, 360f)]
		public float StartAngle;
	}
}
