using System;
using Obi;
using UnityEngine;

// Token: 0x02000384 RID: 900
public class CursorController : MonoBehaviour
{
	// Token: 0x0600167A RID: 5754 RVA: 0x0007E351 File Offset: 0x0007C751
	public CursorController()
	{
	}

	// Token: 0x0600167B RID: 5755 RVA: 0x0007E364 File Offset: 0x0007C764
	private void Start()
	{
		this.cursor = base.GetComponentInChildren<ObiRopeCursor>();
	}

	// Token: 0x0600167C RID: 5756 RVA: 0x0007E374 File Offset: 0x0007C774
	private void Update()
	{
		if (Input.GetKey(KeyCode.W) && this.cursor.rope.RestLength > this.minLength)
		{
			this.cursor.ChangeLength(this.cursor.rope.RestLength - 1f * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S))
		{
			this.cursor.ChangeLength(this.cursor.rope.RestLength + 1f * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.A))
		{
			this.cursor.rope.transform.Translate(Vector3.left * Time.deltaTime, Space.World);
		}
		if (Input.GetKey(KeyCode.D))
		{
			this.cursor.rope.transform.Translate(Vector3.right * Time.deltaTime, Space.World);
		}
	}

	// Token: 0x0400129E RID: 4766
	private ObiRopeCursor cursor;

	// Token: 0x0400129F RID: 4767
	public float minLength = 0.1f;
}
