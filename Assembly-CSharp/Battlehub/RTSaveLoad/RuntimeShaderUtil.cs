using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200022F RID: 559
	public class RuntimeShaderUtil : IRuntimeShaderUtil
	{
		// Token: 0x06000BB3 RID: 2995 RVA: 0x00049FA9 File Offset: 0x000483A9
		public RuntimeShaderUtil()
		{
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00049FB1 File Offset: 0x000483B1
		public static string GetPath(bool resourcesFolder)
		{
			return "/Battlehub/RTSaveLoad/ShaderInfo" + ((!resourcesFolder) ? string.Empty : "/Resources");
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00049FD4 File Offset: 0x000483D4
		public static long FileNameToInstanceID(string fileName)
		{
			int num = fileName.LastIndexOf("_");
			if (num == -1)
			{
				return 0L;
			}
			long result;
			if (long.TryParse(fileName.Substring(num + 1).Replace(".txt", string.Empty), out result))
			{
				return result;
			}
			return 0L;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0004A020 File Offset: 0x00048420
		public static string GetShaderInfoFileName(Shader shader, bool withoutExtension = false)
		{
			return string.Format("rt_shader_{0}_{1}" + ((!withoutExtension) ? ".txt" : string.Empty), shader.name.Replace("/", "__"), shader.GetMappedInstanceID().ToString());
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0004A07A File Offset: 0x0004847A
		public static void AddExtra(string key, TextAsset[] textAssets)
		{
			if (!RuntimeShaderUtil.m_textAssets.ContainsKey(key))
			{
				RuntimeShaderUtil.m_textAssets.Add(key, textAssets);
			}
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0004A098 File Offset: 0x00048498
		public static void RemoveExtra(string key)
		{
			RuntimeShaderUtil.m_textAssets.Remove(key);
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0004A0A8 File Offset: 0x000484A8
		public RuntimeShaderInfo GetShaderInfo(Shader shader)
		{
			RuntimeShaderUtil.<GetShaderInfo>c__AnonStorey0 <GetShaderInfo>c__AnonStorey = new RuntimeShaderUtil.<GetShaderInfo>c__AnonStorey0();
			if (shader == null)
			{
				throw new ArgumentNullException("shader");
			}
			<GetShaderInfo>c__AnonStorey.shaderName = RuntimeShaderUtil.GetShaderInfoFileName(shader, true);
			TextAsset textAsset = Resources.Load<TextAsset>(<GetShaderInfo>c__AnonStorey.shaderName);
			if (textAsset == null)
			{
				foreach (TextAsset[] source in RuntimeShaderUtil.m_textAssets.Values)
				{
					textAsset = source.Where(new Func<TextAsset, bool>(<GetShaderInfo>c__AnonStorey.<>m__0)).FirstOrDefault<TextAsset>();
					if (textAsset != null)
					{
						break;
					}
				}
			}
			if (textAsset == null)
			{
				Debug.LogFormat("Shader {0} is not found", new object[]
				{
					<GetShaderInfo>c__AnonStorey.shaderName
				});
				return null;
			}
			return JsonUtility.FromJson<RuntimeShaderInfo>(textAsset.text);
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0004A1A0 File Offset: 0x000485A0
		// Note: this type is marked as 'beforefieldinit'.
		static RuntimeShaderUtil()
		{
		}

		// Token: 0x04000CA6 RID: 3238
		private static readonly Dictionary<string, TextAsset[]> m_textAssets = new Dictionary<string, TextAsset[]>();

		// Token: 0x04000CA7 RID: 3239
		private const string Path = "/Battlehub/RTSaveLoad/ShaderInfo";

		// Token: 0x02000EC9 RID: 3785
		[CompilerGenerated]
		private sealed class <GetShaderInfo>c__AnonStorey0
		{
			// Token: 0x060071BC RID: 29116 RVA: 0x0004A1AC File Offset: 0x000485AC
			public <GetShaderInfo>c__AnonStorey0()
			{
			}

			// Token: 0x060071BD RID: 29117 RVA: 0x0004A1B4 File Offset: 0x000485B4
			internal bool <>m__0(TextAsset t)
			{
				return t.name == this.shaderName;
			}

			// Token: 0x0400659C RID: 26012
			internal string shaderName;
		}
	}
}
