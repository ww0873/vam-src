using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000963 RID: 2403
[RequireComponent(typeof(Rigidbody))]
public class OVRGrabber : MonoBehaviour
{
	// Token: 0x06003BD9 RID: 15321 RVA: 0x00120CD2 File Offset: 0x0011F0D2
	public OVRGrabber()
	{
	}

	// Token: 0x170006A1 RID: 1697
	// (get) Token: 0x06003BDA RID: 15322 RVA: 0x00120D09 File Offset: 0x0011F109
	public OVRGrabbable grabbedObject
	{
		get
		{
			return this.m_grabbedObj;
		}
	}

	// Token: 0x06003BDB RID: 15323 RVA: 0x00120D14 File Offset: 0x0011F114
	public void ForceRelease(OVRGrabbable grabbable)
	{
		bool flag = this.m_grabbedObj != null && this.m_grabbedObj == grabbable;
		if (flag)
		{
			this.GrabEnd();
		}
	}

	// Token: 0x06003BDC RID: 15324 RVA: 0x00120D50 File Offset: 0x0011F150
	protected virtual void Awake()
	{
		this.m_anchorOffsetPosition = base.transform.localPosition;
		this.m_anchorOffsetRotation = base.transform.localRotation;
		OVRCameraRig ovrcameraRig = null;
		if (base.transform.parent != null && base.transform.parent.parent != null)
		{
			ovrcameraRig = base.transform.parent.parent.GetComponent<OVRCameraRig>();
		}
		if (ovrcameraRig != null)
		{
			ovrcameraRig.UpdatedAnchors += this.<Awake>m__0;
			this.operatingWithoutOVRCameraRig = false;
		}
	}

	// Token: 0x06003BDD RID: 15325 RVA: 0x00120DF0 File Offset: 0x0011F1F0
	protected virtual void Start()
	{
		this.m_lastPos = base.transform.position;
		this.m_lastRot = base.transform.rotation;
		if (this.m_parentTransform == null)
		{
			if (base.gameObject.transform.parent != null)
			{
				this.m_parentTransform = base.gameObject.transform.parent.transform;
			}
			else
			{
				this.m_parentTransform = new GameObject().transform;
				this.m_parentTransform.position = Vector3.zero;
				this.m_parentTransform.rotation = Quaternion.identity;
			}
		}
	}

	// Token: 0x06003BDE RID: 15326 RVA: 0x00120E9B File Offset: 0x0011F29B
	private void FixedUpdate()
	{
		if (this.operatingWithoutOVRCameraRig)
		{
			this.OnUpdatedAnchors();
		}
	}

	// Token: 0x06003BDF RID: 15327 RVA: 0x00120EB0 File Offset: 0x0011F2B0
	private void OnUpdatedAnchors()
	{
		Vector3 localControllerPosition = OVRInput.GetLocalControllerPosition(this.m_controller);
		Quaternion localControllerRotation = OVRInput.GetLocalControllerRotation(this.m_controller);
		Vector3 vector = this.m_parentTransform.TransformPoint(this.m_anchorOffsetPosition + localControllerPosition);
		Quaternion rot = this.m_parentTransform.rotation * localControllerRotation * this.m_anchorOffsetRotation;
		base.GetComponent<Rigidbody>().MovePosition(vector);
		base.GetComponent<Rigidbody>().MoveRotation(rot);
		if (!this.m_parentHeldObject)
		{
			this.MoveGrabbedObject(vector, rot, false);
		}
		this.m_lastPos = base.transform.position;
		this.m_lastRot = base.transform.rotation;
		float prevFlex = this.m_prevFlex;
		this.m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, this.m_controller);
		this.CheckForGrabOrRelease(prevFlex);
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x00120F7A File Offset: 0x0011F37A
	private void OnDestroy()
	{
		if (this.m_grabbedObj != null)
		{
			this.GrabEnd();
		}
	}

	// Token: 0x06003BE1 RID: 15329 RVA: 0x00120F94 File Offset: 0x0011F394
	private void OnTriggerEnter(Collider otherCollider)
	{
		OVRGrabbable ovrgrabbable = otherCollider.GetComponent<OVRGrabbable>() ?? otherCollider.GetComponentInParent<OVRGrabbable>();
		if (ovrgrabbable == null)
		{
			return;
		}
		int num = 0;
		this.m_grabCandidates.TryGetValue(ovrgrabbable, out num);
		this.m_grabCandidates[ovrgrabbable] = num + 1;
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x00120FE4 File Offset: 0x0011F3E4
	private void OnTriggerExit(Collider otherCollider)
	{
		OVRGrabbable ovrgrabbable = otherCollider.GetComponent<OVRGrabbable>() ?? otherCollider.GetComponentInParent<OVRGrabbable>();
		if (ovrgrabbable == null)
		{
			return;
		}
		int num = 0;
		if (!this.m_grabCandidates.TryGetValue(ovrgrabbable, out num))
		{
			return;
		}
		if (num > 1)
		{
			this.m_grabCandidates[ovrgrabbable] = num - 1;
		}
		else
		{
			this.m_grabCandidates.Remove(ovrgrabbable);
		}
	}

	// Token: 0x06003BE3 RID: 15331 RVA: 0x00121054 File Offset: 0x0011F454
	protected void CheckForGrabOrRelease(float prevFlex)
	{
		if (this.m_prevFlex >= this.grabBegin && prevFlex < this.grabBegin)
		{
			this.GrabBegin();
		}
		else if (this.m_prevFlex <= this.grabEnd && prevFlex > this.grabEnd)
		{
			this.GrabEnd();
		}
	}

	// Token: 0x06003BE4 RID: 15332 RVA: 0x001210AC File Offset: 0x0011F4AC
	protected virtual void GrabBegin()
	{
		float num = float.MaxValue;
		OVRGrabbable ovrgrabbable = null;
		Collider grabPoint = null;
		foreach (OVRGrabbable ovrgrabbable2 in this.m_grabCandidates.Keys)
		{
			if (!ovrgrabbable2.isGrabbed || ovrgrabbable2.allowOffhandGrab)
			{
				for (int i = 0; i < ovrgrabbable2.grabPoints.Length; i++)
				{
					Collider collider = ovrgrabbable2.grabPoints[i];
					Vector3 b = collider.ClosestPointOnBounds(this.m_gripTransform.position);
					float sqrMagnitude = (this.m_gripTransform.position - b).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						ovrgrabbable = ovrgrabbable2;
						grabPoint = collider;
					}
				}
			}
		}
		this.GrabVolumeEnable(false);
		if (ovrgrabbable != null)
		{
			if (ovrgrabbable.isGrabbed)
			{
				ovrgrabbable.grabbedBy.OffhandGrabbed(ovrgrabbable);
			}
			this.m_grabbedObj = ovrgrabbable;
			this.m_grabbedObj.GrabBegin(this, grabPoint);
			this.m_lastPos = base.transform.position;
			this.m_lastRot = base.transform.rotation;
			if (this.m_grabbedObj.snapPosition)
			{
				this.m_grabbedObjectPosOff = this.m_gripTransform.localPosition;
				if (this.m_grabbedObj.snapOffset)
				{
					Vector3 position = this.m_grabbedObj.snapOffset.position;
					if (this.m_controller == OVRInput.Controller.LTouch)
					{
						position.x = -position.x;
					}
					this.m_grabbedObjectPosOff += position;
				}
			}
			else
			{
				Vector3 vector = this.m_grabbedObj.transform.position - base.transform.position;
				vector = Quaternion.Inverse(base.transform.rotation) * vector;
				this.m_grabbedObjectPosOff = vector;
			}
			if (this.m_grabbedObj.snapOrientation)
			{
				this.m_grabbedObjectRotOff = this.m_gripTransform.localRotation;
				if (this.m_grabbedObj.snapOffset)
				{
					this.m_grabbedObjectRotOff = this.m_grabbedObj.snapOffset.rotation * this.m_grabbedObjectRotOff;
				}
			}
			else
			{
				Quaternion grabbedObjectRotOff = Quaternion.Inverse(base.transform.rotation) * this.m_grabbedObj.transform.rotation;
				this.m_grabbedObjectRotOff = grabbedObjectRotOff;
			}
			this.MoveGrabbedObject(this.m_lastPos, this.m_lastRot, true);
			if (this.m_parentHeldObject)
			{
				this.m_grabbedObj.transform.parent = base.transform;
			}
		}
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x00121380 File Offset: 0x0011F780
	protected virtual void MoveGrabbedObject(Vector3 pos, Quaternion rot, bool forceTeleport = false)
	{
		if (this.m_grabbedObj == null)
		{
			return;
		}
		Rigidbody grabbedRigidbody = this.m_grabbedObj.grabbedRigidbody;
		Vector3 position = pos + rot * this.m_grabbedObjectPosOff;
		Quaternion quaternion = rot * this.m_grabbedObjectRotOff;
		if (forceTeleport)
		{
			grabbedRigidbody.transform.position = position;
			grabbedRigidbody.transform.rotation = quaternion;
		}
		else
		{
			grabbedRigidbody.MovePosition(position);
			grabbedRigidbody.MoveRotation(quaternion);
		}
	}

	// Token: 0x06003BE6 RID: 15334 RVA: 0x001213FC File Offset: 0x0011F7FC
	protected void GrabEnd()
	{
		if (this.m_grabbedObj != null)
		{
			OVRPose lhs = new OVRPose
			{
				position = OVRInput.GetLocalControllerPosition(this.m_controller),
				orientation = OVRInput.GetLocalControllerRotation(this.m_controller)
			};
			OVRPose rhs = new OVRPose
			{
				position = this.m_anchorOffsetPosition,
				orientation = this.m_anchorOffsetRotation
			};
			lhs *= rhs;
			OVRPose ovrpose = base.transform.ToOVRPose(false) * lhs.Inverse();
			Vector3 linearVelocity = ovrpose.orientation * OVRInput.GetLocalControllerVelocity(this.m_controller);
			Vector3 angularVelocity = ovrpose.orientation * OVRInput.GetLocalControllerAngularVelocity(this.m_controller);
			this.GrabbableRelease(linearVelocity, angularVelocity);
		}
		this.GrabVolumeEnable(true);
	}

	// Token: 0x06003BE7 RID: 15335 RVA: 0x001214D0 File Offset: 0x0011F8D0
	protected void GrabbableRelease(Vector3 linearVelocity, Vector3 angularVelocity)
	{
		this.m_grabbedObj.GrabEnd(linearVelocity, angularVelocity);
		if (this.m_parentHeldObject)
		{
			this.m_grabbedObj.transform.parent = null;
		}
		this.m_grabbedObj = null;
	}

	// Token: 0x06003BE8 RID: 15336 RVA: 0x00121504 File Offset: 0x0011F904
	protected virtual void GrabVolumeEnable(bool enabled)
	{
		if (this.m_grabVolumeEnabled == enabled)
		{
			return;
		}
		this.m_grabVolumeEnabled = enabled;
		for (int i = 0; i < this.m_grabVolumes.Length; i++)
		{
			Collider collider = this.m_grabVolumes[i];
			collider.enabled = this.m_grabVolumeEnabled;
		}
		if (!this.m_grabVolumeEnabled)
		{
			this.m_grabCandidates.Clear();
		}
	}

	// Token: 0x06003BE9 RID: 15337 RVA: 0x00121569 File Offset: 0x0011F969
	protected virtual void OffhandGrabbed(OVRGrabbable grabbable)
	{
		if (this.m_grabbedObj == grabbable)
		{
			this.GrabbableRelease(Vector3.zero, Vector3.zero);
		}
	}

	// Token: 0x06003BEA RID: 15338 RVA: 0x0012158C File Offset: 0x0011F98C
	[CompilerGenerated]
	private void <Awake>m__0(OVRCameraRig r)
	{
		this.OnUpdatedAnchors();
	}

	// Token: 0x04002DC0 RID: 11712
	public float grabBegin = 0.55f;

	// Token: 0x04002DC1 RID: 11713
	public float grabEnd = 0.35f;

	// Token: 0x04002DC2 RID: 11714
	[SerializeField]
	protected bool m_parentHeldObject;

	// Token: 0x04002DC3 RID: 11715
	[SerializeField]
	protected Transform m_gripTransform;

	// Token: 0x04002DC4 RID: 11716
	[SerializeField]
	protected Collider[] m_grabVolumes;

	// Token: 0x04002DC5 RID: 11717
	[SerializeField]
	protected OVRInput.Controller m_controller;

	// Token: 0x04002DC6 RID: 11718
	[SerializeField]
	protected Transform m_parentTransform;

	// Token: 0x04002DC7 RID: 11719
	protected bool m_grabVolumeEnabled = true;

	// Token: 0x04002DC8 RID: 11720
	protected Vector3 m_lastPos;

	// Token: 0x04002DC9 RID: 11721
	protected Quaternion m_lastRot;

	// Token: 0x04002DCA RID: 11722
	protected Quaternion m_anchorOffsetRotation;

	// Token: 0x04002DCB RID: 11723
	protected Vector3 m_anchorOffsetPosition;

	// Token: 0x04002DCC RID: 11724
	protected float m_prevFlex;

	// Token: 0x04002DCD RID: 11725
	protected OVRGrabbable m_grabbedObj;

	// Token: 0x04002DCE RID: 11726
	protected Vector3 m_grabbedObjectPosOff;

	// Token: 0x04002DCF RID: 11727
	protected Quaternion m_grabbedObjectRotOff;

	// Token: 0x04002DD0 RID: 11728
	protected Dictionary<OVRGrabbable, int> m_grabCandidates = new Dictionary<OVRGrabbable, int>();

	// Token: 0x04002DD1 RID: 11729
	protected bool operatingWithoutOVRCameraRig = true;
}
