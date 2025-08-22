using System;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTHandles
{
	// Token: 0x020000EE RID: 238
	public class EditorHint : MonoBehaviour
	{
		// Token: 0x0600053E RID: 1342 RVA: 0x0001D5DB File Offset: 0x0001B9DB
		public EditorHint()
		{
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001D5E4 File Offset: 0x0001B9E4
		private void Start()
		{
			string empty = string.Empty;
			Text component = base.GetComponent<Text>();
			component.text = string.Concat(new object[]
			{
				"Right / Mid Mouse Button or Arrows - scene navigation\nMouse Wheel - zoom\n",
				this.EditorDemo.FocusKey,
				" - focus \n",
				empty,
				this.EditorDemo.ModifierKey,
				" + ",
				this.EditorDemo.SnapToGridKey,
				" - snap to grid \n",
				this.EditorDemo.ModifierKey,
				" + ",
				this.EditorDemo.DuplicateKey,
				" - duplicate object",
				this.EditorDemo.DeleteKey,
				" - delete object \n",
				this.EditorDemo.ModifierKey,
				" + ",
				this.EditorDemo.EnterPlayModeKey,
				" - toggle playmode \nQ W E R - select  handle \nX - toggle coordinate system \nTo create prefab click corresponding button"
			});
		}

		// Token: 0x040004DB RID: 1243
		[SerializeField]
		private EditorDemo EditorDemo;
	}
}
