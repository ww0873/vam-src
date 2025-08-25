using System;
using UnityEngine;

// Token: 0x02000C78 RID: 3192
public class ThreadControl : MonoBehaviour
{
	// Token: 0x06005F68 RID: 24424 RVA: 0x002408DF File Offset: 0x0023ECDF
	public ThreadControl()
	{
	}

	// Token: 0x17000DFA RID: 3578
	// (get) Token: 0x06005F69 RID: 24425 RVA: 0x002408E7 File Offset: 0x0023ECE7
	// (set) Token: 0x06005F6A RID: 24426 RVA: 0x002408EF File Offset: 0x0023ECEF
	public int numSubThreads
	{
		get
		{
			return this._numSubThreads;
		}
		set
		{
			if (this._numSubThreads != value)
			{
				this._numSubThreads = value;
				if (this.onNumSubThreadsChangedHandlers != null)
				{
					this.onNumSubThreadsChangedHandlers(this._numSubThreads);
				}
			}
		}
	}

	// Token: 0x17000DFB RID: 3579
	// (get) Token: 0x06005F6B RID: 24427 RVA: 0x00240920 File Offset: 0x0023ED20
	// (set) Token: 0x06005F6C RID: 24428 RVA: 0x00240929 File Offset: 0x0023ED29
	public float numSubThreadsFloat
	{
		get
		{
			return (float)this._numSubThreads;
		}
		set
		{
			this.numSubThreads = (int)value;
		}
	}

	// Token: 0x06005F6D RID: 24429 RVA: 0x00240933 File Offset: 0x0023ED33
	private void Awake()
	{
		ThreadControl.singleton = this;
	}

	// Token: 0x04004F2C RID: 20268
	public static ThreadControl singleton;

	// Token: 0x04004F2D RID: 20269
	public ThreadControl.OnNumSubThreadsChanged onNumSubThreadsChangedHandlers;

	// Token: 0x04004F2E RID: 20270
	[SerializeField]
	private int _numSubThreads;

	// Token: 0x02000C79 RID: 3193
	// (Invoke) Token: 0x06005F6F RID: 24431
	public delegate void OnNumSubThreadsChanged(int numThreads);
}
