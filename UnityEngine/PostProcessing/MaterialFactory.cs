using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000093 RID: 147
	public sealed class MaterialFactory : IDisposable
	{
		// Token: 0x0600021A RID: 538 RVA: 0x00010126 File Offset: 0x0000E526
		public MaterialFactory()
		{
			this.m_Materials = new Dictionary<string, Material>();
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0001013C File Offset: 0x0000E53C
		public Material Get(string shaderName)
		{
			Material material;
			if (!this.m_Materials.TryGetValue(shaderName, out material))
			{
				Shader shader = Shader.Find(shaderName);
				if (shader == null)
				{
					throw new ArgumentException(string.Format("Shader not found ({0})", shaderName));
				}
				material = new Material(shader)
				{
					name = string.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1)),
					hideFlags = HideFlags.DontSave
				};
				this.m_Materials.Add(shaderName, material);
			}
			return material;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000101C4 File Offset: 0x0000E5C4
		public void Dispose()
		{
			foreach (KeyValuePair<string, Material> keyValuePair in this.m_Materials)
			{
				Material value = keyValuePair.Value;
				GraphicsUtils.Destroy(value);
			}
			this.m_Materials.Clear();
		}

		// Token: 0x04000312 RID: 786
		private Dictionary<string, Material> m_Materials;
	}
}
