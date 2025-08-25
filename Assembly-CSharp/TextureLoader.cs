using System;
using System.IO;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public class TextureLoader : MonoBehaviour
{
	// Token: 0x060019F6 RID: 6646 RVA: 0x00091B15 File Offset: 0x0008FF15
	public TextureLoader()
	{
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x00091B20 File Offset: 0x0008FF20
	public static Texture2D LoadTGA(string fileName)
	{
		Texture2D result;
		using (FileStream fileStream = File.OpenRead(fileName))
		{
			result = TextureLoader.LoadTGA(fileStream, TextureFormat.RGBA32, true, false);
		}
		return result;
	}

	// Token: 0x060019F8 RID: 6648 RVA: 0x00091B64 File Offset: 0x0008FF64
	public static Texture2D LoadDDSManual(string ddsPath)
	{
		Texture2D result;
		try
		{
			byte[] array = File.ReadAllBytes(ddsPath);
			byte b = array[4];
			if (b != 124)
			{
				throw new Exception("Invalid DDS DXTn texture. Unable to read");
			}
			int height = (int)array[13] * 256 + (int)array[12];
			int width = (int)array[17] * 256 + (int)array[16];
			byte b2 = array[87];
			TextureFormat format = TextureFormat.DXT5;
			if (b2 == 49)
			{
				format = TextureFormat.DXT1;
			}
			if (b2 == 53)
			{
				format = TextureFormat.DXT5;
			}
			int num = 128;
			byte[] array2 = new byte[array.Length - num];
			Buffer.BlockCopy(array, num, array2, 0, array.Length - num);
			FileInfo fileInfo = new FileInfo(ddsPath);
			Texture2D texture2D = new Texture2D(width, height, format, false);
			texture2D.LoadRawTextureData(array2);
			texture2D.Apply();
			texture2D.name = fileInfo.Name;
			result = texture2D;
		}
		catch (Exception arg)
		{
			Debug.LogError("Error: Could not load DDS " + arg);
			result = new Texture2D(8, 8);
		}
		return result;
	}

	// Token: 0x060019F9 RID: 6649 RVA: 0x00091C68 File Offset: 0x00090068
	public static void SetNormalMap(ref Texture2D tex)
	{
		Color[] pixels = tex.GetPixels();
		for (int i = 0; i < pixels.Length; i++)
		{
			Color color = pixels[i];
			color.r = 1f;
			color.a = pixels[i].r;
			pixels[i] = color;
		}
		tex.SetPixels(pixels);
		tex.Apply(true);
	}

	// Token: 0x060019FA RID: 6650 RVA: 0x00091CDC File Offset: 0x000900DC
	public static Texture2D LoadTexture(string fn, bool normalMap = false)
	{
		if (!File.Exists(fn))
		{
			return null;
		}
		string a = Path.GetExtension(fn).ToLower();
		if (a == ".png" || a == ".jpg")
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.LoadImage(File.ReadAllBytes(fn));
			if (normalMap)
			{
				TextureLoader.SetNormalMap(ref texture2D);
			}
			return texture2D;
		}
		if (a == ".dds")
		{
			Texture2D result = TextureLoader.LoadDDSManual(fn);
			if (normalMap)
			{
				TextureLoader.SetNormalMap(ref result);
			}
			return result;
		}
		if (a == ".tga")
		{
			Texture2D result2 = TextureLoader.LoadTGA(fn);
			if (normalMap)
			{
				TextureLoader.SetNormalMap(ref result2);
			}
			return result2;
		}
		Debug.Log("texture not supported : " + fn);
		return null;
	}

	// Token: 0x060019FB RID: 6651 RVA: 0x00091DA4 File Offset: 0x000901A4
	public static Texture2D LoadTGA(Stream TGAStream, TextureFormat tf = TextureFormat.RGBA32, bool useMipMap = true, bool linear = false)
	{
		Texture2D result;
		using (BinaryReader binaryReader = new BinaryReader(TGAStream))
		{
			binaryReader.BaseStream.Seek(12L, SeekOrigin.Begin);
			short num = binaryReader.ReadInt16();
			short num2 = binaryReader.ReadInt16();
			int num3 = (int)binaryReader.ReadByte();
			binaryReader.BaseStream.Seek(1L, SeekOrigin.Current);
			Texture2D texture2D = new Texture2D((int)num, (int)num2, tf, useMipMap, linear);
			Color32[] array = new Color32[(int)(num * num2)];
			if (num3 == 32)
			{
				for (int i = 0; i < (int)(num * num2); i++)
				{
					byte b = binaryReader.ReadByte();
					byte g = binaryReader.ReadByte();
					byte r = binaryReader.ReadByte();
					byte a = binaryReader.ReadByte();
					array[i] = new Color32(r, g, b, a);
				}
			}
			else
			{
				if (num3 != 24)
				{
					throw new Exception("TGA texture had non 32/24 bit depth.");
				}
				for (int j = 0; j < (int)(num * num2); j++)
				{
					byte b2 = binaryReader.ReadByte();
					byte g2 = binaryReader.ReadByte();
					byte r2 = binaryReader.ReadByte();
					array[j] = new Color32(r2, g2, b2, 1);
				}
			}
			texture2D.SetPixels32(array);
			texture2D.Apply();
			result = texture2D;
		}
		return result;
	}
}
