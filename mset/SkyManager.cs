using System;
using System.Collections.Generic;
using UnityEngine;

namespace mset
{
	// Token: 0x02000337 RID: 823
	[ExecuteInEditMode]
	public class SkyManager : MonoBehaviour
	{
		// Token: 0x060013DA RID: 5082 RVA: 0x00071C8C File Offset: 0x0007008C
		public SkyManager()
		{
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x00071D45 File Offset: 0x00070145
		public static SkyManager Get()
		{
			if (SkyManager._Instance == null)
			{
				SkyManager._Instance = UnityEngine.Object.FindObjectOfType<SkyManager>();
			}
			return SkyManager._Instance;
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x00071D66 File Offset: 0x00070166
		// (set) Token: 0x060013DD RID: 5085 RVA: 0x00071D6E File Offset: 0x0007016E
		public bool BlendingSupport
		{
			get
			{
				return this._BlendingSupport;
			}
			set
			{
				this._BlendingSupport = value;
				Sky.EnableBlendingSupport(value);
				if (!value)
				{
					Sky.EnableTerrainBlending(false);
				}
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x00071D89 File Offset: 0x00070189
		// (set) Token: 0x060013DF RID: 5087 RVA: 0x00071D91 File Offset: 0x00070191
		public bool ProjectionSupport
		{
			get
			{
				return this._ProjectionSupport;
			}
			set
			{
				this._ProjectionSupport = value;
				Sky.EnableProjectionSupport(value);
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x00071DA0 File Offset: 0x000701A0
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x00071DA8 File Offset: 0x000701A8
		public Sky GlobalSky
		{
			get
			{
				return this._GlobalSky;
			}
			set
			{
				this.BlendToGlobalSky(value, 0f);
			}
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x00071DB6 File Offset: 0x000701B6
		public void BlendToGlobalSky(Sky next)
		{
			this.BlendToGlobalSky(next, this.GlobalBlendTime, 0f);
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x00071DCA File Offset: 0x000701CA
		public void BlendToGlobalSky(Sky next, float blendTime)
		{
			this.BlendToGlobalSky(next, blendTime, 0f);
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x00071DD9 File Offset: 0x000701D9
		public void BlendToGlobalSky(Sky next, float blendTime, float skipTime)
		{
			if (next != null)
			{
				this.nextSky = next;
				this.nextBlendTime = blendTime;
				this.nextSkipTime = skipTime;
			}
			this._GlobalSky = this.nextSky;
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00071E08 File Offset: 0x00070208
		private void ResetLightBlend()
		{
			if (this.nextLights != null)
			{
				for (int i = 0; i < this.nextLights.Length; i++)
				{
					this.nextLights[i].intensity = this.nextIntensities[i];
					this.nextLights[i].enabled = true;
				}
				this.nextLights = null;
				this.nextIntensities = null;
			}
			if (this.prevLights != null)
			{
				for (int j = 0; j < this.prevLights.Length; j++)
				{
					this.prevLights[j].intensity = this.prevIntensities[j];
					this.prevLights[j].enabled = false;
				}
				this.prevLights = null;
				this.prevIntensities = null;
			}
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x00071EC0 File Offset: 0x000702C0
		private void StartLightBlend(Sky prev, Sky next)
		{
			this.prevLights = null;
			this.prevIntensities = null;
			if (prev)
			{
				this.prevLights = prev.GetComponentsInChildren<Light>();
				if (this.prevLights != null && this.prevLights.Length > 0)
				{
					this.prevIntensities = new float[this.prevLights.Length];
					for (int i = 0; i < this.prevLights.Length; i++)
					{
						this.prevLights[i].enabled = true;
						this.prevIntensities[i] = this.prevLights[i].intensity;
					}
				}
			}
			this.nextLights = null;
			this.nextIntensities = null;
			if (next)
			{
				this.nextLights = next.GetComponentsInChildren<Light>();
				if (this.nextLights != null && this.nextLights.Length > 0)
				{
					this.nextIntensities = new float[this.nextLights.Length];
					for (int j = 0; j < this.nextLights.Length; j++)
					{
						this.nextIntensities[j] = this.nextLights[j].intensity;
						this.nextLights[j].enabled = true;
						this.nextLights[j].intensity = 0f;
					}
				}
			}
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00071FFC File Offset: 0x000703FC
		private void UpdateLightBlend()
		{
			if (this.GlobalBlender.IsBlending)
			{
				float blendWeight = this.GlobalBlender.BlendWeight;
				float num = 1f - blendWeight;
				for (int i = 0; i < this.prevLights.Length; i++)
				{
					this.prevLights[i].intensity = num * this.prevIntensities[i];
				}
				for (int j = 0; j < this.nextLights.Length; j++)
				{
					this.nextLights[j].intensity = blendWeight * this.nextIntensities[j];
				}
			}
			else
			{
				this.ResetLightBlend();
			}
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00072098 File Offset: 0x00070498
		private void HandleGlobalSkyChange()
		{
			if (this.nextSky != null)
			{
				this.ResetLightBlend();
				if (this.BlendingSupport && this.nextBlendTime > 0f)
				{
					Sky currentSky = this.GlobalBlender.CurrentSky;
					this.GlobalBlender.BlendTime = this.nextBlendTime;
					this.GlobalBlender.BlendToSky(this.nextSky);
					Sky[] array = UnityEngine.Object.FindObjectsOfType<Sky>();
					foreach (Sky sky in array)
					{
						sky.ToggleChildLights(false);
					}
					this.GlobalBlender.SkipTime(this.nextSkipTime);
					this.StartLightBlend(currentSky, this.nextSky);
				}
				else
				{
					this.GlobalBlender.SnapToSky(this.nextSky);
					this.nextSky.Apply(0);
					this.nextSky.Apply(1);
					Sky[] array3 = UnityEngine.Object.FindObjectsOfType<Sky>();
					foreach (Sky sky2 in array3)
					{
						sky2.ToggleChildLights(false);
					}
					this.nextSky.ToggleChildLights(true);
				}
				this._GlobalSky = this.nextSky;
				this.nextSky = null;
				if (!Application.isPlaying)
				{
					this.EditorApplySkies(true);
				}
			}
			this.UpdateLightBlend();
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x000721E8 File Offset: 0x000705E8
		private Material SkyboxMaterial
		{
			get
			{
				if (this._SkyboxMaterial == null)
				{
					this._SkyboxMaterial = Resources.Load<Material>("skyboxMat");
					if (!this._SkyboxMaterial)
					{
						Debug.LogError("Failed to find skyboxMat material in Resources folder!");
					}
				}
				return this._SkyboxMaterial;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x00072236 File Offset: 0x00070636
		// (set) Token: 0x060013EB RID: 5099 RVA: 0x00072240 File Offset: 0x00070640
		public bool ShowSkybox
		{
			get
			{
				return this._ShowSkybox;
			}
			set
			{
				if (value)
				{
					if (this.SkyboxMaterial && RenderSettings.skybox != this.SkyboxMaterial)
					{
						RenderSettings.skybox = this.SkyboxMaterial;
					}
				}
				else if (RenderSettings.skybox != null && (RenderSettings.skybox == this._SkyboxMaterial || RenderSettings.skybox.name == "Internal IBL Skybox"))
				{
					RenderSettings.skybox = null;
				}
				this._ShowSkybox = value;
			}
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x000722D4 File Offset: 0x000706D4
		private void Start()
		{
			Sky.ScrubGlobalKeywords();
			this._SkyboxMaterial = this.SkyboxMaterial;
			this.ShowSkybox = this._ShowSkybox;
			this.BlendingSupport = this._BlendingSupport;
			this.ProjectionSupport = this._ProjectionSupport;
			if (this._GlobalSky == null)
			{
				this._GlobalSky = base.gameObject.GetComponent<Sky>();
			}
			if (this._GlobalSky == null)
			{
				this._GlobalSky = UnityEngine.Object.FindObjectOfType<Sky>();
			}
			this.GlobalBlender.SnapToSky(this._GlobalSky);
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00072368 File Offset: 0x00070768
		public void RegisterApplicator(SkyApplicator app)
		{
			this.skyApplicators.Add(app);
			foreach (Renderer rend in this.dynamicRenderers)
			{
				app.RendererInside(rend);
			}
			foreach (Renderer rend2 in this.staticRenderers)
			{
				app.RendererInside(rend2);
			}
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00072420 File Offset: 0x00070820
		public void UnregisterApplicator(SkyApplicator app, HashSet<Renderer> renderersToClear)
		{
			this.skyApplicators.Remove(app);
			foreach (Renderer target in renderersToClear)
			{
				if (this._GlobalSky != null)
				{
					this._GlobalSky.Apply(target, 0);
				}
			}
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0007249C File Offset: 0x0007089C
		public void UnregisterRenderer(Renderer rend)
		{
			if (!this.dynamicRenderers.Remove(rend))
			{
				this.staticRenderers.Remove(rend);
			}
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x000724BC File Offset: 0x000708BC
		public void RegisterNewRenderer(Renderer rend)
		{
			if (!rend.gameObject.activeInHierarchy)
			{
				return;
			}
			int num = 1 << rend.gameObject.layer;
			if ((this.IgnoredLayerMask & num) != 0)
			{
				return;
			}
			if (rend.gameObject.isStatic)
			{
				if (!this.staticRenderers.Contains(rend))
				{
					this.staticRenderers.Add(rend);
					this.ApplyCorrectSky(rend);
				}
			}
			else if (!this.dynamicRenderers.Contains(rend))
			{
				this.dynamicRenderers.Add(rend);
				if (rend.GetComponent<SkyAnchor>() == null)
				{
					rend.gameObject.AddComponent(typeof(SkyAnchor));
				}
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x00072578 File Offset: 0x00070978
		public void SeekNewRenderers()
		{
			Renderer[] array = UnityEngine.Object.FindObjectsOfType<MeshRenderer>();
			for (int i = 0; i < array.Length; i++)
			{
				this.RegisterNewRenderer(array[i]);
			}
			array = UnityEngine.Object.FindObjectsOfType<SkinnedMeshRenderer>();
			for (int j = 0; j < array.Length; j++)
			{
				this.RegisterNewRenderer(array[j]);
			}
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x000725CC File Offset: 0x000709CC
		public void ApplyCorrectSky(Renderer rend)
		{
			bool flag = false;
			SkyAnchor component = rend.GetComponent<SkyAnchor>();
			if (component && component.BindType == SkyAnchor.AnchorBindType.TargetSky)
			{
				component.Apply();
				flag = true;
			}
			foreach (SkyApplicator skyApplicator in this.skyApplicators)
			{
				if (flag)
				{
					skyApplicator.RemoveRenderer(rend);
				}
				else if (skyApplicator.RendererInside(rend))
				{
					flag = true;
				}
			}
			if (!flag && this._GlobalSky != null)
			{
				if (component != null)
				{
					if (component.CurrentApplicator != null)
					{
						component.CurrentApplicator.RemoveRenderer(rend);
						component.CurrentApplicator = null;
					}
					component.BlendToGlobalSky(this._GlobalSky);
				}
				if (!this.globalSkyChildren.Contains(rend))
				{
					this.globalSkyChildren.Add(rend);
				}
			}
			if ((flag || this._GlobalSky == null) && this.globalSkyChildren.Contains(rend))
			{
				this.globalSkyChildren.Remove(rend);
			}
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x00072714 File Offset: 0x00070B14
		public void EditorUpdate(bool forceApply)
		{
			Sky.EnableGlobalProjection(true);
			Sky.EnableBlendingSupport(false);
			Sky.EnableTerrainBlending(false);
			if (this._GlobalSky)
			{
				this._GlobalSky.Apply(0);
				this._GlobalSky.Apply(1);
				if (this.SkyboxMaterial)
				{
					this._GlobalSky.Apply(this.SkyboxMaterial, 0);
					this._GlobalSky.Apply(this.SkyboxMaterial, 1);
				}
				this._GlobalSky.Dirty = false;
			}
			this.HandleGlobalSkyChange();
			if (this.EditorAutoApply)
			{
				this.EditorApplySkies(forceApply);
			}
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x000727B4 File Offset: 0x00070BB4
		private void EditorApplySkies(bool forceApply)
		{
			Shader.SetGlobalVector("_UniformOcclusion", Vector4.one);
			SkyApplicator[] apps = UnityEngine.Object.FindObjectsOfType<SkyApplicator>();
			object[] renderers = UnityEngine.Object.FindObjectsOfType<MeshRenderer>();
			this.EditorApplyToList(renderers, apps, forceApply);
			renderers = UnityEngine.Object.FindObjectsOfType<SkinnedMeshRenderer>();
			this.EditorApplyToList(renderers, apps, forceApply);
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x000727F4 File Offset: 0x00070BF4
		private void EditorApplyToList(object[] renderers, SkyApplicator[] apps, bool forceApply)
		{
			foreach (object obj in renderers)
			{
				Renderer renderer = (Renderer)obj;
				int num = 1 << renderer.gameObject.layer;
				if ((this.IgnoredLayerMask & num) == 0)
				{
					if (renderer.gameObject.activeInHierarchy)
					{
						if (forceApply)
						{
							MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
							materialPropertyBlock.Clear();
							renderer.SetPropertyBlock(materialPropertyBlock);
						}
						SkyAnchor skyAnchor = renderer.gameObject.GetComponent<SkyAnchor>();
						if (skyAnchor && !skyAnchor.enabled)
						{
							skyAnchor = null;
						}
						bool flag = renderer.transform.hasChanged || (skyAnchor && skyAnchor.HasChanged);
						bool flag2 = false;
						if (skyAnchor && skyAnchor.BindType == SkyAnchor.AnchorBindType.TargetSky)
						{
							skyAnchor.Apply();
							flag2 = true;
						}
						if (this.GameAutoApply && !flag2)
						{
							foreach (SkyApplicator skyApplicator in apps)
							{
								if (skyApplicator.gameObject.activeInHierarchy)
								{
									if (skyApplicator.TargetSky && (forceApply || skyApplicator.HasChanged || skyApplicator.TargetSky.Dirty || flag))
									{
										flag2 |= skyApplicator.ApplyInside(renderer);
										skyApplicator.TargetSky.Dirty = false;
									}
									skyApplicator.HasChanged = false;
								}
							}
						}
						if (!flag2 && this._GlobalSky && (forceApply || this._GlobalSky.Dirty || flag))
						{
							this._GlobalSky.Apply(renderer, 0);
						}
						renderer.transform.hasChanged = false;
						if (skyAnchor)
						{
							skyAnchor.HasChanged = false;
						}
					}
				}
			}
			if (forceApply && this._GlobalSky)
			{
				this._GlobalSky.Apply(0);
				if (this._SkyboxMaterial)
				{
					this._GlobalSky.Apply(this._SkyboxMaterial, 0);
				}
				this._GlobalSky.Dirty = false;
			}
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00072A4C File Offset: 0x00070E4C
		public void LateUpdate()
		{
			if (this.firstFrame && this._GlobalSky)
			{
				this.firstFrame = false;
				this._GlobalSky.Apply(0);
				this._GlobalSky.Apply(1);
				if (this._SkyboxMaterial)
				{
					this._GlobalSky.Apply(this._SkyboxMaterial, 0);
					this._GlobalSky.Apply(this._SkyboxMaterial, 1);
				}
			}
			float num = 0f;
			if (this.lastTimestamp > 0f)
			{
				num = Time.realtimeSinceStartup - this.lastTimestamp;
			}
			this.lastTimestamp = Time.realtimeSinceStartup;
			this.seekTimer -= num;
			this.HandleGlobalSkyChange();
			this.GameApplySkies(false);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00072B10 File Offset: 0x00070F10
		public void GameApplySkies(bool forceApply)
		{
			this.GlobalBlender.ApplyToTerrain();
			this.GlobalBlender.Apply();
			if (this._SkyboxMaterial)
			{
				this.GlobalBlender.Apply(this._SkyboxMaterial);
			}
			if (this.GameAutoApply || forceApply)
			{
				if (this.seekTimer <= 0f || forceApply)
				{
					this.SeekNewRenderers();
					this.seekTimer = 0.5f;
				}
				List<SkyApplicator> list = new List<SkyApplicator>();
				foreach (SkyApplicator skyApplicator in this.skyApplicators)
				{
					if (skyApplicator == null || skyApplicator.gameObject == null)
					{
						list.Add(skyApplicator);
					}
				}
				foreach (SkyApplicator item in list)
				{
					this.skyApplicators.Remove(item);
				}
				if (this.GlobalBlender.IsBlending || this.GlobalBlender.CurrentSky.Dirty || this.GlobalBlender.WasBlending(Time.deltaTime))
				{
					foreach (Renderer renderer in this.globalSkyChildren)
					{
						if (renderer)
						{
							SkyAnchor component = renderer.GetComponent<SkyAnchor>();
							if (component != null)
							{
								this.GlobalBlender.Apply(renderer, component.materials);
							}
						}
					}
				}
				int num = 0;
				int num2 = 0;
				List<Renderer> list2 = new List<Renderer>();
				foreach (Renderer renderer2 in this.dynamicRenderers)
				{
					num2++;
					if (forceApply || num2 >= this.renderCheckIterator)
					{
						if (renderer2 == null || renderer2.gameObject == null)
						{
							list2.Add(renderer2);
						}
						else if (renderer2.gameObject.activeInHierarchy)
						{
							this.renderCheckIterator++;
							if (!forceApply && num > 50)
							{
								num = 0;
								this.renderCheckIterator--;
								break;
							}
							SkyAnchor component2 = renderer2.GetComponent<SkyAnchor>();
							if (component2.HasChanged)
							{
								num++;
								component2.HasChanged = false;
								if (this.AutoMaterial)
								{
									component2.UpdateMaterials();
								}
								this.ApplyCorrectSky(renderer2);
							}
						}
					}
				}
				foreach (Renderer item2 in list2)
				{
					this.dynamicRenderers.Remove(item2);
				}
				if (this.renderCheckIterator >= this.dynamicRenderers.Count)
				{
					this.renderCheckIterator = 0;
				}
			}
			this._GlobalSky.Dirty = false;
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x00072EA4 File Offset: 0x000712A4
		public void ProbeSkies(GameObject[] objects, Sky[] skies, bool probeAll, bool probeIBL)
		{
			int num = 0;
			List<Sky> list = new List<Sky>();
			string str = string.Empty;
			if (skies != null)
			{
				foreach (Sky sky in skies)
				{
					if (sky)
					{
						if (probeAll || sky.IsProbe)
						{
							list.Add(sky);
						}
						else
						{
							num++;
							str = str + sky.name + "\n";
						}
					}
				}
			}
			if (objects != null)
			{
				foreach (GameObject gameObject in objects)
				{
					Sky component = gameObject.GetComponent<Sky>();
					if (component)
					{
						if (probeAll || component.IsProbe)
						{
							list.Add(component);
						}
						else
						{
							num++;
							str = str + component.name + "\n";
						}
					}
				}
			}
			if (num > 0)
			{
			}
			if (list.Count > 0)
			{
				this.ProbeExposures = ((!probeIBL) ? Vector4.zero : Vector4.one);
				this.SkiesToProbe = new Sky[list.Count];
				for (int k = 0; k < list.Count; k++)
				{
					this.SkiesToProbe[k] = list[k];
				}
			}
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00073005 File Offset: 0x00071405
		// Note: this type is marked as 'beforefieldinit'.
		static SkyManager()
		{
		}

		// Token: 0x0400112E RID: 4398
		private static SkyManager _Instance;

		// Token: 0x0400112F RID: 4399
		public bool LinearSpace = true;

		// Token: 0x04001130 RID: 4400
		[SerializeField]
		private bool _BlendingSupport = true;

		// Token: 0x04001131 RID: 4401
		[SerializeField]
		private bool _ProjectionSupport = true;

		// Token: 0x04001132 RID: 4402
		public bool GameAutoApply = true;

		// Token: 0x04001133 RID: 4403
		public bool EditorAutoApply = true;

		// Token: 0x04001134 RID: 4404
		public bool AutoMaterial;

		// Token: 0x04001135 RID: 4405
		public int IgnoredLayerMask;

		// Token: 0x04001136 RID: 4406
		public int[] _IgnoredLayers;

		// Token: 0x04001137 RID: 4407
		public int _IgnoredLayerCount;

		// Token: 0x04001138 RID: 4408
		[SerializeField]
		private Sky _GlobalSky;

		// Token: 0x04001139 RID: 4409
		[SerializeField]
		private SkyBlender GlobalBlender = new SkyBlender();

		// Token: 0x0400113A RID: 4410
		private Sky nextSky;

		// Token: 0x0400113B RID: 4411
		private float nextBlendTime;

		// Token: 0x0400113C RID: 4412
		private float nextSkipTime;

		// Token: 0x0400113D RID: 4413
		public float LocalBlendTime = 0.25f;

		// Token: 0x0400113E RID: 4414
		public float GlobalBlendTime = 0.25f;

		// Token: 0x0400113F RID: 4415
		private Light[] prevLights;

		// Token: 0x04001140 RID: 4416
		private Light[] nextLights;

		// Token: 0x04001141 RID: 4417
		private float[] prevIntensities;

		// Token: 0x04001142 RID: 4418
		private float[] nextIntensities;

		// Token: 0x04001143 RID: 4419
		private Material _SkyboxMaterial;

		// Token: 0x04001144 RID: 4420
		[SerializeField]
		private bool _ShowSkybox = true;

		// Token: 0x04001145 RID: 4421
		public Camera ProbeCamera;

		// Token: 0x04001146 RID: 4422
		private HashSet<Renderer> staticRenderers = new HashSet<Renderer>();

		// Token: 0x04001147 RID: 4423
		private HashSet<Renderer> dynamicRenderers = new HashSet<Renderer>();

		// Token: 0x04001148 RID: 4424
		private HashSet<Renderer> globalSkyChildren = new HashSet<Renderer>();

		// Token: 0x04001149 RID: 4425
		private HashSet<SkyApplicator> skyApplicators = new HashSet<SkyApplicator>();

		// Token: 0x0400114A RID: 4426
		private float seekTimer;

		// Token: 0x0400114B RID: 4427
		private float lastTimestamp = -1f;

		// Token: 0x0400114C RID: 4428
		private int renderCheckIterator;

		// Token: 0x0400114D RID: 4429
		private bool firstFrame = true;

		// Token: 0x0400114E RID: 4430
		public Sky[] SkiesToProbe;

		// Token: 0x0400114F RID: 4431
		public int ProbeExponent = 512;

		// Token: 0x04001150 RID: 4432
		public Vector4 ProbeExposures = Vector4.one;

		// Token: 0x04001151 RID: 4433
		public bool ProbeWithCubeRT = true;

		// Token: 0x04001152 RID: 4434
		public bool ProbeOnlyStatic;
	}
}
