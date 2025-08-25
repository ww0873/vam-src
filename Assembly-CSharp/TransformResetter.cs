using System;
using UnityEngine;

// Token: 0x02000D92 RID: 3474
public class TransformResetter : PhysicsSimulator
{
	// Token: 0x06006B1A RID: 27418 RVA: 0x00284E3E File Offset: 0x0028323E
	public TransformResetter()
	{
	}

	// Token: 0x06006B1B RID: 27419 RVA: 0x00284E46 File Offset: 0x00283246
	protected override void SyncResetSimulation()
	{
		base.SyncResetSimulation();
		this.Init();
		this.ResetTransform();
	}

	// Token: 0x06006B1C RID: 27420 RVA: 0x00284E5A File Offset: 0x0028325A
	protected void Init()
	{
		if (!this.wasInit)
		{
			this.wasInit = true;
			this.startingLocalPosition = base.transform.localPosition;
			this.startingLocalRotation = base.transform.localRotation;
		}
	}

	// Token: 0x06006B1D RID: 27421 RVA: 0x00284E90 File Offset: 0x00283290
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x06006B1E RID: 27422 RVA: 0x00284E98 File Offset: 0x00283298
	protected void ResetTransform()
	{
		base.transform.localPosition = this.startingLocalPosition;
		base.transform.localRotation = this.startingLocalRotation;
	}

	// Token: 0x04005CED RID: 23789
	public Vector3 startingLocalPosition;

	// Token: 0x04005CEE RID: 23790
	public Quaternion startingLocalRotation;

	// Token: 0x04005CEF RID: 23791
	protected bool wasInit;
}
