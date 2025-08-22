using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using MVR.FileManagement;
using UnityEngine;
using UnityEngine.UI;

namespace OldMoatGames
{
	// Token: 0x0200000C RID: 12
	[AddComponentMenu("Miscellaneous/Animated GIF Player")]
	public class AnimatedGifPlayer : MonoBehaviour
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00003A34 File Offset: 0x00001E34
		public AnimatedGifPlayer()
		{
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00003A89 File Offset: 0x00001E89
		public int Width
		{
			get
			{
				return (this._gifDecoder != null) ? this._gifDecoder.GetFrameWidth() : 0;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003AA7 File Offset: 0x00001EA7
		public int Height
		{
			get
			{
				return (this._gifDecoder != null) ? this._gifDecoder.GetFrameHeight() : 0;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003AC5 File Offset: 0x00001EC5
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00003ACD File Offset: 0x00001ECD
		public Component TargetComponent
		{
			get
			{
				return this._targetComponent;
			}
			set
			{
				this._targetComponent = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003AD6 File Offset: 0x00001ED6
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00003ADE File Offset: 0x00001EDE
		public int TargetMaterialNumber
		{
			get
			{
				return this._targetMaterial;
			}
			set
			{
				this._targetMaterial = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003AE7 File Offset: 0x00001EE7
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00003AEF File Offset: 0x00001EEF
		public GifPlayerState State
		{
			[CompilerGenerated]
			get
			{
				return this.<State>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<State>k__BackingField = value;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600006D RID: 109 RVA: 0x00003AF8 File Offset: 0x00001EF8
		// (remove) Token: 0x0600006E RID: 110 RVA: 0x00003B30 File Offset: 0x00001F30
		public event AnimatedGifPlayer.OnReadyAction OnReady
		{
			add
			{
				AnimatedGifPlayer.OnReadyAction onReadyAction = this.OnReady;
				AnimatedGifPlayer.OnReadyAction onReadyAction2;
				do
				{
					onReadyAction2 = onReadyAction;
					onReadyAction = Interlocked.CompareExchange<AnimatedGifPlayer.OnReadyAction>(ref this.OnReady, (AnimatedGifPlayer.OnReadyAction)Delegate.Combine(onReadyAction2, value), onReadyAction);
				}
				while (onReadyAction != onReadyAction2);
			}
			remove
			{
				AnimatedGifPlayer.OnReadyAction onReadyAction = this.OnReady;
				AnimatedGifPlayer.OnReadyAction onReadyAction2;
				do
				{
					onReadyAction2 = onReadyAction;
					onReadyAction = Interlocked.CompareExchange<AnimatedGifPlayer.OnReadyAction>(ref this.OnReady, (AnimatedGifPlayer.OnReadyAction)Delegate.Remove(onReadyAction2, value), onReadyAction);
				}
				while (onReadyAction != onReadyAction2);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600006F RID: 111 RVA: 0x00003B68 File Offset: 0x00001F68
		// (remove) Token: 0x06000070 RID: 112 RVA: 0x00003BA0 File Offset: 0x00001FA0
		public event AnimatedGifPlayer.OnLoadErrorAction OnLoadError
		{
			add
			{
				AnimatedGifPlayer.OnLoadErrorAction onLoadErrorAction = this.OnLoadError;
				AnimatedGifPlayer.OnLoadErrorAction onLoadErrorAction2;
				do
				{
					onLoadErrorAction2 = onLoadErrorAction;
					onLoadErrorAction = Interlocked.CompareExchange<AnimatedGifPlayer.OnLoadErrorAction>(ref this.OnLoadError, (AnimatedGifPlayer.OnLoadErrorAction)Delegate.Combine(onLoadErrorAction2, value), onLoadErrorAction);
				}
				while (onLoadErrorAction != onLoadErrorAction2);
			}
			remove
			{
				AnimatedGifPlayer.OnLoadErrorAction onLoadErrorAction = this.OnLoadError;
				AnimatedGifPlayer.OnLoadErrorAction onLoadErrorAction2;
				do
				{
					onLoadErrorAction2 = onLoadErrorAction;
					onLoadErrorAction = Interlocked.CompareExchange<AnimatedGifPlayer.OnLoadErrorAction>(ref this.OnLoadError, (AnimatedGifPlayer.OnLoadErrorAction)Delegate.Remove(onLoadErrorAction2, value), onLoadErrorAction);
				}
				while (onLoadErrorAction != onLoadErrorAction2);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003BD6 File Offset: 0x00001FD6
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00003BDE File Offset: 0x00001FDE
		private GifDecoder.GifFrame CurrentFrame
		{
			[CompilerGenerated]
			get
			{
				return this.<CurrentFrame>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CurrentFrame>k__BackingField = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003BE7 File Offset: 0x00001FE7
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00003BEF File Offset: 0x00001FEF
		private int CurrentFrameNumber
		{
			[CompilerGenerated]
			get
			{
				return this.<CurrentFrameNumber>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CurrentFrameNumber>k__BackingField = value;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003BF8 File Offset: 0x00001FF8
		private void Awake()
		{
			if (this.State == GifPlayerState.PreProcessing)
			{
				this.Init();
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003C0B File Offset: 0x0000200B
		private void OnDestroy()
		{
			if (this._createdTexture != null)
			{
				UnityEngine.Object.Destroy(this._createdTexture);
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003C29 File Offset: 0x00002029
		public void Update()
		{
			this.CheckFrameChange();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003C31 File Offset: 0x00002031
		private void OnApplicationQuit()
		{
			this.EndDecodeThread();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003C3C File Offset: 0x0000203C
		public void Init()
		{
			this._cacheFrames = this.CacheFrames;
			this._bufferAllFrames = this.BufferAllFrames;
			this._useThreadedDecoder = this.UseThreadedDecoder;
			if (this._bufferAllFrames && !this._cacheFrames)
			{
				this._bufferAllFrames = false;
			}
			if (this._cacheFrames)
			{
				this._cachedFrames = new List<GifDecoder.GifFrame>();
			}
			this._targetComponent = this.GetTargetComponent();
			this._gifDecoder = new GifDecoder();
			this.CurrentFrameNumber = 0;
			this._hasFirstFrameBeenShown = false;
			this._frameIsReady = false;
			this.State = GifPlayerState.Disabled;
			this.StartDecodeThread();
			if (this.FileName.Length <= 0)
			{
				return;
			}
			base.StartCoroutine(this.Load());
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003CF8 File Offset: 0x000020F8
		public void Play()
		{
			if (this.State != GifPlayerState.Stopped)
			{
				UnityEngine.Debug.LogWarning("Can't play GIF playback. State is: " + this.State);
				return;
			}
			this.State = GifPlayerState.Playing;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003D28 File Offset: 0x00002128
		public void Pause()
		{
			if (this.State != GifPlayerState.Playing)
			{
				UnityEngine.Debug.LogWarning("Can't pause GIF is not playing. State is: " + this.State);
				return;
			}
			this.State = GifPlayerState.Stopped;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003D58 File Offset: 0x00002158
		public int GetNumberOfFrames()
		{
			return (this._gifDecoder != null) ? this._gifDecoder.GetFrameCount() : 0;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003D78 File Offset: 0x00002178
		private IEnumerator Load()
		{
			if (this.FileName.Length == 0)
			{
				UnityEngine.Debug.LogWarning("File name not set");
				yield break;
			}
			this.State = GifPlayerState.Loading;
			bool isPackageFile = false;
			string path;
			if (this.FileName.Substring(0, 4) == "http")
			{
				path = this.FileName;
			}
			else
			{
				path = this.FileName;
				if (FileManager.IsFileInPackage(path))
				{
					isPackageFile = true;
				}
				else if (!Regex.IsMatch(path, "^file"))
				{
					path = "file:///.\\" + path;
				}
			}
			if (isPackageFile)
			{
				byte[] buffer = FileManager.ReadAllBytes(path, false);
				object locker = this._locker;
				lock (locker)
				{
					if (this._gifDecoder.Read(new MemoryStream(buffer)) == GifDecoder.Status.StatusOk)
					{
						this.State = GifPlayerState.PreProcessing;
						this.CreateTargetTexture();
						this.StartDecoder();
					}
					else
					{
						UnityEngine.Debug.LogWarning("Error loading gif");
						this.State = GifPlayerState.Error;
						if (this.OnLoadError != null)
						{
							this.OnLoadError();
						}
					}
				}
			}
			else
			{
				using (WWW www = new WWW(path))
				{
					yield return www;
					if (!string.IsNullOrEmpty(www.error))
					{
						UnityEngine.Debug.LogWarning("File load error.\n" + www.error);
						this.State = GifPlayerState.Error;
					}
					else
					{
						object locker2 = this._locker;
						lock (locker2)
						{
							if (this._gifDecoder.Read(new MemoryStream(www.bytes)) == GifDecoder.Status.StatusOk)
							{
								this.State = GifPlayerState.PreProcessing;
								this.CreateTargetTexture();
								this.StartDecoder();
							}
							else
							{
								UnityEngine.Debug.LogWarning("Error loading gif");
								this.State = GifPlayerState.Error;
								if (this.OnLoadError != null)
								{
									this.OnLoadError();
								}
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003D94 File Offset: 0x00002194
		private void CreateTargetTexture()
		{
			if (this.GifTexture != null && this._gifDecoder != null && this.GifTexture.width == this._gifDecoder.GetFrameWidth() && this.GifTexture.height == this._gifDecoder.GetFrameWidth())
			{
				return;
			}
			if (this._gifDecoder == null || this._gifDecoder.GetFrameWidth() == 0 || this._gifDecoder.GetFrameWidth() == 0)
			{
				this.GifTexture = Texture2D.blackTexture;
				return;
			}
			this.GifTexture = AnimatedGifPlayer.CreateTexture(this._gifDecoder.GetFrameWidth(), this._gifDecoder.GetFrameHeight());
			if (this._createdTexture != null)
			{
				UnityEngine.Object.Destroy(this._createdTexture);
			}
			this._createdTexture = this.GifTexture;
			ImageControl component = base.GetComponent<ImageControl>();
			if (component != null)
			{
				component.SyncImageRatio(this.GifTexture, false);
			}
			this.GifTexture.hideFlags = HideFlags.HideAndDontSave;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003EA4 File Offset: 0x000022A4
		private void SetTexture()
		{
			if (this._targetComponent == null)
			{
				return;
			}
			if (this._targetComponent is SpriteRenderer)
			{
				SpriteRenderer spriteRenderer = (SpriteRenderer)this._targetComponent;
				Vector2 size = spriteRenderer.size;
				Sprite sprite = Sprite.Create(this.GifTexture, new Rect(0f, 0f, (float)this.GifTexture.width, (float)this.GifTexture.height), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect);
				sprite.name = "Gif Player Sprite";
				sprite.hideFlags = HideFlags.HideAndDontSave;
				spriteRenderer.sprite = sprite;
				spriteRenderer.size = size;
				return;
			}
			ImageControl component = base.GetComponent<ImageControl>();
			if (component != null)
			{
				component.SyncTexture(this.GifTexture);
				return;
			}
			if (this._targetComponent is Renderer)
			{
				Renderer renderer = (Renderer)this._targetComponent;
				if (renderer.sharedMaterial == null)
				{
					return;
				}
				if (renderer.sharedMaterials.Length > 0 && renderer.sharedMaterials.Length > this._targetMaterial)
				{
					renderer.sharedMaterials[this._targetMaterial].mainTexture = this.GifTexture;
				}
				else
				{
					renderer.sharedMaterial.mainTexture = this.GifTexture;
				}
				return;
			}
			else
			{
				if (this._targetComponent is RawImage)
				{
					RawImage rawImage = (RawImage)this._targetComponent;
					rawImage.texture = this.GifTexture;
					return;
				}
				return;
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004020 File Offset: 0x00002420
		private Component GetTargetComponent()
		{
			Component[] components = base.GetComponents<Component>();
			IEnumerable<Component> source = components;
			if (AnimatedGifPlayer.<>f__am$cache0 == null)
			{
				AnimatedGifPlayer.<>f__am$cache0 = new Func<Component, bool>(AnimatedGifPlayer.<GetTargetComponent>m__0);
			}
			return source.FirstOrDefault(AnimatedGifPlayer.<>f__am$cache0);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004058 File Offset: 0x00002458
		private void SetTargetTexture()
		{
			if (this.GifTexture == null || this.GifTexture.width != this._gifDecoder.GetFrameWidth() || this.GifTexture.height != this._gifDecoder.GetFrameWidth())
			{
				this.GifTexture = AnimatedGifPlayer.CreateTexture(this._gifDecoder.GetFrameWidth(), this._gifDecoder.GetFrameHeight());
				if (this._createdTexture != null)
				{
					UnityEngine.Object.Destroy(this._createdTexture);
				}
				this._createdTexture = this.GifTexture;
				ImageControl component = base.GetComponent<ImageControl>();
				if (component != null)
				{
					component.SyncImageRatio(this.GifTexture, false);
				}
			}
			this.GifTexture.hideFlags = HideFlags.HideAndDontSave;
			if (this.TargetComponent == null)
			{
				return;
			}
			if (this.TargetComponent is MeshRenderer)
			{
				Renderer renderer = (Renderer)this.TargetComponent;
				if (renderer.sharedMaterial == null)
				{
					return;
				}
				if (renderer.sharedMaterials.Length > 0 && renderer.sharedMaterials.Length > this._targetMaterial)
				{
					renderer.sharedMaterials[this._targetMaterial].mainTexture = this.GifTexture;
				}
				else
				{
					renderer.sharedMaterial.mainTexture = this.GifTexture;
				}
			}
			if (this.TargetComponent is SpriteRenderer)
			{
				SpriteRenderer spriteRenderer = (SpriteRenderer)this.TargetComponent;
				Sprite sprite = Sprite.Create(this.GifTexture, new Rect(0f, 0f, (float)this.GifTexture.width, (float)this.GifTexture.height), new Vector2(0.5f, 0.5f));
				sprite.name = "Gif Player Sprite";
				sprite.hideFlags = HideFlags.HideAndDontSave;
				spriteRenderer.sprite = sprite;
			}
			if (this.TargetComponent is RawImage)
			{
				RawImage rawImage = (RawImage)this.TargetComponent;
				rawImage.texture = this.GifTexture;
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004253 File Offset: 0x00002653
		private static Texture2D CreateTexture(int width, int height)
		{
			return new Texture2D(width, height, TextureFormat.RGBA32, false);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004260 File Offset: 0x00002660
		private void BufferFrames()
		{
			if (this._useThreadedDecoder)
			{
				this._wh.Set();
				return;
			}
			object locker = this._locker;
			lock (locker)
			{
				for (;;)
				{
					this._gifDecoder.ReadNextFrame(false);
					if (this._gifDecoder.AllFramesRead)
					{
						break;
					}
					GifDecoder.GifFrame currentFrame = this._gifDecoder.GetCurrentFrame();
					this.AddFrameToCache(currentFrame);
				}
				this._frameIsReady = true;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000042F0 File Offset: 0x000026F0
		private void AddFrameToCache(GifDecoder.GifFrame frame)
		{
			byte[] array = new byte[frame.Image.Length];
			Buffer.BlockCopy(frame.Image, 0, array, 0, frame.Image.Length);
			frame.Image = array;
			object cachedFrames = this._cachedFrames;
			lock (cachedFrames)
			{
				this._cachedFrames.Add(frame);
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004360 File Offset: 0x00002760
		private void StartDecoder()
		{
			if (this._bufferAllFrames)
			{
				this.BufferFrames();
			}
			else
			{
				this.StartReadFrame();
			}
			this.State = GifPlayerState.Stopped;
			if (this.OnReady != null)
			{
				this.OnReady();
			}
			if (this.AutoPlay)
			{
				this.Play();
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000043B7 File Offset: 0x000027B7
		private void SetNextFrameTime()
		{
			this._secondsTillNextFrame = this.CurrentFrame.Delay;
			if (this._secondsTillNextFrame == 0f)
			{
				this._secondsTillNextFrame = 0.041666668f;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000043E8 File Offset: 0x000027E8
		private void UpdateFrameTime()
		{
			if (this.State != GifPlayerState.Playing)
			{
				return;
			}
			if (!Application.isPlaying || this.OverrideTimeScale)
			{
				if (this.OverrideTimeScale)
				{
					this._secondsTillNextFrame -= (Time.realtimeSinceStartup - this._editorPreviousUpdateTime) * this.TimeScale;
				}
				else
				{
					this._secondsTillNextFrame -= Time.realtimeSinceStartup - this._editorPreviousUpdateTime;
				}
				this._editorPreviousUpdateTime = Time.realtimeSinceStartup;
				return;
			}
			this._secondsTillNextFrame -= Time.deltaTime;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004480 File Offset: 0x00002880
		private void UpdateFrame()
		{
			if (this._gifDecoder.NumberOfFrames > 0 && this._gifDecoder.NumberOfFrames == this.CurrentFrameNumber)
			{
				this.CurrentFrameNumber = 0;
				if (!this.Loop)
				{
					this.Pause();
					return;
				}
			}
			if (this._cacheFrames)
			{
				object cachedFrames = this._cachedFrames;
				lock (cachedFrames)
				{
					this.CurrentFrame = ((this._cachedFrames.Count <= this.CurrentFrameNumber) ? this._gifDecoder.GetCurrentFrame() : this._cachedFrames[this.CurrentFrameNumber]);
				}
				if (!this._gifDecoder.AllFramesRead)
				{
					this.StartReadFrame();
				}
			}
			else
			{
				this.CurrentFrame = this._gifDecoder.GetCurrentFrame();
				this.StartReadFrame();
			}
			this.UpdateTexture();
			this.SetNextFrameTime();
			this.CurrentFrameNumber++;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000458C File Offset: 0x0000298C
		private void CheckFrameChange()
		{
			if ((this.State != GifPlayerState.Playing && this._hasFirstFrameBeenShown) || !this._frameIsReady)
			{
				return;
			}
			if (!this._hasFirstFrameBeenShown)
			{
				this.SetTexture();
				object locker = this._locker;
				lock (locker)
				{
					this.UpdateFrame();
				}
				this._hasFirstFrameBeenShown = true;
				return;
			}
			this.UpdateFrameTime();
			if (this._secondsTillNextFrame > 0f)
			{
				return;
			}
			object locker2 = this._locker;
			lock (locker2)
			{
				this.UpdateFrame();
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004648 File Offset: 0x00002A48
		private void UpdateTexture()
		{
			this.GifTexture.LoadRawTextureData(this.CurrentFrame.Image);
			this.GifTexture.Apply();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000466C File Offset: 0x00002A6C
		private void StartReadFrame()
		{
			this._frameIsReady = false;
			if (this._useThreadedDecoder)
			{
				this._wh.Set();
				return;
			}
			if (this._cacheFrames && this._gifDecoder.AllFramesRead)
			{
				return;
			}
			this._gifDecoder.ReadNextFrame(!this._cacheFrames);
			if (this._cacheFrames && !this._gifDecoder.AllFramesRead)
			{
				this.AddFrameToCache(this._gifDecoder.GetCurrentFrame());
			}
			this._frameIsReady = true;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000046FC File Offset: 0x00002AFC
		private void StartDecodeThread()
		{
			if (!this._useThreadedDecoder || (this._decodeThread != null && this._decodeThread.IsAlive))
			{
				return;
			}
			this._threadIsCanceled = false;
			this._decodeThread = new Thread(new ThreadStart(this.<StartDecodeThread>m__1));
			this._decodeThread.Name = "gifDecoder" + this._decodeThread.ManagedThreadId;
			this._decodeThread.IsBackground = true;
			this._decodeThread.Start();
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000478A File Offset: 0x00002B8A
		private void EndDecodeThread()
		{
			if (this._threadIsCanceled)
			{
				return;
			}
			this._threadIsCanceled = true;
			this._wh.Set();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000047AC File Offset: 0x00002BAC
		private void FrameDataThread(bool loopDecoder, bool readAllFrames)
		{
			this._wh.WaitOne();
			while (!this._threadIsCanceled)
			{
				object locker = this._locker;
				lock (locker)
				{
					this._gifDecoder.ReadNextFrame(loopDecoder);
					if (this._cacheFrames && this._gifDecoder.AllFramesRead)
					{
						this._frameIsReady = true;
						break;
					}
					if (this._cacheFrames)
					{
						this.AddFrameToCache(this._gifDecoder.GetCurrentFrame());
					}
					if (readAllFrames)
					{
						if (this._gifDecoder.AllFramesRead)
						{
							this._frameIsReady = true;
							break;
						}
						continue;
					}
					else
					{
						this._frameIsReady = true;
					}
				}
				this._wh.WaitOne();
			}
			this._threadIsCanceled = true;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004890 File Offset: 0x00002C90
		[CompilerGenerated]
		private static bool <GetTargetComponent>m__0(Component component)
		{
			return component is Renderer || component is RawImage;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000048A9 File Offset: 0x00002CA9
		[CompilerGenerated]
		private void <StartDecodeThread>m__1()
		{
			this.FrameDataThread(!this._cacheFrames, this._bufferAllFrames);
		}

		// Token: 0x04000065 RID: 101
		public bool Loop = true;

		// Token: 0x04000066 RID: 102
		public bool AutoPlay = true;

		// Token: 0x04000067 RID: 103
		public string FileName = string.Empty;

		// Token: 0x04000068 RID: 104
		public GifPath Path;

		// Token: 0x04000069 RID: 105
		public bool CacheFrames;

		// Token: 0x0400006A RID: 106
		public bool BufferAllFrames;

		// Token: 0x0400006B RID: 107
		public bool UseThreadedDecoder = true;

		// Token: 0x0400006C RID: 108
		public bool OverrideTimeScale;

		// Token: 0x0400006D RID: 109
		public float TimeScale = 1f;

		// Token: 0x0400006E RID: 110
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GifPlayerState <State>k__BackingField;

		// Token: 0x0400006F RID: 111
		public Texture2D GifTexture;

		// Token: 0x04000070 RID: 112
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private AnimatedGifPlayer.OnReadyAction OnReady;

		// Token: 0x04000071 RID: 113
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private AnimatedGifPlayer.OnLoadErrorAction OnLoadError;

		// Token: 0x04000072 RID: 114
		private GifDecoder _gifDecoder;

		// Token: 0x04000073 RID: 115
		private bool _hasFirstFrameBeenShown;

		// Token: 0x04000074 RID: 116
		[SerializeField]
		private Component _targetComponent;

		// Token: 0x04000075 RID: 117
		[SerializeField]
		private int _targetMaterial;

		// Token: 0x04000076 RID: 118
		private bool _cacheFrames;

		// Token: 0x04000077 RID: 119
		private bool _bufferAllFrames;

		// Token: 0x04000078 RID: 120
		private bool _useThreadedDecoder;

		// Token: 0x04000079 RID: 121
		private float _secondsTillNextFrame;

		// Token: 0x0400007A RID: 122
		private List<GifDecoder.GifFrame> _cachedFrames;

		// Token: 0x0400007B RID: 123
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GifDecoder.GifFrame <CurrentFrame>k__BackingField;

		// Token: 0x0400007C RID: 124
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <CurrentFrameNumber>k__BackingField;

		// Token: 0x0400007D RID: 125
		private Thread _decodeThread;

		// Token: 0x0400007E RID: 126
		private readonly EventWaitHandle _wh = new AutoResetEvent(false);

		// Token: 0x0400007F RID: 127
		private bool _threadIsCanceled;

		// Token: 0x04000080 RID: 128
		private bool _frameIsReady;

		// Token: 0x04000081 RID: 129
		private readonly object _locker = new object();

		// Token: 0x04000082 RID: 130
		private float _editorPreviousUpdateTime;

		// Token: 0x04000083 RID: 131
		protected Texture2D _createdTexture;

		// Token: 0x04000084 RID: 132
		[CompilerGenerated]
		private static Func<Component, bool> <>f__am$cache0;

		// Token: 0x0200000D RID: 13
		// (Invoke) Token: 0x06000092 RID: 146
		public delegate void OnReadyAction();

		// Token: 0x0200000E RID: 14
		// (Invoke) Token: 0x06000096 RID: 150
		public delegate void OnLoadErrorAction();

		// Token: 0x02000E9E RID: 3742
		[CompilerGenerated]
		private sealed class <Load>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007156 RID: 29014 RVA: 0x000048C0 File Offset: 0x00002CC0
			[DebuggerHidden]
			public <Load>c__Iterator0()
			{
			}

			// Token: 0x06007157 RID: 29015 RVA: 0x000048C8 File Offset: 0x00002CC8
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					if (this.FileName.Length == 0)
					{
						UnityEngine.Debug.LogWarning("File name not set");
						return false;
					}
					base.State = GifPlayerState.Loading;
					isPackageFile = false;
					if (this.FileName.Substring(0, 4) == "http")
					{
						path = this.FileName;
					}
					else
					{
						path = this.FileName;
						if (FileManager.IsFileInPackage(path))
						{
							isPackageFile = true;
						}
						else if (!Regex.IsMatch(path, "^file"))
						{
							path = "file:///.\\" + path;
						}
					}
					if (isPackageFile)
					{
						byte[] buffer = FileManager.ReadAllBytes(path, false);
						object locker = this._locker;
						lock (locker)
						{
							if (this._gifDecoder.Read(new MemoryStream(buffer)) == GifDecoder.Status.StatusOk)
							{
								base.State = GifPlayerState.PreProcessing;
								base.CreateTargetTexture();
								base.StartDecoder();
							}
							else
							{
								UnityEngine.Debug.LogWarning("Error loading gif");
								base.State = GifPlayerState.Error;
								if (this.OnLoadError != null)
								{
									this.OnLoadError();
								}
							}
						}
						goto IL_2D1;
					}
					www = new WWW(path);
					num = 4294967293U;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1U:
						if (!string.IsNullOrEmpty(www.error))
						{
							UnityEngine.Debug.LogWarning("File load error.\n" + www.error);
							base.State = GifPlayerState.Error;
						}
						else
						{
							object locker2 = this._locker;
							lock (locker2)
							{
								if (this._gifDecoder.Read(new MemoryStream(www.bytes)) == GifDecoder.Status.StatusOk)
								{
									base.State = GifPlayerState.PreProcessing;
									base.CreateTargetTexture();
									base.StartDecoder();
								}
								else
								{
									UnityEngine.Debug.LogWarning("Error loading gif");
									base.State = GifPlayerState.Error;
									if (this.OnLoadError != null)
									{
										this.OnLoadError();
									}
								}
							}
						}
						break;
					default:
						this.$current = www;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						this.<>__Finally0();
					}
				}
				IL_2D1:
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010A1 RID: 4257
			// (get) Token: 0x06007158 RID: 29016 RVA: 0x00004BFC File Offset: 0x00002FFC
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010A2 RID: 4258
			// (get) Token: 0x06007159 RID: 29017 RVA: 0x00004C04 File Offset: 0x00003004
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600715A RID: 29018 RVA: 0x00004C0C File Offset: 0x0000300C
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1U:
					try
					{
					}
					finally
					{
						this.<>__Finally0();
					}
					break;
				}
			}

			// Token: 0x0600715B RID: 29019 RVA: 0x00004C60 File Offset: 0x00003060
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600715C RID: 29020 RVA: 0x00004C67 File Offset: 0x00003067
			private void <>__Finally0()
			{
				if (www != null)
				{
					((IDisposable)www).Dispose();
				}
			}

			// Token: 0x0400652C RID: 25900
			internal bool <isPackageFile>__0;

			// Token: 0x0400652D RID: 25901
			internal string <path>__1;

			// Token: 0x0400652E RID: 25902
			internal WWW <www>__2;

			// Token: 0x0400652F RID: 25903
			internal AnimatedGifPlayer $this;

			// Token: 0x04006530 RID: 25904
			internal object $current;

			// Token: 0x04006531 RID: 25905
			internal bool $disposing;

			// Token: 0x04006532 RID: 25906
			internal int $PC;
		}
	}
}
