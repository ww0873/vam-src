using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

// Token: 0x0200095D RID: 2397
public class OVRDebugInfo : MonoBehaviour
{
	// Token: 0x06003B71 RID: 15217 RVA: 0x0011EBC8 File Offset: 0x0011CFC8
	public OVRDebugInfo()
	{
	}

	// Token: 0x06003B72 RID: 15218 RVA: 0x0011EBE8 File Offset: 0x0011CFE8
	private void Awake()
	{
		this.debugUIManager = new GameObject();
		this.debugUIManager.name = "DebugUIManager";
		this.debugUIManager.transform.parent = GameObject.Find("LeftEyeAnchor").transform;
		RectTransform rectTransform = this.debugUIManager.AddComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(100f, 100f);
		rectTransform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
		rectTransform.localPosition = new Vector3(0.01f, 0.17f, 0.53f);
		rectTransform.localEulerAngles = Vector3.zero;
		Canvas canvas = this.debugUIManager.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.WorldSpace;
		canvas.pixelPerfect = false;
	}

	// Token: 0x06003B73 RID: 15219 RVA: 0x0011ECAC File Offset: 0x0011D0AC
	private void Update()
	{
		if (this.initUIComponent && !this.isInited)
		{
			this.InitUIComponents();
		}
		if (Input.GetKeyDown(KeyCode.Space) && this.riftPresentTimeout < 0f)
		{
			this.initUIComponent = true;
			this.showVRVars ^= true;
		}
		this.UpdateDeviceDetection();
		if (this.showVRVars)
		{
			this.debugUIManager.SetActive(true);
			this.UpdateVariable();
			this.UpdateStrings();
		}
		else
		{
			this.debugUIManager.SetActive(false);
		}
	}

	// Token: 0x06003B74 RID: 15220 RVA: 0x0011ED40 File Offset: 0x0011D140
	private void OnDestroy()
	{
		this.isInited = false;
	}

	// Token: 0x06003B75 RID: 15221 RVA: 0x0011ED4C File Offset: 0x0011D14C
	private void InitUIComponents()
	{
		float num = 0f;
		int fontSize = 20;
		this.debugUIObject = new GameObject();
		this.debugUIObject.name = "DebugInfo";
		this.debugUIObject.transform.parent = GameObject.Find("DebugUIManager").transform;
		this.debugUIObject.transform.localPosition = new Vector3(0f, 100f, 0f);
		this.debugUIObject.transform.localEulerAngles = Vector3.zero;
		this.debugUIObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (!string.IsNullOrEmpty(this.strFPS))
		{
			this.fps = this.VariableObjectManager(this.fps, "FPS", num -= this.offsetY, this.strFPS, fontSize);
		}
		if (!string.IsNullOrEmpty(this.strIPD))
		{
			this.ipd = this.VariableObjectManager(this.ipd, "IPD", num -= this.offsetY, this.strIPD, fontSize);
		}
		if (!string.IsNullOrEmpty(this.strFOV))
		{
			this.fov = this.VariableObjectManager(this.fov, "FOV", num -= this.offsetY, this.strFOV, fontSize);
		}
		if (!string.IsNullOrEmpty(this.strHeight))
		{
			this.height = this.VariableObjectManager(this.height, "Height", num -= this.offsetY, this.strHeight, fontSize);
		}
		if (!string.IsNullOrEmpty(this.strDepth))
		{
			this.depth = this.VariableObjectManager(this.depth, "Depth", num -= this.offsetY, this.strDepth, fontSize);
		}
		if (!string.IsNullOrEmpty(this.strResolutionEyeTexture))
		{
			this.resolutionEyeTexture = this.VariableObjectManager(this.resolutionEyeTexture, "Resolution", num -= this.offsetY, this.strResolutionEyeTexture, fontSize);
		}
		if (!string.IsNullOrEmpty(this.strLatencies))
		{
			this.latencies = this.VariableObjectManager(this.latencies, "Latency", num - this.offsetY, this.strLatencies, 17);
		}
		this.initUIComponent = false;
		this.isInited = true;
	}

	// Token: 0x06003B76 RID: 15222 RVA: 0x0011EF96 File Offset: 0x0011D396
	private void UpdateVariable()
	{
		this.UpdateIPD();
		this.UpdateEyeHeightOffset();
		this.UpdateEyeDepthOffset();
		this.UpdateFOV();
		this.UpdateResolutionEyeTexture();
		this.UpdateLatencyValues();
		this.UpdateFPS();
	}

	// Token: 0x06003B77 RID: 15223 RVA: 0x0011EFC4 File Offset: 0x0011D3C4
	private void UpdateStrings()
	{
		if (this.debugUIObject == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.strFPS))
		{
			this.fps.GetComponentInChildren<Text>().text = this.strFPS;
		}
		if (!string.IsNullOrEmpty(this.strIPD))
		{
			this.ipd.GetComponentInChildren<Text>().text = this.strIPD;
		}
		if (!string.IsNullOrEmpty(this.strFOV))
		{
			this.fov.GetComponentInChildren<Text>().text = this.strFOV;
		}
		if (!string.IsNullOrEmpty(this.strResolutionEyeTexture))
		{
			this.resolutionEyeTexture.GetComponentInChildren<Text>().text = this.strResolutionEyeTexture;
		}
		if (!string.IsNullOrEmpty(this.strLatencies))
		{
			this.latencies.GetComponentInChildren<Text>().text = this.strLatencies;
			this.latencies.GetComponentInChildren<Text>().fontSize = 14;
		}
		if (!string.IsNullOrEmpty(this.strHeight))
		{
			this.height.GetComponentInChildren<Text>().text = this.strHeight;
		}
		if (!string.IsNullOrEmpty(this.strDepth))
		{
			this.depth.GetComponentInChildren<Text>().text = this.strDepth;
		}
	}

	// Token: 0x06003B78 RID: 15224 RVA: 0x0011F100 File Offset: 0x0011D500
	private void RiftPresentGUI(GameObject guiMainOBj)
	{
		this.riftPresent = this.ComponentComposition(this.riftPresent);
		this.riftPresent.transform.SetParent(guiMainOBj.transform);
		this.riftPresent.name = "RiftPresent";
		RectTransform component = this.riftPresent.GetComponent<RectTransform>();
		component.localPosition = new Vector3(0f, 0f, 0f);
		component.localScale = new Vector3(1f, 1f, 1f);
		component.localEulerAngles = Vector3.zero;
		Text componentInChildren = this.riftPresent.GetComponentInChildren<Text>();
		componentInChildren.text = this.strRiftPresent;
		componentInChildren.fontSize = 20;
	}

	// Token: 0x06003B79 RID: 15225 RVA: 0x0011F1B0 File Offset: 0x0011D5B0
	private void UpdateDeviceDetection()
	{
		if (this.riftPresentTimeout >= 0f)
		{
			this.riftPresentTimeout -= Time.deltaTime;
		}
	}

	// Token: 0x06003B7A RID: 15226 RVA: 0x0011F1D4 File Offset: 0x0011D5D4
	private GameObject VariableObjectManager(GameObject gameObject, string name, float posY, string str, int fontSize)
	{
		gameObject = this.ComponentComposition(gameObject);
		gameObject.name = name;
		gameObject.transform.SetParent(this.debugUIObject.transform);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.localPosition = new Vector3(0f, posY -= this.offsetY, 0f);
		Text componentInChildren = gameObject.GetComponentInChildren<Text>();
		componentInChildren.text = str;
		componentInChildren.fontSize = fontSize;
		gameObject.transform.localEulerAngles = Vector3.zero;
		component.localScale = new Vector3(1f, 1f, 1f);
		return gameObject;
	}

	// Token: 0x06003B7B RID: 15227 RVA: 0x0011F270 File Offset: 0x0011D670
	private GameObject ComponentComposition(GameObject GO)
	{
		GO = new GameObject();
		GO.AddComponent<RectTransform>();
		GO.AddComponent<CanvasRenderer>();
		GO.AddComponent<Image>();
		GO.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 50f);
		GO.GetComponent<Image>().color = new Color(0.02745098f, 0.1764706f, 0.2784314f, 0.78431374f);
		this.texts = new GameObject();
		this.texts.AddComponent<RectTransform>();
		this.texts.AddComponent<CanvasRenderer>();
		this.texts.AddComponent<Text>();
		this.texts.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 50f);
		this.texts.GetComponent<Text>().font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
		this.texts.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
		this.texts.transform.SetParent(GO.transform);
		this.texts.name = "TextBox";
		return GO;
	}

	// Token: 0x06003B7C RID: 15228 RVA: 0x0011F386 File Offset: 0x0011D786
	private void UpdateIPD()
	{
		this.strIPD = string.Format("IPD (mm): {0:F4}", OVRManager.profile.ipd * 1000f);
	}

	// Token: 0x06003B7D RID: 15229 RVA: 0x0011F3B0 File Offset: 0x0011D7B0
	private void UpdateEyeHeightOffset()
	{
		float eyeHeight = OVRManager.profile.eyeHeight;
		this.strHeight = string.Format("Eye Height (m): {0:F3}", eyeHeight);
	}

	// Token: 0x06003B7E RID: 15230 RVA: 0x0011F3E0 File Offset: 0x0011D7E0
	private void UpdateEyeDepthOffset()
	{
		float eyeDepth = OVRManager.profile.eyeDepth;
		this.strDepth = string.Format("Eye Depth (m): {0:F3}", eyeDepth);
	}

	// Token: 0x06003B7F RID: 15231 RVA: 0x0011F410 File Offset: 0x0011D810
	private void UpdateFOV()
	{
		this.strFOV = string.Format("FOV (deg): {0:F3}", OVRManager.display.GetEyeRenderDesc(XRNode.LeftEye).fov.y);
	}

	// Token: 0x06003B80 RID: 15232 RVA: 0x0011F44C File Offset: 0x0011D84C
	private void UpdateResolutionEyeTexture()
	{
		OVRDisplay.EyeRenderDesc eyeRenderDesc = OVRManager.display.GetEyeRenderDesc(XRNode.LeftEye);
		OVRDisplay.EyeRenderDesc eyeRenderDesc2 = OVRManager.display.GetEyeRenderDesc(XRNode.RightEye);
		float renderViewportScale = XRSettings.renderViewportScale;
		float num = (float)((int)(renderViewportScale * (eyeRenderDesc.resolution.x + eyeRenderDesc2.resolution.x)));
		float num2 = (float)((int)(renderViewportScale * Mathf.Max(eyeRenderDesc.resolution.y, eyeRenderDesc2.resolution.y)));
		this.strResolutionEyeTexture = string.Format("Resolution : {0} x {1}", num, num2);
	}

	// Token: 0x06003B81 RID: 15233 RVA: 0x0011F4D8 File Offset: 0x0011D8D8
	private void UpdateLatencyValues()
	{
		OVRDisplay.LatencyData latency = OVRManager.display.latency;
		if (latency.render < 1E-06f && latency.timeWarp < 1E-06f && latency.postPresent < 1E-06f)
		{
			this.strLatencies = string.Format("Latency values are not available.", new object[0]);
		}
		else
		{
			this.strLatencies = string.Format("Render: {0:F3} TimeWarp: {1:F3} Post-Present: {2:F3}\nRender Error: {3:F3} TimeWarp Error: {4:F3}", new object[]
			{
				latency.render,
				latency.timeWarp,
				latency.postPresent,
				latency.renderError,
				latency.timeWarpError
			});
		}
	}

	// Token: 0x06003B82 RID: 15234 RVA: 0x0011F5A0 File Offset: 0x0011D9A0
	private void UpdateFPS()
	{
		this.timeLeft -= Time.unscaledDeltaTime;
		this.accum += Time.unscaledDeltaTime;
		this.frames++;
		if ((double)this.timeLeft <= 0.0)
		{
			float num = (float)this.frames / this.accum;
			this.strFPS = string.Format("FPS: {0:F2}", num);
			this.timeLeft += this.updateInterval;
			this.accum = 0f;
			this.frames = 0;
		}
	}

	// Token: 0x04002D57 RID: 11607
	private GameObject debugUIManager;

	// Token: 0x04002D58 RID: 11608
	private GameObject debugUIObject;

	// Token: 0x04002D59 RID: 11609
	private GameObject riftPresent;

	// Token: 0x04002D5A RID: 11610
	private GameObject fps;

	// Token: 0x04002D5B RID: 11611
	private GameObject ipd;

	// Token: 0x04002D5C RID: 11612
	private GameObject fov;

	// Token: 0x04002D5D RID: 11613
	private GameObject height;

	// Token: 0x04002D5E RID: 11614
	private GameObject depth;

	// Token: 0x04002D5F RID: 11615
	private GameObject resolutionEyeTexture;

	// Token: 0x04002D60 RID: 11616
	private GameObject latencies;

	// Token: 0x04002D61 RID: 11617
	private GameObject texts;

	// Token: 0x04002D62 RID: 11618
	private string strRiftPresent;

	// Token: 0x04002D63 RID: 11619
	private string strFPS;

	// Token: 0x04002D64 RID: 11620
	private string strIPD;

	// Token: 0x04002D65 RID: 11621
	private string strFOV;

	// Token: 0x04002D66 RID: 11622
	private string strHeight;

	// Token: 0x04002D67 RID: 11623
	private string strDepth;

	// Token: 0x04002D68 RID: 11624
	private string strResolutionEyeTexture;

	// Token: 0x04002D69 RID: 11625
	private string strLatencies;

	// Token: 0x04002D6A RID: 11626
	private float updateInterval = 0.5f;

	// Token: 0x04002D6B RID: 11627
	private float accum;

	// Token: 0x04002D6C RID: 11628
	private int frames;

	// Token: 0x04002D6D RID: 11629
	private float timeLeft;

	// Token: 0x04002D6E RID: 11630
	private bool initUIComponent;

	// Token: 0x04002D6F RID: 11631
	private bool isInited;

	// Token: 0x04002D70 RID: 11632
	private float offsetY = 55f;

	// Token: 0x04002D71 RID: 11633
	private float riftPresentTimeout;

	// Token: 0x04002D72 RID: 11634
	private bool showVRVars;
}
