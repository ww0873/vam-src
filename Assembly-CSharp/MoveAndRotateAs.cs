using System;
using UnityEngine;

// Token: 0x02000BB4 RID: 2996
public class MoveAndRotateAs : MonoBehaviour
{
	// Token: 0x0600556A RID: 21866 RVA: 0x001F401F File Offset: 0x001F241F
	public MoveAndRotateAs()
	{
	}

	// Token: 0x0600556B RID: 21867 RVA: 0x001F4038 File Offset: 0x001F2438
	public void DoUpdate()
	{
		if (this.MoveAndRotateAsTransform)
		{
			if (this.MoveAsEnabled)
			{
				if (this.useRigidbody)
				{
					Rigidbody component = base.transform.GetComponent<Rigidbody>();
					if (component != null)
					{
						component.MovePosition(this.MoveAndRotateAsTransform.position + this.Offset);
					}
				}
				else
				{
					base.transform.position = this.MoveAndRotateAsTransform.position + this.Offset;
				}
			}
			if (this.RotateAsEnabled)
			{
				if (this.useRigidbody)
				{
					Rigidbody component2 = base.transform.GetComponent<Rigidbody>();
					if (component2 != null)
					{
						component2.MoveRotation(this.MoveAndRotateAsTransform.rotation);
					}
				}
				else
				{
					base.transform.rotation = this.MoveAndRotateAsTransform.rotation;
				}
			}
		}
	}

	// Token: 0x0600556C RID: 21868 RVA: 0x001F411F File Offset: 0x001F251F
	private void OnEnable()
	{
		this.DoUpdate();
	}

	// Token: 0x0600556D RID: 21869 RVA: 0x001F4127 File Offset: 0x001F2527
	private void Start()
	{
		this.DoUpdate();
	}

	// Token: 0x0600556E RID: 21870 RVA: 0x001F412F File Offset: 0x001F252F
	private void FixedUpdate()
	{
		if (this.updateTime == MoveAndRotateAs.UpdateTime.Fixed)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x0600556F RID: 21871 RVA: 0x001F4143 File Offset: 0x001F2543
	private void Update()
	{
		if (this.updateTime == MoveAndRotateAs.UpdateTime.Normal)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x06005570 RID: 21872 RVA: 0x001F4156 File Offset: 0x001F2556
	private void LateUpdate()
	{
		if (this.updateTime == MoveAndRotateAs.UpdateTime.Late)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x04004684 RID: 18052
	public Transform MoveAndRotateAsTransform;

	// Token: 0x04004685 RID: 18053
	public Vector3 Offset;

	// Token: 0x04004686 RID: 18054
	public bool MoveAsEnabled = true;

	// Token: 0x04004687 RID: 18055
	public bool RotateAsEnabled = true;

	// Token: 0x04004688 RID: 18056
	public bool useRigidbody;

	// Token: 0x04004689 RID: 18057
	public MoveAndRotateAs.UpdateTime updateTime;

	// Token: 0x02000BB5 RID: 2997
	public enum UpdateTime
	{
		// Token: 0x0400468B RID: 18059
		Normal,
		// Token: 0x0400468C RID: 18060
		Late,
		// Token: 0x0400468D RID: 18061
		Fixed,
		// Token: 0x0400468E RID: 18062
		Batch
	}
}
