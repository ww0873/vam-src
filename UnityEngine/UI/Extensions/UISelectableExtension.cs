using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000562 RID: 1378
	[AddComponentMenu("UI/Extensions/UI Selectable Extension")]
	[RequireComponent(typeof(Selectable))]
	public class UISelectableExtension : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x06002306 RID: 8966 RVA: 0x000C7E31 File Offset: 0x000C6231
		public UISelectableExtension()
		{
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x000C7E39 File Offset: 0x000C6239
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.OnButtonPress != null)
			{
				this.OnButtonPress.Invoke(eventData.button);
			}
			this._pressed = true;
			this._heldEventData = eventData;
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x000C7E65 File Offset: 0x000C6265
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (this.OnButtonRelease != null)
			{
				this.OnButtonRelease.Invoke(eventData.button);
			}
			this._pressed = false;
			this._heldEventData = null;
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x000C7E91 File Offset: 0x000C6291
		private void Update()
		{
			if (!this._pressed)
			{
				return;
			}
			if (this.OnButtonHeld != null)
			{
				this.OnButtonHeld.Invoke(this._heldEventData.button);
			}
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x000C7EC0 File Offset: 0x000C62C0
		public void TestClicked()
		{
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000C7EC2 File Offset: 0x000C62C2
		public void TestPressed()
		{
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x000C7EC4 File Offset: 0x000C62C4
		public void TestReleased()
		{
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x000C7EC6 File Offset: 0x000C62C6
		public void TestHold()
		{
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x000C7EC8 File Offset: 0x000C62C8
		private void OnDisable()
		{
			this._pressed = false;
		}

		// Token: 0x04001CFC RID: 7420
		[Tooltip("Event that fires when a button is initially pressed down")]
		public UISelectableExtension.UIButtonEvent OnButtonPress;

		// Token: 0x04001CFD RID: 7421
		[Tooltip("Event that fires when a button is released")]
		public UISelectableExtension.UIButtonEvent OnButtonRelease;

		// Token: 0x04001CFE RID: 7422
		[Tooltip("Event that continually fires while a button is held down")]
		public UISelectableExtension.UIButtonEvent OnButtonHeld;

		// Token: 0x04001CFF RID: 7423
		private bool _pressed;

		// Token: 0x04001D00 RID: 7424
		private PointerEventData _heldEventData;

		// Token: 0x02000563 RID: 1379
		[Serializable]
		public class UIButtonEvent : UnityEvent<PointerEventData.InputButton>
		{
			// Token: 0x0600230F RID: 8975 RVA: 0x000C7ED1 File Offset: 0x000C62D1
			public UIButtonEvent()
			{
			}
		}
	}
}
