using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x02000E30 RID: 3632
[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(Browser))]
public class VRWebBrowser : JSONStorable, IBrowserUI, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, LookInputModule.IPointerMoveHandler, INewWindowHandler, KeyEventHandler, IScrollHandler, IEventSystemHandler
{
	// Token: 0x06006FDB RID: 28635 RVA: 0x002A1624 File Offset: 0x0029FA24
	public VRWebBrowser()
	{
	}

	// Token: 0x1700107C RID: 4220
	// (get) Token: 0x06006FDC RID: 28636 RVA: 0x002A1662 File Offset: 0x0029FA62
	// (set) Token: 0x06006FDD RID: 28637 RVA: 0x002A167C File Offset: 0x0029FA7C
	public string HoveredURL
	{
		get
		{
			if (this.hoveredURLJSON != null)
			{
				return this.hoveredURLJSON.val;
			}
			return null;
		}
		set
		{
			if (this.hoveredURLJSON != null)
			{
				this.hoveredURLJSON.val = value;
			}
		}
	}

	// Token: 0x06006FDE RID: 28638 RVA: 0x002A1698 File Offset: 0x0029FA98
	protected void SyncUrl(string u)
	{
		this._url = u;
		if (string.IsNullOrEmpty(u))
		{
			return;
		}
		bool flag = false;
		if (UserPreferences.singleton == null)
		{
			flag = true;
		}
		else if (UserPreferences.singleton.enableWebBrowser)
		{
			if (u != null && u.StartsWith("file:"))
			{
				flag = true;
			}
			else if (UserPreferences.singleton.CheckWhitelistDomain(u))
			{
				flag = true;
			}
			else
			{
				if (this.browser != null)
				{
					this.browser.Url = "about:blank";
					if (this.nonWhitelistSiteObject != null)
					{
						this.nonWhitelistSiteObject.SetActive(true);
					}
					if (this.nonWhitelistSiteText != null)
					{
						this.nonWhitelistSiteText.text = u;
					}
				}
				SuperController.LogError("Attempted to load browser URL " + this._url + " which is not on whitelist", true, !UserPreferences.singleton.hideDisabledWebMessages);
			}
		}
		else if (!UserPreferences.singleton.hideDisabledWebMessages)
		{
			SuperController.LogError("Attempted to load browser URL when web browser option is disabled. To enable, see User Preferences -> Security tab");
			SuperController.singleton.ShowMainHUDAuto();
			SuperController.singleton.SetActiveUI("MainMenu");
			SuperController.singleton.SetMainMenuTab("TabUserPrefs");
			SuperController.singleton.SetUserPrefsTab("TabSecurity");
		}
		if (flag && this.browser != null)
		{
			if (this.nonWhitelistSiteObject != null)
			{
				this.nonWhitelistSiteObject.SetActive(false);
			}
			this.browser.Url = u;
		}
	}

	// Token: 0x06006FDF RID: 28639 RVA: 0x002A182C File Offset: 0x0029FC2C
	protected void SyncJSONToBrowserURL()
	{
		if (this.urlJSON != null && this.browser != null)
		{
			this.urlJSON.valNoCallback = this.browser.Url;
		}
	}

	// Token: 0x1700107D RID: 4221
	// (get) Token: 0x06006FE0 RID: 28640 RVA: 0x002A1860 File Offset: 0x0029FC60
	// (set) Token: 0x06006FE1 RID: 28641 RVA: 0x002A1868 File Offset: 0x0029FC68
	public string url
	{
		get
		{
			return this._url;
		}
		set
		{
			if (this.urlJSON != null)
			{
				this.urlJSON.val = value;
			}
			else if (this._url != value)
			{
				this.SyncUrl(this._url);
			}
		}
	}

	// Token: 0x06006FE2 RID: 28642 RVA: 0x002A18A3 File Offset: 0x0029FCA3
	protected void SyncFullMouseClickOnDown(bool b)
	{
		this._fullMouseClickOnDown = b;
	}

	// Token: 0x1700107E RID: 4222
	// (get) Token: 0x06006FE3 RID: 28643 RVA: 0x002A18AC File Offset: 0x0029FCAC
	// (set) Token: 0x06006FE4 RID: 28644 RVA: 0x002A18B4 File Offset: 0x0029FCB4
	public bool fullMouseClickOnDown
	{
		get
		{
			return this._fullMouseClickOnDown;
		}
		set
		{
			if (this.fullMouseClickOnDownJSON != null)
			{
				this.fullMouseClickOnDownJSON.val = value;
			}
			else if (this._fullMouseClickOnDown != value)
			{
				this.SyncFullMouseClickOnDown(value);
			}
		}
	}

	// Token: 0x06006FE5 RID: 28645 RVA: 0x002A18E8 File Offset: 0x0029FCE8
	public void SyncDisableInteraction()
	{
		IgnoreRaycast component = base.GetComponent<IgnoreRaycast>();
		if (this._disableInteraction)
		{
			if (component == null)
			{
				base.gameObject.AddComponent<IgnoreRaycast>();
			}
		}
		else if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x06006FE6 RID: 28646 RVA: 0x002A1936 File Offset: 0x0029FD36
	protected void SyncDisableInteraction(bool b)
	{
		this._disableInteraction = b;
		this.SyncDisableInteraction();
	}

	// Token: 0x1700107F RID: 4223
	// (get) Token: 0x06006FE7 RID: 28647 RVA: 0x002A1945 File Offset: 0x0029FD45
	// (set) Token: 0x06006FE8 RID: 28648 RVA: 0x002A194D File Offset: 0x0029FD4D
	public bool disableInteraction
	{
		get
		{
			return this._disableInteraction;
		}
		set
		{
			if (this.disableInteractionJSON != null)
			{
				this.disableInteractionJSON.val = value;
			}
			else if (this._disableInteraction != value)
			{
				this.SyncDisableInteraction(value);
			}
		}
	}

	// Token: 0x06006FE9 RID: 28649 RVA: 0x002A1980 File Offset: 0x0029FD80
	public Browser CreateBrowser(Browser parent)
	{
		GameObject gameObject = new GameObject("PopupBrowser");
		gameObject.transform.SetParent(base.transform);
		return gameObject.AddComponent<Browser>();
	}

	// Token: 0x06006FEA RID: 28650 RVA: 0x002A19B1 File Offset: 0x0029FDB1
	public void GoBack()
	{
		if (this.browser != null)
		{
			this.browser.GoBack();
			this.SyncJSONToBrowserURL();
		}
	}

	// Token: 0x06006FEB RID: 28651 RVA: 0x002A19D5 File Offset: 0x0029FDD5
	public void GoForward()
	{
		if (this.browser != null)
		{
			this.browser.GoForward();
			this.SyncJSONToBrowserURL();
		}
	}

	// Token: 0x06006FEC RID: 28652 RVA: 0x002A19F9 File Offset: 0x0029FDF9
	public void GoHome()
	{
		if (this.browser != null && this.homeUrl != null && this.homeUrl != string.Empty)
		{
			this.url = this.homeUrl;
		}
	}

	// Token: 0x06006FED RID: 28653 RVA: 0x002A1A38 File Offset: 0x0029FE38
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			VRWebBrowserUI componentInChildren = this.UITransform.GetComponentInChildren<VRWebBrowserUI>(true);
			if (componentInChildren != null)
			{
				this.fullMouseClickOnDownJSON.toggle = componentInChildren.fullMouseClickOnDownToggle;
				this.disableInteractionJSON.toggle = componentInChildren.disableInteractionToggle;
				this.urlJSON.inputField = componentInChildren.urlInput;
				this.urlJSON.inputFieldAction = componentInChildren.urlInputAction;
				this.urlJSON.copyToClipboardButton = componentInChildren.copyToClipboardButton;
				this.urlJSON.copyFromClipboardButton = componentInChildren.copyFromClipboardButton;
				this.urlJSON.setValToInputFieldButton = componentInChildren.goButton;
				this.hoveredURLJSON.text = componentInChildren.hoveredURLText;
				this.navigatedURLText = componentInChildren.navigatedURLText;
				if (this.navigatedURLText != null)
				{
					this.navigatedURLText.text = this.navigatedURL;
				}
				if (componentInChildren.backButton != null)
				{
					componentInChildren.backButton.onClick.AddListener(new UnityAction(this.GoBack));
				}
				if (componentInChildren.forwardButton != null)
				{
					componentInChildren.forwardButton.onClick.AddListener(new UnityAction(this.GoForward));
				}
				if (componentInChildren.quickSitesPopup != null)
				{
					componentInChildren.quickSitesPopup.useDifferentDisplayValues = true;
					int num = 0;
					if (this.homeUrl != null && this.homeUrl != string.Empty)
					{
						componentInChildren.quickSitesPopup.numPopupValues = this.quickSitesList.Count + 1;
						componentInChildren.quickSitesPopup.setDisplayPopupValue(0, "Home");
						componentInChildren.quickSitesPopup.setPopupValue(0, this.homeUrl);
						num = 1;
					}
					else
					{
						componentInChildren.quickSitesPopup.numPopupValues = this.quickSitesList.Count;
					}
					for (int i = 0; i < this.quickSitesList.Count; i++)
					{
						componentInChildren.quickSitesPopup.setDisplayPopupValue(i + num, this.quickSitesList[i].name);
						componentInChildren.quickSitesPopup.setPopupValue(i + num, this.quickSitesList[i].url);
					}
					UIPopup quickSitesPopup = componentInChildren.quickSitesPopup;
					quickSitesPopup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(quickSitesPopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetUrl));
				}
				if (componentInChildren.homeButton != null)
				{
					componentInChildren.homeButton.onClick.AddListener(new UnityAction(this.GoHome));
				}
			}
		}
	}

	// Token: 0x06006FEE RID: 28654 RVA: 0x002A1CCC File Offset: 0x002A00CC
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			VRWebBrowserUI componentInChildren = this.UITransformAlt.GetComponentInChildren<VRWebBrowserUI>(true);
			if (componentInChildren != null)
			{
				this.fullMouseClickOnDownJSON.toggleAlt = componentInChildren.fullMouseClickOnDownToggle;
				this.disableInteractionJSON.toggleAlt = componentInChildren.disableInteractionToggle;
				this.urlJSON.inputFieldAlt = componentInChildren.urlInput;
				this.urlJSON.inputFieldActionAlt = componentInChildren.urlInputAction;
				this.urlJSON.copyToClipboardButtonAlt = componentInChildren.copyToClipboardButton;
				this.urlJSON.copyFromClipboardButtonAlt = componentInChildren.copyFromClipboardButton;
				this.urlJSON.setValToInputFieldButtonAlt = componentInChildren.goButton;
				this.navigatedURLTextAlt = componentInChildren.navigatedURLText;
				if (this.navigatedURLTextAlt != null)
				{
					this.navigatedURLTextAlt.text = this.navigatedURL;
				}
				if (componentInChildren.backButton != null)
				{
					componentInChildren.backButton.onClick.AddListener(new UnityAction(this.GoBack));
				}
				if (componentInChildren.forwardButton != null)
				{
					componentInChildren.forwardButton.onClick.AddListener(new UnityAction(this.GoForward));
				}
				if (componentInChildren.quickSitesPopup != null)
				{
					componentInChildren.quickSitesPopup.useDifferentDisplayValues = true;
					int num = 0;
					if (this.homeUrl != null && this.homeUrl != string.Empty)
					{
						componentInChildren.quickSitesPopup.numPopupValues = this.quickSitesList.Count + 1;
						componentInChildren.quickSitesPopup.setDisplayPopupValue(0, "Home");
						componentInChildren.quickSitesPopup.setPopupValue(0, this.homeUrl);
						num = 1;
					}
					else
					{
						componentInChildren.quickSitesPopup.numPopupValues = this.quickSitesList.Count;
					}
					for (int i = 0; i < this.quickSitesList.Count; i++)
					{
						componentInChildren.quickSitesPopup.setDisplayPopupValue(i + num, this.quickSitesList[i].name);
						componentInChildren.quickSitesPopup.setPopupValue(i + num, this.quickSitesList[i].url);
					}
					UIPopup quickSitesPopup = componentInChildren.quickSitesPopup;
					quickSitesPopup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(quickSitesPopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetUrl));
				}
				if (componentInChildren.homeButton != null)
				{
					componentInChildren.homeButton.onClick.AddListener(new UnityAction(this.GoHome));
				}
			}
		}
	}

	// Token: 0x06006FEF RID: 28655 RVA: 0x002A1F50 File Offset: 0x002A0350
	protected void Init()
	{
		this.BrowserCursor = new BrowserCursor();
		if (this.cursor != null)
		{
			this.cursor.texture = this.BrowserCursor.Texture;
		}
		this.InputSettings = new BrowserInputSettings();
		this.browser = base.GetComponent<Browser>();
		this.myImage = base.GetComponent<RawImage>();
		this.fullMouseClickOnDownJSON = new JSONStorableBool("fullMouseClickOnDown", this._fullMouseClickOnDown, new JSONStorableBool.SetBoolCallback(this.SyncFullMouseClickOnDown));
		base.RegisterBool(this.fullMouseClickOnDownJSON);
		this.disableInteractionJSON = new JSONStorableBool("disableInteraction", this._disableInteraction, new JSONStorableBool.SetBoolCallback(this.SyncDisableInteraction));
		base.RegisterBool(this.disableInteractionJSON);
		this.urlJSON = new JSONStorableUrl("url", this.browser.Url, new JSONStorableString.SetStringCallback(this.SyncUrl));
		this.urlJSON.disableOnEndEdit = true;
		base.RegisterUrl(this.urlJSON);
		this.hoveredURLJSON = new JSONStorableString("hoveredUrl", string.Empty);
		this.SyncDisableInteraction();
		MVRDownloadManager component = base.GetComponent<MVRDownloadManager>();
		if (component != null)
		{
			component.ManageDownloads(this.browser);
		}
		if (UserPreferences.singleton == null)
		{
			this.browser.enabled = false;
			this.myImage.enabled = false;
		}
		else
		{
			this.browser.enabled = UserPreferences.singleton.enableWebBrowser;
			this.myImage.enabled = UserPreferences.singleton.enableWebBrowser;
		}
		this.browser.afterResize += this.UpdateTexture;
		this.browser.UIHandler = this;
		this.browser.SetNewWindowHandler(this.newWindowAction, this);
		this.BrowserCursor.cursorChange += this.<Init>m__0;
		this.browser.onLoad += this.<Init>m__1;
		this.rTransform = base.GetComponent<RectTransform>();
		this.quickSitesList = new List<VRWebBrowser.QuickSite>();
		if (this.quickSitesFile != null && this.quickSitesFile != string.Empty && File.Exists(this.quickSitesFile))
		{
			try
			{
				using (StreamReader streamReader = new StreamReader(this.quickSitesFile))
				{
					string aJSON = streamReader.ReadToEnd();
					SimpleJSON.JSONNode jsonnode = JSON.Parse(aJSON);
					JSONArray asArray = jsonnode["sites"].AsArray;
					if (asArray != null)
					{
						for (int i = 0; i < asArray.Count; i++)
						{
							JSONArray asArray2 = asArray[i].AsArray;
							if (asArray2 != null)
							{
								VRWebBrowser.QuickSite item = new VRWebBrowser.QuickSite
								{
									name = asArray2[0],
									url = asArray2[1]
								};
								this.quickSitesList.Add(item);
							}
						}
					}
				}
			}
			catch (Exception arg)
			{
				SuperController.LogError("Exception during read of quick sites file " + arg);
			}
		}
	}

	// Token: 0x06006FF0 RID: 28656 RVA: 0x002A2288 File Offset: 0x002A0688
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

	// Token: 0x06006FF1 RID: 28657 RVA: 0x002A22AD File Offset: 0x002A06AD
	protected void OnEnable()
	{
		base.StartCoroutine(this.WatchResizeAndEnable());
	}

	// Token: 0x06006FF2 RID: 28658 RVA: 0x002A22BC File Offset: 0x002A06BC
	protected void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06006FF3 RID: 28659 RVA: 0x002A22C4 File Offset: 0x002A06C4
	private IEnumerator WatchResizeAndEnable()
	{
		Rect currentSize = default(Rect);
		for (;;)
		{
			bool userPrefEnabled = true;
			if (UserPreferences.singleton != null)
			{
				userPrefEnabled = UserPreferences.singleton.enableWebBrowser;
			}
			if (this.browser != null)
			{
				bool enabled = this.browser.enabled;
				this.browser.enabled = userPrefEnabled;
				if (!enabled && userPrefEnabled)
				{
					this.SyncUrl(this._url);
				}
				else if (enabled && !userPrefEnabled)
				{
					this.browser.Url = "about:blank";
				}
			}
			if (this.myImage != null)
			{
				this.myImage.enabled = userPrefEnabled;
			}
			if (this.browsersDisabledObject != null)
			{
				this.browsersDisabledObject.SetActive(!userPrefEnabled);
			}
			if (this.browserNotReadyObject != null)
			{
				this.browserNotReadyObject.SetActive(!this.browser.IsReady && userPrefEnabled);
			}
			Rect rect = this.rTransform.rect;
			if (rect.size.x <= 0f || rect.size.y <= 0f)
			{
				rect.size = new Vector2(512f, 512f);
			}
			if (rect.size != currentSize.size)
			{
				this.browser.Resize((int)rect.size.x, (int)rect.size.y);
				currentSize = rect;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006FF4 RID: 28660 RVA: 0x002A22DF File Offset: 0x002A06DF
	protected void UpdateTexture(Texture2D texture)
	{
		this.myImage.texture = texture;
		this.myImage.uvRect = new Rect(0f, 0f, 1f, 1f);
	}

	// Token: 0x06006FF5 RID: 28661 RVA: 0x002A2311 File Offset: 0x002A0711
	public void CopyURLToClipboard()
	{
		GUIUtility.systemCopyBuffer = this.url;
	}

	// Token: 0x06006FF6 RID: 28662 RVA: 0x002A231E File Offset: 0x002A071E
	public void CopyURLFromClipboard()
	{
		this.url = GUIUtility.systemCopyBuffer;
	}

	// Token: 0x06006FF7 RID: 28663 RVA: 0x002A232B File Offset: 0x002A072B
	public void SetUrl(string surl)
	{
		this.url = surl;
	}

	// Token: 0x06006FF8 RID: 28664 RVA: 0x002A2334 File Offset: 0x002A0734
	public void OpenLinkInExtenalBrowser(string url)
	{
		this.OpenLinkInExternalBrowser(url);
	}

	// Token: 0x06006FF9 RID: 28665 RVA: 0x002A233D File Offset: 0x002A073D
	public void OpenLinkInExternalBrowser(string url)
	{
		if (Regex.IsMatch(url, "^http") && UserPreferences.singleton.enableWebBrowser && UserPreferences.singleton.CheckWhitelistDomain(url))
		{
			Process.Start(url);
		}
	}

	// Token: 0x06006FFA RID: 28666 RVA: 0x002A2378 File Offset: 0x002A0778
	public virtual void InputUpdate()
	{
		List<Event> list = this.keyEvents;
		this.keyEvents = this.keyEventsLast;
		this.keyEventsLast = list;
		this.keyEvents.Clear();
		if (this.navigatedURLText != null)
		{
			this.navigatedURLText.text = this.browser.Url;
			this.SyncJSONToBrowserURL();
		}
		if (this.navigatedURLTextAlt != null)
		{
			this.navigatedURLTextAlt.text = this.browser.Url;
			this.SyncJSONToBrowserURL();
		}
		if (this.MouseHasFocus)
		{
			this.MousePosition = this._eventPosition;
			if (Input.GetMouseButtonDown(3))
			{
				this.GoBack();
			}
			if (Input.GetMouseButtonDown(4))
			{
				this.GoForward();
			}
			MouseButton mouseButton = (MouseButton)0;
			if (this._eventPointerDown)
			{
				mouseButton |= MouseButton.Left;
				if (this.fullMouseClickOnDown && !Input.GetMouseButton(0))
				{
					this._eventPointerDown = false;
				}
			}
			this.MouseButtons = mouseButton;
			this.MouseScroll = this._mouseScroll;
			this._mouseScroll.x = 0f;
			this._mouseScroll.y = 0f;
		}
		else
		{
			this.MouseButtons = (MouseButton)0;
		}
	}

	// Token: 0x06006FFB RID: 28667 RVA: 0x002A24AA File Offset: 0x002A08AA
	public void AddKeyEvent(Event ev)
	{
		this.keyEvents.Add(new Event(ev));
	}

	// Token: 0x06006FFC RID: 28668 RVA: 0x002A24C0 File Offset: 0x002A08C0
	public void OnGUI()
	{
		Event current = Event.current;
		if (current.type != EventType.KeyDown && current.type != EventType.KeyUp)
		{
			return;
		}
		this.keyEvents.Add(new Event(current));
	}

	// Token: 0x06006FFD RID: 28669 RVA: 0x002A2500 File Offset: 0x002A0900
	protected virtual void SetCursor(BrowserCursor newCursor)
	{
		if (this.cursor != null)
		{
			if (newCursor == null)
			{
				this.cursor.gameObject.SetActive(false);
				this.cursor.texture = null;
			}
			else
			{
				this.cursor.gameObject.SetActive(true);
				this.cursor.texture = newCursor.Texture;
			}
		}
	}

	// Token: 0x17001080 RID: 4224
	// (get) Token: 0x06006FFE RID: 28670 RVA: 0x002A2568 File Offset: 0x002A0968
	public bool MouseHasFocus
	{
		get
		{
			return this._mouseHasFocus && this.enableInput;
		}
	}

	// Token: 0x17001081 RID: 4225
	// (get) Token: 0x06006FFF RID: 28671 RVA: 0x002A257E File Offset: 0x002A097E
	// (set) Token: 0x06007000 RID: 28672 RVA: 0x002A2586 File Offset: 0x002A0986
	public Vector2 MousePosition
	{
		[CompilerGenerated]
		get
		{
			return this.<MousePosition>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<MousePosition>k__BackingField = value;
		}
	}

	// Token: 0x17001082 RID: 4226
	// (get) Token: 0x06007001 RID: 28673 RVA: 0x002A258F File Offset: 0x002A098F
	// (set) Token: 0x06007002 RID: 28674 RVA: 0x002A2597 File Offset: 0x002A0997
	public MouseButton MouseButtons
	{
		[CompilerGenerated]
		get
		{
			return this.<MouseButtons>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<MouseButtons>k__BackingField = value;
		}
	}

	// Token: 0x17001083 RID: 4227
	// (get) Token: 0x06007003 RID: 28675 RVA: 0x002A25A0 File Offset: 0x002A09A0
	// (set) Token: 0x06007004 RID: 28676 RVA: 0x002A25A8 File Offset: 0x002A09A8
	public Vector2 MouseScroll
	{
		[CompilerGenerated]
		get
		{
			return this.<MouseScroll>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<MouseScroll>k__BackingField = value;
		}
	}

	// Token: 0x17001084 RID: 4228
	// (get) Token: 0x06007005 RID: 28677 RVA: 0x002A25B1 File Offset: 0x002A09B1
	public bool KeyboardHasFocus
	{
		get
		{
			return this._keyboardHasFocus && this.enableInput;
		}
	}

	// Token: 0x17001085 RID: 4229
	// (get) Token: 0x06007006 RID: 28678 RVA: 0x002A25C7 File Offset: 0x002A09C7
	public List<Event> KeyEvents
	{
		get
		{
			return this.keyEventsLast;
		}
	}

	// Token: 0x17001086 RID: 4230
	// (get) Token: 0x06007007 RID: 28679 RVA: 0x002A25CF File Offset: 0x002A09CF
	// (set) Token: 0x06007008 RID: 28680 RVA: 0x002A25D7 File Offset: 0x002A09D7
	public BrowserCursor BrowserCursor
	{
		[CompilerGenerated]
		get
		{
			return this.<BrowserCursor>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<BrowserCursor>k__BackingField = value;
		}
	}

	// Token: 0x17001087 RID: 4231
	// (get) Token: 0x06007009 RID: 28681 RVA: 0x002A25E0 File Offset: 0x002A09E0
	// (set) Token: 0x0600700A RID: 28682 RVA: 0x002A25E8 File Offset: 0x002A09E8
	public BrowserInputSettings InputSettings
	{
		[CompilerGenerated]
		get
		{
			return this.<InputSettings>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<InputSettings>k__BackingField = value;
		}
	}

	// Token: 0x0600700B RID: 28683 RVA: 0x002A25F1 File Offset: 0x002A09F1
	public void OnSelect(BaseEventData eventData)
	{
		this._keyboardHasFocus = true;
	}

	// Token: 0x0600700C RID: 28684 RVA: 0x002A25FA File Offset: 0x002A09FA
	public void OnDeselect(BaseEventData eventData)
	{
		this._keyboardHasFocus = false;
	}

	// Token: 0x0600700D RID: 28685 RVA: 0x002A2604 File Offset: 0x002A0A04
	public void OnScroll(PointerEventData eventData)
	{
		if (Mathf.Abs(eventData.scrollDelta.x) > 0.01f || Mathf.Abs(eventData.scrollDelta.y) > 0.01f)
		{
			this._mouseScroll = eventData.scrollDelta * 0.01f;
		}
	}

	// Token: 0x0600700E RID: 28686 RVA: 0x002A2661 File Offset: 0x002A0A61
	public void Scroll(float scrollAmount)
	{
		this._mouseScroll.y = scrollAmount;
	}

	// Token: 0x0600700F RID: 28687 RVA: 0x002A266F File Offset: 0x002A0A6F
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.pointerCount++;
		this._mouseHasFocus = true;
		this.SetCursor(this.BrowserCursor);
	}

	// Token: 0x06007010 RID: 28688 RVA: 0x002A2692 File Offset: 0x002A0A92
	public void OnPointerExit(PointerEventData eventData)
	{
		this.pointerCount--;
		if (this.pointerCount == 0)
		{
			this._mouseHasFocus = false;
			this._eventPointerDown = false;
			this.SetCursor(null);
		}
	}

	// Token: 0x06007011 RID: 28689 RVA: 0x002A26C4 File Offset: 0x002A0AC4
	public void OnPointerUp(PointerEventData eventData)
	{
		this._eventPointerDown = false;
		Vector2 eventPosition;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)base.transform, eventData.position, eventData.enterEventCamera, out eventPosition))
		{
			eventPosition.x = eventPosition.x / this.rTransform.rect.width + 0.5f;
			eventPosition.y = eventPosition.y / this.rTransform.rect.height + 0.5f;
			this._eventPosition = eventPosition;
		}
	}

	// Token: 0x06007012 RID: 28690 RVA: 0x002A2754 File Offset: 0x002A0B54
	public void OnPointerDown(PointerEventData eventData)
	{
		this._eventPointerDown = true;
		Vector2 eventPosition;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)base.transform, eventData.position, eventData.enterEventCamera, out eventPosition))
		{
			eventPosition.x = eventPosition.x / this.rTransform.rect.width + 0.5f;
			eventPosition.y = eventPosition.y / this.rTransform.rect.height + 0.5f;
			this._eventPosition = eventPosition;
		}
		if (LookInputModule.singleton != null)
		{
			LookInputModule.singleton.Select(base.gameObject);
		}
	}

	// Token: 0x06007013 RID: 28691 RVA: 0x002A2804 File Offset: 0x002A0C04
	public void OnPointerMove(PointerEventData eventData)
	{
		Vector2 vector;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)base.transform, eventData.position, eventData.enterEventCamera, out vector))
		{
			if (this.cursor != null)
			{
				this.cursor.rectTransform.anchoredPosition = vector;
			}
			vector.x = vector.x / this.rTransform.rect.width + 0.5f;
			vector.y = vector.y / this.rTransform.rect.height + 0.5f;
			this._eventPosition = vector;
		}
	}

	// Token: 0x06007014 RID: 28692 RVA: 0x002A28AD File Offset: 0x002A0CAD
	[CompilerGenerated]
	private void <Init>m__0()
	{
		this.SetCursor(this.BrowserCursor);
	}

	// Token: 0x06007015 RID: 28693 RVA: 0x002A28BC File Offset: 0x002A0CBC
	[CompilerGenerated]
	private void <Init>m__1(ZenFulcrum.EmbeddedBrowser.JSONNode info)
	{
		Browser browser = this.browser;
		string name = "VRWBReportImageClick";
		if (VRWebBrowser.<>f__am$cache0 == null)
		{
			VRWebBrowser.<>f__am$cache0 = new Browser.JSCallback(VRWebBrowser.<Init>m__2);
		}
		browser.RegisterFunction(name, VRWebBrowser.<>f__am$cache0);
		this.browser.RegisterFunction("VRWBReportUrlHover", new Browser.JSCallback(this.<Init>m__3));
		this.browser.EvalJS("\r\n\t\t\t\twindow.addEventListener('click', ev => {\r\n\t\t\t\t\tif (ev.target.tagName == 'IMG') VRWBReportImageClick(ev.target.src);\r\n\t\t\t\t});\r\n\t\t\t\tfunction VRWBMouseEnterHandler(event) {\r\n\t\t\t\t\tVRWBReportUrlHover(event.target.href);\r\n\t\t\t\t};\r\n\t\t\t\tfunction VRWBMouseLeaveHandler(event) {\r\n\t\t\t\t\tVRWBReportUrlHover('');\r\n\t\t\t\t};\r\n\t\t\t\talla = document.getElementsByTagName('A');\r\n\t\t\t\tvar i;\r\n\t\t\t\tfor (i = 0; i < alla.length; i++) {\r\n\t\t\t\t  //alla[i].style.backgroundColor = 'red';\r\n\t\t\t\t  alla[i].addEventListener('mouseenter', VRWBMouseEnterHandler);\r\n\t\t\t\t  alla[i].addEventListener('mouseleave', VRWBMouseLeaveHandler);\r\n\t\t\t\t}\r\n\t\t\t", "scripted command");
	}

	// Token: 0x06007016 RID: 28694 RVA: 0x002A2928 File Offset: 0x002A0D28
	[CompilerGenerated]
	private static void <Init>m__2(ZenFulcrum.EmbeddedBrowser.JSONNode args)
	{
		string text = args[0];
		if (Regex.IsMatch(text, "^http"))
		{
			UnityEngine.Debug.Log("Image clicked " + text);
			GUIUtility.systemCopyBuffer = text;
		}
	}

	// Token: 0x06007017 RID: 28695 RVA: 0x002A2968 File Offset: 0x002A0D68
	[CompilerGenerated]
	private void <Init>m__3(ZenFulcrum.EmbeddedBrowser.JSONNode args)
	{
		string val = args[0];
		if (this.hoveredURLJSON != null)
		{
			this.hoveredURLJSON.val = val;
		}
	}

	// Token: 0x04006196 RID: 24982
	protected RawImage myImage;

	// Token: 0x04006197 RID: 24983
	protected Browser browser;

	// Token: 0x04006198 RID: 24984
	public GameObject nonWhitelistSiteObject;

	// Token: 0x04006199 RID: 24985
	public Text nonWhitelistSiteText;

	// Token: 0x0400619A RID: 24986
	public GameObject browsersDisabledObject;

	// Token: 0x0400619B RID: 24987
	public GameObject browserNotReadyObject;

	// Token: 0x0400619C RID: 24988
	public bool enableInput = true;

	// Token: 0x0400619D RID: 24989
	public RawImage cursor;

	// Token: 0x0400619E RID: 24990
	public string navigatedURL;

	// Token: 0x0400619F RID: 24991
	public Text navigatedURLText;

	// Token: 0x040061A0 RID: 24992
	public Text navigatedURLTextAlt;

	// Token: 0x040061A1 RID: 24993
	protected JSONStorableString hoveredURLJSON;

	// Token: 0x040061A2 RID: 24994
	protected List<VRWebBrowser.QuickSite> quickSitesList;

	// Token: 0x040061A3 RID: 24995
	public string homeUrl;

	// Token: 0x040061A4 RID: 24996
	public string quickSitesFile;

	// Token: 0x040061A5 RID: 24997
	protected JSONStorableUrl urlJSON;

	// Token: 0x040061A6 RID: 24998
	protected string _url;

	// Token: 0x040061A7 RID: 24999
	protected JSONStorableBool fullMouseClickOnDownJSON;

	// Token: 0x040061A8 RID: 25000
	[SerializeField]
	protected bool _fullMouseClickOnDown = true;

	// Token: 0x040061A9 RID: 25001
	protected JSONStorableBool disableInteractionJSON;

	// Token: 0x040061AA RID: 25002
	[SerializeField]
	protected bool _disableInteraction;

	// Token: 0x040061AB RID: 25003
	public Browser.NewWindowAction newWindowAction = Browser.NewWindowAction.Ignore;

	// Token: 0x040061AC RID: 25004
	protected List<Event> keyEvents = new List<Event>();

	// Token: 0x040061AD RID: 25005
	protected List<Event> keyEventsLast = new List<Event>();

	// Token: 0x040061AE RID: 25006
	protected BaseRaycaster raycaster;

	// Token: 0x040061AF RID: 25007
	protected RectTransform rTransform;

	// Token: 0x040061B0 RID: 25008
	protected float urlUpdateTimer = 1f;

	// Token: 0x040061B1 RID: 25009
	protected bool _mouseHasFocus;

	// Token: 0x040061B2 RID: 25010
	protected bool _eventPointerDown;

	// Token: 0x040061B3 RID: 25011
	protected Vector2 _eventPosition;

	// Token: 0x040061B4 RID: 25012
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Vector2 <MousePosition>k__BackingField;

	// Token: 0x040061B5 RID: 25013
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private MouseButton <MouseButtons>k__BackingField;

	// Token: 0x040061B6 RID: 25014
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Vector2 <MouseScroll>k__BackingField;

	// Token: 0x040061B7 RID: 25015
	protected bool _keyboardHasFocus;

	// Token: 0x040061B8 RID: 25016
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private BrowserCursor <BrowserCursor>k__BackingField;

	// Token: 0x040061B9 RID: 25017
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private BrowserInputSettings <InputSettings>k__BackingField;

	// Token: 0x040061BA RID: 25018
	protected Vector2 _mouseScroll;

	// Token: 0x040061BB RID: 25019
	protected int pointerCount;

	// Token: 0x040061BC RID: 25020
	[CompilerGenerated]
	private static Browser.JSCallback <>f__am$cache0;

	// Token: 0x02000E31 RID: 3633
	public struct QuickSite
	{
		// Token: 0x040061BD RID: 25021
		public string name;

		// Token: 0x040061BE RID: 25022
		public string url;
	}

	// Token: 0x02001042 RID: 4162
	[CompilerGenerated]
	private sealed class <WatchResizeAndEnable>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060077A6 RID: 30630 RVA: 0x002A2999 File Offset: 0x002A0D99
		[DebuggerHidden]
		public <WatchResizeAndEnable>c__Iterator0()
		{
		}

		// Token: 0x060077A7 RID: 30631 RVA: 0x002A29A4 File Offset: 0x002A0DA4
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				currentSize = default(Rect);
				break;
			case 1U:
				break;
			default:
				return false;
			}
			userPrefEnabled = true;
			if (UserPreferences.singleton != null)
			{
				userPrefEnabled = UserPreferences.singleton.enableWebBrowser;
			}
			if (this.browser != null)
			{
				bool enabled = this.browser.enabled;
				this.browser.enabled = userPrefEnabled;
				if (!enabled && userPrefEnabled)
				{
					base.SyncUrl(this._url);
				}
				else if (enabled && !userPrefEnabled)
				{
					this.browser.Url = "about:blank";
				}
			}
			if (this.myImage != null)
			{
				this.myImage.enabled = userPrefEnabled;
			}
			if (this.browsersDisabledObject != null)
			{
				this.browsersDisabledObject.SetActive(!userPrefEnabled);
			}
			if (this.browserNotReadyObject != null)
			{
				this.browserNotReadyObject.SetActive(!this.browser.IsReady && userPrefEnabled);
			}
			rect = this.rTransform.rect;
			if (rect.size.x <= 0f || rect.size.y <= 0f)
			{
				rect.size = new Vector2(512f, 512f);
			}
			if (rect.size != currentSize.size)
			{
				this.browser.Resize((int)rect.size.x, (int)rect.size.y);
				currentSize = rect;
			}
			this.$current = null;
			if (!this.$disposing)
			{
				this.$PC = 1;
			}
			return true;
		}

		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x060077A8 RID: 30632 RVA: 0x002A2C31 File Offset: 0x002A1031
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x060077A9 RID: 30633 RVA: 0x002A2C39 File Offset: 0x002A1039
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060077AA RID: 30634 RVA: 0x002A2C41 File Offset: 0x002A1041
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060077AB RID: 30635 RVA: 0x002A2C51 File Offset: 0x002A1051
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006BA3 RID: 27555
		internal Rect <currentSize>__0;

		// Token: 0x04006BA4 RID: 27556
		internal bool <userPrefEnabled>__1;

		// Token: 0x04006BA5 RID: 27557
		internal Rect <rect>__1;

		// Token: 0x04006BA6 RID: 27558
		internal VRWebBrowser $this;

		// Token: 0x04006BA7 RID: 27559
		internal object $current;

		// Token: 0x04006BA8 RID: 27560
		internal bool $disposing;

		// Token: 0x04006BA9 RID: 27561
		internal int $PC;
	}
}
