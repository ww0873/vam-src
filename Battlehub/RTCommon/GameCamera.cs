using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000B2 RID: 178
	public class GameCamera : MonoBehaviour
	{
		// Token: 0x060002E0 RID: 736 RVA: 0x00013973 File Offset: 0x00011D73
		public GameCamera()
		{
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060002E1 RID: 737 RVA: 0x0001397C File Offset: 0x00011D7C
		// (remove) Token: 0x060002E2 RID: 738 RVA: 0x000139B0 File Offset: 0x00011DB0
		public static event GameCameraEvent Awaked
		{
			add
			{
				GameCameraEvent gameCameraEvent = GameCamera.Awaked;
				GameCameraEvent gameCameraEvent2;
				do
				{
					gameCameraEvent2 = gameCameraEvent;
					gameCameraEvent = Interlocked.CompareExchange<GameCameraEvent>(ref GameCamera.Awaked, (GameCameraEvent)Delegate.Combine(gameCameraEvent2, value), gameCameraEvent);
				}
				while (gameCameraEvent != gameCameraEvent2);
			}
			remove
			{
				GameCameraEvent gameCameraEvent = GameCamera.Awaked;
				GameCameraEvent gameCameraEvent2;
				do
				{
					gameCameraEvent2 = gameCameraEvent;
					gameCameraEvent = Interlocked.CompareExchange<GameCameraEvent>(ref GameCamera.Awaked, (GameCameraEvent)Delegate.Remove(gameCameraEvent2, value), gameCameraEvent);
				}
				while (gameCameraEvent != gameCameraEvent2);
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060002E3 RID: 739 RVA: 0x000139E4 File Offset: 0x00011DE4
		// (remove) Token: 0x060002E4 RID: 740 RVA: 0x00013A18 File Offset: 0x00011E18
		public static event GameCameraEvent Destroyed
		{
			add
			{
				GameCameraEvent gameCameraEvent = GameCamera.Destroyed;
				GameCameraEvent gameCameraEvent2;
				do
				{
					gameCameraEvent2 = gameCameraEvent;
					gameCameraEvent = Interlocked.CompareExchange<GameCameraEvent>(ref GameCamera.Destroyed, (GameCameraEvent)Delegate.Combine(gameCameraEvent2, value), gameCameraEvent);
				}
				while (gameCameraEvent != gameCameraEvent2);
			}
			remove
			{
				GameCameraEvent gameCameraEvent = GameCamera.Destroyed;
				GameCameraEvent gameCameraEvent2;
				do
				{
					gameCameraEvent2 = gameCameraEvent;
					gameCameraEvent = Interlocked.CompareExchange<GameCameraEvent>(ref GameCamera.Destroyed, (GameCameraEvent)Delegate.Remove(gameCameraEvent2, value), gameCameraEvent);
				}
				while (gameCameraEvent != gameCameraEvent2);
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060002E5 RID: 741 RVA: 0x00013A4C File Offset: 0x00011E4C
		// (remove) Token: 0x060002E6 RID: 742 RVA: 0x00013A80 File Offset: 0x00011E80
		public static event GameCameraEvent Enabled
		{
			add
			{
				GameCameraEvent gameCameraEvent = GameCamera.Enabled;
				GameCameraEvent gameCameraEvent2;
				do
				{
					gameCameraEvent2 = gameCameraEvent;
					gameCameraEvent = Interlocked.CompareExchange<GameCameraEvent>(ref GameCamera.Enabled, (GameCameraEvent)Delegate.Combine(gameCameraEvent2, value), gameCameraEvent);
				}
				while (gameCameraEvent != gameCameraEvent2);
			}
			remove
			{
				GameCameraEvent gameCameraEvent = GameCamera.Enabled;
				GameCameraEvent gameCameraEvent2;
				do
				{
					gameCameraEvent2 = gameCameraEvent;
					gameCameraEvent = Interlocked.CompareExchange<GameCameraEvent>(ref GameCamera.Enabled, (GameCameraEvent)Delegate.Remove(gameCameraEvent2, value), gameCameraEvent);
				}
				while (gameCameraEvent != gameCameraEvent2);
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060002E7 RID: 743 RVA: 0x00013AB4 File Offset: 0x00011EB4
		// (remove) Token: 0x060002E8 RID: 744 RVA: 0x00013AE8 File Offset: 0x00011EE8
		public static event GameCameraEvent Disabled
		{
			add
			{
				GameCameraEvent gameCameraEvent = GameCamera.Disabled;
				GameCameraEvent gameCameraEvent2;
				do
				{
					gameCameraEvent2 = gameCameraEvent;
					gameCameraEvent = Interlocked.CompareExchange<GameCameraEvent>(ref GameCamera.Disabled, (GameCameraEvent)Delegate.Combine(gameCameraEvent2, value), gameCameraEvent);
				}
				while (gameCameraEvent != gameCameraEvent2);
			}
			remove
			{
				GameCameraEvent gameCameraEvent = GameCamera.Disabled;
				GameCameraEvent gameCameraEvent2;
				do
				{
					gameCameraEvent2 = gameCameraEvent;
					gameCameraEvent = Interlocked.CompareExchange<GameCameraEvent>(ref GameCamera.Disabled, (GameCameraEvent)Delegate.Remove(gameCameraEvent2, value), gameCameraEvent);
				}
				while (gameCameraEvent != gameCameraEvent2);
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00013B1C File Offset: 0x00011F1C
		private void Awake()
		{
			if (GameCamera.Awaked != null)
			{
				GameCamera.Awaked();
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00013B32 File Offset: 0x00011F32
		private void OnDestroy()
		{
			if (GameCamera.Destroyed != null)
			{
				GameCamera.Destroyed();
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00013B48 File Offset: 0x00011F48
		private void OnEnable()
		{
			if (GameCamera.Enabled != null)
			{
				GameCamera.Enabled();
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00013B5E File Offset: 0x00011F5E
		private void OnDisable()
		{
			if (GameCamera.Disabled != null)
			{
				GameCamera.Disabled();
			}
		}

		// Token: 0x0400039C RID: 924
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static GameCameraEvent Awaked;

		// Token: 0x0400039D RID: 925
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static GameCameraEvent Destroyed;

		// Token: 0x0400039E RID: 926
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static GameCameraEvent Enabled;

		// Token: 0x0400039F RID: 927
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static GameCameraEvent Disabled;
	}
}
