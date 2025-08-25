using System;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000524 RID: 1316
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("Layout/Extensions/Vertical Scroller")]
	public class UIVerticalScroller : MonoBehaviour
	{
		// Token: 0x06002148 RID: 8520 RVA: 0x000BE369 File Offset: 0x000BC769
		public UIVerticalScroller()
		{
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x000BE378 File Offset: 0x000BC778
		public UIVerticalScroller(RectTransform scrollingPanel, GameObject[] arrayOfElements, RectTransform center)
		{
			this._scrollingPanel = scrollingPanel;
			this._arrayOfElements = arrayOfElements;
			this._center = center;
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x000BE39C File Offset: 0x000BC79C
		public void Awake()
		{
			ScrollRect component = base.GetComponent<ScrollRect>();
			if (!this._scrollingPanel)
			{
				this._scrollingPanel = component.content;
			}
			if (!this._center)
			{
				Debug.LogError("Please define the RectTransform for the Center viewport of the scrollable area");
			}
			if (this._arrayOfElements == null || this._arrayOfElements.Length == 0)
			{
				int childCount = component.content.childCount;
				if (childCount > 0)
				{
					this._arrayOfElements = new GameObject[childCount];
					for (int i = 0; i < childCount; i++)
					{
						this._arrayOfElements[i] = component.content.GetChild(i).gameObject;
					}
				}
			}
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x000BE448 File Offset: 0x000BC848
		public void Start()
		{
			if (this._arrayOfElements.Length < 1)
			{
				Debug.Log("No child content found, exiting..");
				return;
			}
			this.elementLength = this._arrayOfElements.Length;
			this.distance = new float[this.elementLength];
			this.distReposition = new float[this.elementLength];
			this.deltaY = this._arrayOfElements[0].GetComponent<RectTransform>().rect.height * (float)this.elementLength / 3f * 2f;
			Vector2 anchoredPosition = new Vector2(this._scrollingPanel.anchoredPosition.x, -this.deltaY);
			this._scrollingPanel.anchoredPosition = anchoredPosition;
			for (int i = 0; i < this._arrayOfElements.Length; i++)
			{
				this.AddListener(this._arrayOfElements[i], i);
			}
			if (this.ScrollUpButton)
			{
				this.ScrollUpButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.<Start>m__0));
			}
			if (this.ScrollDownButton)
			{
				this.ScrollDownButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.<Start>m__1));
			}
			if (this.StartingIndex > -1)
			{
				this.StartingIndex = ((this.StartingIndex <= this._arrayOfElements.Length) ? this.StartingIndex : (this._arrayOfElements.Length - 1));
				this.SnapToElement(this.StartingIndex);
			}
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000BE5D0 File Offset: 0x000BC9D0
		private void AddListener(GameObject button, int index)
		{
			UIVerticalScroller.<AddListener>c__AnonStorey0 <AddListener>c__AnonStorey = new UIVerticalScroller.<AddListener>c__AnonStorey0();
			<AddListener>c__AnonStorey.index = index;
			<AddListener>c__AnonStorey.$this = this;
			button.GetComponent<Button>().onClick.AddListener(new UnityAction(<AddListener>c__AnonStorey.<>m__0));
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x000BE60D File Offset: 0x000BCA0D
		private void DoSomething(int index)
		{
			if (this.ButtonClicked != null)
			{
				this.ButtonClicked.Invoke(index);
			}
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x000BE628 File Offset: 0x000BCA28
		public void Update()
		{
			if (this._arrayOfElements.Length < 1)
			{
				return;
			}
			for (int i = 0; i < this.elementLength; i++)
			{
				this.distReposition[i] = this._center.GetComponent<RectTransform>().position.y - this._arrayOfElements[i].GetComponent<RectTransform>().position.y;
				this.distance[i] = Mathf.Abs(this.distReposition[i]);
				float num = Mathf.Max(0.7f, 1f / (1f + this.distance[i] / 200f));
				this._arrayOfElements[i].GetComponent<RectTransform>().transform.localScale = new Vector3(num, num, 1f);
			}
			float num2 = Mathf.Min(this.distance);
			for (int j = 0; j < this.elementLength; j++)
			{
				this._arrayOfElements[j].GetComponent<CanvasGroup>().interactable = false;
				if (num2 == this.distance[j])
				{
					this.minElementsNum = j;
					this._arrayOfElements[j].GetComponent<CanvasGroup>().interactable = true;
					this.result = this._arrayOfElements[j].GetComponentInChildren<Text>().text;
				}
			}
			this.ScrollingElements(-this._arrayOfElements[this.minElementsNum].GetComponent<RectTransform>().anchoredPosition.y);
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x000BE79C File Offset: 0x000BCB9C
		private void ScrollingElements(float position)
		{
			float y = Mathf.Lerp(this._scrollingPanel.anchoredPosition.y, position, Time.deltaTime * 1f);
			Vector2 anchoredPosition = new Vector2(this._scrollingPanel.anchoredPosition.x, y);
			this._scrollingPanel.anchoredPosition = anchoredPosition;
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x000BE7F5 File Offset: 0x000BCBF5
		public string GetResults()
		{
			return this.result;
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000BE800 File Offset: 0x000BCC00
		public void SnapToElement(int element)
		{
			float num = this._arrayOfElements[0].GetComponent<RectTransform>().rect.height * (float)element;
			Vector2 anchoredPosition = new Vector2(this._scrollingPanel.anchoredPosition.x, -num);
			this._scrollingPanel.anchoredPosition = anchoredPosition;
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x000BE854 File Offset: 0x000BCC54
		public void ScrollUp()
		{
			float num = this._arrayOfElements[0].GetComponent<RectTransform>().rect.height / 1.2f;
			Vector2 b = new Vector2(this._scrollingPanel.anchoredPosition.x, this._scrollingPanel.anchoredPosition.y - num);
			this._scrollingPanel.anchoredPosition = Vector2.Lerp(this._scrollingPanel.anchoredPosition, b, 1f);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x000BE8D4 File Offset: 0x000BCCD4
		public void ScrollDown()
		{
			float num = this._arrayOfElements[0].GetComponent<RectTransform>().rect.height / 1.2f;
			Vector2 anchoredPosition = new Vector2(this._scrollingPanel.anchoredPosition.x, this._scrollingPanel.anchoredPosition.y + num);
			this._scrollingPanel.anchoredPosition = anchoredPosition;
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x000BE93E File Offset: 0x000BCD3E
		[CompilerGenerated]
		private void <Start>m__0()
		{
			this.ScrollUp();
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x000BE946 File Offset: 0x000BCD46
		[CompilerGenerated]
		private void <Start>m__1()
		{
			this.ScrollDown();
		}

		// Token: 0x04001BD6 RID: 7126
		[Tooltip("Scrollable area (content of desired ScrollRect)")]
		public RectTransform _scrollingPanel;

		// Token: 0x04001BD7 RID: 7127
		[Tooltip("Elements to populate inside the scroller")]
		public GameObject[] _arrayOfElements;

		// Token: 0x04001BD8 RID: 7128
		[Tooltip("Center display area (position of zoomed content)")]
		public RectTransform _center;

		// Token: 0x04001BD9 RID: 7129
		[Tooltip("Select the item to be in center on start. (optional)")]
		public int StartingIndex = -1;

		// Token: 0x04001BDA RID: 7130
		[Tooltip("Button to go to the next page. (optional)")]
		public GameObject ScrollUpButton;

		// Token: 0x04001BDB RID: 7131
		[Tooltip("Button to go to the previous page. (optional)")]
		public GameObject ScrollDownButton;

		// Token: 0x04001BDC RID: 7132
		[Tooltip("Event fired when a specific item is clicked, exposes index number of item. (optional)")]
		public UnityEvent<int> ButtonClicked;

		// Token: 0x04001BDD RID: 7133
		private float[] distReposition;

		// Token: 0x04001BDE RID: 7134
		private float[] distance;

		// Token: 0x04001BDF RID: 7135
		private int minElementsNum;

		// Token: 0x04001BE0 RID: 7136
		private int elementLength;

		// Token: 0x04001BE1 RID: 7137
		private float deltaY;

		// Token: 0x04001BE2 RID: 7138
		private string result;

		// Token: 0x02000F74 RID: 3956
		[CompilerGenerated]
		private sealed class <AddListener>c__AnonStorey0
		{
			// Token: 0x060073E9 RID: 29673 RVA: 0x000BE94E File Offset: 0x000BCD4E
			public <AddListener>c__AnonStorey0()
			{
			}

			// Token: 0x060073EA RID: 29674 RVA: 0x000BE956 File Offset: 0x000BCD56
			internal void <>m__0()
			{
				this.$this.DoSomething(this.index);
			}

			// Token: 0x04006803 RID: 26627
			internal int index;

			// Token: 0x04006804 RID: 26628
			internal UIVerticalScroller $this;
		}
	}
}
