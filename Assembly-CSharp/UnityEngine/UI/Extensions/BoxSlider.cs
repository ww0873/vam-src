using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004AB RID: 1195
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/BoxSlider")]
	public class BoxSlider : Selectable, IDragHandler, IInitializePotentialDragHandler, ICanvasElement, IEventSystemHandler
	{
		// Token: 0x06001E1E RID: 7710 RVA: 0x000AC364 File Offset: 0x000AA764
		protected BoxSlider()
		{
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06001E1F RID: 7711 RVA: 0x000AC3A3 File Offset: 0x000AA7A3
		// (set) Token: 0x06001E20 RID: 7712 RVA: 0x000AC3AB File Offset: 0x000AA7AB
		public RectTransform HandleRect
		{
			get
			{
				return this.m_HandleRect;
			}
			set
			{
				if (BoxSlider.SetClass<RectTransform>(ref this.m_HandleRect, value))
				{
					this.UpdateCachedReferences();
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06001E21 RID: 7713 RVA: 0x000AC3CA File Offset: 0x000AA7CA
		// (set) Token: 0x06001E22 RID: 7714 RVA: 0x000AC3D2 File Offset: 0x000AA7D2
		public float MinValue
		{
			get
			{
				return this.m_MinValue;
			}
			set
			{
				if (BoxSlider.SetStruct<float>(ref this.m_MinValue, value))
				{
					this.SetX(this.m_ValueX);
					this.SetY(this.m_ValueY);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001E23 RID: 7715 RVA: 0x000AC403 File Offset: 0x000AA803
		// (set) Token: 0x06001E24 RID: 7716 RVA: 0x000AC40B File Offset: 0x000AA80B
		public float MaxValue
		{
			get
			{
				return this.m_MaxValue;
			}
			set
			{
				if (BoxSlider.SetStruct<float>(ref this.m_MaxValue, value))
				{
					this.SetX(this.m_ValueX);
					this.SetY(this.m_ValueY);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06001E25 RID: 7717 RVA: 0x000AC43C File Offset: 0x000AA83C
		// (set) Token: 0x06001E26 RID: 7718 RVA: 0x000AC444 File Offset: 0x000AA844
		public bool WholeNumbers
		{
			get
			{
				return this.m_WholeNumbers;
			}
			set
			{
				if (BoxSlider.SetStruct<bool>(ref this.m_WholeNumbers, value))
				{
					this.SetX(this.m_ValueX);
					this.SetY(this.m_ValueY);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001E27 RID: 7719 RVA: 0x000AC475 File Offset: 0x000AA875
		// (set) Token: 0x06001E28 RID: 7720 RVA: 0x000AC494 File Offset: 0x000AA894
		public float ValueX
		{
			get
			{
				if (this.WholeNumbers)
				{
					return Mathf.Round(this.m_ValueX);
				}
				return this.m_ValueX;
			}
			set
			{
				this.SetX(value);
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06001E29 RID: 7721 RVA: 0x000AC49D File Offset: 0x000AA89D
		// (set) Token: 0x06001E2A RID: 7722 RVA: 0x000AC4D2 File Offset: 0x000AA8D2
		public float NormalizedValueX
		{
			get
			{
				if (Mathf.Approximately(this.MinValue, this.MaxValue))
				{
					return 0f;
				}
				return Mathf.InverseLerp(this.MinValue, this.MaxValue, this.ValueX);
			}
			set
			{
				this.ValueX = Mathf.Lerp(this.MinValue, this.MaxValue, value);
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06001E2B RID: 7723 RVA: 0x000AC4EC File Offset: 0x000AA8EC
		// (set) Token: 0x06001E2C RID: 7724 RVA: 0x000AC50B File Offset: 0x000AA90B
		public float ValueY
		{
			get
			{
				if (this.WholeNumbers)
				{
					return Mathf.Round(this.m_ValueY);
				}
				return this.m_ValueY;
			}
			set
			{
				this.SetY(value);
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x000AC514 File Offset: 0x000AA914
		// (set) Token: 0x06001E2E RID: 7726 RVA: 0x000AC549 File Offset: 0x000AA949
		public float NormalizedValueY
		{
			get
			{
				if (Mathf.Approximately(this.MinValue, this.MaxValue))
				{
					return 0f;
				}
				return Mathf.InverseLerp(this.MinValue, this.MaxValue, this.ValueY);
			}
			set
			{
				this.ValueY = Mathf.Lerp(this.MinValue, this.MaxValue, value);
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001E2F RID: 7727 RVA: 0x000AC563 File Offset: 0x000AA963
		// (set) Token: 0x06001E30 RID: 7728 RVA: 0x000AC56B File Offset: 0x000AA96B
		public BoxSlider.BoxSliderEvent OnValueChanged
		{
			get
			{
				return this.m_OnValueChanged;
			}
			set
			{
				this.m_OnValueChanged = value;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001E31 RID: 7729 RVA: 0x000AC574 File Offset: 0x000AA974
		private float StepSize
		{
			get
			{
				return (!this.WholeNumbers) ? ((this.MaxValue - this.MinValue) * 0.1f) : 1f;
			}
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x000AC59E File Offset: 0x000AA99E
		public virtual void Rebuild(CanvasUpdate executing)
		{
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000AC5A0 File Offset: 0x000AA9A0
		public void LayoutComplete()
		{
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x000AC5A2 File Offset: 0x000AA9A2
		public void GraphicUpdateComplete()
		{
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x000AC5A4 File Offset: 0x000AA9A4
		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x000AC5FD File Offset: 0x000AA9FD
		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (currentValue.Equals(newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x000AC620 File Offset: 0x000AAA20
		protected override void OnEnable()
		{
			base.OnEnable();
			this.UpdateCachedReferences();
			this.SetX(this.m_ValueX, false);
			this.SetY(this.m_ValueY, false);
			this.UpdateVisuals();
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x000AC64E File Offset: 0x000AAA4E
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			base.OnDisable();
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x000AC664 File Offset: 0x000AAA64
		private void UpdateCachedReferences()
		{
			if (this.m_HandleRect)
			{
				this.m_HandleTransform = this.m_HandleRect.transform;
				if (this.m_HandleTransform.parent != null)
				{
					this.m_HandleContainerRect = this.m_HandleTransform.parent.GetComponent<RectTransform>();
				}
			}
			else
			{
				this.m_HandleContainerRect = null;
			}
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x000AC6CA File Offset: 0x000AAACA
		private void SetX(float input)
		{
			this.SetX(input, true);
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x000AC6D4 File Offset: 0x000AAAD4
		private void SetX(float input, bool sendCallback)
		{
			float num = Mathf.Clamp(input, this.MinValue, this.MaxValue);
			if (this.WholeNumbers)
			{
				num = Mathf.Round(num);
			}
			if (this.m_ValueX == num)
			{
				return;
			}
			this.m_ValueX = num;
			this.UpdateVisuals();
			if (sendCallback)
			{
				this.m_OnValueChanged.Invoke(num, this.ValueY);
			}
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x000AC738 File Offset: 0x000AAB38
		private void SetY(float input)
		{
			this.SetY(input, true);
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x000AC744 File Offset: 0x000AAB44
		private void SetY(float input, bool sendCallback)
		{
			float num = Mathf.Clamp(input, this.MinValue, this.MaxValue);
			if (this.WholeNumbers)
			{
				num = Mathf.Round(num);
			}
			if (this.m_ValueY == num)
			{
				return;
			}
			this.m_ValueY = num;
			this.UpdateVisuals();
			if (sendCallback)
			{
				this.m_OnValueChanged.Invoke(this.ValueX, num);
			}
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x000AC7A8 File Offset: 0x000AABA8
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			this.UpdateVisuals();
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x000AC7B8 File Offset: 0x000AABB8
		private void UpdateVisuals()
		{
			this.m_Tracker.Clear();
			if (this.m_HandleContainerRect != null)
			{
				this.m_Tracker.Add(this, this.m_HandleRect, DrivenTransformProperties.Anchors);
				Vector2 zero = Vector2.zero;
				Vector2 one = Vector2.one;
				int index = 0;
				float value = this.NormalizedValueX;
				one[0] = value;
				zero[index] = value;
				int index2 = 1;
				value = this.NormalizedValueY;
				one[1] = value;
				zero[index2] = value;
				this.m_HandleRect.anchorMin = zero;
				this.m_HandleRect.anchorMax = one;
			}
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x000AC850 File Offset: 0x000AAC50
		private void UpdateDrag(PointerEventData eventData, Camera cam)
		{
			RectTransform handleContainerRect = this.m_HandleContainerRect;
			if (handleContainerRect != null && handleContainerRect.rect.size[0] > 0f)
			{
				Vector2 a;
				if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(handleContainerRect, eventData.position, cam, out a))
				{
					return;
				}
				a -= handleContainerRect.rect.position;
				float normalizedValueX = Mathf.Clamp01((a - this.m_Offset)[0] / handleContainerRect.rect.size[0]);
				this.NormalizedValueX = normalizedValueX;
				float normalizedValueY = Mathf.Clamp01((a - this.m_Offset)[1] / handleContainerRect.rect.size[1]);
				this.NormalizedValueY = normalizedValueY;
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x000AC93A File Offset: 0x000AAD3A
		private bool CanDrag(PointerEventData eventData)
		{
			return this.IsActive() && this.IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x000AC960 File Offset: 0x000AAD60
		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!this.CanDrag(eventData))
			{
				return;
			}
			base.OnPointerDown(eventData);
			this.m_Offset = Vector2.zero;
			if (this.m_HandleContainerRect != null && RectTransformUtility.RectangleContainsScreenPoint(this.m_HandleRect, eventData.position, eventData.enterEventCamera))
			{
				Vector2 offset;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_HandleRect, eventData.position, eventData.pressEventCamera, out offset))
				{
					this.m_Offset = offset;
				}
				this.m_Offset.y = -this.m_Offset.y;
			}
			else
			{
				this.UpdateDrag(eventData, eventData.pressEventCamera);
			}
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000ACA07 File Offset: 0x000AAE07
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!this.CanDrag(eventData))
			{
				return;
			}
			this.UpdateDrag(eventData, eventData.pressEventCamera);
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000ACA23 File Offset: 0x000AAE23
		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			eventData.useDragThreshold = false;
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000ACA2C File Offset: 0x000AAE2C
		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x000ACA34 File Offset: 0x000AAE34
		bool ICanvasElement.IsDestroyed()
		{
			return base.IsDestroyed();
		}

		// Token: 0x04001978 RID: 6520
		[SerializeField]
		private RectTransform m_HandleRect;

		// Token: 0x04001979 RID: 6521
		[Space(6f)]
		[SerializeField]
		private float m_MinValue;

		// Token: 0x0400197A RID: 6522
		[SerializeField]
		private float m_MaxValue = 1f;

		// Token: 0x0400197B RID: 6523
		[SerializeField]
		private bool m_WholeNumbers;

		// Token: 0x0400197C RID: 6524
		[SerializeField]
		private float m_ValueX = 1f;

		// Token: 0x0400197D RID: 6525
		[SerializeField]
		private float m_ValueY = 1f;

		// Token: 0x0400197E RID: 6526
		[Space(6f)]
		[SerializeField]
		private BoxSlider.BoxSliderEvent m_OnValueChanged = new BoxSlider.BoxSliderEvent();

		// Token: 0x0400197F RID: 6527
		private Transform m_HandleTransform;

		// Token: 0x04001980 RID: 6528
		private RectTransform m_HandleContainerRect;

		// Token: 0x04001981 RID: 6529
		private Vector2 m_Offset = Vector2.zero;

		// Token: 0x04001982 RID: 6530
		private DrivenRectTransformTracker m_Tracker;

		// Token: 0x020004AC RID: 1196
		public enum Direction
		{
			// Token: 0x04001984 RID: 6532
			LeftToRight,
			// Token: 0x04001985 RID: 6533
			RightToLeft,
			// Token: 0x04001986 RID: 6534
			BottomToTop,
			// Token: 0x04001987 RID: 6535
			TopToBottom
		}

		// Token: 0x020004AD RID: 1197
		[Serializable]
		public class BoxSliderEvent : UnityEvent<float, float>
		{
			// Token: 0x06001E47 RID: 7751 RVA: 0x000ACA3C File Offset: 0x000AAE3C
			public BoxSliderEvent()
			{
			}
		}

		// Token: 0x020004AE RID: 1198
		private enum Axis
		{
			// Token: 0x04001989 RID: 6537
			Horizontal,
			// Token: 0x0400198A RID: 6538
			Vertical
		}
	}
}
