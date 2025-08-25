using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200020C RID: 524
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentMaterial : PersistentObject
	{
		// Token: 0x06000A87 RID: 2695 RVA: 0x00040FD0 File Offset: 0x0003F3D0
		public PersistentMaterial()
		{
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00040FD8 File Offset: 0x0003F3D8
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Material material = (Material)obj;
			material.shader = (Shader)objects.Get(this.shader);
			if (material.HasProperty("_Color"))
			{
				material.color = this.color;
			}
			if (material.HasProperty("_MainTex"))
			{
				material.mainTexture = (Texture)objects.Get(this.mainTexture);
				material.mainTextureOffset = this.mainTextureOffset;
				material.mainTextureScale = this.mainTextureScale;
			}
			material.renderQueue = this.renderQueue;
			material.shaderKeywords = this.shaderKeywords;
			material.globalIlluminationFlags = (MaterialGlobalIlluminationFlags)this.globalIlluminationFlags;
			material.enableInstancing = this.enableInstancing;
			if (this.m_keywords != null)
			{
				foreach (string keyword in this.m_keywords)
				{
					material.EnableKeyword(keyword);
				}
			}
			if (this.m_propertyNames != null)
			{
				for (int j = 0; j < this.m_propertyNames.Length; j++)
				{
					string name = this.m_propertyNames[j];
					switch (this.m_propertyTypes[j])
					{
					case RTShaderPropertyType.Color:
						if (this.m_propertyValues[j].AsPrimitive.ValueBase is Color)
						{
							material.SetColor(name, (Color)this.m_propertyValues[j].AsPrimitive.ValueBase);
						}
						break;
					case RTShaderPropertyType.Vector:
						if (this.m_propertyValues[j].AsPrimitive.ValueBase is Vector4)
						{
							material.SetVector(name, (Vector4)this.m_propertyValues[j].AsPrimitive.ValueBase);
						}
						break;
					case RTShaderPropertyType.Float:
						if (this.m_propertyValues[j].AsPrimitive.ValueBase is float)
						{
							material.SetFloat(name, (float)this.m_propertyValues[j].AsPrimitive.ValueBase);
						}
						break;
					case RTShaderPropertyType.Range:
						if (this.m_propertyValues[j].AsPrimitive.ValueBase is float)
						{
							material.SetFloat(name, (float)this.m_propertyValues[j].AsPrimitive.ValueBase);
						}
						break;
					case RTShaderPropertyType.TexEnv:
						if (this.m_propertyValues[j].AsPrimitive.ValueBase is long)
						{
							material.SetTexture(name, objects.Get((long)this.m_propertyValues[j].AsPrimitive.ValueBase) as Texture);
						}
						break;
					}
				}
			}
			return material;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00041290 File Offset: 0x0003F690
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.shader, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.mainTexture, dependencies, objects, allowNulls);
			if (this.m_propertyValues != null)
			{
				for (int i = 0; i < this.m_propertyValues.Length; i++)
				{
					RTShaderPropertyType rtshaderPropertyType = this.m_propertyTypes[i];
					if (rtshaderPropertyType == RTShaderPropertyType.TexEnv)
					{
						if (this.m_propertyValues[i].AsPrimitive.ValueBase is long)
						{
							base.AddDependency<T>((long)this.m_propertyValues[i].AsPrimitive.ValueBase, dependencies, objects, allowNulls);
						}
					}
				}
			}
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00041340 File Offset: 0x0003F740
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Material material = (Material)obj;
			base.AddDependency(material.shader, dependencies);
			if (material.HasProperty("_MainTex"))
			{
				base.AddDependency(material.mainTexture, dependencies);
			}
			RuntimeShaderInfo runtimeShaderInfo = null;
			IRuntimeShaderUtil shaderUtil = Dependencies.ShaderUtil;
			if (shaderUtil != null)
			{
				runtimeShaderInfo = shaderUtil.GetShaderInfo(material.shader);
			}
			if (runtimeShaderInfo == null)
			{
				return;
			}
			for (int i = 0; i < runtimeShaderInfo.PropertyCount; i++)
			{
				string name = runtimeShaderInfo.PropertyNames[i];
				RTShaderPropertyType rtshaderPropertyType = runtimeShaderInfo.PropertyTypes[i];
				if (rtshaderPropertyType == RTShaderPropertyType.TexEnv)
				{
					base.AddDependency(material.GetTexture(name), dependencies);
				}
			}
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x000413FC File Offset: 0x0003F7FC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Material material = (Material)obj;
			this.shader = material.shader.GetMappedInstanceID();
			if (material.HasProperty("_Color"))
			{
				this.color = material.color;
			}
			if (material.HasProperty("_MainTex"))
			{
				this.mainTexture = material.mainTexture.GetMappedInstanceID();
				this.mainTextureOffset = material.mainTextureOffset;
				this.mainTextureScale = material.mainTextureScale;
			}
			this.renderQueue = material.renderQueue;
			this.shaderKeywords = material.shaderKeywords;
			this.globalIlluminationFlags = (uint)material.globalIlluminationFlags;
			this.enableInstancing = material.enableInstancing;
			if (material.shader == null)
			{
				return;
			}
			RuntimeShaderInfo runtimeShaderInfo = null;
			IRuntimeShaderUtil shaderUtil = Dependencies.ShaderUtil;
			if (shaderUtil != null)
			{
				runtimeShaderInfo = shaderUtil.GetShaderInfo(material.shader);
			}
			if (runtimeShaderInfo == null)
			{
				return;
			}
			this.m_propertyNames = new string[runtimeShaderInfo.PropertyCount];
			this.m_propertyTypes = new RTShaderPropertyType[runtimeShaderInfo.PropertyCount];
			this.m_propertyValues = new DataContract[runtimeShaderInfo.PropertyCount];
			for (int i = 0; i < runtimeShaderInfo.PropertyCount; i++)
			{
				string text = runtimeShaderInfo.PropertyNames[i];
				RTShaderPropertyType rtshaderPropertyType = runtimeShaderInfo.PropertyTypes[i];
				this.m_propertyNames[i] = text;
				this.m_propertyTypes[i] = rtshaderPropertyType;
				switch (rtshaderPropertyType)
				{
				case RTShaderPropertyType.Color:
					this.m_propertyValues[i] = new DataContract(PrimitiveContract.Create<Color>(material.GetColor(text)));
					break;
				case RTShaderPropertyType.Vector:
					this.m_propertyValues[i] = new DataContract(PrimitiveContract.Create<Vector4>(material.GetVector(text)));
					break;
				case RTShaderPropertyType.Float:
					this.m_propertyValues[i] = new DataContract(PrimitiveContract.Create<float>(material.GetFloat(text)));
					break;
				case RTShaderPropertyType.Range:
					this.m_propertyValues[i] = new DataContract(PrimitiveContract.Create<float>(material.GetFloat(text)));
					break;
				case RTShaderPropertyType.TexEnv:
					this.m_propertyValues[i] = new DataContract(PrimitiveContract.Create<long>(material.GetTexture(text).GetMappedInstanceID()));
					break;
				case RTShaderPropertyType.Unknown:
					this.m_propertyValues[i] = null;
					break;
				}
			}
			this.m_keywords = material.shaderKeywords;
		}

		// Token: 0x04000BBC RID: 3004
		public RTShaderPropertyType[] m_propertyTypes;

		// Token: 0x04000BBD RID: 3005
		public string[] m_propertyNames;

		// Token: 0x04000BBE RID: 3006
		public DataContract[] m_propertyValues;

		// Token: 0x04000BBF RID: 3007
		public string[] m_keywords;

		// Token: 0x04000BC0 RID: 3008
		public long shader;

		// Token: 0x04000BC1 RID: 3009
		public Color color;

		// Token: 0x04000BC2 RID: 3010
		public long mainTexture;

		// Token: 0x04000BC3 RID: 3011
		public Vector2 mainTextureOffset;

		// Token: 0x04000BC4 RID: 3012
		public Vector2 mainTextureScale;

		// Token: 0x04000BC5 RID: 3013
		public int renderQueue;

		// Token: 0x04000BC6 RID: 3014
		public string[] shaderKeywords;

		// Token: 0x04000BC7 RID: 3015
		public uint globalIlluminationFlags;

		// Token: 0x04000BC8 RID: 3016
		public bool enableInstancing;
	}
}
