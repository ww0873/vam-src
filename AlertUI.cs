using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000BFB RID: 3067
public class AlertUI : MonoBehaviour
{
	// Token: 0x0600589E RID: 22686 RVA: 0x0020796A File Offset: 0x00205D6A
	public AlertUI()
	{
	}

	// Token: 0x0600589F RID: 22687 RVA: 0x00207972 File Offset: 0x00205D72
	public void SetText(string txt)
	{
		if (this.alertText != null)
		{
			this.alertText.text = txt;
		}
	}

	// Token: 0x060058A0 RID: 22688 RVA: 0x00207991 File Offset: 0x00205D91
	public void DoOKCallback()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		if (this._okCallback != null)
		{
			this._okCallback();
		}
	}

	// Token: 0x060058A1 RID: 22689 RVA: 0x002079B4 File Offset: 0x00205DB4
	public void SetOKButton(UnityAction okCallback)
	{
		this._okCallback = okCallback;
	}

	// Token: 0x060058A2 RID: 22690 RVA: 0x002079BD File Offset: 0x00205DBD
	public void DoCancelCallback()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		if (this._cancelCallback != null)
		{
			this._cancelCallback();
		}
	}

	// Token: 0x060058A3 RID: 22691 RVA: 0x002079E0 File Offset: 0x00205DE0
	public void SetCancelButton(UnityAction cancelCallback)
	{
		if (this.cancelButton != null)
		{
			this._cancelCallback = cancelCallback;
		}
	}

	// Token: 0x060058A4 RID: 22692 RVA: 0x002079FC File Offset: 0x00205DFC
	private void Awake()
	{
		if (this.okButton != null)
		{
			this.okButton.onClick.AddListener(new UnityAction(this.DoOKCallback));
		}
		if (this.cancelButton != null)
		{
			this.cancelButton.onClick.AddListener(new UnityAction(this.DoCancelCallback));
		}
	}

	// Token: 0x040048F1 RID: 18673
	public Text alertText;

	// Token: 0x040048F2 RID: 18674
	public Button okButton;

	// Token: 0x040048F3 RID: 18675
	protected UnityAction _okCallback;

	// Token: 0x040048F4 RID: 18676
	public Button cancelButton;

	// Token: 0x040048F5 RID: 18677
	protected UnityAction _cancelCallback;
}
