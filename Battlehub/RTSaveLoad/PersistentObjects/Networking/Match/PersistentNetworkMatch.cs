using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Networking.Match;

namespace Battlehub.RTSaveLoad.PersistentObjects.Networking.Match
{
	// Token: 0x02000191 RID: 401
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentNetworkMatch : PersistentMonoBehaviour
	{
		// Token: 0x06000899 RID: 2201 RVA: 0x000370F5 File Offset: 0x000354F5
		public PersistentNetworkMatch()
		{
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00037100 File Offset: 0x00035500
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			NetworkMatch networkMatch = (NetworkMatch)obj;
			networkMatch.baseUri = this.baseUri;
			return networkMatch;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00037134 File Offset: 0x00035534
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			NetworkMatch networkMatch = (NetworkMatch)obj;
			this.baseUri = networkMatch.baseUri;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00037162 File Offset: 0x00035562
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400096B RID: 2411
		public Uri baseUri;
	}
}
