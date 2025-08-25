using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001C2 RID: 450
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentStateMachineBehaviour : PersistentScriptableObject
	{
		// Token: 0x06000939 RID: 2361 RVA: 0x00039475 File Offset: 0x00037875
		public PersistentStateMachineBehaviour()
		{
		}
	}
}
