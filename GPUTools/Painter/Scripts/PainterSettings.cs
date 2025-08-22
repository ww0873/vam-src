using System;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Painter.Scripts
{
	// Token: 0x02000A35 RID: 2613
	public class PainterSettings : MonoBehaviour
	{
		// Token: 0x0600436F RID: 17263 RVA: 0x0013C32C File Offset: 0x0013A72C
		public PainterSettings()
		{
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06004370 RID: 17264 RVA: 0x0013C33F File Offset: 0x0013A73F
		public Material SharedMaterial
		{
			get
			{
				return base.GetComponent<Renderer>().sharedMaterial;
			}
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x0013C34C File Offset: 0x0013A74C
		private void Start()
		{
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x0013C34E File Offset: 0x0013A74E
		private void Update()
		{
		}

		// Token: 0x04003268 RID: 12904
		[SerializeField]
		public MeshProvider MeshProvider = new MeshProvider();

		// Token: 0x04003269 RID: 12905
		[SerializeField]
		public ColorBrush Brush;

		// Token: 0x0400326A RID: 12906
		[SerializeField]
		public Color[] Colors;
	}
}
