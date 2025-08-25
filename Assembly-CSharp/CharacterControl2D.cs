using System;
using UnityEngine;

// Token: 0x0200036C RID: 876
public class CharacterControl2D : MonoBehaviour
{
	// Token: 0x060015DB RID: 5595 RVA: 0x0007CE4F File Offset: 0x0007B24F
	public CharacterControl2D()
	{
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x0007CE6D File Offset: 0x0007B26D
	public void Awake()
	{
		this.rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060015DD RID: 5597 RVA: 0x0007CE7C File Offset: 0x0007B27C
	private void FixedUpdate()
	{
		this.rigidbody.AddForce(new Vector3(Input.GetAxis("Horizontal") * this.speed, 0f, 0f));
		if (Input.GetButtonDown("Jump"))
		{
			this.rigidbody.AddForce(Vector3.up * this.jumpPower, ForceMode.VelocityChange);
		}
	}

	// Token: 0x04001247 RID: 4679
	public float speed = 10f;

	// Token: 0x04001248 RID: 4680
	public float jumpPower = 2f;

	// Token: 0x04001249 RID: 4681
	private Rigidbody rigidbody;
}
