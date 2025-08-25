using System;
using Obi;
using UnityEngine;

// Token: 0x0200036F RID: 879
public class CraneController : MonoBehaviour
{
	// Token: 0x060015E9 RID: 5609 RVA: 0x0007D0E4 File Offset: 0x0007B4E4
	public CraneController()
	{
	}

	// Token: 0x060015EA RID: 5610 RVA: 0x0007D0EC File Offset: 0x0007B4EC
	private void Start()
	{
		this.cursor = base.GetComponentInChildren<ObiRopeCursor>();
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x0007D0FC File Offset: 0x0007B4FC
	private void Update()
	{
		if (Input.GetKey(KeyCode.W) && this.cursor.rope.RestLength > 6.5f)
		{
			this.cursor.ChangeLength(this.cursor.rope.RestLength - 1f * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S))
		{
			this.cursor.ChangeLength(this.cursor.rope.RestLength + 1f * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.A))
		{
			base.transform.Rotate(0f, Time.deltaTime * 15f, 0f);
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.transform.Rotate(0f, -Time.deltaTime * 15f, 0f);
		}
	}

	// Token: 0x0400124D RID: 4685
	private ObiRopeCursor cursor;
}
