using System;
using System.IO;
using MeshVR;
using MVR.FileManagement;
using UnityEngine;

// Token: 0x02000B19 RID: 2841
public class DAZSkinWrapStore : ScriptableObject, IBinaryStorable
{
	// Token: 0x06004D96 RID: 19862 RVA: 0x001B39DC File Offset: 0x001B1DDC
	public DAZSkinWrapStore()
	{
	}

	// Token: 0x06004D97 RID: 19863 RVA: 0x001B39E4 File Offset: 0x001B1DE4
	public bool LoadFromBinaryReader(BinaryReader binReader)
	{
		try
		{
			string a = binReader.ReadString();
			if (a != "DAZSkinWrapStore")
			{
				SuperController.LogError("Binary file corrupted. Tried to read DAZSkinWrapStore in wrong section");
				return false;
			}
			string text = binReader.ReadString();
			if (text != "1.0")
			{
				SuperController.LogError("DAZSkinWrapStore schema " + text + " is not compatible with this version of software");
				return false;
			}
			int num = binReader.ReadInt32();
			this.wrapVertices = new DAZSkinWrapStore.SkinWrapVert[num];
			for (int i = 0; i < num; i++)
			{
				this.wrapVertices[i] = default(DAZSkinWrapStore.SkinWrapVert);
				this.wrapVertices[i].closestTriangle = binReader.ReadInt32();
				this.wrapVertices[i].Vertex1 = binReader.ReadInt32();
				this.wrapVertices[i].Vertex2 = binReader.ReadInt32();
				this.wrapVertices[i].Vertex3 = binReader.ReadInt32();
				this.wrapVertices[i].surfaceNormalProjection = binReader.ReadSingle();
				this.wrapVertices[i].surfaceTangent1Projection = binReader.ReadSingle();
				this.wrapVertices[i].surfaceTangent2Projection = binReader.ReadSingle();
				this.wrapVertices[i].surfaceNormalWrapNormalDot = binReader.ReadSingle();
				this.wrapVertices[i].surfaceTangent1WrapNormalDot = binReader.ReadSingle();
				this.wrapVertices[i].surfaceTangent2WrapNormalDot = binReader.ReadSingle();
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Error while loading DAZSkinWrapStore from binary reader " + arg);
			return false;
		}
		return true;
	}

	// Token: 0x06004D98 RID: 19864 RVA: 0x001B3BC0 File Offset: 0x001B1FC0
	public bool LoadFromBinaryFile(string path)
	{
		bool result = false;
		try
		{
			using (FileEntryStream fileEntryStream = FileManager.OpenStream(path, true))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileEntryStream.Stream))
				{
					result = this.LoadFromBinaryReader(binaryReader);
				}
			}
		}
		catch (Exception ex)
		{
			SuperController.LogError(string.Concat(new object[]
			{
				"Error while loading DAZSkinWrapStore from binary file ",
				path,
				" ",
				ex
			}));
		}
		return result;
	}

	// Token: 0x06004D99 RID: 19865 RVA: 0x001B3C6C File Offset: 0x001B206C
	public bool StoreToBinaryWriter(BinaryWriter binWriter)
	{
		try
		{
			binWriter.Write("DAZSkinWrapStore");
			binWriter.Write("1.0");
			binWriter.Write(this.wrapVertices.Length);
			for (int i = 0; i < this.wrapVertices.Length; i++)
			{
				binWriter.Write(this.wrapVertices[i].closestTriangle);
				binWriter.Write(this.wrapVertices[i].Vertex1);
				binWriter.Write(this.wrapVertices[i].Vertex2);
				binWriter.Write(this.wrapVertices[i].Vertex3);
				binWriter.Write(this.wrapVertices[i].surfaceNormalProjection);
				binWriter.Write(this.wrapVertices[i].surfaceTangent1Projection);
				binWriter.Write(this.wrapVertices[i].surfaceTangent2Projection);
				binWriter.Write(this.wrapVertices[i].surfaceNormalWrapNormalDot);
				binWriter.Write(this.wrapVertices[i].surfaceTangent1WrapNormalDot);
				binWriter.Write(this.wrapVertices[i].surfaceTangent2WrapNormalDot);
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Error while storing DAZSkinWrapStore to binary reader " + arg);
			return false;
		}
		return true;
	}

	// Token: 0x06004D9A RID: 19866 RVA: 0x001B3DD8 File Offset: 0x001B21D8
	public bool StoreToBinaryFile(string path)
	{
		bool result = false;
		try
		{
			FileManager.AssertNotCalledFromPlugin();
			using (FileStream fileStream = FileManager.OpenStreamForCreate(path))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
				{
					result = this.StoreToBinaryWriter(binaryWriter);
				}
			}
		}
		catch (Exception ex)
		{
			SuperController.LogError(string.Concat(new object[]
			{
				"Error while storing DAZSkinWrapStore to binary file ",
				path,
				" ",
				ex
			}));
		}
		return result;
	}

	// Token: 0x04003D55 RID: 15701
	[HideInInspector]
	public DAZSkinWrapStore.SkinWrapVert[] wrapVertices;

	// Token: 0x02000B1A RID: 2842
	[Serializable]
	public struct SkinWrapVert
	{
		// Token: 0x04003D56 RID: 15702
		public int closestTriangle;

		// Token: 0x04003D57 RID: 15703
		public int Vertex1;

		// Token: 0x04003D58 RID: 15704
		public int Vertex2;

		// Token: 0x04003D59 RID: 15705
		public int Vertex3;

		// Token: 0x04003D5A RID: 15706
		public float surfaceNormalProjection;

		// Token: 0x04003D5B RID: 15707
		public float surfaceTangent1Projection;

		// Token: 0x04003D5C RID: 15708
		public float surfaceTangent2Projection;

		// Token: 0x04003D5D RID: 15709
		public float surfaceNormalWrapNormalDot;

		// Token: 0x04003D5E RID: 15710
		public float surfaceTangent1WrapNormalDot;

		// Token: 0x04003D5F RID: 15711
		public float surfaceTangent2WrapNormalDot;
	}
}
