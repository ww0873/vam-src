using System;
using UnityEngine;

// Token: 0x0200076E RID: 1902
public class InspectorNoteAttribute : PropertyAttribute
{
	// Token: 0x06003121 RID: 12577 RVA: 0x000FF778 File Offset: 0x000FDB78
	public InspectorNoteAttribute(string header, string message = "")
	{
		this.header = header;
		this.message = message;
	}

	// Token: 0x040024E1 RID: 9441
	public readonly string header;

	// Token: 0x040024E2 RID: 9442
	public readonly string message;
}
