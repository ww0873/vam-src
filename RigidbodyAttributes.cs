using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000D8B RID: 3467
[ExecuteInEditMode]
public class RigidbodyAttributes : ScaleChangeReceiver
{
	// Token: 0x06006ADC RID: 27356 RVA: 0x00284019 File Offset: 0x00282419
	public RigidbodyAttributes()
	{
	}

	// Token: 0x06006ADD RID: 27357 RVA: 0x0028403B File Offset: 0x0028243B
	public Rigidbody GetRigidbody()
	{
		return this.rb;
	}

	// Token: 0x06006ADE RID: 27358 RVA: 0x00284044 File Offset: 0x00282444
	public void SyncTensor()
	{
		if (this.rb != null)
		{
			if (this._useOverrideTensor)
			{
				this.rb.inertiaTensor = this._inertiaTensor * this._scale;
			}
			else
			{
				this.rb.ResetInertiaTensor();
			}
		}
	}

	// Token: 0x06006ADF RID: 27359 RVA: 0x00284099 File Offset: 0x00282499
	public override void ScaleChanged(float scale)
	{
		base.ScaleChanged(scale);
		this.SyncTensor();
	}

	// Token: 0x17000FB5 RID: 4021
	// (get) Token: 0x06006AE0 RID: 27360 RVA: 0x002840A8 File Offset: 0x002824A8
	// (set) Token: 0x06006AE1 RID: 27361 RVA: 0x002840B0 File Offset: 0x002824B0
	public bool useOverrideTensor
	{
		get
		{
			return this._useOverrideTensor;
		}
		set
		{
			if (this._useOverrideTensor != value)
			{
				this._useOverrideTensor = value;
				this.SyncTensor();
			}
		}
	}

	// Token: 0x06006AE2 RID: 27362 RVA: 0x002840CB File Offset: 0x002824CB
	private void SetOriginalTensor()
	{
		if (this.rb != null)
		{
			this._originalTensor = this.rb.inertiaTensor;
			this._originalTensorRotation = this.rb.inertiaTensorRotation;
		}
	}

	// Token: 0x17000FB6 RID: 4022
	// (get) Token: 0x06006AE3 RID: 27363 RVA: 0x00284100 File Offset: 0x00282500
	public Vector3 originalTensor
	{
		get
		{
			return this._originalTensor;
		}
	}

	// Token: 0x17000FB7 RID: 4023
	// (get) Token: 0x06006AE4 RID: 27364 RVA: 0x00284108 File Offset: 0x00282508
	public Quaternion originalTensorRotation
	{
		get
		{
			return this._originalTensorRotation;
		}
	}

	// Token: 0x17000FB8 RID: 4024
	// (get) Token: 0x06006AE5 RID: 27365 RVA: 0x00284110 File Offset: 0x00282510
	public Vector3 currentTensor
	{
		get
		{
			if (this.rb != null)
			{
				return this.rb.inertiaTensor;
			}
			return Vector3.zero;
		}
	}

	// Token: 0x17000FB9 RID: 4025
	// (get) Token: 0x06006AE6 RID: 27366 RVA: 0x00284134 File Offset: 0x00282534
	// (set) Token: 0x06006AE7 RID: 27367 RVA: 0x0028413C File Offset: 0x0028253C
	public Vector3 inertiaTensor
	{
		get
		{
			return this._inertiaTensor;
		}
		set
		{
			if (this._inertiaTensor != value)
			{
				this._inertiaTensor = value;
				this.SyncTensor();
			}
		}
	}

	// Token: 0x17000FBA RID: 4026
	// (get) Token: 0x06006AE8 RID: 27368 RVA: 0x0028415C File Offset: 0x0028255C
	public float maxDepenetrationVelocity
	{
		get
		{
			if (this.rb != null)
			{
				return this.rb.maxDepenetrationVelocity;
			}
			return -1f;
		}
	}

	// Token: 0x17000FBB RID: 4027
	// (get) Token: 0x06006AE9 RID: 27369 RVA: 0x00284180 File Offset: 0x00282580
	public float maxAngularVelocity
	{
		get
		{
			if (this.rb != null)
			{
				return this.rb.maxAngularVelocity;
			}
			return -1f;
		}
	}

	// Token: 0x06006AEA RID: 27370 RVA: 0x002841A4 File Offset: 0x002825A4
	private void SyncInterpolation()
	{
		if (this.rb != null)
		{
			if (Application.isPlaying && base.isActiveAndEnabled && this._useInterpolation)
			{
				this.rb.interpolation = RigidbodyInterpolation.Interpolate;
			}
			else
			{
				this.rb.interpolation = RigidbodyInterpolation.None;
			}
		}
	}

	// Token: 0x06006AEB RID: 27371 RVA: 0x00284200 File Offset: 0x00282600
	private IEnumerator ResumeInterpolation()
	{
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		this.SyncInterpolation();
		yield break;
	}

	// Token: 0x06006AEC RID: 27372 RVA: 0x0028421B File Offset: 0x0028261B
	public void TempDisableInterpolation()
	{
		if (this.rb != null)
		{
			this.rb.interpolation = RigidbodyInterpolation.None;
			if (this._isEnabled)
			{
				base.StopAllCoroutines();
				base.StartCoroutine(this.ResumeInterpolation());
			}
		}
	}

	// Token: 0x17000FBC RID: 4028
	// (get) Token: 0x06006AED RID: 27373 RVA: 0x00284258 File Offset: 0x00282658
	// (set) Token: 0x06006AEE RID: 27374 RVA: 0x00284260 File Offset: 0x00282660
	public bool useInterpolation
	{
		get
		{
			return this._useInterpolation;
		}
		set
		{
			if (this._useInterpolation != value)
			{
				this._useInterpolation = value;
				this.SyncInterpolation();
			}
		}
	}

	// Token: 0x06006AEF RID: 27375 RVA: 0x0028427C File Offset: 0x0028267C
	protected void SetOverrideIterations()
	{
		if (this.rb != null && Application.isPlaying && base.isActiveAndEnabled && this._useOverrideIterations)
		{
			this.rb.solverIterations = this._solverIterations;
			this.rb.solverVelocityIterations = this._solverVelocityIterations;
		}
	}

	// Token: 0x17000FBD RID: 4029
	// (get) Token: 0x06006AF0 RID: 27376 RVA: 0x002842DC File Offset: 0x002826DC
	// (set) Token: 0x06006AF1 RID: 27377 RVA: 0x002842E4 File Offset: 0x002826E4
	public bool useOverrideIterations
	{
		get
		{
			return this._useOverrideIterations;
		}
		set
		{
			if (this._useOverrideIterations != value)
			{
				this._useOverrideIterations = value;
			}
		}
	}

	// Token: 0x06006AF2 RID: 27378 RVA: 0x002842F9 File Offset: 0x002826F9
	private void SetOriginalIterations()
	{
		if (this.rb != null)
		{
			this._originalIterations = this.rb.solverIterations;
			this._originalVelocityIterations = this.rb.solverVelocityIterations;
		}
	}

	// Token: 0x17000FBE RID: 4030
	// (get) Token: 0x06006AF3 RID: 27379 RVA: 0x0028432E File Offset: 0x0028272E
	public int origianlIterations
	{
		get
		{
			return this._originalIterations;
		}
	}

	// Token: 0x17000FBF RID: 4031
	// (get) Token: 0x06006AF4 RID: 27380 RVA: 0x00284336 File Offset: 0x00282736
	public int origianlVelocityIterations
	{
		get
		{
			return this._originalVelocityIterations;
		}
	}

	// Token: 0x17000FC0 RID: 4032
	// (get) Token: 0x06006AF5 RID: 27381 RVA: 0x0028433E File Offset: 0x0028273E
	public int currentIterations
	{
		get
		{
			if (this.rb != null)
			{
				return this.rb.solverIterations;
			}
			return 0;
		}
	}

	// Token: 0x17000FC1 RID: 4033
	// (get) Token: 0x06006AF6 RID: 27382 RVA: 0x0028435E File Offset: 0x0028275E
	// (set) Token: 0x06006AF7 RID: 27383 RVA: 0x00284366 File Offset: 0x00282766
	public int solverIterations
	{
		get
		{
			return this._solverIterations;
		}
		set
		{
			if (this._solverIterations != value)
			{
				this._solverIterations = value;
				this.SetOverrideIterations();
			}
		}
	}

	// Token: 0x17000FC2 RID: 4034
	// (get) Token: 0x06006AF8 RID: 27384 RVA: 0x00284381 File Offset: 0x00282781
	// (set) Token: 0x06006AF9 RID: 27385 RVA: 0x00284389 File Offset: 0x00282789
	public int solverVelocityIterations
	{
		get
		{
			return this._solverVelocityIterations;
		}
		set
		{
			if (this._solverVelocityIterations != value)
			{
				this._solverVelocityIterations = value;
				this.SetOverrideIterations();
			}
		}
	}

	// Token: 0x06006AFA RID: 27386 RVA: 0x002843A4 File Offset: 0x002827A4
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.SetOriginalTensor();
		this.SetOriginalIterations();
		this.SyncTensor();
		this.SetOverrideIterations();
	}

	// Token: 0x06006AFB RID: 27387 RVA: 0x002843CA File Offset: 0x002827CA
	private void OnEnable()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.SyncInterpolation();
		this.SyncTensor();
		this.SetOverrideIterations();
		this._isEnabled = true;
	}

	// Token: 0x06006AFC RID: 27388 RVA: 0x002843F1 File Offset: 0x002827F1
	private void OnDisable()
	{
		this.SyncInterpolation();
		this._isEnabled = false;
	}

	// Token: 0x04005CC7 RID: 23751
	private Rigidbody rb;

	// Token: 0x04005CC8 RID: 23752
	[SerializeField]
	private bool _useOverrideTensor;

	// Token: 0x04005CC9 RID: 23753
	private Vector3 _originalTensor;

	// Token: 0x04005CCA RID: 23754
	private Quaternion _originalTensorRotation;

	// Token: 0x04005CCB RID: 23755
	[SerializeField]
	private Vector3 _inertiaTensor = Vector3.one;

	// Token: 0x04005CCC RID: 23756
	[SerializeField]
	private bool _useInterpolation;

	// Token: 0x04005CCD RID: 23757
	[SerializeField]
	private bool _useOverrideIterations;

	// Token: 0x04005CCE RID: 23758
	private int _originalIterations;

	// Token: 0x04005CCF RID: 23759
	private int _originalVelocityIterations;

	// Token: 0x04005CD0 RID: 23760
	[SerializeField]
	private int _solverIterations = 10;

	// Token: 0x04005CD1 RID: 23761
	[SerializeField]
	private int _solverVelocityIterations = 1;

	// Token: 0x04005CD2 RID: 23762
	protected bool _isEnabled;

	// Token: 0x02001038 RID: 4152
	[CompilerGenerated]
	private sealed class <ResumeInterpolation>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600777D RID: 30589 RVA: 0x00284400 File Offset: 0x00282800
		[DebuggerHidden]
		public <ResumeInterpolation>c__Iterator0()
		{
		}

		// Token: 0x0600777E RID: 30590 RVA: 0x00284408 File Offset: 0x00282808
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
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			case 2U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 3;
				}
				return true;
			case 3U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 4;
				}
				return true;
			case 4U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 5;
				}
				return true;
			case 5U:
				base.SyncInterpolation();
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x0600777F RID: 30591 RVA: 0x002844E2 File Offset: 0x002828E2
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x06007780 RID: 30592 RVA: 0x002844EA File Offset: 0x002828EA
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007781 RID: 30593 RVA: 0x002844F2 File Offset: 0x002828F2
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007782 RID: 30594 RVA: 0x00284502 File Offset: 0x00282902
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006B82 RID: 27522
		internal RigidbodyAttributes $this;

		// Token: 0x04006B83 RID: 27523
		internal object $current;

		// Token: 0x04006B84 RID: 27524
		internal bool $disposing;

		// Token: 0x04006B85 RID: 27525
		internal int $PC;
	}
}
