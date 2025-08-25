using System;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;

// Token: 0x02000C28 RID: 3112
public class LeapMotionGrabber : MonoBehaviour
{
	// Token: 0x06005A7D RID: 23165 RVA: 0x0021363B File Offset: 0x00211A3B
	public LeapMotionGrabber()
	{
	}

	// Token: 0x17000D57 RID: 3415
	// (get) Token: 0x06005A7E RID: 23166 RVA: 0x0021364A File Offset: 0x00211A4A
	// (set) Token: 0x06005A7F RID: 23167 RVA: 0x00213652 File Offset: 0x00211A52
	public bool controllerGrabOn
	{
		get
		{
			return this._controllerGrabOn;
		}
		set
		{
			if (this._controllerGrabOn != value)
			{
				this._controllerGrabOn = value;
				if (!this._controllerGrabOn)
				{
					this.ReleaseController();
				}
			}
		}
	}

	// Token: 0x06005A80 RID: 23168 RVA: 0x00213678 File Offset: 0x00211A78
	public void ReleaseController()
	{
		if (this.grabbedController != null)
		{
			if (this.grabbedController.linkToRB == this.rb)
			{
				this.grabbedController.RestorePreLinkState();
			}
			this.grabbedController = null;
		}
	}

	// Token: 0x06005A81 RID: 23169 RVA: 0x002136B8 File Offset: 0x00211AB8
	private void Update()
	{
		if (this.rb != null && this.pinchDetector != null)
		{
			if (this.pinchDetector.DidStartPinch)
			{
				if (this.grabSphere != null)
				{
					this.grabSphere.enabled = true;
				}
				if (this.grabbedController == null && this._controllerGrabOn)
				{
					List<FreeControllerV3> overlappingTargets = SuperController.singleton.GetOverlappingTargets(base.transform, 0.02f);
					if (overlappingTargets.Count > 0)
					{
						foreach (FreeControllerV3 freeControllerV in overlappingTargets)
						{
							bool flag = true;
							FreeControllerV3.SelectLinkState linkState = FreeControllerV3.SelectLinkState.Position;
							if (freeControllerV.canGrabPosition)
							{
								if (freeControllerV.canGrabRotation)
								{
									linkState = FreeControllerV3.SelectLinkState.PositionAndRotation;
								}
							}
							else if (freeControllerV.canGrabRotation)
							{
								linkState = FreeControllerV3.SelectLinkState.Rotation;
							}
							else
							{
								flag = false;
							}
							if (flag)
							{
								if (freeControllerV.linkToRB != null)
								{
									LeapMotionGrabber component = freeControllerV.linkToRB.GetComponent<LeapMotionGrabber>();
									if (component != null)
									{
										component.ReleaseController();
									}
								}
								this.grabbedController = freeControllerV;
								this.grabbedController.SelectLinkToRigidbody(this.rb, linkState, false, true);
								break;
							}
						}
					}
				}
			}
			if (this.pinchDetector.DidEndPinch)
			{
				if (this.grabSphere != null)
				{
					this.grabSphere.enabled = false;
				}
				this.ReleaseController();
			}
		}
	}

	// Token: 0x06005A82 RID: 23170 RVA: 0x0021385C File Offset: 0x00211C5C
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.grabSphere = base.GetComponent<GpuGrabSphere>();
	}

	// Token: 0x06005A83 RID: 23171 RVA: 0x00213876 File Offset: 0x00211C76
	private void OnDisable()
	{
		this.ReleaseController();
	}

	// Token: 0x04004ABE RID: 19134
	public PinchDetector pinchDetector;

	// Token: 0x04004ABF RID: 19135
	private FreeControllerV3 grabbedController;

	// Token: 0x04004AC0 RID: 19136
	private Rigidbody rb;

	// Token: 0x04004AC1 RID: 19137
	private GpuGrabSphere grabSphere;

	// Token: 0x04004AC2 RID: 19138
	private bool _controllerGrabOn = true;
}
