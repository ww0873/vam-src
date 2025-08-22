using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x02000280 RID: 640
	public class PopupWindow : MonoBehaviour
	{
		// Token: 0x06000E35 RID: 3637 RVA: 0x000530E8 File Offset: 0x000514E8
		public PopupWindow()
		{
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x000530F0 File Offset: 0x000514F0
		public bool IsOpened
		{
			get
			{
				return this.m_isOpened;
			}
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x000530F8 File Offset: 0x000514F8
		private void Awake()
		{
			if (this.Prefab != null)
			{
				PopupWindow.m_instance = this;
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00053114 File Offset: 0x00051514
		private void Start()
		{
			if (this.BtnCancel != null)
			{
				this.BtnCancel.onClick.AddListener(new UnityAction(this.OnBtnCancel));
			}
			if (this.BtnOk != null)
			{
				this.BtnOk.onClick.AddListener(new UnityAction(this.OnBtnOk));
			}
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0005317B File Offset: 0x0005157B
		private void Update()
		{
			if (this == PopupWindow.m_instance)
			{
				return;
			}
			if (Input.GetKeyDown(KeyCode.Return))
			{
				this.OnBtnOk();
			}
			else if (Input.GetKeyDown(KeyCode.Escape))
			{
				this.OnBtnCancel();
			}
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x000531B8 File Offset: 0x000515B8
		private void OnDestroy()
		{
			if (this.BtnCancel != null)
			{
				this.BtnCancel.onClick.RemoveListener(new UnityAction(this.OnBtnCancel));
			}
			if (this.BtnOk != null)
			{
				this.BtnOk.onClick.RemoveListener(new UnityAction(this.OnBtnOk));
			}
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00053220 File Offset: 0x00051620
		private void OnBtnOk()
		{
			if (this.OK != null)
			{
				PopupWindowArgs popupWindowArgs = new PopupWindowArgs();
				this.OK.Invoke(popupWindowArgs);
				if (popupWindowArgs.Cancel)
				{
					return;
				}
			}
			if (this.m_okCallback != null)
			{
				PopupWindowArgs popupWindowArgs2 = new PopupWindowArgs();
				this.m_okCallback(popupWindowArgs2);
				if (popupWindowArgs2.Cancel)
				{
					return;
				}
			}
			this.HidePopup();
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00053288 File Offset: 0x00051688
		private void OnBtnCancel()
		{
			if (this.Cancel != null)
			{
				PopupWindowArgs popupWindowArgs = new PopupWindowArgs();
				this.Cancel.Invoke(popupWindowArgs);
				if (popupWindowArgs.Cancel)
				{
					return;
				}
			}
			if (this.m_cancelCallback != null)
			{
				PopupWindowArgs popupWindowArgs2 = new PopupWindowArgs();
				this.m_cancelCallback(popupWindowArgs2);
				if (popupWindowArgs2.Cancel)
				{
					return;
				}
			}
			this.HidePopup();
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x000532F0 File Offset: 0x000516F0
		private void HidePopup()
		{
			if (this.m_openedPopupWindows != null)
			{
				foreach (PopupWindow popupWindow in this.m_openedPopupWindows)
				{
					if (popupWindow != null)
					{
						popupWindow.gameObject.SetActive(true);
					}
				}
			}
			this.m_openedPopupWindows = null;
			base.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(base.gameObject);
			this.m_okCallback = null;
			this.m_cancelCallback = null;
			this.m_isOpened = false;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00053374 File Offset: 0x00051774
		private void ShowPopup(string header, Transform body, string ok = null, PopupWindowAction okCallback = null, string cancel = null, PopupWindowAction cancelCallback = null, float width = 500f)
		{
			IEnumerable<PopupWindow> source = UnityEngine.Object.FindObjectsOfType<PopupWindow>();
			if (PopupWindow.<>f__am$cache0 == null)
			{
				PopupWindow.<>f__am$cache0 = new Func<PopupWindow, bool>(PopupWindow.<ShowPopup>m__0);
			}
			this.m_openedPopupWindows = source.Where(PopupWindow.<>f__am$cache0).ToArray<PopupWindow>();
			foreach (PopupWindow popupWindow in this.m_openedPopupWindows)
			{
				popupWindow.gameObject.SetActive(false);
			}
			base.gameObject.SetActive(true);
			if (this.TxtHeader != null)
			{
				this.TxtHeader.text = header;
			}
			if (this.Body != null)
			{
				body.SetParent(this.Body, false);
			}
			if (this.BtnOk != null)
			{
				if (string.IsNullOrEmpty(ok))
				{
					this.BtnOk.gameObject.SetActive(false);
				}
				else
				{
					Text componentInChildren = this.BtnOk.GetComponentInChildren<Text>();
					if (componentInChildren != null)
					{
						componentInChildren.text = ok;
					}
				}
			}
			if (this.BtnCancel != null)
			{
				if (string.IsNullOrEmpty(cancel))
				{
					this.BtnCancel.gameObject.SetActive(false);
				}
				else
				{
					Text componentInChildren2 = this.BtnCancel.GetComponentInChildren<Text>();
					if (componentInChildren2 != null)
					{
						componentInChildren2.text = cancel;
					}
				}
			}
			if (this.Panel != null)
			{
				this.Panel.preferredWidth = width;
			}
			this.m_okCallback = okCallback;
			this.m_cancelCallback = cancelCallback;
			this.m_isOpened = true;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00053501 File Offset: 0x00051901
		public void Close(bool result)
		{
			if (result)
			{
				this.OnBtnOk();
			}
			else
			{
				this.OnBtnCancel();
			}
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0005351C File Offset: 0x0005191C
		public static void Show(string header, string body, string ok, PopupWindowAction okCallback = null, string cancel = null, PopupWindowAction cancelCallback = null, float width = 530f)
		{
			if (PopupWindow.m_instance == null)
			{
				Debug.LogWarning("PopupWindows.m_instance is null");
				return;
			}
			PopupWindow popupWindow = UnityEngine.Object.Instantiate<PopupWindow>(PopupWindow.m_instance.Prefab);
			popupWindow.transform.position = Vector3.zero;
			popupWindow.transform.SetParent(PopupWindow.m_instance.transform, false);
			popupWindow.DefaultBody.text = body;
			popupWindow.ShowPopup(header, popupWindow.DefaultBody.transform, ok, okCallback, cancel, cancelCallback, width);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x000535A0 File Offset: 0x000519A0
		public static void Show(string header, Transform body, string ok, PopupWindowAction okCallback = null, string cancel = null, PopupWindowAction cancelCallback = null, float width = 530f)
		{
			if (PopupWindow.m_instance == null)
			{
				Debug.LogWarning("PopupWindows.m_instance is null");
				return;
			}
			PopupWindow popupWindow = UnityEngine.Object.Instantiate<PopupWindow>(PopupWindow.m_instance.Prefab);
			popupWindow.transform.position = Vector3.zero;
			popupWindow.transform.SetParent(PopupWindow.m_instance.transform, false);
			if (popupWindow.DefaultBody != null)
			{
				UnityEngine.Object.Destroy(popupWindow.DefaultBody);
			}
			popupWindow.ShowPopup(header, body, ok, okCallback, cancel, cancelCallback, width);
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0005362A File Offset: 0x00051A2A
		[CompilerGenerated]
		private static bool <ShowPopup>m__0(PopupWindow wnd)
		{
			return wnd.IsOpened && wnd.isActiveAndEnabled;
		}

		// Token: 0x04000DA8 RID: 3496
		[SerializeField]
		private PopupWindow Prefab;

		// Token: 0x04000DA9 RID: 3497
		[SerializeField]
		private Text DefaultBody;

		// Token: 0x04000DAA RID: 3498
		[SerializeField]
		private Text TxtHeader;

		// Token: 0x04000DAB RID: 3499
		[SerializeField]
		private Transform Body;

		// Token: 0x04000DAC RID: 3500
		[SerializeField]
		private Button BtnCancel;

		// Token: 0x04000DAD RID: 3501
		[SerializeField]
		private Button BtnOk;

		// Token: 0x04000DAE RID: 3502
		[SerializeField]
		private LayoutElement Panel;

		// Token: 0x04000DAF RID: 3503
		public PopupWindowEvent OK;

		// Token: 0x04000DB0 RID: 3504
		public PopupWindowEvent Cancel;

		// Token: 0x04000DB1 RID: 3505
		private PopupWindowAction m_okCallback;

		// Token: 0x04000DB2 RID: 3506
		private PopupWindowAction m_cancelCallback;

		// Token: 0x04000DB3 RID: 3507
		private PopupWindow[] m_openedPopupWindows;

		// Token: 0x04000DB4 RID: 3508
		private static PopupWindow m_instance;

		// Token: 0x04000DB5 RID: 3509
		private bool m_isOpened;

		// Token: 0x04000DB6 RID: 3510
		[CompilerGenerated]
		private static Func<PopupWindow, bool> <>f__am$cache0;
	}
}
