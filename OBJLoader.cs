using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020003FC RID: 1020
public class OBJLoader
{
	// Token: 0x060019EE RID: 6638 RVA: 0x00090C9B File Offset: 0x0008F09B
	public OBJLoader()
	{
	}

	// Token: 0x060019EF RID: 6639 RVA: 0x00090CA4 File Offset: 0x0008F0A4
	public static Vector3 ParseVectorFromCMPS(string[] cmps)
	{
		float x = float.Parse(cmps[1]);
		float y = float.Parse(cmps[2]);
		if (cmps.Length == 4)
		{
			float z = float.Parse(cmps[3]);
			return new Vector3(x, y, z);
		}
		return new Vector2(x, y);
	}

	// Token: 0x060019F0 RID: 6640 RVA: 0x00090CEC File Offset: 0x0008F0EC
	public static Color ParseColorFromCMPS(string[] cmps, float scalar = 1f)
	{
		float r = float.Parse(cmps[1]) * scalar;
		float g = float.Parse(cmps[2]) * scalar;
		float b = float.Parse(cmps[3]) * scalar;
		return new Color(r, g, b);
	}

	// Token: 0x060019F1 RID: 6641 RVA: 0x00090D24 File Offset: 0x0008F124
	public static string OBJGetFilePath(string path, string basePath, string fileName)
	{
		foreach (string text in OBJLoader.searchPaths)
		{
			string str = text.Replace("%FileName%", fileName);
			if (File.Exists(basePath + str + path))
			{
				return basePath + str + path;
			}
			if (File.Exists(path))
			{
				return path;
			}
		}
		return null;
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x00090D88 File Offset: 0x0008F188
	public static Material[] LoadMTLFile(string fn)
	{
		Material material = null;
		List<Material> list = new List<Material>();
		FileInfo fileInfo = new FileInfo(fn);
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fn);
		string basePath = fileInfo.Directory.FullName + Path.DirectorySeparatorChar;
		foreach (string text in File.ReadAllLines(fn))
		{
			string text2 = text.Trim().Replace("  ", " ");
			string[] array2 = text2.Split(new char[]
			{
				' '
			});
			string text3 = text2.Remove(0, text2.IndexOf(' ') + 1);
			if (array2[0] == "newmtl")
			{
				if (material != null)
				{
					list.Add(material);
				}
				material = new Material(Shader.Find("Standard (Specular setup)"));
				material.name = text3;
			}
			else if (array2[0] == "Kd")
			{
				material.SetColor("_Color", OBJLoader.ParseColorFromCMPS(array2, 1f));
			}
			else if (array2[0] == "map_Kd")
			{
				string text4 = OBJLoader.OBJGetFilePath(text3, basePath, fileNameWithoutExtension);
				if (text4 != null)
				{
					material.SetTexture("_MainTex", TextureLoader.LoadTexture(text4, false));
				}
			}
			else if (array2[0] == "map_Bump")
			{
				string text5 = OBJLoader.OBJGetFilePath(text3, basePath, fileNameWithoutExtension);
				if (text5 != null)
				{
					material.SetTexture("_BumpMap", TextureLoader.LoadTexture(text5, true));
					material.EnableKeyword("_NORMALMAP");
				}
			}
			else if (array2[0] == "Ks")
			{
				material.SetColor("_SpecColor", OBJLoader.ParseColorFromCMPS(array2, 1f));
			}
			else if (array2[0] == "Ka")
			{
				material.SetColor("_EmissionColor", OBJLoader.ParseColorFromCMPS(array2, 0.05f));
				material.EnableKeyword("_EMISSION");
			}
			else if (array2[0] == "d")
			{
				float num = float.Parse(array2[1]);
				if (num < 1f)
				{
					Color color = material.color;
					color.a = num;
					material.SetColor("_Color", color);
					material.SetFloat("_Mode", 3f);
					material.SetInt("_SrcBlend", 5);
					material.SetInt("_DstBlend", 10);
					material.SetInt("_ZWrite", 0);
					material.DisableKeyword("_ALPHATEST_ON");
					material.EnableKeyword("_ALPHABLEND_ON");
					material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = 3000;
				}
			}
			else if (array2[0] == "Ns")
			{
				float num2 = float.Parse(array2[1]);
				num2 /= 1000f;
				material.SetFloat("_Glossiness", num2);
			}
		}
		if (material != null)
		{
			list.Add(material);
		}
		return list.ToArray();
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x00091090 File Offset: 0x0008F490
	public static GameObject LoadOBJFile(string fn)
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fn);
		bool flag = false;
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector2> list3 = new List<Vector2>();
		List<Vector3> list4 = new List<Vector3>();
		List<Vector3> list5 = new List<Vector3>();
		List<Vector2> list6 = new List<Vector2>();
		List<string> list7 = new List<string>();
		List<string> list8 = new List<string>();
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		List<OBJLoader.OBJFace> list9 = new List<OBJLoader.OBJFace>();
		string text = string.Empty;
		string text2 = "default";
		Material[] array = null;
		FileInfo fileInfo = new FileInfo(fn);
		foreach (string text3 in File.ReadAllLines(fn))
		{
			if (text3.Length > 0 && text3[0] != '#')
			{
				string text4 = text3.Trim().Replace("  ", " ");
				string[] array3 = text4.Split(new char[]
				{
					' '
				});
				string text5 = text4.Remove(0, text4.IndexOf(' ') + 1);
				if (array3[0] == "mtllib")
				{
					string text6 = OBJLoader.OBJGetFilePath(text5, fileInfo.Directory.FullName + Path.DirectorySeparatorChar, fileNameWithoutExtension);
					if (text6 != null)
					{
						array = OBJLoader.LoadMTLFile(text6);
					}
				}
				else if ((array3[0] == "g" || array3[0] == "o") && !OBJLoader.splitByMaterial)
				{
					text2 = text5;
					if (!list8.Contains(text2))
					{
						list8.Add(text2);
					}
				}
				else if (array3[0] == "usemtl")
				{
					text = text5;
					if (!list7.Contains(text))
					{
						list7.Add(text);
					}
					if (OBJLoader.splitByMaterial && !list8.Contains(text))
					{
						list8.Add(text);
					}
				}
				else if (array3[0] == "v")
				{
					list.Add(OBJLoader.ParseVectorFromCMPS(array3));
				}
				else if (array3[0] == "vn")
				{
					list2.Add(OBJLoader.ParseVectorFromCMPS(array3));
				}
				else if (array3[0] == "vt")
				{
					list3.Add(OBJLoader.ParseVectorFromCMPS(array3));
				}
				else if (array3[0] == "f")
				{
					int[] array4 = new int[array3.Length - 1];
					for (int j = 1; j < array3.Length; j++)
					{
						string text7 = array3[j];
						int num = -1;
						int num2 = -1;
						int num3;
						if (text7.Contains("//"))
						{
							string[] array5 = text7.Split(new char[]
							{
								'/'
							});
							num3 = int.Parse(array5[0]) - 1;
							num = int.Parse(array5[2]) - 1;
						}
						else
						{
							IEnumerable<char> source = text7;
							if (OBJLoader.<>f__am$cache0 == null)
							{
								OBJLoader.<>f__am$cache0 = new Func<char, bool>(OBJLoader.<LoadOBJFile>m__0);
							}
							if (source.Count(OBJLoader.<>f__am$cache0) == 2)
							{
								string[] array6 = text7.Split(new char[]
								{
									'/'
								});
								num3 = int.Parse(array6[0]) - 1;
								num2 = int.Parse(array6[1]) - 1;
								num = int.Parse(array6[2]) - 1;
							}
							else if (!text7.Contains("/"))
							{
								num3 = int.Parse(text7) - 1;
							}
							else
							{
								string[] array7 = text7.Split(new char[]
								{
									'/'
								});
								num3 = int.Parse(array7[0]) - 1;
								num2 = int.Parse(array7[1]) - 1;
							}
						}
						string key = string.Concat(new object[]
						{
							num3,
							"|",
							num,
							"|",
							num2
						});
						if (dictionary.ContainsKey(key))
						{
							array4[j - 1] = dictionary[key];
						}
						else
						{
							array4[j - 1] = dictionary.Count;
							dictionary[key] = dictionary.Count;
							list4.Add(list[num3]);
							if (num < 0 || num > list2.Count - 1)
							{
								list5.Add(Vector3.zero);
							}
							else
							{
								flag = true;
								list5.Add(list2[num]);
							}
							if (num2 < 0 || num2 > list3.Count - 1)
							{
								list6.Add(Vector2.zero);
							}
							else
							{
								list6.Add(list3[num2]);
							}
						}
					}
					if (array4.Length < 5 && array4.Length >= 3)
					{
						list9.Add(new OBJLoader.OBJFace
						{
							materialName = text,
							indexes = new int[]
							{
								array4[0],
								array4[1],
								array4[2]
							},
							meshName = ((!OBJLoader.splitByMaterial) ? text2 : text)
						});
						if (array4.Length > 3)
						{
							list9.Add(new OBJLoader.OBJFace
							{
								materialName = text,
								meshName = ((!OBJLoader.splitByMaterial) ? text2 : text),
								indexes = new int[]
								{
									array4[2],
									array4[3],
									array4[0]
								}
							});
						}
					}
				}
			}
		}
		if (list8.Count == 0)
		{
			list8.Add("default");
		}
		GameObject gameObject = new GameObject(fileNameWithoutExtension);
		using (List<string>.Enumerator enumerator = list8.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				OBJLoader.<LoadOBJFile>c__AnonStorey0 <LoadOBJFile>c__AnonStorey = new OBJLoader.<LoadOBJFile>c__AnonStorey0();
				<LoadOBJFile>c__AnonStorey.obj = enumerator.Current;
				OBJLoader.<LoadOBJFile>c__AnonStorey2 <LoadOBJFile>c__AnonStorey2 = new OBJLoader.<LoadOBJFile>c__AnonStorey2();
				<LoadOBJFile>c__AnonStorey2.<>f__ref$0 = <LoadOBJFile>c__AnonStorey;
				GameObject gameObject2 = new GameObject(<LoadOBJFile>c__AnonStorey.obj);
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localScale = new Vector3(-1f, 1f, 1f);
				Mesh mesh = new Mesh();
				mesh.name = <LoadOBJFile>c__AnonStorey.obj;
				List<Vector3> list10 = new List<Vector3>();
				List<Vector3> list11 = new List<Vector3>();
				List<Vector2> list12 = new List<Vector2>();
				List<int[]> list13 = new List<int[]>();
				Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
				<LoadOBJFile>c__AnonStorey2.meshMaterialNames = new List<string>();
				OBJLoader.OBJFace[] source2 = list9.Where(new Func<OBJLoader.OBJFace, bool>(<LoadOBJFile>c__AnonStorey2.<>m__0)).ToArray<OBJLoader.OBJFace>();
				using (List<string>.Enumerator enumerator2 = list7.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						OBJLoader.<LoadOBJFile>c__AnonStorey1 <LoadOBJFile>c__AnonStorey3 = new OBJLoader.<LoadOBJFile>c__AnonStorey1();
						<LoadOBJFile>c__AnonStorey3.mn = enumerator2.Current;
						OBJLoader.OBJFace[] array8 = source2.Where(new Func<OBJLoader.OBJFace, bool>(<LoadOBJFile>c__AnonStorey3.<>m__0)).ToArray<OBJLoader.OBJFace>();
						if (array8.Length > 0)
						{
							int[] array9 = new int[0];
							foreach (OBJLoader.OBJFace objface in array8)
							{
								int num4 = array9.Length;
								Array.Resize<int>(ref array9, num4 + objface.indexes.Length);
								Array.Copy(objface.indexes, 0, array9, num4, objface.indexes.Length);
							}
							<LoadOBJFile>c__AnonStorey2.meshMaterialNames.Add(<LoadOBJFile>c__AnonStorey3.mn);
							if (mesh.subMeshCount != <LoadOBJFile>c__AnonStorey2.meshMaterialNames.Count)
							{
								mesh.subMeshCount = <LoadOBJFile>c__AnonStorey2.meshMaterialNames.Count;
							}
							for (int l = 0; l < array9.Length; l++)
							{
								int num5 = array9[l];
								if (dictionary2.ContainsKey(num5))
								{
									array9[l] = dictionary2[num5];
								}
								else
								{
									list10.Add(list4[num5]);
									list11.Add(list5[num5]);
									list12.Add(list6[num5]);
									dictionary2[num5] = list10.Count - 1;
									array9[l] = dictionary2[num5];
								}
							}
							list13.Add(array9);
						}
					}
				}
				mesh.vertices = list10.ToArray();
				mesh.normals = list11.ToArray();
				mesh.uv = list12.ToArray();
				for (int m = 0; m < list13.Count; m++)
				{
					mesh.SetTriangles(list13[m], m);
				}
				if (!flag)
				{
					mesh.RecalculateNormals();
				}
				mesh.RecalculateBounds();
				MeshFilter meshFilter = gameObject2.AddComponent<MeshFilter>();
				MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
				Material[] array11 = new Material[<LoadOBJFile>c__AnonStorey2.meshMaterialNames.Count];
				OBJLoader.<LoadOBJFile>c__AnonStorey3 <LoadOBJFile>c__AnonStorey4 = new OBJLoader.<LoadOBJFile>c__AnonStorey3();
				<LoadOBJFile>c__AnonStorey4.<>f__ref$2 = <LoadOBJFile>c__AnonStorey2;
				<LoadOBJFile>c__AnonStorey4.i = 0;
				while (<LoadOBJFile>c__AnonStorey4.i < <LoadOBJFile>c__AnonStorey2.meshMaterialNames.Count)
				{
					if (array == null)
					{
						array11[<LoadOBJFile>c__AnonStorey4.i] = new Material(Shader.Find("Standard (Specular setup)"));
					}
					else
					{
						Material material = Array.Find<Material>(array, new Predicate<Material>(<LoadOBJFile>c__AnonStorey4.<>m__0));
						if (material == null)
						{
							array11[<LoadOBJFile>c__AnonStorey4.i] = new Material(Shader.Find("Standard (Specular setup)"));
						}
						else
						{
							array11[<LoadOBJFile>c__AnonStorey4.i] = material;
						}
					}
					array11[<LoadOBJFile>c__AnonStorey4.i].name = <LoadOBJFile>c__AnonStorey2.meshMaterialNames[<LoadOBJFile>c__AnonStorey4.i];
					<LoadOBJFile>c__AnonStorey4.i++;
				}
				meshRenderer.materials = array11;
				meshFilter.mesh = mesh;
			}
		}
		return gameObject;
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x00091A6C File Offset: 0x0008FE6C
	// Note: this type is marked as 'beforefieldinit'.
	static OBJLoader()
	{
	}

	// Token: 0x060019F5 RID: 6645 RVA: 0x00091A9E File Offset: 0x0008FE9E
	[CompilerGenerated]
	private static bool <LoadOBJFile>m__0(char x)
	{
		return x == '/';
	}

	// Token: 0x04001516 RID: 5398
	public static bool splitByMaterial = false;

	// Token: 0x04001517 RID: 5399
	public static string[] searchPaths = new string[]
	{
		string.Empty,
		"%FileName%_Textures" + Path.DirectorySeparatorChar
	};

	// Token: 0x04001518 RID: 5400
	[CompilerGenerated]
	private static Func<char, bool> <>f__am$cache0;

	// Token: 0x020003FD RID: 1021
	private struct OBJFace
	{
		// Token: 0x04001519 RID: 5401
		public string materialName;

		// Token: 0x0400151A RID: 5402
		public string meshName;

		// Token: 0x0400151B RID: 5403
		public int[] indexes;
	}

	// Token: 0x02000F48 RID: 3912
	[CompilerGenerated]
	private sealed class <LoadOBJFile>c__AnonStorey0
	{
		// Token: 0x06007370 RID: 29552 RVA: 0x00091AA5 File Offset: 0x0008FEA5
		public <LoadOBJFile>c__AnonStorey0()
		{
		}

		// Token: 0x04006763 RID: 26467
		internal string obj;
	}

	// Token: 0x02000F49 RID: 3913
	[CompilerGenerated]
	private sealed class <LoadOBJFile>c__AnonStorey2
	{
		// Token: 0x06007371 RID: 29553 RVA: 0x00091AAD File Offset: 0x0008FEAD
		public <LoadOBJFile>c__AnonStorey2()
		{
		}

		// Token: 0x06007372 RID: 29554 RVA: 0x00091AB5 File Offset: 0x0008FEB5
		internal bool <>m__0(OBJLoader.OBJFace x)
		{
			return x.meshName == this.<>f__ref$0.obj;
		}

		// Token: 0x04006764 RID: 26468
		internal List<string> meshMaterialNames;

		// Token: 0x04006765 RID: 26469
		internal OBJLoader.<LoadOBJFile>c__AnonStorey0 <>f__ref$0;
	}

	// Token: 0x02000F4A RID: 3914
	[CompilerGenerated]
	private sealed class <LoadOBJFile>c__AnonStorey1
	{
		// Token: 0x06007373 RID: 29555 RVA: 0x00091ACE File Offset: 0x0008FECE
		public <LoadOBJFile>c__AnonStorey1()
		{
		}

		// Token: 0x06007374 RID: 29556 RVA: 0x00091AD6 File Offset: 0x0008FED6
		internal bool <>m__0(OBJLoader.OBJFace x)
		{
			return x.materialName == this.mn;
		}

		// Token: 0x04006766 RID: 26470
		internal string mn;
	}

	// Token: 0x02000F4B RID: 3915
	[CompilerGenerated]
	private sealed class <LoadOBJFile>c__AnonStorey3
	{
		// Token: 0x06007375 RID: 29557 RVA: 0x00091AEA File Offset: 0x0008FEEA
		public <LoadOBJFile>c__AnonStorey3()
		{
		}

		// Token: 0x06007376 RID: 29558 RVA: 0x00091AF2 File Offset: 0x0008FEF2
		internal bool <>m__0(Material x)
		{
			return x.name == this.<>f__ref$2.meshMaterialNames[this.i];
		}

		// Token: 0x04006767 RID: 26471
		internal int i;

		// Token: 0x04006768 RID: 26472
		internal OBJLoader.<LoadOBJFile>c__AnonStorey2 <>f__ref$2;
	}
}
