using System;
using Battlehub.Cubeman;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000EC RID: 236
	public class EditorCharacter : MonoBehaviour
	{
		// Token: 0x060004F9 RID: 1273 RVA: 0x0001BA38 File Offset: 0x00019E38
		public EditorCharacter()
		{
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001BA40 File Offset: 0x00019E40
		private void Start()
		{
			this.m_character = base.GetComponent<CubemanCharacter>();
			this.m_rigidBody = base.GetComponent<Rigidbody>();
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001BA5C File Offset: 0x00019E5C
		private void Update()
		{
			if (InputController.GetKeyDown(KeyCode.V))
			{
				this.m_isKinematic = this.m_rigidBody.isKinematic;
				this.m_rigidBody.isKinematic = true;
				this.m_isEnabled = this.m_character.Enabled;
				this.m_character.Enabled = false;
			}
			else if (InputController.GetKeyUp(KeyCode.V))
			{
				this.m_rigidBody.isKinematic = this.m_isKinematic;
				this.m_character.Enabled = this.m_isEnabled;
			}
			if (base.transform.position.y < -5000f)
			{
				this.m_rigidBody.isKinematic = true;
				this.m_character.Enabled = false;
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001BB17 File Offset: 0x00019F17
		public void OnSelected(ExposeToEditor obj)
		{
			if (EditorDemo.Instance != null && EditorDemo.Instance.EnableCharacters)
			{
				this.EnableCharacter(obj.gameObject);
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001BB44 File Offset: 0x00019F44
		private void EnableCharacter(GameObject obj)
		{
			if (!this.m_rigidBody)
			{
				return;
			}
			this.m_rigidBody.isKinematic = false;
			this.m_character.Enabled = true;
			CubemanUserControl component = obj.GetComponent<CubemanUserControl>();
			if (component != null)
			{
				component.HandleInput = true;
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001BB94 File Offset: 0x00019F94
		public void OnUnselected(ExposeToEditor obj)
		{
			Rigidbody component = obj.GetComponent<Rigidbody>();
			if (component)
			{
				component.isKinematic = true;
			}
			CubemanCharacter component2 = obj.GetComponent<CubemanCharacter>();
			if (component2 != null)
			{
				component2.Move(Vector3.zero, false, false);
				component2.Enabled = false;
			}
			CubemanUserControl component3 = obj.GetComponent<CubemanUserControl>();
			if (component3 != null)
			{
				component3.HandleInput = false;
			}
		}

		// Token: 0x04000490 RID: 1168
		private Rigidbody m_rigidBody;

		// Token: 0x04000491 RID: 1169
		private CubemanCharacter m_character;

		// Token: 0x04000492 RID: 1170
		private bool m_isKinematic;

		// Token: 0x04000493 RID: 1171
		private bool m_isEnabled;
	}
}
