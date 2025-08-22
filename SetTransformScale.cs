using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000BC2 RID: 3010
public class SetTransformScale : JSONStorable
{
	// Token: 0x060055A6 RID: 21926 RVA: 0x001F50BE File Offset: 0x001F34BE
	public SetTransformScale()
	{
	}

	// Token: 0x060055A7 RID: 21927 RVA: 0x001F50D4 File Offset: 0x001F34D4
	protected void FixForUnitScale()
	{
		if (this.needsUnitScaleFix)
		{
			if (this.additionalScaleTransform != null)
			{
				this.additionalScaleTransform.localScale = Vector3.one;
			}
			base.transform.localScale = Vector3.one;
			this.needsUnitScaleFix = false;
		}
	}

	// Token: 0x060055A8 RID: 21928 RVA: 0x001F5124 File Offset: 0x001F3524
	protected IEnumerator FixForUnitScaleCo()
	{
		yield return null;
		this.FixForUnitScale();
		yield break;
	}

	// Token: 0x060055A9 RID: 21929 RVA: 0x001F5140 File Offset: 0x001F3540
	protected void SyncScale(float newScale)
	{
		this._scale = newScale;
		if (newScale == 1f)
		{
			this.needsUnitScaleFix = true;
			newScale = 1.00001f;
		}
		else
		{
			this.needsUnitScaleFix = false;
		}
		Vector3 localScale;
		localScale.x = newScale;
		localScale.y = newScale;
		localScale.z = newScale;
		if (this.additionalScaleTransform != null)
		{
			if (this.transformToReparentWhenScaling != null)
			{
				Transform parent = this.transformToReparentWhenScaling.parent;
				this.transformToReparentWhenScaling.SetParent(base.transform, true);
				base.transform.localScale = localScale;
				this.additionalScaleTransform.localScale = localScale;
				this.transformToReparentWhenScaling.SetParent(parent, true);
			}
			else
			{
				base.transform.localScale = localScale;
				this.additionalScaleTransform.localScale = localScale;
			}
		}
		else
		{
			base.transform.localScale = localScale;
		}
		if (this.needsUnitScaleFix)
		{
			base.StartCoroutine(this.FixForUnitScaleCo());
		}
		if (this.containingAtom != null)
		{
			this.containingAtom.ScaleChanged(this._scale);
		}
	}

	// Token: 0x060055AA RID: 21930 RVA: 0x001F5260 File Offset: 0x001F3660
	protected void Init()
	{
		if (!this.wasInit)
		{
			this.scaleJSON = new JSONStorableFloat("scale", this._scale, new JSONStorableFloat.SetFloatCallback(this.SyncScale), 0.01f, 10f, false, true);
			base.RegisterFloat(this.scaleJSON);
			this.wasInit = true;
		}
	}

	// Token: 0x060055AB RID: 21931 RVA: 0x001F52BC File Offset: 0x001F36BC
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			this.Init();
			SetTransformScaleUI componentInChildren = this.UITransform.GetComponentInChildren<SetTransformScaleUI>(true);
			if (componentInChildren != null)
			{
				this.scaleJSON.slider = componentInChildren.scaleSlider;
			}
		}
	}

	// Token: 0x060055AC RID: 21932 RVA: 0x001F530C File Offset: 0x001F370C
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			this.Init();
			SetTransformScaleUI componentInChildren = this.UITransformAlt.GetComponentInChildren<SetTransformScaleUI>(true);
			if (componentInChildren != null)
			{
				this.scaleJSON.sliderAlt = componentInChildren.scaleSlider;
			}
		}
	}

	// Token: 0x060055AD RID: 21933 RVA: 0x001F535A File Offset: 0x001F375A
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

	// Token: 0x060055AE RID: 21934 RVA: 0x001F537F File Offset: 0x001F377F
	protected void OnEnable()
	{
		this.FixForUnitScale();
	}

	// Token: 0x060055AF RID: 21935 RVA: 0x001F5387 File Offset: 0x001F3787
	protected void OnDisable()
	{
		this.FixForUnitScale();
	}

	// Token: 0x040046CB RID: 18123
	public Transform additionalScaleTransform;

	// Token: 0x040046CC RID: 18124
	public Transform transformToReparentWhenScaling;

	// Token: 0x040046CD RID: 18125
	private bool needsUnitScaleFix;

	// Token: 0x040046CE RID: 18126
	protected JSONStorableFloat scaleJSON;

	// Token: 0x040046CF RID: 18127
	[SerializeField]
	protected float _scale = 1f;

	// Token: 0x040046D0 RID: 18128
	protected bool wasInit;

	// Token: 0x02000FE2 RID: 4066
	[CompilerGenerated]
	private sealed class <FixForUnitScaleCo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060075ED RID: 30189 RVA: 0x001F538F File Offset: 0x001F378F
		[DebuggerHidden]
		public <FixForUnitScaleCo>c__Iterator0()
		{
		}

		// Token: 0x060075EE RID: 30190 RVA: 0x001F5398 File Offset: 0x001F3798
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
				base.FixForUnitScale();
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x060075EF RID: 30191 RVA: 0x001F53F6 File Offset: 0x001F37F6
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x060075F0 RID: 30192 RVA: 0x001F53FE File Offset: 0x001F37FE
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060075F1 RID: 30193 RVA: 0x001F5406 File Offset: 0x001F3806
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060075F2 RID: 30194 RVA: 0x001F5416 File Offset: 0x001F3816
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x040069B6 RID: 27062
		internal SetTransformScale $this;

		// Token: 0x040069B7 RID: 27063
		internal object $current;

		// Token: 0x040069B8 RID: 27064
		internal bool $disposing;

		// Token: 0x040069B9 RID: 27065
		internal int $PC;
	}
}
