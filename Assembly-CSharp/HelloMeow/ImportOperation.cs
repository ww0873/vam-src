using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace HelloMeow
{
	// Token: 0x020002FB RID: 763
	public class ImportOperation : CustomYieldInstruction
	{
		// Token: 0x06001209 RID: 4617 RVA: 0x0006341E File Offset: 0x0006181E
		public ImportOperation(AudioImporter importer)
		{
			this.importer = importer;
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x0006342D File Offset: 0x0006182D
		public override bool keepWaiting
		{
			get
			{
				return !this.importer.isLoaded && !this.importer.isError;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600120B RID: 4619 RVA: 0x00063450 File Offset: 0x00061850
		// (set) Token: 0x0600120C RID: 4620 RVA: 0x00063458 File Offset: 0x00061858
		public AudioImporter importer
		{
			[CompilerGenerated]
			get
			{
				return this.<importer>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<importer>k__BackingField = value;
			}
		}

		// Token: 0x04000F6E RID: 3950
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private AudioImporter <importer>k__BackingField;
	}
}
