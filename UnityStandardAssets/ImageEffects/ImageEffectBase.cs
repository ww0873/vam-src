using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E7F RID: 3711
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("")]
	public class ImageEffectBase : MonoBehaviour
	{
		// Token: 0x060070F8 RID: 28920 RVA: 0x002AB27D File Offset: 0x002A967D
		public ImageEffectBase()
		{
		}

		// Token: 0x060070F9 RID: 28921 RVA: 0x002AB285 File Offset: 0x002A9685
		protected virtual void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (!this.shader || !this.shader.isSupported)
			{
				base.enabled = false;
			}
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x060070FA RID: 28922 RVA: 0x002AB2C0 File Offset: 0x002A96C0
		protected Material material
		{
			get
			{
				if (this.m_Material == null)
				{
					this.m_Material = new Material(this.shader);
					this.m_Material.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_Material;
			}
		}

		// Token: 0x060070FB RID: 28923 RVA: 0x002AB2F7 File Offset: 0x002A96F7
		protected virtual void OnDisable()
		{
			if (this.m_Material)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Material);
			}
		}

		// Token: 0x04006451 RID: 25681
		public Shader shader;

		// Token: 0x04006452 RID: 25682
		private Material m_Material;
	}
}
