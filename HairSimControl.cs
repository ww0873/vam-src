using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using GPUTools.Hair.Scripts;
using GPUTools.Hair.Scripts.Geometry.Create;
using GPUTools.Hair.Scripts.Types;
using MeshVR;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D6C RID: 3436
public class HairSimControl : PhysicsSimulatorJSONStorable, RenderSuspend
{
	// Token: 0x0600699C RID: 27036 RVA: 0x00278B36 File Offset: 0x00276F36
	public HairSimControl()
	{
	}

	// Token: 0x17000F99 RID: 3993
	// (get) Token: 0x0600699D RID: 27037 RVA: 0x00278B45 File Offset: 0x00276F45
	// (set) Token: 0x0600699E RID: 27038 RVA: 0x00278B50 File Offset: 0x00276F50
	public bool renderSuspend
	{
		get
		{
			return this._renderSuspend;
		}
		set
		{
			if (this._renderSuspend != value)
			{
				this._renderSuspend = value;
				if (this.hairSettings != null && this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.render != null)
				{
					MeshRenderer component = this.hairSettings.HairBuidCommand.render.GetComponent<MeshRenderer>();
					if (component != null)
					{
						component.enabled = !this._renderSuspend;
					}
				}
			}
		}
	}

	// Token: 0x0600699F RID: 27039 RVA: 0x00278BE0 File Offset: 0x00276FE0
	protected void UpdateAllHairSettings(bool keepParticlePositions = false)
	{
		if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particles != null)
		{
			if (keepParticlePositions)
			{
				this.hairSettings.HairBuidCommand.particles.UpdateParticleRadius();
				this.hairSettings.HairBuidCommand.particles.SaveGPUState();
			}
			this.hairSettings.HairBuidCommand.particles.UpdateSettings();
		}
		if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.distanceJoints != null)
		{
			this.hairSettings.HairBuidCommand.distanceJoints.UpdateSettings();
		}
		if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.compressionJoints != null)
		{
			this.hairSettings.HairBuidCommand.compressionJoints.UpdateSettings();
		}
		if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particles != null && keepParticlePositions)
		{
			this.hairSettings.HairBuidCommand.particles.RestoreGPUState();
		}
	}

	// Token: 0x060069A0 RID: 27040 RVA: 0x00278D0C File Offset: 0x0027710C
	public override void ScaleChanged(float scale)
	{
		base.ScaleChanged(scale);
		if (this.hairSettings != null)
		{
			if (this.hairSettings.PhysicsSettings != null)
			{
				this.hairSettings.PhysicsSettings.WorldScale = scale;
			}
			this.UpdateAllHairSettings(false);
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.physics != null)
			{
				this.hairSettings.HairBuidCommand.physics.PartialResetPhysics();
			}
		}
	}

	// Token: 0x060069A1 RID: 27041 RVA: 0x00278D99 File Offset: 0x00277199
	public void Rebuild()
	{
		if (this.hairSettings != null && this.hairSettings.HairBuidCommand != null)
		{
			this.hairSettings.HairBuidCommand.RebuildHair();
		}
	}

	// Token: 0x060069A2 RID: 27042 RVA: 0x00278DCC File Offset: 0x002771CC
	public void SyncStyleText()
	{
		if (this.creator != null && this.simNearbyJointCountText != null)
		{
			List<Vector4ListContainer> nearbyVertexGroups = this.creator.GetNearbyVertexGroups();
			int num = 0;
			if (nearbyVertexGroups != null)
			{
				IEnumerable<Vector4ListContainer> source = nearbyVertexGroups;
				if (HairSimControl.<>f__am$cache0 == null)
				{
					HairSimControl.<>f__am$cache0 = new Func<Vector4ListContainer, int>(HairSimControl.<SyncStyleText>m__0);
				}
				num = source.Sum(HairSimControl.<>f__am$cache0);
			}
			this.simNearbyJointCountText.text = num.ToString();
			if (this.rebuildStyleJointsAction.button != null)
			{
				if (num == 0)
				{
					this.rebuildStyleJointsAction.button.image.color = Color.yellow;
				}
				else
				{
					this.rebuildStyleJointsAction.button.image.color = Color.gray;
				}
			}
		}
	}

	// Token: 0x060069A3 RID: 27043 RVA: 0x00278EA0 File Offset: 0x002772A0
	protected void SyncStyleModeButtons()
	{
		if (this.styleModelPanel != null)
		{
			this.styleModelPanel.gameObject.SetActive(this._styleMode);
		}
		if (this.resetAndStartStyleModeAction.button != null)
		{
			this.resetAndStartStyleModeAction.button.interactable = !this._styleMode;
		}
		if (this.startStyleModeAction.button != null)
		{
			this.startStyleModeAction.button.interactable = !this._styleMode;
		}
		if (this.cancelStyleModeAction.button != null)
		{
			this.cancelStyleModeAction.button.interactable = this._styleMode;
		}
		if (this.keepStyleAction.button != null)
		{
			this.keepStyleAction.button.interactable = this._styleMode;
		}
		if (this.rebuildStyleJointsAction.button != null)
		{
			this.rebuildStyleJointsAction.button.interactable = !this._styleMode;
		}
	}

	// Token: 0x060069A4 RID: 27044 RVA: 0x00278FBC File Offset: 0x002773BC
	public void StartStyleMode(bool reset = false)
	{
		if (this.hairSettings != null)
		{
			this._styleMode = true;
			this.SyncStyleModeButtons();
			this.SyncStyleToolVisibility();
			this.hairSettings.PhysicsSettings.StyleMode = true;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.pointJoints != null)
			{
				this.hairSettings.HairBuidCommand.pointJoints.UpdateSettings();
			}
			if (reset)
			{
				if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particles != null)
				{
					this.hairSettings.HairBuidCommand.particles.UpdateSettings();
					this.hairSettings.HairBuidCommand.physics.ResetPhysics();
				}
			}
			else if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particles != null)
			{
				this.hairSettings.HairBuidCommand.particles.UpdateParticleRadius();
			}
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
			if (SuperController.singleton != null)
			{
				SuperController.singleton.SelectModeCustomWithTargetControl("Use tools to style hair. Save and cancel when done");
				SuperController.singleton.DisableRemoteHoldGrab();
			}
		}
	}

	// Token: 0x060069A5 RID: 27045 RVA: 0x0027912F File Offset: 0x0027752F
	protected void StartStyleModeWithReset()
	{
		this.StartStyleMode(true);
	}

	// Token: 0x060069A6 RID: 27046 RVA: 0x00279138 File Offset: 0x00277538
	protected void StartStyleModeWithoutReset()
	{
		this.StartStyleMode(false);
	}

	// Token: 0x060069A7 RID: 27047 RVA: 0x00279144 File Offset: 0x00277544
	public void CancelStyleMode()
	{
		if (this.hairSettings != null && this._styleMode)
		{
			this._styleMode = false;
			this.SyncStyleModeButtons();
			this.SyncStyleToolVisibility();
			this.hairSettings.PhysicsSettings.StyleMode = false;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.pointJoints != null)
			{
				this.hairSettings.HairBuidCommand.pointJoints.UpdateSettings();
			}
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particles != null)
			{
				this.hairSettings.HairBuidCommand.particles.UpdateParticleRadius();
			}
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.distanceJoints != null)
			{
				this.hairSettings.HairBuidCommand.distanceJoints.UpdateSettings();
			}
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.compressionJoints != null)
			{
				this.hairSettings.HairBuidCommand.compressionJoints.UpdateSettings();
			}
			if (SuperController.singleton != null)
			{
				SuperController.singleton.SelectModeOff();
				SuperController.singleton.EnableRemoteHoldGrab();
			}
		}
	}

	// Token: 0x060069A8 RID: 27048 RVA: 0x002792D8 File Offset: 0x002776D8
	public void KeepStyle()
	{
		if (this.hairSettings != null)
		{
			this._styleMode = false;
			this.SyncStyleModeButtons();
			this.SyncStyleToolVisibility();
			this.hairSettings.PhysicsSettings.StyleMode = false;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.pointJoints != null)
			{
				this.hairSettings.HairBuidCommand.pointJoints.RebuildFromGPUData();
			}
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
			this.UpdateAllHairSettings(true);
			if (this.rebuildStyleJointsAction.button != null)
			{
				this.rebuildStyleJointsAction.button.image.color = Color.yellow;
			}
			if (SuperController.singleton != null)
			{
				SuperController.singleton.SelectModeOff();
				SuperController.singleton.EnableRemoteHoldGrab();
			}
		}
	}

	// Token: 0x060069A9 RID: 27049 RVA: 0x002793E9 File Offset: 0x002777E9
	protected void SyncStyleModeAllowControlOtherNodes(bool b)
	{
		this.SyncStyleToolVisibility();
	}

	// Token: 0x060069AA RID: 27050 RVA: 0x002793F1 File Offset: 0x002777F1
	public void SyncStyleJointsSearchDistance(float f)
	{
		if (this.creator != null)
		{
			this.creator.NearbyVertexSearchDistance = f;
		}
	}

	// Token: 0x060069AB RID: 27051 RVA: 0x00279410 File Offset: 0x00277810
	public void ClearStyleJoints()
	{
		if (this.creator != null)
		{
			this.creator.ClearNearbyVertexGroups();
			if (this.hairSettings != null)
			{
				this.Rebuild();
			}
			this.SyncStyleText();
		}
	}

	// Token: 0x060069AC RID: 27052 RVA: 0x0027944C File Offset: 0x0027784C
	protected void AbortRebuildStyleJointsThread(bool wait = true)
	{
		if (this.rebuildStyleJointsThread != null && this.rebuildStyleJointsThread.IsAlive)
		{
			this.creator.CancelCalculateNearbyVertexGroups();
			if (wait)
			{
				while (this.rebuildStyleJointsThread.IsAlive)
				{
					Thread.Sleep(0);
				}
			}
		}
	}

	// Token: 0x060069AD RID: 27053 RVA: 0x002794A0 File Offset: 0x002778A0
	protected void RebuildStyleJointsThreaded()
	{
		this.creator.CalculateNearbyVertexGroups();
	}

	// Token: 0x060069AE RID: 27054 RVA: 0x002794B0 File Offset: 0x002778B0
	protected IEnumerator RebuildStyleJointsCo()
	{
		yield return null;
		this.threadError = null;
		this.creator.PrepareCalculateNearbyVertexGroups();
		this.rebuildStyleJointsThread = new Thread(new ThreadStart(this.RebuildStyleJointsThreaded));
		this.rebuildStyleJointsThread.Start();
		while (this.rebuildStyleJointsThread.IsAlive)
		{
			if (this.styleStatusText != null)
			{
				this.styleStatusText.text = this.creator.status;
			}
			yield return null;
		}
		if (this.styleStatusText != null)
		{
			this.styleStatusText.text = this.creator.status;
		}
		if (this.hairSettings != null)
		{
			this.Rebuild();
		}
		this.SyncStyleText();
		this.isRebuildingStyleJoints = false;
		yield break;
	}

	// Token: 0x060069AF RID: 27055 RVA: 0x002794CB File Offset: 0x002778CB
	public void RebuildStyleJoints()
	{
		if (!this.isRebuildingStyleJoints)
		{
			this.isRebuildingStyleJoints = true;
			base.StartCoroutine(this.RebuildStyleJointsCo());
		}
	}

	// Token: 0x060069B0 RID: 27056 RVA: 0x002794EC File Offset: 0x002778EC
	public void SyncStyleModeGravityMultiplier(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.StyleModeGravityMultiplier = f;
		}
	}

	// Token: 0x060069B1 RID: 27057 RVA: 0x00279510 File Offset: 0x00277910
	public void SyncStyleModeShowCurls(bool b)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.StyleModeShowCurls = b;
		}
	}

	// Token: 0x060069B2 RID: 27058 RVA: 0x00279534 File Offset: 0x00277934
	public void SyncStyleModeUpHairPullStrength(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.ReverseSplineJointPower = f;
		}
	}

	// Token: 0x060069B3 RID: 27059 RVA: 0x00279558 File Offset: 0x00277958
	public void SyncStyleModeCollisionRadius(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.StyleModeStrandRadius = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particles != null)
			{
				this.hairSettings.HairBuidCommand.particles.UpdateParticleRadius();
			}
		}
	}

	// Token: 0x060069B4 RID: 27060 RVA: 0x002795C4 File Offset: 0x002779C4
	public void SyncStyleModeCollisionRadiusRoot(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.StyleModeStrandRootRadius = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particles != null)
			{
				this.hairSettings.HairBuidCommand.particles.UpdateParticleRadius();
			}
		}
	}

	// Token: 0x17000F9A RID: 3994
	// (get) Token: 0x060069B5 RID: 27061 RVA: 0x0027962D File Offset: 0x00277A2D
	protected HairSimControlTools hairSimControlTools
	{
		get
		{
			if (this._hairSimControlTools == null)
			{
				this._hairSimControlTools = base.GetComponentInParent<HairSimControlTools>();
			}
			return this._hairSimControlTools;
		}
	}

	// Token: 0x060069B6 RID: 27062 RVA: 0x00279654 File Offset: 0x00277A54
	protected void SyncStyleToolVisibility()
	{
		HairSimControlTools hairSimControlTools = this.hairSimControlTools;
		if (hairSimControlTools != null)
		{
			hairSimControlTools.SetHairStyleToolVisibility(this._styleMode && this.styleModeShowTool1JSON.val, this._styleMode && this.styleModeShowTool2JSON.val, this._styleMode && this.styleModeShowTool3JSON.val, this._styleMode && this.styleModeShowTool4JSON.val);
			hairSimControlTools.SetOnlyToolsControllable(this._styleMode && !this.styleModeAllowControlOtherNodesJSON.val);
		}
	}

	// Token: 0x060069B7 RID: 27063 RVA: 0x00279700 File Offset: 0x00277B00
	protected void SyncShowStyleTool1(bool b)
	{
		this.SyncStyleToolVisibility();
	}

	// Token: 0x060069B8 RID: 27064 RVA: 0x00279708 File Offset: 0x00277B08
	protected void SyncShowStyleTool2(bool b)
	{
		this.SyncStyleToolVisibility();
	}

	// Token: 0x060069B9 RID: 27065 RVA: 0x00279710 File Offset: 0x00277B10
	protected void SyncShowStyleTool3(bool b)
	{
		this.SyncStyleToolVisibility();
	}

	// Token: 0x060069BA RID: 27066 RVA: 0x00279718 File Offset: 0x00277B18
	protected void SyncShowStyleTool4(bool b)
	{
		this.SyncStyleToolVisibility();
	}

	// Token: 0x060069BB RID: 27067 RVA: 0x00279720 File Offset: 0x00277B20
	public void CopyPhysicsParameters()
	{
		HairSimControl.hasPhysicsCopyData = true;
		HairSimControl.copiedCollisionEnabled = this.collisionEnabledJSON.val;
		HairSimControl.copiedCollisionRadius = this.collisionRadiusJSON.val;
		HairSimControl.copiedCollisionRadiusRoot = this.collisionRadiusRootJSON.val;
		HairSimControl.copiedDrag = this.dragJSON.val;
		HairSimControl.copiedFriction = this.frictionJSON.val;
		HairSimControl.copiedGravityMultiplier = this.gravityMultiplierJSON.val;
		HairSimControl.copiedWeight = this.weightJSON.val;
		HairSimControl.copiedIterations = this.iterationsJSON.val;
		HairSimControl.copiedRootRigidity = this.rootRigidityJSON.val;
		HairSimControl.copiedMainRigidity = this.mainRigidityJSON.val;
		HairSimControl.copiedTipRigidity = this.tipRigidityJSON.val;
		HairSimControl.copiedRigidityRolloffPower = this.rigidityRolloffPowerJSON.val;
		if (this.jointRigidityJSON != null)
		{
			HairSimControl.copiedJointRigidity = this.jointRigidityJSON.val;
		}
		HairSimControl.copiedCling = this.clingJSON.val;
		HairSimControl.copiedClingRolloff = this.clingRolloffJSON.val;
		HairSimControl.copiedSnap = this.snapJSON.val;
		HairSimControl.copiedBendResistance = this.bendResistanceJSON.val;
	}

	// Token: 0x060069BC RID: 27068 RVA: 0x00279850 File Offset: 0x00277C50
	public void PastePhysicsParameters()
	{
		if (HairSimControl.hasPhysicsCopyData)
		{
			this.hasPhysicsPasteData = true;
			this.pasteUndoCollisionEnabled = this.collisionEnabledJSON.val;
			this.pasteUndoCollisionRadius = this.collisionRadiusJSON.val;
			this.pasteUndoCollisionRadiusRoot = this.collisionRadiusRootJSON.val;
			this.pasteUndoDrag = this.dragJSON.val;
			this.pasteUndoFriction = this.frictionJSON.val;
			this.pasteUndoGravityMultiplier = this.gravityMultiplierJSON.val;
			this.pasteUndoWeight = this.weightJSON.val;
			this.pasteUndoIterations = this.iterationsJSON.val;
			this.pasteUndoRootRigidity = this.rootRigidityJSON.val;
			this.pasteUndoMainRigidity = this.mainRigidityJSON.val;
			this.pasteUndoTipRigidity = this.tipRigidityJSON.val;
			this.pasteUndoRigidityRolloffPower = this.rigidityRolloffPowerJSON.val;
			if (this.jointRigidityJSON != null)
			{
				this.pasteUndoJointRigidity = this.jointRigidityJSON.val;
			}
			this.pasteUndoCling = this.clingJSON.val;
			this.pasteUndoClingRolloff = this.clingRolloffJSON.val;
			this.pasteUndoSnap = this.snapJSON.val;
			this.pasteUndoBendResistance = this.bendResistanceJSON.val;
			this.collisionEnabledJSON.val = HairSimControl.copiedCollisionEnabled;
			this.collisionRadiusJSON.val = HairSimControl.copiedCollisionRadius;
			this.collisionRadiusRootJSON.val = HairSimControl.copiedCollisionRadiusRoot;
			this.dragJSON.val = HairSimControl.copiedDrag;
			this.frictionJSON.val = HairSimControl.copiedFriction;
			this.gravityMultiplierJSON.val = HairSimControl.copiedGravityMultiplier;
			this.weightJSON.val = HairSimControl.copiedWeight;
			this.iterationsJSON.val = HairSimControl.copiedIterations;
			this.rootRigidityJSON.val = HairSimControl.copiedRootRigidity;
			this.mainRigidityJSON.val = HairSimControl.copiedMainRigidity;
			this.tipRigidityJSON.val = HairSimControl.copiedTipRigidity;
			this.rigidityRolloffPowerJSON.val = HairSimControl.copiedRigidityRolloffPower;
			if (this.jointRigidityJSON != null)
			{
				this.jointRigidityJSON.val = HairSimControl.copiedJointRigidity;
			}
			this.clingJSON.val = HairSimControl.copiedCling;
			this.clingRolloffJSON.val = HairSimControl.copiedClingRolloff;
			this.snapJSON.val = HairSimControl.copiedSnap;
			this.bendResistanceJSON.val = HairSimControl.copiedBendResistance;
		}
	}

	// Token: 0x060069BD RID: 27069 RVA: 0x00279AB8 File Offset: 0x00277EB8
	public void UndoPastePhysicsParameters()
	{
		if (this.hasPhysicsPasteData)
		{
			this.collisionEnabledJSON.val = this.pasteUndoCollisionEnabled;
			this.collisionRadiusJSON.val = this.pasteUndoCollisionRadius;
			this.collisionRadiusRootJSON.val = this.pasteUndoCollisionRadiusRoot;
			this.dragJSON.val = this.pasteUndoDrag;
			this.frictionJSON.val = this.pasteUndoFriction;
			this.gravityMultiplierJSON.val = this.pasteUndoGravityMultiplier;
			this.weightJSON.val = this.pasteUndoWeight;
			this.iterationsJSON.val = this.pasteUndoIterations;
			this.rootRigidityJSON.val = this.pasteUndoRootRigidity;
			this.mainRigidityJSON.val = this.pasteUndoMainRigidity;
			this.tipRigidityJSON.val = this.pasteUndoTipRigidity;
			this.rigidityRolloffPowerJSON.val = this.pasteUndoRigidityRolloffPower;
			if (this.jointRigidityJSON != null)
			{
				this.jointRigidityJSON.val = this.pasteUndoJointRigidity;
			}
			this.clingJSON.val = this.pasteUndoCling;
			this.clingRolloffJSON.val = this.pasteUndoClingRolloff;
			this.snapJSON.val = this.pasteUndoSnap;
			this.bendResistanceJSON.val = this.pasteUndoBendResistance;
			this.hasPhysicsPasteData = false;
		}
	}

	// Token: 0x060069BE RID: 27070 RVA: 0x00279C03 File Offset: 0x00278003
	public void SyncSimulationEnabled(bool b)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.IsEnabled = b;
		}
	}

	// Token: 0x060069BF RID: 27071 RVA: 0x00279C28 File Offset: 0x00278028
	protected override void SyncCollisionEnabled()
	{
		bool flag = true;
		if (this.collisionEnabledJSON != null)
		{
			flag = this.collisionEnabledJSON.val;
		}
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.IsCollisionEnabled = (this._collisionEnabled && flag);
		}
	}

	// Token: 0x060069C0 RID: 27072 RVA: 0x00279C7E File Offset: 0x0027807E
	public void SyncCollisionEnabled(bool b)
	{
		this.SyncCollisionEnabled();
	}

	// Token: 0x060069C1 RID: 27073 RVA: 0x00279C88 File Offset: 0x00278088
	public void SyncCollisionRadius(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.StandRadius = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particles != null)
			{
				this.hairSettings.HairBuidCommand.particles.UpdateParticleRadius();
			}
		}
	}

	// Token: 0x060069C2 RID: 27074 RVA: 0x00279CF4 File Offset: 0x002780F4
	public void SyncCollisionRadiusRoot(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.StandRootRadius = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particles != null)
			{
				this.hairSettings.HairBuidCommand.particles.UpdateParticleRadius();
			}
		}
	}

	// Token: 0x060069C3 RID: 27075 RVA: 0x00279D5D File Offset: 0x0027815D
	public void SyncDrag(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.Drag = f;
		}
	}

	// Token: 0x060069C4 RID: 27076 RVA: 0x00279D81 File Offset: 0x00278181
	public void SyncFriction(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.Friction = f;
		}
	}

	// Token: 0x060069C5 RID: 27077 RVA: 0x00279DA5 File Offset: 0x002781A5
	public void SyncGravityMultiplier(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.GravityMultiplier = f;
		}
	}

	// Token: 0x060069C6 RID: 27078 RVA: 0x00279DC9 File Offset: 0x002781C9
	public void SyncWeight(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.Weight = f;
		}
	}

	// Token: 0x060069C7 RID: 27079 RVA: 0x00279DED File Offset: 0x002781ED
	public void SyncIterations(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.Iterations = Mathf.FloorToInt(f);
		}
	}

	// Token: 0x060069C8 RID: 27080 RVA: 0x00279E18 File Offset: 0x00278218
	public void SyncUsePaintedRigidity(bool b)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.UsePaintedRigidity = b;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.pointJoints != null)
			{
				this.hairSettings.HairBuidCommand.pointJoints.UpdateSettings();
			}
		}
		if (this.rootRigidityJSON.slider != null)
		{
			this.rootRigidityJSON.slider.gameObject.SetActive(!b);
		}
		if (this.rootRigidityJSON.sliderAlt != null)
		{
			this.rootRigidityJSON.sliderAlt.gameObject.SetActive(!b);
		}
		if (this.mainRigidityJSON.slider != null)
		{
			this.mainRigidityJSON.slider.gameObject.SetActive(!b);
		}
		if (this.mainRigidityJSON.sliderAlt != null)
		{
			this.mainRigidityJSON.sliderAlt.gameObject.SetActive(!b);
		}
		if (this.tipRigidityJSON.slider != null)
		{
			this.tipRigidityJSON.slider.gameObject.SetActive(!b);
		}
		if (this.tipRigidityJSON.sliderAlt != null)
		{
			this.tipRigidityJSON.sliderAlt.gameObject.SetActive(!b);
		}
		if (this.rigidityRolloffPowerJSON.slider != null)
		{
			this.rigidityRolloffPowerJSON.slider.gameObject.SetActive(!b);
		}
		if (this.rigidityRolloffPowerJSON.sliderAlt != null)
		{
			this.rigidityRolloffPowerJSON.sliderAlt.gameObject.SetActive(!b);
		}
		if (this.paintedRigidityIndicatorPanel != null)
		{
			this.paintedRigidityIndicatorPanel.gameObject.SetActive(b);
		}
		HairSimControlTools hairSimControlTools = this.hairSimControlTools;
		if (hairSimControlTools != null)
		{
			hairSimControlTools.SetAllowRigidityPaint(b);
		}
	}

	// Token: 0x060069C9 RID: 27081 RVA: 0x0027A038 File Offset: 0x00278438
	public void SyncRootRigidity(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.RootRigidity = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.pointJoints != null)
			{
				this.hairSettings.HairBuidCommand.pointJoints.UpdateSettings();
			}
		}
	}

	// Token: 0x060069CA RID: 27082 RVA: 0x0027A0A4 File Offset: 0x002784A4
	public void SyncMainRigidity(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.MainRigidity = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.pointJoints != null)
			{
				this.hairSettings.HairBuidCommand.pointJoints.UpdateSettings();
			}
		}
	}

	// Token: 0x060069CB RID: 27083 RVA: 0x0027A110 File Offset: 0x00278510
	public void SyncTipRigidity(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.TipRigidity = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.pointJoints != null)
			{
				this.hairSettings.HairBuidCommand.pointJoints.UpdateSettings();
			}
		}
	}

	// Token: 0x060069CC RID: 27084 RVA: 0x0027A17C File Offset: 0x0027857C
	public void SyncRigidityRolloffPower(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.RigidityRolloffPower = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.pointJoints != null)
			{
				this.hairSettings.HairBuidCommand.pointJoints.UpdateSettings();
			}
		}
	}

	// Token: 0x060069CD RID: 27085 RVA: 0x0027A1E8 File Offset: 0x002785E8
	public void SyncJointRigidity(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.JointRigidity = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.pointJoints != null)
			{
				this.hairSettings.HairBuidCommand.pointJoints.UpdateSettings();
			}
		}
	}

	// Token: 0x060069CE RID: 27086 RVA: 0x0027A251 File Offset: 0x00278651
	public void SyncCling(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.NearbyJointPower = f;
		}
	}

	// Token: 0x060069CF RID: 27087 RVA: 0x0027A275 File Offset: 0x00278675
	public void SyncClingRolloff(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.NearbyJointPowerRolloff = f;
		}
	}

	// Token: 0x060069D0 RID: 27088 RVA: 0x0027A299 File Offset: 0x00278699
	public void SyncSnap(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.SplineJointPower = f;
		}
	}

	// Token: 0x060069D1 RID: 27089 RVA: 0x0027A2BD File Offset: 0x002786BD
	public void SyncBendResistance(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.PhysicsSettings.CompressionJointPower = f;
		}
	}

	// Token: 0x060069D2 RID: 27090 RVA: 0x0027A2E1 File Offset: 0x002786E1
	protected void SyncWind()
	{
		if (this.windJSON != null)
		{
			this.SyncWind(this.windJSON.val);
		}
	}

	// Token: 0x060069D3 RID: 27091 RVA: 0x0027A2FF File Offset: 0x002786FF
	protected void SyncWind(Vector3 v)
	{
		if (this.hairSettings != null && this.hairSettings.RuntimeData != null)
		{
			this.hairSettings.RuntimeData.Wind = v + WindControl.globalWind;
		}
	}

	// Token: 0x060069D4 RID: 27092 RVA: 0x0027A340 File Offset: 0x00278740
	public void CopyLightingParameters()
	{
		HairSimControl.hasLightingCopyData = true;
		HairSimControl.copiedShaderType = this.shaderTypeJSON.val;
		HairSimControl.copiedRootColor = this.rootColorJSON.val;
		HairSimControl.copiedTipColor = this.tipColorJSON.val;
		HairSimControl.copiedColorRolloff = this.colorRolloffJSON.val;
		HairSimControl.copiedSpecularColor = this.specularColorJSON.val;
		HairSimControl.copiedDiffuseSoftness = this.diffuseSoftnessJSON.val;
		HairSimControl.copiedPrimarySpecularSharpness = this.primarySpecularSharpnessJSON.val;
		HairSimControl.copiedSecondarySpecularSharpness = this.secondarySpecularSharpnessJSON.val;
		HairSimControl.copiedSpecularShift = this.specularShiftJSON.val;
		HairSimControl.copiedFresnelPower = this.fresnelPowerJSON.val;
		HairSimControl.copiedFresnelAttenuation = this.fresnelAttenuationJSON.val;
		HairSimControl.copiedRandomColorPower = this.randomColorPowerJSON.val;
		HairSimControl.copiedRandomColorOffset = this.randomColorOffsetJSON.val;
		HairSimControl.copiedIBLFactor = this.IBLFactorJSON.val;
		HairSimControl.copiedNormalRandomize = this.normalRandomizeJSON.val;
	}

	// Token: 0x060069D5 RID: 27093 RVA: 0x0027A444 File Offset: 0x00278844
	public void PasteLightingParameters()
	{
		if (HairSimControl.hasLightingCopyData)
		{
			this.hasLightingPasteData = true;
			this.pasteUndoShaderType = this.shaderTypeJSON.val;
			this.pasteUndoRootColor = this.rootColorJSON.val;
			this.pasteUndoTipColor = this.tipColorJSON.val;
			this.pasteUndoColorRolloff = this.colorRolloffJSON.val;
			HairSimControl.pasteUndoSpecularColor = this.specularColorJSON.val;
			this.pasteUndoDiffuseSoftness = this.diffuseSoftnessJSON.val;
			this.pasteUndoPrimarySpecularSharpness = this.primarySpecularSharpnessJSON.val;
			this.pasteUndoSecondarySpecularSharpness = this.secondarySpecularSharpnessJSON.val;
			this.pasteUndoSpecularShift = this.specularShiftJSON.val;
			this.pasteUndoFresnelPower = this.fresnelPowerJSON.val;
			this.pasteUndoFresnelAttenuation = this.fresnelAttenuationJSON.val;
			this.pasteUndoRandomColorPower = this.randomColorPowerJSON.val;
			this.pasteUndoRandomColorOffset = this.randomColorOffsetJSON.val;
			this.pasteUndoIBLFactor = this.IBLFactorJSON.val;
			this.pasteUndoNormalRandomize = this.normalRandomizeJSON.val;
			this.shaderTypeJSON.val = HairSimControl.copiedShaderType;
			this.rootColorJSON.val = HairSimControl.copiedRootColor;
			this.tipColorJSON.val = HairSimControl.copiedTipColor;
			this.colorRolloffJSON.val = HairSimControl.copiedColorRolloff;
			this.specularColorJSON.val = HairSimControl.copiedSpecularColor;
			this.diffuseSoftnessJSON.val = HairSimControl.copiedDiffuseSoftness;
			this.primarySpecularSharpnessJSON.val = HairSimControl.copiedPrimarySpecularSharpness;
			this.secondarySpecularSharpnessJSON.val = HairSimControl.copiedSecondarySpecularSharpness;
			this.specularShiftJSON.val = HairSimControl.copiedSpecularShift;
			this.fresnelPowerJSON.val = HairSimControl.copiedFresnelPower;
			this.fresnelAttenuationJSON.val = HairSimControl.copiedFresnelAttenuation;
			this.randomColorPowerJSON.val = HairSimControl.copiedRandomColorPower;
			this.randomColorOffsetJSON.val = HairSimControl.copiedRandomColorOffset;
			this.IBLFactorJSON.val = HairSimControl.copiedIBLFactor;
			this.normalRandomizeJSON.val = HairSimControl.copiedNormalRandomize;
		}
	}

	// Token: 0x060069D6 RID: 27094 RVA: 0x0027A650 File Offset: 0x00278A50
	public void UndoPasteLightingParameters()
	{
		if (this.hasLightingPasteData)
		{
			this.shaderTypeJSON.val = this.pasteUndoShaderType;
			this.rootColorJSON.val = this.pasteUndoRootColor;
			this.tipColorJSON.val = this.pasteUndoTipColor;
			this.colorRolloffJSON.val = this.pasteUndoColorRolloff;
			this.specularColorJSON.val = HairSimControl.pasteUndoSpecularColor;
			this.diffuseSoftnessJSON.val = this.pasteUndoDiffuseSoftness;
			this.primarySpecularSharpnessJSON.val = this.pasteUndoPrimarySpecularSharpness;
			this.secondarySpecularSharpnessJSON.val = this.pasteUndoSecondarySpecularSharpness;
			this.specularShiftJSON.val = this.pasteUndoSpecularShift;
			this.fresnelPowerJSON.val = this.pasteUndoFresnelPower;
			this.fresnelAttenuationJSON.val = this.pasteUndoFresnelAttenuation;
			this.randomColorPowerJSON.val = this.pasteUndoRandomColorPower;
			this.randomColorOffsetJSON.val = this.pasteUndoRandomColorOffset;
			this.IBLFactorJSON.val = this.pasteUndoIBLFactor;
			this.normalRandomizeJSON.val = this.pasteUndoNormalRandomize;
			this.hasLightingPasteData = false;
		}
	}

	// Token: 0x060069D7 RID: 27095 RVA: 0x0027A770 File Offset: 0x00278B70
	protected void SyncShader()
	{
		if (this._currentShaderType == HairSimControl.ShaderType.Quality)
		{
			if (this.qualityShader != null && this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.render != null)
			{
				this.hairSettings.HairBuidCommand.render.SetShader(this.qualityShader);
			}
		}
		else if (this._currentShaderType == HairSimControl.ShaderType.QualityThicken)
		{
			if (this.qualityThickenShader != null && this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.render != null)
			{
				this.hairSettings.HairBuidCommand.render.SetShader(this.qualityThickenShader);
			}
		}
		else if (this._currentShaderType == HairSimControl.ShaderType.QualityThickenMore)
		{
			if (this.qualityThickenMoreShader != null && this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.render != null)
			{
				this.hairSettings.HairBuidCommand.render.SetShader(this.qualityThickenMoreShader);
			}
		}
		else if (this._currentShaderType == HairSimControl.ShaderType.Fast)
		{
			if (this.fastShader != null && this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.render != null)
			{
				this.hairSettings.HairBuidCommand.render.SetShader(this.fastShader);
			}
		}
		else if (this._currentShaderType == HairSimControl.ShaderType.NonStandard && this.nonStandardShader != null && this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.render != null)
		{
			this.hairSettings.HairBuidCommand.render.SetShader(this.nonStandardShader);
		}
	}

	// Token: 0x060069D8 RID: 27096 RVA: 0x0027A980 File Offset: 0x00278D80
	protected void SyncShaderType(string shaderTypeString)
	{
		try
		{
			HairSimControl.ShaderType currentShaderType = (HairSimControl.ShaderType)Enum.Parse(typeof(HairSimControl.ShaderType), shaderTypeString);
			this._currentShaderType = currentShaderType;
			this.SyncShader();
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogError("Attempted to set shader type to " + shaderTypeString);
		}
	}

	// Token: 0x060069D9 RID: 27097 RVA: 0x0027A9DC File Offset: 0x00278DDC
	protected void SyncRootColor(float h, float s, float v)
	{
		this._rootHSVColor.H = h;
		this._rootHSVColor.S = s;
		this._rootHSVColor.V = v;
		this._rootColor = HSVColorPicker.HSVToRGB(h, s, v);
		if (this.hairSettings != null && this.hairSettings.RenderSettings.RootTipColorProvider != null)
		{
			this.hairSettings.RenderSettings.RootTipColorProvider.RootColor = this._rootColor;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x060069DA RID: 27098 RVA: 0x0027AA98 File Offset: 0x00278E98
	protected void SyncTipColor(float h, float s, float v)
	{
		this._tipHSVColor.H = h;
		this._tipHSVColor.S = s;
		this._tipHSVColor.V = v;
		this._tipColor = HSVColorPicker.HSVToRGB(h, s, v);
		if (this.hairSettings != null && this.hairSettings.RenderSettings.RootTipColorProvider != null)
		{
			this.hairSettings.RenderSettings.RootTipColorProvider.TipColor = this._tipColor;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x060069DB RID: 27099 RVA: 0x0027AB54 File Offset: 0x00278F54
	public void SyncColorRolloff(float f)
	{
		if (this.hairSettings != null && this.hairSettings.RenderSettings.RootTipColorProvider != null)
		{
			this.hairSettings.RenderSettings.RootTipColorProvider.ColorRolloff = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x060069DC RID: 27100 RVA: 0x0027ABD8 File Offset: 0x00278FD8
	protected void SyncSpecularColor(float h, float s, float v)
	{
		this._specularHSVColor.H = h;
		this._specularHSVColor.S = s;
		this._specularHSVColor.V = v;
		this._specularColor = HSVColorPicker.HSVToRGB(h, s, v);
		this.hairSettings.RenderSettings.SpecularColor = this._specularColor;
	}

	// Token: 0x060069DD RID: 27101 RVA: 0x0027AC2D File Offset: 0x0027902D
	public void SyncDiffuseSoftness(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.Diffuse = f;
		}
	}

	// Token: 0x060069DE RID: 27102 RVA: 0x0027AC51 File Offset: 0x00279051
	public void SyncPrimarySpecularSharpness(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.PrimarySpecular = f;
		}
	}

	// Token: 0x060069DF RID: 27103 RVA: 0x0027AC75 File Offset: 0x00279075
	public void SyncSecondarySpecularSharpness(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.SecondarySpecular = f;
		}
	}

	// Token: 0x060069E0 RID: 27104 RVA: 0x0027AC99 File Offset: 0x00279099
	public void SyncSpecularShift(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.SpecularShift = f;
		}
	}

	// Token: 0x060069E1 RID: 27105 RVA: 0x0027ACBD File Offset: 0x002790BD
	public void SyncFresnelPower(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.FresnelPower = f;
		}
	}

	// Token: 0x060069E2 RID: 27106 RVA: 0x0027ACE1 File Offset: 0x002790E1
	public void SyncFresnelAttenuation(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.FresnelAttenuation = f;
		}
	}

	// Token: 0x060069E3 RID: 27107 RVA: 0x0027AD05 File Offset: 0x00279105
	public void SyncRandomColorPower(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.RandomTexColorPower = f;
		}
	}

	// Token: 0x060069E4 RID: 27108 RVA: 0x0027AD29 File Offset: 0x00279129
	public void SyncRandomColorOffset(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.RandomTexColorOffset = f;
		}
	}

	// Token: 0x060069E5 RID: 27109 RVA: 0x0027AD4D File Offset: 0x0027914D
	public void SyncIBLFactor(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.IBLFactor = f;
		}
	}

	// Token: 0x060069E6 RID: 27110 RVA: 0x0027AD71 File Offset: 0x00279171
	public void SyncNormalRandomize(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.NormalRandomize = f;
		}
	}

	// Token: 0x060069E7 RID: 27111 RVA: 0x0027AD98 File Offset: 0x00279198
	public void CopyLookParameters()
	{
		HairSimControl.hasLookCopyData = true;
		HairSimControl.copiedCurlX = this.curlXJSON.val;
		HairSimControl.copiedCurlY = this.curlYJSON.val;
		HairSimControl.copiedCurlZ = this.curlZJSON.val;
		HairSimControl.copiedCurlScale = this.curlScaleJSON.val;
		HairSimControl.copiedCurlScaleRandomness = this.curlScaleRandomnessJSON.val;
		HairSimControl.copiedCurlFrequency = this.curlFrequencyJSON.val;
		HairSimControl.copiedCurlFrequencyRandomness = this.curlFrequencyRandomnessJSON.val;
		HairSimControl.copiedCurlAllowReverse = this.curlAllowReverseJSON.val;
		HairSimControl.copiedCurlAllowFlipAxis = this.curlAllowFlipAxisJSON.val;
		HairSimControl.copiedCurlNormalAdjust = this.curlNormalAdjustJSON.val;
		HairSimControl.copiedCurlRoot = this.curlRootJSON.val;
		HairSimControl.copiedCurlMid = this.curlMidJSON.val;
		HairSimControl.copiedCurlTip = this.curlTipJSON.val;
		HairSimControl.copiedCurlMidpoint = this.curlMidpointJSON.val;
		HairSimControl.copiedCurlCurvePower = this.curlCurvePowerJSON.val;
		HairSimControl.copiedLength1 = this.length1JSON.val;
		HairSimControl.copiedLength2 = this.length2JSON.val;
		HairSimControl.copiedLength3 = this.length3JSON.val;
		HairSimControl.copiedWidth = this.widthJSON.val;
		HairSimControl.copiedMaxSpread = this.maxSpreadJSON.val;
		HairSimControl.copiedSpreadRoot = this.spreadRootJSON.val;
		HairSimControl.copiedSpreadMid = this.spreadMidJSON.val;
		HairSimControl.copiedSpreadTip = this.spreadTipJSON.val;
		HairSimControl.copiedSpreadMidpoint = this.spreadMidpointJSON.val;
		HairSimControl.copiedSpreadCurvePower = this.spreadCurvePowerJSON.val;
	}

	// Token: 0x060069E8 RID: 27112 RVA: 0x0027AF3C File Offset: 0x0027933C
	public void PasteLookParameters()
	{
		if (HairSimControl.hasLookCopyData)
		{
			HairSimControl.hasLookPasteData = true;
			this.pasteUndoCurlX = this.curlXJSON.val;
			this.pasteUndoCurlY = this.curlYJSON.val;
			this.pasteUndoCurlZ = this.curlZJSON.val;
			this.pasteUndoCurlScale = this.curlScaleJSON.val;
			this.pasteUndoCurlScaleRandomness = this.curlScaleRandomnessJSON.val;
			this.pasteUndoCurlFrequency = this.curlFrequencyJSON.val;
			this.pasteUndoCurlFrequencyRandomness = this.curlFrequencyRandomnessJSON.val;
			this.pasteUndoCurlAllowReverse = this.curlAllowReverseJSON.val;
			this.pasteUndoCurlAllowFlipAxis = this.curlAllowFlipAxisJSON.val;
			this.pasteUndoCurlNormalAdjust = this.curlNormalAdjustJSON.val;
			this.pasteUndoCurlRoot = this.curlRootJSON.val;
			this.pasteUndoCurlMid = this.curlMidJSON.val;
			this.pasteUndoCurlTip = this.curlTipJSON.val;
			this.pasteUndoCurlMidpoint = this.curlMidpointJSON.val;
			this.pasteUndoCurlCurvePower = this.curlCurvePowerJSON.val;
			this.pasteUndoLength1 = this.length1JSON.val;
			this.pasteUndoLength2 = this.length2JSON.val;
			this.pasteUndoLength3 = this.length3JSON.val;
			this.pasteUndoWidth = this.widthJSON.val;
			this.pasteUndoMaxSpread = this.maxSpreadJSON.val;
			this.pasteUndoSpreadRoot = this.spreadRootJSON.val;
			this.pasteUndoSpreadMid = this.spreadMidJSON.val;
			this.pasteUndoSpreadTip = this.spreadTipJSON.val;
			this.pasteUndoSpreadMidpoint = this.spreadMidpointJSON.val;
			this.pasteUndoSpreadCurvePower = this.spreadCurvePowerJSON.val;
			this.curlXJSON.val = HairSimControl.copiedCurlX;
			this.curlYJSON.val = HairSimControl.copiedCurlY;
			this.curlZJSON.val = HairSimControl.copiedCurlZ;
			this.curlScaleJSON.val = HairSimControl.copiedCurlScale;
			this.curlScaleRandomnessJSON.val = HairSimControl.copiedCurlScaleRandomness;
			this.curlFrequencyJSON.val = HairSimControl.copiedCurlFrequency;
			this.curlFrequencyRandomnessJSON.val = HairSimControl.copiedCurlFrequencyRandomness;
			this.curlAllowReverseJSON.val = HairSimControl.copiedCurlAllowReverse;
			this.curlAllowFlipAxisJSON.val = HairSimControl.copiedCurlAllowFlipAxis;
			this.curlNormalAdjustJSON.val = HairSimControl.copiedCurlNormalAdjust;
			this.curlRootJSON.val = HairSimControl.copiedCurlRoot;
			this.curlMidJSON.val = HairSimControl.copiedCurlMid;
			this.curlTipJSON.val = HairSimControl.copiedCurlTip;
			this.curlMidpointJSON.val = HairSimControl.copiedCurlMidpoint;
			this.curlCurvePowerJSON.val = HairSimControl.copiedCurlCurvePower;
			this.length1JSON.val = HairSimControl.copiedLength1;
			this.length2JSON.val = HairSimControl.copiedLength2;
			this.length3JSON.val = HairSimControl.copiedLength3;
			this.widthJSON.val = HairSimControl.copiedWidth;
			this.maxSpreadJSON.val = HairSimControl.copiedMaxSpread;
			this.spreadRootJSON.val = HairSimControl.copiedSpreadRoot;
			this.spreadMidJSON.val = HairSimControl.copiedSpreadMid;
			this.spreadTipJSON.val = HairSimControl.copiedSpreadTip;
			this.spreadMidpointJSON.val = HairSimControl.copiedSpreadMidpoint;
			this.spreadCurvePowerJSON.val = HairSimControl.copiedSpreadCurvePower;
		}
	}

	// Token: 0x060069E9 RID: 27113 RVA: 0x0027B294 File Offset: 0x00279694
	public void UndoPasteLookParameters()
	{
		if (HairSimControl.hasLookPasteData)
		{
			this.curlXJSON.val = this.pasteUndoCurlX;
			this.curlYJSON.val = this.pasteUndoCurlY;
			this.curlZJSON.val = this.pasteUndoCurlZ;
			this.curlScaleJSON.val = this.pasteUndoCurlScale;
			this.curlScaleRandomnessJSON.val = this.pasteUndoCurlScaleRandomness;
			this.curlFrequencyJSON.val = this.pasteUndoCurlFrequency;
			this.curlFrequencyRandomnessJSON.val = this.pasteUndoCurlFrequencyRandomness;
			this.curlAllowReverseJSON.val = this.pasteUndoCurlAllowReverse;
			this.curlAllowFlipAxisJSON.val = this.pasteUndoCurlAllowFlipAxis;
			this.curlNormalAdjustJSON.val = this.pasteUndoCurlNormalAdjust;
			this.curlRootJSON.val = this.pasteUndoCurlRoot;
			this.curlMidJSON.val = this.pasteUndoCurlMid;
			this.curlTipJSON.val = this.pasteUndoCurlTip;
			this.curlMidpointJSON.val = this.pasteUndoCurlMidpoint;
			this.curlCurvePowerJSON.val = this.pasteUndoCurlCurvePower;
			this.length1JSON.val = this.pasteUndoLength1;
			this.length2JSON.val = this.pasteUndoLength2;
			this.length3JSON.val = this.pasteUndoLength3;
			this.widthJSON.val = this.pasteUndoWidth;
			this.maxSpreadJSON.val = this.pasteUndoMaxSpread;
			this.spreadRootJSON.val = this.pasteUndoSpreadRoot;
			this.spreadMidJSON.val = this.pasteUndoSpreadMid;
			this.spreadTipJSON.val = this.pasteUndoSpreadTip;
			this.spreadMidpointJSON.val = this.pasteUndoSpreadMidpoint;
			this.spreadCurvePowerJSON.val = this.pasteUndoSpreadCurvePower;
			HairSimControl.hasLookPasteData = false;
		}
	}

	// Token: 0x060069EA RID: 27114 RVA: 0x0027B45C File Offset: 0x0027985C
	public void SyncCurlX(float f)
	{
		if (this.hairSettings != null)
		{
			Vector3 wavinessAxis = this.hairSettings.RenderSettings.WavinessAxis;
			wavinessAxis.x = f;
			this.hairSettings.RenderSettings.WavinessAxis = wavinessAxis;
		}
	}

	// Token: 0x060069EB RID: 27115 RVA: 0x0027B4A4 File Offset: 0x002798A4
	public void SyncCurlY(float f)
	{
		if (this.hairSettings != null)
		{
			Vector3 wavinessAxis = this.hairSettings.RenderSettings.WavinessAxis;
			wavinessAxis.y = f;
			this.hairSettings.RenderSettings.WavinessAxis = wavinessAxis;
		}
	}

	// Token: 0x060069EC RID: 27116 RVA: 0x0027B4EC File Offset: 0x002798EC
	public void SyncCurlZ(float f)
	{
		if (this.hairSettings != null)
		{
			Vector3 wavinessAxis = this.hairSettings.RenderSettings.WavinessAxis;
			wavinessAxis.z = f;
			this.hairSettings.RenderSettings.WavinessAxis = wavinessAxis;
		}
	}

	// Token: 0x060069ED RID: 27117 RVA: 0x0027B534 File Offset: 0x00279934
	public void SyncCurlScale(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.WavinessScale = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x060069EE RID: 27118 RVA: 0x0027B59D File Offset: 0x0027999D
	public void SyncCurlScaleRandomness(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.WavinessScaleRandomness = f;
		}
	}

	// Token: 0x060069EF RID: 27119 RVA: 0x0027B5C4 File Offset: 0x002799C4
	public void SyncCurlFrequency(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.WavinessFrequency = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x060069F0 RID: 27120 RVA: 0x0027B62D File Offset: 0x00279A2D
	public void SyncCurlFrequencyRandomness(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.WavinessFrequencyRandomness = f;
		}
	}

	// Token: 0x060069F1 RID: 27121 RVA: 0x0027B651 File Offset: 0x00279A51
	public void SyncCurlAllowReverse(bool b)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.WavinessAllowReverse = b;
		}
	}

	// Token: 0x060069F2 RID: 27122 RVA: 0x0027B675 File Offset: 0x00279A75
	public void SyncCurlAllowFlipAxis(bool b)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.WavinessAllowFlipAxis = b;
		}
	}

	// Token: 0x060069F3 RID: 27123 RVA: 0x0027B699 File Offset: 0x00279A99
	public void SyncCurlNormalAdjust(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.WavinessNormalAdjust = f;
		}
	}

	// Token: 0x060069F4 RID: 27124 RVA: 0x0027B6C0 File Offset: 0x00279AC0
	public void SyncCurlRoot(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.WavinessRoot = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x060069F5 RID: 27125 RVA: 0x0027B72C File Offset: 0x00279B2C
	public void SyncCurlMid(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.WavinessMid = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x060069F6 RID: 27126 RVA: 0x0027B798 File Offset: 0x00279B98
	public void SyncCurlTip(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.WavinessTip = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x060069F7 RID: 27127 RVA: 0x0027B804 File Offset: 0x00279C04
	public void SyncCurlMidpoint(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.WavinessMidpoint = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x060069F8 RID: 27128 RVA: 0x0027B870 File Offset: 0x00279C70
	public void SyncCurlCurvePower(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.WavinessCurvePower = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x060069F9 RID: 27129 RVA: 0x0027B8D9 File Offset: 0x00279CD9
	public void SyncLength1(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.Length1 = f;
		}
	}

	// Token: 0x060069FA RID: 27130 RVA: 0x0027B8FD File Offset: 0x00279CFD
	public void SyncLength2(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.Length2 = f;
		}
	}

	// Token: 0x060069FB RID: 27131 RVA: 0x0027B921 File Offset: 0x00279D21
	public void SyncLength3(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.Length3 = f;
		}
	}

	// Token: 0x060069FC RID: 27132 RVA: 0x0027B945 File Offset: 0x00279D45
	public void SyncWidth(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.LODSettings.FixedWidth = f;
		}
	}

	// Token: 0x060069FD RID: 27133 RVA: 0x0027B969 File Offset: 0x00279D69
	public void SyncDensity(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.LODSettings.FixedDensity = Mathf.FloorToInt(f);
		}
	}

	// Token: 0x060069FE RID: 27134 RVA: 0x0027B992 File Offset: 0x00279D92
	public void SyncDetail(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.LODSettings.FixedDetail = Mathf.FloorToInt(f);
		}
	}

	// Token: 0x060069FF RID: 27135 RVA: 0x0027B9BB File Offset: 0x00279DBB
	public void SyncMaxSpread(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.MaxSpread = f;
		}
	}

	// Token: 0x06006A00 RID: 27136 RVA: 0x0027B9E0 File Offset: 0x00279DE0
	public void SyncSpreadRoot(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.InterpolationRoot = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x06006A01 RID: 27137 RVA: 0x0027BA4C File Offset: 0x00279E4C
	public void SyncSpreadMid(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.InterpolationMid = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x06006A02 RID: 27138 RVA: 0x0027BAB8 File Offset: 0x00279EB8
	public void SyncSpreadTip(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.InterpolationTip = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x06006A03 RID: 27139 RVA: 0x0027BB24 File Offset: 0x00279F24
	public void SyncSpreadMidpoint(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.InterpolationMidpoint = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x06006A04 RID: 27140 RVA: 0x0027BB90 File Offset: 0x00279F90
	public void SyncSpreadCurvePower(float f)
	{
		if (this.hairSettings != null)
		{
			this.hairSettings.RenderSettings.InterpolationCurvePower = f;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.particlesData != null)
			{
				this.hairSettings.HairBuidCommand.particlesData.UpdateSettings();
			}
		}
	}

	// Token: 0x06006A05 RID: 27141 RVA: 0x0027BBFC File Offset: 0x00279FFC
	protected void Init()
	{
		if (this.hairSettings == null)
		{
			this.hairSettings = base.GetComponent<HairSettings>();
		}
		if (this.hairSettings != null)
		{
			if (this.creator != null)
			{
				this.resetAndStartStyleModeAction = new JSONStorableAction("ResetAndStartStyleMode", new JSONStorableAction.ActionCallback(this.StartStyleModeWithReset));
				base.RegisterAction(this.resetAndStartStyleModeAction);
				this.startStyleModeAction = new JSONStorableAction("StartStyleMode", new JSONStorableAction.ActionCallback(this.StartStyleModeWithoutReset));
				base.RegisterAction(this.startStyleModeAction);
				this.cancelStyleModeAction = new JSONStorableAction("CancelStyleMode", new JSONStorableAction.ActionCallback(this.CancelStyleMode));
				base.RegisterAction(this.cancelStyleModeAction);
				this.keepStyleAction = new JSONStorableAction("KeepStyle", new JSONStorableAction.ActionCallback(this.KeepStyle));
				base.RegisterAction(this.keepStyleAction);
				this.rebuildStyleJointsAction = new JSONStorableAction("RebuildStyleJoints", new JSONStorableAction.ActionCallback(this.RebuildStyleJoints));
				base.RegisterAction(this.rebuildStyleJointsAction);
				this.styleModeAllowControlOtherNodesJSON = new JSONStorableBool("styleModeAllowControlOtherNodes", false, new JSONStorableBool.SetBoolCallback(this.SyncStyleModeAllowControlOtherNodes));
				base.RegisterBool(this.styleModeAllowControlOtherNodesJSON);
				this.styleJointsSearchDistanceJSON = new JSONStorableFloat("styleJointsSearchDistance", this.creator.NearbyVertexSearchDistance, new JSONStorableFloat.SetFloatCallback(this.SyncStyleJointsSearchDistance), 0.0001f, 0.01f, true, true);
				base.RegisterFloat(this.styleJointsSearchDistanceJSON);
				this.clearStyleJointsAction = new JSONStorableAction("ClearStyleJoints", new JSONStorableAction.ActionCallback(this.ClearStyleJoints));
				base.RegisterAction(this.clearStyleJointsAction);
				this.styleModeCollisionRadiusJSON = new JSONStorableFloat("styleModeCollisionRadius", this.hairSettings.PhysicsSettings.StyleModeStrandRadius, new JSONStorableFloat.SetFloatCallback(this.SyncStyleModeCollisionRadius), 0.001f, 0.1f, true, true);
				base.RegisterFloat(this.styleModeCollisionRadiusJSON);
				if (this.hairSettings.PhysicsSettings.UseSeparateRootRadius)
				{
					this.styleModeCollisionRadiusRootJSON = new JSONStorableFloat("styleModeCollisionRadiusRoot", this.hairSettings.PhysicsSettings.StyleModeStrandRootRadius, new JSONStorableFloat.SetFloatCallback(this.SyncStyleModeCollisionRadiusRoot), 0.001f, 0.1f, true, true);
					base.RegisterFloat(this.styleModeCollisionRadiusRootJSON);
				}
				this.styleModeGravityMultiplierJSON = new JSONStorableFloat("styleModeGravityMultiplier", this.hairSettings.PhysicsSettings.StyleModeGravityMultiplier, new JSONStorableFloat.SetFloatCallback(this.SyncStyleModeGravityMultiplier), -2f, 2f, true, true);
				base.RegisterFloat(this.styleModeGravityMultiplierJSON);
				this.styleModeShowCurlsJSON = new JSONStorableBool("styleModeShowCurls", this.hairSettings.RenderSettings.StyleModeShowCurls, new JSONStorableBool.SetBoolCallback(this.SyncStyleModeShowCurls));
				base.RegisterBool(this.styleModeShowCurlsJSON);
				this.styleModeUpHairPullStrengthJSON = new JSONStorableFloat("styleModeUpHairPullStrength", this.hairSettings.PhysicsSettings.ReverseSplineJointPower, new JSONStorableFloat.SetFloatCallback(this.SyncStyleModeUpHairPullStrength), 0.1f, 1f, true, true);
				base.RegisterFloat(this.styleModeUpHairPullStrengthJSON);
				this.styleModeShowTool1JSON = new JSONStorableBool("styleModeShowTool1", true, new JSONStorableBool.SetBoolCallback(this.SyncShowStyleTool1));
				base.RegisterBool(this.styleModeShowTool1JSON);
				this.styleModeShowTool2JSON = new JSONStorableBool("styleModeShowTool2", true, new JSONStorableBool.SetBoolCallback(this.SyncShowStyleTool2));
				base.RegisterBool(this.styleModeShowTool2JSON);
				this.styleModeShowTool3JSON = new JSONStorableBool("styleModeShowTool3", false, new JSONStorableBool.SetBoolCallback(this.SyncShowStyleTool3));
				base.RegisterBool(this.styleModeShowTool3JSON);
				this.styleModeShowTool4JSON = new JSONStorableBool("styleModeShowTool4", false, new JSONStorableBool.SetBoolCallback(this.SyncShowStyleTool4));
				base.RegisterBool(this.styleModeShowTool4JSON);
			}
			this.copyPhysicsParametersAction = new JSONStorableAction("CopyPhysicsParameters", new JSONStorableAction.ActionCallback(this.CopyPhysicsParameters));
			base.RegisterAction(this.copyPhysicsParametersAction);
			this.pastePhysicsParametersAction = new JSONStorableAction("PastePhysicsParameters", new JSONStorableAction.ActionCallback(this.PastePhysicsParameters));
			base.RegisterAction(this.pastePhysicsParametersAction);
			this.undoPastePhysicsParametersAction = new JSONStorableAction("UndoPastePhysicsParameters", new JSONStorableAction.ActionCallback(this.UndoPastePhysicsParameters));
			base.RegisterAction(this.undoPastePhysicsParametersAction);
			this.simulationEnabledJSON = new JSONStorableBool("simulationEnabled", this.hairSettings.PhysicsSettings.IsEnabled, new JSONStorableBool.SetBoolCallback(this.SyncSimulationEnabled));
			base.RegisterBool(this.simulationEnabledJSON);
			this.collisionEnabledJSON = new JSONStorableBool("collisionEnabled", this.hairSettings.PhysicsSettings.IsCollisionEnabled, new JSONStorableBool.SetBoolCallback(this.SyncCollisionEnabled));
			base.RegisterBool(this.collisionEnabledJSON);
			this.collisionRadiusJSON = new JSONStorableFloat("collisionRadius", this.hairSettings.PhysicsSettings.StandRadius, new JSONStorableFloat.SetFloatCallback(this.SyncCollisionRadius), 0.001f, 0.1f, true, true);
			base.RegisterFloat(this.collisionRadiusJSON);
			if (this.hairSettings.PhysicsSettings.UseSeparateRootRadius)
			{
				this.collisionRadiusRootJSON = new JSONStorableFloat("collisionRadiusRoot", this.hairSettings.PhysicsSettings.StandRootRadius, new JSONStorableFloat.SetFloatCallback(this.SyncCollisionRadiusRoot), 0.001f, 0.1f, true, true);
				base.RegisterFloat(this.collisionRadiusRootJSON);
			}
			this.dragJSON = new JSONStorableFloat("drag", this.hairSettings.PhysicsSettings.Drag, new JSONStorableFloat.SetFloatCallback(this.SyncDrag), 0f, 1f, true, true);
			base.RegisterFloat(this.dragJSON);
			this.usePaintedRigidityJSON = new JSONStorableBool("usePaintedRigidity", this.hairSettings.PhysicsSettings.UsePaintedRigidity, new JSONStorableBool.SetBoolCallback(this.SyncUsePaintedRigidity));
			base.RegisterBool(this.usePaintedRigidityJSON);
			this.rootRigidityJSON = new JSONStorableFloat("rootRigidity", this.hairSettings.PhysicsSettings.RootRigidity, new JSONStorableFloat.SetFloatCallback(this.SyncRootRigidity), 0f, 1f, true, true);
			base.RegisterFloat(this.rootRigidityJSON);
			this.mainRigidityJSON = new JSONStorableFloat("mainRigidity", this.hairSettings.PhysicsSettings.MainRigidity, new JSONStorableFloat.SetFloatCallback(this.SyncMainRigidity), 0f, 1f, true, true);
			base.RegisterFloat(this.mainRigidityJSON);
			this.tipRigidityJSON = new JSONStorableFloat("tipRigidity", this.hairSettings.PhysicsSettings.TipRigidity, new JSONStorableFloat.SetFloatCallback(this.SyncTipRigidity), 0f, 1f, true, true);
			base.RegisterFloat(this.tipRigidityJSON);
			this.rigidityRolloffPowerJSON = new JSONStorableFloat("rigidityRolloffPower", this.hairSettings.PhysicsSettings.RigidityRolloffPower, new JSONStorableFloat.SetFloatCallback(this.SyncRigidityRolloffPower), 0f, 16f, true, true);
			base.RegisterFloat(this.rigidityRolloffPowerJSON);
			if (this.hairSettings.PhysicsSettings.JointAreas != null && this.hairSettings.PhysicsSettings.JointAreas.Count > 0)
			{
				this.jointRigidityJSON = new JSONStorableFloat("jointRigidity", this.hairSettings.PhysicsSettings.JointRigidity, new JSONStorableFloat.SetFloatCallback(this.SyncJointRigidity), 0f, 1f, true, true);
				base.RegisterFloat(this.jointRigidityJSON);
			}
			this.frictionJSON = new JSONStorableFloat("friction", this.hairSettings.PhysicsSettings.Friction, new JSONStorableFloat.SetFloatCallback(this.SyncFriction), 0f, 1f, true, true);
			base.RegisterFloat(this.frictionJSON);
			this.gravityMultiplierJSON = new JSONStorableFloat("gravityMultiplier", this.hairSettings.PhysicsSettings.GravityMultiplier, new JSONStorableFloat.SetFloatCallback(this.SyncGravityMultiplier), -2f, 2f, true, true);
			base.RegisterFloat(this.gravityMultiplierJSON);
			this.weightJSON = new JSONStorableFloat("weight", this.hairSettings.PhysicsSettings.Weight, new JSONStorableFloat.SetFloatCallback(this.SyncWeight), 0f, 2f, true, true);
			base.RegisterFloat(this.weightJSON);
			this.iterationsJSON = new JSONStorableFloat("iterations", (float)this.hairSettings.PhysicsSettings.Iterations, new JSONStorableFloat.SetFloatCallback(this.SyncIterations), 1f, 5f, true, true);
			base.RegisterFloat(this.iterationsJSON);
			this.clingJSON = new JSONStorableFloat("cling", this.hairSettings.PhysicsSettings.NearbyJointPower, new JSONStorableFloat.SetFloatCallback(this.SyncCling), 0f, 1f, true, true);
			base.RegisterFloat(this.clingJSON);
			this.clingRolloffJSON = new JSONStorableFloat("clingRolloff", this.hairSettings.PhysicsSettings.NearbyJointPowerRolloff, new JSONStorableFloat.SetFloatCallback(this.SyncClingRolloff), 0f, 1f, true, true);
			base.RegisterFloat(this.clingRolloffJSON);
			this.snapJSON = new JSONStorableFloat("snap", this.hairSettings.PhysicsSettings.SplineJointPower, new JSONStorableFloat.SetFloatCallback(this.SyncSnap), 0f, 1f, true, true);
			base.RegisterFloat(this.snapJSON);
			this.bendResistanceJSON = new JSONStorableFloat("bendResistance", this.hairSettings.PhysicsSettings.CompressionJointPower, new JSONStorableFloat.SetFloatCallback(this.SyncBendResistance), 0f, 1f, true, true);
			base.RegisterFloat(this.bendResistanceJSON);
			this.windJSON = new JSONStorableVector3("wind", Vector3.zero, new Vector3(-50f, -50f, -50f), new Vector3(50f, 50f, 50f), false, true);
			base.RegisterVector3(this.windJSON);
			this.copyLightingParametersAction = new JSONStorableAction("CopyLightingParameters", new JSONStorableAction.ActionCallback(this.CopyLightingParameters));
			base.RegisterAction(this.copyLightingParametersAction);
			this.pasteLightingParametersAction = new JSONStorableAction("PasteLightingParameters", new JSONStorableAction.ActionCallback(this.PasteLightingParameters));
			base.RegisterAction(this.pasteLightingParametersAction);
			this.undoPasteLightingParametersAction = new JSONStorableAction("UndoPasteLightingParameters", new JSONStorableAction.ActionCallback(this.UndoPasteLightingParameters));
			base.RegisterAction(this.undoPasteLightingParametersAction);
			Shader x = null;
			if (this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.render != null)
			{
				x = this.hairSettings.HairBuidCommand.render.GetShader();
			}
			else if (this.hairSettings.RenderSettings.material != null)
			{
				x = this.hairSettings.RenderSettings.material.shader;
			}
			if (x != null)
			{
				if (this.qualityShader != null && x == this.qualityShader)
				{
					this._currentShaderType = HairSimControl.ShaderType.Quality;
				}
				else if (this.qualityThickenShader != null && x == this.qualityThickenShader)
				{
					this._currentShaderType = HairSimControl.ShaderType.QualityThicken;
				}
				else if (this.qualityThickenMoreShader != null && x == this.qualityThickenMoreShader)
				{
					this._currentShaderType = HairSimControl.ShaderType.QualityThickenMore;
				}
				else if (this.fastShader != null && x == this.fastShader)
				{
					this._currentShaderType = HairSimControl.ShaderType.Fast;
				}
				else
				{
					this._currentShaderType = HairSimControl.ShaderType.NonStandard;
					this.nonStandardShader = x;
				}
			}
			List<string> list = new List<string>();
			if (this.fastShader != null)
			{
				list.Add("Fast");
			}
			if (this.qualityShader != null)
			{
				list.Add("Quality");
			}
			if (this.qualityThickenShader != null)
			{
				list.Add("QualityThicken");
			}
			if (this.qualityThickenMoreShader != null)
			{
				list.Add("QualityThickenMore");
			}
			if (this.nonStandardShader != null)
			{
				list.Add("NonStandard");
			}
			this.shaderTypeJSON = new JSONStorableStringChooser("shaderType", list, this._currentShaderType.ToString(), "Shader Type", new JSONStorableStringChooser.SetStringCallback(this.SyncShaderType));
			base.RegisterStringChooser(this.shaderTypeJSON);
			if (this.hairSettings.RenderSettings.RootTipColorProvider != null)
			{
				this._rootColor = this.hairSettings.RenderSettings.RootTipColorProvider.RootColor;
				this._tipColor = this.hairSettings.RenderSettings.RootTipColorProvider.TipColor;
			}
			this._specularColor = this.hairSettings.RenderSettings.SpecularColor;
			this._rootHSVColor = HSVColorPicker.RGBToHSV(this._rootColor.r, this._rootColor.g, this._rootColor.b);
			this._tipHSVColor = HSVColorPicker.RGBToHSV(this._tipColor.r, this._tipColor.g, this._tipColor.b);
			this._specularHSVColor = HSVColorPicker.RGBToHSV(this._specularColor.r, this._specularColor.g, this._specularColor.b);
			this.rootColorJSON = new JSONStorableColor("rootColor", this._rootHSVColor, new JSONStorableColor.SetHSVColorCallback(this.SyncRootColor));
			base.RegisterColor(this.rootColorJSON);
			this.tipColorJSON = new JSONStorableColor("tipColor", this._tipHSVColor, new JSONStorableColor.SetHSVColorCallback(this.SyncTipColor));
			base.RegisterColor(this.tipColorJSON);
			this.colorRolloffJSON = new JSONStorableFloat("colorRolloff", this.hairSettings.RenderSettings.RootTipColorProvider.ColorRolloff, new JSONStorableFloat.SetFloatCallback(this.SyncColorRolloff), 0f, 5f, true, true);
			base.RegisterFloat(this.colorRolloffJSON);
			this.specularColorJSON = new JSONStorableColor("specularColor", this._specularHSVColor, new JSONStorableColor.SetHSVColorCallback(this.SyncSpecularColor));
			base.RegisterColor(this.specularColorJSON);
			this.diffuseSoftnessJSON = new JSONStorableFloat("diffuseSoftness", this.hairSettings.RenderSettings.Diffuse, new JSONStorableFloat.SetFloatCallback(this.SyncDiffuseSoftness), 0f, 1f, true, true);
			base.RegisterFloat(this.diffuseSoftnessJSON);
			this.primarySpecularSharpnessJSON = new JSONStorableFloat("primarySpecularSharpness", this.hairSettings.RenderSettings.PrimarySpecular, new JSONStorableFloat.SetFloatCallback(this.SyncPrimarySpecularSharpness), 2f, 256f, true, true);
			base.RegisterFloat(this.primarySpecularSharpnessJSON);
			this.secondarySpecularSharpnessJSON = new JSONStorableFloat("secondarySpecularSharpness", this.hairSettings.RenderSettings.SecondarySpecular, new JSONStorableFloat.SetFloatCallback(this.SyncSecondarySpecularSharpness), 2f, 256f, true, true);
			base.RegisterFloat(this.secondarySpecularSharpnessJSON);
			this.specularShiftJSON = new JSONStorableFloat("specularShift", this.hairSettings.RenderSettings.SpecularShift, new JSONStorableFloat.SetFloatCallback(this.SyncSpecularShift), 0f, 1f, true, true);
			base.RegisterFloat(this.specularShiftJSON);
			this.fresnelPowerJSON = new JSONStorableFloat("fresnelPower", this.hairSettings.RenderSettings.FresnelPower, new JSONStorableFloat.SetFloatCallback(this.SyncFresnelPower), 0f, 10f, true, true);
			base.RegisterFloat(this.fresnelPowerJSON);
			this.fresnelAttenuationJSON = new JSONStorableFloat("fresnelAttenuation", this.hairSettings.RenderSettings.FresnelAttenuation, new JSONStorableFloat.SetFloatCallback(this.SyncFresnelAttenuation), 0f, 1f, true, true);
			base.RegisterFloat(this.fresnelAttenuationJSON);
			this.randomColorPowerJSON = new JSONStorableFloat("randomColorPower", this.hairSettings.RenderSettings.RandomTexColorPower, new JSONStorableFloat.SetFloatCallback(this.SyncRandomColorPower), 0f, 10f, true, true);
			base.RegisterFloat(this.randomColorPowerJSON);
			this.randomColorOffsetJSON = new JSONStorableFloat("randomColorOffset", this.hairSettings.RenderSettings.RandomTexColorOffset, new JSONStorableFloat.SetFloatCallback(this.SyncRandomColorOffset), 0f, 1f, true, true);
			base.RegisterFloat(this.randomColorOffsetJSON);
			this.IBLFactorJSON = new JSONStorableFloat("IBLFactor", this.hairSettings.RenderSettings.IBLFactor, new JSONStorableFloat.SetFloatCallback(this.SyncIBLFactor), 0f, 1f, true, true);
			base.RegisterFloat(this.IBLFactorJSON);
			this.normalRandomizeJSON = new JSONStorableFloat("normalRandomize", this.hairSettings.RenderSettings.NormalRandomize, new JSONStorableFloat.SetFloatCallback(this.SyncNormalRandomize), 0f, 1f, true, true);
			base.RegisterFloat(this.normalRandomizeJSON);
			this.copyLookParametersAction = new JSONStorableAction("CopyLookParameters", new JSONStorableAction.ActionCallback(this.CopyLookParameters));
			base.RegisterAction(this.copyLookParametersAction);
			this.pasteLookParametersAction = new JSONStorableAction("PasteLookParameters", new JSONStorableAction.ActionCallback(this.PasteLookParameters));
			base.RegisterAction(this.pasteLookParametersAction);
			this.undoPasteLookParametersAction = new JSONStorableAction("UndoPasteLookParameters", new JSONStorableAction.ActionCallback(this.UndoPasteLookParameters));
			base.RegisterAction(this.undoPasteLookParametersAction);
			this.curlXJSON = new JSONStorableFloat("curlX", this.hairSettings.RenderSettings.WavinessAxis.x, new JSONStorableFloat.SetFloatCallback(this.SyncCurlX), 0f, 1f, true, true);
			base.RegisterFloat(this.curlXJSON);
			this.curlYJSON = new JSONStorableFloat("curlY", this.hairSettings.RenderSettings.WavinessAxis.y, new JSONStorableFloat.SetFloatCallback(this.SyncCurlY), 0f, 1f, true, true);
			base.RegisterFloat(this.curlYJSON);
			this.curlZJSON = new JSONStorableFloat("curlZ", this.hairSettings.RenderSettings.WavinessAxis.z, new JSONStorableFloat.SetFloatCallback(this.SyncCurlZ), 0f, 1f, true, true);
			base.RegisterFloat(this.curlZJSON);
			this.curlScaleJSON = new JSONStorableFloat("curlScale", this.hairSettings.RenderSettings.WavinessScale, new JSONStorableFloat.SetFloatCallback(this.SyncCurlScale), 0f, 1f, true, true);
			base.RegisterFloat(this.curlScaleJSON);
			this.curlScaleRandomnessJSON = new JSONStorableFloat("curlScaleRandomness", this.hairSettings.RenderSettings.WavinessScaleRandomness, new JSONStorableFloat.SetFloatCallback(this.SyncCurlScaleRandomness), 0f, 2f, true, true);
			base.RegisterFloat(this.curlScaleRandomnessJSON);
			this.curlFrequencyJSON = new JSONStorableFloat("curlFrequency", this.hairSettings.RenderSettings.WavinessFrequency, new JSONStorableFloat.SetFloatCallback(this.SyncCurlFrequency), 0f, 20f, true, true);
			base.RegisterFloat(this.curlFrequencyJSON);
			this.curlFrequencyRandomnessJSON = new JSONStorableFloat("curlFrequencyRandomness", this.hairSettings.RenderSettings.WavinessFrequencyRandomness, new JSONStorableFloat.SetFloatCallback(this.SyncCurlFrequencyRandomness), 0f, 2f, true, true);
			base.RegisterFloat(this.curlFrequencyRandomnessJSON);
			this.curlAllowReverseJSON = new JSONStorableBool("curlAllowReverse", this.hairSettings.RenderSettings.WavinessAllowReverse, new JSONStorableBool.SetBoolCallback(this.SyncCurlAllowReverse));
			base.RegisterBool(this.curlAllowReverseJSON);
			this.curlAllowFlipAxisJSON = new JSONStorableBool("curlAllowFlipAxis", this.hairSettings.RenderSettings.WavinessAllowFlipAxis, new JSONStorableBool.SetBoolCallback(this.SyncCurlAllowFlipAxis));
			base.RegisterBool(this.curlAllowFlipAxisJSON);
			this.curlNormalAdjustJSON = new JSONStorableFloat("curlNormalAdjust", this.hairSettings.RenderSettings.WavinessNormalAdjust, new JSONStorableFloat.SetFloatCallback(this.SyncCurlNormalAdjust), -0.5f, 0.5f, true, true);
			base.RegisterFloat(this.curlNormalAdjustJSON);
			if (!this.hairSettings.RenderSettings.UseWavinessCurves)
			{
				this.curlRootJSON = new JSONStorableFloat("curlRoot", this.hairSettings.RenderSettings.WavinessRoot, new JSONStorableFloat.SetFloatCallback(this.SyncCurlRoot), 0f, 1f, true, true);
				base.RegisterFloat(this.curlRootJSON);
				this.curlMidJSON = new JSONStorableFloat("curlMid", this.hairSettings.RenderSettings.WavinessMid, new JSONStorableFloat.SetFloatCallback(this.SyncCurlMid), 0f, 1f, true, true);
				base.RegisterFloat(this.curlMidJSON);
				this.curlTipJSON = new JSONStorableFloat("curlTip", this.hairSettings.RenderSettings.WavinessTip, new JSONStorableFloat.SetFloatCallback(this.SyncCurlTip), 0f, 1f, true, true);
				base.RegisterFloat(this.curlTipJSON);
				this.curlMidpointJSON = new JSONStorableFloat("curlMidpoint", this.hairSettings.RenderSettings.WavinessMidpoint, new JSONStorableFloat.SetFloatCallback(this.SyncCurlMidpoint), 0f, 1f, true, true);
				base.RegisterFloat(this.curlMidpointJSON);
				this.curlCurvePowerJSON = new JSONStorableFloat("curlCurvePower", this.hairSettings.RenderSettings.WavinessCurvePower, new JSONStorableFloat.SetFloatCallback(this.SyncCurlCurvePower), 0f, 16f, true, true);
				base.RegisterFloat(this.curlCurvePowerJSON);
			}
			this.length1JSON = new JSONStorableFloat("length1", this.hairSettings.RenderSettings.Length1, new JSONStorableFloat.SetFloatCallback(this.SyncLength1), 0f, 1f, true, true);
			base.RegisterFloat(this.length1JSON);
			this.length2JSON = new JSONStorableFloat("length2", this.hairSettings.RenderSettings.Length2, new JSONStorableFloat.SetFloatCallback(this.SyncLength2), 0f, 1f, true, true);
			base.RegisterFloat(this.length2JSON);
			this.length3JSON = new JSONStorableFloat("length3", this.hairSettings.RenderSettings.Length3, new JSONStorableFloat.SetFloatCallback(this.SyncLength3), 0f, 1f, true, true);
			base.RegisterFloat(this.length3JSON);
			this.widthJSON = new JSONStorableFloat("width", this.hairSettings.LODSettings.FixedWidth, new JSONStorableFloat.SetFloatCallback(this.SyncWidth), 0f, 0.001f, true, true);
			base.RegisterFloat(this.widthJSON);
			this.SyncWidth(this.widthJSON.val);
			this.densityJSON = new JSONStorableFloat("curveDensity", (float)this.hairSettings.LODSettings.FixedDensity, new JSONStorableFloat.SetFloatCallback(this.SyncDensity), 2f, 64f, true, true);
			base.RegisterFloat(this.densityJSON);
			this.detailJSON = new JSONStorableFloat("hairMultiplier", (float)this.hairSettings.LODSettings.FixedDetail, new JSONStorableFloat.SetFloatCallback(this.SyncDetail), 1f, 64f, true, true);
			base.RegisterFloat(this.detailJSON);
			this.maxSpreadJSON = new JSONStorableFloat("maxSpread", this.hairSettings.RenderSettings.MaxSpread, new JSONStorableFloat.SetFloatCallback(this.SyncMaxSpread), 0f, 0.5f, true, true);
			base.RegisterFloat(this.maxSpreadJSON);
			if (!this.hairSettings.RenderSettings.UseInterpolationCurves)
			{
				this.spreadRootJSON = new JSONStorableFloat("spreadRoot", this.hairSettings.RenderSettings.InterpolationRoot, new JSONStorableFloat.SetFloatCallback(this.SyncSpreadRoot), 0f, 1f, true, true);
				base.RegisterFloat(this.spreadRootJSON);
				this.spreadMidJSON = new JSONStorableFloat("spreadMid", this.hairSettings.RenderSettings.InterpolationMid, new JSONStorableFloat.SetFloatCallback(this.SyncSpreadMid), 0f, 1f, true, true);
				base.RegisterFloat(this.spreadMidJSON);
				this.spreadTipJSON = new JSONStorableFloat("spreadTip", this.hairSettings.RenderSettings.InterpolationTip, new JSONStorableFloat.SetFloatCallback(this.SyncSpreadTip), 0f, 1f, true, true);
				base.RegisterFloat(this.spreadTipJSON);
				this.spreadMidpointJSON = new JSONStorableFloat("spreadMidpoint", this.hairSettings.RenderSettings.InterpolationMidpoint, new JSONStorableFloat.SetFloatCallback(this.SyncSpreadMidpoint), 0f, 1f, true, true);
				base.RegisterFloat(this.spreadMidpointJSON);
				this.spreadCurvePowerJSON = new JSONStorableFloat("spreadCurvePower", this.hairSettings.RenderSettings.InterpolationCurvePower, new JSONStorableFloat.SetFloatCallback(this.SyncSpreadCurvePower), 0f, 16f, true, true);
				base.RegisterFloat(this.spreadCurvePowerJSON);
			}
		}
	}

	// Token: 0x06006A06 RID: 27142 RVA: 0x0027D460 File Offset: 0x0027B860
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			HairSimControlUI componentInChildren = this.UITransform.GetComponentInChildren<HairSimControlUI>();
			if (componentInChildren != null)
			{
				this.restoreAllFromDefaultsAction.button = componentInChildren.restoreAllFromDefaultsButton;
				this.saveToStore1Action.button = componentInChildren.saveToStore1Button;
				this.restoreAllFromStore1Action.button = componentInChildren.restoreFromStore1Button;
				if (this.creator != null)
				{
					this.styleStatusText = componentInChildren.styleStatusText;
					this.styleModelPanel = componentInChildren.styleModePanel;
					this.simNearbyJointCountText = componentInChildren.simNearbyJointCountText;
					this.resetAndStartStyleModeAction.button = componentInChildren.resetAndStartStyleModeButton;
					this.startStyleModeAction.button = componentInChildren.startStyleModeButton;
					this.cancelStyleModeAction.button = componentInChildren.cancelStyleModeButton;
					this.keepStyleAction.button = componentInChildren.keepStyleButton;
					this.styleModeAllowControlOtherNodesJSON.toggle = componentInChildren.styleModelAllowControlOtherNodesToggle;
					this.styleJointsSearchDistanceJSON.slider = componentInChildren.styleJointsSearchDistanceSlider;
					this.rebuildStyleJointsAction.button = componentInChildren.rebuildStyleJointsButton;
					this.clearStyleJointsAction.button = componentInChildren.clearStyleJointsButton;
					this.styleModeCollisionRadiusJSON.slider = componentInChildren.styleModeCollisionRadiusSlider;
					if (this.styleModeCollisionRadiusRootJSON != null)
					{
						this.styleModeCollisionRadiusRootJSON.slider = componentInChildren.styleModeCollisionRadiusRootSlider;
					}
					this.styleModeGravityMultiplierJSON.slider = componentInChildren.styleModeGravityMultiplierSlider;
					this.styleModeShowCurlsJSON.toggle = componentInChildren.styleModeShowCurlsToggle;
					this.styleModeUpHairPullStrengthJSON.slider = componentInChildren.styleModeUpHairPullStrengthSlider;
					this.styleModeShowTool1JSON.toggle = componentInChildren.styleModeShowTool1Toggle;
					this.styleModeShowTool2JSON.toggle = componentInChildren.styleModeShowTool2Toggle;
					this.styleModeShowTool3JSON.toggle = componentInChildren.styleModeShowTool3Toggle;
					this.styleModeShowTool4JSON.toggle = componentInChildren.styleModeShowTool4Toggle;
					this.SyncStyleModeButtons();
				}
				this.copyPhysicsParametersAction.button = componentInChildren.copyPhysicsParametersButton;
				this.pastePhysicsParametersAction.button = componentInChildren.pastePhysicsParametersButton;
				this.undoPastePhysicsParametersAction.button = componentInChildren.undoPastePhysicsParametersButton;
				this.simulationEnabledJSON.toggle = componentInChildren.simulationEnabledToggle;
				this.collisionEnabledJSON.toggle = componentInChildren.collisionEnabledToggle;
				this.collisionRadiusJSON.slider = componentInChildren.collisionRadiusSlider;
				if (this.collisionRadiusRootJSON != null)
				{
					this.collisionRadiusRootJSON.slider = componentInChildren.collisionRadiusRootSlider;
					if (componentInChildren.collisionRadiusRootSlider != null)
					{
						componentInChildren.collisionRadiusRootSlider.transform.parent.gameObject.SetActive(true);
					}
				}
				else if (componentInChildren.collisionRadiusRootSlider != null)
				{
					componentInChildren.collisionRadiusRootSlider.transform.parent.gameObject.SetActive(false);
				}
				this.dragJSON.slider = componentInChildren.dragSlider;
				this.usePaintedRigidityJSON.toggle = componentInChildren.usePaintedRigidityToggle;
				this.rootRigidityJSON.slider = componentInChildren.rootRigiditySlider;
				this.mainRigidityJSON.slider = componentInChildren.mainRigiditySlider;
				this.tipRigidityJSON.slider = componentInChildren.tipRigiditySlider;
				this.rigidityRolloffPowerJSON.slider = componentInChildren.rigidityRolloffPowerSlider;
				this.paintedRigidityIndicatorPanel = componentInChildren.paintedRigidityIndicatorPanel;
				this.SyncUsePaintedRigidity(this.usePaintedRigidityJSON.val);
				if (this.jointRigidityJSON != null)
				{
					if (componentInChildren.jointRigiditySlider != null)
					{
						this.jointRigidityJSON.slider = componentInChildren.jointRigiditySlider;
						componentInChildren.jointRigiditySlider.transform.parent.gameObject.SetActive(true);
					}
				}
				else if (componentInChildren.jointRigiditySlider != null)
				{
					componentInChildren.jointRigiditySlider.transform.parent.gameObject.SetActive(false);
				}
				this.frictionJSON.slider = componentInChildren.frictionSlider;
				this.gravityMultiplierJSON.slider = componentInChildren.gravityMultiplierSlider;
				this.weightJSON.slider = componentInChildren.weightSlider;
				this.iterationsJSON.slider = componentInChildren.iterationsSlider;
				this.clingJSON.slider = componentInChildren.clingSlider;
				this.clingRolloffJSON.slider = componentInChildren.clingRolloffSlider;
				this.snapJSON.slider = componentInChildren.snapSlider;
				this.bendResistanceJSON.slider = componentInChildren.bendResistanceSlider;
				this.windJSON.sliderX = componentInChildren.windXSlider;
				this.windJSON.sliderY = componentInChildren.windYSlider;
				this.windJSON.sliderZ = componentInChildren.windZSlider;
				this.copyLightingParametersAction.button = componentInChildren.copyLightingParametersButton;
				this.pasteLightingParametersAction.button = componentInChildren.pasteLightingParametersButton;
				this.undoPasteLightingParametersAction.button = componentInChildren.undoPasteLightingParametersButton;
				this.shaderTypeJSON.popup = componentInChildren.shaderTypePopup;
				this.rootColorJSON.colorPicker = componentInChildren.rootColorPicker;
				this.tipColorJSON.colorPicker = componentInChildren.tipColorPicker;
				this.colorRolloffJSON.slider = componentInChildren.colorRolloffSlider;
				this.specularColorJSON.colorPicker = componentInChildren.specularColorPicker;
				this.diffuseSoftnessJSON.slider = componentInChildren.diffuseSoftnessSlider;
				this.primarySpecularSharpnessJSON.slider = componentInChildren.primarySpecularSharpnessSlider;
				this.secondarySpecularSharpnessJSON.slider = componentInChildren.secondarySpecularSharpnessSlider;
				this.specularShiftJSON.slider = componentInChildren.specularShiftSlider;
				this.fresnelPowerJSON.slider = componentInChildren.fresnelPowerSlider;
				this.fresnelAttenuationJSON.slider = componentInChildren.fresnelAttenuationSlider;
				this.randomColorPowerJSON.slider = componentInChildren.randomColorPowerSlider;
				this.randomColorOffsetJSON.slider = componentInChildren.randomColorOffsetSlider;
				this.IBLFactorJSON.slider = componentInChildren.IBLFactorSlider;
				this.normalRandomizeJSON.slider = componentInChildren.normalRandomizeSlider;
				this.copyLookParametersAction.button = componentInChildren.copyLookParametersButton;
				this.pasteLookParametersAction.button = componentInChildren.pasteLookParametersButton;
				this.undoPasteLookParametersAction.button = componentInChildren.undoPasteLookParametersButton;
				this.curlXJSON.slider = componentInChildren.curlXSlider;
				this.curlYJSON.slider = componentInChildren.curlYSlider;
				this.curlZJSON.slider = componentInChildren.curlZSlider;
				this.curlScaleJSON.slider = componentInChildren.curlScaleSlider;
				this.curlFrequencyJSON.slider = componentInChildren.curlFrequencySlider;
				this.curlScaleRandomnessJSON.slider = componentInChildren.curlScaleRandomnessSlider;
				this.curlFrequencyRandomnessJSON.slider = componentInChildren.curlFrequencyRandomnessSlider;
				this.curlAllowReverseJSON.toggle = componentInChildren.curlAllowReverseToggle;
				this.curlAllowFlipAxisJSON.toggle = componentInChildren.curlAllowFlipAxisToggle;
				this.curlNormalAdjustJSON.slider = componentInChildren.curlNormalAdjustSlider;
				if (this.curlRootJSON != null)
				{
					this.curlRootJSON.slider = componentInChildren.curlRootSlider;
				}
				if (this.curlMidJSON != null)
				{
					this.curlMidJSON.slider = componentInChildren.curlMidSlider;
				}
				if (this.curlTipJSON != null)
				{
					this.curlTipJSON.slider = componentInChildren.curlTipSlider;
				}
				if (this.curlMidpointJSON != null)
				{
					this.curlMidpointJSON.slider = componentInChildren.curlMidpointSlider;
				}
				if (this.curlCurvePowerJSON != null)
				{
					this.curlCurvePowerJSON.slider = componentInChildren.curlCurvePowerSlider;
				}
				this.length1JSON.slider = componentInChildren.length1Slider;
				this.length2JSON.slider = componentInChildren.length2Slider;
				this.length3JSON.slider = componentInChildren.length3Slider;
				this.widthJSON.slider = componentInChildren.widthSlider;
				this.densityJSON.slider = componentInChildren.densitySlider;
				this.detailJSON.slider = componentInChildren.detailSlider;
				this.maxSpreadJSON.slider = componentInChildren.maxSpreadSlider;
				if (this.spreadRootJSON != null)
				{
					this.spreadRootJSON.slider = componentInChildren.spreadRootSlider;
				}
				if (this.spreadMidJSON != null)
				{
					this.spreadMidJSON.slider = componentInChildren.spreadMidSlider;
				}
				if (this.spreadTipJSON != null)
				{
					this.spreadTipJSON.slider = componentInChildren.spreadTipSlider;
				}
				if (this.spreadMidpointJSON != null)
				{
					this.spreadMidpointJSON.slider = componentInChildren.spreadMidpointSlider;
				}
				if (this.spreadCurvePowerJSON != null)
				{
					this.spreadCurvePowerJSON.slider = componentInChildren.spreadCurvePowerSlider;
				}
			}
		}
	}

	// Token: 0x06006A07 RID: 27143 RVA: 0x0027DC64 File Offset: 0x0027C064
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			HairSimControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<HairSimControlUI>();
			if (componentInChildren != null)
			{
				this.copyPhysicsParametersAction.buttonAlt = componentInChildren.copyPhysicsParametersButton;
				this.pastePhysicsParametersAction.buttonAlt = componentInChildren.pastePhysicsParametersButton;
				this.undoPastePhysicsParametersAction.buttonAlt = componentInChildren.undoPastePhysicsParametersButton;
				this.simulationEnabledJSON.toggleAlt = componentInChildren.simulationEnabledToggle;
				this.collisionEnabledJSON.toggleAlt = componentInChildren.collisionEnabledToggle;
				this.collisionRadiusJSON.sliderAlt = componentInChildren.collisionRadiusSlider;
				this.dragJSON.sliderAlt = componentInChildren.dragSlider;
				this.usePaintedRigidityJSON.toggleAlt = componentInChildren.usePaintedRigidityToggle;
				this.rootRigidityJSON.sliderAlt = componentInChildren.rootRigiditySlider;
				this.mainRigidityJSON.sliderAlt = componentInChildren.mainRigiditySlider;
				this.tipRigidityJSON.sliderAlt = componentInChildren.tipRigiditySlider;
				this.rigidityRolloffPowerJSON.sliderAlt = componentInChildren.rigidityRolloffPowerSlider;
				this.SyncUsePaintedRigidity(this.usePaintedRigidityJSON.val);
				if (this.jointRigidityJSON != null)
				{
					if (componentInChildren.jointRigiditySlider != null)
					{
						this.jointRigidityJSON.sliderAlt = componentInChildren.jointRigiditySlider;
						componentInChildren.jointRigiditySlider.transform.parent.gameObject.SetActive(true);
					}
				}
				else if (componentInChildren.jointRigiditySlider != null)
				{
					componentInChildren.jointRigiditySlider.transform.parent.gameObject.SetActive(false);
				}
				this.frictionJSON.sliderAlt = componentInChildren.frictionSlider;
				this.gravityMultiplierJSON.sliderAlt = componentInChildren.gravityMultiplierSlider;
				this.weightJSON.sliderAlt = componentInChildren.weightSlider;
				this.iterationsJSON.sliderAlt = componentInChildren.iterationsSlider;
				this.clingJSON.sliderAlt = componentInChildren.clingSlider;
				this.clingRolloffJSON.sliderAlt = componentInChildren.clingRolloffSlider;
				this.snapJSON.sliderAlt = componentInChildren.snapSlider;
				this.bendResistanceJSON.sliderAlt = componentInChildren.bendResistanceSlider;
				this.windJSON.sliderXAlt = componentInChildren.windXSlider;
				this.windJSON.sliderYAlt = componentInChildren.windYSlider;
				this.windJSON.sliderZAlt = componentInChildren.windZSlider;
				this.copyLightingParametersAction.buttonAlt = componentInChildren.copyLightingParametersButton;
				this.pasteLightingParametersAction.buttonAlt = componentInChildren.pasteLightingParametersButton;
				this.undoPasteLightingParametersAction.buttonAlt = componentInChildren.undoPasteLightingParametersButton;
				this.shaderTypeJSON.popupAlt = componentInChildren.shaderTypePopup;
				this.rootColorJSON.colorPickerAlt = componentInChildren.rootColorPicker;
				this.tipColorJSON.colorPickerAlt = componentInChildren.tipColorPicker;
				this.colorRolloffJSON.sliderAlt = componentInChildren.colorRolloffSlider;
				this.specularColorJSON.colorPickerAlt = componentInChildren.specularColorPicker;
				this.diffuseSoftnessJSON.sliderAlt = componentInChildren.diffuseSoftnessSlider;
				this.primarySpecularSharpnessJSON.sliderAlt = componentInChildren.primarySpecularSharpnessSlider;
				this.secondarySpecularSharpnessJSON.sliderAlt = componentInChildren.secondarySpecularSharpnessSlider;
				this.specularShiftJSON.sliderAlt = componentInChildren.specularShiftSlider;
				this.fresnelPowerJSON.sliderAlt = componentInChildren.fresnelPowerSlider;
				this.fresnelAttenuationJSON.sliderAlt = componentInChildren.fresnelAttenuationSlider;
				this.randomColorPowerJSON.sliderAlt = componentInChildren.randomColorPowerSlider;
				this.randomColorOffsetJSON.sliderAlt = componentInChildren.randomColorOffsetSlider;
				this.IBLFactorJSON.sliderAlt = componentInChildren.IBLFactorSlider;
				this.normalRandomizeJSON.sliderAlt = componentInChildren.normalRandomizeSlider;
				this.copyLookParametersAction.buttonAlt = componentInChildren.copyLookParametersButton;
				this.pasteLookParametersAction.buttonAlt = componentInChildren.pasteLookParametersButton;
				this.undoPasteLookParametersAction.buttonAlt = componentInChildren.undoPasteLookParametersButton;
				this.curlXJSON.sliderAlt = componentInChildren.curlXSlider;
				this.curlYJSON.sliderAlt = componentInChildren.curlYSlider;
				this.curlZJSON.sliderAlt = componentInChildren.curlZSlider;
				this.curlScaleJSON.sliderAlt = componentInChildren.curlScaleSlider;
				this.curlFrequencyJSON.sliderAlt = componentInChildren.curlFrequencySlider;
				this.curlScaleRandomnessJSON.sliderAlt = componentInChildren.curlScaleRandomnessSlider;
				this.curlFrequencyRandomnessJSON.sliderAlt = componentInChildren.curlFrequencyRandomnessSlider;
				this.curlAllowReverseJSON.toggleAlt = componentInChildren.curlAllowReverseToggle;
				this.curlAllowFlipAxisJSON.toggleAlt = componentInChildren.curlAllowFlipAxisToggle;
				this.curlNormalAdjustJSON.sliderAlt = componentInChildren.curlNormalAdjustSlider;
				if (this.curlRootJSON != null)
				{
					this.curlRootJSON.sliderAlt = componentInChildren.curlRootSlider;
				}
				if (this.curlMidJSON != null)
				{
					this.curlMidJSON.sliderAlt = componentInChildren.curlMidSlider;
				}
				if (this.curlTipJSON != null)
				{
					this.curlTipJSON.sliderAlt = componentInChildren.curlTipSlider;
				}
				if (this.curlMidpointJSON != null)
				{
					this.curlMidpointJSON.sliderAlt = componentInChildren.curlMidpointSlider;
				}
				if (this.curlCurvePowerJSON != null)
				{
					this.curlCurvePowerJSON.sliderAlt = componentInChildren.curlCurvePowerSlider;
				}
				this.length1JSON.sliderAlt = componentInChildren.length1Slider;
				this.length2JSON.sliderAlt = componentInChildren.length2Slider;
				this.length3JSON.sliderAlt = componentInChildren.length3Slider;
				this.widthJSON.sliderAlt = componentInChildren.widthSlider;
				this.densityJSON.sliderAlt = componentInChildren.densitySlider;
				this.detailJSON.sliderAlt = componentInChildren.detailSlider;
				this.maxSpreadJSON.sliderAlt = componentInChildren.maxSpreadSlider;
				if (this.spreadRootJSON != null)
				{
					this.spreadRootJSON.sliderAlt = componentInChildren.spreadRootSlider;
				}
				if (this.spreadMidJSON != null)
				{
					this.spreadMidJSON.sliderAlt = componentInChildren.spreadMidSlider;
				}
				if (this.spreadTipJSON != null)
				{
					this.spreadTipJSON.sliderAlt = componentInChildren.spreadTipSlider;
				}
				if (this.spreadMidpointJSON != null)
				{
					this.spreadMidpointJSON.sliderAlt = componentInChildren.spreadMidpointSlider;
				}
				if (this.spreadCurvePowerJSON != null)
				{
					this.spreadCurvePowerJSON.sliderAlt = componentInChildren.spreadCurvePowerSlider;
				}
			}
		}
	}

	// Token: 0x06006A08 RID: 27144 RVA: 0x0027E24C File Offset: 0x0027C64C
	protected void DeregisterUI()
	{
		if (this.creator != null)
		{
			this.simNearbyJointCountText = null;
			this.resetAndStartStyleModeAction.button = null;
			this.startStyleModeAction.button = null;
			this.cancelStyleModeAction.button = null;
			this.keepStyleAction.button = null;
			this.styleModeAllowControlOtherNodesJSON.toggle = null;
			this.styleJointsSearchDistanceJSON.slider = null;
			this.rebuildStyleJointsAction.button = null;
			this.clearStyleJointsAction.button = null;
			this.styleModeGravityMultiplierJSON.slider = null;
			this.styleModeShowCurlsJSON.toggle = null;
			this.styleModeUpHairPullStrengthJSON.slider = null;
			this.styleModeCollisionRadiusJSON.slider = null;
			if (this.styleModeCollisionRadiusRootJSON != null)
			{
				this.styleModeCollisionRadiusRootJSON.slider = null;
			}
			this.styleModeShowTool1JSON.toggle = null;
			this.styleModeShowTool2JSON.toggle = null;
			this.styleModeShowTool3JSON.toggle = null;
			this.styleModeShowTool4JSON.toggle = null;
		}
		this.simulationEnabledJSON.toggle = null;
		this.collisionEnabledJSON.toggle = null;
		this.collisionRadiusJSON.slider = null;
		if (this.collisionRadiusRootJSON != null)
		{
			this.collisionRadiusRootJSON.slider = null;
		}
		this.dragJSON.slider = null;
		this.usePaintedRigidityJSON.toggle = null;
		this.rootRigidityJSON.slider = null;
		this.mainRigidityJSON.slider = null;
		this.tipRigidityJSON.slider = null;
		this.rigidityRolloffPowerJSON.slider = null;
		if (this.jointRigidityJSON != null)
		{
			this.jointRigidityJSON.slider = null;
		}
		this.frictionJSON.slider = null;
		this.gravityMultiplierJSON.slider = null;
		this.weightJSON.slider = null;
		this.iterationsJSON.slider = null;
		this.clingJSON.slider = null;
		this.clingRolloffJSON.slider = null;
		this.snapJSON.slider = null;
		this.bendResistanceJSON.slider = null;
		this.windJSON.sliderX = null;
		this.windJSON.sliderY = null;
		this.windJSON.sliderZ = null;
		this.shaderTypeJSON.popup = null;
		this.rootColorJSON.colorPicker = null;
		this.tipColorJSON.colorPicker = null;
		this.colorRolloffJSON.slider = null;
		this.specularColorJSON.colorPicker = null;
		this.diffuseSoftnessJSON.slider = null;
		this.primarySpecularSharpnessJSON.slider = null;
		this.secondarySpecularSharpnessJSON.slider = null;
		this.specularShiftJSON.slider = null;
		this.fresnelPowerJSON.slider = null;
		this.fresnelAttenuationJSON.slider = null;
		this.randomColorPowerJSON.slider = null;
		this.randomColorOffsetJSON.slider = null;
		this.IBLFactorJSON.slider = null;
		this.normalRandomizeJSON.slider = null;
		this.curlXJSON.slider = null;
		this.curlYJSON.slider = null;
		this.curlZJSON.slider = null;
		this.curlScaleJSON.slider = null;
		this.curlFrequencyJSON.slider = null;
		this.curlScaleRandomnessJSON.slider = null;
		this.curlFrequencyRandomnessJSON.slider = null;
		this.curlAllowReverseJSON.toggle = null;
		this.curlAllowFlipAxisJSON.toggle = null;
		this.curlNormalAdjustJSON.slider = null;
		if (this.curlRootJSON != null)
		{
			this.curlRootJSON.slider = null;
		}
		if (this.curlMidJSON != null)
		{
			this.curlMidJSON.slider = null;
		}
		if (this.curlTipJSON != null)
		{
			this.curlTipJSON.slider = null;
		}
		if (this.curlMidpointJSON != null)
		{
			this.curlMidpointJSON.slider = null;
		}
		if (this.curlCurvePowerJSON != null)
		{
			this.curlCurvePowerJSON.slider = null;
		}
		this.length1JSON.slider = null;
		this.length2JSON.slider = null;
		this.length3JSON.slider = null;
		this.widthJSON.slider = null;
		this.densityJSON.slider = null;
		this.detailJSON.slider = null;
		this.maxSpreadJSON.slider = null;
		if (this.spreadRootJSON != null)
		{
			this.spreadRootJSON.slider = null;
		}
		if (this.spreadMidJSON != null)
		{
			this.spreadMidJSON.slider = null;
		}
		if (this.spreadTipJSON != null)
		{
			this.spreadTipJSON.slider = null;
		}
		if (this.spreadMidpointJSON != null)
		{
			this.spreadMidpointJSON.slider = null;
		}
		if (this.spreadCurvePowerJSON != null)
		{
			this.spreadCurvePowerJSON.slider = null;
		}
		this.simulationEnabledJSON.toggleAlt = null;
		this.collisionEnabledJSON.toggleAlt = null;
		this.collisionRadiusJSON.sliderAlt = null;
		this.dragJSON.sliderAlt = null;
		this.usePaintedRigidityJSON.toggleAlt = null;
		this.rootRigidityJSON.sliderAlt = null;
		this.mainRigidityJSON.sliderAlt = null;
		this.tipRigidityJSON.sliderAlt = null;
		this.rigidityRolloffPowerJSON.sliderAlt = null;
		if (this.jointRigidityJSON != null)
		{
			this.jointRigidityJSON.sliderAlt = null;
		}
		this.frictionJSON.sliderAlt = null;
		this.gravityMultiplierJSON.sliderAlt = null;
		this.weightJSON.sliderAlt = null;
		this.iterationsJSON.sliderAlt = null;
		this.clingJSON.sliderAlt = null;
		this.clingRolloffJSON.sliderAlt = null;
		this.snapJSON.sliderAlt = null;
		this.bendResistanceJSON.sliderAlt = null;
		this.shaderTypeJSON.popupAlt = null;
		this.rootColorJSON.colorPickerAlt = null;
		this.tipColorJSON.colorPickerAlt = null;
		this.colorRolloffJSON.sliderAlt = null;
		this.specularColorJSON.colorPickerAlt = null;
		this.diffuseSoftnessJSON.sliderAlt = null;
		this.primarySpecularSharpnessJSON.sliderAlt = null;
		this.secondarySpecularSharpnessJSON.sliderAlt = null;
		this.specularShiftJSON.sliderAlt = null;
		this.fresnelPowerJSON.sliderAlt = null;
		this.fresnelAttenuationJSON.sliderAlt = null;
		this.randomColorPowerJSON.sliderAlt = null;
		this.randomColorOffsetJSON.sliderAlt = null;
		this.IBLFactorJSON.sliderAlt = null;
		this.normalRandomizeJSON.sliderAlt = null;
		this.curlXJSON.sliderAlt = null;
		this.curlYJSON.sliderAlt = null;
		this.curlZJSON.sliderAlt = null;
		this.curlScaleJSON.sliderAlt = null;
		this.curlFrequencyJSON.sliderAlt = null;
		this.curlScaleRandomnessJSON.sliderAlt = null;
		this.curlFrequencyRandomnessJSON.sliderAlt = null;
		this.curlAllowReverseJSON.toggleAlt = null;
		this.curlAllowFlipAxisJSON.toggleAlt = null;
		this.curlNormalAdjustJSON.slider = null;
		if (this.curlRootJSON != null)
		{
			this.curlRootJSON.sliderAlt = null;
		}
		if (this.curlMidJSON != null)
		{
			this.curlMidJSON.sliderAlt = null;
		}
		if (this.curlTipJSON != null)
		{
			this.curlTipJSON.sliderAlt = null;
		}
		if (this.curlMidpointJSON != null)
		{
			this.curlMidpointJSON.sliderAlt = null;
		}
		if (this.curlCurvePowerJSON != null)
		{
			this.curlCurvePowerJSON.sliderAlt = null;
		}
		this.length1JSON.sliderAlt = null;
		this.length2JSON.sliderAlt = null;
		this.length3JSON.sliderAlt = null;
		this.widthJSON.sliderAlt = null;
		this.densityJSON.sliderAlt = null;
		this.detailJSON.sliderAlt = null;
		this.maxSpreadJSON.sliderAlt = null;
		if (this.spreadRootJSON != null)
		{
			this.spreadRootJSON.sliderAlt = null;
		}
		if (this.spreadMidJSON != null)
		{
			this.spreadMidJSON.sliderAlt = null;
		}
		if (this.spreadTipJSON != null)
		{
			this.spreadTipJSON.sliderAlt = null;
		}
		if (this.spreadMidpointJSON != null)
		{
			this.spreadMidpointJSON.sliderAlt = null;
		}
		if (this.spreadCurvePowerJSON != null)
		{
			this.spreadCurvePowerJSON.sliderAlt = null;
		}
	}

	// Token: 0x06006A09 RID: 27145 RVA: 0x0027EA15 File Offset: 0x0027CE15
	public override void SetUI(Transform t)
	{
		if (this.UITransform != t)
		{
			this.UITransform = t;
			if (base.isActiveAndEnabled)
			{
				this.InitUI();
			}
		}
	}

	// Token: 0x06006A0A RID: 27146 RVA: 0x0027EA40 File Offset: 0x0027CE40
	public override void SetUIAlt(Transform t)
	{
		if (this.UITransformAlt != t)
		{
			this.UITransformAlt = t;
			if (base.isActiveAndEnabled)
			{
				this.InitUIAlt();
			}
		}
	}

	// Token: 0x06006A0B RID: 27147 RVA: 0x0027EA6B File Offset: 0x0027CE6B
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
		}
	}

	// Token: 0x06006A0C RID: 27148 RVA: 0x0027EA84 File Offset: 0x0027CE84
	private void OnEnable()
	{
		this.InitUI();
		this.InitUIAlt();
	}

	// Token: 0x06006A0D RID: 27149 RVA: 0x0027EA92 File Offset: 0x0027CE92
	private void OnDisable()
	{
		this.DeregisterUI();
		if (this.isRebuildingStyleJoints)
		{
			this.AbortRebuildStyleJointsThread(false);
			this.isRebuildingStyleJoints = false;
		}
		this.CancelStyleMode();
	}

	// Token: 0x06006A0E RID: 27150 RVA: 0x0027EABC File Offset: 0x0027CEBC
	protected override void Update()
	{
		base.Update();
		if (this._resetSimulation && this.hairSettings != null && this.hairSettings.HairBuidCommand != null && this.hairSettings.HairBuidCommand.physics != null)
		{
			this.hairSettings.HairBuidCommand.physics.ResetPhysics();
		}
		this.SyncWind();
		if (this._styleMode && SuperController.singleton != null)
		{
			HairSimControlTools hairSimControlTools = this.hairSimControlTools;
			if (hairSimControlTools != null)
			{
				if (hairSimControlTools.StyleTool1 != null)
				{
					hairSimControlTools.StyleTool1.ResetToolStrengthMultiplier();
				}
				if (hairSimControlTools.StyleTool2 != null)
				{
					hairSimControlTools.StyleTool2.ResetToolStrengthMultiplier();
				}
				if (hairSimControlTools.StyleTool3 != null)
				{
					hairSimControlTools.StyleTool3.ResetToolStrengthMultiplier();
				}
				if (hairSimControlTools.StyleTool4 != null)
				{
					hairSimControlTools.StyleTool4.ResetToolStrengthMultiplier();
				}
				FreeControllerV3 rightFullGrabbedController = SuperController.singleton.RightFullGrabbedController;
				if (rightFullGrabbedController != null)
				{
					HairSimStyleToolControl hairSimStyleToolControl = null;
					if (rightFullGrabbedController == hairSimControlTools.hairStyleTool1Controller)
					{
						hairSimStyleToolControl = hairSimControlTools.StyleTool1;
					}
					else if (rightFullGrabbedController == hairSimControlTools.hairStyleTool2Controller)
					{
						hairSimStyleToolControl = hairSimControlTools.StyleTool2;
					}
					else if (rightFullGrabbedController == hairSimControlTools.hairStyleTool3Controller)
					{
						hairSimStyleToolControl = hairSimControlTools.StyleTool3;
					}
					else if (rightFullGrabbedController == hairSimControlTools.hairStyleTool4Controller)
					{
						hairSimStyleToolControl = hairSimControlTools.StyleTool4;
					}
					if (hairSimStyleToolControl != null)
					{
						hairSimStyleToolControl.SetToolStrengthMultiplier(SuperController.singleton.GetRightGrabVal());
					}
				}
				FreeControllerV3 leftFullGrabbedController = SuperController.singleton.LeftFullGrabbedController;
				if (leftFullGrabbedController != null)
				{
					HairSimStyleToolControl hairSimStyleToolControl2 = null;
					if (leftFullGrabbedController == hairSimControlTools.hairStyleTool1Controller)
					{
						hairSimStyleToolControl2 = hairSimControlTools.StyleTool1;
					}
					else if (leftFullGrabbedController == hairSimControlTools.hairStyleTool2Controller)
					{
						hairSimStyleToolControl2 = hairSimControlTools.StyleTool2;
					}
					else if (leftFullGrabbedController == hairSimControlTools.hairStyleTool3Controller)
					{
						hairSimStyleToolControl2 = hairSimControlTools.StyleTool3;
					}
					else if (leftFullGrabbedController == hairSimControlTools.hairStyleTool4Controller)
					{
						hairSimStyleToolControl2 = hairSimControlTools.StyleTool4;
					}
					if (hairSimStyleToolControl2 != null)
					{
						hairSimStyleToolControl2.SetToolStrengthMultiplier(SuperController.singleton.GetLeftGrabVal());
					}
				}
			}
		}
	}

	// Token: 0x06006A0F RID: 27151 RVA: 0x0027ED1E File Offset: 0x0027D11E
	// Note: this type is marked as 'beforefieldinit'.
	static HairSimControl()
	{
	}

	// Token: 0x06006A10 RID: 27152 RVA: 0x0027ED20 File Offset: 0x0027D120
	[CompilerGenerated]
	private static int <SyncStyleText>m__0(Vector4ListContainer container)
	{
		return container.List.Count;
	}

	// Token: 0x04005A97 RID: 23191
	public HairSettings hairSettings;

	// Token: 0x04005A98 RID: 23192
	public RuntimeHairGeometryCreator creator;

	// Token: 0x04005A99 RID: 23193
	protected bool _renderSuspend;

	// Token: 0x04005A9A RID: 23194
	protected bool _styleMode;

	// Token: 0x04005A9B RID: 23195
	protected Text simNearbyJointCountText;

	// Token: 0x04005A9C RID: 23196
	protected Transform styleModelPanel;

	// Token: 0x04005A9D RID: 23197
	protected JSONStorableAction startStyleModeAction;

	// Token: 0x04005A9E RID: 23198
	protected JSONStorableAction resetAndStartStyleModeAction;

	// Token: 0x04005A9F RID: 23199
	protected JSONStorableAction cancelStyleModeAction;

	// Token: 0x04005AA0 RID: 23200
	protected JSONStorableAction keepStyleAction;

	// Token: 0x04005AA1 RID: 23201
	protected JSONStorableBool styleModeAllowControlOtherNodesJSON;

	// Token: 0x04005AA2 RID: 23202
	protected JSONStorableFloat styleJointsSearchDistanceJSON;

	// Token: 0x04005AA3 RID: 23203
	protected JSONStorableAction clearStyleJointsAction;

	// Token: 0x04005AA4 RID: 23204
	protected Text styleStatusText;

	// Token: 0x04005AA5 RID: 23205
	protected Thread rebuildStyleJointsThread;

	// Token: 0x04005AA6 RID: 23206
	protected string threadError;

	// Token: 0x04005AA7 RID: 23207
	protected bool isRebuildingStyleJoints;

	// Token: 0x04005AA8 RID: 23208
	protected JSONStorableAction rebuildStyleJointsAction;

	// Token: 0x04005AA9 RID: 23209
	protected JSONStorableFloat styleModeGravityMultiplierJSON;

	// Token: 0x04005AAA RID: 23210
	protected JSONStorableBool styleModeShowCurlsJSON;

	// Token: 0x04005AAB RID: 23211
	protected JSONStorableFloat styleModeUpHairPullStrengthJSON;

	// Token: 0x04005AAC RID: 23212
	protected JSONStorableFloat styleModeCollisionRadiusJSON;

	// Token: 0x04005AAD RID: 23213
	protected JSONStorableFloat styleModeCollisionRadiusRootJSON;

	// Token: 0x04005AAE RID: 23214
	protected HairSimControlTools _hairSimControlTools;

	// Token: 0x04005AAF RID: 23215
	protected JSONStorableBool styleModeShowTool1JSON;

	// Token: 0x04005AB0 RID: 23216
	protected JSONStorableBool styleModeShowTool2JSON;

	// Token: 0x04005AB1 RID: 23217
	protected JSONStorableBool styleModeShowTool3JSON;

	// Token: 0x04005AB2 RID: 23218
	protected JSONStorableBool styleModeShowTool4JSON;

	// Token: 0x04005AB3 RID: 23219
	public static bool hasPhysicsCopyData;

	// Token: 0x04005AB4 RID: 23220
	public JSONStorableAction copyPhysicsParametersAction;

	// Token: 0x04005AB5 RID: 23221
	protected bool hasPhysicsPasteData;

	// Token: 0x04005AB6 RID: 23222
	public JSONStorableAction pastePhysicsParametersAction;

	// Token: 0x04005AB7 RID: 23223
	public JSONStorableAction undoPastePhysicsParametersAction;

	// Token: 0x04005AB8 RID: 23224
	protected JSONStorableBool simulationEnabledJSON;

	// Token: 0x04005AB9 RID: 23225
	public static bool copiedCollisionEnabled;

	// Token: 0x04005ABA RID: 23226
	protected bool pasteUndoCollisionEnabled;

	// Token: 0x04005ABB RID: 23227
	protected JSONStorableBool collisionEnabledJSON;

	// Token: 0x04005ABC RID: 23228
	public static float copiedCollisionRadius;

	// Token: 0x04005ABD RID: 23229
	protected float pasteUndoCollisionRadius;

	// Token: 0x04005ABE RID: 23230
	protected JSONStorableFloat collisionRadiusJSON;

	// Token: 0x04005ABF RID: 23231
	public static float copiedCollisionRadiusRoot;

	// Token: 0x04005AC0 RID: 23232
	protected float pasteUndoCollisionRadiusRoot;

	// Token: 0x04005AC1 RID: 23233
	protected JSONStorableFloat collisionRadiusRootJSON;

	// Token: 0x04005AC2 RID: 23234
	public static float copiedDrag;

	// Token: 0x04005AC3 RID: 23235
	protected float pasteUndoDrag;

	// Token: 0x04005AC4 RID: 23236
	protected JSONStorableFloat dragJSON;

	// Token: 0x04005AC5 RID: 23237
	public static float copiedFriction;

	// Token: 0x04005AC6 RID: 23238
	protected float pasteUndoFriction;

	// Token: 0x04005AC7 RID: 23239
	protected JSONStorableFloat frictionJSON;

	// Token: 0x04005AC8 RID: 23240
	public static float copiedGravityMultiplier;

	// Token: 0x04005AC9 RID: 23241
	protected float pasteUndoGravityMultiplier;

	// Token: 0x04005ACA RID: 23242
	protected JSONStorableFloat gravityMultiplierJSON;

	// Token: 0x04005ACB RID: 23243
	public static float copiedWeight;

	// Token: 0x04005ACC RID: 23244
	protected float pasteUndoWeight;

	// Token: 0x04005ACD RID: 23245
	protected JSONStorableFloat weightJSON;

	// Token: 0x04005ACE RID: 23246
	public static float copiedIterations;

	// Token: 0x04005ACF RID: 23247
	protected float pasteUndoIterations;

	// Token: 0x04005AD0 RID: 23248
	protected JSONStorableFloat iterationsJSON;

	// Token: 0x04005AD1 RID: 23249
	protected RectTransform paintedRigidityIndicatorPanel;

	// Token: 0x04005AD2 RID: 23250
	protected JSONStorableBool usePaintedRigidityJSON;

	// Token: 0x04005AD3 RID: 23251
	public static float copiedRootRigidity;

	// Token: 0x04005AD4 RID: 23252
	protected float pasteUndoRootRigidity;

	// Token: 0x04005AD5 RID: 23253
	protected JSONStorableFloat rootRigidityJSON;

	// Token: 0x04005AD6 RID: 23254
	public static float copiedMainRigidity;

	// Token: 0x04005AD7 RID: 23255
	protected float pasteUndoMainRigidity;

	// Token: 0x04005AD8 RID: 23256
	protected JSONStorableFloat mainRigidityJSON;

	// Token: 0x04005AD9 RID: 23257
	public static float copiedTipRigidity;

	// Token: 0x04005ADA RID: 23258
	protected float pasteUndoTipRigidity;

	// Token: 0x04005ADB RID: 23259
	protected JSONStorableFloat tipRigidityJSON;

	// Token: 0x04005ADC RID: 23260
	public static float copiedRigidityRolloffPower;

	// Token: 0x04005ADD RID: 23261
	protected float pasteUndoRigidityRolloffPower;

	// Token: 0x04005ADE RID: 23262
	protected JSONStorableFloat rigidityRolloffPowerJSON;

	// Token: 0x04005ADF RID: 23263
	public static float copiedJointRigidity;

	// Token: 0x04005AE0 RID: 23264
	protected float pasteUndoJointRigidity;

	// Token: 0x04005AE1 RID: 23265
	protected JSONStorableFloat jointRigidityJSON;

	// Token: 0x04005AE2 RID: 23266
	public static float copiedCling;

	// Token: 0x04005AE3 RID: 23267
	protected float pasteUndoCling;

	// Token: 0x04005AE4 RID: 23268
	protected JSONStorableFloat clingJSON;

	// Token: 0x04005AE5 RID: 23269
	public static float copiedClingRolloff;

	// Token: 0x04005AE6 RID: 23270
	protected float pasteUndoClingRolloff;

	// Token: 0x04005AE7 RID: 23271
	protected JSONStorableFloat clingRolloffJSON;

	// Token: 0x04005AE8 RID: 23272
	public static float copiedSnap;

	// Token: 0x04005AE9 RID: 23273
	protected float pasteUndoSnap;

	// Token: 0x04005AEA RID: 23274
	protected JSONStorableFloat snapJSON;

	// Token: 0x04005AEB RID: 23275
	public static float copiedBendResistance;

	// Token: 0x04005AEC RID: 23276
	protected float pasteUndoBendResistance;

	// Token: 0x04005AED RID: 23277
	protected JSONStorableFloat bendResistanceJSON;

	// Token: 0x04005AEE RID: 23278
	protected JSONStorableVector3 windJSON;

	// Token: 0x04005AEF RID: 23279
	public static bool hasLightingCopyData;

	// Token: 0x04005AF0 RID: 23280
	public JSONStorableAction copyLightingParametersAction;

	// Token: 0x04005AF1 RID: 23281
	protected bool hasLightingPasteData;

	// Token: 0x04005AF2 RID: 23282
	public JSONStorableAction pasteLightingParametersAction;

	// Token: 0x04005AF3 RID: 23283
	public JSONStorableAction undoPasteLightingParametersAction;

	// Token: 0x04005AF4 RID: 23284
	public Shader qualityShader;

	// Token: 0x04005AF5 RID: 23285
	public Shader qualityThickenShader;

	// Token: 0x04005AF6 RID: 23286
	public Shader qualityThickenMoreShader;

	// Token: 0x04005AF7 RID: 23287
	public Shader fastShader;

	// Token: 0x04005AF8 RID: 23288
	protected Shader nonStandardShader;

	// Token: 0x04005AF9 RID: 23289
	public static string copiedShaderType;

	// Token: 0x04005AFA RID: 23290
	protected string pasteUndoShaderType;

	// Token: 0x04005AFB RID: 23291
	protected HairSimControl.ShaderType _currentShaderType = HairSimControl.ShaderType.Quality;

	// Token: 0x04005AFC RID: 23292
	protected JSONStorableStringChooser shaderTypeJSON;

	// Token: 0x04005AFD RID: 23293
	protected Color _rootColor;

	// Token: 0x04005AFE RID: 23294
	protected HSVColor _rootHSVColor;

	// Token: 0x04005AFF RID: 23295
	public static HSVColor copiedRootColor;

	// Token: 0x04005B00 RID: 23296
	protected HSVColor pasteUndoRootColor;

	// Token: 0x04005B01 RID: 23297
	protected JSONStorableColor rootColorJSON;

	// Token: 0x04005B02 RID: 23298
	protected Color _tipColor;

	// Token: 0x04005B03 RID: 23299
	protected HSVColor _tipHSVColor;

	// Token: 0x04005B04 RID: 23300
	public static HSVColor copiedTipColor;

	// Token: 0x04005B05 RID: 23301
	protected HSVColor pasteUndoTipColor;

	// Token: 0x04005B06 RID: 23302
	protected JSONStorableColor tipColorJSON;

	// Token: 0x04005B07 RID: 23303
	public static float copiedColorRolloff;

	// Token: 0x04005B08 RID: 23304
	protected float pasteUndoColorRolloff;

	// Token: 0x04005B09 RID: 23305
	protected JSONStorableFloat colorRolloffJSON;

	// Token: 0x04005B0A RID: 23306
	protected Color _specularColor;

	// Token: 0x04005B0B RID: 23307
	protected HSVColor _specularHSVColor;

	// Token: 0x04005B0C RID: 23308
	public static HSVColor copiedSpecularColor;

	// Token: 0x04005B0D RID: 23309
	protected static HSVColor pasteUndoSpecularColor;

	// Token: 0x04005B0E RID: 23310
	protected JSONStorableColor specularColorJSON;

	// Token: 0x04005B0F RID: 23311
	public static float copiedDiffuseSoftness;

	// Token: 0x04005B10 RID: 23312
	protected float pasteUndoDiffuseSoftness;

	// Token: 0x04005B11 RID: 23313
	protected JSONStorableFloat diffuseSoftnessJSON;

	// Token: 0x04005B12 RID: 23314
	public static float copiedPrimarySpecularSharpness;

	// Token: 0x04005B13 RID: 23315
	protected float pasteUndoPrimarySpecularSharpness;

	// Token: 0x04005B14 RID: 23316
	protected JSONStorableFloat primarySpecularSharpnessJSON;

	// Token: 0x04005B15 RID: 23317
	public static float copiedSecondarySpecularSharpness;

	// Token: 0x04005B16 RID: 23318
	protected float pasteUndoSecondarySpecularSharpness;

	// Token: 0x04005B17 RID: 23319
	protected JSONStorableFloat secondarySpecularSharpnessJSON;

	// Token: 0x04005B18 RID: 23320
	public static float copiedSpecularShift;

	// Token: 0x04005B19 RID: 23321
	protected float pasteUndoSpecularShift;

	// Token: 0x04005B1A RID: 23322
	protected JSONStorableFloat specularShiftJSON;

	// Token: 0x04005B1B RID: 23323
	public static float copiedFresnelPower;

	// Token: 0x04005B1C RID: 23324
	protected float pasteUndoFresnelPower;

	// Token: 0x04005B1D RID: 23325
	protected JSONStorableFloat fresnelPowerJSON;

	// Token: 0x04005B1E RID: 23326
	public static float copiedFresnelAttenuation;

	// Token: 0x04005B1F RID: 23327
	protected float pasteUndoFresnelAttenuation;

	// Token: 0x04005B20 RID: 23328
	protected JSONStorableFloat fresnelAttenuationJSON;

	// Token: 0x04005B21 RID: 23329
	public static float copiedRandomColorPower;

	// Token: 0x04005B22 RID: 23330
	protected float pasteUndoRandomColorPower;

	// Token: 0x04005B23 RID: 23331
	protected JSONStorableFloat randomColorPowerJSON;

	// Token: 0x04005B24 RID: 23332
	public static float copiedRandomColorOffset;

	// Token: 0x04005B25 RID: 23333
	protected float pasteUndoRandomColorOffset;

	// Token: 0x04005B26 RID: 23334
	protected JSONStorableFloat randomColorOffsetJSON;

	// Token: 0x04005B27 RID: 23335
	public static float copiedIBLFactor;

	// Token: 0x04005B28 RID: 23336
	protected float pasteUndoIBLFactor;

	// Token: 0x04005B29 RID: 23337
	protected JSONStorableFloat IBLFactorJSON;

	// Token: 0x04005B2A RID: 23338
	public static float copiedNormalRandomize;

	// Token: 0x04005B2B RID: 23339
	protected float pasteUndoNormalRandomize;

	// Token: 0x04005B2C RID: 23340
	protected JSONStorableFloat normalRandomizeJSON;

	// Token: 0x04005B2D RID: 23341
	public static bool hasLookCopyData;

	// Token: 0x04005B2E RID: 23342
	public JSONStorableAction copyLookParametersAction;

	// Token: 0x04005B2F RID: 23343
	public static bool hasLookPasteData;

	// Token: 0x04005B30 RID: 23344
	public JSONStorableAction pasteLookParametersAction;

	// Token: 0x04005B31 RID: 23345
	public JSONStorableAction undoPasteLookParametersAction;

	// Token: 0x04005B32 RID: 23346
	public static float copiedCurlX;

	// Token: 0x04005B33 RID: 23347
	protected float pasteUndoCurlX;

	// Token: 0x04005B34 RID: 23348
	protected JSONStorableFloat curlXJSON;

	// Token: 0x04005B35 RID: 23349
	public static float copiedCurlY;

	// Token: 0x04005B36 RID: 23350
	protected float pasteUndoCurlY;

	// Token: 0x04005B37 RID: 23351
	protected JSONStorableFloat curlYJSON;

	// Token: 0x04005B38 RID: 23352
	public static float copiedCurlZ;

	// Token: 0x04005B39 RID: 23353
	protected float pasteUndoCurlZ;

	// Token: 0x04005B3A RID: 23354
	protected JSONStorableFloat curlZJSON;

	// Token: 0x04005B3B RID: 23355
	public static float copiedCurlScale;

	// Token: 0x04005B3C RID: 23356
	protected float pasteUndoCurlScale;

	// Token: 0x04005B3D RID: 23357
	protected JSONStorableFloat curlScaleJSON;

	// Token: 0x04005B3E RID: 23358
	public static float copiedCurlScaleRandomness;

	// Token: 0x04005B3F RID: 23359
	protected float pasteUndoCurlScaleRandomness;

	// Token: 0x04005B40 RID: 23360
	protected JSONStorableFloat curlScaleRandomnessJSON;

	// Token: 0x04005B41 RID: 23361
	public static float copiedCurlFrequency;

	// Token: 0x04005B42 RID: 23362
	protected float pasteUndoCurlFrequency;

	// Token: 0x04005B43 RID: 23363
	protected JSONStorableFloat curlFrequencyJSON;

	// Token: 0x04005B44 RID: 23364
	public static float copiedCurlFrequencyRandomness;

	// Token: 0x04005B45 RID: 23365
	protected float pasteUndoCurlFrequencyRandomness;

	// Token: 0x04005B46 RID: 23366
	protected JSONStorableFloat curlFrequencyRandomnessJSON;

	// Token: 0x04005B47 RID: 23367
	public static bool copiedCurlAllowReverse;

	// Token: 0x04005B48 RID: 23368
	protected bool pasteUndoCurlAllowReverse;

	// Token: 0x04005B49 RID: 23369
	protected JSONStorableBool curlAllowReverseJSON;

	// Token: 0x04005B4A RID: 23370
	public static bool copiedCurlAllowFlipAxis;

	// Token: 0x04005B4B RID: 23371
	protected bool pasteUndoCurlAllowFlipAxis;

	// Token: 0x04005B4C RID: 23372
	protected JSONStorableBool curlAllowFlipAxisJSON;

	// Token: 0x04005B4D RID: 23373
	public static float copiedCurlNormalAdjust;

	// Token: 0x04005B4E RID: 23374
	protected float pasteUndoCurlNormalAdjust;

	// Token: 0x04005B4F RID: 23375
	protected JSONStorableFloat curlNormalAdjustJSON;

	// Token: 0x04005B50 RID: 23376
	public static float copiedCurlRoot;

	// Token: 0x04005B51 RID: 23377
	protected float pasteUndoCurlRoot;

	// Token: 0x04005B52 RID: 23378
	protected JSONStorableFloat curlRootJSON;

	// Token: 0x04005B53 RID: 23379
	public static float copiedCurlMid;

	// Token: 0x04005B54 RID: 23380
	protected float pasteUndoCurlMid;

	// Token: 0x04005B55 RID: 23381
	protected JSONStorableFloat curlMidJSON;

	// Token: 0x04005B56 RID: 23382
	public static float copiedCurlTip;

	// Token: 0x04005B57 RID: 23383
	protected float pasteUndoCurlTip;

	// Token: 0x04005B58 RID: 23384
	protected JSONStorableFloat curlTipJSON;

	// Token: 0x04005B59 RID: 23385
	public static float copiedCurlMidpoint;

	// Token: 0x04005B5A RID: 23386
	protected float pasteUndoCurlMidpoint;

	// Token: 0x04005B5B RID: 23387
	protected JSONStorableFloat curlMidpointJSON;

	// Token: 0x04005B5C RID: 23388
	public static float copiedCurlCurvePower;

	// Token: 0x04005B5D RID: 23389
	protected float pasteUndoCurlCurvePower;

	// Token: 0x04005B5E RID: 23390
	protected JSONStorableFloat curlCurvePowerJSON;

	// Token: 0x04005B5F RID: 23391
	public static float copiedLength1;

	// Token: 0x04005B60 RID: 23392
	protected float pasteUndoLength1;

	// Token: 0x04005B61 RID: 23393
	protected JSONStorableFloat length1JSON;

	// Token: 0x04005B62 RID: 23394
	public static float copiedLength2;

	// Token: 0x04005B63 RID: 23395
	protected float pasteUndoLength2;

	// Token: 0x04005B64 RID: 23396
	protected JSONStorableFloat length2JSON;

	// Token: 0x04005B65 RID: 23397
	public static float copiedLength3;

	// Token: 0x04005B66 RID: 23398
	protected float pasteUndoLength3;

	// Token: 0x04005B67 RID: 23399
	protected JSONStorableFloat length3JSON;

	// Token: 0x04005B68 RID: 23400
	public static float copiedWidth;

	// Token: 0x04005B69 RID: 23401
	protected float pasteUndoWidth;

	// Token: 0x04005B6A RID: 23402
	protected JSONStorableFloat widthJSON;

	// Token: 0x04005B6B RID: 23403
	protected JSONStorableFloat densityJSON;

	// Token: 0x04005B6C RID: 23404
	protected JSONStorableFloat detailJSON;

	// Token: 0x04005B6D RID: 23405
	public static float copiedMaxSpread;

	// Token: 0x04005B6E RID: 23406
	protected float pasteUndoMaxSpread;

	// Token: 0x04005B6F RID: 23407
	protected JSONStorableFloat maxSpreadJSON;

	// Token: 0x04005B70 RID: 23408
	public static float copiedSpreadRoot;

	// Token: 0x04005B71 RID: 23409
	protected float pasteUndoSpreadRoot;

	// Token: 0x04005B72 RID: 23410
	protected JSONStorableFloat spreadRootJSON;

	// Token: 0x04005B73 RID: 23411
	public static float copiedSpreadMid;

	// Token: 0x04005B74 RID: 23412
	protected float pasteUndoSpreadMid;

	// Token: 0x04005B75 RID: 23413
	protected JSONStorableFloat spreadMidJSON;

	// Token: 0x04005B76 RID: 23414
	public static float copiedSpreadTip;

	// Token: 0x04005B77 RID: 23415
	protected float pasteUndoSpreadTip;

	// Token: 0x04005B78 RID: 23416
	protected JSONStorableFloat spreadTipJSON;

	// Token: 0x04005B79 RID: 23417
	public static float copiedSpreadMidpoint;

	// Token: 0x04005B7A RID: 23418
	protected float pasteUndoSpreadMidpoint;

	// Token: 0x04005B7B RID: 23419
	protected JSONStorableFloat spreadMidpointJSON;

	// Token: 0x04005B7C RID: 23420
	public static float copiedSpreadCurvePower;

	// Token: 0x04005B7D RID: 23421
	protected float pasteUndoSpreadCurvePower;

	// Token: 0x04005B7E RID: 23422
	protected JSONStorableFloat spreadCurvePowerJSON;

	// Token: 0x04005B7F RID: 23423
	[CompilerGenerated]
	private static Func<Vector4ListContainer, int> <>f__am$cache0;

	// Token: 0x02000D6D RID: 3437
	public enum ShaderType
	{
		// Token: 0x04005B81 RID: 23425
		Fast,
		// Token: 0x04005B82 RID: 23426
		Quality,
		// Token: 0x04005B83 RID: 23427
		QualityThicken,
		// Token: 0x04005B84 RID: 23428
		QualityThickenMore,
		// Token: 0x04005B85 RID: 23429
		NonStandard
	}

	// Token: 0x02001037 RID: 4151
	[CompilerGenerated]
	private sealed class <RebuildStyleJointsCo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007777 RID: 30583 RVA: 0x0027ED2D File Offset: 0x0027D12D
		[DebuggerHidden]
		public <RebuildStyleJointsCo>c__Iterator0()
		{
		}

		// Token: 0x06007778 RID: 30584 RVA: 0x0027ED38 File Offset: 0x0027D138
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				this.threadError = null;
				this.creator.PrepareCalculateNearbyVertexGroups();
				this.rebuildStyleJointsThread = new Thread(new ThreadStart(base.RebuildStyleJointsThreaded));
				this.rebuildStyleJointsThread.Start();
				break;
			case 2U:
				break;
			default:
				return false;
			}
			if (this.rebuildStyleJointsThread.IsAlive)
			{
				if (this.styleStatusText != null)
				{
					this.styleStatusText.text = this.creator.status;
				}
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			}
			if (this.styleStatusText != null)
			{
				this.styleStatusText.text = this.creator.status;
			}
			if (this.hairSettings != null)
			{
				base.Rebuild();
			}
			base.SyncStyleText();
			this.isRebuildingStyleJoints = false;
			this.$PC = -1;
			return false;
		}

		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x06007779 RID: 30585 RVA: 0x0027EEB5 File Offset: 0x0027D2B5
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x0600777A RID: 30586 RVA: 0x0027EEBD File Offset: 0x0027D2BD
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600777B RID: 30587 RVA: 0x0027EEC5 File Offset: 0x0027D2C5
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600777C RID: 30588 RVA: 0x0027EED5 File Offset: 0x0027D2D5
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006B7E RID: 27518
		internal HairSimControl $this;

		// Token: 0x04006B7F RID: 27519
		internal object $current;

		// Token: 0x04006B80 RID: 27520
		internal bool $disposing;

		// Token: 0x04006B81 RID: 27521
		internal int $PC;
	}
}
