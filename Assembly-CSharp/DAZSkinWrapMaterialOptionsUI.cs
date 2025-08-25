using System;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

// Token: 0x02000B18 RID: 2840
public class DAZSkinWrapMaterialOptionsUI : MaterialOptionsUI
{
	// Token: 0x06004D92 RID: 19858 RVA: 0x001B3919 File Offset: 0x001B1D19
	public DAZSkinWrapMaterialOptionsUI()
	{
	}

	// Token: 0x06004D93 RID: 19859 RVA: 0x001B3924 File Offset: 0x001B1D24
	protected override void Awake()
	{
		base.Awake();
		if (this.onHoverTextureUrlPanel != null && this.onHoverTextureUrlText != null && this.customSimTextureUrlText != null)
		{
			UIHoverTextNotifier component = this.customSimTextureUrlText.GetComponent<UIHoverTextNotifier>();
			if (component != null)
			{
				UIHoverTextNotifier uihoverTextNotifier = component;
				uihoverTextNotifier.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__0));
				UIHoverTextNotifier uihoverTextNotifier2 = component;
				uihoverTextNotifier2.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier2.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__1));
			}
		}
	}

	// Token: 0x06004D94 RID: 19860 RVA: 0x001B39C6 File Offset: 0x001B1DC6
	[CompilerGenerated]
	private void <Awake>m__0(Text text)
	{
		base.OpenHoverTextureUrlPanel(text.text);
	}

	// Token: 0x06004D95 RID: 19861 RVA: 0x001B39D4 File Offset: 0x001B1DD4
	[CompilerGenerated]
	private void <Awake>m__1(Text text)
	{
		base.CloseHoverTextureUrlPanel();
	}

	// Token: 0x04003D4F RID: 15695
	public Button customSimTextureFileBrowseButton;

	// Token: 0x04003D50 RID: 15696
	public Button customSimTextureReloadButton;

	// Token: 0x04003D51 RID: 15697
	public Button customSimTextureClearButton;

	// Token: 0x04003D52 RID: 15698
	public Button customSimTextureNullButton;

	// Token: 0x04003D53 RID: 15699
	public Button customSimTextureDefaultButton;

	// Token: 0x04003D54 RID: 15700
	public Text customSimTextureUrlText;
}
