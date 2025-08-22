using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace MVR.FileManagement
{
	// Token: 0x02000BD5 RID: 3029
	public class CacheManager : MonoBehaviour
	{
		// Token: 0x060055F3 RID: 22003 RVA: 0x001F6E21 File Offset: 0x001F5221
		public CacheManager()
		{
		}

		// Token: 0x060055F4 RID: 22004 RVA: 0x001F6E34 File Offset: 0x001F5234
		public static string GetCacheDir()
		{
			if (CacheManager.singleton != null)
			{
				return CacheManager.singleton.CacheDir;
			}
			return null;
		}

		// Token: 0x060055F5 RID: 22005 RVA: 0x001F6E52 File Offset: 0x001F5252
		public static string GetTextureCacheDir()
		{
			if (CacheManager.singleton != null)
			{
				return CacheManager.singleton.CacheDir + "/" + CacheManager.textureFolderName;
			}
			return null;
		}

		// Token: 0x060055F6 RID: 22006 RVA: 0x001F6E7F File Offset: 0x001F527F
		public static string GetPackageJSONCacheDir()
		{
			if (CacheManager.singleton != null)
			{
				return CacheManager.singleton.CacheDir + "/" + CacheManager.packageJSONFolderName;
			}
			return null;
		}

		// Token: 0x060055F7 RID: 22007 RVA: 0x001F6EAC File Offset: 0x001F52AC
		public static string GetVideoCacheDir()
		{
			if (CacheManager.singleton != null)
			{
				return CacheManager.singleton.CacheDir + "/" + CacheManager.videoFolderName;
			}
			return null;
		}

		// Token: 0x060055F8 RID: 22008 RVA: 0x001F6ED9 File Offset: 0x001F52D9
		public static void SetCacheDir(string dir)
		{
			if (CacheManager.singleton != null)
			{
				CacheManager.singleton.CacheDir = dir;
			}
		}

		// Token: 0x060055F9 RID: 22009 RVA: 0x001F6EF6 File Offset: 0x001F52F6
		public static void ResetCacheDir()
		{
			if (CacheManager.singleton != null)
			{
				CacheManager.singleton.CacheDir = CacheManager.singleton.defaultCacheDir;
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x060055FA RID: 22010 RVA: 0x001F6F1C File Offset: 0x001F531C
		// (set) Token: 0x060055FB RID: 22011 RVA: 0x001F6F23 File Offset: 0x001F5323
		public static bool CachingEnabled
		{
			[CompilerGenerated]
			get
			{
				return CacheManager.<CachingEnabled>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				CacheManager.<CachingEnabled>k__BackingField = value;
			}
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x060055FC RID: 22012 RVA: 0x001F6F2B File Offset: 0x001F532B
		// (set) Token: 0x060055FD RID: 22013 RVA: 0x001F6F33 File Offset: 0x001F5333
		public string CacheDir
		{
			get
			{
				return this._cacheDir;
			}
			set
			{
				if (this._cacheDir != value)
				{
					this.ClearCache();
					this._cacheDir = value;
					this.InitCache();
				}
			}
		}

		// Token: 0x060055FE RID: 22014 RVA: 0x001F6F5C File Offset: 0x001F535C
		protected void GetCacheSizeThreaded()
		{
			long num = 0L;
			DirectoryInfo directoryInfo = new DirectoryInfo(this.CacheDir);
			FileInfo[] files = directoryInfo.GetFiles("*.vamcache", SearchOption.AllDirectories);
			foreach (FileInfo fileInfo in files)
			{
				num += fileInfo.Length;
			}
			files = directoryInfo.GetFiles("*.vamcachemeta", SearchOption.AllDirectories);
			foreach (FileInfo fileInfo2 in files)
			{
				num += fileInfo2.Length;
			}
			files = directoryInfo.GetFiles("*.vamcachejson", SearchOption.AllDirectories);
			foreach (FileInfo fileInfo3 in files)
			{
				num += fileInfo3.Length;
			}
			this.cacheSizeThreaded = (float)num / 1E+09f;
		}

		// Token: 0x060055FF RID: 22015 RVA: 0x001F7034 File Offset: 0x001F5434
		private IEnumerator GetCacheSizeCo()
		{
			if (this.cacheSizeRecalcThread == null)
			{
				this.cacheSizeRecalcThread = new Thread(new ThreadStart(base.<>m__0));
				this.cacheSizeRecalcThread.Start();
			}
			while (this.cacheSizeRecalcThread.IsAlive)
			{
				yield return null;
			}
			this.cacheSize = this.cacheSizeThreaded;
			this.cacheSizeRecalcThread = null;
			this.cacheSizeCoroutine = null;
			foreach (CacheManager.CacheSizeCallback cacheSizeCallback in this.callbacks)
			{
				if (cacheSizeCallback != null)
				{
					cacheSizeCallback(this.cacheSize);
				}
			}
			this.callbacks.Clear();
			yield break;
		}

		// Token: 0x06005600 RID: 22016 RVA: 0x001F704F File Offset: 0x001F544F
		public void GetCacheSize(CacheManager.CacheSizeCallback callback)
		{
			this.callbacks.Add(callback);
			if (this.cacheSizeCoroutine == null)
			{
				this.cacheSizeCoroutine = base.StartCoroutine(this.GetCacheSizeCo());
			}
		}

		// Token: 0x06005601 RID: 22017 RVA: 0x001F707C File Offset: 0x001F547C
		protected void InitCache()
		{
			try
			{
				if (!Directory.Exists(this.CacheDir))
				{
					Directory.CreateDirectory(this.CacheDir);
				}
				string path = this.CacheDir + "/" + CacheManager.textureFolderName;
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				string path2 = this.CacheDir + "/" + CacheManager.packageJSONFolderName;
				if (!Directory.Exists(path2))
				{
					Directory.CreateDirectory(path2);
				}
				string path3 = this.CacheDir + "/" + CacheManager.videoFolderName;
				if (!Directory.Exists(path3))
				{
					Directory.CreateDirectory(path3);
				}
			}
			catch (Exception arg)
			{
				SuperController.LogError("Failed to init cache " + arg);
			}
		}

		// Token: 0x06005602 RID: 22018 RVA: 0x001F7148 File Offset: 0x001F5548
		public void ClearCache()
		{
			if (Directory.Exists(this.CacheDir))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(this.CacheDir);
				FileInfo[] files = directoryInfo.GetFiles("*.vamcache", SearchOption.AllDirectories);
				foreach (FileInfo fileInfo in files)
				{
					try
					{
						fileInfo.Delete();
					}
					catch (Exception)
					{
					}
				}
				files = directoryInfo.GetFiles("*.vamcachemeta", SearchOption.AllDirectories);
				foreach (FileInfo fileInfo2 in files)
				{
					try
					{
						fileInfo2.Delete();
					}
					catch (Exception)
					{
					}
				}
				files = directoryInfo.GetFiles("*.vamcachejson", SearchOption.AllDirectories);
				foreach (FileInfo fileInfo3 in files)
				{
					try
					{
						fileInfo3.Delete();
					}
					catch (Exception)
					{
					}
				}
				files = directoryInfo.GetFiles("*.avi", SearchOption.AllDirectories);
				foreach (FileInfo fileInfo4 in files)
				{
					try
					{
						fileInfo4.Delete();
					}
					catch (Exception)
					{
					}
				}
				files = directoryInfo.GetFiles("*.mp4", SearchOption.AllDirectories);
				foreach (FileInfo fileInfo5 in files)
				{
					try
					{
						fileInfo5.Delete();
					}
					catch (Exception)
					{
					}
				}
				if (this.cacheSizeUI != null)
				{
					this.cacheSizeUI.UpdateUsed();
				}
			}
		}

		// Token: 0x06005603 RID: 22019 RVA: 0x001F7314 File Offset: 0x001F5714
		private void Awake()
		{
			CacheManager.singleton = this;
			this.callbacks = new List<CacheManager.CacheSizeCallback>();
			if (this.CacheDir == null)
			{
				this.CacheDir = this.defaultCacheDir;
				this.InitCache();
			}
		}

		// Token: 0x06005604 RID: 22020 RVA: 0x001F7344 File Offset: 0x001F5744
		private void OnDisable()
		{
			if (this.cacheSizeCoroutine != null)
			{
				base.StopCoroutine(this.cacheSizeCoroutine);
				this.cacheSizeCoroutine = null;
				this.cacheSizeRecalcThread = null;
			}
		}

		// Token: 0x06005605 RID: 22021 RVA: 0x001F736B File Offset: 0x001F576B
		private void OnDestroy()
		{
			CacheManager.CachingEnabled = false;
			this._cacheDir = null;
			CacheManager.singleton = null;
		}

		// Token: 0x06005606 RID: 22022 RVA: 0x001F7380 File Offset: 0x001F5780
		// Note: this type is marked as 'beforefieldinit'.
		static CacheManager()
		{
		}

		// Token: 0x0400472C RID: 18220
		public static CacheManager singleton;

		// Token: 0x0400472D RID: 18221
		protected static string textureFolderName = "Textures";

		// Token: 0x0400472E RID: 18222
		protected static string packageJSONFolderName = "PackageJSON";

		// Token: 0x0400472F RID: 18223
		protected static string videoFolderName = "Videos";

		// Token: 0x04004730 RID: 18224
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static bool <CachingEnabled>k__BackingField;

		// Token: 0x04004731 RID: 18225
		public string defaultCacheDir = "Cache";

		// Token: 0x04004732 RID: 18226
		protected string _cacheDir;

		// Token: 0x04004733 RID: 18227
		public CacheSizeUI cacheSizeUI;

		// Token: 0x04004734 RID: 18228
		protected List<CacheManager.CacheSizeCallback> callbacks;

		// Token: 0x04004735 RID: 18229
		protected float cacheSize;

		// Token: 0x04004736 RID: 18230
		protected float cacheSizeThreaded;

		// Token: 0x04004737 RID: 18231
		protected Thread cacheSizeRecalcThread;

		// Token: 0x04004738 RID: 18232
		protected Coroutine cacheSizeCoroutine;

		// Token: 0x02000BD6 RID: 3030
		// (Invoke) Token: 0x06005608 RID: 22024
		public delegate void CacheSizeCallback(float f);

		// Token: 0x02000FE3 RID: 4067
		[CompilerGenerated]
		private sealed class <GetCacheSizeCo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060075F3 RID: 30195 RVA: 0x001F73A0 File Offset: 0x001F57A0
			[DebuggerHidden]
			public <GetCacheSizeCo>c__Iterator0()
			{
			}

			// Token: 0x060075F4 RID: 30196 RVA: 0x001F73A8 File Offset: 0x001F57A8
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (this.cacheSizeRecalcThread == null)
					{
						this.cacheSizeRecalcThread = new Thread(new ThreadStart(this.<>m__0));
						this.cacheSizeRecalcThread.Start();
					}
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (this.cacheSizeRecalcThread.IsAlive)
				{
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.cacheSize = this.cacheSizeThreaded;
				this.cacheSizeRecalcThread = null;
				this.cacheSizeCoroutine = null;
				enumerator = this.callbacks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						CacheManager.CacheSizeCallback cacheSizeCallback = enumerator.Current;
						if (cacheSizeCallback != null)
						{
							cacheSizeCallback(this.cacheSize);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				this.callbacks.Clear();
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001167 RID: 4455
			// (get) Token: 0x060075F5 RID: 30197 RVA: 0x001F7504 File Offset: 0x001F5904
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001168 RID: 4456
			// (get) Token: 0x060075F6 RID: 30198 RVA: 0x001F750C File Offset: 0x001F590C
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060075F7 RID: 30199 RVA: 0x001F7514 File Offset: 0x001F5914
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060075F8 RID: 30200 RVA: 0x001F7524 File Offset: 0x001F5924
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060075F9 RID: 30201 RVA: 0x001F752B File Offset: 0x001F592B
			internal void <>m__0()
			{
				base.GetCacheSizeThreaded();
			}

			// Token: 0x040069BA RID: 27066
			internal List<CacheManager.CacheSizeCallback>.Enumerator $locvar0;

			// Token: 0x040069BB RID: 27067
			internal CacheManager $this;

			// Token: 0x040069BC RID: 27068
			internal object $current;

			// Token: 0x040069BD RID: 27069
			internal bool $disposing;

			// Token: 0x040069BE RID: 27070
			internal int $PC;
		}
	}
}
