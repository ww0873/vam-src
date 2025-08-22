using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DA4 RID: 3492
public class Trigger : TriggerActionHandler
{
	// Token: 0x06006BAD RID: 27565 RVA: 0x002856EC File Offset: 0x00283AEC
	public Trigger()
	{
		this.previewTextJSON = new JSONStorableString("previewText", string.Empty, new JSONStorableString.SetStringCallback(this.SyncPreviewText));
		this.displayNameJSON = new JSONStorableString("displayName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncDisplayName));
		this.discreteActionsStart = new List<TriggerActionDiscrete>();
		this.transitionActions = new List<TriggerActionTransition>();
		this.discreteActionsEnd = new List<TriggerActionDiscrete>();
		this.actionsWithActiveTimers = new HashSet<TriggerActionDiscrete>();
	}

	// Token: 0x17000FCD RID: 4045
	// (get) Token: 0x06006BAE RID: 27566 RVA: 0x00285774 File Offset: 0x00283B74
	public IEnumerable<TriggerActionDiscrete> DiscreteActionsStart
	{
		get
		{
			return this.discreteActionsStart;
		}
	}

	// Token: 0x06006BAF RID: 27567 RVA: 0x0028577C File Offset: 0x00283B7C
	public void CopyDiscreteActionsStart()
	{
		if (this.discreteActionsStart != null)
		{
			if (Trigger.copyOfDiscreteActions == null)
			{
				Trigger.copyOfDiscreteActions = new List<TriggerActionDiscrete>();
			}
			else
			{
				Trigger.copyOfDiscreteActions.Clear();
			}
			foreach (TriggerActionDiscrete triggerActionDiscrete in this.discreteActionsStart)
			{
				JSONClass json = triggerActionDiscrete.GetJSON();
				TriggerActionDiscrete triggerActionDiscrete2 = new TriggerActionDiscrete();
				triggerActionDiscrete2.doActionsInReverse = this.doActionsInReverse;
				triggerActionDiscrete2.RestoreFromJSON(json);
				Trigger.copyOfDiscreteActions.Add(triggerActionDiscrete2);
			}
		}
	}

	// Token: 0x06006BB0 RID: 27568 RVA: 0x0028582C File Offset: 0x00283C2C
	public void PasteDiscreteActionsStart()
	{
		if (Trigger.copyOfDiscreteActions != null)
		{
			foreach (TriggerActionDiscrete triggerActionDiscrete in Trigger.copyOfDiscreteActions)
			{
				JSONClass json = triggerActionDiscrete.GetJSON();
				TriggerActionDiscrete triggerActionDiscrete2 = this.CreateDiscreteActionStartInternal(-1);
				triggerActionDiscrete2.RestoreFromJSON(json);
			}
		}
	}

	// Token: 0x17000FCE RID: 4046
	// (get) Token: 0x06006BB1 RID: 27569 RVA: 0x002858A0 File Offset: 0x00283CA0
	public IEnumerable<TriggerActionTransition> TransitionActions
	{
		get
		{
			return this.transitionActions;
		}
	}

	// Token: 0x06006BB2 RID: 27570 RVA: 0x002858A8 File Offset: 0x00283CA8
	public void CopyTransitionActions()
	{
		if (this.transitionActions != null)
		{
			if (Trigger.copyOfTransitionActions == null)
			{
				Trigger.copyOfTransitionActions = new List<TriggerActionTransition>();
			}
			else
			{
				Trigger.copyOfTransitionActions.Clear();
			}
			foreach (TriggerActionTransition triggerActionTransition in this.transitionActions)
			{
				JSONClass json = triggerActionTransition.GetJSON();
				TriggerActionTransition triggerActionTransition2 = new TriggerActionTransition();
				triggerActionTransition2.RestoreFromJSON(json);
				Trigger.copyOfTransitionActions.Add(triggerActionTransition2);
			}
		}
	}

	// Token: 0x06006BB3 RID: 27571 RVA: 0x0028594C File Offset: 0x00283D4C
	public void PasteTransitionActions()
	{
		if (Trigger.copyOfTransitionActions != null)
		{
			foreach (TriggerActionTransition triggerActionTransition in Trigger.copyOfTransitionActions)
			{
				JSONClass json = triggerActionTransition.GetJSON();
				TriggerActionTransition triggerActionTransition2 = this.CreateTransitionActionInternal(-1);
				triggerActionTransition2.RestoreFromJSON(json);
			}
		}
	}

	// Token: 0x17000FCF RID: 4047
	// (get) Token: 0x06006BB4 RID: 27572 RVA: 0x002859C0 File Offset: 0x00283DC0
	public IEnumerable<TriggerActionDiscrete> DiscreteActionsEnd
	{
		get
		{
			return this.discreteActionsEnd;
		}
	}

	// Token: 0x06006BB5 RID: 27573 RVA: 0x002859C8 File Offset: 0x00283DC8
	public void CopyDiscreteActionsEnd()
	{
		if (this.discreteActionsEnd != null)
		{
			if (Trigger.copyOfDiscreteActions == null)
			{
				Trigger.copyOfDiscreteActions = new List<TriggerActionDiscrete>();
			}
			else
			{
				Trigger.copyOfDiscreteActions.Clear();
			}
			foreach (TriggerActionDiscrete triggerActionDiscrete in this.discreteActionsEnd)
			{
				JSONClass json = triggerActionDiscrete.GetJSON();
				TriggerActionDiscrete triggerActionDiscrete2 = new TriggerActionDiscrete();
				triggerActionDiscrete2.doActionsInReverse = this.doActionsInReverse;
				triggerActionDiscrete2.RestoreFromJSON(json);
				Trigger.copyOfDiscreteActions.Add(triggerActionDiscrete2);
			}
		}
	}

	// Token: 0x06006BB6 RID: 27574 RVA: 0x00285A78 File Offset: 0x00283E78
	public void PasteDiscreteActionsEnd()
	{
		if (Trigger.copyOfDiscreteActions != null)
		{
			foreach (TriggerActionDiscrete triggerActionDiscrete in Trigger.copyOfDiscreteActions)
			{
				JSONClass json = triggerActionDiscrete.GetJSON();
				TriggerActionDiscrete triggerActionDiscrete2 = this.CreateDiscreteActionEndInternal(-1);
				triggerActionDiscrete2.RestoreFromJSON(json);
			}
		}
	}

	// Token: 0x06006BB7 RID: 27575 RVA: 0x00285AEC File Offset: 0x00283EEC
	public virtual JSONClass GetJSON()
	{
		return this.GetJSON(null);
	}

	// Token: 0x06006BB8 RID: 27576 RVA: 0x00285AF8 File Offset: 0x00283EF8
	public virtual JSONClass GetJSON(string subScenePrefix)
	{
		JSONClass jsonclass = new JSONClass();
		JSONArray jsonarray = new JSONArray();
		JSONArray jsonarray2 = new JSONArray();
		JSONArray jsonarray3 = new JSONArray();
		this.displayNameJSON.StoreJSON(jsonclass, true, true, false);
		jsonclass["startActions"] = jsonarray;
		jsonclass["transitionActions"] = jsonarray2;
		jsonclass["endActions"] = jsonarray3;
		foreach (TriggerActionDiscrete triggerActionDiscrete in this.discreteActionsStart)
		{
			JSONClass json = triggerActionDiscrete.GetJSON(subScenePrefix);
			if (json != null)
			{
				jsonarray.Add(json);
			}
		}
		foreach (TriggerActionTransition triggerActionTransition in this.transitionActions)
		{
			JSONClass json2 = triggerActionTransition.GetJSON(subScenePrefix);
			if (json2 != null)
			{
				jsonarray2.Add(json2);
			}
		}
		foreach (TriggerActionDiscrete triggerActionDiscrete2 in this.discreteActionsEnd)
		{
			JSONClass json3 = triggerActionDiscrete2.GetJSON(subScenePrefix);
			if (json3 != null)
			{
				jsonarray3.Add(json3);
			}
		}
		return jsonclass;
	}

	// Token: 0x06006BB9 RID: 27577 RVA: 0x00285C88 File Offset: 0x00284088
	public bool HasActions()
	{
		return this.discreteActionsStart.Count > 0 || this.transitionActions.Count > 0 || this.discreteActionsEnd.Count > 0;
	}

	// Token: 0x06006BBA RID: 27578 RVA: 0x00285CC4 File Offset: 0x002840C4
	protected void ClearActions()
	{
		List<TriggerAction> list = new List<TriggerAction>();
		foreach (TriggerActionDiscrete item in this.discreteActionsStart)
		{
			list.Add(item);
		}
		foreach (TriggerActionTransition item2 in this.transitionActions)
		{
			list.Add(item2);
		}
		foreach (TriggerActionDiscrete item3 in this.discreteActionsEnd)
		{
			list.Add(item3);
		}
		foreach (TriggerAction triggerAction in list)
		{
			triggerAction.Remove();
		}
	}

	// Token: 0x06006BBB RID: 27579 RVA: 0x00285E08 File Offset: 0x00284208
	public virtual void RestoreFromJSON(JSONClass jc)
	{
		this.RestoreFromJSON(jc, null, false);
	}

	// Token: 0x06006BBC RID: 27580 RVA: 0x00285E14 File Offset: 0x00284214
	public virtual void RestoreFromJSON(JSONClass jc, string subScenePrefix, bool isMerge)
	{
		if (!isMerge)
		{
			this.ClearActions();
		}
		this.displayNameJSON.RestoreFromJSON(jc, true, true, true);
		if (jc["startActions"] != null)
		{
			JSONArray asArray = jc["startActions"].AsArray;
			if (asArray != null)
			{
				IEnumerator enumerator = asArray.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						JSONNode jsonnode = (JSONNode)obj;
						JSONClass asObject = jsonnode.AsObject;
						if (asObject != null)
						{
							TriggerActionDiscrete triggerActionDiscrete = this.CreateDiscreteActionStartInternal(-1);
							triggerActionDiscrete.RestoreFromJSON(asObject, subScenePrefix);
						}
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
		}
		if (jc["transitionActions"] != null)
		{
			JSONArray asArray2 = jc["transitionActions"].AsArray;
			if (asArray2 != null)
			{
				IEnumerator enumerator2 = asArray2.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						JSONNode jsonnode2 = (JSONNode)obj2;
						JSONClass asObject2 = jsonnode2.AsObject;
						if (asObject2 != null)
						{
							TriggerActionTransition triggerActionTransition = this.CreateTransitionActionInternal(-1);
							triggerActionTransition.RestoreFromJSON(asObject2, subScenePrefix);
							triggerActionTransition.active = this._active;
							triggerActionTransition.TriggerInterp(this._transitionInterpValue, false);
						}
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
			}
		}
		if (jc["endActions"] != null)
		{
			JSONArray asArray3 = jc["endActions"].AsArray;
			if (asArray3 != null)
			{
				IEnumerator enumerator3 = asArray3.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object obj3 = enumerator3.Current;
						JSONNode jsonnode3 = (JSONNode)obj3;
						JSONClass asObject3 = jsonnode3.AsObject;
						if (asObject3 != null)
						{
							TriggerActionDiscrete triggerActionDiscrete2 = this.CreateDiscreteActionEndInternal(-1);
							triggerActionDiscrete2.RestoreFromJSON(asObject3, subScenePrefix);
						}
					}
				}
				finally
				{
					IDisposable disposable3;
					if ((disposable3 = (enumerator3 as IDisposable)) != null)
					{
						disposable3.Dispose();
					}
				}
			}
		}
	}

	// Token: 0x06006BBD RID: 27581 RVA: 0x0028605C File Offset: 0x0028445C
	public virtual void Remove()
	{
		this.ClearActions();
		if (this.handler != null)
		{
			this.DeregisterUI();
			this.handler.RemoveTrigger(this);
		}
	}

	// Token: 0x06006BBE RID: 27582 RVA: 0x00286081 File Offset: 0x00284481
	public virtual void Duplicate()
	{
		if (this.handler != null)
		{
			this.handler.DuplicateTrigger(this);
		}
	}

	// Token: 0x06006BBF RID: 27583 RVA: 0x0028609C File Offset: 0x0028449C
	public virtual void SyncAtomNames()
	{
		foreach (TriggerActionDiscrete triggerActionDiscrete in this.discreteActionsStart)
		{
			triggerActionDiscrete.SyncAtomName();
		}
		foreach (TriggerActionTransition triggerActionTransition in this.transitionActions)
		{
			triggerActionTransition.SyncAtomName();
		}
		foreach (TriggerActionDiscrete triggerActionDiscrete2 in this.discreteActionsEnd)
		{
			triggerActionDiscrete2.SyncAtomName();
		}
	}

	// Token: 0x06006BC0 RID: 27584 RVA: 0x00286190 File Offset: 0x00284590
	public virtual void Validate()
	{
		List<TriggerActionDiscrete> list = new List<TriggerActionDiscrete>(this.discreteActionsStart);
		foreach (TriggerActionDiscrete triggerActionDiscrete in list)
		{
			triggerActionDiscrete.Validate();
		}
		List<TriggerActionTransition> list2 = new List<TriggerActionTransition>(this.transitionActions);
		foreach (TriggerActionTransition triggerActionTransition in list2)
		{
			triggerActionTransition.Validate();
		}
		list = new List<TriggerActionDiscrete>(this.discreteActionsEnd);
		foreach (TriggerActionDiscrete triggerActionDiscrete2 in list)
		{
			triggerActionDiscrete2.Validate();
		}
	}

	// Token: 0x06006BC1 RID: 27585 RVA: 0x0028629C File Offset: 0x0028469C
	protected void CreateTriggerActionsPanel()
	{
		if (this.triggerActionsParent != null)
		{
			this.triggerActionsPanel = this.handler.CreateTriggerActionsUI();
			if (this.triggerActionsPanel != null)
			{
				this.triggerActionsPanel.SetParent(this.triggerActionsParent, false);
				this.InitTriggerActionsUI();
			}
		}
		else
		{
			Debug.LogError("Attempted to CreateTriggerActionsPanel when triggerActionsParent was null");
		}
	}

	// Token: 0x06006BC2 RID: 27586 RVA: 0x00286304 File Offset: 0x00284704
	public virtual void OpenTriggerActionsPanel()
	{
		if (this.triggerActionsPanel == null)
		{
			this.CreateTriggerActionsPanel();
		}
		if (this.triggerActionsPanel != null)
		{
			this.triggerActionsPanel.gameObject.SetActive(true);
		}
		if (this.onOpenTriggerActionsPanel != null)
		{
			this.onOpenTriggerActionsPanel();
		}
	}

	// Token: 0x06006BC3 RID: 27587 RVA: 0x00286360 File Offset: 0x00284760
	public virtual void CloseTriggerActionsPanel()
	{
		if (this.triggerActionsPanel != null)
		{
			this.triggerActionsPanel.gameObject.SetActive(false);
		}
		if (this.onCloseTriggerActionsPanel != null)
		{
			this.onCloseTriggerActionsPanel();
		}
	}

	// Token: 0x06006BC4 RID: 27588 RVA: 0x0028639A File Offset: 0x0028479A
	protected void SyncPreviewText(string n)
	{
	}

	// Token: 0x17000FD0 RID: 4048
	// (get) Token: 0x06006BC5 RID: 27589 RVA: 0x0028639C File Offset: 0x0028479C
	// (set) Token: 0x06006BC6 RID: 27590 RVA: 0x002863A9 File Offset: 0x002847A9
	public string previewText
	{
		get
		{
			return this.previewTextJSON.val;
		}
		set
		{
			this.previewTextJSON.val = value;
		}
	}

	// Token: 0x06006BC7 RID: 27591 RVA: 0x002863B8 File Offset: 0x002847B8
	protected virtual void AutoSetPreviewText()
	{
		if (this.discreteActionsStart.Count > 0)
		{
			this.previewText = this.discreteActionsStart[0].previewText;
		}
		else if (this.transitionActions.Count > 0)
		{
			this.previewText = this.transitionActions[0].previewText;
		}
		else if (this.discreteActionsEnd.Count > 0)
		{
			this.previewText = this.discreteActionsEnd[0].previewText;
		}
		else
		{
			this.previewText = string.Empty;
		}
	}

	// Token: 0x06006BC8 RID: 27592 RVA: 0x00286457 File Offset: 0x00284857
	protected void SyncDisplayName(string n)
	{
	}

	// Token: 0x17000FD1 RID: 4049
	// (get) Token: 0x06006BC9 RID: 27593 RVA: 0x00286459 File Offset: 0x00284859
	// (set) Token: 0x06006BCA RID: 27594 RVA: 0x00286466 File Offset: 0x00284866
	public string displayName
	{
		get
		{
			return this.displayNameJSON.val;
		}
		set
		{
			this.displayNameJSON.val = value;
		}
	}

	// Token: 0x06006BCB RID: 27595 RVA: 0x00286474 File Offset: 0x00284874
	protected virtual void AutoSetDisplayName()
	{
		if (this.discreteActionsStart.Count > 0)
		{
			this.displayName = this.discreteActionsStart[0].name;
		}
		else if (this.transitionActions.Count > 0)
		{
			this.displayName = this.transitionActions[0].name;
		}
		else if (this.discreteActionsEnd.Count > 0)
		{
			this.displayName = this.discreteActionsEnd[0].name;
		}
		else
		{
			this.displayName = string.Empty;
		}
	}

	// Token: 0x06006BCC RID: 27596 RVA: 0x00286514 File Offset: 0x00284914
	protected virtual void AutoSetPreviewTextAndDisplayName()
	{
		this.AutoSetPreviewText();
		if (this.displayNameJSON.val != null && (this.displayNameJSON.val == string.Empty || this.displayNameJSON.val.StartsWith("A_")))
		{
			this.AutoSetDisplayName();
		}
	}

	// Token: 0x17000FD2 RID: 4050
	// (get) Token: 0x06006BCD RID: 27597 RVA: 0x00286571 File Offset: 0x00284971
	public IEnumerable<TriggerActionDiscrete> ActionsWithActiveTimers
	{
		get
		{
			return this.actionsWithActiveTimers;
		}
	}

	// Token: 0x17000FD3 RID: 4051
	// (get) Token: 0x06006BCE RID: 27598 RVA: 0x00286579 File Offset: 0x00284979
	public bool HasActionsWithActiveTimers
	{
		get
		{
			return this.actionsWithActiveTimers.Count > 0;
		}
	}

	// Token: 0x06006BCF RID: 27599 RVA: 0x00286589 File Offset: 0x00284989
	public void SetHasActiveTimer(TriggerActionDiscrete tad, bool hasTimer)
	{
		if (hasTimer)
		{
			this.actionsWithActiveTimers.Add(tad);
		}
		else
		{
			this.actionsWithActiveTimers.Remove(tad);
		}
	}

	// Token: 0x17000FD4 RID: 4052
	// (get) Token: 0x06006BD0 RID: 27600 RVA: 0x002865B0 File Offset: 0x002849B0
	// (set) Token: 0x06006BD1 RID: 27601 RVA: 0x002865B8 File Offset: 0x002849B8
	public float transitionInterpValue
	{
		get
		{
			return this._transitionInterpValue;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this._transitionInterpValue != num)
			{
				this._transitionInterpValue = num;
				if (this.transitionInterpValueSlider != null)
				{
					this.transitionInterpValueSlider.value = this._transitionInterpValue;
				}
				foreach (TriggerActionTransition triggerActionTransition in this.transitionActions)
				{
					triggerActionTransition.TriggerInterp(this._transitionInterpValue, false);
				}
			}
		}
	}

	// Token: 0x17000FD5 RID: 4053
	// (get) Token: 0x06006BD2 RID: 27602 RVA: 0x00286658 File Offset: 0x00284A58
	// (set) Token: 0x06006BD3 RID: 27603 RVA: 0x00286660 File Offset: 0x00284A60
	public bool active
	{
		get
		{
			return this._active;
		}
		set
		{
			if (this._active != value)
			{
				this._active = value;
				if (this.activeToggle)
				{
					this.activeToggle.isOn = this._active;
				}
				foreach (TriggerActionTransition triggerActionTransition in this.transitionActions)
				{
					triggerActionTransition.active = this._active;
					if (this._active && !this.reverse)
					{
						triggerActionTransition.InitTriggerStart();
					}
				}
				if ((this._active && !this.reverse) || (!this._active && this.reverse))
				{
					foreach (TriggerActionDiscrete triggerActionDiscrete in this.discreteActionsStart)
					{
						triggerActionDiscrete.Trigger(this.reverse, false);
					}
				}
				else
				{
					foreach (TriggerActionDiscrete triggerActionDiscrete2 in this.discreteActionsEnd)
					{
						triggerActionDiscrete2.Trigger(this.reverse, false);
					}
				}
			}
		}
	}

	// Token: 0x06006BD4 RID: 27604 RVA: 0x002867E8 File Offset: 0x00284BE8
	public void ForceTransitionsActive()
	{
		foreach (TriggerActionTransition triggerActionTransition in this.transitionActions)
		{
			if (!triggerActionTransition.active)
			{
				if (!this.reverse)
				{
					triggerActionTransition.InitTriggerStart();
				}
				triggerActionTransition.active = true;
			}
		}
	}

	// Token: 0x06006BD5 RID: 27605 RVA: 0x00286860 File Offset: 0x00284C60
	public void ForceTransitionsInactive()
	{
		foreach (TriggerActionTransition triggerActionTransition in this.transitionActions)
		{
			triggerActionTransition.active = false;
		}
	}

	// Token: 0x06006BD6 RID: 27606 RVA: 0x002868BC File Offset: 0x00284CBC
	public void Reset()
	{
		this._active = true;
		foreach (TriggerActionTransition triggerActionTransition in this.transitionActions)
		{
			triggerActionTransition.active = this._active;
		}
		this.transitionInterpValue = 0f;
		foreach (TriggerActionTransition triggerActionTransition2 in this.transitionActions)
		{
			triggerActionTransition2.TriggerInterp(this._transitionInterpValue, false);
		}
		this._active = false;
		foreach (TriggerActionTransition triggerActionTransition3 in this.transitionActions)
		{
			triggerActionTransition3.active = this._active;
		}
		if (this.activeToggle)
		{
			this.activeToggle.isOn = this._active;
		}
	}

	// Token: 0x06006BD7 RID: 27607 RVA: 0x002869FC File Offset: 0x00284DFC
	protected virtual void CreateTriggerActionMiniUI(ScrollRectContentManager contentManager, TriggerAction ta, int index, int totalCount)
	{
		if (contentManager != null && this.handler != null)
		{
			RectTransform rectTransform = this.handler.CreateTriggerActionMiniUI();
			if (rectTransform != null)
			{
				contentManager.AddItem(rectTransform, index, false);
				ta.triggerActionMiniPanel = rectTransform;
				if (index == -1)
				{
					rectTransform.SetSiblingIndex(0);
				}
				else
				{
					rectTransform.SetSiblingIndex(totalCount - index);
				}
				ta.InitTriggerActionMiniPanelUI();
			}
			else
			{
				Debug.LogError("Failed to create trigger action mini UI");
			}
		}
	}

	// Token: 0x06006BD8 RID: 27608 RVA: 0x00286A7C File Offset: 0x00284E7C
	public virtual RectTransform CreateTriggerActionDiscreteUI()
	{
		if (this.triggerActionsParent != null)
		{
			RectTransform rectTransform = this.handler.CreateTriggerActionDiscreteUI();
			rectTransform.SetParent(this.triggerActionsParent, false);
			rectTransform.gameObject.SetActive(false);
			return rectTransform;
		}
		Debug.LogError("Attempted to CreateTriggerActionDiscreteUI when triggerActionsParent was null");
		return null;
	}

	// Token: 0x06006BD9 RID: 27609 RVA: 0x00286ACC File Offset: 0x00284ECC
	public virtual RectTransform CreateTriggerActionTransitionUI()
	{
		if (this.triggerActionsParent != null)
		{
			RectTransform rectTransform = this.handler.CreateTriggerActionTransitionUI();
			rectTransform.SetParent(this.triggerActionsParent, false);
			rectTransform.gameObject.SetActive(false);
			return rectTransform;
		}
		Debug.LogError("Attempted to CreateTriggerActionTransitionUI when triggerActionsParent was null");
		return null;
	}

	// Token: 0x06006BDA RID: 27610 RVA: 0x00286B1C File Offset: 0x00284F1C
	public virtual TriggerActionDiscrete CreateDiscreteActionStartInternal(int index = -1)
	{
		TriggerActionDiscrete triggerActionDiscrete = new TriggerActionDiscrete();
		triggerActionDiscrete.doActionsInReverse = this.doActionsInReverse;
		triggerActionDiscrete.handler = this;
		if (index == -1)
		{
			this.discreteActionsStart.Add(triggerActionDiscrete);
		}
		else
		{
			this.discreteActionsStart.Insert(index, triggerActionDiscrete);
		}
		this.CreateTriggerActionMiniUI(this.discreteActionStartContentManager, triggerActionDiscrete, index, this.discreteActionsStart.Count);
		return triggerActionDiscrete;
	}

	// Token: 0x06006BDB RID: 27611 RVA: 0x00286B81 File Offset: 0x00284F81
	public virtual void CreateDiscreteActionStart()
	{
		this.CreateDiscreteActionStartInternal(-1);
	}

	// Token: 0x06006BDC RID: 27612 RVA: 0x00286B8C File Offset: 0x00284F8C
	public virtual TriggerActionTransition CreateTransitionActionInternal(int index = -1)
	{
		TriggerActionTransition triggerActionTransition = new TriggerActionTransition();
		triggerActionTransition.handler = this;
		if (index == -1)
		{
			this.transitionActions.Add(triggerActionTransition);
		}
		else
		{
			this.transitionActions.Insert(index, triggerActionTransition);
		}
		this.CreateTriggerActionMiniUI(this.transitionActionsContentManager, triggerActionTransition, index, this.transitionActions.Count);
		return triggerActionTransition;
	}

	// Token: 0x06006BDD RID: 27613 RVA: 0x00286BE8 File Offset: 0x00284FE8
	public virtual void CreateTransitionAction()
	{
		TriggerActionTransition triggerActionTransition = this.CreateTransitionActionInternal(-1);
		triggerActionTransition.active = this._active;
		triggerActionTransition.TriggerInterp(this._transitionInterpValue, false);
	}

	// Token: 0x06006BDE RID: 27614 RVA: 0x00286C18 File Offset: 0x00285018
	public virtual TriggerActionDiscrete CreateDiscreteActionEndInternal(int index = -1)
	{
		TriggerActionDiscrete triggerActionDiscrete = new TriggerActionDiscrete();
		triggerActionDiscrete.doActionsInReverse = this.doActionsInReverse;
		triggerActionDiscrete.handler = this;
		if (index == -1)
		{
			this.discreteActionsEnd.Add(triggerActionDiscrete);
		}
		else
		{
			this.discreteActionsEnd.Insert(index, triggerActionDiscrete);
		}
		this.CreateTriggerActionMiniUI(this.discreteActionEndContentManager, triggerActionDiscrete, index, this.discreteActionsEnd.Count);
		return triggerActionDiscrete;
	}

	// Token: 0x06006BDF RID: 27615 RVA: 0x00286C7D File Offset: 0x0028507D
	public virtual void CreateDiscreteActionEnd()
	{
		this.CreateDiscreteActionEndInternal(-1);
	}

	// Token: 0x06006BE0 RID: 27616 RVA: 0x00286C88 File Offset: 0x00285088
	public void RemoveTriggerAction(TriggerAction ta)
	{
		if (ta is TriggerActionDiscrete)
		{
			if (this.discreteActionsStart.Remove(ta as TriggerActionDiscrete))
			{
				this.actionsWithActiveTimers.Remove(ta as TriggerActionDiscrete);
				if (ta.triggerActionMiniPanel != null && this.discreteActionStartContentManager != null)
				{
					this.discreteActionStartContentManager.RemoveItem(ta.triggerActionMiniPanel);
				}
			}
			else
			{
				if (!this.discreteActionsEnd.Remove(ta as TriggerActionDiscrete))
				{
					Debug.LogError("TriggerAction not found. Cannot remove");
					return;
				}
				this.actionsWithActiveTimers.Remove(ta as TriggerActionDiscrete);
				if (ta.triggerActionMiniPanel != null && this.discreteActionEndContentManager != null)
				{
					this.discreteActionEndContentManager.RemoveItem(ta.triggerActionMiniPanel);
				}
			}
		}
		else if (ta is TriggerActionTransition)
		{
			if (!this.transitionActions.Remove(ta as TriggerActionTransition))
			{
				Debug.LogError("TriggerAction not found. Cannot remove");
				return;
			}
			if (ta.triggerActionMiniPanel != null && this.transitionActionsContentManager != null)
			{
				this.transitionActionsContentManager.RemoveItem(ta.triggerActionMiniPanel);
			}
		}
		if (this.handler != null)
		{
			if (ta.triggerActionPanel != null)
			{
				this.handler.RemoveTriggerActionUI(ta.triggerActionPanel);
				ta.triggerActionPanel = null;
			}
			if (ta.triggerActionMiniPanel != null)
			{
				this.handler.RemoveTriggerActionUI(ta.triggerActionMiniPanel);
				ta.triggerActionMiniPanel = null;
			}
		}
	}

	// Token: 0x06006BE1 RID: 27617 RVA: 0x00286E34 File Offset: 0x00285234
	public void DuplicateTriggerAction(TriggerAction ta)
	{
		if (ta is TriggerActionDiscrete)
		{
			TriggerActionDiscrete triggerActionDiscrete = ta as TriggerActionDiscrete;
			int num = this.discreteActionsStart.IndexOf(triggerActionDiscrete);
			if (num == -1)
			{
				num = this.discreteActionsEnd.IndexOf(triggerActionDiscrete);
				if (num != -1)
				{
					JSONClass json = triggerActionDiscrete.GetJSON();
					TriggerActionDiscrete triggerActionDiscrete2 = this.CreateDiscreteActionEndInternal(num + 1);
					triggerActionDiscrete2.RestoreFromJSON(json);
				}
			}
			else
			{
				JSONClass json2 = triggerActionDiscrete.GetJSON();
				TriggerActionDiscrete triggerActionDiscrete3 = this.CreateDiscreteActionStartInternal(num + 1);
				triggerActionDiscrete3.RestoreFromJSON(json2);
			}
		}
		else if (ta is TriggerActionTransition)
		{
			TriggerActionTransition triggerActionTransition = ta as TriggerActionTransition;
			int num2 = this.transitionActions.IndexOf(triggerActionTransition);
			if (num2 != -1)
			{
				JSONClass json3 = triggerActionTransition.GetJSON();
				TriggerActionTransition triggerActionTransition2 = this.CreateTransitionActionInternal(num2);
				triggerActionTransition2.RestoreFromJSON(json3);
				triggerActionTransition2.active = this._active;
				triggerActionTransition2.TriggerInterp(this._transitionInterpValue, false);
			}
		}
	}

	// Token: 0x06006BE2 RID: 27618 RVA: 0x00286F1A File Offset: 0x0028531A
	public void TriggerActionNameChange(TriggerAction ta)
	{
		this.AutoSetPreviewTextAndDisplayName();
	}

	// Token: 0x06006BE3 RID: 27619 RVA: 0x00286F24 File Offset: 0x00285324
	public virtual void InitTriggerUI()
	{
		if (this.triggerPanel != null)
		{
			TriggerUI componentInChildren = this.triggerPanel.GetComponentInChildren<TriggerUI>();
			if (componentInChildren != null)
			{
				this.removeButton = componentInChildren.removeButton;
				this.duplicateButton = componentInChildren.duplicateButton;
				this.triggerActionsButton = componentInChildren.triggerActionsButton;
				this.displayNameJSON.inputField = componentInChildren.displayNameField;
				this.previewTextJSON.text = componentInChildren.previewTextPopupText;
				this.activeToggle = componentInChildren.activeToggle;
				this.transitionInterpValueSlider = componentInChildren.transitionInterpValueSlider;
			}
			if (this.removeButton != null)
			{
				this.removeButton.onClick.AddListener(new UnityAction(this.Remove));
			}
			if (this.duplicateButton != null)
			{
				this.duplicateButton.onClick.AddListener(new UnityAction(this.Duplicate));
			}
			if (this.triggerActionsButton != null)
			{
				this.triggerActionsButton.onClick.AddListener(new UnityAction(this.OpenTriggerActionsPanel));
			}
			if (this.activeToggle != null)
			{
				this.activeToggle.isOn = this._active;
			}
			if (this.transitionInterpValueSlider != null)
			{
				this.transitionInterpValueSlider.value = this._transitionInterpValue;
			}
		}
	}

	// Token: 0x06006BE4 RID: 27620 RVA: 0x00287088 File Offset: 0x00285488
	public virtual void InitTriggerActionsUI()
	{
		if (this.triggerActionsPanel != null)
		{
			TriggerActionsPanelUI componentInChildren = this.triggerActionsPanel.GetComponentInChildren<TriggerActionsPanelUI>();
			if (componentInChildren != null)
			{
				this.displayNameJSON.text = componentInChildren.triggerDisplayNameText;
				this.closeTriggerActionsPanelButton = componentInChildren.closeTriggerActionsPanelButton;
				this.clearActionsButton = componentInChildren.clearActionsButtons;
				this.addDiscreteActionStartButton = componentInChildren.addDiscreteActionStartButton;
				this.addTransitionActionButton = componentInChildren.addTransitionActionButton;
				this.addDiscreteActionEndButton = componentInChildren.addDiscreteActionEndButton;
				this.copyDiscreteActionsStartButton = componentInChildren.copyDiscreteActionsStartButton;
				this.pasteDiscreteActionsStartButton = componentInChildren.pasteDiscreteActionsStartButton;
				this.copyTransitionActionsButton = componentInChildren.copyTransitionActionsButton;
				this.pasteTransitionActionsButton = componentInChildren.pasteTransitionActionsButton;
				this.copyDiscreteActionsEndButton = componentInChildren.copyDiscreteActionsEndButton;
				this.pasteDiscreteActionsEndButton = componentInChildren.pasteDiscreteActionsEndButton;
				this.discreteActionStartContentManager = componentInChildren.discreteActionsStartContentManager;
				if (this.discreteActionStartContentManager != null)
				{
					this.discreteActionStartContentManager.RemoveAllItems();
					for (int i = 0; i < this.discreteActionsStart.Count; i++)
					{
						this.CreateTriggerActionMiniUI(this.discreteActionStartContentManager, this.discreteActionsStart[i], i, this.discreteActionsStart.Count);
					}
				}
				this.transitionActionsContentManager = componentInChildren.transitionActionsContentManager;
				if (this.transitionActionsContentManager != null)
				{
					this.transitionActionsContentManager.RemoveAllItems();
					for (int j = 0; j < this.transitionActions.Count; j++)
					{
						this.CreateTriggerActionMiniUI(this.transitionActionsContentManager, this.transitionActions[j], j, this.transitionActions.Count);
					}
				}
				this.discreteActionEndContentManager = componentInChildren.discreteActionsEndContentManager;
				if (this.discreteActionEndContentManager != null)
				{
					this.discreteActionEndContentManager.RemoveAllItems();
					for (int k = 0; k < this.discreteActionsEnd.Count; k++)
					{
						this.CreateTriggerActionMiniUI(this.discreteActionEndContentManager, this.discreteActionsEnd[k], k, this.discreteActionsEnd.Count);
					}
				}
			}
			if (this.closeTriggerActionsPanelButton != null)
			{
				this.closeTriggerActionsPanelButton.onClick.AddListener(new UnityAction(this.CloseTriggerActionsPanel));
			}
			if (this.clearActionsButton != null)
			{
				this.clearActionsButton.onClick.AddListener(new UnityAction(this.ClearActions));
			}
			if (this.addDiscreteActionStartButton != null)
			{
				this.addDiscreteActionStartButton.onClick.AddListener(new UnityAction(this.CreateDiscreteActionStart));
			}
			if (this.addTransitionActionButton != null)
			{
				this.addTransitionActionButton.onClick.AddListener(new UnityAction(this.CreateTransitionAction));
			}
			if (this.addDiscreteActionEndButton != null)
			{
				this.addDiscreteActionEndButton.onClick.AddListener(new UnityAction(this.CreateDiscreteActionEnd));
			}
			if (this.copyDiscreteActionsStartButton != null)
			{
				this.copyDiscreteActionsStartButton.onClick.AddListener(new UnityAction(this.CopyDiscreteActionsStart));
			}
			if (this.pasteDiscreteActionsStartButton != null)
			{
				this.pasteDiscreteActionsStartButton.onClick.AddListener(new UnityAction(this.PasteDiscreteActionsStart));
			}
			if (this.copyTransitionActionsButton != null)
			{
				this.copyTransitionActionsButton.onClick.AddListener(new UnityAction(this.CopyTransitionActions));
			}
			if (this.pasteTransitionActionsButton != null)
			{
				this.pasteTransitionActionsButton.onClick.AddListener(new UnityAction(this.PasteTransitionActions));
			}
			if (this.copyDiscreteActionsEndButton != null)
			{
				this.copyDiscreteActionsEndButton.onClick.AddListener(new UnityAction(this.CopyDiscreteActionsEnd));
			}
			if (this.pasteDiscreteActionsEndButton != null)
			{
				this.pasteDiscreteActionsEndButton.onClick.AddListener(new UnityAction(this.PasteDiscreteActionsEnd));
			}
		}
	}

	// Token: 0x06006BE5 RID: 27621 RVA: 0x00287480 File Offset: 0x00285880
	public virtual void DeregisterUI()
	{
		this.displayNameJSON.inputField = null;
		this.previewTextJSON.text = null;
		if (this.removeButton != null)
		{
			this.removeButton.onClick.RemoveListener(new UnityAction(this.Remove));
		}
		if (this.duplicateButton != null)
		{
			this.duplicateButton.onClick.RemoveListener(new UnityAction(this.Duplicate));
		}
		if (this.triggerActionsButton != null)
		{
			this.triggerActionsButton.onClick.RemoveListener(new UnityAction(this.OpenTriggerActionsPanel));
		}
		if (this.closeTriggerActionsPanelButton != null)
		{
			this.closeTriggerActionsPanelButton.onClick.RemoveListener(new UnityAction(this.CloseTriggerActionsPanel));
		}
		if (this.clearActionsButton != null)
		{
			this.clearActionsButton.onClick.RemoveListener(new UnityAction(this.ClearActions));
		}
		if (this.addDiscreteActionStartButton != null)
		{
			this.addDiscreteActionStartButton.onClick.RemoveListener(new UnityAction(this.CreateDiscreteActionStart));
		}
		if (this.addTransitionActionButton != null)
		{
			this.addTransitionActionButton.onClick.RemoveListener(new UnityAction(this.CreateTransitionAction));
		}
		if (this.addDiscreteActionEndButton != null)
		{
			this.addDiscreteActionEndButton.onClick.RemoveListener(new UnityAction(this.CreateDiscreteActionEnd));
		}
		if (this.copyDiscreteActionsStartButton != null)
		{
			this.copyDiscreteActionsStartButton.onClick.RemoveListener(new UnityAction(this.CopyDiscreteActionsStart));
		}
		if (this.pasteDiscreteActionsStartButton != null)
		{
			this.pasteDiscreteActionsStartButton.onClick.RemoveListener(new UnityAction(this.PasteDiscreteActionsStart));
		}
		if (this.copyTransitionActionsButton != null)
		{
			this.copyTransitionActionsButton.onClick.RemoveListener(new UnityAction(this.CopyTransitionActions));
		}
		if (this.pasteTransitionActionsButton != null)
		{
			this.pasteTransitionActionsButton.onClick.RemoveListener(new UnityAction(this.PasteTransitionActions));
		}
		if (this.copyDiscreteActionsEndButton != null)
		{
			this.copyDiscreteActionsEndButton.onClick.RemoveListener(new UnityAction(this.CopyDiscreteActionsEnd));
		}
		if (this.pasteDiscreteActionsEndButton != null)
		{
			this.pasteDiscreteActionsEndButton.onClick.RemoveListener(new UnityAction(this.PasteDiscreteActionsEnd));
		}
	}

	// Token: 0x06006BE6 RID: 27622 RVA: 0x00287724 File Offset: 0x00285B24
	public virtual void Update()
	{
		if (this.actionsWithActiveTimers.Count > 0)
		{
			foreach (TriggerActionDiscrete triggerActionDiscrete in this.discreteActionsStart)
			{
				triggerActionDiscrete.Update();
			}
			foreach (TriggerActionDiscrete triggerActionDiscrete2 in this.discreteActionsEnd)
			{
				triggerActionDiscrete2.Update();
			}
		}
	}

	// Token: 0x04005D5E RID: 23902
	public static List<TriggerActionDiscrete> copyOfDiscreteActions;

	// Token: 0x04005D5F RID: 23903
	public static List<TriggerActionTransition> copyOfTransitionActions;

	// Token: 0x04005D60 RID: 23904
	protected List<TriggerActionDiscrete> discreteActionsStart;

	// Token: 0x04005D61 RID: 23905
	protected List<TriggerActionTransition> transitionActions;

	// Token: 0x04005D62 RID: 23906
	protected List<TriggerActionDiscrete> discreteActionsEnd;

	// Token: 0x04005D63 RID: 23907
	protected Button closeTriggerActionsPanelButton;

	// Token: 0x04005D64 RID: 23908
	protected Button clearActionsButton;

	// Token: 0x04005D65 RID: 23909
	protected Button addDiscreteActionStartButton;

	// Token: 0x04005D66 RID: 23910
	protected Button addTransitionActionButton;

	// Token: 0x04005D67 RID: 23911
	protected Button addDiscreteActionEndButton;

	// Token: 0x04005D68 RID: 23912
	protected Button copyDiscreteActionsStartButton;

	// Token: 0x04005D69 RID: 23913
	protected Button pasteDiscreteActionsStartButton;

	// Token: 0x04005D6A RID: 23914
	protected Button copyTransitionActionsButton;

	// Token: 0x04005D6B RID: 23915
	protected Button pasteTransitionActionsButton;

	// Token: 0x04005D6C RID: 23916
	protected Button copyDiscreteActionsEndButton;

	// Token: 0x04005D6D RID: 23917
	protected Button pasteDiscreteActionsEndButton;

	// Token: 0x04005D6E RID: 23918
	protected ScrollRectContentManager discreteActionStartContentManager;

	// Token: 0x04005D6F RID: 23919
	protected ScrollRectContentManager transitionActionsContentManager;

	// Token: 0x04005D70 RID: 23920
	protected ScrollRectContentManager discreteActionEndContentManager;

	// Token: 0x04005D71 RID: 23921
	public TriggerHandler handler;

	// Token: 0x04005D72 RID: 23922
	public Transform triggerPanel;

	// Token: 0x04005D73 RID: 23923
	public Transform triggerActionsPanel;

	// Token: 0x04005D74 RID: 23924
	public Transform triggerActionsParent;

	// Token: 0x04005D75 RID: 23925
	protected Button removeButton;

	// Token: 0x04005D76 RID: 23926
	protected Button duplicateButton;

	// Token: 0x04005D77 RID: 23927
	protected Button triggerActionsButton;

	// Token: 0x04005D78 RID: 23928
	public Trigger.OnOpenTriggerActionsPanel onOpenTriggerActionsPanel;

	// Token: 0x04005D79 RID: 23929
	public Trigger.OnCloseTriggerActionsPanel onCloseTriggerActionsPanel;

	// Token: 0x04005D7A RID: 23930
	protected JSONStorableString previewTextJSON;

	// Token: 0x04005D7B RID: 23931
	protected JSONStorableString displayNameJSON;

	// Token: 0x04005D7C RID: 23932
	protected HashSet<TriggerActionDiscrete> actionsWithActiveTimers;

	// Token: 0x04005D7D RID: 23933
	protected Slider transitionInterpValueSlider;

	// Token: 0x04005D7E RID: 23934
	protected float _transitionInterpValue;

	// Token: 0x04005D7F RID: 23935
	protected Toggle activeToggle;

	// Token: 0x04005D80 RID: 23936
	public bool doActionsInReverse = true;

	// Token: 0x04005D81 RID: 23937
	public bool reverse;

	// Token: 0x04005D82 RID: 23938
	protected bool _active;

	// Token: 0x02000DA5 RID: 3493
	// (Invoke) Token: 0x06006BE8 RID: 27624
	public delegate void OnOpenTriggerActionsPanel();

	// Token: 0x02000DA6 RID: 3494
	// (Invoke) Token: 0x06006BEC RID: 27628
	public delegate void OnCloseTriggerActionsPanel();
}
