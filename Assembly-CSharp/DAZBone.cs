using System;
using System.Collections;
using System.Collections.Generic;
using MVR;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000AA7 RID: 2727
public class DAZBone : JSONStorable
{
	// Token: 0x06004738 RID: 18232 RVA: 0x0014BEA8 File Offset: 0x0014A2A8
	public DAZBone()
	{
	}

	// Token: 0x06004739 RID: 18233 RVA: 0x0014BF2D File Offset: 0x0014A32D
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x0600473A RID: 18234 RVA: 0x0014BF38 File Offset: 0x0014A338
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if (includePhysical || forceStore)
		{
			this.needsStore = true;
			if (this.isRoot)
			{
				Vector3 vector;
				Vector3 vector2;
				if (this.saveControl != null)
				{
					Transform parent = base.transform.parent;
					base.transform.parent = this.saveControl.transform;
					vector = base.transform.localPosition;
					json["relativeRootPosition"]["x"].AsFloat = vector.x;
					json["relativeRootPosition"]["y"].AsFloat = vector.y;
					json["relativeRootPosition"]["z"].AsFloat = vector.z;
					vector2 = base.transform.localEulerAngles;
					json["relativeRootRotation"]["x"].AsFloat = vector2.x;
					json["relativeRootRotation"]["y"].AsFloat = vector2.y;
					json["relativeRootRotation"]["z"].AsFloat = vector2.z;
					base.transform.parent = parent;
				}
				vector = base.transform.position;
				json["rootPosition"]["x"].AsFloat = vector.x;
				json["rootPosition"]["y"].AsFloat = vector.y;
				json["rootPosition"]["z"].AsFloat = vector.z;
				vector2 = base.transform.eulerAngles;
				json["rootRotation"]["x"].AsFloat = vector2.x;
				json["rootRotation"]["y"].AsFloat = vector2.y;
				json["rootRotation"]["z"].AsFloat = vector2.z;
			}
			else
			{
				Vector3 vector = base.transform.localPosition;
				json["position"]["x"].AsFloat = vector.x;
				json["position"]["y"].AsFloat = vector.y;
				json["position"]["z"].AsFloat = vector.z;
				Vector3 vector2 = base.transform.localEulerAngles;
				json["rotation"]["x"].AsFloat = vector2.x;
				json["rotation"]["y"].AsFloat = vector2.y;
				json["rotation"]["z"].AsFloat = vector2.z;
			}
		}
		return json;
	}

	// Token: 0x0600473B RID: 18235 RVA: 0x0014C24C File Offset: 0x0014A64C
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		this.Init();
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical)
		{
			Transform parent = base.transform.parent;
			bool flag = false;
			if (this.isRoot && this.saveControl != null && jc["relativeRootPosition"] != null)
			{
				if (!base.IsCustomPhysicalParamLocked("relativeRootPosition"))
				{
					flag = true;
					base.transform.parent = this.saveControl.transform;
					Vector3 localPosition = base.transform.localPosition;
					if (jc["relativeRootPosition"]["x"] != null)
					{
						localPosition.x = jc["relativeRootPosition"]["x"].AsFloat;
					}
					if (jc["relativeRootPosition"]["y"] != null)
					{
						localPosition.y = jc["relativeRootPosition"]["y"].AsFloat;
					}
					if (jc["relativeRootPosition"]["z"] != null)
					{
						localPosition.z = jc["relativeRootPosition"]["z"].AsFloat;
					}
					base.transform.localPosition = localPosition;
				}
			}
			else if (this.isRoot && jc["rootPosition"] != null)
			{
				if (!base.IsCustomPhysicalParamLocked("rootPosition"))
				{
					Vector3 position = base.transform.position;
					if (jc["rootPosition"]["x"] != null)
					{
						position.x = jc["rootPosition"]["x"].AsFloat;
					}
					if (jc["rootPosition"]["y"] != null)
					{
						position.y = jc["rootPosition"]["y"].AsFloat;
					}
					if (jc["rootPosition"]["z"] != null)
					{
						position.z = jc["rootPosition"]["z"].AsFloat;
					}
					base.transform.position = position;
				}
			}
			else if (jc["position"] != null)
			{
				if (!base.IsCustomPhysicalParamLocked("position"))
				{
					Vector3 localPosition2 = base.transform.localPosition;
					if (jc["position"]["x"] != null)
					{
						localPosition2.x = jc["position"]["x"].AsFloat;
					}
					if (jc["position"]["y"] != null)
					{
						localPosition2.y = jc["position"]["y"].AsFloat;
					}
					if (jc["position"]["z"] != null)
					{
						localPosition2.z = jc["position"]["z"].AsFloat;
					}
					base.transform.localPosition = localPosition2;
				}
			}
			else if (setMissingToDefault)
			{
				if (this.isRoot)
				{
					if (this.saveControl != null)
					{
						if (!base.IsCustomPhysicalParamLocked("relativeRootPosition"))
						{
							base.transform.position = this.saveControl.transform.position;
						}
					}
					else if (!base.IsCustomPhysicalParamLocked("rootPosition"))
					{
						base.transform.localPosition = this._startingLocalPosition;
					}
				}
				else if (!base.IsCustomPhysicalParamLocked("position"))
				{
					base.transform.localPosition = this._startingLocalPosition;
				}
			}
			if (this.isRoot && this.saveControl != null && jc["relativeRootRotation"] != null)
			{
				if (!base.IsCustomPhysicalParamLocked("relativeRootRotation"))
				{
					flag = true;
					base.transform.parent = this.saveControl.transform;
					Vector3 localEulerAngles = base.transform.localEulerAngles;
					if (jc["relativeRootRotation"]["x"] != null)
					{
						localEulerAngles.x = jc["relativeRootRotation"]["x"].AsFloat;
					}
					if (jc["relativeRootRotation"]["y"] != null)
					{
						localEulerAngles.y = jc["relativeRootRotation"]["y"].AsFloat;
					}
					if (jc["relativeRootRotation"]["z"] != null)
					{
						localEulerAngles.z = jc["relativeRootRotation"]["z"].AsFloat;
					}
					base.transform.localEulerAngles = localEulerAngles;
				}
			}
			else if (this.isRoot && jc["rootRotation"] != null)
			{
				if (!base.IsCustomPhysicalParamLocked("rootRotation"))
				{
					Vector3 eulerAngles = base.transform.eulerAngles;
					if (jc["rootRotation"]["x"] != null)
					{
						eulerAngles.x = jc["rootRotation"]["x"].AsFloat;
					}
					if (jc["rootRotation"]["y"] != null)
					{
						eulerAngles.y = jc["rootRotation"]["y"].AsFloat;
					}
					if (jc["rootRotation"]["z"] != null)
					{
						eulerAngles.z = jc["rootRotation"]["z"].AsFloat;
					}
					base.transform.eulerAngles = eulerAngles;
				}
			}
			else if (jc["rotation"] != null)
			{
				if (!base.IsCustomPhysicalParamLocked("rotation"))
				{
					Vector3 localEulerAngles2 = base.transform.localEulerAngles;
					if (jc["rotation"]["x"] != null)
					{
						localEulerAngles2.x = jc["rotation"]["x"].AsFloat;
					}
					if (jc["rotation"]["y"] != null)
					{
						localEulerAngles2.y = jc["rotation"]["y"].AsFloat;
					}
					if (jc["rotation"]["z"] != null)
					{
						localEulerAngles2.z = jc["rotation"]["z"].AsFloat;
					}
					base.transform.localEulerAngles = localEulerAngles2;
				}
			}
			else if (setMissingToDefault)
			{
				if (this.isRoot)
				{
					if (this.saveControl != null)
					{
						if (!base.IsCustomPhysicalParamLocked("relativeRootRotation"))
						{
							base.transform.rotation = this.control.transform.rotation;
						}
					}
					else if (!base.IsCustomPhysicalParamLocked("rootRotation"))
					{
						base.transform.localRotation = this._startingLocalRotation;
					}
				}
				else if (!base.IsCustomPhysicalParamLocked("rotation"))
				{
					base.transform.localRotation = this._startingLocalRotation;
				}
			}
			if (flag)
			{
				base.transform.parent = parent;
			}
		}
	}

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x0600473C RID: 18236 RVA: 0x0014CA7B File Offset: 0x0014AE7B
	// (set) Token: 0x0600473D RID: 18237 RVA: 0x0014CAAF File Offset: 0x0014AEAF
	public string id
	{
		get
		{
			if (this._id == null || this._id == string.Empty)
			{
				this._id = base.name;
			}
			return this._id;
		}
		set
		{
			this._id = value;
		}
	}

	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x0600473E RID: 18238 RVA: 0x0014CAB8 File Offset: 0x0014AEB8
	public Vector3 worldPosition
	{
		get
		{
			if (this.dazBones != null && this.dazBones.isMale)
			{
				return this._maleWorldPosition;
			}
			return this._worldPosition;
		}
	}

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x0600473F RID: 18239 RVA: 0x0014CAE8 File Offset: 0x0014AEE8
	public Vector3 importWorldPosition
	{
		get
		{
			return this._worldPosition;
		}
	}

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x06004740 RID: 18240 RVA: 0x0014CAF0 File Offset: 0x0014AEF0
	public Vector3 maleWorldPosition
	{
		get
		{
			return this._maleWorldPosition;
		}
	}

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x06004741 RID: 18241 RVA: 0x0014CAF8 File Offset: 0x0014AEF8
	public Vector3 worldOrientation
	{
		get
		{
			if (this.dazBones != null && this.dazBones.isMale)
			{
				return this._maleWorldOrientation;
			}
			return this._worldOrientation;
		}
	}

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x06004742 RID: 18242 RVA: 0x0014CB28 File Offset: 0x0014AF28
	public Vector3 importWorldOrientation
	{
		get
		{
			return this._worldOrientation;
		}
	}

	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x06004743 RID: 18243 RVA: 0x0014CB30 File Offset: 0x0014AF30
	public Vector3 maleWorldOrientation
	{
		get
		{
			return this._maleWorldOrientation;
		}
	}

	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x06004744 RID: 18244 RVA: 0x0014CB38 File Offset: 0x0014AF38
	public Vector3 morphedWorldPosition
	{
		get
		{
			return this._morphedWorldPosition;
		}
	}

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x06004745 RID: 18245 RVA: 0x0014CB40 File Offset: 0x0014AF40
	public Vector3 morphedWorldOrientation
	{
		get
		{
			return this._morphedWorldOrientation;
		}
	}

	// Token: 0x170009F8 RID: 2552
	// (get) Token: 0x06004746 RID: 18246 RVA: 0x0014CB48 File Offset: 0x0014AF48
	public Matrix4x4 morphedLocalToWorldMatrix
	{
		get
		{
			return this._morphedLocalToWorldMatrix;
		}
	}

	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x06004747 RID: 18247 RVA: 0x0014CB50 File Offset: 0x0014AF50
	public Matrix4x4 morphedWorldToLocalMatrix
	{
		get
		{
			return this._morphedWorldToLocalMatrix;
		}
	}

	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x06004748 RID: 18248 RVA: 0x0014CB58 File Offset: 0x0014AF58
	public Matrix4x4 nonMorphedLocalToWorldMatrix
	{
		get
		{
			return this._nonMorphedLocalToWorldMatrix;
		}
	}

	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x06004749 RID: 18249 RVA: 0x0014CB60 File Offset: 0x0014AF60
	public Matrix4x4 nonMorphedWorldToLocalMatrix
	{
		get
		{
			return this._nonMorphedWorldToLocalMatrix;
		}
	}

	// Token: 0x170009FC RID: 2556
	// (get) Token: 0x0600474A RID: 18250 RVA: 0x0014CB68 File Offset: 0x0014AF68
	public Quaternion2Angles.RotationOrder rotationOrder
	{
		get
		{
			if (this.dazBones != null && this.dazBones.isMale)
			{
				return this._maleRotationOrder;
			}
			return this._rotationOrder;
		}
	}

	// Token: 0x170009FD RID: 2557
	// (get) Token: 0x0600474B RID: 18251 RVA: 0x0014CB98 File Offset: 0x0014AF98
	public Dictionary<string, Vector3> morphOffsets
	{
		get
		{
			return this._morphOffsets;
		}
	}

	// Token: 0x170009FE RID: 2558
	// (get) Token: 0x0600474C RID: 18252 RVA: 0x0014CBA0 File Offset: 0x0014AFA0
	public Dictionary<string, Vector3> morphOrientationOffsets
	{
		get
		{
			return this._morphOrientationOffsets;
		}
	}

	// Token: 0x170009FF RID: 2559
	// (get) Token: 0x0600474D RID: 18253 RVA: 0x0014CBA8 File Offset: 0x0014AFA8
	public Dictionary<string, Vector3> morphRotations
	{
		get
		{
			return this._morphRotations;
		}
	}

	// Token: 0x0600474E RID: 18254 RVA: 0x0014CBB0 File Offset: 0x0014AFB0
	public void ForceClearMorphs()
	{
		this._morphOffsets.Clear();
		this._morphOrientationOffsets.Clear();
		this._morphRotations.Clear();
	}

	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x0600474F RID: 18255 RVA: 0x0014CBD3 File Offset: 0x0014AFD3
	public Vector3 startingLocalPosition
	{
		get
		{
			return this._startingLocalPosition;
		}
	}

	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x06004750 RID: 18256 RVA: 0x0014CBDB File Offset: 0x0014AFDB
	public Quaternion inverseStartingLocalRotation
	{
		get
		{
			return this._inverseStartingLocalRotation;
		}
	}

	// Token: 0x17000A02 RID: 2562
	// (get) Token: 0x06004751 RID: 18257 RVA: 0x0014CBE3 File Offset: 0x0014AFE3
	public Quaternion startingLocalRotation
	{
		get
		{
			return this._startingLocalRotation;
		}
	}

	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x06004752 RID: 18258 RVA: 0x0014CBEB File Offset: 0x0014AFEB
	public Quaternion startingRotationRelativeToRoot
	{
		get
		{
			return this._startingRotationRelativeToRoot;
		}
	}

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x06004753 RID: 18259 RVA: 0x0014CBF3 File Offset: 0x0014AFF3
	public FreeControllerV3 control
	{
		get
		{
			return this.saveControl;
		}
	}

	// Token: 0x06004754 RID: 18260 RVA: 0x0014CBFC File Offset: 0x0014AFFC
	public void ImportNode(JSONNode jn, bool isMale)
	{
		this._id = jn["id"];
		IEnumerator enumerator = jn["center_point"].AsArray.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				JSONNode jsonnode = (JSONNode)obj;
				string a = jsonnode["id"];
				if (a == "x")
				{
					if (isMale)
					{
						this._maleWorldPosition.x = jsonnode["value"].AsFloat * -0.01f;
					}
					else
					{
						this._worldPosition.x = jsonnode["value"].AsFloat * -0.01f;
					}
				}
				else if (a == "y")
				{
					if (isMale)
					{
						this._maleWorldPosition.y = jsonnode["value"].AsFloat * 0.01f;
					}
					else
					{
						this._worldPosition.y = jsonnode["value"].AsFloat * 0.01f;
					}
				}
				else if (a == "z")
				{
					if (isMale)
					{
						this._maleWorldPosition.z = jsonnode["value"].AsFloat * 0.01f;
					}
					else
					{
						this._worldPosition.z = jsonnode["value"].AsFloat * 0.01f;
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		IEnumerator enumerator2 = jn["orientation"].AsArray.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				JSONNode jsonnode2 = (JSONNode)obj2;
				string a2 = jsonnode2["id"];
				if (a2 == "x")
				{
					if (isMale)
					{
						this._maleWorldOrientation.x = jsonnode2["value"].AsFloat;
					}
					else
					{
						this._worldOrientation.x = jsonnode2["value"].AsFloat;
					}
				}
				else if (a2 == "y")
				{
					if (isMale)
					{
						this._maleWorldOrientation.y = -jsonnode2["value"].AsFloat;
					}
					else
					{
						this._worldOrientation.y = -jsonnode2["value"].AsFloat;
					}
				}
				else if (a2 == "z")
				{
					if (isMale)
					{
						this._maleWorldOrientation.z = -jsonnode2["value"].AsFloat;
					}
					else
					{
						this._worldOrientation.z = -jsonnode2["value"].AsFloat;
					}
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		string text = jn["rotation_order"];
		Quaternion2Angles.RotationOrder rotationOrder;
		if (text != null)
		{
			if (text == "XYZ")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.ZYX;
				goto IL_3CF;
			}
			if (text == "XZY")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.YZX;
				goto IL_3CF;
			}
			if (text == "YXZ")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.ZXY;
				goto IL_3CF;
			}
			if (text == "YZX")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.XZY;
				goto IL_3CF;
			}
			if (text == "ZXY")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.YXZ;
				goto IL_3CF;
			}
			if (text == "ZYX")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.XYZ;
				goto IL_3CF;
			}
		}
		Debug.LogError("Bad rotation order in json: " + text);
		rotationOrder = Quaternion2Angles.RotationOrder.XYZ;
		IL_3CF:
		if (isMale)
		{
			this._maleRotationOrder = rotationOrder;
		}
		else
		{
			this._rotationOrder = rotationOrder;
		}
		this.SetTransformToImportValues();
	}

	// Token: 0x06004755 RID: 18261 RVA: 0x0014D030 File Offset: 0x0014B430
	public void Rotate(Vector3 rotationToUse)
	{
		switch (this.rotationOrder)
		{
		case Quaternion2Angles.RotationOrder.XYZ:
			base.transform.Rotate(rotationToUse.x, 0f, 0f);
			base.transform.Rotate(0f, rotationToUse.y, 0f);
			base.transform.Rotate(0f, 0f, rotationToUse.z);
			break;
		case Quaternion2Angles.RotationOrder.XZY:
			base.transform.Rotate(rotationToUse.x, 0f, 0f);
			base.transform.Rotate(0f, 0f, rotationToUse.z);
			base.transform.Rotate(0f, rotationToUse.y, 0f);
			break;
		case Quaternion2Angles.RotationOrder.YXZ:
			base.transform.Rotate(0f, rotationToUse.y, 0f);
			base.transform.Rotate(rotationToUse.x, 0f, 0f);
			base.transform.Rotate(0f, 0f, rotationToUse.z);
			break;
		case Quaternion2Angles.RotationOrder.YZX:
			base.transform.Rotate(0f, rotationToUse.y, 0f);
			base.transform.Rotate(0f, 0f, rotationToUse.z);
			base.transform.Rotate(rotationToUse.x, 0f, 0f);
			break;
		case Quaternion2Angles.RotationOrder.ZXY:
			base.transform.Rotate(0f, 0f, rotationToUse.z);
			base.transform.Rotate(rotationToUse.x, 0f, 0f);
			base.transform.Rotate(0f, rotationToUse.y, 0f);
			break;
		case Quaternion2Angles.RotationOrder.ZYX:
			base.transform.Rotate(0f, 0f, rotationToUse.z);
			base.transform.Rotate(0f, rotationToUse.y, 0f);
			base.transform.Rotate(rotationToUse.x, 0f, 0f);
			break;
		}
	}

	// Token: 0x06004756 RID: 18262 RVA: 0x0014D282 File Offset: 0x0014B682
	public void ApplyOffsetTransform()
	{
		if (this.dazBones != null)
		{
			base.transform.position += this.dazBones.transform.position;
		}
	}

	// Token: 0x06004757 RID: 18263 RVA: 0x0014D2BC File Offset: 0x0014B6BC
	public void SetTransformToImportValues()
	{
		if (!Application.isPlaying)
		{
			base.transform.position = this.worldPosition;
			if (this.useUnityEulerOrientation)
			{
				base.transform.rotation = Quaternion.Euler(this.worldOrientation);
			}
			else
			{
				base.transform.rotation = Quaternion2Angles.EulerToQuaternion(this.worldOrientation, Quaternion2Angles.RotationOrder.ZYX);
			}
			this.ApplyOffsetTransform();
		}
	}

	// Token: 0x06004758 RID: 18264 RVA: 0x0014D327 File Offset: 0x0014B727
	public void ApplyPresetLocalTransforms()
	{
		base.transform.localPosition += this.presetLocalTranslation;
		this.Rotate(this.presetLocalRotation);
	}

	// Token: 0x06004759 RID: 18265 RVA: 0x0014D354 File Offset: 0x0014B754
	public void SetImportValuesToTransform()
	{
		if (!Application.isPlaying)
		{
			if (this.dazBones != null && this.dazBones.isMale)
			{
				this._maleWorldPosition = base.transform.position;
				this._maleWorldOrientation = base.transform.eulerAngles;
			}
			else
			{
				this._worldPosition = base.transform.position;
				this._worldOrientation = base.transform.eulerAngles;
			}
		}
	}

	// Token: 0x0600475A RID: 18266 RVA: 0x0014D3D8 File Offset: 0x0014B7D8
	public void SetTransformToMorphPositionAndRotation(bool useScale, float globalScale)
	{
		this.transformDirty = false;
		this.childDirty = false;
		base.transform.position = this._morphedWorldPosition;
		if (this.useUnityEulerOrientation)
		{
			base.transform.rotation = Quaternion.Euler(this._morphedWorldOrientation);
		}
		else
		{
			base.transform.rotation = Quaternion2Angles.EulerToQuaternion(this._morphedWorldOrientation, Quaternion2Angles.RotationOrder.ZYX);
		}
		if (useScale)
		{
			base.transform.position *= globalScale;
		}
	}

	// Token: 0x0600475B RID: 18267 RVA: 0x0014D460 File Offset: 0x0014B860
	public void SetMorphedTransform(bool useScale, float globalScale)
	{
		this.transformDirty = false;
		this.childDirty = false;
		this._nonMorphedLocalToWorldMatrix = this._morphedLocalToWorldMatrix;
		this._nonMorphedWorldToLocalMatrix = this._morphedWorldToLocalMatrix;
		if (!this.disableMorph)
		{
			base.transform.position = this.worldPosition;
			if (this.useUnityEulerOrientation)
			{
				base.transform.rotation = Quaternion.Euler(this.worldOrientation);
			}
			else
			{
				base.transform.rotation = Quaternion2Angles.EulerToQuaternion(this.worldOrientation, Quaternion2Angles.RotationOrder.ZYX);
			}
			this._nonMorphedLocalToWorldMatrix = base.transform.localToWorldMatrix;
			this._nonMorphedWorldToLocalMatrix = base.transform.worldToLocalMatrix;
			this._morphedWorldPosition = this.worldPosition;
			this.InitMorphOffsets();
			foreach (string key in this._morphOffsets.Keys)
			{
				Vector3 b;
				if (this._morphOffsets.TryGetValue(key, out b))
				{
					this._morphedWorldPosition += b;
				}
			}
			if (this.parentForMorphOffsets != null)
			{
				foreach (string key2 in this.parentForMorphOffsets.morphOffsets.Keys)
				{
					Vector3 b2;
					if (this.parentForMorphOffsets.morphOffsets.TryGetValue(key2, out b2))
					{
						this._morphedWorldPosition += b2;
					}
				}
			}
			base.transform.position = this._morphedWorldPosition;
			this._morphedWorldOrientation = this.worldOrientation;
			foreach (string key3 in this._morphOrientationOffsets.Keys)
			{
				Vector3 b3;
				if (this._morphOrientationOffsets.TryGetValue(key3, out b3))
				{
					this._morphedWorldOrientation += b3;
				}
			}
			if (this.parentForMorphOffsets != null)
			{
				foreach (string key4 in this.parentForMorphOffsets.morphOrientationOffsets.Keys)
				{
					Vector3 b4;
					if (this.parentForMorphOffsets.morphOrientationOffsets.TryGetValue(key4, out b4))
					{
						this._morphedWorldOrientation += b4;
					}
				}
			}
			if (this.useUnityEulerOrientation)
			{
				base.transform.rotation = Quaternion.Euler(this._morphedWorldOrientation);
			}
			else
			{
				base.transform.rotation = Quaternion2Angles.EulerToQuaternion(this._morphedWorldOrientation, Quaternion2Angles.RotationOrder.ZYX);
			}
			this._morphedLocalToWorldMatrix = base.transform.localToWorldMatrix;
			this._morphedWorldToLocalMatrix = base.transform.worldToLocalMatrix;
			if (this.saveControl != null)
			{
				this.saveControl.transform.position = base.transform.position;
				this.saveControl.transform.rotation = base.transform.rotation;
			}
			if (useScale)
			{
				base.transform.position *= globalScale;
			}
		}
	}

	// Token: 0x0600475C RID: 18268 RVA: 0x0014D7FC File Offset: 0x0014BBFC
	public void SaveTransform()
	{
		this.saveBonePosition = base.transform.position;
		this.saveBoneRotation = base.transform.rotation;
		if (this.saveControl != null)
		{
			this.saveControl.PauseComply(10);
			this.saveControlPosition = this.saveControl.transform.position;
			this.saveControlRotation = this.saveControl.transform.rotation;
		}
	}

	// Token: 0x0600475D RID: 18269 RVA: 0x0014D878 File Offset: 0x0014BC78
	public void RestoreTransform()
	{
		if (!this.disableMorph)
		{
			ConfigurableJoint[] components = base.GetComponents<ConfigurableJoint>();
			if (components.Length > 0)
			{
				if (this.saveControl != null)
				{
					this.saveControl.transform.position = this.saveControlPosition;
					this.saveControl.transform.rotation = this.saveControlRotation;
				}
				if (this.didDetachJoint)
				{
					this.didDetachJoint = false;
					if (this.boneRigidbody != null && this.boneRigidbody.constraints != RigidbodyConstraints.FreezeAll)
					{
						base.transform.position = this.saveBonePosition;
					}
				}
				else
				{
					base.transform.position = this.saveBonePosition;
				}
				base.transform.rotation = this.saveBoneRotation;
				RigidbodyAttributes component = base.GetComponent<RigidbodyAttributes>();
				if (component != null)
				{
					component.TempDisableInterpolation();
				}
				JointPositionHardLimit component2 = base.GetComponent<JointPositionHardLimit>();
				if (component2 != null && component2.useOffsetPosition)
				{
					component2.SetTargetPositionFromPercent();
				}
			}
			else
			{
				AdjustRotationTarget component3 = base.GetComponent<AdjustRotationTarget>();
				if (component3 != null)
				{
					component3.Adjust();
				}
				else
				{
					base.transform.rotation = this.saveBoneRotation;
				}
			}
		}
	}

	// Token: 0x0600475E RID: 18270 RVA: 0x0014D9BB File Offset: 0x0014BDBB
	public void SaveAndDetachParent()
	{
		if (!this.disableMorph)
		{
			this.saveParent = base.transform.parent;
			base.transform.parent = null;
		}
	}

	// Token: 0x0600475F RID: 18271 RVA: 0x0014D9E8 File Offset: 0x0014BDE8
	public void RestoreParent()
	{
		if (!this.disableMorph)
		{
			base.transform.parent = this.saveParent;
			if (!this.isRoot)
			{
				this._startingLocalPosition = base.transform.localPosition;
				this._startingLocalRotation = base.transform.localRotation;
				this._inverseStartingLocalRotation = Quaternion.Inverse(this._startingLocalRotation);
			}
			this.ResetScale();
		}
	}

	// Token: 0x06004760 RID: 18272 RVA: 0x0014DA55 File Offset: 0x0014BE55
	public void ResetScale()
	{
		base.transform.localScale = Vector3.one;
	}

	// Token: 0x06004761 RID: 18273 RVA: 0x0014DA67 File Offset: 0x0014BE67
	public void ResetToStartingLocalPositionRotation()
	{
		if (!this.isRoot)
		{
			base.transform.localPosition = this._startingLocalPosition;
			base.transform.localRotation = this._startingLocalRotation;
		}
	}

	// Token: 0x06004762 RID: 18274 RVA: 0x0014DA98 File Offset: 0x0014BE98
	public void DetachJoint()
	{
		if (!this.disableMorph && !this.isRoot)
		{
			ConfigurableJoint[] components = base.GetComponents<ConfigurableJoint>();
			if (components.Length > 0)
			{
				ConfigurableJoint configurableJoint = components[0];
				if (configurableJoint != null && configurableJoint.connectedBody != null && !configurableJoint.connectedBody.GetComponent<FreeControllerV3>())
				{
					this.didDetachJoint = true;
					this.saveConnectedBody = configurableJoint.connectedBody;
					configurableJoint.connectedBody = null;
					if (this.saveControlJoint != null)
					{
						this.saveControlJoint.connectedBody = null;
					}
				}
			}
		}
	}

	// Token: 0x06004763 RID: 18275 RVA: 0x0014DB38 File Offset: 0x0014BF38
	public void AttachJoint()
	{
		if (!this.isRoot)
		{
			ConfigurableJoint[] components = base.GetComponents<ConfigurableJoint>();
			if (components.Length > 0)
			{
				ConfigurableJoint configurableJoint = components[0];
				if (this.didDetachJoint)
				{
					configurableJoint.connectedBody = this.saveConnectedBody;
					Vector3 localPosition = base.transform.localPosition;
					if (configurableJoint.connectedAnchor != localPosition)
					{
						configurableJoint.connectedAnchor = localPosition;
					}
					JointPositionHardLimit component = base.GetComponent<JointPositionHardLimit>();
					if (component != null)
					{
						component.startAnchor = configurableJoint.connectedAnchor;
						component.startRotation = base.transform.localRotation;
					}
					if (this.saveControl != null)
					{
						Rigidbody component2 = this.saveControl.GetComponent<Rigidbody>();
						this.saveControlJoint.connectedBody = component2;
					}
				}
			}
		}
	}

	// Token: 0x06004764 RID: 18276 RVA: 0x0014DBFC File Offset: 0x0014BFFC
	public void RepairJoint()
	{
		if (!this.disableMorph && !this.isRoot)
		{
			ConfigurableJoint[] components = base.GetComponents<ConfigurableJoint>();
			if (components.Length > 0)
			{
				ConfigurableJoint configurableJoint = components[0];
				if (configurableJoint != null && configurableJoint.connectedBody != null && !configurableJoint.connectedBody.GetComponent<FreeControllerV3>())
				{
					base.transform.localPosition = configurableJoint.connectedAnchor;
				}
			}
		}
	}

	// Token: 0x06004765 RID: 18277 RVA: 0x0014DC78 File Offset: 0x0014C078
	private void InitMorphOffsets()
	{
		if (this._morphOffsets == null)
		{
			this._morphOffsets = new Dictionary<string, Vector3>();
		}
		if (this._morphOrientationOffsets == null)
		{
			this._morphOrientationOffsets = new Dictionary<string, Vector3>();
		}
		if (this._morphRotations == null)
		{
			this._morphRotations = new Dictionary<string, Vector3>();
		}
	}

	// Token: 0x06004766 RID: 18278 RVA: 0x0014DCC8 File Offset: 0x0014C0C8
	public void SetBoneXOffset(string morphName, float xoffset)
	{
		this.InitMorphOffsets();
		Vector3 vector = this.zeroVector;
		if (this._morphOffsets.TryGetValue(morphName, out vector))
		{
			vector.x = xoffset;
			this._morphOffsets.Remove(morphName);
			if (vector != this.zeroVector)
			{
				this._morphOffsets.Add(morphName, vector);
			}
		}
		else
		{
			vector.x = xoffset;
			if (vector != this.zeroVector)
			{
				this._morphOffsets.Add(morphName, vector);
			}
		}
		this.transformDirty = true;
	}

	// Token: 0x06004767 RID: 18279 RVA: 0x0014DD5C File Offset: 0x0014C15C
	public void SetBoneYOffset(string morphName, float yoffset)
	{
		this.InitMorphOffsets();
		Vector3 vector = this.zeroVector;
		if (this._morphOffsets.TryGetValue(morphName, out vector))
		{
			vector.y = yoffset;
			this._morphOffsets.Remove(morphName);
			if (vector != this.zeroVector)
			{
				this._morphOffsets.Add(morphName, vector);
			}
		}
		else
		{
			vector.y = yoffset;
			if (vector != this.zeroVector)
			{
				this._morphOffsets.Add(morphName, vector);
			}
		}
		this.transformDirty = true;
	}

	// Token: 0x06004768 RID: 18280 RVA: 0x0014DDF0 File Offset: 0x0014C1F0
	public void SetBoneZOffset(string morphName, float zoffset)
	{
		this.InitMorphOffsets();
		Vector3 vector = this.zeroVector;
		if (this._morphOffsets.TryGetValue(morphName, out vector))
		{
			vector.z = zoffset;
			this._morphOffsets.Remove(morphName);
			if (vector != this.zeroVector)
			{
				this._morphOffsets.Add(morphName, vector);
			}
		}
		else
		{
			vector.z = zoffset;
			if (vector != this.zeroVector)
			{
				this._morphOffsets.Add(morphName, vector);
			}
		}
		this.transformDirty = true;
	}

	// Token: 0x06004769 RID: 18281 RVA: 0x0014DE84 File Offset: 0x0014C284
	public void SetBoneOrientationXOffset(string morphName, float xoffset)
	{
		this.InitMorphOffsets();
		Vector3 vector = this.zeroVector;
		if (this._morphOrientationOffsets.TryGetValue(morphName, out vector))
		{
			vector.x = xoffset;
			this._morphOrientationOffsets.Remove(morphName);
			if (vector != this.zeroVector)
			{
				this._morphOrientationOffsets.Add(morphName, vector);
			}
		}
		else
		{
			vector.x = xoffset;
			if (vector != this.zeroVector)
			{
				this._morphOrientationOffsets.Add(morphName, vector);
			}
		}
		this.transformDirty = true;
	}

	// Token: 0x0600476A RID: 18282 RVA: 0x0014DF18 File Offset: 0x0014C318
	public void SetBoneOrientationYOffset(string morphName, float yoffset)
	{
		this.InitMorphOffsets();
		Vector3 vector = this.zeroVector;
		if (this._morphOrientationOffsets.TryGetValue(morphName, out vector))
		{
			vector.y = yoffset;
			this._morphOrientationOffsets.Remove(morphName);
			if (vector != this.zeroVector)
			{
				this._morphOrientationOffsets.Add(morphName, vector);
			}
		}
		else
		{
			vector.y = yoffset;
			if (vector != this.zeroVector)
			{
				this._morphOrientationOffsets.Add(morphName, vector);
			}
		}
		this.transformDirty = true;
	}

	// Token: 0x0600476B RID: 18283 RVA: 0x0014DFAC File Offset: 0x0014C3AC
	public void SetBoneOrientationZOffset(string morphName, float zoffset)
	{
		this.InitMorphOffsets();
		Vector3 vector = this.zeroVector;
		if (this._morphOrientationOffsets.TryGetValue(morphName, out vector))
		{
			vector.z = zoffset;
			this._morphOrientationOffsets.Remove(morphName);
			if (vector != this.zeroVector)
			{
				this._morphOrientationOffsets.Add(morphName, vector);
			}
		}
		else
		{
			vector.z = zoffset;
			if (vector != this.zeroVector)
			{
				this._morphOrientationOffsets.Add(morphName, vector);
			}
		}
		this.transformDirty = true;
	}

	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x0600476C RID: 18284 RVA: 0x0014E03E File Offset: 0x0014C43E
	// (set) Token: 0x0600476D RID: 18285 RVA: 0x0014E046 File Offset: 0x0014C446
	public Vector3 baseJointRotation
	{
		get
		{
			return this._baseJointRotation;
		}
		set
		{
			this._baseJointRotation = value;
			this.SyncMorphBoneRotations(false);
		}
	}

	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x0600476E RID: 18286 RVA: 0x0014E056 File Offset: 0x0014C456
	// (set) Token: 0x0600476F RID: 18287 RVA: 0x0014E05E File Offset: 0x0014C45E
	public bool rotationMorphsEnabled
	{
		get
		{
			return this._rotationMorphsEnabled;
		}
		set
		{
			if (this._rotationMorphsEnabled != value)
			{
				this._rotationMorphsEnabled = value;
				this.SyncMorphBoneRotations(true);
			}
		}
	}

	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x06004770 RID: 18288 RVA: 0x0014E07A File Offset: 0x0014C47A
	// (set) Token: 0x06004771 RID: 18289 RVA: 0x0014E082 File Offset: 0x0014C482
	public bool jointRotationDisabled
	{
		get
		{
			return this._jointRotationDisabled;
		}
		set
		{
			if (this.jointRotationDisabled != value)
			{
				this._jointRotationDisabled = value;
				this.SyncMorphBoneRotations(true);
			}
		}
	}

	// Token: 0x06004772 RID: 18290 RVA: 0x0014E0A0 File Offset: 0x0014C4A0
	public void SyncMorphBoneRotations(bool force = false)
	{
		if (!this._jointRotationDisabled && (this._rotationMorphsEnabled || force))
		{
			Vector3 vector = this._baseJointRotation;
			if (this._rotationMorphsEnabled)
			{
				foreach (string key in this.morphRotations.Keys)
				{
					Vector3 b;
					if (this.morphRotations.TryGetValue(key, out b))
					{
						vector += b;
					}
				}
			}
			ConfigurableJoint[] components = base.GetComponents<ConfigurableJoint>();
			if (components.Length > 0)
			{
				Vector3 r = vector;
				if (this.useCustomJointMap)
				{
					switch (this.xJointMap)
					{
					case DAZBone.JointTarget.X:
						r.x = vector.x;
						break;
					case DAZBone.JointTarget.NegX:
						r.x = -vector.x;
						break;
					case DAZBone.JointTarget.Y:
						r.x = vector.y;
						break;
					case DAZBone.JointTarget.NegY:
						r.x = -vector.y;
						break;
					case DAZBone.JointTarget.Z:
						r.x = vector.z;
						break;
					case DAZBone.JointTarget.NegZ:
						r.x = -vector.z;
						break;
					}
					switch (this.yJointMap)
					{
					case DAZBone.JointTarget.X:
						r.y = vector.x;
						break;
					case DAZBone.JointTarget.NegX:
						r.y = -vector.x;
						break;
					case DAZBone.JointTarget.Y:
						r.y = vector.y;
						break;
					case DAZBone.JointTarget.NegY:
						r.y = -vector.y;
						break;
					case DAZBone.JointTarget.Z:
						r.y = vector.z;
						break;
					case DAZBone.JointTarget.NegZ:
						r.y = -vector.z;
						break;
					}
					switch (this.zJointMap)
					{
					case DAZBone.JointTarget.X:
						r.z = vector.x;
						break;
					case DAZBone.JointTarget.NegX:
						r.z = -vector.x;
						break;
					case DAZBone.JointTarget.Y:
						r.z = vector.y;
						break;
					case DAZBone.JointTarget.NegY:
						r.z = -vector.y;
						break;
					case DAZBone.JointTarget.Z:
						r.z = vector.z;
						break;
					case DAZBone.JointTarget.NegZ:
						r.z = -vector.z;
						break;
					}
				}
				else
				{
					ConfigurableJoint configurableJoint = components[0];
					if (configurableJoint.axis.x == 1f)
					{
						r.x = -vector.x;
						if (configurableJoint.secondaryAxis.y == 1f)
						{
							r.y = vector.y;
							r.z = vector.z;
						}
						else
						{
							r.y = vector.z;
							r.z = vector.y;
						}
					}
					else if (configurableJoint.axis.y == 1f)
					{
						r.x = vector.y;
						if (configurableJoint.secondaryAxis.x == 1f)
						{
							r.y = -vector.x;
							r.z = vector.z;
						}
						else
						{
							r.y = vector.z;
							r.z = -vector.x;
						}
					}
					else
					{
						r.x = vector.z;
						if (configurableJoint.secondaryAxis.x == 1f)
						{
							r.y = -vector.x;
							r.z = vector.y;
						}
						else
						{
							r.y = vector.y;
							r.z = -vector.x;
						}
					}
				}
				if (components.Length > 1)
				{
					Rigidbody connectedBody = components[1].connectedBody;
					FreeControllerV3 component = connectedBody.GetComponent<FreeControllerV3>();
					if (component != null)
					{
						component.jointRotationDriveXTargetAdditional = r.x;
						component.jointRotationDriveYTargetAdditional = r.y;
						component.jointRotationDriveZTargetAdditional = r.z;
					}
					else
					{
						this.SetJointDriveTargetRotation(components[0], r);
					}
				}
				else
				{
					this.SetJointDriveTargetRotation(components[0], r);
				}
			}
		}
	}

	// Token: 0x06004773 RID: 18291 RVA: 0x0014E53C File Offset: 0x0014C93C
	protected void SetJointDriveTargetRotation(ConfigurableJoint cj, Vector3 r)
	{
		Quaternion targetRotation = Quaternion2Angles.EulerToQuaternion(r, this.jointDriveTargetRotationOrder);
		cj.targetRotation = targetRotation;
	}

	// Token: 0x06004774 RID: 18292 RVA: 0x0014E560 File Offset: 0x0014C960
	public void SetBoneXRotation(string morphName, float rot)
	{
		this.InitMorphOffsets();
		Vector3 vector = this.zeroVector;
		if (this._morphRotations.TryGetValue(morphName, out vector))
		{
			vector.x = rot;
			this._morphRotations.Remove(morphName);
			if (vector != this.zeroVector)
			{
				this._morphRotations.Add(morphName, vector);
			}
		}
		else
		{
			vector.x = rot;
			if (vector != this.zeroVector)
			{
				this._morphRotations.Add(morphName, vector);
			}
		}
	}

	// Token: 0x06004775 RID: 18293 RVA: 0x0014E5EC File Offset: 0x0014C9EC
	public void SetBoneYRotation(string morphName, float rot)
	{
		this.InitMorphOffsets();
		Vector3 vector = this.zeroVector;
		if (this._morphRotations.TryGetValue(morphName, out vector))
		{
			vector.y = rot;
			this._morphRotations.Remove(morphName);
			if (vector != this.zeroVector)
			{
				this._morphRotations.Add(morphName, vector);
			}
		}
		else
		{
			vector.y = rot;
			if (vector != this.zeroVector)
			{
				this._morphRotations.Add(morphName, vector);
			}
		}
	}

	// Token: 0x06004776 RID: 18294 RVA: 0x0014E678 File Offset: 0x0014CA78
	public void SetBoneZRotation(string morphName, float rot)
	{
		this.InitMorphOffsets();
		Vector3 vector = this.zeroVector;
		if (this._morphRotations.TryGetValue(morphName, out vector))
		{
			vector.z = rot;
			this._morphRotations.Remove(morphName);
			if (vector != this.zeroVector)
			{
				this._morphRotations.Add(morphName, vector);
			}
		}
		else
		{
			vector.z = rot;
			if (vector != this.zeroVector)
			{
				this._morphRotations.Add(morphName, vector);
			}
		}
	}

	// Token: 0x06004777 RID: 18295 RVA: 0x0014E703 File Offset: 0x0014CB03
	public void SetBoneScaleX(string morphName, float xscale)
	{
	}

	// Token: 0x06004778 RID: 18296 RVA: 0x0014E705 File Offset: 0x0014CB05
	public Vector3 GetAngles()
	{
		return this.currentAnglesRadians;
	}

	// Token: 0x06004779 RID: 18297 RVA: 0x0014E70D File Offset: 0x0014CB0D
	public Vector3 GetAnglesDegrees()
	{
		return this.currentAngles;
	}

	// Token: 0x0600477A RID: 18298 RVA: 0x0014E718 File Offset: 0x0014CB18
	public void Init()
	{
		if (!this.wasInit)
		{
			this.wasInit = true;
			this._startingLocalPosition = base.transform.localPosition;
			this._startingLocalRotation = base.transform.localRotation;
			this._inverseStartingLocalRotation = Quaternion.Inverse(this._startingLocalRotation);
			if (this.dazBones != null)
			{
				this._startingRotationRelativeToRoot = Quaternion.Inverse(this.dazBones.transform.rotation) * base.transform.rotation;
			}
			this.changeFromOriginalMatrix = base.transform.localToWorldMatrix * this._morphedLocalToWorldMatrix;
			this.currentAnglesRadians = Quaternion2Angles.GetAngles(this._inverseStartingLocalRotation * base.transform.localRotation, this.rotationOrder);
			this.currentAngles = this.currentAnglesRadians * 57.29578f;
			this.InitMorphOffsets();
			this.saveControl = null;
			this.saveControlJoint = null;
			this.boneRigidbody = base.GetComponent<Rigidbody>();
			ConfigurableJoint[] components = base.GetComponents<ConfigurableJoint>();
			foreach (ConfigurableJoint configurableJoint in components)
			{
				Rigidbody connectedBody = configurableJoint.connectedBody;
				if (connectedBody != null)
				{
					this.saveControl = connectedBody.GetComponent<FreeControllerV3>();
					this.saveControlJoint = configurableJoint;
				}
			}
		}
	}

	// Token: 0x0600477B RID: 18299 RVA: 0x0014E869 File Offset: 0x0014CC69
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
		}
	}

	// Token: 0x0600477C RID: 18300 RVA: 0x0014E882 File Offset: 0x0014CC82
	public void SetResetVelocity()
	{
		if (this.boneRigidbody != null)
		{
			this.boneRigidbody.velocity = Vector3.zero;
			this.boneRigidbody.angularVelocity = Vector3.zero;
		}
	}

	// Token: 0x0600477D RID: 18301 RVA: 0x0014E8B5 File Offset: 0x0014CCB5
	public void PrepThreadUpdate()
	{
		this._localToWorldMatrix = base.transform.localToWorldMatrix;
		this._localRotation = base.transform.localRotation;
	}

	// Token: 0x0600477E RID: 18302 RVA: 0x0014E8DC File Offset: 0x0014CCDC
	public void ThreadsafeUpdate()
	{
		if (NaNUtils.IsMatrixValid(this._localToWorldMatrix) && NaNUtils.IsMatrixValid(this._morphedLocalToWorldMatrix) && NaNUtils.IsQuaternionValid(this._localRotation))
		{
			this.worldToLocalMatrix = this._localToWorldMatrix.inverse;
			this.changeFromOriginalMatrix = this._localToWorldMatrix * this._morphedWorldToLocalMatrix;
			Vector3 angles = Quaternion2Angles.GetAngles(this._inverseStartingLocalRotation * this._localRotation, this.rotationOrder);
			if (NaNUtils.IsVector3Valid(angles))
			{
				this.currentAnglesRadians = angles;
				this.currentAngles = this.currentAnglesRadians * 57.29578f;
			}
			else
			{
				this.detectedPhysicsCorruptionOnThread = true;
				this.physicsCorruptionType = "Quaternion2Angles";
			}
		}
		else
		{
			this.detectedPhysicsCorruptionOnThread = true;
			this.physicsCorruptionType = "Matrix";
		}
	}

	// Token: 0x0600477F RID: 18303 RVA: 0x0014E9B4 File Offset: 0x0014CDB4
	public void FinishThreadUpdate()
	{
		if (this.detectedPhysicsCorruptionOnThread)
		{
			if (this.containingAtom != null)
			{
				this.containingAtom.AlertPhysicsCorruption("DAZBone " + this.physicsCorruptionType + " " + base.name);
			}
			this.detectedPhysicsCorruptionOnThread = false;
		}
	}

	// Token: 0x06004780 RID: 18304 RVA: 0x0014EA0C File Offset: 0x0014CE0C
	private void Update()
	{
		this.changeFromOriginalMatrix = base.transform.localToWorldMatrix * this._morphedWorldToLocalMatrix;
		this.currentAnglesRadians = Quaternion2Angles.GetAngles(this._inverseStartingLocalRotation * base.transform.localRotation, this.rotationOrder);
		this.currentAngles = this.currentAnglesRadians * 57.29578f;
	}

	// Token: 0x0400347E RID: 13438
	protected string[] customParamNames = new string[]
	{
		"relativeRootPosition",
		"relativeRootRotation",
		"rootPosition",
		"rootRotation",
		"position",
		"rotation"
	};

	// Token: 0x0400347F RID: 13439
	protected const float geoScale = 0.01f;

	// Token: 0x04003480 RID: 13440
	public bool isRoot;

	// Token: 0x04003481 RID: 13441
	[SerializeField]
	private string _id;

	// Token: 0x04003482 RID: 13442
	[SerializeField]
	private Vector3 _worldPosition;

	// Token: 0x04003483 RID: 13443
	[SerializeField]
	private Vector3 _maleWorldPosition;

	// Token: 0x04003484 RID: 13444
	[SerializeField]
	private Vector3 _worldOrientation;

	// Token: 0x04003485 RID: 13445
	[SerializeField]
	private Vector3 _maleWorldOrientation;

	// Token: 0x04003486 RID: 13446
	[SerializeField]
	private Vector3 _morphedWorldPosition;

	// Token: 0x04003487 RID: 13447
	[SerializeField]
	private Vector3 _morphedWorldOrientation;

	// Token: 0x04003488 RID: 13448
	[SerializeField]
	private Matrix4x4 _morphedLocalToWorldMatrix;

	// Token: 0x04003489 RID: 13449
	[SerializeField]
	private Matrix4x4 _morphedWorldToLocalMatrix;

	// Token: 0x0400348A RID: 13450
	public Matrix4x4 changeFromOriginalMatrix;

	// Token: 0x0400348B RID: 13451
	private Matrix4x4 _nonMorphedLocalToWorldMatrix;

	// Token: 0x0400348C RID: 13452
	private Matrix4x4 _nonMorphedWorldToLocalMatrix;

	// Token: 0x0400348D RID: 13453
	[SerializeField]
	private Quaternion2Angles.RotationOrder _maleRotationOrder;

	// Token: 0x0400348E RID: 13454
	[SerializeField]
	private Quaternion2Angles.RotationOrder _rotationOrder;

	// Token: 0x0400348F RID: 13455
	[SerializeField]
	private Dictionary<string, Vector3> _morphOffsets;

	// Token: 0x04003490 RID: 13456
	[SerializeField]
	private Dictionary<string, Vector3> _morphOrientationOffsets;

	// Token: 0x04003491 RID: 13457
	[SerializeField]
	private Dictionary<string, Vector3> _morphRotations;

	// Token: 0x04003492 RID: 13458
	public DAZBone parentForMorphOffsets;

	// Token: 0x04003493 RID: 13459
	public Vector3 currentAnglesRadians;

	// Token: 0x04003494 RID: 13460
	public Vector3 currentAngles;

	// Token: 0x04003495 RID: 13461
	public Vector3 presetLocalTranslation;

	// Token: 0x04003496 RID: 13462
	public Vector3 presetLocalRotation;

	// Token: 0x04003497 RID: 13463
	private Vector3 _startingLocalPosition;

	// Token: 0x04003498 RID: 13464
	private Quaternion _inverseStartingLocalRotation;

	// Token: 0x04003499 RID: 13465
	private Quaternion _startingLocalRotation;

	// Token: 0x0400349A RID: 13466
	private Quaternion _startingRotationRelativeToRoot;

	// Token: 0x0400349B RID: 13467
	public DAZBones dazBones;

	// Token: 0x0400349C RID: 13468
	public DAZBone parentBone;

	// Token: 0x0400349D RID: 13469
	public bool useUnityEulerOrientation;

	// Token: 0x0400349E RID: 13470
	public bool disableMorph;

	// Token: 0x0400349F RID: 13471
	private Vector3 saveBonePosition;

	// Token: 0x040034A0 RID: 13472
	private Quaternion saveBoneRotation;

	// Token: 0x040034A1 RID: 13473
	private Vector3 saveControlPosition;

	// Token: 0x040034A2 RID: 13474
	private Quaternion saveControlRotation;

	// Token: 0x040034A3 RID: 13475
	private FreeControllerV3 saveControl;

	// Token: 0x040034A4 RID: 13476
	private Rigidbody boneRigidbody;

	// Token: 0x040034A5 RID: 13477
	private ConfigurableJoint saveControlJoint;

	// Token: 0x040034A6 RID: 13478
	private Rigidbody saveConnectedBody;

	// Token: 0x040034A7 RID: 13479
	private Vector3 zeroVector = Vector3.zero;

	// Token: 0x040034A8 RID: 13480
	private Transform saveParent;

	// Token: 0x040034A9 RID: 13481
	public bool transformDirty;

	// Token: 0x040034AA RID: 13482
	public bool childDirty;

	// Token: 0x040034AB RID: 13483
	public bool useCustomJointMap;

	// Token: 0x040034AC RID: 13484
	public DAZBone.JointTarget xJointMap;

	// Token: 0x040034AD RID: 13485
	public DAZBone.JointTarget yJointMap = DAZBone.JointTarget.Y;

	// Token: 0x040034AE RID: 13486
	public DAZBone.JointTarget zJointMap = DAZBone.JointTarget.Z;

	// Token: 0x040034AF RID: 13487
	protected bool didDetachJoint;

	// Token: 0x040034B0 RID: 13488
	protected Vector3 _baseJointRotation = Vector3.zero;

	// Token: 0x040034B1 RID: 13489
	protected bool _rotationMorphsEnabled = true;

	// Token: 0x040034B2 RID: 13490
	protected bool _jointRotationDisabled;

	// Token: 0x040034B3 RID: 13491
	public Quaternion2Angles.RotationOrder jointDriveTargetRotationOrder;

	// Token: 0x040034B4 RID: 13492
	protected bool wasInit;

	// Token: 0x040034B5 RID: 13493
	public Matrix4x4 worldToLocalMatrix;

	// Token: 0x040034B6 RID: 13494
	protected Matrix4x4 _localToWorldMatrix;

	// Token: 0x040034B7 RID: 13495
	protected Quaternion _localRotation;

	// Token: 0x040034B8 RID: 13496
	protected bool detectedPhysicsCorruptionOnThread;

	// Token: 0x040034B9 RID: 13497
	protected string physicsCorruptionType = string.Empty;

	// Token: 0x02000AA8 RID: 2728
	public enum JointTarget
	{
		// Token: 0x040034BB RID: 13499
		X,
		// Token: 0x040034BC RID: 13500
		NegX,
		// Token: 0x040034BD RID: 13501
		Y,
		// Token: 0x040034BE RID: 13502
		NegY,
		// Token: 0x040034BF RID: 13503
		Z,
		// Token: 0x040034C0 RID: 13504
		NegZ
	}
}
