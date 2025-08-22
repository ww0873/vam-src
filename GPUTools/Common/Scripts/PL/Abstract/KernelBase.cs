using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Utils;
using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Abstract
{
	// Token: 0x020009B2 RID: 2482
	public class KernelBase : IPass
	{
		// Token: 0x06003ED3 RID: 16083 RVA: 0x0012B0BC File Offset: 0x001294BC
		public KernelBase(string shaderPath, string kernelName)
		{
			this.Shader = Resources.Load<ComputeShader>(shaderPath);
			this.KernalName = kernelName;
			this.KernelId = this.Shader.FindKernel(kernelName);
			this.IsEnabled = true;
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06003ED4 RID: 16084 RVA: 0x0012B111 File Offset: 0x00129511
		// (set) Token: 0x06003ED5 RID: 16085 RVA: 0x0012B119 File Offset: 0x00129519
		public bool IsEnabled
		{
			[CompilerGenerated]
			get
			{
				return this.<IsEnabled>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsEnabled>k__BackingField = value;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06003ED6 RID: 16086 RVA: 0x0012B122 File Offset: 0x00129522
		// (set) Token: 0x06003ED7 RID: 16087 RVA: 0x0012B12A File Offset: 0x0012952A
		private protected ComputeShader Shader
		{
			[CompilerGenerated]
			protected get
			{
				return this.<Shader>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Shader>k__BackingField = value;
			}
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x0012B134 File Offset: 0x00129534
		public virtual void Dispatch()
		{
			if (!this.IsEnabled)
			{
				return;
			}
			if (this.Props.Count == 0)
			{
				this.CacheAttributes();
			}
			this.BindAttributes();
			int groupsNumX = this.GetGroupsNumX();
			int groupsNumY = this.GetGroupsNumY();
			int groupsNumZ = this.GetGroupsNumZ();
			if (groupsNumX != 0 && groupsNumY != 0 && groupsNumZ != 0)
			{
				this.Shader.Dispatch(this.KernelId, groupsNumX, groupsNumY, groupsNumZ);
			}
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x0012B1A4 File Offset: 0x001295A4
		public virtual void Dispose()
		{
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x0012B1A6 File Offset: 0x001295A6
		public virtual int GetGroupsNumX()
		{
			return 1;
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x0012B1A9 File Offset: 0x001295A9
		public virtual int GetGroupsNumY()
		{
			return 1;
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x0012B1AC File Offset: 0x001295AC
		public virtual int GetGroupsNumZ()
		{
			return 1;
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x0012B1AF File Offset: 0x001295AF
		public void ClearCacheAttributes()
		{
			this.CacheAttributes();
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x0012B1B8 File Offset: 0x001295B8
		protected virtual void CacheAttributes()
		{
			this.Props.Clear();
			this.BufferToLengthAttributeName.Clear();
			PropertyInfo[] properties = base.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (Attribute.IsDefined(propertyInfo, typeof(GpuData)))
				{
					GpuData gpuData = (GpuData)Attribute.GetCustomAttribute(propertyInfo, typeof(GpuData));
					object value = propertyInfo.GetValue(this, null);
					if (value is IBufferWrapper)
					{
						this.BufferToLengthAttributeName.Add(value as IBufferWrapper, gpuData.Name + "Length");
					}
					this.Props.Add(new KeyValuePair<GpuData, object>(gpuData, value));
				}
			}
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x0012B280 File Offset: 0x00129680
		protected void BindAttributes()
		{
			for (int i = 0; i < this.Props.Count; i++)
			{
				GpuData key = this.Props[i].Key;
				object value = this.Props[i].Value;
				if (value is IBufferWrapper)
				{
					IBufferWrapper bufferWrapper = (IBufferWrapper)value;
					ComputeBuffer computeBuffer = bufferWrapper.ComputeBuffer;
					if (computeBuffer != null)
					{
						if (!computeBuffer.IsValid())
						{
							UnityEngine.Debug.LogError(string.Concat(new object[]
							{
								"Compute buffer ",
								computeBuffer.GetHashCode(),
								" is not valid for ",
								this.KernalName,
								" ",
								key.Name
							}));
						}
						else
						{
							this.Shader.SetBuffer(this.KernelId, key.Name, computeBuffer);
							string name;
							if (this.BufferToLengthAttributeName.TryGetValue(bufferWrapper, out name))
							{
								this.Shader.SetInt(name, computeBuffer.count);
							}
						}
					}
					else
					{
						UnityEngine.Debug.LogError("Null compute buffer for " + this.KernalName);
					}
				}
				else if (value is Texture)
				{
					this.Shader.SetTexture(this.KernelId, key.Name, (Texture)value);
				}
				else if (value is GpuValue<int>)
				{
					this.Shader.SetInt(key.Name, ((GpuValue<int>)value).Value);
				}
				else if (value is GpuValue<float>)
				{
					this.Shader.SetFloat(key.Name, ((GpuValue<float>)value).Value);
				}
				else if (value is GpuValue<Vector3>)
				{
					this.Shader.SetVector(key.Name, ((GpuValue<Vector3>)value).Value);
				}
				else if (value is GpuValue<Color>)
				{
					this.Shader.SetVector(key.Name, ((GpuValue<Color>)value).Value.ToVector());
				}
				else if (value is GpuValue<bool>)
				{
					this.Shader.SetBool(key.Name, ((GpuValue<bool>)value).Value);
				}
				else if (value is GpuValue<GpuMatrix4x4>)
				{
					this.Shader.SetFloats(key.Name, ((GpuValue<GpuMatrix4x4>)value).Value.Values);
				}
			}
		}

		// Token: 0x04002FD9 RID: 12249
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <IsEnabled>k__BackingField;

		// Token: 0x04002FDA RID: 12250
		protected readonly string KernalName;

		// Token: 0x04002FDB RID: 12251
		protected readonly int KernelId;

		// Token: 0x04002FDC RID: 12252
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ComputeShader <Shader>k__BackingField;

		// Token: 0x04002FDD RID: 12253
		protected readonly List<KeyValuePair<GpuData, object>> Props = new List<KeyValuePair<GpuData, object>>();

		// Token: 0x04002FDE RID: 12254
		protected readonly Dictionary<IBufferWrapper, string> BufferToLengthAttributeName = new Dictionary<IBufferWrapper, string>();
	}
}
