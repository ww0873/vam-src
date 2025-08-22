using System;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002C6 RID: 710
	public sealed class CodeUI : MonoBehaviour
	{
		// Token: 0x0600106A RID: 4202 RVA: 0x0005C4C6 File Offset: 0x0005A8C6
		public CodeUI()
		{
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0005C4CE File Offset: 0x0005A8CE
		public void Start()
		{
			this.OnNewClicked();
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0005C4D6 File Offset: 0x0005A8D6
		public void OnNewClicked()
		{
			if (CodeUI.onNewClicked != null)
			{
				CodeUI.onNewClicked(this);
			}
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0005C4ED File Offset: 0x0005A8ED
		public void OnExampleClicked()
		{
			if (CodeUI.onLoadClicked != null)
			{
				CodeUI.onLoadClicked(this);
			}
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0005C504 File Offset: 0x0005A904
		public void OnShowHelpClicked()
		{
			this.helpObject.SetActive(true);
			this.codeEditorObject.SetActive(false);
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0005C51E File Offset: 0x0005A91E
		public void OnHideHelpClicked()
		{
			this.helpObject.SetActive(false);
			this.codeEditorObject.SetActive(true);
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0005C538 File Offset: 0x0005A938
		public void OnRunClicked()
		{
			if (CodeUI.onCompileClicked != null)
			{
				CodeUI.onCompileClicked(this);
			}
		}

		// Token: 0x04000E97 RID: 3735
		public static Action<CodeUI> onNewClicked;

		// Token: 0x04000E98 RID: 3736
		public static Action<CodeUI> onLoadClicked;

		// Token: 0x04000E99 RID: 3737
		public static Action<CodeUI> onCompileClicked;

		// Token: 0x04000E9A RID: 3738
		public GameObject codeEditorObject;

		// Token: 0x04000E9B RID: 3739
		public GameObject helpObject;

		// Token: 0x04000E9C RID: 3740
		public InputField codeEditor;
	}
}
