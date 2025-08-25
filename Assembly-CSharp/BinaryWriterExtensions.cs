using System;
using System.IO;
using UnityEngine;

// Token: 0x02000790 RID: 1936
internal static class BinaryWriterExtensions
{
	// Token: 0x060031C8 RID: 12744 RVA: 0x00104E38 File Offset: 0x00103238
	public static void Write(this BinaryWriter writer, OvrAvatarDriver.PoseFrame frame)
	{
		writer.Write(frame.headPosition);
		writer.Write(frame.headRotation);
		writer.Write(frame.handLeftPosition);
		writer.Write(frame.handLeftRotation);
		writer.Write(frame.handRightPosition);
		writer.Write(frame.handRightRotation);
		writer.Write(frame.voiceAmplitude);
		writer.Write(frame.controllerLeftPose);
		writer.Write(frame.controllerRightPose);
	}

	// Token: 0x060031C9 RID: 12745 RVA: 0x00104EBA File Offset: 0x001032BA
	public static void Write(this BinaryWriter writer, Vector3 vec3)
	{
		writer.Write(vec3.x);
		writer.Write(vec3.y);
		writer.Write(vec3.z);
	}

	// Token: 0x060031CA RID: 12746 RVA: 0x00104EE3 File Offset: 0x001032E3
	public static void Write(this BinaryWriter writer, Vector2 vec2)
	{
		writer.Write(vec2.x);
		writer.Write(vec2.y);
	}

	// Token: 0x060031CB RID: 12747 RVA: 0x00104EFF File Offset: 0x001032FF
	public static void Write(this BinaryWriter writer, Quaternion quat)
	{
		writer.Write(quat.x);
		writer.Write(quat.y);
		writer.Write(quat.z);
		writer.Write(quat.w);
	}

	// Token: 0x060031CC RID: 12748 RVA: 0x00104F38 File Offset: 0x00103338
	public static void Write(this BinaryWriter writer, OvrAvatarDriver.ControllerPose pose)
	{
		writer.Write((uint)pose.buttons);
		writer.Write((uint)pose.touches);
		writer.Write(pose.joystickPosition);
		writer.Write(pose.indexTrigger);
		writer.Write(pose.handTrigger);
		writer.Write(pose.isActive);
	}
}
