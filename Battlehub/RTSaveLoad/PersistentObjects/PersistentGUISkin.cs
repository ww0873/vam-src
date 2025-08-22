using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000173 RID: 371
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentGUISkin : PersistentScriptableObject
	{
		// Token: 0x06000824 RID: 2084 RVA: 0x00034D31 File Offset: 0x00033131
		public PersistentGUISkin()
		{
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00034D3C File Offset: 0x0003313C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			GUISkin guiskin = (GUISkin)obj;
			guiskin.font = (Font)objects.Get(this.font);
			guiskin.box = base.Write<GUIStyle>(guiskin.box, this.box, objects);
			guiskin.label = base.Write<GUIStyle>(guiskin.label, this.label, objects);
			guiskin.textField = base.Write<GUIStyle>(guiskin.textField, this.textField, objects);
			guiskin.textArea = base.Write<GUIStyle>(guiskin.textArea, this.textArea, objects);
			guiskin.button = base.Write<GUIStyle>(guiskin.button, this.button, objects);
			guiskin.toggle = base.Write<GUIStyle>(guiskin.toggle, this.toggle, objects);
			guiskin.window = base.Write<GUIStyle>(guiskin.window, this.window, objects);
			guiskin.horizontalSlider = base.Write<GUIStyle>(guiskin.horizontalSlider, this.horizontalSlider, objects);
			guiskin.horizontalSliderThumb = base.Write<GUIStyle>(guiskin.horizontalSliderThumb, this.horizontalSliderThumb, objects);
			guiskin.verticalSlider = base.Write<GUIStyle>(guiskin.verticalSlider, this.verticalSlider, objects);
			guiskin.verticalSliderThumb = base.Write<GUIStyle>(guiskin.verticalSliderThumb, this.verticalSliderThumb, objects);
			guiskin.horizontalScrollbar = base.Write<GUIStyle>(guiskin.horizontalScrollbar, this.horizontalScrollbar, objects);
			guiskin.horizontalScrollbarThumb = base.Write<GUIStyle>(guiskin.horizontalScrollbarThumb, this.horizontalScrollbarThumb, objects);
			guiskin.horizontalScrollbarLeftButton = base.Write<GUIStyle>(guiskin.horizontalScrollbarLeftButton, this.horizontalScrollbarLeftButton, objects);
			guiskin.horizontalScrollbarRightButton = base.Write<GUIStyle>(guiskin.horizontalScrollbarRightButton, this.horizontalScrollbarRightButton, objects);
			guiskin.verticalScrollbar = base.Write<GUIStyle>(guiskin.verticalScrollbar, this.verticalScrollbar, objects);
			guiskin.verticalScrollbarThumb = base.Write<GUIStyle>(guiskin.verticalScrollbarThumb, this.verticalScrollbarThumb, objects);
			guiskin.verticalScrollbarUpButton = base.Write<GUIStyle>(guiskin.verticalScrollbarUpButton, this.verticalScrollbarUpButton, objects);
			guiskin.verticalScrollbarDownButton = base.Write<GUIStyle>(guiskin.verticalScrollbarDownButton, this.verticalScrollbarDownButton, objects);
			guiskin.scrollView = base.Write<GUIStyle>(guiskin.scrollView, this.scrollView, objects);
			guiskin.customStyles = base.Write<GUIStyle>(guiskin.customStyles, this.customStyles, objects);
			return guiskin;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00034F88 File Offset: 0x00033388
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			GUISkin guiskin = (GUISkin)obj;
			this.font = guiskin.font.GetMappedInstanceID();
			this.box = base.Read<PersistentGUIStyle>(this.box, guiskin.box);
			this.label = base.Read<PersistentGUIStyle>(this.label, guiskin.label);
			this.textField = base.Read<PersistentGUIStyle>(this.textField, guiskin.textField);
			this.textArea = base.Read<PersistentGUIStyle>(this.textArea, guiskin.textArea);
			this.button = base.Read<PersistentGUIStyle>(this.button, guiskin.button);
			this.toggle = base.Read<PersistentGUIStyle>(this.toggle, guiskin.toggle);
			this.window = base.Read<PersistentGUIStyle>(this.window, guiskin.window);
			this.horizontalSlider = base.Read<PersistentGUIStyle>(this.horizontalSlider, guiskin.horizontalSlider);
			this.horizontalSliderThumb = base.Read<PersistentGUIStyle>(this.horizontalSliderThumb, guiskin.horizontalSliderThumb);
			this.verticalSlider = base.Read<PersistentGUIStyle>(this.verticalSlider, guiskin.verticalSlider);
			this.verticalSliderThumb = base.Read<PersistentGUIStyle>(this.verticalSliderThumb, guiskin.verticalSliderThumb);
			this.horizontalScrollbar = base.Read<PersistentGUIStyle>(this.horizontalScrollbar, guiskin.horizontalScrollbar);
			this.horizontalScrollbarThumb = base.Read<PersistentGUIStyle>(this.horizontalScrollbarThumb, guiskin.horizontalScrollbarThumb);
			this.horizontalScrollbarLeftButton = base.Read<PersistentGUIStyle>(this.horizontalScrollbarLeftButton, guiskin.horizontalScrollbarLeftButton);
			this.horizontalScrollbarRightButton = base.Read<PersistentGUIStyle>(this.horizontalScrollbarRightButton, guiskin.horizontalScrollbarRightButton);
			this.verticalScrollbar = base.Read<PersistentGUIStyle>(this.verticalScrollbar, guiskin.verticalScrollbar);
			this.verticalScrollbarThumb = base.Read<PersistentGUIStyle>(this.verticalScrollbarThumb, guiskin.verticalScrollbarThumb);
			this.verticalScrollbarUpButton = base.Read<PersistentGUIStyle>(this.verticalScrollbarUpButton, guiskin.verticalScrollbarUpButton);
			this.verticalScrollbarDownButton = base.Read<PersistentGUIStyle>(this.verticalScrollbarDownButton, guiskin.verticalScrollbarDownButton);
			this.scrollView = base.Read<PersistentGUIStyle>(this.scrollView, guiskin.scrollView);
			this.customStyles = base.Read<PersistentGUIStyle, GUIStyle>(this.customStyles, guiskin.customStyles);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x000351B4 File Offset: 0x000335B4
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.font, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.box, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.label, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.textField, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.textArea, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.button, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.toggle, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.window, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.horizontalSlider, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.horizontalSliderThumb, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.verticalSlider, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.verticalSliderThumb, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.horizontalScrollbar, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.horizontalScrollbarThumb, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.horizontalScrollbarLeftButton, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.horizontalScrollbarRightButton, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.verticalScrollbar, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.verticalScrollbarThumb, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.verticalScrollbarUpButton, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.verticalScrollbarDownButton, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.scrollView, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGUIStyle>(this.customStyles, dependencies, objects, allowNulls);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00035314 File Offset: 0x00033714
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			GUISkin guiskin = (GUISkin)obj;
			base.AddDependency(guiskin.font, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.box, guiskin.box, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.label, guiskin.label, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.textField, guiskin.textField, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.textArea, guiskin.textArea, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.button, guiskin.button, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.toggle, guiskin.toggle, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.window, guiskin.window, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.horizontalSlider, guiskin.horizontalSlider, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.horizontalSliderThumb, guiskin.horizontalSliderThumb, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.verticalSlider, guiskin.verticalSlider, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.verticalSliderThumb, guiskin.verticalSliderThumb, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.horizontalScrollbar, guiskin.horizontalScrollbar, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.horizontalScrollbarThumb, guiskin.horizontalScrollbarThumb, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.horizontalScrollbarLeftButton, guiskin.horizontalScrollbarLeftButton, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.horizontalScrollbarRightButton, guiskin.horizontalScrollbarRightButton, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.verticalScrollbar, guiskin.verticalScrollbar, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.verticalScrollbarThumb, guiskin.verticalScrollbarThumb, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.verticalScrollbarUpButton, guiskin.verticalScrollbarUpButton, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.verticalScrollbarDownButton, guiskin.verticalScrollbarDownButton, dependencies);
			base.GetDependencies<PersistentGUIStyle>(this.scrollView, guiskin.scrollView, dependencies);
			base.GetDependencies<PersistentGUIStyle, GUIStyle>(this.customStyles, guiskin.customStyles, dependencies);
		}

		// Token: 0x040008AB RID: 2219
		public long font;

		// Token: 0x040008AC RID: 2220
		public PersistentGUIStyle box;

		// Token: 0x040008AD RID: 2221
		public PersistentGUIStyle label;

		// Token: 0x040008AE RID: 2222
		public PersistentGUIStyle textField;

		// Token: 0x040008AF RID: 2223
		public PersistentGUIStyle textArea;

		// Token: 0x040008B0 RID: 2224
		public PersistentGUIStyle button;

		// Token: 0x040008B1 RID: 2225
		public PersistentGUIStyle toggle;

		// Token: 0x040008B2 RID: 2226
		public PersistentGUIStyle window;

		// Token: 0x040008B3 RID: 2227
		public PersistentGUIStyle horizontalSlider;

		// Token: 0x040008B4 RID: 2228
		public PersistentGUIStyle horizontalSliderThumb;

		// Token: 0x040008B5 RID: 2229
		public PersistentGUIStyle verticalSlider;

		// Token: 0x040008B6 RID: 2230
		public PersistentGUIStyle verticalSliderThumb;

		// Token: 0x040008B7 RID: 2231
		public PersistentGUIStyle horizontalScrollbar;

		// Token: 0x040008B8 RID: 2232
		public PersistentGUIStyle horizontalScrollbarThumb;

		// Token: 0x040008B9 RID: 2233
		public PersistentGUIStyle horizontalScrollbarLeftButton;

		// Token: 0x040008BA RID: 2234
		public PersistentGUIStyle horizontalScrollbarRightButton;

		// Token: 0x040008BB RID: 2235
		public PersistentGUIStyle verticalScrollbar;

		// Token: 0x040008BC RID: 2236
		public PersistentGUIStyle verticalScrollbarThumb;

		// Token: 0x040008BD RID: 2237
		public PersistentGUIStyle verticalScrollbarUpButton;

		// Token: 0x040008BE RID: 2238
		public PersistentGUIStyle verticalScrollbarDownButton;

		// Token: 0x040008BF RID: 2239
		public PersistentGUIStyle scrollView;

		// Token: 0x040008C0 RID: 2240
		public PersistentGUIStyle[] customStyles;
	}
}
