using System;
using UnityEngine;

// Token: 0x02000D9C RID: 3484
public class CollisionTriggerGroup : JSONStorableTriggerHandler
{
	// Token: 0x06006B81 RID: 27521 RVA: 0x00289390 File Offset: 0x00287790
	public CollisionTriggerGroup()
	{
	}

	// Token: 0x06006B82 RID: 27522 RVA: 0x00289398 File Offset: 0x00287798
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			CollisionTriggerGroupUI componentInChildren = this.UITransform.GetComponentInChildren<CollisionTriggerGroupUI>();
			if (componentInChildren != null)
			{
				if (componentInChildren.triggerContentManager != null)
				{
					if (this.triggerPrefab != null)
					{
						foreach (CollisionTrigger collisionTrigger in this.collisionTriggers)
						{
							if (collisionTrigger.trigger != null && this.triggerPrefab != null)
							{
								RectTransform rectTransform = base.CreateTriggerActionsUI();
								if (rectTransform != null)
								{
									rectTransform.SetParent(componentInChildren.transform, false);
									rectTransform.gameObject.SetActive(false);
									collisionTrigger.UITransform = rectTransform;
									RectTransform rectTransform2 = UnityEngine.Object.Instantiate<RectTransform>(this.triggerPrefab);
									componentInChildren.triggerContentManager.AddItem(rectTransform2, -1, false);
									collisionTrigger.trigger.displayName = collisionTrigger.name;
									collisionTrigger.trigger.triggerPanel = rectTransform2;
									collisionTrigger.InitUI();
								}
							}
						}
					}
					else
					{
						Debug.LogError("Attempted to InitUI on CollisionTriggerGroup when triggerPrefab not set");
					}
				}
				else
				{
					Debug.LogError("Attempt to InitUI on CollisionTriggerGroup when triggerContentManager not set");
				}
			}
		}
	}

	// Token: 0x06006B83 RID: 27523 RVA: 0x002894C1 File Offset: 0x002878C1
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.InitUI();
		}
	}

	// Token: 0x04005D40 RID: 23872
	public RectTransform triggerPrefab;

	// Token: 0x04005D41 RID: 23873
	public CollisionTrigger[] collisionTriggers;
}
