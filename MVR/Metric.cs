using System;

namespace MVR
{
	// Token: 0x02000DD2 RID: 3538
	public class Metric
	{
		// Token: 0x06006D96 RID: 28054 RVA: 0x0029385E File Offset: 0x00291C5E
		public Metric(string name, string valueFormat = "F2")
		{
			this.Name = name;
			this.ValueFormat = valueFormat;
		}

		// Token: 0x06006D97 RID: 28055 RVA: 0x00293874 File Offset: 0x00291C74
		protected void SyncNameText()
		{
			if (this._UI != null && this._UI.nameText != null)
			{
				this._UI.nameText.text = this._name;
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x06006D98 RID: 28056 RVA: 0x002938B3 File Offset: 0x00291CB3
		// (set) Token: 0x06006D99 RID: 28057 RVA: 0x002938BB File Offset: 0x00291CBB
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (this._name != value)
				{
					this._name = value;
					this.SyncNameText();
				}
			}
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06006D9A RID: 28058 RVA: 0x002938DB File Offset: 0x00291CDB
		// (set) Token: 0x06006D9B RID: 28059 RVA: 0x002938E3 File Offset: 0x00291CE3
		public string ValueFormat
		{
			get
			{
				return this._valueFormat;
			}
			set
			{
				if (this._valueFormat != value)
				{
					this._valueFormat = value;
					this.SyncValueText();
					this.SyncAverageValueText();
				}
			}
		}

		// Token: 0x06006D9C RID: 28060 RVA: 0x0029390C File Offset: 0x00291D0C
		protected void SyncValueText()
		{
			if (this._UI != null && this._UI.valueText != null)
			{
				this._UI.valueText.text = this._value.ToString(this._valueFormat);
			}
		}

		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x06006D9D RID: 28061 RVA: 0x00293961 File Offset: 0x00291D61
		// (set) Token: 0x06006D9E RID: 28062 RVA: 0x00293969 File Offset: 0x00291D69
		public float Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (this._value != value)
				{
					this._value = value;
					this.SyncValueText();
				}
			}
		}

		// Token: 0x06006D9F RID: 28063 RVA: 0x00293984 File Offset: 0x00291D84
		public void Accumulate(float valueToAdd)
		{
			this.Value = valueToAdd;
			this._accumulateValue += valueToAdd;
			this._accumulateCount++;
		}

		// Token: 0x06006DA0 RID: 28064 RVA: 0x002939A9 File Offset: 0x00291DA9
		public void CalculateAverage()
		{
			this.AverageValue = this._accumulateValue / (float)this._accumulateCount;
			this._accumulateValue = 0f;
			this._accumulateCount = 0;
		}

		// Token: 0x06006DA1 RID: 28065 RVA: 0x002939D4 File Offset: 0x00291DD4
		protected void SyncAverageValueText()
		{
			if (this._UI != null && this._UI.averageValueText != null)
			{
				this._UI.averageValueText.text = this._averageValue.ToString(this._valueFormat);
			}
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x06006DA2 RID: 28066 RVA: 0x00293A29 File Offset: 0x00291E29
		// (set) Token: 0x06006DA3 RID: 28067 RVA: 0x00293A31 File Offset: 0x00291E31
		public float AverageValue
		{
			get
			{
				return this._averageValue;
			}
			set
			{
				if (this._averageValue != value)
				{
					this._averageValue = value;
					this.SyncAverageValueText();
				}
			}
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x06006DA4 RID: 28068 RVA: 0x00293A4C File Offset: 0x00291E4C
		// (set) Token: 0x06006DA5 RID: 28069 RVA: 0x00293A54 File Offset: 0x00291E54
		public MetricUI UI
		{
			get
			{
				return this._UI;
			}
			set
			{
				if (this._UI != value)
				{
					this._UI = value;
					this.SyncNameText();
					this.SyncValueText();
					this.SyncAverageValueText();
				}
			}
		}

		// Token: 0x04005EED RID: 24301
		protected string _name;

		// Token: 0x04005EEE RID: 24302
		protected string _valueFormat;

		// Token: 0x04005EEF RID: 24303
		protected float _value;

		// Token: 0x04005EF0 RID: 24304
		protected int _accumulateCount;

		// Token: 0x04005EF1 RID: 24305
		protected float _accumulateValue;

		// Token: 0x04005EF2 RID: 24306
		protected float _averageValue;

		// Token: 0x04005EF3 RID: 24307
		protected MetricUI _UI;
	}
}
