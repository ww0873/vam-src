using System;
using UnityEngine;

// Token: 0x02000962 RID: 2402
public class OVRGrabbable : MonoBehaviour
{
	// Token: 0x06003BCA RID: 15306 RVA: 0x00120B8C File Offset: 0x0011EF8C
	public OVRGrabbable()
	{
	}

	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x06003BCB RID: 15307 RVA: 0x00120B9B File Offset: 0x0011EF9B
	public bool allowOffhandGrab
	{
		get
		{
			return this.m_allowOffhandGrab;
		}
	}

	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x06003BCC RID: 15308 RVA: 0x00120BA3 File Offset: 0x0011EFA3
	public bool isGrabbed
	{
		get
		{
			return this.m_grabbedBy != null;
		}
	}

	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x06003BCD RID: 15309 RVA: 0x00120BB1 File Offset: 0x0011EFB1
	public bool snapPosition
	{
		get
		{
			return this.m_snapPosition;
		}
	}

	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x06003BCE RID: 15310 RVA: 0x00120BB9 File Offset: 0x0011EFB9
	public bool snapOrientation
	{
		get
		{
			return this.m_snapOrientation;
		}
	}

	// Token: 0x1700069C RID: 1692
	// (get) Token: 0x06003BCF RID: 15311 RVA: 0x00120BC1 File Offset: 0x0011EFC1
	public Transform snapOffset
	{
		get
		{
			return this.m_snapOffset;
		}
	}

	// Token: 0x1700069D RID: 1693
	// (get) Token: 0x06003BD0 RID: 15312 RVA: 0x00120BC9 File Offset: 0x0011EFC9
	public OVRGrabber grabbedBy
	{
		get
		{
			return this.m_grabbedBy;
		}
	}

	// Token: 0x1700069E RID: 1694
	// (get) Token: 0x06003BD1 RID: 15313 RVA: 0x00120BD1 File Offset: 0x0011EFD1
	public Transform grabbedTransform
	{
		get
		{
			return this.m_grabbedCollider.transform;
		}
	}

	// Token: 0x1700069F RID: 1695
	// (get) Token: 0x06003BD2 RID: 15314 RVA: 0x00120BDE File Offset: 0x0011EFDE
	public Rigidbody grabbedRigidbody
	{
		get
		{
			return this.m_grabbedCollider.attachedRigidbody;
		}
	}

	// Token: 0x170006A0 RID: 1696
	// (get) Token: 0x06003BD3 RID: 15315 RVA: 0x00120BEB File Offset: 0x0011EFEB
	public Collider[] grabPoints
	{
		get
		{
			return this.m_grabPoints;
		}
	}

	// Token: 0x06003BD4 RID: 15316 RVA: 0x00120BF3 File Offset: 0x0011EFF3
	public virtual void GrabBegin(OVRGrabber hand, Collider grabPoint)
	{
		this.m_grabbedBy = hand;
		this.m_grabbedCollider = grabPoint;
		base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x00120C14 File Offset: 0x0011F014
	public virtual void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
	{
		Rigidbody component = base.gameObject.GetComponent<Rigidbody>();
		component.isKinematic = this.m_grabbedKinematic;
		component.velocity = linearVelocity;
		component.angularVelocity = angularVelocity;
		this.m_grabbedBy = null;
		this.m_grabbedCollider = null;
	}

	// Token: 0x06003BD6 RID: 15318 RVA: 0x00120C58 File Offset: 0x0011F058
	private void Awake()
	{
		if (this.m_grabPoints.Length == 0)
		{
			Collider component = base.GetComponent<Collider>();
			if (component == null)
			{
				throw new ArgumentException("Grabbables cannot have zero grab points and no collider -- please add a grab point or collider.");
			}
			this.m_grabPoints = new Collider[]
			{
				component
			};
		}
	}

	// Token: 0x06003BD7 RID: 15319 RVA: 0x00120CA0 File Offset: 0x0011F0A0
	protected virtual void Start()
	{
		this.m_grabbedKinematic = base.GetComponent<Rigidbody>().isKinematic;
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x00120CB3 File Offset: 0x0011F0B3
	private void OnDestroy()
	{
		if (this.m_grabbedBy != null)
		{
			this.m_grabbedBy.ForceRelease(this);
		}
	}

	// Token: 0x04002DB8 RID: 11704
	[SerializeField]
	protected bool m_allowOffhandGrab = true;

	// Token: 0x04002DB9 RID: 11705
	[SerializeField]
	protected bool m_snapPosition;

	// Token: 0x04002DBA RID: 11706
	[SerializeField]
	protected bool m_snapOrientation;

	// Token: 0x04002DBB RID: 11707
	[SerializeField]
	protected Transform m_snapOffset;

	// Token: 0x04002DBC RID: 11708
	[SerializeField]
	protected Collider[] m_grabPoints;

	// Token: 0x04002DBD RID: 11709
	protected bool m_grabbedKinematic;

	// Token: 0x04002DBE RID: 11710
	protected Collider m_grabbedCollider;

	// Token: 0x04002DBF RID: 11711
	protected OVRGrabber m_grabbedBy;
}
