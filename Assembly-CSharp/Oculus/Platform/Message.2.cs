using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Oculus.Platform.Models;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x020007FE RID: 2046
	public class Message
	{
		// Token: 0x060035DA RID: 13786 RVA: 0x0010AA24 File Offset: 0x00108E24
		public Message(IntPtr c_message)
		{
			this.type = CAPI.ovr_Message_GetType(c_message);
			bool flag = CAPI.ovr_Message_IsError(c_message);
			this.requestID = CAPI.ovr_Message_GetRequestID(c_message);
			if (flag)
			{
				IntPtr obj = CAPI.ovr_Message_GetError(c_message);
				this.error = new Error(CAPI.ovr_Error_GetCode(obj), CAPI.ovr_Error_GetMessage(obj), CAPI.ovr_Error_GetHttpCode(obj));
			}
			else if (Core.LogMessages)
			{
				string text = CAPI.ovr_Message_GetString(c_message);
				if (text != null)
				{
					UnityEngine.Debug.Log(text);
				}
				else
				{
					UnityEngine.Debug.Log(string.Format("null message string {0}", c_message));
				}
			}
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x0010AABC File Offset: 0x00108EBC
		~Message()
		{
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060035DC RID: 13788 RVA: 0x0010AAE8 File Offset: 0x00108EE8
		public Message.MessageType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060035DD RID: 13789 RVA: 0x0010AAF0 File Offset: 0x00108EF0
		public bool IsError
		{
			get
			{
				return this.error != null;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060035DE RID: 13790 RVA: 0x0010AAFE File Offset: 0x00108EFE
		public ulong RequestID
		{
			get
			{
				return this.requestID;
			}
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x0010AB06 File Offset: 0x00108F06
		public virtual Error GetError()
		{
			return this.error;
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x0010AB0E File Offset: 0x00108F0E
		public virtual PingResult GetPingResult()
		{
			return null;
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x0010AB11 File Offset: 0x00108F11
		public virtual NetworkingPeer GetNetworkingPeer()
		{
			return null;
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x0010AB14 File Offset: 0x00108F14
		public virtual HttpTransferUpdate GetHttpTransferUpdate()
		{
			return null;
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x0010AB17 File Offset: 0x00108F17
		public virtual PlatformInitialize GetPlatformInitialize()
		{
			return null;
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x0010AB1A File Offset: 0x00108F1A
		public virtual AchievementDefinitionList GetAchievementDefinitions()
		{
			return null;
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x0010AB1D File Offset: 0x00108F1D
		public virtual AchievementProgressList GetAchievementProgressList()
		{
			return null;
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x0010AB20 File Offset: 0x00108F20
		public virtual AchievementUpdate GetAchievementUpdate()
		{
			return null;
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x0010AB23 File Offset: 0x00108F23
		public virtual ApplicationVersion GetApplicationVersion()
		{
			return null;
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x0010AB26 File Offset: 0x00108F26
		public virtual AssetFileDeleteResult GetAssetFileDeleteResult()
		{
			return null;
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x0010AB29 File Offset: 0x00108F29
		public virtual AssetFileDownloadCancelResult GetAssetFileDownloadCancelResult()
		{
			return null;
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x0010AB2C File Offset: 0x00108F2C
		public virtual AssetFileDownloadResult GetAssetFileDownloadResult()
		{
			return null;
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x0010AB2F File Offset: 0x00108F2F
		public virtual AssetFileDownloadUpdate GetAssetFileDownloadUpdate()
		{
			return null;
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x0010AB32 File Offset: 0x00108F32
		public virtual CloudStorageConflictMetadata GetCloudStorageConflictMetadata()
		{
			return null;
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x0010AB35 File Offset: 0x00108F35
		public virtual CloudStorageData GetCloudStorageData()
		{
			return null;
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x0010AB38 File Offset: 0x00108F38
		public virtual CloudStorageMetadata GetCloudStorageMetadata()
		{
			return null;
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x0010AB3B File Offset: 0x00108F3B
		public virtual CloudStorageMetadataList GetCloudStorageMetadataList()
		{
			return null;
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x0010AB3E File Offset: 0x00108F3E
		public virtual CloudStorageUpdateResponse GetCloudStorageUpdateResponse()
		{
			return null;
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x0010AB41 File Offset: 0x00108F41
		public virtual InstalledApplicationList GetInstalledApplicationList()
		{
			return null;
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x0010AB44 File Offset: 0x00108F44
		public virtual bool GetLeaderboardDidUpdate()
		{
			return false;
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x0010AB47 File Offset: 0x00108F47
		public virtual LeaderboardEntryList GetLeaderboardEntryList()
		{
			return null;
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x0010AB4A File Offset: 0x00108F4A
		public virtual LivestreamingApplicationStatus GetLivestreamingApplicationStatus()
		{
			return null;
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x0010AB4D File Offset: 0x00108F4D
		public virtual LivestreamingStartResult GetLivestreamingStartResult()
		{
			return null;
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x0010AB50 File Offset: 0x00108F50
		public virtual LivestreamingStatus GetLivestreamingStatus()
		{
			return null;
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x0010AB53 File Offset: 0x00108F53
		public virtual LivestreamingVideoStats GetLivestreamingVideoStats()
		{
			return null;
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x0010AB56 File Offset: 0x00108F56
		public virtual MatchmakingAdminSnapshot GetMatchmakingAdminSnapshot()
		{
			return null;
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x0010AB59 File Offset: 0x00108F59
		public virtual MatchmakingBrowseResult GetMatchmakingBrowseResult()
		{
			return null;
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x0010AB5C File Offset: 0x00108F5C
		public virtual MatchmakingEnqueueResult GetMatchmakingEnqueueResult()
		{
			return null;
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x0010AB5F File Offset: 0x00108F5F
		public virtual MatchmakingEnqueueResultAndRoom GetMatchmakingEnqueueResultAndRoom()
		{
			return null;
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x0010AB62 File Offset: 0x00108F62
		public virtual MatchmakingStats GetMatchmakingStats()
		{
			return null;
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x0010AB65 File Offset: 0x00108F65
		public virtual OrgScopedID GetOrgScopedID()
		{
			return null;
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x0010AB68 File Offset: 0x00108F68
		public virtual Party GetParty()
		{
			return null;
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x0010AB6B File Offset: 0x00108F6B
		public virtual PartyID GetPartyID()
		{
			return null;
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x0010AB6E File Offset: 0x00108F6E
		public virtual PidList GetPidList()
		{
			return null;
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x0010AB71 File Offset: 0x00108F71
		public virtual ProductList GetProductList()
		{
			return null;
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x0010AB74 File Offset: 0x00108F74
		public virtual Purchase GetPurchase()
		{
			return null;
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x0010AB77 File Offset: 0x00108F77
		public virtual PurchaseList GetPurchaseList()
		{
			return null;
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x0010AB7A File Offset: 0x00108F7A
		public virtual Room GetRoom()
		{
			return null;
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x0010AB7D File Offset: 0x00108F7D
		public virtual RoomInviteNotification GetRoomInviteNotification()
		{
			return null;
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x0010AB80 File Offset: 0x00108F80
		public virtual RoomInviteNotificationList GetRoomInviteNotificationList()
		{
			return null;
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x0010AB83 File Offset: 0x00108F83
		public virtual RoomList GetRoomList()
		{
			return null;
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x0010AB86 File Offset: 0x00108F86
		public virtual SdkAccountList GetSdkAccountList()
		{
			return null;
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x0010AB89 File Offset: 0x00108F89
		public virtual ShareMediaResult GetShareMediaResult()
		{
			return null;
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x0010AB8C File Offset: 0x00108F8C
		public virtual string GetString()
		{
			return null;
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x0010AB8F File Offset: 0x00108F8F
		public virtual SystemPermission GetSystemPermission()
		{
			return null;
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x0010AB92 File Offset: 0x00108F92
		public virtual SystemVoipState GetSystemVoipState()
		{
			return null;
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x0010AB95 File Offset: 0x00108F95
		public virtual User GetUser()
		{
			return null;
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x0010AB98 File Offset: 0x00108F98
		public virtual UserAndRoomList GetUserAndRoomList()
		{
			return null;
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x0010AB9B File Offset: 0x00108F9B
		public virtual UserList GetUserList()
		{
			return null;
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x0010AB9E File Offset: 0x00108F9E
		public virtual UserProof GetUserProof()
		{
			return null;
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x0010ABA1 File Offset: 0x00108FA1
		public virtual UserReportID GetUserReportID()
		{
			return null;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x0010ABA4 File Offset: 0x00108FA4
		internal static Message ParseMessageHandle(IntPtr messageHandle)
		{
			if (messageHandle.ToInt64() == 0L)
			{
				return null;
			}
			Message.MessageType messageType = CAPI.ovr_Message_GetType(messageHandle);
			Message message;
			if (messageType != Message.MessageType.Media_ShareToFacebook)
			{
				if (messageType != Message.MessageType.Room_UpdateDataStore && messageType != Message.MessageType.Matchmaking_CreateRoom)
				{
					if (messageType != Message.MessageType.Achievements_GetAllDefinitions)
					{
						if (messageType != Message.MessageType.CloudStorage_LoadMetadata)
						{
							if (messageType != Message.MessageType.Achievements_AddCount)
							{
								if (messageType != Message.MessageType.Notification_ApplicationLifecycle_LaunchIntentChanged)
								{
									if (messageType != Message.MessageType.ApplicationLifecycle_GetRegisteredPIDs)
									{
										if (messageType != Message.MessageType.Notification_GetNextRoomInviteNotificationArrayPage)
										{
											if (messageType == Message.MessageType.User_GetAccessToken)
											{
												goto IL_6B4;
											}
											if (messageType != Message.MessageType.AssetFile_DownloadCancel)
											{
												if (messageType != Message.MessageType.Room_GetModeratedRooms)
												{
													if (messageType != Message.MessageType.Room_GetCurrent)
													{
														if (messageType != Message.MessageType.User_LaunchProfile)
														{
															if (messageType == Message.MessageType.Notification_Matchmaking_MatchFound)
															{
																return new MessageWithMatchmakingNotification(messageHandle);
															}
															if (messageType == Message.MessageType.Room_GetCurrentForUser)
															{
																goto IL_660;
															}
															if (messageType != Message.MessageType.Matchmaking_Cancel2)
															{
																if (messageType == Message.MessageType.Room_UpdatePrivateRoomJoinPolicy)
																{
																	goto IL_66C;
																}
																if (messageType == Message.MessageType.AssetFile_Download)
																{
																	return new MessageWithAssetFileDownloadResult(messageHandle);
																}
																if (messageType != Message.MessageType.Leaderboard_WriteEntry)
																{
																	if (messageType != Message.MessageType.Matchmaking_Enqueue2)
																	{
																		if (messageType != Message.MessageType.Achievements_AddFields)
																		{
																			if (messageType != Message.MessageType.Achievements_GetProgressByName)
																			{
																				if (messageType != Message.MessageType.Room_Join)
																				{
																					if (messageType != Message.MessageType.Leaderboard_GetEntriesAfterRank)
																					{
																						if (messageType == Message.MessageType.Entitlement_GetIsViewerEntitled)
																						{
																							goto IL_5A0;
																						}
																						if (messageType == Message.MessageType.User_GetOrgScopedID)
																						{
																							return new MessageWithOrgScopedID(messageHandle);
																						}
																						if (messageType != Message.MessageType.Matchmaking_ReportResultInsecure)
																						{
																							if (messageType != Message.MessageType.Platform_InitializeAndroidAsynchronous)
																							{
																								if (messageType != Message.MessageType.IAP_GetNextProductArrayPage)
																								{
																									if (messageType != Message.MessageType.Room_GetInvitableUsers)
																									{
																										if (messageType != Message.MessageType.Matchmaking_Browse)
																										{
																											if (messageType != Message.MessageType.IAP_ConsumePurchase && messageType != Message.MessageType.Matchmaking_Cancel)
																											{
																												if (messageType != Message.MessageType.Notification_Livestreaming_StatusChange && messageType != Message.MessageType.Livestreaming_ResumeStream)
																												{
																													if (messageType == Message.MessageType.User_GetUserProof)
																													{
																														return new MessageWithUserProof(messageHandle);
																													}
																													if (messageType != Message.MessageType.User_GetNextUserArrayPage)
																													{
																														if (messageType != Message.MessageType.CloudStorage_Delete)
																														{
																															if (messageType != Message.MessageType.Matchmaking_CreateAndEnqueueRoom2)
																															{
																																if (messageType != Message.MessageType.User_GetLoggedInUserRecentlyMetUsersAndRooms)
																																{
																																	if (messageType == Message.MessageType.Achievements_GetNextAchievementDefinitionArrayPage)
																																	{
																																		goto IL_504;
																																	}
																																	if (messageType == Message.MessageType.Achievements_GetNextAchievementProgressArrayPage)
																																	{
																																		goto IL_510;
																																	}
																																	if (messageType == Message.MessageType.Notification_AssetFile_DownloadUpdate)
																																	{
																																		return new MessageWithAssetFileDownloadUpdate(messageHandle);
																																	}
																																	if (messageType == Message.MessageType.Room_SetDescription)
																																	{
																																		goto IL_66C;
																																	}
																																	if (messageType == Message.MessageType.CloudStorage_ResolveKeepLocal)
																																	{
																																		goto IL_594;
																																	}
																																	if (messageType != Message.MessageType.Room_LaunchInvitableUserFlow)
																																	{
																																		if (messageType != Message.MessageType.CloudStorage_LoadHandle)
																																		{
																																			if (messageType == Message.MessageType.Room_UpdateOwner)
																																			{
																																				goto IL_5A0;
																																			}
																																			if (messageType == Message.MessageType.Notification_Voip_StateChange || messageType == Message.MessageType.Notification_Voip_ConnectRequest)
																																			{
																																				return new MessageWithNetworkingPeer(messageHandle);
																																			}
																																			if (messageType == Message.MessageType.Livestreaming_PauseStream)
																																			{
																																				goto IL_5C4;
																																			}
																																			if (messageType != Message.MessageType.Room_UpdateMembershipLockStatus)
																																			{
																																				if (messageType != Message.MessageType.IAP_GetViewerPurchases)
																																				{
																																					if (messageType == Message.MessageType.ApplicationLifecycle_GetSessionKey)
																																					{
																																						goto IL_6B4;
																																					}
																																					if (messageType == Message.MessageType.Matchmaking_GetAdminSnapshot)
																																					{
																																						return new MessageWithMatchmakingAdminSnapshot(messageHandle);
																																					}
																																					if (messageType == Message.MessageType.IAP_LaunchCheckoutFlow)
																																					{
																																						return new MessageWithPurchase(messageHandle);
																																					}
																																					if (messageType == Message.MessageType.CloudStorage_Load)
																																					{
																																						goto IL_570;
																																					}
																																					if (messageType == Message.MessageType.Matchmaking_Enqueue)
																																					{
																																						goto IL_5E8;
																																					}
																																					if (messageType == Message.MessageType.Room_InviteUser)
																																					{
																																						goto IL_66C;
																																					}
																																					if (messageType != Message.MessageType.Matchmaking_GetStats)
																																					{
																																						if (messageType != Message.MessageType.User_GetLoggedInUser)
																																						{
																																							if (messageType == Message.MessageType.CloudStorage_LoadConflictMetadata)
																																							{
																																								return new MessageWithCloudStorageConflictMetadata(messageHandle);
																																							}
																																							if (messageType == Message.MessageType.Matchmaking_StartMatch)
																																							{
																																								goto IL_5A0;
																																							}
																																							if (messageType == Message.MessageType.Voip_SetSystemVoipSuppressed)
																																							{
																																								return new MessageWithSystemVoipState(messageHandle);
																																							}
																																							if (messageType == Message.MessageType.IAP_GetNextPurchaseArrayPage)
																																							{
																																								goto IL_648;
																																							}
																																							if (messageType == Message.MessageType.Party_GetCurrent)
																																							{
																																								return new MessageWithPartyUnderCurrentParty(messageHandle);
																																							}
																																							if (messageType == Message.MessageType.Livestreaming_GetStatus)
																																							{
																																								goto IL_5C4;
																																							}
																																							if (messageType == Message.MessageType.Leaderboard_GetPreviousEntries)
																																							{
																																								goto IL_5AC;
																																							}
																																							if (messageType == Message.MessageType.Matchmaking_CreateRoom2 || messageType == Message.MessageType.Room_KickUser)
																																							{
																																								goto IL_66C;
																																							}
																																							if (messageType != Message.MessageType.CloudStorage_Save)
																																							{
																																								if (messageType != Message.MessageType.Notification_Networking_PeerConnectRequest)
																																								{
																																									if (messageType == Message.MessageType.Matchmaking_JoinRoom || messageType == Message.MessageType.Room_Join2)
																																									{
																																										goto IL_66C;
																																									}
																																									if (messageType == Message.MessageType.ApplicationLifecycle_RegisterSessionKey)
																																									{
																																										goto IL_5A0;
																																									}
																																									if (messageType == Message.MessageType.Leaderboard_GetNextEntries)
																																									{
																																										goto IL_5AC;
																																									}
																																									if (messageType == Message.MessageType.Room_GetNextRoomArrayPage)
																																									{
																																										goto IL_678;
																																									}
																																									if (messageType == Message.MessageType.Room_GetInvitableUsers2)
																																									{
																																										goto IL_6E4;
																																									}
																																									if (messageType == Message.MessageType.Achievements_GetAllProgress)
																																									{
																																										goto IL_510;
																																									}
																																									if (messageType == Message.MessageType.Notification_Networking_PingResult)
																																									{
																																										return new MessageWithPingResult(messageHandle);
																																									}
																																									if (messageType == Message.MessageType.Platform_InitializeStandaloneOculus)
																																									{
																																										goto IL_744;
																																									}
																																									if (messageType == Message.MessageType.Application_LaunchOtherApp)
																																									{
																																										goto IL_6B4;
																																									}
																																									if (messageType == Message.MessageType.Matchmaking_EnqueueRoom2)
																																									{
																																										goto IL_5E8;
																																									}
																																									if (messageType == Message.MessageType.User_GetLoggedInUserFriends)
																																									{
																																										goto IL_6E4;
																																									}
																																									if (messageType == Message.MessageType.Notification_Voip_SystemVoipState)
																																									{
																																										return new MessageWithSystemVoipState(messageHandle);
																																									}
																																									if (messageType == Message.MessageType.Achievements_Unlock)
																																									{
																																										goto IL_51C;
																																									}
																																									if (messageType != Message.MessageType.Room_CreateAndJoinPrivate2)
																																									{
																																										if (messageType != Message.MessageType.CloudStorage_GetNextCloudStorageMetadataArrayPage)
																																										{
																																											if (messageType == Message.MessageType.Leaderboard_GetEntries)
																																											{
																																												goto IL_5AC;
																																											}
																																											if (messageType == Message.MessageType.Notification_Networking_ConnectionStateChange)
																																											{
																																												goto IL_6FC;
																																											}
																																											if (messageType == Message.MessageType.User_GetLoggedInUserFriendsAndRooms)
																																											{
																																												goto IL_6D8;
																																											}
																																											if (messageType == Message.MessageType.Matchmaking_CreateAndEnqueueRoom)
																																											{
																																												goto IL_5F4;
																																											}
																																											if (messageType == Message.MessageType.Notification_Room_RoomUpdate)
																																											{
																																												goto IL_66C;
																																											}
																																											if (messageType == Message.MessageType.Achievements_GetDefinitionsByName)
																																											{
																																												goto IL_504;
																																											}
																																											if (messageType == Message.MessageType.Room_Get)
																																											{
																																												return new MessageWithRoom(messageHandle);
																																											}
																																											if (messageType == Message.MessageType.Matchmaking_Browse2)
																																											{
																																												goto IL_5DC;
																																											}
																																											if (messageType == Message.MessageType.User_GetSdkAccounts)
																																											{
																																												return new MessageWithSdkAccountList(messageHandle);
																																											}
																																											if (messageType == Message.MessageType.Application_GetVersion)
																																											{
																																												return new MessageWithApplicationVersion(messageHandle);
																																											}
																																											if (messageType == Message.MessageType.Notification_Room_InviteReceived)
																																											{
																																												return new MessageWithRoomInviteNotification(messageHandle);
																																											}
																																											if (messageType == Message.MessageType.User_Get)
																																											{
																																												goto IL_6CC;
																																											}
																																											if (messageType == Message.MessageType.Notification_Room_InviteAccepted)
																																											{
																																												goto IL_6B4;
																																											}
																																											if (messageType == Message.MessageType.AssetFile_Delete)
																																											{
																																												return new MessageWithAssetFileDeleteResult(messageHandle);
																																											}
																																											if (messageType == Message.MessageType.Platform_InitializeWindowsAsynchronous)
																																											{
																																												goto IL_744;
																																											}
																																											if (messageType == Message.MessageType.Notification_GetRoomInvites)
																																											{
																																												goto IL_690;
																																											}
																																											if (messageType == Message.MessageType.Matchmaking_EnqueueRoom)
																																											{
																																												goto IL_5E8;
																																											}
																																											if (messageType == Message.MessageType.Notification_MarkAsRead)
																																											{
																																												goto IL_5A0;
																																											}
																																											if (messageType == Message.MessageType.Room_Leave)
																																											{
																																												goto IL_66C;
																																											}
																																											if (messageType != Message.MessageType.CloudStorage_LoadBucketMetadata)
																																											{
																																												if (messageType == Message.MessageType.CloudStorage_ResolveKeepRemote)
																																												{
																																													goto IL_594;
																																												}
																																												if (messageType == Message.MessageType.Room_CreateAndJoinPrivate)
																																												{
																																													goto IL_66C;
																																												}
																																												if (messageType == Message.MessageType.Notification_HTTP_Transfer)
																																												{
																																													return new MessageWithHttpTransferUpdate(messageHandle);
																																												}
																																												if (messageType == Message.MessageType.IAP_GetProductsBySKU)
																																												{
																																													goto IL_630;
																																												}
																																												if (messageType != Message.MessageType.User_GetNextUserAndRoomArrayPage)
																																												{
																																													message = PlatformInternal.ParseMessageHandle(messageHandle, messageType);
																																													if (message == null)
																																													{
																																														UnityEngine.Debug.LogError(string.Format("Unrecognized message type {0}\n", messageType));
																																													}
																																													return message;
																																												}
																																												goto IL_6D8;
																																											}
																																										}
																																										return new MessageWithCloudStorageMetadataList(messageHandle);
																																									}
																																									goto IL_66C;
																																								}
																																								IL_6FC:
																																								return new MessageWithNetworkingPeer(messageHandle);
																																							}
																																							goto IL_594;
																																						}
																																						IL_6CC:
																																						return new MessageWithUser(messageHandle);
																																					}
																																					return new MessageWithMatchmakingStatsUnderMatchmakingStats(messageHandle);
																																				}
																																				IL_648:
																																				return new MessageWithPurchaseList(messageHandle);
																																			}
																																			goto IL_66C;
																																		}
																																		IL_570:
																																		return new MessageWithCloudStorageData(messageHandle);
																																	}
																																	goto IL_5A0;
																																}
																																IL_6D8:
																																return new MessageWithUserAndRoomList(messageHandle);
																															}
																															IL_5F4:
																															return new MessageWithMatchmakingEnqueueResultAndRoom(messageHandle);
																														}
																														IL_594:
																														return new MessageWithCloudStorageUpdateResponse(messageHandle);
																													}
																													goto IL_6E4;
																												}
																												IL_5C4:
																												return new MessageWithLivestreamingStatus(messageHandle);
																											}
																											goto IL_5A0;
																										}
																										IL_5DC:
																										return new MessageWithMatchmakingBrowseResult(messageHandle);
																									}
																									IL_6E4:
																									return new MessageWithUserList(messageHandle);
																								}
																								IL_630:
																								return new MessageWithProductList(messageHandle);
																							}
																							IL_744:
																							return new MessageWithPlatformInitialize(messageHandle);
																						}
																						goto IL_5A0;
																					}
																					IL_5AC:
																					return new MessageWithLeaderboardEntryList(messageHandle);
																				}
																				goto IL_66C;
																			}
																			IL_510:
																			return new MessageWithAchievementProgressList(messageHandle);
																		}
																		goto IL_51C;
																	}
																	IL_5E8:
																	return new MessageWithMatchmakingEnqueueResult(messageHandle);
																}
																return new MessageWithLeaderboardDidUpdate(messageHandle);
															}
														}
														IL_5A0:
														return new Message(messageHandle);
													}
													IL_660:
													return new MessageWithRoomUnderCurrentRoom(messageHandle);
												}
												IL_678:
												return new MessageWithRoomList(messageHandle);
											}
											return new MessageWithAssetFileDownloadCancelResult(messageHandle);
										}
										IL_690:
										return new MessageWithRoomInviteNotificationList(messageHandle);
									}
									return new MessageWithPidList(messageHandle);
								}
								IL_6B4:
								return new MessageWithString(messageHandle);
							}
							IL_51C:
							return new MessageWithAchievementUpdate(messageHandle);
						}
						return new MessageWithCloudStorageMetadataUnderLocal(messageHandle);
					}
					IL_504:
					return new MessageWithAchievementDefinitions(messageHandle);
				}
				IL_66C:
				message = new MessageWithRoomUnderViewerRoom(messageHandle);
			}
			else
			{
				message = new MessageWithShareMediaResult(messageHandle);
			}
			return message;
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x0010B32C File Offset: 0x0010972C
		public static Message PopMessage()
		{
			if (!Core.IsInitialized())
			{
				return null;
			}
			IntPtr intPtr = CAPI.ovr_PopMessage();
			Message result = Message.ParseMessageHandle(intPtr);
			CAPI.ovr_FreeMessage(intPtr);
			return result;
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06003615 RID: 13845 RVA: 0x0010B361 File Offset: 0x00109761
		// (set) Token: 0x06003614 RID: 13844 RVA: 0x0010B359 File Offset: 0x00109759
		internal static Message.ExtraMessageTypesHandler HandleExtraMessageTypes
		{
			[CompilerGenerated]
			private get
			{
				return Message.<HandleExtraMessageTypes>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				Message.<HandleExtraMessageTypes>k__BackingField = value;
			}
		}

		// Token: 0x04002766 RID: 10086
		private Message.MessageType type;

		// Token: 0x04002767 RID: 10087
		private ulong requestID;

		// Token: 0x04002768 RID: 10088
		private Error error;

		// Token: 0x04002769 RID: 10089
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Message.ExtraMessageTypesHandler <HandleExtraMessageTypes>k__BackingField;

		// Token: 0x020007FF RID: 2047
		// (Invoke) Token: 0x06003617 RID: 13847
		public delegate void Callback(Message message);

		// Token: 0x02000800 RID: 2048
		public enum MessageType : uint
		{
			// Token: 0x0400276B RID: 10091
			Unknown,
			// Token: 0x0400276C RID: 10092
			Achievements_AddCount = 65495601U,
			// Token: 0x0400276D RID: 10093
			Achievements_AddFields = 346693929U,
			// Token: 0x0400276E RID: 10094
			Achievements_GetAllDefinitions = 64177549U,
			// Token: 0x0400276F RID: 10095
			Achievements_GetAllProgress = 1335877149U,
			// Token: 0x04002770 RID: 10096
			Achievements_GetDefinitionsByName = 1653670332U,
			// Token: 0x04002771 RID: 10097
			Achievements_GetNextAchievementDefinitionArrayPage = 712888917U,
			// Token: 0x04002772 RID: 10098
			Achievements_GetNextAchievementProgressArrayPage = 792913703U,
			// Token: 0x04002773 RID: 10099
			Achievements_GetProgressByName = 354837425U,
			// Token: 0x04002774 RID: 10100
			Achievements_Unlock = 1497156573U,
			// Token: 0x04002775 RID: 10101
			ApplicationLifecycle_GetRegisteredPIDs = 82169698U,
			// Token: 0x04002776 RID: 10102
			ApplicationLifecycle_GetSessionKey = 984570141U,
			// Token: 0x04002777 RID: 10103
			ApplicationLifecycle_RegisterSessionKey = 1303818232U,
			// Token: 0x04002778 RID: 10104
			Application_GetVersion = 1751583246U,
			// Token: 0x04002779 RID: 10105
			Application_LaunchOtherApp = 1424151032U,
			// Token: 0x0400277A RID: 10106
			AssetFile_Delete = 1834842246U,
			// Token: 0x0400277B RID: 10107
			AssetFile_Download = 289710021U,
			// Token: 0x0400277C RID: 10108
			AssetFile_DownloadCancel = 134927303U,
			// Token: 0x0400277D RID: 10109
			CloudStorage_Delete = 685393261U,
			// Token: 0x0400277E RID: 10110
			CloudStorage_GetNextCloudStorageMetadataArrayPage = 1544004335U,
			// Token: 0x0400277F RID: 10111
			CloudStorage_Load = 1082420033U,
			// Token: 0x04002780 RID: 10112
			CloudStorage_LoadBucketMetadata = 1931977997U,
			// Token: 0x04002781 RID: 10113
			CloudStorage_LoadConflictMetadata = 1146770162U,
			// Token: 0x04002782 RID: 10114
			CloudStorage_LoadHandle = 845863478U,
			// Token: 0x04002783 RID: 10115
			CloudStorage_LoadMetadata = 65446546U,
			// Token: 0x04002784 RID: 10116
			CloudStorage_ResolveKeepLocal = 811109637U,
			// Token: 0x04002785 RID: 10117
			CloudStorage_ResolveKeepRemote = 1965400838U,
			// Token: 0x04002786 RID: 10118
			CloudStorage_Save = 1270570030U,
			// Token: 0x04002787 RID: 10119
			Entitlement_GetIsViewerEntitled = 409688241U,
			// Token: 0x04002788 RID: 10120
			IAP_ConsumePurchase = 532378329U,
			// Token: 0x04002789 RID: 10121
			IAP_GetNextProductArrayPage = 467225263U,
			// Token: 0x0400278A RID: 10122
			IAP_GetNextPurchaseArrayPage = 1196886677U,
			// Token: 0x0400278B RID: 10123
			IAP_GetProductsBySKU = 2124073717U,
			// Token: 0x0400278C RID: 10124
			IAP_GetViewerPurchases = 974095385U,
			// Token: 0x0400278D RID: 10125
			IAP_LaunchCheckoutFlow = 1067126029U,
			// Token: 0x0400278E RID: 10126
			Leaderboard_GetEntries = 1572030284U,
			// Token: 0x0400278F RID: 10127
			Leaderboard_GetEntriesAfterRank = 406293487U,
			// Token: 0x04002790 RID: 10128
			Leaderboard_GetNextEntries = 1310751961U,
			// Token: 0x04002791 RID: 10129
			Leaderboard_GetPreviousEntries = 1224858304U,
			// Token: 0x04002792 RID: 10130
			Leaderboard_WriteEntry = 293587198U,
			// Token: 0x04002793 RID: 10131
			Livestreaming_GetStatus = 1218079125U,
			// Token: 0x04002794 RID: 10132
			Livestreaming_PauseStream = 916223619U,
			// Token: 0x04002795 RID: 10133
			Livestreaming_ResumeStream = 575827343U,
			// Token: 0x04002796 RID: 10134
			Matchmaking_Browse = 509948616U,
			// Token: 0x04002797 RID: 10135
			Matchmaking_Browse2 = 1715641947U,
			// Token: 0x04002798 RID: 10136
			Matchmaking_Cancel = 543705519U,
			// Token: 0x04002799 RID: 10137
			Matchmaking_Cancel2 = 285117908U,
			// Token: 0x0400279A RID: 10138
			Matchmaking_CreateAndEnqueueRoom = 1615617480U,
			// Token: 0x0400279B RID: 10139
			Matchmaking_CreateAndEnqueueRoom2 = 693889755U,
			// Token: 0x0400279C RID: 10140
			Matchmaking_CreateRoom = 54203178U,
			// Token: 0x0400279D RID: 10141
			Matchmaking_CreateRoom2 = 1231922052U,
			// Token: 0x0400279E RID: 10142
			Matchmaking_Enqueue = 1086418033U,
			// Token: 0x0400279F RID: 10143
			Matchmaking_Enqueue2 = 303174325U,
			// Token: 0x040027A0 RID: 10144
			Matchmaking_EnqueueRoom = 1888108644U,
			// Token: 0x040027A1 RID: 10145
			Matchmaking_EnqueueRoom2 = 1428741028U,
			// Token: 0x040027A2 RID: 10146
			Matchmaking_GetAdminSnapshot = 1008820116U,
			// Token: 0x040027A3 RID: 10147
			Matchmaking_GetStats = 1123849272U,
			// Token: 0x040027A4 RID: 10148
			Matchmaking_JoinRoom = 1295177725U,
			// Token: 0x040027A5 RID: 10149
			Matchmaking_ReportResultInsecure = 439800205U,
			// Token: 0x040027A6 RID: 10150
			Matchmaking_StartMatch = 1154746693U,
			// Token: 0x040027A7 RID: 10151
			Media_ShareToFacebook = 14912239U,
			// Token: 0x040027A8 RID: 10152
			Notification_GetNextRoomInviteNotificationArrayPage = 102890359U,
			// Token: 0x040027A9 RID: 10153
			Notification_GetRoomInvites = 1871801234U,
			// Token: 0x040027AA RID: 10154
			Notification_MarkAsRead = 1903319523U,
			// Token: 0x040027AB RID: 10155
			Party_GetCurrent = 1200830304U,
			// Token: 0x040027AC RID: 10156
			Room_CreateAndJoinPrivate = 1977017207U,
			// Token: 0x040027AD RID: 10157
			Room_CreateAndJoinPrivate2 = 1513775683U,
			// Token: 0x040027AE RID: 10158
			Room_Get = 1704628152U,
			// Token: 0x040027AF RID: 10159
			Room_GetCurrent = 161916164U,
			// Token: 0x040027B0 RID: 10160
			Room_GetCurrentForUser = 234887141U,
			// Token: 0x040027B1 RID: 10161
			Room_GetInvitableUsers = 506615698U,
			// Token: 0x040027B2 RID: 10162
			Room_GetInvitableUsers2 = 1330899120U,
			// Token: 0x040027B3 RID: 10163
			Room_GetModeratedRooms = 159645047U,
			// Token: 0x040027B4 RID: 10164
			Room_GetNextRoomArrayPage = 1317239238U,
			// Token: 0x040027B5 RID: 10165
			Room_InviteUser = 1093266451U,
			// Token: 0x040027B6 RID: 10166
			Room_Join = 382373641U,
			// Token: 0x040027B7 RID: 10167
			Room_Join2 = 1303059522U,
			// Token: 0x040027B8 RID: 10168
			Room_KickUser = 1233344310U,
			// Token: 0x040027B9 RID: 10169
			Room_LaunchInvitableUserFlow = 843047539U,
			// Token: 0x040027BA RID: 10170
			Room_Leave = 1916281973U,
			// Token: 0x040027BB RID: 10171
			Room_SetDescription = 809796911U,
			// Token: 0x040027BC RID: 10172
			Room_UpdateDataStore = 40779816U,
			// Token: 0x040027BD RID: 10173
			Room_UpdateMembershipLockStatus = 923514796U,
			// Token: 0x040027BE RID: 10174
			Room_UpdateOwner = 850803997U,
			// Token: 0x040027BF RID: 10175
			Room_UpdatePrivateRoomJoinPolicy = 289473179U,
			// Token: 0x040027C0 RID: 10176
			User_Get = 1808768583U,
			// Token: 0x040027C1 RID: 10177
			User_GetAccessToken = 111696574U,
			// Token: 0x040027C2 RID: 10178
			User_GetLoggedInUser = 1131361373U,
			// Token: 0x040027C3 RID: 10179
			User_GetLoggedInUserFriends = 1484532365U,
			// Token: 0x040027C4 RID: 10180
			User_GetLoggedInUserFriendsAndRooms = 1585908615U,
			// Token: 0x040027C5 RID: 10181
			User_GetLoggedInUserRecentlyMetUsersAndRooms = 694139440U,
			// Token: 0x040027C6 RID: 10182
			User_GetNextUserAndRoomArrayPage = 2143146719U,
			// Token: 0x040027C7 RID: 10183
			User_GetNextUserArrayPage = 645723971U,
			// Token: 0x040027C8 RID: 10184
			User_GetOrgScopedID = 418426907U,
			// Token: 0x040027C9 RID: 10185
			User_GetSdkAccounts = 1733454467U,
			// Token: 0x040027CA RID: 10186
			User_GetUserProof = 578880643U,
			// Token: 0x040027CB RID: 10187
			User_LaunchProfile = 171537047U,
			// Token: 0x040027CC RID: 10188
			Voip_SetSystemVoipSuppressed = 1161808298U,
			// Token: 0x040027CD RID: 10189
			Notification_ApplicationLifecycle_LaunchIntentChanged = 78859427U,
			// Token: 0x040027CE RID: 10190
			Notification_AssetFile_DownloadUpdate = 803015885U,
			// Token: 0x040027CF RID: 10191
			Notification_HTTP_Transfer = 2111073839U,
			// Token: 0x040027D0 RID: 10192
			Notification_Livestreaming_StatusChange = 575101294U,
			// Token: 0x040027D1 RID: 10193
			Notification_Matchmaking_MatchFound = 197393623U,
			// Token: 0x040027D2 RID: 10194
			Notification_Networking_ConnectionStateChange = 1577243802U,
			// Token: 0x040027D3 RID: 10195
			Notification_Networking_PeerConnectRequest = 1295114959U,
			// Token: 0x040027D4 RID: 10196
			Notification_Networking_PingResult = 1360343058U,
			// Token: 0x040027D5 RID: 10197
			Notification_Room_InviteAccepted = 1829794225U,
			// Token: 0x040027D6 RID: 10198
			Notification_Room_InviteReceived = 1783209300U,
			// Token: 0x040027D7 RID: 10199
			Notification_Room_RoomUpdate = 1626094639U,
			// Token: 0x040027D8 RID: 10200
			Notification_Voip_ConnectRequest = 908343318U,
			// Token: 0x040027D9 RID: 10201
			Notification_Voip_StateChange = 888120928U,
			// Token: 0x040027DA RID: 10202
			Notification_Voip_SystemVoipState = 1490179237U,
			// Token: 0x040027DB RID: 10203
			Platform_InitializeStandaloneOculus = 1375260172U,
			// Token: 0x040027DC RID: 10204
			Platform_InitializeAndroidAsynchronous = 450037684U,
			// Token: 0x040027DD RID: 10205
			Platform_InitializeWindowsAsynchronous = 1839708815U
		}

		// Token: 0x02000801 RID: 2049
		// (Invoke) Token: 0x0600361B RID: 13851
		internal delegate Message ExtraMessageTypesHandler(IntPtr messageHandle, Message.MessageType message_type);
	}
}
