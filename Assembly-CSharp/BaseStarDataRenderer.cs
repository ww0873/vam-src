using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

// Token: 0x020002EC RID: 748
public abstract class BaseStarDataRenderer
{
	// Token: 0x0600119F RID: 4511 RVA: 0x00061779 File Offset: 0x0005FB79
	protected BaseStarDataRenderer()
	{
	}

	// Token: 0x14000077 RID: 119
	// (add) Token: 0x060011A0 RID: 4512 RVA: 0x0006178C File Offset: 0x0005FB8C
	// (remove) Token: 0x060011A1 RID: 4513 RVA: 0x000617C4 File Offset: 0x0005FBC4
	public event BaseStarDataRenderer.StarDataProgress progressCallback
	{
		add
		{
			BaseStarDataRenderer.StarDataProgress starDataProgress = this.progressCallback;
			BaseStarDataRenderer.StarDataProgress starDataProgress2;
			do
			{
				starDataProgress2 = starDataProgress;
				starDataProgress = Interlocked.CompareExchange<BaseStarDataRenderer.StarDataProgress>(ref this.progressCallback, (BaseStarDataRenderer.StarDataProgress)Delegate.Combine(starDataProgress2, value), starDataProgress);
			}
			while (starDataProgress != starDataProgress2);
		}
		remove
		{
			BaseStarDataRenderer.StarDataProgress starDataProgress = this.progressCallback;
			BaseStarDataRenderer.StarDataProgress starDataProgress2;
			do
			{
				starDataProgress2 = starDataProgress;
				starDataProgress = Interlocked.CompareExchange<BaseStarDataRenderer.StarDataProgress>(ref this.progressCallback, (BaseStarDataRenderer.StarDataProgress)Delegate.Remove(starDataProgress2, value), starDataProgress);
			}
			while (starDataProgress != starDataProgress2);
		}
	}

	// Token: 0x14000078 RID: 120
	// (add) Token: 0x060011A2 RID: 4514 RVA: 0x000617FC File Offset: 0x0005FBFC
	// (remove) Token: 0x060011A3 RID: 4515 RVA: 0x00061834 File Offset: 0x0005FC34
	public event BaseStarDataRenderer.StarDataComplete completionCallback
	{
		add
		{
			BaseStarDataRenderer.StarDataComplete starDataComplete = this.completionCallback;
			BaseStarDataRenderer.StarDataComplete starDataComplete2;
			do
			{
				starDataComplete2 = starDataComplete;
				starDataComplete = Interlocked.CompareExchange<BaseStarDataRenderer.StarDataComplete>(ref this.completionCallback, (BaseStarDataRenderer.StarDataComplete)Delegate.Combine(starDataComplete2, value), starDataComplete);
			}
			while (starDataComplete != starDataComplete2);
		}
		remove
		{
			BaseStarDataRenderer.StarDataComplete starDataComplete = this.completionCallback;
			BaseStarDataRenderer.StarDataComplete starDataComplete2;
			do
			{
				starDataComplete2 = starDataComplete;
				starDataComplete = Interlocked.CompareExchange<BaseStarDataRenderer.StarDataComplete>(ref this.completionCallback, (BaseStarDataRenderer.StarDataComplete)Delegate.Remove(starDataComplete2, value), starDataComplete);
			}
			while (starDataComplete != starDataComplete2);
		}
	}

	// Token: 0x060011A4 RID: 4516
	public abstract IEnumerator ComputeStarData();

	// Token: 0x060011A5 RID: 4517 RVA: 0x0006186A File Offset: 0x0005FC6A
	protected void SendProgress(float progress)
	{
		if (this.progressCallback != null)
		{
			this.progressCallback(this, progress);
		}
	}

	// Token: 0x060011A6 RID: 4518 RVA: 0x00061884 File Offset: 0x0005FC84
	protected void SendCompletion(Texture2D texture, bool success)
	{
		if (this.completionCallback != null)
		{
			this.completionCallback(this, texture, success);
		}
	}

	// Token: 0x04000F3B RID: 3899
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private BaseStarDataRenderer.StarDataProgress progressCallback;

	// Token: 0x04000F3C RID: 3900
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private BaseStarDataRenderer.StarDataComplete completionCallback;

	// Token: 0x04000F3D RID: 3901
	public float density;

	// Token: 0x04000F3E RID: 3902
	public float imageSize;

	// Token: 0x04000F3F RID: 3903
	protected float sphereRadius = 1f;

	// Token: 0x020002ED RID: 749
	// (Invoke) Token: 0x060011A8 RID: 4520
	public delegate void StarDataProgress(BaseStarDataRenderer renderer, float progress);

	// Token: 0x020002EE RID: 750
	// (Invoke) Token: 0x060011AC RID: 4524
	public delegate void StarDataComplete(BaseStarDataRenderer renderer, Texture2D texture, bool success);
}
