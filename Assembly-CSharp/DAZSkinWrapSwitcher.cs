using System;
using System.Collections.Generic;
using GPUTools.Cloth.Scripts;
using GPUTools.Painter.Scripts;
using UnityEngine;

// Token: 0x02000B1B RID: 2843
public class DAZSkinWrapSwitcher : JSONStorable
{
	// Token: 0x06004D9B RID: 19867 RVA: 0x001B3E84 File Offset: 0x001B2284
	public DAZSkinWrapSwitcher()
	{
	}

	// Token: 0x06004D9C RID: 19868 RVA: 0x001B3E8C File Offset: 0x001B228C
	public void SetCurrentWrapName(string wrapName)
	{
		DAZSkinWrap[] components = base.GetComponents<DAZSkinWrap>();
		this._currentWrapName = null;
		this._currentWrap = null;
		foreach (DAZSkinWrap dazskinWrap in components)
		{
			if (dazskinWrap.enabled)
			{
				if (this.clothSettings != null)
				{
					dazskinWrap.draw = false;
					if (dazskinWrap.wrapName == wrapName)
					{
						this.clothSettings.MeshProvider.PreCalcProvider = dazskinWrap;
						PainterSettings[] componentsInChildren = base.GetComponentsInChildren<PainterSettings>(true);
						PainterSettings painterSettings = null;
						PainterSettings painterSettings2 = null;
						string b = base.name + "Painter";
						string b2 = base.name + "Painter" + wrapName;
						foreach (PainterSettings painterSettings3 in componentsInChildren)
						{
							if (painterSettings3.name == b)
							{
								painterSettings = painterSettings3;
							}
							else if (painterSettings3.name == b2)
							{
								painterSettings2 = painterSettings3;
							}
						}
						bool flag = false;
						if (painterSettings2 != null)
						{
							if (this.clothSettings.EditorPainter != painterSettings2)
							{
								flag = true;
								this.clothSettings.EditorPainter = painterSettings2;
							}
						}
						else if (painterSettings != null && this.clothSettings.EditorPainter != painterSettings)
						{
							flag = true;
							this.clothSettings.EditorPainter = painterSettings;
						}
						if (flag && this.clothSettings.builder != null)
						{
							if (this.clothSettings.builder.physicsBlend != null)
							{
								this.clothSettings.builder.physicsBlend.Build();
							}
							if (this.clothSettings.builder.pointJoints != null)
							{
								this.clothSettings.builder.pointJoints.UpdateSettings();
							}
						}
						this._currentWrapName = wrapName;
						this._currentWrap = dazskinWrap;
						dazskinWrap.draw = true;
					}
				}
				else
				{
					dazskinWrap.draw = false;
					if (dazskinWrap.wrapName == wrapName)
					{
						this._currentWrapName = wrapName;
						this._currentWrap = dazskinWrap;
						dazskinWrap.draw = true;
					}
				}
			}
		}
		this.SyncSkinWrap();
	}

	// Token: 0x06004D9D RID: 19869 RVA: 0x001B40C0 File Offset: 0x001B24C0
	protected void SyncSkinWrap()
	{
		DAZSkinWrapMaterialOptions[] components = base.GetComponents<DAZSkinWrapMaterialOptions>();
		foreach (DAZSkinWrapMaterialOptions dazskinWrapMaterialOptions in components)
		{
			dazskinWrapMaterialOptions.skinWrap = this._currentWrap;
		}
		DAZSkinWrapControl component = base.GetComponent<DAZSkinWrapControl>();
		if (component != null)
		{
			component.wrap = this._currentWrap;
		}
	}

	// Token: 0x17000B0D RID: 2829
	// (get) Token: 0x06004D9E RID: 19870 RVA: 0x001B411C File Offset: 0x001B251C
	// (set) Token: 0x06004D9F RID: 19871 RVA: 0x001B4124 File Offset: 0x001B2524
	public string currentWrapName
	{
		get
		{
			return this._currentWrapName;
		}
		set
		{
			if (this.currentWrapNameJSON != null)
			{
				this.currentWrapNameJSON.val = value;
			}
			else if (this._currentWrapName != value)
			{
				this.SetCurrentWrapName(value);
			}
		}
	}

	// Token: 0x06004DA0 RID: 19872 RVA: 0x001B415C File Offset: 0x001B255C
	public void Init()
	{
		DAZSkinWrap[] components = base.GetComponents<DAZSkinWrap>();
		List<string> list = new List<string>();
		this.clothSettings = base.GetComponent<ClothSettings>();
		foreach (DAZSkinWrap dazskinWrap in components)
		{
			if (dazskinWrap.enabled)
			{
				list.Add(dazskinWrap.wrapName);
				if (this.clothSettings != null && this.clothSettings.MeshProvider.PreCalcProvider != null)
				{
					this._currentWrap = (this.clothSettings.MeshProvider.PreCalcProvider as DAZSkinWrap);
					this._currentWrapName = this._currentWrap.wrapName;
				}
				else if (dazskinWrap.draw)
				{
					this._currentWrap = dazskinWrap;
					this._currentWrapName = this._currentWrap.wrapName;
				}
			}
		}
		if (list.Count > 1 && this._currentWrap != null)
		{
			if (this.currentWrapNameJSON == null)
			{
				this.currentWrapNameJSON = new JSONStorableStringChooser("wrapName", list, this._currentWrapName, null, new JSONStorableStringChooser.SetStringCallback(this.SetCurrentWrapName));
				base.RegisterStringChooser(this.currentWrapNameJSON);
			}
			else
			{
				this.currentWrapNameJSON.choices = list;
				this.currentWrapNameJSON.valNoCallback = this._currentWrapName;
			}
			this.SetCurrentWrapName(this._currentWrapName);
		}
	}

	// Token: 0x06004DA1 RID: 19873 RVA: 0x001B42C0 File Offset: 0x001B26C0
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			DAZSkinWrapSwitcherUI componentInChildren = this.UITransform.GetComponentInChildren<DAZSkinWrapSwitcherUI>(true);
			if (componentInChildren != null && componentInChildren.currentWrapNamePopup != null)
			{
				if (this.currentWrapNameJSON != null)
				{
					componentInChildren.currentWrapNamePopup.gameObject.SetActive(true);
					this.currentWrapNameJSON.popup = componentInChildren.currentWrapNamePopup;
				}
				else
				{
					componentInChildren.currentWrapNamePopup.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06004DA2 RID: 19874 RVA: 0x001B434C File Offset: 0x001B274C
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			DAZSkinWrapSwitcherUI componentInChildren = this.UITransformAlt.GetComponentInChildren<DAZSkinWrapSwitcherUI>(true);
			if (componentInChildren != null && componentInChildren.currentWrapNamePopup != null)
			{
				if (this.currentWrapNameJSON != null)
				{
					componentInChildren.currentWrapNamePopup.gameObject.SetActive(true);
					this.currentWrapNameJSON.popupAlt = componentInChildren.currentWrapNamePopup;
				}
				else
				{
					componentInChildren.currentWrapNamePopup.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06004DA3 RID: 19875 RVA: 0x001B43D7 File Offset: 0x001B27D7
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

	// Token: 0x04003D60 RID: 15712
	protected DAZSkinWrap _currentWrap;

	// Token: 0x04003D61 RID: 15713
	protected ClothSettings clothSettings;

	// Token: 0x04003D62 RID: 15714
	protected JSONStorableStringChooser currentWrapNameJSON;

	// Token: 0x04003D63 RID: 15715
	[SerializeField]
	protected string _currentWrapName;
}
