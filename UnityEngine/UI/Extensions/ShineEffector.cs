using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000503 RID: 1283
	[ExecuteInEditMode]
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Effects/Extensions/Shining Effect")]
	public class ShineEffector : MonoBehaviour
	{
		// Token: 0x06002059 RID: 8281 RVA: 0x000B9037 File Offset: 0x000B7437
		public ShineEffector()
		{
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x0600205A RID: 8282 RVA: 0x000B9055 File Offset: 0x000B7455
		// (set) Token: 0x0600205B RID: 8283 RVA: 0x000B905D File Offset: 0x000B745D
		public float YOffset
		{
			get
			{
				return this.yOffset;
			}
			set
			{
				this.ChangeVal(value);
				this.yOffset = value;
			}
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x000B9070 File Offset: 0x000B7470
		private void OnEnable()
		{
			if (this.effector == null)
			{
				GameObject gameObject = new GameObject("effector");
				this.effectRoot = new GameObject("ShineEffect");
				this.effectRoot.transform.SetParent(base.transform);
				this.effectRoot.AddComponent<Image>().sprite = base.gameObject.GetComponent<Image>().sprite;
				this.effectRoot.GetComponent<Image>().type = base.gameObject.GetComponent<Image>().type;
				this.effectRoot.AddComponent<Mask>().showMaskGraphic = false;
				this.effectRoot.transform.localScale = Vector3.one;
				this.effectRoot.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
				this.effectRoot.GetComponent<RectTransform>().anchorMax = Vector2.one;
				this.effectRoot.GetComponent<RectTransform>().anchorMin = Vector2.zero;
				this.effectRoot.GetComponent<RectTransform>().offsetMax = Vector2.zero;
				this.effectRoot.GetComponent<RectTransform>().offsetMin = Vector2.zero;
				this.effectRoot.transform.SetAsFirstSibling();
				gameObject.AddComponent<RectTransform>();
				gameObject.transform.SetParent(this.effectRoot.transform);
				this.effectorRect = gameObject.GetComponent<RectTransform>();
				this.effectorRect.localScale = Vector3.one;
				this.effectorRect.anchoredPosition3D = Vector3.zero;
				this.effectorRect.gameObject.AddComponent<ShineEffect>();
				this.effectorRect.anchorMax = Vector2.one;
				this.effectorRect.anchorMin = Vector2.zero;
				this.effectorRect.Rotate(0f, 0f, -8f);
				this.effector = gameObject.GetComponent<ShineEffect>();
				this.effectorRect.offsetMax = Vector2.zero;
				this.effectorRect.offsetMin = Vector2.zero;
				this.OnValidate();
			}
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000B9264 File Offset: 0x000B7664
		private void OnValidate()
		{
			this.effector.Yoffset = this.yOffset;
			this.effector.Width = this.width;
			if (this.yOffset <= -1f || this.yOffset >= 1f)
			{
				this.effectRoot.SetActive(false);
			}
			else if (!this.effectRoot.activeSelf)
			{
				this.effectRoot.SetActive(true);
			}
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x000B92E0 File Offset: 0x000B76E0
		private void ChangeVal(float value)
		{
			this.effector.Yoffset = value;
			if (value <= -1f || value >= 1f)
			{
				this.effectRoot.SetActive(false);
			}
			else if (!this.effectRoot.activeSelf)
			{
				this.effectRoot.SetActive(true);
			}
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x000B933C File Offset: 0x000B773C
		private void OnDestroy()
		{
			if (!Application.isPlaying)
			{
				Object.DestroyImmediate(this.effectRoot);
			}
			else
			{
				Object.Destroy(this.effectRoot);
			}
		}

		// Token: 0x04001B17 RID: 6935
		public ShineEffect effector;

		// Token: 0x04001B18 RID: 6936
		[SerializeField]
		[HideInInspector]
		private GameObject effectRoot;

		// Token: 0x04001B19 RID: 6937
		[Range(-1f, 1f)]
		public float yOffset = -1f;

		// Token: 0x04001B1A RID: 6938
		[Range(0.1f, 1f)]
		public float width = 0.5f;

		// Token: 0x04001B1B RID: 6939
		private RectTransform effectorRect;
	}
}
