using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D7C RID: 3452
public class IgnoreChildColliders : MonoBehaviour
{
	// Token: 0x06006A4B RID: 27211 RVA: 0x00280D6F File Offset: 0x0027F16F
	public IgnoreChildColliders()
	{
	}

	// Token: 0x06006A4C RID: 27212 RVA: 0x00280D78 File Offset: 0x0027F178
	private void GetCollidersRecursive(Transform rootTransform, Transform t, List<Collider> colliders)
	{
		if (t != rootTransform && t.GetComponent<Rigidbody>())
		{
			return;
		}
		foreach (Collider collider in t.GetComponents<Collider>())
		{
			if (collider != null)
			{
				colliders.Add(collider);
			}
		}
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform t2 = (Transform)obj;
				this.GetCollidersRecursive(rootTransform, t2, colliders);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06006A4D RID: 27213 RVA: 0x00280E30 File Offset: 0x0027F230
	private void GetRigidbodyChildrenRecursive(Transform rootTransform, Transform t, List<Transform> children)
	{
		if (t != rootTransform && t.GetComponent<Rigidbody>())
		{
			children.Add(t);
			return;
		}
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform t2 = (Transform)obj;
				this.GetRigidbodyChildrenRecursive(rootTransform, t2, children);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06006A4E RID: 27214 RVA: 0x00280EB8 File Offset: 0x0027F2B8
	public void SyncColliders()
	{
		if (this.myCollidersList == null)
		{
			this.myCollidersList = new List<Collider>();
		}
		else
		{
			this.myCollidersList.Clear();
		}
		if (this.allPossibleCollidersList == null)
		{
			this.allPossibleCollidersList = new List<Collider>();
			this.GetCollidersRecursive(base.transform, base.transform, this.allPossibleCollidersList);
		}
		foreach (Collider collider in this.allPossibleCollidersList)
		{
			if (collider != null && collider.gameObject.activeInHierarchy && collider.enabled)
			{
				this.myCollidersList.Add(collider);
			}
		}
		if (this.rigidbodyChildren == null)
		{
			this.rigidbodyChildren = new List<Transform>();
			this.GetRigidbodyChildrenRecursive(base.transform, base.transform, this.rigidbodyChildren);
		}
		if (this.childCollidersList == null)
		{
			this.childCollidersList = new List<Collider>();
		}
		else
		{
			this.childCollidersList.Clear();
		}
		if (this.allPossibleChildCollidersList == null)
		{
			this.allPossibleChildCollidersList = new List<Collider>();
			foreach (Transform transform in this.rigidbodyChildren)
			{
				this.GetCollidersRecursive(transform, transform, this.allPossibleChildCollidersList);
			}
		}
		foreach (Collider collider2 in this.allPossibleChildCollidersList)
		{
			if (collider2 != null && collider2.gameObject.activeInHierarchy && collider2.enabled)
			{
				this.childCollidersList.Add(collider2);
			}
		}
		foreach (Collider collider3 in this.myCollidersList)
		{
			foreach (Collider collider4 in this.childCollidersList)
			{
				Physics.IgnoreCollision(collider3, collider4);
			}
		}
		if (this.additionalIgnores != null)
		{
			if (this.additionalCollidersList == null)
			{
				this.additionalCollidersList = new List<Collider>();
			}
			else
			{
				this.additionalCollidersList.Clear();
			}
			if (this.allPossibleAdditionalCollidersList == null)
			{
				this.allPossibleAdditionalCollidersList = new List<Collider>();
				foreach (Transform transform2 in this.additionalIgnores)
				{
					this.GetCollidersRecursive(transform2, transform2, this.allPossibleAdditionalCollidersList);
				}
			}
			foreach (Collider collider5 in this.allPossibleAdditionalCollidersList)
			{
				if (collider5 != null && collider5.gameObject.activeInHierarchy && collider5.enabled)
				{
					this.additionalCollidersList.Add(collider5);
				}
			}
			foreach (Collider collider6 in this.myCollidersList)
			{
				foreach (Collider collider7 in this.additionalCollidersList)
				{
					Physics.IgnoreCollision(collider6, collider7);
				}
			}
		}
	}

	// Token: 0x06006A4F RID: 27215 RVA: 0x002812F4 File Offset: 0x0027F6F4
	private void OnEnable()
	{
		this.SyncColliders();
	}

	// Token: 0x04005C57 RID: 23639
	public Transform[] additionalIgnores;

	// Token: 0x04005C58 RID: 23640
	protected List<Collider> allPossibleCollidersList;

	// Token: 0x04005C59 RID: 23641
	protected List<Collider> myCollidersList;

	// Token: 0x04005C5A RID: 23642
	protected List<Transform> rigidbodyChildren;

	// Token: 0x04005C5B RID: 23643
	protected List<Collider> allPossibleChildCollidersList;

	// Token: 0x04005C5C RID: 23644
	protected List<Collider> childCollidersList;

	// Token: 0x04005C5D RID: 23645
	protected List<Collider> allPossibleAdditionalCollidersList;

	// Token: 0x04005C5E RID: 23646
	protected List<Collider> additionalCollidersList;
}
