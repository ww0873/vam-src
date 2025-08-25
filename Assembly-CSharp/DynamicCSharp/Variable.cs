using System;

namespace DynamicCSharp
{
	// Token: 0x020002E9 RID: 745
	public class Variable
	{
		// Token: 0x06001195 RID: 4501 RVA: 0x00061694 File Offset: 0x0005FA94
		internal Variable(string name, object data)
		{
			this.name = name;
			this.data = data;
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x000616B5 File Offset: 0x0005FAB5
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06001197 RID: 4503 RVA: 0x000616BD File Offset: 0x0005FABD
		public object Value
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x000616C5 File Offset: 0x0005FAC5
		internal void Update(object data)
		{
			this.data = data;
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x000616CE File Offset: 0x0005FACE
		public override string ToString()
		{
			return (this.data != null) ? this.data.ToString() : "null";
		}

		// Token: 0x04000F38 RID: 3896
		protected string name = string.Empty;

		// Token: 0x04000F39 RID: 3897
		protected object data;
	}
}
