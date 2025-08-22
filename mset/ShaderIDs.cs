using System;
using UnityEngine;

namespace mset
{
	// Token: 0x0200032D RID: 813
	public class ShaderIDs
	{
		// Token: 0x06001336 RID: 4918 RVA: 0x0006EA58 File Offset: 0x0006CE58
		public ShaderIDs()
		{
			this.SH = new int[9];
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x0006EAC5 File Offset: 0x0006CEC5
		public bool valid
		{
			get
			{
				return this._valid;
			}
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0006EACD File Offset: 0x0006CECD
		public void Link()
		{
			this.Link(string.Empty);
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0006EADC File Offset: 0x0006CEDC
		public void Link(string postfix)
		{
			this.specCubeIBL = Shader.PropertyToID("_SpecCubeIBL" + postfix);
			this.skyCubeIBL = Shader.PropertyToID("_SkyCubeIBL" + postfix);
			this.skyMatrix = Shader.PropertyToID("_SkyMatrix" + postfix);
			this.invSkyMatrix = Shader.PropertyToID("_InvSkyMatrix" + postfix);
			this.skyMin = Shader.PropertyToID("_SkyMin" + postfix);
			this.skyMax = Shader.PropertyToID("_SkyMax" + postfix);
			this.exposureIBL = Shader.PropertyToID("_ExposureIBL" + postfix);
			this.exposureLM = Shader.PropertyToID("_ExposureLM" + postfix);
			for (int i = 0; i < 9; i++)
			{
				this.SH[i] = Shader.PropertyToID("_SH" + i + postfix);
			}
			this.blendWeightIBL = Shader.PropertyToID("_BlendWeightIBL");
			this._valid = true;
		}

		// Token: 0x040010D1 RID: 4305
		public int specCubeIBL = -1;

		// Token: 0x040010D2 RID: 4306
		public int skyCubeIBL = -1;

		// Token: 0x040010D3 RID: 4307
		public int skyMatrix = -1;

		// Token: 0x040010D4 RID: 4308
		public int invSkyMatrix = -1;

		// Token: 0x040010D5 RID: 4309
		public int skySize = -1;

		// Token: 0x040010D6 RID: 4310
		public int skyMin = -1;

		// Token: 0x040010D7 RID: 4311
		public int skyMax = -1;

		// Token: 0x040010D8 RID: 4312
		public int[] SH;

		// Token: 0x040010D9 RID: 4313
		public int exposureIBL = -1;

		// Token: 0x040010DA RID: 4314
		public int exposureLM = -1;

		// Token: 0x040010DB RID: 4315
		public int oldExposureIBL = -1;

		// Token: 0x040010DC RID: 4316
		public int blendWeightIBL = -1;

		// Token: 0x040010DD RID: 4317
		private bool _valid;
	}
}
