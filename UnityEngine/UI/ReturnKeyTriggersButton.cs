using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	// Token: 0x0200054E RID: 1358
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Extensions/Return Key Trigger")]
	public class ReturnKeyTriggersButton : MonoBehaviour, ISubmitHandler, IEventSystemHandler
	{
		// Token: 0x06002293 RID: 8851 RVA: 0x000C54B6 File Offset: 0x000C38B6
		public ReturnKeyTriggersButton()
		{
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x000C54D0 File Offset: 0x000C38D0
		private void Start()
		{
			this._system = EventSystem.current;
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x000C54DD File Offset: 0x000C38DD
		private void RemoveHighlight()
		{
			this.button.OnPointerExit(new PointerEventData(this._system));
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x000C54F8 File Offset: 0x000C38F8
		public void OnSubmit(BaseEventData eventData)
		{
			if (this.highlight)
			{
				this.button.OnPointerEnter(new PointerEventData(this._system));
			}
			this.button.OnPointerClick(new PointerEventData(this._system));
			if (this.highlight)
			{
				base.Invoke("RemoveHighlight", this.highlightDuration);
			}
		}

		// Token: 0x04001C9B RID: 7323
		private EventSystem _system;

		// Token: 0x04001C9C RID: 7324
		public Button button;

		// Token: 0x04001C9D RID: 7325
		private bool highlight = true;

		// Token: 0x04001C9E RID: 7326
		public float highlightDuration = 0.2f;
	}
}
