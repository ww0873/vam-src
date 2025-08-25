using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004C8 RID: 1224
	[Serializable]
	public class DropDownListItem
	{
		// Token: 0x06001EDE RID: 7902 RVA: 0x000B02E0 File Offset: 0x000AE6E0
		public DropDownListItem(string caption = "", string inId = "", Sprite image = null, bool disabled = false, Action onSelect = null)
		{
			this._caption = caption;
			this._image = image;
			this._id = inId;
			this._isDisabled = disabled;
			this.OnSelect = onSelect;
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001EDF RID: 7903 RVA: 0x000B030D File Offset: 0x000AE70D
		// (set) Token: 0x06001EE0 RID: 7904 RVA: 0x000B0315 File Offset: 0x000AE715
		public string Caption
		{
			get
			{
				return this._caption;
			}
			set
			{
				this._caption = value;
				if (this.OnUpdate != null)
				{
					this.OnUpdate();
				}
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001EE1 RID: 7905 RVA: 0x000B0334 File Offset: 0x000AE734
		// (set) Token: 0x06001EE2 RID: 7906 RVA: 0x000B033C File Offset: 0x000AE73C
		public Sprite Image
		{
			get
			{
				return this._image;
			}
			set
			{
				this._image = value;
				if (this.OnUpdate != null)
				{
					this.OnUpdate();
				}
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001EE3 RID: 7907 RVA: 0x000B035B File Offset: 0x000AE75B
		// (set) Token: 0x06001EE4 RID: 7908 RVA: 0x000B0363 File Offset: 0x000AE763
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				this._isDisabled = value;
				if (this.OnUpdate != null)
				{
					this.OnUpdate();
				}
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001EE5 RID: 7909 RVA: 0x000B0382 File Offset: 0x000AE782
		// (set) Token: 0x06001EE6 RID: 7910 RVA: 0x000B038A File Offset: 0x000AE78A
		public string ID
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x04001A1A RID: 6682
		[SerializeField]
		private string _caption;

		// Token: 0x04001A1B RID: 6683
		[SerializeField]
		private Sprite _image;

		// Token: 0x04001A1C RID: 6684
		[SerializeField]
		private bool _isDisabled;

		// Token: 0x04001A1D RID: 6685
		[SerializeField]
		private string _id;

		// Token: 0x04001A1E RID: 6686
		public Action OnSelect;

		// Token: 0x04001A1F RID: 6687
		internal Action OnUpdate;
	}
}
