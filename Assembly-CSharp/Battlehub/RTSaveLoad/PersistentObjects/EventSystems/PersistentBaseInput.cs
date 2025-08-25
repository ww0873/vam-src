using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlehub.RTSaveLoad.PersistentObjects.EventSystems
{
	// Token: 0x02000140 RID: 320
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentBaseInput : PersistentUIBehaviour
	{
		// Token: 0x06000759 RID: 1881 RVA: 0x000323CD File Offset: 0x000307CD
		public PersistentBaseInput()
		{
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x000323D8 File Offset: 0x000307D8
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			BaseInput baseInput = (BaseInput)obj;
			baseInput.imeCompositionMode = (IMECompositionMode)this.imeCompositionMode;
			baseInput.compositionCursorPos = this.compositionCursorPos;
			return baseInput;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00032418 File Offset: 0x00030818
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			BaseInput baseInput = (BaseInput)obj;
			this.imeCompositionMode = (uint)baseInput.imeCompositionMode;
			this.compositionCursorPos = baseInput.compositionCursorPos;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00032452 File Offset: 0x00030852
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040007D6 RID: 2006
		public uint imeCompositionMode;

		// Token: 0x040007D7 RID: 2007
		public Vector2 compositionCursorPos;
	}
}
