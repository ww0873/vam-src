using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using Battlehub.RTSaveLoad.PersistentObjects.Networking.Match;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200018B RID: 395
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1069, typeof(PersistentNetworkMatch))]
	[ProtoInclude(1070, typeof(PersistentEventTrigger))]
	[ProtoInclude(1071, typeof(PersistentUIBehaviour))]
	[Serializable]
	public class PersistentMonoBehaviour : PersistentBehaviour
	{
		// Token: 0x06000887 RID: 2183 RVA: 0x000315E9 File Offset: 0x0002F9E9
		public PersistentMonoBehaviour()
		{
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000315F4 File Offset: 0x0002F9F4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			MonoBehaviour monoBehaviour = (MonoBehaviour)obj;
			monoBehaviour.useGUILayout = this.useGUILayout;
			return monoBehaviour;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00031628 File Offset: 0x0002FA28
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			MonoBehaviour monoBehaviour = (MonoBehaviour)obj;
			this.useGUILayout = monoBehaviour.useGUILayout;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00031656 File Offset: 0x0002FA56
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400094A RID: 2378
		public bool useGUILayout;
	}
}
