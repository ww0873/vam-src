using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace MeshVR.AnimationPatternV2
{
	// Token: 0x02000B56 RID: 2902
	public class AnimationPatternV2 : JSONStorable, AnimationTimelineTriggerHandler, TriggerHandler
	{
		// Token: 0x06005105 RID: 20741 RVA: 0x001D4749 File Offset: 0x001D2B49
		public AnimationPatternV2()
		{
		}

		// Token: 0x06005106 RID: 20742 RVA: 0x001D4754 File Offset: 0x001D2B54
		public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
		{
			return base.GetJSON(includePhysical, includeAppearance, forceStore);
		}

		// Token: 0x06005107 RID: 20743 RVA: 0x001D476C File Offset: 0x001D2B6C
		public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
		{
			base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		}

		// Token: 0x06005108 RID: 20744 RVA: 0x001D477B File Offset: 0x001D2B7B
		public float GetCurrentTimeCounter()
		{
			return 0f;
		}

		// Token: 0x06005109 RID: 20745 RVA: 0x001D4782 File Offset: 0x001D2B82
		public float GetTotalTime()
		{
			return 0f;
		}

		// Token: 0x0600510A RID: 20746 RVA: 0x001D4789 File Offset: 0x001D2B89
		public void RemoveTrigger(Trigger t)
		{
		}

		// Token: 0x0600510B RID: 20747 RVA: 0x001D478B File Offset: 0x001D2B8B
		public void DuplicateTrigger(Trigger t)
		{
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x001D478D File Offset: 0x001D2B8D
		public RectTransform CreateTriggerActionsUI()
		{
			return null;
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x001D4790 File Offset: 0x001D2B90
		public RectTransform CreateTriggerActionMiniUI()
		{
			return null;
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x001D4793 File Offset: 0x001D2B93
		public RectTransform CreateTriggerActionDiscreteUI()
		{
			return null;
		}

		// Token: 0x0600510F RID: 20751 RVA: 0x001D4796 File Offset: 0x001D2B96
		public RectTransform CreateTriggerActionTransitionUI()
		{
			return null;
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x001D4799 File Offset: 0x001D2B99
		public void RemoveTriggerActionUI(RectTransform rt)
		{
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x001D479C File Offset: 0x001D2B9C
		protected void OnAtomUIDRename(string fromid, string toid)
		{
			foreach (Track track in this.tracks)
			{
				if (track.freeControllerAtomUID == fromid)
				{
					track.freeControllerAtomUID = toid;
				}
				if (track.receiverAtomSelectionPopup != null)
				{
					track.receiverAtomSelectionPopup.currentValueNoCallback = toid;
				}
			}
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x001D4828 File Offset: 0x001D2C28
		protected void RunAnimation()
		{
		}

		// Token: 0x06005113 RID: 20755 RVA: 0x001D482A File Offset: 0x001D2C2A
		protected void Init()
		{
			if (SuperController.singleton != null)
			{
				SuperController singleton = SuperController.singleton;
				singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomUIDRename));
			}
		}

		// Token: 0x06005114 RID: 20756 RVA: 0x001D4862 File Offset: 0x001D2C62
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

		// Token: 0x06005115 RID: 20757 RVA: 0x001D4887 File Offset: 0x001D2C87
		private void OnDestroy()
		{
			if (SuperController.singleton != null)
			{
				SuperController singleton = SuperController.singleton;
				singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomUIDRename));
			}
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x001D48BF File Offset: 0x001D2CBF
		private void Update()
		{
			if (SuperController.singleton == null || !SuperController.singleton.freezeAnimation)
			{
				this.RunAnimation();
			}
		}

		// Token: 0x040040EB RID: 16619
		protected List<Track> tracks;
	}
}
