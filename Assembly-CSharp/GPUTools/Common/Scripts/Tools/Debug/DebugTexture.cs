using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
	// Token: 0x020009C3 RID: 2499
	public class DebugTexture : MonoBehaviour
	{
		// Token: 0x06003F25 RID: 16165 RVA: 0x0012E9A4 File Offset: 0x0012CDA4
		public DebugTexture()
		{
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x0012E9AC File Offset: 0x0012CDAC
		public static void SetTexture(Texture texture)
		{
			GameObject gameObject = new GameObject("DebugTexture");
			DebugTexture debugTexture = gameObject.AddComponent<DebugTexture>();
			debugTexture.Texture = texture;
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06003F27 RID: 16167 RVA: 0x0012E9D2 File Offset: 0x0012CDD2
		// (set) Token: 0x06003F28 RID: 16168 RVA: 0x0012E9DA File Offset: 0x0012CDDA
		public Texture Texture
		{
			[CompilerGenerated]
			get
			{
				return this.<Texture>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Texture>k__BackingField = value;
			}
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x0012E9E3 File Offset: 0x0012CDE3
		private void OnGUI()
		{
			GUI.DrawTexture(new Rect(0f, 0f, 400f, 400f), this.Texture, ScaleMode.ScaleToFit, false, 1f);
		}

		// Token: 0x04002FF2 RID: 12274
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Texture <Texture>k__BackingField;
	}
}
