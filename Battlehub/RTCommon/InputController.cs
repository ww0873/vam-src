using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battlehub.RTCommon
{
	// Token: 0x020000B8 RID: 184
	public class InputController : MonoBehaviour
	{
		// Token: 0x060002FE RID: 766 RVA: 0x00013FC6 File Offset: 0x000123C6
		public InputController()
		{
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00013FCE File Offset: 0x000123CE
		public static InputController Instance
		{
			get
			{
				return InputController.m_instance;
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00013FD5 File Offset: 0x000123D5
		public static bool GetKeyDown(KeyCode key)
		{
			return (!(InputController.m_instance != null) || !InputController.m_instance.m_isInputFieldSelected) && Input.GetKeyDown(key);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00013FFE File Offset: 0x000123FE
		public static bool GetKeyUp(KeyCode key)
		{
			return (!(InputController.m_instance != null) || !InputController.m_instance.m_isInputFieldSelected) && Input.GetKeyUp(key);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00014027 File Offset: 0x00012427
		public static bool GetKey(KeyCode key)
		{
			return (!(InputController.m_instance != null) || !InputController.m_instance.m_isInputFieldSelected) && Input.GetKey(key);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00014050 File Offset: 0x00012450
		private void Awake()
		{
			if (InputController.m_instance != null)
			{
				Debug.LogWarning("Another instance of InputController exists");
			}
			InputController.m_instance = this;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00014072 File Offset: 0x00012472
		private void OnDestroy()
		{
			if (InputController.m_instance == this)
			{
				InputController.m_instance = null;
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0001408C File Offset: 0x0001248C
		private void Update()
		{
			if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != this.m_selectedGameObject)
			{
				this.m_selectedGameObject = EventSystem.current.currentSelectedGameObject;
				this.m_isInputFieldSelected = (this.m_selectedGameObject != null && this.m_selectedGameObject.GetComponent<InputField>() != null);
			}
		}

		// Token: 0x040003AC RID: 940
		private static InputController m_instance;

		// Token: 0x040003AD RID: 941
		private bool m_isInputFieldSelected;

		// Token: 0x040003AE RID: 942
		private GameObject m_selectedGameObject;
	}
}
