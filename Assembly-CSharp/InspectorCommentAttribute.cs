using System;
using UnityEngine;

// Token: 0x0200076F RID: 1903
public class InspectorCommentAttribute : PropertyAttribute
{
	// Token: 0x06003122 RID: 12578 RVA: 0x000FF78E File Offset: 0x000FDB8E
	public InspectorCommentAttribute(string message = "")
	{
		this.message = message;
	}

	// Token: 0x040024E3 RID: 9443
	public readonly string message;
}
