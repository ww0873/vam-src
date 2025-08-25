using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000535 RID: 1333
	public class UIPrimitiveBase : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter
	{
		// Token: 0x060021D2 RID: 8658 RVA: 0x000BF40F File Offset: 0x000BD80F
		protected UIPrimitiveBase()
		{
			base.useLegacyMeshGeneration = false;
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060021D3 RID: 8659 RVA: 0x000BF429 File Offset: 0x000BD829
		// (set) Token: 0x060021D4 RID: 8660 RVA: 0x000BF431 File Offset: 0x000BD831
		public Sprite sprite
		{
			get
			{
				return this.m_Sprite;
			}
			set
			{
				if (SetPropertyUtility.SetClass<Sprite>(ref this.m_Sprite, value))
				{
					this.GeneratedUVs();
				}
				this.SetAllDirty();
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060021D5 RID: 8661 RVA: 0x000BF450 File Offset: 0x000BD850
		// (set) Token: 0x060021D6 RID: 8662 RVA: 0x000BF458 File Offset: 0x000BD858
		public Sprite overrideSprite
		{
			get
			{
				return this.activeSprite;
			}
			set
			{
				if (SetPropertyUtility.SetClass<Sprite>(ref this.m_OverrideSprite, value))
				{
					this.GeneratedUVs();
				}
				this.SetAllDirty();
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060021D7 RID: 8663 RVA: 0x000BF477 File Offset: 0x000BD877
		protected Sprite activeSprite
		{
			get
			{
				return (!(this.m_OverrideSprite != null)) ? this.sprite : this.m_OverrideSprite;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060021D8 RID: 8664 RVA: 0x000BF49B File Offset: 0x000BD89B
		// (set) Token: 0x060021D9 RID: 8665 RVA: 0x000BF4A3 File Offset: 0x000BD8A3
		public float eventAlphaThreshold
		{
			get
			{
				return this.m_EventAlphaThreshold;
			}
			set
			{
				this.m_EventAlphaThreshold = value;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060021DA RID: 8666 RVA: 0x000BF4AC File Offset: 0x000BD8AC
		// (set) Token: 0x060021DB RID: 8667 RVA: 0x000BF4B4 File Offset: 0x000BD8B4
		public ResolutionMode ImproveResolution
		{
			get
			{
				return this.m_improveResolution;
			}
			set
			{
				this.m_improveResolution = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060021DC RID: 8668 RVA: 0x000BF4C3 File Offset: 0x000BD8C3
		// (set) Token: 0x060021DD RID: 8669 RVA: 0x000BF4CB File Offset: 0x000BD8CB
		public float Resoloution
		{
			get
			{
				return this.m_Resolution;
			}
			set
			{
				this.m_Resolution = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060021DE RID: 8670 RVA: 0x000BF4DA File Offset: 0x000BD8DA
		// (set) Token: 0x060021DF RID: 8671 RVA: 0x000BF4E2 File Offset: 0x000BD8E2
		public bool UseNativeSize
		{
			get
			{
				return this.m_useNativeSize;
			}
			set
			{
				this.m_useNativeSize = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060021E0 RID: 8672 RVA: 0x000BF4F1 File Offset: 0x000BD8F1
		public static Material defaultETC1GraphicMaterial
		{
			get
			{
				if (UIPrimitiveBase.s_ETC1DefaultUI == null)
				{
					UIPrimitiveBase.s_ETC1DefaultUI = Canvas.GetETC1SupportedCanvasMaterial();
				}
				return UIPrimitiveBase.s_ETC1DefaultUI;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060021E1 RID: 8673 RVA: 0x000BF514 File Offset: 0x000BD914
		public override Texture mainTexture
		{
			get
			{
				if (!(this.activeSprite == null))
				{
					return this.activeSprite.texture;
				}
				if (this.material != null && this.material.mainTexture != null)
				{
					return this.material.mainTexture;
				}
				return Graphic.s_WhiteTexture;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060021E2 RID: 8674 RVA: 0x000BF578 File Offset: 0x000BD978
		public bool hasBorder
		{
			get
			{
				return this.activeSprite != null && this.activeSprite.border.sqrMagnitude > 0f;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060021E3 RID: 8675 RVA: 0x000BF5B4 File Offset: 0x000BD9B4
		public float pixelsPerUnit
		{
			get
			{
				float num = 100f;
				if (this.activeSprite)
				{
					num = this.activeSprite.pixelsPerUnit;
				}
				float num2 = 100f;
				if (base.canvas)
				{
					num2 = base.canvas.referencePixelsPerUnit;
				}
				return num / num2;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060021E4 RID: 8676 RVA: 0x000BF608 File Offset: 0x000BDA08
		// (set) Token: 0x060021E5 RID: 8677 RVA: 0x000BF65F File Offset: 0x000BDA5F
		public override Material material
		{
			get
			{
				if (this.m_Material != null)
				{
					return this.m_Material;
				}
				if (this.activeSprite && this.activeSprite.associatedAlphaSplitTexture != null)
				{
					return UIPrimitiveBase.defaultETC1GraphicMaterial;
				}
				return this.defaultMaterial;
			}
			set
			{
				base.material = value;
			}
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x000BF668 File Offset: 0x000BDA68
		protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
		{
			UIVertex[] array = new UIVertex[4];
			for (int i = 0; i < vertices.Length; i++)
			{
				UIVertex simpleVert = UIVertex.simpleVert;
				simpleVert.color = this.color;
				simpleVert.position = vertices[i];
				simpleVert.uv0 = uvs[i];
				array[i] = simpleVert;
			}
			return array;
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x000BF6E4 File Offset: 0x000BDAE4
		protected Vector2[] IncreaseResolution(Vector2[] input)
		{
			List<Vector2> list = new List<Vector2>();
			ResolutionMode improveResolution = this.ImproveResolution;
			if (improveResolution != ResolutionMode.PerLine)
			{
				if (improveResolution == ResolutionMode.PerSegment)
				{
					for (int i = 0; i < input.Length - 1; i++)
					{
						Vector2 vector = input[i];
						list.Add(vector);
						Vector2 vector2 = input[i + 1];
						this.ResolutionToNativeSize(Vector2.Distance(vector, vector2));
						float num = 1f / this.m_Resolution;
						for (float num2 = 1f; num2 < this.m_Resolution; num2 += 1f)
						{
							list.Add(Vector2.Lerp(vector, vector2, num * num2));
						}
						list.Add(vector2);
					}
				}
			}
			else
			{
				float num3 = 0f;
				for (int j = 0; j < input.Length - 1; j++)
				{
					num3 += Vector2.Distance(input[j], input[j + 1]);
				}
				this.ResolutionToNativeSize(num3);
				float num = num3 / this.m_Resolution;
				int num4 = 0;
				for (int k = 0; k < input.Length - 1; k++)
				{
					Vector2 vector3 = input[k];
					list.Add(vector3);
					Vector2 vector4 = input[k + 1];
					float num5 = Vector2.Distance(vector3, vector4) / num;
					float num6 = 1f / num5;
					int num7 = 0;
					while ((float)num7 < num5)
					{
						list.Add(Vector2.Lerp(vector3, vector4, (float)num7 * num6));
						num4++;
						num7++;
					}
					list.Add(vector4);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x000BF8AE File Offset: 0x000BDCAE
		protected virtual void GeneratedUVs()
		{
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x000BF8B0 File Offset: 0x000BDCB0
		protected virtual void ResolutionToNativeSize(float distance)
		{
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x000BF8B2 File Offset: 0x000BDCB2
		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x000BF8B4 File Offset: 0x000BDCB4
		public virtual void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060021EC RID: 8684 RVA: 0x000BF8B6 File Offset: 0x000BDCB6
		public virtual float minWidth
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060021ED RID: 8685 RVA: 0x000BF8C0 File Offset: 0x000BDCC0
		public virtual float preferredWidth
		{
			get
			{
				if (this.overrideSprite == null)
				{
					return 0f;
				}
				return this.overrideSprite.rect.size.x / this.pixelsPerUnit;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060021EE RID: 8686 RVA: 0x000BF906 File Offset: 0x000BDD06
		public virtual float flexibleWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060021EF RID: 8687 RVA: 0x000BF90D File Offset: 0x000BDD0D
		public virtual float minHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060021F0 RID: 8688 RVA: 0x000BF914 File Offset: 0x000BDD14
		public virtual float preferredHeight
		{
			get
			{
				if (this.overrideSprite == null)
				{
					return 0f;
				}
				return this.overrideSprite.rect.size.y / this.pixelsPerUnit;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060021F1 RID: 8689 RVA: 0x000BF95A File Offset: 0x000BDD5A
		public virtual float flexibleHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060021F2 RID: 8690 RVA: 0x000BF961 File Offset: 0x000BDD61
		public virtual int layoutPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x000BF964 File Offset: 0x000BDD64
		public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			if (this.m_EventAlphaThreshold >= 1f)
			{
				return true;
			}
			Sprite overrideSprite = this.overrideSprite;
			if (overrideSprite == null)
			{
				return true;
			}
			Vector2 local;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, screenPoint, eventCamera, out local);
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			local.x += base.rectTransform.pivot.x * pixelAdjustedRect.width;
			local.y += base.rectTransform.pivot.y * pixelAdjustedRect.height;
			local = this.MapCoordinate(local, pixelAdjustedRect);
			Rect textureRect = overrideSprite.textureRect;
			Vector2 vector = new Vector2(local.x / textureRect.width, local.y / textureRect.height);
			float x = Mathf.Lerp(textureRect.x, textureRect.xMax, vector.x) / (float)overrideSprite.texture.width;
			float y = Mathf.Lerp(textureRect.y, textureRect.yMax, vector.y) / (float)overrideSprite.texture.height;
			bool result;
			try
			{
				result = (overrideSprite.texture.GetPixelBilinear(x, y).a >= this.m_EventAlphaThreshold);
			}
			catch (UnityException ex)
			{
				Debug.LogError("Using clickAlphaThreshold lower than 1 on Image whose sprite texture cannot be read. " + ex.Message + " Also make sure to disable sprite packing for this sprite.", this);
				result = true;
			}
			return result;
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x000BFAEC File Offset: 0x000BDEEC
		private Vector2 MapCoordinate(Vector2 local, Rect rect)
		{
			Rect rect2 = this.sprite.rect;
			return new Vector2(local.x * rect.width, local.y * rect.height);
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x000BFB28 File Offset: 0x000BDF28
		private Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
		{
			for (int i = 0; i <= 1; i++)
			{
				float num = border[i] + border[i + 2];
				if (rect.size[i] < num && num != 0f)
				{
					float num2 = rect.size[i] / num;
					ref Vector4 ptr = ref border;
					int index;
					border[index = i] = ptr[index] * num2;
					ptr = ref border;
					int index2;
					border[index2 = i + 2] = ptr[index2] * num2;
				}
			}
			return border;
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x000BFBC5 File Offset: 0x000BDFC5
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetAllDirty();
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x000BFBD3 File Offset: 0x000BDFD3
		// Note: this type is marked as 'beforefieldinit'.
		static UIPrimitiveBase()
		{
		}

		// Token: 0x04001C34 RID: 7220
		protected static Material s_ETC1DefaultUI;

		// Token: 0x04001C35 RID: 7221
		[SerializeField]
		private Sprite m_Sprite;

		// Token: 0x04001C36 RID: 7222
		[NonSerialized]
		private Sprite m_OverrideSprite;

		// Token: 0x04001C37 RID: 7223
		internal float m_EventAlphaThreshold = 1f;

		// Token: 0x04001C38 RID: 7224
		[SerializeField]
		private ResolutionMode m_improveResolution;

		// Token: 0x04001C39 RID: 7225
		[SerializeField]
		protected float m_Resolution;

		// Token: 0x04001C3A RID: 7226
		[SerializeField]
		private bool m_useNativeSize;
	}
}
