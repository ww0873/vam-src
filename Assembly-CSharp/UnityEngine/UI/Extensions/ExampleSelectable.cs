using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004DC RID: 1244
	public class ExampleSelectable : MonoBehaviour, IBoxSelectable
	{
		// Token: 0x06001F6F RID: 8047 RVA: 0x000B2A72 File Offset: 0x000B0E72
		public ExampleSelectable()
		{
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06001F70 RID: 8048 RVA: 0x000B2A7A File Offset: 0x000B0E7A
		// (set) Token: 0x06001F71 RID: 8049 RVA: 0x000B2A82 File Offset: 0x000B0E82
		public bool selected
		{
			get
			{
				return this._selected;
			}
			set
			{
				this._selected = value;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06001F72 RID: 8050 RVA: 0x000B2A8B File Offset: 0x000B0E8B
		// (set) Token: 0x06001F73 RID: 8051 RVA: 0x000B2A93 File Offset: 0x000B0E93
		public bool preSelected
		{
			get
			{
				return this._preSelected;
			}
			set
			{
				this._preSelected = value;
			}
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x000B2A9C File Offset: 0x000B0E9C
		private void Start()
		{
			this.spriteRenderer = base.transform.GetComponent<SpriteRenderer>();
			this.image = base.transform.GetComponent<Image>();
			this.text = base.transform.GetComponent<Text>();
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x000B2AD4 File Offset: 0x000B0ED4
		private void Update()
		{
			Color color = Color.white;
			if (this.preSelected)
			{
				color = Color.yellow;
			}
			if (this.selected)
			{
				color = Color.green;
			}
			if (this.spriteRenderer)
			{
				this.spriteRenderer.color = color;
			}
			else if (this.text)
			{
				this.text.color = color;
			}
			else if (this.image)
			{
				this.image.color = color;
			}
			else if (base.GetComponent<Renderer>())
			{
				base.GetComponent<Renderer>().material.color = color;
			}
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x000B2B8D File Offset: 0x000B0F8D
		Transform IBoxSelectable.get_transform()
		{
			return base.transform;
		}

		// Token: 0x04001A83 RID: 6787
		private bool _selected;

		// Token: 0x04001A84 RID: 6788
		private bool _preSelected;

		// Token: 0x04001A85 RID: 6789
		private SpriteRenderer spriteRenderer;

		// Token: 0x04001A86 RID: 6790
		private Image image;

		// Token: 0x04001A87 RID: 6791
		private Text text;
	}
}
