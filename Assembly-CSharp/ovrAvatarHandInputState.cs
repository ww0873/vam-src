using System;
using System.Runtime.InteropServices;

// Token: 0x020007A2 RID: 1954
public struct ovrAvatarHandInputState
{
	// Token: 0x04002606 RID: 9734
	public ovrAvatarTransform transform;

	// Token: 0x04002607 RID: 9735
	public ovrAvatarButton buttonMask;

	// Token: 0x04002608 RID: 9736
	public ovrAvatarTouch touchMask;

	// Token: 0x04002609 RID: 9737
	public float joystickX;

	// Token: 0x0400260A RID: 9738
	public float joystickY;

	// Token: 0x0400260B RID: 9739
	public float indexTrigger;

	// Token: 0x0400260C RID: 9740
	public float handTrigger;

	// Token: 0x0400260D RID: 9741
	[MarshalAs(UnmanagedType.I1)]
	public bool isActive;
}
