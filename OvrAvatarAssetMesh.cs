using System;
using System.Runtime.InteropServices;
using Oculus.Avatar;
using UnityEngine;

// Token: 0x02000782 RID: 1922
public class OvrAvatarAssetMesh : OvrAvatarAsset
{
	// Token: 0x06003197 RID: 12695 RVA: 0x00102838 File Offset: 0x00100C38
	public OvrAvatarAssetMesh(ulong _assetId, IntPtr asset)
	{
		this.assetID = _assetId;
		ovrAvatarMeshAssetData ovrAvatarMeshAssetData = CAPI.ovrAvatarAsset_GetMeshData(asset);
		this.mesh = new Mesh();
		this.mesh.name = "Procedural Geometry for asset " + _assetId;
		long num = (long)((ulong)ovrAvatarMeshAssetData.vertexCount);
		Vector3[] array = new Vector3[num];
		Vector3[] array2 = new Vector3[num];
		Vector4[] array3 = new Vector4[num];
		Vector2[] array4 = new Vector2[num];
		Color[] array5 = new Color[num];
		BoneWeight[] array6 = new BoneWeight[num];
		long num2 = (long)Marshal.SizeOf(typeof(ovrAvatarMeshVertex));
		long num3 = ovrAvatarMeshAssetData.vertexBuffer.ToInt64();
		for (long num4 = 0L; num4 < num; num4 += 1L)
		{
			long num5 = num2 * num4;
			ovrAvatarMeshVertex ovrAvatarMeshVertex = (ovrAvatarMeshVertex)Marshal.PtrToStructure(new IntPtr(num3 + num5), typeof(ovrAvatarMeshVertex));
			array[(int)(checked((IntPtr)num4))] = new Vector3(ovrAvatarMeshVertex.x, ovrAvatarMeshVertex.y, -ovrAvatarMeshVertex.z);
			array2[(int)(checked((IntPtr)num4))] = new Vector3(ovrAvatarMeshVertex.nx, ovrAvatarMeshVertex.ny, -ovrAvatarMeshVertex.nz);
			array3[(int)(checked((IntPtr)num4))] = new Vector4(ovrAvatarMeshVertex.tx, ovrAvatarMeshVertex.ty, -ovrAvatarMeshVertex.tz, ovrAvatarMeshVertex.tw);
			checked
			{
				array4[(int)((IntPtr)num4)] = new Vector2(ovrAvatarMeshVertex.u, ovrAvatarMeshVertex.v);
				array5[(int)((IntPtr)num4)] = new Color(0f, 0f, 0f, 1f);
				array6[(int)((IntPtr)num4)].boneIndex0 = (int)ovrAvatarMeshVertex.blendIndices[0];
				array6[(int)((IntPtr)num4)].boneIndex1 = (int)ovrAvatarMeshVertex.blendIndices[1];
				array6[(int)((IntPtr)num4)].boneIndex2 = (int)ovrAvatarMeshVertex.blendIndices[2];
				array6[(int)((IntPtr)num4)].boneIndex3 = (int)ovrAvatarMeshVertex.blendIndices[3];
				array6[(int)((IntPtr)num4)].weight0 = ovrAvatarMeshVertex.blendWeights[0];
				array6[(int)((IntPtr)num4)].weight1 = ovrAvatarMeshVertex.blendWeights[1];
				array6[(int)((IntPtr)num4)].weight2 = ovrAvatarMeshVertex.blendWeights[2];
				array6[(int)((IntPtr)num4)].weight3 = ovrAvatarMeshVertex.blendWeights[3];
			}
		}
		this.mesh.vertices = array;
		this.mesh.normals = array2;
		this.mesh.uv = array4;
		this.mesh.tangents = array3;
		this.mesh.boneWeights = array6;
		this.mesh.colors = array5;
		this.skinnedBindPose = ovrAvatarMeshAssetData.skinnedBindPose;
		ulong num6 = (ulong)ovrAvatarMeshAssetData.indexCount;
		short[] array7 = new short[num6];
		IntPtr indexBuffer = ovrAvatarMeshAssetData.indexBuffer;
		Marshal.Copy(indexBuffer, array7, 0, (int)num6);
		int[] array8 = new int[num6];
		for (ulong num7 = 0UL; num7 < num6; num7 += 3UL)
		{
			checked
			{
				array8[(int)((IntPtr)(unchecked(num7 + 2UL)))] = (int)array7[(int)((IntPtr)num7)];
				array8[(int)((IntPtr)(unchecked(num7 + 1UL)))] = (int)array7[(int)((IntPtr)(unchecked(num7 + 1UL)))];
				array8[(int)((IntPtr)num7)] = (int)array7[(int)((IntPtr)(unchecked(num7 + 2UL)))];
			}
		}
		this.mesh.triangles = array8;
		uint jointCount = this.skinnedBindPose.jointCount;
		this.jointNames = new string[jointCount];
		for (uint num8 = 0U; num8 < jointCount; num8 += 1U)
		{
			this.jointNames[(int)((UIntPtr)num8)] = Marshal.PtrToStringAnsi(this.skinnedBindPose.jointNames[(int)((UIntPtr)num8)]);
		}
	}

	// Token: 0x06003198 RID: 12696 RVA: 0x00102BDC File Offset: 0x00100FDC
	public SkinnedMeshRenderer CreateSkinnedMeshRendererOnObject(GameObject target)
	{
		SkinnedMeshRenderer skinnedMeshRenderer = target.AddComponent<SkinnedMeshRenderer>();
		skinnedMeshRenderer.sharedMesh = this.mesh;
		this.mesh.name = "AvatarMesh_" + this.assetID;
		uint jointCount = this.skinnedBindPose.jointCount;
		GameObject[] array = new GameObject[jointCount];
		Transform[] array2 = new Transform[jointCount];
		Matrix4x4[] array3 = new Matrix4x4[jointCount];
		for (uint num = 0U; num < jointCount; num += 1U)
		{
			array[(int)((UIntPtr)num)] = new GameObject();
			array2[(int)((UIntPtr)num)] = array[(int)((UIntPtr)num)].transform;
			array[(int)((UIntPtr)num)].name = this.jointNames[(int)((UIntPtr)num)];
			int num2 = this.skinnedBindPose.jointParents[(int)((UIntPtr)num)];
			if (num2 == -1)
			{
				array[(int)((UIntPtr)num)].transform.parent = skinnedMeshRenderer.transform;
				skinnedMeshRenderer.rootBone = array[(int)((UIntPtr)num)].transform;
			}
			else
			{
				array[(int)((UIntPtr)num)].transform.parent = array[num2].transform;
			}
			Vector3 position = this.skinnedBindPose.jointTransform[(int)((UIntPtr)num)].position;
			position.z = -position.z;
			array[(int)((UIntPtr)num)].transform.localPosition = position;
			Quaternion orientation = this.skinnedBindPose.jointTransform[(int)((UIntPtr)num)].orientation;
			orientation.x = -orientation.x;
			orientation.y = -orientation.y;
			array[(int)((UIntPtr)num)].transform.localRotation = orientation;
			array[(int)((UIntPtr)num)].transform.localScale = this.skinnedBindPose.jointTransform[(int)((UIntPtr)num)].scale;
			array3[(int)((UIntPtr)num)] = array[(int)((UIntPtr)num)].transform.worldToLocalMatrix * skinnedMeshRenderer.transform.localToWorldMatrix;
		}
		skinnedMeshRenderer.bones = array2;
		this.mesh.bindposes = array3;
		return skinnedMeshRenderer;
	}

	// Token: 0x0400256F RID: 9583
	public Mesh mesh;

	// Token: 0x04002570 RID: 9584
	private ovrAvatarSkinnedMeshPose skinnedBindPose;

	// Token: 0x04002571 RID: 9585
	public string[] jointNames;
}
