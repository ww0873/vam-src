using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000116 RID: 278
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentKeyframe : PersistentData
	{
		// Token: 0x060006B2 RID: 1714 RVA: 0x0002DB24 File Offset: 0x0002BF24
		public PersistentKeyframe()
		{
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0002DB2C File Offset: 0x0002BF2C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Keyframe keyframe = (Keyframe)obj;
			keyframe.time = this.time;
			keyframe.value = this.value;
			keyframe.inTangent = this.inTangent;
			keyframe.outTangent = this.outTangent;
			return keyframe;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0002DB8C File Offset: 0x0002BF8C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Keyframe keyframe = (Keyframe)obj;
			this.time = keyframe.time;
			this.value = keyframe.value;
			this.inTangent = keyframe.inTangent;
			this.outTangent = keyframe.outTangent;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0002DBE2 File Offset: 0x0002BFE2
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000680 RID: 1664
		public float time;

		// Token: 0x04000681 RID: 1665
		public float value;

		// Token: 0x04000682 RID: 1666
		public float inTangent;

		// Token: 0x04000683 RID: 1667
		public float outTangent;

		// Token: 0x04000684 RID: 1668
		public int tangentMode;
	}
}
