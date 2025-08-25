using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Skinner.Scripts.Providers;
using MeshVR;
using MVR.FileManagement;
using UnityEngine;

// Token: 0x02000B12 RID: 2834
[ExecuteInEditMode]
public class DAZSkinWrap : PreCalcMeshProvider, IBinaryStorable, RenderSuspend
{
	// Token: 0x06004D2A RID: 19754 RVA: 0x001AE64C File Offset: 0x001ACA4C
	public DAZSkinWrap()
	{
	}

	// Token: 0x06004D2B RID: 19755 RVA: 0x001AE701 File Offset: 0x001ACB01
	protected void RegisterAllocatedObject(UnityEngine.Object o)
	{
		if (Application.isPlaying)
		{
			if (this.allocatedObjects == null)
			{
				this.allocatedObjects = new List<UnityEngine.Object>();
			}
			this.allocatedObjects.Add(o);
		}
	}

	// Token: 0x06004D2C RID: 19756 RVA: 0x001AE730 File Offset: 0x001ACB30
	protected void DestroyAllocatedObjects()
	{
		if (Application.isPlaying && this.allocatedObjects != null)
		{
			foreach (UnityEngine.Object obj in this.allocatedObjects)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	// Token: 0x17000AF9 RID: 2809
	// (get) Token: 0x06004D2D RID: 19757 RVA: 0x001AE7A0 File Offset: 0x001ACBA0
	public override Mesh Mesh
	{
		get
		{
			this.InitMesh(false);
			return this.mesh;
		}
	}

	// Token: 0x17000AFA RID: 2810
	// (get) Token: 0x06004D2E RID: 19758 RVA: 0x001AE7AF File Offset: 0x001ACBAF
	public override Mesh BaseMesh
	{
		get
		{
			this.InitMesh(false);
			if (this.dazMesh != null)
			{
				return this.dazMesh.baseMesh;
			}
			return null;
		}
	}

	// Token: 0x17000AFB RID: 2811
	// (get) Token: 0x06004D2F RID: 19759 RVA: 0x001AE7D6 File Offset: 0x001ACBD6
	public override Mesh MeshForImport
	{
		get
		{
			this.InitStartMesh();
			return this.startMesh;
		}
	}

	// Token: 0x17000AFC RID: 2812
	// (get) Token: 0x06004D30 RID: 19760 RVA: 0x001AE7E4 File Offset: 0x001ACBE4
	public override GpuBuffer<Matrix4x4> ToWorldMatricesBuffer
	{
		get
		{
			this.InitMesh(false);
			this.SkinWrapGPUInit();
			return this._ToWorldMatricesBuffer;
		}
	}

	// Token: 0x17000AFD RID: 2813
	// (get) Token: 0x06004D31 RID: 19761 RVA: 0x001AE7F9 File Offset: 0x001ACBF9
	// (set) Token: 0x06004D32 RID: 19762 RVA: 0x001AE80E File Offset: 0x001ACC0E
	public override GpuBuffer<Vector3> PreCalculatedVerticesBuffer
	{
		get
		{
			this.InitMesh(false);
			this.SkinWrapGPUInit();
			return this._PreCalculatedVerticesBuffer;
		}
		protected set
		{
			this._PreCalculatedVerticesBuffer = value;
		}
	}

	// Token: 0x17000AFE RID: 2814
	// (get) Token: 0x06004D33 RID: 19763 RVA: 0x001AE817 File Offset: 0x001ACC17
	// (set) Token: 0x06004D34 RID: 19764 RVA: 0x001AE82C File Offset: 0x001ACC2C
	public override GpuBuffer<Vector3> NormalsBuffer
	{
		get
		{
			this.InitMesh(false);
			this.SkinWrapGPUInit();
			return this._NormalsBuffer;
		}
		protected set
		{
			this._NormalsBuffer = value;
		}
	}

	// Token: 0x06004D35 RID: 19765 RVA: 0x001AE838 File Offset: 0x001ACC38
	protected void RunNormalTangentRecalc()
	{
		if (this.recalculateNormals && this.recalcNormalsGPU != null)
		{
			this.recalcNormalsGPU.RecalculateNormals(this._drawVerticesBuffer);
		}
		if (this.recalculateTangents && this.recalcTangentsGPU != null)
		{
			this.recalcTangentsGPU.RecalculateTangents(this._drawVerticesBuffer, this._normalsBuffer);
		}
	}

	// Token: 0x06004D36 RID: 19766 RVA: 0x001AE89C File Offset: 0x001ACC9C
	public override void Dispatch()
	{
		this.UpdateVertsGPU(this.provideToWorldMatrices);
		if (this._drawVerticesBuffer != null && this._delayedVertsBuffer != null)
		{
			this.GPUSkinWrapper.SetBuffer(this._copyKernel, "inVerts", this._drawVerticesBuffer);
			this.GPUSkinWrapper.SetBuffer(this._copyKernel, "outVerts", this._delayedVertsBuffer);
			this.GPUSkinWrapper.Dispatch(this._copyKernel, this.numVertThreadGroups, 1, 1);
			if (this.provideToWorldMatrices)
			{
				this.GPUSkinWrapper.SetBuffer(this._skinWrapCalcChangeMatricesKernel, "originalPositions", this._originalVerticesBuffer);
				this.GPUSkinWrapper.SetBuffer(this._skinWrapCalcChangeMatricesKernel, "originalNormals", this._originalNormalsBuffer);
				this.GPUSkinWrapper.SetBuffer(this._skinWrapCalcChangeMatricesKernel, "originalTangents", this._originalTangentsBuffer);
				this.GPUSkinWrapper.SetBuffer(this._skinWrapCalcChangeMatricesKernel, "currentPositions", this._drawVerticesBuffer);
				this.GPUSkinWrapper.SetBuffer(this._skinWrapCalcChangeMatricesKernel, "currentNormals", this._normalsBuffer);
				this.GPUSkinWrapper.SetBuffer(this._skinWrapCalcChangeMatricesKernel, "currentTangents", this._tangentsBuffer);
				this.GPUSkinWrapper.SetBuffer(this._skinWrapCalcChangeMatricesKernel, "vertexChangeMatrices", this._matricesBuffer);
				this.GPUSkinWrapper.Dispatch(this._skinWrapCalcChangeMatricesKernel, this.numVertThreadGroups, 1, 1);
			}
		}
	}

	// Token: 0x06004D37 RID: 19767 RVA: 0x001AEA04 File Offset: 0x001ACE04
	public override void Dispose()
	{
		this._updateDrawDisabled = false;
	}

	// Token: 0x06004D38 RID: 19768 RVA: 0x001AEA0D File Offset: 0x001ACE0D
	public override void Stop()
	{
		this._updateDrawDisabled = false;
	}

	// Token: 0x06004D39 RID: 19769 RVA: 0x001AEA18 File Offset: 0x001ACE18
	public override void PostProcessDispatch(ComputeBuffer finalVerts)
	{
		if (this.drawInPostProcess && this._wrapVerticesBuffer != null)
		{
			if (!this.provideToWorldMatrices)
			{
				if (this.smoothOuterLoops > 0 && (this.laplacianSmoothPasses > 0 || this.springSmoothPasses > 0))
				{
					this.DoSmoothingGPU(finalVerts);
				}
				else
				{
					this._drawVerticesBuffer = finalVerts;
				}
				this.mapVerticesGPU.Map(this._drawVerticesBuffer);
				this.RunNormalTangentRecalc();
			}
			this._updateDrawDisabled = true;
			this.DrawMeshGPU(true);
		}
	}

	// Token: 0x17000AFF RID: 2815
	// (get) Token: 0x06004D3A RID: 19770 RVA: 0x001AEAA4 File Offset: 0x001ACEA4
	public override Color[] VertexSimColors
	{
		get
		{
			this.InitMesh(false);
			Color[] array = new Color[this.dazMesh.numUVVertices];
			if (this.simTextures != null)
			{
				Vector2[] uv = this.dazMesh.UV;
				foreach (MeshPoly meshPoly in this.dazMesh.UVPolyList)
				{
					if (this.simTextures.Length > meshPoly.materialNum && this.simTextures[meshPoly.materialNum] != null)
					{
						for (int j = 0; j < meshPoly.vertices.Length; j++)
						{
							int num = meshPoly.vertices[j];
							Vector2 vector = uv[num];
							array[num] = this.simTextures[meshPoly.materialNum].GetPixelBilinear(vector.x, vector.y);
						}
					}
				}
			}
			return array;
		}
	}

	// Token: 0x06004D3B RID: 19771 RVA: 0x001AEB9C File Offset: 0x001ACF9C
	public bool LoadFromBinaryReader(BinaryReader binReader)
	{
		try
		{
			string a = binReader.ReadString();
			if (a != "DAZSkinWrap")
			{
				SuperController.LogError("Binary file corrupted. Tried to read DAZSkinWrap in wrong section");
				return false;
			}
			string text = binReader.ReadString();
			if (text != "1.0")
			{
				SuperController.LogError("DAZSkinWrap schema " + text + " is not compatible with this version of software");
				return false;
			}
			this.wrapName = binReader.ReadString();
			if (this.wrapStore == null)
			{
				this.wrapStore = ScriptableObject.CreateInstance<DAZSkinWrapStore>();
			}
			if (!this.wrapStore.LoadFromBinaryReader(binReader))
			{
				return false;
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Error while loading DAZSkinWrap from binary reader " + arg);
			return false;
		}
		return true;
	}

	// Token: 0x06004D3C RID: 19772 RVA: 0x001AEC74 File Offset: 0x001AD074
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
				"Error while loading DAZSkinWrap from binary file ",
				path,
				" ",
				ex
			}));
		}
		return result;
	}

	// Token: 0x06004D3D RID: 19773 RVA: 0x001AED20 File Offset: 0x001AD120
	public bool StoreToBinaryWriter(BinaryWriter binWriter)
	{
		try
		{
			binWriter.Write("DAZSkinWrap");
			binWriter.Write("1.0");
			binWriter.Write(this.wrapName);
			if (!this.wrapStore.StoreToBinaryWriter(binWriter))
			{
				return false;
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Error while storing DAZSkinWrap to binary writer " + arg);
			return false;
		}
		return true;
	}

	// Token: 0x06004D3E RID: 19774 RVA: 0x001AED98 File Offset: 0x001AD198
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
				"Error while storing DAZSkinWrap to binary file ",
				path,
				" ",
				ex
			}));
		}
		return result;
	}

	// Token: 0x17000B00 RID: 2816
	// (get) Token: 0x06004D3F RID: 19775 RVA: 0x001AEE44 File Offset: 0x001AD244
	// (set) Token: 0x06004D40 RID: 19776 RVA: 0x001AEE4C File Offset: 0x001AD24C
	public bool renderSuspend
	{
		get
		{
			return this._renderSuspend;
		}
		set
		{
			this._renderSuspend = value;
		}
	}

	// Token: 0x17000B01 RID: 2817
	// (get) Token: 0x06004D41 RID: 19777 RVA: 0x001AEE55 File Offset: 0x001AD255
	public int wrapProgress
	{
		get
		{
			return this._wrapProgress;
		}
	}

	// Token: 0x17000B02 RID: 2818
	// (get) Token: 0x06004D42 RID: 19778 RVA: 0x001AEE5D File Offset: 0x001AD25D
	public List<string> usedMorphNames
	{
		get
		{
			return this._usedMorphNames;
		}
	}

	// Token: 0x17000B03 RID: 2819
	// (get) Token: 0x06004D43 RID: 19779 RVA: 0x001AEE65 File Offset: 0x001AD265
	public List<float> usedMorphValue
	{
		get
		{
			return this._usedMorphValues;
		}
	}

	// Token: 0x06004D44 RID: 19780 RVA: 0x001AEE70 File Offset: 0x001AD270
	public void SetUsedMorphsFromCurrentMorphs()
	{
		this._usedMorphNames = new List<string>();
		this._usedMorphValues = new List<float>();
		if (this.dazMesh != null && this.dazMesh.morphBank != null)
		{
			this.dazMesh.ConnectMorphBank();
			this.dazMesh.morphBank.Init();
			foreach (DAZMorph dazmorph in this.dazMesh.morphBank.morphs)
			{
				if (dazmorph.morphValue != 0f)
				{
					this._usedMorphNames.Add(dazmorph.morphName);
					this._usedMorphValues.Add(dazmorph.morphValue);
				}
			}
		}
	}

	// Token: 0x06004D45 RID: 19781 RVA: 0x001AEF5C File Offset: 0x001AD35C
	public void SetMeshMorphsFromUsedValue()
	{
		if (this.dazMesh != null && this.dazMesh.morphBank != null)
		{
			this.dazMesh.ConnectMorphBank();
			this.dazMesh.morphBank.Init();
			foreach (DAZMorph dazmorph in this.dazMesh.morphBank.morphs)
			{
				dazmorph.morphValue = 0f;
			}
			int num = 0;
			foreach (string morphName in this._usedMorphNames)
			{
				DAZMorph morph = this.dazMesh.morphBank.GetMorph(morphName);
				if (morph != null)
				{
					morph.morphValueAdjustLimits = this._usedMorphValues[num];
				}
				num++;
			}
			this.dazMesh.ApplyMorphs(true);
		}
	}

	// Token: 0x06004D46 RID: 19782 RVA: 0x001AF08C File Offset: 0x001AD48C
	public void ClearUsedMorphs()
	{
		this._usedMorphNames = new List<string>();
		this._usedMorphValues = new List<float>();
	}

	// Token: 0x06004D47 RID: 19783 RVA: 0x001AF0A4 File Offset: 0x001AD4A4
	public void CopyMorphs()
	{
		if (this.morphCopyFrom != null && this.dazMesh != null && this.dazMesh.morphBank != null)
		{
			List<string> usedMorphNames = this.morphCopyFrom.usedMorphNames;
			List<float> usedMorphValue = this.morphCopyFrom.usedMorphValue;
			this._usedMorphNames = new List<string>();
			this._usedMorphValues = new List<float>();
			int num = 0;
			foreach (string text in usedMorphNames)
			{
				DAZMorph morph = this.dazMesh.morphBank.GetMorph(text);
				if (morph != null)
				{
					this._usedMorphNames.Add(text);
					this._usedMorphValues.Add(usedMorphValue[num]);
				}
				else
				{
					UnityEngine.Debug.LogError("Could not find morph " + text);
				}
				num++;
			}
		}
	}

	// Token: 0x06004D48 RID: 19784 RVA: 0x001AF1B0 File Offset: 0x001AD5B0
	protected void WrapThreaded()
	{
		try
		{
			if (this.dazMesh != null && this.skin != null && this.skin.dazMesh != null)
			{
				this.wrapStore.wrapVertices = new DAZSkinWrapStore.SkinWrapVert[this.dazMesh.numUVVertices];
				Vector3[] morphedBaseVertices = this.dazMesh.morphedBaseVertices;
				Vector3[] morphedUVNormals = this.dazMesh.morphedUVNormals;
				Vector3[] baseSurfaceNormals = this.skin.dazMesh.baseSurfaceNormals;
				int[] baseTriangles = this.skin.dazMesh.baseTriangles;
				Vector3[] array;
				if (this.wrapToSkinnedVertices && this.appIsPlayingThreaded)
				{
					if (this.skin.skinMethod == DAZSkinV2.SkinMethod.CPUAndGPU)
					{
						array = this.skin.rawSkinnedVerts;
					}
					else
					{
						array = this.skin.drawVerts;
					}
				}
				else if (this.wrapToMorphedVertices)
				{
					if (this.appIsPlayingThreaded)
					{
						array = this.skin.dazMesh.morphedUVVertices;
					}
					else
					{
						array = this.skin.dazMesh.morphedBaseVertices;
					}
				}
				else
				{
					array = this.skin.dazMesh.baseVertices;
				}
				for (int i = 0; i < this.dazMesh.numUVVertices; i++)
				{
					this.wrapStore.wrapVertices[i] = default(DAZSkinWrapStore.SkinWrapVert);
				}
				Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
				int[][] baseMaterialVertices = this.skin.dazMesh.baseMaterialVertices;
				bool[] array2 = this.skin.dazMesh.materialsEnabled;
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j])
					{
						for (int k = 0; k < baseMaterialVertices[j].Length; k++)
						{
							if (!dictionary.ContainsKey(baseMaterialVertices[j][k]))
							{
								dictionary.Add(baseMaterialVertices[j][k], true);
							}
						}
					}
				}
				for (int l = 0; l < this.dazMesh.numBaseVertices; l++)
				{
					if (this.killThread)
					{
						this.wrapStatus = null;
						return;
					}
					float num = (float)l * 1f / (float)this.dazMesh.numBaseVertices;
					this.threadedCount = (float)l;
					this.threadedProgress = num;
					int num2 = -1;
					float num3 = 10000f;
					int num4 = -1;
					for (int m = 0; m < baseTriangles.Length; m += 3)
					{
						num4++;
						int num5 = baseTriangles[m];
						int num6 = baseTriangles[m + 1];
						int num7 = baseTriangles[m + 2];
						if (this.vertexIndexLimit == -1 || (num5 < this.vertexIndexLimit && num6 < this.vertexIndexLimit && num7 < this.vertexIndexLimit))
						{
							if (this.wrapCheckNormals)
							{
								float num8 = Vector3.Dot(baseSurfaceNormals[num4], morphedUVNormals[l]);
								if (num8 < 0f)
								{
									goto IL_395;
								}
							}
							if (this.wrapToDisabledMaterials || (dictionary.ContainsKey(num5) && dictionary.ContainsKey(num6) && dictionary.ContainsKey(num7)))
							{
								Vector3 a = (array[num5] + array[num6] + array[num7]) * 0.33333f;
								float magnitude = (a - morphedBaseVertices[l]).magnitude;
								if (magnitude < num3)
								{
									num2 = num4;
									num3 = magnitude;
								}
							}
						}
						IL_395:;
					}
					if (num2 == -1)
					{
						UnityEngine.Debug.LogError("Could not find closest triangle during skin wrap");
					}
					DAZSkinWrapStore.SkinWrapVert skinWrapVert = this.wrapStore.wrapVertices[l];
					skinWrapVert.closestTriangle = num2;
					int num9 = num2 * 3;
					int num10 = baseTriangles[num9];
					int num11 = baseTriangles[num9 + 1];
					int num12 = baseTriangles[num9 + 2];
					float magnitude2 = (array[num10] - morphedBaseVertices[l]).magnitude;
					float magnitude3 = (array[num11] - morphedBaseVertices[l]).magnitude;
					float magnitude4 = (array[num12] - morphedBaseVertices[l]).magnitude;
					if (magnitude2 < magnitude3)
					{
						if (magnitude2 < magnitude4)
						{
							skinWrapVert.Vertex1 = num10;
							if (magnitude3 < magnitude4)
							{
								skinWrapVert.Vertex2 = num11;
								skinWrapVert.Vertex3 = num12;
							}
							else
							{
								skinWrapVert.Vertex2 = num12;
								skinWrapVert.Vertex3 = num11;
							}
						}
						else
						{
							skinWrapVert.Vertex1 = num12;
							skinWrapVert.Vertex2 = num10;
							skinWrapVert.Vertex3 = num11;
						}
					}
					else if (magnitude3 < magnitude4)
					{
						skinWrapVert.Vertex1 = num11;
						if (magnitude2 < magnitude4)
						{
							skinWrapVert.Vertex2 = num10;
							skinWrapVert.Vertex3 = num12;
						}
						else
						{
							skinWrapVert.Vertex2 = num12;
							skinWrapVert.Vertex3 = num10;
						}
					}
					else
					{
						skinWrapVert.Vertex1 = num12;
						skinWrapVert.Vertex2 = num11;
						skinWrapVert.Vertex3 = num10;
					}
					Vector3 a2 = (array[skinWrapVert.Vertex1] + array[skinWrapVert.Vertex2] + array[skinWrapVert.Vertex3]) * 0.33333f;
					Vector3 vector = a2 - array[skinWrapVert.Vertex1];
					Vector3 rhs = baseSurfaceNormals[num2];
					Vector3 rhs2 = Vector3.Cross(vector, rhs);
					Vector3 lhs = morphedBaseVertices[l] - array[skinWrapVert.Vertex1];
					skinWrapVert.surfaceNormalProjection = Vector3.Dot(lhs, rhs) / rhs.sqrMagnitude;
					skinWrapVert.surfaceTangent1Projection = Vector3.Dot(lhs, vector) / vector.sqrMagnitude;
					skinWrapVert.surfaceTangent2Projection = Vector3.Dot(lhs, rhs2) / rhs2.sqrMagnitude;
					skinWrapVert.surfaceNormalWrapNormalDot = Vector3.Dot(morphedUVNormals[l], rhs);
					skinWrapVert.surfaceTangent1WrapNormalDot = Vector3.Dot(morphedUVNormals[l], vector) / vector.sqrMagnitude;
					skinWrapVert.surfaceTangent2WrapNormalDot = Vector3.Dot(morphedUVNormals[l], rhs2) / rhs2.sqrMagnitude;
					this.wrapStore.wrapVertices[l] = skinWrapVert;
				}
				foreach (DAZVertexMap dazvertexMap in this.dazMesh.baseVerticesToUVVertices)
				{
					this.wrapStore.wrapVertices[dazvertexMap.tovert].closestTriangle = this.wrapStore.wrapVertices[dazvertexMap.fromvert].closestTriangle;
					this.wrapStore.wrapVertices[dazvertexMap.tovert].Vertex1 = this.wrapStore.wrapVertices[dazvertexMap.fromvert].Vertex1;
					this.wrapStore.wrapVertices[dazvertexMap.tovert].Vertex2 = this.wrapStore.wrapVertices[dazvertexMap.fromvert].Vertex2;
					this.wrapStore.wrapVertices[dazvertexMap.tovert].Vertex3 = this.wrapStore.wrapVertices[dazvertexMap.fromvert].Vertex3;
					this.wrapStore.wrapVertices[dazvertexMap.tovert].surfaceNormalProjection = this.wrapStore.wrapVertices[dazvertexMap.fromvert].surfaceNormalProjection;
					this.wrapStore.wrapVertices[dazvertexMap.tovert].surfaceTangent1Projection = this.wrapStore.wrapVertices[dazvertexMap.fromvert].surfaceTangent1Projection;
					this.wrapStore.wrapVertices[dazvertexMap.tovert].surfaceTangent2Projection = this.wrapStore.wrapVertices[dazvertexMap.fromvert].surfaceTangent2Projection;
					this.wrapStore.wrapVertices[dazvertexMap.tovert].surfaceNormalWrapNormalDot = this.wrapStore.wrapVertices[dazvertexMap.fromvert].surfaceNormalWrapNormalDot;
					this.wrapStore.wrapVertices[dazvertexMap.tovert].surfaceTangent1WrapNormalDot = this.wrapStore.wrapVertices[dazvertexMap.fromvert].surfaceTangent1WrapNormalDot;
					this.wrapStore.wrapVertices[dazvertexMap.tovert].surfaceTangent2WrapNormalDot = this.wrapStore.wrapVertices[dazvertexMap.fromvert].surfaceTangent2WrapNormalDot;
				}
			}
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("Exception during wrap: " + arg);
		}
	}

	// Token: 0x06004D49 RID: 19785 RVA: 0x001AFB28 File Offset: 0x001ADF28
	public void CreateFolderIfNeeded(string filename)
	{
		string directoryName = Path.GetDirectoryName(filename);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
	}

	// Token: 0x06004D4A RID: 19786 RVA: 0x001AFB50 File Offset: 0x001ADF50
	private IEnumerator WatchThread()
	{
		while (this.wrapThread.IsAlive)
		{
			this.wrapStatus = string.Concat(new object[]
			{
				"Wrapped ",
				this.threadedCount,
				" of ",
				this.dazMesh.numBaseVertices,
				" vertices"
			});
			yield return null;
		}
		this.isWrapping = false;
		yield break;
	}

	// Token: 0x17000B04 RID: 2820
	// (get) Token: 0x06004D4B RID: 19787 RVA: 0x001AFB6B File Offset: 0x001ADF6B
	public bool IsWrapping
	{
		get
		{
			return this.isWrapping;
		}
	}

	// Token: 0x17000B05 RID: 2821
	// (get) Token: 0x06004D4C RID: 19788 RVA: 0x001AFB73 File Offset: 0x001ADF73
	public string WrapStatus
	{
		get
		{
			return this.wrapStatus;
		}
	}

	// Token: 0x06004D4D RID: 19789 RVA: 0x001AFB7C File Offset: 0x001ADF7C
	public void Wrap()
	{
		if (!this.isWrapping)
		{
			this.isWrapping = true;
			this.wrapStatus = "Wrapping";
			bool flag = false;
			if (this.wrapStore != null)
			{
				flag = true;
			}
			this.wrapStore = ScriptableObject.CreateInstance<DAZSkinWrapStore>();
			this.RegisterAllocatedObject(this.wrapStore);
			bool flag2 = !Application.isPlaying || this.wrapToSkinnedVertices;
			if (flag)
			{
				this.SetMeshMorphsFromUsedValue();
			}
			else
			{
				this.SetUsedMorphsFromCurrentMorphs();
			}
			if (this.dazMesh != null && this.skin != null && this.skin.dazMesh != null)
			{
				this.dazMesh.Init();
				this.skin.dazMesh.Init();
				if (this.dazMesh.morphBank != null)
				{
					this.dazMesh.ConnectMorphBank();
					this.dazMesh.morphBank.Init();
				}
			}
			this.threadedCount = 0f;
			this.threadedProgress = 0f;
			this.killThread = false;
			this.appIsPlayingThreaded = Application.isPlaying;
			this.wrapThread = new Thread(new ThreadStart(this.WrapThreaded));
			this.wrapThread.Start();
			if (!flag2)
			{
				base.StartCoroutine(this.WatchThread());
			}
			else
			{
				while (this.wrapThread.IsAlive)
				{
					Thread.Sleep(100);
				}
				this.isWrapping = false;
			}
		}
	}

	// Token: 0x06004D4E RID: 19790 RVA: 0x001AFD08 File Offset: 0x001AE108
	protected void InitSmoothing()
	{
		if (this.meshSmooth == null && this.dazMesh != null)
		{
			this.meshSmooth = new MeshSmooth(this.dazMesh.baseVertices, this.dazMesh.basePolyList);
		}
		if (this.meshSmoothGPU == null && this.GPUMeshCompute != null && Application.isPlaying && this.dazMesh != null)
		{
			this.meshSmoothGPU = new MeshSmoothGPU(this.GPUMeshCompute, this.dazMesh.baseVertices, this.dazMesh.basePolyList);
		}
	}

	// Token: 0x06004D4F RID: 19791 RVA: 0x001AFDB0 File Offset: 0x001AE1B0
	public Mesh GetStartMesh()
	{
		return this.startMesh;
	}

	// Token: 0x06004D50 RID: 19792 RVA: 0x001AFDB8 File Offset: 0x001AE1B8
	public void InitMesh(bool force = false)
	{
		if (this.dazMesh != null && (force || !this.meshWasInit))
		{
			this.dazMesh.Init();
			this.defaultAdditionalThicknessMultiplier = this.additionalThicknessMultiplier;
			this.defaultSurfaceOffset = this.surfaceOffset;
			this.meshSmooth = null;
			this.meshSmoothGPU = null;
			this.meshWasInit = true;
			this._verts1 = (Vector3[])this.dazMesh.morphedUVVertices.Clone();
			this._verts2 = (Vector3[])this.dazMesh.morphedUVVertices.Clone();
			this.mesh = UnityEngine.Object.Instantiate<Mesh>(this.dazMesh.morphedUVMappedMesh);
			this.RegisterAllocatedObject(this.mesh);
			Vector3 size = new Vector3(10000f, 10000f, 10000f);
			Bounds bounds = new Bounds(base.transform.position, size);
			this.mesh.bounds = bounds;
			this.startMesh = UnityEngine.Object.Instantiate<Mesh>(this.dazMesh.morphedUVMappedMesh);
			this.RegisterAllocatedObject(this.startMesh);
			this.startMesh.bounds = bounds;
		}
	}

	// Token: 0x06004D51 RID: 19793 RVA: 0x001AFEDC File Offset: 0x001AE2DC
	public void InitStartMesh()
	{
		if (this.dazMesh != null && !this.startMeshWasInit && this.wrapStore != null && this.wrapStore.wrapVertices != null)
		{
			this.startMeshWasInit = true;
			this.startMesh = UnityEngine.Object.Instantiate<Mesh>(this.dazMesh.morphedUVMappedMesh);
			this.RegisterAllocatedObject(this.startMesh);
			Vector3 size = new Vector3(10000f, 10000f, 10000f);
			Bounds bounds = new Bounds(base.transform.position, size);
			this.startMesh.bounds = bounds;
			this.UpdateVerts(true);
		}
	}

	// Token: 0x17000B06 RID: 2822
	// (get) Token: 0x06004D52 RID: 19794 RVA: 0x001AFF8B File Offset: 0x001AE38B
	public int numMaterials
	{
		get
		{
			return this._numMaterials;
		}
	}

	// Token: 0x17000B07 RID: 2823
	// (get) Token: 0x06004D53 RID: 19795 RVA: 0x001AFF93 File Offset: 0x001AE393
	public string[] materialNames
	{
		get
		{
			return this._materialNames;
		}
	}

	// Token: 0x06004D54 RID: 19796 RVA: 0x001AFF9C File Offset: 0x001AE39C
	public void CopyMaterials()
	{
		if (this.dazMesh != null)
		{
			this._numMaterials = this.dazMesh.materials.Length;
			this.GPUsimpleMaterial = this.dazMesh.simpleMaterial;
			this.GPUmaterials = new Material[this._numMaterials];
			this.simTextures = new Texture2D[this._numMaterials];
			this.materialsEnabled = new bool[this._numMaterials];
			this._materialNames = new string[this._numMaterials];
			for (int i = 0; i < this._numMaterials; i++)
			{
				this.GPUmaterials[i] = this.dazMesh.materials[i];
				this.materialsEnabled[i] = this.dazMesh.materialsEnabled[i];
				this._materialNames[i] = this.dazMesh.materialNames[i];
			}
		}
	}

	// Token: 0x06004D55 RID: 19797 RVA: 0x001B0078 File Offset: 0x001AE478
	protected void InitRecalcNormalsTangents()
	{
		if (this.recalculateNormals && this.originalRecalcNormalsGPU == null && this.dazMesh != null)
		{
			this.originalRecalcNormalsGPU = new RecalculateNormalsGPU(this.GPUMeshCompute, this.dazMesh.baseTriangles, this.dazMesh.numUVVertices, this.dazMesh.baseVerticesToUVVertices);
			this._originalNormalsBuffer = this.originalRecalcNormalsGPU.normalsBuffer;
			this._originalSurfaceNormalsBuffer = this.originalRecalcNormalsGPU.surfaceNormalsBuffer;
			this.originalRecalcNormalsGPU.RecalculateNormals(this._drawVerticesBuffer);
		}
		if (this.recalculateNormals && this.recalcNormalsGPU == null && this.dazMesh != null)
		{
			this.recalcNormalsGPU = new RecalculateNormalsGPU(this.GPUMeshCompute, this.dazMesh.baseTriangles, this.dazMesh.numUVVertices, this.dazMesh.baseVerticesToUVVertices);
			this._normalsBuffer = this.recalcNormalsGPU.normalsBuffer;
			this._surfaceNormalsBuffer = this.recalcNormalsGPU.surfaceNormalsBuffer;
		}
		if (this.recalculateTangents && this.originalRecalcTangentsGPU == null && this.dazMesh != null)
		{
			this.originalRecalcTangentsGPU = new RecalculateTangentsGPU(this.GPUMeshCompute, this.dazMesh.UVTriangles, this.dazMesh.UV, this.dazMesh.numUVVertices);
			this._originalTangentsBuffer = this.originalRecalcTangentsGPU.tangentsBuffer;
			this.originalRecalcTangentsGPU.RecalculateTangents(this._drawVerticesBuffer, this._originalNormalsBuffer);
		}
		if (this.recalculateTangents && this.recalcTangentsGPU == null && this.dazMesh != null)
		{
			this.recalcTangentsGPU = new RecalculateTangentsGPU(this.GPUMeshCompute, this.dazMesh.UVTriangles, this.dazMesh.UV, this.dazMesh.numUVVertices);
			this._tangentsBuffer = this.recalcTangentsGPU.tangentsBuffer;
		}
	}

	// Token: 0x06004D56 RID: 19798 RVA: 0x001B0278 File Offset: 0x001AE678
	protected void GPURecheckSurfaceNormalOffsets()
	{
		if (this.currentMoveToSurface != this.moveToSurface || this.currentMoveToSurfaceOffset != this.moveToSurfaceOffset)
		{
			if (this.moveToSurface)
			{
				for (int i = 0; i < this.dazMesh.numBaseVertices; i++)
				{
					if (this.startingWrapStore.wrapVertices[i].surfaceNormalProjection < this.moveToSurfaceOffset)
					{
						this.wrapStore.wrapVertices[i].surfaceNormalProjection = this.moveToSurfaceOffset;
					}
					else
					{
						this.wrapStore.wrapVertices[i].surfaceNormalProjection = this.startingWrapStore.wrapVertices[i].surfaceNormalProjection;
					}
				}
			}
			else
			{
				for (int j = 0; j < this.dazMesh.numBaseVertices; j++)
				{
					this.wrapStore.wrapVertices[j].surfaceNormalProjection = this.startingWrapStore.wrapVertices[j].surfaceNormalProjection;
				}
			}
			this.currentMoveToSurface = this.moveToSurface;
			this.currentMoveToSurfaceOffset = this.moveToSurfaceOffset;
			if (this._wrapVerticesBuffer != null)
			{
				this._wrapVerticesBuffer.SetData(this.wrapStore.wrapVertices);
			}
		}
	}

	// Token: 0x06004D57 RID: 19799 RVA: 0x001B03C4 File Offset: 0x001AE7C4
	protected void SkinWrapGPUInit()
	{
		if (Application.isPlaying && this._wrapVerticesBuffer == null)
		{
			this._skinWrapKernel = this.GPUSkinWrapper.FindKernel("SkinWrap");
			this._skinWrapCalcChangeMatricesKernel = this.GPUSkinWrapper.FindKernel("SkinWrapCalcChangeMatrices");
			this._copyKernel = this.GPUSkinWrapper.FindKernel("SkinWrapCopyVerts");
			this._copyTangentsKernel = this.GPUMeshCompute.FindKernel("CopyTangents");
			int numUVVertices = this.dazMesh.numUVVertices;
			this.numVertThreadGroups = numUVVertices / 256;
			int num = numUVVertices % 256;
			if (num != 0)
			{
				this.numVertThreadGroups++;
			}
			if (this.startingWrapStore == null)
			{
				this.startingWrapStore = this.wrapStore;
				this.wrapStore = UnityEngine.Object.Instantiate<DAZSkinWrapStore>(this.wrapStore);
				this.RegisterAllocatedObject(this.wrapStore);
			}
			this.GPURecheckSurfaceNormalOffsets();
			int num2 = this.numVertThreadGroups * 256;
			this._wrapVerticesBuffer = new ComputeBuffer(num2, 40);
			this._wrapVerticesBuffer.SetData(this.wrapStore.wrapVertices);
			this._verticesBuffer1 = new ComputeBuffer(num2, 12);
			this._verticesBuffer2 = new ComputeBuffer(num2, 12);
			this._delayedVertsBuffer = new ComputeBuffer(num2, 12);
			this._drawVerticesBuffer = this._delayedVertsBuffer;
			this._drawVerticesBuffer.SetData(this.dazMesh.morphedUVVertices);
			this._delayedNormalsBuffer = new ComputeBuffer(num2, 12);
			this._delayedTangentsBuffer = new ComputeBuffer(num2, 16);
			this._originalVerticesBuffer = new ComputeBuffer(num2, 12);
			this._originalVerticesBuffer.SetData(this.dazMesh.morphedUVVertices);
			this.mapVerticesGPU = new MapVerticesGPU(this.GPUMeshCompute, this.dazMesh.baseVerticesToUVVertices);
			this.InitRecalcNormalsTangents();
			this.PreCalculatedVerticesBuffer = new GpuBuffer<Vector3>(this._delayedVertsBuffer);
			this.PreCalculatedVerticesBuffer.Data = new Vector3[num2];
			this._matricesBuffer = new ComputeBuffer(num2, 64);
			this._ToWorldMatricesBuffer = new GpuBuffer<Matrix4x4>(this._matricesBuffer);
			this.NormalsBuffer = new GpuBuffer<Vector3>(this._normalsBuffer);
			this.InitMaterials();
			if (this.GPUAutoSwapShader)
			{
				for (int i = 0; i < this.GPUmaterials.Length; i++)
				{
					Shader shader = this.GPUmaterials[i].shader;
					Shader shader2 = Shader.Find(shader.name + "ComputeBuff");
					int renderQueue = this.GPUmaterials[i].renderQueue;
					if (shader2 != null)
					{
						this.GPUmaterials[i].shader = shader2;
						this.GPUmaterials[i].renderQueue = renderQueue;
					}
				}
			}
		}
	}

	// Token: 0x06004D58 RID: 19800 RVA: 0x001B0670 File Offset: 0x001AEA70
	protected void DetermineUsedSkinVerts()
	{
		if (this.usedSkinVerts == null && this.dazMesh != null)
		{
			Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
			for (int i = 0; i < this.dazMesh.numBaseVertices; i++)
			{
				DAZSkinWrapStore.SkinWrapVert skinWrapVert = this.wrapStore.wrapVertices[i];
				if (!dictionary.ContainsKey(skinWrapVert.Vertex1))
				{
					dictionary.Add(skinWrapVert.Vertex1, true);
				}
				if (!dictionary.ContainsKey(skinWrapVert.Vertex2))
				{
					dictionary.Add(skinWrapVert.Vertex2, true);
				}
				if (!dictionary.ContainsKey(skinWrapVert.Vertex3))
				{
					dictionary.Add(skinWrapVert.Vertex3, true);
				}
			}
			this.usedSkinVerts = new List<int>(dictionary.Keys);
		}
	}

	// Token: 0x06004D59 RID: 19801 RVA: 0x001B0744 File Offset: 0x001AEB44
	protected void UpdatePostSkinVerts()
	{
		this.DetermineUsedSkinVerts();
		foreach (int num in this.usedSkinVerts)
		{
			if (!this.skin.postSkinVerts[num])
			{
				this.skin.postSkinVerts[num] = true;
				this.skin.postSkinVertsChanged = true;
			}
			if (!this.skin.postSkinNormalVerts[num])
			{
				this.skin.postSkinNormalVerts[num] = true;
				this.skin.postSkinVertsChanged = true;
			}
		}
	}

	// Token: 0x06004D5A RID: 19802 RVA: 0x001B07F8 File Offset: 0x001AEBF8
	public void UpdateVerts(bool updateStartMesh = false)
	{
		this.InitMesh(false);
		Vector3[] array;
		Vector3[] array2;
		if (Application.isPlaying)
		{
			if (updateStartMesh)
			{
				array = this.skin.dazMesh.baseVertices;
				array2 = this.skin.dazMesh.baseSurfaceNormals;
			}
			else if (this.skin.skinMethod == DAZSkinV2.SkinMethod.CPUAndGPU)
			{
				this.UpdatePostSkinVerts();
				array = this.skin.rawSkinnedVerts;
				array2 = this.skin.drawSurfaceNormals;
			}
			else if (this.skin.skin)
			{
				array = this.skin.drawVerts;
				array2 = this.skin.drawSurfaceNormals;
			}
			else
			{
				array = this.skin.dazMesh.morphedUVVertices;
				array2 = this.skin.dazMesh.baseSurfaceNormals;
			}
		}
		else
		{
			array = this.skin.dazMesh.morphedUVVertices;
			array2 = this.skin.dazMesh.baseSurfaceNormals;
		}
		int[] baseTriangles = this.skin.dazMesh.baseTriangles;
		bool flag = true;
		for (int i = 0; i < this._numMaterials; i++)
		{
			if (!this.dazMesh.materialsEnabled[i])
			{
				flag = false;
				break;
			}
		}
		float x = base.transform.lossyScale.x;
		float d = 1f / x;
		if (flag || !this.onlyUpdateEnabledMaterials || this.dazMesh.baseMaterialVertices == null)
		{
			for (int j = 0; j < this.dazMesh.numBaseVertices; j++)
			{
				DAZSkinWrapStore.SkinWrapVert skinWrapVert = this.wrapStore.wrapVertices[j];
				int closestTriangle = skinWrapVert.closestTriangle;
				Vector3 vector = array[skinWrapVert.Vertex1];
				Vector3 vector2 = (array[skinWrapVert.Vertex1] + array[skinWrapVert.Vertex2] + array[skinWrapVert.Vertex3]) * 0.33333f;
				Vector3 vector3 = (vector2 - vector) * d;
				Vector3 vector4 = array2[closestTriangle];
				Vector3 vector5 = Vector3.Cross(vector3, vector4);
				float surfaceNormalProjection = skinWrapVert.surfaceNormalProjection;
				if (this.moveToSurface && surfaceNormalProjection < this.moveToSurfaceOffset)
				{
					surfaceNormalProjection = this.moveToSurfaceOffset;
				}
				Vector3 vector6 = vector + vector3 * x * (skinWrapVert.surfaceTangent1Projection + skinWrapVert.surfaceTangent1WrapNormalDot * this.additionalThicknessMultiplier) + vector5 * x * (skinWrapVert.surfaceTangent2Projection + skinWrapVert.surfaceTangent2WrapNormalDot * this.additionalThicknessMultiplier) + vector4 * x * (surfaceNormalProjection + this.surfaceOffset + skinWrapVert.surfaceNormalWrapNormalDot * this.additionalThicknessMultiplier);
				this._verts1[j] = vector6;
				if (this.debug && j >= this.debugStartVert && j <= this.debugStopVert)
				{
					this.debugTan1 = skinWrapVert.surfaceTangent1Projection;
					this.debugTan2 = skinWrapVert.surfaceTangent2Projection;
					MyDebug.DrawWireCube(vector2, this.debugSize * 0.5f, Color.cyan);
					MyDebug.DrawWireCube(vector6, this.debugSize * 0.6f, Color.gray);
					MyDebug.DrawWireCube(vector, this.debugSize, Color.blue);
					MyDebug.DrawWireCube(this._verts1[j], this.debugSize, Color.green);
					UnityEngine.Debug.DrawLine(vector, this._verts1[j], Color.yellow);
					UnityEngine.Debug.DrawLine(vector, vector2, Color.yellow);
					UnityEngine.Debug.DrawLine(vector, vector + vector3, Color.red);
					UnityEngine.Debug.DrawLine(vector, vector + vector5, Color.red);
				}
			}
		}
		else
		{
			int[][] baseMaterialVertices = this.dazMesh.baseMaterialVertices;
			for (int k = 0; k < this._numMaterials; k++)
			{
				if (this.dazMesh.materialsEnabled[k])
				{
					for (int l = 0; l < baseMaterialVertices[k].Length; l++)
					{
						int num = baseMaterialVertices[k][l];
						DAZSkinWrapStore.SkinWrapVert skinWrapVert2 = this.wrapStore.wrapVertices[num];
						int closestTriangle2 = skinWrapVert2.closestTriangle;
						Vector3 vector7 = array[skinWrapVert2.Vertex1];
						Vector3 a = (array[skinWrapVert2.Vertex1] + array[skinWrapVert2.Vertex2] + array[skinWrapVert2.Vertex3]) * 0.33333f;
						Vector3 vector8 = (a - vector7) * d;
						Vector3 vector9 = array2[closestTriangle2];
						Vector3 a2 = Vector3.Cross(vector8, vector9);
						float surfaceNormalProjection2 = skinWrapVert2.surfaceNormalProjection;
						if (this.moveToSurface && surfaceNormalProjection2 < this.moveToSurfaceOffset)
						{
							surfaceNormalProjection2 = this.moveToSurfaceOffset;
						}
						Vector3 vector10 = vector7 + vector8 * x * (skinWrapVert2.surfaceTangent1Projection + skinWrapVert2.surfaceTangent1WrapNormalDot * this.additionalThicknessMultiplier) + a2 * x * (skinWrapVert2.surfaceTangent2Projection + skinWrapVert2.surfaceTangent2WrapNormalDot * this.additionalThicknessMultiplier) + vector9 * x * (surfaceNormalProjection2 + this.surfaceOffset + skinWrapVert2.surfaceNormalWrapNormalDot * this.additionalThicknessMultiplier);
						this._verts1[num] = vector10;
					}
				}
			}
		}
		if (this.smoothOuterLoops > 0 && (this.laplacianSmoothPasses > 0 || this.springSmoothPasses > 0))
		{
			this.InitSmoothing();
			if (this.meshSmooth != null)
			{
				int num2 = 0;
				for (int m = 0; m < this.smoothOuterLoops; m++)
				{
					for (int n = 0; n < this.laplacianSmoothPasses; n++)
					{
						if (num2 % 2 == 0)
						{
							this.meshSmooth.LaplacianSmooth(this._verts1, this._verts2, 0, 100000000);
							this.meshSmooth.HCCorrection(this._verts1, this._verts2, this.laplacianSmoothBeta, 0, 1000000000);
							this._drawVerts = this._verts2;
						}
						else
						{
							this.meshSmooth.LaplacianSmooth(this._verts2, this._verts1, 0, 100000000);
							this.meshSmooth.HCCorrection(this._verts2, this._verts1, this.laplacianSmoothBeta, 0, 1000000000);
							this._drawVerts = this._verts1;
						}
						num2++;
					}
					for (int num3 = 0; num3 < this.springSmoothPasses; num3++)
					{
						if (num2 % 2 == 0)
						{
							this.meshSmooth.SpringSmooth(this._verts1, this._verts2, this.springSmoothFactor, x, 0, 100000000);
							this._drawVerts = this._verts2;
						}
						else
						{
							this.meshSmooth.SpringSmooth(this._verts2, this._verts1, this.springSmoothFactor, x, 0, 100000000);
							this._drawVerts = this._verts1;
						}
						num2++;
					}
				}
			}
			else
			{
				this._drawVerts = this._verts1;
			}
		}
		else
		{
			this._drawVerts = this._verts1;
		}
		foreach (DAZVertexMap dazvertexMap in this.dazMesh.baseVerticesToUVVertices)
		{
			this._drawVerts[dazvertexMap.tovert] = this._drawVerts[dazvertexMap.fromvert];
		}
		if (updateStartMesh)
		{
			this.startMesh.vertices = this._drawVerts;
		}
		else
		{
			this.mesh.vertices = this._drawVerts;
		}
	}

	// Token: 0x06004D5B RID: 19803 RVA: 0x001B1024 File Offset: 0x001AF424
	protected void DoSmoothingGPU(ComputeBuffer startBuffer)
	{
		this.InitSmoothing();
		float x = base.transform.lossyScale.x;
		int num = 0;
		for (int i = 0; i < this.smoothOuterLoops; i++)
		{
			for (int j = 0; j < this.laplacianSmoothPasses; j++)
			{
				if (num % 2 == 0)
				{
					this.meshSmoothGPU.LaplacianSmoothGPU(startBuffer, this._verticesBuffer2);
					this.meshSmoothGPU.HCCorrectionGPU(startBuffer, this._verticesBuffer2, this.laplacianSmoothBeta);
					this._drawVerticesBuffer = this._verticesBuffer2;
				}
				else
				{
					this.meshSmoothGPU.LaplacianSmoothGPU(this._verticesBuffer2, startBuffer);
					this.meshSmoothGPU.HCCorrectionGPU(this._verticesBuffer2, startBuffer, this.laplacianSmoothBeta);
					this._drawVerticesBuffer = startBuffer;
				}
				num++;
			}
			for (int k = 0; k < this.springSmoothPasses; k++)
			{
				if (num % 2 == 0)
				{
					if (this.useSpring2)
					{
						this.meshSmoothGPU.SpringSmooth2GPU(startBuffer, this._verticesBuffer2, this.spring2SmoothFactor, x);
					}
					else
					{
						this.meshSmoothGPU.SpringSmoothGPU(startBuffer, this._verticesBuffer2, this.springSmoothFactor, x);
					}
					this._drawVerticesBuffer = this._verticesBuffer2;
				}
				else
				{
					if (this.useSpring2)
					{
						this.meshSmoothGPU.SpringSmooth2GPU(this._verticesBuffer2, startBuffer, this.spring2SmoothFactor, x);
					}
					else
					{
						this.meshSmoothGPU.SpringSmoothGPU(this._verticesBuffer2, startBuffer, this.springSmoothFactor, x);
					}
					this._drawVerticesBuffer = startBuffer;
				}
				num++;
			}
		}
	}

	// Token: 0x06004D5C RID: 19804 RVA: 0x001B11BC File Offset: 0x001AF5BC
	protected void UpdateVertsGPU(bool fullUpdate = true)
	{
		if (this.skin != null && this.skin.isActiveAndEnabled && this.GPUSkinWrapper != null)
		{
			this.InitMesh(false);
			this.SkinWrapGPUInit();
			this.GPURecheckSurfaceNormalOffsets();
			if (this._wrapVerticesBuffer != null)
			{
				float x = base.transform.lossyScale.x;
				float val = 1f / x;
				this.GPUSkinWrapper.SetFloat("skinWrapScale", x);
				this.GPUSkinWrapper.SetFloat("skinWrapOneOverScale", val);
				if (this.skin.useSmoothing)
				{
					if (this.forceRawSkinVerts)
					{
						this.GPUSkinWrapper.SetBuffer(this._skinWrapKernel, "skinToVertices", this.skin.rawVertsBuffer);
						this.GPUSkinWrapper.SetBuffer(this._skinWrapKernel, "skinToSurfaceNormals", this.skin.surfaceNormalsRawBuffer);
					}
					else
					{
						this.GPUSkinWrapper.SetBuffer(this._skinWrapKernel, "skinToVertices", this.skin.smoothedVertsBuffer);
						this.GPUSkinWrapper.SetBuffer(this._skinWrapKernel, "skinToSurfaceNormals", this.skin.surfaceNormalsBuffer);
					}
				}
				else
				{
					this.GPUSkinWrapper.SetBuffer(this._skinWrapKernel, "skinToVertices", this.skin.rawVertsBuffer);
					this.GPUSkinWrapper.SetBuffer(this._skinWrapKernel, "skinToSurfaceNormals", this.skin.surfaceNormalsBuffer);
				}
				this.GPUSkinWrapper.SetBuffer(this._skinWrapKernel, "skinWrapVerts", this._wrapVerticesBuffer);
				this.GPUSkinWrapper.SetBuffer(this._skinWrapKernel, "outVerts", this._verticesBuffer1);
				this.GPUSkinWrapper.SetFloat("skinWrapNormalOffset", this.surfaceOffset);
				this.GPUSkinWrapper.SetFloat("skinWrapThicknessMultiplier", this.additionalThicknessMultiplier);
				this.GPUSkinWrapper.Dispatch(this._skinWrapKernel, this.numVertThreadGroups, 1, 1);
				if (fullUpdate && this.smoothOuterLoops > 0 && (this.laplacianSmoothPasses > 0 || this.springSmoothPasses > 0))
				{
					this.DoSmoothingGPU(this._verticesBuffer1);
				}
				else
				{
					this._drawVerticesBuffer = this._verticesBuffer1;
				}
				this.mapVerticesGPU.Map(this._drawVerticesBuffer);
				if (fullUpdate)
				{
					if (this.recalculateNormals && this.recalcNormalsGPU != null)
					{
						this.recalcNormalsGPU.RecalculateNormals(this._drawVerticesBuffer);
					}
					if (this.recalculateTangents && this.recalcTangentsGPU != null)
					{
						this.recalcTangentsGPU.RecalculateTangents(this._drawVerticesBuffer, this._normalsBuffer);
					}
				}
			}
		}
	}

	// Token: 0x06004D5D RID: 19805 RVA: 0x001B1474 File Offset: 0x001AF874
	public void DrawMesh()
	{
		if (this.mesh != null && !this._renderSuspend)
		{
			Matrix4x4 matrix;
			if (Application.isPlaying)
			{
				matrix = Matrix4x4.identity;
			}
			else if (this.skin != null && this.skin.root != null)
			{
				matrix = this.skin.root.transform.localToWorldMatrix;
			}
			else
			{
				matrix = base.transform.localToWorldMatrix;
			}
			for (int i = 0; i < this.mesh.subMeshCount; i++)
			{
				if (this.dazMesh.useSimpleMaterial && this.dazMesh.simpleMaterial)
				{
					Graphics.DrawMesh(this.mesh, matrix, this.dazMesh.simpleMaterial, 0, null, i, null, this.dazMesh.castShadows, this.dazMesh.receiveShadows);
				}
				else if (this.dazMesh.materialsEnabled[i] && this.dazMesh.materials[i] != null)
				{
					Graphics.DrawMesh(this.mesh, matrix, this.dazMesh.materials[i], 0, null, i, null, this.dazMesh.castShadows, this.dazMesh.receiveShadows);
				}
			}
		}
	}

	// Token: 0x06004D5E RID: 19806 RVA: 0x001B15D4 File Offset: 0x001AF9D4
	protected void DrawMeshNative()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		MeshRenderer component2 = base.GetComponent<MeshRenderer>();
		if (component != null && component2 != null)
		{
			if (component.sharedMesh != this.mesh)
			{
				component.sharedMesh = this.mesh;
			}
			for (int i = 0; i < this.mesh.subMeshCount; i++)
			{
				if (this.dazMesh.materialsEnabled[i])
				{
					if (this.dazMesh.useSimpleMaterial)
					{
						component2.sharedMaterials[i] = this.dazMesh.simpleMaterial;
					}
					else
					{
						component2.sharedMaterials[i] = this.dazMesh.materials[i];
					}
				}
				else
				{
					component2.materials[i] = null;
				}
			}
		}
	}

	// Token: 0x06004D5F RID: 19807 RVA: 0x001B16A2 File Offset: 0x001AFAA2
	public void FlushBuffers()
	{
		this.materialVertsBuffers = new Dictionary<int, ComputeBuffer>();
		this.materialNormalsBuffers = new Dictionary<int, ComputeBuffer>();
		this.materialTangentsBuffers = new Dictionary<int, ComputeBuffer>();
	}

	// Token: 0x06004D60 RID: 19808 RVA: 0x001B16C5 File Offset: 0x001AFAC5
	private void OnApplicationFocus(bool focus)
	{
		this.FlushBuffers();
	}

	// Token: 0x06004D61 RID: 19809 RVA: 0x001B16D0 File Offset: 0x001AFAD0
	protected void DrawMeshGPU(bool noDelay = false)
	{
		if (!this._renderSuspend)
		{
			if (this.materialVertsBuffers == null)
			{
				this.materialVertsBuffers = new Dictionary<int, ComputeBuffer>();
			}
			if (this.materialNormalsBuffers == null)
			{
				this.materialNormalsBuffers = new Dictionary<int, ComputeBuffer>();
			}
			if (this.materialTangentsBuffers == null)
			{
				this.materialTangentsBuffers = new Dictionary<int, ComputeBuffer>();
			}
			if (DAZSkinWrap.staticDraw)
			{
				Matrix4x4 identity = Matrix4x4.identity;
				this.dazMesh.DrawMorphedUVMappedMesh(identity);
			}
			else if (this.mesh != null)
			{
				Matrix4x4 identity2 = Matrix4x4.identity;
				if (this.dazMesh.drawFromBone != null)
				{
					Bounds bounds = this.mesh.bounds;
					bounds.center = this.dazMesh.drawFromBone.transform.position;
					Vector3 size = bounds.size;
					size.x = 0.1f;
					size.y = 0.1f;
					size.z = 0.1f;
					bounds.size = size;
					this.mesh.bounds = bounds;
				}
				for (int i = 0; i < this.mesh.subMeshCount; i++)
				{
					if (this.GPUuseSimpleMaterial && this.GPUsimpleMaterial)
					{
						if (this.skin.delayDisplayOneFrame && !noDelay)
						{
							this.GPUsimpleMaterial.SetBuffer("verts", this._delayedVertsBuffer);
							this.GPUsimpleMaterial.SetBuffer("normals", this._delayedNormalsBuffer);
							this.GPUsimpleMaterial.SetBuffer("tangents", this._delayedTangentsBuffer);
						}
						else
						{
							this.GPUsimpleMaterial.SetBuffer("verts", this._drawVerticesBuffer);
							this.GPUsimpleMaterial.SetBuffer("normals", this._normalsBuffer);
							this.GPUsimpleMaterial.SetBuffer("tangents", this._tangentsBuffer);
						}
						Graphics.DrawMesh(this.mesh, identity2, this.GPUsimpleMaterial, 0, null, i, null, this.dazMesh.castShadows, this.dazMesh.receiveShadows);
					}
					else if (this.materialsEnabled != null && this.materialsEnabled[i] && this.GPUmaterials[i] != null)
					{
						if (this.skin != null && this.skin.delayDisplayOneFrame && !noDelay)
						{
							ComputeBuffer computeBuffer;
							this.materialVertsBuffers.TryGetValue(i, out computeBuffer);
							if (computeBuffer != this._delayedVertsBuffer)
							{
								this.GPUmaterials[i].SetBuffer("verts", this._delayedVertsBuffer);
								this.materialVertsBuffers.Remove(i);
								this.materialVertsBuffers.Add(i, this._delayedVertsBuffer);
							}
							this.materialNormalsBuffers.TryGetValue(i, out computeBuffer);
							if (computeBuffer != this._delayedNormalsBuffer)
							{
								this.GPUmaterials[i].SetBuffer("normals", this._delayedNormalsBuffer);
								this.materialNormalsBuffers.Remove(i);
								this.materialNormalsBuffers.Add(i, this._delayedNormalsBuffer);
							}
							this.materialTangentsBuffers.TryGetValue(i, out computeBuffer);
							if (computeBuffer != this._delayedTangentsBuffer)
							{
								this.GPUmaterials[i].SetBuffer("tangents", this._delayedTangentsBuffer);
								this.materialTangentsBuffers.Remove(i);
								this.materialTangentsBuffers.Add(i, this._delayedTangentsBuffer);
							}
						}
						else
						{
							ComputeBuffer computeBuffer2;
							this.materialVertsBuffers.TryGetValue(i, out computeBuffer2);
							if (computeBuffer2 != this._drawVerticesBuffer)
							{
								this.GPUmaterials[i].SetBuffer("verts", this._drawVerticesBuffer);
								this.materialVertsBuffers.Remove(i);
								this.materialVertsBuffers.Add(i, this._drawVerticesBuffer);
							}
							this.materialNormalsBuffers.TryGetValue(i, out computeBuffer2);
							if (computeBuffer2 != this._normalsBuffer)
							{
								this.GPUmaterials[i].SetBuffer("normals", this._normalsBuffer);
								this.materialNormalsBuffers.Remove(i);
								this.materialNormalsBuffers.Add(i, this._normalsBuffer);
							}
							this.materialTangentsBuffers.TryGetValue(i, out computeBuffer2);
							if (computeBuffer2 != this._tangentsBuffer)
							{
								this.GPUmaterials[i].SetBuffer("tangents", this._tangentsBuffer);
								this.materialTangentsBuffers.Remove(i);
								this.materialTangentsBuffers.Add(i, this._tangentsBuffer);
							}
						}
						Graphics.DrawMesh(this.mesh, identity2, this.GPUmaterials[i], 0, null, i, null, this.dazMesh.castShadows, this.dazMesh.receiveShadows);
					}
				}
			}
		}
	}

	// Token: 0x06004D62 RID: 19810 RVA: 0x001B1B80 File Offset: 0x001AFF80
	protected void GPUCleanup()
	{
		if (this.meshSmoothGPU != null)
		{
			this.meshSmoothGPU.Release();
			this.meshSmoothGPU = null;
		}
		if (this.mapVerticesGPU != null)
		{
			this.mapVerticesGPU.Release();
			this.mapVerticesGPU = null;
		}
		if (this.originalRecalcNormalsGPU != null)
		{
			this.originalRecalcNormalsGPU.Release();
			this.originalRecalcNormalsGPU = null;
		}
		if (this.recalcNormalsGPU != null)
		{
			this.recalcNormalsGPU.Release();
			this.recalcNormalsGPU = null;
		}
		if (this.originalRecalcTangentsGPU != null)
		{
			this.originalRecalcTangentsGPU.Release();
			this.originalRecalcTangentsGPU = null;
		}
		if (this.recalcTangentsGPU != null)
		{
			this.recalcTangentsGPU.Release();
			this.recalcTangentsGPU = null;
		}
		if (this._wrapVerticesBuffer != null)
		{
			this._wrapVerticesBuffer.Release();
			this._wrapVerticesBuffer = null;
		}
		if (this._verticesBuffer1 != null)
		{
			this._verticesBuffer1.Release();
			this._verticesBuffer1 = null;
		}
		if (this._verticesBuffer2 != null)
		{
			this._verticesBuffer2.Release();
			this._verticesBuffer2 = null;
		}
		if (this._delayedVertsBuffer != null)
		{
			this._delayedVertsBuffer.Release();
			this._delayedVertsBuffer = null;
		}
		if (this._delayedNormalsBuffer != null)
		{
			this._delayedNormalsBuffer.Release();
			this._delayedNormalsBuffer = null;
		}
		if (this._delayedTangentsBuffer != null)
		{
			this._delayedTangentsBuffer.Release();
			this._delayedTangentsBuffer = null;
		}
		if (this._originalVerticesBuffer != null)
		{
			this._originalVerticesBuffer.Release();
			this._originalVerticesBuffer = null;
		}
		if (this._matricesBuffer != null)
		{
			this._matricesBuffer.Release();
			this._matricesBuffer = null;
		}
	}

	// Token: 0x06004D63 RID: 19811 RVA: 0x001B1D23 File Offset: 0x001B0123
	private void OnEnable()
	{
		this.FlushBuffers();
	}

	// Token: 0x06004D64 RID: 19812 RVA: 0x001B1D2B File Offset: 0x001B012B
	protected void OnDestroy()
	{
		this.DestroyAllocatedObjects();
		this.GPUCleanup();
		this.killThread = true;
	}

	// Token: 0x06004D65 RID: 19813 RVA: 0x001B1D40 File Offset: 0x001B0140
	public void InitMaterials()
	{
		if (Application.isPlaying && !this._materialsWereInit && this.GPUmaterials != null)
		{
			this._materialsWereInit = true;
			for (int i = 0; i < this.GPUmaterials.Length; i++)
			{
				Material material = new Material(this.GPUmaterials[i]);
				this.RegisterAllocatedObject(material);
				this.GPUmaterials[i] = material;
			}
		}
	}

	// Token: 0x06004D66 RID: 19814 RVA: 0x001B1DAC File Offset: 0x001B01AC
	private void LateUpdate()
	{
		if ((this.draw || this.debug) && this.skin != null && !this._updateDrawDisabled && this.dazMesh != null && !this.isWrapping && this.wrapStore != null && this.wrapStore.wrapVertices != null)
		{
			if (this.autoColliderToEnable != null)
			{
				this.autoColliderToEnable.on = true;
			}
			if (this.autoColliderToDisable != null)
			{
				this.autoColliderToDisable.on = false;
			}
			if (this.skin.skinMethod == DAZSkinV2.SkinMethod.CPU || this.forceCPU || !Application.isPlaying)
			{
				this.UpdateVerts(false);
				if (this.draw)
				{
					this.DrawMesh();
				}
			}
			else
			{
				if (this.skin.delayDisplayOneFrame && this.GPUSkinWrapper != null && this._delayedVertsBuffer != null)
				{
					this.GPUSkinWrapper.SetBuffer(this._copyKernel, "inVerts", this._drawVerticesBuffer);
					this.GPUSkinWrapper.SetBuffer(this._copyKernel, "outVerts", this._delayedVertsBuffer);
					this.GPUSkinWrapper.Dispatch(this._copyKernel, this.numVertThreadGroups, 1, 1);
					if (this._normalsBuffer != null)
					{
						this.GPUSkinWrapper.SetBuffer(this._copyKernel, "inVerts", this._normalsBuffer);
						this.GPUSkinWrapper.SetBuffer(this._copyKernel, "outVerts", this._delayedNormalsBuffer);
						this.GPUSkinWrapper.Dispatch(this._copyKernel, this.numVertThreadGroups, 1, 1);
					}
					if (this._tangentsBuffer != null)
					{
						this.GPUMeshCompute.SetBuffer(this._copyTangentsKernel, "inTangents", this._tangentsBuffer);
						this.GPUMeshCompute.SetBuffer(this._copyTangentsKernel, "outTangents", this._delayedTangentsBuffer);
						this.GPUMeshCompute.Dispatch(this._copyTangentsKernel, this.numVertThreadGroups, 1, 1);
					}
				}
				this.UpdateVertsGPU(true);
				if (this.draw)
				{
					this.DrawMeshGPU(false);
				}
			}
		}
	}

	// Token: 0x06004D67 RID: 19815 RVA: 0x001B1FF2 File Offset: 0x001B03F2
	// Note: this type is marked as 'beforefieldinit'.
	static DAZSkinWrap()
	{
	}

	// Token: 0x04003CC4 RID: 15556
	public static bool staticDraw;

	// Token: 0x04003CC5 RID: 15557
	protected List<UnityEngine.Object> allocatedObjects;

	// Token: 0x04003CC6 RID: 15558
	private GpuBuffer<Matrix4x4> _ToWorldMatricesBuffer;

	// Token: 0x04003CC7 RID: 15559
	private GpuBuffer<Vector3> _PreCalculatedVerticesBuffer;

	// Token: 0x04003CC8 RID: 15560
	private GpuBuffer<Vector3> _NormalsBuffer;

	// Token: 0x04003CC9 RID: 15561
	protected bool _updateDrawDisabled;

	// Token: 0x04003CCA RID: 15562
	protected bool _renderSuspend;

	// Token: 0x04003CCB RID: 15563
	protected DAZSkinWrapStore startingWrapStore;

	// Token: 0x04003CCC RID: 15564
	public DAZSkinWrapStore wrapStore;

	// Token: 0x04003CCD RID: 15565
	public string wrapName = "Normal";

	// Token: 0x04003CCE RID: 15566
	public bool draw;

	// Token: 0x04003CCF RID: 15567
	public bool debug;

	// Token: 0x04003CD0 RID: 15568
	public int debugStartVert;

	// Token: 0x04003CD1 RID: 15569
	public int debugStopVert;

	// Token: 0x04003CD2 RID: 15570
	public float debugTan1;

	// Token: 0x04003CD3 RID: 15571
	public float debugTan2;

	// Token: 0x04003CD4 RID: 15572
	public float debugSize = 0.005f;

	// Token: 0x04003CD5 RID: 15573
	public DAZMesh dazMesh;

	// Token: 0x04003CD6 RID: 15574
	public Transform skinTransform;

	// Token: 0x04003CD7 RID: 15575
	public DAZSkinV2 skin;

	// Token: 0x04003CD8 RID: 15576
	public bool forceRawSkinVerts = true;

	// Token: 0x04003CD9 RID: 15577
	[NonSerialized]
	public float defaultAdditionalThicknessMultiplier;

	// Token: 0x04003CDA RID: 15578
	public float additionalThicknessMultiplier = 0.001f;

	// Token: 0x04003CDB RID: 15579
	[NonSerialized]
	public float defaultSurfaceOffset;

	// Token: 0x04003CDC RID: 15580
	public float surfaceOffset = 0.0003f;

	// Token: 0x04003CDD RID: 15581
	public int smoothOuterLoops = 1;

	// Token: 0x04003CDE RID: 15582
	public int laplacianSmoothPasses = 1;

	// Token: 0x04003CDF RID: 15583
	public float laplacianSmoothBeta = 0.5f;

	// Token: 0x04003CE0 RID: 15584
	public int springSmoothPasses;

	// Token: 0x04003CE1 RID: 15585
	public float springSmoothFactor = 0.2f;

	// Token: 0x04003CE2 RID: 15586
	public bool useSpring2;

	// Token: 0x04003CE3 RID: 15587
	public float spring2SmoothFactor = 1f;

	// Token: 0x04003CE4 RID: 15588
	public bool moveToSurface = true;

	// Token: 0x04003CE5 RID: 15589
	public float moveToSurfaceOffset = 0.0003f;

	// Token: 0x04003CE6 RID: 15590
	public AutoCollider autoColliderToEnable;

	// Token: 0x04003CE7 RID: 15591
	public AutoCollider autoColliderToDisable;

	// Token: 0x04003CE8 RID: 15592
	public bool forceCPU;

	// Token: 0x04003CE9 RID: 15593
	protected Vector3[] _verts1;

	// Token: 0x04003CEA RID: 15594
	protected Vector3[] _verts2;

	// Token: 0x04003CEB RID: 15595
	protected Vector3[] _drawVerts;

	// Token: 0x04003CEC RID: 15596
	protected int _wrapProgress;

	// Token: 0x04003CED RID: 15597
	public Transform morphCopyFromTransform;

	// Token: 0x04003CEE RID: 15598
	public DAZSkinWrap morphCopyFrom;

	// Token: 0x04003CEF RID: 15599
	[SerializeField]
	protected List<string> _usedMorphNames;

	// Token: 0x04003CF0 RID: 15600
	[SerializeField]
	protected List<float> _usedMorphValues;

	// Token: 0x04003CF1 RID: 15601
	public int vertexIndexLimit = -1;

	// Token: 0x04003CF2 RID: 15602
	public bool wrapCheckNormals;

	// Token: 0x04003CF3 RID: 15603
	public bool wrapToSkinnedVertices;

	// Token: 0x04003CF4 RID: 15604
	public bool wrapToMorphedVertices;

	// Token: 0x04003CF5 RID: 15605
	public bool wrapToDisabledMaterials;

	// Token: 0x04003CF6 RID: 15606
	protected float threadedProgress;

	// Token: 0x04003CF7 RID: 15607
	protected float threadedCount;

	// Token: 0x04003CF8 RID: 15608
	protected bool killThread;

	// Token: 0x04003CF9 RID: 15609
	public string assetSavePath = "Assets/VaMAssets/Generated/";

	// Token: 0x04003CFA RID: 15610
	protected Thread wrapThread;

	// Token: 0x04003CFB RID: 15611
	protected bool appIsPlayingThreaded;

	// Token: 0x04003CFC RID: 15612
	protected bool isWrapping;

	// Token: 0x04003CFD RID: 15613
	protected string wrapStatus;

	// Token: 0x04003CFE RID: 15614
	protected MeshSmooth meshSmooth;

	// Token: 0x04003CFF RID: 15615
	protected MeshSmoothGPU meshSmoothGPU;

	// Token: 0x04003D00 RID: 15616
	protected Mesh mesh;

	// Token: 0x04003D01 RID: 15617
	protected Mesh startMesh;

	// Token: 0x04003D02 RID: 15618
	protected bool meshWasInit;

	// Token: 0x04003D03 RID: 15619
	protected bool startMeshWasInit;

	// Token: 0x04003D04 RID: 15620
	protected const int vertGroupSize = 256;

	// Token: 0x04003D05 RID: 15621
	protected int numVertThreadGroups;

	// Token: 0x04003D06 RID: 15622
	public ComputeShader GPUSkinWrapper;

	// Token: 0x04003D07 RID: 15623
	public ComputeShader GPUMeshCompute;

	// Token: 0x04003D08 RID: 15624
	protected ComputeBuffer _wrapVerticesBuffer;

	// Token: 0x04003D09 RID: 15625
	protected ComputeBuffer _verticesBuffer1;

	// Token: 0x04003D0A RID: 15626
	protected ComputeBuffer _verticesBuffer2;

	// Token: 0x04003D0B RID: 15627
	protected ComputeBuffer _originalVerticesBuffer;

	// Token: 0x04003D0C RID: 15628
	protected ComputeBuffer _matricesBuffer;

	// Token: 0x04003D0D RID: 15629
	protected ComputeBuffer _drawVerticesBuffer;

	// Token: 0x04003D0E RID: 15630
	protected ComputeBuffer _delayedVertsBuffer;

	// Token: 0x04003D0F RID: 15631
	protected ComputeBuffer _delayedNormalsBuffer;

	// Token: 0x04003D10 RID: 15632
	protected ComputeBuffer _delayedTangentsBuffer;

	// Token: 0x04003D11 RID: 15633
	protected MapVerticesGPU mapVerticesGPU;

	// Token: 0x04003D12 RID: 15634
	protected int _skinWrapKernel;

	// Token: 0x04003D13 RID: 15635
	protected int _skinWrapCalcChangeMatricesKernel;

	// Token: 0x04003D14 RID: 15636
	protected int _copyKernel;

	// Token: 0x04003D15 RID: 15637
	protected int _copyTangentsKernel;

	// Token: 0x04003D16 RID: 15638
	protected int _nullVertexIndex;

	// Token: 0x04003D17 RID: 15639
	protected int[] numSubsetVertThreadGroups;

	// Token: 0x04003D18 RID: 15640
	public bool GPUuseSimpleMaterial;

	// Token: 0x04003D19 RID: 15641
	public Material GPUsimpleMaterial;

	// Token: 0x04003D1A RID: 15642
	public Material[] GPUmaterials;

	// Token: 0x04003D1B RID: 15643
	public Texture2D[] simTextures;

	// Token: 0x04003D1C RID: 15644
	public bool GPUAutoSwapShader = true;

	// Token: 0x04003D1D RID: 15645
	public bool onlyUpdateEnabledMaterials = true;

	// Token: 0x04003D1E RID: 15646
	public bool[] materialsEnabled;

	// Token: 0x04003D1F RID: 15647
	[SerializeField]
	protected int _numMaterials;

	// Token: 0x04003D20 RID: 15648
	[SerializeField]
	protected string[] _materialNames;

	// Token: 0x04003D21 RID: 15649
	public bool recalculateNormals = true;

	// Token: 0x04003D22 RID: 15650
	public bool recalculateTangents = true;

	// Token: 0x04003D23 RID: 15651
	protected RecalculateNormalsGPU originalRecalcNormalsGPU;

	// Token: 0x04003D24 RID: 15652
	protected RecalculateNormalsGPU recalcNormalsGPU;

	// Token: 0x04003D25 RID: 15653
	protected RecalculateTangentsGPU originalRecalcTangentsGPU;

	// Token: 0x04003D26 RID: 15654
	protected RecalculateTangentsGPU recalcTangentsGPU;

	// Token: 0x04003D27 RID: 15655
	protected ComputeBuffer _originalNormalsBuffer;

	// Token: 0x04003D28 RID: 15656
	protected ComputeBuffer _normalsBuffer;

	// Token: 0x04003D29 RID: 15657
	protected ComputeBuffer _originalTangentsBuffer;

	// Token: 0x04003D2A RID: 15658
	protected ComputeBuffer _tangentsBuffer;

	// Token: 0x04003D2B RID: 15659
	protected ComputeBuffer _originalSurfaceNormalsBuffer;

	// Token: 0x04003D2C RID: 15660
	protected ComputeBuffer _surfaceNormalsBuffer;

	// Token: 0x04003D2D RID: 15661
	protected bool currentMoveToSurface;

	// Token: 0x04003D2E RID: 15662
	protected float currentMoveToSurfaceOffset;

	// Token: 0x04003D2F RID: 15663
	protected List<int> usedSkinVerts;

	// Token: 0x04003D30 RID: 15664
	protected Dictionary<int, ComputeBuffer> materialVertsBuffers;

	// Token: 0x04003D31 RID: 15665
	protected Dictionary<int, ComputeBuffer> materialNormalsBuffers;

	// Token: 0x04003D32 RID: 15666
	protected Dictionary<int, ComputeBuffer> materialTangentsBuffers;

	// Token: 0x04003D33 RID: 15667
	protected bool _materialsWereInit;

	// Token: 0x02000B13 RID: 2835
	protected struct Triangle
	{
		// Token: 0x04003D34 RID: 15668
		public int vert1;

		// Token: 0x04003D35 RID: 15669
		public int vert2;

		// Token: 0x04003D36 RID: 15670
		public int vert3;
	}

	// Token: 0x02000FCE RID: 4046
	[CompilerGenerated]
	private sealed class <WatchThread>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007547 RID: 30023 RVA: 0x001B1FF4 File Offset: 0x001B03F4
		[DebuggerHidden]
		public <WatchThread>c__Iterator0()
		{
		}

		// Token: 0x06007548 RID: 30024 RVA: 0x001B1FFC File Offset: 0x001B03FC
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				break;
			case 1U:
				break;
			default:
				return false;
			}
			if (this.wrapThread.IsAlive)
			{
				this.wrapStatus = string.Concat(new object[]
				{
					"Wrapped ",
					this.threadedCount,
					" of ",
					this.dazMesh.numBaseVertices,
					" vertices"
				});
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			this.isWrapping = false;
			this.$PC = -1;
			return false;
		}

		// Token: 0x17001157 RID: 4439
		// (get) Token: 0x06007549 RID: 30025 RVA: 0x001B20CE File Offset: 0x001B04CE
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x0600754A RID: 30026 RVA: 0x001B20D6 File Offset: 0x001B04D6
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600754B RID: 30027 RVA: 0x001B20DE File Offset: 0x001B04DE
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600754C RID: 30028 RVA: 0x001B20EE File Offset: 0x001B04EE
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400695E RID: 26974
		internal DAZSkinWrap $this;

		// Token: 0x0400695F RID: 26975
		internal object $current;

		// Token: 0x04006960 RID: 26976
		internal bool $disposing;

		// Token: 0x04006961 RID: 26977
		internal int $PC;
	}
}
