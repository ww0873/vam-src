using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.VR.WSA;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000156 RID: 342
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1041, typeof(PersistentBehaviour))]
	[ProtoInclude(1042, typeof(PersistentOcclusionArea))]
	[ProtoInclude(1043, typeof(PersistentOcclusionPortal))]
	[ProtoInclude(1044, typeof(PersistentMeshFilter))]
	[ProtoInclude(1045, typeof(PersistentRenderer))]
	[ProtoInclude(1046, typeof(PersistentLODGroup))]
	[ProtoInclude(1047, typeof(PersistentWindZone))]
	[ProtoInclude(1048, typeof(PersistentTransform))]
	[ProtoInclude(1049, typeof(PersistentParticleSystem))]
	[ProtoInclude(1050, typeof(PersistentRigidbody))]
	[ProtoInclude(1051, typeof(PersistentJoint))]
	[ProtoInclude(1052, typeof(PersistentCollider))]
	[ProtoInclude(1053, typeof(PersistentRigidbody2D))]
	[ProtoInclude(1054, typeof(PersistentCloth))]
	[ProtoInclude(1055, typeof(PersistentTree))]
	[ProtoInclude(1056, typeof(PersistentTextMesh))]
	[ProtoInclude(1057, typeof(PersistentCanvasGroup))]
	[ProtoInclude(1058, typeof(PersistentCanvasRenderer))]
	[ProtoInclude(1059, typeof(PersistentWorldAnchor))]
	[Serializable]
	public class PersistentComponent : PersistentObject
	{
		// Token: 0x060007AE RID: 1966 RVA: 0x00030C49 File Offset: 0x0002F049
		public PersistentComponent()
		{
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00030C54 File Offset: 0x0002F054
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Component component = (Component)obj;
			try
			{
				component.tag = this.tag;
			}
			catch (UnityException ex)
			{
				Debug.LogWarning(ex.Message);
			}
			return component;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00030CB0 File Offset: 0x0002F0B0
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Component component = (Component)obj;
			this.tag = component.tag;
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00030CDE File Offset: 0x0002F0DE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400084E RID: 2126
		public string tag;
	}
}
