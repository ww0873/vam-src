using System;
using UnityEngine;

// Token: 0x02000B2C RID: 2860
public class SetDAZMorph : MonoBehaviour
{
	// Token: 0x06004E48 RID: 20040 RVA: 0x001B9446 File Offset: 0x001B7846
	public SetDAZMorph()
	{
	}

	// Token: 0x17000B23 RID: 2851
	// (get) Token: 0x06004E49 RID: 20041 RVA: 0x001B9460 File Offset: 0x001B7860
	// (set) Token: 0x06004E4A RID: 20042 RVA: 0x001B9468 File Offset: 0x001B7868
	public DAZMorphBank morphBank
	{
		get
		{
			return this._morphBank;
		}
		set
		{
			if (this._morphBank != value)
			{
				this._morphBank = value;
				this.InitMorphs(false);
			}
		}
	}

	// Token: 0x17000B24 RID: 2852
	// (get) Token: 0x06004E4B RID: 20043 RVA: 0x001B9489 File Offset: 0x001B7889
	// (set) Token: 0x06004E4C RID: 20044 RVA: 0x001B9491 File Offset: 0x001B7891
	public DAZMorphBank morphBankAlt
	{
		get
		{
			return this._morphBankAlt;
		}
		set
		{
			if (this._morphBankAlt != value)
			{
				this._morphBankAlt = value;
				this.InitMorphs(false);
			}
		}
	}

	// Token: 0x17000B25 RID: 2853
	// (get) Token: 0x06004E4D RID: 20045 RVA: 0x001B94B2 File Offset: 0x001B78B2
	// (set) Token: 0x06004E4E RID: 20046 RVA: 0x001B94BA File Offset: 0x001B78BA
	public DAZMorphBank morphBankAlt2
	{
		get
		{
			return this._morphBankAlt2;
		}
		set
		{
			if (this._morphBankAlt2 != value)
			{
				this._morphBankAlt2 = value;
				this.InitMorphs(false);
			}
		}
	}

	// Token: 0x17000B26 RID: 2854
	// (get) Token: 0x06004E4F RID: 20047 RVA: 0x001B94DB File Offset: 0x001B78DB
	// (set) Token: 0x06004E50 RID: 20048 RVA: 0x001B94E4 File Offset: 0x001B78E4
	public float morphPercent
	{
		get
		{
			return this._morphPercent;
		}
		set
		{
			this._morphPercent = value;
			if (this.morph1 != null)
			{
				this.currentMorph1Value = Mathf.Lerp(this.morph1Low, this.morph1High, this._morphPercent);
				this.morph1.morphValue = this.currentMorph1Value;
			}
		}
	}

	// Token: 0x17000B27 RID: 2855
	// (get) Token: 0x06004E51 RID: 20049 RVA: 0x001B9531 File Offset: 0x001B7931
	// (set) Token: 0x06004E52 RID: 20050 RVA: 0x001B953C File Offset: 0x001B793C
	public float morphPercentUnclamped
	{
		get
		{
			return this._morphPercent;
		}
		set
		{
			this._morphPercent = value;
			if (this.morph1 != null)
			{
				this.currentMorph1Value = Mathf.LerpUnclamped(this.morph1Low, this.morph1High, this._morphPercent);
				this.morph1.morphValue = this.currentMorph1Value;
			}
		}
	}

	// Token: 0x17000B28 RID: 2856
	// (get) Token: 0x06004E53 RID: 20051 RVA: 0x001B9589 File Offset: 0x001B7989
	// (set) Token: 0x06004E54 RID: 20052 RVA: 0x001B95A7 File Offset: 0x001B79A7
	public float morphRawValue
	{
		get
		{
			if (this.morph1 != null)
			{
				return this.morph1.morphValue;
			}
			return 0f;
		}
		set
		{
			if (this.morph1 != null)
			{
				this.currentMorph1Value = value;
				this.morph1.morphValue = value;
			}
		}
	}

	// Token: 0x06004E55 RID: 20053 RVA: 0x001B95C8 File Offset: 0x001B79C8
	protected void InitMorphs(bool isEnable = false)
	{
		if (this._morphBank != null)
		{
			this._morphBank.Init();
			DAZMorph builtInMorph = this._morphBank.GetBuiltInMorph(this.morph1Name);
			if (builtInMorph == null)
			{
				if (this._morphBankAlt != null)
				{
					this._morphBankAlt.Init();
					builtInMorph = this._morphBankAlt.GetBuiltInMorph(this.morph1Name);
				}
				if (builtInMorph == null && this._morphBankAlt2 != null)
				{
					this._morphBankAlt2.Init();
					builtInMorph = this._morphBankAlt2.GetBuiltInMorph(this.morph1Name);
				}
			}
			if (this.morph1 != null && this.morph1 != builtInMorph)
			{
				this.morph1.morphValue = 0f;
			}
			this.morph1 = builtInMorph;
			if (this.morph1 != null && isEnable && this.updateOnEnableAndDisable)
			{
				this.morph1.morphValue = this.currentMorph1Value;
			}
		}
		else
		{
			if (this.morph1 != null)
			{
				this.morph1.morphValue = 0f;
			}
			this.morph1 = null;
		}
	}

	// Token: 0x06004E56 RID: 20054 RVA: 0x001B96ED File Offset: 0x001B7AED
	private void OnEnable()
	{
		this.isOn = true;
		this.InitMorphs(true);
	}

	// Token: 0x06004E57 RID: 20055 RVA: 0x001B96FD File Offset: 0x001B7AFD
	private void Start()
	{
		this.InitMorphs(true);
	}

	// Token: 0x06004E58 RID: 20056 RVA: 0x001B9706 File Offset: 0x001B7B06
	private void OnDisable()
	{
		this.isOn = false;
		if (this.morph1 != null && this.updateOnEnableAndDisable)
		{
			this.morph1.morphValue = 0f;
		}
	}

	// Token: 0x04003DFE RID: 15870
	protected bool isOn;

	// Token: 0x04003DFF RID: 15871
	[SerializeField]
	protected DAZMorphBank _morphBank;

	// Token: 0x04003E00 RID: 15872
	[SerializeField]
	protected DAZMorphBank _morphBankAlt;

	// Token: 0x04003E01 RID: 15873
	[SerializeField]
	protected DAZMorphBank _morphBankAlt2;

	// Token: 0x04003E02 RID: 15874
	public string morph1Name;

	// Token: 0x04003E03 RID: 15875
	public float morph1Low;

	// Token: 0x04003E04 RID: 15876
	public float morph1High = 1f;

	// Token: 0x04003E05 RID: 15877
	public float currentMorph1Value;

	// Token: 0x04003E06 RID: 15878
	public bool updateOnEnableAndDisable = true;

	// Token: 0x04003E07 RID: 15879
	private float _morphPercent;

	// Token: 0x04003E08 RID: 15880
	protected DAZMorph morph1;
}
