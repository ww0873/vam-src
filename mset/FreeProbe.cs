using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace mset
{
	// Token: 0x02000325 RID: 805
	[RequireComponent(typeof(Camera))]
	public class FreeProbe : MonoBehaviour
	{
		// Token: 0x06001302 RID: 4866 RVA: 0x0006CD88 File Offset: 0x0006B188
		public FreeProbe()
		{
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06001303 RID: 4867 RVA: 0x0006CE1A File Offset: 0x0006B21A
		// (set) Token: 0x06001304 RID: 4868 RVA: 0x0006CE22 File Offset: 0x0006B222
		private Cubemap targetCube
		{
			get
			{
				return this._targetCube;
			}
			set
			{
				this._targetCube = value;
				this.UpdateFaceTexture();
			}
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0006CE34 File Offset: 0x0006B234
		private void UpdateFaceTexture()
		{
			if (this._targetCube == null)
			{
				return;
			}
			if (this.faceTexture == null || this.faceTexture.width != this._targetCube.width)
			{
				if (this.faceTexture)
				{
					UnityEngine.Object.DestroyImmediate(this.faceTexture);
				}
				this.faceTexture = new Texture2D(this._targetCube.width, this._targetCube.width, TextureFormat.ARGB32, true, false);
				this.RT = RenderTexture.GetTemporary(this._targetCube.width, this._targetCube.width, 24, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
				this.RT.Release();
				this.RT.dimension = TextureDimension.Tex2D;
				this.RT.useMipMap = false;
				this.RT.autoGenerateMips = false;
				this.RT.Create();
				if (!this.RT.IsCreated() && !this.RT.Create())
				{
					Debug.LogWarning("Failed to create HDR RenderTexture, capturing in LDR mode.");
					RenderTexture.ReleaseTemporary(this.RT);
					this.RT = null;
				}
			}
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0006CF5C File Offset: 0x0006B35C
		private void FreeFaceTexture()
		{
			if (this.faceTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.faceTexture);
				this.faceTexture = null;
			}
			if (this.RT)
			{
				if (RenderTexture.active == this.RT)
				{
					RenderTexture.active = null;
				}
				RenderTexture.ReleaseTemporary(this.RT);
				this.RT = null;
			}
			this.probeQueue = null;
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0006CFCF File Offset: 0x0006B3CF
		private void Start()
		{
			this.UpdateFaceTexture();
			this.convolveSkybox = new Material(Shader.Find("Hidden/Marmoset/RGBM Convolve"));
			this.convolveSkybox.name = "Internal Convolution Skybox";
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0006CFFC File Offset: 0x0006B3FC
		private void Awake()
		{
			this.sceneSkybox = RenderSettings.skybox;
			SkyManager skyManager = SkyManager.Get();
			if (skyManager && skyManager.ProbeCamera)
			{
				base.GetComponent<Camera>().CopyFrom(skyManager.ProbeCamera);
			}
			else if (Camera.main)
			{
				base.GetComponent<Camera>().CopyFrom(Camera.main);
			}
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0006D06C File Offset: 0x0006B46C
		public void QueueSkies(Sky[] skiesToProbe)
		{
			if (this.probeQueue == null)
			{
				this.probeQueue = new Queue<FreeProbe.ProbeTarget>();
			}
			else
			{
				this.probeQueue.Clear();
			}
			foreach (Sky sky in skiesToProbe)
			{
				if (sky != null && sky.SpecularCube as Cubemap != null)
				{
					this.QueueCubemap(sky.SpecularCube as Cubemap, sky.HDRSpec, sky.transform.position, sky.transform.rotation);
				}
			}
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x0006D108 File Offset: 0x0006B508
		public void QueueCubemap(Cubemap cube, bool HDR, Vector3 pos, Quaternion rot)
		{
			if (cube != null)
			{
				FreeProbe.ProbeTarget probeTarget = new FreeProbe.ProbeTarget();
				probeTarget.cube = cube;
				probeTarget.position = pos;
				probeTarget.rotation = rot;
				probeTarget.HDR = HDR;
				this.probeQueue.Enqueue(probeTarget);
				this.progressTotal++;
			}
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x0006D15E File Offset: 0x0006B55E
		private void ClearQueue()
		{
			this.probeQueue = null;
			this.progressTotal = 0;
			this.progress = 0;
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0006D178 File Offset: 0x0006B578
		public void RunQueue()
		{
			this.probeQueue.Enqueue(null);
			SkyProbe.buildRandomValueTable();
			SkyManager skyManager = SkyManager.Get();
			if (skyManager.ProbeCamera)
			{
				base.GetComponent<Camera>().CopyFrom(skyManager.ProbeCamera);
				this.defaultCullMask = skyManager.ProbeCamera.cullingMask;
			}
			else if (Camera.main)
			{
				base.GetComponent<Camera>().CopyFrom(Camera.main);
				this.defaultCullMask = base.GetComponent<Camera>().cullingMask;
			}
			this.disabledCameras.Clear();
			foreach (Camera camera in Camera.allCameras)
			{
				if (camera.enabled)
				{
					camera.enabled = false;
					this.disabledCameras.Add(camera);
				}
			}
			base.GetComponent<Camera>().enabled = true;
			base.GetComponent<Camera>().fieldOfView = 90f;
			base.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
			base.GetComponent<Camera>().cullingMask = this.defaultCullMask;
			base.GetComponent<Camera>().useOcclusionCulling = false;
			this.StartStage(FreeProbe.Stage.NEXTSKY);
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0006D298 File Offset: 0x0006B698
		private void StartStage(FreeProbe.Stage nextStage)
		{
			if (this.probeQueue == null)
			{
				nextStage = FreeProbe.Stage.DONE;
			}
			if (nextStage == FreeProbe.Stage.NEXTSKY)
			{
				RenderSettings.skybox = this.sceneSkybox;
				FreeProbe.ProbeTarget probeTarget = this.probeQueue.Dequeue();
				if (probeTarget != null)
				{
					this.progress++;
					if (this.ProgressCallback != null && this.progressTotal > 0)
					{
						this.ProgressCallback((float)this.progress / (float)this.progressTotal);
					}
					this.targetCube = probeTarget.cube;
					this.captureHDR = (probeTarget.HDR && this.RT != null);
					this.lookPos = probeTarget.position;
					this.lookRot = probeTarget.rotation;
				}
				else
				{
					nextStage = FreeProbe.Stage.DONE;
				}
			}
			if (nextStage == FreeProbe.Stage.CAPTURE)
			{
				this.drawShot = -1;
				RenderSettings.skybox = this.sceneSkybox;
				this.targetMip = 0;
				this.captureSize = this.targetCube.width;
				this.mipCount = QPow.Log2i(this.captureSize) - 1;
				base.GetComponent<Camera>().cullingMask = this.defaultCullMask;
			}
			if (nextStage == FreeProbe.Stage.CONVOLVE)
			{
				Shader.SetGlobalVector("_UniformOcclusion", Vector4.one);
				this.drawShot = 0;
				this.targetMip = 1;
				if (this.targetMip < this.mipCount)
				{
					base.GetComponent<Camera>().cullingMask = 0;
					RenderSettings.skybox = this.convolveSkybox;
					Matrix4x4 identity = Matrix4x4.identity;
					this.convolveSkybox.SetMatrix("_SkyMatrix", identity);
					this.convolveSkybox.SetTexture("_CubeHDR", this.targetCube);
					FreeProbe.toggleKeywordPair("MARMO_RGBM_INPUT_ON", "MARMO_RGBM_INPUT_OFF", this.captureHDR && this.RT != null);
					FreeProbe.toggleKeywordPair("MARMO_RGBM_OUTPUT_ON", "MARMO_RGBM_OUTPUT_OFF", this.captureHDR && this.RT != null);
					SkyProbe.bindRandomValueTable(this.convolveSkybox, "_PhongRands", this.targetCube.width);
				}
			}
			if (nextStage == FreeProbe.Stage.DONE)
			{
				RenderSettings.skybox = this.sceneSkybox;
				this.ClearQueue();
				this.FreeFaceTexture();
				foreach (Camera camera in this.disabledCameras)
				{
					camera.enabled = true;
				}
				this.disabledCameras.Clear();
				if (this.DoneCallback != null)
				{
					this.DoneCallback();
					this.DoneCallback = null;
				}
			}
			this.stage = nextStage;
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0006D538 File Offset: 0x0006B938
		private void OnPreCull()
		{
			if (this.stage == FreeProbe.Stage.CAPTURE || this.stage == FreeProbe.Stage.CONVOLVE || this.stage == FreeProbe.Stage.PRECAPTURE)
			{
				if (this.stage == FreeProbe.Stage.CONVOLVE)
				{
					this.captureSize = 1 << this.mipCount - this.targetMip;
					float value = (float)QPow.clampedDownShift(this.maxExponent, this.targetMip - 1, 1);
					this.convolveSkybox.SetFloat("_SpecularExp", value);
					this.convolveSkybox.SetFloat("_SpecularScale", this.convolutionScale);
				}
				if (this.stage == FreeProbe.Stage.CAPTURE || this.stage == FreeProbe.Stage.PRECAPTURE)
				{
					Shader.SetGlobalVector("_UniformOcclusion", this.exposures);
				}
				int num = this.captureSize;
				float width = (float)num / (float)Screen.width;
				float height = (float)num / (float)Screen.height;
				base.GetComponent<Camera>().rect = new Rect(0f, 0f, width, height);
				base.GetComponent<Camera>().pixelRect = new Rect(0f, 0f, (float)num, (float)num);
				base.transform.position = this.lookPos;
				base.transform.rotation = this.lookRot;
				if (this.stage == FreeProbe.Stage.CAPTURE || this.stage == FreeProbe.Stage.PRECAPTURE)
				{
					this.upLook = base.transform.up;
					this.forwardLook = base.transform.forward;
					this.rightLook = base.transform.right;
				}
				else
				{
					this.upLook = Vector3.up;
					this.forwardLook = Vector3.forward;
					this.rightLook = Vector3.right;
				}
				if (this.drawShot == 0)
				{
					base.transform.LookAt(this.lookPos + this.forwardLook, this.upLook);
				}
				else if (this.drawShot == 1)
				{
					base.transform.LookAt(this.lookPos - this.forwardLook, this.upLook);
				}
				else if (this.drawShot == 2)
				{
					base.transform.LookAt(this.lookPos - this.rightLook, this.upLook);
				}
				else if (this.drawShot == 3)
				{
					base.transform.LookAt(this.lookPos + this.rightLook, this.upLook);
				}
				else if (this.drawShot == 4)
				{
					base.transform.LookAt(this.lookPos + this.upLook, this.forwardLook);
				}
				else if (this.drawShot == 5)
				{
					base.transform.LookAt(this.lookPos - this.upLook, -this.forwardLook);
				}
				base.GetComponent<Camera>().ResetWorldToCameraMatrix();
			}
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0006D818 File Offset: 0x0006BC18
		private void Update()
		{
			this.frameID++;
			if (this.RT && this.captureHDR && this.stage == FreeProbe.Stage.CAPTURE)
			{
				this.stage = FreeProbe.Stage.PRECAPTURE;
				bool allowHDR = base.GetComponent<Camera>().allowHDR;
				base.GetComponent<Camera>().allowHDR = true;
				RenderTexture.active = RenderTexture.active;
				RenderTexture.active = this.RT;
				base.GetComponent<Camera>().targetTexture = this.RT;
				base.GetComponent<Camera>().Render();
				base.GetComponent<Camera>().allowHDR = allowHDR;
				base.GetComponent<Camera>().targetTexture = null;
				RenderTexture.active = null;
				this.stage = FreeProbe.Stage.CAPTURE;
			}
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0006D8D0 File Offset: 0x0006BCD0
		private void OnPostRender()
		{
			if (this.captureHDR && this.RT && this.stage == FreeProbe.Stage.CAPTURE)
			{
				int width = this.RT.width;
				int num = 0;
				int num2 = 0;
				if (!this.blitMat)
				{
					this.blitMat = new Material(Shader.Find("Hidden/Marmoset/RGBM Blit"));
				}
				FreeProbe.toggleKeywordPair("MARMO_RGBM_INPUT_ON", "MARMO_RGBM_INPUT_OFF", false);
				FreeProbe.toggleKeywordPair("MARMO_RGBM_OUTPUT_ON", "MARMO_RGBM_OUTPUT_OFF", true);
				GL.PushMatrix();
				GL.LoadPixelMatrix(0f, (float)width, (float)width, 0f);
				Graphics.DrawTexture(new Rect((float)num, (float)num2, (float)width, (float)width), this.RT, this.blitMat);
				GL.PopMatrix();
			}
			if (this.stage != FreeProbe.Stage.NEXTSKY)
			{
				if (this.stage == FreeProbe.Stage.CAPTURE || this.stage == FreeProbe.Stage.CONVOLVE)
				{
					int num3 = this.captureSize;
					bool convertHDR = !this.captureHDR;
					if (num3 > Screen.width || num3 > Screen.height)
					{
						Debug.LogWarning(string.Concat(new object[]
						{
							"<b>Skipping Cubemap</b> - The viewport is too small (",
							Screen.width,
							"x",
							Screen.height,
							") to probe the cubemap \"",
							this.targetCube.name,
							"\" (",
							num3,
							"x",
							num3,
							")"
						}));
						this.StartStage(FreeProbe.Stage.NEXTSKY);
						return;
					}
					if (this.drawShot == 0)
					{
						this.faceTexture.ReadPixels(new Rect(0f, 0f, (float)num3, (float)num3), 0, 0);
						this.faceTexture.Apply();
						FreeProbe.SetFacePixels(this.targetCube, CubemapFace.PositiveZ, this.faceTexture, this.targetMip, false, true, convertHDR);
					}
					else if (this.drawShot == 1)
					{
						this.faceTexture.ReadPixels(new Rect(0f, 0f, (float)num3, (float)num3), 0, 0);
						this.faceTexture.Apply();
						FreeProbe.SetFacePixels(this.targetCube, CubemapFace.NegativeZ, this.faceTexture, this.targetMip, false, true, convertHDR);
					}
					else if (this.drawShot == 2)
					{
						this.faceTexture.ReadPixels(new Rect(0f, 0f, (float)num3, (float)num3), 0, 0);
						this.faceTexture.Apply();
						FreeProbe.SetFacePixels(this.targetCube, CubemapFace.NegativeX, this.faceTexture, this.targetMip, false, true, convertHDR);
					}
					else if (this.drawShot == 3)
					{
						this.faceTexture.ReadPixels(new Rect(0f, 0f, (float)num3, (float)num3), 0, 0);
						this.faceTexture.Apply();
						FreeProbe.SetFacePixels(this.targetCube, CubemapFace.PositiveX, this.faceTexture, this.targetMip, false, true, convertHDR);
					}
					else if (this.drawShot == 4)
					{
						this.faceTexture.ReadPixels(new Rect(0f, 0f, (float)num3, (float)num3), 0, 0);
						this.faceTexture.Apply();
						FreeProbe.SetFacePixels(this.targetCube, CubemapFace.PositiveY, this.faceTexture, this.targetMip, true, false, convertHDR);
					}
					else if (this.drawShot == 5)
					{
						this.faceTexture.ReadPixels(new Rect(0f, 0f, (float)num3, (float)num3), 0, 0);
						this.faceTexture.Apply();
						FreeProbe.SetFacePixels(this.targetCube, CubemapFace.NegativeY, this.faceTexture, this.targetMip, true, false, convertHDR);
						if (this.stage == FreeProbe.Stage.CAPTURE)
						{
							this.targetCube.Apply(true, false);
							this.StartStage(FreeProbe.Stage.CONVOLVE);
							return;
						}
						this.targetCube.Apply(false, false);
						this.targetMip++;
						if (this.targetMip < this.mipCount)
						{
							this.drawShot = 0;
							return;
						}
						this.StartStage(FreeProbe.Stage.NEXTSKY);
						return;
					}
					this.drawShot++;
				}
				return;
			}
			if (this.targetCube != null)
			{
				this.StartStage(FreeProbe.Stage.CAPTURE);
				return;
			}
			this.StartStage(FreeProbe.Stage.DONE);
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0006DCFC File Offset: 0x0006C0FC
		private static void SetFacePixels(Cubemap cube, CubemapFace face, Texture2D tex, int mip, bool flipHorz, bool flipVert, bool convertHDR)
		{
			Color[] pixels = tex.GetPixels();
			Color color = Color.black;
			int num = tex.width >> mip;
			int num2 = tex.height >> mip;
			Color[] array = new Color[num * num2];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					int num3 = i + j * tex.width;
					int num4 = i + j * num;
					array[num4] = pixels[num3];
					if (convertHDR)
					{
						array[num4].a = 0.16666667f;
					}
				}
			}
			if (flipHorz)
			{
				for (int k = 0; k < num / 2; k++)
				{
					for (int l = 0; l < num2; l++)
					{
						int num5 = num - k - 1;
						int num6 = k + l * num;
						int num7 = num5 + l * num;
						color = array[num7];
						array[num7] = array[num6];
						array[num6] = color;
					}
				}
			}
			if (flipVert)
			{
				for (int m = 0; m < num; m++)
				{
					for (int n = 0; n < num2 / 2; n++)
					{
						int num8 = num2 - n - 1;
						int num9 = m + n * num;
						int num10 = m + num8 * num;
						color = array[num10];
						array[num10] = array[num9];
						array[num9] = color;
					}
				}
			}
			cube.SetPixels(array, face, mip);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0006DEC6 File Offset: 0x0006C2C6
		private static void toggleKeywordPair(string on, string off, bool yes)
		{
			if (yes)
			{
				Shader.EnableKeyword(on);
				Shader.DisableKeyword(off);
			}
			else
			{
				Shader.EnableKeyword(off);
				Shader.DisableKeyword(on);
			}
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x0006DEEB File Offset: 0x0006C2EB
		private static void toggleKeywordPair(Material mat, string on, string off, bool yes)
		{
			if (yes)
			{
				mat.EnableKeyword(on);
				mat.DisableKeyword(off);
			}
			else
			{
				mat.EnableKeyword(off);
				mat.DisableKeyword(on);
			}
		}

		// Token: 0x04001094 RID: 4244
		private RenderTexture RT;

		// Token: 0x04001095 RID: 4245
		public Action<float> ProgressCallback;

		// Token: 0x04001096 RID: 4246
		public Action DoneCallback;

		// Token: 0x04001097 RID: 4247
		public bool linear = true;

		// Token: 0x04001098 RID: 4248
		public int maxExponent = 512;

		// Token: 0x04001099 RID: 4249
		public Vector4 exposures = Vector4.one;

		// Token: 0x0400109A RID: 4250
		public float convolutionScale = 1f;

		// Token: 0x0400109B RID: 4251
		private List<Camera> disabledCameras = new List<Camera>();

		// Token: 0x0400109C RID: 4252
		private Cubemap _targetCube;

		// Token: 0x0400109D RID: 4253
		private Texture2D faceTexture;

		// Token: 0x0400109E RID: 4254
		private FreeProbe.Stage stage = FreeProbe.Stage.DONE;

		// Token: 0x0400109F RID: 4255
		private int drawShot;

		// Token: 0x040010A0 RID: 4256
		private int targetMip;

		// Token: 0x040010A1 RID: 4257
		private int mipCount;

		// Token: 0x040010A2 RID: 4258
		private int captureSize;

		// Token: 0x040010A3 RID: 4259
		private bool captureHDR = true;

		// Token: 0x040010A4 RID: 4260
		private int progress;

		// Token: 0x040010A5 RID: 4261
		private int progressTotal;

		// Token: 0x040010A6 RID: 4262
		private Vector3 lookPos = Vector3.zero;

		// Token: 0x040010A7 RID: 4263
		private Quaternion lookRot = Quaternion.identity;

		// Token: 0x040010A8 RID: 4264
		private Vector3 forwardLook = Vector3.forward;

		// Token: 0x040010A9 RID: 4265
		private Vector3 rightLook = Vector3.right;

		// Token: 0x040010AA RID: 4266
		private Vector3 upLook = Vector3.up;

		// Token: 0x040010AB RID: 4267
		private Queue<FreeProbe.ProbeTarget> probeQueue;

		// Token: 0x040010AC RID: 4268
		private int defaultCullMask = -1;

		// Token: 0x040010AD RID: 4269
		private Material sceneSkybox;

		// Token: 0x040010AE RID: 4270
		private Material convolveSkybox;

		// Token: 0x040010AF RID: 4271
		private int frameID;

		// Token: 0x040010B0 RID: 4272
		private Material blitMat;

		// Token: 0x02000326 RID: 806
		private enum Stage
		{
			// Token: 0x040010B2 RID: 4274
			NEXTSKY,
			// Token: 0x040010B3 RID: 4275
			PRECAPTURE,
			// Token: 0x040010B4 RID: 4276
			CAPTURE,
			// Token: 0x040010B5 RID: 4277
			CONVOLVE,
			// Token: 0x040010B6 RID: 4278
			DONE
		}

		// Token: 0x02000327 RID: 807
		private class ProbeTarget
		{
			// Token: 0x06001314 RID: 4884 RVA: 0x0006DF14 File Offset: 0x0006C314
			public ProbeTarget()
			{
			}

			// Token: 0x040010B7 RID: 4279
			public Cubemap cube;

			// Token: 0x040010B8 RID: 4280
			public bool HDR;

			// Token: 0x040010B9 RID: 4281
			public Vector3 position = Vector3.zero;

			// Token: 0x040010BA RID: 4282
			public Quaternion rotation = Quaternion.identity;
		}
	}
}
