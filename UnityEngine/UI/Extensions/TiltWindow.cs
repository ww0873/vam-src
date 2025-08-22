using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004BD RID: 1213
	public class TiltWindow : MonoBehaviour
	{
		// Token: 0x06001EA0 RID: 7840 RVA: 0x000AE40C File Offset: 0x000AC80C
		public TiltWindow()
		{
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000AE434 File Offset: 0x000AC834
		private void Start()
		{
			this.mTrans = base.transform;
			this.mStart = this.mTrans.localRotation;
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000AE454 File Offset: 0x000AC854
		private void Update()
		{
			Vector3 mousePosition = Input.mousePosition;
			float num = (float)Screen.width * 0.5f;
			float num2 = (float)Screen.height * 0.5f;
			float x = Mathf.Clamp((mousePosition.x - num) / num, -1f, 1f);
			float y = Mathf.Clamp((mousePosition.y - num2) / num2, -1f, 1f);
			this.mRot = Vector2.Lerp(this.mRot, new Vector2(x, y), Time.deltaTime * 5f);
			this.mTrans.localRotation = this.mStart * Quaternion.Euler(-this.mRot.y * this.range.y, this.mRot.x * this.range.x, 0f);
		}

		// Token: 0x040019BE RID: 6590
		public Vector2 range = new Vector2(5f, 3f);

		// Token: 0x040019BF RID: 6591
		private Transform mTrans;

		// Token: 0x040019C0 RID: 6592
		private Quaternion mStart;

		// Token: 0x040019C1 RID: 6593
		private Vector2 mRot = Vector2.zero;
	}
}
