using System;
using Oculus.Platform;
using Oculus.Platform.Models;

// Token: 0x0200077A RID: 1914
public class VoipManager
{
	// Token: 0x06003169 RID: 12649 RVA: 0x00101350 File Offset: 0x000FF750
	public VoipManager()
	{
		Voip.SetVoipConnectRequestCallback(new Message<NetworkingPeer>.Callback(this.VoipConnectRequestCallback));
		Voip.SetVoipStateChangeCallback(new Message<NetworkingPeer>.Callback(this.VoipStateChangedCallback));
	}

	// Token: 0x0600316A RID: 12650 RVA: 0x0010137A File Offset: 0x000FF77A
	public void ConnectTo(ulong userID)
	{
		if (PlatformManager.MyID < userID)
		{
			Voip.Start(userID);
			PlatformManager.LogOutput("Voip connect to " + userID);
		}
	}

	// Token: 0x0600316B RID: 12651 RVA: 0x001013A4 File Offset: 0x000FF7A4
	public void Disconnect(ulong userID)
	{
		if (userID != 0UL)
		{
			Voip.Stop(userID);
			RemotePlayer remoteUser = PlatformManager.GetRemoteUser(userID);
			if (remoteUser != null)
			{
				remoteUser.voipConnectionState = PeerConnectionState.Unknown;
			}
		}
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x001013D4 File Offset: 0x000FF7D4
	private void VoipConnectRequestCallback(Message<NetworkingPeer> msg)
	{
		PlatformManager.LogOutput("Voip request from " + msg.Data.ID);
		RemotePlayer remoteUser = PlatformManager.GetRemoteUser(msg.Data.ID);
		if (remoteUser != null)
		{
			PlatformManager.LogOutput("Voip request accepted from " + msg.Data.ID);
			Voip.Accept(msg.Data.ID);
		}
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x00101448 File Offset: 0x000FF848
	private void VoipStateChangedCallback(Message<NetworkingPeer> msg)
	{
		PlatformManager.LogOutput(string.Concat(new object[]
		{
			"Voip state to ",
			msg.Data.ID,
			" changed to  ",
			msg.Data.State
		}));
		RemotePlayer remoteUser = PlatformManager.GetRemoteUser(msg.Data.ID);
		if (remoteUser != null)
		{
			remoteUser.voipConnectionState = msg.Data.State;
			if (msg.Data.State == PeerConnectionState.Timeout && PlatformManager.MyID < msg.Data.ID)
			{
				Voip.Start(msg.Data.ID);
				PlatformManager.LogOutput("Voip re-connect to " + msg.Data.ID);
			}
		}
	}
}
