using System;
using UnityEngine;

// Token: 0x02000D3C RID: 3388
public class AdjustJointCOG : MonoBehaviour
{
	// Token: 0x0600678E RID: 26510 RVA: 0x0026F16B File Offset: 0x0026D56B
	public AdjustJointCOG()
	{
	}

	// Token: 0x17000F15 RID: 3861
	// (get) Token: 0x0600678F RID: 26511 RVA: 0x0026F17A File Offset: 0x0026D57A
	// (set) Token: 0x06006790 RID: 26512 RVA: 0x0026F182 File Offset: 0x0026D582
	public float percent
	{
		get
		{
			return this._percent;
		}
		set
		{
			this._percent = value;
		}
	}

	// Token: 0x06006791 RID: 26513 RVA: 0x0026F18B File Offset: 0x0026D58B
	private void Start()
	{
		this.RB = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06006792 RID: 26514 RVA: 0x0026F19C File Offset: 0x0026D59C
	private void Update()
	{
		if (this.on)
		{
			this.currentCOG = this.RB.centerOfMass;
			if (this.RB != null)
			{
				Vector3 vector = Vector3.Lerp(this.lowCOG, this.highCOG, this._percent);
				if (this.RB.centerOfMass != vector)
				{
					this.RB.centerOfMass = vector;
				}
			}
		}
	}

	// Token: 0x040058A6 RID: 22694
	public bool on = true;

	// Token: 0x040058A7 RID: 22695
	[SerializeField]
	private float _percent;

	// Token: 0x040058A8 RID: 22696
	public Vector3 currentCOG;

	// Token: 0x040058A9 RID: 22697
	public Vector3 lowCOG;

	// Token: 0x040058AA RID: 22698
	public Vector3 highCOG;

	// Token: 0x040058AB RID: 22699
	private Rigidbody RB;
}
