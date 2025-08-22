using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000174 RID: 372
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentGUIStyle : PersistentData
	{
		// Token: 0x06000829 RID: 2089 RVA: 0x000354D3 File Offset: 0x000338D3
		public PersistentGUIStyle()
		{
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x000354DC File Offset: 0x000338DC
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			GUIStyle guistyle = (GUIStyle)obj;
			guistyle.normal = base.Write<GUIStyleState>(guistyle.normal, this.normal, objects);
			guistyle.hover = base.Write<GUIStyleState>(guistyle.hover, this.hover, objects);
			guistyle.active = base.Write<GUIStyleState>(guistyle.active, this.active, objects);
			guistyle.onNormal = base.Write<GUIStyleState>(guistyle.onNormal, this.onNormal, objects);
			guistyle.onHover = base.Write<GUIStyleState>(guistyle.onHover, this.onHover, objects);
			guistyle.onActive = base.Write<GUIStyleState>(guistyle.onActive, this.onActive, objects);
			guistyle.focused = base.Write<GUIStyleState>(guistyle.focused, this.focused, objects);
			guistyle.onFocused = base.Write<GUIStyleState>(guistyle.onFocused, this.onFocused, objects);
			guistyle.border = this.border;
			guistyle.margin = this.margin;
			guistyle.padding = this.padding;
			guistyle.overflow = this.overflow;
			guistyle.font = (Font)objects.Get(this.font);
			guistyle.name = this.name;
			guistyle.imagePosition = (ImagePosition)this.imagePosition;
			guistyle.alignment = (TextAnchor)this.alignment;
			guistyle.wordWrap = this.wordWrap;
			guistyle.clipping = (TextClipping)this.clipping;
			guistyle.contentOffset = this.contentOffset;
			guistyle.fixedWidth = this.fixedWidth;
			guistyle.fixedHeight = this.fixedHeight;
			guistyle.stretchWidth = this.stretchWidth;
			guistyle.stretchHeight = this.stretchHeight;
			guistyle.fontSize = this.fontSize;
			guistyle.fontStyle = (FontStyle)this.fontStyle;
			guistyle.richText = this.richText;
			return guistyle;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x000356B0 File Offset: 0x00033AB0
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			GUIStyle guistyle = (GUIStyle)obj;
			this.normal = base.Read<PersistentGUIStyleState>(this.normal, guistyle.normal);
			this.hover = base.Read<PersistentGUIStyleState>(this.hover, guistyle.hover);
			this.active = base.Read<PersistentGUIStyleState>(this.active, guistyle.active);
			this.onNormal = base.Read<PersistentGUIStyleState>(this.onNormal, guistyle.onNormal);
			this.onHover = base.Read<PersistentGUIStyleState>(this.onHover, guistyle.onHover);
			this.onActive = base.Read<PersistentGUIStyleState>(this.onActive, guistyle.onActive);
			this.focused = base.Read<PersistentGUIStyleState>(this.focused, guistyle.focused);
			this.onFocused = base.Read<PersistentGUIStyleState>(this.onFocused, guistyle.onFocused);
			this.border = guistyle.border;
			this.margin = guistyle.margin;
			this.padding = guistyle.padding;
			this.overflow = guistyle.overflow;
			this.font = guistyle.font.GetMappedInstanceID();
			this.name = guistyle.name;
			this.imagePosition = (uint)guistyle.imagePosition;
			this.alignment = (uint)guistyle.alignment;
			this.wordWrap = guistyle.wordWrap;
			this.clipping = (uint)guistyle.clipping;
			this.contentOffset = guistyle.contentOffset;
			this.fixedWidth = guistyle.fixedWidth;
			this.fixedHeight = guistyle.fixedHeight;
			this.stretchWidth = guistyle.stretchWidth;
			this.stretchHeight = guistyle.stretchHeight;
			this.fontSize = guistyle.fontSize;
			this.fontStyle = (uint)guistyle.fontStyle;
			this.richText = guistyle.richText;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00035870 File Offset: 0x00033C70
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyleState>(this.normal, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyleState>(this.hover, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyleState>(this.active, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyleState>(this.onNormal, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyleState>(this.onHover, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyleState>(this.onActive, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyleState>(this.focused, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyleState>(this.onFocused, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.font, dependencies, objects, allowNulls);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00035910 File Offset: 0x00033D10
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			GUIStyle guistyle = (GUIStyle)obj;
			base.GetDependencies<PersistentGUIStyleState>(this.normal, guistyle.normal, dependencies);
			base.GetDependencies<PersistentGUIStyleState>(this.hover, guistyle.hover, dependencies);
			base.GetDependencies<PersistentGUIStyleState>(this.active, guistyle.active, dependencies);
			base.GetDependencies<PersistentGUIStyleState>(this.onNormal, guistyle.onNormal, dependencies);
			base.GetDependencies<PersistentGUIStyleState>(this.onHover, guistyle.onHover, dependencies);
			base.GetDependencies<PersistentGUIStyleState>(this.onActive, guistyle.onActive, dependencies);
			base.GetDependencies<PersistentGUIStyleState>(this.focused, guistyle.focused, dependencies);
			base.GetDependencies<PersistentGUIStyleState>(this.onFocused, guistyle.onFocused, dependencies);
			base.AddDependency(guistyle.font, dependencies);
		}

		// Token: 0x040008C1 RID: 2241
		public PersistentGUIStyleState normal;

		// Token: 0x040008C2 RID: 2242
		public PersistentGUIStyleState hover;

		// Token: 0x040008C3 RID: 2243
		public PersistentGUIStyleState active;

		// Token: 0x040008C4 RID: 2244
		public PersistentGUIStyleState onNormal;

		// Token: 0x040008C5 RID: 2245
		public PersistentGUIStyleState onHover;

		// Token: 0x040008C6 RID: 2246
		public PersistentGUIStyleState onActive;

		// Token: 0x040008C7 RID: 2247
		public PersistentGUIStyleState focused;

		// Token: 0x040008C8 RID: 2248
		public PersistentGUIStyleState onFocused;

		// Token: 0x040008C9 RID: 2249
		public RectOffset border;

		// Token: 0x040008CA RID: 2250
		public RectOffset margin;

		// Token: 0x040008CB RID: 2251
		public RectOffset padding;

		// Token: 0x040008CC RID: 2252
		public RectOffset overflow;

		// Token: 0x040008CD RID: 2253
		public long font;

		// Token: 0x040008CE RID: 2254
		public string name;

		// Token: 0x040008CF RID: 2255
		public uint imagePosition;

		// Token: 0x040008D0 RID: 2256
		public uint alignment;

		// Token: 0x040008D1 RID: 2257
		public bool wordWrap;

		// Token: 0x040008D2 RID: 2258
		public uint clipping;

		// Token: 0x040008D3 RID: 2259
		public Vector2 contentOffset;

		// Token: 0x040008D4 RID: 2260
		public float fixedWidth;

		// Token: 0x040008D5 RID: 2261
		public float fixedHeight;

		// Token: 0x040008D6 RID: 2262
		public bool stretchWidth;

		// Token: 0x040008D7 RID: 2263
		public bool stretchHeight;

		// Token: 0x040008D8 RID: 2264
		public int fontSize;

		// Token: 0x040008D9 RID: 2265
		public uint fontStyle;

		// Token: 0x040008DA RID: 2266
		public bool richText;
	}
}
