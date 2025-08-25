using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C3C RID: 3132
public class MVRScriptUI : UIProvider
{
	// Token: 0x06005B38 RID: 23352 RVA: 0x00217F57 File Offset: 0x00216357
	public MVRScriptUI()
	{
	}

	// Token: 0x06005B39 RID: 23353 RVA: 0x00217F60 File Offset: 0x00216360
	private IEnumerator FixScrollRect(ScrollRect sr)
	{
		yield return null;
		sr.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
		yield break;
	}

	// Token: 0x06005B3A RID: 23354 RVA: 0x00217F7C File Offset: 0x0021637C
	private void OnEnable()
	{
		ScrollRect componentInChildren = base.GetComponentInChildren<ScrollRect>();
		if (componentInChildren != null)
		{
			componentInChildren.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
			base.StartCoroutine(this.FixScrollRect(componentInChildren));
		}
	}

	// Token: 0x04004B34 RID: 19252
	public RectTransform rightUIContent;

	// Token: 0x04004B35 RID: 19253
	public RectTransform leftUIContent;

	// Token: 0x04004B36 RID: 19254
	public RectTransform fullWidthUIContent;

	// Token: 0x04004B37 RID: 19255
	public Button closeButton;

	// Token: 0x02001006 RID: 4102
	[CompilerGenerated]
	private sealed class <FixScrollRect>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007688 RID: 30344 RVA: 0x00217FB1 File Offset: 0x002163B1
		[DebuggerHidden]
		public <FixScrollRect>c__Iterator0()
		{
		}

		// Token: 0x06007689 RID: 30345 RVA: 0x00217FBC File Offset: 0x002163BC
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				sr.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x0600768A RID: 30346 RVA: 0x0021801B File Offset: 0x0021641B
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x0600768B RID: 30347 RVA: 0x00218023 File Offset: 0x00216423
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600768C RID: 30348 RVA: 0x0021802B File Offset: 0x0021642B
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600768D RID: 30349 RVA: 0x0021803B File Offset: 0x0021643B
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006A54 RID: 27220
		internal ScrollRect sr;

		// Token: 0x04006A55 RID: 27221
		internal object $current;

		// Token: 0x04006A56 RID: 27222
		internal bool $disposing;

		// Token: 0x04006A57 RID: 27223
		internal int $PC;
	}
}
