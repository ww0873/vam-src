using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Leap
{
	// Token: 0x020005BD RID: 1469
	public static class CSharpExtensions
	{
		// Token: 0x0600253F RID: 9535 RVA: 0x000D65E0 File Offset: 0x000D49E0
		public static bool NearlyEquals(this float a, float b, float epsilon = 1.1920929E-07f)
		{
			float num = Math.Abs(a);
			float num2 = Math.Abs(b);
			float num3 = Math.Abs(a - b);
			if (a == b)
			{
				return true;
			}
			if (a == 0f || b == 0f || num3 < -3.4028235E+38f)
			{
				return num3 < epsilon * float.MinValue;
			}
			return num3 / (num + num2) < epsilon;
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x000D6644 File Offset: 0x000D4A44
		public static bool HasMethod(this object objectToCheck, string methodName)
		{
			Type type = objectToCheck.GetType();
			return type.GetMethod(methodName) != null;
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x000D6665 File Offset: 0x000D4A65
		public static int indexOf(this Enum enumItem)
		{
			return Array.IndexOf(Enum.GetValues(enumItem.GetType()), enumItem);
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x000D6678 File Offset: 0x000D4A78
		public static T itemFor<T>(this int ordinal)
		{
			T[] array = (T[])Enum.GetValues(typeof(T));
			return array[ordinal];
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x000D66A1 File Offset: 0x000D4AA1
		public static void Dispatch<T>(this EventHandler<T> handler, object sender, T eventArgs) where T : EventArgs
		{
			if (handler != null)
			{
				handler(sender, eventArgs);
			}
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x000D66B4 File Offset: 0x000D4AB4
		public static void DispatchOnContext<T>(this EventHandler<T> handler, object sender, SynchronizationContext context, T eventArgs) where T : EventArgs
		{
			CSharpExtensions.<DispatchOnContext>c__AnonStorey0<T> <DispatchOnContext>c__AnonStorey = new CSharpExtensions.<DispatchOnContext>c__AnonStorey0<T>();
			<DispatchOnContext>c__AnonStorey.handler = handler;
			<DispatchOnContext>c__AnonStorey.sender = sender;
			if (<DispatchOnContext>c__AnonStorey.handler != null)
			{
				if (context != null)
				{
					SendOrPostCallback d = new SendOrPostCallback(<DispatchOnContext>c__AnonStorey.<>m__0);
					context.Post(d, eventArgs);
				}
				else
				{
					<DispatchOnContext>c__AnonStorey.handler(<DispatchOnContext>c__AnonStorey.sender, eventArgs);
				}
			}
		}

		// Token: 0x02000F8D RID: 3981
		[CompilerGenerated]
		private sealed class <DispatchOnContext>c__AnonStorey0<T> where T : EventArgs
		{
			// Token: 0x06007451 RID: 29777 RVA: 0x000D6717 File Offset: 0x000D4B17
			public <DispatchOnContext>c__AnonStorey0()
			{
			}

			// Token: 0x06007452 RID: 29778 RVA: 0x000D671F File Offset: 0x000D4B1F
			internal void <>m__0(object spc_args)
			{
				this.handler(this.sender, spc_args as T);
			}

			// Token: 0x04006861 RID: 26721
			internal EventHandler<T> handler;

			// Token: 0x04006862 RID: 26722
			internal object sender;
		}
	}
}
