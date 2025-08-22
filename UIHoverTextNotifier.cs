using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DF4 RID: 3572
public class UIHoverTextNotifier : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	// Token: 0x06006E71 RID: 28273 RVA: 0x00297289 File Offset: 0x00295689
	public UIHoverTextNotifier()
	{
	}

	// Token: 0x06006E72 RID: 28274 RVA: 0x00297291 File Offset: 0x00295691
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.text != null && this.onEnterNotifier != null)
		{
			this.onEnterNotifier(this.text);
		}
	}

	// Token: 0x06006E73 RID: 28275 RVA: 0x002972C0 File Offset: 0x002956C0
	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.text != null && this.onExitNotifier != null)
		{
			this.onExitNotifier(this.text);
		}
	}

	// Token: 0x06006E74 RID: 28276 RVA: 0x002972EF File Offset: 0x002956EF
	private void Awake()
	{
		this.text = base.transform.GetComponent<Text>();
	}

	// Token: 0x04005F90 RID: 24464
	public UIHoverTextNotifier.TextNotifier onEnterNotifier;

	// Token: 0x04005F91 RID: 24465
	public UIHoverTextNotifier.TextNotifier onExitNotifier;

	// Token: 0x04005F92 RID: 24466
	protected Text text;

	// Token: 0x02000DF5 RID: 3573
	// (Invoke) Token: 0x06006E76 RID: 28278
	public delegate void TextNotifier(Text text);
}
