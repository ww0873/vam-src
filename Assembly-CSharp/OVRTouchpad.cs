using System;
using UnityEngine;

// Token: 0x020007CD RID: 1997
internal static class OVRTouchpad
{
	// Token: 0x060032AA RID: 12970 RVA: 0x00107A57 File Offset: 0x00105E57
	public static void Create()
	{
	}

	// Token: 0x060032AB RID: 12971 RVA: 0x00107A5C File Offset: 0x00105E5C
	public static void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			OVRTouchpad.moveAmountMouse = Input.mousePosition;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			OVRTouchpad.moveAmountMouse -= Input.mousePosition;
			OVRTouchpad.HandleInputMouse(ref OVRTouchpad.moveAmountMouse);
		}
	}

	// Token: 0x060032AC RID: 12972 RVA: 0x00107AAC File Offset: 0x00105EAC
	public static void OnDisable()
	{
	}

	// Token: 0x060032AD RID: 12973 RVA: 0x00107AB0 File Offset: 0x00105EB0
	private static void HandleInputMouse(ref Vector3 move)
	{
		if (move.magnitude < OVRTouchpad.minMovMagnitudeMouse)
		{
			OVRMessenger.Broadcast<OVRTouchpad.TouchEvent>("Touchpad", OVRTouchpad.TouchEvent.SingleTap);
		}
		else
		{
			move.Normalize();
			if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
			{
				if (move.x > 0f)
				{
					OVRMessenger.Broadcast<OVRTouchpad.TouchEvent>("Touchpad", OVRTouchpad.TouchEvent.Left);
				}
				else
				{
					OVRMessenger.Broadcast<OVRTouchpad.TouchEvent>("Touchpad", OVRTouchpad.TouchEvent.Right);
				}
			}
			else if (move.y > 0f)
			{
				OVRMessenger.Broadcast<OVRTouchpad.TouchEvent>("Touchpad", OVRTouchpad.TouchEvent.Down);
			}
			else
			{
				OVRMessenger.Broadcast<OVRTouchpad.TouchEvent>("Touchpad", OVRTouchpad.TouchEvent.Up);
			}
		}
	}

	// Token: 0x060032AE RID: 12974 RVA: 0x00107B59 File Offset: 0x00105F59
	// Note: this type is marked as 'beforefieldinit'.
	static OVRTouchpad()
	{
	}

	// Token: 0x04002699 RID: 9881
	private static Vector3 moveAmountMouse;

	// Token: 0x0400269A RID: 9882
	private static float minMovMagnitudeMouse = 25f;

	// Token: 0x0400269B RID: 9883
	private static OVRTouchpadHelper touchpadHelper = new GameObject("OVRTouchpadHelper").AddComponent<OVRTouchpadHelper>();

	// Token: 0x020007CE RID: 1998
	public enum TouchEvent
	{
		// Token: 0x0400269D RID: 9885
		SingleTap,
		// Token: 0x0400269E RID: 9886
		DoubleTap,
		// Token: 0x0400269F RID: 9887
		Left,
		// Token: 0x040026A0 RID: 9888
		Right,
		// Token: 0x040026A1 RID: 9889
		Up,
		// Token: 0x040026A2 RID: 9890
		Down
	}
}
