using System;

// Token: 0x02000BC1 RID: 3009
public class ScaleChangeReceiverJSONStorable : JSONStorable
{
	// Token: 0x060055A3 RID: 21923 RVA: 0x0014EA72 File Offset: 0x0014CE72
	public ScaleChangeReceiverJSONStorable()
	{
	}

	// Token: 0x17000C80 RID: 3200
	// (get) Token: 0x060055A4 RID: 21924 RVA: 0x0014EA85 File Offset: 0x0014CE85
	public float scale
	{
		get
		{
			return this._scale;
		}
	}

	// Token: 0x060055A5 RID: 21925 RVA: 0x0014EA8D File Offset: 0x0014CE8D
	public virtual void ScaleChanged(float scale)
	{
		this._scale = scale;
	}

	// Token: 0x040046CA RID: 18122
	protected float _scale = 1f;
}
