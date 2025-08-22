using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Leap.Unity.Query;
using LeapInternal;
using UnityEngine;
using UnityEngine.Serialization;

namespace Leap.Unity
{
	// Token: 0x020006F5 RID: 1781
	[RequireComponent(typeof(Camera))]
	[RequireComponent(typeof(LeapServiceProvider))]
	public class LeapImageRetriever : MonoBehaviour
	{
		// Token: 0x06002B21 RID: 11041 RVA: 0x000E8EA7 File Offset: 0x000E72A7
		public LeapImageRetriever()
		{
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06002B22 RID: 11042 RVA: 0x000E8ED2 File Offset: 0x000E72D2
		public LeapImageRetriever.EyeTextureData TextureData
		{
			get
			{
				return this._eyeTextureData;
			}
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x000E8EDA File Offset: 0x000E72DA
		private void Awake()
		{
			this._provider = base.GetComponent<LeapServiceProvider>();
			if (this._provider == null)
			{
				this._provider = base.GetComponentInChildren<LeapServiceProvider>();
			}
			MemoryManager.EnablePooling = true;
			this.ApplyGammaCorrectionValues();
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x000E8F11 File Offset: 0x000E7311
		private void OnEnable()
		{
			this.subscribeToService();
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x000E8F19 File Offset: 0x000E7319
		private void OnDisable()
		{
			this.unsubscribeFromService();
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x000E8F24 File Offset: 0x000E7324
		private void OnDestroy()
		{
			base.StopAllCoroutines();
			Controller leapController = this._provider.GetLeapController();
			if (leapController != null)
			{
				this._provider.GetLeapController().DistortionChange -= new EventHandler<DistortionEventArgs>(this.onDistortionChange);
			}
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000E8F68 File Offset: 0x000E7368
		private void LateUpdate()
		{
			Frame currentFrame = this._provider.CurrentFrame;
			this._currentImage = null;
			Image image;
			while (this._imageQueue.TryPeek(out image))
			{
				if (image.SequenceId > currentFrame.Id)
				{
					break;
				}
				this._currentImage = image;
				this._imageQueue.TryDequeue();
			}
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000E8FC8 File Offset: 0x000E73C8
		private void OnPreRender()
		{
			if (this._currentImage != null)
			{
				if (this._eyeTextureData.CheckStale(this._currentImage))
				{
					this._eyeTextureData.Reconstruct(this._currentImage);
				}
				this._eyeTextureData.UpdateTextures(this._currentImage);
			}
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x000E9018 File Offset: 0x000E7418
		private void subscribeToService()
		{
			if (this._serviceCoroutine != null)
			{
				return;
			}
			this._serviceCoroutine = base.StartCoroutine(this.serviceCoroutine());
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x000E9038 File Offset: 0x000E7438
		private void unsubscribeFromService()
		{
			if (this._serviceCoroutine != null)
			{
				base.StopCoroutine(this._serviceCoroutine);
				this._serviceCoroutine = null;
			}
			Controller leapController = this._provider.GetLeapController();
			if (leapController != null)
			{
				leapController.ClearPolicy(Controller.PolicyFlag.POLICY_IMAGES);
				leapController.ImageReady -= this.onImageReady;
				leapController.DistortionChange -= new EventHandler<DistortionEventArgs>(this.onDistortionChange);
			}
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x000E90A0 File Offset: 0x000E74A0
		private IEnumerator serviceCoroutine()
		{
			Controller controller = null;
			do
			{
				controller = this._provider.GetLeapController();
				yield return null;
			}
			while (controller == null);
			controller.SetPolicy(Controller.PolicyFlag.POLICY_IMAGES);
			controller.ImageReady += this.onImageReady;
			controller.DistortionChange += new EventHandler<DistortionEventArgs>(this.onDistortionChange);
			yield break;
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x000E90BC File Offset: 0x000E74BC
		private void onImageReady(object sender, ImageEventArgs args)
		{
			Image image = args.image;
			this._imageQueue.TryEnqueue(image);
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x000E90E0 File Offset: 0x000E74E0
		public void ApplyGammaCorrectionValues()
		{
			float value = 1f;
			if (QualitySettings.activeColorSpace != ColorSpace.Linear)
			{
				value = -Mathf.Log10(Mathf.GammaToLinearSpace(0.1f));
			}
			Shader.SetGlobalFloat("_LeapGlobalColorSpaceGamma", value);
			Shader.SetGlobalFloat("_LeapGlobalGammaCorrectionExponent", 1f / this._gammaCorrection);
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x000E9130 File Offset: 0x000E7530
		private void onDistortionChange(object sender, LeapEventArgs args)
		{
			this._eyeTextureData.MarkStale();
		}

		// Token: 0x040022E2 RID: 8930
		public const string GLOBAL_COLOR_SPACE_GAMMA_NAME = "_LeapGlobalColorSpaceGamma";

		// Token: 0x040022E3 RID: 8931
		public const string GLOBAL_GAMMA_CORRECTION_EXPONENT_NAME = "_LeapGlobalGammaCorrectionExponent";

		// Token: 0x040022E4 RID: 8932
		public const string GLOBAL_CAMERA_PROJECTION_NAME = "_LeapGlobalProjection";

		// Token: 0x040022E5 RID: 8933
		public const int IMAGE_WARNING_WAIT = 10;

		// Token: 0x040022E6 RID: 8934
		public const int LEFT_IMAGE_INDEX = 0;

		// Token: 0x040022E7 RID: 8935
		public const int RIGHT_IMAGE_INDEX = 1;

		// Token: 0x040022E8 RID: 8936
		public const float IMAGE_SETTING_POLL_RATE = 2f;

		// Token: 0x040022E9 RID: 8937
		[SerializeField]
		[FormerlySerializedAs("gammaCorrection")]
		private float _gammaCorrection = 1f;

		// Token: 0x040022EA RID: 8938
		private LeapServiceProvider _provider;

		// Token: 0x040022EB RID: 8939
		private LeapImageRetriever.EyeTextureData _eyeTextureData = new LeapImageRetriever.EyeTextureData();

		// Token: 0x040022EC RID: 8940
		protected ProduceConsumeBuffer<Image> _imageQueue = new ProduceConsumeBuffer<Image>(32);

		// Token: 0x040022ED RID: 8941
		protected Image _currentImage;

		// Token: 0x040022EE RID: 8942
		private Coroutine _serviceCoroutine;

		// Token: 0x020006F6 RID: 1782
		public class LeapTextureData
		{
			// Token: 0x06002B2F RID: 11055 RVA: 0x000E913D File Offset: 0x000E753D
			public LeapTextureData()
			{
			}

			// Token: 0x1700056F RID: 1391
			// (get) Token: 0x06002B30 RID: 11056 RVA: 0x000E9145 File Offset: 0x000E7545
			public Texture2D CombinedTexture
			{
				get
				{
					return this._combinedTexture;
				}
			}

			// Token: 0x06002B31 RID: 11057 RVA: 0x000E9150 File Offset: 0x000E7550
			public bool CheckStale(Image image)
			{
				return this._combinedTexture == null || this._intermediateArray == null || (image.Width != this._combinedTexture.width || image.Height * 2 != this._combinedTexture.height) || this._combinedTexture.format != this.getTextureFormat(image);
			}

			// Token: 0x06002B32 RID: 11058 RVA: 0x000E91C8 File Offset: 0x000E75C8
			public void Reconstruct(Image image, string globalShaderName, string pixelSizeName)
			{
				int width = image.Width;
				int num = image.Height * 2;
				TextureFormat textureFormat = this.getTextureFormat(image);
				if (this._combinedTexture != null)
				{
					UnityEngine.Object.DestroyImmediate(this._combinedTexture);
				}
				this._combinedTexture = new Texture2D(width, num, textureFormat, false, true);
				this._combinedTexture.wrapMode = TextureWrapMode.Clamp;
				this._combinedTexture.filterMode = FilterMode.Bilinear;
				this._combinedTexture.name = globalShaderName;
				this._combinedTexture.hideFlags = HideFlags.DontSave;
				this._intermediateArray = new byte[width * num * this.bytesPerPixel(textureFormat)];
				Shader.SetGlobalTexture(globalShaderName, this._combinedTexture);
				Shader.SetGlobalVector(pixelSizeName, new Vector2(1f / (float)image.Width, 1f / (float)image.Height));
			}

			// Token: 0x06002B33 RID: 11059 RVA: 0x000E9296 File Offset: 0x000E7696
			public void UpdateTexture(Image image)
			{
				this._combinedTexture.LoadRawTextureData(image.Data(Image.CameraType.LEFT));
				this._combinedTexture.Apply();
			}

			// Token: 0x06002B34 RID: 11060 RVA: 0x000E92B8 File Offset: 0x000E76B8
			private TextureFormat getTextureFormat(Image image)
			{
				Image.FormatType format = image.Format;
				if (format != Image.FormatType.INFRARED)
				{
					throw new Exception("Unexpected image format " + image.Format + "!");
				}
				return TextureFormat.Alpha8;
			}

			// Token: 0x06002B35 RID: 11061 RVA: 0x000E92F8 File Offset: 0x000E76F8
			private int bytesPerPixel(TextureFormat format)
			{
				if (format != TextureFormat.Alpha8)
				{
					throw new Exception("Unexpected texture format " + format);
				}
				return 1;
			}

			// Token: 0x040022EF RID: 8943
			private Texture2D _combinedTexture;

			// Token: 0x040022F0 RID: 8944
			private byte[] _intermediateArray;
		}

		// Token: 0x020006F7 RID: 1783
		public class LeapDistortionData
		{
			// Token: 0x06002B36 RID: 11062 RVA: 0x000E931D File Offset: 0x000E771D
			public LeapDistortionData()
			{
			}

			// Token: 0x17000570 RID: 1392
			// (get) Token: 0x06002B37 RID: 11063 RVA: 0x000E9325 File Offset: 0x000E7725
			public Texture2D CombinedTexture
			{
				get
				{
					return this._combinedTexture;
				}
			}

			// Token: 0x06002B38 RID: 11064 RVA: 0x000E932D File Offset: 0x000E772D
			public bool CheckStale()
			{
				return this._combinedTexture == null;
			}

			// Token: 0x06002B39 RID: 11065 RVA: 0x000E933C File Offset: 0x000E773C
			public void Reconstruct(Image image, string shaderName)
			{
				int num = image.DistortionWidth / 2;
				int num2 = image.DistortionHeight * 2;
				if (this._combinedTexture != null)
				{
					UnityEngine.Object.DestroyImmediate(this._combinedTexture);
				}
				Color32[] array = new Color32[num * num2];
				this._combinedTexture = new Texture2D(num, num2, TextureFormat.RGBA32, false, true);
				this._combinedTexture.filterMode = FilterMode.Bilinear;
				this._combinedTexture.wrapMode = TextureWrapMode.Clamp;
				this._combinedTexture.hideFlags = HideFlags.DontSave;
				this.addDistortionData(image, array, 0);
				this._combinedTexture.SetPixels32(array);
				this._combinedTexture.Apply();
				Shader.SetGlobalTexture(shaderName, this._combinedTexture);
			}

			// Token: 0x06002B3A RID: 11066 RVA: 0x000E93E4 File Offset: 0x000E77E4
			private void addDistortionData(Image image, Color32[] colors, int startIndex)
			{
				float[] array = image.Distortion(Image.CameraType.LEFT).Query<float>().Concat(image.Distortion(Image.CameraType.RIGHT)).ToArray<float>();
				for (int i = 0; i < array.Length; i += 2)
				{
					byte r;
					byte g;
					this.encodeFloat(array[i], out r, out g);
					byte b;
					byte a;
					this.encodeFloat(array[i + 1], out b, out a);
					colors[i / 2 + startIndex] = new Color32(r, g, b, a);
				}
			}

			// Token: 0x06002B3B RID: 11067 RVA: 0x000E945C File Offset: 0x000E785C
			private void encodeFloat(float value, out byte byte0, out byte byte1)
			{
				value = (value + 0.6f) / 2.3f;
				float num = value;
				float num2 = value * 255f;
				num -= (float)((int)num);
				num2 -= (float)((int)num2);
				num -= 0.003921569f * num2;
				byte0 = (byte)(num * 256f);
				byte1 = (byte)(num2 * 256f);
			}

			// Token: 0x040022F1 RID: 8945
			private Texture2D _combinedTexture;
		}

		// Token: 0x020006F8 RID: 1784
		public class EyeTextureData
		{
			// Token: 0x06002B3C RID: 11068 RVA: 0x000E94AC File Offset: 0x000E78AC
			public EyeTextureData()
			{
				this.TextureData = new LeapImageRetriever.LeapTextureData();
				this.Distortion = new LeapImageRetriever.LeapDistortionData();
			}

			// Token: 0x06002B3D RID: 11069 RVA: 0x000E94CC File Offset: 0x000E78CC
			public static void ResetGlobalShaderValues()
			{
				Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false, false);
				texture2D.name = "EmptyTexture";
				texture2D.hideFlags = HideFlags.DontSave;
				texture2D.SetPixel(0, 0, new Color(0f, 0f, 0f, 0f));
				Shader.SetGlobalTexture("_LeapGlobalRawTexture", texture2D);
				Shader.SetGlobalTexture("_LeapGlobalDistortion", texture2D);
			}

			// Token: 0x06002B3E RID: 11070 RVA: 0x000E952E File Offset: 0x000E792E
			public bool CheckStale(Image image)
			{
				return this.TextureData.CheckStale(image) || this.Distortion.CheckStale() || this._isStale;
			}

			// Token: 0x06002B3F RID: 11071 RVA: 0x000E955A File Offset: 0x000E795A
			public void MarkStale()
			{
				this._isStale = true;
			}

			// Token: 0x06002B40 RID: 11072 RVA: 0x000E9563 File Offset: 0x000E7963
			public void Reconstruct(Image image)
			{
				this.TextureData.Reconstruct(image, "_LeapGlobalRawTexture", "_LeapGlobalRawPixelSize");
				this.Distortion.Reconstruct(image, "_LeapGlobalDistortion");
				this._isStale = false;
			}

			// Token: 0x06002B41 RID: 11073 RVA: 0x000E9593 File Offset: 0x000E7993
			public void UpdateTextures(Image image)
			{
				this.TextureData.UpdateTexture(image);
			}

			// Token: 0x040022F2 RID: 8946
			private const string GLOBAL_RAW_TEXTURE_NAME = "_LeapGlobalRawTexture";

			// Token: 0x040022F3 RID: 8947
			private const string GLOBAL_DISTORTION_TEXTURE_NAME = "_LeapGlobalDistortion";

			// Token: 0x040022F4 RID: 8948
			private const string GLOBAL_RAW_PIXEL_SIZE_NAME = "_LeapGlobalRawPixelSize";

			// Token: 0x040022F5 RID: 8949
			public readonly LeapImageRetriever.LeapTextureData TextureData;

			// Token: 0x040022F6 RID: 8950
			public readonly LeapImageRetriever.LeapDistortionData Distortion;

			// Token: 0x040022F7 RID: 8951
			private bool _isStale;
		}

		// Token: 0x02000FAB RID: 4011
		[CompilerGenerated]
		private sealed class <serviceCoroutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060074BC RID: 29884 RVA: 0x000E95A1 File Offset: 0x000E79A1
			[DebuggerHidden]
			public <serviceCoroutine>c__Iterator0()
			{
			}

			// Token: 0x060074BD RID: 29885 RVA: 0x000E95AC File Offset: 0x000E79AC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					controller = null;
					break;
				case 1U:
					if (controller != null)
					{
						controller.SetPolicy(Controller.PolicyFlag.POLICY_IMAGES);
						controller.ImageReady += base.onImageReady;
						controller.DistortionChange += new EventHandler<DistortionEventArgs>(base.onDistortionChange);
						this.$PC = -1;
						return false;
					}
					break;
				default:
					return false;
				}
				controller = this._provider.GetLeapController();
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}

			// Token: 0x17001135 RID: 4405
			// (get) Token: 0x060074BE RID: 29886 RVA: 0x000E966B File Offset: 0x000E7A6B
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001136 RID: 4406
			// (get) Token: 0x060074BF RID: 29887 RVA: 0x000E9673 File Offset: 0x000E7A73
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060074C0 RID: 29888 RVA: 0x000E967B File Offset: 0x000E7A7B
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060074C1 RID: 29889 RVA: 0x000E968B File Offset: 0x000E7A8B
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040068D1 RID: 26833
			internal Controller <controller>__0;

			// Token: 0x040068D2 RID: 26834
			internal LeapImageRetriever $this;

			// Token: 0x040068D3 RID: 26835
			internal object $current;

			// Token: 0x040068D4 RID: 26836
			internal bool $disposing;

			// Token: 0x040068D5 RID: 26837
			internal int $PC;
		}
	}
}
