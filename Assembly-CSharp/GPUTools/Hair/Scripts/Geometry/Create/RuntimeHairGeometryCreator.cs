using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Geometry.Tools;
using GPUTools.Hair.Scripts.Types;
using GPUTools.Hair.Scripts.Utils;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
	// Token: 0x020009F7 RID: 2551
	public class RuntimeHairGeometryCreator : GeometryProviderBase
	{
		// Token: 0x06004064 RID: 16484 RVA: 0x00132209 File Offset: 0x00130609
		public RuntimeHairGeometryCreator()
		{
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x00132248 File Offset: 0x00130648
		public void StoreToBinaryWriter(BinaryWriter binWriter)
		{
			binWriter.Write("RuntimeHairGeometryCreator");
			if (this.rigidities == null)
			{
				binWriter.Write("1.0");
			}
			else
			{
				binWriter.Write("1.1");
			}
			binWriter.Write(this._ScalpProviderName);
			binWriter.Write(this._Segments);
			binWriter.Write(this.SegmentLength);
			this.strandsMask.StoreToBinaryWriter(binWriter);
			binWriter.Write(this.strands.Length);
			for (int i = 0; i < this.strands.Length; i++)
			{
				this.strands[i].StoreToBinaryWriter(binWriter);
			}
			binWriter.Write(this.indices.Length);
			for (int j = 0; j < this.indices.Length; j++)
			{
				binWriter.Write(this.indices[j]);
			}
			binWriter.Write(this.vertices.Count);
			for (int k = 0; k < this.vertices.Count; k++)
			{
				binWriter.Write(this.vertices[k].x);
				binWriter.Write(this.vertices[k].y);
				binWriter.Write(this.vertices[k].z);
			}
			if (this.rigidities != null)
			{
				binWriter.Write(this.rigidities.Count);
				for (int l = 0; l < this.rigidities.Count; l++)
				{
					binWriter.Write(this.rigidities[l]);
				}
			}
			binWriter.Write(this.hairRootToScalpIndices.Length);
			for (int m = 0; m < this.hairRootToScalpIndices.Length; m++)
			{
				binWriter.Write(this.hairRootToScalpIndices[m]);
			}
			if (this.nearbyVertexGroups != null)
			{
				binWriter.Write(this.nearbyVertexGroups.Count);
				for (int n = 0; n < this.nearbyVertexGroups.Count; n++)
				{
					List<Vector4> list = this.nearbyVertexGroups[n].List;
					binWriter.Write(list.Count);
					for (int num = 0; num < list.Count; num++)
					{
						binWriter.Write(list[num].x);
						binWriter.Write(list[num].y);
						binWriter.Write(list[num].z);
						binWriter.Write(list[num].w);
					}
				}
			}
			else
			{
				binWriter.Write(0);
			}
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0013250C File Offset: 0x0013090C
		public void StoreAuxToBinaryWriter(BinaryWriter binWriter)
		{
			this._usingAuxData = true;
			binWriter.Write("RuntimeHairGeometryCreatorAux");
			if (this.rigidities == null)
			{
				binWriter.Write("1.0");
			}
			else
			{
				binWriter.Write("1.1");
			}
			binWriter.Write(this.vertices.Count);
			for (int i = 0; i < this.vertices.Count; i++)
			{
				binWriter.Write(this.vertices[i].x);
				binWriter.Write(this.vertices[i].y);
				binWriter.Write(this.vertices[i].z);
			}
			if (this.rigidities != null)
			{
				binWriter.Write(this.rigidities.Count);
				for (int j = 0; j < this.rigidities.Count; j++)
				{
					binWriter.Write(this.rigidities[j]);
				}
			}
			if (this.nearbyVertexGroups != null)
			{
				binWriter.Write(this.nearbyVertexGroups.Count);
				for (int k = 0; k < this.nearbyVertexGroups.Count; k++)
				{
					List<Vector4> list = this.nearbyVertexGroups[k].List;
					binWriter.Write(list.Count);
					for (int l = 0; l < list.Count; l++)
					{
						binWriter.Write(list[l].x);
						binWriter.Write(list[l].y);
						binWriter.Write(list[l].z);
						binWriter.Write(list[l].w);
					}
				}
			}
			else
			{
				binWriter.Write(0);
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06004067 RID: 16487 RVA: 0x00132700 File Offset: 0x00130B00
		public bool usingAuxData
		{
			get
			{
				return this._usingAuxData;
			}
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x00132708 File Offset: 0x00130B08
		public void LoadFromBinaryReader(BinaryReader binReader)
		{
			this._usingAuxData = false;
			string a = binReader.ReadString();
			if (a != "RuntimeHairGeometryCreator")
			{
				throw new Exception("Invalid binary format for binary data passed to RuntimeHairGeometryCreator");
			}
			string text = binReader.ReadString();
			bool flag = false;
			if (text != "1.0" && text != "1.1")
			{
				throw new Exception("Invalid schema version " + text + " for binary data passed to RuntimeHairGeometryCreator");
			}
			if (text == "1.1")
			{
				flag = true;
			}
			this._ScalpProviderName = binReader.ReadString();
			bool flag2 = false;
			foreach (Transform transform in this.ScalpProviders)
			{
				if (transform.name == this._ScalpProviderName)
				{
					transform.gameObject.SetActive(true);
					PreCalcMeshProvider component = transform.GetComponent<PreCalcMeshProvider>();
					if (component != null)
					{
						flag2 = true;
						this.ScalpProvider = component;
					}
				}
				else
				{
					transform.gameObject.SetActive(false);
				}
			}
			if (!flag2)
			{
				this.ScalpProvider = null;
				throw new Exception("Could not find scalp provider " + this._ScalpProviderName);
			}
			this._Segments = binReader.ReadInt32();
			this.SegmentLength = binReader.ReadSingle();
			this.strandsMask.LoadFromBinaryReader(binReader);
			int num = binReader.ReadInt32();
			int vertexCount;
			if (this._ScalpProvider.useBaseMesh)
			{
				vertexCount = this._ScalpProvider.BaseMesh.vertexCount;
			}
			else
			{
				vertexCount = this._ScalpProvider.Mesh.vertexCount;
			}
			if (num != vertexCount)
			{
				throw new Exception("Binary hair data not compatible with chosen scalp mesh");
			}
			for (int j = 0; j < num; j++)
			{
				this.strands[j].LoadFromBinaryReader(binReader);
			}
			int num2 = binReader.ReadInt32();
			this.indices = new int[num2];
			for (int k = 0; k < num2; k++)
			{
				this.indices[k] = binReader.ReadInt32();
			}
			int num3 = binReader.ReadInt32();
			this.vertices = new List<Vector3>();
			for (int l = 0; l < num3; l++)
			{
				Vector3 item;
				item.x = binReader.ReadSingle();
				item.y = binReader.ReadSingle();
				item.z = binReader.ReadSingle();
				this.vertices.Add(item);
			}
			this.verticesLoaded = this.vertices;
			if (flag)
			{
				int num4 = binReader.ReadInt32();
				if ((float)num4 == 0f)
				{
					this.rigidities = null;
				}
				else
				{
					this.rigidities = new List<float>();
					for (int m = 0; m < num4; m++)
					{
						float item2 = binReader.ReadSingle();
						this.rigidities.Add(item2);
					}
				}
			}
			else
			{
				this.rigidities = null;
			}
			this.rigiditiesLoaded = this.rigidities;
			int num5 = binReader.ReadInt32();
			this.hairRootToScalpIndices = new int[num5];
			for (int n = 0; n < num5; n++)
			{
				this.hairRootToScalpIndices[n] = binReader.ReadInt32();
			}
			int num6 = binReader.ReadInt32();
			this.nearbyVertexGroups = new List<Vector4ListContainer>();
			for (int num7 = 0; num7 < num6; num7++)
			{
				Vector4ListContainer vector4ListContainer = new Vector4ListContainer();
				List<Vector4> list = new List<Vector4>();
				vector4ListContainer.List = list;
				this.nearbyVertexGroups.Add(vector4ListContainer);
				int num8 = binReader.ReadInt32();
				for (int num9 = 0; num9 < num8; num9++)
				{
					Vector4 item3;
					item3.x = binReader.ReadSingle();
					item3.y = binReader.ReadSingle();
					item3.z = binReader.ReadSingle();
					item3.w = binReader.ReadSingle();
					list.Add(item3);
				}
			}
			this.nearbyVertexGroupsLoaded = this.nearbyVertexGroups;
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x00132AEC File Offset: 0x00130EEC
		public void RevertToLoadedData()
		{
			if (this._usingAuxData && this.verticesLoaded != null)
			{
				this._usingAuxData = false;
				this.vertices = this.verticesLoaded;
				this.rigidities = this.rigiditiesLoaded;
				this.nearbyVertexGroups = this.nearbyVertexGroupsLoaded;
			}
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x00132B3C File Offset: 0x00130F3C
		public void LoadAuxFromBinaryReader(BinaryReader binReader)
		{
			this._usingAuxData = true;
			string a = binReader.ReadString();
			if (a != "RuntimeHairGeometryCreatorAux")
			{
				throw new Exception("Invalid aux binary format for binary data passed to RuntimeHairGeometryCreator");
			}
			string text = binReader.ReadString();
			bool flag = false;
			if (text != "1.0" && text != "1.1")
			{
				throw new Exception("Invalid schema version " + text + " for binary data passed to RuntimeHairGeometryCreator");
			}
			if (text == "1.1")
			{
				flag = true;
			}
			int num = binReader.ReadInt32();
			this.vertices = new List<Vector3>();
			for (int i = 0; i < num; i++)
			{
				Vector3 item;
				item.x = binReader.ReadSingle();
				item.y = binReader.ReadSingle();
				item.z = binReader.ReadSingle();
				this.vertices.Add(item);
			}
			if (flag)
			{
				int num2 = binReader.ReadInt32();
				if ((float)num2 == 0f)
				{
					this.rigidities = null;
				}
				else
				{
					this.rigidities = new List<float>();
					for (int j = 0; j < num2; j++)
					{
						float item2 = binReader.ReadSingle();
						this.rigidities.Add(item2);
					}
				}
			}
			else
			{
				this.rigidities = null;
			}
			int num3 = binReader.ReadInt32();
			this.nearbyVertexGroups = new List<Vector4ListContainer>();
			for (int k = 0; k < num3; k++)
			{
				Vector4ListContainer vector4ListContainer = new Vector4ListContainer();
				List<Vector4> list = new List<Vector4>();
				vector4ListContainer.List = list;
				this.nearbyVertexGroups.Add(vector4ListContainer);
				int num4 = binReader.ReadInt32();
				for (int l = 0; l < num4; l++)
				{
					Vector4 item3;
					item3.x = binReader.ReadSingle();
					item3.y = binReader.ReadSingle();
					item3.z = binReader.ReadSingle();
					item3.w = binReader.ReadSingle();
					list.Add(item3);
				}
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x0600406B RID: 16491 RVA: 0x00132D33 File Offset: 0x00131133
		// (set) Token: 0x0600406C RID: 16492 RVA: 0x00132D3B File Offset: 0x0013113B
		public int Segments
		{
			get
			{
				return this._Segments;
			}
			set
			{
				if (this._Segments != value)
				{
					this._Segments = value;
					this.Clear();
				}
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x0600406D RID: 16493 RVA: 0x00132D56 File Offset: 0x00131156
		// (set) Token: 0x0600406E RID: 16494 RVA: 0x00132D5E File Offset: 0x0013115E
		public float SegmentLength
		{
			get
			{
				return this._SegmentLength;
			}
			set
			{
				if (this._SegmentLength != value)
				{
					this._SegmentLength = value;
				}
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x0600406F RID: 16495 RVA: 0x00132D73 File Offset: 0x00131173
		public string ScalpProviderName
		{
			get
			{
				return this._ScalpProviderName;
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06004070 RID: 16496 RVA: 0x00132D7B File Offset: 0x0013117B
		public bool[] enabledIndices
		{
			get
			{
				return this._enabledIndices;
			}
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x00132D84 File Offset: 0x00131184
		protected void SyncToScalpProvider()
		{
			if (Application.isPlaying && this._ScalpProvider != null)
			{
				this._ScalpProviderName = this._ScalpProvider.name;
				int vertexCount;
				if (this._ScalpProvider.useBaseMesh)
				{
					if (this._ScalpProvider.BaseMesh == null)
					{
						return;
					}
					vertexCount = this._ScalpProvider.BaseMesh.vertexCount;
				}
				else
				{
					if (this._ScalpProvider.Mesh == null)
					{
						return;
					}
					vertexCount = this._ScalpProvider.Mesh.vertexCount;
				}
				HashSet<int> hashSet = new HashSet<int>();
				if (this._ScalpProvider.materialsToUse != null && this._ScalpProvider.materialsToUse.Length > 0)
				{
					for (int i = 0; i < this._ScalpProvider.materialsToUse.Length; i++)
					{
						int[] array;
						if (this._ScalpProvider.useBaseMesh)
						{
							array = this._ScalpProvider.BaseMesh.GetIndices(this._ScalpProvider.materialsToUse[i]);
						}
						else
						{
							array = this._ScalpProvider.Mesh.GetIndices(this._ScalpProvider.materialsToUse[i]);
						}
						for (int j = 0; j < array.Length; j++)
						{
							hashSet.Add(array[j]);
						}
					}
				}
				else
				{
					int subMeshCount = this._ScalpProvider.Mesh.subMeshCount;
					for (int k = 0; k < subMeshCount; k++)
					{
						int[] array2;
						if (this._ScalpProvider.useBaseMesh)
						{
							array2 = this._ScalpProvider.BaseMesh.GetIndices(k);
						}
						else
						{
							array2 = this._ScalpProvider.Mesh.GetIndices(k);
						}
						for (int l = 0; l < array2.Length; l++)
						{
							hashSet.Add(array2[l]);
						}
					}
				}
				this._enabledIndices = new bool[vertexCount];
				for (int m = 0; m < vertexCount; m++)
				{
					this._enabledIndices[m] = hashSet.Contains(m);
				}
				this.strandsMask = new RuntimeHairGeometryCreator.ScalpMask(vertexCount);
				this.strandsMaskWorking = new RuntimeHairGeometryCreator.ScalpMask(vertexCount);
				this.strands = new RuntimeHairGeometryCreator.Strand[vertexCount];
				for (int n = 0; n < vertexCount; n++)
				{
					this.strands[n] = new RuntimeHairGeometryCreator.Strand(n);
				}
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06004072 RID: 16498 RVA: 0x00132FEA File Offset: 0x001313EA
		// (set) Token: 0x06004073 RID: 16499 RVA: 0x00132FF2 File Offset: 0x001313F2
		public PreCalcMeshProvider ScalpProvider
		{
			get
			{
				return this._ScalpProvider;
			}
			set
			{
				if (this._ScalpProvider != value)
				{
					this._ScalpProvider = value;
					this.SyncToScalpProvider();
					this.ClearNearbyVertexGroups();
					this.GenerateOutput();
				}
			}
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x0013301E File Offset: 0x0013141E
		private void Awake()
		{
			this.SyncToScalpProvider();
			if (Application.isPlaying && !this.isProcessed)
			{
				this.Process();
			}
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x00133041 File Offset: 0x00131441
		public void Optimize()
		{
			this.Process();
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x00133049 File Offset: 0x00131449
		public void SetDirty()
		{
			this.isProcessed = false;
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x00133052 File Offset: 0x00131452
		public void ClearNearbyVertices()
		{
			this.ClearNearbyVertexGroups();
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x0013305A File Offset: 0x0013145A
		public bool IsDirty()
		{
			return !this.isProcessed;
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x00133065 File Offset: 0x00131465
		public void MaskClearAll()
		{
			if (this.strandsMaskWorking != null)
			{
				this.strandsMaskWorking.ClearAll();
			}
			this.ApplyMaskChanges();
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x00133083 File Offset: 0x00131483
		public void MaskSetAll()
		{
			if (this.strandsMaskWorking != null)
			{
				this.strandsMaskWorking.SetAll();
			}
			this.ApplyMaskChanges();
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x001330A1 File Offset: 0x001314A1
		public void SetWorkingMaskToCurrentMask()
		{
			if (this.strandsMask != null)
			{
				this.strandsMaskWorking.CopyVerticesFrom(this.strandsMask);
			}
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x001330BF File Offset: 0x001314BF
		public void ApplyMaskChanges()
		{
			if (this.strandsMask != null)
			{
				this.strandsMask.CopyVerticesFrom(this.strandsMaskWorking);
				this.GenerateAll();
			}
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x001330E3 File Offset: 0x001314E3
		public void Clear()
		{
			this.ClearAllStrands();
			this.ClearNearbyVertexGroups();
			this.GenerateOutput();
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x001330F8 File Offset: 0x001314F8
		public void ClearAllStrands()
		{
			if (this.strands != null)
			{
				for (int i = 0; i < this.strands.Length; i++)
				{
					this.strands[i].vertices = null;
				}
			}
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x00133137 File Offset: 0x00131537
		public void GenerateAll()
		{
			this.GenerateStrands();
			this.ClearNearbyVertexGroups();
			this.GenerateOutput();
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x0013314C File Offset: 0x0013154C
		protected void GenerateStrands()
		{
			if (this._ScalpProvider != null)
			{
				Mesh mesh;
				if (this._ScalpProvider.useBaseMesh)
				{
					mesh = this._ScalpProvider.BaseMesh;
				}
				else
				{
					mesh = this._ScalpProvider.Mesh;
				}
				if (mesh != null)
				{
					Vector3[] array = mesh.vertices;
					Vector3[] normals = mesh.normals;
					for (int i = 0; i < this.strands.Length; i++)
					{
						if (this.strandsMask.vertices[i] || !this._enabledIndices[i])
						{
							this.strands[i].vertices = null;
						}
						else if (this.strands[i].vertices == null || this.strands[i].vertices.Count != this.Segments)
						{
							this.strands[i].vertices = new List<Vector3>();
							this.strands[i].vertices.Add(array[i]);
							for (int j = 1; j < this.Segments; j++)
							{
								this.strands[i].vertices.Add(array[i] + normals[i] * (float)j * this.SegmentLength);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x001332B8 File Offset: 0x001316B8
		public void GenerateOutput()
		{
			if (this.ScalpProvider != null && this.strands != null)
			{
				Mesh mesh;
				if (this._ScalpProvider.useBaseMesh)
				{
					mesh = this._ScalpProvider.BaseMesh;
				}
				else
				{
					mesh = this._ScalpProvider.Mesh;
				}
				if (mesh != null)
				{
					List<int> list = new List<int>();
					this.vertices = new List<Vector3>();
					this.rigidities = null;
					this.colors = new List<Color>();
					for (int i = 0; i < this.strands.Length; i++)
					{
						if (this.strands[i].vertices != null)
						{
							list.Add(i);
							this.colors.Add(Color.black);
							this.vertices.AddRange(this.strands[i].vertices);
						}
					}
					this.hairRootToScalpIndices = list.ToArray();
					List<List<Vector3>> list2 = new List<List<Vector3>>();
					list2.Add(this.vertices);
					float accuracy = ScalpProcessingTools.MiddleDistanceBetweenPoints(mesh) * 0.1f;
					List<int> list3 = new List<int>();
					if (this._ScalpProvider.materialsToUse != null && this._ScalpProvider.materialsToUse.Length > 0)
					{
						for (int j = 0; j < this._ScalpProvider.materialsToUse.Length; j++)
						{
							int[] array = mesh.GetIndices(this._ScalpProvider.materialsToUse[j]);
							for (int k = 0; k < array.Length; k++)
							{
								list3.Add(array[k]);
							}
						}
					}
					else
					{
						int subMeshCount = this._ScalpProvider.Mesh.subMeshCount;
						for (int l = 0; l < subMeshCount; l++)
						{
							int[] array2 = mesh.GetIndices(l);
							for (int m = 0; m < array2.Length; m++)
							{
								list3.Add(array2[m]);
							}
						}
					}
					this.indices = ScalpProcessingTools.ProcessIndices(list3, mesh.vertices.ToList<Vector3>(), list2, this.Segments, accuracy).ToArray();
				}
			}
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x001334CD File Offset: 0x001318CD
		public void Process()
		{
			if (this._ScalpProvider == null || !this._ScalpProvider.Validate(true))
			{
				return;
			}
			this.GenerateOutput();
			this.isProcessed = true;
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x001334FF File Offset: 0x001318FF
		public override void Dispatch()
		{
			if (this._ScalpProvider != null)
			{
				this._ScalpProvider.provideToWorldMatrices = true;
				this._ScalpProvider.Dispatch();
			}
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x00133529 File Offset: 0x00131929
		public override bool Validate(bool log)
		{
			return this._ScalpProvider != null && this._ScalpProvider.Validate(log);
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x0013354A File Offset: 0x0013194A
		private void OnDestroy()
		{
			if (this._ScalpProvider != null)
			{
				this._ScalpProvider.Dispose();
			}
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x00133568 File Offset: 0x00131968
		public override Bounds GetBounds()
		{
			return base.transform.TransformBounds(this.Bounds);
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x0013357B File Offset: 0x0013197B
		public override int GetSegmentsNum()
		{
			return this.Segments;
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x00133583 File Offset: 0x00131983
		public override int GetStandsNum()
		{
			return this.vertices.Count / this.Segments;
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x00133597 File Offset: 0x00131997
		public override int[] GetIndices()
		{
			return this.indices;
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x0013359F File Offset: 0x0013199F
		public override List<Vector3> GetVertices()
		{
			return this.vertices;
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x001335A8 File Offset: 0x001319A8
		public override void SetVertices(List<Vector3> verts)
		{
			this.vertices = verts;
			int num = 0;
			for (int i = 0; i < this.hairRootToScalpIndices.Length; i++)
			{
				int num2 = this.hairRootToScalpIndices[i];
				List<Vector3> list = new List<Vector3>();
				this.strands[num2].vertices = list;
				for (int j = 0; j < this.Segments; j++)
				{
					list.Add(this.vertices[num]);
					num++;
				}
			}
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x00133625 File Offset: 0x00131A25
		public override List<float> GetRigidities()
		{
			return this.rigidities;
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x0013362D File Offset: 0x00131A2D
		public override void SetRigidities(List<float> rigiditiesList)
		{
			this.rigidities = rigiditiesList;
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x00133636 File Offset: 0x00131A36
		private bool AddToHashSet(HashSet<Vector4> set, int i1, int i2, float distance, float distanceRatio)
		{
			return i1 != -1 && i2 != -1 && set.Add((i1 <= i2) ? new Vector4((float)i2, (float)i1, distance, distanceRatio) : new Vector4((float)i1, (float)i2, distance, distanceRatio));
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x00133674 File Offset: 0x00131A74
		public void ClearNearbyVertexGroups()
		{
			this.nearbyVertexGroups = new List<Vector4ListContainer>();
			Vector4ListContainer item = new Vector4ListContainer();
			this.nearbyVertexGroups.Add(item);
		}

		// Token: 0x06004090 RID: 16528 RVA: 0x0013369E File Offset: 0x00131A9E
		public void CancelCalculateNearbyVertexGroups()
		{
			this.cancelCalculateNearbyVertexGroups = true;
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x001336A7 File Offset: 0x00131AA7
		public void PrepareCalculateNearbyVertexGroups()
		{
			this.toWorldMatrix = this.GetToWorldMatrix();
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x001336B8 File Offset: 0x00131AB8
		public override void CalculateNearbyVertexGroups()
		{
			this.cancelCalculateNearbyVertexGroups = false;
			this.nearbyVertexGroups = new List<Vector4ListContainer>();
			this.status = "Preparing vertices";
			List<Vector3> list = new List<Vector3>();
			foreach (Vector3 point in this.vertices)
			{
				Vector3 item = this.toWorldMatrix.MultiplyPoint3x4(point);
				list.Add(item);
			}
			HashSet<Vector4> hashSet = new HashSet<Vector4>();
			HashSet<int> hashSet2 = new HashSet<int>();
			bool flag = this.MaxNearbyVertsPerVert == 1;
			if (this.cancelCalculateNearbyVertexGroups)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (i % this.Segments != 0)
				{
					if (!flag || !hashSet2.Contains(i))
					{
						int num = i / this.Segments;
						List<RuntimeHairGeometryCreator.VertDistance> list2 = new List<RuntimeHairGeometryCreator.VertDistance>();
						for (int j = 0; j < list.Count; j++)
						{
							if (j % this.Segments != 0)
							{
								if (!flag || !hashSet2.Contains(j))
								{
									int num2 = j / this.Segments;
									if (num != num2)
									{
										float num3 = Vector3.Distance(list[i], list[j]);
										if (num3 < this.NearbyVertexSearchDistance && num3 > this.NearbyVertexSearchMinDistance)
										{
											RuntimeHairGeometryCreator.VertDistance item2;
											item2.vert = j;
											item2.distance = num3;
											list2.Add(item2);
										}
									}
								}
							}
						}
						List<RuntimeHairGeometryCreator.VertDistance> list3 = list2;
						if (RuntimeHairGeometryCreator.<>f__am$cache0 == null)
						{
							RuntimeHairGeometryCreator.<>f__am$cache0 = new Comparison<RuntimeHairGeometryCreator.VertDistance>(RuntimeHairGeometryCreator.<CalculateNearbyVertexGroups>m__0);
						}
						list3.Sort(RuntimeHairGeometryCreator.<>f__am$cache0);
						int num4 = 0;
						foreach (RuntimeHairGeometryCreator.VertDistance vertDistance in list2)
						{
							if (num4 >= this.MaxNearbyVertsPerVert)
							{
								break;
							}
							if (flag)
							{
								hashSet2.Add(i);
								hashSet2.Add(vertDistance.vert);
							}
							this.AddToHashSet(hashSet, i, vertDistance.vert, vertDistance.distance, (this.NearbyVertexSearchDistance - vertDistance.distance) / this.NearbyVertexSearchDistance);
							num4++;
						}
						this.status = string.Concat(new object[]
						{
							"Processed ",
							i,
							" of ",
							list.Count,
							" vertices"
						});
						if (this.cancelCalculateNearbyVertexGroups)
						{
							return;
						}
					}
				}
			}
			Debug.Log("Found " + hashSet.Count + " nearby vertex pairs");
			List<Vector4> list4 = hashSet.ToList<Vector4>();
			List<HashSet<int>> list5 = new List<HashSet<int>>();
			this.status = "Converting to vertex groups";
			foreach (Vector4 item3 in list4)
			{
				bool flag2 = false;
				int item4 = (int)item3.x;
				int item5 = (int)item3.y;
				for (int k = 0; k < list5.Count; k++)
				{
					HashSet<int> hashSet3 = list5[k];
					if (!hashSet3.Contains(item4) && !hashSet3.Contains(item5))
					{
						flag2 = true;
						hashSet3.Add(item4);
						hashSet3.Add(item5);
						Vector4ListContainer vector4ListContainer = this.nearbyVertexGroups[k];
						vector4ListContainer.List.Add(item3);
						break;
					}
				}
				if (!flag2)
				{
					HashSet<int> hashSet4 = new HashSet<int>();
					list5.Add(hashSet4);
					hashSet4.Add(item4);
					hashSet4.Add(item5);
					Vector4ListContainer vector4ListContainer2 = new Vector4ListContainer();
					vector4ListContainer2.List.Add(item3);
					this.nearbyVertexGroups.Add(vector4ListContainer2);
				}
				if (this.cancelCalculateNearbyVertexGroups)
				{
					this.nearbyVertexGroups = new List<Vector4ListContainer>();
					return;
				}
			}
			this.status = "Complete";
			Debug.Log("Created " + this.nearbyVertexGroups.Count + " nearby vertex pair groups");
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x00133B58 File Offset: 0x00131F58
		public override List<Vector4ListContainer> GetNearbyVertexGroups()
		{
			return this.nearbyVertexGroups;
		}

		// Token: 0x06004094 RID: 16532 RVA: 0x00133B60 File Offset: 0x00131F60
		public override List<Color> GetColors()
		{
			return this.colors;
		}

		// Token: 0x06004095 RID: 16533 RVA: 0x00133B68 File Offset: 0x00131F68
		public override Matrix4x4 GetToWorldMatrix()
		{
			if (this._ScalpProvider != null)
			{
				return this._ScalpProvider.ToWorldMatrix;
			}
			return Matrix4x4.identity;
		}

		// Token: 0x06004096 RID: 16534 RVA: 0x00133B8C File Offset: 0x00131F8C
		public override GpuBuffer<Matrix4x4> GetTransformsBuffer()
		{
			if (this._ScalpProvider != null)
			{
				return this._ScalpProvider.ToWorldMatricesBuffer;
			}
			return null;
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x00133BAC File Offset: 0x00131FAC
		public override GpuBuffer<Vector3> GetNormalsBuffer()
		{
			if (this._ScalpProvider != null)
			{
				return this._ScalpProvider.NormalsBuffer;
			}
			return null;
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x00133BCC File Offset: 0x00131FCC
		public override int[] GetHairRootToScalpMap()
		{
			return this.hairRootToScalpIndices;
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x00133BD4 File Offset: 0x00131FD4
		private void Update()
		{
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x00133BD8 File Offset: 0x00131FD8
		private void OnDrawGizmos()
		{
			if (!this.DebugDraw || !this._ScalpProvider.Validate(false))
			{
				return;
			}
			Bounds bounds = this.GetBounds();
			Gizmos.DrawWireCube(bounds.center, bounds.size);
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x00133C1C File Offset: 0x0013201C
		[CompilerGenerated]
		private static int <CalculateNearbyVertexGroups>m__0(RuntimeHairGeometryCreator.VertDistance vd1, RuntimeHairGeometryCreator.VertDistance vd2)
		{
			return vd1.distance.CompareTo(vd2.distance);
		}

		// Token: 0x04003096 RID: 12438
		protected bool _usingAuxData;

		// Token: 0x04003097 RID: 12439
		[SerializeField]
		protected int _Segments = 5;

		// Token: 0x04003098 RID: 12440
		[SerializeField]
		protected float _SegmentLength = 0.02f;

		// Token: 0x04003099 RID: 12441
		protected string _ScalpProviderName;

		// Token: 0x0400309A RID: 12442
		protected bool[] _enabledIndices;

		// Token: 0x0400309B RID: 12443
		public Transform[] ScalpProviders;

		// Token: 0x0400309C RID: 12444
		[SerializeField]
		protected PreCalcMeshProvider _ScalpProvider;

		// Token: 0x0400309D RID: 12445
		public Bounds Bounds;

		// Token: 0x0400309E RID: 12446
		public float NearbyVertexSearchMinDistance;

		// Token: 0x0400309F RID: 12447
		public float NearbyVertexSearchDistance = 0.01f;

		// Token: 0x040030A0 RID: 12448
		public int MaxNearbyVertsPerVert = 4;

		// Token: 0x040030A1 RID: 12449
		[NonSerialized]
		public RuntimeHairGeometryCreator.ScalpMask strandsMask;

		// Token: 0x040030A2 RID: 12450
		[NonSerialized]
		public RuntimeHairGeometryCreator.ScalpMask strandsMaskWorking;

		// Token: 0x040030A3 RID: 12451
		[NonSerialized]
		public RuntimeHairGeometryCreator.Strand[] strands;

		// Token: 0x040030A4 RID: 12452
		private int[] indices;

		// Token: 0x040030A5 RID: 12453
		private List<Vector3> vertices;

		// Token: 0x040030A6 RID: 12454
		private List<Vector3> verticesLoaded;

		// Token: 0x040030A7 RID: 12455
		private List<Color> colors;

		// Token: 0x040030A8 RID: 12456
		private int[] hairRootToScalpIndices;

		// Token: 0x040030A9 RID: 12457
		private List<Vector4ListContainer> nearbyVertexGroups;

		// Token: 0x040030AA RID: 12458
		private List<Vector4ListContainer> nearbyVertexGroupsLoaded;

		// Token: 0x040030AB RID: 12459
		private List<float> rigidities;

		// Token: 0x040030AC RID: 12460
		private List<float> rigiditiesLoaded;

		// Token: 0x040030AD RID: 12461
		public bool DebugDraw;

		// Token: 0x040030AE RID: 12462
		public bool DebugDrawUnselectedGroups = true;

		// Token: 0x040030AF RID: 12463
		private bool isProcessed;

		// Token: 0x040030B0 RID: 12464
		public string status = string.Empty;

		// Token: 0x040030B1 RID: 12465
		protected bool cancelCalculateNearbyVertexGroups;

		// Token: 0x040030B2 RID: 12466
		protected Matrix4x4 toWorldMatrix;

		// Token: 0x040030B3 RID: 12467
		[CompilerGenerated]
		private static Comparison<RuntimeHairGeometryCreator.VertDistance> <>f__am$cache0;

		// Token: 0x020009F8 RID: 2552
		[Serializable]
		public class ScalpMask
		{
			// Token: 0x0600409C RID: 16540 RVA: 0x00133C31 File Offset: 0x00132031
			public ScalpMask(int vertexCount)
			{
				this.vertices = new bool[vertexCount];
				this.name = string.Empty;
			}

			// Token: 0x0600409D RID: 16541 RVA: 0x00133C50 File Offset: 0x00132050
			public void CopyVerticesFrom(RuntimeHairGeometryCreator.ScalpMask otherMask)
			{
				if (this.vertices.Length == otherMask.vertices.Length)
				{
					for (int i = 0; i < this.vertices.Length; i++)
					{
						this.vertices[i] = otherMask.vertices[i];
					}
				}
				else
				{
					Debug.LogError("Attempted to copy mask vertices from a mask that has different vertex count");
				}
			}

			// Token: 0x0600409E RID: 16542 RVA: 0x00133CAC File Offset: 0x001320AC
			public void StoreToBinaryWriter(BinaryWriter binWriter)
			{
				binWriter.Write(this.name);
				binWriter.Write(this.vertices.Length);
				for (int i = 0; i < this.vertices.Length; i++)
				{
					binWriter.Write(this.vertices[i]);
				}
			}

			// Token: 0x0600409F RID: 16543 RVA: 0x00133CFC File Offset: 0x001320FC
			public void LoadFromBinaryReader(BinaryReader binReader)
			{
				this.name = binReader.ReadString();
				int num = binReader.ReadInt32();
				this.vertices = new bool[num];
				for (int i = 0; i < num; i++)
				{
					this.vertices[i] = binReader.ReadBoolean();
				}
			}

			// Token: 0x060040A0 RID: 16544 RVA: 0x00133D48 File Offset: 0x00132148
			public void ClearAll()
			{
				for (int i = 0; i < this.vertices.Length; i++)
				{
					this.vertices[i] = false;
				}
			}

			// Token: 0x060040A1 RID: 16545 RVA: 0x00133D78 File Offset: 0x00132178
			public void SetAll()
			{
				for (int i = 0; i < this.vertices.Length; i++)
				{
					this.vertices[i] = true;
				}
			}

			// Token: 0x040030B4 RID: 12468
			public string name;

			// Token: 0x040030B5 RID: 12469
			public bool[] vertices;
		}

		// Token: 0x020009F9 RID: 2553
		[Serializable]
		public class Strand
		{
			// Token: 0x060040A2 RID: 16546 RVA: 0x00133DA7 File Offset: 0x001321A7
			public Strand(int index)
			{
				this.scalpIndex = index;
			}

			// Token: 0x060040A3 RID: 16547 RVA: 0x00133DB8 File Offset: 0x001321B8
			public void StoreToBinaryWriter(BinaryWriter binWriter)
			{
				binWriter.Write(this.scalpIndex);
				if (this.vertices != null)
				{
					binWriter.Write(this.vertices.Count);
					for (int i = 0; i < this.vertices.Count; i++)
					{
						binWriter.Write(this.vertices[i].x);
						binWriter.Write(this.vertices[i].y);
						binWriter.Write(this.vertices[i].z);
					}
				}
				else
				{
					binWriter.Write(0);
				}
			}

			// Token: 0x060040A4 RID: 16548 RVA: 0x00133E64 File Offset: 0x00132264
			public void LoadFromBinaryReader(BinaryReader binReader)
			{
				this.scalpIndex = binReader.ReadInt32();
				int num = binReader.ReadInt32();
				if (num == 0)
				{
					this.vertices = null;
				}
				else
				{
					this.vertices = new List<Vector3>();
					for (int i = 0; i < num; i++)
					{
						Vector3 item;
						item.x = binReader.ReadSingle();
						item.y = binReader.ReadSingle();
						item.z = binReader.ReadSingle();
						this.vertices.Add(item);
					}
				}
			}

			// Token: 0x040030B6 RID: 12470
			public int scalpIndex;

			// Token: 0x040030B7 RID: 12471
			public List<Vector3> vertices;
		}

		// Token: 0x020009FA RID: 2554
		protected struct VertDistance
		{
			// Token: 0x040030B8 RID: 12472
			public int vert;

			// Token: 0x040030B9 RID: 12473
			public float distance;
		}
	}
}
