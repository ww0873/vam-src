using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Obi.CrossPlatformInput
{
	// Token: 0x02000370 RID: 880
	public class AxisTouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x060015EC RID: 5612 RVA: 0x0007D1E2 File Offset: 0x0007B5E2
		public AxisTouchButton()
		{
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0007D218 File Offset: 0x0007B618
		private void OnEnable()
		{
			if (!CrossPlatformInputManager.AxisExists(this.axisName))
			{
				this.m_Axis = new CrossPlatformInputManager.VirtualAxis(this.axisName);
				CrossPlatformInputManager.RegisterVirtualAxis(this.m_Axis);
			}
			else
			{
				this.m_Axis = CrossPlatformInputManager.VirtualAxisReference(this.axisName);
			}
			this.FindPairedButton();
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x0007D270 File Offset: 0x0007B670
		private void FindPairedButton()
		{
			AxisTouchButton[] array = UnityEngine.Object.FindObjectsOfType(typeof(AxisTouchButton)) as AxisTouchButton[];
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].axisName == this.axisName && array[i] != this)
					{
						this.m_PairedWith = array[i];
					}
				}
			}
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x0007D2DB File Offset: 0x0007B6DB
		private void OnDisable()
		{
			this.m_Axis.Remove();
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x0007D2E8 File Offset: 0x0007B6E8
		public void OnPointerDown(PointerEventData data)
		{
			if (this.m_PairedWith == null)
			{
				this.FindPairedButton();
			}
			this.m_Axis.Update(Mathf.MoveTowards(this.m_Axis.GetValue, this.axisValue, this.responseSpeed * Time.deltaTime));
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x0007D339 File Offset: 0x0007B739
		public void OnPointerUp(PointerEventData data)
		{
			this.m_Axis.Update(Mathf.MoveTowards(this.m_Axis.GetValue, 0f, this.responseSpeed * Time.deltaTime));
		}

		// Token: 0x0400124E RID: 4686
		public string axisName = "Horizontal";

		// Token: 0x0400124F RID: 4687
		public float axisValue = 1f;

		// Token: 0x04001250 RID: 4688
		public float responseSpeed = 3f;

		// Token: 0x04001251 RID: 4689
		public float returnToCentreSpeed = 3f;

		// Token: 0x04001252 RID: 4690
		private AxisTouchButton m_PairedWith;

		// Token: 0x04001253 RID: 4691
		private CrossPlatformInputManager.VirtualAxis m_Axis;
	}
}
