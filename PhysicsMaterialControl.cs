using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D81 RID: 3457
public class PhysicsMaterialControl : JSONStorable
{
	// Token: 0x06006A7A RID: 27258 RVA: 0x00282980 File Offset: 0x00280D80
	public PhysicsMaterialControl()
	{
	}

	// Token: 0x06006A7B RID: 27259 RVA: 0x002829A0 File Offset: 0x00280DA0
	public void SyncColliders()
	{
		Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>(true);
		List<Collider> list = new List<Collider>();
		foreach (Collider collider in componentsInChildren)
		{
			PhysicsMaterialControl component = collider.GetComponent<PhysicsMaterialControl>();
			if ((component == null || component == this) && (collider.sharedMaterial == null || collider.sharedMaterial.name == "Default"))
			{
				collider.sharedMaterial = this.pMaterial;
				list.Add(collider);
			}
		}
		this.colliders = list.ToArray();
	}

	// Token: 0x06006A7C RID: 27260 RVA: 0x00282A46 File Offset: 0x00280E46
	protected void SyncDynamicFriction(float f)
	{
		this._dynamicFriction = f;
		if (this.pMaterial != null)
		{
			this.pMaterial.dynamicFriction = f;
		}
	}

	// Token: 0x17000FA2 RID: 4002
	// (get) Token: 0x06006A7D RID: 27261 RVA: 0x00282A6C File Offset: 0x00280E6C
	// (set) Token: 0x06006A7E RID: 27262 RVA: 0x00282A74 File Offset: 0x00280E74
	public float dynamicFriction
	{
		get
		{
			return this._dynamicFriction;
		}
		set
		{
			if (this.dynamicFrictionJSON != null)
			{
				this.dynamicFrictionJSON.val = value;
			}
			else
			{
				this.SyncDynamicFriction(value);
			}
		}
	}

	// Token: 0x06006A7F RID: 27263 RVA: 0x00282A99 File Offset: 0x00280E99
	protected void SyncStaticFriction(float f)
	{
		this._staticFriction = f;
		if (this.pMaterial != null)
		{
			this.pMaterial.staticFriction = f;
		}
	}

	// Token: 0x17000FA3 RID: 4003
	// (get) Token: 0x06006A80 RID: 27264 RVA: 0x00282ABF File Offset: 0x00280EBF
	// (set) Token: 0x06006A81 RID: 27265 RVA: 0x00282AC7 File Offset: 0x00280EC7
	public float staticFriction
	{
		get
		{
			return this._staticFriction;
		}
		set
		{
			if (this.staticFrictionJSON != null)
			{
				this.staticFrictionJSON.val = value;
			}
			else
			{
				this.SyncStaticFriction(value);
			}
		}
	}

	// Token: 0x06006A82 RID: 27266 RVA: 0x00282AEC File Offset: 0x00280EEC
	protected void SyncBounciness(float f)
	{
		this._bounciness = f;
		if (this.pMaterial != null)
		{
			this.pMaterial.bounciness = f;
		}
	}

	// Token: 0x17000FA4 RID: 4004
	// (get) Token: 0x06006A83 RID: 27267 RVA: 0x00282B12 File Offset: 0x00280F12
	// (set) Token: 0x06006A84 RID: 27268 RVA: 0x00282B1A File Offset: 0x00280F1A
	public float bounciness
	{
		get
		{
			return this._bounciness;
		}
		set
		{
			if (this.bouncinessJSON != null)
			{
				this.bouncinessJSON.val = value;
			}
			else
			{
				this.SyncBounciness(value);
			}
		}
	}

	// Token: 0x06006A85 RID: 27269 RVA: 0x00282B40 File Offset: 0x00280F40
	protected void SetFrictionCombine(string s)
	{
		if (this.pMaterial != null)
		{
			try
			{
				PhysicMaterialCombine physicMaterialCombine = (PhysicMaterialCombine)Enum.Parse(typeof(PhysicMaterialCombine), s);
				if (this.pMaterial.frictionCombine != physicMaterialCombine)
				{
					this.pMaterial.frictionCombine = physicMaterialCombine;
				}
			}
			catch (ArgumentException)
			{
				Debug.LogError("Attempted to set friction combine " + s + " which is not a valid value");
			}
		}
	}

	// Token: 0x06006A86 RID: 27270 RVA: 0x00282BC4 File Offset: 0x00280FC4
	protected void SetBounceCombine(string s)
	{
		if (this.pMaterial != null)
		{
			try
			{
				PhysicMaterialCombine physicMaterialCombine = (PhysicMaterialCombine)Enum.Parse(typeof(PhysicMaterialCombine), s);
				if (this.pMaterial.bounceCombine != physicMaterialCombine)
				{
					this.pMaterial.bounceCombine = physicMaterialCombine;
				}
			}
			catch (ArgumentException)
			{
				Debug.LogError("Attempted to set bounce combine " + s + " which is not a valid value");
			}
		}
	}

	// Token: 0x06006A87 RID: 27271 RVA: 0x00282C48 File Offset: 0x00281048
	protected void Init()
	{
		this.pMaterial = new PhysicMaterial();
		this.SyncColliders();
		this.SyncDynamicFriction(this._dynamicFriction);
		this.dynamicFrictionJSON = new JSONStorableFloat("dynamicFriction", this._dynamicFriction, new JSONStorableFloat.SetFloatCallback(this.SyncDynamicFriction), 0f, 1f, true, true);
		this.dynamicFrictionJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.dynamicFrictionJSON);
		this.SyncStaticFriction(this._staticFriction);
		this.staticFrictionJSON = new JSONStorableFloat("staticFriction", this._staticFriction, new JSONStorableFloat.SetFloatCallback(this.SyncStaticFriction), 0f, 1f, true, true);
		this.staticFrictionJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.staticFrictionJSON);
		this.SyncBounciness(this._bounciness);
		this.bouncinessJSON = new JSONStorableFloat("bounciness", this._bounciness, new JSONStorableFloat.SetFloatCallback(this.SyncBounciness), 0f, 1f, true, true);
		this.bouncinessJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.bouncinessJSON);
		string[] names = Enum.GetNames(typeof(PhysicMaterialCombine));
		List<string> choicesList = new List<string>(names);
		this.frictionCombineJSON = new JSONStorableStringChooser("frictionCombine", choicesList, this.pMaterial.frictionCombine.ToString(), "Friction Combine", new JSONStorableStringChooser.SetStringCallback(this.SetFrictionCombine));
		this.frictionCombineJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.frictionCombineJSON);
		this.bounceCombineJSON = new JSONStorableStringChooser("bounceCombine", choicesList, this.pMaterial.bounceCombine.ToString(), "Bounce Combine", new JSONStorableStringChooser.SetStringCallback(this.SetBounceCombine));
		this.bounceCombineJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.bounceCombineJSON);
	}

	// Token: 0x06006A88 RID: 27272 RVA: 0x00282E1C File Offset: 0x0028121C
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			PhysicsMaterialControlUI componentInChildren = this.UITransform.GetComponentInChildren<PhysicsMaterialControlUI>(true);
			if (componentInChildren != null)
			{
				this.dynamicFrictionJSON.slider = componentInChildren.dynamicFrictionSlider;
				this.staticFrictionJSON.slider = componentInChildren.staticFrictionSlider;
				this.bouncinessJSON.slider = componentInChildren.bouncinessSlider;
				this.frictionCombineJSON.popup = componentInChildren.frictionCombinePopup;
				this.bounceCombineJSON.popup = componentInChildren.bounceCombinePopup;
			}
		}
	}

	// Token: 0x06006A89 RID: 27273 RVA: 0x00282EA8 File Offset: 0x002812A8
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			PhysicsMaterialControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<PhysicsMaterialControlUI>(true);
			if (componentInChildren != null)
			{
				this.dynamicFrictionJSON.sliderAlt = componentInChildren.dynamicFrictionSlider;
				this.staticFrictionJSON.sliderAlt = componentInChildren.staticFrictionSlider;
				this.bouncinessJSON.sliderAlt = componentInChildren.bouncinessSlider;
				this.frictionCombineJSON.popupAlt = componentInChildren.frictionCombinePopup;
				this.bounceCombineJSON.popupAlt = componentInChildren.bounceCombinePopup;
			}
		}
	}

	// Token: 0x06006A8A RID: 27274 RVA: 0x00282F34 File Offset: 0x00281334
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x04005C7C RID: 23676
	protected PhysicMaterial pMaterial;

	// Token: 0x04005C7D RID: 23677
	protected Collider[] colliders;

	// Token: 0x04005C7E RID: 23678
	protected JSONStorableFloat dynamicFrictionJSON;

	// Token: 0x04005C7F RID: 23679
	[SerializeField]
	protected float _dynamicFriction = 0.6f;

	// Token: 0x04005C80 RID: 23680
	protected JSONStorableFloat staticFrictionJSON;

	// Token: 0x04005C81 RID: 23681
	[SerializeField]
	protected float _staticFriction = 0.6f;

	// Token: 0x04005C82 RID: 23682
	protected JSONStorableFloat bouncinessJSON;

	// Token: 0x04005C83 RID: 23683
	[SerializeField]
	protected float _bounciness;

	// Token: 0x04005C84 RID: 23684
	protected JSONStorableStringChooser frictionCombineJSON;

	// Token: 0x04005C85 RID: 23685
	protected JSONStorableStringChooser bounceCombineJSON;
}
