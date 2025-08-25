using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using SimpleJSON;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000B3D RID: 2877
	public class DAZImportMaterial
	{
		// Token: 0x06004EB9 RID: 20153 RVA: 0x001BFA50 File Offset: 0x001BDE50
		public DAZImportMaterial()
		{
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x001BFAE8 File Offset: 0x001BDEE8
		protected Color ProcessColorNode(JSONNode colorNode)
		{
			float asFloat = colorNode[0].AsFloat;
			float asFloat2 = colorNode[1].AsFloat;
			float asFloat3 = colorNode[2].AsFloat;
			Color result = new Color(asFloat, asFloat2, asFloat3);
			return result;
		}

		// Token: 0x06004EBB RID: 20155 RVA: 0x001BFB28 File Offset: 0x001BDF28
		protected Texture2D CopyAndImportImage(DAZImport importObject, string MaterialFolder, string path, bool isNormalMap = false, bool isTransparency = false, bool isBumpMap = false, float bumpStrength = 1f, bool isGlossMap = false, bool forceLinear = false, string invertWithSuffix = null, string addSuffix = "")
		{
			if (path == "null")
			{
				return null;
			}
			path = DAZImport.DAZurlFix(path);
			string text = Regex.Replace(path, ".*/", string.Empty);
			string text2;
			if (addSuffix == string.Empty)
			{
				text2 = MaterialFolder + "/" + text;
			}
			else
			{
				string extension = Path.GetExtension(text);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
				text2 = string.Concat(new string[]
				{
					MaterialFolder,
					"/",
					fileNameWithoutExtension,
					addSuffix,
					extension
				});
			}
			Texture2D texture2D = null;
			bool flag = invertWithSuffix != null;
			string path2;
			if (flag && !Application.isPlaying)
			{
				string fileNameWithoutExtension2 = Path.GetFileNameWithoutExtension(text2);
				path2 = string.Concat(new string[]
				{
					MaterialFolder,
					"/",
					fileNameWithoutExtension2,
					invertWithSuffix,
					".png"
				});
			}
			else
			{
				path2 = text2;
			}
			if (!File.Exists(text2) || !File.Exists(path2))
			{
				string text3 = importObject.DetermineFilePath(path);
				if (!File.Exists(text3))
				{
					Debug.LogError("Could not find referenced texture " + path + " in DAZ library. Skipping");
					return null;
				}
				File.Copy(text3, text2);
			}
			if (Application.isPlaying && ImageLoaderThreaded.singleton != null)
			{
				ImageLoaderThreaded.QueuedImage queuedImage = new ImageLoaderThreaded.QueuedImage();
				queuedImage.imgPath = text2;
				queuedImage.createMipMaps = true;
				queuedImage.isNormalMap = isNormalMap;
				queuedImage.isThumbnail = false;
				queuedImage.linear = (isNormalMap || isBumpMap || isGlossMap || forceLinear);
				queuedImage.createNormalFromBump = isBumpMap;
				queuedImage.bumpStrength = 0.5f * bumpStrength;
				queuedImage.createAlphaFromGrayscale = isTransparency;
				queuedImage.compress = (!isNormalMap && !isBumpMap);
				queuedImage.invert = flag;
				ImageLoaderThreaded.singleton.ProcessImageImmediate(queuedImage);
				if (isBumpMap && queuedImage.tex != null)
				{
					text2 = Regex.Replace(text2, "\\.[a-zA-Z]+$", "GenNM.png");
					byte[] bytes = queuedImage.tex.EncodeToPNG();
					File.WriteAllBytes(text2, bytes);
				}
				if (queuedImage.hadError)
				{
					SuperController.LogError("Error during process of image " + queuedImage.imgPath + ": " + queuedImage.errorText);
				}
				else
				{
					texture2D = queuedImage.tex;
					if (texture2D != null)
					{
						texture2D.name = text2;
						importObject.SetTextureSourcePath(texture2D, text2);
					}
					else
					{
						SuperController.LogError("Unexpected null texture from " + queuedImage.imgPath);
					}
				}
			}
			return texture2D;
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x001BFDC8 File Offset: 0x001BE1C8
		public void ProcessJSON(JSONNode sm)
		{
			JSONNode jsonnode = sm["diffuse"]["channel"];
			if (jsonnode != null)
			{
				if (jsonnode["current_value"] != null && !this.ignoreDiffuseColor)
				{
					this.diffuseColor = this.ProcessColorNode(jsonnode["current_value"]);
				}
				if (jsonnode["image_file"] != null)
				{
					this.diffuseTexturePath = jsonnode["image_file"];
					this.hasDiffuseMap = true;
				}
			}
			jsonnode = sm["diffuse_strength"]["channel"];
			if (jsonnode != null)
			{
				if (jsonnode["current_value"] != null)
				{
					this.diffuseStrength = jsonnode["current_value"].AsFloat;
				}
				if (jsonnode["image_file"] != null)
				{
					Debug.LogError("Don't support diffuse_strength texture map");
				}
			}
			jsonnode = sm["transparency"]["channel"];
			if (jsonnode != null)
			{
				if (jsonnode["current_value"] != null)
				{
					this.alphaStrength = jsonnode["current_value"].AsFloat;
					if (this.alphaStrength != 1f)
					{
						this.isTransparent = true;
					}
				}
				if (jsonnode["image_file"] != null)
				{
					this.alphaTexturePath = jsonnode["image_file"];
					this.hasAlphaMap = true;
					this.isTransparent = true;
				}
			}
			jsonnode = sm["specular"]["channel"];
			if (jsonnode != null)
			{
				if (jsonnode["current_value"] != null && !this.ignoreSpecularColor)
				{
					this.specularColor = this.ProcessColorNode(jsonnode["current_value"]);
				}
				if (jsonnode["image_file"] != null)
				{
					if (this.useSpecularAsGlossMap)
					{
						this.glossTexturePath = jsonnode["image_file"];
						this.hasGlossMap = true;
					}
					this.specularColorTexturePath = jsonnode["image_file"];
					this.hasSpecularColorMap = true;
				}
			}
			jsonnode = sm["specular_strength"]["channel"];
			if (jsonnode != null)
			{
				if (jsonnode["current_value"] != null)
				{
					this.specularStrength = jsonnode["current_value"].AsFloat;
				}
				if (jsonnode["image_file"] != null)
				{
					if (this.useSpecularAsGlossMap)
					{
						this.glossTexturePath = jsonnode["image_file"];
						this.hasGlossMap = true;
					}
					this.specularStrengthTexturePath = jsonnode["image_file"];
					this.hasSpecularStrengthMap = true;
				}
			}
			jsonnode = sm["glossiness"]["channel"];
			if (jsonnode != null)
			{
				if (jsonnode["current_value"] != null)
				{
					this.gloss = jsonnode["current_value"].AsFloat;
				}
				if (jsonnode["image_file"] != null)
				{
					this.glossTexturePath = jsonnode["image_file"];
					this.hasGlossMap = true;
				}
			}
			jsonnode = sm["bump"]["channel"];
			if (jsonnode != null)
			{
				if (jsonnode["current_value"] != null)
				{
					this.bumpStrength = jsonnode["current_value"].AsFloat;
				}
				if (jsonnode["image_file"] != null)
				{
					this.bumpTexturePath = jsonnode["image_file"];
					this.hasBumpMap = true;
				}
			}
			jsonnode = sm["normal"]["channel"];
			if (jsonnode != null && jsonnode["image_file"] != null && (!this.hasBumpMap || !this.forceBumpAsNormalMap))
			{
				this.normalTexturePath = jsonnode["image_file"];
				this.hasNormalMap = true;
			}
			JSONNode jsonnode2 = sm["u_scale"]["channel"]["current_value"];
			this.uvScale.x = 1f;
			if (jsonnode2 != null)
			{
				this.uvScale.x = jsonnode2.AsFloat;
			}
			JSONNode jsonnode3 = sm["v_scale"]["channel"]["current_value"];
			this.uvScale.y = 1f;
			if (jsonnode3 != null)
			{
				this.uvScale.y = jsonnode3.AsFloat;
			}
			JSONNode jsonnode4 = sm["u_offset"]["channel"]["current_value"];
			this.uvOffset.x = 0f;
			if (jsonnode4 != null)
			{
				this.uvOffset.x = jsonnode4.AsFloat;
			}
			JSONNode jsonnode5 = sm["v_offset"]["channel"]["current_value"];
			this.uvOffset.y = 0f;
			if (jsonnode5 != null)
			{
				this.uvOffset.y = jsonnode5.AsFloat;
			}
			JSONNode jsonnode6 = null;
			if (sm["extra"] != null)
			{
				IEnumerator enumerator = sm["extra"].AsArray.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						JSONNode jsonnode7 = (JSONNode)obj;
						string a = jsonnode7["type"];
						if (a == "studio_material_channels")
						{
							jsonnode6 = jsonnode7["channels"];
							if (jsonnode6 != null)
							{
								break;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			if (jsonnode6 != null)
			{
				IEnumerator enumerator2 = jsonnode6.AsArray.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						JSONNode jsonnode8 = (JSONNode)obj2;
						string text = jsonnode8["channel"]["id"];
						JSONNode jsonnode9 = jsonnode8["channel"]["visible"];
						if (text != null && (!(jsonnode9 != null) || jsonnode9.AsBool))
						{
							switch (text)
							{
							case "Diffuse Color":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null && !this.ignoreDiffuseColor)
								{
									this.diffuseColor = this.ProcessColorNode(jsonnode["current_value"]);
								}
								if (jsonnode["image_file"] != null)
								{
									this.diffuseTexturePath = jsonnode["image_file"];
									this.hasDiffuseMap = true;
								}
								break;
							case "Diffuse Strength":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null)
								{
									this.diffuseStrength = jsonnode["current_value"].AsFloat;
								}
								if (jsonnode["image_file"] != null)
								{
									Debug.LogError("Don't currently support Diffuse Strength texture map");
								}
								break;
							case "Cutout Opacity":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null && jsonnode["current_value"].AsFloat != 1f)
								{
									this.isTransparent = true;
								}
								if (jsonnode["image_file"] != null)
								{
									this.alphaTexturePath = jsonnode["image_file"];
									this.hasAlphaMap = true;
									this.isTransparent = true;
								}
								break;
							case "Refraction Weight":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null && jsonnode["current_value"].AsFloat > 0f)
								{
									this.isTransparent = true;
								}
								break;
							case "Specular Color":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null && !this.ignoreSpecularColor)
								{
									this.specularColor = this.ProcessColorNode(jsonnode["current_value"]);
								}
								if (jsonnode["image_file"] != null)
								{
									if (this.useSpecularAsGlossMap)
									{
										this.glossTexturePath = jsonnode["image_file"];
										this.hasGlossMap = true;
									}
									this.specularColorTexturePath = jsonnode["image_file"];
									this.hasSpecularColorMap = true;
								}
								break;
							case "Glossy Color":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null && !this.ignoreSpecularColor)
								{
									this.specularColor = this.ProcessColorNode(jsonnode["current_value"]);
								}
								if (jsonnode["image_file"] != null)
								{
									if (this.useSpecularAsGlossMap)
									{
										this.glossTexturePath = jsonnode["image_file"];
										this.hasGlossMap = true;
									}
									this.specularColorTexturePath = jsonnode["image_file"];
									this.hasSpecularColorMap = true;
								}
								break;
							case "Glossy Layered Weight":
							case "Glossy Weight":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null)
								{
									this.glossWeight = jsonnode["current_value"].AsFloat;
								}
								if (jsonnode["image_file"] != null)
								{
									this.glossTexturePath = jsonnode["image_file"];
									this.hasGlossMap = true;
								}
								break;
							case "Glossiness":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null)
								{
									this.gloss = jsonnode["current_value"].AsFloat;
								}
								if (jsonnode["image_file"] != null)
								{
									this.glossTexturePath = jsonnode["image_file"];
									this.hasGlossMap = true;
								}
								break;
							case "Glossy Roughness":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null)
								{
									this.glossWeight = jsonnode["current_value"].AsFloat;
								}
								if (jsonnode["image_file"] != null)
								{
									this.roughnessTexturePath = jsonnode["image_file"];
									this.glossWeight = 1f;
									this.hasGlossMap = true;
								}
								break;
							case "Bump Strength":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null)
								{
									this.bumpStrength = jsonnode["current_value"].AsFloat;
								}
								if (jsonnode["image_file"] != null)
								{
									this.bumpTexturePath = jsonnode["image_file"];
									this.hasBumpMap = true;
								}
								break;
							case "Normal Map":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["image_file"] != null && (!this.hasBumpMap || !this.forceBumpAsNormalMap))
								{
									this.normalTexturePath = jsonnode["image_file"];
									this.hasNormalMap = true;
								}
								break;
							case "Subsurface Color":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null)
								{
								}
								if (jsonnode["image_file"] != null)
								{
									this.subsurfaceColorTexturePath = jsonnode["image_file"];
									this.hasSubsurfaceColorMap = true;
								}
								break;
							case "Subsurface Strength":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null)
								{
									this.subsurfaceStrength = jsonnode["current_value"].AsFloat;
								}
								if (jsonnode["image_file"] != null)
								{
									this.subsurfaceStrengthTexturePath = jsonnode["image_file"];
									this.hasSubsurfaceStrengthMap = true;
								}
								break;
							case "Reflection Color":
								this.hasReflectionColor = true;
								break;
							case "Reflection Strength":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null)
								{
									this.reflectionStrength = jsonnode["current_value"].AsFloat;
								}
								break;
							case "Translucency Color":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null)
								{
									this.translucencyColor = this.ProcessColorNode(jsonnode["current_value"]);
								}
								if (jsonnode["image_file"] != null)
								{
									this.translucencyColorTexturePath = jsonnode["image_file"];
									this.hasTranslucencyColorMap = true;
								}
								break;
							case "Translucency Strength":
								jsonnode = jsonnode8["channel"];
								if (jsonnode["current_value"] != null)
								{
									this.translucencyStrength = jsonnode["current_value"].AsFloat;
								}
								if (jsonnode["image_file"] != null)
								{
									this.translucencyStrengthTexturePath = jsonnode["image_file"];
									this.hasTranslucencyStrengthMap = true;
								}
								break;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
			}
		}

		// Token: 0x06004EBD RID: 20157 RVA: 0x001C0D90 File Offset: 0x001BF190
		public void Report()
		{
			string str = "Material " + this.name + " import report:";
			if (this.hasDiffuseMap)
			{
				str = str + " Has Diffuse Map " + this.diffuseTexturePath;
			}
			if (this.hasAlphaMap)
			{
				str = str + " Has Alpha Map " + this.alphaTexturePath;
			}
			if (this.hasSpecularColorMap)
			{
				str = str + " Has Specular Color Map " + this.specularColorTexturePath;
			}
			if (this.hasSpecularStrengthMap)
			{
				str = str + " Has Specular Strength Map " + this.specularStrengthTexturePath;
			}
			if (this.hasGlossMap)
			{
				str = str + " Has Gloss Map " + this.glossTexturePath;
			}
			if (this.hasBumpMap)
			{
				str = str + " Has Bump Map " + this.bumpTexturePath;
			}
			if (this.hasNormalMap)
			{
				str = str + " Has Normal Map " + this.normalTexturePath;
			}
			if (this.hasSubsurfaceColorMap)
			{
				str = str + " Has Subsurface Color Map " + this.subsurfaceColorTexturePath;
			}
			if (this.hasSubsurfaceStrengthMap)
			{
				str = str + " Has Subsurface Strength Map " + this.subsurfaceStrengthTexturePath;
			}
			if (this.hasTranslucencyColorMap)
			{
				str = str + " Has Translucency Color Map " + this.translucencyColorTexturePath;
			}
			if (this.hasTranslucencyStrengthMap)
			{
				str = str + " Has Translucency Strength Map " + this.translucencyStrengthTexturePath;
			}
		}

		// Token: 0x06004EBE RID: 20158 RVA: 0x001C0EF4 File Offset: 0x001BF2F4
		public void ImportImages(DAZImport importObject, string MaterialFolder)
		{
			if (this.diffuseTexturePath != null)
			{
				this.diffuseTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.diffuseTexturePath, false, false, false, 1f, false, false, null, string.Empty);
			}
			if (this.alphaTexturePath != null)
			{
				this.alphaTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.alphaTexturePath, false, true, false, 1f, false, false, null, string.Empty);
			}
			if (this.specularColorTexturePath != null)
			{
				this.specularColorTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.specularColorTexturePath, false, false, false, 1f, false, false, null, string.Empty);
			}
			if (this.specularStrengthTexturePath != null)
			{
				this.specularColorTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.specularStrengthTexturePath, false, false, false, 1f, false, true, null, string.Empty);
			}
			if (this.glossTexturePath != null)
			{
				this.glossTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.glossTexturePath, false, false, false, 1f, true, false, null, string.Empty);
			}
			if (this.roughnessTexturePath != null)
			{
				this.glossTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.roughnessTexturePath, false, false, false, 1f, true, false, "Inverted", string.Empty);
			}
			if (this.bumpTexturePath != null)
			{
				if (this.copyBumpAsSpecularColorMap)
				{
					this.specularColorTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.bumpTexturePath, false, false, false, 1f, false, true, null, "AsSpec");
				}
				this.bumpTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.bumpTexturePath, false, false, true, this.bumpStrength, false, false, null, string.Empty);
			}
			if (this.normalTexturePath != null)
			{
				this.normalTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.normalTexturePath, true, false, false, 1f, false, false, null, string.Empty);
			}
			if (this.subsurfaceColorTexturePath != null)
			{
				this.subsurfaceColorTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.subsurfaceColorTexturePath, false, false, false, 1f, false, false, null, string.Empty);
			}
			if (this.subsurfaceStrengthTexturePath != null)
			{
				this.subsurfaceStrengthTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.subsurfaceStrengthTexturePath, false, false, false, 1f, false, true, null, string.Empty);
			}
			if (this.translucencyColorTexturePath != null)
			{
				this.translucencyColorTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.translucencyColorTexturePath, false, false, false, 1f, false, false, null, string.Empty);
			}
			if (this.translucencyStrengthTexturePath != null)
			{
				this.translucencyStrengthTexture = this.CopyAndImportImage(importObject, MaterialFolder, this.translucencyStrengthTexturePath, false, false, false, 1f, false, true, null, string.Empty);
			}
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x001C116C File Offset: 0x001BF56C
		public Material CreateMaterialTypeMVR()
		{
			string text;
			if (this.isTransparent && this.hasReflectionColor && this.reflectionStrength > 0f)
			{
				text = this.reflTransparentShader;
			}
			else if (this.isTransparent)
			{
				if (this.hasNormalMap || this.hasBumpMap)
				{
					text = this.transparentNormalMapShader;
				}
				else
				{
					text = this.transparentShader;
				}
			}
			else if (this.hasNormalMap || this.hasBumpMap)
			{
				text = this.normalMapShader;
			}
			else if (this.hasGlossMap)
			{
				text = this.glossShader;
			}
			else
			{
				text = this.standardShader;
			}
			if (text == null)
			{
				Debug.LogError("Shader names not properly set on DAZImportMaterial object");
				return null;
			}
			Shader shader = Shader.Find(text);
			if (shader == null)
			{
				Debug.LogError("Could not find shader " + text + ". Can't import material");
				return null;
			}
			Material material = new Material(shader);
			if (material == null)
			{
				Debug.LogError("Failed to create material with shader " + shader.name);
			}
			else
			{
				material.name = this.name;
				if (this.isTransparent)
				{
					this.diffuseColor.a = this.alphaStrength;
				}
				if (material.HasProperty("_Color"))
				{
					Color color = this.diffuseColor;
					if (!this.ignoreDiffuseColor)
					{
						color *= this.diffuseStrength;
						color.a = this.diffuseColor.a;
						color.r = Mathf.Clamp01(color.r);
						color.g = Mathf.Clamp01(color.g);
						color.b = Mathf.Clamp01(color.b);
					}
					material.SetColor("_Color", color);
				}
				bool flag = false;
				if (material.HasProperty("_AlphaTex") && this.isTransparent && this.alphaTexture != null)
				{
					flag = true;
					material.SetTexture("_AlphaTex", this.alphaTexture);
					material.SetTextureScale("_AlphaTex", this.uvScale);
					material.SetTextureOffset("_AlphaTex", this.uvOffset);
				}
				if (material.HasProperty("_MainTex"))
				{
					if (this.diffuseTexture != null)
					{
						material.SetTexture("_MainTex", this.diffuseTexture);
						if (!flag && this.isTransparent && this.alphaTexture != null)
						{
							Debug.LogError("Alpha texture " + this.alphaTexturePath + " has gone unused because shader does not support separate alpha texture and a diffuse texture also exists");
						}
						material.SetTextureScale("_MainTex", this.uvScale);
						material.SetTextureOffset("_MainTex", this.uvOffset);
					}
					else if (!flag && this.isTransparent && this.alphaTexture != null)
					{
						material.SetTexture("_MainTex", this.alphaTexture);
						material.SetTextureScale("_MainTex", this.uvScale);
						material.SetTextureOffset("_MainTex", this.uvOffset);
					}
				}
				if (material.HasProperty("_SpecInt"))
				{
					material.SetFloat("_SpecInt", this.specularStrength);
				}
				if (material.HasProperty("_SpecColor"))
				{
					material.SetColor("_SpecColor", this.specularColor);
				}
				if (material.HasProperty("_SpecTex"))
				{
					if (this.specularColorTexture != null)
					{
						material.SetTexture("_SpecTex", this.specularColorTexture);
						material.SetTextureScale("_SpecTex", this.uvScale);
						material.SetTextureOffset("_SpecTex", this.uvOffset);
						if (this.specularStrengthTexture != null)
						{
							Debug.LogError("Have both specular color and specular strength textures. Only using specular color texture");
						}
					}
					else if (this.specularStrengthTexture != null)
					{
						material.SetTexture("_SpecTex", this.specularStrengthTexture);
						material.SetTextureScale("_SpecTex", this.uvScale);
						material.SetTextureOffset("_SpecTex", this.uvOffset);
					}
				}
				if (material.HasProperty("_Shininess"))
				{
					float value = 2f + this.gloss * this.glossWeight * 6f;
					value = Mathf.Clamp(value, 2f, 8f);
					material.SetFloat("_Shininess", value);
				}
				if (material.HasProperty("_Fresnel"))
				{
					material.SetFloat("_Fresnel", this.fresnelStrength);
				}
				if (material.HasProperty("_GlossTex"))
				{
					material.SetTexture("_GlossTex", this.glossTexture);
					material.SetTextureScale("_GlossTex", this.uvScale);
					material.SetTextureOffset("_GlossTex", this.uvOffset);
				}
				else if (this.glossTexture != null)
				{
					Debug.LogError("Found gloss texture, but shader " + text + " does not support it");
				}
				if (material.HasProperty("_BumpMap"))
				{
					if (this.normalTexture != null)
					{
						material.SetTexture("_BumpMap", this.normalTexture);
						material.SetTextureScale("_BumpMap", this.uvScale);
						material.SetTextureOffset("_BumpMap", this.uvOffset);
					}
					else if (this.bumpTexture != null)
					{
						material.SetTexture("_BumpMap", this.bumpTexture);
						material.SetTextureScale("_BumpMap", this.uvScale);
						material.SetTextureOffset("_BumpMap", this.uvOffset);
					}
				}
				else if (this.normalTexture != null || this.bumpTexture != null)
				{
					Debug.LogError("Found normal texture, but shader does not support normal mapping");
				}
				if (material.HasProperty("_DiffuseBumpiness"))
				{
					material.SetFloat("_DiffuseBumpiness", this.bumpiness);
				}
				if (material.HasProperty("_SpecularBumpiness"))
				{
					material.SetFloat("_SpecularBumpiness", this.bumpiness);
				}
				if (material.HasProperty("_DetailMap"))
				{
					if (this.normalTexture != null && this.bumpTexture != null)
					{
						material.SetTexture("_DetailMap", this.bumpTexture);
						material.SetTextureScale("_DetailMap", this.uvScale);
						material.SetTextureOffset("_DetailMap", this.uvOffset);
					}
				}
				else if (this.normalTexture != null && this.bumpTexture != null)
				{
					Debug.LogError("Found both normal and bump texture, but shader does not support detail map");
				}
				if (material.HasProperty("_SubdermisColor"))
				{
					material.SetColor("_SubdermisColor", this.subsurfaceColor);
				}
				if (material.HasProperty("_SubdermisTex"))
				{
					if (this.subsurfaceColorTexture != null)
					{
						material.SetTexture("_SubdermisTex", this.subsurfaceColorTexture);
						material.SetTextureScale("_SubdermisTex", this.uvScale);
						material.SetTextureOffset("_SubdermisTex", this.uvOffset);
					}
				}
				else if (this.subsurfaceColorTexture != null)
				{
					Debug.LogError("Found subsurface color texture, but shader does not support using it");
				}
				if (material.HasProperty("_Subdermis"))
				{
					material.SetFloat("_Subdermis", this.subsurfaceStrength);
				}
				if (this.subsurfaceStrengthTexture != null)
				{
					Debug.LogError("Found subsurface strength texture, but shader does not support using it");
				}
				if (material.HasProperty("_TranslucencyColor"))
				{
					material.SetColor("_TranslucencyColor", this.translucencyColor);
				}
				if (material.HasProperty("_TranslucencyTex"))
				{
					if (this.translucencyColorTexture != null)
					{
						material.SetTexture("_TranslucencyTex", this.translucencyColorTexture);
						material.SetTextureScale("_TranslucencyTex", this.uvScale);
						material.SetTextureOffset("_TranslucencyTex", this.uvOffset);
					}
				}
				else if (this.translucencyColorTexture != null)
				{
					Debug.LogError("Found translucency color texture, but shader does not support using it");
				}
				if (this.translucencyStrengthTexture != null)
				{
					Debug.LogError("Found translucency strength texture, but shader does not support using it");
				}
			}
			return material;
		}

		// Token: 0x06004EC0 RID: 20160 RVA: 0x001C1988 File Offset: 0x001BFD88
		public Material CreateMaterialTypeStandard(Shader s)
		{
			Material material = new Material(s);
			if (material == null)
			{
				Debug.LogError("Failed to create material with shader " + s.name);
			}
			else
			{
				Debug.LogError("TODO CreateMaterialTypeStandard");
			}
			return material;
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x001C19D0 File Offset: 0x001BFDD0
		public Material CreateMaterialTypeHDRP(Shader s)
		{
			Material material = new Material(s);
			if (material == null)
			{
				Debug.LogError("Failed to create material with shader " + s.name);
			}
			else
			{
				Debug.LogError("TODO CreateMaterialTypeHDRP");
			}
			return material;
		}

		// Token: 0x04003EBB RID: 16059
		public string name;

		// Token: 0x04003EBC RID: 16060
		public Vector2 uvScale;

		// Token: 0x04003EBD RID: 16061
		public Vector2 uvOffset;

		// Token: 0x04003EBE RID: 16062
		public bool hasDiffuseMap;

		// Token: 0x04003EBF RID: 16063
		public bool ignoreDiffuseColor;

		// Token: 0x04003EC0 RID: 16064
		public Color diffuseColor = Color.white;

		// Token: 0x04003EC1 RID: 16065
		public float diffuseStrength = 1f;

		// Token: 0x04003EC2 RID: 16066
		public string diffuseTexturePath;

		// Token: 0x04003EC3 RID: 16067
		public Texture2D diffuseTexture;

		// Token: 0x04003EC4 RID: 16068
		public bool isTransparent;

		// Token: 0x04003EC5 RID: 16069
		public bool hasAlphaMap;

		// Token: 0x04003EC6 RID: 16070
		public float alphaStrength = 1f;

		// Token: 0x04003EC7 RID: 16071
		public string alphaTexturePath;

		// Token: 0x04003EC8 RID: 16072
		public Texture2D alphaTexture;

		// Token: 0x04003EC9 RID: 16073
		public bool useSpecularAsGlossMap;

		// Token: 0x04003ECA RID: 16074
		public bool hasSpecularColorMap;

		// Token: 0x04003ECB RID: 16075
		public bool ignoreSpecularColor;

		// Token: 0x04003ECC RID: 16076
		public Color specularColor = Color.white;

		// Token: 0x04003ECD RID: 16077
		public float fresnelStrength = 0.5f;

		// Token: 0x04003ECE RID: 16078
		public string specularColorTexturePath;

		// Token: 0x04003ECF RID: 16079
		public Texture2D specularColorTexture;

		// Token: 0x04003ED0 RID: 16080
		public bool hasSpecularStrengthMap;

		// Token: 0x04003ED1 RID: 16081
		public float specularStrength = 1f;

		// Token: 0x04003ED2 RID: 16082
		public string specularStrengthTexturePath;

		// Token: 0x04003ED3 RID: 16083
		public Texture2D specularStrengthTexture;

		// Token: 0x04003ED4 RID: 16084
		public bool hasGlossMap;

		// Token: 0x04003ED5 RID: 16085
		public float gloss = 0.5f;

		// Token: 0x04003ED6 RID: 16086
		public float glossWeight = 1f;

		// Token: 0x04003ED7 RID: 16087
		public string glossTexturePath;

		// Token: 0x04003ED8 RID: 16088
		public string roughnessTexturePath;

		// Token: 0x04003ED9 RID: 16089
		public Texture2D glossTexture;

		// Token: 0x04003EDA RID: 16090
		public bool copyBumpAsSpecularColorMap;

		// Token: 0x04003EDB RID: 16091
		public bool forceBumpAsNormalMap;

		// Token: 0x04003EDC RID: 16092
		public bool hasBumpMap;

		// Token: 0x04003EDD RID: 16093
		public float bumpStrength = 1f;

		// Token: 0x04003EDE RID: 16094
		public float bumpiness = 1f;

		// Token: 0x04003EDF RID: 16095
		public string bumpTexturePath;

		// Token: 0x04003EE0 RID: 16096
		public Texture2D bumpTexture;

		// Token: 0x04003EE1 RID: 16097
		public bool hasNormalMap;

		// Token: 0x04003EE2 RID: 16098
		public string normalTexturePath;

		// Token: 0x04003EE3 RID: 16099
		public Texture2D normalTexture;

		// Token: 0x04003EE4 RID: 16100
		public bool hasSubsurfaceColorMap;

		// Token: 0x04003EE5 RID: 16101
		public Color subsurfaceColor = Color.white;

		// Token: 0x04003EE6 RID: 16102
		public string subsurfaceColorTexturePath;

		// Token: 0x04003EE7 RID: 16103
		public Texture2D subsurfaceColorTexture;

		// Token: 0x04003EE8 RID: 16104
		public bool hasSubsurfaceStrengthMap;

		// Token: 0x04003EE9 RID: 16105
		public float subsurfaceStrength;

		// Token: 0x04003EEA RID: 16106
		public string subsurfaceStrengthTexturePath;

		// Token: 0x04003EEB RID: 16107
		public Texture2D subsurfaceStrengthTexture;

		// Token: 0x04003EEC RID: 16108
		public bool hasTranslucencyColorMap;

		// Token: 0x04003EED RID: 16109
		public Color translucencyColor = Color.white;

		// Token: 0x04003EEE RID: 16110
		public string translucencyColorTexturePath;

		// Token: 0x04003EEF RID: 16111
		public Texture2D translucencyColorTexture;

		// Token: 0x04003EF0 RID: 16112
		public bool hasTranslucencyStrengthMap;

		// Token: 0x04003EF1 RID: 16113
		public float translucencyStrength;

		// Token: 0x04003EF2 RID: 16114
		public string translucencyStrengthTexturePath;

		// Token: 0x04003EF3 RID: 16115
		public Texture2D translucencyStrengthTexture;

		// Token: 0x04003EF4 RID: 16116
		public bool hasReflectionColor;

		// Token: 0x04003EF5 RID: 16117
		public float reflectionStrength;

		// Token: 0x04003EF6 RID: 16118
		public string standardShader;

		// Token: 0x04003EF7 RID: 16119
		public string glossShader;

		// Token: 0x04003EF8 RID: 16120
		public string normalMapShader;

		// Token: 0x04003EF9 RID: 16121
		public string transparentShader;

		// Token: 0x04003EFA RID: 16122
		public string reflTransparentShader;

		// Token: 0x04003EFB RID: 16123
		public string transparentNormalMapShader;

		// Token: 0x04003EFC RID: 16124
		[CompilerGenerated]
		private static Dictionary<string, int> <>f__switch$map1;
	}
}
