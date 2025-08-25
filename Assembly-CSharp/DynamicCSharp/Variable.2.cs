using System;

namespace DynamicCSharp
{
	// Token: 0x020002EA RID: 746
	public class Variable<T> : Variable
	{
		// Token: 0x0600119A RID: 4506 RVA: 0x000616F0 File Offset: 0x0005FAF0
		internal Variable(string name, T data) : base(name, data)
		{
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600119B RID: 4507 RVA: 0x00061700 File Offset: 0x0005FB00
		public new T Value
		{
			get
			{
				T result;
				try
				{
					result = (T)((object)this.data);
				}
				catch (InvalidCastException)
				{
					result = default(T);
				}
				return result;
			}
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00061740 File Offset: 0x0005FB40
		public static implicit operator T(Variable<T> var)
		{
			return var.Value;
		}
	}
}
