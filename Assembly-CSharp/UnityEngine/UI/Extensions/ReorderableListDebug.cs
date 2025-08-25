using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004D4 RID: 1236
	public class ReorderableListDebug : MonoBehaviour
	{
		// Token: 0x06001F2E RID: 7982 RVA: 0x000B101B File Offset: 0x000AF41B
		public ReorderableListDebug()
		{
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x000B1024 File Offset: 0x000AF424
		private void Awake()
		{
			foreach (ReorderableList reorderableList in Object.FindObjectsOfType<ReorderableList>())
			{
				reorderableList.OnElementDropped.AddListener(new UnityAction<ReorderableList.ReorderableListEventStruct>(this.ElementDropped));
			}
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x000B1068 File Offset: 0x000AF468
		private void ElementDropped(ReorderableList.ReorderableListEventStruct droppedStruct)
		{
			this.DebugLabel.text = string.Empty;
			Text debugLabel = this.DebugLabel;
			debugLabel.text = debugLabel.text + "Dropped Object: " + droppedStruct.DroppedObject.name + "\n";
			Text debugLabel2 = this.DebugLabel;
			string text = debugLabel2.text;
			debugLabel2.text = string.Concat(new object[]
			{
				text,
				"Is Clone ?: ",
				droppedStruct.IsAClone,
				"\n"
			});
			if (droppedStruct.IsAClone)
			{
				Text debugLabel3 = this.DebugLabel;
				debugLabel3.text = debugLabel3.text + "Source Object: " + droppedStruct.SourceObject.name + "\n";
			}
			Text debugLabel4 = this.DebugLabel;
			debugLabel4.text += string.Format("From {0} at Index {1} \n", droppedStruct.FromList.name, droppedStruct.FromIndex);
			Text debugLabel5 = this.DebugLabel;
			debugLabel5.text += string.Format("To {0} at Index {1} \n", droppedStruct.ToList.name, droppedStruct.ToIndex);
		}

		// Token: 0x04001A57 RID: 6743
		public Text DebugLabel;
	}
}
