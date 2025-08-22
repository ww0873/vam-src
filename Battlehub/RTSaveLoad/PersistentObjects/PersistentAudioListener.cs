using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000136 RID: 310
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioListener : PersistentBehaviour
	{
		// Token: 0x06000737 RID: 1847 RVA: 0x000319E9 File Offset: 0x0002FDE9
		public PersistentAudioListener()
		{
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x000319F4 File Offset: 0x0002FDF4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AudioListener audioListener = (AudioListener)obj;
			audioListener.velocityUpdateMode = (AudioVelocityUpdateMode)this.velocityUpdateMode;
			return audioListener;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00031A28 File Offset: 0x0002FE28
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AudioListener audioListener = (AudioListener)obj;
			this.velocityUpdateMode = (uint)audioListener.velocityUpdateMode;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00031A56 File Offset: 0x0002FE56
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000796 RID: 1942
		public uint velocityUpdateMode;
	}
}
