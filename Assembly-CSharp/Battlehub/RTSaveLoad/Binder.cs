using System;
using System.Runtime.Serialization;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000230 RID: 560
	public class Binder : SerializationBinder
	{
		// Token: 0x06000BBB RID: 3003 RVA: 0x0004A1C7 File Offset: 0x000485C7
		public Binder()
		{
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0004A1CF File Offset: 0x000485CF
		public override Type BindToType(string assemblyName, string typeName)
		{
			return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
		}
	}
}
