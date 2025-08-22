using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000905 RID: 2309
public class OVRPlatformMenu : MonoBehaviour
{
	// Token: 0x06003A1B RID: 14875 RVA: 0x0011C3E9 File Offset: 0x0011A7E9
	public OVRPlatformMenu()
	{
	}

	// Token: 0x06003A1C RID: 14876 RVA: 0x0011C3FC File Offset: 0x0011A7FC
	private OVRPlatformMenu.eBackButtonAction HandleBackButtonState()
	{
		OVRPlatformMenu.eBackButtonAction result = OVRPlatformMenu.eBackButtonAction.NONE;
		if (OVRInput.GetDown(this.inputCode, OVRInput.Controller.Active))
		{
			result = OVRPlatformMenu.eBackButtonAction.SHORT_PRESS;
		}
		return result;
	}

	// Token: 0x06003A1D RID: 14877 RVA: 0x0011C424 File Offset: 0x0011A824
	private void Awake()
	{
		if (this.shortPressHandler == OVRPlatformMenu.eHandler.RetreatOneLevel && this.OnShortPress == null)
		{
			if (OVRPlatformMenu.<>f__mg$cache0 == null)
			{
				OVRPlatformMenu.<>f__mg$cache0 = new Func<bool>(OVRPlatformMenu.RetreatOneLevel);
			}
			this.OnShortPress = OVRPlatformMenu.<>f__mg$cache0;
		}
		if (!OVRManager.isHmdPresent)
		{
			base.enabled = false;
			return;
		}
		OVRPlatformMenu.sceneStack.Push(SceneManager.GetActiveScene().name);
	}

	// Token: 0x06003A1E RID: 14878 RVA: 0x0011C494 File Offset: 0x0011A894
	private void ShowConfirmQuitMenu()
	{
	}

	// Token: 0x06003A1F RID: 14879 RVA: 0x0011C498 File Offset: 0x0011A898
	private static bool RetreatOneLevel()
	{
		if (OVRPlatformMenu.sceneStack.Count > 1)
		{
			string sceneName = OVRPlatformMenu.sceneStack.Pop();
			SceneManager.LoadSceneAsync(sceneName);
			return false;
		}
		return true;
	}

	// Token: 0x06003A20 RID: 14880 RVA: 0x0011C4CA File Offset: 0x0011A8CA
	private void Update()
	{
	}

	// Token: 0x06003A21 RID: 14881 RVA: 0x0011C4CC File Offset: 0x0011A8CC
	// Note: this type is marked as 'beforefieldinit'.
	static OVRPlatformMenu()
	{
	}

	// Token: 0x04002BB4 RID: 11188
	private OVRInput.RawButton inputCode = OVRInput.RawButton.Back;

	// Token: 0x04002BB5 RID: 11189
	public OVRPlatformMenu.eHandler shortPressHandler;

	// Token: 0x04002BB6 RID: 11190
	public Func<bool> OnShortPress;

	// Token: 0x04002BB7 RID: 11191
	private static Stack<string> sceneStack = new Stack<string>();

	// Token: 0x04002BB8 RID: 11192
	[CompilerGenerated]
	private static Func<bool> <>f__mg$cache0;

	// Token: 0x02000906 RID: 2310
	public enum eHandler
	{
		// Token: 0x04002BBA RID: 11194
		ShowConfirmQuit,
		// Token: 0x04002BBB RID: 11195
		RetreatOneLevel
	}

	// Token: 0x02000907 RID: 2311
	private enum eBackButtonAction
	{
		// Token: 0x04002BBD RID: 11197
		NONE,
		// Token: 0x04002BBE RID: 11198
		SHORT_PRESS
	}
}
