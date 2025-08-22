using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200053B RID: 1339
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/Tooltip")]
	public class ToolTip : MonoBehaviour
	{
		// Token: 0x06002216 RID: 8726 RVA: 0x000C3239 File Offset: 0x000C1639
		public ToolTip()
		{
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x000C3244 File Offset: 0x000C1644
		public void Awake()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			this._guiCamera = componentInParent.worldCamera;
			this._guiMode = componentInParent.renderMode;
			this._rectTransform = base.GetComponent<RectTransform>();
			this._text = base.GetComponentInChildren<Text>();
			this._inside = false;
			this.xShift = 0f;
			this.YShift = -30f;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x000C32B4 File Offset: 0x000C16B4
		public void SetTooltip(string ttext)
		{
			if (this._guiMode == RenderMode.ScreenSpaceCamera)
			{
				this._text.text = ttext;
				this._rectTransform.sizeDelta = new Vector2(this._text.preferredWidth + 40f, this._text.preferredHeight + 25f);
				this.OnScreenSpaceCamera();
			}
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x000C3311 File Offset: 0x000C1711
		public void HideTooltip()
		{
			if (this._guiMode == RenderMode.ScreenSpaceCamera)
			{
				base.gameObject.SetActive(false);
				this._inside = false;
			}
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x000C3332 File Offset: 0x000C1732
		private void FixedUpdate()
		{
			if (this._inside && this._guiMode == RenderMode.ScreenSpaceCamera)
			{
				this.OnScreenSpaceCamera();
			}
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x000C3354 File Offset: 0x000C1754
		public void OnScreenSpaceCamera()
		{
			Vector3 position = this._guiCamera.ScreenToViewportPoint(Input.mousePosition - new Vector3(this.xShift, this.YShift, 0f));
			Vector3 vector = this._guiCamera.ViewportToWorldPoint(position);
			this.width = this._rectTransform.sizeDelta[0];
			this.height = this._rectTransform.sizeDelta[1];
			Vector3 vector2 = this._guiCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
			Vector3 vector3 = this._guiCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
			float num = vector.x + this.width / 2f;
			if (num > vector3.x)
			{
				Vector3 vector4 = new Vector3(num - vector3.x, 0f, 0f);
				Vector3 position2 = new Vector3(vector.x - vector4.x, position.y, 0f);
				position.x = this._guiCamera.WorldToViewportPoint(position2).x;
			}
			num = vector.x - this.width / 2f;
			if (num < vector2.x)
			{
				Vector3 vector5 = new Vector3(vector2.x - num, 0f, 0f);
				Vector3 position3 = new Vector3(vector.x + vector5.x, position.y, 0f);
				position.x = this._guiCamera.WorldToViewportPoint(position3).x;
			}
			num = vector.y + this.height / 2f;
			if (num > vector3.y)
			{
				Vector3 vector6 = new Vector3(0f, 35f + this.height / 2f, 0f);
				Vector3 position4 = new Vector3(position.x, vector.y - vector6.y, 0f);
				position.y = this._guiCamera.WorldToViewportPoint(position4).y;
			}
			num = vector.y - this.height / 2f;
			if (num < vector2.y)
			{
				Vector3 vector7 = new Vector3(0f, 35f + this.height / 2f, 0f);
				Vector3 position5 = new Vector3(position.x, vector.y + vector7.y, 0f);
				position.y = this._guiCamera.WorldToViewportPoint(position5).y;
			}
			base.transform.position = new Vector3(vector.x, vector.y, 0f);
			base.gameObject.SetActive(true);
			this._inside = true;
		}

		// Token: 0x04001C5C RID: 7260
		private Text _text;

		// Token: 0x04001C5D RID: 7261
		private RectTransform _rectTransform;

		// Token: 0x04001C5E RID: 7262
		private bool _inside;

		// Token: 0x04001C5F RID: 7263
		private float width;

		// Token: 0x04001C60 RID: 7264
		private float height;

		// Token: 0x04001C61 RID: 7265
		private float YShift;

		// Token: 0x04001C62 RID: 7266
		private float xShift;

		// Token: 0x04001C63 RID: 7267
		private RenderMode _guiMode;

		// Token: 0x04001C64 RID: 7268
		private Camera _guiCamera;
	}
}
