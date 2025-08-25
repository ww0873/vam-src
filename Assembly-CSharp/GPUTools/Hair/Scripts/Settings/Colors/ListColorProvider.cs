using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Colors
{
	// Token: 0x02000A26 RID: 2598
	[Serializable]
	public class ListColorProvider : IColorProvider
	{
		// Token: 0x06004326 RID: 17190 RVA: 0x0013B518 File Offset: 0x00139918
		public ListColorProvider()
		{
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x0013B52B File Offset: 0x0013992B
		public Color GetColor(HairSettings settings, int x, int y, int sizeY)
		{
			return this.GetStandColor((float)y / (float)sizeY);
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x0013B53C File Offset: 0x0013993C
		private Color GetStandColor(float t)
		{
			if (this.Colors.Count == 0)
			{
				return Color.black;
			}
			float value = (float)this.Colors.Count * t;
			int index = (int)Mathf.Clamp(value, 0f, (float)(this.Colors.Count - 1));
			return this.Colors[index];
		}

		// Token: 0x040031DA RID: 12762
		public List<Color> Colors = new List<Color>();
	}
}
