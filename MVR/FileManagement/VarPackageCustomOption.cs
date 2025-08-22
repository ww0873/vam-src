using System;
using SimpleJSON;

namespace MVR.FileManagement
{
	// Token: 0x02000BF9 RID: 3065
	[Serializable]
	public class VarPackageCustomOption
	{
		// Token: 0x0600587D RID: 22653 RVA: 0x002070AC File Offset: 0x002054AC
		public VarPackageCustomOption()
		{
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x0600587E RID: 22654 RVA: 0x002070B4 File Offset: 0x002054B4
		// (set) Token: 0x0600587F RID: 22655 RVA: 0x002070CE File Offset: 0x002054CE
		public bool Value
		{
			get
			{
				return this.boolJSON != null && this.boolJSON.val;
			}
			set
			{
				if (this.boolJSON != null)
				{
					this.boolJSON.val = value;
				}
			}
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06005880 RID: 22656 RVA: 0x002070E7 File Offset: 0x002054E7
		// (set) Token: 0x06005881 RID: 22657 RVA: 0x00207101 File Offset: 0x00205501
		public bool ValueNoCallback
		{
			get
			{
				return this.boolJSON != null && this.boolJSON.val;
			}
			set
			{
				if (this.boolJSON != null)
				{
					this.boolJSON.valNoCallback = value;
				}
			}
		}

		// Token: 0x06005882 RID: 22658 RVA: 0x0020711A File Offset: 0x0020551A
		public void Init(JSONStorableBool.SetJSONBoolCallback callback)
		{
			this.boolJSON = new JSONStorableBool(this.name, this.defaultValue, callback);
		}

		// Token: 0x06005883 RID: 22659 RVA: 0x00207134 File Offset: 0x00205534
		public void SetToggle(UIDynamicToggle toggle)
		{
			this.boolJSON.toggle = toggle.toggle;
			toggle.label = this.displayName;
		}

		// Token: 0x06005884 RID: 22660 RVA: 0x00207153 File Offset: 0x00205553
		public void StoreJSON(JSONClass jc)
		{
			this.boolJSON.StoreJSON(jc, true, true, true);
		}

		// Token: 0x06005885 RID: 22661 RVA: 0x00207165 File Offset: 0x00205565
		public void RestoreFromJSON(JSONClass jc)
		{
			this.boolJSON.RestoreFromJSON(jc, true, true, true);
		}

		// Token: 0x040048E5 RID: 18661
		public string name;

		// Token: 0x040048E6 RID: 18662
		public string displayName;

		// Token: 0x040048E7 RID: 18663
		public bool defaultValue;

		// Token: 0x040048E8 RID: 18664
		protected JSONStorableBool boolJSON;
	}
}
