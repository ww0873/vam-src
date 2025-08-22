using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using HelloMeow;
using ICSharpCode.SharpZipLib.Core;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using SimpleJSON;
using uFileBrowser;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B89 RID: 2953
public class URLAudioClipManager : AudioClipManager
{
	// Token: 0x06005329 RID: 21289 RVA: 0x001E0F3A File Offset: 0x001DF33A
	public URLAudioClipManager()
	{
	}

	// Token: 0x0600532A RID: 21290 RVA: 0x001E0F56 File Offset: 0x001DF356
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x0600532B RID: 21291 RVA: 0x001E0F60 File Offset: 0x001DF360
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if ((includePhysical || forceStore) && this.clips.Count > 0)
		{
			this.needsStore = true;
			JSONArray jsonarray = new JSONArray();
			foreach (NamedAudioClip namedAudioClip in this.clips)
			{
				URLAudioClip urlaudioClip = (URLAudioClip)namedAudioClip;
				JSONClass jsonclass = new JSONClass();
				if (Regex.IsMatch(urlaudioClip.url, "^http"))
				{
					jsonclass["url"] = urlaudioClip.url;
				}
				else if (SuperController.singleton != null)
				{
					jsonclass["url"] = SuperController.singleton.NormalizeSavePath(urlaudioClip.url);
				}
				else
				{
					jsonclass["url"] = urlaudioClip.url;
				}
				jsonclass["displayName"] = urlaudioClip.displayName;
				jsonarray.Add(jsonclass);
			}
			json["clips"] = jsonarray;
		}
		return json;
	}

	// Token: 0x0600532C RID: 21292 RVA: 0x001E10A0 File Offset: 0x001DF4A0
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("clips"))
		{
			if (jc["clips"] != null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				JSONArray asArray = jc["clips"].AsArray;
				IEnumerator enumerator = asArray.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						JSONNode jsonnode = (JSONNode)obj;
						JSONClass asObject = jsonnode.AsObject;
						if (asObject != null && asObject["url"] != null)
						{
							string text = asObject["url"];
							if (asObject["displayName"] != null)
							{
								string value = asObject["displayName"];
								if (SuperController.singleton != null)
								{
									text = SuperController.singleton.NormalizeLoadPath(text);
								}
								if (!dictionary.ContainsKey(text))
								{
									dictionary.Add(text, value);
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				List<NamedAudioClip> list = new List<NamedAudioClip>();
				foreach (NamedAudioClip namedAudioClip in this.clips)
				{
					URLAudioClip urlaudioClip = namedAudioClip as URLAudioClip;
					if (dictionary.ContainsKey(urlaudioClip.url))
					{
						dictionary.Remove(urlaudioClip.url);
					}
					else
					{
						list.Add(namedAudioClip);
					}
				}
				if (!base.mergeRestore)
				{
					foreach (NamedAudioClip nac in list)
					{
						this.RemoveClip(nac, false);
					}
				}
				foreach (KeyValuePair<string, string> keyValuePair in dictionary)
				{
					this.QueueClip(keyValuePair.Key, keyValuePair.Value, true);
				}
			}
			else if (setMissingToDefault)
			{
				this.RemoveAllClips();
			}
		}
	}

	// Token: 0x0600532D RID: 21293 RVA: 0x001E1338 File Offset: 0x001DF738
	protected void LoadFileIntoByteArray(FileEntry fe, out IntPtr byteArray)
	{
		byte[] buffer = new byte[32768];
		using (FileEntryStream fileEntryStream = FileManager.OpenStream(fe))
		{
			byte[] array = new byte[fe.Size];
			using (MemoryStream memoryStream = new MemoryStream(array))
			{
				StreamUtils.Copy(fileEntryStream.Stream, memoryStream, buffer);
				byteArray = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, byteArray, array.Length);
			}
		}
	}

	// Token: 0x0600532E RID: 21294 RVA: 0x001E13D0 File Offset: 0x001DF7D0
	private IEnumerator ProcessQueue()
	{
		for (;;)
		{
			if (this.urlqueue.Count > 0)
			{
				URLAudioClipManager.<ProcessQueue>c__Iterator0.<ProcessQueue>c__AnonStorey1 <ProcessQueue>c__AnonStorey = new URLAudioClipManager.<ProcessQueue>c__Iterator0.<ProcessQueue>c__AnonStorey1();
				<ProcessQueue>c__AnonStorey.<>f__ref$0 = this;
				URLAudioClip uac = this.urlqueue.Dequeue();
				AsyncFlag af = new AsyncFlag("URL Audio: " + uac.displayName);
				if (uac.fromRestore)
				{
					SuperController.singleton.ResetSimulation(af, false);
				}
				AsyncFlag iconFlag = new AsyncFlag("URL Audio");
				SuperController.singleton.SetLoadingIconFlag(iconFlag);
				string url = uac.url;
				<ProcessQueue>c__AnonStorey.fe = FileManager.GetFileEntry(url, false);
				bool isPackageFile = false;
				if (<ProcessQueue>c__AnonStorey.fe != null && <ProcessQueue>c__AnonStorey.fe is VarFileEntry)
				{
					isPackageFile = true;
				}
				if (url.EndsWith(".mp3") || url.EndsWith(".wav") || url.EndsWith(".ogg"))
				{
					BassImporter bi = base.GetComponent<BassImporter>();
					if (bi != null)
					{
						if (isPackageFile)
						{
							URLAudioClipManager.<ProcessQueue>c__Iterator0.<ProcessQueue>c__AnonStorey2 <ProcessQueue>c__AnonStorey2 = new URLAudioClipManager.<ProcessQueue>c__Iterator0.<ProcessQueue>c__AnonStorey2();
							<ProcessQueue>c__AnonStorey2.<>f__ref$0 = this;
							<ProcessQueue>c__AnonStorey2.<>f__ref$1 = <ProcessQueue>c__AnonStorey;
							bi.Prep();
							<ProcessQueue>c__AnonStorey2.byteArray = 0;
							Thread loadThread = new Thread(new ThreadStart(<ProcessQueue>c__AnonStorey2.<>m__0));
							loadThread.Start();
							while (loadThread.IsAlive)
							{
								yield return null;
							}
							yield return bi.SetData(<ProcessQueue>c__AnonStorey2.byteArray, <ProcessQueue>c__AnonStorey.fe.Size);
						}
						else
						{
							if (Regex.IsMatch(url, "^file:///"))
							{
								url = url.Replace("file:///", "file://");
							}
							if (!Regex.IsMatch(url, "^http") && !Regex.IsMatch(url, "^file"))
							{
								if (url.Contains(":/"))
								{
									url = "file://" + url;
								}
								else
								{
									url = "file://./" + url;
								}
							}
							bi.Import(url);
							while (!bi.isLoaded && !bi.isError)
							{
								if (uac.removed)
								{
									break;
								}
								if (uac.loadProgressSlider != null)
								{
									uac.loadProgressSlider.value = bi.progress;
								}
								if (uac.sizeText != null)
								{
									uac.sizeText.text = string.Empty;
								}
								yield return null;
							}
						}
						if (!uac.removed)
						{
							if (!bi.isError)
							{
								uac.sourceClip = bi.audioClip;
								if (uac.loadProgressSlider != null)
								{
									uac.loadProgressSlider.value = 1f;
								}
								if (uac.sourceClip != null)
								{
									uac.ready = true;
								}
								else if (!uac.error)
								{
									uac.errorMsg = "Unable to extract audio from url";
									uac.error = true;
									SuperController.LogError("Unable to extract audio from url " + url);
								}
							}
							else
							{
								uac.errorMsg = bi.error;
								uac.error = true;
								SuperController.LogError("Error during mp3/wav import " + bi.error);
							}
						}
					}
					else
					{
						uac.errorMsg = "MP3/WAV importer not defined";
						uac.error = true;
						SuperController.LogError("MP3/WAV importer not defined. Cannot import mp3/wav");
					}
				}
				else if (!isPackageFile)
				{
					if (!Regex.IsMatch(url, "^http") && !Regex.IsMatch(url, "^file"))
					{
						if (url.Contains(":/"))
						{
							url = "file:///" + url;
						}
						else
						{
							url = "file:///.\\" + url;
						}
					}
					WWW www = new WWW(url);
					while (!www.isDone)
					{
						if (uac.removed)
						{
							www.Dispose();
							break;
						}
						if (uac.loadProgressSlider != null)
						{
							uac.loadProgressSlider.value = www.progress;
						}
						if (uac.sizeText != null)
						{
							float num = (float)www.bytesDownloaded / 1000000f;
							uac.sizeText.text = string.Format("{0:F1}MB", num);
						}
						yield return null;
					}
					if (!uac.removed)
					{
						if (www.error == null || www.error == string.Empty)
						{
							bool isMp3 = false;
							if (www.responseHeaders.Count > 0)
							{
								foreach (KeyValuePair<string, string> keyValuePair in www.responseHeaders)
								{
									if (keyValuePair.Key == "Content-Type")
									{
										string[] array = keyValuePair.Value.Split(new char[]
										{
											';'
										});
										foreach (string text in array)
										{
											if (text.EndsWith(".mp3\""))
											{
												isMp3 = true;
											}
										}
									}
									else if (keyValuePair.Key == "Content-Disposition")
									{
										string[] array3 = keyValuePair.Value.Split(new char[]
										{
											';'
										});
										foreach (string text2 in array3)
										{
											if (text2.EndsWith(".mp3"))
											{
												isMp3 = true;
											}
										}
									}
								}
							}
							float fsize = (float)www.bytesDownloaded / 1000000f;
							if (uac.sizeText != null)
							{
								uac.sizeText.text = string.Format("{0:F1}MB", fsize);
							}
							if (url.EndsWith(".mp3") || isMp3)
							{
								BassImporter bi2 = base.GetComponent<BassImporter>();
								if (bi2 != null)
								{
									yield return bi2.SetData(www.bytes);
									if (!uac.removed)
									{
										if (bi2.isError)
										{
											uac.errorMsg = bi2.error;
											uac.error = true;
											SuperController.LogError("Error during mp3 import " + bi2.error);
										}
										else
										{
											uac.sourceClip = bi2.audioClip;
										}
									}
								}
								else
								{
									uac.errorMsg = "MP3 importer not defined";
									uac.error = true;
									SuperController.LogError("MP3 importer not defined. Cannot import mp3");
								}
							}
							else
							{
								uac.sourceClip = www.GetAudioClip();
								if (uac.sourceClip == null)
								{
									try
									{
										uac.sourceClip = NAudioPlayer.AudioClipFromMp3Data(www.bytes);
									}
									catch (Exception ex)
									{
										uac.error = true;
										uac.errorMsg = "Could not extract audio data";
										SuperController.LogError("Could not extract audio data: " + ex.Message);
									}
								}
							}
							if (uac.loadProgressSlider != null)
							{
								uac.loadProgressSlider.value = 1f;
							}
							if (uac.sourceClip != null)
							{
								uac.ready = true;
							}
							else if (!uac.error)
							{
								uac.errorMsg = "Unable to extract audio from url";
								uac.error = true;
								SuperController.LogError("Unable to extract audio from url " + url);
							}
						}
						else
						{
							uac.error = true;
							uac.errorMsg = www.error;
							SuperController.LogError("Could not load audio source at " + url + " Error: " + www.error);
						}
					}
				}
				else
				{
					uac.error = true;
					uac.errorMsg = "Packages only support mp3, ogg, and wav files";
					SuperController.LogError(string.Concat(new object[]
					{
						"Could not load audio source at ",
						url,
						" Error: ",
						uac.error
					}));
				}
				af.Raise();
				iconFlag.Raise();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600532F RID: 21295 RVA: 0x001E13EB File Offset: 0x001DF7EB
	public override bool RemoveClip(NamedAudioClip nac)
	{
		return this.RemoveClip(nac, true);
	}

	// Token: 0x06005330 RID: 21296 RVA: 0x001E13F8 File Offset: 0x001DF7F8
	protected bool RemoveClip(NamedAudioClip nac, bool validate)
	{
		if (base.RemoveClip(nac))
		{
			URLAudioClip urlaudioClip = nac as URLAudioClip;
			if (nac.sourceClip != null)
			{
				UnityEngine.Object.Destroy(nac.sourceClip);
			}
			if (urlaudioClip.UIpanel != null)
			{
				if (this.clipContentManager != null)
				{
					this.clipContentManager.RemoveItem(urlaudioClip.UIpanel);
				}
				UnityEngine.Object.Destroy(urlaudioClip.UIpanel.gameObject);
			}
			if (validate && SuperController.singleton != null)
			{
				SuperController.singleton.ValidateAllAtoms();
			}
			return true;
		}
		return false;
	}

	// Token: 0x06005331 RID: 21297 RVA: 0x001E14A0 File Offset: 0x001DF8A0
	public override void RemoveAllClips()
	{
		foreach (NamedAudioClip namedAudioClip in this.clips)
		{
			URLAudioClip urlaudioClip = namedAudioClip as URLAudioClip;
			if (urlaudioClip.UIpanel != null)
			{
				if (this.clipContentManager != null)
				{
					this.clipContentManager.RemoveItem(urlaudioClip.UIpanel);
				}
				UnityEngine.Object.Destroy(urlaudioClip.UIpanel.gameObject);
			}
		}
		base.RemoveAllClips();
		if (SuperController.singleton != null)
		{
			SuperController.singleton.ValidateAllAtoms();
		}
	}

	// Token: 0x06005332 RID: 21298 RVA: 0x001E1560 File Offset: 0x001DF960
	protected string URLToUid(string url)
	{
		FileEntry fileEntry = FileManager.GetFileEntry(url, false);
		if (fileEntry != null && fileEntry is VarFileEntry)
		{
			VarFileEntry varFileEntry = fileEntry as VarFileEntry;
			string internalSlashPath = varFileEntry.InternalSlashPath;
			return Regex.Replace(internalSlashPath, ".*/", string.Empty);
		}
		if (Regex.IsMatch(url, "^http"))
		{
			return url;
		}
		string text = Regex.Replace(url, "\\\\", "/");
		return Regex.Replace(url, ".*/", string.Empty);
	}

	// Token: 0x06005333 RID: 21299 RVA: 0x001E15DC File Offset: 0x001DF9DC
	public URLAudioClip QueueClip(string url, string displayName = null, bool fromRestore = false)
	{
		URLAudioClip urlaudioClip = null;
		if (url != null && url != string.Empty)
		{
			bool flag = false;
			if (Regex.IsMatch(url, "^http"))
			{
				if (UserPreferences.singleton == null)
				{
					flag = true;
				}
				else if (UserPreferences.singleton.enableWebMisc)
				{
					if (UserPreferences.singleton.CheckWhitelistDomain(url))
					{
						flag = true;
					}
					else
					{
						SuperController.LogError("Attempted to load audio from URL " + url + " which is not on whitelist", true, !UserPreferences.singleton.hideDisabledWebMessages);
					}
				}
				else if (!UserPreferences.singleton.hideDisabledWebMessages)
				{
					SuperController.LogError("Attempted to load http URL audio when web load option is disabled. To enable, see User Preferences -> Web Security tab");
					SuperController.singleton.ShowMainHUDAuto();
					SuperController.singleton.SetActiveUI("MainMenu");
					SuperController.singleton.SetMainMenuTab("TabUserPrefs");
					SuperController.singleton.SetUserPrefsTab("TabSecurity");
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				string text = this.URLToUid(url);
				if (!this.uidToClip.ContainsKey(text))
				{
					urlaudioClip = new URLAudioClip();
					urlaudioClip.fromRestore = fromRestore;
					urlaudioClip.uid = text;
					urlaudioClip.url = url;
					urlaudioClip.category = "web";
					if (displayName == null)
					{
						urlaudioClip.displayName = Regex.Replace(urlaudioClip.uid, ".*/", string.Empty);
					}
					else
					{
						urlaudioClip.displayName = displayName;
					}
					base.AddClip(urlaudioClip);
					this.urlqueue.Enqueue(urlaudioClip);
					if (this.clipContentManager != null && this.clipPrefab != null)
					{
						RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.clipPrefab);
						this.clipContentManager.AddItem(rectTransform, -1, false);
						urlaudioClip.UIpanel = rectTransform;
						URLAudioClipUI component = rectTransform.GetComponent<URLAudioClipUI>();
						if (component != null)
						{
							urlaudioClip.removeButton = component.removeButton;
							urlaudioClip.testButton = component.testButton;
							urlaudioClip.testButtonText = component.testButtonText;
							urlaudioClip.uidText = component.urlText;
							urlaudioClip.sizeText = component.sizeText;
							urlaudioClip.displayNameField = component.displayNameField;
							urlaudioClip.readyToggle = component.readyToggle;
							urlaudioClip.loadProgressSlider = component.loadProgressSlider;
							urlaudioClip.InitUI();
						}
					}
				}
			}
		}
		return urlaudioClip;
	}

	// Token: 0x06005334 RID: 21300 RVA: 0x001E181C File Offset: 0x001DFC1C
	public void CopyURLToClipboard()
	{
		if (this.urlInputField != null)
		{
			GUIUtility.systemCopyBuffer = this.urlInputField.text;
		}
	}

	// Token: 0x06005335 RID: 21301 RVA: 0x001E183F File Offset: 0x001DFC3F
	public void CopyURLFromClipboard()
	{
		if (this.urlInputField != null)
		{
			this.urlInputField.text = GUIUtility.systemCopyBuffer;
			this.QueueClip(this.urlInputField.text, null, false);
		}
	}

	// Token: 0x06005336 RID: 21302 RVA: 0x001E1878 File Offset: 0x001DFC78
	public void QueueFilePath(string path)
	{
		if (path != null && path != string.Empty)
		{
			path = SuperController.singleton.NormalizeMediaPath(path);
			if (this.urlInputField != null)
			{
				this.urlInputField.text = path;
				this.QueueClip(path, null, false);
			}
		}
	}

	// Token: 0x06005337 RID: 21303 RVA: 0x001E18D0 File Offset: 0x001DFCD0
	public void FileBrowse()
	{
		if (SuperController.singleton != null)
		{
			List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory("Custom/Sounds", true, true, true, true);
			shortCutsForDirectory.Insert(0, new ShortCut
			{
				displayName = "Root",
				path = Path.GetFullPath(".")
			});
			SuperController.singleton.GetMediaPathDialog(new FileBrowserCallback(this.QueueFilePath), "mp3|ogg|wav", "Custom/Sounds", true, true, false, null, false, shortCutsForDirectory, true, false);
		}
	}

	// Token: 0x06005338 RID: 21304 RVA: 0x001E1950 File Offset: 0x001DFD50
	public override void InitUI()
	{
		if (this.urlInputField != null)
		{
			if (this.urlInputFieldAction != null)
			{
				InputFieldAction inputFieldAction = this.urlInputFieldAction;
				inputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(inputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.<InitUI>m__0));
			}
			if (this.addClipButton != null)
			{
				this.addClipButton.onClick.AddListener(new UnityAction(this.<InitUI>m__1));
			}
		}
		if (this.copyToClipboardButton != null)
		{
			this.copyToClipboardButton.onClick.AddListener(new UnityAction(this.CopyURLToClipboard));
		}
		if (this.copyFromClipboardButton != null)
		{
			this.copyFromClipboardButton.onClick.AddListener(new UnityAction(this.CopyURLFromClipboard));
		}
		if (this.fileBrowseButton != null)
		{
			this.fileBrowseButton.onClick.AddListener(new UnityAction(this.FileBrowse));
		}
	}

	// Token: 0x06005339 RID: 21305 RVA: 0x001E1A5C File Offset: 0x001DFE5C
	protected override void Init()
	{
		base.Init();
		URLAudioClipManager.singleton = this;
		if (!FileManager.DirectoryExists("Custom/Sounds", false, false))
		{
			FileManager.CreateDirectory("Custom/Sounds");
		}
		this.InitUI();
		this.urlqueue = new Queue<URLAudioClip>();
		base.StartCoroutine(this.ProcessQueue());
	}

	// Token: 0x0600533A RID: 21306 RVA: 0x001E1AAE File Offset: 0x001DFEAE
	[CompilerGenerated]
	private void <InitUI>m__0()
	{
		this.QueueClip(this.urlInputField.text, null, false);
	}

	// Token: 0x0600533B RID: 21307 RVA: 0x001E1AC4 File Offset: 0x001DFEC4
	[CompilerGenerated]
	private void <InitUI>m__1()
	{
		this.QueueClip(this.urlInputField.text, null, false);
	}

	// Token: 0x04004313 RID: 17171
	public static URLAudioClipManager singleton;

	// Token: 0x04004314 RID: 17172
	protected string[] customParamNames = new string[]
	{
		"clips"
	};

	// Token: 0x04004315 RID: 17173
	public ScrollRectContentManager clipContentManager;

	// Token: 0x04004316 RID: 17174
	public RectTransform clipPrefab;

	// Token: 0x04004317 RID: 17175
	protected Queue<URLAudioClip> urlqueue;

	// Token: 0x04004318 RID: 17176
	public Button fileBrowseButton;

	// Token: 0x04004319 RID: 17177
	public InputField urlInputField;

	// Token: 0x0400431A RID: 17178
	public InputFieldAction urlInputFieldAction;

	// Token: 0x0400431B RID: 17179
	public Button addClipButton;

	// Token: 0x0400431C RID: 17180
	public Button copyToClipboardButton;

	// Token: 0x0400431D RID: 17181
	public Button copyFromClipboardButton;

	// Token: 0x02000FDE RID: 4062
	[CompilerGenerated]
	private sealed class <ProcessQueue>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007586 RID: 30086 RVA: 0x001E1ADA File Offset: 0x001DFEDA
		[DebuggerHidden]
		public <ProcessQueue>c__Iterator0()
		{
		}

		// Token: 0x06007587 RID: 30087 RVA: 0x001E1AE4 File Offset: 0x001DFEE4
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				break;
			case 1U:
				goto IL_22E;
			case 2U:
				goto IL_3E6;
			case 3U:
				goto IL_3C6;
			case 4U:
				goto IL_666;
			case 5U:
				if (!uac.removed)
				{
					if (bi2.isError)
					{
						uac.errorMsg = bi2.error;
						uac.error = true;
						SuperController.LogError("Error during mp3 import " + bi2.error);
					}
					else
					{
						uac.sourceClip = bi2.audioClip;
					}
				}
				goto IL_94D;
			case 6U:
				break;
			default:
				return false;
			}
			if (this.urlqueue.Count <= 0)
			{
				goto IL_B29;
			}
			<ProcessQueue>c__AnonStorey = new URLAudioClipManager.<ProcessQueue>c__Iterator0.<ProcessQueue>c__AnonStorey1();
			<ProcessQueue>c__AnonStorey.<>f__ref$0 = this;
			uac = this.urlqueue.Dequeue();
			af = new AsyncFlag("URL Audio: " + uac.displayName);
			if (uac.fromRestore)
			{
				SuperController.singleton.ResetSimulation(af, false);
			}
			iconFlag = new AsyncFlag("URL Audio");
			SuperController.singleton.SetLoadingIconFlag(iconFlag);
			url = uac.url;
			<ProcessQueue>c__AnonStorey.fe = FileManager.GetFileEntry(url, false);
			isPackageFile = false;
			if (<ProcessQueue>c__AnonStorey.fe != null && <ProcessQueue>c__AnonStorey.fe is VarFileEntry)
			{
				isPackageFile = true;
			}
			if (url.EndsWith(".mp3") || url.EndsWith(".wav") || url.EndsWith(".ogg"))
			{
				bi = base.GetComponent<BassImporter>();
				if (!(bi != null))
				{
					uac.errorMsg = "MP3/WAV importer not defined";
					uac.error = true;
					SuperController.LogError("MP3/WAV importer not defined. Cannot import mp3/wav");
					goto IL_51B;
				}
				if (!isPackageFile)
				{
					if (Regex.IsMatch(url, "^file:///"))
					{
						url = url.Replace("file:///", "file://");
					}
					if (!Regex.IsMatch(url, "^http") && !Regex.IsMatch(url, "^file"))
					{
						if (url.Contains(":/"))
						{
							url = "file://" + url;
						}
						else
						{
							url = "file://./" + url;
						}
					}
					bi.Import(url);
					goto IL_3C6;
				}
				<ProcessQueue>c__AnonStorey2 = new URLAudioClipManager.<ProcessQueue>c__Iterator0.<ProcessQueue>c__AnonStorey2();
				<ProcessQueue>c__AnonStorey2.<>f__ref$0 = this;
				<ProcessQueue>c__AnonStorey2.<>f__ref$1 = <ProcessQueue>c__AnonStorey;
				bi.Prep();
				<ProcessQueue>c__AnonStorey2.byteArray = 0;
				loadThread = new Thread(new ThreadStart(<ProcessQueue>c__AnonStorey2.<>m__0));
				loadThread.Start();
			}
			else
			{
				if (!isPackageFile)
				{
					if (!Regex.IsMatch(url, "^http") && !Regex.IsMatch(url, "^file"))
					{
						if (url.Contains(":/"))
						{
							url = "file:///" + url;
						}
						else
						{
							url = "file:///.\\" + url;
						}
					}
					www = new WWW(url);
					goto IL_666;
				}
				uac.error = true;
				uac.errorMsg = "Packages only support mp3, ogg, and wav files";
				SuperController.LogError(string.Concat(new object[]
				{
					"Could not load audio source at ",
					url,
					" Error: ",
					uac.error
				}));
				goto IL_B13;
			}
			IL_22E:
			if (!loadThread.IsAlive)
			{
				this.$current = bi.SetData(<ProcessQueue>c__AnonStorey2.byteArray, <ProcessQueue>c__AnonStorey.fe.Size);
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			}
			this.$current = null;
			if (!this.$disposing)
			{
				this.$PC = 1;
			}
			return true;
			IL_3C6:
			if (!bi.isLoaded && !bi.isError)
			{
				if (!uac.removed)
				{
					if (uac.loadProgressSlider != null)
					{
						uac.loadProgressSlider.value = bi.progress;
					}
					if (uac.sizeText != null)
					{
						uac.sizeText.text = string.Empty;
					}
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
			}
			IL_3E6:
			if (!uac.removed)
			{
				if (!bi.isError)
				{
					uac.sourceClip = bi.audioClip;
					if (uac.loadProgressSlider != null)
					{
						uac.loadProgressSlider.value = 1f;
					}
					if (uac.sourceClip != null)
					{
						uac.ready = true;
					}
					else if (!uac.error)
					{
						uac.errorMsg = "Unable to extract audio from url";
						uac.error = true;
						SuperController.LogError("Unable to extract audio from url " + url);
					}
				}
				else
				{
					uac.errorMsg = bi.error;
					uac.error = true;
					SuperController.LogError("Error during mp3/wav import " + bi.error);
				}
			}
			IL_51B:
			goto IL_B13;
			IL_666:
			if (!www.isDone)
			{
				if (!uac.removed)
				{
					if (uac.loadProgressSlider != null)
					{
						uac.loadProgressSlider.value = www.progress;
					}
					if (uac.sizeText != null)
					{
						float num2 = (float)www.bytesDownloaded / 1000000f;
						uac.sizeText.text = string.Format("{0:F1}MB", num2);
					}
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				www.Dispose();
			}
			if (uac.removed)
			{
				goto IL_AB6;
			}
			if (www.error != null && !(www.error == string.Empty))
			{
				uac.error = true;
				uac.errorMsg = www.error;
				SuperController.LogError("Could not load audio source at " + url + " Error: " + www.error);
				goto IL_AB6;
			}
			isMp3 = false;
			if (www.responseHeaders.Count > 0)
			{
				foreach (KeyValuePair<string, string> keyValuePair in www.responseHeaders)
				{
					if (keyValuePair.Key == "Content-Type")
					{
						string[] array = keyValuePair.Value.Split(new char[]
						{
							';'
						});
						foreach (string text in array)
						{
							if (text.EndsWith(".mp3\""))
							{
								isMp3 = true;
							}
						}
					}
					else if (keyValuePair.Key == "Content-Disposition")
					{
						string[] array3 = keyValuePair.Value.Split(new char[]
						{
							';'
						});
						foreach (string text2 in array3)
						{
							if (text2.EndsWith(".mp3"))
							{
								isMp3 = true;
							}
						}
					}
				}
			}
			fsize = (float)www.bytesDownloaded / 1000000f;
			if (uac.sizeText != null)
			{
				uac.sizeText.text = string.Format("{0:F1}MB", fsize);
			}
			if (url.EndsWith(".mp3") || isMp3)
			{
				bi2 = base.GetComponent<BassImporter>();
				if (bi2 != null)
				{
					this.$current = bi2.SetData(www.bytes);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				uac.errorMsg = "MP3 importer not defined";
				uac.error = true;
				SuperController.LogError("MP3 importer not defined. Cannot import mp3");
			}
			else
			{
				uac.sourceClip = www.GetAudioClip();
				if (uac.sourceClip == null)
				{
					try
					{
						uac.sourceClip = NAudioPlayer.AudioClipFromMp3Data(www.bytes);
					}
					catch (Exception ex)
					{
						uac.error = true;
						uac.errorMsg = "Could not extract audio data";
						SuperController.LogError("Could not extract audio data: " + ex.Message);
					}
				}
			}
			IL_94D:
			if (uac.loadProgressSlider != null)
			{
				uac.loadProgressSlider.value = 1f;
			}
			if (uac.sourceClip != null)
			{
				uac.ready = true;
			}
			else if (!uac.error)
			{
				uac.errorMsg = "Unable to extract audio from url";
				uac.error = true;
				SuperController.LogError("Unable to extract audio from url " + url);
			}
			IL_AB6:
			IL_B13:
			af.Raise();
			iconFlag.Raise();
			IL_B29:
			this.$current = null;
			if (!this.$disposing)
			{
				this.$PC = 6;
			}
			return true;
		}

		// Token: 0x17001163 RID: 4451
		// (get) Token: 0x06007588 RID: 30088 RVA: 0x001E2660 File Offset: 0x001E0A60
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x06007589 RID: 30089 RVA: 0x001E2668 File Offset: 0x001E0A68
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600758A RID: 30090 RVA: 0x001E2670 File Offset: 0x001E0A70
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600758B RID: 30091 RVA: 0x001E2680 File Offset: 0x001E0A80
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400699E RID: 27038
		internal URLAudioClip <uac>__1;

		// Token: 0x0400699F RID: 27039
		internal AsyncFlag <af>__1;

		// Token: 0x040069A0 RID: 27040
		internal AsyncFlag <iconFlag>__1;

		// Token: 0x040069A1 RID: 27041
		internal string <url>__1;

		// Token: 0x040069A2 RID: 27042
		internal bool <isPackageFile>__1;

		// Token: 0x040069A3 RID: 27043
		internal BassImporter <bi>__2;

		// Token: 0x040069A4 RID: 27044
		internal Thread <loadThread>__3;

		// Token: 0x040069A5 RID: 27045
		internal WWW <www>__4;

		// Token: 0x040069A6 RID: 27046
		internal bool <isMp3>__5;

		// Token: 0x040069A7 RID: 27047
		internal float <fsize>__5;

		// Token: 0x040069A8 RID: 27048
		internal BassImporter <bi>__6;

		// Token: 0x040069A9 RID: 27049
		internal URLAudioClipManager $this;

		// Token: 0x040069AA RID: 27050
		internal object $current;

		// Token: 0x040069AB RID: 27051
		internal bool $disposing;

		// Token: 0x040069AC RID: 27052
		internal int $PC;

		// Token: 0x040069AD RID: 27053
		private URLAudioClipManager.<ProcessQueue>c__Iterator0.<ProcessQueue>c__AnonStorey1 $locvar5;

		// Token: 0x040069AE RID: 27054
		private URLAudioClipManager.<ProcessQueue>c__Iterator0.<ProcessQueue>c__AnonStorey2 $locvar6;

		// Token: 0x02000FDF RID: 4063
		private sealed class <ProcessQueue>c__AnonStorey1
		{
			// Token: 0x0600758C RID: 30092 RVA: 0x001E2687 File Offset: 0x001E0A87
			public <ProcessQueue>c__AnonStorey1()
			{
			}

			// Token: 0x040069AF RID: 27055
			internal FileEntry fe;

			// Token: 0x040069B0 RID: 27056
			internal URLAudioClipManager.<ProcessQueue>c__Iterator0 <>f__ref$0;
		}

		// Token: 0x02000FE0 RID: 4064
		private sealed class <ProcessQueue>c__AnonStorey2
		{
			// Token: 0x0600758D RID: 30093 RVA: 0x001E268F File Offset: 0x001E0A8F
			public <ProcessQueue>c__AnonStorey2()
			{
			}

			// Token: 0x0600758E RID: 30094 RVA: 0x001E2697 File Offset: 0x001E0A97
			internal void <>m__0()
			{
				this.<>f__ref$0.$this.LoadFileIntoByteArray(this.<>f__ref$1.fe, out this.byteArray);
			}

			// Token: 0x040069B1 RID: 27057
			internal IntPtr byteArray;

			// Token: 0x040069B2 RID: 27058
			internal URLAudioClipManager.<ProcessQueue>c__Iterator0 <>f__ref$0;

			// Token: 0x040069B3 RID: 27059
			internal URLAudioClipManager.<ProcessQueue>c__Iterator0.<ProcessQueue>c__AnonStorey1 <>f__ref$1;
		}
	}
}
