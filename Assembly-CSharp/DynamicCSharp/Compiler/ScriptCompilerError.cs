using System;

namespace DynamicCSharp.Compiler
{
	// Token: 0x020002D4 RID: 724
	internal struct ScriptCompilerError
	{
		// Token: 0x04000EDF RID: 3807
		public string errorCode;

		// Token: 0x04000EE0 RID: 3808
		public string errorText;

		// Token: 0x04000EE1 RID: 3809
		public string fileName;

		// Token: 0x04000EE2 RID: 3810
		public int line;

		// Token: 0x04000EE3 RID: 3811
		public int column;

		// Token: 0x04000EE4 RID: 3812
		public bool isWarning;
	}
}
