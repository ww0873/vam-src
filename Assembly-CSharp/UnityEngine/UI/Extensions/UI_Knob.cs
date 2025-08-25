using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004E7 RID: 1255
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Extensions/UI_Knob")]
	public class UI_Knob : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06001FC2 RID: 8130 RVA: 0x000B484A File Offset: 0x000B2C4A
		public UI_Knob()
		{
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x000B4861 File Offset: 0x000B2C61
		public void OnPointerDown(PointerEventData eventData)
		{
			this._canDrag = true;
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x000B486A File Offset: 0x000B2C6A
		public void OnPointerUp(PointerEventData eventData)
		{
			this._canDrag = false;
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x000B4873 File Offset: 0x000B2C73
		public void OnPointerEnter(PointerEventData eventData)
		{
			this._canDrag = true;
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000B487C File Offset: 0x000B2C7C
		public void OnPointerExit(PointerEventData eventData)
		{
			this._canDrag = false;
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x000B4885 File Offset: 0x000B2C85
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.SetInitPointerData(eventData);
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x000B4890 File Offset: 0x000B2C90
		private void SetInitPointerData(PointerEventData eventData)
		{
			this._initRotation = base.transform.rotation;
			this._currentVector = eventData.position - base.transform.position;
			this._initAngle = Mathf.Atan2(this._currentVector.y, this._currentVector.x) * 57.29578f;
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x000B48F8 File Offset: 0x000B2CF8
		public void OnDrag(PointerEventData eventData)
		{
			if (!this._canDrag)
			{
				this.SetInitPointerData(eventData);
				return;
			}
			this._currentVector = eventData.position - base.transform.position;
			this._currentAngle = Mathf.Atan2(this._currentVector.y, this._currentVector.x) * 57.29578f;
			Quaternion rhs = Quaternion.AngleAxis(this._currentAngle - this._initAngle, base.transform.forward);
			rhs.eulerAngles = new Vector3(0f, 0f, rhs.eulerAngles.z);
			Quaternion rotation = this._initRotation * rhs;
			if (this.direction == UI_Knob.Direction.CW)
			{
				this.knobValue = 1f - rotation.eulerAngles.z / 360f;
				if (this.snapToPosition)
				{
					this.SnapToPosition(ref this.knobValue);
					rotation.eulerAngles = new Vector3(0f, 0f, 360f - 360f * this.knobValue);
				}
			}
			else
			{
				this.knobValue = rotation.eulerAngles.z / 360f;
				if (this.snapToPosition)
				{
					this.SnapToPosition(ref this.knobValue);
					rotation.eulerAngles = new Vector3(0f, 0f, 360f * this.knobValue);
				}
			}
			if (Mathf.Abs(this.knobValue - this._previousValue) > 0.5f)
			{
				if (this.knobValue < 0.5f && this.loops > 1 && this._currentLoops < (float)(this.loops - 1))
				{
					this._currentLoops += 1f;
				}
				else if (this.knobValue > 0.5f && this._currentLoops >= 1f)
				{
					this._currentLoops -= 1f;
				}
				else
				{
					if (this.knobValue > 0.5f && this._currentLoops == 0f)
					{
						this.knobValue = 0f;
						base.transform.localEulerAngles = Vector3.zero;
						this.SetInitPointerData(eventData);
						this.InvokeEvents(this.knobValue + this._currentLoops);
						return;
					}
					if (this.knobValue < 0.5f && this._currentLoops == (float)(this.loops - 1))
					{
						this.knobValue = 1f;
						base.transform.localEulerAngles = Vector3.zero;
						this.SetInitPointerData(eventData);
						this.InvokeEvents(this.knobValue + this._currentLoops);
						return;
					}
				}
			}
			if (this.maxValue > 0f && this.knobValue + this._currentLoops > this.maxValue)
			{
				this.knobValue = this.maxValue;
				float z = (this.direction != UI_Knob.Direction.CW) ? (360f * this.maxValue) : (360f - 360f * this.maxValue);
				base.transform.localEulerAngles = new Vector3(0f, 0f, z);
				this.SetInitPointerData(eventData);
				this.InvokeEvents(this.knobValue);
				return;
			}
			base.transform.rotation = rotation;
			this.InvokeEvents(this.knobValue + this._currentLoops);
			this._previousValue = this.knobValue;
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x000B4C80 File Offset: 0x000B3080
		private void SnapToPosition(ref float knobValue)
		{
			float num = 1f / (float)this.snapStepsPerLoop;
			float num2 = Mathf.Round(knobValue / num) * num;
			knobValue = num2;
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x000B4CAA File Offset: 0x000B30AA
		private void InvokeEvents(float value)
		{
			if (this.clampOutput01)
			{
				value /= (float)this.loops;
			}
			this.OnValueChanged.Invoke(value);
		}

		// Token: 0x04001ABA RID: 6842
		[Tooltip("Direction of rotation CW - clockwise, CCW - counterClockwise")]
		public UI_Knob.Direction direction;

		// Token: 0x04001ABB RID: 6843
		[HideInInspector]
		public float knobValue;

		// Token: 0x04001ABC RID: 6844
		[Tooltip("Max value of the knob, maximum RAW output value knob can reach, overrides snap step, IF set to 0 or higher than loops, max value will be set by loops")]
		public float maxValue;

		// Token: 0x04001ABD RID: 6845
		[Tooltip("How many rotations knob can do, if higher than max value, the latter will limit max value")]
		public int loops = 1;

		// Token: 0x04001ABE RID: 6846
		[Tooltip("Clamp output value between 0 and 1, usefull with loops > 1")]
		public bool clampOutput01;

		// Token: 0x04001ABF RID: 6847
		[Tooltip("snap to position?")]
		public bool snapToPosition;

		// Token: 0x04001AC0 RID: 6848
		[Tooltip("Number of positions to snap")]
		public int snapStepsPerLoop = 10;

		// Token: 0x04001AC1 RID: 6849
		[Space(30f)]
		public KnobFloatValueEvent OnValueChanged;

		// Token: 0x04001AC2 RID: 6850
		private float _currentLoops;

		// Token: 0x04001AC3 RID: 6851
		private float _previousValue;

		// Token: 0x04001AC4 RID: 6852
		private float _initAngle;

		// Token: 0x04001AC5 RID: 6853
		private float _currentAngle;

		// Token: 0x04001AC6 RID: 6854
		private Vector2 _currentVector;

		// Token: 0x04001AC7 RID: 6855
		private Quaternion _initRotation;

		// Token: 0x04001AC8 RID: 6856
		private bool _canDrag;

		// Token: 0x020004E8 RID: 1256
		public enum Direction
		{
			// Token: 0x04001ACA RID: 6858
			CW,
			// Token: 0x04001ACB RID: 6859
			CCW
		}
	}
}
