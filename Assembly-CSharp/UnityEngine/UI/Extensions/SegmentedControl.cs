using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004D9 RID: 1241
	[AddComponentMenu("UI/Extensions/Segmented Control")]
	[RequireComponent(typeof(RectTransform))]
	public class SegmentedControl : UIBehaviour
	{
		// Token: 0x06001F45 RID: 8005 RVA: 0x000B204B File Offset: 0x000B044B
		protected SegmentedControl()
		{
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06001F46 RID: 8006 RVA: 0x000B2068 File Offset: 0x000B0468
		protected float SeparatorWidth
		{
			get
			{
				if (this.m_separatorWidth == 0f && this.separator)
				{
					this.m_separatorWidth = this.separator.rectTransform.rect.width;
					Image component = this.separator.GetComponent<Image>();
					if (component)
					{
						this.m_separatorWidth /= component.pixelsPerUnit;
					}
				}
				return this.m_separatorWidth;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001F47 RID: 8007 RVA: 0x000B20E3 File Offset: 0x000B04E3
		public Selectable[] segments
		{
			get
			{
				if (this.m_segments == null || this.m_segments.Length == 0)
				{
					this.m_segments = this.GetChildSegments();
				}
				return this.m_segments;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001F48 RID: 8008 RVA: 0x000B210F File Offset: 0x000B050F
		// (set) Token: 0x06001F49 RID: 8009 RVA: 0x000B2117 File Offset: 0x000B0517
		public Graphic separator
		{
			get
			{
				return this.m_separator;
			}
			set
			{
				this.m_separator = value;
				this.m_separatorWidth = 0f;
				this.LayoutSegments();
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001F4A RID: 8010 RVA: 0x000B2131 File Offset: 0x000B0531
		// (set) Token: 0x06001F4B RID: 8011 RVA: 0x000B2139 File Offset: 0x000B0539
		public bool allowSwitchingOff
		{
			get
			{
				return this.m_allowSwitchingOff;
			}
			set
			{
				this.m_allowSwitchingOff = value;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001F4C RID: 8012 RVA: 0x000B2142 File Offset: 0x000B0542
		// (set) Token: 0x06001F4D RID: 8013 RVA: 0x000B2158 File Offset: 0x000B0558
		public int selectedSegmentIndex
		{
			get
			{
				return Array.IndexOf<Selectable>(this.segments, this.selectedSegment);
			}
			set
			{
				value = Math.Max(value, -1);
				value = Math.Min(value, this.segments.Length - 1);
				this.m_selectedSegmentIndex = value;
				if (value == -1)
				{
					if (this.selectedSegment)
					{
						this.selectedSegment.GetComponent<Segment>().selected = false;
						this.selectedSegment = null;
					}
				}
				else
				{
					this.segments[value].GetComponent<Segment>().selected = true;
				}
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001F4E RID: 8014 RVA: 0x000B21CE File Offset: 0x000B05CE
		// (set) Token: 0x06001F4F RID: 8015 RVA: 0x000B21D6 File Offset: 0x000B05D6
		public SegmentedControl.SegmentSelectedEvent onValueChanged
		{
			get
			{
				return this.m_onValueChanged;
			}
			set
			{
				this.m_onValueChanged = value;
			}
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x000B21DF File Offset: 0x000B05DF
		protected override void Start()
		{
			base.Start();
			this.LayoutSegments();
			if (this.m_selectedSegmentIndex != -1)
			{
				this.selectedSegmentIndex = this.m_selectedSegmentIndex;
			}
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x000B2208 File Offset: 0x000B0608
		private Selectable[] GetChildSegments()
		{
			Selectable[] componentsInChildren = base.GetComponentsInChildren<Selectable>();
			if (componentsInChildren.Length < 2)
			{
				throw new InvalidOperationException("A segmented control must have at least two Button children");
			}
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Segment segment = componentsInChildren[i].GetComponent<Segment>();
				if (segment == null)
				{
					segment = componentsInChildren[i].gameObject.AddComponent<Segment>();
				}
				segment.index = i;
			}
			return componentsInChildren;
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x000B226F File Offset: 0x000B066F
		public void SetAllSegmentsOff()
		{
			this.selectedSegment = null;
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x000B2278 File Offset: 0x000B0678
		private void RecreateSprites()
		{
			for (int i = 0; i < this.segments.Length; i++)
			{
				if (!(this.segments[i].image == null))
				{
					Sprite sprite = this.segments[i].image.sprite;
					if (sprite.border.x != 0f && sprite.border.z != 0f)
					{
						Rect rect = sprite.rect;
						Vector4 border = sprite.border;
						if (i > 0)
						{
							rect.xMin = border.x;
							border.x = 0f;
						}
						if (i < this.segments.Length - 1)
						{
							rect.xMax = border.z;
							border.z = 0f;
						}
						this.segments[i].image.sprite = Sprite.Create(sprite.texture, rect, sprite.pivot, sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect, border);
					}
				}
			}
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x000B2390 File Offset: 0x000B0790
		public void LayoutSegments()
		{
			this.RecreateSprites();
			RectTransform rectTransform = base.transform as RectTransform;
			float num = rectTransform.rect.width / (float)this.segments.Length - this.SeparatorWidth * (float)(this.segments.Length - 1);
			for (int i = 0; i < this.segments.Length; i++)
			{
				float num2 = (num + this.SeparatorWidth) * (float)i;
				RectTransform component = this.segments[i].GetComponent<RectTransform>();
				component.anchorMin = Vector2.zero;
				component.anchorMax = Vector2.zero;
				component.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, num2, num);
				component.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rectTransform.rect.height);
				if (this.separator && i > 0)
				{
					Transform transform = base.gameObject.transform.Find("Separator " + i);
					Graphic graphic = (!(transform != null)) ? Object.Instantiate<GameObject>(this.separator.gameObject).GetComponent<Graphic>() : transform.GetComponent<Graphic>();
					graphic.gameObject.name = "Separator " + i;
					graphic.gameObject.SetActive(true);
					graphic.rectTransform.SetParent(base.transform, false);
					graphic.rectTransform.anchorMin = Vector2.zero;
					graphic.rectTransform.anchorMax = Vector2.zero;
					graphic.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, num2 - this.SeparatorWidth, this.SeparatorWidth);
					graphic.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rectTransform.rect.height);
				}
			}
		}

		// Token: 0x04001A79 RID: 6777
		private Selectable[] m_segments;

		// Token: 0x04001A7A RID: 6778
		[SerializeField]
		[Tooltip("A GameObject with an Image to use as a separator between segments. Size of the RectTransform will determine the size of the separator used.\nNote, make sure to disable the separator GO so that it does not affect the scene")]
		private Graphic m_separator;

		// Token: 0x04001A7B RID: 6779
		private float m_separatorWidth;

		// Token: 0x04001A7C RID: 6780
		[SerializeField]
		[Tooltip("When True, it allows each button to be toggled on/off")]
		private bool m_allowSwitchingOff;

		// Token: 0x04001A7D RID: 6781
		[SerializeField]
		[Tooltip("The selected default for the control (zero indexed array)")]
		private int m_selectedSegmentIndex = -1;

		// Token: 0x04001A7E RID: 6782
		[SerializeField]
		[Tooltip("Event to fire once the selection has been changed")]
		private SegmentedControl.SegmentSelectedEvent m_onValueChanged = new SegmentedControl.SegmentSelectedEvent();

		// Token: 0x04001A7F RID: 6783
		protected internal Selectable selectedSegment;

		// Token: 0x04001A80 RID: 6784
		[SerializeField]
		public Color selectedColor;

		// Token: 0x020004DA RID: 1242
		[Serializable]
		public class SegmentSelectedEvent : UnityEvent<int>
		{
			// Token: 0x06001F55 RID: 8021 RVA: 0x000B2553 File Offset: 0x000B0953
			public SegmentSelectedEvent()
			{
			}
		}
	}
}
