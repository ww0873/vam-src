using System;
using UnityEngine;

// Token: 0x02000C8F RID: 3215
public class DynamicGeometry : JSONStorable
{
	// Token: 0x060060FB RID: 24827 RVA: 0x0024A8FF File Offset: 0x00248CFF
	public DynamicGeometry()
	{
	}

	// Token: 0x060060FC RID: 24828 RVA: 0x0024A908 File Offset: 0x00248D08
	protected void SyncUITabs()
	{
		if (this.materialUIConnector != null && this.materialUIConnectorMaster != null)
		{
			this.materialUIConnector.ClearConnectors();
			this.materialUIConnectorMaster.ClearRuntimeTabs(true);
			MaterialOptions[] componentsInChildren = base.GetComponentsInChildren<MaterialOptions>(false);
			foreach (MaterialOptions materialOptions in componentsInChildren)
			{
				this.materialUIConnector.AddConnector(materialOptions);
				TabbedUIBuilder.Tab tab = new TabbedUIBuilder.Tab();
				tab.name = materialOptions.storeId;
				tab.prefab = this.materialUIPrefab;
				tab.color = this.materialUITabColor;
				this.materialUIConnectorMaster.AddTab(tab, null);
			}
			if (this.imageControlUIConnector != null)
			{
				this.imageControlUIConnector.ClearConnectors();
				ImageControl[] componentsInChildren2 = base.GetComponentsInChildren<ImageControl>(false);
				foreach (ImageControl imageControl in componentsInChildren2)
				{
					this.imageControlUIConnector.AddConnector(imageControl);
					TabbedUIBuilder.Tab tab2 = new TabbedUIBuilder.Tab();
					tab2.name = imageControl.storeId;
					tab2.prefab = this.imageControlUIPrefab;
					tab2.color = this.imageControlUITabColor;
					this.materialUIConnectorMaster.AddTab(tab2, null);
				}
			}
			if (this.audioSourceControlUIConnector != null)
			{
				this.audioSourceControlUIConnector.ClearConnectors();
				AudioSourceControl[] componentsInChildren3 = base.GetComponentsInChildren<AudioSourceControl>(false);
				foreach (AudioSourceControl audioSourceControl in componentsInChildren3)
				{
					this.audioSourceControlUIConnector.AddConnector(audioSourceControl);
					TabbedUIBuilder.Tab tab3 = new TabbedUIBuilder.Tab();
					tab3.name = audioSourceControl.storeId;
					tab3.prefab = this.audioSourceControlUIPrefab;
					tab3.color = this.audioSourceControlUITabColor;
					this.materialUIConnectorMaster.AddTab(tab3, null);
				}
			}
		}
	}

	// Token: 0x060060FD RID: 24829 RVA: 0x0024AADE File Offset: 0x00248EDE
	protected void GeometryChanged(string newChoice)
	{
		this.SyncUITabs();
		this.materialUIConnectorMaster.Rebuild();
	}

	// Token: 0x060060FE RID: 24830 RVA: 0x0024AAF4 File Offset: 0x00248EF4
	protected virtual void Init()
	{
		if (this.choosers != null)
		{
			foreach (ObjectChooser objectChooser in this.choosers)
			{
				if (objectChooser != null)
				{
					objectChooser.ForceAwake();
					ObjectChooser objectChooser2 = objectChooser;
					objectChooser2.onChoiceChangedHandlers = (ObjectChooser.ChoiceChanged)Delegate.Combine(objectChooser2.onChoiceChangedHandlers, new ObjectChooser.ChoiceChanged(this.GeometryChanged));
				}
			}
		}
		if (this.materialUIConnectorMaster != null)
		{
			UIMultiConnector[] components = this.materialUIConnectorMaster.GetComponents<UIMultiConnector>();
			foreach (UIMultiConnector uimultiConnector in components)
			{
				if (uimultiConnector.typeToConnect.Type == typeof(MaterialOptions))
				{
					this.materialUIConnector = uimultiConnector;
				}
				else if (uimultiConnector.typeToConnect.Type == typeof(ImageControl))
				{
					this.imageControlUIConnector = uimultiConnector;
				}
				else if (uimultiConnector.typeToConnect.Type == typeof(AudioSourceControl))
				{
					this.audioSourceControlUIConnector = uimultiConnector;
				}
			}
			this.SyncUITabs();
		}
	}

	// Token: 0x060060FF RID: 24831 RVA: 0x0024AC18 File Offset: 0x00249018
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null && !this.isSyncingUITabs)
		{
			DynamicGeometryUI componentInChildren = t.GetComponentInChildren<DynamicGeometryUI>(true);
			if (componentInChildren != null)
			{
				RectTransform chooserPopupContainer = componentInChildren.chooserPopupContainer;
				if (chooserPopupContainer != null)
				{
					foreach (ObjectChooser objectChooser in this.choosers)
					{
						if (objectChooser != null)
						{
							RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.chooserPopupPrefab);
							rectTransform.SetParent(chooserPopupContainer, false);
							UIPopup componentInChildren2 = rectTransform.GetComponentInChildren<UIPopup>();
							objectChooser.chooserJSON.popup = componentInChildren2;
						}
					}
				}
			}
		}
	}

	// Token: 0x06006100 RID: 24832 RVA: 0x0024ACBF File Offset: 0x002490BF
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

	// Token: 0x0400508E RID: 20622
	public ObjectChooser[] choosers;

	// Token: 0x0400508F RID: 20623
	public RectTransform chooserPopupPrefab;

	// Token: 0x04005090 RID: 20624
	public RectTransform materialUIPrefab;

	// Token: 0x04005091 RID: 20625
	public RectTransform imageControlUIPrefab;

	// Token: 0x04005092 RID: 20626
	public RectTransform audioSourceControlUIPrefab;

	// Token: 0x04005093 RID: 20627
	public Color materialUITabColor;

	// Token: 0x04005094 RID: 20628
	public Color imageControlUITabColor;

	// Token: 0x04005095 RID: 20629
	public Color audioSourceControlUITabColor;

	// Token: 0x04005096 RID: 20630
	public UIConnectorMaster materialUIConnectorMaster;

	// Token: 0x04005097 RID: 20631
	protected UIMultiConnector materialUIConnector;

	// Token: 0x04005098 RID: 20632
	protected UIMultiConnector imageControlUIConnector;

	// Token: 0x04005099 RID: 20633
	protected UIMultiConnector audioSourceControlUIConnector;

	// Token: 0x0400509A RID: 20634
	protected bool isSyncingUITabs;
}
