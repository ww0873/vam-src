using System;
using System.IO;

namespace MeshVR
{
	// Token: 0x02000C1E RID: 3102
	public interface IBinaryStorable
	{
		// Token: 0x06005A3F RID: 23103
		bool LoadFromBinaryFile(string path);

		// Token: 0x06005A40 RID: 23104
		bool LoadFromBinaryReader(BinaryReader reader);

		// Token: 0x06005A41 RID: 23105
		bool StoreToBinaryFile(string path);

		// Token: 0x06005A42 RID: 23106
		bool StoreToBinaryWriter(BinaryWriter writer);
	}
}
