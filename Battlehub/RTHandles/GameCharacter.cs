using System;
using Battlehub.Cubeman;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000EF RID: 239
	[DisallowMultipleComponent]
	public class GameCharacter : MonoBehaviour
	{
		// Token: 0x06000540 RID: 1344 RVA: 0x0001D6FD File Offset: 0x0001BAFD
		public GameCharacter()
		{
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x0001D705 File Offset: 0x0001BB05
		// (set) Token: 0x06000542 RID: 1346 RVA: 0x0001D712 File Offset: 0x0001BB12
		public Transform Camera
		{
			get
			{
				return this.m_userControl.Cam;
			}
			set
			{
				this.m_userControl.Cam = value;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x0001D720 File Offset: 0x0001BB20
		// (set) Token: 0x06000544 RID: 1348 RVA: 0x0001D72D File Offset: 0x0001BB2D
		public bool IsActive
		{
			get
			{
				return this.m_userControl.HandleInput;
			}
			set
			{
				this.m_userControl.HandleInput = value;
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001D73B File Offset: 0x0001BB3B
		private void Awake()
		{
			this.m_userControl = base.GetComponent<CubemanUserControl>();
			this.m_soul = base.transform.Find("Soul");
			this.m_skinnedMeshRenderer = base.GetComponentInChildren<SkinnedMeshRenderer>();
			this.m_rigidBody = base.GetComponent<Rigidbody>();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001D778 File Offset: 0x0001BB78
		private void Update()
		{
			if (base.transform.position.y < -10f)
			{
				this.Die();
			}
			if (InputController.GetKeyDown(KeyCode.K))
			{
				this.Die();
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001D7BC File Offset: 0x0001BBBC
		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Finish")
			{
				this.Game.OnPlayerFinish(this);
				if (this.m_skinnedMeshRenderer != null)
				{
					this.m_skinnedMeshRenderer.enabled = false;
				}
				if (this.m_soul != null)
				{
					this.m_soul.gameObject.SetActive(true);
				}
				UnityEngine.Object.Destroy(base.gameObject, 2f);
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001D83C File Offset: 0x0001BC3C
		private void Die()
		{
			this.Game.OnPlayerDie(this);
			if (this.m_skinnedMeshRenderer != null)
			{
				this.m_skinnedMeshRenderer.enabled = false;
			}
			if (this.m_rigidBody != null)
			{
				this.m_rigidBody.isKinematic = true;
			}
			if (this.m_userControl != null)
			{
				this.m_userControl.HandleInput = false;
			}
			if (this.m_soul != null)
			{
				this.m_soul.gameObject.SetActive(true);
			}
			UnityEngine.Object.Destroy(base.gameObject, 2f);
		}

		// Token: 0x040004DC RID: 1244
		public CubemenGame Game;

		// Token: 0x040004DD RID: 1245
		private Rigidbody m_rigidBody;

		// Token: 0x040004DE RID: 1246
		private CubemanUserControl m_userControl;

		// Token: 0x040004DF RID: 1247
		private Transform m_soul;

		// Token: 0x040004E0 RID: 1248
		private SkinnedMeshRenderer m_skinnedMeshRenderer;
	}
}
