using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D70 RID: 3440
public class HairSimScalpMaskToolControl : CapsuleToolControl
{
	// Token: 0x06006A1C RID: 27164 RVA: 0x0027F203 File Offset: 0x0027D603
	public HairSimScalpMaskToolControl()
	{
	}

	// Token: 0x06006A1D RID: 27165 RVA: 0x0027F20C File Offset: 0x0027D60C
	protected void SyncTool()
	{
		this.maskTool.enabled = false;
		this.unmaskTool.enabled = false;
		HairSimScalpMaskToolControl.ToolChoice toolChoice = this.toolChoice;
		if (toolChoice != HairSimScalpMaskToolControl.ToolChoice.Mask)
		{
			if (toolChoice == HairSimScalpMaskToolControl.ToolChoice.Unmask)
			{
				this.unmaskTool.enabled = true;
			}
		}
		else
		{
			this.maskTool.enabled = true;
		}
	}

	// Token: 0x06006A1E RID: 27166 RVA: 0x0027F26C File Offset: 0x0027D66C
	protected void SyncToolChoice(string s)
	{
		try
		{
			this.toolChoice = (HairSimScalpMaskToolControl.ToolChoice)Enum.Parse(typeof(HairSimScalpMaskToolControl.ToolChoice), s);
			this.SyncTool();
		}
		catch (ArgumentException)
		{
			Debug.LogError("Attempted to set tool to " + s + " which is not a valid ToolChoice");
		}
	}

	// Token: 0x06006A1F RID: 27167 RVA: 0x0027F2CC File Offset: 0x0027D6CC
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (this.wasInit)
		{
			base.InitUI(t, isAlt);
			if (t != null)
			{
				HairSimScalpMaskToolControlUI componentInChildren = this.UITransform.GetComponentInChildren<HairSimScalpMaskToolControlUI>();
				if (componentInChildren != null)
				{
					this.toolChoiceJSON.RegisterPopup(componentInChildren.toolChoicePopup, isAlt);
				}
			}
			this.SyncTool();
		}
	}

	// Token: 0x06006A20 RID: 27168 RVA: 0x0027F328 File Offset: 0x0027D728
	protected override void Init()
	{
		base.Init();
		List<string> choicesList = new List<string>(Enum.GetNames(typeof(HairSimScalpMaskToolControl.ToolChoice)));
		this.toolChoiceJSON = new JSONStorableStringChooser("toolChoice", choicesList, this.toolChoice.ToString(), "Tool", new JSONStorableStringChooser.SetStringCallback(this.SyncToolChoice));
		base.RegisterStringChooser(this.toolChoiceJSON);
		this.SyncTool();
	}

	// Token: 0x04005BFB RID: 23547
	public SelectableUnselect maskTool;

	// Token: 0x04005BFC RID: 23548
	public SelectableSelect unmaskTool;

	// Token: 0x04005BFD RID: 23549
	public HairSimScalpMaskToolControl.ToolChoice toolChoice;

	// Token: 0x04005BFE RID: 23550
	protected JSONStorableStringChooser toolChoiceJSON;

	// Token: 0x02000D71 RID: 3441
	public enum ToolChoice
	{
		// Token: 0x04005C00 RID: 23552
		Mask,
		// Token: 0x04005C01 RID: 23553
		Unmask
	}
}
