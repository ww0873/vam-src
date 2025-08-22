using System;
using Oculus.Platform;
using Oculus.Platform.Models;

// Token: 0x02000779 RID: 1913
public class RoomManager
{
	// Token: 0x0600315E RID: 12638 RVA: 0x00100F8F File Offset: 0x000FF38F
	public RoomManager()
	{
		this.amIServer = false;
		this.startupDone = false;
		Rooms.SetRoomInviteNotificationCallback(new Message<string>.Callback(this.AcceptingInviteCallback));
		Rooms.SetUpdateNotificationCallback(new Message<Room>.Callback(this.RoomUpdateCallback));
	}

	// Token: 0x0600315F RID: 12639 RVA: 0x00100FC8 File Offset: 0x000FF3C8
	private void AcceptingInviteCallback(Message<string> msg)
	{
		if (msg.IsError)
		{
			PlatformManager.TerminateWithError(msg);
			return;
		}
		PlatformManager.LogOutput("Launched Invite to join Room: " + msg.Data);
		this.invitedRoomID = Convert.ToUInt64(msg.GetString());
		if (this.startupDone)
		{
			this.CheckForInvite();
		}
	}

	// Token: 0x06003160 RID: 12640 RVA: 0x0010101F File Offset: 0x000FF41F
	public bool CheckForInvite()
	{
		this.startupDone = true;
		if (this.invitedRoomID != 0UL)
		{
			this.JoinExistingRoom(this.invitedRoomID);
			return true;
		}
		return false;
	}

	// Token: 0x06003161 RID: 12641 RVA: 0x00101044 File Offset: 0x000FF444
	public void CreateRoom()
	{
		Rooms.CreateAndJoinPrivate(RoomJoinPolicy.InvitedUsers, 4U, true).OnComplete(new Message<Room>.Callback(this.CreateAndJoinPrivateRoomCallback));
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x00101060 File Offset: 0x000FF460
	private void CreateAndJoinPrivateRoomCallback(Message<Room> msg)
	{
		if (msg.IsError)
		{
			PlatformManager.TerminateWithError(msg);
			return;
		}
		this.roomID = msg.Data.ID;
		if (msg.Data.Owner.ID == PlatformManager.MyID)
		{
			this.amIServer = true;
		}
		else
		{
			this.amIServer = false;
		}
		PlatformManager.TransitionToState(PlatformManager.State.WAITING_IN_A_ROOM);
		PlatformManager.SetFloorColorForState(this.amIServer);
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x001010CE File Offset: 0x000FF4CE
	private void OnLaunchInviteWorkflowComplete(Message msg)
	{
		if (msg.IsError)
		{
			PlatformManager.TerminateWithError(msg);
			return;
		}
	}

	// Token: 0x06003164 RID: 12644 RVA: 0x001010E2 File Offset: 0x000FF4E2
	public void JoinExistingRoom(ulong roomID)
	{
		PlatformManager.TransitionToState(PlatformManager.State.JOINING_A_ROOM);
		Rooms.Join(roomID, true).OnComplete(new Message<Room>.Callback(this.JoinRoomCallback));
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x00101104 File Offset: 0x000FF504
	private void JoinRoomCallback(Message<Room> msg)
	{
		if (msg.IsError)
		{
			return;
		}
		PlatformManager.LogOutput(string.Concat(new object[]
		{
			"Joined Room ",
			msg.Data.ID,
			" owner: ",
			msg.Data.Owner.OculusID,
			" count: ",
			msg.Data.Users.Count
		}));
		this.roomID = msg.Data.ID;
		this.ProcessRoomData(msg);
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x0010119C File Offset: 0x000FF59C
	private void RoomUpdateCallback(Message<Room> msg)
	{
		if (msg.IsError)
		{
			PlatformManager.TerminateWithError(msg);
			return;
		}
		PlatformManager.LogOutput(string.Concat(new object[]
		{
			"Room Update ",
			msg.Data.ID,
			" owner: ",
			msg.Data.Owner.OculusID,
			" count: ",
			msg.Data.Users.Count
		}));
		this.ProcessRoomData(msg);
	}

	// Token: 0x06003167 RID: 12647 RVA: 0x00101228 File Offset: 0x000FF628
	public void LeaveCurrentRoom()
	{
		if (this.roomID != 0UL)
		{
			Rooms.Leave(this.roomID);
			this.roomID = 0UL;
		}
		PlatformManager.TransitionToState(PlatformManager.State.LEAVING_A_ROOM);
	}

	// Token: 0x06003168 RID: 12648 RVA: 0x00101254 File Offset: 0x000FF654
	private void ProcessRoomData(Message<Room> msg)
	{
		if (msg.Data.Owner.ID == PlatformManager.MyID)
		{
			this.amIServer = true;
		}
		else
		{
			this.amIServer = false;
		}
		if (msg.Data.Users.Count == 1)
		{
			PlatformManager.TransitionToState(PlatformManager.State.WAITING_IN_A_ROOM);
		}
		else
		{
			PlatformManager.TransitionToState(PlatformManager.State.CONNECTED_IN_A_ROOM);
		}
		PlatformManager.MarkAllRemoteUsersAsNotInRoom();
		foreach (User user in msg.Data.Users)
		{
			if (user.ID != PlatformManager.MyID)
			{
				if (!PlatformManager.IsUserInRoom(user.ID))
				{
					PlatformManager.AddRemoteUser(user.ID);
				}
				else
				{
					PlatformManager.MarkRemoteUserInRoom(user.ID);
				}
			}
		}
		PlatformManager.ForgetRemoteUsersNotInRoom();
		PlatformManager.SetFloorColorForState(this.amIServer);
	}

	// Token: 0x04002530 RID: 9520
	public ulong roomID;

	// Token: 0x04002531 RID: 9521
	private ulong invitedRoomID;

	// Token: 0x04002532 RID: 9522
	private bool amIServer;

	// Token: 0x04002533 RID: 9523
	private bool startupDone;
}
