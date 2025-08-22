using System;
using System.Collections.Generic;
using System.Reflection;
using GPUTools.Common.Scripts.PL.Attributes;
using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Abstract
{
	// Token: 0x020009B3 RID: 2483
	public class PrimitiveBase : IPass
	{
		// Token: 0x06003EE0 RID: 16096 RVA: 0x0012B712 File Offset: 0x00129B12
		public PrimitiveBase()
		{
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x0012B73B File Offset: 0x00129B3B
		protected void Bind()
		{
			this.CachePassAttributes();
			this.CacheOwnAttributes();
			this.BindAttributes();
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x0012B750 File Offset: 0x00129B50
		public virtual void Dispatch()
		{
			for (int i = 0; i < this.passes.Count; i++)
			{
				this.passes[i].Dispatch();
			}
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x0012B78C File Offset: 0x00129B8C
		public virtual void Dispose()
		{
			for (int i = 0; i < this.passes.Count; i++)
			{
				this.passes[i].Dispose();
			}
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x0012B7C6 File Offset: 0x00129BC6
		public void AddPass(IPass pass)
		{
			this.passes.Add(pass);
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x0012B7D4 File Offset: 0x00129BD4
		public void RemovePass(IPass pass)
		{
			if (!this.passes.Contains(pass))
			{
				Debug.LogError("Can't find pass");
				return;
			}
			this.passes.Remove(pass);
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x0012B800 File Offset: 0x00129C00
		private void CachePassAttributes()
		{
			this.passesAttributes.Clear();
			for (int i = 0; i < this.passes.Count; i++)
			{
				IPass pass = this.passes[i];
				PropertyInfo[] properties = pass.GetType().GetProperties();
				List<KeyValuePair<GpuData, PropertyInfo>> list = new List<KeyValuePair<GpuData, PropertyInfo>>();
				this.passesAttributes.Add(list);
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (Attribute.IsDefined(propertyInfo, typeof(GpuData)))
					{
						GpuData key = (GpuData)Attribute.GetCustomAttribute(propertyInfo, typeof(GpuData));
						list.Add(new KeyValuePair<GpuData, PropertyInfo>(key, propertyInfo));
					}
				}
			}
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x0012B8C0 File Offset: 0x00129CC0
		private void CacheOwnAttributes()
		{
			this.ownAttributes.Clear();
			foreach (PropertyInfo propertyInfo in base.GetType().GetProperties())
			{
				if (Attribute.IsDefined(propertyInfo, typeof(GpuData)))
				{
					GpuData key = (GpuData)Attribute.GetCustomAttribute(propertyInfo, typeof(GpuData));
					this.ownAttributes.Add(new KeyValuePair<GpuData, PropertyInfo>(key, propertyInfo));
				}
			}
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x0012B940 File Offset: 0x00129D40
		protected void BindAttributes()
		{
			for (int i = 0; i < this.ownAttributes.Count; i++)
			{
				KeyValuePair<GpuData, PropertyInfo> keyValuePair = this.ownAttributes[i];
				for (int j = 0; j < this.passesAttributes.Count; j++)
				{
					List<KeyValuePair<GpuData, PropertyInfo>> list = this.passesAttributes[j];
					for (int k = 0; k < list.Count; k++)
					{
						KeyValuePair<GpuData, PropertyInfo> keyValuePair2 = list[k];
						if (keyValuePair2.Key.Name.Equals(keyValuePair.Key.Name))
						{
							keyValuePair2.Value.SetValue(this.passes[j], keyValuePair.Value.GetValue(this, null), null);
						}
					}
				}
			}
		}

		// Token: 0x04002FDF RID: 12255
		private readonly List<IPass> passes = new List<IPass>();

		// Token: 0x04002FE0 RID: 12256
		private readonly List<List<KeyValuePair<GpuData, PropertyInfo>>> passesAttributes = new List<List<KeyValuePair<GpuData, PropertyInfo>>>();

		// Token: 0x04002FE1 RID: 12257
		private readonly List<KeyValuePair<GpuData, PropertyInfo>> ownAttributes = new List<KeyValuePair<GpuData, PropertyInfo>>();
	}
}
