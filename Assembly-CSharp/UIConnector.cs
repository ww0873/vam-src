using System;
using UnityEngine;

// Token: 0x02000D06 RID: 3334
[RequireComponent(typeof(UIConnectorMaster))]
public class UIConnector : MonoBehaviour
{
	// Token: 0x060065B6 RID: 26038 RVA: 0x001D5B6F File Offset: 0x001D3F6F
	public UIConnector()
	{
	}

	// Token: 0x060065B7 RID: 26039 RVA: 0x001D5B78 File Offset: 0x001D3F78
	public virtual void Connect()
	{
		if (this.receiver == null && this.receiverTransform != null)
		{
			JSONStorable[] components = this.receiverTransform.GetComponents<JSONStorable>();
			foreach (JSONStorable jsonstorable in components)
			{
				if (jsonstorable.storeId == this.storeid)
				{
					this.receiver = jsonstorable;
				}
			}
		}
		if (this.receiver != null)
		{
			if (this.altConnector)
			{
				this.receiver.SetUIAlt(base.transform);
			}
			else
			{
				this.receiver.SetUI(base.transform);
			}
		}
	}

	// Token: 0x060065B8 RID: 26040 RVA: 0x001D5C2D File Offset: 0x001D402D
	public virtual void Disconnect()
	{
		if (this.receiver != null)
		{
			if (this.altConnector)
			{
				this.receiver.SetUIAlt(null);
			}
			else
			{
				this.receiver.SetUI(null);
			}
		}
	}

	// Token: 0x04005510 RID: 21776
	public bool altConnector;

	// Token: 0x04005511 RID: 21777
	public bool disable;

	// Token: 0x04005512 RID: 21778
	public Transform receiverTransform;

	// Token: 0x04005513 RID: 21779
	public string storeid;

	// Token: 0x04005514 RID: 21780
	public JSONStorable receiver;
}
