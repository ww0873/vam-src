using System;
using Oculus.Platform;
using UnityEngine;

// Token: 0x02000777 RID: 1911
public class PlayerController : PlatformManager
{
	// Token: 0x06003156 RID: 12630 RVA: 0x00100DDC File Offset: 0x000FF1DC
	public PlayerController()
	{
	}

	// Token: 0x06003157 RID: 12631 RVA: 0x00100DEB File Offset: 0x000FF1EB
	public override void Awake()
	{
		base.Awake();
		this.cameraRig = this.localPlayerHead.gameObject;
	}

	// Token: 0x06003158 RID: 12632 RVA: 0x00100E04 File Offset: 0x000FF204
	public override void Start()
	{
		OVRManager.instance.trackingOriginType = OVRManager.TrackingOrigin.EyeLevel;
		base.Start();
		this.spyCamera.enabled = false;
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x00100E23 File Offset: 0x000FF223
	public override void Update()
	{
		base.Update();
		this.checkInput();
	}

	// Token: 0x0600315A RID: 12634 RVA: 0x00100E34 File Offset: 0x000FF234
	private void checkInput()
	{
		if (UnityEngine.Application.platform == RuntimePlatform.Android)
		{
			if (OVRInput.GetDown(OVRInput.Button.Back, OVRInput.Controller.Active))
			{
				Rooms.LaunchInvitableUserFlow(this.roomManager.roomID);
			}
			if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad, OVRInput.Controller.Active))
			{
				this.ToggleCamera();
			}
			if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Active))
			{
				this.ToggleUI();
			}
		}
		else
		{
			if (OVRInput.GetDown(OVRInput.Button.Three, OVRInput.Controller.Active))
			{
				Rooms.LaunchInvitableUserFlow(this.roomManager.roomID);
			}
			if (OVRInput.GetDown(OVRInput.Button.Four, OVRInput.Controller.Active))
			{
				this.ToggleCamera();
			}
			if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.Active))
			{
				this.ToggleUI();
			}
		}
	}

	// Token: 0x0600315B RID: 12635 RVA: 0x00100EFC File Offset: 0x000FF2FC
	private void ToggleCamera()
	{
		this.spyCamera.enabled = !this.spyCamera.enabled;
		this.localAvatar.ShowThirdPerson = !this.localAvatar.ShowThirdPerson;
		this.cameraRig.SetActive(!this.cameraRig.activeSelf);
	}

	// Token: 0x0600315C RID: 12636 RVA: 0x00100F54 File Offset: 0x000FF354
	private void ToggleUI()
	{
		this.showUI = !this.showUI;
		this.helpPanel.SetActive(this.showUI);
		this.localAvatar.ShowLeftController(this.showUI);
	}

	// Token: 0x04002523 RID: 9507
	public Camera spyCamera;

	// Token: 0x04002524 RID: 9508
	private GameObject cameraRig;

	// Token: 0x04002525 RID: 9509
	private bool showUI = true;
}
