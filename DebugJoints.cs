using System;
using UnityEngine;

// Token: 0x02000BCC RID: 3020
[ExecuteInEditMode]
public class DebugJoints : MonoBehaviour
{
	// Token: 0x060055D4 RID: 21972 RVA: 0x001F643E File Offset: 0x001F483E
	public DebugJoints()
	{
	}

	// Token: 0x060055D5 RID: 21973 RVA: 0x001F6448 File Offset: 0x001F4848
	private void SyncJoints()
	{
		if (this.debugJoints != null)
		{
			foreach (DebugJoint debugJoint in this.debugJoints)
			{
				if (this._showJoints)
				{
					debugJoint.gameObject.SetActive(true);
				}
				else
				{
					debugJoint.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x17000C81 RID: 3201
	// (get) Token: 0x060055D6 RID: 21974 RVA: 0x001F64A7 File Offset: 0x001F48A7
	// (set) Token: 0x060055D7 RID: 21975 RVA: 0x001F64AF File Offset: 0x001F48AF
	public bool showJoints
	{
		get
		{
			return this._showJoints;
		}
		set
		{
			if (this._showJoints != value)
			{
				this._showJoints = value;
				this.SyncJoints();
			}
		}
	}

	// Token: 0x060055D8 RID: 21976 RVA: 0x001F64CA File Offset: 0x001F48CA
	public void InitJoints()
	{
		this.debugJoints = base.GetComponentsInChildren<DebugJoint>(true);
	}

	// Token: 0x060055D9 RID: 21977 RVA: 0x001F64D9 File Offset: 0x001F48D9
	private void Start()
	{
		this.InitJoints();
		this.SyncJoints();
	}

	// Token: 0x040046FC RID: 18172
	private bool _showJoints;

	// Token: 0x040046FD RID: 18173
	private DebugJoint[] debugJoints;
}
