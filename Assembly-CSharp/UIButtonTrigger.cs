using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SimpleJSON;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DB2 RID: 3506
public class UIButtonTrigger : JSONStorableTriggerHandler
{
	// Token: 0x06006CB2 RID: 27826 RVA: 0x00290493 File Offset: 0x0028E893
	public UIButtonTrigger()
	{
	}

	// Token: 0x06006CB3 RID: 27827 RVA: 0x0029049C File Offset: 0x0028E89C
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if ((includePhysical || forceStore) && this.trigger != null && (this.trigger.HasActions() || forceStore))
		{
			this.needsStore = true;
			json["trigger"] = this.trigger.GetJSON(base.subScenePrefix);
		}
		return json;
	}

	// Token: 0x06006CB4 RID: 27828 RVA: 0x00290504 File Offset: 0x0028E904
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		base.LateRestoreFromJSON(jc, restorePhysical, restoreAppearance, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("trigger"))
		{
			if (jc["trigger"] != null)
			{
				JSONClass asObject = jc["trigger"].AsObject;
				if (asObject != null)
				{
					this.trigger.RestoreFromJSON(asObject, base.subScenePrefix, base.mergeRestore);
				}
			}
			else if (setMissingToDefault && !base.mergeRestore)
			{
				this.trigger.RestoreFromJSON(new JSONClass());
			}
		}
	}

	// Token: 0x06006CB5 RID: 27829 RVA: 0x002905AF File Offset: 0x0028E9AF
	public override void Validate()
	{
		base.Validate();
		if (this.trigger != null)
		{
			this.trigger.Validate();
		}
	}

	// Token: 0x06006CB6 RID: 27830 RVA: 0x002905D0 File Offset: 0x0028E9D0
	private IEnumerator ReleaseTrigger()
	{
		yield return null;
		if (this.trigger != null)
		{
			this.trigger.active = false;
		}
		yield break;
	}

	// Token: 0x06006CB7 RID: 27831 RVA: 0x002905EB File Offset: 0x0028E9EB
	protected void OnButtonClick()
	{
		if (this.trigger != null)
		{
			this.trigger.active = true;
			this.trigger.active = false;
		}
	}

	// Token: 0x06006CB8 RID: 27832 RVA: 0x00290610 File Offset: 0x0028EA10
	protected void OnAtomRename(string oldid, string newid)
	{
		if (this.trigger != null)
		{
			this.trigger.SyncAtomNames();
		}
	}

	// Token: 0x06006CB9 RID: 27833 RVA: 0x00290628 File Offset: 0x0028EA28
	protected void Init()
	{
		this.trigger = new Trigger();
		this.trigger.handler = this;
		if (this.button != null)
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
		}
		if (SuperController.singleton)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x06006CBA RID: 27834 RVA: 0x002906B0 File Offset: 0x0028EAB0
	public override void InitUI()
	{
		if (this.UITransform != null && this.trigger != null)
		{
			UIButtonTriggerUI componentInChildren = this.UITransform.GetComponentInChildren<UIButtonTriggerUI>();
			if (componentInChildren != null)
			{
				this.trigger.triggerActionsParent = componentInChildren.transform;
				this.trigger.triggerPanel = componentInChildren.transform;
				this.trigger.triggerActionsPanel = componentInChildren.transform;
				this.trigger.InitTriggerUI();
				this.trigger.InitTriggerActionsUI();
			}
		}
	}

	// Token: 0x06006CBB RID: 27835 RVA: 0x0029073A File Offset: 0x0028EB3A
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
		}
	}

	// Token: 0x06006CBC RID: 27836 RVA: 0x00290759 File Offset: 0x0028EB59
	protected void Update()
	{
		if (this.trigger != null)
		{
			this.trigger.Update();
		}
	}

	// Token: 0x06006CBD RID: 27837 RVA: 0x00290774 File Offset: 0x0028EB74
	protected void OnDestroy()
	{
		if (this.trigger != null)
		{
			this.trigger.Remove();
		}
		if (SuperController.singleton)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x04005E52 RID: 24146
	public Trigger trigger;

	// Token: 0x04005E53 RID: 24147
	public Button button;

	// Token: 0x02001039 RID: 4153
	[CompilerGenerated]
	private sealed class <ReleaseTrigger>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007783 RID: 30595 RVA: 0x002907CC File Offset: 0x0028EBCC
		[DebuggerHidden]
		public <ReleaseTrigger>c__Iterator0()
		{
		}

		// Token: 0x06007784 RID: 30596 RVA: 0x002907D4 File Offset: 0x0028EBD4
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
				if (this.trigger != null)
				{
					this.trigger.active = false;
				}
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x06007785 RID: 30597 RVA: 0x00290848 File Offset: 0x0028EC48
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x06007786 RID: 30598 RVA: 0x00290850 File Offset: 0x0028EC50
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007787 RID: 30599 RVA: 0x00290858 File Offset: 0x0028EC58
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007788 RID: 30600 RVA: 0x00290868 File Offset: 0x0028EC68
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006B86 RID: 27526
		internal UIButtonTrigger $this;

		// Token: 0x04006B87 RID: 27527
		internal object $current;

		// Token: 0x04006B88 RID: 27528
		internal bool $disposing;

		// Token: 0x04006B89 RID: 27529
		internal int $PC;
	}
}
