using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GPUTools.Cloth.Scripts.Types;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Data
{
	// Token: 0x02000989 RID: 2441
	[Serializable]
	public class ClothGeometryData
	{
		// Token: 0x06003CF2 RID: 15602 RVA: 0x00127184 File Offset: 0x00125584
		public ClothGeometryData()
		{
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x001271B8 File Offset: 0x001255B8
		public void ResetParticlesBlend()
		{
			for (int i = 0; i < this.ParticlesBlend.Length; i++)
			{
				this.ParticlesBlend[i] = 0f;
			}
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x001271EC File Offset: 0x001255EC
		public bool LoadFromBinaryReader(BinaryReader reader)
		{
			try
			{
				string a = reader.ReadString();
				if (a != "ClothGeometryData")
				{
					Debug.LogError("Binary file corrupted. Tried to read ClothGeometryData in wrong section");
					return false;
				}
				string text = reader.ReadString();
				if (text != "1.0")
				{
					Debug.LogError("ClothGeometryData schema " + text + " is not compatible with this version of software");
					return false;
				}
				int num = reader.ReadInt32();
				this.AllTringles = new int[num];
				for (int i = 0; i < num; i++)
				{
					this.AllTringles[i] = reader.ReadInt32();
				}
				int num2 = reader.ReadInt32();
				this.Particles = new Vector3[num2];
				for (int j = 0; j < num2; j++)
				{
					Vector3 vector;
					vector.x = reader.ReadSingle();
					vector.y = reader.ReadSingle();
					vector.z = reader.ReadSingle();
					this.Particles[j] = vector;
				}
				int num3 = reader.ReadInt32();
				this.MeshToPhysicsVerticesMap = new int[num3];
				for (int k = 0; k < num3; k++)
				{
					this.MeshToPhysicsVerticesMap[k] = reader.ReadInt32();
				}
				int num4 = reader.ReadInt32();
				this.PhysicsToMeshVerticesMap = new int[num4];
				for (int l = 0; l < num4; l++)
				{
					this.PhysicsToMeshVerticesMap[l] = reader.ReadInt32();
				}
				int num5 = reader.ReadInt32();
				this.JointGroups = new List<Int2ListContainer>();
				for (int m = 0; m < num5; m++)
				{
					Int2ListContainer int2ListContainer = new Int2ListContainer();
					this.JointGroups.Add(int2ListContainer);
					int num6 = reader.ReadInt32();
					int2ListContainer.List = new List<Int2>();
					for (int n = 0; n < num6; n++)
					{
						Int2 item = default(Int2);
						item.X = reader.ReadInt32();
						item.Y = reader.ReadInt32();
						int2ListContainer.List.Add(item);
					}
				}
				int num7 = reader.ReadInt32();
				this.StiffnessJointGroups = new List<Int2ListContainer>();
				for (int num8 = 0; num8 < num7; num8++)
				{
					Int2ListContainer int2ListContainer2 = new Int2ListContainer();
					this.StiffnessJointGroups.Add(int2ListContainer2);
					int num9 = reader.ReadInt32();
					int2ListContainer2.List = new List<Int2>();
					for (int num10 = 0; num10 < num9; num10++)
					{
						Int2 item2 = default(Int2);
						item2.X = reader.ReadInt32();
						item2.Y = reader.ReadInt32();
						int2ListContainer2.List.Add(item2);
					}
				}
				int num11 = reader.ReadInt32();
				this.NearbyJointGroups = new List<Int2ListContainer>();
				for (int num12 = 0; num12 < num11; num12++)
				{
					Int2ListContainer int2ListContainer3 = new Int2ListContainer();
					this.NearbyJointGroups.Add(int2ListContainer3);
					int num13 = reader.ReadInt32();
					int2ListContainer3.List = new List<Int2>();
					for (int num14 = 0; num14 < num13; num14++)
					{
						Int2 item3 = default(Int2);
						item3.X = reader.ReadInt32();
						item3.Y = reader.ReadInt32();
						int2ListContainer3.List.Add(item3);
					}
				}
				int num15 = reader.ReadInt32();
				this.ParticleToNeibor = new int[num15];
				for (int num16 = 0; num16 < num15; num16++)
				{
					this.ParticleToNeibor[num16] = reader.ReadInt32();
				}
				int num17 = reader.ReadInt32();
				this.ParticleToNeiborCounts = new int[num17];
				for (int num18 = 0; num18 < num17; num18++)
				{
					this.ParticleToNeiborCounts[num18] = reader.ReadInt32();
				}
				int num19 = reader.ReadInt32();
				this.ParticlesBlend = new float[num19];
				for (int num20 = 0; num20 < num19; num20++)
				{
					this.ParticlesBlend[num20] = reader.ReadSingle();
				}
				int num21 = reader.ReadInt32();
				this.ParticlesStrength = new float[num21];
				for (int num22 = 0; num22 < num21; num22++)
				{
					this.ParticlesStrength[num22] = reader.ReadSingle();
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Error while loading ClothGeometryData from binary reader " + arg);
				return false;
			}
			return true;
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x00127660 File Offset: 0x00125A60
		public bool StoreToBinaryWriter(BinaryWriter writer)
		{
			try
			{
				writer.Write("ClothGeometryData");
				writer.Write("1.0");
				writer.Write(this.AllTringles.Length);
				for (int i = 0; i < this.AllTringles.Length; i++)
				{
					writer.Write(this.AllTringles[i]);
				}
				writer.Write(this.Particles.Length);
				for (int j = 0; j < this.Particles.Length; j++)
				{
					writer.Write(this.Particles[j].x);
					writer.Write(this.Particles[j].y);
					writer.Write(this.Particles[j].z);
				}
				writer.Write(this.MeshToPhysicsVerticesMap.Length);
				for (int k = 0; k < this.MeshToPhysicsVerticesMap.Length; k++)
				{
					writer.Write(this.MeshToPhysicsVerticesMap[k]);
				}
				writer.Write(this.PhysicsToMeshVerticesMap.Length);
				for (int l = 0; l < this.PhysicsToMeshVerticesMap.Length; l++)
				{
					writer.Write(this.PhysicsToMeshVerticesMap[l]);
				}
				writer.Write(this.JointGroups.Count);
				for (int m = 0; m < this.JointGroups.Count; m++)
				{
					Int2ListContainer int2ListContainer = this.JointGroups[m];
					writer.Write(int2ListContainer.List.Count);
					for (int n = 0; n < int2ListContainer.List.Count; n++)
					{
						writer.Write(int2ListContainer.List[n].X);
						writer.Write(int2ListContainer.List[n].Y);
					}
				}
				writer.Write(this.StiffnessJointGroups.Count);
				for (int num = 0; num < this.StiffnessJointGroups.Count; num++)
				{
					Int2ListContainer int2ListContainer2 = this.StiffnessJointGroups[num];
					writer.Write(int2ListContainer2.List.Count);
					for (int num2 = 0; num2 < int2ListContainer2.List.Count; num2++)
					{
						writer.Write(int2ListContainer2.List[num2].X);
						writer.Write(int2ListContainer2.List[num2].Y);
					}
				}
				writer.Write(this.NearbyJointGroups.Count);
				for (int num3 = 0; num3 < this.NearbyJointGroups.Count; num3++)
				{
					Int2ListContainer int2ListContainer3 = this.NearbyJointGroups[num3];
					writer.Write(int2ListContainer3.List.Count);
					for (int num4 = 0; num4 < int2ListContainer3.List.Count; num4++)
					{
						writer.Write(int2ListContainer3.List[num4].X);
						writer.Write(int2ListContainer3.List[num4].Y);
					}
				}
				writer.Write(this.ParticleToNeibor.Length);
				for (int num5 = 0; num5 < this.ParticleToNeibor.Length; num5++)
				{
					writer.Write(this.ParticleToNeibor[num5]);
				}
				writer.Write(this.ParticleToNeiborCounts.Length);
				for (int num6 = 0; num6 < this.ParticleToNeiborCounts.Length; num6++)
				{
					writer.Write(this.ParticleToNeiborCounts[num6]);
				}
				writer.Write(this.ParticlesBlend.Length);
				for (int num7 = 0; num7 < this.ParticlesBlend.Length; num7++)
				{
					writer.Write(this.ParticlesBlend[num7]);
				}
				writer.Write(this.ParticlesStrength.Length);
				for (int num8 = 0; num8 < this.ParticlesStrength.Length; num8++)
				{
					writer.Write(this.ParticlesStrength[num8]);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Error while storeing ClothGeometryData to binary writer " + arg);
				return false;
			}
			return true;
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x00127AC0 File Offset: 0x00125EC0
		public void LogStatistics()
		{
			Debug.Log("Vertices Num: " + this.MeshToPhysicsVerticesMap.Length);
			Debug.Log("Physics Vertices Num: " + this.Particles.Length);
			Debug.Log("Mesh Vertex To Neibor Num: " + this.ParticleToNeibor.Length);
			IEnumerable<Int2ListContainer> jointGroups = this.JointGroups;
			if (ClothGeometryData.<>f__am$cache0 == null)
			{
				ClothGeometryData.<>f__am$cache0 = new Func<Int2ListContainer, int>(ClothGeometryData.<LogStatistics>m__0);
			}
			int num = jointGroups.Sum(ClothGeometryData.<>f__am$cache0);
			Debug.Log("Joints Num: " + num);
			IEnumerable<Int2ListContainer> stiffnessJointGroups = this.StiffnessJointGroups;
			if (ClothGeometryData.<>f__am$cache1 == null)
			{
				ClothGeometryData.<>f__am$cache1 = new Func<Int2ListContainer, int>(ClothGeometryData.<LogStatistics>m__1);
			}
			int num2 = stiffnessJointGroups.Sum(ClothGeometryData.<>f__am$cache1);
			Debug.Log("Stiffness Joints Num: " + num2);
			IEnumerable<Int2ListContainer> nearbyJointGroups = this.NearbyJointGroups;
			if (ClothGeometryData.<>f__am$cache2 == null)
			{
				ClothGeometryData.<>f__am$cache2 = new Func<Int2ListContainer, int>(ClothGeometryData.<LogStatistics>m__2);
			}
			int num3 = nearbyJointGroups.Sum(ClothGeometryData.<>f__am$cache2);
			Debug.Log("Nearby Joints Num: " + num3);
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x00127BDB File Offset: 0x00125FDB
		[CompilerGenerated]
		private static int <LogStatistics>m__0(Int2ListContainer container)
		{
			return container.List.Count;
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x00127BE8 File Offset: 0x00125FE8
		[CompilerGenerated]
		private static int <LogStatistics>m__1(Int2ListContainer container)
		{
			return container.List.Count;
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x00127BF5 File Offset: 0x00125FF5
		[CompilerGenerated]
		private static int <LogStatistics>m__2(Int2ListContainer container)
		{
			return container.List.Count;
		}

		// Token: 0x04002EEC RID: 12012
		[NonSerialized]
		public string status = string.Empty;

		// Token: 0x04002EED RID: 12013
		public int[] AllTringles;

		// Token: 0x04002EEE RID: 12014
		public Vector3[] Particles;

		// Token: 0x04002EEF RID: 12015
		public int[] MeshToPhysicsVerticesMap;

		// Token: 0x04002EF0 RID: 12016
		public int[] PhysicsToMeshVerticesMap;

		// Token: 0x04002EF1 RID: 12017
		public List<Int2ListContainer> JointGroups = new List<Int2ListContainer>();

		// Token: 0x04002EF2 RID: 12018
		public List<Int2ListContainer> StiffnessJointGroups = new List<Int2ListContainer>();

		// Token: 0x04002EF3 RID: 12019
		public List<Int2ListContainer> NearbyJointGroups = new List<Int2ListContainer>();

		// Token: 0x04002EF4 RID: 12020
		public int[] ParticleToNeibor;

		// Token: 0x04002EF5 RID: 12021
		public int[] ParticleToNeiborCounts;

		// Token: 0x04002EF6 RID: 12022
		public float[] ParticlesBlend;

		// Token: 0x04002EF7 RID: 12023
		public float[] ParticlesStrength;

		// Token: 0x04002EF8 RID: 12024
		public bool IsProcessed;

		// Token: 0x04002EF9 RID: 12025
		[CompilerGenerated]
		private static Func<Int2ListContainer, int> <>f__am$cache0;

		// Token: 0x04002EFA RID: 12026
		[CompilerGenerated]
		private static Func<Int2ListContainer, int> <>f__am$cache1;

		// Token: 0x04002EFB RID: 12027
		[CompilerGenerated]
		private static Func<Int2ListContainer, int> <>f__am$cache2;
	}
}
