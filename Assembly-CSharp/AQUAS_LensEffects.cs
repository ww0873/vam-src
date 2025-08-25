using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PostProcessing;

// Token: 0x02000015 RID: 21
public class AQUAS_LensEffects : MonoBehaviour
{
	// Token: 0x060000AF RID: 175 RVA: 0x00005E14 File Offset: 0x00004214
	public AQUAS_LensEffects()
	{
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x060000B0 RID: 176 RVA: 0x00005E78 File Offset: 0x00004278
	// (set) Token: 0x060000B1 RID: 177 RVA: 0x00005E80 File Offset: 0x00004280
	public bool underWater
	{
		[CompilerGenerated]
		get
		{
			return this.<underWater>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<underWater>k__BackingField = value;
		}
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00005E8C File Offset: 0x0000428C
	private void Start()
	{
		if (this.gameObjects.mainCamera.GetComponent<PostProcessingBehaviour>() == null)
		{
			this.gameObjects.mainCamera.AddComponent<PostProcessingBehaviour>();
		}
		this.postProcessing = this.gameObjects.mainCamera.GetComponent<PostProcessingBehaviour>();
		this.waterLensAudio = this.gameObjects.waterLens.GetComponent<AudioSource>();
		this.airLensAudio = this.gameObjects.airLens.GetComponent<AudioSource>();
		this.audioComp = base.GetComponent<AudioSource>();
		this.cameraAudio = this.gameObjects.mainCamera.GetComponent<AudioSource>();
		this.bubbleBehaviour = this.gameObjects.bubble.GetComponent<AQUAS_BubbleBehaviour>();
		this.gameObjects.airLens.SetActive(true);
		this.gameObjects.waterLens.SetActive(false);
		this.airLensMaterial = this.gameObjects.airLens.GetComponent<Renderer>().material;
		this.waterPlaneMaterial = this.gameObjects.waterPlanes[0].GetComponent<Renderer>().material;
		this.t = this.wetLens.wetTime + this.wetLens.dryingTime;
		this.t2 = 0f;
		this.bubbleSpawnTimer = 0f;
		this.defaultFog = RenderSettings.fog;
		this.defaultFogDensity = RenderSettings.fogDensity;
		this.defaultFogColor = RenderSettings.fogColor;
		this.defaultFoamContrast = this.waterPlaneMaterial.GetFloat("_FoamContrast");
		this.defaultSpecularity = this.waterPlaneMaterial.GetFloat("_Specular");
		if (this.waterPlaneMaterial.HasProperty("_Refraction"))
		{
			this.defaultRefraction = this.waterPlaneMaterial.GetFloat("_Refraction");
		}
		this.postProcessing.profile = this.underWaterParameters.defaultProfile;
		this.audioComp.clip = this.soundEffects.sounds[0];
		this.audioComp.loop = true;
		this.audioComp.Stop();
		this.airLensAudio.clip = this.soundEffects.sounds[1];
		this.airLensAudio.loop = false;
		this.airLensAudio.Stop();
		this.waterLensAudio.clip = this.soundEffects.sounds[2];
		this.waterLensAudio.loop = false;
		this.waterLensAudio.Stop();
		if (GameObject.Find("Tenkoku DynamicSky") != null)
		{
			this.tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
		}
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00006110 File Offset: 0x00004510
	private void Update()
	{
		this.CheckIfStillUnderWater();
		if (this.underWater)
		{
			this.t = 0f;
			this.t2 += Time.deltaTime;
			this.gameObjects.airLens.SetActive(false);
			this.gameObjects.waterLens.SetActive(true);
			this.sprayFrameIndex = 0;
			this.rundown = true;
			this.BubbleSpawner();
			if (this.playUnderwater)
			{
				this.audioComp.Play();
				this.playUnderwater = false;
			}
			if (this.playDiveSplash)
			{
				this.waterLensAudio.Play();
				this.playDiveSplash = false;
			}
			this.playSurfaceSplash = true;
			this.airLensAudio.Stop();
			this.cameraAudio.enabled = false;
			this.airLensAudio.volume = this.soundEffects.surfacingVolume;
			this.audioComp.volume = this.soundEffects.diveVolume;
			this.waterLensAudio.volume = this.soundEffects.underwaterVolume;
			if (this.primaryCausticsProjector != null)
			{
				this.primaryCausticsProjector.material.SetTextureScale("_Texture", new Vector2(this.causticSettings.causticTiling.y, this.causticSettings.causticTiling.y));
				this.primaryCausticsProjector.material.SetFloat("_Intensity", this.causticSettings.causticIntensity.y);
				this.primaryAquasCaustics.maxCausticDepth = this.causticSettings.maxCausticDepth;
			}
			if (this.secondaryCausticsProjector != null)
			{
				this.secondaryCausticsProjector.material.SetTextureScale("_Texture", new Vector2(this.causticSettings.causticTiling.y, this.causticSettings.causticTiling.y));
				this.secondaryCausticsProjector.material.SetFloat("_Intensity", this.causticSettings.causticIntensity.y);
				this.secondaryAquasCaustics.maxCausticDepth = this.causticSettings.maxCausticDepth;
			}
			this.waterPlaneMaterial.SetFloat("_UnderwaterMode", 1f);
			this.waterPlaneMaterial.SetFloat("_FoamContrast", 0f);
			this.waterPlaneMaterial.SetFloat("_Specular", this.defaultSpecularity * 5f);
			this.waterPlaneMaterial.SetFloat("_Refraction", 0.7f);
			if (this.postProcessing.profile != this.underWaterParameters.underwaterProfile)
			{
				this.postProcessing.profile = this.underWaterParameters.underwaterProfile;
			}
			if (this.tenkokuObj != null)
			{
				Component component = this.tenkokuObj.GetComponent("TenkokuModule");
				FieldInfo field = component.GetType().GetField("enableFog", BindingFlags.Instance | BindingFlags.Public);
				if (field != null)
				{
					field.SetValue(component, false);
				}
			}
			RenderSettings.fog = true;
			RenderSettings.fogDensity = this.underWaterParameters.fogDensity;
			RenderSettings.fogColor = this.underWaterParameters.fogColor;
		}
		else
		{
			this.t2 = 0f;
			this.t += Time.deltaTime;
			this.gameObjects.airLens.SetActive(true);
			this.gameObjects.waterLens.SetActive(false);
			if (this.rundown)
			{
				this.sprayFrameIndex = 0;
				this.NextFrame();
				base.InvokeRepeating("NextFrame", 1f / this.wetLens.rundownSpeed, 1f / this.wetLens.rundownSpeed);
				this.rundown = false;
			}
			this.bubbleCount = 0;
			this.maxBubbleCount = UnityEngine.Random.Range(this.bubbleSpawnCriteria.minBubbleCount, this.bubbleSpawnCriteria.maxBubbleCount);
			this.bubbleSpawnTimer = 0f;
			if (this.playSurfaceSplash)
			{
				this.airLensAudio.Play();
				this.playSurfaceSplash = false;
			}
			this.playUnderwater = true;
			this.playDiveSplash = true;
			this.audioComp.Stop();
			this.waterLensAudio.Stop();
			this.cameraAudio.enabled = true;
			if (this.primaryCausticsProjector != null)
			{
				this.primaryCausticsProjector.material.SetTextureScale("_Texture", new Vector2(this.causticSettings.causticTiling.x, this.causticSettings.causticTiling.x));
				this.primaryCausticsProjector.material.SetFloat("_Intensity", this.causticSettings.causticIntensity.x);
			}
			if (this.secondaryCausticsProjector != null)
			{
				this.secondaryCausticsProjector.material.SetTextureScale("_Texture", new Vector2(this.causticSettings.causticTiling.x, this.causticSettings.causticTiling.x));
				this.secondaryCausticsProjector.material.SetFloat("_Intensity", this.causticSettings.causticIntensity.x);
			}
			if (this.t <= this.wetLens.wetTime)
			{
				this.airLensMaterial.SetFloat("_Refraction", 1f);
				this.airLensMaterial.SetFloat("_Transparency", 0.01f);
			}
			else
			{
				this.airLensMaterial.SetFloat("_Refraction", Mathf.Lerp(1f, 0f, (this.t - this.wetLens.wetTime) / this.wetLens.dryingTime));
				this.airLensMaterial.SetFloat("_Transparency", Mathf.Lerp(0.01f, 0f, (this.t - this.wetLens.wetTime) / this.wetLens.dryingTime));
			}
			this.waterPlaneMaterial.SetFloat("_FoamContrast", this.defaultFoamContrast);
			this.waterPlaneMaterial.SetFloat("_UnderwaterMode", 0f);
			this.waterPlaneMaterial.SetFloat("_Specular", this.defaultSpecularity);
			this.waterPlaneMaterial.SetFloat("_Refraction", this.defaultRefraction);
			if (this.postProcessing.profile != this.underWaterParameters.defaultProfile)
			{
				this.postProcessing.profile = this.underWaterParameters.defaultProfile;
			}
			if (this.tenkokuObj != null)
			{
				Component component2 = this.tenkokuObj.GetComponent("TenkokuModule");
				FieldInfo field2 = component2.GetType().GetField("enableFog", BindingFlags.Instance | BindingFlags.Public);
				if (field2 != null)
				{
					field2.SetValue(component2, true);
				}
			}
			RenderSettings.fog = this.defaultFog;
			if (this.setAfloatFog)
			{
				RenderSettings.fogColor = this.defaultFogColor;
				RenderSettings.fogDensity = this.defaultFogDensity;
			}
		}
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x000067D8 File Offset: 0x00004BD8
	private bool CheckIfUnderWater(int waterPlanesCount)
	{
		if (!this.gameObjects.useSquaredPlanes)
		{
			for (int i = 0; i < waterPlanesCount; i++)
			{
				if (Mathf.Pow(base.transform.position.x - this.gameObjects.waterPlanes[i].transform.position.x, 2f) + Mathf.Pow(base.transform.position.z - this.gameObjects.waterPlanes[i].transform.position.z, 2f) < Mathf.Pow(this.gameObjects.waterPlanes[i].GetComponent<Renderer>().bounds.extents.x, 2f))
				{
					if (this.activePlane != this.lastActivePlane)
					{
						if (this.gameObjects.waterPlanes[this.activePlane].transform.Find("PrimaryCausticsProjector") != null)
						{
							this.primaryCausticsProjector = this.gameObjects.waterPlanes[this.activePlane].transform.Find("PrimaryCausticsProjector").GetComponent<Projector>();
							this.primaryAquasCaustics = this.gameObjects.waterPlanes[this.activePlane].transform.Find("PrimaryCausticsProjector").GetComponent<AQUAS_Caustics>();
						}
						if (this.gameObjects.waterPlanes[this.activePlane].transform.Find("SecondaryCausticsProjector") != null)
						{
							this.secondaryCausticsProjector = this.gameObjects.waterPlanes[this.activePlane].transform.Find("SecondaryCausticsProjector").GetComponent<Projector>();
							this.secondaryAquasCaustics = this.gameObjects.waterPlanes[this.activePlane].transform.Find("SecondaryCausticsProjector").GetComponent<AQUAS_Caustics>();
						}
						this.lastActivePlane = this.activePlane;
					}
					this.activePlane = i;
					if (base.transform.position.y < this.gameObjects.waterPlanes[i].transform.position.y)
					{
						this.waterPlaneMaterial = this.gameObjects.waterPlanes[i].GetComponent<Renderer>().material;
						this.activePlane = i;
						return true;
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < waterPlanesCount; j++)
			{
				if (Mathf.Abs(base.transform.position.x - this.gameObjects.waterPlanes[j].transform.position.x) < this.gameObjects.waterPlanes[j].GetComponent<Renderer>().bounds.extents.x && Mathf.Abs(base.transform.position.z - this.gameObjects.waterPlanes[j].transform.position.z) < this.gameObjects.waterPlanes[j].GetComponent<Renderer>().bounds.extents.z)
				{
					if (this.activePlane != this.lastActivePlane)
					{
						if (this.gameObjects.waterPlanes[this.activePlane].transform.Find("PrimaryCausticsProjector") != null)
						{
							this.primaryCausticsProjector = this.gameObjects.waterPlanes[this.activePlane].transform.Find("PrimaryCausticsProjector").GetComponent<Projector>();
							this.primaryAquasCaustics = this.gameObjects.waterPlanes[this.activePlane].transform.Find("PrimaryCausticsProjector").GetComponent<AQUAS_Caustics>();
						}
						if (this.gameObjects.waterPlanes[this.activePlane].transform.Find("SecondaryCausticsProjector") != null)
						{
							this.secondaryCausticsProjector = this.gameObjects.waterPlanes[this.activePlane].transform.Find("SecondaryCausticsProjector").GetComponent<Projector>();
							this.secondaryAquasCaustics = this.gameObjects.waterPlanes[this.activePlane].transform.Find("SecondaryCausticsProjector").GetComponent<AQUAS_Caustics>();
						}
						this.lastActivePlane = this.activePlane;
					}
					this.activePlane = j;
					if (base.transform.position.y < this.gameObjects.waterPlanes[j].transform.position.y)
					{
						this.waterPlaneMaterial = this.gameObjects.waterPlanes[0].GetComponent<Renderer>().material;
						this.activePlane = j;
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00006D20 File Offset: 0x00005120
	private void CheckIfStillUnderWater()
	{
		if (!this.gameObjects.useSquaredPlanes)
		{
			if (this.underWater && Mathf.Pow(base.transform.position.x - this.gameObjects.waterPlanes[this.activePlane].transform.position.x, 2f) + Mathf.Pow(base.transform.position.z - this.gameObjects.waterPlanes[this.activePlane].transform.position.z, 2f) > Mathf.Pow(this.gameObjects.waterPlanes[this.activePlane].GetComponent<Renderer>().bounds.extents.x, 2f))
			{
				this.underWater = false;
			}
			else if (this.underWater && base.transform.position.y > this.gameObjects.waterPlanes[this.activePlane].transform.position.y)
			{
				this.underWater = false;
			}
			else if (!this.underWater)
			{
				this.underWater = this.CheckIfUnderWater(this.gameObjects.waterPlanes.Count);
			}
		}
		else if ((this.underWater && Mathf.Abs(base.transform.position.x - this.gameObjects.waterPlanes[this.activePlane].transform.position.x) > this.gameObjects.waterPlanes[this.activePlane].GetComponent<Renderer>().bounds.extents.x) || (this.underWater && Mathf.Abs(base.transform.position.z - this.gameObjects.waterPlanes[this.activePlane].transform.position.z) > this.gameObjects.waterPlanes[this.activePlane].GetComponent<Renderer>().bounds.extents.z))
		{
			this.underWater = false;
		}
		else if (this.underWater && base.transform.position.y > this.gameObjects.waterPlanes[this.activePlane].transform.position.y)
		{
			this.underWater = false;
		}
		else if (!this.underWater)
		{
			this.underWater = this.CheckIfUnderWater(this.gameObjects.waterPlanes.Count);
		}
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00007040 File Offset: 0x00005440
	private void NextFrame()
	{
		if (this.sprayFrameIndex >= this.wetLens.sprayFrames.Length - 1)
		{
			this.sprayFrameIndex = 0;
			base.CancelInvoke("NextFrame");
		}
		this.airLensMaterial.SetTexture("_CutoutReferenceTexture", this.wetLens.sprayFramesCutout[this.sprayFrameIndex]);
		this.airLensMaterial.SetTexture("_Normal", this.wetLens.sprayFrames[this.sprayFrameIndex]);
		this.sprayFrameIndex++;
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x000070CC File Offset: 0x000054CC
	private void BubbleSpawner()
	{
		if (this.t2 > this.bubbleSpawnTimer && this.maxBubbleCount > this.bubbleCount)
		{
			float num = UnityEngine.Random.Range(0f, this.bubbleSpawnCriteria.avgScaleSummand * 2f);
			this.bubbleBehaviour.mainCamera = this.gameObjects.mainCamera;
			this.bubbleBehaviour.waterLevel = this.gameObjects.waterPlanes[this.activePlane].transform.position.y;
			this.bubbleBehaviour.averageUpdrift = this.bubbleSpawnCriteria.averageUpdrift + UnityEngine.Random.Range(-this.bubbleSpawnCriteria.averageUpdrift * 0.75f, this.bubbleSpawnCriteria.averageUpdrift * 0.75f);
			this.gameObjects.bubble.transform.localScale += new Vector3(num, num, num);
			UnityEngine.Object.Instantiate<GameObject>(this.gameObjects.bubble, new Vector3(base.transform.position.x + UnityEngine.Random.Range(-this.bubbleSpawnCriteria.maxSpawnDistance, this.bubbleSpawnCriteria.maxSpawnDistance), base.transform.position.y - 0.4f, base.transform.position.z + UnityEngine.Random.Range(-this.bubbleSpawnCriteria.maxSpawnDistance, this.bubbleSpawnCriteria.maxSpawnDistance)), Quaternion.identity);
			this.bubbleSpawnTimer += UnityEngine.Random.Range(this.bubbleSpawnCriteria.minSpawnTimer, this.bubbleSpawnCriteria.maxSpawnTimer);
			this.bubbleCount++;
			this.gameObjects.bubble.transform.localScale = new Vector3(this.bubbleSpawnCriteria.baseScale, this.bubbleSpawnCriteria.baseScale, this.bubbleSpawnCriteria.baseScale);
		}
		else if (this.t2 > this.bubbleSpawnTimer && this.maxBubbleCount == this.bubbleCount)
		{
			float num2 = UnityEngine.Random.Range(0f, this.bubbleSpawnCriteria.avgScaleSummand * 2f);
			this.bubbleBehaviour.mainCamera = this.gameObjects.mainCamera;
			this.bubbleBehaviour.waterLevel = this.gameObjects.waterPlanes[this.activePlane].transform.position.y;
			this.bubbleBehaviour.averageUpdrift = this.bubbleSpawnCriteria.averageUpdrift + UnityEngine.Random.Range(-this.bubbleSpawnCriteria.averageUpdrift * 0.75f, this.bubbleSpawnCriteria.averageUpdrift * 0.75f);
			this.gameObjects.bubble.transform.localScale += new Vector3(num2, num2, num2);
			UnityEngine.Object.Instantiate<GameObject>(this.gameObjects.bubble, new Vector3(base.transform.position.x + UnityEngine.Random.Range(-this.bubbleSpawnCriteria.maxSpawnDistance, this.bubbleSpawnCriteria.maxSpawnDistance), base.transform.position.y - 0.4f, base.transform.position.z + UnityEngine.Random.Range(-this.bubbleSpawnCriteria.maxSpawnDistance, this.bubbleSpawnCriteria.maxSpawnDistance)), Quaternion.identity);
			this.bubbleSpawnTimer += UnityEngine.Random.Range(this.bubbleSpawnCriteria.minSpawnTimerL, this.bubbleSpawnCriteria.maxSpawnTimerL);
			this.gameObjects.bubble.transform.localScale = new Vector3(this.bubbleSpawnCriteria.baseScale, this.bubbleSpawnCriteria.baseScale, this.bubbleSpawnCriteria.baseScale);
		}
	}

	// Token: 0x040000A8 RID: 168
	public AQUAS_Parameters.UnderWaterParameters underWaterParameters = new AQUAS_Parameters.UnderWaterParameters();

	// Token: 0x040000A9 RID: 169
	public AQUAS_Parameters.GameObjects gameObjects = new AQUAS_Parameters.GameObjects();

	// Token: 0x040000AA RID: 170
	public AQUAS_Parameters.BubbleSpawnCriteria bubbleSpawnCriteria = new AQUAS_Parameters.BubbleSpawnCriteria();

	// Token: 0x040000AB RID: 171
	public AQUAS_Parameters.WetLens wetLens = new AQUAS_Parameters.WetLens();

	// Token: 0x040000AC RID: 172
	public AQUAS_Parameters.CausticSettings causticSettings = new AQUAS_Parameters.CausticSettings();

	// Token: 0x040000AD RID: 173
	public AQUAS_Parameters.Audio soundEffects = new AQUAS_Parameters.Audio();

	// Token: 0x040000AE RID: 174
	private int sprayFrameIndex;

	// Token: 0x040000AF RID: 175
	private GameObject tenkokuObj;

	// Token: 0x040000B0 RID: 176
	private Material airLensMaterial;

	// Token: 0x040000B1 RID: 177
	private Material waterPlaneMaterial;

	// Token: 0x040000B2 RID: 178
	[HideInInspector]
	public float t;

	// Token: 0x040000B3 RID: 179
	private float t2;

	// Token: 0x040000B4 RID: 180
	private float bubbleSpawnTimer;

	// Token: 0x040000B5 RID: 181
	private float defaultFogDensity;

	// Token: 0x040000B6 RID: 182
	private Color defaultFogColor;

	// Token: 0x040000B7 RID: 183
	private float defaultFoamContrast;

	// Token: 0x040000B8 RID: 184
	private float defaultBloomIntensity;

	// Token: 0x040000B9 RID: 185
	private float defaultSpecularity;

	// Token: 0x040000BA RID: 186
	private float defaultRefraction;

	// Token: 0x040000BB RID: 187
	private bool defaultFog;

	// Token: 0x040000BC RID: 188
	private bool defaultSunShaftsEnabled;

	// Token: 0x040000BD RID: 189
	private bool defaultBloomEnabled;

	// Token: 0x040000BE RID: 190
	private bool defaultBlurEnabled;

	// Token: 0x040000BF RID: 191
	private bool defaultVignetteEnabled;

	// Token: 0x040000C0 RID: 192
	private bool defaultNoiseEnabled;

	// Token: 0x040000C1 RID: 193
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <underWater>k__BackingField;

	// Token: 0x040000C2 RID: 194
	[HideInInspector]
	public bool setAfloatFog = true;

	// Token: 0x040000C3 RID: 195
	[HideInInspector]
	public bool rundown;

	// Token: 0x040000C4 RID: 196
	private bool playSurfaceSplash;

	// Token: 0x040000C5 RID: 197
	private bool playDiveSplash;

	// Token: 0x040000C6 RID: 198
	private bool playUnderwater;

	// Token: 0x040000C7 RID: 199
	private int bubbleCount;

	// Token: 0x040000C8 RID: 200
	private int maxBubbleCount;

	// Token: 0x040000C9 RID: 201
	private int activePlane;

	// Token: 0x040000CA RID: 202
	private int lastActivePlane = 100;

	// Token: 0x040000CB RID: 203
	private FieldInfo fi;

	// Token: 0x040000CC RID: 204
	private PostProcessingBehaviour postProcessing;

	// Token: 0x040000CD RID: 205
	private AudioSource waterLensAudio;

	// Token: 0x040000CE RID: 206
	private AudioSource airLensAudio;

	// Token: 0x040000CF RID: 207
	private AudioSource audioComp;

	// Token: 0x040000D0 RID: 208
	private AudioSource cameraAudio;

	// Token: 0x040000D1 RID: 209
	private Projector primaryCausticsProjector;

	// Token: 0x040000D2 RID: 210
	private Projector secondaryCausticsProjector;

	// Token: 0x040000D3 RID: 211
	private AQUAS_Caustics primaryAquasCaustics;

	// Token: 0x040000D4 RID: 212
	private AQUAS_Caustics secondaryAquasCaustics;

	// Token: 0x040000D5 RID: 213
	private AQUAS_BubbleBehaviour bubbleBehaviour;
}
