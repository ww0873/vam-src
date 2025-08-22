using System;
using System.Runtime.InteropServices;

namespace Oculus.Platform
{
	// Token: 0x02000879 RID: 2169
	public sealed class Packet : IDisposable
	{
		// Token: 0x06003724 RID: 14116 RVA: 0x0010D32E File Offset: 0x0010B72E
		public Packet(IntPtr packetHandle)
		{
			this.packetHandle = packetHandle;
			this.size = (ulong)CAPI.ovr_Packet_GetSize(packetHandle);
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x0010D350 File Offset: 0x0010B750
		public ulong ReadBytes(byte[] destination)
		{
			if (destination.LongLength < (long)this.size)
			{
				throw new ArgumentException(string.Format("Destination array was not big enough to hold {0} bytes", this.size));
			}
			Marshal.Copy(CAPI.ovr_Packet_GetBytes(this.packetHandle), destination, 0, (int)this.size);
			return this.size;
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06003726 RID: 14118 RVA: 0x0010D3A8 File Offset: 0x0010B7A8
		public ulong SenderID
		{
			get
			{
				return CAPI.ovr_Packet_GetSenderID(this.packetHandle);
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06003727 RID: 14119 RVA: 0x0010D3B5 File Offset: 0x0010B7B5
		public ulong Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06003728 RID: 14120 RVA: 0x0010D3BD File Offset: 0x0010B7BD
		public SendPolicy Policy
		{
			get
			{
				return CAPI.ovr_Packet_GetSendPolicy(this.packetHandle);
			}
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x0010D3CC File Offset: 0x0010B7CC
		~Packet()
		{
			this.Dispose();
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x0010D3FC File Offset: 0x0010B7FC
		public void Dispose()
		{
			CAPI.ovr_Packet_Free(this.packetHandle);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400288E RID: 10382
		private readonly ulong size;

		// Token: 0x0400288F RID: 10383
		private readonly IntPtr packetHandle;
	}
}
