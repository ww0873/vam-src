using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D22 RID: 3362
public class MaterialOptionsUI : UIProvider
{
	// Token: 0x06006720 RID: 26400 RVA: 0x001B3557 File Offset: 0x001B1957
	public MaterialOptionsUI()
	{
	}

	// Token: 0x06006721 RID: 26401 RVA: 0x001B3560 File Offset: 0x001B1960
	protected void OpenHoverTextureUrlPanel(string text)
	{
		if (this.onHoverTextureUrlPanel != null)
		{
			this.onHoverTextureUrlPanel.gameObject.SetActive(true);
		}
		if (this.onHoverTextureUrlText != null)
		{
			this.onHoverTextureUrlText.text = text;
		}
	}

	// Token: 0x06006722 RID: 26402 RVA: 0x001B35AC File Offset: 0x001B19AC
	protected void CloseHoverTextureUrlPanel()
	{
		if (this.onHoverTextureUrlPanel != null)
		{
			this.onHoverTextureUrlPanel.gameObject.SetActive(false);
		}
	}

	// Token: 0x06006723 RID: 26403 RVA: 0x001B35D0 File Offset: 0x001B19D0
	protected virtual void Awake()
	{
		if (this.onHoverTextureUrlPanel != null && this.onHoverTextureUrlText != null)
		{
			if (this.customTexture1UrlText != null)
			{
				UIHoverTextNotifier component = this.customTexture1UrlText.GetComponent<UIHoverTextNotifier>();
				if (component != null)
				{
					UIHoverTextNotifier uihoverTextNotifier = component;
					uihoverTextNotifier.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__0));
					UIHoverTextNotifier uihoverTextNotifier2 = component;
					uihoverTextNotifier2.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier2.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__1));
				}
			}
			if (this.customTexture2UrlText != null)
			{
				UIHoverTextNotifier component2 = this.customTexture2UrlText.GetComponent<UIHoverTextNotifier>();
				if (component2 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier3 = component2;
					uihoverTextNotifier3.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier3.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__2));
					UIHoverTextNotifier uihoverTextNotifier4 = component2;
					uihoverTextNotifier4.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier4.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__3));
				}
			}
			if (this.customTexture3UrlText != null)
			{
				UIHoverTextNotifier component3 = this.customTexture3UrlText.GetComponent<UIHoverTextNotifier>();
				if (component3 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier5 = component3;
					uihoverTextNotifier5.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier5.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__4));
					UIHoverTextNotifier uihoverTextNotifier6 = component3;
					uihoverTextNotifier6.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier6.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__5));
				}
			}
			if (this.customTexture4UrlText != null)
			{
				UIHoverTextNotifier component4 = this.customTexture4UrlText.GetComponent<UIHoverTextNotifier>();
				if (component4 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier7 = component4;
					uihoverTextNotifier7.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier7.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__6));
					UIHoverTextNotifier uihoverTextNotifier8 = component4;
					uihoverTextNotifier8.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier8.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__7));
				}
			}
			if (this.customTexture5UrlText != null)
			{
				UIHoverTextNotifier component5 = this.customTexture5UrlText.GetComponent<UIHoverTextNotifier>();
				if (component5 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier9 = component5;
					uihoverTextNotifier9.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier9.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__8));
					UIHoverTextNotifier uihoverTextNotifier10 = component5;
					uihoverTextNotifier10.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier10.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__9));
				}
			}
			if (this.customTexture6UrlText != null)
			{
				UIHoverTextNotifier component6 = this.customTexture6UrlText.GetComponent<UIHoverTextNotifier>();
				if (component6 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier11 = component6;
					uihoverTextNotifier11.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier11.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__A));
					UIHoverTextNotifier uihoverTextNotifier12 = component6;
					uihoverTextNotifier12.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier12.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__B));
				}
			}
		}
	}

	// Token: 0x06006724 RID: 26404 RVA: 0x001B3895 File Offset: 0x001B1C95
	[CompilerGenerated]
	private void <Awake>m__0(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text);
	}

	// Token: 0x06006725 RID: 26405 RVA: 0x001B38A3 File Offset: 0x001B1CA3
	[CompilerGenerated]
	private void <Awake>m__1(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x06006726 RID: 26406 RVA: 0x001B38AB File Offset: 0x001B1CAB
	[CompilerGenerated]
	private void <Awake>m__2(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text);
	}

	// Token: 0x06006727 RID: 26407 RVA: 0x001B38B9 File Offset: 0x001B1CB9
	[CompilerGenerated]
	private void <Awake>m__3(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x06006728 RID: 26408 RVA: 0x001B38C1 File Offset: 0x001B1CC1
	[CompilerGenerated]
	private void <Awake>m__4(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text);
	}

	// Token: 0x06006729 RID: 26409 RVA: 0x001B38CF File Offset: 0x001B1CCF
	[CompilerGenerated]
	private void <Awake>m__5(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x0600672A RID: 26410 RVA: 0x001B38D7 File Offset: 0x001B1CD7
	[CompilerGenerated]
	private void <Awake>m__6(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text);
	}

	// Token: 0x0600672B RID: 26411 RVA: 0x001B38E5 File Offset: 0x001B1CE5
	[CompilerGenerated]
	private void <Awake>m__7(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x0600672C RID: 26412 RVA: 0x001B38ED File Offset: 0x001B1CED
	[CompilerGenerated]
	private void <Awake>m__8(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text);
	}

	// Token: 0x0600672D RID: 26413 RVA: 0x001B38FB File Offset: 0x001B1CFB
	[CompilerGenerated]
	private void <Awake>m__9(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x0600672E RID: 26414 RVA: 0x001B3903 File Offset: 0x001B1D03
	[CompilerGenerated]
	private void <Awake>m__A(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text);
	}

	// Token: 0x0600672F RID: 26415 RVA: 0x001B3911 File Offset: 0x001B1D11
	[CompilerGenerated]
	private void <Awake>m__B(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x040057B0 RID: 22448
	public InputField customNameField;

	// Token: 0x040057B1 RID: 22449
	public Slider renderQueueSlider;

	// Token: 0x040057B2 RID: 22450
	public Toggle hideMaterialToggle;

	// Token: 0x040057B3 RID: 22451
	public Toggle linkToOtherMaterialsToggle;

	// Token: 0x040057B4 RID: 22452
	public RectTransform onHoverTextureUrlPanel;

	// Token: 0x040057B5 RID: 22453
	public Text onHoverTextureUrlText;

	// Token: 0x040057B6 RID: 22454
	public Text color1DisplayNameText;

	// Token: 0x040057B7 RID: 22455
	public HSVColorPicker color1Picker;

	// Token: 0x040057B8 RID: 22456
	public RectTransform color1Container;

	// Token: 0x040057B9 RID: 22457
	public Text color2DisplayNameText;

	// Token: 0x040057BA RID: 22458
	public HSVColorPicker color2Picker;

	// Token: 0x040057BB RID: 22459
	public RectTransform color2Container;

	// Token: 0x040057BC RID: 22460
	public Text color3DisplayNameText;

	// Token: 0x040057BD RID: 22461
	public HSVColorPicker color3Picker;

	// Token: 0x040057BE RID: 22462
	public RectTransform color3Container;

	// Token: 0x040057BF RID: 22463
	public Text param1DisplayNameText;

	// Token: 0x040057C0 RID: 22464
	public Slider param1Slider;

	// Token: 0x040057C1 RID: 22465
	public Text param2DisplayNameText;

	// Token: 0x040057C2 RID: 22466
	public Slider param2Slider;

	// Token: 0x040057C3 RID: 22467
	public Text param3DisplayNameText;

	// Token: 0x040057C4 RID: 22468
	public Slider param3Slider;

	// Token: 0x040057C5 RID: 22469
	public Text param4DisplayNameText;

	// Token: 0x040057C6 RID: 22470
	public Slider param4Slider;

	// Token: 0x040057C7 RID: 22471
	public Text param5DisplayNameText;

	// Token: 0x040057C8 RID: 22472
	public Slider param5Slider;

	// Token: 0x040057C9 RID: 22473
	public Text param6DisplayNameText;

	// Token: 0x040057CA RID: 22474
	public Slider param6Slider;

	// Token: 0x040057CB RID: 22475
	public Text param7DisplayNameText;

	// Token: 0x040057CC RID: 22476
	public Slider param7Slider;

	// Token: 0x040057CD RID: 22477
	public Text param8DisplayNameText;

	// Token: 0x040057CE RID: 22478
	public Slider param8Slider;

	// Token: 0x040057CF RID: 22479
	public Text param9DisplayNameText;

	// Token: 0x040057D0 RID: 22480
	public Slider param9Slider;

	// Token: 0x040057D1 RID: 22481
	public Text param10DisplayNameText;

	// Token: 0x040057D2 RID: 22482
	public Slider param10Slider;

	// Token: 0x040057D3 RID: 22483
	public UIPopup textureGroup1Popup;

	// Token: 0x040057D4 RID: 22484
	public Text textureGroup1Text;

	// Token: 0x040057D5 RID: 22485
	public UIPopup textureGroup2Popup;

	// Token: 0x040057D6 RID: 22486
	public Text textureGroup2Text;

	// Token: 0x040057D7 RID: 22487
	public UIPopup textureGroup3Popup;

	// Token: 0x040057D8 RID: 22488
	public Text textureGroup3Text;

	// Token: 0x040057D9 RID: 22489
	public UIPopup textureGroup4Popup;

	// Token: 0x040057DA RID: 22490
	public Text textureGroup4Text;

	// Token: 0x040057DB RID: 22491
	public UIPopup textureGroup5Popup;

	// Token: 0x040057DC RID: 22492
	public Text textureGroup5Text;

	// Token: 0x040057DD RID: 22493
	public Button restoreFromDefaultsButton;

	// Token: 0x040057DE RID: 22494
	public Button saveToStore1Button;

	// Token: 0x040057DF RID: 22495
	public Button restoreFromStore1Button;

	// Token: 0x040057E0 RID: 22496
	public Button saveToStore2Button;

	// Token: 0x040057E1 RID: 22497
	public Button restoreFromStore2Button;

	// Token: 0x040057E2 RID: 22498
	public Button saveToStore3Button;

	// Token: 0x040057E3 RID: 22499
	public Button restoreFromStore3Button;

	// Token: 0x040057E4 RID: 22500
	public Button createUVTemplateTextureButton;

	// Token: 0x040057E5 RID: 22501
	public Button createSimTemplateTextureButton;

	// Token: 0x040057E6 RID: 22502
	public Button openTextureFolderInExplorerButton;

	// Token: 0x040057E7 RID: 22503
	public Button customTexture1FileBrowseButton;

	// Token: 0x040057E8 RID: 22504
	public Button customTexture1ReloadButton;

	// Token: 0x040057E9 RID: 22505
	public Button customTexture1ClearButton;

	// Token: 0x040057EA RID: 22506
	public Button customTexture1NullButton;

	// Token: 0x040057EB RID: 22507
	public Button customTexture1DefaultButton;

	// Token: 0x040057EC RID: 22508
	public Text customTexture1UrlText;

	// Token: 0x040057ED RID: 22509
	public Text customTexture1Label;

	// Token: 0x040057EE RID: 22510
	public Slider customTexture1TileXSlider;

	// Token: 0x040057EF RID: 22511
	public Slider customTexture1TileYSlider;

	// Token: 0x040057F0 RID: 22512
	public Slider customTexture1OffsetXSlider;

	// Token: 0x040057F1 RID: 22513
	public Slider customTexture1OffsetYSlider;

	// Token: 0x040057F2 RID: 22514
	public Button customTexture2FileBrowseButton;

	// Token: 0x040057F3 RID: 22515
	public Button customTexture2ReloadButton;

	// Token: 0x040057F4 RID: 22516
	public Button customTexture2ClearButton;

	// Token: 0x040057F5 RID: 22517
	public Button customTexture2NullButton;

	// Token: 0x040057F6 RID: 22518
	public Button customTexture2DefaultButton;

	// Token: 0x040057F7 RID: 22519
	public Text customTexture2UrlText;

	// Token: 0x040057F8 RID: 22520
	public Text customTexture2Label;

	// Token: 0x040057F9 RID: 22521
	public Slider customTexture2TileXSlider;

	// Token: 0x040057FA RID: 22522
	public Slider customTexture2TileYSlider;

	// Token: 0x040057FB RID: 22523
	public Slider customTexture2OffsetXSlider;

	// Token: 0x040057FC RID: 22524
	public Slider customTexture2OffsetYSlider;

	// Token: 0x040057FD RID: 22525
	public Button customTexture3FileBrowseButton;

	// Token: 0x040057FE RID: 22526
	public Button customTexture3ReloadButton;

	// Token: 0x040057FF RID: 22527
	public Button customTexture3ClearButton;

	// Token: 0x04005800 RID: 22528
	public Button customTexture3NullButton;

	// Token: 0x04005801 RID: 22529
	public Button customTexture3DefaultButton;

	// Token: 0x04005802 RID: 22530
	public Text customTexture3UrlText;

	// Token: 0x04005803 RID: 22531
	public Text customTexture3Label;

	// Token: 0x04005804 RID: 22532
	public Slider customTexture3TileXSlider;

	// Token: 0x04005805 RID: 22533
	public Slider customTexture3TileYSlider;

	// Token: 0x04005806 RID: 22534
	public Slider customTexture3OffsetXSlider;

	// Token: 0x04005807 RID: 22535
	public Slider customTexture3OffsetYSlider;

	// Token: 0x04005808 RID: 22536
	public Button customTexture4FileBrowseButton;

	// Token: 0x04005809 RID: 22537
	public Button customTexture4ReloadButton;

	// Token: 0x0400580A RID: 22538
	public Button customTexture4ClearButton;

	// Token: 0x0400580B RID: 22539
	public Button customTexture4NullButton;

	// Token: 0x0400580C RID: 22540
	public Button customTexture4DefaultButton;

	// Token: 0x0400580D RID: 22541
	public Text customTexture4UrlText;

	// Token: 0x0400580E RID: 22542
	public Text customTexture4Label;

	// Token: 0x0400580F RID: 22543
	public Slider customTexture4TileXSlider;

	// Token: 0x04005810 RID: 22544
	public Slider customTexture4TileYSlider;

	// Token: 0x04005811 RID: 22545
	public Slider customTexture4OffsetXSlider;

	// Token: 0x04005812 RID: 22546
	public Slider customTexture4OffsetYSlider;

	// Token: 0x04005813 RID: 22547
	public Button customTexture5FileBrowseButton;

	// Token: 0x04005814 RID: 22548
	public Button customTexture5ReloadButton;

	// Token: 0x04005815 RID: 22549
	public Button customTexture5ClearButton;

	// Token: 0x04005816 RID: 22550
	public Button customTexture5NullButton;

	// Token: 0x04005817 RID: 22551
	public Button customTexture5DefaultButton;

	// Token: 0x04005818 RID: 22552
	public Text customTexture5UrlText;

	// Token: 0x04005819 RID: 22553
	public Text customTexture5Label;

	// Token: 0x0400581A RID: 22554
	public Slider customTexture5TileXSlider;

	// Token: 0x0400581B RID: 22555
	public Slider customTexture5TileYSlider;

	// Token: 0x0400581C RID: 22556
	public Slider customTexture5OffsetXSlider;

	// Token: 0x0400581D RID: 22557
	public Slider customTexture5OffsetYSlider;

	// Token: 0x0400581E RID: 22558
	public Button customTexture6FileBrowseButton;

	// Token: 0x0400581F RID: 22559
	public Button customTexture6ReloadButton;

	// Token: 0x04005820 RID: 22560
	public Button customTexture6ClearButton;

	// Token: 0x04005821 RID: 22561
	public Button customTexture6NullButton;

	// Token: 0x04005822 RID: 22562
	public Button customTexture6DefaultButton;

	// Token: 0x04005823 RID: 22563
	public Text customTexture6UrlText;

	// Token: 0x04005824 RID: 22564
	public Text customTexture6Label;

	// Token: 0x04005825 RID: 22565
	public Slider customTexture6TileXSlider;

	// Token: 0x04005826 RID: 22566
	public Slider customTexture6TileYSlider;

	// Token: 0x04005827 RID: 22567
	public Slider customTexture6OffsetXSlider;

	// Token: 0x04005828 RID: 22568
	public Slider customTexture6OffsetYSlider;
}
