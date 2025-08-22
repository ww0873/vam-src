using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000E13 RID: 3603
public class GlobalStopwatch : MonoBehaviour
{
	// Token: 0x06006F0A RID: 28426 RVA: 0x0029A5DE File Offset: 0x002989DE
	public GlobalStopwatch()
	{
	}

	// Token: 0x06006F0B RID: 28427 RVA: 0x0029A5E6 File Offset: 0x002989E6
	public static float GetElapsedMilliseconds()
	{
		if (GlobalStopwatch.singleton != null)
		{
			return (float)GlobalStopwatch.singleton.stopwatch.ElapsedTicks * GlobalStopwatch.singleton.f;
		}
		UnityEngine.Debug.LogError("GlobalStopwatch has not started yet");
		return 0f;
	}

	// Token: 0x06006F0C RID: 28428 RVA: 0x0029A623 File Offset: 0x00298A23
	private void Awake()
	{
		GlobalStopwatch.singleton = this;
		this.stopwatch = new Stopwatch();
		this.stopwatch.Start();
		this.f = 1000f / (float)Stopwatch.Frequency;
	}

	// Token: 0x06006F0D RID: 28429 RVA: 0x0029A653 File Offset: 0x00298A53
	private void OnDestroy()
	{
		if (this.stopwatch != null)
		{
			this.stopwatch.Stop();
		}
	}

	// Token: 0x04006008 RID: 24584
	public static GlobalStopwatch singleton;

	// Token: 0x04006009 RID: 24585
	private Stopwatch stopwatch;

	// Token: 0x0400600A RID: 24586
	private float f;
}
