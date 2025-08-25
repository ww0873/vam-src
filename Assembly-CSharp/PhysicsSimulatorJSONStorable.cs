using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D85 RID: 3461
public class PhysicsSimulatorJSONStorable : ScaleChangeReceiverJSONStorable
{
	// Token: 0x06006AA1 RID: 27297 RVA: 0x001B4404 File Offset: 0x001B2804
	public PhysicsSimulatorJSONStorable()
	{
	}

	// Token: 0x06006AA2 RID: 27298 RVA: 0x001B4414 File Offset: 0x001B2814
	protected virtual void SyncResetSimulation()
	{
		this.SyncCollisionEnabled();
	}

	// Token: 0x17000FAA RID: 4010
	// (get) Token: 0x06006AA3 RID: 27299 RVA: 0x001B441C File Offset: 0x001B281C
	// (set) Token: 0x06006AA4 RID: 27300 RVA: 0x001B4424 File Offset: 0x001B2824
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

	// Token: 0x06006AA5 RID: 27301 RVA: 0x001B443F File Offset: 0x001B283F
	protected virtual void SyncFreezeSimulation()
	{
		this.SyncCollisionEnabled();
	}

	// Token: 0x17000FAB RID: 4011
	// (get) Token: 0x06006AA6 RID: 27302 RVA: 0x001B4447 File Offset: 0x001B2847
	// (set) Token: 0x06006AA7 RID: 27303 RVA: 0x001B444F File Offset: 0x001B284F
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

	// Token: 0x06006AA8 RID: 27304 RVA: 0x001B446A File Offset: 0x001B286A
	protected virtual void SyncCollisionEnabled()
	{
	}

	// Token: 0x17000FAC RID: 4012
	// (get) Token: 0x06006AA9 RID: 27305 RVA: 0x001B446C File Offset: 0x001B286C
	// (set) Token: 0x06006AAA RID: 27306 RVA: 0x001B4474 File Offset: 0x001B2874
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

	// Token: 0x06006AAB RID: 27307 RVA: 0x001B448F File Offset: 0x001B288F
	protected virtual void SyncUseInterpolation()
	{
	}

	// Token: 0x17000FAD RID: 4013
	// (get) Token: 0x06006AAC RID: 27308 RVA: 0x001B4491 File Offset: 0x001B2891
	// (set) Token: 0x06006AAD RID: 27309 RVA: 0x001B4499 File Offset: 0x001B2899
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

	// Token: 0x06006AAE RID: 27310 RVA: 0x001B44B4 File Offset: 0x001B28B4
	protected virtual void SyncSolverIterations()
	{
	}

	// Token: 0x17000FAE RID: 4014
	// (get) Token: 0x06006AAF RID: 27311 RVA: 0x001B44B6 File Offset: 0x001B28B6
	// (set) Token: 0x06006AB0 RID: 27312 RVA: 0x001B44BE File Offset: 0x001B28BE
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

	// Token: 0x06006AB1 RID: 27313 RVA: 0x001B44DC File Offset: 0x001B28DC
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

	// Token: 0x06006AB2 RID: 27314 RVA: 0x001B45F0 File Offset: 0x001B29F0
	public virtual bool IsSimulationResetting()
	{
		return this._resetSimulation;
	}

	// Token: 0x06006AB3 RID: 27315 RVA: 0x001B45F8 File Offset: 0x001B29F8
	public virtual void ResetSimulation(AsyncFlag waitFor)
	{
		if (this.waitResumeSimulationFlags == null)
		{
			this.waitResumeSimulationFlags = new List<AsyncFlag>();
		}
		this.waitResumeSimulationFlags.Add(waitFor);
		this.resetSimulation = true;
	}

	// Token: 0x06006AB4 RID: 27316 RVA: 0x001B4623 File Offset: 0x001B2A23
	protected virtual void Update()
	{
		if (Application.isPlaying)
		{
			this.CheckResumeSimulation();
		}
	}

	// Token: 0x04005C91 RID: 23697
	protected bool _resetSimulation;

	// Token: 0x04005C92 RID: 23698
	protected bool _freezeSimulation;

	// Token: 0x04005C93 RID: 23699
	protected bool _collisionEnabled;

	// Token: 0x04005C94 RID: 23700
	protected bool _useInterpolation;

	// Token: 0x04005C95 RID: 23701
	protected int _solverIterations = 15;

	// Token: 0x04005C96 RID: 23702
	protected List<AsyncFlag> waitResumeSimulationFlags;
}
