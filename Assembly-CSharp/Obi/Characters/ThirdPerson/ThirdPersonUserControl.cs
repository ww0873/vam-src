using System;
using Obi.CrossPlatformInput;
using UnityEngine;

namespace Obi.Characters.ThirdPerson
{
	// Token: 0x0200038D RID: 909
	[RequireComponent(typeof(ThirdPersonCharacter))]
	public class ThirdPersonUserControl : MonoBehaviour
	{
		// Token: 0x0600169F RID: 5791 RVA: 0x0007F4DC File Offset: 0x0007D8DC
		public ThirdPersonUserControl()
		{
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x0007F4E4 File Offset: 0x0007D8E4
		private void Start()
		{
			if (Camera.main != null)
			{
				this.m_Cam = Camera.main.transform;
			}
			else
			{
				Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
			}
			this.m_Character = base.GetComponent<ThirdPersonCharacter>();
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x0007F521 File Offset: 0x0007D921
		private void Update()
		{
			if (!this.m_Jump)
			{
				this.m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x0007F540 File Offset: 0x0007D940
		private void FixedUpdate()
		{
			float axis = CrossPlatformInputManager.GetAxis("Horizontal");
			float axis2 = CrossPlatformInputManager.GetAxis("Vertical");
			bool key = Input.GetKey(KeyCode.C);
			if (this.m_Cam != null)
			{
				this.m_CamForward = Vector3.Scale(this.m_Cam.forward, new Vector3(1f, 0f, 1f)).normalized;
				this.m_Move = axis2 * this.m_CamForward + axis * this.m_Cam.right;
			}
			else
			{
				this.m_Move = axis2 * Vector3.forward + axis * Vector3.right;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.m_Move *= 0.5f;
			}
			this.m_Character.Move(this.m_Move, key, this.m_Jump);
			this.m_Jump = false;
		}

		// Token: 0x040012CF RID: 4815
		private ThirdPersonCharacter m_Character;

		// Token: 0x040012D0 RID: 4816
		private Transform m_Cam;

		// Token: 0x040012D1 RID: 4817
		private Vector3 m_CamForward;

		// Token: 0x040012D2 RID: 4818
		private Vector3 m_Move;

		// Token: 0x040012D3 RID: 4819
		private bool m_Jump;
	}
}
