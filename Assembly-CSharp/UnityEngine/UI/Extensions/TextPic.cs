using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004E3 RID: 1251
	[AddComponentMenu("UI/Extensions/TextPic")]
	[ExecuteInEditMode]
	public class TextPic : Text, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x06001FAE RID: 8110 RVA: 0x000B39B8 File Offset: 0x000B1DB8
		public TextPic()
		{
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001FAF RID: 8111 RVA: 0x000B3A4B File Offset: 0x000B1E4B
		// (set) Token: 0x06001FB0 RID: 8112 RVA: 0x000B3A53 File Offset: 0x000B1E53
		public bool AllowClickParents
		{
			get
			{
				return this.m_ClickParents;
			}
			set
			{
				this.m_ClickParents = value;
			}
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000B3A5C File Offset: 0x000B1E5C
		public override void SetVerticesDirty()
		{
			base.SetVerticesDirty();
			this.UpdateQuadImage();
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x000B3A6C File Offset: 0x000B1E6C
		private new void Start()
		{
			this.button = base.GetComponentInParent<Button>();
			if (this.button != null)
			{
				CanvasGroup canvasGroup = base.GetComponent<CanvasGroup>();
				if (canvasGroup == null)
				{
					canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
				}
				canvasGroup.blocksRaycasts = false;
				this.highlightselectable = canvasGroup.GetComponent<Selectable>();
			}
			else
			{
				this.highlightselectable = base.GetComponent<Selectable>();
			}
			this.Reset_m_HrefInfos();
			base.Start();
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x000B3AE8 File Offset: 0x000B1EE8
		protected void UpdateQuadImage()
		{
			this.m_OutputText = this.GetOutputText();
			this.m_ImagesVertexIndex.Clear();
			IEnumerator enumerator = TextPic.s_Regex.Matches(this.m_OutputText).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Match match = (Match)obj;
					int index = match.Index;
					int item = index * 4 + 3;
					this.m_ImagesVertexIndex.Add(item);
					List<Image> imagesPool = this.m_ImagesPool;
					if (TextPic.<>f__am$cache0 == null)
					{
						TextPic.<>f__am$cache0 = new Predicate<Image>(TextPic.<UpdateQuadImage>m__0);
					}
					imagesPool.RemoveAll(TextPic.<>f__am$cache0);
					if (this.m_ImagesPool.Count == 0)
					{
						base.GetComponentsInChildren<Image>(this.m_ImagesPool);
					}
					if (this.m_ImagesVertexIndex.Count > this.m_ImagesPool.Count)
					{
						GameObject gameObject = DefaultControls.CreateImage(default(DefaultControls.Resources));
						gameObject.layer = base.gameObject.layer;
						RectTransform rectTransform = gameObject.transform as RectTransform;
						if (rectTransform)
						{
							rectTransform.SetParent(base.rectTransform);
							rectTransform.localPosition = Vector3.zero;
							rectTransform.localRotation = Quaternion.identity;
							rectTransform.localScale = Vector3.one;
						}
						this.m_ImagesPool.Add(gameObject.GetComponent<Image>());
					}
					string value = match.Groups[1].Value;
					Image image = this.m_ImagesPool[this.m_ImagesVertexIndex.Count - 1];
					Vector2 b = Vector2.zero;
					if ((image.sprite == null || image.sprite.name != value) && this.inspectorIconList != null && this.inspectorIconList.Length > 0)
					{
						foreach (TextPic.IconName iconName in this.inspectorIconList)
						{
							if (iconName.name == value)
							{
								image.sprite = iconName.sprite;
								image.rectTransform.sizeDelta = new Vector2((float)base.fontSize * this.ImageScalingFactor * iconName.scale.x, (float)base.fontSize * this.ImageScalingFactor * iconName.scale.y);
								b = iconName.offset;
								break;
							}
						}
					}
					image.enabled = true;
					if (this.positions.Count == this.m_ImagesPool.Count)
					{
						List<Vector2> list;
						int index2;
						image.rectTransform.anchoredPosition = ((list = this.positions)[index2 = this.m_ImagesVertexIndex.Count - 1] = list[index2] + b);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			for (int j = this.m_ImagesVertexIndex.Count; j < this.m_ImagesPool.Count; j++)
			{
				if (this.m_ImagesPool[j])
				{
					this.m_ImagesPool[j].gameObject.SetActive(false);
					this.m_ImagesPool[j].gameObject.hideFlags = HideFlags.HideAndDontSave;
					this.culled_ImagesPool.Add(this.m_ImagesPool[j].gameObject);
					this.m_ImagesPool.Remove(this.m_ImagesPool[j]);
				}
			}
			if (this.culled_ImagesPool.Count > 1)
			{
				this.clearImages = true;
			}
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x000B3EB0 File Offset: 0x000B22B0
		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			string text = this.m_Text;
			this.m_Text = this.GetOutputText();
			base.OnPopulateMesh(toFill);
			this.m_Text = text;
			this.positions.Clear();
			UIVertex vertex = default(UIVertex);
			for (int i = 0; i < this.m_ImagesVertexIndex.Count; i++)
			{
				int num = this.m_ImagesVertexIndex[i];
				RectTransform rectTransform = this.m_ImagesPool[i].rectTransform;
				Vector2 sizeDelta = rectTransform.sizeDelta;
				if (num < toFill.currentVertCount)
				{
					toFill.PopulateUIVertex(ref vertex, num);
					this.positions.Add(new Vector2(vertex.position.x + sizeDelta.x / 2f, vertex.position.y + sizeDelta.y / 2f) + this.imageOffset);
					toFill.PopulateUIVertex(ref vertex, num - 3);
					Vector3 position = vertex.position;
					int j = num;
					int num2 = num - 3;
					while (j > num2)
					{
						toFill.PopulateUIVertex(ref vertex, num);
						vertex.position = position;
						toFill.SetUIVertex(vertex, j);
						j--;
					}
				}
			}
			if (this.m_ImagesVertexIndex.Count != 0)
			{
				this.m_ImagesVertexIndex.Clear();
			}
			foreach (TextPic.HrefInfo hrefInfo in this.m_HrefInfos)
			{
				hrefInfo.boxes.Clear();
				if (hrefInfo.startIndex < toFill.currentVertCount)
				{
					toFill.PopulateUIVertex(ref vertex, hrefInfo.startIndex);
					Vector3 position2 = vertex.position;
					Bounds bounds = new Bounds(position2, Vector3.zero);
					int k = hrefInfo.startIndex;
					int endIndex = hrefInfo.endIndex;
					while (k < endIndex)
					{
						if (k >= toFill.currentVertCount)
						{
							break;
						}
						toFill.PopulateUIVertex(ref vertex, k);
						position2 = vertex.position;
						if (position2.x < bounds.min.x)
						{
							hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
							bounds = new Bounds(position2, Vector3.zero);
						}
						else
						{
							bounds.Encapsulate(position2);
						}
						k++;
					}
					hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
				}
			}
			this.UpdateQuadImage();
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001FB5 RID: 8117 RVA: 0x000B417C File Offset: 0x000B257C
		// (set) Token: 0x06001FB6 RID: 8118 RVA: 0x000B4184 File Offset: 0x000B2584
		public TextPic.HrefClickEvent onHrefClick
		{
			get
			{
				return this.m_OnHrefClick;
			}
			set
			{
				this.m_OnHrefClick = value;
			}
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x000B4190 File Offset: 0x000B2590
		protected string GetOutputText()
		{
			TextPic.s_TextBuilder.Length = 0;
			int num = 0;
			this.fixedString = this.text;
			if (this.inspectorIconList != null && this.inspectorIconList.Length > 0)
			{
				foreach (TextPic.IconName iconName in this.inspectorIconList)
				{
					if (iconName.name != null && iconName.name != string.Empty)
					{
						this.fixedString = this.fixedString.Replace(iconName.name, string.Concat(new object[]
						{
							"<quad name=",
							iconName.name,
							" size=",
							base.fontSize,
							" width=1 />"
						}));
					}
				}
			}
			int num2 = 0;
			IEnumerator enumerator = TextPic.s_HrefRegex.Matches(this.fixedString).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Match match = (Match)obj;
					TextPic.s_TextBuilder.Append(this.fixedString.Substring(num, match.Index - num));
					TextPic.s_TextBuilder.Append("<color=" + this.hyperlinkColor + ">");
					Group group = match.Groups[1];
					if (this.isCreating_m_HrefInfos)
					{
						TextPic.HrefInfo item = new TextPic.HrefInfo
						{
							startIndex = TextPic.s_TextBuilder.Length * 4,
							endIndex = (TextPic.s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
							name = group.Value
						};
						this.m_HrefInfos.Add(item);
					}
					else if (this.m_HrefInfos.Count > 0)
					{
						this.m_HrefInfos[num2].startIndex = TextPic.s_TextBuilder.Length * 4;
						this.m_HrefInfos[num2].endIndex = (TextPic.s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3;
						num2++;
					}
					TextPic.s_TextBuilder.Append(match.Groups[2].Value);
					TextPic.s_TextBuilder.Append("</color>");
					num = match.Index + match.Length;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (this.isCreating_m_HrefInfos)
			{
				this.isCreating_m_HrefInfos = false;
			}
			TextPic.s_TextBuilder.Append(this.fixedString.Substring(num, this.fixedString.Length - num));
			return TextPic.s_TextBuilder.ToString();
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x000B4488 File Offset: 0x000B2888
		public void OnPointerClick(PointerEventData eventData)
		{
			Vector2 point;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, eventData.position, eventData.pressEventCamera, out point);
			foreach (TextPic.HrefInfo hrefInfo in this.m_HrefInfos)
			{
				List<Rect> boxes = hrefInfo.boxes;
				for (int i = 0; i < boxes.Count; i++)
				{
					if (boxes[i].Contains(point))
					{
						this.m_OnHrefClick.Invoke(hrefInfo.name);
						return;
					}
				}
			}
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x000B4548 File Offset: 0x000B2948
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.m_ImagesPool.Count >= 1)
			{
				foreach (Image image in this.m_ImagesPool)
				{
					if (this.highlightselectable != null && this.highlightselectable.isActiveAndEnabled)
					{
						image.color = this.highlightselectable.colors.highlightedColor;
					}
				}
			}
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x000B45E8 File Offset: 0x000B29E8
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.m_ImagesPool.Count >= 1)
			{
				foreach (Image image in this.m_ImagesPool)
				{
					if (this.highlightselectable != null && this.highlightselectable.isActiveAndEnabled)
					{
						image.color = this.highlightselectable.colors.normalColor;
					}
					else
					{
						image.color = this.color;
					}
				}
			}
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x000B469C File Offset: 0x000B2A9C
		public void OnSelect(BaseEventData eventData)
		{
			if (this.m_ImagesPool.Count >= 1)
			{
				foreach (Image image in this.m_ImagesPool)
				{
					if (this.highlightselectable != null && this.highlightselectable.isActiveAndEnabled)
					{
						image.color = this.highlightselectable.colors.highlightedColor;
					}
				}
			}
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x000B473C File Offset: 0x000B2B3C
		private void Update()
		{
			object obj = this.thisLock;
			lock (obj)
			{
				if (this.clearImages)
				{
					for (int i = 0; i < this.culled_ImagesPool.Count; i++)
					{
						Object.DestroyImmediate(this.culled_ImagesPool[i]);
					}
					this.culled_ImagesPool.Clear();
					this.clearImages = false;
				}
			}
			if (this.previousText != this.text)
			{
				this.Reset_m_HrefInfos();
			}
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x000B47D8 File Offset: 0x000B2BD8
		private void Reset_m_HrefInfos()
		{
			this.previousText = this.text;
			this.m_HrefInfos.Clear();
			this.isCreating_m_HrefInfos = true;
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x000B47F8 File Offset: 0x000B2BF8
		// Note: this type is marked as 'beforefieldinit'.
		static TextPic()
		{
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x000B4826 File Offset: 0x000B2C26
		[CompilerGenerated]
		private static bool <UpdateQuadImage>m__0(Image image)
		{
			return image == null;
		}

		// Token: 0x04001A9B RID: 6811
		private readonly List<Image> m_ImagesPool = new List<Image>();

		// Token: 0x04001A9C RID: 6812
		private readonly List<GameObject> culled_ImagesPool = new List<GameObject>();

		// Token: 0x04001A9D RID: 6813
		private bool clearImages;

		// Token: 0x04001A9E RID: 6814
		private Object thisLock = new Object();

		// Token: 0x04001A9F RID: 6815
		private readonly List<int> m_ImagesVertexIndex = new List<int>();

		// Token: 0x04001AA0 RID: 6816
		private static readonly Regex s_Regex = new Regex("<quad name=(.+?) size=(\\d*\\.?\\d+%?) width=(\\d*\\.?\\d+%?) />", RegexOptions.Singleline);

		// Token: 0x04001AA1 RID: 6817
		private string fixedString;

		// Token: 0x04001AA2 RID: 6818
		[SerializeField]
		[Tooltip("Allow click events to be received by parents, (default) blocks")]
		private bool m_ClickParents;

		// Token: 0x04001AA3 RID: 6819
		private string m_OutputText;

		// Token: 0x04001AA4 RID: 6820
		public TextPic.IconName[] inspectorIconList;

		// Token: 0x04001AA5 RID: 6821
		[Tooltip("Global scaling factor for all images")]
		public float ImageScalingFactor = 1f;

		// Token: 0x04001AA6 RID: 6822
		public string hyperlinkColor = "blue";

		// Token: 0x04001AA7 RID: 6823
		[SerializeField]
		public Vector2 imageOffset = Vector2.zero;

		// Token: 0x04001AA8 RID: 6824
		private Button button;

		// Token: 0x04001AA9 RID: 6825
		private Selectable highlightselectable;

		// Token: 0x04001AAA RID: 6826
		private List<Vector2> positions = new List<Vector2>();

		// Token: 0x04001AAB RID: 6827
		private string previousText = string.Empty;

		// Token: 0x04001AAC RID: 6828
		public bool isCreating_m_HrefInfos = true;

		// Token: 0x04001AAD RID: 6829
		private readonly List<TextPic.HrefInfo> m_HrefInfos = new List<TextPic.HrefInfo>();

		// Token: 0x04001AAE RID: 6830
		private static readonly StringBuilder s_TextBuilder = new StringBuilder();

		// Token: 0x04001AAF RID: 6831
		private static readonly Regex s_HrefRegex = new Regex("<a href=([^>\\n\\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

		// Token: 0x04001AB0 RID: 6832
		[SerializeField]
		private TextPic.HrefClickEvent m_OnHrefClick = new TextPic.HrefClickEvent();

		// Token: 0x04001AB1 RID: 6833
		[CompilerGenerated]
		private static Predicate<Image> <>f__am$cache0;

		// Token: 0x020004E4 RID: 1252
		[Serializable]
		public struct IconName
		{
			// Token: 0x04001AB2 RID: 6834
			public string name;

			// Token: 0x04001AB3 RID: 6835
			public Sprite sprite;

			// Token: 0x04001AB4 RID: 6836
			public Vector2 offset;

			// Token: 0x04001AB5 RID: 6837
			public Vector2 scale;
		}

		// Token: 0x020004E5 RID: 1253
		[Serializable]
		public class HrefClickEvent : UnityEvent<string>
		{
			// Token: 0x06001FC0 RID: 8128 RVA: 0x000B482F File Offset: 0x000B2C2F
			public HrefClickEvent()
			{
			}
		}

		// Token: 0x020004E6 RID: 1254
		private class HrefInfo
		{
			// Token: 0x06001FC1 RID: 8129 RVA: 0x000B4837 File Offset: 0x000B2C37
			public HrefInfo()
			{
			}

			// Token: 0x04001AB6 RID: 6838
			public int startIndex;

			// Token: 0x04001AB7 RID: 6839
			public int endIndex;

			// Token: 0x04001AB8 RID: 6840
			public string name;

			// Token: 0x04001AB9 RID: 6841
			public readonly List<Rect> boxes = new List<Rect>();
		}
	}
}
