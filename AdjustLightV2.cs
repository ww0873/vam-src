using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000D0A RID: 3338
public class AdjustLightV2 : JSONStorable
{
	// Token: 0x060065CB RID: 26059 RVA: 0x00266284 File Offset: 0x00264684
	public AdjustLightV2()
	{
	}

	// Token: 0x060065CC RID: 26060 RVA: 0x00266304 File Offset: 0x00264704
	protected void SyncEmissiveRenderers()
	{
		if (Application.isPlaying)
		{
			foreach (MeshRenderer meshRenderer in this.emissiveRenderers)
			{
				if (meshRenderer != null)
				{
					meshRenderer.enabled = this._on;
					foreach (Material material in meshRenderer.materials)
					{
						Color color = this._light.color * this._light.intensity;
						if (material.HasProperty("_MKGlowColor"))
						{
							material.SetColor("_MKGlowColor", color);
						}
						if (material.HasProperty("_Color"))
						{
							material.SetColor("_Color", color + Color.gray);
						}
					}
				}
			}
			foreach (MeshRenderer meshRenderer2 in this.offRenderers)
			{
				if (meshRenderer2 != null)
				{
					meshRenderer2.enabled = !this._on;
				}
			}
		}
	}

	// Token: 0x060065CD RID: 26061 RVA: 0x0026641F File Offset: 0x0026481F
	public void ToggleOn()
	{
		if (this.onJSONParam != null)
		{
			this.onJSONParam.val = !this.onJSONParam.val;
		}
	}

	// Token: 0x060065CE RID: 26062 RVA: 0x00266445 File Offset: 0x00264845
	public void SyncOn(bool val)
	{
		this._on = val;
		if (this._light != null)
		{
			this._light.enabled = val;
		}
		this.SyncEmissiveRenderers();
	}

	// Token: 0x060065CF RID: 26063 RVA: 0x00266471 File Offset: 0x00264871
	protected void SyncPointBias(float f)
	{
		this.pointBias = f;
		this.SetAutoShadowType();
	}

	// Token: 0x060065D0 RID: 26064 RVA: 0x00266480 File Offset: 0x00264880
	public void SyncIntensity(float val)
	{
		this._intensity = val;
		if (this._light != null)
		{
			this._light.intensity = val;
		}
		this.SyncEmissiveRenderers();
	}

	// Token: 0x060065D1 RID: 26065 RVA: 0x002664AC File Offset: 0x002648AC
	public void SyncRange(float val)
	{
		this._range = val;
		if (this._light != null)
		{
			this._light.range = val;
		}
	}

	// Token: 0x060065D2 RID: 26066 RVA: 0x002664D4 File Offset: 0x002648D4
	public void SyncColor(float h, float s, float v)
	{
		this._HSVcolor.H = h;
		this._HSVcolor.S = s;
		this._HSVcolor.V = v;
		this._color = HSVColorPicker.HSVToRGB(h, s, v);
		if (this._light != null)
		{
			this._light.color = this._color;
		}
		this.SyncEmissiveRenderers();
	}

	// Token: 0x060065D3 RID: 26067 RVA: 0x0026653C File Offset: 0x0026493C
	public void SyncShadowsOn(bool val)
	{
		this._shadowsOn = val;
		if (this._light != null)
		{
			if (this._shadowsOn)
			{
				this._light.shadows = this.saveShadowType;
			}
			else
			{
				this.saveShadowType = this._light.shadows;
				this._light.shadows = LightShadows.None;
			}
		}
	}

	// Token: 0x060065D4 RID: 26068 RVA: 0x0026659F File Offset: 0x0026499F
	public void SyncShadowStrength(float val)
	{
		this._shadowStrength = val;
		if (this._light != null)
		{
			this._light.shadowStrength = this._shadowStrength;
		}
	}

	// Token: 0x060065D5 RID: 26069 RVA: 0x002665CA File Offset: 0x002649CA
	public void SyncSpotAngle(float val)
	{
		this._spotAngle = val;
		if (this._light != null)
		{
			this._light.spotAngle = val;
		}
	}

	// Token: 0x060065D6 RID: 26070 RVA: 0x002665F0 File Offset: 0x002649F0
	protected void SyncShowHalo(bool val)
	{
		this._showHalo = val;
		Behaviour behaviour = (Behaviour)base.GetComponent("Halo");
		if (behaviour != null)
		{
			behaviour.enabled = this._showHalo;
		}
	}

	// Token: 0x060065D7 RID: 26071 RVA: 0x00266630 File Offset: 0x00264A30
	protected void SyncShowDust(bool val)
	{
		this._showDust = val;
		ParticleSystem component = base.GetComponent<ParticleSystem>();
		if (component != null)
		{
			component.emission.enabled = this._showDust;
		}
		ParticleSystemRenderer component2 = base.GetComponent<ParticleSystemRenderer>();
		if (component2 != null)
		{
			component2.enabled = this._showDust;
		}
	}

	// Token: 0x060065D8 RID: 26072 RVA: 0x0026668C File Offset: 0x00264A8C
	protected void SetAutoShadowType()
	{
		if (this.controlBias)
		{
			LightType type = this._light.type;
			if (type != LightType.Directional)
			{
				if (type != LightType.Point)
				{
					if (type == LightType.Spot)
					{
						this._light.shadowBias = this.spotBias;
					}
				}
				else
				{
					this._light.shadowBias = this.pointBias;
				}
			}
			else
			{
				this._light.shadowBias = this.directionalBias;
			}
		}
		if (this.controlNearPlane)
		{
			LightType type2 = this._light.type;
			if (type2 != LightType.Directional)
			{
				if (type2 != LightType.Point)
				{
					if (type2 == LightType.Spot)
					{
						this._light.shadowNearPlane = this.spotNearPlane;
					}
				}
				else
				{
					this._light.shadowNearPlane = this.pointNearPlane;
				}
			}
			else
			{
				this._light.shadowNearPlane = this.directionalNearPlane;
			}
		}
		if (this.autoShadowType)
		{
			if (this._light.type == LightType.Directional)
			{
				this.saveShadowType = LightShadows.Soft;
				if (this._light.shadows != LightShadows.None)
				{
					this.SetShadowType("Soft");
				}
			}
			else
			{
				this.saveShadowType = LightShadows.Hard;
				if (this._light.shadows != LightShadows.None)
				{
					this.SetShadowType("Hard");
				}
			}
		}
	}

	// Token: 0x060065D9 RID: 26073 RVA: 0x002667E4 File Offset: 0x00264BE4
	public void SetLightType(string type)
	{
		if (this._light != null)
		{
			try
			{
				LightType lightType = (LightType)Enum.Parse(typeof(LightType), type);
				if (this._light.type != lightType)
				{
					this._light.type = lightType;
					this.SetAutoShadowType();
				}
			}
			catch (ArgumentException)
			{
				Debug.LogError("Attempted to set light type to " + type + " which is not a valid light type");
			}
		}
	}

	// Token: 0x060065DA RID: 26074 RVA: 0x0026686C File Offset: 0x00264C6C
	public void SetShadowResolution(string res)
	{
		if (this._light != null)
		{
			try
			{
				LightShadowResolution lightShadowResolution = (LightShadowResolution)Enum.Parse(typeof(LightShadowResolution), res);
				if (this._light.shadowResolution != lightShadowResolution)
				{
					this._light.shadowResolution = lightShadowResolution;
				}
			}
			catch (ArgumentException)
			{
				Debug.LogError("Attempted to set light shadow resolution " + res + " which is not a valid value");
			}
		}
	}

	// Token: 0x060065DB RID: 26075 RVA: 0x002668F0 File Offset: 0x00264CF0
	public void SetRenderMode(string mode)
	{
		if (this._light != null)
		{
			try
			{
				LightRenderMode lightRenderMode = (LightRenderMode)Enum.Parse(typeof(LightRenderMode), mode);
				if (this._light.renderMode != lightRenderMode)
				{
					this._light.renderMode = lightRenderMode;
				}
			}
			catch (ArgumentException)
			{
				Debug.LogError("Attempted to set light render mode to " + mode + " which is not a valid light render mode");
			}
		}
	}

	// Token: 0x060065DC RID: 26076 RVA: 0x00266974 File Offset: 0x00264D74
	public void SetShadowType(string type)
	{
		if (this._light != null)
		{
			try
			{
				LightShadows lightShadows = (LightShadows)Enum.Parse(typeof(LightShadows), type);
				if (this._light.shadows != lightShadows)
				{
					this._light.shadows = lightShadows;
				}
			}
			catch (ArgumentException)
			{
				Debug.LogError("Attempted to set light shadow type to " + type + " which is not a valid light shadow type");
			}
		}
	}

	// Token: 0x060065DD RID: 26077 RVA: 0x002669F8 File Offset: 0x00264DF8
	protected void Init()
	{
		this._light = base.GetComponent<Light>();
		if (this._light != null)
		{
			this._on = this._light.enabled;
			this.onJSONParam = new JSONStorableBool("on", this._on, new JSONStorableBool.SetBoolCallback(this.SyncOn));
			base.RegisterBool(this.onJSONParam);
			this.toggleOnJSONAction = new JSONStorableAction("ToggleOn", new JSONStorableAction.ActionCallback(this.ToggleOn));
			base.RegisterAction(this.toggleOnJSONAction);
			this._intensity = this._light.intensity;
			this.intensityJSONParam = new JSONStorableFloat("intensity", this._intensity, new JSONStorableFloat.SetFloatCallback(this.SyncIntensity), 0f, 8f, true, true);
			base.RegisterFloat(this.intensityJSONParam);
			this._range = this._light.range;
			this.rangeJSONParam = new JSONStorableFloat("range", this._range, new JSONStorableFloat.SetFloatCallback(this.SyncRange), 0f, 25f, true, true);
			base.RegisterFloat(this.rangeJSONParam);
			this._color = this._light.color;
			this._HSVcolor = HSVColorPicker.RGBToHSV(this._color.r, this._color.g, this._color.b);
			this.colorJSONParam = new JSONStorableColor("color", this._HSVcolor, new JSONStorableColor.SetHSVColorCallback(this.SyncColor));
			base.RegisterColor(this.colorJSONParam);
			this._spotAngle = this._light.spotAngle;
			this.spotAngleJSONParam = new JSONStorableFloat("spotAngle", this._spotAngle, new JSONStorableFloat.SetFloatCallback(this.SyncSpotAngle), 1f, 180f, true, true);
			base.RegisterFloat(this.spotAngleJSONParam);
			this.pointBiasJSON = new JSONStorableFloat("pointBias", this.pointBias, new JSONStorableFloat.SetFloatCallback(this.SyncPointBias), 0f, 0.03f, true, true);
			base.RegisterFloat(this.pointBiasJSON);
			ParticleSystem component = base.GetComponent<ParticleSystem>();
			if (component != null)
			{
				this.showDustJSONParam = new JSONStorableBool("showDust", this._showDust, new JSONStorableBool.SetBoolCallback(this.SyncShowDust));
				base.RegisterBool(this.showDustJSONParam);
				this.SyncShowDust(this._showDust);
			}
			Behaviour x = (Behaviour)base.GetComponent("Halo");
			if (x != null)
			{
				this.showHaloJSONParam = new JSONStorableBool("showHalo", this._showHalo, new JSONStorableBool.SetBoolCallback(this.SyncShowHalo));
				base.RegisterBool(this.showHaloJSONParam);
				this.SyncShowHalo(this._showHalo);
			}
			if (this._light.shadows != LightShadows.None)
			{
				this._shadowsOn = true;
				this.saveShadowType = this._light.shadows;
			}
			else
			{
				this._shadowsOn = false;
				this.saveShadowType = LightShadows.Soft;
			}
			this.shadowsOnJSONParam = new JSONStorableBool("shadowsOn", this._shadowsOn, new JSONStorableBool.SetBoolCallback(this.SyncShadowsOn));
			base.RegisterBool(this.shadowsOnJSONParam);
			this._shadowStrength = this._light.shadowStrength;
			this.shadowStrengthJSONParam = new JSONStorableFloat("shadowStrength", this._shadowStrength, new JSONStorableFloat.SetFloatCallback(this.SyncShadowStrength), 0f, 1f, true, true);
			base.RegisterFloat(this.shadowStrengthJSONParam);
			string[] names = Enum.GetNames(typeof(LightType));
			List<string> choicesList = new List<string>(names);
			this.lightTypeJSON = new JSONStorableStringChooser("type", choicesList, this._light.type.ToString(), "Light Type", new JSONStorableStringChooser.SetStringCallback(this.SetLightType));
			base.RegisterStringChooser(this.lightTypeJSON);
			string[] names2 = Enum.GetNames(typeof(LightRenderMode));
			List<string> choicesList2 = new List<string>(names2);
			this.renderModeJSON = new JSONStorableStringChooser("renderType", choicesList2, this._light.renderMode.ToString(), "Render Mode", new JSONStorableStringChooser.SetStringCallback(this.SetRenderMode));
			base.RegisterStringChooser(this.renderModeJSON);
			string[] names3 = Enum.GetNames(typeof(LightShadowResolution));
			List<string> choicesList3 = new List<string>(names3);
			this.shadowResolutionJSON = new JSONStorableStringChooser("shadowResolution", choicesList3, this._light.shadowResolution.ToString(), "Shadow Resolution", new JSONStorableStringChooser.SetStringCallback(this.SetShadowResolution));
			base.RegisterStringChooser(this.shadowResolutionJSON);
			this.SyncEmissiveRenderers();
		}
	}

	// Token: 0x060065DE RID: 26078 RVA: 0x00266E94 File Offset: 0x00265294
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			AdjustLightV2UI componentInChildren = this.UITransform.GetComponentInChildren<AdjustLightV2UI>(true);
			if (componentInChildren != null)
			{
				this.onJSONParam.toggle = componentInChildren.onToggle;
				this.intensityJSONParam.slider = componentInChildren.intensitySlider;
				this.rangeJSONParam.slider = componentInChildren.rangeSlider;
				this.colorJSONParam.colorPicker = componentInChildren.colorPicker;
				this.spotAngleJSONParam.slider = componentInChildren.spotAngleSlider;
				if (this.showHaloJSONParam != null)
				{
					this.showHaloJSONParam.toggle = componentInChildren.showHaloToggle;
					if (componentInChildren.showHaloToggle != null)
					{
						componentInChildren.showHaloToggle.gameObject.SetActive(true);
					}
				}
				else if (componentInChildren.showHaloToggle != null)
				{
					componentInChildren.showHaloToggle.gameObject.SetActive(false);
				}
				if (this.showDustJSONParam != null)
				{
					this.showDustJSONParam.toggle = componentInChildren.showDustToggle;
					if (componentInChildren.showDustToggle != null)
					{
						componentInChildren.showDustToggle.gameObject.SetActive(true);
					}
				}
				else if (componentInChildren.showDustToggle != null)
				{
					componentInChildren.showDustToggle.gameObject.SetActive(false);
				}
				this.shadowsOnJSONParam.toggle = componentInChildren.shadowsToggle;
				this.shadowStrengthJSONParam.slider = componentInChildren.shadowStrengthSlider;
				this.pointBiasJSON.slider = componentInChildren.pointBiasSlider;
				this.lightTypeJSON.popup = componentInChildren.typeSelector;
				this.shadowResolutionJSON.popup = componentInChildren.shadowResolutionSelector;
				this.renderModeJSON.popup = componentInChildren.renderModeSelector;
			}
		}
	}

	// Token: 0x060065DF RID: 26079 RVA: 0x00267050 File Offset: 0x00265450
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			AdjustLightV2UI componentInChildren = this.UITransformAlt.GetComponentInChildren<AdjustLightV2UI>(true);
			if (componentInChildren != null)
			{
				this.onJSONParam.toggleAlt = componentInChildren.onToggle;
				this.intensityJSONParam.sliderAlt = componentInChildren.intensitySlider;
				this.rangeJSONParam.sliderAlt = componentInChildren.rangeSlider;
				this.colorJSONParam.colorPickerAlt = componentInChildren.colorPicker;
				this.spotAngleJSONParam.sliderAlt = componentInChildren.spotAngleSlider;
				if (this.showHaloJSONParam != null)
				{
					this.showHaloJSONParam.toggleAlt = componentInChildren.showHaloToggle;
					if (componentInChildren.showHaloToggle != null)
					{
						componentInChildren.showHaloToggle.gameObject.SetActive(true);
					}
				}
				else if (componentInChildren.showHaloToggle != null)
				{
					componentInChildren.showHaloToggle.gameObject.SetActive(false);
				}
				if (this.showDustJSONParam != null)
				{
					this.showDustJSONParam.toggleAlt = componentInChildren.showDustToggle;
					if (componentInChildren.showDustToggle != null)
					{
						componentInChildren.showDustToggle.gameObject.SetActive(true);
					}
				}
				else if (componentInChildren.showDustToggle != null)
				{
					componentInChildren.showDustToggle.gameObject.SetActive(false);
				}
				this.shadowsOnJSONParam.toggleAlt = componentInChildren.shadowsToggle;
				this.shadowStrengthJSONParam.sliderAlt = componentInChildren.shadowStrengthSlider;
				this.pointBiasJSON.sliderAlt = componentInChildren.pointBiasSlider;
				this.lightTypeJSON.popupAlt = componentInChildren.typeSelector;
				this.shadowResolutionJSON.popupAlt = componentInChildren.shadowResolutionSelector;
				this.renderModeJSON.popupAlt = componentInChildren.renderModeSelector;
			}
		}
	}

	// Token: 0x060065E0 RID: 26080 RVA: 0x0026720C File Offset: 0x0026560C
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

	// Token: 0x04005529 RID: 21801
	public bool controlBias = true;

	// Token: 0x0400552A RID: 21802
	public float pointBias = 0.001f;

	// Token: 0x0400552B RID: 21803
	public float spotBias = 0.001f;

	// Token: 0x0400552C RID: 21804
	public float directionalBias = 0.02f;

	// Token: 0x0400552D RID: 21805
	public bool controlNearPlane;

	// Token: 0x0400552E RID: 21806
	public float pointNearPlane = 0.1f;

	// Token: 0x0400552F RID: 21807
	public float spotNearPlane = 0.5f;

	// Token: 0x04005530 RID: 21808
	public float directionalNearPlane = 0.5f;

	// Token: 0x04005531 RID: 21809
	public MeshRenderer[] emissiveRenderers;

	// Token: 0x04005532 RID: 21810
	public MeshRenderer[] offRenderers;

	// Token: 0x04005533 RID: 21811
	public bool autoShadowType = true;

	// Token: 0x04005534 RID: 21812
	protected Light _light;

	// Token: 0x04005535 RID: 21813
	protected JSONStorableAction toggleOnJSONAction;

	// Token: 0x04005536 RID: 21814
	protected JSONStorableBool onJSONParam;

	// Token: 0x04005537 RID: 21815
	protected bool _on = true;

	// Token: 0x04005538 RID: 21816
	protected JSONStorableFloat pointBiasJSON;

	// Token: 0x04005539 RID: 21817
	protected JSONStorableFloat intensityJSONParam;

	// Token: 0x0400553A RID: 21818
	protected float _intensity;

	// Token: 0x0400553B RID: 21819
	protected JSONStorableFloat rangeJSONParam;

	// Token: 0x0400553C RID: 21820
	protected float _range;

	// Token: 0x0400553D RID: 21821
	protected JSONStorableColor colorJSONParam;

	// Token: 0x0400553E RID: 21822
	protected Color _color;

	// Token: 0x0400553F RID: 21823
	protected HSVColor _HSVcolor;

	// Token: 0x04005540 RID: 21824
	protected LightShadows saveShadowType = LightShadows.Soft;

	// Token: 0x04005541 RID: 21825
	protected JSONStorableBool shadowsOnJSONParam;

	// Token: 0x04005542 RID: 21826
	protected bool _shadowsOn;

	// Token: 0x04005543 RID: 21827
	protected JSONStorableFloat shadowStrengthJSONParam;

	// Token: 0x04005544 RID: 21828
	protected float _shadowStrength;

	// Token: 0x04005545 RID: 21829
	protected JSONStorableFloat spotAngleJSONParam;

	// Token: 0x04005546 RID: 21830
	protected float _spotAngle;

	// Token: 0x04005547 RID: 21831
	protected JSONStorableBool showHaloJSONParam;

	// Token: 0x04005548 RID: 21832
	[SerializeField]
	protected bool _showHalo = true;

	// Token: 0x04005549 RID: 21833
	protected JSONStorableBool showDustJSONParam;

	// Token: 0x0400554A RID: 21834
	[SerializeField]
	protected bool _showDust = true;

	// Token: 0x0400554B RID: 21835
	protected JSONStorableStringChooser lightTypeJSON;

	// Token: 0x0400554C RID: 21836
	protected JSONStorableStringChooser shadowResolutionJSON;

	// Token: 0x0400554D RID: 21837
	protected JSONStorableStringChooser renderModeJSON;
}
