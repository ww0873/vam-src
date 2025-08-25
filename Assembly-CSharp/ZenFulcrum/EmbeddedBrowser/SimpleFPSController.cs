using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005A9 RID: 1449
	[RequireComponent(typeof(Rigidbody))]
	public class SimpleFPSController : MonoBehaviour
	{
		// Token: 0x06002458 RID: 9304 RVA: 0x000D1E5C File Offset: 0x000D025C
		public SimpleFPSController()
		{
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000D1EC0 File Offset: 0x000D02C0
		public void Awake()
		{
			this.head = base.GetComponentInChildren<Camera>();
			this.body = base.GetComponent<Rigidbody>();
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000D1EDC File Offset: 0x000D02DC
		public void Update()
		{
			Vector2 vector = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * this.lookSpeed;
			Quaternion rhs = Quaternion.AngleAxis(vector.x, Vector3.up);
			base.transform.localRotation *= rhs;
			this.lookPitch += -vector.y;
			this.lookPitch = Mathf.Clamp(this.lookPitch, -90f, 90f);
			this.head.transform.localRotation = Quaternion.Euler(this.lookPitch, 0f, 0f);
			if (Input.GetButtonDown("Jump") && this.Grounded)
			{
				this.body.AddForce(-Physics.gravity.normalized * this.jumpForce, ForceMode.Impulse);
			}
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000D1FD0 File Offset: 0x000D03D0
		public void FixedUpdate()
		{
			if (Time.frameCount < 5)
			{
				return;
			}
			bool grounded = this.Grounded;
			if (grounded)
			{
				this.body.drag = this.dampening;
			}
			else
			{
				this.body.drag = 0f;
			}
			Vector3 velocity = this.body.velocity;
			velocity.y = 0f;
			Vector3 vector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
			if (vector.magnitude > 1f)
			{
				vector = vector.normalized;
			}
			if (velocity.magnitude > this.moveSpeed)
			{
				return;
			}
			vector = base.transform.TransformVector(vector);
			Vector3 vector2 = vector * this.moveForce * Time.deltaTime;
			if (vector2.magnitude > 0f)
			{
				if (!grounded)
				{
					vector2 *= 0.5f;
				}
				this.body.AddForce(vector2, ForceMode.Force);
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x000D20D4 File Offset: 0x000D04D4
		public bool Grounded
		{
			get
			{
				RaycastHit[] array = Physics.SphereCastAll(new Ray(base.transform.position + this.bottom + base.transform.up * 0.01f, Physics.gravity.normalized), 0.1f, 0.1f);
				for (int i = 0; i < array.Length; i++)
				{
					if (!(array[i].rigidbody == this.body))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x04001E8E RID: 7822
		public float lookSpeed = 100f;

		// Token: 0x04001E8F RID: 7823
		public float moveSpeed = 10f;

		// Token: 0x04001E90 RID: 7824
		public float moveForce = 20000f;

		// Token: 0x04001E91 RID: 7825
		public float jumpForce = 50f;

		// Token: 0x04001E92 RID: 7826
		public float dampening = 2f;

		// Token: 0x04001E93 RID: 7827
		private Vector3 bottom = new Vector3(0f, -1f, 0f);

		// Token: 0x04001E94 RID: 7828
		private Camera head;

		// Token: 0x04001E95 RID: 7829
		private Rigidbody body;

		// Token: 0x04001E96 RID: 7830
		private float lookPitch;
	}
}
