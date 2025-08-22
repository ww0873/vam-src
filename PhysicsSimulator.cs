using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D84 RID: 3460
public class PhysicsSimulator : ScaleChangeReceiver
{
	// Token: 0x06006A8D RID: 27277 RVA: 0x0014595A File Offset: 0x00143D5A
	public PhysicsSimulator()
	{
	}

	// Token: 0x06006A8E RID: 27278 RVA: 0x0014596A File Offset: 0x00143D6A
	protected virtual void SyncResetSimulation()
	{
		this.SyncCollisionEnabled();
	}

	// Token: 0x17000FA5 RID: 4005
	// (get) Token: 0x06006A8F RID: 27279 RVA: 0x00145972 File Offset: 0x00143D72
	// (set) Token: 0x06006A90 RID: 27280 RVA: 0x0014597A File Offset: 0x00143D7A
	public virtual bool resetSimulation
	{
		get
		{
			return this._resetSimulation;
		}
		set
		{
			if (this._resetSimulation != value)
			{
				this._resetSimulation = value;
				this.SyncResetSimulation();
			}
		}
	}

	// Token: 0x06006A91 RID: 27281 RVA: 0x00145995 File Offset: 0x00143D95
	protected virtual void SyncFreezeSimulation()
	{
		this.SyncCollisionEnabled();
	}

	// Token: 0x17000FA6 RID: 4006
	// (get) Token: 0x06006A92 RID: 27282 RVA: 0x0014599D File Offset: 0x00143D9D
	// (set) Token: 0x06006A93 RID: 27283 RVA: 0x001459A5 File Offset: 0x00143DA5
	public virtual bool freezeSimulation
	{
		get
		{
			return this._freezeSimulation;
		}
		set
		{
			if (this._freezeSimulation != value)
			{
				this._freezeSimulation = value;
				this.SyncFreezeSimulation();
			}
		}
	}

	// Token: 0x06006A94 RID: 27284 RVA: 0x001459C0 File Offset: 0x00143DC0
	protected virtual void SyncCollisionEnabled()
	{
	}

	// Token: 0x17000FA7 RID: 4007
	// (get) Token: 0x06006A95 RID: 27285 RVA: 0x001459C2 File Offset: 0x00143DC2
	// (set) Token: 0x06006A96 RID: 27286 RVA: 0x001459CA File Offset: 0x00143DCA
	public virtual bool collisionEnabled
	{
		get
		{
			return this._collisionEnabled;
		}
		set
		{
			if (this._collisionEnabled != value)
			{
				this._collisionEnabled = value;
				this.SyncCollisionEnabled();
			}
		}
	}

	// Token: 0x06006A97 RID: 27287 RVA: 0x001459E5 File Offset: 0x00143DE5
	protected virtual void SyncUseInterpolation()
	{
	}

	// Token: 0x17000FA8 RID: 4008
	// (get) Token: 0x06006A98 RID: 27288 RVA: 0x001459E7 File Offset: 0x00143DE7
	// (set) Token: 0x06006A99 RID: 27289 RVA: 0x001459EF File Offset: 0x00143DEF
	public virtual bool useInterpolation
	{
		get
		{
			return this._useInterpolation;
		}
		set
		{
			if (this._useInterpolation != value)
			{
				this._useInterpolation = value;
				this.SyncUseInterpolation();
			}
		}
	}

	// Token: 0x06006A9A RID: 27290 RVA: 0x00145A0A File Offset: 0x00143E0A
	protected virtual void SyncSolverIterations()
	{
	}

	// Token: 0x17000FA9 RID: 4009
	// (get) Token: 0x06006A9B RID: 27291 RVA: 0x00145A0C File Offset: 0x00143E0C
	// (set) Token: 0x06006A9C RID: 27292 RVA: 0x00145A14 File Offset: 0x00143E14
	public virtual int solverIterations
	{
		get
		{
			return this._solverIterations;
		}
		set
		{
			if (this._solverIterations != value)
			{
				this._solverIterations = value;
				this.SyncSolverIterations();
			}
		}
	}

	// Token: 0x06006A9D RID: 27293 RVA: 0x00145A30 File Offset: 0x00143E30
	protected virtual void CheckResumeSimulation()
	{
		if (this.waitResumeSimulationFlags == null)
		{
			this.waitResumeSimulationFlags = new List<AsyncFlag>();
		}
		if (this.waitResumeSimulationFlags.Count > 0)
		{
			bool flag = false;
			List<AsyncFlag> list = new List<AsyncFlag>();
			foreach (AsyncFlag asyncFlag in this.waitResumeSimulationFlags)
			{
				if (asyncFlag.Raised)
				{
					list.Add(asyncFlag);
					flag = true;
				}
			}
			foreach (AsyncFlag item in list)
			{
				this.waitResumeSimulationFlags.Remove(item);
			}
			if (this.waitResumeSimulationFlags.Count > 0)
			{
				this.resetSimulation = true;
			}
			else if (flag)
			{
				this.resetSimulation = false;
			}
		}
	}

	// Token: 0x06006A9E RID: 27294 RVA: 0x00145B44 File Offset: 0x00143F44
	public virtual bool IsSimulationResetting()
	{
		return this._resetSimulation;
	}

	// Token: 0x06006A9F RID: 27295 RVA: 0x00145B4C File Offset: 0x00143F4C
	public virtual void ResetSimulation(AsyncFlag waitFor)
	{
		if (this.waitResumeSimulationFlags == null)
		{
			this.waitResumeSimulationFlags = new List<AsyncFlag>();
		}
		this.waitResumeSimulationFlags.Add(waitFor);
		this.resetSimulation = true;
	}

	// Token: 0x06006AA0 RID: 27296 RVA: 0x00145B77 File Offset: 0x00143F77
	protected virtual void Update()
	{
		if (Application.isPlaying)
		{
			this.CheckResumeSimulation();
		}
	}

	// Token: 0x04005C8B RID: 23691
	protected bool _resetSimulation;

	// Token: 0x04005C8C RID: 23692
	protected bool _freezeSimulation;

	// Token: 0x04005C8D RID: 23693
	protected bool _collisionEnabled;

	// Token: 0x04005C8E RID: 23694
	protected bool _useInterpolation;

	// Token: 0x04005C8F RID: 23695
	protected int _solverIterations = 15;

	// Token: 0x04005C90 RID: 23696
	protected List<AsyncFlag> waitResumeSimulationFlags;
}
