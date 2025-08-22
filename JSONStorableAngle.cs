using System;

// Token: 0x02000CD8 RID: 3288
public class JSONStorableAngle : JSONStorableFloat
{
	// Token: 0x0600635B RID: 25435 RVA: 0x0025E17E File Offset: 0x0025C57E
	public JSONStorableAngle(string n, float v, JSONStorableFloat.SetFloatCallback callback) : base(n, v, callback, -180f, 180f, true, true)
	{
	}

	// Token: 0x17000E92 RID: 3730
	// (get) Token: 0x0600635C RID: 25436 RVA: 0x0025E195 File Offset: 0x0025C595
	// (set) Token: 0x0600635D RID: 25437 RVA: 0x0025E1A0 File Offset: 0x0025C5A0
	public override float val
	{
		get
		{
			return this._val;
		}
		set
		{
			float num;
			for (num = value; num > 180f; num -= 360f)
			{
			}
			while (num < -180f)
			{
				num += 360f;
			}
			if (this._val != num)
			{
				this._val = num;
				if (this._slider != null)
				{
					this._slider.value = this._val;
				}
				if (this._sliderAlt != null)
				{
					this._sliderAlt.value = this._val;
				}
				if (this.setCallbackFunction != null)
				{
					this.setCallbackFunction(this._val);
				}
			}
		}
	}
}
