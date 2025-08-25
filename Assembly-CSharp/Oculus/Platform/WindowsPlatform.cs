using System;
using System.Runtime.InteropServices;
using Oculus.Platform.Models;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x020008B0 RID: 2224
	public class WindowsPlatform
	{
		// Token: 0x060037F1 RID: 14321 RVA: 0x0010EFBC File Offset: 0x0010D3BC
		public WindowsPlatform()
		{
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x0010EFC4 File Offset: 0x0010D3C4
		private void CPPLogCallback(IntPtr tag, IntPtr message)
		{
			Debug.Log(string.Format("{0}: {1}", Marshal.PtrToStringAnsi(tag), Marshal.PtrToStringAnsi(message)));
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x0010EFE1 File Offset: 0x0010D3E1
		private IntPtr getCallbackPointer()
		{
			return IntPtr.Zero;
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x0010EFE8 File Offset: 0x0010D3E8
		public bool Initialize(string appId)
		{
			if (string.IsNullOrEmpty(appId))
			{
				throw new UnityException("AppID must not be null or empty");
			}
			CAPI.ovr_UnityInitWrapperWindows(appId, this.getCallbackPointer());
			return true;
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x0010F00E File Offset: 0x0010D40E
		public Request<PlatformInitialize> AsyncInitialize(string appId)
		{
			if (string.IsNullOrEmpty(appId))
			{
				throw new UnityException("AppID must not be null or empty");
			}
			return new Request<PlatformInitialize>(CAPI.ovr_UnityInitWrapperWindowsAsynchronous(appId, this.getCallbackPointer()));
		}

		// Token: 0x020008B1 RID: 2225
		// (Invoke) Token: 0x060037F7 RID: 14327
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void UnityLogDelegate(IntPtr tag, IntPtr msg);
	}
}
