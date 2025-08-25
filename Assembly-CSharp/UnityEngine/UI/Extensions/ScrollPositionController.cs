using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000513 RID: 1299
	public class ScrollPositionController : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x060020CE RID: 8398 RVA: 0x000BC6F8 File Offset: 0x000BAAF8
		public ScrollPositionController()
		{
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x000BC76C File Offset: 0x000BAB6C
		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.pointerStartLocalPosition = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewport, eventData.position, eventData.pressEventCamera, out this.pointerStartLocalPosition);
			this.dragStartScrollPosition = this.currentScrollPosition;
			this.dragging = true;
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x000BC7C4 File Offset: 0x000BABC4
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (!this.dragging)
			{
				return;
			}
			Vector2 a;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewport, eventData.position, eventData.pressEventCamera, out a))
			{
				return;
			}
			Vector2 vector = a - this.pointerStartLocalPosition;
			float num = ((this.directionOfRecognize != ScrollPositionController.ScrollDirection.Horizontal) ? vector.y : (-vector.x)) / this.GetViewportSize() * this.scrollSensitivity + this.dragStartScrollPosition;
			float num2 = this.CalculateOffset(num);
			num += num2;
			if (this.movementType == ScrollPositionController.MovementType.Elastic && num2 != 0f)
			{
				num -= this.RubberDelta(num2, this.scrollSensitivity);
			}
			this.UpdatePosition(num);
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x000BC885 File Offset: 0x000BAC85
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.dragging = false;
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x000BC89C File Offset: 0x000BAC9C
		private float GetViewportSize()
		{
			return (this.directionOfRecognize != ScrollPositionController.ScrollDirection.Horizontal) ? this.viewport.rect.size.y : this.viewport.rect.size.x;
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x000BC8F0 File Offset: 0x000BACF0
		private float CalculateOffset(float position)
		{
			if (this.movementType == ScrollPositionController.MovementType.Unrestricted)
			{
				return 0f;
			}
			if (position < 0f)
			{
				return -position;
			}
			if (position > (float)(this.dataCount - 1))
			{
				return (float)(this.dataCount - 1) - position;
			}
			return 0f;
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x000BC93C File Offset: 0x000BAD3C
		private void UpdatePosition(float position)
		{
			this.currentScrollPosition = position;
			if (this.OnUpdatePosition != null)
			{
				this.OnUpdatePosition.Invoke(this.currentScrollPosition);
			}
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x000BC961 File Offset: 0x000BAD61
		private float RubberDelta(float overStretching, float viewSize)
		{
			return (1f - 1f / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1f)) * viewSize * Mathf.Sign(overStretching);
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000BC98C File Offset: 0x000BAD8C
		public void SetDataCount(int dataCont)
		{
			this.dataCount = dataCont;
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x000BC998 File Offset: 0x000BAD98
		private void Update()
		{
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			float num = this.CalculateOffset(this.currentScrollPosition);
			if (this.autoScrolling)
			{
				float num2 = Mathf.Clamp01((Time.unscaledTime - this.autoScrollStartTime) / Mathf.Max(this.autoScrollDuration, float.Epsilon));
				float position = Mathf.Lerp(this.dragStartScrollPosition, this.autoScrollPosition, this.EaseInOutCubic(0f, 1f, num2));
				this.UpdatePosition(position);
				if (Mathf.Approximately(num2, 1f))
				{
					this.autoScrolling = false;
					if (this.OnItemSelected != null)
					{
						this.OnItemSelected.Invoke(Mathf.RoundToInt(this.GetLoopPosition(this.autoScrollPosition, this.dataCount)));
					}
				}
			}
			else if (!this.dragging && (num != 0f || this.velocity != 0f))
			{
				float num3 = this.currentScrollPosition;
				if (this.movementType == ScrollPositionController.MovementType.Elastic && num != 0f)
				{
					float num4 = this.velocity;
					num3 = Mathf.SmoothDamp(this.currentScrollPosition, this.currentScrollPosition + num, ref num4, this.elasticity, float.PositiveInfinity, unscaledDeltaTime);
					this.velocity = num4;
				}
				else if (this.inertia)
				{
					this.velocity *= Mathf.Pow(this.decelerationRate, unscaledDeltaTime);
					if (Mathf.Abs(this.velocity) < 0.001f)
					{
						this.velocity = 0f;
					}
					num3 += this.velocity * unscaledDeltaTime;
					if (this.snap.Enable && Mathf.Abs(this.velocity) < this.snap.VelocityThreshold)
					{
						this.ScrollTo(Mathf.RoundToInt(this.currentScrollPosition), this.snap.Duration);
					}
				}
				else
				{
					this.velocity = 0f;
				}
				if (this.velocity != 0f)
				{
					if (this.movementType == ScrollPositionController.MovementType.Clamped)
					{
						num = this.CalculateOffset(num3);
						num3 += num;
					}
					this.UpdatePosition(num3);
				}
			}
			if (!this.autoScrolling && this.dragging && this.inertia)
			{
				float b = (this.currentScrollPosition - this.prevScrollPosition) / unscaledDeltaTime;
				this.velocity = Mathf.Lerp(this.velocity, b, unscaledDeltaTime * 10f);
			}
			if (this.currentScrollPosition != this.prevScrollPosition)
			{
				this.prevScrollPosition = this.currentScrollPosition;
			}
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x000BCC1C File Offset: 0x000BB01C
		public void ScrollTo(int index, float duration)
		{
			this.velocity = 0f;
			this.autoScrolling = true;
			this.autoScrollDuration = duration;
			this.autoScrollStartTime = Time.unscaledTime;
			this.dragStartScrollPosition = this.currentScrollPosition;
			this.autoScrollPosition = ((this.movementType != ScrollPositionController.MovementType.Unrestricted) ? ((float)index) : this.CalculateClosestPosition(index));
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x000BCC78 File Offset: 0x000BB078
		private float CalculateClosestPosition(int index)
		{
			float num = this.GetLoopPosition((float)index, this.dataCount) - this.GetLoopPosition(this.currentScrollPosition, this.dataCount);
			if (Mathf.Abs(num) > (float)this.dataCount * 0.5f)
			{
				num = Mathf.Sign(-num) * ((float)this.dataCount - Mathf.Abs(num));
			}
			return num + this.currentScrollPosition;
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x000BCCDE File Offset: 0x000BB0DE
		private float GetLoopPosition(float position, int length)
		{
			if (position < 0f)
			{
				position = (float)(length - 1) + (position + 1f) % (float)length;
			}
			else if (position > (float)(length - 1))
			{
				position %= (float)length;
			}
			return position;
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000BCD14 File Offset: 0x000BB114
		private float EaseInOutCubic(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end * 0.5f * value * value * value + start;
			}
			value -= 2f;
			return end * 0.5f * (value * value * value + 2f) + start;
		}

		// Token: 0x04001B5C RID: 7004
		[SerializeField]
		private RectTransform viewport;

		// Token: 0x04001B5D RID: 7005
		[SerializeField]
		private ScrollPositionController.ScrollDirection directionOfRecognize;

		// Token: 0x04001B5E RID: 7006
		[SerializeField]
		private ScrollPositionController.MovementType movementType = ScrollPositionController.MovementType.Elastic;

		// Token: 0x04001B5F RID: 7007
		[SerializeField]
		private float elasticity = 0.1f;

		// Token: 0x04001B60 RID: 7008
		[SerializeField]
		private float scrollSensitivity = 1f;

		// Token: 0x04001B61 RID: 7009
		[SerializeField]
		private bool inertia = true;

		// Token: 0x04001B62 RID: 7010
		[SerializeField]
		[Tooltip("Only used when inertia is enabled")]
		private float decelerationRate = 0.03f;

		// Token: 0x04001B63 RID: 7011
		[SerializeField]
		[Tooltip("Only used when inertia is enabled")]
		private ScrollPositionController.Snap snap = new ScrollPositionController.Snap
		{
			Enable = true,
			VelocityThreshold = 0.5f,
			Duration = 0.3f
		};

		// Token: 0x04001B64 RID: 7012
		[SerializeField]
		private int dataCount;

		// Token: 0x04001B65 RID: 7013
		[Tooltip("Event that fires when the position of an item changes")]
		public ScrollPositionController.UpdatePositionEvent OnUpdatePosition;

		// Token: 0x04001B66 RID: 7014
		[Tooltip("Event that fires when an item is selected/focused")]
		public ScrollPositionController.ItemSelectedEvent OnItemSelected;

		// Token: 0x04001B67 RID: 7015
		private Vector2 pointerStartLocalPosition;

		// Token: 0x04001B68 RID: 7016
		private float dragStartScrollPosition;

		// Token: 0x04001B69 RID: 7017
		private float currentScrollPosition;

		// Token: 0x04001B6A RID: 7018
		private bool dragging;

		// Token: 0x04001B6B RID: 7019
		private float velocity;

		// Token: 0x04001B6C RID: 7020
		private float prevScrollPosition;

		// Token: 0x04001B6D RID: 7021
		private bool autoScrolling;

		// Token: 0x04001B6E RID: 7022
		private float autoScrollDuration;

		// Token: 0x04001B6F RID: 7023
		private float autoScrollStartTime;

		// Token: 0x04001B70 RID: 7024
		private float autoScrollPosition;

		// Token: 0x02000514 RID: 1300
		[Serializable]
		public class UpdatePositionEvent : UnityEvent<float>
		{
			// Token: 0x060020DC RID: 8412 RVA: 0x000BCD68 File Offset: 0x000BB168
			public UpdatePositionEvent()
			{
			}
		}

		// Token: 0x02000515 RID: 1301
		[Serializable]
		public class ItemSelectedEvent : UnityEvent<int>
		{
			// Token: 0x060020DD RID: 8413 RVA: 0x000BCD70 File Offset: 0x000BB170
			public ItemSelectedEvent()
			{
			}
		}

		// Token: 0x02000516 RID: 1302
		[Serializable]
		private struct Snap
		{
			// Token: 0x04001B71 RID: 7025
			public bool Enable;

			// Token: 0x04001B72 RID: 7026
			public float VelocityThreshold;

			// Token: 0x04001B73 RID: 7027
			public float Duration;
		}

		// Token: 0x02000517 RID: 1303
		private enum ScrollDirection
		{
			// Token: 0x04001B75 RID: 7029
			Vertical,
			// Token: 0x04001B76 RID: 7030
			Horizontal
		}

		// Token: 0x02000518 RID: 1304
		private enum MovementType
		{
			// Token: 0x04001B78 RID: 7032
			Unrestricted,
			// Token: 0x04001B79 RID: 7033
			Elastic,
			// Token: 0x04001B7A RID: 7034
			Clamped
		}
	}
}
