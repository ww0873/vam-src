using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Core;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using OldMoatGames;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000D16 RID: 3350
public class ImageControl : JSONStorableTriggerHandler
{
	// Token: 0x06006644 RID: 26180 RVA: 0x0026A466 File Offset: 0x00268866
	public ImageControl()
	{
	}

	// Token: 0x06006645 RID: 26181 RVA: 0x0026A484 File Offset: 0x00268884
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if (includePhysical || forceStore)
		{
			if (this.videoReadyTrigger != null)
			{
				this.needsStore = true;
				json["videoReadyTrigger"] = this.videoReadyTrigger.GetJSON(base.subScenePrefix);
			}
			if (this.videoStartedTrigger != null)
			{
				this.needsStore = true;
				json["videoStartedTrigger"] = this.videoStartedTrigger.GetJSON(base.subScenePrefix);
			}
			if (this.videoStoppedTrigger != null)
			{
				this.needsStore = true;
				json["videoStoppedTrigger"] = this.videoStoppedTrigger.GetJSON(base.subScenePrefix);
			}
		}
		return json;
	}

	// Token: 0x06006646 RID: 26182 RVA: 0x0026A534 File Offset: 0x00268934
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		base.LateRestoreFromJSON(jc, restorePhysical, restoreAppearance, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical)
		{
			if (!base.IsCustomPhysicalParamLocked("videoReadyTrigger"))
			{
				if (jc["videoReadyTrigger"] != null)
				{
					JSONClass asObject = jc["videoReadyTrigger"].AsObject;
					if (asObject != null)
					{
						this.videoReadyTrigger.RestoreFromJSON(asObject, base.subScenePrefix, base.mergeRestore);
					}
				}
				else if (setMissingToDefault)
				{
					this.videoReadyTrigger.RestoreFromJSON(new JSONClass());
				}
			}
			if (!base.IsCustomPhysicalParamLocked("videoStartedTrigger"))
			{
				if (jc["videoStartedTrigger"] != null)
				{
					JSONClass asObject2 = jc["videoStartedTrigger"].AsObject;
					if (asObject2 != null)
					{
						this.videoStartedTrigger.RestoreFromJSON(asObject2, base.subScenePrefix, base.mergeRestore);
					}
				}
				else if (setMissingToDefault)
				{
					this.videoStartedTrigger.RestoreFromJSON(new JSONClass());
				}
			}
			if (!base.IsCustomPhysicalParamLocked("videoStoppedTrigger"))
			{
				if (jc["videoStoppedTrigger"] != null)
				{
					JSONClass asObject3 = jc["videoStoppedTrigger"].AsObject;
					if (asObject3 != null)
					{
						this.videoStoppedTrigger.RestoreFromJSON(asObject3, base.subScenePrefix, base.mergeRestore);
					}
				}
				else if (setMissingToDefault)
				{
					this.videoStoppedTrigger.RestoreFromJSON(new JSONClass());
				}
			}
		}
	}

	// Token: 0x06006647 RID: 26183 RVA: 0x0026A6C4 File Offset: 0x00268AC4
	public override void Validate()
	{
		base.Validate();
		if (this.videoReadyTrigger != null)
		{
			this.videoReadyTrigger.Validate();
		}
		if (this.videoStartedTrigger != null)
		{
			this.videoStartedTrigger.Validate();
		}
		if (this.videoStoppedTrigger != null)
		{
			this.videoStoppedTrigger.Validate();
		}
	}

	// Token: 0x06006648 RID: 26184 RVA: 0x0026A71C File Offset: 0x00268B1C
	public void StartSyncUrl()
	{
		if (this._url != null && this._url != string.Empty)
		{
			bool flag = false;
			if (Regex.IsMatch(this.url, "^http"))
			{
				if (UserPreferences.singleton == null)
				{
					flag = true;
				}
				else if (UserPreferences.singleton.enableWebMisc)
				{
					if (UserPreferences.singleton.CheckWhitelistDomain(this._url))
					{
						flag = true;
					}
					else
					{
						if (this.nonWhitelistSiteObject != null)
						{
							this.nonWhitelistSiteObject.SetActive(true);
						}
						if (this.nonWhitelistSiteText != null)
						{
							this.nonWhitelistSiteText.text = this._url;
						}
						SuperController.LogError("Attempted to load image from URL " + this._url + " which is not on whitelist", true, !UserPreferences.singleton.hideDisabledWebMessages);
					}
				}
				else if (!UserPreferences.singleton.hideDisabledWebMessages)
				{
					if (this.webDisabledObject != null)
					{
						this.webDisabledObject.SetActive(true);
					}
					SuperController.LogError("Attempted to load http URL image when web load option is disabled. To enable, see User Preferences -> Web Security tab");
					SuperController.singleton.ShowMainHUDAuto();
					SuperController.singleton.SetActiveUI("MainMenu");
					SuperController.singleton.SetMainMenuTab("TabUserPrefs");
					SuperController.singleton.SetUserPrefsTab("TabSecurity");
				}
				else if (this.webDisabledObject != null)
				{
					this.webDisabledObject.SetActive(true);
				}
			}
			else
			{
				flag = true;
				if (this.webDisabledObject != null)
				{
					this.webDisabledObject.SetActive(false);
				}
			}
			if (flag)
			{
				if (this.nonWhitelistSiteObject != null)
				{
					this.nonWhitelistSiteObject.SetActive(false);
				}
				if (base.gameObject.activeInHierarchy)
				{
					if (this._url.EndsWith(".avi") || this._url.EndsWith(".mp4"))
					{
						this.ClearImage();
						this.SyncVideo();
					}
					else
					{
						this.DisableVideoPlayer();
						AnimatedGifPlayer component = base.GetComponent<AnimatedGifPlayer>();
						if (this._url.EndsWith(".gif"))
						{
							if (component != null)
							{
								component.enabled = true;
								component.FileName = this.url;
								component.Init();
							}
						}
						else if (this._url.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase) || this._url.EndsWith(".jpeg", StringComparison.InvariantCultureIgnoreCase) || this._url.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
						{
							if (component != null)
							{
								component.enabled = false;
							}
							if (this._url.StartsWith("http") || this._url.StartsWith("file") || ImageLoaderThreaded.singleton == null)
							{
								base.StartCoroutine(this.SyncImage());
							}
							else
							{
								ImageLoaderThreaded.QueuedImage queuedImage = new ImageLoaderThreaded.QueuedImage();
								queuedImage.imgPath = this._url;
								queuedImage.createMipMaps = true;
								queuedImage.callback = new ImageLoaderThreaded.ImageLoaderCallback(this.OnImageLoaded);
								ImageLoaderThreaded.singleton.QueueImage(queuedImage);
							}
						}
						else
						{
							if (component != null)
							{
								component.enabled = false;
							}
							this.ClearImage();
							this.SyncVideo();
						}
					}
				}
				else
				{
					this.notActiveOnSync = true;
				}
			}
		}
		else
		{
			this.DisableVideoPlayer();
			AnimatedGifPlayer component2 = base.GetComponent<AnimatedGifPlayer>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
			this.ClearImage();
		}
	}

	// Token: 0x06006649 RID: 26185 RVA: 0x0026AAAD File Offset: 0x00268EAD
	protected void SyncUrl(string s)
	{
		this._url = s;
		this.StartSyncUrl();
	}

	// Token: 0x17000F0E RID: 3854
	// (get) Token: 0x0600664A RID: 26186 RVA: 0x0026AABC File Offset: 0x00268EBC
	// (set) Token: 0x0600664B RID: 26187 RVA: 0x0026AAC4 File Offset: 0x00268EC4
	public string url
	{
		get
		{
			return this._url;
		}
		set
		{
			if (this.urlJSON != null)
			{
				this.urlJSON.val = value;
			}
			if (this._url != value)
			{
				this.SyncUrl(value);
			}
		}
	}

	// Token: 0x0600664C RID: 26188 RVA: 0x0026AAF8 File Offset: 0x00268EF8
	protected void BeginBrowse(JSONStorableUrl jsurl)
	{
		List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory("Custom/Images", true, true, true, true);
		shortCutsForDirectory.Insert(0, new ShortCut
		{
			displayName = "Root",
			path = Path.GetFullPath(".")
		});
		jsurl.shortCuts = shortCutsForDirectory;
	}

	// Token: 0x0600664D RID: 26189 RVA: 0x0026AB44 File Offset: 0x00268F44
	public void SyncImageRatio(Texture tex, bool isVideo = false)
	{
		if (this.matchScaleToImageRatio)
		{
			float num = 1f;
			float num2 = 1f;
			float num3 = 1f;
			if (this.targetTransformForScale != null)
			{
				num2 = this.targetTransformForScale.localScale.x;
				num3 = this.targetTransformForScale.localScale.y;
				num = num2 / num3;
			}
			Vector3 localScale;
			localScale.z = 1f;
			if (tex != null)
			{
				float num4 = (float)tex.width;
				float num5 = (float)tex.height;
				if (isVideo && this._useAnamorphicVideoAspectRatio)
				{
					num4 *= 1.33f;
				}
				float num6 = num4 / num5;
				if (num6 > num)
				{
					localScale.x = num2;
					localScale.y = num2 / num6;
				}
				else
				{
					localScale.x = num6 * num3;
					localScale.y = num3;
				}
			}
			else
			{
				localScale.x = num2;
				localScale.y = num3;
			}
			if (this.transformForScaling != null)
			{
				this.transformForScaling.localScale = localScale;
			}
			else
			{
				base.transform.localScale = localScale;
			}
		}
	}

	// Token: 0x0600664E RID: 26190 RVA: 0x0026AC70 File Offset: 0x00269070
	public void SyncTexture(Texture2D tex)
	{
		this.SyncImageRatio(tex, false);
		this.currentTexture = tex;
		this.SyncAllowTiling();
		if (this.rawImage != null)
		{
			this.rawImage.texture = tex;
		}
		else
		{
			MeshRenderer[] components = base.GetComponents<MeshRenderer>();
			foreach (MeshRenderer meshRenderer in components)
			{
				Material[] array2;
				if (Application.isPlaying)
				{
					array2 = meshRenderer.materials;
				}
				else
				{
					array2 = meshRenderer.sharedMaterials;
				}
				Material material = array2[0];
				if (material.HasProperty("_MainTex"))
				{
					material.SetTexture("_MainTex", tex);
				}
				if (this.alsoSetSpecularTexture && material.HasProperty("_SpecTex"))
				{
					material.SetTexture("_SpecTex", tex);
				}
			}
			base.GetComponent<Renderer>().material.mainTexture = tex;
		}
	}

	// Token: 0x0600664F RID: 26191 RVA: 0x0026AD54 File Offset: 0x00269154
	protected void SyncVideoTexture()
	{
		if (this.videoPlayer.texture != null && this.videoPlayer.texture != this.currentVideoTexture)
		{
			this.currentVideoTexture = this.videoPlayer.texture;
			this.SyncImageRatio(this.currentVideoTexture, true);
			if (this.rawImage == null && this.alsoSetSpecularTexture)
			{
				MeshRenderer[] components = base.GetComponents<MeshRenderer>();
				foreach (MeshRenderer meshRenderer in components)
				{
					Material[] array2;
					if (Application.isPlaying)
					{
						array2 = meshRenderer.materials;
					}
					else
					{
						array2 = meshRenderer.sharedMaterials;
					}
					Material material = array2[0];
					if (material.HasProperty("_SpecTex"))
					{
						material.SetTexture("_SpecTex", this.currentVideoTexture);
					}
				}
			}
		}
	}

	// Token: 0x06006650 RID: 26192 RVA: 0x0026AE38 File Offset: 0x00269238
	protected string GetVideoCacheFileName(string path)
	{
		string result = null;
		FileEntry fileEntry = FileManager.GetFileEntry(path, false);
		if (fileEntry != null)
		{
			string text = fileEntry.Size.ToString();
			string text2 = fileEntry.LastWriteTime.ToFileTime().ToString();
			string text3 = Path.GetFileNameWithoutExtension(path);
			string extension = Path.GetExtension(path);
			text3 = text3.Replace('.', '_');
			result = string.Concat(new string[]
			{
				text3,
				"_",
				text,
				"_",
				text2,
				extension
			});
		}
		return result;
	}

	// Token: 0x06006651 RID: 26193 RVA: 0x0026AED8 File Offset: 0x002692D8
	protected string GetVideoCachePath(string path)
	{
		string result = null;
		string videoCacheDir = CacheManager.GetVideoCacheDir();
		if (videoCacheDir != null)
		{
			string str = videoCacheDir + "/";
			string videoCacheFileName = this.GetVideoCacheFileName(path);
			if (videoCacheFileName != null)
			{
				result = str + videoCacheFileName;
			}
		}
		return result;
	}

	// Token: 0x06006652 RID: 26194 RVA: 0x0026AF16 File Offset: 0x00269316
	protected void DeleteTempVideo()
	{
		if (this.currentTempVideo != null)
		{
			File.Delete(this.currentTempVideo);
			this.currentTempVideo = null;
		}
	}

	// Token: 0x06006653 RID: 26195 RVA: 0x0026AF38 File Offset: 0x00269338
	protected string MakeTempVideo(string inputPath)
	{
		string result = null;
		try
		{
			Directory.CreateDirectory("Temp");
			this.DeleteTempVideo();
			this.currentTempVideo = "Temp/" + this.GetVideoCacheFileName(inputPath);
			FileManager.CopyFile(inputPath, this.currentTempVideo, false);
			result = this.currentTempVideo;
		}
		catch (Exception ex)
		{
			SuperController.LogError(string.Concat(new object[]
			{
				"Exception during creation of temporary copy of video ",
				inputPath,
				": ",
				ex
			}));
		}
		return result;
	}

	// Token: 0x06006654 RID: 26196 RVA: 0x0026AFC8 File Offset: 0x002693C8
	protected void SyncVideo()
	{
		if (this.videoPlayer != null)
		{
			this.videoPlayer.enabled = true;
			string text = this._url;
			if (FileManager.IsFileInPackage(text))
			{
				if (CacheManager.CachingEnabled)
				{
					try
					{
						string videoCachePath = this.GetVideoCachePath(text);
						if (videoCachePath == null)
						{
							throw new Exception("Unable to get cache path");
						}
						if (FileManager.FileExists(videoCachePath, false, false))
						{
							text = videoCachePath;
						}
						else
						{
							byte[] buffer = new byte[4096];
							using (FileEntryStream fileEntryStream = FileManager.OpenStream(text, false))
							{
								using (FileStream fileStream = File.Open(videoCachePath, FileMode.Create))
								{
									StreamUtils.Copy(fileEntryStream.Stream, fileStream, buffer);
									text = videoCachePath;
								}
							}
						}
					}
					catch (Exception ex)
					{
						SuperController.LogError(string.Concat(new object[]
						{
							"Exception during cache handling video ",
							text,
							": ",
							ex
						}));
						return;
					}
				}
				else
				{
					string text2 = this.MakeTempVideo(text);
					if (text2 == null)
					{
						return;
					}
					text = text2;
				}
			}
			if (File.Exists(text))
			{
				text = Path.GetFullPath(text);
			}
			this.videoPlayer.url = text;
			this.videoHadErrorJSON.val = false;
			this.videoIsLoadingJSON.val = true;
			this.videoPlayer.Prepare();
		}
	}

	// Token: 0x06006655 RID: 26197 RVA: 0x0026B150 File Offset: 0x00269550
	public bool IsVideoReady()
	{
		return this.videoPlayer != null && this.videoPlayer.enabled && this.videoPlayer.isPrepared && this.videoPlayer.frameCount != 0UL;
	}

	// Token: 0x06006656 RID: 26198 RVA: 0x0026B1A5 File Offset: 0x002695A5
	public bool IsVideoPlaying()
	{
		return this.videoPlayer != null && this.videoPlayer.enabled && this.videoPlayer.isPlaying;
	}

	// Token: 0x06006657 RID: 26199 RVA: 0x0026B1D5 File Offset: 0x002695D5
	protected void SyncPlayVideoWhenReady(bool b)
	{
		this._playVideoWhenReady = b;
	}

	// Token: 0x17000F0F RID: 3855
	// (get) Token: 0x06006658 RID: 26200 RVA: 0x0026B1DE File Offset: 0x002695DE
	// (set) Token: 0x06006659 RID: 26201 RVA: 0x0026B1E6 File Offset: 0x002695E6
	public bool playVideoWhenReady
	{
		get
		{
			return this._playVideoWhenReady;
		}
		set
		{
			if (this.playVideoWhenReadyJSON != null)
			{
				this.playVideoWhenReadyJSON.val = value;
			}
			else
			{
				this.SyncPlayVideoWhenReady(value);
			}
		}
	}

	// Token: 0x0600665A RID: 26202 RVA: 0x0026B20C File Offset: 0x0026960C
	protected void SyncUseAnamorphicVideoAspectRatio(bool b)
	{
		this._useAnamorphicVideoAspectRatio = b;
		if (this.videoPlayer != null && this.videoPlayer.isPrepared)
		{
			this.SyncImageRatio(this.currentVideoTexture, true);
		}
		else
		{
			this.SyncImageRatio(this.currentTexture, false);
		}
	}

	// Token: 0x17000F10 RID: 3856
	// (get) Token: 0x0600665B RID: 26203 RVA: 0x0026B260 File Offset: 0x00269660
	// (set) Token: 0x0600665C RID: 26204 RVA: 0x0026B268 File Offset: 0x00269668
	public bool useAnamorphicVideoAspectRatio
	{
		get
		{
			return this._useAnamorphicVideoAspectRatio;
		}
		set
		{
			if (this.useAnamorphicVideoAspectRatioJSON != null)
			{
				this.useAnamorphicVideoAspectRatioJSON.val = value;
			}
			else
			{
				this.SyncUseAnamorphicVideoAspectRatio(value);
			}
		}
	}

	// Token: 0x0600665D RID: 26205 RVA: 0x0026B28D File Offset: 0x0026968D
	protected void SyncLoopVideo(bool b)
	{
		this._loopVideo = b;
		if (this.videoPlayer != null)
		{
			this.videoPlayer.isLooping = this._loopVideo;
		}
	}

	// Token: 0x17000F11 RID: 3857
	// (get) Token: 0x0600665E RID: 26206 RVA: 0x0026B2B8 File Offset: 0x002696B8
	// (set) Token: 0x0600665F RID: 26207 RVA: 0x0026B2C0 File Offset: 0x002696C0
	public bool loopVideo
	{
		get
		{
			return this._loopVideo;
		}
		set
		{
			if (this.loopVideoJSON != null)
			{
				this.loopVideoJSON.val = value;
			}
			else
			{
				this.SyncLoopVideo(value);
			}
		}
	}

	// Token: 0x06006660 RID: 26208 RVA: 0x0026B2E5 File Offset: 0x002696E5
	protected void SyncPlaybackTime(float f)
	{
		if (this.videoPlayer != null)
		{
			this.videoPlayer.time = (double)f;
		}
	}

	// Token: 0x06006661 RID: 26209 RVA: 0x0026B308 File Offset: 0x00269708
	protected void UpdateVideo()
	{
		if (this.videoPlayer != null)
		{
			bool flag = this.videoPlayer.isPrepared && this.videoPlayer.frameCount != 0UL;
			if (this.videoIsReadyJSON != null)
			{
				this.videoIsReadyJSON.val = flag;
			}
			if (this.videoReadyTrigger != null)
			{
				this.videoReadyTrigger.active = flag;
			}
			if (this.videoStoppedTrigger != null && this.wasPlaying && !this.videoPlayer.isPlaying)
			{
				this.videoStoppedTrigger.active = true;
				this.videoStoppedTrigger.active = false;
			}
			this.wasPlaying = this.videoPlayer.isPlaying;
			if (this.playVideoAction != null && this.playVideoAction.button != null)
			{
				this.playVideoAction.button.gameObject.SetActive(!this.videoPlayer.isPlaying);
			}
			if (this.pauseVideoAction != null && this.pauseVideoAction.button != null)
			{
				this.pauseVideoAction.button.gameObject.SetActive(this.videoPlayer.isPlaying);
			}
			if (this.playbackTimeJSON != null)
			{
				float max = 0f;
				if (this.videoPlayer.frameRate > 0f)
				{
					max = this.videoPlayer.frameCount / this.videoPlayer.frameRate;
				}
				this.playbackTimeJSON.max = max;
				if (this.videoPlayer.isPlaying)
				{
					this.playbackTimeJSON.valNoCallback = (float)this.videoPlayer.time;
				}
				this.playbackTimeJSON.SetInteractble(this.videoPlayer.canSetTime);
			}
		}
	}

	// Token: 0x06006662 RID: 26210 RVA: 0x0026B4D8 File Offset: 0x002698D8
	public void PlayVideo()
	{
		if (this.videoPlayer != null)
		{
			if (!this.videoPlayer.isPrepared)
			{
				this.videoHadErrorJSON.val = false;
				this.videoIsLoadingJSON.val = true;
			}
			this.videoPlayer.Play();
		}
	}

	// Token: 0x06006663 RID: 26211 RVA: 0x0026B529 File Offset: 0x00269929
	public void PauseVideo()
	{
		if (this.videoPlayer != null)
		{
			this.videoPlayer.Pause();
		}
	}

	// Token: 0x06006664 RID: 26212 RVA: 0x0026B548 File Offset: 0x00269948
	public void StopVideo()
	{
		if (this.videoPlayer != null)
		{
			if (this.videoPlayer.canSetTime)
			{
				this.playbackTimeJSON.val = 0f;
				this.videoPlayer.Pause();
			}
			else
			{
				this.videoPlayer.Stop();
			}
		}
	}

	// Token: 0x06006665 RID: 26213 RVA: 0x0026B5A4 File Offset: 0x002699A4
	public void SeekToVideoStart()
	{
		if (this.videoPlayer != null)
		{
			if (this.videoPlayer.canSetTime)
			{
				this.playbackTimeJSON.val = 0f;
			}
			else if (this.videoPlayer.isPlaying)
			{
				this.videoPlayer.Stop();
				this.videoPlayer.Play();
			}
			else
			{
				this.videoPlayer.Stop();
			}
		}
	}

	// Token: 0x06006666 RID: 26214 RVA: 0x0026B620 File Offset: 0x00269A20
	protected void InitVideoPlayer()
	{
		this.videoPlayer = base.GetComponent<VideoPlayer>();
		if (this.videoPlayer != null)
		{
			this.videoPlayer.enabled = false;
			this.videoPlayer.errorReceived += this.VideoPlayer_errorReceived;
			this.videoPlayer.prepareCompleted += this.VideoPlayer_prepareCompleted;
			this.videoPlayer.started += this.VideoPlayer_started;
			this.videoPlayer.seekCompleted += this.VideoPlayer_seekCompleted;
		}
	}

	// Token: 0x06006667 RID: 26215 RVA: 0x0026B6B2 File Offset: 0x00269AB2
	protected void VideoPlayer_seekCompleted(VideoPlayer source)
	{
	}

	// Token: 0x06006668 RID: 26216 RVA: 0x0026B6B4 File Offset: 0x00269AB4
	protected void VideoPlayer_started(VideoPlayer source)
	{
		if (this.videoStartedTrigger != null)
		{
			this.videoStartedTrigger.active = true;
			this.videoStartedTrigger.active = false;
		}
	}

	// Token: 0x06006669 RID: 26217 RVA: 0x0026B6D9 File Offset: 0x00269AD9
	protected void VideoPlayer_prepareCompleted(VideoPlayer source)
	{
		this.videoIsLoadingJSON.val = false;
		this.SyncVideoTexture();
		if (this._playVideoWhenReady)
		{
			this.videoPlayer.Play();
		}
	}

	// Token: 0x0600666A RID: 26218 RVA: 0x0026B703 File Offset: 0x00269B03
	protected void VideoPlayer_errorReceived(VideoPlayer source, string message)
	{
		this.videoIsLoadingJSON.val = false;
		this.videoHadErrorJSON.val = true;
		SuperController.LogError("Error with video " + message);
	}

	// Token: 0x0600666B RID: 26219 RVA: 0x0026B72D File Offset: 0x00269B2D
	protected void DisableVideoPlayer()
	{
		if (this.videoPlayer != null)
		{
			this.videoPlayer.enabled = false;
		}
		this.videoIsLoadingJSON.val = false;
		this.videoHadErrorJSON.val = false;
	}

	// Token: 0x0600666C RID: 26220 RVA: 0x0026B764 File Offset: 0x00269B64
	protected void OnAtomRename(string oldid, string newid)
	{
		if (this.videoReadyTrigger != null)
		{
			this.videoReadyTrigger.SyncAtomNames();
		}
		if (this.videoStartedTrigger != null)
		{
			this.videoStartedTrigger.SyncAtomNames();
		}
		if (this.videoStoppedTrigger != null)
		{
			this.videoStoppedTrigger.SyncAtomNames();
		}
	}

	// Token: 0x0600666D RID: 26221 RVA: 0x0026B7B3 File Offset: 0x00269BB3
	protected void ClearImage()
	{
		if (this.blankTexture != null)
		{
			this.SyncTexture(this.blankTexture);
		}
		else
		{
			this.SyncTexture(null);
		}
	}

	// Token: 0x0600666E RID: 26222 RVA: 0x0026B7E0 File Offset: 0x00269BE0
	private IEnumerator SyncImage()
	{
		Texture2D tex = new Texture2D(4, 4, TextureFormat.DXT5, true);
		string urltoload = this._url;
		if (!Regex.IsMatch(urltoload, "^http") && !Regex.IsMatch(urltoload, "^file"))
		{
			if (urltoload.Contains(":/"))
			{
				urltoload = "file:///" + urltoload;
			}
			else
			{
				urltoload = "file:///.\\" + urltoload;
			}
		}
		WWW www = new WWW(urltoload);
		yield return www;
		if (www.error == null || www.error == string.Empty)
		{
			www.LoadImageIntoTexture(tex);
			if (this.createdTexture != null)
			{
				UnityEngine.Object.Destroy(this.createdTexture);
			}
			this.createdTexture = tex;
			this.SyncTexture(tex);
		}
		else
		{
			SuperController.LogError("Could not load image at " + urltoload + " Error: " + www.error);
		}
		yield break;
	}

	// Token: 0x0600666F RID: 26223 RVA: 0x0026B7FC File Offset: 0x00269BFC
	protected void OnImageLoaded(ImageLoaderThreaded.QueuedImage qi)
	{
		if (qi.tex != null && this != null)
		{
			this.SyncTexture(qi.tex);
			ImageLoaderThreaded.singleton.RegisterTextureUse(qi.tex);
			if (this.registeredTexture != null)
			{
				ImageLoaderThreaded.singleton.DeregisterTextureUse(this.registeredTexture);
			}
			this.registeredTexture = qi.tex;
		}
	}

	// Token: 0x06006670 RID: 26224 RVA: 0x0026B871 File Offset: 0x00269C71
	protected void SyncAllowTiling()
	{
		if (this.currentTexture != null)
		{
			if (this._allowTiling)
			{
				this.currentTexture.wrapMode = TextureWrapMode.Repeat;
			}
			else
			{
				this.currentTexture.wrapMode = TextureWrapMode.Clamp;
			}
		}
	}

	// Token: 0x06006671 RID: 26225 RVA: 0x0026B8AC File Offset: 0x00269CAC
	protected void SyncAllowTiling(bool b)
	{
		this._allowTiling = b;
		this.SyncAllowTiling();
	}

	// Token: 0x06006672 RID: 26226 RVA: 0x0026B8BC File Offset: 0x00269CBC
	protected void Init()
	{
		this.videoReadyTrigger = new Trigger();
		this.videoReadyTrigger.handler = this;
		this.videoStartedTrigger = new Trigger();
		this.videoStartedTrigger.handler = this;
		this.videoStoppedTrigger = new Trigger();
		this.videoStoppedTrigger.handler = this;
		this.InitVideoPlayer();
		if (!FileManager.DirectoryExists("Custom/Images", false, false))
		{
			FileManager.CreateDirectory("Custom/Images");
		}
		if (SuperController.singleton)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
		this.allowTilingJSON = new JSONStorableBool("allowTiling", this._allowTiling, new JSONStorableBool.SetBoolCallback(this.SyncAllowTiling));
		base.RegisterBool(this.allowTilingJSON);
		this.playVideoWhenReadyJSON = new JSONStorableBool("playVideoWhenReady", this._playVideoWhenReady, new JSONStorableBool.SetBoolCallback(this.SyncPlayVideoWhenReady));
		base.RegisterBool(this.playVideoWhenReadyJSON);
		this.useAnamorphicVideoAspectRatioJSON = new JSONStorableBool("useAnamorphicVideoAspectRatio", this._useAnamorphicVideoAspectRatio, new JSONStorableBool.SetBoolCallback(this.SyncUseAnamorphicVideoAspectRatio));
		base.RegisterBool(this.useAnamorphicVideoAspectRatioJSON);
		this.videoIsReadyJSON = new JSONStorableBool("videoIsReady", false);
		this.videoIsLoadingJSON = new JSONStorableBool("videoIsLoading", false);
		this.videoIsLoadingJSON.RegisterIndicator(this.videoIsLoadingIndicator, true);
		this.videoHadErrorJSON = new JSONStorableBool("videoHadError", false);
		this.videoHadErrorJSON.RegisterIndicator(this.videoHadErrorIndicator, true);
		this.loopVideoJSON = new JSONStorableBool("loopVideo", this._loopVideo, new JSONStorableBool.SetBoolCallback(this.SyncLoopVideo));
		this.SyncLoopVideo(this._loopVideo);
		base.RegisterBool(this.loopVideoJSON);
		this.playbackTimeJSON = new JSONStorableFloat("playbackTime", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncPlaybackTime), 0f, 0f, true, true);
		this.playbackTimeJSON.isRestorable = false;
		this.playbackTimeJSON.isStorable = false;
		base.RegisterFloat(this.playbackTimeJSON);
		this.playVideoAction = new JSONStorableAction("PlayVideo", new JSONStorableAction.ActionCallback(this.PlayVideo));
		base.RegisterAction(this.playVideoAction);
		this.pauseVideoAction = new JSONStorableAction("PauseVideo", new JSONStorableAction.ActionCallback(this.PauseVideo));
		base.RegisterAction(this.pauseVideoAction);
		this.stopVideoAction = new JSONStorableAction("StopVideo", new JSONStorableAction.ActionCallback(this.StopVideo));
		base.RegisterAction(this.stopVideoAction);
		this.seekToVideoStartAction = new JSONStorableAction("SeekToVideoStart", new JSONStorableAction.ActionCallback(this.SeekToVideoStart));
		base.RegisterAction(this.seekToVideoStartAction);
		if (this.videoPlayer != null)
		{
			this.urlJSON = new JSONStorableUrl("url", this._url, new JSONStorableString.SetStringCallback(this.SyncUrl), "jpg|jpeg|png|gif|avi|mp4", "Custom/Images");
		}
		else
		{
			this.urlJSON = new JSONStorableUrl("url", this._url, new JSONStorableString.SetStringCallback(this.SyncUrl), "jpg|jpeg|png|gif", "Custom/Images");
		}
		this.urlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		this.urlJSON.disableOnEndEdit = true;
		base.RegisterUrl(this.urlJSON);
	}

	// Token: 0x06006673 RID: 26227 RVA: 0x0026BC10 File Offset: 0x0026A010
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			ImageControlUI componentInChildren = t.GetComponentInChildren<ImageControlUI>(true);
			if (componentInChildren != null)
			{
				this.urlJSON.RegisterInputField(componentInChildren.urlInputField, isAlt);
				this.urlJSON.RegisterInputFieldAction(componentInChildren.urlInputFieldAction, isAlt);
				this.urlJSON.RegisterCopyToClipboardButton(componentInChildren.copyToClipboardButton, isAlt);
				this.urlJSON.RegisterCopyFromClipboardButton(componentInChildren.copyFromClipboardButton, isAlt);
				this.urlJSON.RegisterFileBrowseButton(componentInChildren.fileBrowseButton, isAlt);
				this.urlJSON.RegisterSetValToInputFieldButton(componentInChildren.loadButton, isAlt);
				this.urlJSON.RegisterClearValButton(componentInChildren.clearUrlButton, isAlt);
				this.allowTilingJSON.RegisterToggle(componentInChildren.allowImageTilingToggle, isAlt);
				this.playVideoWhenReadyJSON.RegisterToggle(componentInChildren.playVideoWhenReadyToggle, isAlt);
				this.useAnamorphicVideoAspectRatioJSON.RegisterToggle(componentInChildren.useAnamorphicVideoAspectRatioToggle, isAlt);
				this.videoIsReadyJSON.RegisterIndicator(componentInChildren.videoIsReadyIndicator, isAlt);
				this.videoIsLoadingJSON.RegisterIndicator(componentInChildren.videoIsLoadingIndicator, isAlt);
				this.videoHadErrorJSON.RegisterIndicator(componentInChildren.videoHadErrorIndicator, isAlt);
				this.loopVideoJSON.RegisterToggle(componentInChildren.loopVideoToggle, isAlt);
				this.playbackTimeJSON.RegisterSlider(componentInChildren.playbackTimeSilder, isAlt);
				this.playVideoAction.RegisterButton(componentInChildren.playVideoButton, isAlt);
				this.pauseVideoAction.RegisterButton(componentInChildren.pauseVideoButton, isAlt);
				this.stopVideoAction.RegisterButton(componentInChildren.stopVideoButton, isAlt);
				this.seekToVideoStartAction.RegisterButton(componentInChildren.seekToVideoStartButton, isAlt);
			}
			VideoReadyTriggerUI componentInChildren2 = t.GetComponentInChildren<VideoReadyTriggerUI>(true);
			if (componentInChildren2 != null)
			{
				this.videoReadyTrigger.triggerActionsParent = componentInChildren2.transform;
				this.videoReadyTrigger.triggerPanel = componentInChildren2.transform;
				this.videoReadyTrigger.triggerActionsPanel = componentInChildren2.transform;
				this.videoReadyTrigger.InitTriggerUI();
				this.videoReadyTrigger.InitTriggerActionsUI();
			}
			VideoStartedTriggerUI componentInChildren3 = t.GetComponentInChildren<VideoStartedTriggerUI>(true);
			if (componentInChildren3 != null)
			{
				this.videoStartedTrigger.triggerActionsParent = componentInChildren3.transform;
				this.videoStartedTrigger.triggerPanel = componentInChildren3.transform;
				this.videoStartedTrigger.triggerActionsPanel = componentInChildren3.transform;
				this.videoStartedTrigger.InitTriggerUI();
				this.videoStartedTrigger.InitTriggerActionsUI();
			}
			VideoStoppedTriggerUI componentInChildren4 = t.GetComponentInChildren<VideoStoppedTriggerUI>(true);
			if (componentInChildren4 != null)
			{
				this.videoStoppedTrigger.triggerActionsParent = componentInChildren4.transform;
				this.videoStoppedTrigger.triggerPanel = componentInChildren4.transform;
				this.videoStoppedTrigger.triggerActionsPanel = componentInChildren4.transform;
				this.videoStoppedTrigger.InitTriggerUI();
				this.videoStoppedTrigger.InitTriggerActionsUI();
			}
		}
	}

	// Token: 0x06006674 RID: 26228 RVA: 0x0026BEAC File Offset: 0x0026A2AC
	protected void CheckEnable()
	{
		bool flag = true;
		if (UserPreferences.singleton != null)
		{
			flag = UserPreferences.singleton.enableWebMisc;
		}
		if (!this.wasWebEnabled && flag)
		{
			this.SyncUrl(this._url);
		}
		this.wasWebEnabled = flag;
	}

	// Token: 0x06006675 RID: 26229 RVA: 0x0026BEFA File Offset: 0x0026A2FA
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x06006676 RID: 26230 RVA: 0x0026BF1F File Offset: 0x0026A31F
	protected void OnEnable()
	{
		if (this.notActiveOnSync)
		{
			this.notActiveOnSync = false;
			this.StartSyncUrl();
		}
	}

	// Token: 0x06006677 RID: 26231 RVA: 0x0026BF39 File Offset: 0x0026A339
	protected void OnDisable()
	{
	}

	// Token: 0x06006678 RID: 26232 RVA: 0x0026BF3C File Offset: 0x0026A33C
	protected void Update()
	{
		this.CheckEnable();
		if (this.videoReadyTrigger != null)
		{
			this.videoReadyTrigger.Update();
		}
		if (this.videoStartedTrigger != null)
		{
			this.videoStartedTrigger.Update();
		}
		if (this.videoStoppedTrigger != null)
		{
			this.videoStoppedTrigger.Update();
		}
		this.UpdateVideo();
	}

	// Token: 0x06006679 RID: 26233 RVA: 0x0026BF98 File Offset: 0x0026A398
	protected void OnDestroy()
	{
		this.DeleteTempVideo();
		if (this.createdTexture != null)
		{
			UnityEngine.Object.Destroy(this.createdTexture);
		}
		if (this.registeredTexture != null)
		{
			ImageLoaderThreaded.singleton.DeregisterTextureUse(this.registeredTexture);
		}
		if (this.videoReadyTrigger != null)
		{
			this.videoReadyTrigger.Remove();
		}
		if (this.videoStartedTrigger != null)
		{
			this.videoStartedTrigger.Remove();
		}
		if (this.videoStoppedTrigger != null)
		{
			this.videoStoppedTrigger.Remove();
		}
		if (SuperController.singleton)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x0600667A RID: 26234 RVA: 0x0026C060 File Offset: 0x0026A460
	public void Start()
	{
		this.StartSyncUrl();
	}

	// Token: 0x040055D6 RID: 21974
	public GameObject nonWhitelistSiteObject;

	// Token: 0x040055D7 RID: 21975
	public Text nonWhitelistSiteText;

	// Token: 0x040055D8 RID: 21976
	public GameObject webDisabledObject;

	// Token: 0x040055D9 RID: 21977
	protected bool notActiveOnSync;

	// Token: 0x040055DA RID: 21978
	public Transform targetTransformForScale;

	// Token: 0x040055DB RID: 21979
	public Transform transformForScaling;

	// Token: 0x040055DC RID: 21980
	protected JSONStorableUrl urlJSON;

	// Token: 0x040055DD RID: 21981
	[SerializeField]
	protected string _url;

	// Token: 0x040055DE RID: 21982
	public bool matchScaleToImageRatio = true;

	// Token: 0x040055DF RID: 21983
	protected Texture2D currentTexture;

	// Token: 0x040055E0 RID: 21984
	public Texture2D blankTexture;

	// Token: 0x040055E1 RID: 21985
	public RawImage rawImage;

	// Token: 0x040055E2 RID: 21986
	public bool alsoSetSpecularTexture = true;

	// Token: 0x040055E3 RID: 21987
	protected Texture currentVideoTexture;

	// Token: 0x040055E4 RID: 21988
	protected VideoPlayer videoPlayer;

	// Token: 0x040055E5 RID: 21989
	protected string currentTempVideo;

	// Token: 0x040055E6 RID: 21990
	protected JSONStorableBool playVideoWhenReadyJSON;

	// Token: 0x040055E7 RID: 21991
	[SerializeField]
	protected bool _playVideoWhenReady = true;

	// Token: 0x040055E8 RID: 21992
	protected JSONStorableBool useAnamorphicVideoAspectRatioJSON;

	// Token: 0x040055E9 RID: 21993
	protected bool _useAnamorphicVideoAspectRatio;

	// Token: 0x040055EA RID: 21994
	public GameObject videoIsLoadingIndicator;

	// Token: 0x040055EB RID: 21995
	public GameObject videoHadErrorIndicator;

	// Token: 0x040055EC RID: 21996
	protected JSONStorableBool videoIsLoadingJSON;

	// Token: 0x040055ED RID: 21997
	protected JSONStorableBool videoIsReadyJSON;

	// Token: 0x040055EE RID: 21998
	protected JSONStorableBool videoHadErrorJSON;

	// Token: 0x040055EF RID: 21999
	protected JSONStorableBool loopVideoJSON;

	// Token: 0x040055F0 RID: 22000
	[SerializeField]
	protected bool _loopVideo;

	// Token: 0x040055F1 RID: 22001
	protected JSONStorableFloat playbackTimeJSON;

	// Token: 0x040055F2 RID: 22002
	protected bool wasPlaying;

	// Token: 0x040055F3 RID: 22003
	protected JSONStorableAction playVideoAction;

	// Token: 0x040055F4 RID: 22004
	protected JSONStorableAction pauseVideoAction;

	// Token: 0x040055F5 RID: 22005
	protected JSONStorableAction stopVideoAction;

	// Token: 0x040055F6 RID: 22006
	protected JSONStorableAction seekToVideoStartAction;

	// Token: 0x040055F7 RID: 22007
	public Trigger videoReadyTrigger;

	// Token: 0x040055F8 RID: 22008
	public Trigger videoStartedTrigger;

	// Token: 0x040055F9 RID: 22009
	public Trigger videoStoppedTrigger;

	// Token: 0x040055FA RID: 22010
	protected Texture2D createdTexture;

	// Token: 0x040055FB RID: 22011
	protected Texture2D registeredTexture;

	// Token: 0x040055FC RID: 22012
	protected bool _allowTiling;

	// Token: 0x040055FD RID: 22013
	protected JSONStorableBool allowTilingJSON;

	// Token: 0x040055FE RID: 22014
	protected bool wasWebEnabled;

	// Token: 0x02001035 RID: 4149
	[CompilerGenerated]
	private sealed class <SyncImage>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600776B RID: 30571 RVA: 0x0026C068 File Offset: 0x0026A468
		[DebuggerHidden]
		public <SyncImage>c__Iterator0()
		{
		}

		// Token: 0x0600776C RID: 30572 RVA: 0x0026C070 File Offset: 0x0026A470
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				tex = new Texture2D(4, 4, TextureFormat.DXT5, true);
				urltoload = this._url;
				if (!Regex.IsMatch(urltoload, "^http") && !Regex.IsMatch(urltoload, "^file"))
				{
					if (urltoload.Contains(":/"))
					{
						urltoload = "file:///" + urltoload;
					}
					else
					{
						urltoload = "file:///.\\" + urltoload;
					}
				}
				www = new WWW(urltoload);
				this.$current = www;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				if (www.error == null || www.error == string.Empty)
				{
					www.LoadImageIntoTexture(tex);
					if (this.createdTexture != null)
					{
						UnityEngine.Object.Destroy(this.createdTexture);
					}
					this.createdTexture = tex;
					base.SyncTexture(tex);
				}
				else
				{
					SuperController.LogError("Could not load image at " + urltoload + " Error: " + www.error);
				}
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x0600776D RID: 30573 RVA: 0x0026C217 File Offset: 0x0026A617
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x0600776E RID: 30574 RVA: 0x0026C21F File Offset: 0x0026A61F
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600776F RID: 30575 RVA: 0x0026C227 File Offset: 0x0026A627
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007770 RID: 30576 RVA: 0x0026C237 File Offset: 0x0026A637
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006B72 RID: 27506
		internal Texture2D <tex>__0;

		// Token: 0x04006B73 RID: 27507
		internal string <urltoload>__0;

		// Token: 0x04006B74 RID: 27508
		internal WWW <www>__0;

		// Token: 0x04006B75 RID: 27509
		internal ImageControl $this;

		// Token: 0x04006B76 RID: 27510
		internal object $current;

		// Token: 0x04006B77 RID: 27511
		internal bool $disposing;

		// Token: 0x04006B78 RID: 27512
		internal int $PC;
	}
}
