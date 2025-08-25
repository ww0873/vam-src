using System;
using UnityEngine;

// Token: 0x02000D9F RID: 3487
public class JSONStorableTriggerHandler : JSONStorable, TriggerHandler
{
	// Token: 0x06006B86 RID: 27526 RVA: 0x001D715C File Offset: 0x001D555C
	public JSONStorableTriggerHandler()
	{
	}

	// Token: 0x06006B87 RID: 27527 RVA: 0x001D7164 File Offset: 0x001D5564
	public virtual void RemoveTrigger(Trigger trigger)
	{
	}

	// Token: 0x06006B88 RID: 27528 RVA: 0x001D7166 File Offset: 0x001D5566
	public virtual void DuplicateTrigger(Trigger trigger)
	{
	}

	// Token: 0x06006B89 RID: 27529 RVA: 0x001D7168 File Offset: 0x001D5568
	public RectTransform CreateTriggerActionsUI()
	{
		RectTransform result = null;
		if (this.triggerActionsPrefab != null)
		{
			result = UnityEngine.Object.Instantiate<RectTransform>(this.triggerActionsPrefab);
		}
		else
		{
			Debug.LogError("Attempted to make TriggerActionsUI when prefab was not set");
		}
		return result;
	}

	// Token: 0x06006B8A RID: 27530 RVA: 0x001D71A4 File Offset: 0x001D55A4
	public RectTransform CreateTriggerActionMiniUI()
	{
		RectTransform result = null;
		if (this.triggerActionMiniPrefab != null)
		{
			result = UnityEngine.Object.Instantiate<RectTransform>(this.triggerActionMiniPrefab);
		}
		else
		{
			Debug.LogError("Attempted to make TriggerActionMiniUI when prefab was not set");
		}
		return result;
	}

	// Token: 0x06006B8B RID: 27531 RVA: 0x001D71E0 File Offset: 0x001D55E0
	public RectTransform CreateTriggerActionDiscreteUI()
	{
		RectTransform result = null;
		if (this.triggerActionDiscretePrefab != null)
		{
			result = UnityEngine.Object.Instantiate<RectTransform>(this.triggerActionDiscretePrefab);
		}
		else
		{
			Debug.LogError("Attempted to make TriggerActionDiscreteUI when prefab was not set");
		}
		return result;
	}

	// Token: 0x06006B8C RID: 27532 RVA: 0x001D721C File Offset: 0x001D561C
	public RectTransform CreateTriggerActionTransitionUI()
	{
		RectTransform result = null;
		if (this.triggerActionTransitionPrefab != null)
		{
			result = UnityEngine.Object.Instantiate<RectTransform>(this.triggerActionTransitionPrefab);
		}
		else
		{
			Debug.LogError("Attempted to make TriggerActionTransitionUI when prefab was not set");
		}
		return result;
	}

	// Token: 0x06006B8D RID: 27533 RVA: 0x001D7258 File Offset: 0x001D5658
	public void RemoveTriggerActionUI(RectTransform rt)
	{
		if (rt != null)
		{
			UnityEngine.Object.Destroy(rt.gameObject);
		}
	}

	// Token: 0x04005D4B RID: 23883
	public RectTransform triggerActionsPrefab;

	// Token: 0x04005D4C RID: 23884
	public RectTransform triggerActionMiniPrefab;

	// Token: 0x04005D4D RID: 23885
	public RectTransform triggerActionDiscretePrefab;

	// Token: 0x04005D4E RID: 23886
	public RectTransform triggerActionTransitionPrefab;
}
