using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x020001C8 RID: 456
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentText : PersistentMaskableGraphic
	{
		// Token: 0x06000951 RID: 2385 RVA: 0x00039CCC File Offset: 0x000380CC
		public PersistentText()
		{
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00039CD4 File Offset: 0x000380D4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Text text = (Text)obj;
			text.font = (Font)objects.Get(this.font);
			text.text = this.text;
			text.supportRichText = this.supportRichText;
			text.resizeTextForBestFit = this.resizeTextForBestFit;
			text.resizeTextMinSize = this.resizeTextMinSize;
			text.resizeTextMaxSize = this.resizeTextMaxSize;
			text.alignment = (TextAnchor)this.alignment;
			text.alignByGeometry = this.alignByGeometry;
			text.fontSize = this.fontSize;
			text.horizontalOverflow = (HorizontalWrapMode)this.horizontalOverflow;
			text.verticalOverflow = (VerticalWrapMode)this.verticalOverflow;
			text.lineSpacing = this.lineSpacing;
			text.fontStyle = (FontStyle)this.fontStyle;
			return text;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x00039DA4 File Offset: 0x000381A4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Text text = (Text)obj;
			this.font = text.font.GetMappedInstanceID();
			this.text = text.text;
			this.supportRichText = text.supportRichText;
			this.resizeTextForBestFit = text.resizeTextForBestFit;
			this.resizeTextMinSize = text.resizeTextMinSize;
			this.resizeTextMaxSize = text.resizeTextMaxSize;
			this.alignment = (uint)text.alignment;
			this.alignByGeometry = text.alignByGeometry;
			this.fontSize = text.fontSize;
			this.horizontalOverflow = (uint)text.horizontalOverflow;
			this.verticalOverflow = (uint)text.verticalOverflow;
			this.lineSpacing = text.lineSpacing;
			this.fontStyle = (uint)text.fontStyle;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x00039E67 File Offset: 0x00038267
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.font, dependencies, objects, allowNulls);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x00039E84 File Offset: 0x00038284
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Text text = (Text)obj;
			base.AddDependency(text.font, dependencies);
		}

		// Token: 0x04000A76 RID: 2678
		public long font;

		// Token: 0x04000A77 RID: 2679
		public string text;

		// Token: 0x04000A78 RID: 2680
		public bool supportRichText;

		// Token: 0x04000A79 RID: 2681
		public bool resizeTextForBestFit;

		// Token: 0x04000A7A RID: 2682
		public int resizeTextMinSize;

		// Token: 0x04000A7B RID: 2683
		public int resizeTextMaxSize;

		// Token: 0x04000A7C RID: 2684
		public uint alignment;

		// Token: 0x04000A7D RID: 2685
		public bool alignByGeometry;

		// Token: 0x04000A7E RID: 2686
		public int fontSize;

		// Token: 0x04000A7F RID: 2687
		public uint horizontalOverflow;

		// Token: 0x04000A80 RID: 2688
		public uint verticalOverflow;

		// Token: 0x04000A81 RID: 2689
		public float lineSpacing;

		// Token: 0x04000A82 RID: 2690
		public uint fontStyle;
	}
}
