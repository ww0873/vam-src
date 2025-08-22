using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using MVR.FileManagement;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Profiling;
using UnityEngine.UI;

// Token: 0x02000D19 RID: 3353
public class ImageLoaderThreaded : MonoBehaviour
{
	// Token: 0x0600667E RID: 26238 RVA: 0x0026C25A File Offset: 0x0026A65A
	public ImageLoaderThreaded()
	{
	}

	// Token: 0x0600667F RID: 26239 RVA: 0x0026C264 File Offset: 0x0026A664
	protected void MTTask(object info)
	{
		ImageLoaderThreaded.ImageLoaderTaskInfo imageLoaderTaskInfo = (ImageLoaderThreaded.ImageLoaderTaskInfo)info;
		while (this._threadsRunning)
		{
			imageLoaderTaskInfo.resetEvent.WaitOne(-1, true);
			if (imageLoaderTaskInfo.kill)
			{
				break;
			}
			this.ProcessImageQueueThreaded();
			imageLoaderTaskInfo.working = false;
		}
	}

	// Token: 0x06006680 RID: 26240 RVA: 0x0026C2B8 File Offset: 0x0026A6B8
	protected void StopThreads()
	{
		this._threadsRunning = false;
		if (this.imageLoaderTask != null)
		{
			this.imageLoaderTask.kill = true;
			this.imageLoaderTask.resetEvent.Set();
			while (this.imageLoaderTask.thread.IsAlive)
			{
			}
			this.imageLoaderTask = null;
		}
	}

	// Token: 0x06006681 RID: 26241 RVA: 0x0026C318 File Offset: 0x0026A718
	protected void StartThreads()
	{
		if (!this._threadsRunning)
		{
			this._threadsRunning = true;
			this.imageLoaderTask = new ImageLoaderThreaded.ImageLoaderTaskInfo();
			this.imageLoaderTask.name = "ImageLoaderTask";
			this.imageLoaderTask.resetEvent = new AutoResetEvent(false);
			this.imageLoaderTask.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.imageLoaderTask.thread.Priority = System.Threading.ThreadPriority.Normal;
			this.imageLoaderTask.thread.Start(this.imageLoaderTask);
		}
	}

	// Token: 0x06006682 RID: 26242 RVA: 0x0026C3A8 File Offset: 0x0026A7A8
	public bool RegisterTextureUse(Texture2D tex)
	{
		if (this.textureTrackedCache.ContainsKey(tex))
		{
			int num = 0;
			if (this.textureUseCount.TryGetValue(tex, out num))
			{
				this.textureUseCount.Remove(tex);
			}
			num++;
			this.textureUseCount.Add(tex, num);
			return true;
		}
		return false;
	}

	// Token: 0x06006683 RID: 26243 RVA: 0x0026C3FC File Offset: 0x0026A7FC
	public bool DeregisterTextureUse(Texture2D tex)
	{
		int num = 0;
		if (this.textureUseCount.TryGetValue(tex, out num))
		{
			this.textureUseCount.Remove(tex);
			num--;
			if (num > 0)
			{
				this.textureUseCount.Add(tex, num);
			}
			else
			{
				this.textureUseCount.Remove(tex);
				this.textureCache.Remove(tex.name);
				this.textureTrackedCache.Remove(tex);
				UnityEngine.Object.Destroy(tex);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06006684 RID: 26244 RVA: 0x0026C480 File Offset: 0x0026A880
	public void ReportOnTextures()
	{
		int num = 0;
		if (this.textureCache != null)
		{
			foreach (Texture2D texture2D in this.textureCache.Values)
			{
				num++;
				int num2 = 0;
				if (this.textureUseCount.TryGetValue(texture2D, out num2))
				{
					SuperController.LogMessage(string.Concat(new object[]
					{
						"Texture ",
						texture2D.name,
						" is in use ",
						num2,
						" times"
					}));
				}
			}
		}
		SuperController.LogMessage("Using " + num + " textures");
	}

	// Token: 0x06006685 RID: 26245 RVA: 0x0026C554 File Offset: 0x0026A954
	protected void ReportMemory(List<string> reports)
	{
		int num = 0;
		long num2 = 0L;
		if (this.textureCache != null)
		{
			foreach (Texture2D o in this.textureCache.Values)
			{
				num++;
				if (Debug.isDebugBuild)
				{
					num2 += Profiler.GetRuntimeMemorySizeLong(o);
				}
			}
		}
		reports.Add("Textures cached: " + num);
		if (Debug.isDebugBuild)
		{
			reports.Add("Textures memory usage: " + ((float)num2 * MemoryOptimizer.GByteMult).ToString("F2") + " GB");
		}
		int num3 = 0;
		long num4 = 0L;
		if (this.thumbnailCache != null)
		{
			foreach (Texture2D o2 in this.thumbnailCache.Values)
			{
				num3++;
				if (Debug.isDebugBuild)
				{
					num4 += Profiler.GetRuntimeMemorySizeLong(o2);
				}
			}
		}
		reports.Add("Thumbnails cached: " + num3);
		if (Debug.isDebugBuild)
		{
			reports.Add("Thumbnails memory usage: " + ((float)num4 * MemoryOptimizer.GByteMult).ToString("F2") + " GB");
		}
	}

	// Token: 0x06006686 RID: 26246 RVA: 0x0026C6E8 File Offset: 0x0026AAE8
	public void OptimizeMemory()
	{
	}

	// Token: 0x06006687 RID: 26247 RVA: 0x0026C6EC File Offset: 0x0026AAEC
	public void PurgeAllTextures()
	{
		if (this.textureCache != null)
		{
			foreach (Texture2D obj in this.textureCache.Values)
			{
				UnityEngine.Object.Destroy(obj);
			}
			this.textureUseCount.Clear();
			this.textureCache.Clear();
			this.textureTrackedCache.Clear();
		}
	}

	// Token: 0x06006688 RID: 26248 RVA: 0x0026C778 File Offset: 0x0026AB78
	public void PurgeAllImmediateTextures()
	{
		if (this.immediateTextureCache != null)
		{
			foreach (Texture2D obj in this.immediateTextureCache.Values)
			{
				UnityEngine.Object.Destroy(obj);
			}
			this.immediateTextureCache.Clear();
		}
	}

	// Token: 0x06006689 RID: 26249 RVA: 0x0026C7F0 File Offset: 0x0026ABF0
	public void PurgeAllThumbnailTextures()
	{
		if (this.thumbnailCache != null)
		{
			foreach (Texture2D obj in this.thumbnailCache.Values)
			{
				UnityEngine.Object.Destroy(obj);
			}
			this.thumbnailCache.Clear();
		}
	}

	// Token: 0x0600668A RID: 26250 RVA: 0x0026C868 File Offset: 0x0026AC68
	public void ClearCacheThumbnail(string imgPath)
	{
		Texture2D obj;
		if (this.thumbnailCache != null && this.thumbnailCache.TryGetValue(imgPath, out obj))
		{
			this.thumbnailCache.Remove(imgPath);
			UnityEngine.Object.Destroy(obj);
		}
	}

	// Token: 0x0600668B RID: 26251 RVA: 0x0026C8A8 File Offset: 0x0026ACA8
	protected void ProcessImageQueueThreaded()
	{
		if (this.queuedImages != null && this.queuedImages.Count > 0)
		{
			ImageLoaderThreaded.QueuedImage value = this.queuedImages.First.Value;
			value.Process();
		}
	}

	// Token: 0x0600668C RID: 26252 RVA: 0x0026C8F0 File Offset: 0x0026ACF0
	public Texture2D GetCachedThumbnail(string path)
	{
		Texture2D result;
		if (this.thumbnailCache != null && this.thumbnailCache.TryGetValue(path, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x0600668D RID: 26253 RVA: 0x0026C91E File Offset: 0x0026AD1E
	public void QueueImage(ImageLoaderThreaded.QueuedImage qi)
	{
		if (this.queuedImages != null)
		{
			this.queuedImages.AddLast(qi);
		}
		this.numRealQueuedImages++;
		this.progressMax++;
	}

	// Token: 0x0600668E RID: 26254 RVA: 0x0026C958 File Offset: 0x0026AD58
	public void QueueThumbnail(ImageLoaderThreaded.QueuedImage qi)
	{
		qi.isThumbnail = true;
		if (this.queuedImages != null)
		{
			this.queuedImages.AddLast(qi);
		}
	}

	// Token: 0x0600668F RID: 26255 RVA: 0x0026C980 File Offset: 0x0026AD80
	public void QueueThumbnailImmediate(ImageLoaderThreaded.QueuedImage qi)
	{
		qi.isThumbnail = true;
		if (this.queuedImages != null)
		{
			if (this.queuedImages.Count > 0)
			{
				LinkedListNode<ImageLoaderThreaded.QueuedImage> first = this.queuedImages.First;
				this.queuedImages.AddAfter(first, qi);
			}
			else
			{
				this.queuedImages.AddLast(qi);
			}
		}
	}

	// Token: 0x06006690 RID: 26256 RVA: 0x0026C9E8 File Offset: 0x0026ADE8
	public void ProcessImageImmediate(ImageLoaderThreaded.QueuedImage qi)
	{
		Texture2D tex;
		if (!qi.skipCache && this.immediateTextureCache != null && this.immediateTextureCache.TryGetValue(qi.cacheSignature, out tex))
		{
			this.UseCachedTex(qi, tex);
		}
		qi.Process();
		qi.Finish();
		if (!qi.skipCache && !this.immediateTextureCache.ContainsKey(qi.cacheSignature) && qi.tex != null)
		{
			this.immediateTextureCache.Add(qi.cacheSignature, qi.tex);
		}
	}

	// Token: 0x06006691 RID: 26257 RVA: 0x0026CA80 File Offset: 0x0026AE80
	protected void PostProcessImageQueue()
	{
		if (this.queuedImages != null && this.queuedImages.Count > 0)
		{
			ImageLoaderThreaded.QueuedImage value = this.queuedImages.First.Value;
			if (value.processed)
			{
				this.queuedImages.RemoveFirst();
				if (!value.isThumbnail)
				{
					this.progress++;
					this.numRealQueuedImages--;
					if (this.numRealQueuedImages == 0)
					{
						this.progress = 0;
						this.progressMax = 0;
						if (this.progressHUD != null)
						{
							this.progressHUD.SetActive(false);
						}
					}
					else
					{
						if (this.progressHUD != null)
						{
							this.progressHUD.SetActive(true);
						}
						if (this.progressSlider != null)
						{
							this.progressSlider.maxValue = (float)this.progressMax;
							this.progressSlider.value = (float)this.progress;
						}
					}
				}
				value.Finish();
				if (!value.skipCache && value.imgPath != null && value.imgPath != "NULL")
				{
					if (value.isThumbnail)
					{
						if (!this.thumbnailCache.ContainsKey(value.imgPath) && value.tex != null)
						{
							this.thumbnailCache.Add(value.imgPath, value.tex);
						}
					}
					else if (!this.textureCache.ContainsKey(value.cacheSignature) && value.tex != null)
					{
						this.textureCache.Add(value.cacheSignature, value.tex);
						this.textureTrackedCache.Add(value.tex, true);
					}
				}
				value.DoCallback();
			}
			if (this.numRealQueuedImages != 0)
			{
				if (this.loadFlag == null)
				{
					this.loadFlag = new AsyncFlag("ImageLoader");
					SuperController.singleton.SetLoadingIconFlag(this.loadFlag);
				}
			}
			else if (this.loadFlag != null)
			{
				this.loadFlag.Raise();
				this.loadFlag = null;
			}
		}
	}

	// Token: 0x06006692 RID: 26258 RVA: 0x0026CCBC File Offset: 0x0026B0BC
	protected void UseCachedTex(ImageLoaderThreaded.QueuedImage qi, Texture2D tex)
	{
		qi.tex = tex;
		if (qi.forceReload)
		{
			qi.width = tex.width;
			qi.height = tex.height;
			qi.setSize = true;
			qi.fillBackground = false;
		}
		else
		{
			qi.processed = true;
			qi.finished = true;
		}
	}

	// Token: 0x06006693 RID: 26259 RVA: 0x0026CD14 File Offset: 0x0026B114
	protected void RemoveCanceledImages()
	{
		if (this.queuedImages != null)
		{
			while (this.queuedImages.Count > 0 && this.queuedImages.First.Value.cancel)
			{
				this.queuedImages.RemoveFirst();
			}
		}
	}

	// Token: 0x06006694 RID: 26260 RVA: 0x0026CD70 File Offset: 0x0026B170
	protected void PreprocessImageQueue()
	{
		this.RemoveCanceledImages();
		if (this.queuedImages != null && this.queuedImages.Count > 0)
		{
			ImageLoaderThreaded.QueuedImage value = this.queuedImages.First.Value;
			if (value != null)
			{
				if (!value.skipCache && value.imgPath != null && value.imgPath != "NULL")
				{
					Texture2D texture2D;
					if (value.isThumbnail)
					{
						if (this.thumbnailCache != null && this.thumbnailCache.TryGetValue(value.imgPath, out texture2D))
						{
							if (texture2D == null)
							{
								Debug.LogError("Trying to use cached texture at " + value.imgPath + " after it has been destroyed");
								this.thumbnailCache.Remove(value.imgPath);
							}
							else
							{
								this.UseCachedTex(value, texture2D);
							}
						}
					}
					else if (this.textureCache != null && this.textureCache.TryGetValue(value.cacheSignature, out texture2D))
					{
						if (texture2D == null)
						{
							Debug.LogError("Trying to use cached texture at " + value.imgPath + " after it has been destroyed");
							this.textureCache.Remove(value.cacheSignature);
							this.textureTrackedCache.Remove(texture2D);
						}
						else
						{
							this.UseCachedTex(value, texture2D);
						}
					}
				}
				if (!value.processed && value.imgPath != null && Regex.IsMatch(value.imgPath, "^http"))
				{
					if (CacheManager.CachingEnabled && value.WebCachePathExists())
					{
						value.useWebCache = true;
					}
					else
					{
						if (value.webRequest == null)
						{
							value.webRequest = UnityWebRequest.Get(value.imgPath);
							value.webRequest.SendWebRequest();
						}
						if (value.webRequest.isDone)
						{
							if (!value.webRequest.isNetworkError)
							{
								if (value.webRequest.responseCode == 200L)
								{
									value.webRequestData = value.webRequest.downloadHandler.data;
									value.webRequestDone = true;
								}
								else
								{
									value.webRequestHadError = true;
									value.webRequestDone = true;
									value.hadError = true;
									value.errorText = "Error " + value.webRequest.responseCode;
								}
							}
							else
							{
								value.webRequestHadError = true;
								value.webRequestDone = true;
								value.hadError = true;
								value.errorText = value.webRequest.error;
							}
						}
					}
				}
				if (!value.isThumbnail && this.progressText != null)
				{
					this.progressText.text = string.Concat(new object[]
					{
						"[",
						this.progress,
						"/",
						this.progressMax,
						"] ",
						value.imgPath
					});
				}
			}
		}
	}

	// Token: 0x06006695 RID: 26261 RVA: 0x0026D070 File Offset: 0x0026B470
	private void Update()
	{
		this.StartThreads();
		if (!this.imageLoaderTask.working)
		{
			this.PostProcessImageQueue();
			if (this.queuedImages != null && this.queuedImages.Count > 0)
			{
				this.PreprocessImageQueue();
				this.imageLoaderTask.working = true;
				this.imageLoaderTask.resetEvent.Set();
			}
		}
	}

	// Token: 0x06006696 RID: 26262 RVA: 0x0026D0E0 File Offset: 0x0026B4E0
	protected void OnDestroy()
	{
		if (Application.isPlaying)
		{
			this.StopThreads();
		}
		if (this.loadFlag != null)
		{
			this.loadFlag.Raise();
		}
		this.PurgeAllTextures();
		this.PurgeAllImmediateTextures();
		if (MemoryOptimizer.singleton != null)
		{
			MemoryOptimizer.singleton.DeregisterMemoryOptimizerReporter(new MemoryOptimizer.MemoryOptimizerReporter(this.ReportMemory));
			MemoryOptimizer.singleton.DeregisterMemoryOptimizerListener(new MemoryOptimizer.MemoryOptimizerCallback(this.OptimizeMemory));
		}
	}

	// Token: 0x06006697 RID: 26263 RVA: 0x0026D15B File Offset: 0x0026B55B
	protected void OnApplicationQuit()
	{
		if (Application.isPlaying)
		{
			this.StopThreads();
		}
	}

	// Token: 0x06006698 RID: 26264 RVA: 0x0026D16D File Offset: 0x0026B56D
	private void Start()
	{
		if (MemoryOptimizer.singleton != null)
		{
			MemoryOptimizer.singleton.RegisterMemoryOptimizerReporter(new MemoryOptimizer.MemoryOptimizerReporter(this.ReportMemory));
			MemoryOptimizer.singleton.RegisterMemoryOptimizerListener(new MemoryOptimizer.MemoryOptimizerCallback(this.OptimizeMemory));
		}
	}

	// Token: 0x06006699 RID: 26265 RVA: 0x0026D1AC File Offset: 0x0026B5AC
	private void Awake()
	{
		ImageLoaderThreaded.singleton = this;
		this.immediateTextureCache = new Dictionary<string, Texture2D>();
		this.textureCache = new Dictionary<string, Texture2D>();
		this.textureTrackedCache = new Dictionary<Texture2D, bool>();
		this.thumbnailCache = new Dictionary<string, Texture2D>();
		this.textureUseCount = new Dictionary<Texture2D, int>();
		this.queuedImages = new LinkedList<ImageLoaderThreaded.QueuedImage>();
	}

	// Token: 0x04005613 RID: 22035
	public static ImageLoaderThreaded singleton;

	// Token: 0x04005614 RID: 22036
	public GameObject progressHUD;

	// Token: 0x04005615 RID: 22037
	public Slider progressSlider;

	// Token: 0x04005616 RID: 22038
	public Text progressText;

	// Token: 0x04005617 RID: 22039
	protected ImageLoaderThreaded.ImageLoaderTaskInfo imageLoaderTask;

	// Token: 0x04005618 RID: 22040
	protected bool _threadsRunning;

	// Token: 0x04005619 RID: 22041
	protected Dictionary<string, Texture2D> thumbnailCache;

	// Token: 0x0400561A RID: 22042
	protected Dictionary<string, Texture2D> textureCache;

	// Token: 0x0400561B RID: 22043
	protected Dictionary<string, Texture2D> immediateTextureCache;

	// Token: 0x0400561C RID: 22044
	protected Dictionary<Texture2D, bool> textureTrackedCache;

	// Token: 0x0400561D RID: 22045
	protected Dictionary<Texture2D, int> textureUseCount;

	// Token: 0x0400561E RID: 22046
	protected volatile LinkedList<ImageLoaderThreaded.QueuedImage> queuedImages;

	// Token: 0x0400561F RID: 22047
	protected int numRealQueuedImages;

	// Token: 0x04005620 RID: 22048
	protected int progress;

	// Token: 0x04005621 RID: 22049
	protected int progressMax;

	// Token: 0x04005622 RID: 22050
	protected AsyncFlag loadFlag;

	// Token: 0x02000D1A RID: 3354
	// (Invoke) Token: 0x0600669B RID: 26267
	public delegate void ImageLoaderCallback(ImageLoaderThreaded.QueuedImage qi);

	// Token: 0x02000D1B RID: 3355
	public class QueuedImage
	{
		// Token: 0x0600669E RID: 26270 RVA: 0x0016F177 File Offset: 0x0016D577
		public QueuedImage()
		{
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x0600669F RID: 26271 RVA: 0x0016F194 File Offset: 0x0016D594
		public string cacheSignature
		{
			get
			{
				string text = this.imgPath;
				if (this.compress)
				{
					text += ":C";
				}
				if (this.linear)
				{
					text += ":L";
				}
				if (this.isNormalMap)
				{
					text += ":N";
				}
				if (this.createAlphaFromGrayscale)
				{
					text += ":A";
				}
				if (this.createNormalFromBump)
				{
					text = text + ":BN" + this.bumpStrength;
				}
				if (this.invert)
				{
					text += ":I";
				}
				return text;
			}
		}

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x060066A0 RID: 26272 RVA: 0x0016F240 File Offset: 0x0016D640
		protected string diskCacheSignature
		{
			get
			{
				string text;
				if (this.setSize)
				{
					text = this.width + "_" + this.height;
				}
				else
				{
					text = string.Empty;
				}
				if (this.compress)
				{
					text += "_C";
				}
				if (this.linear)
				{
					text += "_L";
				}
				if (this.isNormalMap)
				{
					text += "_N";
				}
				if (this.createAlphaFromGrayscale)
				{
					text += "_A";
				}
				if (this.createNormalFromBump)
				{
					text = text + "_BN" + this.bumpStrength;
				}
				if (this.invert)
				{
					text += "_I";
				}
				return text;
			}
		}

		// Token: 0x060066A1 RID: 26273 RVA: 0x0016F31C File Offset: 0x0016D71C
		protected string GetDiskCachePath()
		{
			string result = null;
			FileEntry fileEntry = FileManager.GetFileEntry(this.imgPath, false);
			string textureCacheDir = CacheManager.GetTextureCacheDir();
			if (fileEntry != null && textureCacheDir != null)
			{
				string text = fileEntry.Size.ToString();
				string text2 = fileEntry.LastWriteTime.ToFileTime().ToString();
				string text3 = textureCacheDir + "/";
				string text4 = Path.GetFileName(this.imgPath);
				text4 = text4.Replace('.', '_');
				result = string.Concat(new string[]
				{
					text3,
					text4,
					"_",
					text,
					"_",
					text2,
					"_",
					this.diskCacheSignature,
					".vamcache"
				});
			}
			return result;
		}

		// Token: 0x060066A2 RID: 26274 RVA: 0x0016F3F4 File Offset: 0x0016D7F4
		protected string GetWebCachePath()
		{
			string result = null;
			string textureCacheDir = CacheManager.GetTextureCacheDir();
			if (textureCacheDir != null)
			{
				string text = this.imgPath.Replace("https://", string.Empty);
				text = text.Replace("http://", string.Empty);
				text = text.Replace("/", "__");
				text = text.Replace("?", "_");
				string text2 = textureCacheDir + "/";
				result = string.Concat(new string[]
				{
					text2,
					text,
					"_",
					this.diskCacheSignature,
					".vamcache"
				});
			}
			return result;
		}

		// Token: 0x060066A3 RID: 26275 RVA: 0x0016F494 File Offset: 0x0016D894
		public bool WebCachePathExists()
		{
			string webCachePath = this.GetWebCachePath();
			return webCachePath != null && FileManager.FileExists(webCachePath, false, false);
		}

		// Token: 0x060066A4 RID: 26276 RVA: 0x0016F4C0 File Offset: 0x0016D8C0
		public void CreateTexture()
		{
			if (this.tex == null)
			{
				this.tex = new Texture2D(this.width, this.height, this.textureFormat, this.createMipMaps, this.linear);
				this.tex.name = this.cacheSignature;
			}
		}

		// Token: 0x060066A5 RID: 26277 RVA: 0x0016F518 File Offset: 0x0016D918
		protected void ReadMetaJson(string jsonString)
		{
			JSONNode jsonnode = JSON.Parse(jsonString);
			JSONClass asObject = jsonnode.AsObject;
			if (asObject != null)
			{
				if (asObject["width"] != null)
				{
					this.width = asObject["width"].AsInt;
				}
				if (asObject["height"] != null)
				{
					this.height = asObject["height"].AsInt;
				}
				if (asObject["format"] != null)
				{
					this.textureFormat = (TextureFormat)Enum.Parse(typeof(TextureFormat), asObject["format"]);
				}
			}
		}

		// Token: 0x060066A6 RID: 26278 RVA: 0x0016F5D8 File Offset: 0x0016D9D8
		protected void ProcessFromStream(Stream st)
		{
			Bitmap bitmap = new Bitmap(st);
			SolidBrush solidBrush = new SolidBrush(System.Drawing.Color.White);
			bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
			if (!this.setSize)
			{
				this.width = bitmap.Width;
				this.height = bitmap.Height;
				if (this.compress)
				{
					int num = this.width / 4;
					if (num == 0)
					{
						num = 1;
					}
					this.width = num * 4;
					int num2 = this.height / 4;
					if (num2 == 0)
					{
						num2 = 1;
					}
					this.height = num2 * 4;
				}
			}
			int num3 = 3;
			this.textureFormat = TextureFormat.RGB24;
			PixelFormat format = PixelFormat.Format24bppRgb;
			if (this.createAlphaFromGrayscale || this.isNormalMap || this.createNormalFromBump || bitmap.PixelFormat == PixelFormat.Format32bppArgb)
			{
				this.textureFormat = TextureFormat.RGBA32;
				format = PixelFormat.Format32bppArgb;
				num3 = 4;
			}
			Bitmap bitmap2 = new Bitmap(this.width, this.height, format);
			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap2);
			Rectangle rect = new Rectangle(0, 0, this.width, this.height);
			if (this.setSize)
			{
				if (this.fillBackground)
				{
					graphics.FillRectangle(solidBrush, rect);
				}
				float num4 = Mathf.Min((float)this.width / (float)bitmap.Width, (float)this.height / (float)bitmap.Height);
				int num5 = (int)((float)bitmap.Width * num4);
				int num6 = (int)((float)bitmap.Height * num4);
				graphics.DrawImage(bitmap, (this.width - num5) / 2, (this.height - num6) / 2, num5, num6);
			}
			else
			{
				graphics.DrawImage(bitmap, 0, 0, this.width, this.height);
			}
			BitmapData bitmapData = bitmap2.LockBits(rect, ImageLockMode.ReadOnly, bitmap2.PixelFormat);
			int num7 = this.width * this.height;
			int num8 = num7 * num3;
			int num9 = Mathf.CeilToInt((float)num8 * 1.5f);
			this.raw = new byte[num9];
			Marshal.Copy(bitmapData.Scan0, this.raw, 0, num8);
			bitmap2.UnlockBits(bitmapData);
			bool flag = this.isNormalMap && num3 == 4;
			for (int i = 0; i < num8; i += num3)
			{
				byte b = this.raw[i];
				this.raw[i] = this.raw[i + 2];
				this.raw[i + 2] = b;
				if (flag)
				{
					this.raw[i + 3] = byte.MaxValue;
				}
			}
			if (this.invert)
			{
				for (int j = 0; j < num8; j++)
				{
					int num10 = (int)(byte.MaxValue - this.raw[j]);
					this.raw[j] = (byte)num10;
				}
			}
			if (this.createAlphaFromGrayscale)
			{
				for (int k = 0; k < num8; k += 4)
				{
					int num11 = (int)this.raw[k];
					int num12 = (int)this.raw[k + 1];
					int num13 = (int)this.raw[k + 2];
					int num14 = (num11 + num12 + num13) / 3;
					this.raw[k + 3] = (byte)num14;
				}
			}
			if (this.createNormalFromBump)
			{
				byte[] array = new byte[num8 * 2];
				float[][] array2 = new float[this.height][];
				for (int l = 0; l < this.height; l++)
				{
					array2[l] = new float[this.width];
					for (int m = 0; m < this.width; m++)
					{
						int num15 = (l * this.width + m) * 4;
						int num16 = (int)this.raw[num15];
						int num17 = (int)this.raw[num15 + 1];
						int num18 = (int)this.raw[num15 + 2];
						float num19 = (float)(num16 + num17 + num18) / 768f;
						array2[l][m] = num19;
					}
				}
				for (int n = 0; n < this.height; n++)
				{
					for (int num20 = 0; num20 < this.width; num20++)
					{
						float num21 = 0.5f;
						float num22 = 0.5f;
						float num23 = 0.5f;
						float num24 = 0.5f;
						float num25 = 0.5f;
						float num26 = 0.5f;
						float num27 = 0.5f;
						float num28 = 0.5f;
						int num29 = num20 - 1;
						int num30 = num20 + 1;
						int num31 = n + 1;
						int num32 = n - 1;
						int num33 = num31;
						int num34 = num29;
						int num35 = num32;
						int num36 = num29;
						int num37 = num31;
						int num38 = num30;
						int num39 = num32;
						int num40 = num30;
						if (num33 >= 0 && num33 < this.height && num34 >= 0 && num34 < this.width)
						{
							num21 = array2[num33][num34];
						}
						if (num29 >= 0 && num29 < this.width)
						{
							num22 = array2[n][num29];
						}
						if (num35 >= 0 && num35 < this.height && num36 >= 0 && num36 < this.width)
						{
							num23 = array2[num35][num36];
						}
						if (num31 >= 0 && num31 < this.height)
						{
							num24 = array2[num31][num20];
						}
						if (num32 >= 0 && num32 < this.height)
						{
							num25 = array2[num32][num20];
						}
						if (num37 >= 0 && num37 < this.height && num38 >= 0 && num38 < this.width)
						{
							num26 = array2[num37][num38];
						}
						if (num30 >= 0 && num30 < this.width)
						{
							num27 = array2[n][num30];
						}
						if (num39 >= 0 && num39 < this.height && num40 >= 0 && num40 < this.width)
						{
							num28 = array2[num39][num40];
						}
						float num41 = num26 + 2f * num27 + num28 - num21 - 2f * num22 - num23;
						float num42 = num23 + 2f * num25 + num28 - num21 - 2f * num24 - num26;
						Vector3 vector;
						vector.x = num41 * this.bumpStrength;
						vector.y = num42 * this.bumpStrength;
						vector.z = 1f;
						vector.Normalize();
						vector.x = vector.x * 0.5f + 0.5f;
						vector.y = vector.y * 0.5f + 0.5f;
						vector.z = vector.z * 0.5f + 0.5f;
						int num43 = (int)(vector.x * 255f);
						int num44 = (int)(vector.y * 255f);
						int num45 = (int)(vector.z * 255f);
						int num46 = (n * this.width + num20) * 4;
						array[num46] = (byte)num43;
						array[num46 + 1] = (byte)num44;
						array[num46 + 2] = (byte)num45;
						array[num46 + 3] = byte.MaxValue;
					}
				}
				this.raw = array;
			}
			solidBrush.Dispose();
			graphics.Dispose();
			bitmap.Dispose();
			bitmap2.Dispose();
		}

		// Token: 0x060066A7 RID: 26279 RVA: 0x0016FCEC File Offset: 0x0016E0EC
		public void Process()
		{
			if (!this.processed)
			{
				if (this.imgPath != null && this.imgPath != "NULL")
				{
					if (this.useWebCache)
					{
						string webCachePath = this.GetWebCachePath();
						try
						{
							string text = webCachePath + "meta";
							if (FileManager.FileExists(text, false, false))
							{
								string jsonString = FileManager.ReadAllText(text, false);
								this.ReadMetaJson(jsonString);
								this.raw = FileManager.ReadAllBytes(webCachePath, false);
								this.preprocessed = true;
							}
							else
							{
								this.hadError = true;
								this.errorText = "Missing cache meta file " + text;
							}
						}
						catch (Exception ex)
						{
							Debug.LogError("Exception during cache file read " + ex);
							this.hadError = true;
							this.errorText = ex.ToString();
						}
					}
					else if (this.webRequest != null)
					{
						if (!this.webRequestDone)
						{
							return;
						}
						try
						{
							if (!this.webRequestHadError && this.webRequestData != null)
							{
								using (MemoryStream memoryStream = new MemoryStream(this.webRequestData))
								{
									this.ProcessFromStream(memoryStream);
								}
							}
						}
						catch (Exception ex2)
						{
							this.hadError = true;
							Debug.LogError("Exception " + ex2);
							this.errorText = ex2.ToString();
						}
					}
					else if (FileManager.FileExists(this.imgPath, false, false))
					{
						string diskCachePath = this.GetDiskCachePath();
						if (CacheManager.CachingEnabled && diskCachePath != null && FileManager.FileExists(diskCachePath, false, false))
						{
							try
							{
								string text2 = diskCachePath + "meta";
								if (FileManager.FileExists(text2, false, false))
								{
									string jsonString2 = FileManager.ReadAllText(text2, false);
									this.ReadMetaJson(jsonString2);
									this.raw = FileManager.ReadAllBytes(diskCachePath, false);
									this.preprocessed = true;
								}
								else
								{
									this.hadError = true;
									this.errorText = "Missing cache meta file " + text2;
								}
							}
							catch (Exception ex3)
							{
								Debug.LogError("Exception during cache file read " + ex3);
								this.hadError = true;
								this.errorText = ex3.ToString();
							}
						}
						else
						{
							try
							{
								using (FileEntryStream fileEntryStream = FileManager.OpenStream(this.imgPath, false))
								{
									Stream stream = fileEntryStream.Stream;
									this.ProcessFromStream(stream);
								}
							}
							catch (Exception ex4)
							{
								this.hadError = true;
								Debug.LogError("Exception " + ex4);
								this.errorText = ex4.ToString();
							}
						}
					}
					else
					{
						this.hadError = true;
						this.errorText = "Path " + this.imgPath + " is not valid";
					}
				}
				else
				{
					this.finished = true;
				}
				this.processed = true;
			}
		}

		// Token: 0x060066A8 RID: 26280 RVA: 0x00170008 File Offset: 0x0016E408
		protected bool IsPowerOfTwo(uint x)
		{
			return x != 0U && (x & x - 1U) == 0U;
		}

		// Token: 0x060066A9 RID: 26281 RVA: 0x0017001C File Offset: 0x0016E41C
		public void Finish()
		{
			if (this.webRequest != null)
			{
				this.webRequest.Dispose();
				this.webRequestData = null;
				this.webRequest = null;
			}
			if (!this.hadError && !this.finished)
			{
				bool flag = (!this.createMipMaps || !this.compress || (this.IsPowerOfTwo((uint)this.width) && this.IsPowerOfTwo((uint)this.height))) && this.compress;
				this.CreateTexture();
				if (this.preprocessed)
				{
					try
					{
						this.tex.LoadRawTextureData(this.raw);
					}
					catch
					{
						UnityEngine.Object.Destroy(this.tex);
						this.tex = null;
						this.createMipMaps = false;
						this.CreateTexture();
						this.tex.LoadRawTextureData(this.raw);
					}
					this.tex.Apply(false);
					if (this.compress && this.textureFormat != TextureFormat.DXT1 && this.textureFormat != TextureFormat.DXT5)
					{
						this.tex.Compress(true);
					}
				}
				else if (this.tex.format == TextureFormat.DXT1 || this.tex.format == TextureFormat.DXT5)
				{
					Texture2D texture2D = new Texture2D(this.width, this.height, this.textureFormat, this.createMipMaps, this.linear);
					texture2D.LoadRawTextureData(this.raw);
					texture2D.Apply();
					texture2D.Compress(true);
					byte[] rawTextureData = texture2D.GetRawTextureData();
					this.tex.LoadRawTextureData(rawTextureData);
					this.tex.Apply();
					UnityEngine.Object.Destroy(texture2D);
				}
				else
				{
					this.tex.LoadRawTextureData(this.raw);
					this.tex.Apply();
					if (flag)
					{
						this.tex.Compress(true);
					}
					if (CacheManager.CachingEnabled)
					{
						string text;
						if (Regex.IsMatch(this.imgPath, "^http"))
						{
							text = this.GetWebCachePath();
						}
						else
						{
							text = this.GetDiskCachePath();
						}
						if (text != null && !FileManager.FileExists(text, false, false))
						{
							try
							{
								JSONClass jsonclass = new JSONClass();
								jsonclass["type"] = "image";
								jsonclass["width"].AsInt = this.tex.width;
								jsonclass["height"].AsInt = this.tex.height;
								jsonclass["format"] = this.tex.format.ToString();
								string contents = jsonclass.ToString(string.Empty);
								byte[] rawTextureData2 = this.tex.GetRawTextureData();
								File.WriteAllText(text + "meta", contents);
								File.WriteAllBytes(text, rawTextureData2);
							}
							catch (Exception ex)
							{
								Debug.LogError("Exception during caching " + ex);
								this.hadError = true;
								this.errorText = string.Concat(new object[]
								{
									"Exception during caching of ",
									this.imgPath,
									": ",
									ex
								});
							}
						}
					}
				}
				this.finished = true;
			}
		}

		// Token: 0x060066AA RID: 26282 RVA: 0x00170370 File Offset: 0x0016E770
		public void DoCallback()
		{
			if (this.rawImageToLoad != null)
			{
				this.rawImageToLoad.texture = this.tex;
			}
			if (this.callback != null)
			{
				this.callback(this);
				this.callback = null;
			}
		}

		// Token: 0x04005623 RID: 22051
		public bool isThumbnail;

		// Token: 0x04005624 RID: 22052
		public string imgPath;

		// Token: 0x04005625 RID: 22053
		public bool skipCache;

		// Token: 0x04005626 RID: 22054
		public bool forceReload;

		// Token: 0x04005627 RID: 22055
		public bool createMipMaps;

		// Token: 0x04005628 RID: 22056
		public bool compress = true;

		// Token: 0x04005629 RID: 22057
		public bool linear;

		// Token: 0x0400562A RID: 22058
		public bool processed;

		// Token: 0x0400562B RID: 22059
		public bool preprocessed;

		// Token: 0x0400562C RID: 22060
		public bool cancel;

		// Token: 0x0400562D RID: 22061
		public bool finished;

		// Token: 0x0400562E RID: 22062
		public bool isNormalMap;

		// Token: 0x0400562F RID: 22063
		public bool createAlphaFromGrayscale;

		// Token: 0x04005630 RID: 22064
		public bool createNormalFromBump;

		// Token: 0x04005631 RID: 22065
		public float bumpStrength = 1f;

		// Token: 0x04005632 RID: 22066
		public bool invert;

		// Token: 0x04005633 RID: 22067
		public bool setSize;

		// Token: 0x04005634 RID: 22068
		public bool fillBackground;

		// Token: 0x04005635 RID: 22069
		public int width;

		// Token: 0x04005636 RID: 22070
		public int height;

		// Token: 0x04005637 RID: 22071
		public byte[] raw;

		// Token: 0x04005638 RID: 22072
		public bool hadError;

		// Token: 0x04005639 RID: 22073
		public string errorText;

		// Token: 0x0400563A RID: 22074
		public TextureFormat textureFormat;

		// Token: 0x0400563B RID: 22075
		public Texture2D tex;

		// Token: 0x0400563C RID: 22076
		public RawImage rawImageToLoad;

		// Token: 0x0400563D RID: 22077
		public bool useWebCache;

		// Token: 0x0400563E RID: 22078
		public UnityWebRequest webRequest;

		// Token: 0x0400563F RID: 22079
		public bool webRequestDone;

		// Token: 0x04005640 RID: 22080
		public bool webRequestHadError;

		// Token: 0x04005641 RID: 22081
		public byte[] webRequestData;

		// Token: 0x04005642 RID: 22082
		public ImageLoaderThreaded.ImageLoaderCallback callback;
	}

	// Token: 0x02000D1C RID: 3356
	protected class ImageLoaderTaskInfo
	{
		// Token: 0x060066AB RID: 26283 RVA: 0x0026D203 File Offset: 0x0026B603
		public ImageLoaderTaskInfo()
		{
		}

		// Token: 0x04005643 RID: 22083
		public string name;

		// Token: 0x04005644 RID: 22084
		public AutoResetEvent resetEvent;

		// Token: 0x04005645 RID: 22085
		public Thread thread;

		// Token: 0x04005646 RID: 22086
		public volatile bool working;

		// Token: 0x04005647 RID: 22087
		public volatile bool kill;
	}
}
