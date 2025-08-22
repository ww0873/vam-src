using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ABA RID: 2746
public class DAZCharacterTextureControlUI : UIProvider
{
	// Token: 0x060048C8 RID: 18632 RVA: 0x001703C5 File Offset: 0x0016E7C5
	public DAZCharacterTextureControlUI()
	{
	}

	// Token: 0x060048C9 RID: 18633 RVA: 0x001703D0 File Offset: 0x0016E7D0
	private void OpenHoverTextureUrlPanel(string text, Color color)
	{
		if (this.onHoverTextureUrlPanel != null)
		{
			this.onHoverTextureUrlPanel.gameObject.SetActive(true);
		}
		if (this.onHoverTextureUrlText != null)
		{
			this.onHoverTextureUrlText.text = text;
			this.onHoverTextureUrlText.color = color;
		}
	}

	// Token: 0x060048CA RID: 18634 RVA: 0x00170428 File Offset: 0x0016E828
	private void CloseHoverTextureUrlPanel()
	{
		if (this.onHoverTextureUrlPanel != null)
		{
			this.onHoverTextureUrlPanel.gameObject.SetActive(false);
		}
	}

	// Token: 0x060048CB RID: 18635 RVA: 0x0017044C File Offset: 0x0016E84C
	private void Awake()
	{
		if (this.onHoverTextureUrlPanel != null && this.onHoverTextureUrlText != null)
		{
			if (this.faceDiffuseUrlText != null)
			{
				UIHoverTextNotifier component = this.faceDiffuseUrlText.GetComponent<UIHoverTextNotifier>();
				if (component != null)
				{
					UIHoverTextNotifier uihoverTextNotifier = component;
					uihoverTextNotifier.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__0));
					UIHoverTextNotifier uihoverTextNotifier2 = component;
					uihoverTextNotifier2.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier2.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__1));
				}
			}
			if (this.torsoDiffuseUrlText != null)
			{
				UIHoverTextNotifier component2 = this.torsoDiffuseUrlText.GetComponent<UIHoverTextNotifier>();
				if (component2 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier3 = component2;
					uihoverTextNotifier3.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier3.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__2));
					UIHoverTextNotifier uihoverTextNotifier4 = component2;
					uihoverTextNotifier4.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier4.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__3));
				}
			}
			if (this.limbsDiffuseUrlText != null)
			{
				UIHoverTextNotifier component3 = this.limbsDiffuseUrlText.GetComponent<UIHoverTextNotifier>();
				if (component3 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier5 = component3;
					uihoverTextNotifier5.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier5.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__4));
					UIHoverTextNotifier uihoverTextNotifier6 = component3;
					uihoverTextNotifier6.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier6.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__5));
				}
			}
			if (this.genitalsDiffuseUrlText != null)
			{
				UIHoverTextNotifier component4 = this.genitalsDiffuseUrlText.GetComponent<UIHoverTextNotifier>();
				if (component4 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier7 = component4;
					uihoverTextNotifier7.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier7.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__6));
					UIHoverTextNotifier uihoverTextNotifier8 = component4;
					uihoverTextNotifier8.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier8.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__7));
				}
			}
			if (this.faceSpecularUrlText != null)
			{
				UIHoverTextNotifier component5 = this.faceSpecularUrlText.GetComponent<UIHoverTextNotifier>();
				if (component5 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier9 = component5;
					uihoverTextNotifier9.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier9.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__8));
					UIHoverTextNotifier uihoverTextNotifier10 = component5;
					uihoverTextNotifier10.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier10.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__9));
				}
			}
			if (this.torsoSpecularUrlText != null)
			{
				UIHoverTextNotifier component6 = this.torsoSpecularUrlText.GetComponent<UIHoverTextNotifier>();
				if (component6 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier11 = component6;
					uihoverTextNotifier11.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier11.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__A));
					UIHoverTextNotifier uihoverTextNotifier12 = component6;
					uihoverTextNotifier12.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier12.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__B));
				}
			}
			if (this.limbsSpecularUrlText != null)
			{
				UIHoverTextNotifier component7 = this.limbsSpecularUrlText.GetComponent<UIHoverTextNotifier>();
				if (component7 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier13 = component7;
					uihoverTextNotifier13.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier13.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__C));
					UIHoverTextNotifier uihoverTextNotifier14 = component7;
					uihoverTextNotifier14.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier14.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__D));
				}
			}
			if (this.genitalsSpecularUrlText != null)
			{
				UIHoverTextNotifier component8 = this.genitalsSpecularUrlText.GetComponent<UIHoverTextNotifier>();
				if (component8 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier15 = component8;
					uihoverTextNotifier15.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier15.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__E));
					UIHoverTextNotifier uihoverTextNotifier16 = component8;
					uihoverTextNotifier16.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier16.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__F));
				}
			}
			if (this.faceGlossUrlText != null)
			{
				UIHoverTextNotifier component9 = this.faceGlossUrlText.GetComponent<UIHoverTextNotifier>();
				if (component9 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier17 = component9;
					uihoverTextNotifier17.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier17.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__10));
					UIHoverTextNotifier uihoverTextNotifier18 = component9;
					uihoverTextNotifier18.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier18.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__11));
				}
			}
			if (this.torsoGlossUrlText != null)
			{
				UIHoverTextNotifier component10 = this.torsoGlossUrlText.GetComponent<UIHoverTextNotifier>();
				if (component10 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier19 = component10;
					uihoverTextNotifier19.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier19.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__12));
					UIHoverTextNotifier uihoverTextNotifier20 = component10;
					uihoverTextNotifier20.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier20.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__13));
				}
			}
			if (this.limbsGlossUrlText != null)
			{
				UIHoverTextNotifier component11 = this.limbsGlossUrlText.GetComponent<UIHoverTextNotifier>();
				if (component11 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier21 = component11;
					uihoverTextNotifier21.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier21.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__14));
					UIHoverTextNotifier uihoverTextNotifier22 = component11;
					uihoverTextNotifier22.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier22.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__15));
				}
			}
			if (this.genitalsGlossUrlText != null)
			{
				UIHoverTextNotifier component12 = this.genitalsGlossUrlText.GetComponent<UIHoverTextNotifier>();
				if (component12 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier23 = component12;
					uihoverTextNotifier23.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier23.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__16));
					UIHoverTextNotifier uihoverTextNotifier24 = component12;
					uihoverTextNotifier24.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier24.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__17));
				}
			}
			if (this.faceNormalUrlText != null)
			{
				UIHoverTextNotifier component13 = this.faceNormalUrlText.GetComponent<UIHoverTextNotifier>();
				if (component13 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier25 = component13;
					uihoverTextNotifier25.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier25.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__18));
					UIHoverTextNotifier uihoverTextNotifier26 = component13;
					uihoverTextNotifier26.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier26.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__19));
				}
			}
			if (this.torsoNormalUrlText != null)
			{
				UIHoverTextNotifier component14 = this.torsoNormalUrlText.GetComponent<UIHoverTextNotifier>();
				if (component14 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier27 = component14;
					uihoverTextNotifier27.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier27.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__1A));
					UIHoverTextNotifier uihoverTextNotifier28 = component14;
					uihoverTextNotifier28.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier28.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__1B));
				}
			}
			if (this.limbsNormalUrlText != null)
			{
				UIHoverTextNotifier component15 = this.limbsNormalUrlText.GetComponent<UIHoverTextNotifier>();
				if (component15 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier29 = component15;
					uihoverTextNotifier29.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier29.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__1C));
					UIHoverTextNotifier uihoverTextNotifier30 = component15;
					uihoverTextNotifier30.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier30.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__1D));
				}
			}
			if (this.genitalsNormalUrlText != null)
			{
				UIHoverTextNotifier component16 = this.genitalsNormalUrlText.GetComponent<UIHoverTextNotifier>();
				if (component16 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier31 = component16;
					uihoverTextNotifier31.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier31.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__1E));
					UIHoverTextNotifier uihoverTextNotifier32 = component16;
					uihoverTextNotifier32.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier32.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__1F));
				}
			}
			if (this.faceDetailUrlText != null)
			{
				UIHoverTextNotifier component17 = this.faceDetailUrlText.GetComponent<UIHoverTextNotifier>();
				if (component17 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier33 = component17;
					uihoverTextNotifier33.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier33.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__20));
					UIHoverTextNotifier uihoverTextNotifier34 = component17;
					uihoverTextNotifier34.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier34.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__21));
				}
			}
			if (this.torsoDetailUrlText != null)
			{
				UIHoverTextNotifier component18 = this.torsoDetailUrlText.GetComponent<UIHoverTextNotifier>();
				if (component18 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier35 = component18;
					uihoverTextNotifier35.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier35.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__22));
					UIHoverTextNotifier uihoverTextNotifier36 = component18;
					uihoverTextNotifier36.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier36.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__23));
				}
			}
			if (this.limbsDetailUrlText != null)
			{
				UIHoverTextNotifier component19 = this.limbsDetailUrlText.GetComponent<UIHoverTextNotifier>();
				if (component19 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier37 = component19;
					uihoverTextNotifier37.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier37.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__24));
					UIHoverTextNotifier uihoverTextNotifier38 = component19;
					uihoverTextNotifier38.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier38.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__25));
				}
			}
			if (this.genitalsDetailUrlText != null)
			{
				UIHoverTextNotifier component20 = this.genitalsDetailUrlText.GetComponent<UIHoverTextNotifier>();
				if (component20 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier39 = component20;
					uihoverTextNotifier39.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier39.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__26));
					UIHoverTextNotifier uihoverTextNotifier40 = component20;
					uihoverTextNotifier40.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier40.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__27));
				}
			}
			if (this.faceDecalUrlText != null)
			{
				UIHoverTextNotifier component21 = this.faceDecalUrlText.GetComponent<UIHoverTextNotifier>();
				if (component21 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier41 = component21;
					uihoverTextNotifier41.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier41.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__28));
					UIHoverTextNotifier uihoverTextNotifier42 = component21;
					uihoverTextNotifier42.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier42.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__29));
				}
			}
			if (this.torsoDecalUrlText != null)
			{
				UIHoverTextNotifier component22 = this.torsoDecalUrlText.GetComponent<UIHoverTextNotifier>();
				if (component22 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier43 = component22;
					uihoverTextNotifier43.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier43.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__2A));
					UIHoverTextNotifier uihoverTextNotifier44 = component22;
					uihoverTextNotifier44.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier44.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__2B));
				}
			}
			if (this.limbsDecalUrlText != null)
			{
				UIHoverTextNotifier component23 = this.limbsDecalUrlText.GetComponent<UIHoverTextNotifier>();
				if (component23 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier45 = component23;
					uihoverTextNotifier45.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier45.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__2C));
					UIHoverTextNotifier uihoverTextNotifier46 = component23;
					uihoverTextNotifier46.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier46.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__2D));
				}
			}
			if (this.genitalsDecalUrlText != null)
			{
				UIHoverTextNotifier component24 = this.genitalsDecalUrlText.GetComponent<UIHoverTextNotifier>();
				if (component24 != null)
				{
					UIHoverTextNotifier uihoverTextNotifier47 = component24;
					uihoverTextNotifier47.onEnterNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier47.onEnterNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__2E));
					UIHoverTextNotifier uihoverTextNotifier48 = component24;
					uihoverTextNotifier48.onExitNotifier = (UIHoverTextNotifier.TextNotifier)Delegate.Combine(uihoverTextNotifier48.onExitNotifier, new UIHoverTextNotifier.TextNotifier(this.<Awake>m__2F));
				}
			}
		}
	}

	// Token: 0x060048CC RID: 18636 RVA: 0x00170F03 File Offset: 0x0016F303
	[CompilerGenerated]
	private void <Awake>m__0(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048CD RID: 18637 RVA: 0x00170F17 File Offset: 0x0016F317
	[CompilerGenerated]
	private void <Awake>m__1(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048CE RID: 18638 RVA: 0x00170F1F File Offset: 0x0016F31F
	[CompilerGenerated]
	private void <Awake>m__2(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048CF RID: 18639 RVA: 0x00170F33 File Offset: 0x0016F333
	[CompilerGenerated]
	private void <Awake>m__3(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048D0 RID: 18640 RVA: 0x00170F3B File Offset: 0x0016F33B
	[CompilerGenerated]
	private void <Awake>m__4(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048D1 RID: 18641 RVA: 0x00170F4F File Offset: 0x0016F34F
	[CompilerGenerated]
	private void <Awake>m__5(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048D2 RID: 18642 RVA: 0x00170F57 File Offset: 0x0016F357
	[CompilerGenerated]
	private void <Awake>m__6(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048D3 RID: 18643 RVA: 0x00170F6B File Offset: 0x0016F36B
	[CompilerGenerated]
	private void <Awake>m__7(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048D4 RID: 18644 RVA: 0x00170F73 File Offset: 0x0016F373
	[CompilerGenerated]
	private void <Awake>m__8(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048D5 RID: 18645 RVA: 0x00170F87 File Offset: 0x0016F387
	[CompilerGenerated]
	private void <Awake>m__9(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048D6 RID: 18646 RVA: 0x00170F8F File Offset: 0x0016F38F
	[CompilerGenerated]
	private void <Awake>m__A(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048D7 RID: 18647 RVA: 0x00170FA3 File Offset: 0x0016F3A3
	[CompilerGenerated]
	private void <Awake>m__B(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048D8 RID: 18648 RVA: 0x00170FAB File Offset: 0x0016F3AB
	[CompilerGenerated]
	private void <Awake>m__C(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048D9 RID: 18649 RVA: 0x00170FBF File Offset: 0x0016F3BF
	[CompilerGenerated]
	private void <Awake>m__D(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048DA RID: 18650 RVA: 0x00170FC7 File Offset: 0x0016F3C7
	[CompilerGenerated]
	private void <Awake>m__E(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048DB RID: 18651 RVA: 0x00170FDB File Offset: 0x0016F3DB
	[CompilerGenerated]
	private void <Awake>m__F(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048DC RID: 18652 RVA: 0x00170FE3 File Offset: 0x0016F3E3
	[CompilerGenerated]
	private void <Awake>m__10(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048DD RID: 18653 RVA: 0x00170FF7 File Offset: 0x0016F3F7
	[CompilerGenerated]
	private void <Awake>m__11(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048DE RID: 18654 RVA: 0x00170FFF File Offset: 0x0016F3FF
	[CompilerGenerated]
	private void <Awake>m__12(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048DF RID: 18655 RVA: 0x00171013 File Offset: 0x0016F413
	[CompilerGenerated]
	private void <Awake>m__13(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048E0 RID: 18656 RVA: 0x0017101B File Offset: 0x0016F41B
	[CompilerGenerated]
	private void <Awake>m__14(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048E1 RID: 18657 RVA: 0x0017102F File Offset: 0x0016F42F
	[CompilerGenerated]
	private void <Awake>m__15(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048E2 RID: 18658 RVA: 0x00171037 File Offset: 0x0016F437
	[CompilerGenerated]
	private void <Awake>m__16(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048E3 RID: 18659 RVA: 0x0017104B File Offset: 0x0016F44B
	[CompilerGenerated]
	private void <Awake>m__17(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048E4 RID: 18660 RVA: 0x00171053 File Offset: 0x0016F453
	[CompilerGenerated]
	private void <Awake>m__18(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048E5 RID: 18661 RVA: 0x00171067 File Offset: 0x0016F467
	[CompilerGenerated]
	private void <Awake>m__19(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048E6 RID: 18662 RVA: 0x0017106F File Offset: 0x0016F46F
	[CompilerGenerated]
	private void <Awake>m__1A(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048E7 RID: 18663 RVA: 0x00171083 File Offset: 0x0016F483
	[CompilerGenerated]
	private void <Awake>m__1B(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048E8 RID: 18664 RVA: 0x0017108B File Offset: 0x0016F48B
	[CompilerGenerated]
	private void <Awake>m__1C(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048E9 RID: 18665 RVA: 0x0017109F File Offset: 0x0016F49F
	[CompilerGenerated]
	private void <Awake>m__1D(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048EA RID: 18666 RVA: 0x001710A7 File Offset: 0x0016F4A7
	[CompilerGenerated]
	private void <Awake>m__1E(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048EB RID: 18667 RVA: 0x001710BB File Offset: 0x0016F4BB
	[CompilerGenerated]
	private void <Awake>m__1F(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048EC RID: 18668 RVA: 0x001710C3 File Offset: 0x0016F4C3
	[CompilerGenerated]
	private void <Awake>m__20(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048ED RID: 18669 RVA: 0x001710D7 File Offset: 0x0016F4D7
	[CompilerGenerated]
	private void <Awake>m__21(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048EE RID: 18670 RVA: 0x001710DF File Offset: 0x0016F4DF
	[CompilerGenerated]
	private void <Awake>m__22(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048EF RID: 18671 RVA: 0x001710F3 File Offset: 0x0016F4F3
	[CompilerGenerated]
	private void <Awake>m__23(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048F0 RID: 18672 RVA: 0x001710FB File Offset: 0x0016F4FB
	[CompilerGenerated]
	private void <Awake>m__24(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048F1 RID: 18673 RVA: 0x0017110F File Offset: 0x0016F50F
	[CompilerGenerated]
	private void <Awake>m__25(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048F2 RID: 18674 RVA: 0x00171117 File Offset: 0x0016F517
	[CompilerGenerated]
	private void <Awake>m__26(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048F3 RID: 18675 RVA: 0x0017112B File Offset: 0x0016F52B
	[CompilerGenerated]
	private void <Awake>m__27(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048F4 RID: 18676 RVA: 0x00171133 File Offset: 0x0016F533
	[CompilerGenerated]
	private void <Awake>m__28(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048F5 RID: 18677 RVA: 0x00171147 File Offset: 0x0016F547
	[CompilerGenerated]
	private void <Awake>m__29(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048F6 RID: 18678 RVA: 0x0017114F File Offset: 0x0016F54F
	[CompilerGenerated]
	private void <Awake>m__2A(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048F7 RID: 18679 RVA: 0x00171163 File Offset: 0x0016F563
	[CompilerGenerated]
	private void <Awake>m__2B(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048F8 RID: 18680 RVA: 0x0017116B File Offset: 0x0016F56B
	[CompilerGenerated]
	private void <Awake>m__2C(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048F9 RID: 18681 RVA: 0x0017117F File Offset: 0x0016F57F
	[CompilerGenerated]
	private void <Awake>m__2D(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x060048FA RID: 18682 RVA: 0x00171187 File Offset: 0x0016F587
	[CompilerGenerated]
	private void <Awake>m__2E(Text text)
	{
		this.OpenHoverTextureUrlPanel(text.text, text.color);
	}

	// Token: 0x060048FB RID: 18683 RVA: 0x0017119B File Offset: 0x0016F59B
	[CompilerGenerated]
	private void <Awake>m__2F(Text text)
	{
		this.CloseHoverTextureUrlPanel();
	}

	// Token: 0x040036F7 RID: 14071
	public Text uvLabel;

	// Token: 0x040036F8 RID: 14072
	public Button faceDiffuseFileBrowseButton;

	// Token: 0x040036F9 RID: 14073
	public Button faceDiffuseReloadButton;

	// Token: 0x040036FA RID: 14074
	public Button faceDiffuseClearButton;

	// Token: 0x040036FB RID: 14075
	public Text faceDiffuseUrlText;

	// Token: 0x040036FC RID: 14076
	public Button torsoDiffuseFileBrowseButton;

	// Token: 0x040036FD RID: 14077
	public Button torsoDiffuseReloadButton;

	// Token: 0x040036FE RID: 14078
	public Button torsoDiffuseClearButton;

	// Token: 0x040036FF RID: 14079
	public Text torsoDiffuseUrlText;

	// Token: 0x04003700 RID: 14080
	public Button limbsDiffuseFileBrowseButton;

	// Token: 0x04003701 RID: 14081
	public Button limbsDiffuseReloadButton;

	// Token: 0x04003702 RID: 14082
	public Button limbsDiffuseClearButton;

	// Token: 0x04003703 RID: 14083
	public Text limbsDiffuseUrlText;

	// Token: 0x04003704 RID: 14084
	public Button genitalsDiffuseFileBrowseButton;

	// Token: 0x04003705 RID: 14085
	public Button genitalsDiffuseReloadButton;

	// Token: 0x04003706 RID: 14086
	public Button genitalsDiffuseClearButton;

	// Token: 0x04003707 RID: 14087
	public Text genitalsDiffuseUrlText;

	// Token: 0x04003708 RID: 14088
	public Button faceSpecularFileBrowseButton;

	// Token: 0x04003709 RID: 14089
	public Button faceSpecularReloadButton;

	// Token: 0x0400370A RID: 14090
	public Button faceSpecularClearButton;

	// Token: 0x0400370B RID: 14091
	public Text faceSpecularUrlText;

	// Token: 0x0400370C RID: 14092
	public Button torsoSpecularFileBrowseButton;

	// Token: 0x0400370D RID: 14093
	public Button torsoSpecularReloadButton;

	// Token: 0x0400370E RID: 14094
	public Button torsoSpecularClearButton;

	// Token: 0x0400370F RID: 14095
	public Text torsoSpecularUrlText;

	// Token: 0x04003710 RID: 14096
	public Button limbsSpecularFileBrowseButton;

	// Token: 0x04003711 RID: 14097
	public Button limbsSpecularReloadButton;

	// Token: 0x04003712 RID: 14098
	public Button limbsSpecularClearButton;

	// Token: 0x04003713 RID: 14099
	public Text limbsSpecularUrlText;

	// Token: 0x04003714 RID: 14100
	public Button genitalsSpecularFileBrowseButton;

	// Token: 0x04003715 RID: 14101
	public Button genitalsSpecularReloadButton;

	// Token: 0x04003716 RID: 14102
	public Button genitalsSpecularClearButton;

	// Token: 0x04003717 RID: 14103
	public Text genitalsSpecularUrlText;

	// Token: 0x04003718 RID: 14104
	public Button faceGlossFileBrowseButton;

	// Token: 0x04003719 RID: 14105
	public Button faceGlossReloadButton;

	// Token: 0x0400371A RID: 14106
	public Button faceGlossClearButton;

	// Token: 0x0400371B RID: 14107
	public Text faceGlossUrlText;

	// Token: 0x0400371C RID: 14108
	public Button torsoGlossFileBrowseButton;

	// Token: 0x0400371D RID: 14109
	public Button torsoGlossReloadButton;

	// Token: 0x0400371E RID: 14110
	public Button torsoGlossClearButton;

	// Token: 0x0400371F RID: 14111
	public Text torsoGlossUrlText;

	// Token: 0x04003720 RID: 14112
	public Button limbsGlossFileBrowseButton;

	// Token: 0x04003721 RID: 14113
	public Button limbsGlossReloadButton;

	// Token: 0x04003722 RID: 14114
	public Button limbsGlossClearButton;

	// Token: 0x04003723 RID: 14115
	public Text limbsGlossUrlText;

	// Token: 0x04003724 RID: 14116
	public Button genitalsGlossFileBrowseButton;

	// Token: 0x04003725 RID: 14117
	public Button genitalsGlossReloadButton;

	// Token: 0x04003726 RID: 14118
	public Button genitalsGlossClearButton;

	// Token: 0x04003727 RID: 14119
	public Text genitalsGlossUrlText;

	// Token: 0x04003728 RID: 14120
	public Button faceNormalFileBrowseButton;

	// Token: 0x04003729 RID: 14121
	public Button faceNormalReloadButton;

	// Token: 0x0400372A RID: 14122
	public Button faceNormalClearButton;

	// Token: 0x0400372B RID: 14123
	public Text faceNormalUrlText;

	// Token: 0x0400372C RID: 14124
	public Button torsoNormalFileBrowseButton;

	// Token: 0x0400372D RID: 14125
	public Button torsoNormalReloadButton;

	// Token: 0x0400372E RID: 14126
	public Button torsoNormalClearButton;

	// Token: 0x0400372F RID: 14127
	public Text torsoNormalUrlText;

	// Token: 0x04003730 RID: 14128
	public Button limbsNormalFileBrowseButton;

	// Token: 0x04003731 RID: 14129
	public Button limbsNormalReloadButton;

	// Token: 0x04003732 RID: 14130
	public Button limbsNormalClearButton;

	// Token: 0x04003733 RID: 14131
	public Text limbsNormalUrlText;

	// Token: 0x04003734 RID: 14132
	public Button genitalsNormalFileBrowseButton;

	// Token: 0x04003735 RID: 14133
	public Button genitalsNormalReloadButton;

	// Token: 0x04003736 RID: 14134
	public Button genitalsNormalClearButton;

	// Token: 0x04003737 RID: 14135
	public Text genitalsNormalUrlText;

	// Token: 0x04003738 RID: 14136
	public Button faceDetailFileBrowseButton;

	// Token: 0x04003739 RID: 14137
	public Button faceDetailReloadButton;

	// Token: 0x0400373A RID: 14138
	public Button faceDetailClearButton;

	// Token: 0x0400373B RID: 14139
	public Text faceDetailUrlText;

	// Token: 0x0400373C RID: 14140
	public Button torsoDetailFileBrowseButton;

	// Token: 0x0400373D RID: 14141
	public Button torsoDetailReloadButton;

	// Token: 0x0400373E RID: 14142
	public Button torsoDetailClearButton;

	// Token: 0x0400373F RID: 14143
	public Text torsoDetailUrlText;

	// Token: 0x04003740 RID: 14144
	public Button limbsDetailFileBrowseButton;

	// Token: 0x04003741 RID: 14145
	public Button limbsDetailReloadButton;

	// Token: 0x04003742 RID: 14146
	public Button limbsDetailClearButton;

	// Token: 0x04003743 RID: 14147
	public Text limbsDetailUrlText;

	// Token: 0x04003744 RID: 14148
	public Button genitalsDetailFileBrowseButton;

	// Token: 0x04003745 RID: 14149
	public Button genitalsDetailReloadButton;

	// Token: 0x04003746 RID: 14150
	public Button genitalsDetailClearButton;

	// Token: 0x04003747 RID: 14151
	public Text genitalsDetailUrlText;

	// Token: 0x04003748 RID: 14152
	public Button faceDecalFileBrowseButton;

	// Token: 0x04003749 RID: 14153
	public Button faceDecalReloadButton;

	// Token: 0x0400374A RID: 14154
	public Button faceDecalClearButton;

	// Token: 0x0400374B RID: 14155
	public Text faceDecalUrlText;

	// Token: 0x0400374C RID: 14156
	public Button torsoDecalFileBrowseButton;

	// Token: 0x0400374D RID: 14157
	public Button torsoDecalReloadButton;

	// Token: 0x0400374E RID: 14158
	public Button torsoDecalClearButton;

	// Token: 0x0400374F RID: 14159
	public Text torsoDecalUrlText;

	// Token: 0x04003750 RID: 14160
	public Button limbsDecalFileBrowseButton;

	// Token: 0x04003751 RID: 14161
	public Button limbsDecalReloadButton;

	// Token: 0x04003752 RID: 14162
	public Button limbsDecalClearButton;

	// Token: 0x04003753 RID: 14163
	public Text limbsDecalUrlText;

	// Token: 0x04003754 RID: 14164
	public Button genitalsDecalFileBrowseButton;

	// Token: 0x04003755 RID: 14165
	public Button genitalsDecalReloadButton;

	// Token: 0x04003756 RID: 14166
	public Button genitalsDecalClearButton;

	// Token: 0x04003757 RID: 14167
	public Text genitalsDecalUrlText;

	// Token: 0x04003758 RID: 14168
	public Button directoryBrowseButton;

	// Token: 0x04003759 RID: 14169
	public Toggle autoBlendGenitalTexturesToggle;

	// Token: 0x0400375A RID: 14170
	public Toggle autoBlendGenitalSpecGlossNormalTexturesToggle;

	// Token: 0x0400375B RID: 14171
	public Button dumpAutoGeneratedGenitalTexturesButton;

	// Token: 0x0400375C RID: 14172
	public Button autoBlendGenitalDiffuseTextureButton;

	// Token: 0x0400375D RID: 14173
	public Transform autoBlendGenitalColorAdjustContainer;

	// Token: 0x0400375E RID: 14174
	public Slider autoBlendGenitalLightenDarkenSlider;

	// Token: 0x0400375F RID: 14175
	public Slider autoBlendGenitalHueOffsetSlider;

	// Token: 0x04003760 RID: 14176
	public Slider autoBlendGenitalSaturationOffsetSlider;

	// Token: 0x04003761 RID: 14177
	public RectTransform onHoverTextureUrlPanel;

	// Token: 0x04003762 RID: 14178
	public Text onHoverTextureUrlText;
}
