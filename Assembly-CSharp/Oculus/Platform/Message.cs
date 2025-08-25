using System;

namespace Oculus.Platform
{
	// Token: 0x020007FC RID: 2044
	public abstract class Message<T> : Message
	{
		// Token: 0x060035D3 RID: 13779 RVA: 0x0010B368 File Offset: 0x00109768
		public Message(IntPtr c_message) : base(c_message)
		{
			if (!base.IsError)
			{
				this.data = this.GetDataFromMessage(c_message);
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060035D4 RID: 13780 RVA: 0x0010B389 File Offset: 0x00109789
		public T Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x060035D5 RID: 13781
		protected abstract T GetDataFromMessage(IntPtr c_message);

		// Token: 0x04002765 RID: 10085
		private T data;

		// Token: 0x020007FD RID: 2045
		// (Invoke) Token: 0x060035D7 RID: 13783
		public new delegate void Callback(Message<T> message);
	}
}
