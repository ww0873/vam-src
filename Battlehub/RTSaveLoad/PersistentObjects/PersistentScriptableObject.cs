using System;
using Battlehub.RTSaveLoad.PersistentObjects.Experimental.Rendering;
using Battlehub.RTSaveLoad.PersistentObjects.Networking.PlayerConnection;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001B2 RID: 434
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1000, typeof(PersistentStateMachineBehaviour))]
	[ProtoInclude(1001, typeof(PersistentGUISkin))]
	[ProtoInclude(1002, typeof(PersistentPlayerConnection))]
	[ProtoInclude(1003, typeof(PersistentRenderPipelineAsset))]
	[Serializable]
	public class PersistentScriptableObject : PersistentObject
	{
		// Token: 0x06000901 RID: 2305 RVA: 0x00034D29 File Offset: 0x00033129
		public PersistentScriptableObject()
		{
		}
	}
}
