using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004E0 RID: 1248
	[AddComponentMenu("UI/Extensions/Stepper")]
	[RequireComponent(typeof(RectTransform))]
	public class Stepper : UIBehaviour
	{
		// Token: 0x06001F8E RID: 8078 RVA: 0x000B3381 File Offset: 0x000B1781
		protected Stepper()
		{
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x000B33A4 File Offset: 0x000B17A4
		private float separatorWidth
		{
			get
			{
				if (this._separatorWidth == 0f && this.separator)
				{
					this._separatorWidth = this.separator.rectTransform.rect.width;
					Image component = this.separator.GetComponent<Image>();
					if (component)
					{
						this._separatorWidth /= component.pixelsPerUnit;
					}
				}
				return this._separatorWidth;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x000B341F File Offset: 0x000B181F
		public Selectable[] sides
		{
			get
			{
				if (this._sides == null || this._sides.Length == 0)
				{
					this._sides = this.GetSides();
				}
				return this._sides;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x000B344B File Offset: 0x000B184B
		// (set) Token: 0x06001F92 RID: 8082 RVA: 0x000B3453 File Offset: 0x000B1853
		public int value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001F93 RID: 8083 RVA: 0x000B345C File Offset: 0x000B185C
		// (set) Token: 0x06001F94 RID: 8084 RVA: 0x000B3464 File Offset: 0x000B1864
		public int minimum
		{
			get
			{
				return this._minimum;
			}
			set
			{
				this._minimum = value;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x000B346D File Offset: 0x000B186D
		// (set) Token: 0x06001F96 RID: 8086 RVA: 0x000B3475 File Offset: 0x000B1875
		public int maximum
		{
			get
			{
				return this._maximum;
			}
			set
			{
				this._maximum = value;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001F97 RID: 8087 RVA: 0x000B347E File Offset: 0x000B187E
		// (set) Token: 0x06001F98 RID: 8088 RVA: 0x000B3486 File Offset: 0x000B1886
		public int step
		{
			get
			{
				return this._step;
			}
			set
			{
				this._step = value;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x000B348F File Offset: 0x000B188F
		// (set) Token: 0x06001F9A RID: 8090 RVA: 0x000B3497 File Offset: 0x000B1897
		public bool wrap
		{
			get
			{
				return this._wrap;
			}
			set
			{
				this._wrap = value;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x000B34A0 File Offset: 0x000B18A0
		// (set) Token: 0x06001F9C RID: 8092 RVA: 0x000B34A8 File Offset: 0x000B18A8
		public Graphic separator
		{
			get
			{
				return this._separator;
			}
			set
			{
				this._separator = value;
				this._separatorWidth = 0f;
				this.LayoutSides(this.sides);
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x000B34C8 File Offset: 0x000B18C8
		// (set) Token: 0x06001F9E RID: 8094 RVA: 0x000B34D0 File Offset: 0x000B18D0
		public Stepper.StepperValueChangedEvent onValueChanged
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

		// Token: 0x06001F9F RID: 8095 RVA: 0x000B34DC File Offset: 0x000B18DC
		private Selectable[] GetSides()
		{
			Selectable[] componentsInChildren = base.GetComponentsInChildren<Selectable>();
			if (componentsInChildren.Length != 2)
			{
				throw new InvalidOperationException("A stepper must have two Button children");
			}
			for (int i = 0; i < 2; i++)
			{
				StepperSide x = componentsInChildren[i].GetComponent<StepperSide>();
				if (x == null)
				{
					x = componentsInChildren[i].gameObject.AddComponent<StepperSide>();
				}
			}
			if (!this.wrap)
			{
				this.DisableAtExtremes(componentsInChildren);
			}
			this.LayoutSides(componentsInChildren);
			return componentsInChildren;
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x000B3553 File Offset: 0x000B1953
		public void StepUp()
		{
			this.Step(this.step);
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x000B3561 File Offset: 0x000B1961
		public void StepDown()
		{
			this.Step(-this.step);
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x000B3570 File Offset: 0x000B1970
		private void Step(int amount)
		{
			this.value += amount;
			if (this.wrap)
			{
				if (this.value > this.maximum)
				{
					this.value = this.minimum;
				}
				if (this.value < this.minimum)
				{
					this.value = this.maximum;
				}
			}
			else
			{
				this.value = Math.Max(this.minimum, this.value);
				this.value = Math.Min(this.maximum, this.value);
				this.DisableAtExtremes(this.sides);
			}
			this._onValueChanged.Invoke(this.value);
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x000B3620 File Offset: 0x000B1A20
		private void DisableAtExtremes(Selectable[] sides)
		{
			sides[0].interactable = (this.wrap || this.value > this.minimum);
			sides[1].interactable = (this.wrap || this.value < this.maximum);
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x000B3678 File Offset: 0x000B1A78
		private void RecreateSprites(Selectable[] sides)
		{
			for (int i = 0; i < 2; i++)
			{
				if (!(sides[i].image == null))
				{
					Sprite sprite = sides[i].image.sprite;
					if (sprite.border.x != 0f && sprite.border.z != 0f)
					{
						Rect rect = sprite.rect;
						Vector4 border = sprite.border;
						if (i == 0)
						{
							rect.xMax = border.z;
							border.z = 0f;
						}
						else
						{
							rect.xMin = border.x;
							border.x = 0f;
						}
						sides[i].image.sprite = Sprite.Create(sprite.texture, rect, sprite.pivot, sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect, border);
					}
				}
			}
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x000B376C File Offset: 0x000B1B6C
		public void LayoutSides(Selectable[] sides = null)
		{
			sides = (sides ?? this.sides);
			this.RecreateSprites(sides);
			RectTransform rectTransform = base.transform as RectTransform;
			float num = rectTransform.rect.width / 2f - this.separatorWidth;
			for (int i = 0; i < 2; i++)
			{
				float inset = (i != 0) ? (num + this.separatorWidth) : 0f;
				RectTransform component = sides[i].GetComponent<RectTransform>();
				component.anchorMin = Vector2.zero;
				component.anchorMax = Vector2.zero;
				component.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, num);
				component.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rectTransform.rect.height);
			}
			if (this.separator)
			{
				Transform transform = base.gameObject.transform.Find("Separator");
				Graphic graphic = (!(transform != null)) ? Object.Instantiate<GameObject>(this.separator.gameObject).GetComponent<Graphic>() : transform.GetComponent<Graphic>();
				graphic.gameObject.name = "Separator";
				graphic.gameObject.SetActive(true);
				graphic.rectTransform.SetParent(base.transform, false);
				graphic.rectTransform.anchorMin = Vector2.zero;
				graphic.rectTransform.anchorMax = Vector2.zero;
				graphic.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, num, this.separatorWidth);
				graphic.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, rectTransform.rect.height);
			}
		}

		// Token: 0x04001A92 RID: 6802
		private Selectable[] _sides;

		// Token: 0x04001A93 RID: 6803
		[SerializeField]
		[Tooltip("The current step value of the control")]
		private int _value;

		// Token: 0x04001A94 RID: 6804
		[SerializeField]
		[Tooltip("The minimum step value allowed by the control. When reached it will disable the '-' button")]
		private int _minimum;

		// Token: 0x04001A95 RID: 6805
		[SerializeField]
		[Tooltip("The maximum step value allowed by the control. When reached it will disable the '+' button")]
		private int _maximum = 100;

		// Token: 0x04001A96 RID: 6806
		[SerializeField]
		[Tooltip("The step increment used to increment / decrement the step value")]
		private int _step = 1;

		// Token: 0x04001A97 RID: 6807
		[SerializeField]
		[Tooltip("Does the step value loop around from end to end")]
		private bool _wrap;

		// Token: 0x04001A98 RID: 6808
		[SerializeField]
		[Tooltip("A GameObject with an Image to use as a separator between segments. Size of the RectTransform will determine the size of the separator used.\nNote, make sure to disable the separator GO so that it does not affect the scene")]
		private Graphic _separator;

		// Token: 0x04001A99 RID: 6809
		private float _separatorWidth;

		// Token: 0x04001A9A RID: 6810
		[SerializeField]
		private Stepper.StepperValueChangedEvent _onValueChanged = new Stepper.StepperValueChangedEvent();

		// Token: 0x020004E1 RID: 1249
		[Serializable]
		public class StepperValueChangedEvent : UnityEvent<int>
		{
			// Token: 0x06001FA6 RID: 8102 RVA: 0x000B390C File Offset: 0x000B1D0C
			public StepperValueChangedEvent()
			{
			}
		}
	}
}
