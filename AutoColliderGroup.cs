using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AA4 RID: 2724
[ExecuteInEditMode]
public class AutoColliderGroup : MonoBehaviour
{
	// Token: 0x06004706 RID: 18182 RVA: 0x0014B514 File Offset: 0x00149914
	public AutoColliderGroup()
	{
	}

	// Token: 0x06004707 RID: 18183 RVA: 0x0014B5BC File Offset: 0x001499BC
	protected void SyncChildAutoColliders()
	{
		if (this.controlChildAutoColliders)
		{
			AutoCollider[] autoColliders = this.GetAutoColliders();
			foreach (AutoCollider autoCollider in autoColliders)
			{
				if (!autoCollider.ignoreGroupSettings)
				{
					autoCollider.colliderMaterial = this._colliderMaterial;
					autoCollider.jointSpringLook = this._jointSpringLook;
					autoCollider.jointDamperLook = this._jointDamperLook;
					autoCollider.jointSpringUp = this._jointSpringUp;
					autoCollider.jointDamperUp = this._jointDamperUp;
					autoCollider.jointSpringRight = this._jointSpringRight;
					autoCollider.jointDamperRight = this._jointDamperRight;
					autoCollider.jointSpringMaxForce = this._jointSpringMaxForce;
					autoCollider.jointMass = this._jointMass;
					autoCollider.jointBackForce = this._jointBackForce;
					autoCollider.jointBackForceThresholdDistance = this._jointBackForceThresholdDistance;
					autoCollider.jointBackForceMaxForce = this._jointBackForceMaxForce;
					autoCollider.softJointLimit = this._jointLimit;
					autoCollider.softJointLimitSpring = this._jointLimitSpring;
					autoCollider.softJointLimitDamper = this._jointLimitDamper;
					autoCollider.showUsedVerts = this._showUsedVerts;
					autoCollider.autoRadiusMultiplier = this._autoRadiusMultiplier;
				}
			}
		}
	}

	// Token: 0x170009DE RID: 2526
	// (get) Token: 0x06004708 RID: 18184 RVA: 0x0014B6CC File Offset: 0x00149ACC
	// (set) Token: 0x06004709 RID: 18185 RVA: 0x0014B6D4 File Offset: 0x00149AD4
	public PhysicMaterial colliderMaterial
	{
		get
		{
			return this._colliderMaterial;
		}
		set
		{
			if (this._colliderMaterial != value)
			{
				this._colliderMaterial = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009DF RID: 2527
	// (get) Token: 0x0600470A RID: 18186 RVA: 0x0014B6F4 File Offset: 0x00149AF4
	// (set) Token: 0x0600470B RID: 18187 RVA: 0x0014B6FC File Offset: 0x00149AFC
	public float jointLimit
	{
		get
		{
			return this._jointLimit;
		}
		set
		{
			if (this._jointLimit != value)
			{
				this._jointLimit = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009E0 RID: 2528
	// (get) Token: 0x0600470C RID: 18188 RVA: 0x0014B717 File Offset: 0x00149B17
	// (set) Token: 0x0600470D RID: 18189 RVA: 0x0014B71F File Offset: 0x00149B1F
	public float jointLimitSpring
	{
		get
		{
			return this._jointLimitSpring;
		}
		set
		{
			if (this._jointLimitSpring != value)
			{
				this._jointLimitSpring = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009E1 RID: 2529
	// (get) Token: 0x0600470E RID: 18190 RVA: 0x0014B73A File Offset: 0x00149B3A
	// (set) Token: 0x0600470F RID: 18191 RVA: 0x0014B742 File Offset: 0x00149B42
	public float jointLimitDamper
	{
		get
		{
			return this._jointLimitDamper;
		}
		set
		{
			if (this._jointLimitDamper != value)
			{
				this._jointLimitDamper = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009E2 RID: 2530
	// (get) Token: 0x06004710 RID: 18192 RVA: 0x0014B75D File Offset: 0x00149B5D
	// (set) Token: 0x06004711 RID: 18193 RVA: 0x0014B765 File Offset: 0x00149B65
	public float jointSpringLook
	{
		get
		{
			return this._jointSpringLook;
		}
		set
		{
			if (this._jointSpringLook != value)
			{
				this._jointSpringLook = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009E3 RID: 2531
	// (get) Token: 0x06004712 RID: 18194 RVA: 0x0014B780 File Offset: 0x00149B80
	// (set) Token: 0x06004713 RID: 18195 RVA: 0x0014B788 File Offset: 0x00149B88
	public float jointDamperLook
	{
		get
		{
			return this._jointDamperLook;
		}
		set
		{
			if (this._jointDamperLook != value)
			{
				this._jointDamperLook = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009E4 RID: 2532
	// (get) Token: 0x06004714 RID: 18196 RVA: 0x0014B7A3 File Offset: 0x00149BA3
	// (set) Token: 0x06004715 RID: 18197 RVA: 0x0014B7AB File Offset: 0x00149BAB
	public float jointSpringUp
	{
		get
		{
			return this._jointSpringUp;
		}
		set
		{
			if (this._jointSpringUp != value)
			{
				this._jointSpringUp = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009E5 RID: 2533
	// (get) Token: 0x06004716 RID: 18198 RVA: 0x0014B7C6 File Offset: 0x00149BC6
	// (set) Token: 0x06004717 RID: 18199 RVA: 0x0014B7CE File Offset: 0x00149BCE
	public float jointDamperUp
	{
		get
		{
			return this._jointDamperUp;
		}
		set
		{
			if (this._jointDamperUp != value)
			{
				this._jointDamperUp = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009E6 RID: 2534
	// (get) Token: 0x06004718 RID: 18200 RVA: 0x0014B7E9 File Offset: 0x00149BE9
	// (set) Token: 0x06004719 RID: 18201 RVA: 0x0014B7F1 File Offset: 0x00149BF1
	public float jointSpringRight
	{
		get
		{
			return this._jointSpringRight;
		}
		set
		{
			if (this._jointSpringRight != value)
			{
				this._jointSpringRight = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x0600471A RID: 18202 RVA: 0x0014B80C File Offset: 0x00149C0C
	// (set) Token: 0x0600471B RID: 18203 RVA: 0x0014B814 File Offset: 0x00149C14
	public float jointDamperRight
	{
		get
		{
			return this._jointDamperRight;
		}
		set
		{
			if (this._jointDamperRight != value)
			{
				this._jointDamperRight = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009E8 RID: 2536
	// (get) Token: 0x0600471C RID: 18204 RVA: 0x0014B82F File Offset: 0x00149C2F
	// (set) Token: 0x0600471D RID: 18205 RVA: 0x0014B837 File Offset: 0x00149C37
	public float jointSpringMaxForce
	{
		get
		{
			return this._jointSpringMaxForce;
		}
		set
		{
			if (this._jointSpringMaxForce != value)
			{
				this._jointSpringMaxForce = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009E9 RID: 2537
	// (get) Token: 0x0600471E RID: 18206 RVA: 0x0014B852 File Offset: 0x00149C52
	// (set) Token: 0x0600471F RID: 18207 RVA: 0x0014B85A File Offset: 0x00149C5A
	public float jointMass
	{
		get
		{
			return this._jointMass;
		}
		set
		{
			if (this._jointMass != value)
			{
				this._jointMass = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x06004720 RID: 18208 RVA: 0x0014B875 File Offset: 0x00149C75
	// (set) Token: 0x06004721 RID: 18209 RVA: 0x0014B87D File Offset: 0x00149C7D
	public float jointBackForce
	{
		get
		{
			return this._jointBackForce;
		}
		set
		{
			if (this._jointBackForce != value)
			{
				this._jointBackForce = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x06004722 RID: 18210 RVA: 0x0014B898 File Offset: 0x00149C98
	// (set) Token: 0x06004723 RID: 18211 RVA: 0x0014B8A0 File Offset: 0x00149CA0
	public float jointBackForceThresholdDistance
	{
		get
		{
			return this._jointBackForceThresholdDistance;
		}
		set
		{
			if (this._jointBackForceThresholdDistance != value)
			{
				this._jointBackForceThresholdDistance = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x06004724 RID: 18212 RVA: 0x0014B8BB File Offset: 0x00149CBB
	// (set) Token: 0x06004725 RID: 18213 RVA: 0x0014B8C3 File Offset: 0x00149CC3
	public float jointBackForceMaxForce
	{
		get
		{
			return this._jointBackForceMaxForce;
		}
		set
		{
			if (this._jointBackForceMaxForce != value)
			{
				this._jointBackForceMaxForce = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x06004726 RID: 18214 RVA: 0x0014B8DE File Offset: 0x00149CDE
	// (set) Token: 0x06004727 RID: 18215 RVA: 0x0014B8E6 File Offset: 0x00149CE6
	public float autoRadiusMultiplier
	{
		get
		{
			return this._autoRadiusMultiplier;
		}
		set
		{
			if (this._autoRadiusMultiplier != value)
			{
				this._autoRadiusMultiplier = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x06004728 RID: 18216 RVA: 0x0014B901 File Offset: 0x00149D01
	// (set) Token: 0x06004729 RID: 18217 RVA: 0x0014B909 File Offset: 0x00149D09
	public bool showUsedVerts
	{
		get
		{
			return this._showUsedVerts;
		}
		set
		{
			if (this._showUsedVerts != value)
			{
				this._showUsedVerts = value;
				this.SyncChildAutoColliders();
			}
		}
	}

	// Token: 0x0600472A RID: 18218 RVA: 0x0014B924 File Offset: 0x00149D24
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

	// Token: 0x0600472B RID: 18219 RVA: 0x0014B9DC File Offset: 0x00149DDC
	protected void InitAllAutoColliders(bool force = false)
	{
		if (this._allAutoColliders == null || force)
		{
			this._allAutoColliders = base.GetComponentsInChildren<AutoCollider>(true);
		}
	}

	// Token: 0x0600472C RID: 18220 RVA: 0x0014B9FC File Offset: 0x00149DFC
	public AutoCollider[] GetAutoColliders()
	{
		return base.GetComponentsInChildren<AutoCollider>();
	}

	// Token: 0x0600472D RID: 18221 RVA: 0x0014BA04 File Offset: 0x00149E04
	public void InitColliders()
	{
		this.InitAllAutoColliders(false);
		if (this.ignoreCollidersList == null)
		{
			this.ignoreCollidersList = new List<Collider>();
		}
		else
		{
			this.ignoreCollidersList.Clear();
		}
		if (this.allPossibleIgnoreCollidersList == null)
		{
			this.allPossibleIgnoreCollidersList = new List<Collider>();
			foreach (Transform transform in this.ignoreColliders)
			{
				this.GetCollidersRecursive(transform, transform, this.allPossibleIgnoreCollidersList);
			}
		}
		foreach (Collider collider in this.allPossibleIgnoreCollidersList)
		{
			if (collider != null && collider.gameObject.activeInHierarchy && collider.enabled)
			{
				this.ignoreCollidersList.Add(collider);
			}
		}
		foreach (AutoCollider autoCollider in this._allAutoColliders)
		{
			if (autoCollider.jointCollider != null)
			{
				foreach (AutoCollider autoCollider2 in this._allAutoColliders)
				{
					if (autoCollider != autoCollider2 && autoCollider2.jointCollider != null)
					{
						Physics.IgnoreCollision(autoCollider.jointCollider, autoCollider2.jointCollider);
					}
				}
				foreach (Collider collider2 in this.ignoreCollidersList)
				{
					Physics.IgnoreCollision(autoCollider.jointCollider, collider2);
				}
			}
		}
		foreach (AutoColliderGroup autoColliderGroup in this.ignoreColliderGroups)
		{
			AutoCollider[] componentsInChildren = autoColliderGroup.GetComponentsInChildren<AutoCollider>();
			foreach (AutoCollider autoCollider3 in this._allAutoColliders)
			{
				if (autoCollider3.jointCollider != null)
				{
					foreach (AutoCollider autoCollider4 in componentsInChildren)
					{
						if (autoCollider4.jointCollider != null)
						{
							Physics.IgnoreCollision(autoCollider3.jointCollider, autoCollider4.jointCollider);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600472E RID: 18222 RVA: 0x0014BC9C File Offset: 0x0014A09C
	private void OnEnable()
	{
		this.InitAllAutoColliders(!Application.isPlaying);
		foreach (AutoCollider autoCollider in this._allAutoColliders)
		{
			if (!autoCollider.gameObject.activeSelf)
			{
				autoCollider.gameObject.SetActive(true);
			}
		}
		if (Application.isPlaying)
		{
			this.InitColliders();
		}
	}

	// Token: 0x0600472F RID: 18223 RVA: 0x0014BD04 File Offset: 0x0014A104
	private void OnDisable()
	{
		this.InitAllAutoColliders(!Application.isPlaying);
		foreach (AutoCollider autoCollider in this._allAutoColliders)
		{
			if (autoCollider.gameObject.activeSelf)
			{
				autoCollider.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x04003464 RID: 13412
	public bool controlChildAutoColliders = true;

	// Token: 0x04003465 RID: 13413
	[SerializeField]
	private PhysicMaterial _colliderMaterial;

	// Token: 0x04003466 RID: 13414
	[SerializeField]
	private float _jointLimit;

	// Token: 0x04003467 RID: 13415
	[SerializeField]
	private float _jointLimitSpring;

	// Token: 0x04003468 RID: 13416
	[SerializeField]
	private float _jointLimitDamper;

	// Token: 0x04003469 RID: 13417
	[SerializeField]
	private float _jointSpringLook = 2000f;

	// Token: 0x0400346A RID: 13418
	[SerializeField]
	private float _jointDamperLook = 10f;

	// Token: 0x0400346B RID: 13419
	[SerializeField]
	private float _jointSpringUp = 2000f;

	// Token: 0x0400346C RID: 13420
	[SerializeField]
	private float _jointDamperUp = 10f;

	// Token: 0x0400346D RID: 13421
	[SerializeField]
	private float _jointSpringRight = 2000f;

	// Token: 0x0400346E RID: 13422
	[SerializeField]
	private float _jointDamperRight = 10f;

	// Token: 0x0400346F RID: 13423
	[SerializeField]
	private float _jointSpringMaxForce = 1E+23f;

	// Token: 0x04003470 RID: 13424
	[SerializeField]
	private float _jointMass = 0.5f;

	// Token: 0x04003471 RID: 13425
	[SerializeField]
	private float _jointBackForce = 1000f;

	// Token: 0x04003472 RID: 13426
	[SerializeField]
	private float _jointBackForceThresholdDistance = 0.001f;

	// Token: 0x04003473 RID: 13427
	[SerializeField]
	private float _jointBackForceMaxForce = 100f;

	// Token: 0x04003474 RID: 13428
	[SerializeField]
	private float _autoRadiusMultiplier = 1f;

	// Token: 0x04003475 RID: 13429
	[SerializeField]
	private bool _showUsedVerts = true;

	// Token: 0x04003476 RID: 13430
	public Transform[] ignoreColliders;

	// Token: 0x04003477 RID: 13431
	public AutoColliderGroup[] ignoreColliderGroups;

	// Token: 0x04003478 RID: 13432
	protected List<Collider> allPossibleIgnoreCollidersList;

	// Token: 0x04003479 RID: 13433
	protected List<Collider> ignoreCollidersList;

	// Token: 0x0400347A RID: 13434
	protected AutoCollider[] _allAutoColliders;
}
