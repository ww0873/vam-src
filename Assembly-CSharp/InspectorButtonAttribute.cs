using System;
using UnityEngine;

// Token: 0x020003C9 RID: 969
[AttributeUsage(AttributeTargets.Field)]
public class InspectorButtonAttribute : PropertyAttribute
{
	// Token: 0x0600189A RID: 6298 RVA: 0x0008B0B0 File Offset: 0x000894B0
	public InspectorButtonAttribute(string MethodName)
	{
		this.MethodName = MethodName;
	}

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x0600189B RID: 6299 RVA: 0x0008B0CA File Offset: 0x000894CA
	// (set) Token: 0x0600189C RID: 6300 RVA: 0x0008B0D2 File Offset: 0x000894D2
	public float ButtonWidth
	{
		get
		{
			return this._buttonWidth;
		}
		set
		{
			this._buttonWidth = value;
		}
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x0008B0DB File Offset: 0x000894DB
	// Note: this type is marked as 'beforefieldinit'.
	static InspectorButtonAttribute()
	{
	}

	// Token: 0x040013FB RID: 5115
	public static float kDefaultButtonWidth = 80f;

	// Token: 0x040013FC RID: 5116
	public readonly string MethodName;

	// Token: 0x040013FD RID: 5117
	private float _buttonWidth = InspectorButtonAttribute.kDefaultButtonWidth;
}
