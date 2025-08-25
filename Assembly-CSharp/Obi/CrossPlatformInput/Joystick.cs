using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Obi.CrossPlatformInput
{
	// Token: 0x02000377 RID: 887
	public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x0600162C RID: 5676 RVA: 0x0007D684 File Offset: 0x0007BA84
		public Joystick()
		{
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x0007D6AA File Offset: 0x0007BAAA
		private void OnEnable()
		{
			this.CreateVirtualAxes();
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x0007D6B2 File Offset: 0x0007BAB2
		private void Start()
		{
			this.m_StartPos = base.transform.position;
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x0007D6C8 File Offset: 0x0007BAC8
		private void UpdateVirtualAxes(Vector3 value)
		{
			Vector3 a = this.m_StartPos - value;
			a.y = -a.y;
			a /= (float)this.MovementRange;
			if (this.m_UseX)
			{
				this.m_HorizontalVirtualAxis.Update(-a.x);
			}
			if (this.m_UseY)
			{
				this.m_VerticalVirtualAxis.Update(a.y);
			}
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x0007D73C File Offset: 0x0007BB3C
		private void CreateVirtualAxes()
		{
			this.m_UseX = (this.axesToUse == Joystick.AxisOption.Both || this.axesToUse == Joystick.AxisOption.OnlyHorizontal);
			this.m_UseY = (this.axesToUse == Joystick.AxisOption.Both || this.axesToUse == Joystick.AxisOption.OnlyVertical);
			if (this.m_UseX)
			{
				this.m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(this.horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(this.m_HorizontalVirtualAxis);
			}
			if (this.m_UseY)
			{
				this.m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(this.verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(this.m_VerticalVirtualAxis);
			}
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0007D7D4 File Offset: 0x0007BBD4
		public void OnDrag(PointerEventData data)
		{
			Vector3 zero = Vector3.zero;
			if (this.m_UseX)
			{
				int num = (int)(data.position.x - this.m_StartPos.x);
				num = Mathf.Clamp(num, -this.MovementRange, this.MovementRange);
				zero.x = (float)num;
			}
			if (this.m_UseY)
			{
				int num2 = (int)(data.position.y - this.m_StartPos.y);
				num2 = Mathf.Clamp(num2, -this.MovementRange, this.MovementRange);
				zero.y = (float)num2;
			}
			base.transform.position = new Vector3(this.m_StartPos.x + zero.x, this.m_StartPos.y + zero.y, this.m_StartPos.z + zero.z);
			this.UpdateVirtualAxes(base.transform.position);
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x0007D8CA File Offset: 0x0007BCCA
		public void OnPointerUp(PointerEventData data)
		{
			base.transform.position = this.m_StartPos;
			this.UpdateVirtualAxes(this.m_StartPos);
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x0007D8E9 File Offset: 0x0007BCE9
		public void OnPointerDown(PointerEventData data)
		{
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x0007D8EB File Offset: 0x0007BCEB
		private void OnDisable()
		{
			if (this.m_UseX)
			{
				this.m_HorizontalVirtualAxis.Remove();
			}
			if (this.m_UseY)
			{
				this.m_VerticalVirtualAxis.Remove();
			}
		}

		// Token: 0x04001264 RID: 4708
		public int MovementRange = 100;

		// Token: 0x04001265 RID: 4709
		public Joystick.AxisOption axesToUse;

		// Token: 0x04001266 RID: 4710
		public string horizontalAxisName = "Horizontal";

		// Token: 0x04001267 RID: 4711
		public string verticalAxisName = "Vertical";

		// Token: 0x04001268 RID: 4712
		private Vector3 m_StartPos;

		// Token: 0x04001269 RID: 4713
		private bool m_UseX;

		// Token: 0x0400126A RID: 4714
		private bool m_UseY;

		// Token: 0x0400126B RID: 4715
		private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;

		// Token: 0x0400126C RID: 4716
		private CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

		// Token: 0x02000378 RID: 888
		public enum AxisOption
		{
			// Token: 0x0400126E RID: 4718
			Both,
			// Token: 0x0400126F RID: 4719
			OnlyHorizontal,
			// Token: 0x04001270 RID: 4720
			OnlyVertical
		}
	}
}
