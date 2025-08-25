using System;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000242 RID: 578
	public struct ID
	{
		// Token: 0x06000BF9 RID: 3065 RVA: 0x0004AB01 File Offset: 0x00048F01
		public ID(long id)
		{
			this.m_id = id;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0004AB0C File Offset: 0x00048F0C
		public override bool Equals(object obj)
		{
			return obj is ID && this.m_id.Equals(((ID)obj).m_id);
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0004AB3F File Offset: 0x00048F3F
		public override int GetHashCode()
		{
			return this.m_id.GetHashCode();
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0004AB52 File Offset: 0x00048F52
		public override string ToString()
		{
			return this.m_id.ToString();
		}

		// Token: 0x04000CBF RID: 3263
		private long m_id;
	}
}
