using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200054C RID: 1356
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("UI/Extensions/PPIViewer")]
	public class PPIViewer : MonoBehaviour
	{
		// Token: 0x0600228F RID: 8847 RVA: 0x000C5451 File Offset: 0x000C3851
		public PPIViewer()
		{
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x000C5459 File Offset: 0x000C3859
		private void Awake()
		{
			this.label = base.GetComponent<Text>();
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x000C5468 File Offset: 0x000C3868
		private void Start()
		{
			if (this.label != null)
			{
				this.label.text = "PPI: " + Screen.dpi.ToString();
			}
		}

		// Token: 0x04001C9A RID: 7322
		private Text label;
	}
}
