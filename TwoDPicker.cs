using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DE9 RID: 3561
public class TwoDPicker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x06006E17 RID: 28183 RVA: 0x0029180D File Offset: 0x0028FC0D
	public TwoDPicker()
	{
	}

	// Token: 0x17001014 RID: 4116
	// (get) Token: 0x06006E18 RID: 28184 RVA: 0x00291815 File Offset: 0x0028FC15
	// (set) Token: 0x06006E19 RID: 28185 RVA: 0x0029181D File Offset: 0x0028FC1D
	public float xVal
	{
		get
		{
			return this._xVal;
		}
		set
		{
			if (this._xVal != value)
			{
				this._xVal = value;
				this._xVal = Mathf.Clamp01(this._xVal);
				this.SetSelectorPositionFromXYVal();
			}
		}
	}

	// Token: 0x17001015 RID: 4117
	// (get) Token: 0x06006E1A RID: 28186 RVA: 0x00291849 File Offset: 0x0028FC49
	// (set) Token: 0x06006E1B RID: 28187 RVA: 0x00291851 File Offset: 0x0028FC51
	public float yVal
	{
		get
		{
			return this._yVal;
		}
		set
		{
			if (this._yVal != value)
			{
				this._yVal = value;
				this._yVal = Mathf.Clamp01(this._yVal);
				this.SetSelectorPositionFromXYVal();
			}
		}
	}

	// Token: 0x06006E1C RID: 28188 RVA: 0x0029187D File Offset: 0x0028FC7D
	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.Selector != null)
		{
			this.SetDraggedPosition(eventData);
		}
	}

	// Token: 0x06006E1D RID: 28189 RVA: 0x00291897 File Offset: 0x0028FC97
	public void OnBeginDrag(PointerEventData eventData)
	{
	}

	// Token: 0x06006E1E RID: 28190 RVA: 0x00291899 File Offset: 0x0028FC99
	public void OnDrag(PointerEventData data)
	{
		if (this.Selector != null)
		{
			this.SetDraggedPosition(data);
		}
	}

	// Token: 0x06006E1F RID: 28191 RVA: 0x002918B4 File Offset: 0x0028FCB4
	protected virtual void SetXYValFromSelectorPosition()
	{
		if (this.Selector != null)
		{
			RectTransform component = this.Selector.GetComponent<RectTransform>();
			if (this.Container == null)
			{
				this.Container = base.GetComponent<RectTransform>();
			}
			if (this.Container != null && component != null)
			{
				Vector2 anchoredPosition = component.anchoredPosition;
				this._xVal = Mathf.Clamp01(anchoredPosition.x / this.Container.rect.width);
				this._yVal = Mathf.Clamp01(anchoredPosition.y / this.Container.rect.height);
				this.SetSelectorPositionFromXYVal();
			}
		}
	}

	// Token: 0x06006E20 RID: 28192 RVA: 0x00291974 File Offset: 0x0028FD74
	protected virtual void SetSelectorPositionFromXYVal()
	{
		if (this.Selector != null)
		{
			RectTransform component = this.Selector.GetComponent<RectTransform>();
			if (this.Container == null)
			{
				this.Container = base.GetComponent<RectTransform>();
			}
			if (this.Container != null && component != null)
			{
				Vector2 anchoredPosition;
				anchoredPosition.x = this.Container.rect.width * this._xVal;
				anchoredPosition.y = this.Container.rect.height * this._yVal;
				component.anchoredPosition = anchoredPosition;
			}
		}
	}

	// Token: 0x06006E21 RID: 28193 RVA: 0x00291A24 File Offset: 0x0028FE24
	protected void SetDraggedPosition(PointerEventData data)
	{
		if (this.Selector != null)
		{
			RectTransform component = this.Selector.GetComponent<RectTransform>();
			if (this.Container == null)
			{
				this.Container = base.GetComponent<RectTransform>();
			}
			Vector3 position;
			if (this.Container != null && component != null && RectTransformUtility.ScreenPointToWorldPointInRectangle(this.Container, data.position, data.pressEventCamera, out position))
			{
				component.position = position;
				component.rotation = this.Container.rotation;
				this.SetXYValFromSelectorPosition();
			}
		}
	}

	// Token: 0x06006E22 RID: 28194 RVA: 0x00291AC4 File Offset: 0x0028FEC4
	public void OnEndDrag(PointerEventData eventData)
	{
	}

	// Token: 0x06006E23 RID: 28195 RVA: 0x00291AC6 File Offset: 0x0028FEC6
	public virtual void Awake()
	{
		this.SetSelectorPositionFromXYVal();
	}

	// Token: 0x04005F4C RID: 24396
	public Image Selector;

	// Token: 0x04005F4D RID: 24397
	public RectTransform Container;

	// Token: 0x04005F4E RID: 24398
	[SerializeField]
	protected float _xVal;

	// Token: 0x04005F4F RID: 24399
	[SerializeField]
	protected float _yVal;
}
