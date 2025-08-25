using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x02000E15 RID: 3605
public class OBJExporter : MonoBehaviour
{
	// Token: 0x06006F11 RID: 28433 RVA: 0x0029A818 File Offset: 0x00298C18
	public OBJExporter()
	{
	}

	// Token: 0x06006F12 RID: 28434 RVA: 0x0029A830 File Offset: 0x00298C30
	public void Export(string exportPath, Mesh mesh, Vector3[] vertices, Vector3[] normals, Material[] mats, Dictionary<int, bool> enabledMats)
	{
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		FileInfo fileInfo = new FileInfo(exportPath);
		this.lastExportFolder = fileInfo.Directory.FullName;
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(exportPath);
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		if (this.generateMaterials)
		{
			stringBuilder.AppendLine("mtllib " + fileNameWithoutExtension + ".mtl");
		}
		string name = mesh.name;
		if (this.generateMaterials)
		{
			for (int i = 0; i < mats.Length; i++)
			{
				if (enabledMats.ContainsKey(i))
				{
					Material material = mats[i];
					if (!dictionary.ContainsKey(material.name))
					{
						dictionary[material.name] = true;
						stringBuilder2.Append(this.MaterialToString(material));
						stringBuilder2.AppendLine();
					}
				}
			}
		}
		foreach (Vector3 vector in vertices)
		{
			Vector3 vector2 = vector;
			vector2.x *= -1f;
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"v ",
				vector2.x,
				" ",
				vector2.y,
				" ",
				vector2.z
			}));
		}
		foreach (Vector3 vector3 in normals)
		{
			Vector3 vector4 = vector3;
			vector4.x *= -1f;
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"vn ",
				vector4.x,
				" ",
				vector4.y,
				" ",
				vector4.z
			}));
		}
		foreach (Vector2 vector5 in mesh.uv)
		{
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"vt ",
				vector5.x,
				" ",
				vector5.y
			}));
		}
		for (int m = 0; m < mesh.subMeshCount; m++)
		{
			if (enabledMats.ContainsKey(m))
			{
				if (m < mats.Length)
				{
					string name2 = mats[m].name;
					stringBuilder.AppendLine("usemtl " + name2);
				}
				else
				{
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"usemtl ",
						name,
						"_missing",
						m
					}));
				}
				int[] triangles = mesh.GetTriangles(m);
				for (int n = 0; n < triangles.Length; n += 3)
				{
					int index = triangles[n] + 1;
					int index2 = triangles[n + 1] + 1;
					int index3 = triangles[n + 2] + 1;
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"f ",
						this.ConstructOBJString(index3),
						" ",
						this.ConstructOBJString(index2),
						" ",
						this.ConstructOBJString(index)
					}));
				}
			}
		}
		File.WriteAllText(exportPath, stringBuilder.ToString());
		if (this.generateMaterials)
		{
			File.WriteAllText(fileInfo.Directory.FullName + "\\" + fileNameWithoutExtension + ".mtl", stringBuilder2.ToString());
		}
	}

	// Token: 0x06006F13 RID: 28435 RVA: 0x0029AC04 File Offset: 0x00299004
	private string TryExportTexture(string propertyName, Material m)
	{
		if (m.HasProperty(propertyName))
		{
			Texture texture = m.GetTexture(propertyName);
			if (texture != null)
			{
				return this.ExportTexture((Texture2D)texture);
			}
		}
		return "false";
	}

	// Token: 0x06006F14 RID: 28436 RVA: 0x0029AC44 File Offset: 0x00299044
	private string ExportTexture(Texture2D t)
	{
		string result;
		try
		{
			string text = this.lastExportFolder + "\\" + t.name + ".png";
			Texture2D texture2D = new Texture2D(t.width, t.height, TextureFormat.ARGB32, false);
			t.GetRawTextureData();
			texture2D.SetPixels(t.GetPixels());
			File.WriteAllBytes(text, texture2D.EncodeToPNG());
			result = text;
		}
		catch (Exception ex)
		{
			Debug.Log("Could not export texture : " + t.name + ": " + ex.Message);
			result = "null";
		}
		return result;
	}

	// Token: 0x06006F15 RID: 28437 RVA: 0x0029ACE4 File Offset: 0x002990E4
	private string ConstructOBJString(int index)
	{
		string text = index.ToString();
		return string.Concat(new string[]
		{
			text,
			"/",
			text,
			"/",
			text
		});
	}

	// Token: 0x06006F16 RID: 28438 RVA: 0x0029AD28 File Offset: 0x00299128
	private string MaterialToString(Material m)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("newmtl " + m.name);
		if (m.HasProperty("_Color"))
		{
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Kd ",
				m.color.r.ToString(),
				" ",
				m.color.g.ToString(),
				" ",
				m.color.b.ToString()
			}));
			if (m.color.a < 1f)
			{
				stringBuilder.AppendLine("Tr " + (1f - m.color.a).ToString());
				stringBuilder.AppendLine("d " + m.color.a.ToString());
			}
		}
		if (m.HasProperty("_SpecColor"))
		{
			Color color = m.GetColor("_SpecColor");
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Ks ",
				color.r.ToString(),
				" ",
				color.g.ToString(),
				" ",
				color.b.ToString()
			}));
		}
		if (this.exportTextureNames && m.HasProperty("_MainTex"))
		{
			Texture texture = m.GetTexture("_MainTex");
			if (texture != null)
			{
				stringBuilder.AppendLine("map_Kd " + texture.name);
			}
		}
		if (this.exportTextures)
		{
			string text = this.TryExportTexture("_MainTex", m);
			if (text != "false")
			{
				stringBuilder.AppendLine("map_Kd " + text);
			}
			text = this.TryExportTexture("_SpecMap", m);
			if (text != "false")
			{
				stringBuilder.AppendLine("map_Ks " + text);
			}
			text = this.TryExportTexture("_BumpMap", m);
			if (text != "false")
			{
				stringBuilder.AppendLine("map_Bump " + text);
			}
		}
		stringBuilder.AppendLine("illum 2");
		return stringBuilder.ToString();
	}

	// Token: 0x0400600B RID: 24587
	public bool generateMaterials = true;

	// Token: 0x0400600C RID: 24588
	public bool exportTextures = true;

	// Token: 0x0400600D RID: 24589
	public bool exportTextureNames;

	// Token: 0x0400600E RID: 24590
	private string lastExportFolder;
}
