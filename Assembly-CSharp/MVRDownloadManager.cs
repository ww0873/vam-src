using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using MVR.FileManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x02000C31 RID: 3121
public class MVRDownloadManager : MonoBehaviour
{
	// Token: 0x06005AB5 RID: 23221 RVA: 0x002141CC File Offset: 0x002125CC
	public MVRDownloadManager()
	{
	}

	// Token: 0x06005AB6 RID: 23222 RVA: 0x0021420C File Offset: 0x0021260C
	public void Awake()
	{
		if (this.manageAllBrowsers)
		{
			foreach (Browser browser in UnityEngine.Object.FindObjectsOfType<Browser>())
			{
				this.ManageDownloads(browser);
			}
		}
		if (this.extractOnDownloadToggle)
		{
			this.extractOnDownloadToggle.isOn = this.extractOnDownload;
			this.extractOnDownloadToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<Awake>m__0));
		}
	}

	// Token: 0x06005AB7 RID: 23223 RVA: 0x00214288 File Offset: 0x00212688
	public void ManageDownloads(Browser browser)
	{
		MVRDownloadManager.<ManageDownloads>c__AnonStorey0 <ManageDownloads>c__AnonStorey = new MVRDownloadManager.<ManageDownloads>c__AnonStorey0();
		<ManageDownloads>c__AnonStorey.browser = browser;
		<ManageDownloads>c__AnonStorey.$this = this;
		<ManageDownloads>c__AnonStorey.browser.onDownloadStarted = new Action<int, JSONNode>(<ManageDownloads>c__AnonStorey.<>m__0);
		<ManageDownloads>c__AnonStorey.browser.onDownloadStatus += <ManageDownloads>c__AnonStorey.<>m__1;
	}

	// Token: 0x06005AB8 RID: 23224 RVA: 0x002142D8 File Offset: 0x002126D8
	private void HandleDownloadStarted(Browser browser, int downloadId, JSONNode info)
	{
		Debug.Log("Download requested: " + info.AsJSON);
		MVRDownloadManager.Download item = new MVRDownloadManager.Download
		{
			browser = browser,
			downloadId = downloadId,
			name = info["suggestedName"]
		};
		if (this.promptForFileNames)
		{
			browser.DownloadCommand(downloadId, BrowserNative.DownloadAction.Begin, null);
		}
		else
		{
			DirectoryInfo directoryInfo;
			if (string.IsNullOrEmpty(this.saveFolder))
			{
				directoryInfo = new DirectoryInfo(MVRDownloadManager.GetUserDownloadFolder());
			}
			else
			{
				directoryInfo = new DirectoryInfo(this.saveFolder);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
			}
			string text = directoryInfo.FullName + "/" + new FileInfo(info["suggestedName"]).Name;
			if (!this.overwrite)
			{
				while (File.Exists(text))
				{
					string extension = Path.GetExtension(text);
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
					string text2 = DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss");
					text = string.Concat(new string[]
					{
						directoryInfo.FullName,
						"/",
						fileNameWithoutExtension,
						" ",
						text2,
						extension
					});
				}
			}
			browser.DownloadCommand(downloadId, BrowserNative.DownloadAction.Begin, text);
		}
		this.downloads.Add(item);
	}

	// Token: 0x06005AB9 RID: 23225 RVA: 0x00214430 File Offset: 0x00212830
	private void HandleDownloadStatus(Browser browser, int downloadId, JSONNode info)
	{
		Debug.Log("Download status: " + info.AsJSON);
		for (int i = 0; i < this.downloads.Count; i++)
		{
			if (!(this.downloads[i].browser != browser) && this.downloads[i].downloadId == downloadId)
			{
				MVRDownloadManager.Download download = this.downloads[i];
				download.status = info["status"];
				download.speed = info["speed"];
				download.percent = info["percentComplete"];
				if (!string.IsNullOrEmpty(info["fullPath"]))
				{
					download.name = Path.GetFileName(info["fullPath"]);
				}
				if (download.status == "complete" && this.extractOnDownload && SuperController.singleton != null)
				{
					string text = info["fullPath"];
					if (text.EndsWith(".zip") || text.EndsWith(".vac") || (text.EndsWith(".json") && this.autoOpenPackageScene))
					{
						SuperController.singleton.Load(text);
					}
					else if (text.EndsWith(".var"))
					{
						string fileName = Path.GetFileName(text);
						FileManager.MoveFile(text, "AddonPackages/" + fileName, true);
						FileManager.Refresh();
						this._status = "Package " + fileName + " downloaded. Content is ready to be used";
						if (this.autoOpenPackageScene)
						{
							string packageUidOrPath = fileName.Replace(".var", string.Empty);
							VarPackage package = FileManager.GetPackage(packageUidOrPath);
							if (package != null)
							{
								List<FileEntry> list = new List<FileEntry>();
								package.FindFiles("Saves/scene", "*.json", list);
								if (list.Count > 0)
								{
									FileEntry fileEntry = list[0];
									SuperController.singleton.Load(fileEntry.Uid);
								}
							}
						}
					}
				}
				break;
			}
		}
	}

	// Token: 0x06005ABA RID: 23226 RVA: 0x0021466E File Offset: 0x00212A6E
	public void Update()
	{
		if (this.statusBar)
		{
			this.statusBar.text = this.Status;
		}
	}

	// Token: 0x06005ABB RID: 23227 RVA: 0x00214694 File Offset: 0x00212A94
	public void PauseAll()
	{
		for (int i = 0; i < this.downloads.Count; i++)
		{
			if (this.downloads[i].status == "working")
			{
				this.downloads[i].browser.DownloadCommand(this.downloads[i].downloadId, BrowserNative.DownloadAction.Pause, null);
			}
		}
	}

	// Token: 0x06005ABC RID: 23228 RVA: 0x00214708 File Offset: 0x00212B08
	public void ResumeAll()
	{
		for (int i = 0; i < this.downloads.Count; i++)
		{
			if (this.downloads[i].status == "working")
			{
				this.downloads[i].browser.DownloadCommand(this.downloads[i].downloadId, BrowserNative.DownloadAction.Resume, null);
			}
		}
	}

	// Token: 0x06005ABD RID: 23229 RVA: 0x0021477C File Offset: 0x00212B7C
	public void CancelAll()
	{
		for (int i = 0; i < this.downloads.Count; i++)
		{
			if (this.downloads[i].status == "working")
			{
				this.downloads[i].browser.DownloadCommand(this.downloads[i].downloadId, BrowserNative.DownloadAction.Cancel, null);
			}
		}
	}

	// Token: 0x06005ABE RID: 23230 RVA: 0x002147EE File Offset: 0x00212BEE
	public void ClearAll()
	{
		this.CancelAll();
		this.downloads.Clear();
	}

	// Token: 0x17000D65 RID: 3429
	// (get) Token: 0x06005ABF RID: 23231 RVA: 0x00214804 File Offset: 0x00212C04
	public string Status
	{
		get
		{
			if (this.downloads.Count == 0)
			{
				return this._status;
			}
			this.sb.Length = 0;
			int num = 0;
			bool flag = false;
			for (int i = this.downloads.Count - 1; i >= 0; i--)
			{
				if (this.downloads[i].status != "complete")
				{
					flag = true;
					if (this.sb.Length > 0)
					{
						this.sb.Append(", ");
					}
					this.sb.Append(this.downloads[i].name);
					if (this.downloads[i].status == "working")
					{
						if (this.downloads[i].percent >= 0)
						{
							this.sb.Append(" (").Append(this.downloads[i].percent).Append("%)");
						}
						else
						{
							this.sb.Append(" (??%)");
						}
						num += this.downloads[i].speed;
					}
					else
					{
						this.sb.Append(" (").Append(this.downloads[i].status).Append(")");
					}
				}
			}
			if (flag)
			{
				string text = "Downloads";
				if (num > 0)
				{
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						" (",
						Mathf.Round((float)num / 1048576f * 100f) / 100f,
						"MiB/s)"
					});
				}
				this._status = text + ": " + this.sb.ToString();
			}
			return this._status;
		}
	}

	// Token: 0x06005AC0 RID: 23232 RVA: 0x002149FC File Offset: 0x00212DFC
	public static string GetUserDownloadFolder()
	{
		switch (Environment.OSVersion.Platform)
		{
		case PlatformID.Win32NT:
		{
			IntPtr ptr;
			if (MVRDownloadManager.SHGetKnownFolderPath(new Guid("{374DE290-123F-4565-9164-39C4925E467B}"), 32768U, IntPtr.Zero, out ptr) == 0)
			{
				string result = Marshal.PtrToStringUni(ptr);
				Marshal.FreeCoTaskMem(ptr);
				return result;
			}
			throw new Exception("Failed to get user download directory", new Win32Exception(Marshal.GetLastWin32Error()));
		}
		case PlatformID.Unix:
		{
			string text = Environment.GetEnvironmentVariable("HOME") + "/Downloads";
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			return text;
		}
		case PlatformID.MacOSX:
			throw new NotImplementedException();
		}
		throw new NotImplementedException();
	}

	// Token: 0x06005AC1 RID: 23233
	[DllImport("Shell32.dll")]
	private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr ppszPath);

	// Token: 0x06005AC2 RID: 23234 RVA: 0x00214ABB File Offset: 0x00212EBB
	[CompilerGenerated]
	private void <Awake>m__0(bool A_1)
	{
		this.extractOnDownload = this.extractOnDownloadToggle.isOn;
	}

	// Token: 0x04004AE1 RID: 19169
	[Tooltip("If true, this will find all the browser in the scene at startup and take control of their downloads.")]
	public bool manageAllBrowsers;

	// Token: 0x04004AE2 RID: 19170
	[Tooltip("If true, a \"Save as\" style dialog will be given for all downloads.")]
	public bool promptForFileNames;

	// Token: 0x04004AE3 RID: 19171
	[Tooltip("Where to save files. If null or blank, defaults to the user's downloads directory.")]
	public string saveFolder;

	// Token: 0x04004AE4 RID: 19172
	[Tooltip("Automatically open scene on download of a var package if it has one")]
	public bool autoOpenPackageScene = true;

	// Token: 0x04004AE5 RID: 19173
	[Tooltip("If given this text element will be updated with download status info.")]
	public Text statusBar;

	// Token: 0x04004AE6 RID: 19174
	public bool extractOnDownload = true;

	// Token: 0x04004AE7 RID: 19175
	public Toggle extractOnDownloadToggle;

	// Token: 0x04004AE8 RID: 19176
	public bool overwrite = true;

	// Token: 0x04004AE9 RID: 19177
	public List<MVRDownloadManager.Download> downloads = new List<MVRDownloadManager.Download>();

	// Token: 0x04004AEA RID: 19178
	private StringBuilder sb = new StringBuilder();

	// Token: 0x04004AEB RID: 19179
	private string _status = string.Empty;

	// Token: 0x02000C32 RID: 3122
	public class Download
	{
		// Token: 0x06005AC3 RID: 23235 RVA: 0x00214ACE File Offset: 0x00212ECE
		public Download()
		{
		}

		// Token: 0x04004AEC RID: 19180
		public Browser browser;

		// Token: 0x04004AED RID: 19181
		public int downloadId;

		// Token: 0x04004AEE RID: 19182
		public string name;

		// Token: 0x04004AEF RID: 19183
		public string path;

		// Token: 0x04004AF0 RID: 19184
		public int speed;

		// Token: 0x04004AF1 RID: 19185
		public int percent;

		// Token: 0x04004AF2 RID: 19186
		public string status;
	}

	// Token: 0x02001003 RID: 4099
	[CompilerGenerated]
	private sealed class <ManageDownloads>c__AnonStorey0
	{
		// Token: 0x0600767B RID: 30331 RVA: 0x00214AD6 File Offset: 0x00212ED6
		public <ManageDownloads>c__AnonStorey0()
		{
		}

		// Token: 0x0600767C RID: 30332 RVA: 0x00214ADE File Offset: 0x00212EDE
		internal void <>m__0(int id, JSONNode info)
		{
			this.$this.HandleDownloadStarted(this.browser, id, info);
		}

		// Token: 0x0600767D RID: 30333 RVA: 0x00214AF3 File Offset: 0x00212EF3
		internal void <>m__1(int id, JSONNode info)
		{
			this.$this.HandleDownloadStatus(this.browser, id, info);
		}

		// Token: 0x04006A4E RID: 27214
		internal Browser browser;

		// Token: 0x04006A4F RID: 27215
		internal MVRDownloadManager $this;
	}
}
