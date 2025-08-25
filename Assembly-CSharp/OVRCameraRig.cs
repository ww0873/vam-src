using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020008CB RID: 2251
[ExecuteInEditMode]
public class OVRCameraRig : MonoBehaviour
{
	// Token: 0x06003882 RID: 14466 RVA: 0x00113734 File Offset: 0x00111B34
	public OVRCameraRig()
	{
	}

	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x06003883 RID: 14467 RVA: 0x00113794 File Offset: 0x00111B94
	public Camera leftEyeCamera
	{
		get
		{
			return (!this.usePerEyeCameras) ? this._centerEyeCamera : this._leftEyeCamera;
		}
	}

	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x06003884 RID: 14468 RVA: 0x001137B2 File Offset: 0x00111BB2
	public Camera rightEyeCamera
	{
		get
		{
			return (!this.usePerEyeCameras) ? this._centerEyeCamera : this._rightEyeCamera;
		}
	}

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x06003885 RID: 14469 RVA: 0x001137D0 File Offset: 0x00111BD0
	// (set) Token: 0x06003886 RID: 14470 RVA: 0x001137D8 File Offset: 0x00111BD8
	public Transform trackingSpace
	{
		[CompilerGenerated]
		get
		{
			return this.<trackingSpace>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<trackingSpace>k__BackingField = value;
		}
	}

	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x06003887 RID: 14471 RVA: 0x001137E1 File Offset: 0x00111BE1
	// (set) Token: 0x06003888 RID: 14472 RVA: 0x001137E9 File Offset: 0x00111BE9
	public Transform leftEyeAnchor
	{
		[CompilerGenerated]
		get
		{
			return this.<leftEyeAnchor>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<leftEyeAnchor>k__BackingField = value;
		}
	}

	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x06003889 RID: 14473 RVA: 0x001137F2 File Offset: 0x00111BF2
	// (set) Token: 0x0600388A RID: 14474 RVA: 0x001137FA File Offset: 0x00111BFA
	public Transform centerEyeAnchor
	{
		[CompilerGenerated]
		get
		{
			return this.<centerEyeAnchor>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<centerEyeAnchor>k__BackingField = value;
		}
	}

	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x0600388B RID: 14475 RVA: 0x00113803 File Offset: 0x00111C03
	// (set) Token: 0x0600388C RID: 14476 RVA: 0x0011380B File Offset: 0x00111C0B
	public Transform rightEyeAnchor
	{
		[CompilerGenerated]
		get
		{
			return this.<rightEyeAnchor>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<rightEyeAnchor>k__BackingField = value;
		}
	}

	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x0600388D RID: 14477 RVA: 0x00113814 File Offset: 0x00111C14
	// (set) Token: 0x0600388E RID: 14478 RVA: 0x0011381C File Offset: 0x00111C1C
	public Transform leftHandAnchor
	{
		[CompilerGenerated]
		get
		{
			return this.<leftHandAnchor>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<leftHandAnchor>k__BackingField = value;
		}
	}

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x0600388F RID: 14479 RVA: 0x00113825 File Offset: 0x00111C25
	// (set) Token: 0x06003890 RID: 14480 RVA: 0x0011382D File Offset: 0x00111C2D
	public Transform rightHandAnchor
	{
		[CompilerGenerated]
		get
		{
			return this.<rightHandAnchor>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<rightHandAnchor>k__BackingField = value;
		}
	}

	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x06003891 RID: 14481 RVA: 0x00113836 File Offset: 0x00111C36
	// (set) Token: 0x06003892 RID: 14482 RVA: 0x0011383E File Offset: 0x00111C3E
	public Transform trackerAnchor
	{
		[CompilerGenerated]
		get
		{
			return this.<trackerAnchor>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<trackerAnchor>k__BackingField = value;
		}
	}

	// Token: 0x140000BA RID: 186
	// (add) Token: 0x06003893 RID: 14483 RVA: 0x00113848 File Offset: 0x00111C48
	// (remove) Token: 0x06003894 RID: 14484 RVA: 0x00113880 File Offset: 0x00111C80
	public event Action<OVRCameraRig> UpdatedAnchors
	{
		add
		{
			Action<OVRCameraRig> action = this.UpdatedAnchors;
			Action<OVRCameraRig> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<OVRCameraRig>>(ref this.UpdatedAnchors, (Action<OVRCameraRig>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<OVRCameraRig> action = this.UpdatedAnchors;
			Action<OVRCameraRig> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<OVRCameraRig>>(ref this.UpdatedAnchors, (Action<OVRCameraRig>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x06003895 RID: 14485 RVA: 0x001138B6 File Offset: 0x00111CB6
	protected virtual void Awake()
	{
		this._skipUpdate = true;
		this.EnsureGameObjectIntegrity();
	}

	// Token: 0x06003896 RID: 14486 RVA: 0x001138C5 File Offset: 0x00111CC5
	protected virtual void Start()
	{
		this.UpdateAnchors();
	}

	// Token: 0x06003897 RID: 14487 RVA: 0x001138CD File Offset: 0x00111CCD
	protected virtual void FixedUpdate()
	{
		if (this.useFixedUpdateForTracking)
		{
			this.UpdateAnchors();
		}
	}

	// Token: 0x06003898 RID: 14488 RVA: 0x001138E0 File Offset: 0x00111CE0
	protected virtual void Update()
	{
		this._skipUpdate = false;
		if (!this.useFixedUpdateForTracking)
		{
			this.UpdateAnchors();
		}
	}

	// Token: 0x06003899 RID: 14489 RVA: 0x001138FC File Offset: 0x00111CFC
	protected virtual void UpdateAnchors()
	{
		this.EnsureGameObjectIntegrity();
		if (!Application.isPlaying)
		{
			return;
		}
		if (this._skipUpdate)
		{
			this.centerEyeAnchor.FromOVRPose(OVRPose.identity, true);
			this.leftEyeAnchor.FromOVRPose(OVRPose.identity, true);
			this.rightEyeAnchor.FromOVRPose(OVRPose.identity, true);
			return;
		}
		bool monoscopic = OVRManager.instance.monoscopic;
		OVRPose pose = OVRManager.tracker.GetPose(0);
		this.trackerAnchor.localRotation = pose.orientation;
		this.centerEyeAnchor.localRotation = InputTracking.GetLocalRotation(XRNode.CenterEye);
		this.leftEyeAnchor.localRotation = ((!monoscopic) ? InputTracking.GetLocalRotation(XRNode.LeftEye) : this.centerEyeAnchor.localRotation);
		this.rightEyeAnchor.localRotation = ((!monoscopic) ? InputTracking.GetLocalRotation(XRNode.RightEye) : this.centerEyeAnchor.localRotation);
		this.leftHandAnchor.gameObject.SetActive(OVRInput.IsControllerConnected(OVRInput.Controller.LTouch));
		this.rightHandAnchor.gameObject.SetActive(OVRInput.IsControllerConnected(OVRInput.Controller.RTouch));
		this.leftHandAnchor.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
		this.rightHandAnchor.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
		this.trackerAnchor.localPosition = pose.position;
		this.centerEyeAnchor.localPosition = InputTracking.GetLocalPosition(XRNode.CenterEye);
		this.leftEyeAnchor.localPosition = ((!monoscopic) ? InputTracking.GetLocalPosition(XRNode.LeftEye) : this.centerEyeAnchor.localPosition);
		this.rightEyeAnchor.localPosition = ((!monoscopic) ? InputTracking.GetLocalPosition(XRNode.RightEye) : this.centerEyeAnchor.localPosition);
		this.leftHandAnchor.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
		this.rightHandAnchor.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
		this.RaiseUpdatedAnchorsEvent();
	}

	// Token: 0x0600389A RID: 14490 RVA: 0x00113AC8 File Offset: 0x00111EC8
	protected virtual void RaiseUpdatedAnchorsEvent()
	{
		if (this.UpdatedAnchors != null)
		{
			this.UpdatedAnchors(this);
		}
	}

	// Token: 0x0600389B RID: 14491 RVA: 0x00113AE4 File Offset: 0x00111EE4
	public virtual void EnsureGameObjectIntegrity()
	{
		if (this.trackingSpace == null)
		{
			this.trackingSpace = this.ConfigureAnchor(null, this.trackingSpaceName);
		}
		if (this.leftEyeAnchor == null)
		{
			this.leftEyeAnchor = this.ConfigureAnchor(this.trackingSpace, this.leftEyeAnchorName);
		}
		if (this.centerEyeAnchor == null)
		{
			this.centerEyeAnchor = this.ConfigureAnchor(this.trackingSpace, this.centerEyeAnchorName);
		}
		if (this.rightEyeAnchor == null)
		{
			this.rightEyeAnchor = this.ConfigureAnchor(this.trackingSpace, this.rightEyeAnchorName);
		}
		if (this.leftHandAnchor == null)
		{
			this.leftHandAnchor = this.ConfigureAnchor(this.trackingSpace, this.leftHandAnchorName);
		}
		if (this.rightHandAnchor == null)
		{
			this.rightHandAnchor = this.ConfigureAnchor(this.trackingSpace, this.rightHandAnchorName);
		}
		if (this.trackerAnchor == null)
		{
			this.trackerAnchor = this.ConfigureAnchor(this.trackingSpace, this.trackerAnchorName);
		}
		if (this._centerEyeCamera == null || this._leftEyeCamera == null || this._rightEyeCamera == null)
		{
			this._centerEyeCamera = this.centerEyeAnchor.GetComponent<Camera>();
			this._leftEyeCamera = this.leftEyeAnchor.GetComponent<Camera>();
			this._rightEyeCamera = this.rightEyeAnchor.GetComponent<Camera>();
			if (this._centerEyeCamera == null)
			{
				this._centerEyeCamera = this.centerEyeAnchor.gameObject.AddComponent<Camera>();
				this._centerEyeCamera.tag = "MainCamera";
			}
			if (this._leftEyeCamera == null)
			{
				this._leftEyeCamera = this.leftEyeAnchor.gameObject.AddComponent<Camera>();
				this._leftEyeCamera.tag = "MainCamera";
			}
			if (this._rightEyeCamera == null)
			{
				this._rightEyeCamera = this.rightEyeAnchor.gameObject.AddComponent<Camera>();
				this._rightEyeCamera.tag = "MainCamera";
			}
			this._centerEyeCamera.stereoTargetEye = StereoTargetEyeMask.Both;
			this._leftEyeCamera.stereoTargetEye = StereoTargetEyeMask.Left;
			this._rightEyeCamera.stereoTargetEye = StereoTargetEyeMask.Right;
		}
		if (this._centerEyeCamera.enabled == this.usePerEyeCameras || this._leftEyeCamera.enabled == !this.usePerEyeCameras || this._rightEyeCamera.enabled == !this.usePerEyeCameras)
		{
			this._skipUpdate = true;
		}
		this._centerEyeCamera.enabled = !this.usePerEyeCameras;
		this._leftEyeCamera.enabled = this.usePerEyeCameras;
		this._rightEyeCamera.enabled = this.usePerEyeCameras;
	}

	// Token: 0x0600389C RID: 14492 RVA: 0x00113DC0 File Offset: 0x001121C0
	protected virtual Transform ConfigureAnchor(Transform root, string name)
	{
		Transform transform = (!(root != null)) ? null : base.transform.Find(root.name + "/" + name);
		if (transform == null)
		{
			transform = base.transform.Find(name);
		}
		if (transform == null)
		{
			transform = new GameObject(name).transform;
		}
		transform.name = name;
		transform.parent = ((!(root != null)) ? base.transform : root);
		transform.localScale = Vector3.one;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		return transform;
	}

	// Token: 0x0600389D RID: 14493 RVA: 0x00113E74 File Offset: 0x00112274
	public virtual Matrix4x4 ComputeTrackReferenceMatrix()
	{
		if (this.centerEyeAnchor == null)
		{
			UnityEngine.Debug.LogError("centerEyeAnchor is required");
			return Matrix4x4.identity;
		}
		OVRPose ovrpose;
		ovrpose.position = InputTracking.GetLocalPosition(XRNode.Head);
		ovrpose.orientation = InputTracking.GetLocalRotation(XRNode.Head);
		OVRPose ovrpose2 = ovrpose.Inverse();
		Matrix4x4 rhs = Matrix4x4.TRS(ovrpose2.position, ovrpose2.orientation, Vector3.one);
		return this.centerEyeAnchor.localToWorldMatrix * rhs;
	}

	// Token: 0x040029CD RID: 10701
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Transform <trackingSpace>k__BackingField;

	// Token: 0x040029CE RID: 10702
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Transform <leftEyeAnchor>k__BackingField;

	// Token: 0x040029CF RID: 10703
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Transform <centerEyeAnchor>k__BackingField;

	// Token: 0x040029D0 RID: 10704
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Transform <rightEyeAnchor>k__BackingField;

	// Token: 0x040029D1 RID: 10705
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Transform <leftHandAnchor>k__BackingField;

	// Token: 0x040029D2 RID: 10706
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Transform <rightHandAnchor>k__BackingField;

	// Token: 0x040029D3 RID: 10707
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Transform <trackerAnchor>k__BackingField;

	// Token: 0x040029D4 RID: 10708
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<OVRCameraRig> UpdatedAnchors;

	// Token: 0x040029D5 RID: 10709
	public bool usePerEyeCameras;

	// Token: 0x040029D6 RID: 10710
	public bool useFixedUpdateForTracking;

	// Token: 0x040029D7 RID: 10711
	protected bool _skipUpdate;

	// Token: 0x040029D8 RID: 10712
	protected readonly string trackingSpaceName = "TrackingSpace";

	// Token: 0x040029D9 RID: 10713
	protected readonly string trackerAnchorName = "TrackerAnchor";

	// Token: 0x040029DA RID: 10714
	protected readonly string leftEyeAnchorName = "LeftEyeAnchor";

	// Token: 0x040029DB RID: 10715
	protected readonly string centerEyeAnchorName = "CenterEyeAnchor";

	// Token: 0x040029DC RID: 10716
	protected readonly string rightEyeAnchorName = "RightEyeAnchor";

	// Token: 0x040029DD RID: 10717
	protected readonly string leftHandAnchorName = "LeftHandAnchor";

	// Token: 0x040029DE RID: 10718
	protected readonly string rightHandAnchorName = "RightHandAnchor";

	// Token: 0x040029DF RID: 10719
	protected Camera _centerEyeCamera;

	// Token: 0x040029E0 RID: 10720
	protected Camera _leftEyeCamera;

	// Token: 0x040029E1 RID: 10721
	protected Camera _rightEyeCamera;
}
