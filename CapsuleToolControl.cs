using System;
using UnityEngine;

// Token: 0x02000D51 RID: 3409
public class CapsuleToolControl : JSONStorable
{
	// Token: 0x060068C0 RID: 26816 RVA: 0x00273054 File Offset: 0x00271454
	public CapsuleToolControl()
	{
	}

	// Token: 0x060068C1 RID: 26817 RVA: 0x002730C0 File Offset: 0x002714C0
	protected void SyncMesh()
	{
		float val = this.lengthJSON.val;
		float val2 = this.radiusJSON.val;
		float num = val2 * 2f;
		float num2 = val2 * 4f;
		float num3 = val / 2f;
		float num4 = Mathf.Max(num3 - val2, 0f);
		if (this.cylinderMesh != null)
		{
			Vector3 localPosition;
			localPosition.x = 0f;
			localPosition.y = num4;
			localPosition.z = 0f;
			this.cylinderMesh.localPosition = localPosition;
			Vector3 localScale;
			localScale.x = num2;
			localScale.y = num2;
			localScale.z = num4 * 4f;
			this.cylinderMesh.localScale = localScale;
		}
		if (this.sphere1Mesh != null && this.sphere2Mesh != null)
		{
			Vector3 vector;
			vector.x = 0f;
			vector.y = num4;
			vector.z = 0f;
			this.sphere1Mesh.localPosition = vector;
			this.sphere2Mesh.localPosition = -vector;
			Vector3 localScale2;
			localScale2.x = num;
			localScale2.y = num;
			localScale2.z = num;
			this.sphere1Mesh.localScale = localScale2;
			this.sphere2Mesh.localScale = localScale2;
		}
	}

	// Token: 0x060068C2 RID: 26818 RVA: 0x0027320D File Offset: 0x0027160D
	protected void SyncRadius(float f)
	{
		if (this.toolCollider != null)
		{
			this.toolCollider.radius = f;
		}
		this.SyncMesh();
	}

	// Token: 0x060068C3 RID: 26819 RVA: 0x00273232 File Offset: 0x00271632
	protected void SyncLength(float f)
	{
		if (this.toolCollider != null)
		{
			this.toolCollider.height = f;
		}
		this.SyncMesh();
	}

	// Token: 0x060068C4 RID: 26820 RVA: 0x00273257 File Offset: 0x00271657
	protected void SetQuickSize1()
	{
		this.radiusJSON.val = this.quickSize1Radius;
		this.lengthJSON.val = this.quickSize1Length;
	}

	// Token: 0x060068C5 RID: 26821 RVA: 0x0027327B File Offset: 0x0027167B
	protected void SetQuickSize2()
	{
		this.radiusJSON.val = this.quickSize2Radius;
		this.lengthJSON.val = this.quickSize2Length;
	}

	// Token: 0x060068C6 RID: 26822 RVA: 0x0027329F File Offset: 0x0027169F
	protected void SetQuickSize3()
	{
		this.radiusJSON.val = this.quickSize3Radius;
		this.lengthJSON.val = this.quickSize3Length;
	}

	// Token: 0x060068C7 RID: 26823 RVA: 0x002732C3 File Offset: 0x002716C3
	protected void SetQuickSize4()
	{
		this.radiusJSON.val = this.quickSize4Radius;
		this.lengthJSON.val = this.quickSize4Length;
	}

	// Token: 0x060068C8 RID: 26824 RVA: 0x002732E8 File Offset: 0x002716E8
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (this.wasInit)
		{
			base.InitUI(t, isAlt);
			if (t != null)
			{
				CapsuleToolControlUI componentInChildren = this.UITransform.GetComponentInChildren<CapsuleToolControlUI>();
				if (componentInChildren != null)
				{
					this.radiusJSON.RegisterSlider(componentInChildren.radiusSlider, isAlt);
					this.lengthJSON.RegisterSlider(componentInChildren.lengthSlider, isAlt);
					this.SetQuickSize1Action.RegisterButton(componentInChildren.setQuickSize1Button, isAlt);
					this.SetQuickSize2Action.RegisterButton(componentInChildren.setQuickSize2Button, isAlt);
					this.SetQuickSize3Action.RegisterButton(componentInChildren.setQuickSize3Button, isAlt);
					this.SetQuickSize4Action.RegisterButton(componentInChildren.setQuickSize4Button, isAlt);
				}
			}
		}
	}

	// Token: 0x060068C9 RID: 26825 RVA: 0x00273398 File Offset: 0x00271798
	protected virtual void Init()
	{
		this.wasInit = true;
		this.radiusJSON = new JSONStorableFloat("radius", this.toolCollider.radius, new JSONStorableFloat.SetFloatCallback(this.SyncRadius), 0.01f, 10f, true, true);
		base.RegisterFloat(this.radiusJSON);
		this.lengthJSON = new JSONStorableFloat("length", this.toolCollider.height, new JSONStorableFloat.SetFloatCallback(this.SyncLength), 0.01f, 20f, true, true);
		base.RegisterFloat(this.lengthJSON);
		this.SetQuickSize1Action = new JSONStorableAction("SetQuickSize1", new JSONStorableAction.ActionCallback(this.SetQuickSize1));
		base.RegisterAction(this.SetQuickSize1Action);
		this.SetQuickSize2Action = new JSONStorableAction("SetQuickSize2", new JSONStorableAction.ActionCallback(this.SetQuickSize2));
		base.RegisterAction(this.SetQuickSize2Action);
		this.SetQuickSize3Action = new JSONStorableAction("SetQuickSize3", new JSONStorableAction.ActionCallback(this.SetQuickSize3));
		base.RegisterAction(this.SetQuickSize3Action);
		this.SetQuickSize4Action = new JSONStorableAction("SetQuickSize4", new JSONStorableAction.ActionCallback(this.SetQuickSize4));
		base.RegisterAction(this.SetQuickSize4Action);
	}

	// Token: 0x060068CA RID: 26826 RVA: 0x002734CA File Offset: 0x002718CA
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

	// Token: 0x0400597A RID: 22906
	public CapsuleCollider toolCollider;

	// Token: 0x0400597B RID: 22907
	public Transform cylinderMesh;

	// Token: 0x0400597C RID: 22908
	public Transform sphere1Mesh;

	// Token: 0x0400597D RID: 22909
	public Transform sphere2Mesh;

	// Token: 0x0400597E RID: 22910
	public float quickSize1Radius = 0.25f;

	// Token: 0x0400597F RID: 22911
	public float quickSize1Length = 3f;

	// Token: 0x04005980 RID: 22912
	public float quickSize2Radius = 0.5f;

	// Token: 0x04005981 RID: 22913
	public float quickSize2Length = 3f;

	// Token: 0x04005982 RID: 22914
	public float quickSize3Radius = 1f;

	// Token: 0x04005983 RID: 22915
	public float quickSize3Length = 6f;

	// Token: 0x04005984 RID: 22916
	public float quickSize4Radius = 2f;

	// Token: 0x04005985 RID: 22917
	public float quickSize4Length = 8f;

	// Token: 0x04005986 RID: 22918
	protected JSONStorableFloat radiusJSON;

	// Token: 0x04005987 RID: 22919
	protected JSONStorableFloat lengthJSON;

	// Token: 0x04005988 RID: 22920
	protected JSONStorableAction SetQuickSize1Action;

	// Token: 0x04005989 RID: 22921
	protected JSONStorableAction SetQuickSize2Action;

	// Token: 0x0400598A RID: 22922
	protected JSONStorableAction SetQuickSize3Action;

	// Token: 0x0400598B RID: 22923
	protected JSONStorableAction SetQuickSize4Action;

	// Token: 0x0400598C RID: 22924
	protected bool wasInit;
}
