using System;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020004B3 RID: 1203
	public class ColorPickerTester : MonoBehaviour
	{
		// Token: 0x06001E70 RID: 7792 RVA: 0x000AD0B5 File Offset: 0x000AB4B5
		public ColorPickerTester()
		{
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x000AD0BD File Offset: 0x000AB4BD
		private void Awake()
		{
			this.pickerRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x000AD0CB File Offset: 0x000AB4CB
		private void Start()
		{
			this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.<Start>m__0));
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000AD0E9 File Offset: 0x000AB4E9
		[CompilerGenerated]
		private void <Start>m__0(Color color)
		{
			this.pickerRenderer.material.color = color;
		}

		// Token: 0x040019A0 RID: 6560
		public Renderer pickerRenderer;

		// Token: 0x040019A1 RID: 6561
		public ColorPickerControl picker;
	}
}
