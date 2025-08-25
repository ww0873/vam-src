using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000976 RID: 2422
public class OVRSceneSampleController : MonoBehaviour
{
	// Token: 0x06003C6D RID: 15469 RVA: 0x00124B78 File Offset: 0x00122F78
	public OVRSceneSampleController()
	{
	}

	// Token: 0x06003C6E RID: 15470 RVA: 0x00124BA8 File Offset: 0x00122FA8
	private void Awake()
	{
		OVRCameraRig[] componentsInChildren = base.gameObject.GetComponentsInChildren<OVRCameraRig>();
		if (componentsInChildren.Length == 0)
		{
			Debug.LogWarning("OVRMainMenu: No OVRCameraRig attached.");
		}
		else if (componentsInChildren.Length > 1)
		{
			Debug.LogWarning("OVRMainMenu: More then 1 OVRCameraRig attached.");
		}
		else
		{
			this.cameraController = componentsInChildren[0];
		}
		OVRPlayerController[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<OVRPlayerController>();
		if (componentsInChildren2.Length == 0)
		{
			Debug.LogWarning("OVRMainMenu: No OVRPlayerController attached.");
		}
		else if (componentsInChildren2.Length > 1)
		{
			Debug.LogWarning("OVRMainMenu: More then 1 OVRPlayerController attached.");
		}
		else
		{
			this.playerController = componentsInChildren2[0];
		}
	}

	// Token: 0x06003C6F RID: 15471 RVA: 0x00124C40 File Offset: 0x00123040
	private void Start()
	{
		if (!Application.isEditor)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
		if (this.cameraController != null)
		{
			this.gridCube = base.gameObject.AddComponent<OVRGridCube>();
			this.gridCube.SetOVRCameraController(ref this.cameraController);
		}
	}

	// Token: 0x06003C70 RID: 15472 RVA: 0x00124C98 File Offset: 0x00123098
	private void Update()
	{
		this.UpdateRecenterPose();
		this.UpdateVisionMode();
		if (this.playerController != null)
		{
			this.UpdateSpeedAndRotationScaleMultiplier();
		}
		if (Input.GetKeyDown(KeyCode.F11))
		{
			Screen.fullScreen = !Screen.fullScreen;
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			XRSettings.showDeviceView = !XRSettings.showDeviceView;
		}
		if (Input.GetKeyDown(this.quitKey))
		{
			Application.Quit();
		}
	}

	// Token: 0x06003C71 RID: 15473 RVA: 0x00124D12 File Offset: 0x00123112
	private void UpdateVisionMode()
	{
		if (Input.GetKeyDown(KeyCode.F2))
		{
			this.visionMode ^= this.visionMode;
			OVRManager.tracker.isEnabled = this.visionMode;
		}
	}

	// Token: 0x06003C72 RID: 15474 RVA: 0x00124D48 File Offset: 0x00123148
	private void UpdateSpeedAndRotationScaleMultiplier()
	{
		float num = 0f;
		this.playerController.GetMoveScaleMultiplier(ref num);
		if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			num -= this.speedRotationIncrement;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			num += this.speedRotationIncrement;
		}
		this.playerController.SetMoveScaleMultiplier(num);
		float num2 = 0f;
		this.playerController.GetRotationScaleMultiplier(ref num2);
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			num2 -= this.speedRotationIncrement;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			num2 += this.speedRotationIncrement;
		}
		this.playerController.SetRotationScaleMultiplier(num2);
	}

	// Token: 0x06003C73 RID: 15475 RVA: 0x00124DF1 File Offset: 0x001231F1
	private void UpdateRecenterPose()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			OVRManager.display.RecenterPose();
		}
	}

	// Token: 0x04002E51 RID: 11857
	public KeyCode quitKey = KeyCode.Escape;

	// Token: 0x04002E52 RID: 11858
	public Texture fadeInTexture;

	// Token: 0x04002E53 RID: 11859
	public float speedRotationIncrement = 0.05f;

	// Token: 0x04002E54 RID: 11860
	private OVRPlayerController playerController;

	// Token: 0x04002E55 RID: 11861
	private OVRCameraRig cameraController;

	// Token: 0x04002E56 RID: 11862
	public string layerName = "Default";

	// Token: 0x04002E57 RID: 11863
	private bool visionMode = true;

	// Token: 0x04002E58 RID: 11864
	private OVRGridCube gridCube;
}
