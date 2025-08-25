using System;
using UnityEngine;

namespace Battlehub.Cubeman
{
	// Token: 0x020000A3 RID: 163
	[RequireComponent(typeof(CubemanCharacter))]
	public class CubemanUserControl : MonoBehaviour
	{
		// Token: 0x0600026A RID: 618 RVA: 0x00011BBA File Offset: 0x0000FFBA
		public CubemanUserControl()
		{
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00011BC4 File Offset: 0x0000FFC4
		private void Start()
		{
			if (this.Cam == null)
			{
				if (Camera.main != null)
				{
					this.Cam = Camera.main.transform;
				}
				if (this.Cam == null)
				{
				}
			}
			this.m_Character = base.GetComponent<CubemanCharacter>();
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00011C1F File Offset: 0x0001001F
		private void Update()
		{
			if (!this.m_Jump)
			{
				this.m_Jump = (Input.GetKey(KeyCode.Space) && this.HandleInput);
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00011C48 File Offset: 0x00010048
		private void FixedUpdate()
		{
			float d = Input.GetAxis("Horizontal");
			float d2 = Input.GetAxis("Vertical");
			bool crouch = Input.GetKey(KeyCode.C);
			crouch = false;
			if (!this.HandleInput)
			{
				d2 = (d = 0f);
				crouch = false;
			}
			if (this.Cam != null)
			{
				this.m_CamForward = Vector3.Scale(this.Cam.forward, new Vector3(1f, 0f, 1f)).normalized;
				this.m_Move = d2 * this.m_CamForward + d * this.Cam.right;
			}
			else
			{
				this.m_Move = d2 * Vector3.forward + d * Vector3.right;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.m_Move *= 0.5f;
			}
			if (this.m_Character.enabled)
			{
				this.m_Character.Move(this.m_Move, crouch, this.m_Jump);
			}
			this.m_Jump = false;
		}

		// Token: 0x0400034E RID: 846
		public Transform Cam;

		// Token: 0x0400034F RID: 847
		private CubemanCharacter m_Character;

		// Token: 0x04000350 RID: 848
		private Vector3 m_CamForward;

		// Token: 0x04000351 RID: 849
		private Vector3 m_Move;

		// Token: 0x04000352 RID: 850
		private bool m_Jump;

		// Token: 0x04000353 RID: 851
		public bool HandleInput;
	}
}
