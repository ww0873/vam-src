using System;
using System.Runtime.InteropServices;

// Token: 0x020008CE RID: 2254
public class OVRNativeBuffer : IDisposable
{
	// Token: 0x060038B8 RID: 14520 RVA: 0x001145FE File Offset: 0x001129FE
	public OVRNativeBuffer(int numBytes)
	{
		this.Reallocate(numBytes);
	}

	// Token: 0x060038B9 RID: 14521 RVA: 0x00114618 File Offset: 0x00112A18
	~OVRNativeBuffer()
	{
		this.Dispose(false);
	}

	// Token: 0x060038BA RID: 14522 RVA: 0x00114648 File Offset: 0x00112A48
	public void Reset(int numBytes)
	{
		this.Reallocate(numBytes);
	}

	// Token: 0x060038BB RID: 14523 RVA: 0x00114651 File Offset: 0x00112A51
	public int GetCapacity()
	{
		return this.m_numBytes;
	}

	// Token: 0x060038BC RID: 14524 RVA: 0x00114659 File Offset: 0x00112A59
	public IntPtr GetPointer(int byteOffset = 0)
	{
		if (byteOffset < 0 || byteOffset >= this.m_numBytes)
		{
			return IntPtr.Zero;
		}
		return (byteOffset != 0) ? new IntPtr(this.m_ptr.ToInt64() + (long)byteOffset) : this.m_ptr;
	}

	// Token: 0x060038BD RID: 14525 RVA: 0x00114698 File Offset: 0x00112A98
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x060038BE RID: 14526 RVA: 0x001146A7 File Offset: 0x00112AA7
	private void Dispose(bool disposing)
	{
		if (this.disposed)
		{
			return;
		}
		if (disposing)
		{
		}
		this.Release();
		this.disposed = true;
	}

	// Token: 0x060038BF RID: 14527 RVA: 0x001146C8 File Offset: 0x00112AC8
	private void Reallocate(int numBytes)
	{
		this.Release();
		if (numBytes > 0)
		{
			this.m_ptr = Marshal.AllocHGlobal(numBytes);
			this.m_numBytes = numBytes;
		}
		else
		{
			this.m_ptr = IntPtr.Zero;
			this.m_numBytes = 0;
		}
	}

	// Token: 0x060038C0 RID: 14528 RVA: 0x00114701 File Offset: 0x00112B01
	private void Release()
	{
		if (this.m_ptr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.m_ptr);
			this.m_ptr = IntPtr.Zero;
			this.m_numBytes = 0;
		}
	}

	// Token: 0x040029E4 RID: 10724
	private bool disposed;

	// Token: 0x040029E5 RID: 10725
	private int m_numBytes;

	// Token: 0x040029E6 RID: 10726
	private IntPtr m_ptr = IntPtr.Zero;
}
