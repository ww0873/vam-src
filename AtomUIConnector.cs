using System;
using UnityEngine;

// Token: 0x02000C05 RID: 3077
public class AtomUIConnector : UIConnector
{
	// Token: 0x06005989 RID: 22921 RVA: 0x0020F238 File Offset: 0x0020D638
	public AtomUIConnector()
	{
	}

	// Token: 0x0600598A RID: 22922 RVA: 0x0020F240 File Offset: 0x0020D640
	public override void Connect()
	{
		Debug.LogError("AtomUIConnector obsolete but still in use");
	}

	// Token: 0x040049B0 RID: 18864
	public Atom atom;
}
