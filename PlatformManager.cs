using System;
using System.Collections.Generic;
using Oculus.Avatar;
using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine;

// Token: 0x02000775 RID: 1909
public class PlatformManager : MonoBehaviour
{
	// Token: 0x0600313B RID: 12603 RVA: 0x00100166 File Offset: 0x000FE566
	public PlatformManager()
	{
	}

	// Token: 0x0600313C RID: 12604 RVA: 0x0010018B File Offset: 0x000FE58B
	public virtual void Update()
	{
		this.p2pManager.GetRemotePackets();
	}

	// Token: 0x0600313D RID: 12605 RVA: 0x00100198 File Offset: 0x000FE598
	public virtual void Awake()
	{
		this.LogOutputLine("Start Log.");
		this.helpMesh = this.helpPanel.GetComponent<MeshRenderer>();
		this.sphereMesh = this.roomSphere.GetComponent<MeshRenderer>();
		this.floorMesh = this.roomFloor.GetComponent<MeshRenderer>();
		this.localTrackingSpace = base.transform.Find("OVRCameraRig/TrackingSpace").gameObject;
		this.localPlayerHead = base.transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").gameObject;
		if (PlatformManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		PlatformManager.s_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		PlatformManager.TransitionToState(PlatformManager.State.INITIALIZING);
		Core.Initialize(null);
		this.roomManager = new RoomManager();
		this.p2pManager = new P2PManager();
		this.voipManager = new VoipManager();
	}

	// Token: 0x0600313E RID: 12606 RVA: 0x00100273 File Offset: 0x000FE673
	public virtual void Start()
	{
		Entitlements.IsUserEntitledToApplication().OnComplete(new Message.Callback(this.IsEntitledCallback));
		Request.RunCallbacks(0U);
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x00100292 File Offset: 0x000FE692
	private void IsEntitledCallback(Message msg)
	{
		if (msg.IsError)
		{
			PlatformManager.TerminateWithError(msg);
			return;
		}
		Users.GetLoggedInUser().OnComplete(new Message<User>.Callback(this.GetLoggedInUserCallback));
		Request.RunCallbacks(0U);
	}

	// Token: 0x06003140 RID: 12608 RVA: 0x001002C4 File Offset: 0x000FE6C4
	private void GetLoggedInUserCallback(Message<User> msg)
	{
		if (msg.IsError)
		{
			PlatformManager.TerminateWithError(msg);
			return;
		}
		this.myID = msg.Data.ID;
		this.myOculusID = msg.Data.OculusID;
		this.localAvatar = UnityEngine.Object.Instantiate<OvrAvatar>(this.localAvatarPrefab);
		this.localTrackingSpace = base.transform.Find("OVRCameraRig/TrackingSpace").gameObject;
		this.localAvatar.transform.SetParent(this.localTrackingSpace.transform, false);
		this.localAvatar.transform.localPosition = new Vector3(0f, 0f, 0f);
		this.localAvatar.transform.localRotation = Quaternion.identity;
		if (UnityEngine.Application.platform == RuntimePlatform.Android)
		{
			this.helpPanel.transform.SetParent(this.localAvatar.transform.Find("body"), false);
			this.helpPanel.transform.localPosition = new Vector3(0f, 0f, 1f);
			this.helpMesh.material = this.gearMaterial;
		}
		else
		{
			this.helpPanel.transform.SetParent(this.localAvatar.transform.Find("hand_left"), false);
			this.helpPanel.transform.localPosition = new Vector3(0f, 0.2f, 0.2f);
			this.helpMesh.material = this.riftMaterial;
		}
		this.localAvatar.oculusUserID = this.myID;
		this.localAvatar.RecordPackets = true;
		OvrAvatar ovrAvatar = this.localAvatar;
		ovrAvatar.PacketRecorded = (EventHandler<OvrAvatar.PacketEventArgs>)Delegate.Combine(ovrAvatar.PacketRecorded, new EventHandler<OvrAvatar.PacketEventArgs>(this.OnLocalAvatarPacketRecorded));
		Quaternion identity = Quaternion.identity;
		switch (UnityEngine.Random.Range(0, 4))
		{
		case 0:
			identity.eulerAngles = PlatformManager.START_ROTATION_ONE;
			base.transform.localPosition = PlatformManager.START_POSITION_ONE;
			base.transform.localRotation = identity;
			goto IL_29D;
		case 1:
			identity.eulerAngles = PlatformManager.START_ROTATION_TWO;
			base.transform.localPosition = PlatformManager.START_POSITION_TWO;
			base.transform.localRotation = identity;
			goto IL_29D;
		case 2:
			identity.eulerAngles = PlatformManager.START_ROTATION_THREE;
			base.transform.localPosition = PlatformManager.START_POSITION_THREE;
			base.transform.localRotation = identity;
			goto IL_29D;
		}
		identity.eulerAngles = PlatformManager.START_ROTATION_FOUR;
		base.transform.localPosition = PlatformManager.START_POSITION_FOUR;
		base.transform.localRotation = identity;
		IL_29D:
		PlatformManager.TransitionToState(PlatformManager.State.CHECKING_LAUNCH_STATE);
		if (!this.roomManager.CheckForInvite())
		{
			this.roomManager.CreateRoom();
			PlatformManager.TransitionToState(PlatformManager.State.CREATING_A_ROOM);
		}
		Voip.SetMicrophoneFilterCallback(this.micFilterDelegate);
	}

	// Token: 0x06003141 RID: 12609 RVA: 0x001005A0 File Offset: 0x000FE9A0
	public void OnLocalAvatarPacketRecorded(object sender, OvrAvatar.PacketEventArgs args)
	{
		uint num = Oculus.Avatar.CAPI.ovrAvatarPacket_GetSize(args.Packet.ovrNativePacket);
		byte[] array = new byte[num];
		Oculus.Avatar.CAPI.ovrAvatarPacket_Write(args.Packet.ovrNativePacket, num, array);
		foreach (KeyValuePair<ulong, RemotePlayer> keyValuePair in this.remoteUsers)
		{
			this.LogOutputLine("Sending Packet to  " + keyValuePair.Key);
			this.p2pManager.SendAvatarUpdate(keyValuePair.Key, base.transform, this.packetSequence, array);
		}
		this.packetSequence += 1U;
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x0010066C File Offset: 0x000FEA6C
	public void OnApplicationQuit()
	{
		this.roomManager.LeaveCurrentRoom();
		foreach (KeyValuePair<ulong, RemotePlayer> keyValuePair in this.remoteUsers)
		{
			this.p2pManager.Disconnect(keyValuePair.Key);
			this.voipManager.Disconnect(keyValuePair.Key);
		}
		this.LogOutputLine("End Log.");
	}

	// Token: 0x06003143 RID: 12611 RVA: 0x001006FC File Offset: 0x000FEAFC
	public void AddUser(ulong userID, ref RemotePlayer remoteUser)
	{
		this.remoteUsers.Add(userID, remoteUser);
	}

	// Token: 0x06003144 RID: 12612 RVA: 0x0010070C File Offset: 0x000FEB0C
	public void LogOutputLine(string line)
	{
		Debug.Log(Time.time + ": " + line);
	}

	// Token: 0x06003145 RID: 12613 RVA: 0x00100728 File Offset: 0x000FEB28
	public static void TerminateWithError(Message msg)
	{
		PlatformManager.s_instance.LogOutputLine("Error: " + msg.GetError().Message);
		UnityEngine.Application.Quit();
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x06003146 RID: 12614 RVA: 0x0010074E File Offset: 0x000FEB4E
	public static PlatformManager.State CurrentState
	{
		get
		{
			return PlatformManager.s_instance.currentState;
		}
	}

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x06003147 RID: 12615 RVA: 0x0010075A File Offset: 0x000FEB5A
	public static ulong MyID
	{
		get
		{
			if (PlatformManager.s_instance != null)
			{
				return PlatformManager.s_instance.myID;
			}
			return 0UL;
		}
	}

	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x06003148 RID: 12616 RVA: 0x00100779 File Offset: 0x000FEB79
	public static string MyOculusID
	{
		get
		{
			if (PlatformManager.s_instance != null && PlatformManager.s_instance.myOculusID != null)
			{
				return PlatformManager.s_instance.myOculusID;
			}
			return string.Empty;
		}
	}

	// Token: 0x06003149 RID: 12617 RVA: 0x001007AC File Offset: 0x000FEBAC
	public static void TransitionToState(PlatformManager.State newState)
	{
		if (PlatformManager.s_instance)
		{
			PlatformManager.s_instance.LogOutputLine(string.Concat(new object[]
			{
				"State ",
				PlatformManager.s_instance.currentState,
				" -> ",
				newState
			}));
		}
		if (PlatformManager.s_instance && PlatformManager.s_instance.currentState != newState)
		{
			PlatformManager.s_instance.currentState = newState;
			if (newState == PlatformManager.State.SHUTDOWN)
			{
				PlatformManager.s_instance.OnApplicationQuit();
			}
		}
		PlatformManager.SetSphereColorForState();
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x00100858 File Offset: 0x000FEC58
	private static void SetSphereColorForState()
	{
		switch (PlatformManager.s_instance.currentState)
		{
		case PlatformManager.State.INITIALIZING:
		case PlatformManager.State.SHUTDOWN:
			PlatformManager.s_instance.sphereMesh.material.color = PlatformManager.BLACK;
			break;
		case PlatformManager.State.WAITING_IN_A_ROOM:
			PlatformManager.s_instance.sphereMesh.material.color = PlatformManager.WHITE;
			break;
		case PlatformManager.State.CONNECTED_IN_A_ROOM:
			PlatformManager.s_instance.sphereMesh.material.color = PlatformManager.CYAN;
			break;
		}
	}

	// Token: 0x0600314B RID: 12619 RVA: 0x001008FA File Offset: 0x000FECFA
	public static void SetFloorColorForState(bool host)
	{
		if (host)
		{
			PlatformManager.s_instance.floorMesh.material.color = PlatformManager.BLUE;
		}
		else
		{
			PlatformManager.s_instance.floorMesh.material.color = PlatformManager.GREEN;
		}
	}

	// Token: 0x0600314C RID: 12620 RVA: 0x0010093C File Offset: 0x000FED3C
	public static void MarkAllRemoteUsersAsNotInRoom()
	{
		foreach (KeyValuePair<ulong, RemotePlayer> keyValuePair in PlatformManager.s_instance.remoteUsers)
		{
			keyValuePair.Value.stillInRoom = false;
		}
	}

	// Token: 0x0600314D RID: 12621 RVA: 0x001009A4 File Offset: 0x000FEDA4
	public static void MarkRemoteUserInRoom(ulong userID)
	{
		RemotePlayer remotePlayer = new RemotePlayer();
		if (PlatformManager.s_instance.remoteUsers.TryGetValue(userID, out remotePlayer))
		{
			remotePlayer.stillInRoom = true;
		}
	}

	// Token: 0x0600314E RID: 12622 RVA: 0x001009D8 File Offset: 0x000FEDD8
	public static void ForgetRemoteUsersNotInRoom()
	{
		List<ulong> list = new List<ulong>();
		foreach (KeyValuePair<ulong, RemotePlayer> keyValuePair in PlatformManager.s_instance.remoteUsers)
		{
			if (!keyValuePair.Value.stillInRoom)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (ulong userID in list)
		{
			PlatformManager.RemoveRemoteUser(userID);
		}
	}

	// Token: 0x0600314F RID: 12623 RVA: 0x00100A9C File Offset: 0x000FEE9C
	public static void LogOutput(string line)
	{
		PlatformManager.s_instance.LogOutputLine(Time.time + ": " + line);
	}

	// Token: 0x06003150 RID: 12624 RVA: 0x00100ABD File Offset: 0x000FEEBD
	public static bool IsUserInRoom(ulong userID)
	{
		return PlatformManager.s_instance.remoteUsers.ContainsKey(userID);
	}

	// Token: 0x06003151 RID: 12625 RVA: 0x00100AD0 File Offset: 0x000FEED0
	public static void AddRemoteUser(ulong userID)
	{
		RemotePlayer remotePlayer = new RemotePlayer();
		remotePlayer.RemoteAvatar = UnityEngine.Object.Instantiate<OvrAvatar>(PlatformManager.s_instance.remoteAvatarPrefab);
		remotePlayer.RemoteAvatar.oculusUserID = userID;
		remotePlayer.RemoteAvatar.ShowThirdPerson = true;
		remotePlayer.p2pConnectionState = PeerConnectionState.Unknown;
		remotePlayer.voipConnectionState = PeerConnectionState.Unknown;
		remotePlayer.stillInRoom = true;
		remotePlayer.remoteUserID = userID;
		PlatformManager.s_instance.AddUser(userID, ref remotePlayer);
		PlatformManager.s_instance.p2pManager.ConnectTo(userID);
		PlatformManager.s_instance.voipManager.ConnectTo(userID);
		VoipAudioSourceHiLevel voipAudioSourceHiLevel = remotePlayer.RemoteAvatar.gameObject.AddComponent<VoipAudioSourceHiLevel>();
		voipAudioSourceHiLevel.senderID = userID;
		PlatformManager.s_instance.LogOutputLine("Adding User " + userID);
	}

	// Token: 0x06003152 RID: 12626 RVA: 0x00100B8C File Offset: 0x000FEF8C
	public static void RemoveRemoteUser(ulong userID)
	{
		RemotePlayer remotePlayer = new RemotePlayer();
		if (PlatformManager.s_instance.remoteUsers.TryGetValue(userID, out remotePlayer))
		{
			UnityEngine.Object.Destroy(remotePlayer.RemoteAvatar.GetComponent<VoipAudioSourceHiLevel>(), 0f);
			UnityEngine.Object.Destroy(remotePlayer.RemoteAvatar.gameObject, 0f);
			PlatformManager.s_instance.remoteUsers.Remove(userID);
			PlatformManager.s_instance.LogOutputLine("Removing User " + userID);
		}
	}

	// Token: 0x06003153 RID: 12627 RVA: 0x00100C0C File Offset: 0x000FF00C
	public static void MicFilter(short[] pcmData, UIntPtr pcmDataLength, int frequency, int numChannels)
	{
		float[] array = new float[pcmData.Length];
		for (int i = 0; i < pcmData.Length; i++)
		{
			array[i] = (float)pcmData[i] / 32767f;
		}
		PlatformManager.s_instance.localAvatar.UpdateVoiceVisualization(array);
	}

	// Token: 0x06003154 RID: 12628 RVA: 0x00100C54 File Offset: 0x000FF054
	public static RemotePlayer GetRemoteUser(ulong userID)
	{
		RemotePlayer result = new RemotePlayer();
		if (PlatformManager.s_instance.remoteUsers.TryGetValue(userID, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06003155 RID: 12629 RVA: 0x00100C84 File Offset: 0x000FF084
	// Note: this type is marked as 'beforefieldinit'.
	static PlatformManager()
	{
	}

	// Token: 0x040024F6 RID: 9462
	private static readonly Vector3 START_ROTATION_ONE = new Vector3(0f, 180f, 0f);

	// Token: 0x040024F7 RID: 9463
	private static readonly Vector3 START_POSITION_ONE = new Vector3(0f, 2f, 5f);

	// Token: 0x040024F8 RID: 9464
	private static readonly Vector3 START_ROTATION_TWO = new Vector3(0f, 0f, 0f);

	// Token: 0x040024F9 RID: 9465
	private static readonly Vector3 START_POSITION_TWO = new Vector3(0f, 2f, -5f);

	// Token: 0x040024FA RID: 9466
	private static readonly Vector3 START_ROTATION_THREE = new Vector3(0f, 270f, 0f);

	// Token: 0x040024FB RID: 9467
	private static readonly Vector3 START_POSITION_THREE = new Vector3(5f, 2f, 0f);

	// Token: 0x040024FC RID: 9468
	private static readonly Vector3 START_ROTATION_FOUR = new Vector3(0f, 90f, 0f);

	// Token: 0x040024FD RID: 9469
	private static readonly Vector3 START_POSITION_FOUR = new Vector3(-5f, 2f, 0f);

	// Token: 0x040024FE RID: 9470
	private static readonly Color BLACK = new Color(0f, 0f, 0f);

	// Token: 0x040024FF RID: 9471
	private static readonly Color WHITE = new Color(1f, 1f, 1f);

	// Token: 0x04002500 RID: 9472
	private static readonly Color CYAN = new Color(0f, 1f, 1f);

	// Token: 0x04002501 RID: 9473
	private static readonly Color BLUE = new Color(0f, 0f, 1f);

	// Token: 0x04002502 RID: 9474
	private static readonly Color GREEN = new Color(0f, 1f, 0f);

	// Token: 0x04002503 RID: 9475
	public Oculus.Platform.CAPI.FilterCallback micFilterDelegate = new Oculus.Platform.CAPI.FilterCallback(PlatformManager.MicFilter);

	// Token: 0x04002504 RID: 9476
	private uint packetSequence;

	// Token: 0x04002505 RID: 9477
	public OvrAvatar localAvatarPrefab;

	// Token: 0x04002506 RID: 9478
	public OvrAvatar remoteAvatarPrefab;

	// Token: 0x04002507 RID: 9479
	public GameObject helpPanel;

	// Token: 0x04002508 RID: 9480
	protected MeshRenderer helpMesh;

	// Token: 0x04002509 RID: 9481
	public Material riftMaterial;

	// Token: 0x0400250A RID: 9482
	public Material gearMaterial;

	// Token: 0x0400250B RID: 9483
	protected OvrAvatar localAvatar;

	// Token: 0x0400250C RID: 9484
	protected GameObject localTrackingSpace;

	// Token: 0x0400250D RID: 9485
	protected GameObject localPlayerHead;

	// Token: 0x0400250E RID: 9486
	protected Dictionary<ulong, RemotePlayer> remoteUsers = new Dictionary<ulong, RemotePlayer>();

	// Token: 0x0400250F RID: 9487
	public GameObject roomSphere;

	// Token: 0x04002510 RID: 9488
	protected MeshRenderer sphereMesh;

	// Token: 0x04002511 RID: 9489
	public GameObject roomFloor;

	// Token: 0x04002512 RID: 9490
	protected MeshRenderer floorMesh;

	// Token: 0x04002513 RID: 9491
	protected PlatformManager.State currentState;

	// Token: 0x04002514 RID: 9492
	protected static PlatformManager s_instance = null;

	// Token: 0x04002515 RID: 9493
	protected RoomManager roomManager;

	// Token: 0x04002516 RID: 9494
	protected P2PManager p2pManager;

	// Token: 0x04002517 RID: 9495
	protected VoipManager voipManager;

	// Token: 0x04002518 RID: 9496
	protected ulong myID;

	// Token: 0x04002519 RID: 9497
	protected string myOculusID;

	// Token: 0x02000776 RID: 1910
	public enum State
	{
		// Token: 0x0400251B RID: 9499
		INITIALIZING,
		// Token: 0x0400251C RID: 9500
		CHECKING_LAUNCH_STATE,
		// Token: 0x0400251D RID: 9501
		CREATING_A_ROOM,
		// Token: 0x0400251E RID: 9502
		WAITING_IN_A_ROOM,
		// Token: 0x0400251F RID: 9503
		JOINING_A_ROOM,
		// Token: 0x04002520 RID: 9504
		CONNECTED_IN_A_ROOM,
		// Token: 0x04002521 RID: 9505
		LEAVING_A_ROOM,
		// Token: 0x04002522 RID: 9506
		SHUTDOWN
	}
}
