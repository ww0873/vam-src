using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using mset;
using OldMoatGames;
using SimpleJSON;
using uFileBrowser;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D10 RID: 3344
public class SkyshopLightController : JSONStorable
{
	// Token: 0x06006614 RID: 26132 RVA: 0x0026910C File Offset: 0x0026750C
	public SkyshopLightController()
	{
	}

	// Token: 0x06006615 RID: 26133 RVA: 0x0026917D File Offset: 0x0026757D
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x06006616 RID: 26134 RVA: 0x00269188 File Offset: 0x00267588
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if ((includeAppearance || forceStore) && (this._currentSky != this._startingSky || forceStore))
		{
			json["skyName"] = this._skyName;
			this.needsStore = true;
		}
		return json;
	}

	// Token: 0x06006617 RID: 26135 RVA: 0x002691E8 File Offset: 0x002675E8
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.appearanceLocked && restoreAppearance && !base.IsCustomPhysicalParamLocked("skyName"))
		{
			if (jc["skyName"] != null)
			{
				this.SetSky(jc["skyName"]);
			}
			else if (setMissingToDefault)
			{
				this.skyName = this._startingSky.name;
			}
		}
	}

	// Token: 0x17000F0A RID: 3850
	// (get) Token: 0x06006618 RID: 26136 RVA: 0x0026926C File Offset: 0x0026766C
	// (set) Token: 0x06006619 RID: 26137 RVA: 0x00269274 File Offset: 0x00267674
	public bool overlaySkyActive
	{
		get
		{
			return this._overlaySkyActive;
		}
		set
		{
			if (this._overlaySkyActive != value)
			{
				this._overlaySkyActive = value;
				if (this.overlaySkyContainer != null)
				{
					this.overlaySkyContainer.gameObject.SetActive(this._overlaySkyActive);
				}
				this.SyncGlobalSky();
			}
		}
	}

	// Token: 0x0600661A RID: 26138 RVA: 0x002692C4 File Offset: 0x002676C4
	protected void SyncCurrentSky()
	{
		if (this._currentSky != null)
		{
			this._currentSky.MasterIntensity = this.masterIntensityJSON.val;
			this._currentSky.DiffIntensity = this.diffuseIntensityJSON.val;
			this._currentSky.SpecIntensity = this.specularIntensityJSON.val;
			this._currentSky.DiffIntensityLM = this.unityAmbientIntensityJSON.val;
			this._currentSky.CamExposure = this.camExposureJSON.val;
			this._currentSky.SkyIntensity = this.skyboxIntensityJSON.val;
		}
	}

	// Token: 0x0600661B RID: 26139 RVA: 0x00269368 File Offset: 0x00267768
	protected void SyncGlobalSky()
	{
		if (this.skyManager != null)
		{
			if (this.overlaySkyActive && this.overlaySky != null)
			{
				this.skyManager.GlobalSky = this.overlaySky;
			}
			else
			{
				this.skyManager.GlobalSky = this._currentSky;
			}
		}
	}

	// Token: 0x0600661C RID: 26140 RVA: 0x002693C9 File Offset: 0x002677C9
	protected void SetSky(string sname)
	{
		this.skyName = sname;
	}

	// Token: 0x17000F0B RID: 3851
	// (get) Token: 0x0600661D RID: 26141 RVA: 0x002693D2 File Offset: 0x002677D2
	// (set) Token: 0x0600661E RID: 26142 RVA: 0x002693DC File Offset: 0x002677DC
	public string skyName
	{
		get
		{
			return this._skyName;
		}
		set
		{
			if (this._skyName != value)
			{
				foreach (Sky sky in this.skies)
				{
					if (sky.name == value)
					{
						this._skyName = value;
						this._currentSky = sky;
						this.SyncCurrentSky();
						this.SyncGlobalSky();
						if (this.skyPopup != null)
						{
							this.skyPopup.currentValueNoCallback = this._skyName;
						}
						break;
					}
				}
			}
		}
	}

	// Token: 0x0600661F RID: 26143 RVA: 0x0026946B File Offset: 0x0026786B
	protected void SyncMasterIntensity(float f)
	{
		if (this._currentSky != null)
		{
			this._currentSky.MasterIntensity = f;
			this._currentSky.Apply();
		}
	}

	// Token: 0x06006620 RID: 26144 RVA: 0x00269495 File Offset: 0x00267895
	protected void SyncDiffuseIntensity(float f)
	{
		if (this._currentSky != null)
		{
			this._currentSky.DiffIntensity = f;
			this._currentSky.Apply();
		}
	}

	// Token: 0x06006621 RID: 26145 RVA: 0x002694BF File Offset: 0x002678BF
	protected void SyncSpecularIntensity(float f)
	{
		if (this._currentSky != null)
		{
			this._currentSky.SpecIntensity = f;
			this._currentSky.Apply();
		}
	}

	// Token: 0x06006622 RID: 26146 RVA: 0x002694E9 File Offset: 0x002678E9
	protected void SyncUnityAmbientIntensity(float f)
	{
		if (this._currentSky != null)
		{
			this._currentSky.DiffIntensityLM = f;
			this._currentSky.Apply();
		}
	}

	// Token: 0x06006623 RID: 26147 RVA: 0x00269513 File Offset: 0x00267913
	protected void SyncCamExposure(float f)
	{
		if (this._currentSky != null)
		{
			this._currentSky.CamExposure = f;
			this._currentSky.Apply();
		}
	}

	// Token: 0x06006624 RID: 26148 RVA: 0x00269540 File Offset: 0x00267940
	protected void SyncUnityAmbientColor(float h, float s, float v)
	{
		Color ambientLight = HSVColorPicker.HSVToRGB(h, s, v);
		RenderSettings.ambientLight = ambientLight;
	}

	// Token: 0x06006625 RID: 26149 RVA: 0x0026955C File Offset: 0x0026795C
	protected void SyncShowSkybox(bool b)
	{
		this._showSkyBox = b;
		if (this.skyContainer != null)
		{
			this.skyContainer.gameObject.SetActive(b);
		}
	}

	// Token: 0x06006626 RID: 26150 RVA: 0x00269587 File Offset: 0x00267987
	protected void SyncSkyboxIntensity(float f)
	{
		if (this._currentSky != null)
		{
			this._currentSky.SkyIntensity = f;
			this._currentSky.Apply();
		}
	}

	// Token: 0x06006627 RID: 26151 RVA: 0x002695B4 File Offset: 0x002679B4
	protected void SyncSkyboxYAngle(float f)
	{
		if (this.skyContainer != null)
		{
			Vector3 localEulerAngles = this.skyContainer.localEulerAngles;
			localEulerAngles.y = f;
			this.skyContainer.localEulerAngles = localEulerAngles;
		}
		if (this.skyContainer2 != null)
		{
			Vector3 localEulerAngles2 = this.skyContainer2.localEulerAngles;
			localEulerAngles2.y = f;
			this.skyContainer2.localEulerAngles = localEulerAngles2;
		}
	}

	// Token: 0x06006628 RID: 26152 RVA: 0x00269624 File Offset: 0x00267A24
	private IEnumerator FlashIter()
	{
		if (this.skyManager != null && this.skyManager.GlobalSky != null && this.camExposureJSON != null)
		{
			float currentFlashIntensity = this.flashIntensity;
			float flashDecerement = (currentFlashIntensity - this.camExposureJSON.val) / (float)this.flashFrames;
			for (int i = 0; i < this.flashFrames; i++)
			{
				this.skyManager.GlobalSky.CamExposure = currentFlashIntensity;
				this.skyManager.GlobalSky.Apply();
				currentFlashIntensity -= flashDecerement;
				yield return null;
			}
			this.skyManager.GlobalSky.CamExposure = this.camExposureJSON.val;
			this.skyManager.GlobalSky.Apply();
		}
		yield break;
	}

	// Token: 0x06006629 RID: 26153 RVA: 0x0026963F File Offset: 0x00267A3F
	public void Flash()
	{
		base.StartCoroutine(this.FlashIter());
	}

	// Token: 0x0600662A RID: 26154 RVA: 0x00269650 File Offset: 0x00267A50
	private IEnumerator SyncImage()
	{
		Texture2D tex = new Texture2D(4, 4, TextureFormat.DXT5, false);
		tex.wrapMode = TextureWrapMode.Clamp;
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
		}
		else
		{
			SuperController.LogError("Could not load image at " + urltoload + " Error: " + www.error);
		}
		yield break;
	}

	// Token: 0x0600662B RID: 26155 RVA: 0x0026966C File Offset: 0x00267A6C
	public void StartSyncImage()
	{
		if (UserPreferences.singleton == null || UserPreferences.singleton.enableWebMisc || !Regex.IsMatch(this.url, "^http"))
		{
			if (base.gameObject.activeInHierarchy)
			{
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
				else if (this._url.EndsWith(".jpg") || this._url.EndsWith(".jpeg") || this._url.EndsWith(".png"))
				{
					if (component != null)
					{
						component.enabled = false;
					}
					base.StartCoroutine(this.SyncImage());
				}
				else
				{
					UnityEngine.Debug.LogError("Unknown image type for url " + this._url);
				}
			}
			else
			{
				this.notActiveOnSync = true;
			}
		}
		else if (UserPreferences.singleton == null || !UserPreferences.singleton.hideDisabledWebMessages)
		{
			SuperController.LogError("Attempted to load http URL image when web load option is disabled. To enable, see User Preferences -> Web Security tab");
			SuperController.singleton.ShowMainHUDAuto();
			SuperController.singleton.SetActiveUI("MainMenu");
			SuperController.singleton.SetMainMenuTab("TabUserPrefs");
			SuperController.singleton.SetUserPrefsTab("TabSecurity");
		}
	}

	// Token: 0x17000F0C RID: 3852
	// (get) Token: 0x0600662C RID: 26156 RVA: 0x002697ED File Offset: 0x00267BED
	// (set) Token: 0x0600662D RID: 26157 RVA: 0x002697F8 File Offset: 0x00267BF8
	public string url
	{
		get
		{
			return this._url;
		}
		set
		{
			if (this._url != value)
			{
				this._url = value;
				if (this.urlInputField != null)
				{
					this.urlInputField.text = this._url;
				}
				if (this.urlInputFieldAlt != null)
				{
					this.urlInputFieldAlt.text = this._url;
				}
				this.StartSyncImage();
			}
		}
	}

	// Token: 0x0600662E RID: 26158 RVA: 0x00269867 File Offset: 0x00267C67
	public void CopyURLToClipboard()
	{
		GUIUtility.systemCopyBuffer = this._url;
	}

	// Token: 0x0600662F RID: 26159 RVA: 0x00269874 File Offset: 0x00267C74
	public void CopyURLFromClipboard()
	{
		this.url = GUIUtility.systemCopyBuffer;
	}

	// Token: 0x06006630 RID: 26160 RVA: 0x00269881 File Offset: 0x00267C81
	public void SetFilePath(string path)
	{
		if (path != null && path != string.Empty)
		{
			path = SuperController.singleton.NormalizeMediaPath(path);
			this.url = path;
		}
	}

	// Token: 0x06006631 RID: 26161 RVA: 0x002698B0 File Offset: 0x00267CB0
	public void FileBrowse()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.GetMediaPathDialog(new FileBrowserCallback(this.SetFilePath), "jpg|jpeg|png|gif", null, true, true, false, null, false, null, true, false);
		}
	}

	// Token: 0x06006632 RID: 26162 RVA: 0x002698F4 File Offset: 0x00267CF4
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			SkyshopLightControllerUI componentInChildren = this.UITransform.GetComponentInChildren<SkyshopLightControllerUI>();
			if (componentInChildren != null)
			{
				if (this.masterIntensityJSON != null)
				{
					this.masterIntensityJSON.slider = componentInChildren.masterIntensitySlider;
				}
				if (this.diffuseIntensityJSON != null)
				{
					this.diffuseIntensityJSON.slider = componentInChildren.diffuseIntensitySlider;
				}
				if (this.specularIntensityJSON != null)
				{
					this.specularIntensityJSON.slider = componentInChildren.specularIntensitySlider;
				}
				if (this.unityAmbientIntensityJSON != null)
				{
					this.unityAmbientIntensityJSON.slider = componentInChildren.unityAmbientIntensitySlider;
				}
				if (this.unityAmbientColorJSON != null)
				{
					this.unityAmbientColorJSON.colorPicker = componentInChildren.unityAmbientColorPicker;
				}
				if (this.camExposureJSON != null)
				{
					this.camExposureJSON.slider = componentInChildren.camExposureSlider;
				}
				if (this.showSkyboxJSON != null)
				{
					this.showSkyboxJSON.toggle = componentInChildren.showSkyboxToggle;
				}
				if (this.skyboxIntensityJSON != null)
				{
					this.skyboxIntensityJSON.slider = componentInChildren.skyboxIntensitySlider;
				}
				if (this.skyboxYAngleJSON != null)
				{
					this.skyboxYAngleJSON.slider = componentInChildren.skyboxYAngleSlider;
				}
				this.skyPopup = componentInChildren.skyPopup;
				if (this.skyPopup != null)
				{
					this.skyPopup.numPopupValues = this.skies.Length;
					int num = 0;
					foreach (Sky sky in this.skies)
					{
						this.skyPopup.setPopupValue(num, sky.name);
						num++;
					}
					this.skyPopup.currentValue = this._skyName;
					UIPopup uipopup = this.skyPopup;
					uipopup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetSky));
				}
			}
		}
	}

	// Token: 0x06006633 RID: 26163 RVA: 0x00269ACC File Offset: 0x00267ECC
	protected void Init()
	{
		SkyshopLightController.singleton = this;
		if (this.skyManager != null && this.skyManager.GlobalSky != null)
		{
			if (this.autoSkiesContainer != null)
			{
				this.skies = this.autoSkiesContainer.GetComponentsInChildren<Sky>();
			}
			this._startingSky = this.skyManager.GlobalSky;
			this._currentSky = this._startingSky;
			this._skyName = this._currentSky.name;
			this.masterIntensityJSON = new JSONStorableFloat("masterIntensity", this.skyManager.GlobalSky.MasterIntensity, new JSONStorableFloat.SetFloatCallback(this.SyncMasterIntensity), this.minMasterIntensity, this.maxMasterIntensity, true, true);
			base.RegisterFloat(this.masterIntensityJSON);
			this.diffuseIntensityJSON = new JSONStorableFloat("diffuseIntensity", this.skyManager.GlobalSky.DiffIntensity, new JSONStorableFloat.SetFloatCallback(this.SyncDiffuseIntensity), this.minDiffuseIntensity, this.maxDiffuseIntensity, true, true);
			base.RegisterFloat(this.diffuseIntensityJSON);
			this.specularIntensityJSON = new JSONStorableFloat("specularIntensity", this.skyManager.GlobalSky.SpecIntensity, new JSONStorableFloat.SetFloatCallback(this.SyncSpecularIntensity), this.minSpecularIntensity, this.maxSpecularIntensity, true, true);
			base.RegisterFloat(this.specularIntensityJSON);
			this.unityAmbientIntensityJSON = new JSONStorableFloat("unityAmbientIntensity", this.skyManager.GlobalSky.DiffIntensityLM, new JSONStorableFloat.SetFloatCallback(this.SyncUnityAmbientIntensity), 0f, 1f, true, true);
			base.RegisterFloat(this.unityAmbientIntensityJSON);
			Color ambientLight = RenderSettings.ambientLight;
			this.unityAmbientColorJSON = new JSONStorableColor("unityAmbientColor", HSVColorPicker.RGBToHSV(ambientLight.r, ambientLight.g, ambientLight.b), new JSONStorableColor.SetHSVColorCallback(this.SyncUnityAmbientColor));
			base.RegisterColor(this.unityAmbientColorJSON);
			this.camExposureJSON = new JSONStorableFloat("camExposure", this.skyManager.GlobalSky.CamExposure, new JSONStorableFloat.SetFloatCallback(this.SyncCamExposure), this.minCamExposure, this.maxCamExposure, true, true);
			base.RegisterFloat(this.camExposureJSON);
			this.showSkyboxJSON = new JSONStorableBool("showSkybox", this._showSkyBox, new JSONStorableBool.SetBoolCallback(this.SyncShowSkybox));
			base.RegisterBool(this.showSkyboxJSON);
			this.skyboxIntensityJSON = new JSONStorableFloat("skyboxIntensity", this.skyManager.GlobalSky.SkyIntensity, new JSONStorableFloat.SetFloatCallback(this.SyncSkyboxIntensity), this.minMasterIntensity, this.maxMasterIntensity, true, true);
			base.RegisterFloat(this.skyboxIntensityJSON);
			float v = 0f;
			if (this.skyContainer != null)
			{
				v = this.skyContainer.localEulerAngles.y;
			}
			this.skyboxYAngleJSON = new JSONStorableAngle("skyboxYAngle", v, new JSONStorableFloat.SetFloatCallback(this.SyncSkyboxYAngle));
			base.RegisterFloat(this.skyboxYAngleJSON);
		}
	}

	// Token: 0x06006634 RID: 26164 RVA: 0x00269DC2 File Offset: 0x002681C2
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
		}
	}

	// Token: 0x0400558C RID: 21900
	public static SkyshopLightController singleton;

	// Token: 0x0400558D RID: 21901
	protected string[] customParamNames = new string[]
	{
		"skyName"
	};

	// Token: 0x0400558E RID: 21902
	public SkyManager skyManager;

	// Token: 0x0400558F RID: 21903
	public Transform autoSkiesContainer;

	// Token: 0x04005590 RID: 21904
	public Sky[] skies;

	// Token: 0x04005591 RID: 21905
	public Sky customSky;

	// Token: 0x04005592 RID: 21906
	public Transform skyContainer;

	// Token: 0x04005593 RID: 21907
	public Transform skyContainer2;

	// Token: 0x04005594 RID: 21908
	public UIPopup skyPopup;

	// Token: 0x04005595 RID: 21909
	public Transform overlaySkyContainer;

	// Token: 0x04005596 RID: 21910
	public Sky overlaySky;

	// Token: 0x04005597 RID: 21911
	protected bool _overlaySkyActive;

	// Token: 0x04005598 RID: 21912
	protected Sky _startingSky;

	// Token: 0x04005599 RID: 21913
	protected Sky _currentSky;

	// Token: 0x0400559A RID: 21914
	protected string _skyName;

	// Token: 0x0400559B RID: 21915
	public float minMasterIntensity;

	// Token: 0x0400559C RID: 21916
	public float maxMasterIntensity = 10f;

	// Token: 0x0400559D RID: 21917
	public float minDiffuseIntensity;

	// Token: 0x0400559E RID: 21918
	public float maxDiffuseIntensity = 10f;

	// Token: 0x0400559F RID: 21919
	public float minSpecularIntensity;

	// Token: 0x040055A0 RID: 21920
	public float maxSpecularIntensity = 10f;

	// Token: 0x040055A1 RID: 21921
	public float minCamExposure;

	// Token: 0x040055A2 RID: 21922
	public float maxCamExposure = 10f;

	// Token: 0x040055A3 RID: 21923
	protected JSONStorableFloat masterIntensityJSON;

	// Token: 0x040055A4 RID: 21924
	protected JSONStorableFloat diffuseIntensityJSON;

	// Token: 0x040055A5 RID: 21925
	protected JSONStorableFloat specularIntensityJSON;

	// Token: 0x040055A6 RID: 21926
	protected JSONStorableFloat unityAmbientIntensityJSON;

	// Token: 0x040055A7 RID: 21927
	protected JSONStorableColor unityAmbientColorJSON;

	// Token: 0x040055A8 RID: 21928
	protected JSONStorableFloat camExposureJSON;

	// Token: 0x040055A9 RID: 21929
	[SerializeField]
	protected bool _showSkyBox;

	// Token: 0x040055AA RID: 21930
	protected JSONStorableBool showSkyboxJSON;

	// Token: 0x040055AB RID: 21931
	protected JSONStorableFloat skyboxIntensityJSON;

	// Token: 0x040055AC RID: 21932
	protected JSONStorableAngle skyboxYAngleJSON;

	// Token: 0x040055AD RID: 21933
	public int flashFrames = 45;

	// Token: 0x040055AE RID: 21934
	public float flashIntensity = 10f;

	// Token: 0x040055AF RID: 21935
	protected float flashDecerement = 1f;

	// Token: 0x040055B0 RID: 21936
	protected bool notActiveOnSync;

	// Token: 0x040055B1 RID: 21937
	public InputFieldAction urlInputFieldAction;

	// Token: 0x040055B2 RID: 21938
	public InputField urlInputField;

	// Token: 0x040055B3 RID: 21939
	public InputFieldAction urlInputFieldActionAlt;

	// Token: 0x040055B4 RID: 21940
	public InputField urlInputFieldAlt;

	// Token: 0x040055B5 RID: 21941
	[SerializeField]
	protected string _url;

	// Token: 0x040055B6 RID: 21942
	public Button loadButton;

	// Token: 0x040055B7 RID: 21943
	public Button loadButtonAlt;

	// Token: 0x040055B8 RID: 21944
	public Button copyToClipboardButton;

	// Token: 0x040055B9 RID: 21945
	public Button copyToClipboardButtonAlt;

	// Token: 0x040055BA RID: 21946
	public Button copyFromClipboardButton;

	// Token: 0x040055BB RID: 21947
	public Button copyFromClipboardButtonAlt;

	// Token: 0x040055BC RID: 21948
	public Button fileBrowseButton;

	// Token: 0x040055BD RID: 21949
	public Button fileBrowseButtonAlt;

	// Token: 0x02001033 RID: 4147
	[CompilerGenerated]
	private sealed class <FlashIter>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600775F RID: 30559 RVA: 0x00269DE1 File Offset: 0x002681E1
		[DebuggerHidden]
		public <FlashIter>c__Iterator0()
		{
		}

		// Token: 0x06007760 RID: 30560 RVA: 0x00269DEC File Offset: 0x002681EC
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				if (!(this.skyManager != null) || !(this.skyManager.GlobalSky != null) || this.camExposureJSON == null)
				{
					goto IL_165;
				}
				currentFlashIntensity = this.flashIntensity;
				flashDecerement = (currentFlashIntensity - this.camExposureJSON.val) / (float)this.flashFrames;
				i = 0;
				break;
			case 1U:
				i++;
				break;
			default:
				return false;
			}
			if (i < this.flashFrames)
			{
				this.skyManager.GlobalSky.CamExposure = currentFlashIntensity;
				this.skyManager.GlobalSky.Apply();
				currentFlashIntensity -= flashDecerement;
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			this.skyManager.GlobalSky.CamExposure = this.camExposureJSON.val;
			this.skyManager.GlobalSky.Apply();
			IL_165:
			this.$PC = -1;
			return false;
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x06007761 RID: 30561 RVA: 0x00269F68 File Offset: 0x00268368
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x06007762 RID: 30562 RVA: 0x00269F70 File Offset: 0x00268370
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007763 RID: 30563 RVA: 0x00269F78 File Offset: 0x00268378
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007764 RID: 30564 RVA: 0x00269F88 File Offset: 0x00268388
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006B64 RID: 27492
		internal float <currentFlashIntensity>__1;

		// Token: 0x04006B65 RID: 27493
		internal float <flashDecerement>__1;

		// Token: 0x04006B66 RID: 27494
		internal int <i>__2;

		// Token: 0x04006B67 RID: 27495
		internal SkyshopLightController $this;

		// Token: 0x04006B68 RID: 27496
		internal object $current;

		// Token: 0x04006B69 RID: 27497
		internal bool $disposing;

		// Token: 0x04006B6A RID: 27498
		internal int $PC;
	}

	// Token: 0x02001034 RID: 4148
	[CompilerGenerated]
	private sealed class <SyncImage>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007765 RID: 30565 RVA: 0x00269F8F File Offset: 0x0026838F
		[DebuggerHidden]
		public <SyncImage>c__Iterator1()
		{
		}

		// Token: 0x06007766 RID: 30566 RVA: 0x00269F98 File Offset: 0x00268398
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				tex = new Texture2D(4, 4, TextureFormat.DXT5, false);
				tex.wrapMode = TextureWrapMode.Clamp;
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

		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x06007767 RID: 30567 RVA: 0x0026A103 File Offset: 0x00268503
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x06007768 RID: 30568 RVA: 0x0026A10B File Offset: 0x0026850B
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007769 RID: 30569 RVA: 0x0026A113 File Offset: 0x00268513
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600776A RID: 30570 RVA: 0x0026A123 File Offset: 0x00268523
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006B6B RID: 27499
		internal Texture2D <tex>__0;

		// Token: 0x04006B6C RID: 27500
		internal string <urltoload>__0;

		// Token: 0x04006B6D RID: 27501
		internal WWW <www>__0;

		// Token: 0x04006B6E RID: 27502
		internal SkyshopLightController $this;

		// Token: 0x04006B6F RID: 27503
		internal object $current;

		// Token: 0x04006B70 RID: 27504
		internal bool $disposing;

		// Token: 0x04006B71 RID: 27505
		internal int $PC;
	}
}
