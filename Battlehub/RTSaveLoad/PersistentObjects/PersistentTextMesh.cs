using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001CA RID: 458
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTextMesh : PersistentComponent
	{
		// Token: 0x06000957 RID: 2391 RVA: 0x00039EBC File Offset: 0x000382BC
		public PersistentTextMesh()
		{
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00039EC4 File Offset: 0x000382C4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			TextMesh textMesh = (TextMesh)obj;
			textMesh.text = this.text;
			textMesh.font = (Font)objects.Get(this.font);
			textMesh.fontSize = this.fontSize;
			textMesh.fontStyle = (FontStyle)this.fontStyle;
			textMesh.offsetZ = this.offsetZ;
			textMesh.alignment = (TextAlignment)this.alignment;
			textMesh.anchor = (TextAnchor)this.anchor;
			textMesh.characterSize = this.characterSize;
			textMesh.lineSpacing = this.lineSpacing;
			textMesh.tabSize = this.tabSize;
			textMesh.richText = this.richText;
			textMesh.color = this.color;
			return textMesh;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x00039F88 File Offset: 0x00038388
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			TextMesh textMesh = (TextMesh)obj;
			this.text = textMesh.text;
			this.font = textMesh.font.GetMappedInstanceID();
			this.fontSize = textMesh.fontSize;
			this.fontStyle = (uint)textMesh.fontStyle;
			this.offsetZ = textMesh.offsetZ;
			this.alignment = (uint)textMesh.alignment;
			this.anchor = (uint)textMesh.anchor;
			this.characterSize = textMesh.characterSize;
			this.lineSpacing = textMesh.lineSpacing;
			this.tabSize = textMesh.tabSize;
			this.richText = textMesh.richText;
			this.color = textMesh.color;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0003A03F File Offset: 0x0003843F
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.font, dependencies, objects, allowNulls);
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0003A05C File Offset: 0x0003845C
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			TextMesh textMesh = (TextMesh)obj;
			base.AddDependency(textMesh.font, dependencies);
		}

		// Token: 0x04000A83 RID: 2691
		public string text;

		// Token: 0x04000A84 RID: 2692
		public long font;

		// Token: 0x04000A85 RID: 2693
		public int fontSize;

		// Token: 0x04000A86 RID: 2694
		public uint fontStyle;

		// Token: 0x04000A87 RID: 2695
		public float offsetZ;

		// Token: 0x04000A88 RID: 2696
		public uint alignment;

		// Token: 0x04000A89 RID: 2697
		public uint anchor;

		// Token: 0x04000A8A RID: 2698
		public float characterSize;

		// Token: 0x04000A8B RID: 2699
		public float lineSpacing;

		// Token: 0x04000A8C RID: 2700
		public float tabSize;

		// Token: 0x04000A8D RID: 2701
		public bool richText;

		// Token: 0x04000A8E RID: 2702
		public Color color;
	}
}
