using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004CD RID: 1229
	[AddComponentMenu("UI/Extensions/Radial Slider")]
	[RequireComponent(typeof(Image))]
	public class RadialSlider : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x06001F04 RID: 7940 RVA: 0x000B0739 File Offset: 0x000AEB39
		public RadialSlider()
		{
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001F05 RID: 7941 RVA: 0x000B076D File Offset: 0x000AEB6D
		// (set) Token: 0x06001F06 RID: 7942 RVA: 0x000B0780 File Offset: 0x000AEB80
		public float Angle
		{
			get
			{
				return this.RadialImage.fillAmount * 360f;
			}
			set
			{
				if (this.LerpToTarget)
				{
					this.StartLerp(value / 360f);
				}
				else
				{
					this.UpdateRadialImage(value / 360f);
				}
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001F07 RID: 7943 RVA: 0x000B07AC File Offset: 0x000AEBAC
		// (set) Token: 0x06001F08 RID: 7944 RVA: 0x000B07B9 File Offset: 0x000AEBB9
		public float Value
		{
			get
			{
				return this.RadialImage.fillAmount;
			}
			set
			{
				if (this.LerpToTarget)
				{
					this.StartLerp(value);
				}
				else
				{
					this.UpdateRadialImage(value);
				}
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001F09 RID: 7945 RVA: 0x000B07D9 File Offset: 0x000AEBD9
		// (set) Token: 0x06001F0A RID: 7946 RVA: 0x000B07E1 File Offset: 0x000AEBE1
		public Color EndColor
		{
			get
			{
				return this.m_endColor;
			}
			set
			{
				this.m_endColor = value;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001F0B RID: 7947 RVA: 0x000B07EA File Offset: 0x000AEBEA
		// (set) Token: 0x06001F0C RID: 7948 RVA: 0x000B07F2 File Offset: 0x000AEBF2
		public Color StartColor
		{
			get
			{
				return this.m_startColor;
			}
			set
			{
				this.m_startColor = value;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001F0D RID: 7949 RVA: 0x000B07FB File Offset: 0x000AEBFB
		// (set) Token: 0x06001F0E RID: 7950 RVA: 0x000B0803 File Offset: 0x000AEC03
		public bool LerpToTarget
		{
			get
			{
				return this.m_lerpToTarget;
			}
			set
			{
				this.m_lerpToTarget = value;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001F0F RID: 7951 RVA: 0x000B080C File Offset: 0x000AEC0C
		// (set) Token: 0x06001F10 RID: 7952 RVA: 0x000B0814 File Offset: 0x000AEC14
		public AnimationCurve LerpCurve
		{
			get
			{
				return this.m_lerpCurve;
			}
			set
			{
				this.m_lerpCurve = value;
				this.m_lerpTime = this.LerpCurve[this.LerpCurve.length - 1].time;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001F11 RID: 7953 RVA: 0x000B084E File Offset: 0x000AEC4E
		public bool LerpInProgress
		{
			get
			{
				return this.lerpInProgress;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x000B0858 File Offset: 0x000AEC58
		public Image RadialImage
		{
			get
			{
				if (this.m_image == null)
				{
					this.m_image = base.GetComponent<Image>();
					this.m_image.type = Image.Type.Filled;
					this.m_image.fillMethod = Image.FillMethod.Radial360;
					this.m_image.fillOrigin = 3;
					this.m_image.fillAmount = 0f;
				}
				return this.m_image;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001F13 RID: 7955 RVA: 0x000B08BC File Offset: 0x000AECBC
		// (set) Token: 0x06001F14 RID: 7956 RVA: 0x000B08C4 File Offset: 0x000AECC4
		public RadialSlider.RadialSliderValueChangedEvent onValueChanged
		{
			get
			{
				return this._onValueChanged;
			}
			set
			{
				this._onValueChanged = value;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001F15 RID: 7957 RVA: 0x000B08CD File Offset: 0x000AECCD
		// (set) Token: 0x06001F16 RID: 7958 RVA: 0x000B08D5 File Offset: 0x000AECD5
		public RadialSlider.RadialSliderTextValueChangedEvent onTextValueChanged
		{
			get
			{
				return this._onTextValueChanged;
			}
			set
			{
				this._onTextValueChanged = value;
			}
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x000B08E0 File Offset: 0x000AECE0
		private void Awake()
		{
			if (this.LerpCurve != null && this.LerpCurve.length > 0)
			{
				this.m_lerpTime = this.LerpCurve[this.LerpCurve.length - 1].time;
			}
			else
			{
				this.m_lerpTime = 1f;
			}
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x000B0940 File Offset: 0x000AED40
		private void Update()
		{
			if (this.isPointerDown)
			{
				this.m_targetAngle = this.GetAngleFromMousePoint();
				if (!this.lerpInProgress)
				{
					if (!this.LerpToTarget)
					{
						this.UpdateRadialImage(this.m_targetAngle);
						this.NotifyValueChanged();
					}
					else
					{
						if (this.isPointerReleased)
						{
							this.StartLerp(this.m_targetAngle);
						}
						this.isPointerReleased = false;
					}
				}
			}
			if (this.lerpInProgress && this.Value != this.m_lerpTargetAngle)
			{
				this.m_currentLerpTime += Time.deltaTime;
				float num = this.m_currentLerpTime / this.m_lerpTime;
				if (this.LerpCurve != null && this.LerpCurve.length > 0)
				{
					this.UpdateRadialImage(Mathf.Lerp(this.m_startAngle, this.m_lerpTargetAngle, this.LerpCurve.Evaluate(num)));
				}
				else
				{
					this.UpdateRadialImage(Mathf.Lerp(this.m_startAngle, this.m_lerpTargetAngle, num));
				}
			}
			if (this.m_currentLerpTime >= this.m_lerpTime || this.Value == this.m_lerpTargetAngle)
			{
				this.lerpInProgress = false;
				this.UpdateRadialImage(this.m_lerpTargetAngle);
				this.NotifyValueChanged();
			}
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x000B0A82 File Offset: 0x000AEE82
		private void StartLerp(float targetAngle)
		{
			if (!this.lerpInProgress)
			{
				this.m_startAngle = this.Value;
				this.m_lerpTargetAngle = targetAngle;
				this.m_currentLerpTime = 0f;
				this.lerpInProgress = true;
			}
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x000B0AB4 File Offset: 0x000AEEB4
		private float GetAngleFromMousePoint()
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform as RectTransform, Input.mousePosition, this.m_eventCamera, out this.m_localPos);
			return (Mathf.Atan2(-this.m_localPos.y, this.m_localPos.x) * 180f / 3.1415927f + 180f) / 360f;
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x000B0B1C File Offset: 0x000AEF1C
		private void UpdateRadialImage(float targetAngle)
		{
			this.RadialImage.fillAmount = targetAngle;
			this.RadialImage.color = Color.Lerp(this.m_startColor, this.m_endColor, targetAngle);
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000B0B48 File Offset: 0x000AEF48
		private void NotifyValueChanged()
		{
			this._onValueChanged.Invoke((int)(this.m_targetAngle * 360f));
			this._onTextValueChanged.Invoke(((int)(this.m_targetAngle * 360f)).ToString());
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x000B0B93 File Offset: 0x000AEF93
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.m_eventCamera = eventData.enterEventCamera;
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x000B0BA1 File Offset: 0x000AEFA1
		public void OnPointerDown(PointerEventData eventData)
		{
			this.m_eventCamera = eventData.enterEventCamera;
			this.isPointerDown = true;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x000B0BB6 File Offset: 0x000AEFB6
		public void OnPointerUp(PointerEventData eventData)
		{
			this.isPointerDown = false;
			this.isPointerReleased = true;
		}

		// Token: 0x04001A2F RID: 6703
		private bool isPointerDown;

		// Token: 0x04001A30 RID: 6704
		private bool isPointerReleased;

		// Token: 0x04001A31 RID: 6705
		private bool lerpInProgress;

		// Token: 0x04001A32 RID: 6706
		private Vector2 m_localPos;

		// Token: 0x04001A33 RID: 6707
		private float m_targetAngle;

		// Token: 0x04001A34 RID: 6708
		private float m_lerpTargetAngle;

		// Token: 0x04001A35 RID: 6709
		private float m_startAngle;

		// Token: 0x04001A36 RID: 6710
		private float m_currentLerpTime;

		// Token: 0x04001A37 RID: 6711
		private float m_lerpTime;

		// Token: 0x04001A38 RID: 6712
		private Camera m_eventCamera;

		// Token: 0x04001A39 RID: 6713
		private Image m_image;

		// Token: 0x04001A3A RID: 6714
		[SerializeField]
		[Tooltip("Radial Gradient Start Color")]
		private Color m_startColor = Color.green;

		// Token: 0x04001A3B RID: 6715
		[SerializeField]
		[Tooltip("Radial Gradient End Color")]
		private Color m_endColor = Color.red;

		// Token: 0x04001A3C RID: 6716
		[Tooltip("Move slider absolute or use Lerping?\nDragging only supported with absolute")]
		[SerializeField]
		private bool m_lerpToTarget;

		// Token: 0x04001A3D RID: 6717
		[Tooltip("Curve to apply to the Lerp\nMust be set to enable Lerp")]
		[SerializeField]
		private AnimationCurve m_lerpCurve;

		// Token: 0x04001A3E RID: 6718
		[Tooltip("Event fired when value of control changes, outputs an INT angle value")]
		[SerializeField]
		private RadialSlider.RadialSliderValueChangedEvent _onValueChanged = new RadialSlider.RadialSliderValueChangedEvent();

		// Token: 0x04001A3F RID: 6719
		[Tooltip("Event fired when value of control changes, outputs a TEXT angle value")]
		[SerializeField]
		private RadialSlider.RadialSliderTextValueChangedEvent _onTextValueChanged = new RadialSlider.RadialSliderTextValueChangedEvent();

		// Token: 0x020004CE RID: 1230
		[Serializable]
		public class RadialSliderValueChangedEvent : UnityEvent<int>
		{
			// Token: 0x06001F20 RID: 7968 RVA: 0x000B0BC6 File Offset: 0x000AEFC6
			public RadialSliderValueChangedEvent()
			{
			}
		}

		// Token: 0x020004CF RID: 1231
		[Serializable]
		public class RadialSliderTextValueChangedEvent : UnityEvent<string>
		{
			// Token: 0x06001F21 RID: 7969 RVA: 0x000B0BCE File Offset: 0x000AEFCE
			public RadialSliderTextValueChangedEvent()
			{
			}
		}
	}
}
