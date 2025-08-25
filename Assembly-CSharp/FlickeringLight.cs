using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

// Token: 0x020002BB RID: 699
public class FlickeringLight : MonoBehaviour
{
	// Token: 0x0600104B RID: 4171 RVA: 0x0005BD74 File Offset: 0x0005A174
	public FlickeringLight()
	{
	}

	// Token: 0x14000076 RID: 118
	// (add) Token: 0x0600104C RID: 4172 RVA: 0x0005BD7C File Offset: 0x0005A17C
	// (remove) Token: 0x0600104D RID: 4173 RVA: 0x0005BDB4 File Offset: 0x0005A1B4
	public event FlickeringLight.MainLoop mainLoop
	{
		add
		{
			FlickeringLight.MainLoop mainLoop = this.mainLoop;
			FlickeringLight.MainLoop mainLoop2;
			do
			{
				mainLoop2 = mainLoop;
				mainLoop = Interlocked.CompareExchange<FlickeringLight.MainLoop>(ref this.mainLoop, (FlickeringLight.MainLoop)Delegate.Combine(mainLoop2, value), mainLoop);
			}
			while (mainLoop != mainLoop2);
		}
		remove
		{
			FlickeringLight.MainLoop mainLoop = this.mainLoop;
			FlickeringLight.MainLoop mainLoop2;
			do
			{
				mainLoop2 = mainLoop;
				mainLoop = Interlocked.CompareExchange<FlickeringLight.MainLoop>(ref this.mainLoop, (FlickeringLight.MainLoop)Delegate.Remove(mainLoop2, value), mainLoop);
			}
			while (mainLoop != mainLoop2);
		}
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x0005BDEC File Offset: 0x0005A1EC
	private void Start()
	{
		this.light = base.GetComponent<Light>();
		this.intensityOrigin = this.light.intensity;
		this.rangeOrigin = this.light.range;
		this.positionOrigin = base.transform.localPosition;
		this.setNewTargets = true;
		this.mainLoop += this.IntensityAndRange;
		if (!this.MakeSourceStationary)
		{
			this.mainLoop += this.Position;
		}
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x0005BE70 File Offset: 0x0005A270
	private void IntensityAndRange()
	{
		if (this.setNewTargets)
		{
			this.intensityOffset = this.intensityOrigin * this.scale;
			this.rangeOffset = this.rangeOrigin * this.scale * 0.3f;
			this.intensityDelta = (this.intensityOrigin + UnityEngine.Random.Range(-this.intensityOffset, this.intensityOffset) - this.light.intensity) * this.speed;
			this.rangeTarget = this.rangeOrigin + UnityEngine.Random.Range(-this.rangeOffset, this.rangeOffset);
			this.rangeDelta = (this.rangeTarget - this.light.range) * this.speed;
			this.setNewTargets = false;
		}
		this.light.intensity += this.intensityDelta;
		this.light.range += this.rangeDelta;
		if (this.rangeDelta == 0f)
		{
			this.setNewTargets = true;
		}
		else if (this.rangeDelta > 0f)
		{
			if (this.light.range > this.rangeTarget)
			{
				this.setNewTargets = true;
			}
		}
		else if (this.light.range < this.rangeTarget || this.light.range < 0f)
		{
			this.setNewTargets = true;
		}
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x0005BFDC File Offset: 0x0005A3DC
	private void Position()
	{
		if (this.setNewTargets)
		{
			Vector3 a = this.positionOrigin;
			Vector3 vector = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
			this.positionDelta = (a + vector.normalized * this.positionOffset - base.transform.localPosition) * this.speed;
		}
		base.transform.localPosition += this.positionDelta;
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x0005C084 File Offset: 0x0005A484
	private void Update()
	{
		if (this.deltaSum >= 0.02f)
		{
			this.mainLoop();
			this.deltaSum -= 0.02f;
		}
		this.deltaSum += Time.deltaTime;
	}

	// Token: 0x04000E77 RID: 3703
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private FlickeringLight.MainLoop mainLoop;

	// Token: 0x04000E78 RID: 3704
	[Tooltip("This is how big the light is. Experiment with it.")]
	public float scale;

	// Token: 0x04000E79 RID: 3705
	[Tooltip("The moves (new targets for properties; intensity, range, position) your light will do per second.")]
	public float speed;

	// Token: 0x04000E7A RID: 3706
	[HideInInspector]
	public bool MakeSourceStationary;

	// Token: 0x04000E7B RID: 3707
	[HideInInspector]
	public float positionOffset;

	// Token: 0x04000E7C RID: 3708
	private Light light;

	// Token: 0x04000E7D RID: 3709
	public float intensityOrigin;

	// Token: 0x04000E7E RID: 3710
	private float intensityOffset;

	// Token: 0x04000E7F RID: 3711
	private float intensityDelta;

	// Token: 0x04000E80 RID: 3712
	private float rangeOrigin;

	// Token: 0x04000E81 RID: 3713
	private float rangeOffset;

	// Token: 0x04000E82 RID: 3714
	private float rangeTarget;

	// Token: 0x04000E83 RID: 3715
	private float rangeDelta;

	// Token: 0x04000E84 RID: 3716
	private Vector3 positionOrigin;

	// Token: 0x04000E85 RID: 3717
	private Vector3 positionDelta;

	// Token: 0x04000E86 RID: 3718
	private bool setNewTargets;

	// Token: 0x04000E87 RID: 3719
	private float deltaSum;

	// Token: 0x020002BC RID: 700
	// (Invoke) Token: 0x06001053 RID: 4179
	public delegate void MainLoop();
}
