using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using MHLab.PATCH;
using MHLab.PATCH.Debugging;
using MHLab.PATCH.Install;
using MHLab.PATCH.Settings;
using MHLab.PATCH.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityThreading;

// Token: 0x0200035D RID: 861
public class Launcher : MonoBehaviour
{
	// Token: 0x0600153B RID: 5435 RVA: 0x00078700 File Offset: 0x00076B00
	public Launcher()
	{
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x000787AC File Offset: 0x00076BAC
	private void Start()
	{
		Singleton<Localizatron>.Instance.SetLanguage("en_EN");
		this.LocalizeGUI();
		this.OverrideSettings(true);
		this.m_launcher = new LauncherManager();
		this.m_launcher.SetOnSetMainProgressBarAction(new Action<int, int>(this.OnSetMainProgressBar));
		this.m_launcher.SetOnSetDetailProgressBarAction(new Action<int, int>(this.OnSetDetailProgressBar));
		this.m_launcher.SetOnIncreaseMainProgressBarAction(new Action(this.OnIncreaseMainProgressBar));
		this.m_launcher.SetOnIncreaseDetailProgressBarAction(new Action(this.OnIncreaseDetailProgressBar));
		this.m_launcher.SetOnLogAction(new Action<string, string>(this.OnLog));
		this.m_launcher.SetOnErrorAction(new Action<string, string, Exception>(this.OnError));
		this.m_launcher.SetOnFatalErrorAction(new Action<string, string, Exception>(this.OnFatalError));
		this.m_launcher.SetOnTaskStartedAction(new Action<string>(this.OnTaskStarted));
		this.m_launcher.SetOnTaskCompletedAction(new Action<string>(this.OnTaskCompleted));
		this.m_launcher.SetOnDownloadProgressAction(new Action<long, long, int>(this.OnDownloadProgress));
		this.m_launcher.SetOnDownloadCompletedAction(new Action(this.OnDownloadCompleted));
		this.m_installer = new InstallManager();
		this.m_installer.SetOnSetMainProgressBarAction(new Action<int, int>(this.OnSetMainProgressBar));
		this.m_installer.SetOnSetDetailProgressBarAction(new Action<int, int>(this.OnSetDetailProgressBar));
		this.m_installer.SetOnIncreaseMainProgressBarAction(new Action(this.OnIncreaseMainProgressBar));
		this.m_installer.SetOnIncreaseDetailProgressBarAction(new Action(this.OnIncreaseDetailProgressBar));
		this.m_installer.SetOnLogAction(new Action<string, string>(this.OnLog));
		this.m_installer.SetOnErrorAction(new Action<string, string, Exception>(this.OnError));
		this.m_installer.SetOnFatalErrorAction(new Action<string, string, Exception>(this.OnFatalError));
		this.m_installer.SetOnTaskStartedAction(new Action<string>(this.OnTaskStarted));
		this.m_installer.SetOnTaskCompletedAction(new Action<string>(this.OnTaskCompleted));
		this.m_installer.SetOnDownloadProgressAction(new Action<long, long, int>(this.OnDownloadProgress));
		this.m_installer.SetOnDownloadCompletedAction(new Action(this.OnDownloadCompleted));
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x000789E8 File Offset: 0x00076DE8
	private void OnSetMainProgressBar(int min, int max)
	{
		Launcher.<OnSetMainProgressBar>c__AnonStorey0 <OnSetMainProgressBar>c__AnonStorey = new Launcher.<OnSetMainProgressBar>c__AnonStorey0();
		<OnSetMainProgressBar>c__AnonStorey.max = max;
		<OnSetMainProgressBar>c__AnonStorey.min = min;
		<OnSetMainProgressBar>c__AnonStorey.$this = this;
		UnityThreadHelper.Dispatcher.Dispatch(new Action(<OnSetMainProgressBar>c__AnonStorey.<>m__0));
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x00078A28 File Offset: 0x00076E28
	private void OnSetDetailProgressBar(int min, int max)
	{
		Launcher.<OnSetDetailProgressBar>c__AnonStorey1 <OnSetDetailProgressBar>c__AnonStorey = new Launcher.<OnSetDetailProgressBar>c__AnonStorey1();
		<OnSetDetailProgressBar>c__AnonStorey.max = max;
		<OnSetDetailProgressBar>c__AnonStorey.min = min;
		<OnSetDetailProgressBar>c__AnonStorey.$this = this;
		UnityThreadHelper.Dispatcher.Dispatch(new Action(<OnSetDetailProgressBar>c__AnonStorey.<>m__0));
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x00078A67 File Offset: 0x00076E67
	private void OnIncreaseMainProgressBar()
	{
		UnityThreadHelper.Dispatcher.Dispatch(new Action(this.<OnIncreaseMainProgressBar>m__0));
	}

	// Token: 0x06001540 RID: 5440 RVA: 0x00078A80 File Offset: 0x00076E80
	private void OnIncreaseDetailProgressBar()
	{
		UnityThreadHelper.Dispatcher.Dispatch(new Action(this.<OnIncreaseDetailProgressBar>m__1));
	}

	// Token: 0x06001541 RID: 5441 RVA: 0x00078A9C File Offset: 0x00076E9C
	private void OnLog(string main, string detail)
	{
		Launcher.<OnLog>c__AnonStorey2 <OnLog>c__AnonStorey = new Launcher.<OnLog>c__AnonStorey2();
		<OnLog>c__AnonStorey.main = main;
		<OnLog>c__AnonStorey.detail = detail;
		<OnLog>c__AnonStorey.$this = this;
		UnityThreadHelper.Dispatcher.Dispatch(new Action(<OnLog>c__AnonStorey.<>m__0));
		MHLab.PATCH.Debugging.Debugger.Log(<OnLog>c__AnonStorey.main + " - " + <OnLog>c__AnonStorey.detail);
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x00078AF8 File Offset: 0x00076EF8
	private void OnError(string main, string detail, Exception e)
	{
		Launcher.<OnError>c__AnonStorey3 <OnError>c__AnonStorey = new Launcher.<OnError>c__AnonStorey3();
		<OnError>c__AnonStorey.main = main;
		<OnError>c__AnonStorey.detail = detail;
		<OnError>c__AnonStorey.$this = this;
		UnityThreadHelper.Dispatcher.Dispatch(new Action(<OnError>c__AnonStorey.<>m__0));
		MHLab.PATCH.Debugging.Debugger.Log(e.Message);
		this.m_updateCheckingThread.Abort();
	}

	// Token: 0x06001543 RID: 5443 RVA: 0x00078B50 File Offset: 0x00076F50
	private void OnFatalError(string main, string detail, Exception e)
	{
		Launcher.<OnFatalError>c__AnonStorey4 <OnFatalError>c__AnonStorey = new Launcher.<OnFatalError>c__AnonStorey4();
		<OnFatalError>c__AnonStorey.main = main;
		<OnFatalError>c__AnonStorey.detail = detail;
		<OnFatalError>c__AnonStorey.$this = this;
		UnityThreadHelper.Dispatcher.Dispatch(new Action(<OnFatalError>c__AnonStorey.<>m__0));
		MHLab.PATCH.Debugging.Debugger.Log(e.Message);
		this.m_updateCheckingThread.Abort();
	}

	// Token: 0x06001544 RID: 5444 RVA: 0x00078BA8 File Offset: 0x00076FA8
	private void OnTaskStarted(string message)
	{
		Launcher.<OnTaskStarted>c__AnonStorey5 <OnTaskStarted>c__AnonStorey = new Launcher.<OnTaskStarted>c__AnonStorey5();
		<OnTaskStarted>c__AnonStorey.message = message;
		<OnTaskStarted>c__AnonStorey.$this = this;
		UnityThreadHelper.Dispatcher.Dispatch(new Action(<OnTaskStarted>c__AnonStorey.<>m__0));
		MHLab.PATCH.Debugging.Debugger.Log(<OnTaskStarted>c__AnonStorey.message);
	}

	// Token: 0x06001545 RID: 5445 RVA: 0x00078BEC File Offset: 0x00076FEC
	private void OnTaskCompleted(string message)
	{
		Launcher.<OnTaskCompleted>c__AnonStorey6 <OnTaskCompleted>c__AnonStorey = new Launcher.<OnTaskCompleted>c__AnonStorey6();
		<OnTaskCompleted>c__AnonStorey.message = message;
		<OnTaskCompleted>c__AnonStorey.$this = this;
		UnityThreadHelper.Dispatcher.Dispatch(new Action(<OnTaskCompleted>c__AnonStorey.<>m__0));
		MHLab.PATCH.Debugging.Debugger.Log(<OnTaskCompleted>c__AnonStorey.message);
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x00078C30 File Offset: 0x00077030
	private void OnDownloadProgress(long currentFileSize, long totalFileSize, int percentageCompleted)
	{
		Launcher.<OnDownloadProgress>c__AnonStorey7 <OnDownloadProgress>c__AnonStorey = new Launcher.<OnDownloadProgress>c__AnonStorey7();
		<OnDownloadProgress>c__AnonStorey.percentageCompleted = percentageCompleted;
		<OnDownloadProgress>c__AnonStorey.currentFileSize = currentFileSize;
		<OnDownloadProgress>c__AnonStorey.totalFileSize = totalFileSize;
		<OnDownloadProgress>c__AnonStorey.$this = this;
		if (this._lastTime.AddSeconds(1.0) <= DateTime.UtcNow)
		{
			this._downloadSpeed = (int)((double)(<OnDownloadProgress>c__AnonStorey.currentFileSize - this._lastSize) / (DateTime.UtcNow - this._lastTime).TotalSeconds);
			this._lastSize = <OnDownloadProgress>c__AnonStorey.currentFileSize;
			this._lastTime = DateTime.UtcNow;
		}
		UnityThreadHelper.Dispatcher.Dispatch(new Action(<OnDownloadProgress>c__AnonStorey.<>m__0));
	}

	// Token: 0x06001547 RID: 5447 RVA: 0x00078CDE File Offset: 0x000770DE
	private void OnDownloadCompleted()
	{
		UnityThreadHelper.Dispatcher.Dispatch(new Action(this.<OnDownloadCompleted>m__2));
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x00078CF7 File Offset: 0x000770F7
	private void CheckForPatches()
	{
		this.m_launcher.CheckForUpdates();
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x00078D05 File Offset: 0x00077105
	public void CloseButton_click()
	{
		Application.Quit();
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x00078D0C File Offset: 0x0007710C
	public void StartGame_click()
	{
		SceneManager.LoadScene(this.SceneToLoad);
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x00078D1C File Offset: 0x0007711C
	public void OptionButton_click()
	{
		if (this.MainMenu != null)
		{
			this.MainMenu.gameObject.SetActive(false);
		}
		if (this.OptionsMenu != null)
		{
			this.OptionsMenu.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x00078D70 File Offset: 0x00077170
	public void BackButton_click()
	{
		if (this.OptionsMenu != null)
		{
			this.OptionsMenu.gameObject.SetActive(false);
		}
		if (this.MainMenu != null)
		{
			this.MainMenu.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600154D RID: 5453 RVA: 0x00078DC1 File Offset: 0x000771C1
	public void EnglishButton_click()
	{
		Singleton<Localizatron>.Instance.SetLanguage("en_EN");
		this.LocalizeGUI();
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x00078DD9 File Offset: 0x000771D9
	public void ItalianButton_click()
	{
		Singleton<Localizatron>.Instance.SetLanguage("it_IT");
		this.LocalizeGUI();
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x00078DF4 File Offset: 0x000771F4
	public void LaunchButton_click()
	{
		try
		{
			if (!this.InstallInLocalPath)
			{
				SettingsManager.APP_PATH = this.m_installer.GetInstallationPath();
				SettingsManager.RegeneratePaths();
				this.OverrideSettings(false);
			}
			new Process
			{
				StartInfo = 
				{
					FileName = SettingsManager.LAUNCH_APP,
					Arguments = ((!SettingsManager.USE_RAW_LAUNCH_ARG) ? SettingsManager.LAUNCH_COMMAND : SettingsManager.LAUNCH_ARG),
					UseShellExecute = false
				}
			}.Start();
			if (this.CloseLauncherOnStart)
			{
				Application.Quit();
			}
			if (!this.InstallInLocalPath)
			{
				SettingsManager.APP_PATH = Directory.GetParent(Application.dataPath).FullName;
				SettingsManager.RegeneratePaths();
			}
		}
		catch
		{
			if (this.CloseLauncherOnStart)
			{
				Application.Quit();
			}
		}
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x00078ED4 File Offset: 0x000772D4
	public void RestartButton_click()
	{
		new Process
		{
			StartInfo = 
			{
				FileName = SettingsManager.LAUNCH_APP,
				Arguments = ((!SettingsManager.USE_RAW_LAUNCH_ARG) ? SettingsManager.LAUNCH_COMMAND : SettingsManager.LAUNCH_ARG),
				UseShellExecute = false,
				Verb = "runas"
			}
		}.Start();
		Application.Quit();
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x00078F44 File Offset: 0x00077344
	protected void OverrideSettings(bool overrideAppPath = true)
	{
		if (overrideAppPath)
		{
			SettingsManager.APP_PATH = Directory.GetParent(Application.dataPath).FullName;
			SettingsManager.RegeneratePaths();
		}
		SettingsManager.VERSIONS_FILE_DOWNLOAD_URL = this.VersionsFileDownloadURL;
		SettingsManager.PATCHES_DOWNLOAD_URL = this.PatchesDirectoryURL;
		SettingsManager.BUILDS_DOWNLOAD_URL = this.BuildsDirectoryURL;
		SettingsManager.PATCH_DOWNLOAD_RETRY_ATTEMPTS = this.DownloadRetryAttempts;
		SettingsManager.LAUNCH_APP = SettingsManager.APP_PATH + Path.DirectorySeparatorChar + this.AppToLaunch;
		SettingsManager.LAUNCHER_NAME = this.LauncherName;
		SettingsManager.LAUNCH_ARG = this.Argument;
		SettingsManager.USE_RAW_LAUNCH_ARG = this.UseRawArgument;
		SettingsManager.ENABLE_FTP = this.EnableCredentials;
		SettingsManager.FTP_USERNAME = this.Username;
		SettingsManager.FTP_PASSWORD = this.Password;
		SettingsManager.INSTALL_IN_LOCAL_PATH = this.InstallInLocalPath;
		SettingsManager.PROGRAM_FILES_DIRECTORY_TO_INSTALL = this.ProgramFilesDirectoryToInstall;
		SettingsManager.PROGRAM_FILES_DIRECTORY_TO_INSTALL_ABS_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), this.ProgramFilesDirectoryToInstall);
		SettingsManager.PATCH_VERSION_PATH = SettingsManager.APP_PATH + Path.DirectorySeparatorChar + "version";
		SettingsManager.VERSION_FILE_LOCAL_PATH = SettingsManager.APP_PATH + Path.DirectorySeparatorChar + "version";
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x00079066 File Offset: 0x00077466
	protected void LocalizeGUI()
	{
		this.LaunchButton.GetComponentInChildren<Text>().text = Singleton<Localizatron>.Instance.Translate("LAUNCH");
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x00079087 File Offset: 0x00077487
	protected void EnsureExecutePrivileges()
	{
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x00079089 File Offset: 0x00077489
	[CompilerGenerated]
	private void <OnIncreaseMainProgressBar>m__0()
	{
		this.MainBar.PerformStep();
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x00079096 File Offset: 0x00077496
	[CompilerGenerated]
	private void <OnIncreaseDetailProgressBar>m__1()
	{
		this.DetailBar.PerformStep();
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x000790A3 File Offset: 0x000774A3
	[CompilerGenerated]
	private void <OnDownloadCompleted>m__2()
	{
		this.DetailBar.SetProgressText(string.Empty);
	}

	// Token: 0x040011CE RID: 4558
	private LauncherManager m_launcher;

	// Token: 0x040011CF RID: 4559
	private InstallManager m_installer;

	// Token: 0x040011D0 RID: 4560
	[Header("Patcher & Patches")]
	[Tooltip("If enabled your launcher will provide to check and apply patches to your current build.")]
	public bool ActivatePatcher = true;

	// Token: 0x040011D1 RID: 4561
	[Tooltip("Your versions.txt file remote URL")]
	public string VersionsFileDownloadURL = "http://your/url/to/versions.txt";

	// Token: 0x040011D2 RID: 4562
	[Tooltip("Your patches directory remote URL")]
	public string PatchesDirectoryURL = "http://your/url/to/patches/directory/";

	// Token: 0x040011D3 RID: 4563
	[Tooltip("Your game name! This string will be attached to app root path to launch your game when patching process will end!")]
	public string AppToLaunch = "Build.exe";

	// Token: 0x040011D4 RID: 4564
	[Tooltip("Determines if your launcher will be closed after your game starts!")]
	public bool CloseLauncherOnStart = true;

	// Token: 0x040011D5 RID: 4565
	[Tooltip("This argument will be attached to your game running command!")]
	public string Argument = "default";

	// Token: 0x040011D6 RID: 4566
	[Tooltip("If enabled your argument will be sent as raw text, if not your argument will be sent as \"YourGame.exe --LaunchArgs=YourArgument\"")]
	public bool UseRawArgument;

	// Token: 0x040011D7 RID: 4567
	[Tooltip("If enabled your patcher can be included in your Unity game build")]
	public bool IsIntegrated;

	// Token: 0x040011D8 RID: 4568
	[Tooltip("If IsIntegrated is true, your patcher will load this scene after patch process")]
	public int SceneToLoad = 1;

	// Token: 0x040011D9 RID: 4569
	[Space(10f)]
	[Header("Installer & Repairer")]
	[Tooltip("If enabled your launcher will install your build files before patches checking.")]
	public bool ActivateInstaller = true;

	// Token: 0x040011DA RID: 4570
	[Tooltip("If enabled your launcher will start to check files integrity before patches checking. It is useful to fix files corruption of your users' builds!")]
	public bool ActivateRepairer = true;

	// Token: 0x040011DB RID: 4571
	[Tooltip("Your builds directory remote URL")]
	public string BuildsDirectoryURL = "http://your/url/to/builds/directory/";

	// Token: 0x040011DC RID: 4572
	[Tooltip("Your launcher name!")]
	public string LauncherName = "PATCH.exe";

	// Token: 0x040011DD RID: 4573
	[Tooltip("If enabled your installer will install locally your game, if not your installer will install your game in Program Files/ProgramFilesDirectoryToInstall directory")]
	public bool InstallInLocalPath;

	// Token: 0x040011DE RID: 4574
	[Tooltip("If your installer have to install your game under Program Files folder, this will be the name of your game directory!")]
	public string ProgramFilesDirectoryToInstall = "MHLab";

	// Token: 0x040011DF RID: 4575
	[Tooltip("If enabled your installer will create a shortcut to your patcher on desktop")]
	public bool CreateDesktopShortcut = true;

	// Token: 0x040011E0 RID: 4576
	[Space(10f)]
	[Header("Common settings")]
	[Tooltip("How many times downloader can retry to download a file, if an error occurs?")]
	public ushort DownloadRetryAttempts;

	// Token: 0x040011E1 RID: 4577
	[Tooltip("Enables WebRequests or FTPRequests with credentials. Generally, you need this when your remote directories are proteted by login or your remote URLs are FTP ones!")]
	public bool EnableCredentials;

	// Token: 0x040011E2 RID: 4578
	[Tooltip("Username for your requests")]
	public string Username = "YourUsernameHere";

	// Token: 0x040011E3 RID: 4579
	[Tooltip("Password for your requests")]
	public string Password = "YourPasswordHere";

	// Token: 0x040011E4 RID: 4580
	[Space(10f)]
	[Header("GUI Components")]
	public ProgressBar MainBar;

	// Token: 0x040011E5 RID: 4581
	public ProgressBar DetailBar;

	// Token: 0x040011E6 RID: 4582
	public Text MainLog;

	// Token: 0x040011E7 RID: 4583
	public Text DetailLog;

	// Token: 0x040011E8 RID: 4584
	public Button LaunchButton;

	// Token: 0x040011E9 RID: 4585
	public RectTransform Overlay;

	// Token: 0x040011EA RID: 4586
	public RectTransform MainMenu;

	// Token: 0x040011EB RID: 4587
	public RectTransform RestartMenu;

	// Token: 0x040011EC RID: 4588
	public RectTransform OptionsMenu;

	// Token: 0x040011ED RID: 4589
	private ActionThread m_updateCheckingThread;

	// Token: 0x040011EE RID: 4590
	private DateTime _lastTime = DateTime.UtcNow;

	// Token: 0x040011EF RID: 4591
	private long _lastSize;

	// Token: 0x040011F0 RID: 4592
	private int _downloadSpeed;

	// Token: 0x0200035E RID: 862
	private enum LauncherStatus
	{
		// Token: 0x040011F2 RID: 4594
		IDLE,
		// Token: 0x040011F3 RID: 4595
		IS_BUSY
	}

	// Token: 0x02000F2F RID: 3887
	[CompilerGenerated]
	private sealed class <OnSetMainProgressBar>c__AnonStorey0
	{
		// Token: 0x060072F8 RID: 29432 RVA: 0x000790B5 File Offset: 0x000774B5
		public <OnSetMainProgressBar>c__AnonStorey0()
		{
		}

		// Token: 0x060072F9 RID: 29433 RVA: 0x000790C0 File Offset: 0x000774C0
		internal void <>m__0()
		{
			this.$this.MainBar.Clear();
			this.$this.MainBar.Maximum = (float)this.max;
			this.$this.MainBar.Minimum = (float)this.min;
			this.$this.MainBar.Step = 1f;
		}

		// Token: 0x040066CE RID: 26318
		internal int max;

		// Token: 0x040066CF RID: 26319
		internal int min;

		// Token: 0x040066D0 RID: 26320
		internal Launcher $this;
	}

	// Token: 0x02000F30 RID: 3888
	[CompilerGenerated]
	private sealed class <OnSetDetailProgressBar>c__AnonStorey1
	{
		// Token: 0x060072FA RID: 29434 RVA: 0x00079120 File Offset: 0x00077520
		public <OnSetDetailProgressBar>c__AnonStorey1()
		{
		}

		// Token: 0x060072FB RID: 29435 RVA: 0x00079128 File Offset: 0x00077528
		internal void <>m__0()
		{
			this.$this.DetailBar.Clear();
			this.$this.DetailBar.Maximum = (float)this.max;
			this.$this.DetailBar.Minimum = (float)this.min;
			this.$this.DetailBar.Step = 1f;
		}

		// Token: 0x040066D1 RID: 26321
		internal int max;

		// Token: 0x040066D2 RID: 26322
		internal int min;

		// Token: 0x040066D3 RID: 26323
		internal Launcher $this;
	}

	// Token: 0x02000F31 RID: 3889
	[CompilerGenerated]
	private sealed class <OnLog>c__AnonStorey2
	{
		// Token: 0x060072FC RID: 29436 RVA: 0x00079188 File Offset: 0x00077588
		public <OnLog>c__AnonStorey2()
		{
		}

		// Token: 0x060072FD RID: 29437 RVA: 0x00079190 File Offset: 0x00077590
		internal void <>m__0()
		{
			this.$this.MainLog.text = Singleton<Localizatron>.Instance.Translate(this.main);
			this.$this.DetailLog.text = Singleton<Localizatron>.Instance.Translate(this.detail);
		}

		// Token: 0x040066D4 RID: 26324
		internal string main;

		// Token: 0x040066D5 RID: 26325
		internal string detail;

		// Token: 0x040066D6 RID: 26326
		internal Launcher $this;
	}

	// Token: 0x02000F32 RID: 3890
	[CompilerGenerated]
	private sealed class <OnError>c__AnonStorey3
	{
		// Token: 0x060072FE RID: 29438 RVA: 0x000791DD File Offset: 0x000775DD
		public <OnError>c__AnonStorey3()
		{
		}

		// Token: 0x060072FF RID: 29439 RVA: 0x000791E8 File Offset: 0x000775E8
		internal void <>m__0()
		{
			this.$this.MainLog.text = Singleton<Localizatron>.Instance.Translate(this.main);
			this.$this.DetailLog.text = Singleton<Localizatron>.Instance.Translate(this.detail);
		}

		// Token: 0x040066D7 RID: 26327
		internal string main;

		// Token: 0x040066D8 RID: 26328
		internal string detail;

		// Token: 0x040066D9 RID: 26329
		internal Launcher $this;
	}

	// Token: 0x02000F33 RID: 3891
	[CompilerGenerated]
	private sealed class <OnFatalError>c__AnonStorey4
	{
		// Token: 0x06007300 RID: 29440 RVA: 0x00079235 File Offset: 0x00077635
		public <OnFatalError>c__AnonStorey4()
		{
		}

		// Token: 0x06007301 RID: 29441 RVA: 0x00079240 File Offset: 0x00077640
		internal void <>m__0()
		{
			this.$this.MainLog.text = Singleton<Localizatron>.Instance.Translate(this.main);
			this.$this.DetailLog.text = Singleton<Localizatron>.Instance.Translate(this.detail);
		}

		// Token: 0x040066DA RID: 26330
		internal string main;

		// Token: 0x040066DB RID: 26331
		internal string detail;

		// Token: 0x040066DC RID: 26332
		internal Launcher $this;
	}

	// Token: 0x02000F34 RID: 3892
	[CompilerGenerated]
	private sealed class <OnTaskStarted>c__AnonStorey5
	{
		// Token: 0x06007302 RID: 29442 RVA: 0x0007928D File Offset: 0x0007768D
		public <OnTaskStarted>c__AnonStorey5()
		{
		}

		// Token: 0x06007303 RID: 29443 RVA: 0x00079298 File Offset: 0x00077698
		internal void <>m__0()
		{
			this.$this.LaunchButton.interactable = false;
			this.$this.MainLog.text = Singleton<Localizatron>.Instance.Translate(this.message);
			this.$this.DetailLog.text = string.Empty;
		}

		// Token: 0x040066DD RID: 26333
		internal string message;

		// Token: 0x040066DE RID: 26334
		internal Launcher $this;
	}

	// Token: 0x02000F35 RID: 3893
	[CompilerGenerated]
	private sealed class <OnTaskCompleted>c__AnonStorey6
	{
		// Token: 0x06007304 RID: 29444 RVA: 0x000792EB File Offset: 0x000776EB
		public <OnTaskCompleted>c__AnonStorey6()
		{
		}

		// Token: 0x06007305 RID: 29445 RVA: 0x000792F4 File Offset: 0x000776F4
		internal void <>m__0()
		{
			this.$this.MainLog.text = Singleton<Localizatron>.Instance.Translate(this.message);
			this.$this.DetailLog.text = string.Empty;
			this.$this.LaunchButton.interactable = true;
			if (this.$this.IsIntegrated)
			{
				if (this.$this.Overlay != null)
				{
					this.$this.Overlay.gameObject.SetActive(true);
				}
				if (this.$this.m_launcher.IsDirty())
				{
					this.$this.RestartMenu.gameObject.SetActive(true);
				}
				else
				{
					this.$this.MainMenu.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x040066DF RID: 26335
		internal string message;

		// Token: 0x040066E0 RID: 26336
		internal Launcher $this;
	}

	// Token: 0x02000F36 RID: 3894
	[CompilerGenerated]
	private sealed class <OnDownloadProgress>c__AnonStorey7
	{
		// Token: 0x06007306 RID: 29446 RVA: 0x000793C9 File Offset: 0x000777C9
		public <OnDownloadProgress>c__AnonStorey7()
		{
		}

		// Token: 0x06007307 RID: 29447 RVA: 0x000793D4 File Offset: 0x000777D4
		internal void <>m__0()
		{
			this.$this.DetailBar.Progress = (float)this.percentageCompleted;
			this.$this.DetailBar.SetProgressText(string.Concat(new object[]
			{
				this.percentageCompleted,
				"% - (",
				Utility.FormatSizeBinary(this.currentFileSize, 2),
				"/",
				Utility.FormatSizeBinary(this.totalFileSize, 2),
				") @ ",
				Utility.FormatSizeBinary((long)this.$this._downloadSpeed, 2),
				"/s"
			}));
		}

		// Token: 0x040066E1 RID: 26337
		internal int percentageCompleted;

		// Token: 0x040066E2 RID: 26338
		internal long currentFileSize;

		// Token: 0x040066E3 RID: 26339
		internal long totalFileSize;

		// Token: 0x040066E4 RID: 26340
		internal Launcher $this;
	}
}
