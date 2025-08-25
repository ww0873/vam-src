using System;
using UnityEngine;

// Token: 0x02000AA5 RID: 2725
public class CharacterPoseSnapRestore : JSONStorable
{
	// Token: 0x06004730 RID: 18224 RVA: 0x0014BD5A File Offset: 0x0014A15A
	public CharacterPoseSnapRestore()
	{
	}

	// Token: 0x06004731 RID: 18225 RVA: 0x0014BD62 File Offset: 0x0014A162
	public void ForceSnapRestore()
	{
		if (!base.physicalLocked)
		{
			if (this.dazBones != null)
			{
				this.dazBones.SnapAllBonesToControls();
			}
			this.containingAtom.ResetPhysics(false, true);
		}
	}

	// Token: 0x06004732 RID: 18226 RVA: 0x0014BD98 File Offset: 0x0014A198
	public void ResetPhysicsOnly()
	{
		this.containingAtom.ResetPhysics(false, true);
	}

	// Token: 0x06004733 RID: 18227 RVA: 0x0014BDA8 File Offset: 0x0014A1A8
	public void SnapRestore()
	{
		if (!base.physicalLocked && this.enabledJSON != null && this.enabledJSON.val)
		{
			if (this.dazBones != null)
			{
				this.dazBones.SnapAllBonesToControls();
			}
			this.containingAtom.ResetPhysics(false, true);
		}
	}

	// Token: 0x06004734 RID: 18228 RVA: 0x0014BE04 File Offset: 0x0014A204
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			CharacterPoseSnapRestoreUI componentInChildren = t.GetComponentInChildren<CharacterPoseSnapRestoreUI>(true);
			if (componentInChildren != null)
			{
				this.enabledJSON.toggle = componentInChildren.enabledToggle;
			}
		}
	}

	// Token: 0x06004735 RID: 18229 RVA: 0x0014BE42 File Offset: 0x0014A242
	protected void Init()
	{
		this.enabledJSON = new JSONStorableBool("enabled", true);
		this.enabledJSON.isStorable = false;
		this.enabledJSON.isRestorable = false;
		base.RegisterBool(this.enabledJSON);
	}

	// Token: 0x06004736 RID: 18230 RVA: 0x0014BE79 File Offset: 0x0014A279
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

	// Token: 0x0400347B RID: 13435
	public DAZBones dazBones;

	// Token: 0x0400347C RID: 13436
	protected JSONStorableBool enabledJSON;
}
