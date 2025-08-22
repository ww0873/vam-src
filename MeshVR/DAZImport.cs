using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.GZip;
using Microsoft.Win32;
using MVR;
using MVR.FileManagement;
using SimpleJSON;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000B39 RID: 2873
	[ExecuteInEditMode]
	public class DAZImport : ObjectAllocator
	{
		// Token: 0x06004E7E RID: 20094 RVA: 0x001BA238 File Offset: 0x001B8638
		public DAZImport()
		{
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x001BA3EC File Offset: 0x001B87EC
		public string GetDefaultDAZContentPath()
		{
			if (UserPreferences.singleton != null && UserPreferences.singleton.DAZDefaultContentFolder != null && UserPreferences.singleton.DAZDefaultContentFolder != string.Empty)
			{
				return UserPreferences.singleton.DAZDefaultContentFolder;
			}
			if (this.registryDAZLibraryDirectories != null && this.registryDAZLibraryDirectories.Count > 0)
			{
				return this.registryDAZLibraryDirectories[0];
			}
			return null;
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x001BA468 File Offset: 0x001B8868
		public string DetermineFilePath(string pathkey)
		{
			string text = null;
			bool flag = false;
			if (this.DAZLibraryDirectory != null && this.DAZLibraryDirectory != string.Empty)
			{
				text = this.DAZLibraryDirectory + pathkey;
				flag = File.Exists(text);
			}
			if (!flag && this.alternateDAZLibraryDirectory != null && this.alternateDAZLibraryDirectory != string.Empty)
			{
				text = this.alternateDAZLibraryDirectory + pathkey;
				flag = File.Exists(text);
			}
			if (!flag && UserPreferences.singleton != null && UserPreferences.singleton.DAZExtraLibraryFolder != null && UserPreferences.singleton.DAZExtraLibraryFolder != string.Empty)
			{
				text = UserPreferences.singleton.DAZExtraLibraryFolder + "/" + pathkey;
				flag = File.Exists(text);
			}
			if (!flag && this.registryDAZLibraryDirectories != null)
			{
				foreach (string str in this.registryDAZLibraryDirectories)
				{
					text = str + "/" + pathkey;
					flag = File.Exists(text);
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				return text;
			}
			return null;
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x001BA5C4 File Offset: 0x001B89C4
		public string GetTextureSourcePath(Texture2D tex)
		{
			string result = null;
			if (this.textureToSourcePath == null)
			{
				this.textureToSourcePath = new Dictionary<Texture2D, string>();
			}
			this.textureToSourcePath.TryGetValue(tex, out result);
			return result;
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x001BA5FC File Offset: 0x001B89FC
		public void SetTextureSourcePath(Texture2D tex, string path)
		{
			if (this.textureToSourcePath == null)
			{
				this.textureToSourcePath = new Dictionary<Texture2D, string>();
			}
			if (this.textureToSourcePath.ContainsKey(tex))
			{
				this.textureToSourcePath.Remove(tex);
			}
			this.textureToSourcePath.Add(tex, path);
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06004E83 RID: 20099 RVA: 0x001BA64A File Offset: 0x001B8A4A
		public bool shouldCreateSkinsAndNodes
		{
			get
			{
				return (this.importType == DAZImport.ImportType.Environment || this.importType == DAZImport.ImportType.Character || this.importType == DAZImport.ImportType.SkinnedSingleObject) && this.createSkinsAndNodes;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06004E84 RID: 20100 RVA: 0x001BA678 File Offset: 0x001B8A78
		public bool shouldCreateSkins
		{
			get
			{
				return (this.importType == DAZImport.ImportType.SkinnedSingleObject || this.importType == DAZImport.ImportType.Character) && this.createSkins;
			}
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06004E85 RID: 20101 RVA: 0x001BA69A File Offset: 0x001B8A9A
		public bool shouldCreateSkinWrap
		{
			get
			{
				return (this.importType == DAZImport.ImportType.Clothing || this.importType == DAZImport.ImportType.HairScalp) && this.createSkinWrap;
			}
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x001BA6BC File Offset: 0x001B8ABC
		public static JSONNode ReadJSON(string path)
		{
			JSONNode result = null;
			try
			{
				FileManager.AssertNotCalledFromPlugin();
				if (FileChecker.IsGzipped(path))
				{
					using (FileEntryStream fileEntryStream = FileManager.OpenStream(path, false))
					{
						using (Stream stream = new GZipInputStream(fileEntryStream.Stream))
						{
							using (StreamReader streamReader = new StreamReader(stream))
							{
								string aJSON = streamReader.ReadToEnd();
								result = JSON.Parse(aJSON);
							}
						}
					}
				}
				else
				{
					using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(path, false))
					{
						string aJSON2 = fileEntryStreamReader.ReadToEnd();
						result = JSON.Parse(aJSON2);
					}
				}
			}
			catch (Exception ex)
			{
				SuperController.LogError("Exception during ReadJSON " + path + " " + ex.Message);
			}
			return result;
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x001BA7D8 File Offset: 0x001B8BD8
		private List<string> ProcessDAZLibraries(string url, JSONNode jl)
		{
			if (this.DAZ_node_library == null)
			{
				this.DAZ_node_library = new Dictionary<string, JSONNode>();
			}
			JSONNode jsonnode = jl["node_library"];
			if (jsonnode != null)
			{
				IEnumerator enumerator = jsonnode.AsArray.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						JSONNode jsonnode2 = (JSONNode)obj;
						string str = jsonnode2["id"];
						string key = url + "#" + str;
						if (url == string.Empty && this.DAZ_node_library.ContainsKey(key))
						{
							this.DAZ_node_library.Remove(key);
						}
						this.DAZ_node_library.Add(key, jsonnode2);
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
			if (this.DAZ_geometry_library == null)
			{
				this.DAZ_geometry_library = new Dictionary<string, JSONNode>();
			}
			JSONNode jsonnode3 = jl["geometry_library"];
			if (jsonnode3 != null)
			{
				IEnumerator enumerator2 = jsonnode3.AsArray.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						JSONNode jsonnode4 = (JSONNode)obj2;
						string str2 = jsonnode4["id"];
						string key2 = url + "#" + str2;
						if (url == string.Empty && this.DAZ_geometry_library.ContainsKey(key2))
						{
							this.DAZ_geometry_library.Remove(key2);
						}
						this.DAZ_geometry_library.Add(key2, jsonnode4);
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
			if (this.DAZ_modifier_library == null)
			{
				this.DAZ_modifier_library = new Dictionary<string, JSONNode>();
			}
			List<string> list = new List<string>();
			JSONNode jsonnode5 = jl["modifier_library"];
			if (jsonnode5 != null)
			{
				IEnumerator enumerator3 = jsonnode5.AsArray.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object obj3 = enumerator3.Current;
						JSONNode jsonnode6 = (JSONNode)obj3;
						string str3 = jsonnode6["id"];
						string text = url + "#" + str3;
						list.Add(text);
						if (url == string.Empty && this.DAZ_modifier_library.ContainsKey(text))
						{
							this.DAZ_modifier_library.Remove(text);
						}
						this.DAZ_modifier_library.Add(text, jsonnode6);
					}
				}
				finally
				{
					IDisposable disposable3;
					if ((disposable3 = (enumerator3 as IDisposable)) != null)
					{
						disposable3.Dispose();
					}
				}
			}
			if (this.DAZ_uv_library == null)
			{
				this.DAZ_uv_library = new Dictionary<string, JSONNode>();
			}
			JSONNode jsonnode7 = jl["uv_set_library"];
			if (jsonnode7 != null)
			{
				IEnumerator enumerator4 = jsonnode7.AsArray.GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						object obj4 = enumerator4.Current;
						JSONNode jsonnode8 = (JSONNode)obj4;
						string str4 = jsonnode8["id"];
						string key3 = url + "#" + str4;
						if (url == string.Empty && this.DAZ_uv_library.ContainsKey(key3))
						{
							this.DAZ_uv_library.Remove(key3);
						}
						this.DAZ_uv_library.Add(key3, jsonnode8);
					}
				}
				finally
				{
					IDisposable disposable4;
					if ((disposable4 = (enumerator4 as IDisposable)) != null)
					{
						disposable4.Dispose();
					}
				}
			}
			if (this.DAZ_material_library == null)
			{
				this.DAZ_material_library = new Dictionary<string, JSONNode>();
			}
			JSONNode jsonnode9 = jl["material_library"];
			if (jsonnode9 != null)
			{
				IEnumerator enumerator5 = jsonnode9.AsArray.GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						object obj5 = enumerator5.Current;
						JSONNode jsonnode10 = (JSONNode)obj5;
						string str5 = jsonnode10["id"];
						string key4 = url + "#" + str5;
						if (url == string.Empty && this.DAZ_material_library.ContainsKey(key4))
						{
							this.DAZ_material_library.Remove(key4);
						}
						this.DAZ_material_library.Add(key4, jsonnode10);
					}
				}
				finally
				{
					IDisposable disposable5;
					if ((disposable5 = (enumerator5 as IDisposable)) != null)
					{
						disposable5.Dispose();
					}
				}
			}
			return list;
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x001BAC70 File Offset: 0x001B9070
		private List<string> ProcessAltModifiers(string url, JSONNode jl)
		{
			if (this.DAZ_modifier_library == null)
			{
				this.DAZ_modifier_library = new Dictionary<string, JSONNode>();
			}
			List<string> list = new List<string>();
			JSONNode jsonnode = jl["modifier_library"];
			if (jsonnode != null)
			{
				IEnumerator enumerator = jsonnode.AsArray.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						JSONNode jsonnode2 = (JSONNode)obj;
						string str = jsonnode2["id"];
						string text = url + "#" + str;
						list.Add(text);
						if (this.DAZ_modifier_library.ContainsKey(text))
						{
							this.DAZ_modifier_library.Remove(text);
						}
						this.DAZ_modifier_library.Add(text, jsonnode2);
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
			return list;
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x001BAD5C File Offset: 0x001B915C
		public static string ReplaceHex(Match m)
		{
			string value = m.Value.Replace("%", string.Empty);
			uint value2 = Convert.ToUInt32(value, 16);
			return Convert.ToChar(value2).ToString();
		}

		// Token: 0x06004E8A RID: 20106 RVA: 0x001BAD9C File Offset: 0x001B919C
		public static string DAZurlFix(string url)
		{
			MatchEvaluator evaluator = new MatchEvaluator(DAZImport.ReplaceHex);
			return Regex.Replace(url, "%([0-9A-Z][0-9A-Z])", evaluator);
		}

		// Token: 0x06004E8B RID: 20107 RVA: 0x001BADC4 File Offset: 0x001B91C4
		public static string DAZurlToPathKey(string url)
		{
			url = DAZImport.DAZurlFix(url);
			return Regex.Replace(url, "#.*", string.Empty);
		}

		// Token: 0x06004E8C RID: 20108 RVA: 0x001BADEC File Offset: 0x001B91EC
		public static string DAZurlToId(string url)
		{
			url = DAZImport.DAZurlFix(url);
			return Regex.Replace(url, ".*#", string.Empty);
		}

		// Token: 0x06004E8D RID: 20109 RVA: 0x001BAE14 File Offset: 0x001B9214
		public List<string> ReadDAZdsf(string url)
		{
			string text = DAZImport.DAZurlToPathKey(url);
			if (!(text != string.Empty))
			{
				throw new Exception("Could not get path key from url " + url);
			}
			string text2 = this.DetermineFilePath(text);
			if (text2 != null)
			{
				JSONNode jl = DAZImport.ReadJSON(text2);
				return this.ProcessDAZLibraries(text, jl);
			}
			throw new Exception("File " + text + " could not be found in libraries. Check DAZ and alternate library paths for correctness.");
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x001BAE8C File Offset: 0x001B928C
		private JSONNode GetMaterial(string url)
		{
			if (this.DAZ_material_library == null)
			{
				this.DAZ_material_library = new Dictionary<string, JSONNode>();
			}
			url = DAZImport.DAZurlFix(url);
			if (!this.DAZ_material_library.ContainsKey(url))
			{
				this.ReadDAZdsf(url);
			}
			JSONNode result = null;
			if (!this.DAZ_material_library.TryGetValue(url, out result))
			{
				throw new Exception("Could not find material at " + url);
			}
			return result;
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x001BAEF8 File Offset: 0x001B92F8
		private JSONNode GetUV(string url)
		{
			if (this.DAZ_uv_library == null)
			{
				this.DAZ_uv_library = new Dictionary<string, JSONNode>();
			}
			url = DAZImport.DAZurlFix(url);
			if (!this.DAZ_uv_library.ContainsKey(url))
			{
				this.ReadDAZdsf(url);
			}
			JSONNode result;
			if (!this.DAZ_uv_library.TryGetValue(url, out result))
			{
				throw new Exception("Could not find uv at " + url);
			}
			return result;
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x001BAF64 File Offset: 0x001B9364
		private JSONNode GetGeometry(string url)
		{
			if (this.DAZ_geometry_library == null)
			{
				this.DAZ_geometry_library = new Dictionary<string, JSONNode>();
			}
			url = DAZImport.DAZurlFix(url);
			if (!this.DAZ_geometry_library.ContainsKey(url))
			{
				this.ReadDAZdsf(url);
			}
			JSONNode result;
			if (!this.DAZ_geometry_library.TryGetValue(url, out result))
			{
				throw new Exception("Could not find geometry at " + url);
			}
			return result;
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x001BAFD0 File Offset: 0x001B93D0
		private JSONNode GetNode(string url)
		{
			if (this.DAZ_node_library == null)
			{
				this.DAZ_node_library = new Dictionary<string, JSONNode>();
			}
			url = DAZImport.DAZurlFix(url);
			if (!this.DAZ_node_library.ContainsKey(url))
			{
				this.ReadDAZdsf(url);
			}
			JSONNode result;
			if (!this.DAZ_node_library.TryGetValue(url, out result))
			{
				throw new Exception("Could not find node at " + url);
			}
			return result;
		}

		// Token: 0x06004E92 RID: 20114 RVA: 0x001BB03C File Offset: 0x001B943C
		private JSONNode GetModifier(string url)
		{
			if (this.DAZ_modifier_library == null)
			{
				this.DAZ_modifier_library = new Dictionary<string, JSONNode>();
			}
			url = DAZImport.DAZurlFix(url);
			if (!this.DAZ_modifier_library.ContainsKey(url))
			{
				this.ReadDAZdsf(url);
			}
			JSONNode result;
			if (!this.DAZ_modifier_library.TryGetValue(url, out result))
			{
				throw new Exception("Could not find modifier at " + url);
			}
			return result;
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x001BB0A8 File Offset: 0x001B94A8
		private DAZUVMap ProcessUV(string uvurl)
		{
			DAZUVMap dazuvmap;
			if (uvurl == null)
			{
				dazuvmap = new DAZUVMap();
			}
			else
			{
				if (this.DAZ_uv_map.TryGetValue(uvurl, out dazuvmap))
				{
					return dazuvmap;
				}
				dazuvmap = new DAZUVMap();
				this.DAZ_uv_map.Add(uvurl, dazuvmap);
				JSONNode uv = this.GetUV(uvurl);
				if (uv != null)
				{
					dazuvmap.Import(uv);
				}
			}
			return dazuvmap;
		}

		// Token: 0x06004E94 RID: 20116 RVA: 0x001BB10C File Offset: 0x001B950C
		private DAZMesh GetDAZMeshBySceneGeometryId(string sceneGeometryId)
		{
			DAZMesh[] array = base.GetComponents<DAZMesh>();
			DAZMesh dazmesh = null;
			foreach (DAZMesh dazmesh2 in array)
			{
				if (dazmesh2.sceneGeometryId == sceneGeometryId)
				{
					dazmesh = dazmesh2;
					break;
				}
			}
			if (dazmesh == null && this.container != null && this.embedMeshAndSkinOnNodes)
			{
				array = this.container.GetComponentsInChildren<DAZMesh>();
				foreach (DAZMesh dazmesh3 in array)
				{
					if (dazmesh3.sceneGeometryId == sceneGeometryId)
					{
						dazmesh = dazmesh3;
						break;
					}
				}
			}
			return dazmesh;
		}

		// Token: 0x06004E95 RID: 20117 RVA: 0x001BB1CC File Offset: 0x001B95CC
		private DAZMesh GetDAZMeshByGeometryId(string geometryId)
		{
			DAZMesh[] array = base.GetComponents<DAZMesh>();
			DAZMesh dazmesh = null;
			foreach (DAZMesh dazmesh2 in array)
			{
				if (dazmesh2.geometryId == geometryId)
				{
					dazmesh = dazmesh2;
					break;
				}
			}
			if (dazmesh == null && this.container != null && this.embedMeshAndSkinOnNodes)
			{
				array = this.container.GetComponentsInChildren<DAZMesh>();
				foreach (DAZMesh dazmesh3 in array)
				{
					if (dazmesh3.geometryId == geometryId)
					{
						dazmesh = dazmesh3;
						break;
					}
				}
			}
			return dazmesh;
		}

		// Token: 0x06004E96 RID: 20118 RVA: 0x001BB28C File Offset: 0x001B968C
		private DAZMesh GetDAZMeshByNodeId(string nodeId)
		{
			DAZMesh[] array = base.GetComponents<DAZMesh>();
			DAZMesh dazmesh = null;
			foreach (DAZMesh dazmesh2 in array)
			{
				if (dazmesh2.nodeId == nodeId)
				{
					dazmesh = dazmesh2;
					break;
				}
			}
			if (dazmesh == null && this.container != null && this.embedMeshAndSkinOnNodes)
			{
				array = this.container.GetComponentsInChildren<DAZMesh>();
				foreach (DAZMesh dazmesh3 in array)
				{
					if (dazmesh3.nodeId == nodeId)
					{
						dazmesh = dazmesh3;
						break;
					}
				}
			}
			return dazmesh;
		}

		// Token: 0x06004E97 RID: 20119 RVA: 0x001BB34C File Offset: 0x001B974C
		private DAZMesh GetDAZMeshBySceneNodeId(string sceneNodeId)
		{
			DAZMesh[] array = base.GetComponents<DAZMesh>();
			DAZMesh dazmesh = null;
			foreach (DAZMesh dazmesh2 in array)
			{
				if (dazmesh2.sceneNodeId == sceneNodeId)
				{
					dazmesh = dazmesh2;
					break;
				}
			}
			if (dazmesh == null && this.container != null && this.embedMeshAndSkinOnNodes)
			{
				array = this.container.GetComponentsInChildren<DAZMesh>();
				foreach (DAZMesh dazmesh3 in array)
				{
					if (dazmesh3.sceneNodeId == sceneNodeId)
					{
						dazmesh = dazmesh3;
						break;
					}
				}
			}
			return dazmesh;
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x001BB40C File Offset: 0x001B980C
		private DAZMesh CreateDAZMesh(string sceneGeometryId, string geometryId, string sceneNodeId, string nodeId, GameObject meshContainer)
		{
			DAZMesh dazmesh = meshContainer.AddComponent<DAZMesh>();
			dazmesh.sceneGeometryId = sceneGeometryId;
			dazmesh.geometryId = geometryId;
			dazmesh.nodeId = nodeId;
			dazmesh.sceneNodeId = sceneNodeId;
			if ((!this.shouldCreateSkinsAndNodes || !this.shouldCreateSkins) && !this.shouldCreateSkinWrap)
			{
				dazmesh.drawMorphedUVMappedMesh = true;
			}
			if (this.shouldCreateSkinWrap)
			{
				dazmesh.drawInEditorWhenNotPlaying = true;
			}
			return dazmesh;
		}

		// Token: 0x06004E99 RID: 20121 RVA: 0x001BB47C File Offset: 0x001B987C
		private DAZMesh GetOrCreateDAZMesh(string sceneGeometryId, string geometryId, string sceneNodeId, string nodeId, GameObject meshContainer)
		{
			DAZMesh dazmesh = this.GetDAZMeshBySceneGeometryId(sceneGeometryId);
			if (dazmesh == null)
			{
				dazmesh = this.CreateDAZMesh(sceneGeometryId, geometryId, sceneNodeId, nodeId, meshContainer);
			}
			else
			{
				dazmesh.sceneNodeId = sceneNodeId;
				dazmesh.nodeId = nodeId;
			}
			return dazmesh;
		}

		// Token: 0x06004E9A RID: 20122 RVA: 0x001BB4C0 File Offset: 0x001B98C0
		private DAZSkinV2 GetDAZSkin(string skinId, GameObject skinContainer)
		{
			DAZSkinV2[] components = skinContainer.GetComponents<DAZSkinV2>();
			DAZSkinV2 result = null;
			foreach (DAZSkinV2 dazskinV in components)
			{
				if (dazskinV.skinId == skinId)
				{
					result = dazskinV;
					break;
				}
			}
			return result;
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x001BB510 File Offset: 0x001B9910
		private DAZSkinV2 CreateDAZSkin(string skinId, string skinUrl, GameObject skinContainer)
		{
			DAZSkinV2 dazskinV = skinContainer.AddComponent<DAZSkinV2>();
			dazskinV.skinId = skinId;
			dazskinV.skinUrl = DAZImport.DAZurlFix(skinUrl);
			dazskinV.physicsType = this.skinPhysicsType;
			if (Application.isPlaying)
			{
				if (this.GPUSkinCompute != null)
				{
					dazskinV.GPUSkinner = this.GPUSkinCompute;
				}
				if (this.GPUMeshCompute != null)
				{
					dazskinV.GPUMeshCompute = this.GPUMeshCompute;
				}
			}
			return dazskinV;
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x001BB588 File Offset: 0x001B9988
		private DAZSkinV2 GetOrCreateDAZSkin(string skinId, string skinUrl, GameObject skinContainer)
		{
			DAZSkinV2 dazskinV = this.GetDAZSkin(skinId, skinContainer);
			if (dazskinV == null)
			{
				dazskinV = this.CreateDAZSkin(skinId, skinUrl, skinContainer);
			}
			return dazskinV;
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x001BB5B8 File Offset: 0x001B99B8
		private DAZSkinWrap GetDAZSkinWrap(DAZMesh dazMesh)
		{
			DAZSkinWrap[] components = dazMesh.gameObject.GetComponents<DAZSkinWrap>();
			DAZSkinWrap result = null;
			foreach (DAZSkinWrap dazskinWrap in components)
			{
				if (dazskinWrap.dazMesh == dazMesh)
				{
					result = dazskinWrap;
					break;
				}
			}
			return result;
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x001BB60C File Offset: 0x001B9A0C
		private DAZSkinWrap CreateDAZSkinWrap(DAZMesh dazMesh)
		{
			DAZSkinWrap dazskinWrap = dazMesh.gameObject.AddComponent<DAZSkinWrap>();
			dazskinWrap.skinTransform = this.skinToWrapToTransform;
			dazskinWrap.skin = this.skinToWrapTo;
			dazskinWrap.dazMesh = dazMesh;
			if (Application.isPlaying)
			{
				if (this.GPUSkinCompute != null)
				{
					dazskinWrap.GPUSkinWrapper = this.GPUSkinCompute;
				}
				if (this.GPUMeshCompute != null)
				{
					dazskinWrap.GPUMeshCompute = this.GPUMeshCompute;
				}
			}
			return dazskinWrap;
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x001BB68C File Offset: 0x001B9A8C
		private DAZSkinWrap GetOrCreateDAZSkinWrap(DAZMesh dazMesh)
		{
			DAZSkinWrap dazskinWrap = this.GetDAZSkinWrap(dazMesh);
			if (dazskinWrap == null)
			{
				dazskinWrap = this.CreateDAZSkinWrap(dazMesh);
			}
			return dazskinWrap;
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x001BB6B8 File Offset: 0x001B9AB8
		private DAZMesh ProcessGeometry(string geourl, string sceneGeometryId, DAZUVMap[] uvmaplist, string sceneNodeId, string nodeId, GameObject meshContainer)
		{
			JSONNode geometry = this.GetGeometry(geourl);
			string geometryId = geometry["id"];
			DAZMesh orCreateDAZMesh = this.GetOrCreateDAZMesh(sceneGeometryId, geometryId, sceneNodeId, nodeId, meshContainer);
			if (orCreateDAZMesh != null)
			{
				orCreateDAZMesh.Import(geometry, uvmaplist[0], this.DAZ_material_map, true);
			}
			return orCreateDAZMesh;
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x001BB70C File Offset: 0x001B9B0C
		private void SetPositionRotation(JSONNode jn, JSONNode sn, Transform t)
		{
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			IEnumerator enumerator = jn["center_point"].AsArray.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					JSONNode jsonnode = (JSONNode)obj;
					string a = jsonnode["id"];
					if (a == "x")
					{
						zero.x = jsonnode["value"].AsFloat * -0.01f;
					}
					else if (a == "y")
					{
						zero.y = jsonnode["value"].AsFloat * 0.01f;
					}
					else if (a == "z")
					{
						zero.z = jsonnode["value"].AsFloat * 0.01f;
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
			IEnumerator enumerator2 = sn["center_point"].AsArray.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					JSONNode jsonnode2 = (JSONNode)obj2;
					string a2 = jsonnode2["id"];
					if (a2 == "x")
					{
						zero.x = jsonnode2["current_value"].AsFloat * -0.01f;
					}
					else if (a2 == "y")
					{
						zero.y = jsonnode2["current_value"].AsFloat * 0.01f;
					}
					else if (a2 == "z")
					{
						zero.z = jsonnode2["current_value"].AsFloat * 0.01f;
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
			IEnumerator enumerator3 = jn["orientation"].AsArray.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					object obj3 = enumerator3.Current;
					JSONNode jsonnode3 = (JSONNode)obj3;
					string a3 = jsonnode3["id"];
					if (a3 == "x")
					{
						zero2.x = jsonnode3["value"].AsFloat;
					}
					else if (a3 == "y")
					{
						zero2.y = -jsonnode3["value"].AsFloat;
					}
					else if (a3 == "z")
					{
						zero2.z = -jsonnode3["value"].AsFloat;
					}
				}
			}
			finally
			{
				IDisposable disposable3;
				if ((disposable3 = (enumerator3 as IDisposable)) != null)
				{
					disposable3.Dispose();
				}
			}
			IEnumerator enumerator4 = sn["orientation"].AsArray.GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					object obj4 = enumerator4.Current;
					JSONNode jsonnode4 = (JSONNode)obj4;
					string a4 = jsonnode4["id"];
					if (a4 == "x")
					{
						zero2.x = jsonnode4["current_value"].AsFloat;
					}
					else if (a4 == "y")
					{
						zero2.y = -jsonnode4["current_value"].AsFloat;
					}
					else if (a4 == "z")
					{
						zero2.z = -jsonnode4["current_value"].AsFloat;
					}
				}
			}
			finally
			{
				IDisposable disposable4;
				if ((disposable4 = (enumerator4 as IDisposable)) != null)
				{
					disposable4.Dispose();
				}
			}
			t.position = zero;
			t.rotation = Quaternion.Euler(zero2);
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x001BBB34 File Offset: 0x001B9F34
		private bool NodeExists(JSONNode sn, bool isRoot)
		{
			string url = DAZImport.DAZurlFix(sn["url"]);
			JSONNode node = this.GetNode(url);
			string name = node["name"];
			if (isRoot)
			{
				if (this.useSceneLabelsInsteadOfIds)
				{
					name = sn["label"];
				}
				else
				{
					name = sn["id"];
				}
			}
			Transform x = this.container.Find(name);
			return x != null;
		}

		// Token: 0x06004EA3 RID: 20131 RVA: 0x001BBBC4 File Offset: 0x001B9FC4
		private string ProcessNodeCreation(JSONNode sn, bool isRoot)
		{
			string url = DAZImport.DAZurlFix(sn["url"]);
			string text = sn["id"];
			JSONNode node = this.GetNode(url);
			string result = node["id"];
			if (sn["conform_target"] != null)
			{
				string text2 = DAZImport.DAZurlToId(sn["conform_target"]);
				if (this.graftIDToMainID == null)
				{
					this.graftIDToMainID = new Dictionary<string, string>();
				}
				this.graftIDToMainID.Add(text, text2);
				result = text2;
			}
			else if (this.shouldCreateSkinsAndNodes)
			{
				if (this.sceneNodeIDToTransform == null)
				{
					this.sceneNodeIDToTransform = new Dictionary<string, Transform>();
				}
				Transform transform;
				if (sn["parent"] != null)
				{
					string text3 = DAZImport.DAZurlToId(sn["parent"]);
					string text4;
					if (this.graftIDToMainID != null && this.graftIDToMainID.TryGetValue(text3, out text4))
					{
						text3 = text4;
					}
					if (!this.sceneNodeIDToTransform.TryGetValue(text3, out transform))
					{
						UnityEngine.Debug.LogWarning("Could not find parent transform " + text3);
						transform = this.container;
					}
				}
				else
				{
					transform = this.container;
				}
				string name = node["name"];
				if (isRoot)
				{
					if (this.useSceneLabelsInsteadOfIds)
					{
						name = sn["label"];
					}
					else
					{
						name = text;
					}
				}
				GameObject gameObject = null;
				Transform transform2 = transform.Find(name);
				if (transform2 == null)
				{
					transform2 = this.container.Find(name);
				}
				if (transform2 != null)
				{
					gameObject = transform2.gameObject;
				}
				if (gameObject == null)
				{
					if (isRoot && this.nodePrefab != null)
					{
						if (Application.isPlaying)
						{
							gameObject = UnityEngine.Object.Instantiate<GameObject>(this.nodePrefab.gameObject);
						}
						gameObject.name = name;
						gameObject.transform.parent = transform;
						Transform transform3 = gameObject.transform.Find("object");
						if (transform3 != null)
						{
							this.sceneNodeIDToTransform.Add(text, transform3.transform);
						}
						else
						{
							this.sceneNodeIDToTransform.Add(text, gameObject.transform);
						}
					}
					else
					{
						gameObject = new GameObject(name);
						gameObject.transform.parent = transform;
						this.sceneNodeIDToTransform.Add(text, gameObject.transform);
					}
				}
				else
				{
					gameObject.transform.parent = transform;
					if (isRoot && this.nodePrefab != null)
					{
						Transform transform4 = gameObject.transform.Find("object");
						if (transform4 != null)
						{
							this.sceneNodeIDToTransform.Add(text, transform4.transform);
						}
						else
						{
							this.sceneNodeIDToTransform.Add(text, gameObject.transform);
						}
					}
					else
					{
						this.sceneNodeIDToTransform.Add(text, gameObject.transform);
					}
				}
				if (isRoot)
				{
					this.SetPositionRotation(node, sn, gameObject.transform);
				}
				else if (this.shouldCreateSkins)
				{
					DAZBone dazbone = gameObject.GetComponent<DAZBone>();
					if (dazbone == null)
					{
						dazbone = gameObject.AddComponent<DAZBone>();
					}
					dazbone.ImportNode(node, this.importGender == DAZImport.Gender.Male);
					dazbone.exclude = (this.skinPhysicsType != DAZSkinV2.PhysicsType.None);
				}
				else
				{
					this.SetPositionRotation(node, sn, gameObject.transform);
				}
			}
			return result;
		}

		// Token: 0x06004EA4 RID: 20132 RVA: 0x001BBF70 File Offset: 0x001BA370
		private void ProcessNodeTransform(JSONNode sn, bool isRoot)
		{
			string url = DAZImport.DAZurlFix(sn["url"]);
			string text = sn["id"];
			JSONNode node = this.GetNode(url);
			string text2 = node["id"];
			Transform transform;
			if (sn["parent"] != null)
			{
				string text3 = DAZImport.DAZurlToId(sn["parent"]);
				string text4;
				if (this.graftIDToMainID != null && this.graftIDToMainID.TryGetValue(text3, out text4))
				{
					text3 = text4;
				}
				if (!this.sceneNodeIDToTransform.TryGetValue(text3, out transform))
				{
					UnityEngine.Debug.LogWarning("Could not find parent transform " + text3);
					transform = this.container;
				}
			}
			else
			{
				transform = this.container;
			}
			string b = node["name"];
			if (isRoot)
			{
				if (this.useSceneLabelsInsteadOfIds)
				{
					b = sn["label"];
				}
				else
				{
					b = text;
				}
			}
			GameObject gameObject = null;
			IEnumerator enumerator = transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform2 = (Transform)obj;
					if (transform2.name == b)
					{
						gameObject = transform2.gameObject;
						break;
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
			if (gameObject == null)
			{
				return;
			}
			Vector3 zero = Vector3.zero;
			JSONNode jsonnode = sn["translation"];
			if (jsonnode == null)
			{
				jsonnode = node["translation"];
			}
			if (jsonnode != null)
			{
				IEnumerator enumerator2 = jsonnode.AsArray.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						JSONNode jsonnode2 = (JSONNode)obj2;
						string text5 = jsonnode2["id"];
						if (text5 != null)
						{
							if (!(text5 == "x"))
							{
								if (!(text5 == "y"))
								{
									if (text5 == "z")
									{
										zero.z = jsonnode2["current_value"].AsFloat * 0.01f;
									}
								}
								else
								{
									zero.y = jsonnode2["current_value"].AsFloat * 0.01f;
								}
							}
							else
							{
								zero.x = jsonnode2["current_value"].AsFloat * -0.01f;
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
			string text6 = sn["rotation_order"];
			if (text6 == null)
			{
				text6 = node["rotation_order"];
			}
			Quaternion2Angles.RotationOrder rotationOrder = Quaternion2Angles.RotationOrder.XYZ;
			if (text6 != null)
			{
				if (text6 != null)
				{
					if (text6 == "XYZ")
					{
						rotationOrder = Quaternion2Angles.RotationOrder.ZYX;
						goto IL_3AC;
					}
					if (text6 == "XZY")
					{
						rotationOrder = Quaternion2Angles.RotationOrder.YZX;
						goto IL_3AC;
					}
					if (text6 == "YXZ")
					{
						rotationOrder = Quaternion2Angles.RotationOrder.ZXY;
						goto IL_3AC;
					}
					if (text6 == "YZX")
					{
						rotationOrder = Quaternion2Angles.RotationOrder.XZY;
						goto IL_3AC;
					}
					if (text6 == "ZXY")
					{
						rotationOrder = Quaternion2Angles.RotationOrder.YXZ;
						goto IL_3AC;
					}
					if (text6 == "ZYX")
					{
						rotationOrder = Quaternion2Angles.RotationOrder.XYZ;
						goto IL_3AC;
					}
				}
				UnityEngine.Debug.LogError("Bad rotation order in json: " + text6);
				rotationOrder = Quaternion2Angles.RotationOrder.XYZ;
			}
			IL_3AC:
			Vector3 zero2 = Vector3.zero;
			JSONNode jsonnode3 = sn["rotation"];
			if (jsonnode3 == null)
			{
				jsonnode3 = node["rotation"];
			}
			if (jsonnode3 != null)
			{
				IEnumerator enumerator3 = jsonnode3.AsArray.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object obj3 = enumerator3.Current;
						JSONNode jsonnode4 = (JSONNode)obj3;
						string text7 = jsonnode4["id"];
						if (text7 != null)
						{
							if (!(text7 == "x"))
							{
								if (!(text7 == "y"))
								{
									if (text7 == "z")
									{
										zero2.z = -jsonnode4["current_value"].AsFloat;
									}
								}
								else
								{
									zero2.y = -jsonnode4["current_value"].AsFloat;
								}
							}
							else
							{
								zero2.x = jsonnode4["current_value"].AsFloat;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable3;
					if ((disposable3 = (enumerator3 as IDisposable)) != null)
					{
						disposable3.Dispose();
					}
				}
			}
			Quaternion quaternion = Quaternion.identity;
			switch (rotationOrder)
			{
			case Quaternion2Angles.RotationOrder.XYZ:
				quaternion *= Quaternion.Euler(zero2.x, 0f, 0f);
				quaternion *= Quaternion.Euler(0f, zero2.y, 0f);
				quaternion *= Quaternion.Euler(0f, 0f, zero2.z);
				break;
			case Quaternion2Angles.RotationOrder.XZY:
				quaternion *= Quaternion.Euler(zero2.x, 0f, 0f);
				quaternion *= Quaternion.Euler(0f, 0f, zero2.z);
				quaternion *= Quaternion.Euler(0f, zero2.y, 0f);
				break;
			case Quaternion2Angles.RotationOrder.YXZ:
				quaternion *= Quaternion.Euler(0f, zero2.y, 0f);
				quaternion *= Quaternion.Euler(zero2.x, 0f, 0f);
				quaternion *= Quaternion.Euler(0f, 0f, zero2.z);
				break;
			case Quaternion2Angles.RotationOrder.YZX:
				quaternion *= Quaternion.Euler(0f, zero2.y, 0f);
				quaternion *= Quaternion.Euler(0f, 0f, zero2.z);
				quaternion *= Quaternion.Euler(zero2.x, 0f, 0f);
				break;
			case Quaternion2Angles.RotationOrder.ZXY:
				quaternion *= Quaternion.Euler(0f, 0f, zero2.z);
				quaternion *= Quaternion.Euler(zero2.x, 0f, 0f);
				quaternion *= Quaternion.Euler(0f, zero2.y, 0f);
				break;
			case Quaternion2Angles.RotationOrder.ZYX:
				quaternion *= Quaternion.Euler(0f, 0f, zero2.z);
				quaternion *= Quaternion.Euler(0f, zero2.y, 0f);
				quaternion *= Quaternion.Euler(zero2.x, 0f, 0f);
				break;
			}
			if (isRoot)
			{
				gameObject.transform.localPosition += zero;
				gameObject.transform.localRotation *= quaternion;
				if (!this.nestObjects)
				{
					gameObject.transform.parent = this.container;
				}
			}
			else if (this.shouldCreateSkins)
			{
				DAZBone component = gameObject.GetComponent<DAZBone>();
				if (component != null)
				{
					component.presetLocalTranslation = zero;
					component.presetLocalRotation = quaternion.eulerAngles;
				}
			}
			else
			{
				gameObject.transform.localPosition += zero;
				gameObject.transform.localRotation *= quaternion;
			}
		}

		// Token: 0x06004EA5 RID: 20133 RVA: 0x001BC7C8 File Offset: 0x001BABC8
		public DAZMorph ProcessMorph(string modurl, string sceneId = null)
		{
			JSONNode modifier = this.GetModifier(modurl);
			if (modifier != null)
			{
				string text = DAZImport.DAZurlToId(modifier["parent"]);
				if (modifier["formulas"].Count > 0)
				{
					IEnumerator enumerator = modifier["formulas"].AsArray.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							JSONNode jsonnode = (JSONNode)obj;
							string input = jsonnode["output"];
							string a = Regex.Replace(input, "^.*\\?", string.Empty);
							if (a == "value")
							{
								string text2 = Regex.Replace(input, "^.*:", string.Empty);
								text2 = Regex.Replace(text2, "\\?.*", string.Empty);
								if (Regex.IsMatch(text2, "^/"))
								{
									this.ProcessMorph(text2, sceneId);
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
				DAZMorph dazmorph = new DAZMorph();
				dazmorph.Import(modifier);
				DAZMesh dazmesh;
				if (sceneId != null)
				{
					dazmesh = this.GetDAZMeshBySceneGeometryId(sceneId);
					if (dazmesh == null)
					{
						dazmesh = this.GetDAZMeshBySceneNodeId(sceneId);
					}
				}
				else
				{
					dazmesh = this.GetDAZMeshByGeometryId(text);
					if (dazmesh == null)
					{
						dazmesh = this.GetDAZMeshByNodeId(text);
					}
				}
				if (dazmesh != null)
				{
					if (dazmesh.morphBank == null && this.createMorphBank)
					{
						DAZMorphBank dazmorphBank = dazmesh.gameObject.AddComponent<DAZMorphBank>();
						dazmorphBank.connectedMesh = dazmesh;
						dazmesh.morphBank = dazmorphBank;
					}
					if (dazmesh.morphBank != null)
					{
						dazmesh.morphBank.AddMorphUsingSubBanks(dazmorph);
					}
				}
				else if (sceneId != null)
				{
					UnityEngine.Debug.LogWarning("Could not find scene id " + sceneId + " when processing morph " + dazmorph.morphName);
				}
				else
				{
					UnityEngine.Debug.LogWarning("Could not find base id " + text + " when processing morph " + dazmorph.morphName);
				}
				return dazmorph;
			}
			UnityEngine.Debug.LogError("Could not process morph " + modurl);
			return null;
		}

		// Token: 0x06004EA6 RID: 20134 RVA: 0x001BCA08 File Offset: 0x001BAE08
		private DAZSkinV2 ProcessSkin(JSONNode sn, GameObject skinContainer)
		{
			string text = sn["url"];
			string skinId = sn["id"];
			JSONNode modifier = this.GetModifier(text);
			DAZSkinV2 orCreateDAZSkin = this.GetOrCreateDAZSkin(skinId, text, skinContainer);
			if (orCreateDAZSkin != null)
			{
				string text2 = DAZImport.DAZurlToId(sn["parent"]);
				orCreateDAZSkin.sceneGeometryId = text2;
				string text3;
				if (this.sceneGeometryIDToSceneNodeID.TryGetValue(text2, out text3))
				{
					string text4;
					if (this.graftIDToMainID.TryGetValue(text3, out text4))
					{
						text3 = text4;
					}
					Transform transform;
					if (this.sceneNodeIDToTransform.TryGetValue(text3, out transform))
					{
						DAZBones component = transform.GetComponent<DAZBones>();
						if (component != null)
						{
							orCreateDAZSkin.root = component;
						}
						orCreateDAZSkin.Import(modifier);
					}
					else
					{
						UnityEngine.Debug.LogError("Could not find root bone " + text3 + " during ProcessSkin for geometry " + text2);
					}
				}
			}
			return orCreateDAZSkin;
		}

		// Token: 0x06004EA7 RID: 20135 RVA: 0x001BCAF8 File Offset: 0x001BAEF8
		private bool ProcessMaterial(JSONNode sm)
		{
			if (this.createMaterials && this.MaterialFolder != null && this.MaterialFolder != string.Empty)
			{
				string text = sm["id"];
				JSONNode material = this.GetMaterial(sm["url"]);
				string text2 = sm["groups"][0];
				if (text == null || !(material != null))
				{
					return false;
				}
				Material material2 = null;
				string text3 = this.MaterialFolder + "/" + text + ".mat";
				if (material2 == null)
				{
					DAZImportMaterial dazimportMaterial = new DAZImportMaterial();
					dazimportMaterial.subsurfaceColor = this.subdermisColor;
					if (this.forceDiffuseColor)
					{
						dazimportMaterial.ignoreDiffuseColor = true;
						dazimportMaterial.diffuseColor = this.diffuseColor;
					}
					if (this.forceSpecularColor)
					{
						dazimportMaterial.ignoreSpecularColor = true;
						dazimportMaterial.specularColor = this.specularColor;
					}
					dazimportMaterial.bumpiness = this.bumpiness;
					dazimportMaterial.name = text;
					dazimportMaterial.useSpecularAsGlossMap = this.useSpecularAsGlossMap;
					dazimportMaterial.copyBumpAsSpecularColorMap = this.copyBumpAsSpecularColorMap;
					dazimportMaterial.forceBumpAsNormalMap = this.forceBumpAsNormalMap;
					dazimportMaterial.ProcessJSON(material);
					dazimportMaterial.ProcessJSON(sm);
					dazimportMaterial.Report();
					dazimportMaterial.ImportImages(this, this.MaterialFolder);
					dazimportMaterial.standardShader = this.standardShader;
					dazimportMaterial.glossShader = this.glossShader;
					dazimportMaterial.normalMapShader = this.normalMapShader;
					dazimportMaterial.transparentShader = this.transparentShader;
					dazimportMaterial.reflTransparentShader = this.reflTransparentShader;
					dazimportMaterial.transparentNormalMapShader = this.transparentNormalMapShader;
					material2 = dazimportMaterial.CreateMaterialTypeMVR();
					base.RegisterAllocatedObject(material2);
				}
				if (material2 != null && text2 != null && !this.DAZ_material_map.ContainsKey(text2))
				{
					this.DAZ_material_map.Add(text2, material2);
				}
			}
			return true;
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x001BCCF1 File Offset: 0x001BB0F1
		public void ClearPrescan()
		{
			this.sceneNodeIds = null;
			this.sceneNodeLabels = null;
			this.sceneNodeIdImport = null;
			this.sceneNodeIdControllable = null;
			this.sceneNodeIdIsPhysicsObj = null;
			this.sceneNodeIdFloorLock = null;
		}

		// Token: 0x06004EA9 RID: 20137 RVA: 0x001BCD20 File Offset: 0x001BB120
		public void PrescanDuf()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			if (this.DAZSceneDufFile != null && this.DAZSceneDufFile != string.Empty)
			{
				JSONNode jsonnode = DAZImport.ReadJSON(this.DAZSceneDufFile);
				JSONNode jsonnode2 = jsonnode["scene"]["nodes"];
				IEnumerator enumerator = jsonnode2.AsArray.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						JSONNode jsonnode3 = (JSONNode)obj;
						if (jsonnode3["geometries"] != null)
						{
							list.Add(jsonnode3["id"]);
							list2.Add(jsonnode3["label"]);
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
			this.sceneNodeIds = list.ToArray();
			this.sceneNodeLabels = list2.ToArray();
			if (this.sceneNodeIdImport == null || this.sceneNodeIdImport.Length != this.sceneNodeIds.Length)
			{
				this.sceneNodeIdImport = new bool[this.sceneNodeIds.Length];
				for (int i = 0; i < this.sceneNodeIds.Length; i++)
				{
					this.sceneNodeIdImport[i] = true;
				}
			}
			if (this.sceneNodeIdControllable == null || this.sceneNodeIdControllable.Length != this.sceneNodeIds.Length)
			{
				this.sceneNodeIdControllable = new bool[this.sceneNodeIds.Length];
				for (int j = 0; j < this.sceneNodeIds.Length; j++)
				{
					this.sceneNodeIdControllable[j] = true;
				}
			}
			if (this.sceneNodeIdIsPhysicsObj == null || this.sceneNodeIdIsPhysicsObj.Length != this.sceneNodeIds.Length)
			{
				this.sceneNodeIdIsPhysicsObj = new bool[this.sceneNodeIds.Length];
			}
			if (this.sceneNodeIdFloorLock == null || this.sceneNodeIdFloorLock.Length != this.sceneNodeIds.Length)
			{
				this.sceneNodeIdFloorLock = new bool[this.sceneNodeIds.Length];
				for (int k = 0; k < this.sceneNodeIds.Length; k++)
				{
					this.sceneNodeIdFloorLock[k] = true;
				}
			}
		}

		// Token: 0x06004EAA RID: 20138 RVA: 0x001BCF74 File Offset: 0x001BB374
		public void SetRegistryLibPaths()
		{
			this.registryDAZLibraryDirectories = new List<string>();
			int num = (int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\DAZ\\Studio4", "NumContentDirs", 0);
			for (int i = 0; i < num; i++)
			{
				string text = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\DAZ\\Studio4", "ContentDir" + i.ToString(), null);
				if (text != null && text != string.Empty)
				{
					this.registryDAZLibraryDirectories.Add(text);
				}
			}
		}

		// Token: 0x06004EAB RID: 20139 RVA: 0x001BD004 File Offset: 0x001BB404
		protected string GetMaterialSignature(Material m)
		{
			string text = string.Empty;
			if (m.HasProperty("_Color"))
			{
				text = text + ":C:" + m.GetColor("_Color").ToString();
			}
			if (m.HasProperty("_SpecColor"))
			{
				text = text + ":SC:" + m.GetColor("_SpecColor").ToString();
			}
			if (m.HasProperty("_SubdermisColor"))
			{
				text = text + ":SDC:" + m.GetColor("_SubdermisColor").ToString();
			}
			if (m.HasProperty("_AlphaAdjust"))
			{
				text = text + ":AA:" + m.GetFloat("_AlphaAdjust").ToString("F3");
			}
			if (m.HasProperty("_DiffOffset"))
			{
				text = text + ":DO:" + m.GetFloat("_DiffOffset").ToString("F3");
			}
			if (m.HasProperty("_SpecOffset"))
			{
				text = text + ":SO:" + m.GetFloat("_SpecOffset").ToString("F3");
			}
			if (m.HasProperty("_GlossOffset"))
			{
				text = text + ":GO:" + m.GetFloat("_GlossOffset").ToString("F3");
			}
			if (m.HasProperty("_SpecInt"))
			{
				text = text + ":SI:" + m.GetFloat("_SpecInt").ToString("F3");
			}
			if (m.HasProperty("_Shininess"))
			{
				text = text + ":SH:" + m.GetFloat("_Shininess").ToString("F3");
			}
			if (m.HasProperty("_Fresnel"))
			{
				text = text + ":SF:" + m.GetFloat("_Fresnel").ToString("F3");
			}
			if (m.HasProperty("_IBLFilter"))
			{
				text = text + ":IF:" + m.GetFloat("_IBLFilter").ToString("F3");
			}
			if (m.HasProperty("_DiffuseBumpiness"))
			{
				text = text + ":DB:" + m.GetFloat("_DiffuseBumpiness").ToString("F3");
			}
			if (m.HasProperty("_SpecularBumpiness"))
			{
				text = text + ":SB:" + m.GetFloat("_SpecularBumpiness").ToString("F3");
			}
			if (m.HasProperty("_MainTex"))
			{
				Texture texture = m.GetTexture("_MainTex");
				if (texture != null)
				{
					text = text + ":MT:" + texture.name;
				}
			}
			if (m.HasProperty("_SpecTex"))
			{
				Texture texture2 = m.GetTexture("_SpecTex");
				if (texture2 != null)
				{
					text = text + ":ST:" + texture2.name;
				}
			}
			if (m.HasProperty("_GlossTex"))
			{
				Texture texture3 = m.GetTexture("_GlossTex");
				if (texture3 != null)
				{
					text = text + ":GT:" + texture3.name;
				}
			}
			if (m.HasProperty("_AlphaTex"))
			{
				Texture texture4 = m.GetTexture("_AlphaTex");
				if (texture4 != null)
				{
					text = text + ":AT:" + texture4.name;
				}
			}
			if (m.HasProperty("_BumpMap"))
			{
				Texture texture5 = m.GetTexture("_BumpMap");
				if (texture5 != null)
				{
					text = text + ":BM:" + texture5.name;
				}
			}
			if (m.HasProperty("_DetailMap"))
			{
				Texture texture6 = m.GetTexture("_DetailMap");
				if (texture6 != null)
				{
					text = text + ":DM:" + texture6.name;
				}
			}
			if (m.HasProperty("_DecalTex"))
			{
				Texture texture7 = m.GetTexture("_DecalTex");
				if (texture7 != null)
				{
					text = text + ":DT:" + texture7.name;
				}
			}
			return text;
		}

		// Token: 0x06004EAC RID: 20140 RVA: 0x001BD474 File Offset: 0x001BB874
		protected bool CompareMaterials(Material m1, Material m2)
		{
			if (m1.HasProperty("_Color"))
			{
				if (!m2.HasProperty("_Color"))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004EAD RID: 20141 RVA: 0x001BD4A0 File Offset: 0x001BB8A0
		public void CreateMaterialOptionsUI(MaterialOptions mo, string tabName)
		{
			if (this.materialUIConnector != null)
			{
				this.materialUIConnector.AddConnector(mo);
			}
			if (this.materialUIConnectorMaster != null && this.addMaterialTabs)
			{
				TabbedUIBuilder.Tab tab = new TabbedUIBuilder.Tab();
				tab.name = tabName;
				tab.prefab = this.materialUITab;
				tab.color = this.materialUITabColor;
				this.materialUIConnectorMaster.AddTab(tab, this.materialUITabAddBeforeTabName);
			}
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x001BD520 File Offset: 0x001BB920
		public void CreateMaterialOptions(GameObject go, int numMats, string[] materialNames, Material[] materials, Type type, string customTextureFolder, bool createWithExclude = false)
		{
			MaterialOptions[] components = go.GetComponents<MaterialOptions>();
			int num = components.Length;
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			int num2;
			if (this.combineToSingleMaterial)
			{
				num2 = 1;
				List<int> list = new List<int>();
				for (int i = 0; i < materials.Length; i++)
				{
					list.Add(i);
				}
				dictionary.Add(0, list);
			}
			else if (this.combineMaterials)
			{
				Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
				int num3 = 0;
				for (int j = 0; j < materials.Length; j++)
				{
					string materialSignature = this.GetMaterialSignature(materials[j]);
					int key;
					if (dictionary2.TryGetValue(materialSignature, out key))
					{
						List<int> list2;
						if (dictionary.TryGetValue(key, out list2))
						{
							list2.Add(j);
						}
					}
					else
					{
						dictionary2.Add(materialSignature, num3);
						dictionary.Add(num3, new List<int>
						{
							j
						});
						num3++;
					}
				}
				num2 = num3;
			}
			else
			{
				for (int k = 0; k < materials.Length; k++)
				{
					dictionary.Add(k, new List<int>
					{
						k
					});
				}
				num2 = numMats;
			}
			if (num != num2)
			{
				int num4 = num2 - num;
				if (num4 > 0)
				{
					for (int l = 0; l < num4; l++)
					{
						go.AddComponent(type);
					}
				}
				components = go.GetComponents<MaterialOptions>();
				UIMultiConnector uimultiConnector = null;
				UIConnectorMaster uiconnectorMaster = null;
				ConfigurableJoint component = go.GetComponent<ConfigurableJoint>();
				if (component != null && component.connectedBody != null)
				{
					FreeControllerV3 component2 = component.connectedBody.GetComponent<FreeControllerV3>();
					if (component2 != null && component2.UITransforms != null && component2.UITransforms.Length > 0)
					{
						Transform transform = component2.UITransforms[0];
						if (transform != null)
						{
							uimultiConnector = transform.GetComponent<UIMultiConnector>();
							uiconnectorMaster = transform.GetComponent<UIConnectorMaster>();
						}
					}
				}
				if (uimultiConnector == null)
				{
					uimultiConnector = this.materialUIConnector;
				}
				if (uiconnectorMaster == null)
				{
					uiconnectorMaster = this.materialUIConnectorMaster;
				}
				for (int m = 0; m < components.Length; m++)
				{
					string text;
					if (this.combineMaterials || this.combineToSingleMaterial)
					{
						if (components.Length == 1)
						{
							text = "Combined";
						}
						else
						{
							text = "Combined" + (m + 1).ToString();
						}
					}
					else
					{
						text = materialNames[m];
					}
					components[m].deregisterOnDisable = this.materialOptionsDeregisterOnDisable;
					if (this.connectMaterialUI)
					{
						if (uimultiConnector != null)
						{
							uimultiConnector.AddConnector(components[m]);
						}
						if (uiconnectorMaster != null && this.addMaterialTabs)
						{
							uiconnectorMaster.AddTab(new TabbedUIBuilder.Tab
							{
								name = text,
								prefab = this.materialUITab,
								color = this.materialUITabColor
							}, this.materialUITabAddBeforeTabName);
						}
					}
					components[m].exclude = createWithExclude;
					if (this.addParentNameToMaterialOptions)
					{
						components[m].overrideId = "+parent+Material" + text;
					}
					else
					{
						components[m].overrideId = "+Material" + text;
					}
					List<int> list3;
					if (dictionary.TryGetValue(m, out list3))
					{
						components[m].paramMaterialSlots = list3.ToArray();
						components[m].materialForDefaults = materials[list3[0]];
						if (components[m].textureGroup1 == null)
						{
							components[m].textureGroup1 = new MaterialOptionTextureGroup();
						}
						components[m].textureGroup1.materialSlots = list3.ToArray();
						if (Application.isPlaying)
						{
							components[m].customTextureFolder = customTextureFolder;
						}
						components[m].SetStartingValues(this.textureToSourcePath);
					}
				}
			}
		}

		// Token: 0x06004EAF RID: 20143 RVA: 0x001BD904 File Offset: 0x001BBD04
		public void ClearMaterialConnectors()
		{
			if (this.connectMaterialUI)
			{
				if (this.materialUIConnector != null)
				{
					this.materialUIConnector.ClearConnectors();
				}
				if (this.materialUIConnectorMaster != null)
				{
					this.materialUIConnectorMaster.ClearRuntimeTabs(false);
				}
			}
		}

		// Token: 0x06004EB0 RID: 20144 RVA: 0x001BD958 File Offset: 0x001BBD58
		public IEnumerator ImportDufMorphsCo(DAZImport.ImportCallback callback = null)
		{
			this.isImporting = true;
			this.importStatus = "Initialization";
			yield return null;
			this.SetRegistryLibPaths();
			this.DAZ_modifier_library = new Dictionary<string, JSONNode>();
			this.DAZ_geometry_library = new Dictionary<string, JSONNode>();
			this.DAZ_node_library = new Dictionary<string, JSONNode>();
			this.DAZ_uv_library = new Dictionary<string, JSONNode>();
			if (this.DAZSceneDufFile != null && this.DAZSceneDufFile != string.Empty)
			{
				JSONNode djn = null;
				string dufname = Regex.Replace(this.DAZSceneDufFile, ".*/", string.Empty);
				dufname = Regex.Replace(dufname, "\\.duf", string.Empty);
				try
				{
					djn = DAZImport.ReadJSON(this.DAZSceneDufFile);
				}
				catch (Exception ex)
				{
					SuperController.LogError("Exception during DAZ import: " + ex.Message);
					this.importStatus = "Import failed " + ex.Message;
					this.isImporting = false;
					if (callback != null)
					{
						callback();
					}
					yield break;
				}
				yield return null;
				JSONNode JSONSceneModifiers = djn["scene"]["modifiers"];
				if (JSONSceneModifiers != null)
				{
					IEnumerator enumerator = JSONSceneModifiers.AsArray.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							JSONNode jsonnode = (JSONNode)obj;
							string a = jsonnode["channel"]["type"];
							if (a == "float")
							{
								string sceneId = DAZImport.DAZurlToId(jsonnode["parent"]);
								DAZMorph dazmorph = this.ProcessMorph(jsonnode["url"], sceneId);
								if (dazmorph != null && !dazmorph.preserveValueOnReimport)
								{
									float num = jsonnode["channel"]["current_value"].AsFloat;
									if (num <= 0.001f && num >= -0.001f)
									{
										num = 0f;
									}
									dazmorph.importValue = num;
									dazmorph.morphValue = num;
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
			}
			yield break;
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x001BD97C File Offset: 0x001BBD7C
		public IEnumerator ImportDufCo(DAZImport.ImportCallback callback = null)
		{
			this.isImporting = true;
			this.importStatus = "Initialization";
			yield return null;
			this.SetRegistryLibPaths();
			Dictionary<string, bool> sceneNodeIdToImport = new Dictionary<string, bool>();
			Dictionary<string, bool> sceneNodeIdToControllable = new Dictionary<string, bool>();
			Dictionary<string, bool> sceneNodeIdToPhysicsEnabled = new Dictionary<string, bool>();
			Dictionary<string, bool> sceneNodeIdToFloorLock = new Dictionary<string, bool>();
			if (this.sceneNodeIds != null)
			{
				for (int i = 0; i < this.sceneNodeIds.Length; i++)
				{
					string key = this.sceneNodeIds[i];
					sceneNodeIdToImport.Add(key, this.sceneNodeIdImport[i]);
					sceneNodeIdToControllable.Add(key, this.sceneNodeIdControllable[i]);
					sceneNodeIdToPhysicsEnabled.Add(key, this.sceneNodeIdIsPhysicsObj[i]);
					sceneNodeIdToFloorLock.Add(key, this.sceneNodeIdFloorLock[i]);
				}
			}
			if (this.container == null)
			{
				this.container = base.transform;
			}
			this.DAZ_modifier_library = new Dictionary<string, JSONNode>();
			this.DAZ_geometry_library = new Dictionary<string, JSONNode>();
			this.DAZ_node_library = new Dictionary<string, JSONNode>();
			this.DAZ_uv_library = new Dictionary<string, JSONNode>();
			this.DAZ_uv_map = new Dictionary<string, DAZUVMap>();
			this.DAZ_material_map = new Dictionary<string, Material>();
			this.sceneNodeIDToTransform = new Dictionary<string, Transform>();
			this.graftIDToMainID = new Dictionary<string, string>();
			if (this.DAZSceneDufFile != null && this.DAZSceneDufFile != string.Empty)
			{
				JSONNode djn = null;
				string dufname = Regex.Replace(this.DAZSceneDufFile, ".*/", string.Empty);
				dufname = Regex.Replace(dufname, "\\.duf", string.Empty);
				try
				{
					djn = DAZImport.ReadJSON(this.DAZSceneDufFile);
					string str;
					if (Application.isPlaying)
					{
						if (this.importType == DAZImport.ImportType.Character || this.importType == DAZImport.ImportType.Clothing || this.importType == DAZImport.ImportType.Hair || this.importType == DAZImport.ImportType.HairScalp)
						{
							str = this.RuntimeMaterialCreationDirectory + "/" + this.importGender.ToString();
						}
						else
						{
							str = this.RuntimeMaterialCreationDirectory;
						}
					}
					else
					{
						str = this.MaterialCreationDirectory;
					}
					if (this.overrideMaterialFolderName)
					{
						this.MaterialFolder = str + "/" + this.MaterialOverrideFolderName;
					}
					else
					{
						this.MaterialFolder = str + "/" + dufname;
					}
					if (Application.isPlaying)
					{
						if (ImageLoaderThreaded.singleton != null)
						{
							ImageLoaderThreaded.singleton.PurgeAllImmediateTextures();
						}
						Directory.CreateDirectory(this.MaterialFolder);
					}
					this.ClearMaterialConnectors();
					this.importStatus = "Reading libraries";
					this.ProcessDAZLibraries(string.Empty, djn);
				}
				catch (Exception ex)
				{
					SuperController.LogError("Exception during DAZ import: " + ex.Message);
					this.importStatus = "Import failed " + ex.Message;
					this.isImporting = false;
					if (callback != null)
					{
						callback();
					}
					yield break;
				}
				yield return null;
				JSONNode JSONSceneMaterials = null;
				Dictionary<string, List<DAZUVMap>> materialMap = new Dictionary<string, List<DAZUVMap>>();
				try
				{
					JSONSceneMaterials = djn["scene"]["materials"];
					IEnumerator enumerator = JSONSceneMaterials.AsArray.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							JSONNode jsonnode = (JSONNode)obj;
							string key2 = DAZImport.DAZurlToId(jsonnode["geometry"]);
							string uvurl = jsonnode["uv_set"];
							DAZUVMap dazuvmap = this.ProcessUV(uvurl);
							if (dazuvmap != null)
							{
								List<DAZUVMap> list;
								if (!materialMap.TryGetValue(key2, out list))
								{
									list = new List<DAZUVMap>();
									materialMap.Add(key2, list);
								}
								list.Add(dazuvmap);
							}
							else
							{
								this.importStatus = "Error during process of UV maps";
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
				catch (Exception ex2)
				{
					SuperController.LogError("Exception during DAZ import: " + ex2.Message);
					this.importStatus = "Import failed " + ex2.Message;
					this.isImporting = false;
					if (callback != null)
					{
						callback();
					}
					yield break;
				}
				int numMaterials = JSONSceneMaterials.Count;
				int ind = 1;
				IEnumerator enumerator2 = JSONSceneMaterials.AsArray.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						JSONNode sm = (JSONNode)obj2;
						try
						{
							this.importStatus = string.Concat(new object[]
							{
								"Import material ",
								ind,
								" of ",
								numMaterials
							});
							float num = (float)ind * 1f / (float)numMaterials;
							if (!this.ProcessMaterial(sm))
							{
								this.importStatus = "Error during process of material";
							}
						}
						catch (Exception ex3)
						{
							SuperController.LogError("Exception during DAZ import: " + ex3);
							this.importStatus = "Import failed " + ex3.Message;
							this.isImporting = false;
							if (callback != null)
							{
								callback();
							}
							yield break;
						}
						ind++;
						yield return null;
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
				try
				{
					JSONNode jsonnode2 = djn["scene"]["modifiers"];
					this.importStatus = "Process geometry";
					JSONNode jsonnode3 = djn["scene"]["nodes"];
					this.sceneGeometryIDToSceneNodeID = new Dictionary<string, string>();
					DAZSkinWrap dazskinWrap = null;
					Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
					Dictionary<string, Transform> dictionary2 = new Dictionary<string, Transform>();
					IEnumerator enumerator3 = jsonnode3.AsArray.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							object obj3 = enumerator3.Current;
							JSONNode jsonnode4 = (JSONNode)obj3;
							string text = jsonnode4["id"];
							string str2;
							if (this.useSceneLabelsInsteadOfIds)
							{
								str2 = jsonnode4["label"];
							}
							else
							{
								str2 = text;
							}
							bool flag = false;
							if (jsonnode4["geometries"] != null)
							{
								flag = true;
							}
							bool flag2 = true;
							if (sceneNodeIdToImport.ContainsKey(text))
							{
								sceneNodeIdToImport.TryGetValue(text, out flag2);
							}
							if (flag && this.skipImportOfExisting && this.NodeExists(jsonnode4, flag))
							{
								flag2 = false;
							}
							if (flag2)
							{
								string nodeId = this.ProcessNodeCreation(jsonnode4, flag);
								bool flag3 = false;
								bool flag4 = false;
								if (flag)
								{
									Transform transform = base.transform;
									Transform transform2;
									if (this.sceneNodeIDToTransform.TryGetValue(text, out transform2))
									{
										if (this.embedMeshAndSkinOnNodes)
										{
											if (jsonnode4["conform_target"] != null)
											{
												string sceneNodeId = DAZImport.DAZurlToId(jsonnode4["conform_target"]);
												DAZMesh dazmeshBySceneNodeId = this.GetDAZMeshBySceneNodeId(sceneNodeId);
												if (dazmeshBySceneNodeId != null)
												{
													transform = dazmeshBySceneNodeId.transform;
												}
												else
												{
													transform = transform2;
												}
											}
											else
											{
												transform = transform2;
											}
										}
										dictionary.Add(text, transform);
										if (this.shouldCreateSkins)
										{
											DAZBones x = transform2.GetComponent<DAZBones>();
											if (x == null)
											{
												x = transform2.gameObject.AddComponent<DAZBones>();
											}
										}
										GameObject gameObject = null;
										bool flag5 = false;
										bool flag6 = false;
										sceneNodeIdToControllable.TryGetValue(text, out flag6);
										if (flag6)
										{
											if (this.controlContainer != null && this.controlPrefab != null)
											{
												string name = str2 + "Control";
												Transform transform3 = this.controlContainer.Find(name);
												if (transform3 != null)
												{
													gameObject = transform3.gameObject;
												}
												else
												{
													if (Application.isPlaying)
													{
														gameObject = UnityEngine.Object.Instantiate<GameObject>(this.controlPrefab.gameObject);
													}
													gameObject.name = name;
													gameObject.transform.parent = this.controlContainer;
													gameObject.transform.position = transform.position;
													gameObject.transform.rotation = transform.rotation;
												}
												dictionary2.Add(text, gameObject.transform);
												Rigidbody component = gameObject.GetComponent<Rigidbody>();
												Rigidbody component2 = transform.GetComponent<Rigidbody>();
												if (component2 != null && sceneNodeIdToPhysicsEnabled.TryGetValue(text, out flag3))
												{
													component2.isKinematic = !flag3;
												}
												ConfigurableJoint component3 = transform.GetComponent<ConfigurableJoint>();
												if (component3 != null && component != null)
												{
													flag5 = true;
													component3.connectedBody = component;
												}
												sceneNodeIdToFloorLock.TryGetValue(text, out flag4);
											}
											if (this.UIContainer != null && this.UIPrefab != null)
											{
												GameObject gameObject2 = null;
												string name2 = str2 + "UI";
												Transform transform4 = this.UIContainer.Find(name2);
												if (transform4 != null)
												{
													gameObject2 = transform4.gameObject;
												}
												else
												{
													if (Application.isPlaying)
													{
														if (!flag3 && this.UIPrefabNoPhysics != null)
														{
															gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.UIPrefabNoPhysics.gameObject);
														}
														else
														{
															gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.UIPrefab.gameObject);
														}
													}
													gameObject2.name = name2;
													gameObject2.transform.parent = this.UIContainer;
													if (gameObject != null)
													{
														FreeControllerV3 component4 = gameObject.GetComponent<FreeControllerV3>();
														if (component4 != null)
														{
															if (flag5)
															{
																component4.followWhenOff = transform;
															}
															if (flag3)
															{
																component4.startingPositionState = FreeControllerV3.PositionState.Off;
																component4.startingRotationState = FreeControllerV3.RotationState.Off;
															}
															if (flag4)
															{
																component4.yLock = true;
																component4.xRotLock = true;
																component4.zRotLock = true;
															}
															component4.UITransforms = new Transform[1];
															component4.UITransforms[0] = gameObject2.transform;
															UIConnector uiconnector = gameObject2.AddComponent<UIConnector>();
															uiconnector.receiverTransform = component4.transform;
															uiconnector.receiver = component4;
															uiconnector.storeid = component4.storeId;
														}
														MotionAnimationControl component5 = gameObject.GetComponent<MotionAnimationControl>();
														if (component5 != null)
														{
															UIConnector uiconnector2 = gameObject2.AddComponent<UIConnector>();
															uiconnector2.receiverTransform = component5.transform;
															uiconnector2.receiver = component5;
															uiconnector2.storeid = component5.storeId;
														}
														SetTransformScale component6 = transform.GetComponent<SetTransformScale>();
														if (component6 != null)
														{
															UIConnector uiconnector3 = gameObject2.AddComponent<UIConnector>();
															uiconnector3.receiverTransform = component6.transform;
															uiconnector3.receiver = component6;
															uiconnector3.storeid = component6.storeId;
														}
														PhysicsMaterialControl component7 = transform.GetComponent<PhysicsMaterialControl>();
														if (component7 != null)
														{
															UIConnector uiconnector4 = gameObject2.AddComponent<UIConnector>();
															uiconnector4.receiverTransform = component7.transform;
															uiconnector4.receiver = component7;
															uiconnector4.storeid = component7.storeId;
														}
													}
												}
											}
										}
										else
										{
											ConfigurableJoint component8 = transform.GetComponent<ConfigurableJoint>();
											if (component8 != null)
											{
												UnityEngine.Object.DestroyImmediate(component8);
											}
											Rigidbody component9 = transform.GetComponent<Rigidbody>();
											if (component9 != null)
											{
												UnityEngine.Object.DestroyImmediate(component9);
											}
											ForceReceiver component10 = transform.GetComponent<ForceReceiver>();
											if (component10 != null)
											{
												UnityEngine.Object.DestroyImmediate(component10);
											}
											PhysicsMaterialControl component11 = transform.GetComponent<PhysicsMaterialControl>();
											if (component11 != null)
											{
												UnityEngine.Object.DestroyImmediate(component11);
											}
										}
									}
									DAZMesh dazmesh = null;
									IEnumerator enumerator4 = jsonnode4["geometries"].AsArray.GetEnumerator();
									try
									{
										while (enumerator4.MoveNext())
										{
											object obj4 = enumerator4.Current;
											JSONNode jsonnode5 = (JSONNode)obj4;
											string text2 = jsonnode5["url"];
											if (text2 != null)
											{
												string text3 = jsonnode5["id"];
												this.sceneGeometryIDToSceneNodeID.Add(text3, text);
												List<DAZUVMap> list2;
												if (materialMap.TryGetValue(text3, out list2))
												{
													dazmesh = this.ProcessGeometry(text2, text3, list2.ToArray(), text, nodeId, transform.gameObject);
													if (dazmesh != null)
													{
														if (this.shouldCreateSkinWrap)
														{
															if (!Application.isPlaying)
															{
																dazmesh.SaveMeshAsset(false);
															}
															dazmesh.createMeshFilterAndRenderer = true;
															MeshRenderer component12 = dazmesh.GetComponent<MeshRenderer>();
															if (component12 != null)
															{
																component12.enabled = false;
															}
															dazmesh.drawMorphedUVMappedMesh = false;
															dazmesh.enabled = false;
															DAZSkinWrap orCreateDAZSkinWrap = this.GetOrCreateDAZSkinWrap(dazmesh);
															if (orCreateDAZSkinWrap != null)
															{
																dazskinWrap = orCreateDAZSkinWrap;
																dazskinWrap.surfaceOffset = this.skinWrapSurfaceOffset;
																dazskinWrap.additionalThicknessMultiplier = this.skinWrapAdditionalThicknessMultiplier;
																dazskinWrap.smoothOuterLoops = this.skinWrapSmoothOuterLoops;
																orCreateDAZSkinWrap.CopyMaterials();
																if (this.createMaterialOptions)
																{
																	this.CreateMaterialOptions(orCreateDAZSkinWrap.gameObject, orCreateDAZSkinWrap.numMaterials, orCreateDAZSkinWrap.materialNames, orCreateDAZSkinWrap.GPUmaterials, typeof(DAZSkinWrapMaterialOptions), this.MaterialFolder, Application.isPlaying);
																}
															}
														}
														else if (!this.shouldCreateSkins)
														{
															if (this.createMaterialOptions)
															{
																this.CreateMaterialOptions(dazmesh.gameObject, dazmesh.numMaterials, dazmesh.materialNames, dazmesh.materials, typeof(MaterialOptions), this.MaterialFolder, Application.isPlaying);
															}
															if (!Application.isPlaying)
															{
																dazmesh.SaveMeshAsset(false);
															}
															dazmesh.createMeshFilterAndRenderer = true;
															dazmesh.drawMorphedUVMappedMesh = false;
															dazmesh.createMeshCollider = true;
															if (flag3)
															{
																dazmesh.useConvexCollider = true;
															}
															else
															{
																dazmesh.useConvexCollider = false;
															}
															dazmesh.enabled = false;
														}
													}
												}
												else
												{
													UnityEngine.Debug.LogError("Could not find materials for " + text3);
												}
											}
											else
											{
												UnityEngine.Debug.LogError("Could not find geometries url");
											}
											if (dazskinWrap != null && !this.embedMeshAndSkinOnNodes)
											{
												break;
											}
										}
									}
									finally
									{
										IDisposable disposable3;
										if ((disposable3 = (enumerator4 as IDisposable)) != null)
										{
											disposable3.Dispose();
										}
									}
									if (jsonnode4["conform_target"] != null)
									{
										string sceneNodeId2 = DAZImport.DAZurlToId(jsonnode4["conform_target"]);
										DAZMesh dazmeshBySceneNodeId2 = this.GetDAZMeshBySceneNodeId(sceneNodeId2);
										if (dazmeshBySceneNodeId2 != null && dazmesh != null)
										{
											dazmesh.graftTo = dazmeshBySceneNodeId2;
										}
									}
								}
								if (dazskinWrap != null && !this.embedMeshAndSkinOnNodes)
								{
									break;
								}
							}
						}
					}
					finally
					{
						IDisposable disposable4;
						if ((disposable4 = (enumerator3 as IDisposable)) != null)
						{
							disposable4.Dispose();
						}
					}
					if (this.shouldCreateSkinsAndNodes)
					{
						IEnumerator enumerator5 = jsonnode3.AsArray.GetEnumerator();
						try
						{
							while (enumerator5.MoveNext())
							{
								object obj5 = enumerator5.Current;
								JSONNode jsonnode6 = (JSONNode)obj5;
								string key3 = jsonnode6["id"];
								bool flag7 = false;
								if (jsonnode6["geometries"] != null)
								{
									flag7 = true;
								}
								bool flag8 = true;
								if (sceneNodeIdToImport.ContainsKey(key3))
								{
									sceneNodeIdToImport.TryGetValue(key3, out flag8);
								}
								if (flag7 && this.skipImportOfExisting && this.NodeExists(jsonnode6, flag7))
								{
									flag8 = false;
								}
								if (flag8)
								{
									this.ProcessNodeTransform(jsonnode6, flag7);
								}
							}
						}
						finally
						{
							IDisposable disposable5;
							if ((disposable5 = (enumerator5 as IDisposable)) != null)
							{
								disposable5.Dispose();
							}
						}
						IEnumerator enumerator6 = jsonnode3.AsArray.GetEnumerator();
						try
						{
							while (enumerator6.MoveNext())
							{
								object obj6 = enumerator6.Current;
								JSONNode jsonnode7 = (JSONNode)obj6;
								string key4 = jsonnode7["id"];
								Transform transform5;
								Transform transform6;
								if (dictionary2.TryGetValue(key4, out transform5) && dictionary.TryGetValue(key4, out transform6))
								{
									transform5.position = transform6.position;
									transform5.rotation = transform6.rotation;
								}
							}
						}
						finally
						{
							IDisposable disposable6;
							if ((disposable6 = (enumerator6 as IDisposable)) != null)
							{
								disposable6.Dispose();
							}
						}
					}
					if (jsonnode2 != null)
					{
						IEnumerator enumerator7 = jsonnode2.AsArray.GetEnumerator();
						try
						{
							while (enumerator7.MoveNext())
							{
								object obj7 = enumerator7.Current;
								JSONNode jsonnode8 = (JSONNode)obj7;
								string a = jsonnode8["channel"]["type"];
								if (a == "float")
								{
									string sceneId = DAZImport.DAZurlToId(jsonnode8["parent"]);
									DAZMorph dazmorph = this.ProcessMorph(jsonnode8["url"], sceneId);
									if (dazmorph != null && !dazmorph.preserveValueOnReimport)
									{
										float num2 = jsonnode8["channel"]["current_value"].AsFloat;
										if (num2 <= 0.001f && num2 >= -0.001f)
										{
											num2 = 0f;
										}
										dazmorph.importValue = num2;
										dazmorph.morphValue = num2;
									}
								}
							}
						}
						finally
						{
							IDisposable disposable7;
							if ((disposable7 = (enumerator7 as IDisposable)) != null)
							{
								disposable7.Dispose();
							}
						}
					}
					if (this.container != null)
					{
						foreach (DAZBones dazbones in this.container.GetComponentsInChildren<DAZBones>())
						{
							dazbones.Reset();
						}
					}
					foreach (DAZMesh dazmesh2 in base.GetComponents<DAZMesh>())
					{
						dazmesh2.ReInitMorphs();
					}
					if (this.wrapOnImport)
					{
						foreach (DAZSkinWrap dazskinWrap2 in base.GetComponents<DAZSkinWrap>())
						{
							if (dazskinWrap2.wrapStore == null || dazskinWrap2.wrapStore.wrapVertices == null)
							{
								dazskinWrap2.wrapToMorphedVertices = this.wrapToMorphedVertices;
								dazskinWrap2.Wrap();
							}
						}
					}
					DAZSkinWrap component13 = base.GetComponent<DAZSkinWrap>();
					if (component13 != null)
					{
						component13.draw = true;
						if (Application.isPlaying)
						{
							DAZSkinWrapControl component14 = base.GetComponent<DAZSkinWrapControl>();
							if (component14 != null)
							{
								component14.wrap = component13;
							}
						}
					}
					foreach (DAZMergedMesh dazmergedMesh in base.GetComponents<DAZMergedMesh>())
					{
						dazmergedMesh.ManualUpdate();
					}
					if (this.DAZSceneModifierOverrideDufFile != null && this.DAZSceneModifierOverrideDufFile != string.Empty)
					{
						JSONNode jl = DAZImport.ReadJSON(this.DAZSceneModifierOverrideDufFile);
						string url = DAZImport.DAZurlFix(this.modifierOverrideUrl);
						this.ProcessAltModifiers(url, jl);
					}
					if (this.shouldCreateSkinsAndNodes && this.shouldCreateSkins && this.container != null && jsonnode2 != null)
					{
						IEnumerator enumerator8 = jsonnode2.AsArray.GetEnumerator();
						try
						{
							while (enumerator8.MoveNext())
							{
								object obj8 = enumerator8.Current;
								JSONNode jsonnode9 = (JSONNode)obj8;
								string a2 = jsonnode9["extra"][0]["type"];
								if (a2 == "skin_settings")
								{
									string skinId = jsonnode9["id"];
									string skinUrl = jsonnode9["url"];
									string text4 = DAZImport.DAZurlToId(jsonnode9["parent"]);
									Transform transform7 = base.transform;
									if (this.embedMeshAndSkinOnNodes && text4 != null)
									{
										DAZMesh dazmeshBySceneGeometryId = this.GetDAZMeshBySceneGeometryId(text4);
										if (dazmeshBySceneGeometryId != null)
										{
											transform7 = dazmeshBySceneGeometryId.transform;
										}
										else
										{
											UnityEngine.Debug.LogWarning("Could not find DAZMesh with scene geometry ID " + text4);
											transform7 = base.transform;
										}
									}
									DAZSkinV2 orCreateDAZSkin = this.GetOrCreateDAZSkin(skinId, skinUrl, transform7.gameObject);
									orCreateDAZSkin.ImportStart();
									IEnumerator enumerator9 = jsonnode3.AsArray.GetEnumerator();
									try
									{
										while (enumerator9.MoveNext())
										{
											object obj9 = enumerator9.Current;
											JSONNode jsonnode10 = (JSONNode)obj9;
											string url2 = DAZImport.DAZurlFix(jsonnode10["url"]);
											JSONNode node = this.GetNode(url2);
											orCreateDAZSkin.ImportNode(node, url2);
										}
									}
									finally
									{
										IDisposable disposable8;
										if ((disposable8 = (enumerator9 as IDisposable)) != null)
										{
											disposable8.Dispose();
										}
									}
									this.ProcessSkin(jsonnode9, transform7.gameObject);
									if (this.createMaterialOptions)
									{
										this.CreateMaterialOptions(orCreateDAZSkin.gameObject, orCreateDAZSkin.numMaterials, orCreateDAZSkin.materialNames, orCreateDAZSkin.GPUmaterials, typeof(DAZCharacterMaterialOptions), this.MaterialFolder, Application.isPlaying);
									}
								}
							}
						}
						finally
						{
							IDisposable disposable9;
							if ((disposable9 = (enumerator8 as IDisposable)) != null)
							{
								disposable9.Dispose();
							}
						}
						IEnumerator enumerator10 = jsonnode2.AsArray.GetEnumerator();
						try
						{
							while (enumerator10.MoveNext())
							{
								object obj10 = enumerator10.Current;
								JSONNode jsonnode11 = (JSONNode)obj10;
								string a3 = jsonnode11["extra"][0]["type"];
								if (a3 == "skin_settings")
								{
									string skinId2 = jsonnode11["id"];
									string text5 = jsonnode11["url"];
									string text6 = DAZImport.DAZurlToId(jsonnode11["parent"]);
									Transform transform8 = base.transform;
									if (this.embedMeshAndSkinOnNodes && text6 != null)
									{
										DAZMesh dazmeshBySceneGeometryId2 = this.GetDAZMeshBySceneGeometryId(text6);
										if (dazmeshBySceneGeometryId2 != null)
										{
											transform8 = dazmeshBySceneGeometryId2.transform;
										}
										else
										{
											UnityEngine.Debug.LogWarning("Could not find DAZMesh with scene geometry ID " + text6);
											transform8 = base.transform;
										}
									}
									DAZSkinV2 dazskin = this.GetDAZSkin(skinId2, transform8.gameObject);
									if (dazskin.dazMesh.graftTo != null)
									{
										DAZMergedMesh dazmergedMesh2 = transform8.GetComponent<DAZMergedMesh>();
										if (dazmergedMesh2 == null)
										{
											dazmergedMesh2 = transform8.gameObject.AddComponent<DAZMergedMesh>();
										}
										dazmergedMesh2.Merge();
										DAZMergedSkinV2 dazmergedSkinV = transform8.GetComponent<DAZMergedSkinV2>();
										if (dazmergedSkinV == null)
										{
											dazmergedSkinV = transform8.gameObject.AddComponent<DAZMergedSkinV2>();
										}
										dazmergedSkinV.root = dazskin.root;
										dazmergedSkinV.Merge();
										dazmergedSkinV.skin = true;
										dazmergedSkinV.useSmoothing = true;
										dazmergedSkinV.useSmoothVertsForNormalTangentRecalc = true;
										dazmergedSkinV.skinMethod = DAZSkinV2.SkinMethod.CPUAndGPU;
										dazmergedSkinV.CopyMaterials();
									}
								}
							}
						}
						finally
						{
							IDisposable disposable10;
							if ((disposable10 = (enumerator10 as IDisposable)) != null)
							{
								disposable10.Dispose();
							}
						}
					}
					if (this.materialUIConnectorMaster != null)
					{
						this.materialUIConnectorMaster.Rebuild();
					}
				}
				catch (Exception ex4)
				{
					SuperController.LogError("Exception during DAZ import: " + ex4.ToString());
					this.importStatus = "Import failed " + ex4.Message;
					this.isImporting = false;
					if (callback != null)
					{
						callback();
					}
					yield break;
				}
			}
			DAZDynamic dd = base.GetComponent<DAZDynamic>();
			if (dd != null)
			{
				dd.RefreshStorables();
			}
			this.importStatus = "Import complete";
			this.isImporting = false;
			if (callback != null)
			{
				callback();
			}
			yield break;
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x001BD9A0 File Offset: 0x001BBDA0
		public void ImportDufMorphs()
		{
			IEnumerator enumerator = this.ImportDufMorphsCo(null);
			while (enumerator.MoveNext())
			{
			}
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x001BD9C8 File Offset: 0x001BBDC8
		public void ImportDuf()
		{
			IEnumerator enumerator = this.ImportDufCo(null);
			while (enumerator.MoveNext())
			{
			}
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x001BD9ED File Offset: 0x001BBDED
		protected void OnEnable()
		{
			this.SetRegistryLibPaths();
		}

		// Token: 0x04003E47 RID: 15943
		public DAZImport.ImportType importType;

		// Token: 0x04003E48 RID: 15944
		protected const float geoScale = 0.01f;

		// Token: 0x04003E49 RID: 15945
		public string DAZLibraryDirectory = string.Empty;

		// Token: 0x04003E4A RID: 15946
		public string alternateDAZLibraryDirectory = string.Empty;

		// Token: 0x04003E4B RID: 15947
		[NonSerialized]
		public List<string> registryDAZLibraryDirectories;

		// Token: 0x04003E4C RID: 15948
		[NonSerialized]
		public string DAZSceneDufFile;

		// Token: 0x04003E4D RID: 15949
		public string DAZSceneModifierOverrideDufFile;

		// Token: 0x04003E4E RID: 15950
		public string modifierOverrideUrl;

		// Token: 0x04003E4F RID: 15951
		public bool createMaterials = true;

		// Token: 0x04003E50 RID: 15952
		public bool useSpecularAsGlossMap;

		// Token: 0x04003E51 RID: 15953
		public bool copyBumpAsSpecularColorMap;

		// Token: 0x04003E52 RID: 15954
		public bool forceBumpAsNormalMap;

		// Token: 0x04003E53 RID: 15955
		public bool replaceExistingMaterials;

		// Token: 0x04003E54 RID: 15956
		public bool combineToSingleMaterial;

		// Token: 0x04003E55 RID: 15957
		public bool materialOptionsDeregisterOnDisable;

		// Token: 0x04003E56 RID: 15958
		public bool combineMaterials;

		// Token: 0x04003E57 RID: 15959
		public DAZImport.Gender importGender;

		// Token: 0x04003E58 RID: 15960
		public bool createMorphBank;

		// Token: 0x04003E59 RID: 15961
		public ComputeShader GPUSkinCompute;

		// Token: 0x04003E5A RID: 15962
		public ComputeShader GPUMeshCompute;

		// Token: 0x04003E5B RID: 15963
		[HideInInspector]
		public string standardShader = "Custom/Subsurface/Cull";

		// Token: 0x04003E5C RID: 15964
		[HideInInspector]
		public string glossShader = "Custom/Subsurface/GlossCull";

		// Token: 0x04003E5D RID: 15965
		[HideInInspector]
		public string normalMapShader = "Custom/Subsurface/GlossNMCull";

		// Token: 0x04003E5E RID: 15966
		[HideInInspector]
		public string transparentShader = "Custom/Subsurface/Transparent";

		// Token: 0x04003E5F RID: 15967
		[HideInInspector]
		public string reflTransparentShader = "Custom/Subsurface/Transparent";

		// Token: 0x04003E60 RID: 15968
		[HideInInspector]
		public string transparentNormalMapShader = "Custom/Subsurface/TransparentGlossNMNoCullSeparateAlpha";

		// Token: 0x04003E61 RID: 15969
		[NonSerialized]
		public string subsurfaceStandardShader = "Custom/Subsurface/Cull";

		// Token: 0x04003E62 RID: 15970
		[NonSerialized]
		public string subsurfaceNoCullStandardShader = "Custom/Subsurface/NoCull";

		// Token: 0x04003E63 RID: 15971
		[NonSerialized]
		public string subsurfaceGlossMapShader = "Custom/Subsurface/GlossCull";

		// Token: 0x04003E64 RID: 15972
		[NonSerialized]
		public string subsurfaceNoCullGlossMapShader = "Custom/Subsurface/GlossNoCull";

		// Token: 0x04003E65 RID: 15973
		[NonSerialized]
		public string subsurfaceNormalMapShader = "Custom/Subsurface/GlossNMCull";

		// Token: 0x04003E66 RID: 15974
		[NonSerialized]
		public string subsurfaceNoCullNormalMapShader = "Custom/Subsurface/GlossNMNoCull";

		// Token: 0x04003E67 RID: 15975
		[NonSerialized]
		public string subsurfaceTransparentShader = "Custom/Subsurface/TransparentSeparateAlpha";

		// Token: 0x04003E68 RID: 15976
		[NonSerialized]
		public string subsurfaceReflTransparentShader = "Custom/Subsurface/TransparentSeparateAlpha";

		// Token: 0x04003E69 RID: 15977
		[NonSerialized]
		public string subsurfaceTransparentNormalMapShader = "Custom/Subsurface/TransparentGlossNMNoCullSeparateAlpha";

		// Token: 0x04003E6A RID: 15978
		[NonSerialized]
		public string subsurfaceMarmosetReflTransparentShader = "Marmoset/Transparent/Simple Glass/Specular IBL";

		// Token: 0x04003E6B RID: 15979
		[NonSerialized]
		public string subsurfaceAlphaAdjustShader = "Custom/Subsurface/TransparentGlossNMNoCullSeparateAlpha";

		// Token: 0x04003E6C RID: 15980
		[NonSerialized]
		public string hairShader = "Custom/Hair/MainSeparateAlphaLayer1";

		// Token: 0x04003E6D RID: 15981
		[NonSerialized]
		public string hairScalpShader = "Custom/Hair/MainSeparateAlphaLayerScalp";

		// Token: 0x04003E6E RID: 15982
		[NonSerialized]
		public string clothingShader = "Custom/Subsurface/TransparentGlossNMNoCullSeparateAlpha";

		// Token: 0x04003E6F RID: 15983
		public string MaterialCreationDirectory = "Assets/VaMAssets/Import/Environments";

		// Token: 0x04003E70 RID: 15984
		public string RuntimeMaterialCreationDirectory = "Custom/Atoms";

		// Token: 0x04003E71 RID: 15985
		public bool overrideMaterialFolderName;

		// Token: 0x04003E72 RID: 15986
		public string GeneratedMeshAssetDirectory = "Assets/VaMAssets/Generated/Atoms";

		// Token: 0x04003E73 RID: 15987
		public string MaterialOverrideFolderName = "Generic";

		// Token: 0x04003E74 RID: 15988
		private string MaterialFolder;

		// Token: 0x04003E75 RID: 15989
		protected Dictionary<Texture2D, string> textureToSourcePath;

		// Token: 0x04003E76 RID: 15990
		public Color subdermisColor = Color.white;

		// Token: 0x04003E77 RID: 15991
		public float bumpiness = 1f;

		// Token: 0x04003E78 RID: 15992
		public bool forceDiffuseColor;

		// Token: 0x04003E79 RID: 15993
		public Color diffuseColor = Color.white;

		// Token: 0x04003E7A RID: 15994
		public bool forceSpecularColor;

		// Token: 0x04003E7B RID: 15995
		public Color specularColor = Color.white;

		// Token: 0x04003E7C RID: 15996
		public bool createSkinsAndNodes = true;

		// Token: 0x04003E7D RID: 15997
		public bool createSkins = true;

		// Token: 0x04003E7E RID: 15998
		public Transform skinToWrapToTransform;

		// Token: 0x04003E7F RID: 15999
		public DAZSkinV2 skinToWrapTo;

		// Token: 0x04003E80 RID: 16000
		public bool wrapOnImport = true;

		// Token: 0x04003E81 RID: 16001
		public bool wrapToMorphedVertices;

		// Token: 0x04003E82 RID: 16002
		public bool createSkinWrap = true;

		// Token: 0x04003E83 RID: 16003
		public float skinWrapSurfaceOffset = 0.0003f;

		// Token: 0x04003E84 RID: 16004
		public float skinWrapAdditionalThicknessMultiplier = 0.001f;

		// Token: 0x04003E85 RID: 16005
		public int skinWrapSmoothOuterLoops = 1;

		// Token: 0x04003E86 RID: 16006
		public bool createMaterialOptions = true;

		// Token: 0x04003E87 RID: 16007
		public bool addParentNameToMaterialOptions;

		// Token: 0x04003E88 RID: 16008
		public bool connectMaterialUI = true;

		// Token: 0x04003E89 RID: 16009
		public UIMultiConnector materialUIConnector;

		// Token: 0x04003E8A RID: 16010
		public UIConnectorMaster materialUIConnectorMaster;

		// Token: 0x04003E8B RID: 16011
		public bool addMaterialTabs = true;

		// Token: 0x04003E8C RID: 16012
		public RectTransform materialUITab;

		// Token: 0x04003E8D RID: 16013
		public Color materialUITabColor;

		// Token: 0x04003E8E RID: 16014
		public string materialUITabAddBeforeTabName;

		// Token: 0x04003E8F RID: 16015
		public bool nestObjects;

		// Token: 0x04003E90 RID: 16016
		public bool embedMeshAndSkinOnNodes;

		// Token: 0x04003E91 RID: 16017
		public DAZSkinV2.PhysicsType skinPhysicsType;

		// Token: 0x04003E92 RID: 16018
		public Transform container;

		// Token: 0x04003E93 RID: 16019
		public Transform nodePrefab;

		// Token: 0x04003E94 RID: 16020
		public Transform controlPrefab;

		// Token: 0x04003E95 RID: 16021
		public Transform UIPrefab;

		// Token: 0x04003E96 RID: 16022
		public Transform UIPrefabNoPhysics;

		// Token: 0x04003E97 RID: 16023
		public Transform controlContainer;

		// Token: 0x04003E98 RID: 16024
		public Transform UIContainer;

		// Token: 0x04003E99 RID: 16025
		private Dictionary<string, JSONNode> DAZ_uv_library;

		// Token: 0x04003E9A RID: 16026
		private Dictionary<string, DAZUVMap> DAZ_uv_map;

		// Token: 0x04003E9B RID: 16027
		private Dictionary<string, Material> DAZ_material_map;

		// Token: 0x04003E9C RID: 16028
		private Dictionary<string, JSONNode> DAZ_geometry_library;

		// Token: 0x04003E9D RID: 16029
		private Dictionary<string, JSONNode> DAZ_modifier_library;

		// Token: 0x04003E9E RID: 16030
		private Dictionary<string, JSONNode> DAZ_node_library;

		// Token: 0x04003E9F RID: 16031
		private Dictionary<string, JSONNode> DAZ_material_library;

		// Token: 0x04003EA0 RID: 16032
		private Dictionary<string, Transform> sceneNodeIDToTransform;

		// Token: 0x04003EA1 RID: 16033
		private Dictionary<string, string> graftIDToMainID;

		// Token: 0x04003EA2 RID: 16034
		private Dictionary<string, string> geometryIDToNodeID;

		// Token: 0x04003EA3 RID: 16035
		private Dictionary<string, string> sceneGeometryIDToSceneNodeID;

		// Token: 0x04003EA4 RID: 16036
		private DAZSkinV2[] dazSkins;

		// Token: 0x04003EA5 RID: 16037
		public bool useSceneLabelsInsteadOfIds;

		// Token: 0x04003EA6 RID: 16038
		public bool skipImportOfExisting;

		// Token: 0x04003EA7 RID: 16039
		public string[] sceneNodeIds;

		// Token: 0x04003EA8 RID: 16040
		public string[] sceneNodeLabels;

		// Token: 0x04003EA9 RID: 16041
		public bool[] sceneNodeIdImport;

		// Token: 0x04003EAA RID: 16042
		public bool[] sceneNodeIdControllable;

		// Token: 0x04003EAB RID: 16043
		public bool[] sceneNodeIdIsPhysicsObj;

		// Token: 0x04003EAC RID: 16044
		public bool[] sceneNodeIdFloorLock;

		// Token: 0x04003EAD RID: 16045
		public string importStatus;

		// Token: 0x04003EAE RID: 16046
		public bool isImporting;

		// Token: 0x02000B3A RID: 2874
		public enum ImportType
		{
			// Token: 0x04003EB0 RID: 16048
			SingleObject,
			// Token: 0x04003EB1 RID: 16049
			SkinnedSingleObject,
			// Token: 0x04003EB2 RID: 16050
			Environment,
			// Token: 0x04003EB3 RID: 16051
			Character,
			// Token: 0x04003EB4 RID: 16052
			Clothing,
			// Token: 0x04003EB5 RID: 16053
			Hair,
			// Token: 0x04003EB6 RID: 16054
			HairScalp
		}

		// Token: 0x02000B3B RID: 2875
		public enum Gender
		{
			// Token: 0x04003EB8 RID: 16056
			Neutral,
			// Token: 0x04003EB9 RID: 16057
			Female,
			// Token: 0x04003EBA RID: 16058
			Male
		}

		// Token: 0x02000B3C RID: 2876
		// (Invoke) Token: 0x06004EB6 RID: 20150
		public delegate void ImportCallback();

		// Token: 0x02000FD5 RID: 4053
		[CompilerGenerated]
		private sealed class <ImportDufMorphsCo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007560 RID: 30048 RVA: 0x001BD9F5 File Offset: 0x001BBDF5
			[DebuggerHidden]
			public <ImportDufMorphsCo>c__Iterator0()
			{
			}

			// Token: 0x06007561 RID: 30049 RVA: 0x001BDA00 File Offset: 0x001BBE00
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.isImporting = true;
					this.importStatus = "Initialization";
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					base.SetRegistryLibPaths();
					this.DAZ_modifier_library = new Dictionary<string, JSONNode>();
					this.DAZ_geometry_library = new Dictionary<string, JSONNode>();
					this.DAZ_node_library = new Dictionary<string, JSONNode>();
					this.DAZ_uv_library = new Dictionary<string, JSONNode>();
					if (this.DAZSceneDufFile != null && this.DAZSceneDufFile != string.Empty)
					{
						djn = null;
						dufname = Regex.Replace(this.DAZSceneDufFile, ".*/", string.Empty);
						dufname = Regex.Replace(dufname, "\\.duf", string.Empty);
						try
						{
							djn = DAZImport.ReadJSON(this.DAZSceneDufFile);
						}
						catch (Exception ex)
						{
							SuperController.LogError("Exception during DAZ import: " + ex.Message);
							this.importStatus = "Import failed " + ex.Message;
							this.isImporting = false;
							if (callback != null)
							{
								callback();
							}
							return false;
						}
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					break;
				case 2U:
					JSONSceneModifiers = djn["scene"]["modifiers"];
					if (JSONSceneModifiers != null)
					{
						IEnumerator enumerator = JSONSceneModifiers.AsArray.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								JSONNode jsonnode = (JSONNode)obj;
								string a = jsonnode["channel"]["type"];
								if (a == "float")
								{
									string sceneId = DAZImport.DAZurlToId(jsonnode["parent"]);
									DAZMorph dazmorph = base.ProcessMorph(jsonnode["url"], sceneId);
									if (dazmorph != null && !dazmorph.preserveValueOnReimport)
									{
										float num2 = jsonnode["channel"]["current_value"].AsFloat;
										if (num2 <= 0.001f && num2 >= -0.001f)
										{
											num2 = 0f;
										}
										dazmorph.importValue = num2;
										dazmorph.morphValue = num2;
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
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x1700115B RID: 4443
			// (get) Token: 0x06007562 RID: 30050 RVA: 0x001BDD10 File Offset: 0x001BC110
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700115C RID: 4444
			// (get) Token: 0x06007563 RID: 30051 RVA: 0x001BDD18 File Offset: 0x001BC118
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007564 RID: 30052 RVA: 0x001BDD20 File Offset: 0x001BC120
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007565 RID: 30053 RVA: 0x001BDD30 File Offset: 0x001BC130
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400696F RID: 26991
			internal JSONNode <djn>__1;

			// Token: 0x04006970 RID: 26992
			internal string <dufname>__1;

			// Token: 0x04006971 RID: 26993
			internal DAZImport.ImportCallback callback;

			// Token: 0x04006972 RID: 26994
			internal JSONNode <JSONSceneModifiers>__1;

			// Token: 0x04006973 RID: 26995
			internal DAZImport $this;

			// Token: 0x04006974 RID: 26996
			internal object $current;

			// Token: 0x04006975 RID: 26997
			internal bool $disposing;

			// Token: 0x04006976 RID: 26998
			internal int $PC;
		}

		// Token: 0x02000FD6 RID: 4054
		[CompilerGenerated]
		private sealed class <ImportDufCo>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007566 RID: 30054 RVA: 0x001BDD37 File Offset: 0x001BC137
			[DebuggerHidden]
			public <ImportDufCo>c__Iterator1()
			{
			}

			// Token: 0x06007567 RID: 30055 RVA: 0x001BDD40 File Offset: 0x001BC140
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					this.isImporting = true;
					this.importStatus = "Initialization";
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					base.SetRegistryLibPaths();
					sceneNodeIdToImport = new Dictionary<string, bool>();
					sceneNodeIdToControllable = new Dictionary<string, bool>();
					sceneNodeIdToPhysicsEnabled = new Dictionary<string, bool>();
					sceneNodeIdToFloorLock = new Dictionary<string, bool>();
					if (this.sceneNodeIds != null)
					{
						for (int i = 0; i < this.sceneNodeIds.Length; i++)
						{
							string key = this.sceneNodeIds[i];
							sceneNodeIdToImport.Add(key, this.sceneNodeIdImport[i]);
							sceneNodeIdToControllable.Add(key, this.sceneNodeIdControllable[i]);
							sceneNodeIdToPhysicsEnabled.Add(key, this.sceneNodeIdIsPhysicsObj[i]);
							sceneNodeIdToFloorLock.Add(key, this.sceneNodeIdFloorLock[i]);
						}
					}
					if (this.container == null)
					{
						this.container = base.transform;
					}
					this.DAZ_modifier_library = new Dictionary<string, JSONNode>();
					this.DAZ_geometry_library = new Dictionary<string, JSONNode>();
					this.DAZ_node_library = new Dictionary<string, JSONNode>();
					this.DAZ_uv_library = new Dictionary<string, JSONNode>();
					this.DAZ_uv_map = new Dictionary<string, DAZUVMap>();
					this.DAZ_material_map = new Dictionary<string, Material>();
					this.sceneNodeIDToTransform = new Dictionary<string, Transform>();
					this.graftIDToMainID = new Dictionary<string, string>();
					if (this.DAZSceneDufFile != null && this.DAZSceneDufFile != string.Empty)
					{
						djn = null;
						dufname = Regex.Replace(this.DAZSceneDufFile, ".*/", string.Empty);
						dufname = Regex.Replace(dufname, "\\.duf", string.Empty);
						try
						{
							djn = DAZImport.ReadJSON(this.DAZSceneDufFile);
							string str;
							if (Application.isPlaying)
							{
								if (this.importType == DAZImport.ImportType.Character || this.importType == DAZImport.ImportType.Clothing || this.importType == DAZImport.ImportType.Hair || this.importType == DAZImport.ImportType.HairScalp)
								{
									str = this.RuntimeMaterialCreationDirectory + "/" + this.importGender.ToString();
								}
								else
								{
									str = this.RuntimeMaterialCreationDirectory;
								}
							}
							else
							{
								str = this.MaterialCreationDirectory;
							}
							if (this.overrideMaterialFolderName)
							{
								this.MaterialFolder = str + "/" + this.MaterialOverrideFolderName;
							}
							else
							{
								this.MaterialFolder = str + "/" + dufname;
							}
							if (Application.isPlaying)
							{
								if (ImageLoaderThreaded.singleton != null)
								{
									ImageLoaderThreaded.singleton.PurgeAllImmediateTextures();
								}
								Directory.CreateDirectory(this.MaterialFolder);
							}
							base.ClearMaterialConnectors();
							this.importStatus = "Reading libraries";
							base.ProcessDAZLibraries(string.Empty, djn);
						}
						catch (Exception ex)
						{
							SuperController.LogError("Exception during DAZ import: " + ex.Message);
							this.importStatus = "Import failed " + ex.Message;
							this.isImporting = false;
							if (callback != null)
							{
								callback();
							}
							return false;
						}
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					goto IL_1AB4;
				case 2U:
					JSONSceneMaterials = null;
					materialMap = new Dictionary<string, List<DAZUVMap>>();
					try
					{
						JSONSceneMaterials = djn["scene"]["materials"];
						IEnumerator enumerator3 = JSONSceneMaterials.AsArray.GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								object obj = enumerator3.Current;
								JSONNode jsonnode = (JSONNode)obj;
								string key2 = DAZImport.DAZurlToId(jsonnode["geometry"]);
								string uvurl = jsonnode["uv_set"];
								DAZUVMap dazuvmap = base.ProcessUV(uvurl);
								if (dazuvmap != null)
								{
									List<DAZUVMap> list;
									if (!materialMap.TryGetValue(key2, out list))
									{
										list = new List<DAZUVMap>();
										materialMap.Add(key2, list);
									}
									list.Add(dazuvmap);
								}
								else
								{
									this.importStatus = "Error during process of UV maps";
								}
							}
						}
						finally
						{
							IDisposable disposable3;
							if ((disposable3 = (enumerator3 as IDisposable)) != null)
							{
								disposable3.Dispose();
							}
						}
					}
					catch (Exception ex2)
					{
						SuperController.LogError("Exception during DAZ import: " + ex2.Message);
						this.importStatus = "Import failed " + ex2.Message;
						this.isImporting = false;
						if (callback != null)
						{
							callback();
						}
						return false;
					}
					numMaterials = JSONSceneMaterials.Count;
					ind = 1;
					enumerator2 = JSONSceneMaterials.AsArray.GetEnumerator();
					num = 4294967293U;
					break;
				case 3U:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						sm = (JSONNode)enumerator2.Current;
						try
						{
							this.importStatus = string.Concat(new object[]
							{
								"Import material ",
								ind,
								" of ",
								numMaterials
							});
							float num2 = (float)ind * 1f / (float)numMaterials;
							if (!base.ProcessMaterial(sm))
							{
								this.importStatus = "Error during process of material";
							}
						}
						catch (Exception ex3)
						{
							SuperController.LogError("Exception during DAZ import: " + ex3);
							this.importStatus = "Import failed " + ex3.Message;
							this.isImporting = false;
							if (callback != null)
							{
								callback();
							}
							return false;
						}
						ind++;
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if ((disposable2 = (enumerator2 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
				}
				try
				{
					JSONNode jsonnode2 = djn["scene"]["modifiers"];
					this.importStatus = "Process geometry";
					JSONNode jsonnode3 = djn["scene"]["nodes"];
					this.sceneGeometryIDToSceneNodeID = new Dictionary<string, string>();
					DAZSkinWrap dazskinWrap = null;
					Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
					Dictionary<string, Transform> dictionary2 = new Dictionary<string, Transform>();
					IEnumerator enumerator4 = jsonnode3.AsArray.GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							object obj2 = enumerator4.Current;
							JSONNode jsonnode4 = (JSONNode)obj2;
							string text = jsonnode4["id"];
							string str2;
							if (this.useSceneLabelsInsteadOfIds)
							{
								str2 = jsonnode4["label"];
							}
							else
							{
								str2 = text;
							}
							bool flag2 = false;
							if (jsonnode4["geometries"] != null)
							{
								flag2 = true;
							}
							bool flag3 = true;
							if (sceneNodeIdToImport.ContainsKey(text))
							{
								sceneNodeIdToImport.TryGetValue(text, out flag3);
							}
							if (flag2 && this.skipImportOfExisting && base.NodeExists(jsonnode4, flag2))
							{
								flag3 = false;
							}
							if (flag3)
							{
								string nodeId = base.ProcessNodeCreation(jsonnode4, flag2);
								bool flag4 = false;
								bool flag5 = false;
								if (flag2)
								{
									Transform transform = base.transform;
									Transform transform2;
									if (this.sceneNodeIDToTransform.TryGetValue(text, out transform2))
									{
										if (this.embedMeshAndSkinOnNodes)
										{
											if (jsonnode4["conform_target"] != null)
											{
												string sceneNodeId = DAZImport.DAZurlToId(jsonnode4["conform_target"]);
												DAZMesh dazmeshBySceneNodeId = base.GetDAZMeshBySceneNodeId(sceneNodeId);
												if (dazmeshBySceneNodeId != null)
												{
													transform = dazmeshBySceneNodeId.transform;
												}
												else
												{
													transform = transform2;
												}
											}
											else
											{
												transform = transform2;
											}
										}
										dictionary.Add(text, transform);
										if (base.shouldCreateSkins)
										{
											DAZBones x = transform2.GetComponent<DAZBones>();
											if (x == null)
											{
												x = transform2.gameObject.AddComponent<DAZBones>();
											}
										}
										GameObject gameObject = null;
										bool flag6 = false;
										bool flag7 = false;
										sceneNodeIdToControllable.TryGetValue(text, out flag7);
										if (flag7)
										{
											if (this.controlContainer != null && this.controlPrefab != null)
											{
												string name = str2 + "Control";
												Transform transform3 = this.controlContainer.Find(name);
												if (transform3 != null)
												{
													gameObject = transform3.gameObject;
												}
												else
												{
													if (Application.isPlaying)
													{
														gameObject = UnityEngine.Object.Instantiate<GameObject>(this.controlPrefab.gameObject);
													}
													gameObject.name = name;
													gameObject.transform.parent = this.controlContainer;
													gameObject.transform.position = transform.position;
													gameObject.transform.rotation = transform.rotation;
												}
												dictionary2.Add(text, gameObject.transform);
												Rigidbody component = gameObject.GetComponent<Rigidbody>();
												Rigidbody component2 = transform.GetComponent<Rigidbody>();
												if (component2 != null && sceneNodeIdToPhysicsEnabled.TryGetValue(text, out flag4))
												{
													component2.isKinematic = !flag4;
												}
												ConfigurableJoint component3 = transform.GetComponent<ConfigurableJoint>();
												if (component3 != null && component != null)
												{
													flag6 = true;
													component3.connectedBody = component;
												}
												sceneNodeIdToFloorLock.TryGetValue(text, out flag5);
											}
											if (this.UIContainer != null && this.UIPrefab != null)
											{
												GameObject gameObject2 = null;
												string name2 = str2 + "UI";
												Transform transform4 = this.UIContainer.Find(name2);
												if (transform4 != null)
												{
													gameObject2 = transform4.gameObject;
												}
												else
												{
													if (Application.isPlaying)
													{
														if (!flag4 && this.UIPrefabNoPhysics != null)
														{
															gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.UIPrefabNoPhysics.gameObject);
														}
														else
														{
															gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.UIPrefab.gameObject);
														}
													}
													gameObject2.name = name2;
													gameObject2.transform.parent = this.UIContainer;
													if (gameObject != null)
													{
														FreeControllerV3 component4 = gameObject.GetComponent<FreeControllerV3>();
														if (component4 != null)
														{
															if (flag6)
															{
																component4.followWhenOff = transform;
															}
															if (flag4)
															{
																component4.startingPositionState = FreeControllerV3.PositionState.Off;
																component4.startingRotationState = FreeControllerV3.RotationState.Off;
															}
															if (flag5)
															{
																component4.yLock = true;
																component4.xRotLock = true;
																component4.zRotLock = true;
															}
															component4.UITransforms = new Transform[1];
															component4.UITransforms[0] = gameObject2.transform;
															UIConnector uiconnector = gameObject2.AddComponent<UIConnector>();
															uiconnector.receiverTransform = component4.transform;
															uiconnector.receiver = component4;
															uiconnector.storeid = component4.storeId;
														}
														MotionAnimationControl component5 = gameObject.GetComponent<MotionAnimationControl>();
														if (component5 != null)
														{
															UIConnector uiconnector2 = gameObject2.AddComponent<UIConnector>();
															uiconnector2.receiverTransform = component5.transform;
															uiconnector2.receiver = component5;
															uiconnector2.storeid = component5.storeId;
														}
														SetTransformScale component6 = transform.GetComponent<SetTransformScale>();
														if (component6 != null)
														{
															UIConnector uiconnector3 = gameObject2.AddComponent<UIConnector>();
															uiconnector3.receiverTransform = component6.transform;
															uiconnector3.receiver = component6;
															uiconnector3.storeid = component6.storeId;
														}
														PhysicsMaterialControl component7 = transform.GetComponent<PhysicsMaterialControl>();
														if (component7 != null)
														{
															UIConnector uiconnector4 = gameObject2.AddComponent<UIConnector>();
															uiconnector4.receiverTransform = component7.transform;
															uiconnector4.receiver = component7;
															uiconnector4.storeid = component7.storeId;
														}
													}
												}
											}
										}
										else
										{
											ConfigurableJoint component8 = transform.GetComponent<ConfigurableJoint>();
											if (component8 != null)
											{
												UnityEngine.Object.DestroyImmediate(component8);
											}
											Rigidbody component9 = transform.GetComponent<Rigidbody>();
											if (component9 != null)
											{
												UnityEngine.Object.DestroyImmediate(component9);
											}
											ForceReceiver component10 = transform.GetComponent<ForceReceiver>();
											if (component10 != null)
											{
												UnityEngine.Object.DestroyImmediate(component10);
											}
											PhysicsMaterialControl component11 = transform.GetComponent<PhysicsMaterialControl>();
											if (component11 != null)
											{
												UnityEngine.Object.DestroyImmediate(component11);
											}
										}
									}
									DAZMesh dazmesh = null;
									IEnumerator enumerator5 = jsonnode4["geometries"].AsArray.GetEnumerator();
									try
									{
										while (enumerator5.MoveNext())
										{
											object obj3 = enumerator5.Current;
											JSONNode jsonnode5 = (JSONNode)obj3;
											string text2 = jsonnode5["url"];
											if (text2 != null)
											{
												string text3 = jsonnode5["id"];
												this.sceneGeometryIDToSceneNodeID.Add(text3, text);
												List<DAZUVMap> list2;
												if (materialMap.TryGetValue(text3, out list2))
												{
													dazmesh = base.ProcessGeometry(text2, text3, list2.ToArray(), text, nodeId, transform.gameObject);
													if (dazmesh != null)
													{
														if (base.shouldCreateSkinWrap)
														{
															if (!Application.isPlaying)
															{
																dazmesh.SaveMeshAsset(false);
															}
															dazmesh.createMeshFilterAndRenderer = true;
															MeshRenderer component12 = dazmesh.GetComponent<MeshRenderer>();
															if (component12 != null)
															{
																component12.enabled = false;
															}
															dazmesh.drawMorphedUVMappedMesh = false;
															dazmesh.enabled = false;
															DAZSkinWrap orCreateDAZSkinWrap = base.GetOrCreateDAZSkinWrap(dazmesh);
															if (orCreateDAZSkinWrap != null)
															{
																dazskinWrap = orCreateDAZSkinWrap;
																dazskinWrap.surfaceOffset = this.skinWrapSurfaceOffset;
																dazskinWrap.additionalThicknessMultiplier = this.skinWrapAdditionalThicknessMultiplier;
																dazskinWrap.smoothOuterLoops = this.skinWrapSmoothOuterLoops;
																orCreateDAZSkinWrap.CopyMaterials();
																if (this.createMaterialOptions)
																{
																	base.CreateMaterialOptions(orCreateDAZSkinWrap.gameObject, orCreateDAZSkinWrap.numMaterials, orCreateDAZSkinWrap.materialNames, orCreateDAZSkinWrap.GPUmaterials, typeof(DAZSkinWrapMaterialOptions), this.MaterialFolder, Application.isPlaying);
																}
															}
														}
														else if (!base.shouldCreateSkins)
														{
															if (this.createMaterialOptions)
															{
																base.CreateMaterialOptions(dazmesh.gameObject, dazmesh.numMaterials, dazmesh.materialNames, dazmesh.materials, typeof(MaterialOptions), this.MaterialFolder, Application.isPlaying);
															}
															if (!Application.isPlaying)
															{
																dazmesh.SaveMeshAsset(false);
															}
															dazmesh.createMeshFilterAndRenderer = true;
															dazmesh.drawMorphedUVMappedMesh = false;
															dazmesh.createMeshCollider = true;
															if (flag4)
															{
																dazmesh.useConvexCollider = true;
															}
															else
															{
																dazmesh.useConvexCollider = false;
															}
															dazmesh.enabled = false;
														}
													}
												}
												else
												{
													UnityEngine.Debug.LogError("Could not find materials for " + text3);
												}
											}
											else
											{
												UnityEngine.Debug.LogError("Could not find geometries url");
											}
											if (dazskinWrap != null && !this.embedMeshAndSkinOnNodes)
											{
												break;
											}
										}
									}
									finally
									{
										IDisposable disposable4;
										if ((disposable4 = (enumerator5 as IDisposable)) != null)
										{
											disposable4.Dispose();
										}
									}
									if (jsonnode4["conform_target"] != null)
									{
										string sceneNodeId2 = DAZImport.DAZurlToId(jsonnode4["conform_target"]);
										DAZMesh dazmeshBySceneNodeId2 = base.GetDAZMeshBySceneNodeId(sceneNodeId2);
										if (dazmeshBySceneNodeId2 != null && dazmesh != null)
										{
											dazmesh.graftTo = dazmeshBySceneNodeId2;
										}
									}
								}
								if (dazskinWrap != null && !this.embedMeshAndSkinOnNodes)
								{
									break;
								}
							}
						}
					}
					finally
					{
						IDisposable disposable5;
						if ((disposable5 = (enumerator4 as IDisposable)) != null)
						{
							disposable5.Dispose();
						}
					}
					if (base.shouldCreateSkinsAndNodes)
					{
						IEnumerator enumerator6 = jsonnode3.AsArray.GetEnumerator();
						try
						{
							while (enumerator6.MoveNext())
							{
								object obj4 = enumerator6.Current;
								JSONNode jsonnode6 = (JSONNode)obj4;
								string key3 = jsonnode6["id"];
								bool flag8 = false;
								if (jsonnode6["geometries"] != null)
								{
									flag8 = true;
								}
								bool flag9 = true;
								if (sceneNodeIdToImport.ContainsKey(key3))
								{
									sceneNodeIdToImport.TryGetValue(key3, out flag9);
								}
								if (flag8 && this.skipImportOfExisting && base.NodeExists(jsonnode6, flag8))
								{
									flag9 = false;
								}
								if (flag9)
								{
									base.ProcessNodeTransform(jsonnode6, flag8);
								}
							}
						}
						finally
						{
							IDisposable disposable6;
							if ((disposable6 = (enumerator6 as IDisposable)) != null)
							{
								disposable6.Dispose();
							}
						}
						IEnumerator enumerator7 = jsonnode3.AsArray.GetEnumerator();
						try
						{
							while (enumerator7.MoveNext())
							{
								object obj5 = enumerator7.Current;
								JSONNode jsonnode7 = (JSONNode)obj5;
								string key4 = jsonnode7["id"];
								Transform transform5;
								Transform transform6;
								if (dictionary2.TryGetValue(key4, out transform5) && dictionary.TryGetValue(key4, out transform6))
								{
									transform5.position = transform6.position;
									transform5.rotation = transform6.rotation;
								}
							}
						}
						finally
						{
							IDisposable disposable7;
							if ((disposable7 = (enumerator7 as IDisposable)) != null)
							{
								disposable7.Dispose();
							}
						}
					}
					if (jsonnode2 != null)
					{
						IEnumerator enumerator8 = jsonnode2.AsArray.GetEnumerator();
						try
						{
							while (enumerator8.MoveNext())
							{
								object obj6 = enumerator8.Current;
								JSONNode jsonnode8 = (JSONNode)obj6;
								string a = jsonnode8["channel"]["type"];
								if (a == "float")
								{
									string sceneId = DAZImport.DAZurlToId(jsonnode8["parent"]);
									DAZMorph dazmorph = base.ProcessMorph(jsonnode8["url"], sceneId);
									if (dazmorph != null && !dazmorph.preserveValueOnReimport)
									{
										float num3 = jsonnode8["channel"]["current_value"].AsFloat;
										if (num3 <= 0.001f && num3 >= -0.001f)
										{
											num3 = 0f;
										}
										dazmorph.importValue = num3;
										dazmorph.morphValue = num3;
									}
								}
							}
						}
						finally
						{
							IDisposable disposable8;
							if ((disposable8 = (enumerator8 as IDisposable)) != null)
							{
								disposable8.Dispose();
							}
						}
					}
					if (this.container != null)
					{
						foreach (DAZBones dazbones in this.container.GetComponentsInChildren<DAZBones>())
						{
							dazbones.Reset();
						}
					}
					foreach (DAZMesh dazmesh2 in base.GetComponents<DAZMesh>())
					{
						dazmesh2.ReInitMorphs();
					}
					if (this.wrapOnImport)
					{
						foreach (DAZSkinWrap dazskinWrap2 in base.GetComponents<DAZSkinWrap>())
						{
							if (dazskinWrap2.wrapStore == null || dazskinWrap2.wrapStore.wrapVertices == null)
							{
								dazskinWrap2.wrapToMorphedVertices = this.wrapToMorphedVertices;
								dazskinWrap2.Wrap();
							}
						}
					}
					DAZSkinWrap component13 = base.GetComponent<DAZSkinWrap>();
					if (component13 != null)
					{
						component13.draw = true;
						if (Application.isPlaying)
						{
							DAZSkinWrapControl component14 = base.GetComponent<DAZSkinWrapControl>();
							if (component14 != null)
							{
								component14.wrap = component13;
							}
						}
					}
					foreach (DAZMergedMesh dazmergedMesh in base.GetComponents<DAZMergedMesh>())
					{
						dazmergedMesh.ManualUpdate();
					}
					if (this.DAZSceneModifierOverrideDufFile != null && this.DAZSceneModifierOverrideDufFile != string.Empty)
					{
						JSONNode jl = DAZImport.ReadJSON(this.DAZSceneModifierOverrideDufFile);
						string url = DAZImport.DAZurlFix(this.modifierOverrideUrl);
						base.ProcessAltModifiers(url, jl);
					}
					if (base.shouldCreateSkinsAndNodes && base.shouldCreateSkins && this.container != null && jsonnode2 != null)
					{
						IEnumerator enumerator9 = jsonnode2.AsArray.GetEnumerator();
						try
						{
							while (enumerator9.MoveNext())
							{
								object obj7 = enumerator9.Current;
								JSONNode jsonnode9 = (JSONNode)obj7;
								string a2 = jsonnode9["extra"][0]["type"];
								if (a2 == "skin_settings")
								{
									string skinId = jsonnode9["id"];
									string skinUrl = jsonnode9["url"];
									string text4 = DAZImport.DAZurlToId(jsonnode9["parent"]);
									Transform transform7 = base.transform;
									if (this.embedMeshAndSkinOnNodes && text4 != null)
									{
										DAZMesh dazmeshBySceneGeometryId = base.GetDAZMeshBySceneGeometryId(text4);
										if (dazmeshBySceneGeometryId != null)
										{
											transform7 = dazmeshBySceneGeometryId.transform;
										}
										else
										{
											UnityEngine.Debug.LogWarning("Could not find DAZMesh with scene geometry ID " + text4);
											transform7 = base.transform;
										}
									}
									DAZSkinV2 orCreateDAZSkin = base.GetOrCreateDAZSkin(skinId, skinUrl, transform7.gameObject);
									orCreateDAZSkin.ImportStart();
									IEnumerator enumerator10 = jsonnode3.AsArray.GetEnumerator();
									try
									{
										while (enumerator10.MoveNext())
										{
											object obj8 = enumerator10.Current;
											JSONNode jsonnode10 = (JSONNode)obj8;
											string url2 = DAZImport.DAZurlFix(jsonnode10["url"]);
											JSONNode node = base.GetNode(url2);
											orCreateDAZSkin.ImportNode(node, url2);
										}
									}
									finally
									{
										IDisposable disposable9;
										if ((disposable9 = (enumerator10 as IDisposable)) != null)
										{
											disposable9.Dispose();
										}
									}
									base.ProcessSkin(jsonnode9, transform7.gameObject);
									if (this.createMaterialOptions)
									{
										base.CreateMaterialOptions(orCreateDAZSkin.gameObject, orCreateDAZSkin.numMaterials, orCreateDAZSkin.materialNames, orCreateDAZSkin.GPUmaterials, typeof(DAZCharacterMaterialOptions), this.MaterialFolder, Application.isPlaying);
									}
								}
							}
						}
						finally
						{
							IDisposable disposable10;
							if ((disposable10 = (enumerator9 as IDisposable)) != null)
							{
								disposable10.Dispose();
							}
						}
						IEnumerator enumerator11 = jsonnode2.AsArray.GetEnumerator();
						try
						{
							while (enumerator11.MoveNext())
							{
								object obj9 = enumerator11.Current;
								JSONNode jsonnode11 = (JSONNode)obj9;
								string a3 = jsonnode11["extra"][0]["type"];
								if (a3 == "skin_settings")
								{
									string skinId2 = jsonnode11["id"];
									string text5 = jsonnode11["url"];
									string text6 = DAZImport.DAZurlToId(jsonnode11["parent"]);
									Transform transform8 = base.transform;
									if (this.embedMeshAndSkinOnNodes && text6 != null)
									{
										DAZMesh dazmeshBySceneGeometryId2 = base.GetDAZMeshBySceneGeometryId(text6);
										if (dazmeshBySceneGeometryId2 != null)
										{
											transform8 = dazmeshBySceneGeometryId2.transform;
										}
										else
										{
											UnityEngine.Debug.LogWarning("Could not find DAZMesh with scene geometry ID " + text6);
											transform8 = base.transform;
										}
									}
									DAZSkinV2 dazskin = base.GetDAZSkin(skinId2, transform8.gameObject);
									if (dazskin.dazMesh.graftTo != null)
									{
										DAZMergedMesh dazmergedMesh2 = transform8.GetComponent<DAZMergedMesh>();
										if (dazmergedMesh2 == null)
										{
											dazmergedMesh2 = transform8.gameObject.AddComponent<DAZMergedMesh>();
										}
										dazmergedMesh2.Merge();
										DAZMergedSkinV2 dazmergedSkinV = transform8.GetComponent<DAZMergedSkinV2>();
										if (dazmergedSkinV == null)
										{
											dazmergedSkinV = transform8.gameObject.AddComponent<DAZMergedSkinV2>();
										}
										dazmergedSkinV.root = dazskin.root;
										dazmergedSkinV.Merge();
										dazmergedSkinV.skin = true;
										dazmergedSkinV.useSmoothing = true;
										dazmergedSkinV.useSmoothVertsForNormalTangentRecalc = true;
										dazmergedSkinV.skinMethod = DAZSkinV2.SkinMethod.CPUAndGPU;
										dazmergedSkinV.CopyMaterials();
									}
								}
							}
						}
						finally
						{
							IDisposable disposable11;
							if ((disposable11 = (enumerator11 as IDisposable)) != null)
							{
								disposable11.Dispose();
							}
						}
					}
					if (this.materialUIConnectorMaster != null)
					{
						this.materialUIConnectorMaster.Rebuild();
					}
				}
				catch (Exception ex4)
				{
					SuperController.LogError("Exception during DAZ import: " + ex4.ToString());
					this.importStatus = "Import failed " + ex4.Message;
					this.isImporting = false;
					if (callback != null)
					{
						callback();
					}
					return false;
				}
				IL_1AB4:
				dd = base.GetComponent<DAZDynamic>();
				if (dd != null)
				{
					dd.RefreshStorables();
				}
				this.importStatus = "Import complete";
				this.isImporting = false;
				if (callback != null)
				{
					callback();
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x1700115D RID: 4445
			// (get) Token: 0x06007568 RID: 30056 RVA: 0x001BF9C0 File Offset: 0x001BDDC0
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700115E RID: 4446
			// (get) Token: 0x06007569 RID: 30057 RVA: 0x001BF9C8 File Offset: 0x001BDDC8
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600756A RID: 30058 RVA: 0x001BF9D0 File Offset: 0x001BDDD0
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 3U:
					try
					{
					}
					finally
					{
						if ((disposable2 = (enumerator2 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
					break;
				}
			}

			// Token: 0x0600756B RID: 30059 RVA: 0x001BFA48 File Offset: 0x001BDE48
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006977 RID: 26999
			internal Dictionary<string, bool> <sceneNodeIdToImport>__0;

			// Token: 0x04006978 RID: 27000
			internal Dictionary<string, bool> <sceneNodeIdToControllable>__0;

			// Token: 0x04006979 RID: 27001
			internal Dictionary<string, bool> <sceneNodeIdToPhysicsEnabled>__0;

			// Token: 0x0400697A RID: 27002
			internal Dictionary<string, bool> <sceneNodeIdToFloorLock>__0;

			// Token: 0x0400697B RID: 27003
			internal JSONNode <djn>__1;

			// Token: 0x0400697C RID: 27004
			internal string <dufname>__1;

			// Token: 0x0400697D RID: 27005
			internal DAZImport.ImportCallback callback;

			// Token: 0x0400697E RID: 27006
			internal JSONNode <JSONSceneMaterials>__1;

			// Token: 0x0400697F RID: 27007
			internal Dictionary<string, List<DAZUVMap>> <materialMap>__1;

			// Token: 0x04006980 RID: 27008
			internal int <numMaterials>__1;

			// Token: 0x04006981 RID: 27009
			internal int <ind>__1;

			// Token: 0x04006982 RID: 27010
			internal IEnumerator $locvar2;

			// Token: 0x04006983 RID: 27011
			internal JSONNode <sm>__2;

			// Token: 0x04006984 RID: 27012
			internal IDisposable $locvar3;

			// Token: 0x04006985 RID: 27013
			internal DAZDynamic <dd>__0;

			// Token: 0x04006986 RID: 27014
			internal DAZImport $this;

			// Token: 0x04006987 RID: 27015
			internal object $current;

			// Token: 0x04006988 RID: 27016
			internal bool $disposing;

			// Token: 0x04006989 RID: 27017
			internal int $PC;
		}
	}
}
