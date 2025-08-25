using System;
using System.IO;
using UnityEngine;

// Token: 0x02000791 RID: 1937
internal static class BinaryReaderExtensions
{
	// Token: 0x060031CD RID: 12749 RVA: 0x00104F94 File Offset: 0x00103394
	public static OvrAvatarDriver.PoseFrame ReadPoseFrame(this BinaryReader reader)
	{
		return new OvrAvatarDriver.PoseFrame
		{
			headPosition = reader.ReadVector3(),
			headRotation = reader.ReadQuaternion(),
			handLeftPosition = reader.ReadVector3(),
			handLeftRotation = reader.ReadQuaternion(),
			handRightPosition = reader.ReadVector3(),
			handRightRotation = reader.ReadQuaternion(),
			voiceAmplitude = reader.ReadSingle(),
			controllerLeftPose = reader.ReadControllerPose(),
			controllerRightPose = reader.ReadControllerPose()
		};
	}

	// Token: 0x060031CE RID: 12750 RVA: 0x00105020 File Offset: 0x00103420
	public static Vector2 ReadVector2(this BinaryReader reader)
	{
		return new Vector2
		{
			x = reader.ReadSingle(),
			y = reader.ReadSingle()
		};
	}

	// Token: 0x060031CF RID: 12751 RVA: 0x00105050 File Offset: 0x00103450
	public static Vector3 ReadVector3(this BinaryReader reader)
	{
		return new Vector3
		{
			x = reader.ReadSingle(),
			y = reader.ReadSingle(),
			z = reader.ReadSingle()
		};
	}

	// Token: 0x060031D0 RID: 12752 RVA: 0x00105090 File Offset: 0x00103490
	public static Quaternion ReadQuaternion(this BinaryReader reader)
	{
		return new Quaternion
		{
			x = reader.ReadSingle(),
			y = reader.ReadSingle(),
			z = reader.ReadSingle(),
			w = reader.ReadSingle()
		};
	}

	// Token: 0x060031D1 RID: 12753 RVA: 0x001050DC File Offset: 0x001034DC
	public static OvrAvatarDriver.ControllerPose ReadControllerPose(this BinaryReader reader)
	{
		return new OvrAvatarDriver.ControllerPose
		{
			buttons = (ovrAvatarButton)reader.ReadUInt32(),
			touches = (ovrAvatarTouch)reader.ReadUInt32(),
			joystickPosition = reader.ReadVector2(),
			indexTrigger = reader.ReadSingle(),
			handTrigger = reader.ReadSingle(),
			isActive = reader.ReadBoolean()
		};
	}
}
