using System;
using UnityEngine;

// Token: 0x02000BBC RID: 3004
public class MoveAsOtherAndLookAt : MonoBehaviour
{
	// Token: 0x06005598 RID: 21912 RVA: 0x001F498B File Offset: 0x001F2D8B
	public MoveAsOtherAndLookAt()
	{
	}

	// Token: 0x06005599 RID: 21913 RVA: 0x001F499A File Offset: 0x001F2D9A
	private void Start()
	{
	}

	// Token: 0x0600559A RID: 21914 RVA: 0x001F499C File Offset: 0x001F2D9C
	private void LateUpdate()
	{
		if (this.LookAtTransform)
		{
			if (this.MoveAsEnabled && (this.MoveAsTransform || this.MoveAsFreeControllerV3))
			{
				Vector3 vector;
				if (this.MoveAsFreeControllerV3)
				{
					vector = this.MoveAsFreeControllerV3.selectedPosition;
				}
				else
				{
					vector = this.MoveAsTransform.position;
				}
				if (this.MoveToLookHeight)
				{
					vector.y = this.LookAtTransform.position.y;
				}
				if (this.debug)
				{
					MyDebug.DrawWireCube(vector, 0.1f, Color.blue);
				}
				if (this.lookAtTime == MoveAsOtherAndLookAt.LookAtTime.LookAtFirst)
				{
					base.transform.position = vector;
					base.transform.LookAt(this.LookAtTransform);
				}
				Vector3 vector2 = this.LookAtTransform.position - vector;
				float magnitude = vector2.magnitude;
				vector2.Normalize();
				Vector3 a = Vector3.Cross(Vector3.up, vector2);
				a.Normalize();
				vector = vector + Vector3.up * this.UpDownOffsetBeforeMove + a * this.LeftRightOffsetBeforeMove + a * magnitude * this.LeftRightOffsetRelativeToDistanceBeforeMove;
				if (this.debug)
				{
					MyDebug.DrawWireCube(vector, 0.1f, Color.red);
				}
				if (this.lookAtTime == MoveAsOtherAndLookAt.LookAtTime.LookAtBeforeMove || this.lookAtTime == MoveAsOtherAndLookAt.LookAtTime.LookAtBeforeAndAfterMove)
				{
					vector2 = this.LookAtTransform.position - vector;
					vector2.Normalize();
					a = Vector3.Cross(Vector3.up, vector2);
					a.Normalize();
					base.transform.position = vector;
					base.transform.LookAt(this.LookAtTransform);
				}
				if (this.MoveSpecificDistanceFromLookAt)
				{
					float num = Vector3.Distance(this.LookAtTransform.position, vector);
					vector = vector + vector2 * (num - this.MoveTowardsLookAtDistance) + Vector3.up * this.UpDownOffsetAfterMove + a * this.LeftRightOffsetAfterMove;
				}
				else
				{
					vector = vector + vector2 * this.MoveTowardsLookAtDistance + Vector3.up * this.UpDownOffsetAfterMove + a * this.LeftRightOffsetAfterMove;
				}
				base.transform.position = vector;
				if (this.lookAtTime == MoveAsOtherAndLookAt.LookAtTime.LookAtAfterMove || this.lookAtTime == MoveAsOtherAndLookAt.LookAtTime.LookAtBeforeAndAfterMove)
				{
					base.transform.LookAt(this.LookAtTransform);
				}
			}
			else
			{
				base.transform.LookAt(this.LookAtTransform);
			}
		}
	}

	// Token: 0x040046B3 RID: 18099
	public MoveAsOtherAndLookAt.LookAtTime lookAtTime;

	// Token: 0x040046B4 RID: 18100
	public Transform MoveAsTransform;

	// Token: 0x040046B5 RID: 18101
	public FreeControllerV3 MoveAsFreeControllerV3;

	// Token: 0x040046B6 RID: 18102
	public Transform LookAtTransform;

	// Token: 0x040046B7 RID: 18103
	public bool MoveToLookHeight;

	// Token: 0x040046B8 RID: 18104
	public float LeftRightOffsetBeforeMove;

	// Token: 0x040046B9 RID: 18105
	public float LeftRightOffsetRelativeToDistanceBeforeMove;

	// Token: 0x040046BA RID: 18106
	public float UpDownOffsetBeforeMove;

	// Token: 0x040046BB RID: 18107
	public float MoveTowardsLookAtDistance;

	// Token: 0x040046BC RID: 18108
	public float LeftRightOffsetAfterMove;

	// Token: 0x040046BD RID: 18109
	public float UpDownOffsetAfterMove;

	// Token: 0x040046BE RID: 18110
	public bool MoveSpecificDistanceFromLookAt;

	// Token: 0x040046BF RID: 18111
	public bool MoveAsEnabled = true;

	// Token: 0x040046C0 RID: 18112
	public bool debug;

	// Token: 0x02000BBD RID: 3005
	public enum LookAtTime
	{
		// Token: 0x040046C2 RID: 18114
		LookAtFirst,
		// Token: 0x040046C3 RID: 18115
		LookAtBeforeMove,
		// Token: 0x040046C4 RID: 18116
		LookAtAfterMove,
		// Token: 0x040046C5 RID: 18117
		LookAtBeforeAndAfterMove
	}
}
