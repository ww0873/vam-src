using System;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;

// Token: 0x02000D08 RID: 3336
[RequireComponent(typeof(UIConnectorMaster))]
public class UIMultiConnector : MonoBehaviour
{
	// Token: 0x060065C4 RID: 26052 RVA: 0x00265E99 File Offset: 0x00264299
	public UIMultiConnector()
	{
	}

	// Token: 0x060065C5 RID: 26053 RVA: 0x00265EA4 File Offset: 0x002642A4
	public virtual void Connect()
	{
		Component[] componentsInChildren = base.GetComponentsInChildren(this.UITypeToConnect.Type);
		List<UIProvider> list = new List<UIProvider>();
		foreach (Component component in componentsInChildren)
		{
			UIProvider uiprovider = component as UIProvider;
			if (uiprovider != null && uiprovider.completeProvider)
			{
				list.Add(uiprovider);
			}
		}
		if (list.Count > this.connectors.Length)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Number of complete ",
				this.UITypeToConnect.Type,
				" objects ",
				list.Count,
				" is greater than number of connnectors ",
				this.connectors.Length
			}));
		}
		for (int j = 0; j < this.connectors.Length; j++)
		{
			UIMultiConnector.Connector connector = this.connectors[j];
			if (j < list.Count)
			{
				UIProvider uiprovider2 = list[j];
				if (uiprovider2 != null)
				{
					if (connector.receiver == null && connector.receiverTransform != null)
					{
						JSONStorable[] components = connector.receiverTransform.GetComponents<JSONStorable>();
						foreach (JSONStorable jsonstorable in components)
						{
							if (jsonstorable.storeId == connector.storeid)
							{
								connector.receiver = jsonstorable;
							}
						}
					}
					if (connector.receiver != null)
					{
						if (this.altConnector)
						{
							connector.receiver.SetUIAlt(uiprovider2.transform);
						}
						else
						{
							connector.receiver.SetUI(uiprovider2.transform);
						}
					}
					else
					{
						Debug.LogError("Could not get receiver on connector " + connector.storeid);
					}
				}
				else
				{
					Debug.LogError("UIProvider is null");
				}
			}
		}
	}

	// Token: 0x060065C6 RID: 26054 RVA: 0x002660B0 File Offset: 0x002644B0
	public virtual void Disconnect()
	{
		for (int i = 0; i < this.connectors.Length; i++)
		{
			UIMultiConnector.Connector connector = this.connectors[i];
			if (connector.receiver != null)
			{
				if (this.altConnector)
				{
					connector.receiver.SetUIAlt(null);
				}
				else
				{
					connector.receiver.SetUI(null);
				}
			}
		}
	}

	// Token: 0x060065C7 RID: 26055 RVA: 0x00266118 File Offset: 0x00264518
	public virtual void ClearConnectors()
	{
		this.connectors = new UIMultiConnector.Connector[0];
	}

	// Token: 0x060065C8 RID: 26056 RVA: 0x00266128 File Offset: 0x00264528
	public virtual void AddConnector(JSONStorable js)
	{
		bool flag = false;
		if (this.connectors != null)
		{
			for (int i = 0; i < this.connectors.Length; i++)
			{
				if (this.connectors[i].receiver == js)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			int num;
			if (this.connectors != null)
			{
				num = this.connectors.Length + 1;
			}
			else
			{
				num = 1;
			}
			UIMultiConnector.Connector[] array = new UIMultiConnector.Connector[num];
			for (int j = 0; j < num - 1; j++)
			{
				array[j] = this.connectors[j];
			}
			UIMultiConnector.Connector connector = new UIMultiConnector.Connector();
			connector.receiverTransform = js.transform;
			connector.receiver = js;
			connector.storeid = js.storeId;
			array[num - 1] = connector;
			this.connectors = array;
		}
	}

	// Token: 0x060065C9 RID: 26057 RVA: 0x00266204 File Offset: 0x00264604
	public virtual void RemoveConnector(JSONStorable js)
	{
		List<UIMultiConnector.Connector> list = new List<UIMultiConnector.Connector>();
		if (this.connectors != null)
		{
			bool flag = false;
			for (int i = 0; i < this.connectors.Length; i++)
			{
				if (this.connectors[i].receiver == js)
				{
					flag = true;
				}
				else
				{
					list.Add(this.connectors[i]);
				}
			}
			if (flag)
			{
				this.connectors = list.ToArray();
			}
		}
	}

	// Token: 0x04005521 RID: 21793
	public bool altConnector;

	// Token: 0x04005522 RID: 21794
	public bool disable;

	// Token: 0x04005523 RID: 21795
	public UIMultiConnector.Connector[] connectors;

	// Token: 0x04005524 RID: 21796
	[ClassExtends(typeof(JSONStorable))]
	public ClassTypeReference typeToConnect;

	// Token: 0x04005525 RID: 21797
	[ClassExtends(typeof(UIProvider))]
	public ClassTypeReference UITypeToConnect;

	// Token: 0x02000D09 RID: 3337
	[Serializable]
	public class Connector
	{
		// Token: 0x060065CA RID: 26058 RVA: 0x0026627C File Offset: 0x0026467C
		public Connector()
		{
		}

		// Token: 0x04005526 RID: 21798
		public Transform receiverTransform;

		// Token: 0x04005527 RID: 21799
		public string storeid;

		// Token: 0x04005528 RID: 21800
		public JSONStorable receiver;
	}
}
