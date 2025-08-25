using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C3F RID: 3135
public class ObjectChooser : JSONStorable
{
	// Token: 0x06005B40 RID: 23360 RVA: 0x0021804A File Offset: 0x0021644A
	public ObjectChooser()
	{
	}

	// Token: 0x17000D67 RID: 3431
	// (get) Token: 0x06005B41 RID: 23361 RVA: 0x00218052 File Offset: 0x00216452
	// (set) Token: 0x06005B42 RID: 23362 RVA: 0x0021805A File Offset: 0x0021645A
	public ObjectChoice CurrentChoice
	{
		[CompilerGenerated]
		get
		{
			return this.<CurrentChoice>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<CurrentChoice>k__BackingField = value;
		}
	}

	// Token: 0x06005B43 RID: 23363 RVA: 0x00218064 File Offset: 0x00216464
	protected void SyncChoice(string s)
	{
		ObjectChoice objectChoice;
		if (this.addNoneChoice && s == "None")
		{
			if (this.CurrentChoice != null)
			{
				this.CurrentChoice.gameObject.SetActive(false);
			}
			this.CurrentChoice = null;
			if (this.onChoiceChangedHandlers != null)
			{
				this.onChoiceChangedHandlers("None");
			}
		}
		else if (this.choiceNameToObjectChoice.TryGetValue(s, out objectChoice))
		{
			if (this.CurrentChoice != null)
			{
				this.CurrentChoice.gameObject.SetActive(false);
			}
			this.CurrentChoice = objectChoice;
			objectChoice.gameObject.SetActive(true);
			if (this.onChoiceChangedHandlers != null)
			{
				this.onChoiceChangedHandlers(s);
			}
		}
	}

	// Token: 0x06005B44 RID: 23364 RVA: 0x00218134 File Offset: 0x00216534
	protected virtual void Init()
	{
		this.choiceNames = new List<string>();
		if (this.addNoneChoice)
		{
			this.choiceNames.Add("None");
		}
		if (this.choiceContainer != null)
		{
			this.choices = this.choiceContainer.GetComponentsInChildren<ObjectChoice>(true);
		}
		else
		{
			this.choices = base.GetComponentsInChildren<ObjectChoice>(true);
		}
		this.choiceNameToObjectChoice = new Dictionary<string, ObjectChoice>();
		foreach (ObjectChoice objectChoice in this.choices)
		{
			this.choiceNames.Add(objectChoice.displayName);
			this.choiceNameToObjectChoice.Add(objectChoice.displayName, objectChoice);
			if (this.startingChoiceName == null || this.startingChoiceName == string.Empty)
			{
				if (objectChoice.gameObject.activeSelf)
				{
					this.CurrentChoice = objectChoice;
					this.startingChoiceName = this.CurrentChoice.displayName;
				}
			}
			else if (objectChoice.displayName == this.startingChoiceName)
			{
				this.CurrentChoice = objectChoice;
				objectChoice.gameObject.SetActive(true);
			}
			else
			{
				objectChoice.gameObject.SetActive(false);
			}
		}
		if (this.useStoreIdForChooserDisplayName)
		{
			this.chooserJSON = new JSONStorableStringChooser("choiceName", this.choiceNames, this.startingChoiceName, base.storeId, new JSONStorableStringChooser.SetStringCallback(this.SyncChoice));
		}
		else
		{
			this.chooserJSON = new JSONStorableStringChooser("choiceName", this.choiceNames, this.startingChoiceName, null, new JSONStorableStringChooser.SetStringCallback(this.SyncChoice));
		}
		base.RegisterStringChooser(this.chooserJSON);
	}

	// Token: 0x06005B45 RID: 23365 RVA: 0x002182E4 File Offset: 0x002166E4
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			foreach (ObjectChoice objectChoice in this.choices)
			{
				JSONStorable[] componentsInChildren = objectChoice.GetComponentsInChildren<JSONStorable>(true);
				foreach (JSONStorable jsonstorable in componentsInChildren)
				{
					jsonstorable.SetUI(this.UITransform);
				}
			}
			ObjectChooserUI componentInChildren = this.UITransform.GetComponentInChildren<ObjectChooserUI>();
			if (componentInChildren != null)
			{
				this.chooserJSON.popup = componentInChildren.chooserPopup;
			}
		}
	}

	// Token: 0x06005B46 RID: 23366 RVA: 0x00218388 File Offset: 0x00216788
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			foreach (ObjectChoice objectChoice in this.choices)
			{
				JSONStorable[] componentsInChildren = objectChoice.GetComponentsInChildren<JSONStorable>(true);
				foreach (JSONStorable jsonstorable in componentsInChildren)
				{
					jsonstorable.SetUIAlt(this.UITransformAlt);
				}
			}
			ObjectChooserUI componentInChildren = this.UITransformAlt.GetComponentInChildren<ObjectChooserUI>(true);
			if (componentInChildren != null)
			{
				this.chooserJSON.popupAlt = componentInChildren.chooserPopup;
			}
		}
	}

	// Token: 0x06005B47 RID: 23367 RVA: 0x0021842A File Offset: 0x0021682A
	public void ForceAwake()
	{
		this.Awake();
	}

	// Token: 0x06005B48 RID: 23368 RVA: 0x00218432 File Offset: 0x00216832
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

	// Token: 0x04004B3A RID: 19258
	public Transform choiceContainer;

	// Token: 0x04004B3B RID: 19259
	public string startingChoiceName;

	// Token: 0x04004B3C RID: 19260
	public bool addNoneChoice;

	// Token: 0x04004B3D RID: 19261
	protected ObjectChoice[] choices;

	// Token: 0x04004B3E RID: 19262
	protected List<string> choiceNames;

	// Token: 0x04004B3F RID: 19263
	protected Dictionary<string, ObjectChoice> choiceNameToObjectChoice;

	// Token: 0x04004B40 RID: 19264
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private ObjectChoice <CurrentChoice>k__BackingField;

	// Token: 0x04004B41 RID: 19265
	public ObjectChooser.ChoiceChanged onChoiceChangedHandlers;

	// Token: 0x04004B42 RID: 19266
	public bool useStoreIdForChooserDisplayName;

	// Token: 0x04004B43 RID: 19267
	public JSONStorableStringChooser chooserJSON;

	// Token: 0x02000C40 RID: 3136
	// (Invoke) Token: 0x06005B4A RID: 23370
	public delegate void ChoiceChanged(string s);
}
