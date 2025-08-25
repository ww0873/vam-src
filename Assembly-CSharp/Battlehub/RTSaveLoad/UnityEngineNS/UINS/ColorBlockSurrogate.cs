using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.UnityEngineNS.UINS
{
	// Token: 0x020001E1 RID: 481
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class ColorBlockSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009AB RID: 2475 RVA: 0x0003B935 File Offset: 0x00039D35
		public ColorBlockSurrogate()
		{
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0003B940 File Offset: 0x00039D40
		public static implicit operator ColorBlock(ColorBlockSurrogate v)
		{
			return new ColorBlock
			{
				normalColor = v.normalColor,
				highlightedColor = v.highlightedColor,
				pressedColor = v.pressedColor,
				disabledColor = v.disabledColor,
				colorMultiplier = v.colorMultiplier,
				fadeDuration = v.fadeDuration
			};
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0003B9A4 File Offset: 0x00039DA4
		public static implicit operator ColorBlockSurrogate(ColorBlock v)
		{
			return new ColorBlockSurrogate
			{
				normalColor = v.normalColor,
				highlightedColor = v.highlightedColor,
				pressedColor = v.pressedColor,
				disabledColor = v.disabledColor,
				colorMultiplier = v.colorMultiplier,
				fadeDuration = v.fadeDuration
			};
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0003BA08 File Offset: 0x00039E08
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			ColorBlock colorBlock = (ColorBlock)obj;
			info.AddValue("normalColor", colorBlock.normalColor);
			info.AddValue("highlightedColor", colorBlock.highlightedColor);
			info.AddValue("pressedColor", colorBlock.pressedColor);
			info.AddValue("disabledColor", colorBlock.disabledColor);
			info.AddValue("colorMultiplier", colorBlock.colorMultiplier);
			info.AddValue("fadeDuration", colorBlock.fadeDuration);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0003BA9C File Offset: 0x00039E9C
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			ColorBlock colorBlock = (ColorBlock)obj;
			colorBlock.normalColor = (Color)info.GetValue("normalColor", typeof(Color));
			colorBlock.highlightedColor = (Color)info.GetValue("highlightedColor", typeof(Color));
			colorBlock.pressedColor = (Color)info.GetValue("pressedColor", typeof(Color));
			colorBlock.disabledColor = (Color)info.GetValue("disabledColor", typeof(Color));
			colorBlock.colorMultiplier = (float)info.GetValue("colorMultiplier", typeof(float));
			colorBlock.fadeDuration = (float)info.GetValue("fadeDuration", typeof(float));
			return colorBlock;
		}

		// Token: 0x04000ADF RID: 2783
		public Color normalColor;

		// Token: 0x04000AE0 RID: 2784
		public Color highlightedColor;

		// Token: 0x04000AE1 RID: 2785
		public Color pressedColor;

		// Token: 0x04000AE2 RID: 2786
		public Color disabledColor;

		// Token: 0x04000AE3 RID: 2787
		public float colorMultiplier;

		// Token: 0x04000AE4 RID: 2788
		public float fadeDuration;
	}
}
