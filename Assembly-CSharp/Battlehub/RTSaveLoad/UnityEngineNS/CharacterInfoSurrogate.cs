using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001E6 RID: 486
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class CharacterInfoSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009C4 RID: 2500 RVA: 0x0003C20D File Offset: 0x0003A60D
		public CharacterInfoSurrogate()
		{
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0003C218 File Offset: 0x0003A618
		public static implicit operator CharacterInfo(CharacterInfoSurrogate v)
		{
			return new CharacterInfo
			{
				index = v.index,
				size = v.size,
				style = (FontStyle)v.style,
				advance = v.advance,
				glyphWidth = v.glyphWidth,
				glyphHeight = v.glyphHeight,
				bearing = v.bearing,
				minY = v.minY,
				maxY = v.maxY,
				minX = v.minX,
				maxX = v.maxX,
				uvBottomLeft = v.uvBottomLeft,
				uvBottomRight = v.uvBottomRight,
				uvTopRight = v.uvTopRight,
				uvTopLeft = v.uvTopLeft
			};
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0003C2F4 File Offset: 0x0003A6F4
		public static implicit operator CharacterInfoSurrogate(CharacterInfo v)
		{
			return new CharacterInfoSurrogate
			{
				index = v.index,
				size = v.size,
				style = (uint)v.style,
				advance = v.advance,
				glyphWidth = v.glyphWidth,
				glyphHeight = v.glyphHeight,
				bearing = v.bearing,
				minY = v.minY,
				maxY = v.maxY,
				minX = v.minX,
				maxX = v.maxX,
				uvBottomLeft = v.uvBottomLeft,
				uvBottomRight = v.uvBottomRight,
				uvTopRight = v.uvTopRight,
				uvTopLeft = v.uvTopLeft
			};
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0003C3CC File Offset: 0x0003A7CC
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			CharacterInfo characterInfo = (CharacterInfo)obj;
			info.AddValue("index", characterInfo.index);
			info.AddValue("size", characterInfo.size);
			info.AddValue("style", characterInfo.style);
			info.AddValue("advance", characterInfo.advance);
			info.AddValue("glyphWidth", characterInfo.glyphWidth);
			info.AddValue("glyphHeight", characterInfo.glyphHeight);
			info.AddValue("bearing", characterInfo.bearing);
			info.AddValue("minY", characterInfo.minY);
			info.AddValue("maxY", characterInfo.maxY);
			info.AddValue("minX", characterInfo.minX);
			info.AddValue("maxX", characterInfo.maxX);
			info.AddValue("uvBottomLeft", characterInfo.uvBottomLeft);
			info.AddValue("uvBottomRight", characterInfo.uvBottomRight);
			info.AddValue("uvTopRight", characterInfo.uvTopRight);
			info.AddValue("uvTopLeft", characterInfo.uvTopLeft);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0003C508 File Offset: 0x0003A908
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			CharacterInfo characterInfo = (CharacterInfo)obj;
			characterInfo.index = (int)info.GetValue("index", typeof(int));
			characterInfo.size = (int)info.GetValue("size", typeof(int));
			characterInfo.style = (FontStyle)info.GetValue("style", typeof(FontStyle));
			characterInfo.advance = (int)info.GetValue("advance", typeof(int));
			characterInfo.glyphWidth = (int)info.GetValue("glyphWidth", typeof(int));
			characterInfo.glyphHeight = (int)info.GetValue("glyphHeight", typeof(int));
			characterInfo.bearing = (int)info.GetValue("bearing", typeof(int));
			characterInfo.minY = (int)info.GetValue("minY", typeof(int));
			characterInfo.maxY = (int)info.GetValue("maxY", typeof(int));
			characterInfo.minX = (int)info.GetValue("minX", typeof(int));
			characterInfo.maxX = (int)info.GetValue("maxX", typeof(int));
			characterInfo.uvBottomLeft = (Vector2)info.GetValue("uvBottomLeft", typeof(Vector2));
			characterInfo.uvBottomRight = (Vector2)info.GetValue("uvBottomRight", typeof(Vector2));
			characterInfo.uvTopRight = (Vector2)info.GetValue("uvTopRight", typeof(Vector2));
			characterInfo.uvTopLeft = (Vector2)info.GetValue("uvTopLeft", typeof(Vector2));
			return characterInfo;
		}

		// Token: 0x04000AF6 RID: 2806
		public int index;

		// Token: 0x04000AF7 RID: 2807
		public int size;

		// Token: 0x04000AF8 RID: 2808
		public uint style;

		// Token: 0x04000AF9 RID: 2809
		public int advance;

		// Token: 0x04000AFA RID: 2810
		public int glyphWidth;

		// Token: 0x04000AFB RID: 2811
		public int glyphHeight;

		// Token: 0x04000AFC RID: 2812
		public int bearing;

		// Token: 0x04000AFD RID: 2813
		public int minY;

		// Token: 0x04000AFE RID: 2814
		public int maxY;

		// Token: 0x04000AFF RID: 2815
		public int minX;

		// Token: 0x04000B00 RID: 2816
		public int maxX;

		// Token: 0x04000B01 RID: 2817
		public Vector2 uvBottomLeft;

		// Token: 0x04000B02 RID: 2818
		public Vector2 uvBottomRight;

		// Token: 0x04000B03 RID: 2819
		public Vector2 uvTopRight;

		// Token: 0x04000B04 RID: 2820
		public Vector2 uvTopLeft;
	}
}
