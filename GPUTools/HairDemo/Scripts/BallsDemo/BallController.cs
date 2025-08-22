using System;
using UnityEngine;

namespace GPUTools.HairDemo.Scripts.BallsDemo
{
	// Token: 0x020009E5 RID: 2533
	public class BallController : MonoBehaviour
	{
		// Token: 0x06003FCB RID: 16331 RVA: 0x001303DE File Offset: 0x0012E7DE
		public BallController()
		{
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x001303E6 File Offset: 0x0012E7E6
		private void Start()
		{
			this.body = base.GetComponent<Rigidbody>();
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x001303F4 File Offset: 0x0012E7F4
		private void Update()
		{
			if (Input.GetKey(KeyCode.A))
			{
				this.body.velocity += Vector3.left;
			}
			if (Input.GetKey(KeyCode.D))
			{
				this.body.velocity += Vector3.right;
			}
			if (Input.GetKey(KeyCode.W))
			{
				this.body.velocity += Vector3.forward;
			}
			if (Input.GetKey(KeyCode.S))
			{
				this.body.velocity += Vector3.back;
			}
			this.body.velocity = Vector3.ClampMagnitude(this.body.velocity, 2f);
		}

		// Token: 0x04003035 RID: 12341
		private Rigidbody body;
	}
}
