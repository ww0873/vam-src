using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Obi.CrossPlatformInput
{
	// Token: 0x02000380 RID: 896
	[RequireComponent(typeof(Image))]
	public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x06001659 RID: 5721 RVA: 0x0007E062 File Offset: 0x0007C462
		public TouchPad()
		{
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x0007E09D File Offset: 0x0007C49D
		private void OnEnable()
		{
			this.CreateVirtualAxes();
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0007E0A5 File Offset: 0x0007C4A5
		private void Start()
		{
			this.m_Image = base.GetComponent<Image>();
			this.m_Center = this.m_Image.transform.position;
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x0007E0CC File Offset: 0x0007C4CC
		private void CreateVirtualAxes()
		{
			this.m_UseX = (this.axesToUse == TouchPad.AxisOption.Both || this.axesToUse == TouchPad.AxisOption.OnlyHorizontal);
			this.m_UseY = (this.axesToUse == TouchPad.AxisOption.Both || this.axesToUse == TouchPad.AxisOption.OnlyVertical);
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

		// Token: 0x0600165D RID: 5725 RVA: 0x0007E164 File Offset: 0x0007C564
		private void UpdateVirtualAxes(Vector3 value)
		{
			value = value.normalized;
			if (this.m_UseX)
			{
				this.m_HorizontalVirtualAxis.Update(value.x);
			}
			if (this.m_UseY)
			{
				this.m_VerticalVirtualAxis.Update(value.y);
			}
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x0007E1B4 File Offset: 0x0007C5B4
		public void OnPointerDown(PointerEventData data)
		{
			this.m_Dragging = true;
			this.m_Id = data.pointerId;
			if (this.controlStyle != TouchPad.ControlStyle.Absolute)
			{
				this.m_Center = data.position;
			}
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x0007E1E8 File Offset: 0x0007C5E8
		private void Update()
		{
			if (!this.m_Dragging)
			{
				return;
			}
			if (Input.touchCount >= this.m_Id + 1 && this.m_Id != -1)
			{
				if (this.controlStyle == TouchPad.ControlStyle.Swipe)
				{
					this.m_Center = this.m_PreviousTouchPos;
					this.m_PreviousTouchPos = Input.touches[this.m_Id].position;
				}
				Vector2 vector = new Vector2(Input.touches[this.m_Id].position.x - this.m_Center.x, Input.touches[this.m_Id].position.y - this.m_Center.y);
				Vector2 normalized = vector.normalized;
				normalized.x *= this.Xsensitivity;
				normalized.y *= this.Ysensitivity;
				this.UpdateVirtualAxes(new Vector3(normalized.x, normalized.y, 0f));
			}
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x0007E2FE File Offset: 0x0007C6FE
		public void OnPointerUp(PointerEventData data)
		{
			this.m_Dragging = false;
			this.m_Id = -1;
			this.UpdateVirtualAxes(Vector3.zero);
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x0007E319 File Offset: 0x0007C719
		private void OnDisable()
		{
			if (CrossPlatformInputManager.AxisExists(this.horizontalAxisName))
			{
				CrossPlatformInputManager.UnRegisterVirtualAxis(this.horizontalAxisName);
			}
			if (CrossPlatformInputManager.AxisExists(this.verticalAxisName))
			{
				CrossPlatformInputManager.UnRegisterVirtualAxis(this.verticalAxisName);
			}
		}

		// Token: 0x04001280 RID: 4736
		public TouchPad.AxisOption axesToUse;

		// Token: 0x04001281 RID: 4737
		public TouchPad.ControlStyle controlStyle;

		// Token: 0x04001282 RID: 4738
		public string horizontalAxisName = "Horizontal";

		// Token: 0x04001283 RID: 4739
		public string verticalAxisName = "Vertical";

		// Token: 0x04001284 RID: 4740
		public float Xsensitivity = 1f;

		// Token: 0x04001285 RID: 4741
		public float Ysensitivity = 1f;

		// Token: 0x04001286 RID: 4742
		private Vector3 m_StartPos;

		// Token: 0x04001287 RID: 4743
		private Vector2 m_PreviousDelta;

		// Token: 0x04001288 RID: 4744
		private Vector3 m_JoytickOutput;

		// Token: 0x04001289 RID: 4745
		private bool m_UseX;

		// Token: 0x0400128A RID: 4746
		private bool m_UseY;

		// Token: 0x0400128B RID: 4747
		private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;

		// Token: 0x0400128C RID: 4748
		private CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

		// Token: 0x0400128D RID: 4749
		private bool m_Dragging;

		// Token: 0x0400128E RID: 4750
		private int m_Id = -1;

		// Token: 0x0400128F RID: 4751
		private Vector2 m_PreviousTouchPos;

		// Token: 0x04001290 RID: 4752
		private Vector3 m_Center;

		// Token: 0x04001291 RID: 4753
		private Image m_Image;

		// Token: 0x02000381 RID: 897
		public enum AxisOption
		{
			// Token: 0x04001293 RID: 4755
			Both,
			// Token: 0x04001294 RID: 4756
			OnlyHorizontal,
			// Token: 0x04001295 RID: 4757
			OnlyVertical
		}

		// Token: 0x02000382 RID: 898
		public enum ControlStyle
		{
			// Token: 0x04001297 RID: 4759
			Absolute,
			// Token: 0x04001298 RID: 4760
			Relative,
			// Token: 0x04001299 RID: 4761
			Swipe
		}
	}
}
