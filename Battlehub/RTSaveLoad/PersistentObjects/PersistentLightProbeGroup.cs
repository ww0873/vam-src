using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000182 RID: 386
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentLightProbeGroup : PersistentBehaviour
	{
		// Token: 0x06000862 RID: 2146 RVA: 0x000364CD File Offset: 0x000348CD
		public PersistentLightProbeGroup()
		{
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x000364D8 File Offset: 0x000348D8
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			return (LightProbeGroup)obj;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00036500 File Offset: 0x00034900
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			LightProbeGroup lightProbeGroup = (LightProbeGroup)obj;
			this.probePositions = lightProbeGroup.probePositions;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0003652E File Offset: 0x0003492E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000925 RID: 2341
		public Vector3[] probePositions;
	}
}
