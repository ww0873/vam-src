using System;
using System.IO;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006CA RID: 1738
	[Serializable]
	public class StreamingFolder : AssetFolder, ISerializationCallbackReceiver
	{
		// Token: 0x060029D9 RID: 10713 RVA: 0x000E265A File Offset: 0x000E0A5A
		public StreamingFolder()
		{
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x060029DA RID: 10714 RVA: 0x000E2662 File Offset: 0x000E0A62
		// (set) Token: 0x060029DB RID: 10715 RVA: 0x000E2674 File Offset: 0x000E0A74
		public override string Path
		{
			get
			{
				return System.IO.Path.Combine(Application.streamingAssetsPath, this._relativePath);
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060029DC RID: 10716 RVA: 0x000E267B File Offset: 0x000E0A7B
		public void OnAfterDeserialize()
		{
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x000E267D File Offset: 0x000E0A7D
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x04002209 RID: 8713
		[SerializeField]
		private string _relativePath;
	}
}
