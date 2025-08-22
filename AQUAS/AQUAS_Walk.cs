using System;
using UnityEngine;

namespace AQUAS
{
	// Token: 0x02000021 RID: 33
	public class AQUAS_Walk : MonoBehaviour
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x0000820A File Offset: 0x0000660A
		public AQUAS_Walk()
		{
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000821D File Offset: 0x0000661D
		private void Start()
		{
			if (this.m_controller == null)
			{
				this.m_controller = base.GetComponent<CharacterController>();
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000823C File Offset: 0x0000663C
		private void Update()
		{
			if (this.m_controller != null && this.m_controller.enabled)
			{
				Vector3 a = Input.GetAxis("Vertical") * base.transform.TransformDirection(Vector3.forward) * this.m_moveSpeed;
				this.m_controller.Move(a * Time.deltaTime);
				Vector3 a2 = Input.GetAxis("Horizontal") * base.transform.TransformDirection(Vector3.right) * this.m_moveSpeed;
				this.m_controller.Move(a2 * Time.deltaTime);
				this.m_controller.SimpleMove(Physics.gravity);
			}
		}

		// Token: 0x04000110 RID: 272
		public float m_moveSpeed = 10f;

		// Token: 0x04000111 RID: 273
		public CharacterController m_controller;
	}
}
