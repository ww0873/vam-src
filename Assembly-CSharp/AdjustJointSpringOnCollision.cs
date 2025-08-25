using System;
using UnityEngine;

// Token: 0x02000D44 RID: 3396
public class AdjustJointSpringOnCollision : MonoBehaviour
{
	// Token: 0x06006848 RID: 26696 RVA: 0x00271CB6 File Offset: 0x002700B6
	public AdjustJointSpringOnCollision()
	{
	}

	// Token: 0x06006849 RID: 26697 RVA: 0x00271CC0 File Offset: 0x002700C0
	private void OnCollisionEnter()
	{
		if (this.cj)
		{
			this.timerOn = false;
			JointDrive xDrive = this.cj.xDrive;
			xDrive.positionSpring = this.springForceOnCollision;
			this.cj.xDrive = xDrive;
		}
	}

	// Token: 0x0600684A RID: 26698 RVA: 0x00271D09 File Offset: 0x00270109
	private void restoreDrive()
	{
		this.cj.xDrive = this.saveJointDrive;
	}

	// Token: 0x0600684B RID: 26699 RVA: 0x00271D1C File Offset: 0x0027011C
	private void OnCollisionExit()
	{
		if (this.cj)
		{
			this.timerOn = true;
			this.timer = this.delay;
		}
	}

	// Token: 0x0600684C RID: 26700 RVA: 0x00271D41 File Offset: 0x00270141
	private void Start()
	{
		this.cj = base.GetComponent<ConfigurableJoint>();
		if (this.cj)
		{
			this.saveJointDrive = this.cj.xDrive;
		}
	}

	// Token: 0x0600684D RID: 26701 RVA: 0x00271D70 File Offset: 0x00270170
	private void Update()
	{
		if (this.timerOn)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				this.timerOn = false;
				this.restoreDrive();
			}
		}
	}

	// Token: 0x0400592C RID: 22828
	public float springForceOnCollision;

	// Token: 0x0400592D RID: 22829
	public float delay;

	// Token: 0x0400592E RID: 22830
	private ConfigurableJoint cj;

	// Token: 0x0400592F RID: 22831
	private JointDrive saveJointDrive;

	// Token: 0x04005930 RID: 22832
	private bool timerOn;

	// Token: 0x04005931 RID: 22833
	private float timer;
}
