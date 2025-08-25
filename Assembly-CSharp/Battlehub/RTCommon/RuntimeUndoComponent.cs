using System;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000D8 RID: 216
	public class RuntimeUndoComponent : MonoBehaviour
	{
		// Token: 0x06000427 RID: 1063 RVA: 0x000178BB File Offset: 0x00015CBB
		public RuntimeUndoComponent()
		{
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x000178E9 File Offset: 0x00015CE9
		public KeyCode ModifierKey
		{
			get
			{
				return this.RuntimeModifierKey;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x000178F1 File Offset: 0x00015CF1
		public static bool IsInitialized
		{
			get
			{
				return RuntimeUndoComponent.m_instance != null;
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x000178FE File Offset: 0x00015CFE
		private void Awake()
		{
			if (RuntimeUndoComponent.m_instance == null)
			{
				RuntimeUndoComponent.m_instance = this;
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00017916 File Offset: 0x00015D16
		private void OnDestroy()
		{
			if (RuntimeUndoComponent.m_instance == this)
			{
				RuntimeUndoComponent.m_instance = null;
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00017930 File Offset: 0x00015D30
		private void Update()
		{
			if (InputController.GetKeyDown(this.UndoKey) && InputController.GetKey(this.ModifierKey))
			{
				RuntimeUndo.Undo();
			}
			else if (InputController.GetKeyDown(this.RedoKey) && InputController.GetKey(this.ModifierKey))
			{
				RuntimeUndo.Redo();
			}
		}

		// Token: 0x0400044B RID: 1099
		public KeyCode UndoKey = KeyCode.Z;

		// Token: 0x0400044C RID: 1100
		public KeyCode RedoKey = KeyCode.Y;

		// Token: 0x0400044D RID: 1101
		public KeyCode RuntimeModifierKey = KeyCode.LeftControl;

		// Token: 0x0400044E RID: 1102
		public KeyCode EditorModifierKey = KeyCode.LeftShift;

		// Token: 0x0400044F RID: 1103
		private static RuntimeUndoComponent m_instance;
	}
}
