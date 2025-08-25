using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000895 RID: 2197
	public static class PlatformInternal
	{
		// Token: 0x060037B1 RID: 14257 RVA: 0x0010E70F File Offset: 0x0010CB0F
		public static void CrashApplication()
		{
			CAPI.ovr_CrashApplication();
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x0010E718 File Offset: 0x0010CB18
		internal static Message ParseMessageHandle(IntPtr messageHandle, Message.MessageType messageType)
		{
			Message result = null;
			if (messageType != (Message.MessageType)125660812U)
			{
				if (messageType != (Message.MessageType)191729014U)
				{
					if (messageType != (Message.MessageType)292822787U)
					{
						if (messageType != (Message.MessageType)442139697U)
						{
							if (messageType != (Message.MessageType)450042703U)
							{
								if (messageType != (Message.MessageType)475495815U)
								{
									if (messageType == (Message.MessageType)493497353U)
									{
										goto IL_204;
									}
									if (messageType == (Message.MessageType)517416647U)
									{
										goto IL_1F8;
									}
									if (messageType == (Message.MessageType)528318516U)
									{
										return new MessageWithLivestreamingStatus(messageHandle);
									}
									if (messageType != (Message.MessageType)645772532U && messageType != (Message.MessageType)661065560U)
									{
										if (messageType == (Message.MessageType)822018158U)
										{
											goto IL_1F8;
										}
										if (messageType != (Message.MessageType)848430801U)
										{
											if (messageType == (Message.MessageType)901104867U)
											{
												goto IL_1D4;
											}
											if (messageType == (Message.MessageType)921194380U)
											{
												goto IL_1F8;
											}
											if (messageType == (Message.MessageType)1155796426U)
											{
												return new MessageWithLivestreamingVideoStats(messageHandle);
											}
											if (messageType != (Message.MessageType)1317133401U)
											{
												if (messageType != (Message.MessageType)1343932350U)
												{
													if (messageType == (Message.MessageType)1376744524U)
													{
														return new MessageWithInstalledApplicationList(messageHandle);
													}
													if (messageType == (Message.MessageType)1449304081U)
													{
														return new MessageWithUserReportID(messageHandle);
													}
													if (messageType == (Message.MessageType)1480774160U)
													{
														goto IL_1F8;
													}
													if (messageType == (Message.MessageType)1489764138U)
													{
														return new MessageWithPartyUnderCurrentParty(messageHandle);
													}
													if (messageType == (Message.MessageType)1586058173U)
													{
														return new MessageWithParty(messageHandle);
													}
													if (messageType == (Message.MessageType)1636310390U)
													{
														goto IL_1EC;
													}
													if (messageType == (Message.MessageType)1744993395U)
													{
														goto IL_1D4;
													}
													if (messageType == (Message.MessageType)1798743375U || messageType == (Message.MessageType)1874211363U)
													{
														goto IL_1F8;
													}
													if (messageType == (Message.MessageType)1876305192U)
													{
														goto IL_174;
													}
													if (messageType == (Message.MessageType)1921499523U)
													{
														goto IL_1D4;
													}
													if (messageType == (Message.MessageType)1990567876U)
													{
														goto IL_1F8;
													}
													if (messageType != (Message.MessageType)2066701532U)
													{
														if (messageType == (Message.MessageType)2077219214U)
														{
															goto IL_1F8;
														}
														if (messageType != (Message.MessageType)2089683601U)
														{
															return result;
														}
														return new MessageWithRoomUnderViewerRoom(messageHandle);
													}
												}
												return new MessageWithLivestreamingStartResult(messageHandle);
											}
											goto IL_1F8;
										}
									}
								}
								IL_174:
								return new Message(messageHandle);
							}
							IL_1D4:
							return new MessageWithPartyID(messageHandle);
						}
						IL_204:
						return new MessageWithSystemPermission(messageHandle);
					}
					IL_1F8:
					return new MessageWithString(messageHandle);
				}
				return new MessageWithLivestreamingApplicationStatus(messageHandle);
			}
			IL_1EC:
			result = new MessageWithRoomList(messageHandle);
			return result;
		}

		// Token: 0x02000896 RID: 2198
		public enum MessageTypeInternal : uint
		{
			// Token: 0x040028B1 RID: 10417
			Application_ExecuteCoordinatedLaunch = 645772532U,
			// Token: 0x040028B2 RID: 10418
			Application_GetInstalledApplications = 1376744524U,
			// Token: 0x040028B3 RID: 10419
			Avatar_UpdateMetaData = 2077219214U,
			// Token: 0x040028B4 RID: 10420
			GraphAPI_Get = 822018158U,
			// Token: 0x040028B5 RID: 10421
			GraphAPI_Post = 1990567876U,
			// Token: 0x040028B6 RID: 10422
			HTTP_Get = 1874211363U,
			// Token: 0x040028B7 RID: 10423
			HTTP_GetToFile = 1317133401U,
			// Token: 0x040028B8 RID: 10424
			HTTP_MultiPartPost = 1480774160U,
			// Token: 0x040028B9 RID: 10425
			HTTP_Post = 1798743375U,
			// Token: 0x040028BA RID: 10426
			Livestreaming_IsAllowedForApplication = 191729014U,
			// Token: 0x040028BB RID: 10427
			Livestreaming_StartPartyStream = 2066701532U,
			// Token: 0x040028BC RID: 10428
			Livestreaming_StartStream = 1343932350U,
			// Token: 0x040028BD RID: 10429
			Livestreaming_StopPartyStream = 661065560U,
			// Token: 0x040028BE RID: 10430
			Livestreaming_StopStream = 1155796426U,
			// Token: 0x040028BF RID: 10431
			Livestreaming_UpdateCommentsOverlayVisibility = 528318516U,
			// Token: 0x040028C0 RID: 10432
			Livestreaming_UpdateMicStatus = 475495815U,
			// Token: 0x040028C1 RID: 10433
			Party_Create = 450042703U,
			// Token: 0x040028C2 RID: 10434
			Party_GatherInApplication = 1921499523U,
			// Token: 0x040028C3 RID: 10435
			Party_Get = 1586058173U,
			// Token: 0x040028C4 RID: 10436
			Party_GetCurrentForUser = 1489764138U,
			// Token: 0x040028C5 RID: 10437
			Party_Invite = 901104867U,
			// Token: 0x040028C6 RID: 10438
			Party_Join = 1744993395U,
			// Token: 0x040028C7 RID: 10439
			Party_Leave = 848430801U,
			// Token: 0x040028C8 RID: 10440
			Room_CreateOrUpdateAndJoinNamed = 2089683601U,
			// Token: 0x040028C9 RID: 10441
			Room_GetNamedRooms = 125660812U,
			// Token: 0x040028CA RID: 10442
			Room_GetSocialRooms = 1636310390U,
			// Token: 0x040028CB RID: 10443
			SystemPermissions_GetStatus = 493497353U,
			// Token: 0x040028CC RID: 10444
			SystemPermissions_LaunchDeeplink = 442139697U,
			// Token: 0x040028CD RID: 10445
			User_LaunchBlockFlow = 1876305192U,
			// Token: 0x040028CE RID: 10446
			User_LaunchReportFlow = 1449304081U,
			// Token: 0x040028CF RID: 10447
			User_NewEntitledTestUser = 292822787U,
			// Token: 0x040028D0 RID: 10448
			User_NewTestUser = 921194380U,
			// Token: 0x040028D1 RID: 10449
			User_NewTestUserFriends = 517416647U
		}

		// Token: 0x02000897 RID: 2199
		public static class HTTP
		{
			// Token: 0x060037B3 RID: 14259 RVA: 0x0010E942 File Offset: 0x0010CD42
			public static void SetHttpTransferUpdateCallback(Message<HttpTransferUpdate>.Callback callback)
			{
				Callback.SetNotificationCallback<HttpTransferUpdate>(Message.MessageType.Notification_HTTP_Transfer, callback);
			}
		}
	}
}
