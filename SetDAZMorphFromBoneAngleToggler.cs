using System;
using UnityEngine;

// Token: 0x02000B33 RID: 2867
public class SetDAZMorphFromBoneAngleToggler : MonoBehaviour
{
	// Token: 0x06004E6C RID: 20076 RVA: 0x001B9CCA File Offset: 0x001B80CA
	public SetDAZMorphFromBoneAngleToggler()
	{
	}

	// Token: 0x17000B2B RID: 2859
	// (get) Token: 0x06004E6D RID: 20077 RVA: 0x001B9CD9 File Offset: 0x001B80D9
	// (set) Token: 0x06004E6E RID: 20078 RVA: 0x001B9CE4 File Offset: 0x001B80E4
	public bool morphsEnabled
	{
		get
		{
			return this._morphsEnabled;
		}
		set
		{
			this._morphsEnabled = value;
			SetDAZMorphFromBoneAngle[] components = base.GetComponents<SetDAZMorphFromBoneAngle>();
			foreach (string b in this.morphNames)
			{
				foreach (SetDAZMorphFromBoneAngle setDAZMorphFromBoneAngle in components)
				{
					if (setDAZMorphFromBoneAngle.morph1Name == b)
					{
						setDAZMorphFromBoneAngle.enabled = value;
						break;
					}
				}
			}
		}
	}

	// Token: 0x04003E29 RID: 15913
	private bool _morphsEnabled = true;

	// Token: 0x04003E2A RID: 15914
	public string[] morphNames;
}
