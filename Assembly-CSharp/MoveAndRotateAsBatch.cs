using System;
using UnityEngine;

// Token: 0x02000BB6 RID: 2998
public class MoveAndRotateAsBatch : MonoBehaviour
{
	// Token: 0x06005571 RID: 21873 RVA: 0x001F416A File Offset: 0x001F256A
	public MoveAndRotateAsBatch()
	{
	}

	// Token: 0x06005572 RID: 21874 RVA: 0x001F4174 File Offset: 0x001F2574
	protected void DoUpdate()
	{
		foreach (MoveAndRotateAs moveAndRotateAs in this.comps)
		{
			moveAndRotateAs.DoUpdate();
		}
	}

	// Token: 0x06005573 RID: 21875 RVA: 0x001F41A6 File Offset: 0x001F25A6
	private void Awake()
	{
		this.comps = base.GetComponentsInChildren<MoveAndRotateAs>();
	}

	// Token: 0x06005574 RID: 21876 RVA: 0x001F41B4 File Offset: 0x001F25B4
	private void OnEnable()
	{
		this.DoUpdate();
	}

	// Token: 0x06005575 RID: 21877 RVA: 0x001F41BC File Offset: 0x001F25BC
	private void Start()
	{
		this.DoUpdate();
	}

	// Token: 0x06005576 RID: 21878 RVA: 0x001F41C4 File Offset: 0x001F25C4
	private void FixedUpdate()
	{
		if (this.updateTime == MoveAndRotateAsBatch.UpdateTime.Fixed)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x06005577 RID: 21879 RVA: 0x001F41D8 File Offset: 0x001F25D8
	private void Update()
	{
		if (this.updateTime == MoveAndRotateAsBatch.UpdateTime.Normal)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x06005578 RID: 21880 RVA: 0x001F41EB File Offset: 0x001F25EB
	private void LateUpdate()
	{
		if (this.updateTime == MoveAndRotateAsBatch.UpdateTime.Late)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x0400468F RID: 18063
	public MoveAndRotateAsBatch.UpdateTime updateTime;

	// Token: 0x04004690 RID: 18064
	protected MoveAndRotateAs[] comps;

	// Token: 0x02000BB7 RID: 2999
	public enum UpdateTime
	{
		// Token: 0x04004692 RID: 18066
		Normal,
		// Token: 0x04004693 RID: 18067
		Late,
		// Token: 0x04004694 RID: 18068
		Fixed
	}
}
