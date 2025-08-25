using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000881 RID: 2177
	public static class Matchmaking
	{
		// Token: 0x06003750 RID: 14160 RVA: 0x0010D998 File Offset: 0x0010BD98
		public static Request ReportResultsInsecure(ulong roomID, Dictionary<string, int> data)
		{
			if (Core.IsInitialized())
			{
				CAPI.ovrKeyValuePair[] array = new CAPI.ovrKeyValuePair[data.Count];
				int num = 0;
				foreach (KeyValuePair<string, int> keyValuePair in data)
				{
					array[num++] = new CAPI.ovrKeyValuePair(keyValuePair.Key, keyValuePair.Value);
				}
				return new Request(CAPI.ovr_Matchmaking_ReportResultInsecure(roomID, array));
			}
			return null;
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x0010DA34 File Offset: 0x0010BE34
		public static void SetMatchFoundNotificationCallback(Message<Room>.Callback callback)
		{
			Callback.SetNotificationCallback<Room>(Message.MessageType.Notification_Matchmaking_MatchFound, callback);
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x0010DA41 File Offset: 0x0010BE41
		public static Request<MatchmakingStats> GetStats(string pool, uint maxLevel, MatchmakingStatApproach approach = MatchmakingStatApproach.Trailing)
		{
			if (Core.IsInitialized())
			{
				return new Request<MatchmakingStats>(CAPI.ovr_Matchmaking_GetStats(pool, maxLevel, approach));
			}
			return null;
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x0010DA5C File Offset: 0x0010BE5C
		public static Request<MatchmakingBrowseResult> Browse(string pool, Matchmaking.CustomQuery customQueryData = null)
		{
			if (Core.IsInitialized())
			{
				return new Request<MatchmakingBrowseResult>(CAPI.ovr_Matchmaking_Browse(pool, (customQueryData == null) ? IntPtr.Zero : customQueryData.ToUnmanaged()));
			}
			return null;
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x0010DA8B File Offset: 0x0010BE8B
		public static Request<MatchmakingBrowseResult> Browse2(string pool, MatchmakingOptions matchmakingOptions = null)
		{
			if (Core.IsInitialized())
			{
				return new Request<MatchmakingBrowseResult>(CAPI.ovr_Matchmaking_Browse2(pool, (IntPtr)matchmakingOptions));
			}
			return null;
		}

		// Token: 0x06003755 RID: 14165 RVA: 0x0010DAAA File Offset: 0x0010BEAA
		public static Request Cancel(string pool, string requestHash)
		{
			if (Core.IsInitialized())
			{
				return new Request(CAPI.ovr_Matchmaking_Cancel(pool, requestHash));
			}
			return null;
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x0010DAC4 File Offset: 0x0010BEC4
		public static Request Cancel()
		{
			if (Core.IsInitialized())
			{
				return new Request(CAPI.ovr_Matchmaking_Cancel2());
			}
			return null;
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x0010DADC File Offset: 0x0010BEDC
		public static Request<MatchmakingEnqueueResultAndRoom> CreateAndEnqueueRoom(string pool, uint maxUsers, bool subscribeToUpdates = false, Matchmaking.CustomQuery customQueryData = null)
		{
			if (Core.IsInitialized())
			{
				return new Request<MatchmakingEnqueueResultAndRoom>(CAPI.ovr_Matchmaking_CreateAndEnqueueRoom(pool, maxUsers, subscribeToUpdates, (customQueryData == null) ? IntPtr.Zero : customQueryData.ToUnmanaged()));
			}
			return null;
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x0010DB0D File Offset: 0x0010BF0D
		public static Request<MatchmakingEnqueueResultAndRoom> CreateAndEnqueueRoom2(string pool, MatchmakingOptions matchmakingOptions = null)
		{
			if (Core.IsInitialized())
			{
				return new Request<MatchmakingEnqueueResultAndRoom>(CAPI.ovr_Matchmaking_CreateAndEnqueueRoom2(pool, (IntPtr)matchmakingOptions));
			}
			return null;
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x0010DB2C File Offset: 0x0010BF2C
		public static Request<Room> CreateRoom(string pool, uint maxUsers, bool subscribeToUpdates = false)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Matchmaking_CreateRoom(pool, maxUsers, subscribeToUpdates));
			}
			return null;
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x0010DB47 File Offset: 0x0010BF47
		public static Request<Room> CreateRoom2(string pool, MatchmakingOptions matchmakingOptions = null)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Matchmaking_CreateRoom2(pool, (IntPtr)matchmakingOptions));
			}
			return null;
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x0010DB66 File Offset: 0x0010BF66
		public static Request<MatchmakingEnqueueResult> Enqueue(string pool, Matchmaking.CustomQuery customQueryData = null)
		{
			if (Core.IsInitialized())
			{
				return new Request<MatchmakingEnqueueResult>(CAPI.ovr_Matchmaking_Enqueue(pool, (customQueryData == null) ? IntPtr.Zero : customQueryData.ToUnmanaged()));
			}
			return null;
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x0010DB95 File Offset: 0x0010BF95
		public static Request<MatchmakingEnqueueResult> Enqueue2(string pool, MatchmakingOptions matchmakingOptions = null)
		{
			if (Core.IsInitialized())
			{
				return new Request<MatchmakingEnqueueResult>(CAPI.ovr_Matchmaking_Enqueue2(pool, (IntPtr)matchmakingOptions));
			}
			return null;
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x0010DBB4 File Offset: 0x0010BFB4
		public static Request<MatchmakingEnqueueResult> EnqueueRoom(ulong roomID, Matchmaking.CustomQuery customQueryData = null)
		{
			if (Core.IsInitialized())
			{
				return new Request<MatchmakingEnqueueResult>(CAPI.ovr_Matchmaking_EnqueueRoom(roomID, (customQueryData == null) ? IntPtr.Zero : customQueryData.ToUnmanaged()));
			}
			return null;
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x0010DBE3 File Offset: 0x0010BFE3
		public static Request<MatchmakingEnqueueResult> EnqueueRoom2(ulong roomID, MatchmakingOptions matchmakingOptions = null)
		{
			if (Core.IsInitialized())
			{
				return new Request<MatchmakingEnqueueResult>(CAPI.ovr_Matchmaking_EnqueueRoom2(roomID, (IntPtr)matchmakingOptions));
			}
			return null;
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x0010DC02 File Offset: 0x0010C002
		public static Request<MatchmakingAdminSnapshot> GetAdminSnapshot()
		{
			if (Core.IsInitialized())
			{
				return new Request<MatchmakingAdminSnapshot>(CAPI.ovr_Matchmaking_GetAdminSnapshot());
			}
			return null;
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x0010DC1A File Offset: 0x0010C01A
		public static Request<Room> JoinRoom(ulong roomID, bool subscribeToUpdates = false)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Matchmaking_JoinRoom(roomID, subscribeToUpdates));
			}
			return null;
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x0010DC34 File Offset: 0x0010C034
		public static Request StartMatch(ulong roomID)
		{
			if (Core.IsInitialized())
			{
				return new Request(CAPI.ovr_Matchmaking_StartMatch(roomID));
			}
			return null;
		}

		// Token: 0x02000882 RID: 2178
		public class CustomQuery
		{
			// Token: 0x06003762 RID: 14178 RVA: 0x0010DC4D File Offset: 0x0010C04D
			public CustomQuery()
			{
			}

			// Token: 0x06003763 RID: 14179 RVA: 0x0010DC58 File Offset: 0x0010C058
			public IntPtr ToUnmanaged()
			{
				CAPI.ovrMatchmakingCustomQueryData ovrMatchmakingCustomQueryData = default(CAPI.ovrMatchmakingCustomQueryData);
				if (this.criteria != null && this.criteria.Length > 0)
				{
					ovrMatchmakingCustomQueryData.criterionArrayCount = (uint)this.criteria.Length;
					CAPI.ovrMatchmakingCriterion[] array = new CAPI.ovrMatchmakingCriterion[this.criteria.Length];
					for (int i = 0; i < this.criteria.Length; i++)
					{
						array[i].importance_ = this.criteria[i].importance;
						array[i].key_ = this.criteria[i].key;
						if (this.criteria[i].parameters != null && this.criteria[i].parameters.Count > 0)
						{
							array[i].parameterArrayCount = (uint)this.criteria[i].parameters.Count;
							array[i].parameterArray = CAPI.ArrayOfStructsToIntPtr(CAPI.DictionaryToOVRKeyValuePairs(this.criteria[i].parameters));
						}
						else
						{
							array[i].parameterArrayCount = 0U;
							array[i].parameterArray = IntPtr.Zero;
						}
					}
					ovrMatchmakingCustomQueryData.criterionArray = CAPI.ArrayOfStructsToIntPtr(array);
				}
				else
				{
					ovrMatchmakingCustomQueryData.criterionArrayCount = 0U;
					ovrMatchmakingCustomQueryData.criterionArray = IntPtr.Zero;
				}
				if (this.data != null && this.data.Count > 0)
				{
					ovrMatchmakingCustomQueryData.dataArrayCount = (uint)this.data.Count;
					ovrMatchmakingCustomQueryData.dataArray = CAPI.ArrayOfStructsToIntPtr(CAPI.DictionaryToOVRKeyValuePairs(this.data));
				}
				else
				{
					ovrMatchmakingCustomQueryData.dataArrayCount = 0U;
					ovrMatchmakingCustomQueryData.dataArray = IntPtr.Zero;
				}
				IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(ovrMatchmakingCustomQueryData));
				Marshal.StructureToPtr(ovrMatchmakingCustomQueryData, intPtr, true);
				return intPtr;
			}

			// Token: 0x040028A0 RID: 10400
			public Dictionary<string, object> data;

			// Token: 0x040028A1 RID: 10401
			public Matchmaking.CustomQuery.Criterion[] criteria;

			// Token: 0x02000883 RID: 2179
			public struct Criterion
			{
				// Token: 0x06003764 RID: 14180 RVA: 0x0010DE37 File Offset: 0x0010C237
				public Criterion(string key_, MatchmakingCriterionImportance importance_)
				{
					this.key = key_;
					this.importance = importance_;
					this.parameters = null;
				}

				// Token: 0x040028A2 RID: 10402
				public string key;

				// Token: 0x040028A3 RID: 10403
				public MatchmakingCriterionImportance importance;

				// Token: 0x040028A4 RID: 10404
				public Dictionary<string, object> parameters;
			}
		}
	}
}
