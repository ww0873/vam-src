using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D9A RID: 3482
public class CollisionTriggerEventHandler : MonoBehaviour
{
	// Token: 0x06006B69 RID: 27497 RVA: 0x00288BE8 File Offset: 0x00286FE8
	public CollisionTriggerEventHandler()
	{
	}

	// Token: 0x06006B6A RID: 27498 RVA: 0x00288BFC File Offset: 0x00286FFC
	protected bool PassVelocityFilter(Collision c)
	{
		if (!this.useRelativeVelocityFilter)
		{
			return true;
		}
		this.lastRelativeVelocity = c.relativeVelocity.magnitude;
		if (this.relativeVelocityHandlers != null)
		{
			this.relativeVelocityHandlers(this.lastRelativeVelocity);
		}
		if (this.invertRelativeVelocityFilter)
		{
			return this.lastRelativeVelocity < this.relativeVelocityFilter;
		}
		return this.lastRelativeVelocity >= this.relativeVelocityFilter;
	}

	// Token: 0x06006B6B RID: 27499 RVA: 0x00288C74 File Offset: 0x00287074
	protected bool PassVelocityFilter(Collider c)
	{
		if (!this.useRelativeVelocityFilter || !(this.thisRigidbody != null))
		{
			return true;
		}
		this.lastRelativeVelocity = (c.attachedRigidbody.velocity - this.thisRigidbody.velocity).magnitude;
		if (this.relativeVelocityHandlers != null)
		{
			this.relativeVelocityHandlers(this.lastRelativeVelocity);
		}
		if (this.invertRelativeVelocityFilter)
		{
			return this.lastRelativeVelocity < this.relativeVelocityFilter;
		}
		return this.lastRelativeVelocity >= this.relativeVelocityFilter;
	}

	// Token: 0x06006B6C RID: 27500 RVA: 0x00288D10 File Offset: 0x00287110
	protected bool PassAtomFilter(Collider c)
	{
		if (this.atomFilterUID == null || !(this.atomFilterUID != "None"))
		{
			return true;
		}
		ForceReceiver componentInParent = c.GetComponentInParent<ForceReceiver>();
		if (componentInParent != null)
		{
			Atom containingAtom = componentInParent.containingAtom;
			if (this.invertAtomFilter)
			{
				if (containingAtom == null || containingAtom.uid != this.atomFilterUID)
				{
					return true;
				}
			}
			else if (containingAtom != null && containingAtom.uid == this.atomFilterUID)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	// Token: 0x06006B6D RID: 27501 RVA: 0x00288DB9 File Offset: 0x002871B9
	protected bool IsCollidingWith(Collider c)
	{
		return this.collidingWithDictionary != null && this.collidingWithDictionary.ContainsKey(c);
	}

	// Token: 0x06006B6E RID: 27502 RVA: 0x00288DDA File Offset: 0x002871DA
	protected bool IsCollidingWithButFailedVelocityCheck(Collider c)
	{
		return this.collidingWithButFailedVelocityTestDictionary != null && this.collidingWithButFailedVelocityTestDictionary.ContainsKey(c);
	}

	// Token: 0x06006B6F RID: 27503 RVA: 0x00288DFC File Offset: 0x002871FC
	protected void AddCollidingWith(Collider c)
	{
		if (this.collidingWithDictionary != null && !this.collidingWithDictionary.ContainsKey(c))
		{
			this.collidingWithDictionary.Add(c, true);
		}
		if (this.collisionTrigger != null)
		{
			this.collisionTrigger.trigger.active = true;
			this.collisionTrigger.trigger.transitionInterpValue = 1f;
		}
	}

	// Token: 0x06006B70 RID: 27504 RVA: 0x00288E69 File Offset: 0x00287269
	protected void AddCollidingWithButFailedVelocityTest(Collider c)
	{
		if (this.collidingWithButFailedVelocityTestDictionary != null && !this.collidingWithButFailedVelocityTestDictionary.ContainsKey(c))
		{
			this.collidingWithButFailedVelocityTestDictionary.Add(c, true);
		}
	}

	// Token: 0x06006B71 RID: 27505 RVA: 0x00288E94 File Offset: 0x00287294
	protected void RemoveCollidingWith(Collider c)
	{
		if (this.collidingWithDictionary != null)
		{
			this.collidingWithDictionary.Remove(c);
			if (this.collidingWithDictionary.Count == 0 && this.collisionTrigger != null)
			{
				this.collisionTrigger.trigger.transitionInterpValue = 0f;
				this.collisionTrigger.trigger.active = false;
			}
		}
		if (this.collidingWithButFailedVelocityTestDictionary != null)
		{
			this.collidingWithButFailedVelocityTestDictionary.Remove(c);
		}
	}

	// Token: 0x06006B72 RID: 27506 RVA: 0x00288F18 File Offset: 0x00287318
	protected void RemoveAllCollidingWith()
	{
		if (this.collidingWithButFailedVelocityTestDictionary != null)
		{
			this.collidingWithButFailedVelocityTestDictionary.Clear();
		}
		if (this.collidingWithDictionary != null)
		{
			this.collidingWithDictionary.Clear();
		}
		if (this.collisionTrigger != null)
		{
			this.collisionTrigger.trigger.transitionInterpValue = 0f;
			this.collisionTrigger.trigger.active = false;
		}
	}

	// Token: 0x06006B73 RID: 27507 RVA: 0x00288F88 File Offset: 0x00287388
	private void OnCollisionEnter(Collision collision)
	{
		if (this.PassAtomFilter(collision.collider) && (SuperController.singleton == null || !SuperController.singleton.isLoading))
		{
			if (this.PassVelocityFilter(collision))
			{
				this.AddCollidingWith(collision.collider);
			}
			else
			{
				this.AddCollidingWithButFailedVelocityTest(collision.collider);
			}
		}
	}

	// Token: 0x06006B74 RID: 27508 RVA: 0x00288FF0 File Offset: 0x002873F0
	private void OnCollisionStay(Collision collision)
	{
		if (this.PassAtomFilter(collision.collider) && !this.IsCollidingWith(collision.collider) && !this.IsCollidingWithButFailedVelocityCheck(collision.collider) && (SuperController.singleton == null || !SuperController.singleton.isLoading))
		{
			if (this.PassVelocityFilter(collision))
			{
				this.AddCollidingWith(collision.collider);
			}
			else
			{
				this.AddCollidingWithButFailedVelocityTest(collision.collider);
			}
		}
	}

	// Token: 0x06006B75 RID: 27509 RVA: 0x00289078 File Offset: 0x00287478
	private void OnCollisionExit(Collision collision)
	{
		if (SuperController.singleton == null || !SuperController.singleton.isLoading)
		{
			this.RemoveCollidingWith(collision.collider);
		}
	}

	// Token: 0x06006B76 RID: 27510 RVA: 0x002890A8 File Offset: 0x002874A8
	private void OnTriggerEnter(Collider other)
	{
		if (this.PassAtomFilter(other) && (SuperController.singleton == null || !SuperController.singleton.isLoading))
		{
			if (this.PassVelocityFilter(other))
			{
				this.AddCollidingWith(other);
			}
			else
			{
				this.AddCollidingWithButFailedVelocityTest(other);
			}
		}
	}

	// Token: 0x06006B77 RID: 27511 RVA: 0x00289100 File Offset: 0x00287500
	private void OnTriggerStay(Collider other)
	{
		if (this.PassAtomFilter(other) && !this.IsCollidingWith(other) && !this.IsCollidingWithButFailedVelocityCheck(other) && (SuperController.singleton == null || !SuperController.singleton.isLoading))
		{
			if (this.PassVelocityFilter(other))
			{
				this.AddCollidingWith(other);
			}
			else
			{
				this.AddCollidingWithButFailedVelocityTest(other);
			}
		}
	}

	// Token: 0x06006B78 RID: 27512 RVA: 0x0028916F File Offset: 0x0028756F
	private void OnTriggerExit(Collider other)
	{
		if (SuperController.singleton == null || !SuperController.singleton.isLoading)
		{
			this.RemoveCollidingWith(other);
		}
	}

	// Token: 0x06006B79 RID: 27513 RVA: 0x00289198 File Offset: 0x00287598
	public void Reset()
	{
		this.collisionTrigger.trigger.transitionInterpValue = 0f;
		this.collisionTrigger.trigger.active = false;
		this.thisCollider = base.GetComponent<Collider>();
		if (this.thisCollider != null)
		{
			this.thisRigidbody = this.thisCollider.attachedRigidbody;
		}
		if (this.collidingWithDictionary == null)
		{
			this.collidingWithDictionary = new Dictionary<Collider, bool>();
		}
		else
		{
			this.collidingWithDictionary.Clear();
		}
		if (this.collidingWithButFailedVelocityTestDictionary == null)
		{
			this.collidingWithButFailedVelocityTestDictionary = new Dictionary<Collider, bool>();
		}
		else
		{
			this.collidingWithButFailedVelocityTestDictionary.Clear();
		}
	}

	// Token: 0x06006B7A RID: 27514 RVA: 0x00289245 File Offset: 0x00287645
	private void OnDisable()
	{
		this.RemoveAllCollidingWith();
	}

	// Token: 0x06006B7B RID: 27515 RVA: 0x00289250 File Offset: 0x00287650
	private void FixedUpdate()
	{
		if (this.removeList == null)
		{
			this.removeList = new List<Collider>();
		}
		else
		{
			this.removeList.Clear();
		}
		if (this.collidingWithDictionary.Count > 0)
		{
			foreach (Collider collider in this.collidingWithDictionary.Keys)
			{
				if (collider == null)
				{
					this.removeList.Add(collider);
				}
				else if (!collider.gameObject.activeInHierarchy)
				{
					this.removeList.Add(collider);
				}
			}
		}
		foreach (Collider c in this.removeList)
		{
			this.RemoveCollidingWith(c);
		}
		if (this.debug)
		{
			this.collidingWith = new List<Collider>(this.collidingWithDictionary.Keys);
		}
	}

	// Token: 0x06006B7C RID: 27516 RVA: 0x00289388 File Offset: 0x00287788
	protected void OnDestroy()
	{
		this.RemoveAllCollidingWith();
	}

	// Token: 0x04005D31 RID: 23857
	public CollisionTrigger collisionTrigger;

	// Token: 0x04005D32 RID: 23858
	public Collider thisCollider;

	// Token: 0x04005D33 RID: 23859
	public Rigidbody thisRigidbody;

	// Token: 0x04005D34 RID: 23860
	public Dictionary<Collider, bool> collidingWithButFailedVelocityTestDictionary;

	// Token: 0x04005D35 RID: 23861
	public Dictionary<Collider, bool> collidingWithDictionary;

	// Token: 0x04005D36 RID: 23862
	public List<Collider> collidingWith;

	// Token: 0x04005D37 RID: 23863
	public string atomFilterUID;

	// Token: 0x04005D38 RID: 23864
	public bool invertAtomFilter;

	// Token: 0x04005D39 RID: 23865
	public bool useRelativeVelocityFilter;

	// Token: 0x04005D3A RID: 23866
	public bool invertRelativeVelocityFilter;

	// Token: 0x04005D3B RID: 23867
	public float relativeVelocityFilter = 1f;

	// Token: 0x04005D3C RID: 23868
	public float lastRelativeVelocity;

	// Token: 0x04005D3D RID: 23869
	public CollisionTriggerEventHandler.RelativeVelocityCallback relativeVelocityHandlers;

	// Token: 0x04005D3E RID: 23870
	public bool debug;

	// Token: 0x04005D3F RID: 23871
	protected List<Collider> removeList;

	// Token: 0x02000D9B RID: 3483
	// (Invoke) Token: 0x06006B7E RID: 27518
	public delegate void RelativeVelocityCallback(float f);
}
