using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200035A RID: 858
public class CommandLineChecker : MonoBehaviour
{
	// Token: 0x06001524 RID: 5412 RVA: 0x000782F0 File Offset: 0x000766F0
	public CommandLineChecker()
	{
	}

	// Token: 0x06001525 RID: 5413 RVA: 0x00078368 File Offset: 0x00076768
	private void GetCommandLineArgs()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		this.CommandArgs = new List<string>();
		foreach (string item in commandLineArgs)
		{
			this.CommandArgs.Add(item);
		}
	}

	// Token: 0x06001526 RID: 5414 RVA: 0x000783AC File Offset: 0x000767AC
	private bool CheckLaunchArg()
	{
		foreach (string text in this.CommandArgs)
		{
			if (text.Contains("-LaunchArg=" + this.LaunchArgument))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001527 RID: 5415 RVA: 0x00078428 File Offset: 0x00076828
	private void GUICommandLineCheckingFailed()
	{
		this.windowRect0 = GUILayout.Window(0, this.windowRect0, new GUI.WindowFunction(this.WindowFunction), "Launch argument fails!", new GUILayoutOption[0]);
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x00078454 File Offset: 0x00076854
	private void WindowFunction(int id)
	{
		GUILayout.Label("Launch your game with Launcher! You can't open your game without it! Application will now quit!", new GUILayoutOption[0]);
		if (GUILayout.Button("OK", new GUILayoutOption[0]))
		{
			Application.Quit();
		}
		GUI.DragWindow(new Rect((float)(Screen.width / 2) - CommandLineChecker.windowSize.x / 2f - 20f, (float)(Screen.height / 2) - CommandLineChecker.windowSize.y / 2f - 20f, CommandLineChecker.windowSize.x, CommandLineChecker.windowSize.y + 20f));
	}

	// Token: 0x06001529 RID: 5417 RVA: 0x000784ED File Offset: 0x000768ED
	private void Start()
	{
		this.GetCommandLineArgs();
		if (this.CheckLaunchArg())
		{
			SceneManager.LoadScene(this.LoadLevelId);
		}
		else
		{
			this._guiState = GUIState.COMMAND_LINE_CHECKING_FAILED;
		}
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x00078518 File Offset: 0x00076918
	private void OnGUI()
	{
		GUIState guiState = this._guiState;
		if (guiState != GUIState.NONE)
		{
			if (guiState == GUIState.COMMAND_LINE_CHECKING_FAILED)
			{
				this.GUICommandLineCheckingFailed();
			}
		}
	}

	// Token: 0x0600152B RID: 5419 RVA: 0x0007854E File Offset: 0x0007694E
	// Note: this type is marked as 'beforefieldinit'.
	static CommandLineChecker()
	{
	}

	// Token: 0x040011BE RID: 4542
	public string LaunchArgument = "default";

	// Token: 0x040011BF RID: 4543
	public int LoadLevelId = 1;

	// Token: 0x040011C0 RID: 4544
	private List<string> CommandArgs;

	// Token: 0x040011C1 RID: 4545
	private GUIState _guiState;

	// Token: 0x040011C2 RID: 4546
	private static Vector2 windowSize = new Vector2(250f, 150f);

	// Token: 0x040011C3 RID: 4547
	private Rect windowRect0 = new Rect((float)(Screen.width / 2) - CommandLineChecker.windowSize.x / 2f, (float)(Screen.height / 2) - CommandLineChecker.windowSize.y / 2f, CommandLineChecker.windowSize.x, CommandLineChecker.windowSize.y);
}
