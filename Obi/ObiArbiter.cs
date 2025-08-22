using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Obi
{
	// Token: 0x020003E6 RID: 998
	public class ObiArbiter
	{
		// Token: 0x0600193A RID: 6458 RVA: 0x0008B9E5 File Offset: 0x00089DE5
		public ObiArbiter()
		{
		}

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x0600193B RID: 6459 RVA: 0x0008B9F0 File Offset: 0x00089DF0
		// (remove) Token: 0x0600193C RID: 6460 RVA: 0x0008BA24 File Offset: 0x00089E24
		public static event EventHandler OnFrameStart
		{
			add
			{
				EventHandler eventHandler = ObiArbiter.OnFrameStart;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref ObiArbiter.OnFrameStart, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = ObiArbiter.OnFrameStart;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref ObiArbiter.OnFrameStart, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x0600193D RID: 6461 RVA: 0x0008BA58 File Offset: 0x00089E58
		// (remove) Token: 0x0600193E RID: 6462 RVA: 0x0008BA8C File Offset: 0x00089E8C
		public static event EventHandler OnFrameEnd
		{
			add
			{
				EventHandler eventHandler = ObiArbiter.OnFrameEnd;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref ObiArbiter.OnFrameEnd, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = ObiArbiter.OnFrameEnd;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref ObiArbiter.OnFrameEnd, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x0008BAC0 File Offset: 0x00089EC0
		public static void RegisterSolver(ObiSolver solver)
		{
			if (solver != null)
			{
				ObiArbiter.solvers.Add(solver);
			}
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x0008BAD9 File Offset: 0x00089ED9
		public static void UnregisterSolver(ObiSolver solver)
		{
			if (solver != null)
			{
				ObiArbiter.solvers.Remove(solver);
			}
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x0008BAF3 File Offset: 0x00089EF3
		public static void FrameStart()
		{
			if (!ObiArbiter.frameStarted)
			{
				ObiArbiter.frameStarted = true;
				if (ObiArbiter.OnFrameStart != null)
				{
					ObiArbiter.OnFrameStart(null, null);
				}
				Oni.SignalFrameStart();
			}
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x0008BB20 File Offset: 0x00089F20
		public static double FrameEnd()
		{
			return Oni.SignalFrameEnd();
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x0008BB28 File Offset: 0x00089F28
		public static Oni.ProfileInfo[] GetProfileInfo()
		{
			int profilingInfoCount = Oni.GetProfilingInfoCount();
			Oni.ProfileInfo[] array = new Oni.ProfileInfo[profilingInfoCount];
			Oni.GetProfilingInfo(array, profilingInfoCount);
			return array;
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x0008BB4C File Offset: 0x00089F4C
		public static void WaitForAllSolvers()
		{
			ObiArbiter.solverCounter++;
			if (ObiArbiter.solverCounter >= ObiArbiter.solvers.Count)
			{
				ObiArbiter.solverCounter = 0;
				Oni.WaitForAllTasks();
				ObiArbiter.stepCounter--;
				if (ObiArbiter.stepCounter <= 0)
				{
					ObiProfiler.frameDuration = ObiArbiter.FrameEnd();
					ObiProfiler.info = ObiArbiter.GetProfileInfo();
					ObiArbiter.stepCounter = ObiArbiter.profileThrottle;
				}
				if (ObiArbiter.OnFrameEnd != null)
				{
					ObiArbiter.OnFrameEnd(null, null);
				}
				foreach (ObiSolver obiSolver in ObiArbiter.solvers)
				{
					obiSolver.AllSolversStepEnd();
				}
				ObiArbiter.frameStarted = false;
			}
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x0008BC24 File Offset: 0x0008A024
		// Note: this type is marked as 'beforefieldinit'.
		static ObiArbiter()
		{
		}

		// Token: 0x04001496 RID: 5270
		private static List<ObiSolver> solvers = new List<ObiSolver>();

		// Token: 0x04001497 RID: 5271
		private static int solverCounter = 0;

		// Token: 0x04001498 RID: 5272
		private static int profileThrottle = 30;

		// Token: 0x04001499 RID: 5273
		private static int stepCounter = 0;

		// Token: 0x0400149A RID: 5274
		private static bool frameStarted = false;

		// Token: 0x0400149B RID: 5275
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler OnFrameStart;

		// Token: 0x0400149C RID: 5276
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler OnFrameEnd;
	}
}
