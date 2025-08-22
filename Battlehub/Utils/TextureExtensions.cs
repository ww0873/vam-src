using System;
using UnityEngine;

namespace Battlehub.Utils
{
	// Token: 0x020002AD RID: 685
	public static class TextureExtensions
	{
		// Token: 0x0600101C RID: 4124 RVA: 0x0005B678 File Offset: 0x00059A78
		public static bool IsReadable(this Texture2D texture)
		{
			if (texture == null)
			{
				return false;
			}
			bool result;
			try
			{
				texture.GetPixel(0, 0);
				result = true;
			}
			catch (UnityException)
			{
				result = false;
			}
			return result;
		}
	}
}
