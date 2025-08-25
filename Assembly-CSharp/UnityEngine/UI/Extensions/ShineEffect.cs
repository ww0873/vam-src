using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000502 RID: 1282
	public class ShineEffect : MaskableGraphic
	{
		// Token: 0x06002052 RID: 8274 RVA: 0x000B8C7C File Offset: 0x000B707C
		public ShineEffect()
		{
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06002053 RID: 8275 RVA: 0x000B8C9A File Offset: 0x000B709A
		// (set) Token: 0x06002054 RID: 8276 RVA: 0x000B8CA2 File Offset: 0x000B70A2
		public float Yoffset
		{
			get
			{
				return this.yoffset;
			}
			set
			{
				this.SetVerticesDirty();
				this.yoffset = value;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06002055 RID: 8277 RVA: 0x000B8CB1 File Offset: 0x000B70B1
		// (set) Token: 0x06002056 RID: 8278 RVA: 0x000B8CB9 File Offset: 0x000B70B9
		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.SetAllDirty();
				this.width = value;
			}
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x000B8CC8 File Offset: 0x000B70C8
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			Vector4 vector = new Vector4(pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
			float num = (vector.w - vector.y) * 2f;
			Color32 color = this.color;
			vh.Clear();
			color.a = 0;
			vh.AddVert(new Vector3(vector.x - 50f, this.width * vector.y + this.yoffset * num), color, new Vector2(0f, 0f));
			vh.AddVert(new Vector3(vector.z + 50f, this.width * vector.y + this.yoffset * num), color, new Vector2(1f, 0f));
			color.a = (byte)(this.color.a * 255f);
			vh.AddVert(new Vector3(vector.x - 50f, this.width * (vector.y / 4f) + this.yoffset * num), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(vector.z + 50f, this.width * (vector.y / 4f) + this.yoffset * num), color, new Vector2(1f, 1f));
			color.a = (byte)(this.color.a * 255f);
			vh.AddVert(new Vector3(vector.x - 50f, this.width * (vector.w / 4f) + this.yoffset * num), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(vector.z + 50f, this.width * (vector.w / 4f) + this.yoffset * num), color, new Vector2(1f, 1f));
			color.a = (byte)(this.color.a * 255f);
			color.a = 0;
			vh.AddVert(new Vector3(vector.x - 50f, this.width * vector.w + this.yoffset * num), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(vector.z + 50f, this.width * vector.w + this.yoffset * num), color, new Vector2(1f, 1f));
			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 1);
			vh.AddTriangle(2, 3, 4);
			vh.AddTriangle(4, 5, 3);
			vh.AddTriangle(4, 5, 6);
			vh.AddTriangle(6, 7, 5);
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x000B8FE4 File Offset: 0x000B73E4
		public void Triangulate(VertexHelper vh)
		{
			int num = vh.currentVertCount - 2;
			Debug.Log(num);
			for (int i = 0; i <= num / 2 + 1; i += 2)
			{
				vh.AddTriangle(i, i + 1, i + 2);
				vh.AddTriangle(i + 2, i + 3, i + 1);
			}
		}

		// Token: 0x04001B15 RID: 6933
		[SerializeField]
		private float yoffset = -1f;

		// Token: 0x04001B16 RID: 6934
		[SerializeField]
		private float width = 1f;
	}
}
