using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000B65 RID: 2917
public class MotionAnimationClip
{
	// Token: 0x06005163 RID: 20835 RVA: 0x001D5C84 File Offset: 0x001D4084
	public MotionAnimationClip()
	{
		this._steps = new List<MotionAnimationStep>();
		this._interpolatedStep = new MotionAnimationStep();
	}

	// Token: 0x06005164 RID: 20836 RVA: 0x001D5CAC File Offset: 0x001D40AC
	public bool SaveToJSON(JSONClass jc)
	{
		if (this._steps.Count > 0)
		{
			JSONArray jsonarray = new JSONArray();
			foreach (MotionAnimationStep motionAnimationStep in this._steps)
			{
				JSONClass jsonclass = new JSONClass();
				jsonclass["timeStep"].AsFloat = motionAnimationStep.timeStep;
				jsonclass["positionOn"].AsBool = motionAnimationStep.positionOn;
				jsonclass["rotationOn"].AsBool = motionAnimationStep.rotationOn;
				jsonclass["position"]["x"].AsFloat = motionAnimationStep.position.x;
				jsonclass["position"]["y"].AsFloat = motionAnimationStep.position.y;
				jsonclass["position"]["z"].AsFloat = motionAnimationStep.position.z;
				jsonclass["rotation"]["x"].AsFloat = motionAnimationStep.rotation.x;
				jsonclass["rotation"]["y"].AsFloat = motionAnimationStep.rotation.y;
				jsonclass["rotation"]["z"].AsFloat = motionAnimationStep.rotation.z;
				jsonclass["rotation"]["w"].AsFloat = motionAnimationStep.rotation.w;
				jsonarray.Add(jsonclass);
			}
			jc["steps"] = jsonarray;
			return true;
		}
		return false;
	}

	// Token: 0x06005165 RID: 20837 RVA: 0x001D5E88 File Offset: 0x001D4288
	public void RestoreFromJSON(JSONClass jc, bool setMissingToDefault = true)
	{
		JSONArray asArray = jc["steps"].AsArray;
		if (asArray != null)
		{
			this.ClearAllSteps();
			for (int i = 0; i < asArray.Count; i++)
			{
				JSONClass asObject = asArray[i].AsObject;
				if (asObject != null)
				{
					MotionAnimationStep motionAnimationStep = new MotionAnimationStep();
					if (asObject["positionOn"] != null)
					{
						motionAnimationStep.positionOn = asObject["positionOn"].AsBool;
					}
					else
					{
						motionAnimationStep.positionOn = true;
					}
					if (asObject["rotationOn"] != null)
					{
						motionAnimationStep.rotationOn = asObject["rotationOn"].AsBool;
					}
					else
					{
						motionAnimationStep.rotationOn = true;
					}
					Vector3 position;
					if (asObject["position"]["x"] != null)
					{
						position.x = asObject["position"]["x"].AsFloat;
					}
					else
					{
						position.x = 0f;
						Debug.LogWarning("JSON file format error. Missing position x for animation step");
					}
					if (asObject["position"]["y"] != null)
					{
						position.y = asObject["position"]["y"].AsFloat;
					}
					else
					{
						position.y = 0f;
						Debug.LogWarning("JSON file format error. Missing position y for animation step");
					}
					if (asObject["position"]["z"] != null)
					{
						position.z = asObject["position"]["z"].AsFloat;
					}
					else
					{
						position.z = 0f;
						Debug.LogWarning("JSON file format error. Missing position z for animation step");
					}
					Quaternion rotation;
					if (asObject["rotation"]["x"] != null)
					{
						rotation.x = asObject["rotation"]["x"].AsFloat;
					}
					else
					{
						rotation.x = 0f;
						Debug.LogWarning("JSON file format error. Missing rotation x for animation step");
					}
					if (asObject["rotation"]["y"] != null)
					{
						rotation.y = asObject["rotation"]["y"].AsFloat;
					}
					else
					{
						rotation.y = 0f;
						Debug.LogWarning("JSON file format error. Missing rotation y for animation step");
					}
					if (asObject["rotation"]["z"] != null)
					{
						rotation.z = asObject["rotation"]["z"].AsFloat;
					}
					else
					{
						rotation.z = 0f;
						Debug.LogWarning("JSON file format error. Missing rotation z for animation step");
					}
					if (asObject["rotation"]["w"] != null)
					{
						rotation.w = asObject["rotation"]["w"].AsFloat;
					}
					else
					{
						rotation.w = 0f;
						Debug.LogWarning("JSON file format error. Missing rotation w for animation step");
					}
					motionAnimationStep.position = position;
					motionAnimationStep.rotation = rotation;
					if (asObject["timeStep"] != null)
					{
						motionAnimationStep.timeStep = asObject["timeStep"].AsFloat;
					}
					else
					{
						Debug.LogWarning("JSON file format error. Missing timeStep for animation step");
					}
					this._steps.Add(motionAnimationStep);
					this._meshDirty = true;
					this._clipLength = motionAnimationStep.timeStep;
				}
			}
		}
		else if (setMissingToDefault)
		{
			this.ClearAllSteps();
		}
	}

	// Token: 0x17000BD4 RID: 3028
	// (get) Token: 0x06005166 RID: 20838 RVA: 0x001D625D File Offset: 0x001D465D
	public float clipLength
	{
		get
		{
			return this._clipLength;
		}
	}

	// Token: 0x17000BD5 RID: 3029
	// (get) Token: 0x06005167 RID: 20839 RVA: 0x001D6265 File Offset: 0x001D4665
	public List<MotionAnimationStep> steps
	{
		get
		{
			return this._steps;
		}
	}

	// Token: 0x06005168 RID: 20840 RVA: 0x001D626D File Offset: 0x001D466D
	public void ClearAllSteps()
	{
		this._steps = new List<MotionAnimationStep>();
		this._meshDirty = true;
		this._currentStep = null;
		this._nextStep = null;
		this._currentStepIndex = 0;
		this._clipLength = 0f;
		this._loopbackStep = null;
	}

	// Token: 0x06005169 RID: 20841 RVA: 0x001D62A8 File Offset: 0x001D46A8
	public void ClearAllStepsStartingAt(float timeStep)
	{
		this.SeekToTimeStep(timeStep);
		if (this._currentStep != null && this._currentStep.timeStep >= timeStep)
		{
			this._steps.RemoveRange(this._currentStepIndex, this._steps.Count - this._currentStepIndex);
			this._meshDirty = true;
		}
		else if (this._nextStep != null)
		{
			int num = this._currentStepIndex + 1;
			this._steps.RemoveRange(num, this._steps.Count - num);
			this._meshDirty = true;
			this._nextStep = null;
		}
		if (this._currentStep != null)
		{
			this._clipLength = this._currentStep.timeStep;
		}
		else
		{
			this._clipLength = 0f;
		}
		this._loopbackStep = null;
	}

	// Token: 0x0600516A RID: 20842 RVA: 0x001D6378 File Offset: 0x001D4778
	public void ShiftAllSteps(float timeShift)
	{
		foreach (MotionAnimationStep motionAnimationStep in this._steps)
		{
			motionAnimationStep.timeStep += timeShift;
		}
		this._clipLength += timeShift;
		if (timeShift < 0f)
		{
			this.ClearAllNegativeTimeStepSteps();
			if (this._clipLength < 0f)
			{
				this._clipLength = 0f;
			}
		}
		this._meshDirty = true;
		this._currentStep = null;
		this._currentStepIndex = 0;
	}

	// Token: 0x0600516B RID: 20843 RVA: 0x001D642C File Offset: 0x001D482C
	protected void ClearAllNegativeTimeStepSteps()
	{
		List<MotionAnimationStep> list = new List<MotionAnimationStep>();
		foreach (MotionAnimationStep motionAnimationStep in this._steps)
		{
			if (motionAnimationStep.timeStep >= 0f)
			{
				list.Add(motionAnimationStep);
			}
		}
		this._steps = list;
	}

	// Token: 0x0600516C RID: 20844 RVA: 0x001D64A8 File Offset: 0x001D48A8
	public void PrepareRecord(float timeStep)
	{
		this.ClearAllStepsStartingAt(timeStep);
	}

	// Token: 0x0600516D RID: 20845 RVA: 0x001D64B4 File Offset: 0x001D48B4
	public void RecordStep(Transform t, float timeStep, bool positionOn, bool rotationOn, bool forceRecord)
	{
		bool flag = true;
		if (this._steps.Count > 0)
		{
			MotionAnimationStep motionAnimationStep = this._steps[this._steps.Count - 1];
			if (!motionAnimationStep.positionOn && !positionOn && !motionAnimationStep.rotationOn && !rotationOn)
			{
				flag = false;
			}
		}
		if (flag || forceRecord)
		{
			MotionAnimationStep motionAnimationStep2 = new MotionAnimationStep();
			motionAnimationStep2.positionOn = positionOn;
			motionAnimationStep2.rotationOn = rotationOn;
			motionAnimationStep2.position = t.localPosition;
			motionAnimationStep2.rotation = t.localRotation;
			motionAnimationStep2.timeStep = timeStep;
			this._steps.Add(motionAnimationStep2);
			this._meshDirty = true;
			this._clipLength = timeStep;
		}
	}

	// Token: 0x0600516E RID: 20846 RVA: 0x001D656D File Offset: 0x001D496D
	public void FinalizeRecord()
	{
	}

	// Token: 0x0600516F RID: 20847 RVA: 0x001D6570 File Offset: 0x001D4970
	protected void SeekToTimeStep(float timeStep)
	{
		if ((this._currentStep == null || timeStep == 0f) && this._steps.Count > 0)
		{
			this._currentStepIndex = 0;
			this._currentStep = this._steps[0];
			if (this._steps.Count > 1)
			{
				this._nextStep = this._steps[1];
			}
			else
			{
				this._nextStep = null;
			}
		}
		if (this._currentStep != null)
		{
			bool flag = false;
			while (!flag)
			{
				flag = true;
				if (timeStep < this._currentStep.timeStep)
				{
					if (this._currentStepIndex > 0)
					{
						this._nextStep = this._currentStep;
						this._currentStepIndex--;
						this._currentStep = this._steps[this._currentStepIndex];
						flag = false;
					}
				}
				else if (this._nextStep != null && timeStep >= this._nextStep.timeStep)
				{
					this._currentStep = this._nextStep;
					this._currentStepIndex++;
					if (this._currentStepIndex < this._steps.Count)
					{
						this._nextStep = this._steps[this._currentStepIndex];
						flag = false;
					}
					else
					{
						this._nextStep = null;
					}
				}
			}
		}
	}

	// Token: 0x06005170 RID: 20848 RVA: 0x001D66CC File Offset: 0x001D4ACC
	protected MotionAnimationStep CalculateInterpolatedStep(float timeStep)
	{
		if (this._currentStep != null)
		{
			this._interpolatedStep.positionOn = this._currentStep.positionOn;
			this._interpolatedStep.rotationOn = this._currentStep.rotationOn;
			if (this._nextStep != null)
			{
				float t = (timeStep - this._currentStep.timeStep) / (this._nextStep.timeStep - this._currentStep.timeStep);
				this._interpolatedStep.position = Vector3.Lerp(this._currentStep.position, this._nextStep.position, t);
				this._interpolatedStep.rotation = Quaternion.Lerp(this._currentStep.rotation, this._nextStep.rotation, t);
			}
			else
			{
				this._interpolatedStep.position = this._currentStep.position;
				this._interpolatedStep.rotation = this._currentStep.rotation;
			}
		}
		return this._interpolatedStep;
	}

	// Token: 0x06005171 RID: 20849 RVA: 0x001D67C6 File Offset: 0x001D4BC6
	public MotionAnimationStep PlaybackStep(float timeStep)
	{
		this.SeekToTimeStep(timeStep);
		return this.CalculateInterpolatedStep(timeStep);
	}

	// Token: 0x06005172 RID: 20850 RVA: 0x001D67D8 File Offset: 0x001D4BD8
	protected void FindLoopbackStep(float timeStep)
	{
		if (this._steps != null && (this._loopbackStep == null || this._loopbackStepSearchedTimeStep != timeStep))
		{
			for (int i = 0; i < this._steps.Count; i++)
			{
				MotionAnimationStep motionAnimationStep = this._steps[i];
				if (motionAnimationStep.timeStep >= timeStep || i == this._steps.Count - 1)
				{
					this._loopbackStepSearchedTimeStep = timeStep;
					this._loopbackStep = motionAnimationStep;
					break;
				}
			}
		}
	}

	// Token: 0x06005173 RID: 20851 RVA: 0x001D6864 File Offset: 0x001D4C64
	public MotionAnimationStep LoopbackStep(float percent, float toTimeStep)
	{
		if (this._steps.Count > 0)
		{
			this.FindLoopbackStep(toTimeStep);
			if (this._currentStep == null)
			{
				this._currentStep = this._loopbackStep;
			}
			this._interpolatedStep.positionOn = true;
			this._interpolatedStep.rotationOn = true;
			this._interpolatedStep.position = Vector3.Lerp(this._currentStep.position, this._loopbackStep.position, percent);
			this._interpolatedStep.rotation = Quaternion.Lerp(this._currentStep.rotation, this._loopbackStep.rotation, percent);
		}
		return this._interpolatedStep;
	}

	// Token: 0x06005174 RID: 20852 RVA: 0x001D690C File Offset: 0x001D4D0C
	protected void RegenerateMesh()
	{
		if (this._meshDirty)
		{
			this._meshDirty = false;
			if (this._steps != null && this._steps.Count > 0)
			{
				this.mesh = new Mesh();
				this.indices = new int[this._steps.Count];
				this.vertices = new Vector3[this._steps.Count];
				for (int i = 0; i < this._steps.Count; i++)
				{
					this.indices[i] = i;
					this.vertices[i] = this._steps[i].position;
				}
				this.mesh.vertices = this.vertices;
				this.mesh.SetIndices(this.indices, MeshTopology.LineStrip, 0);
			}
			else
			{
				this.mesh = new Mesh();
			}
		}
	}

	// Token: 0x06005175 RID: 20853 RVA: 0x001D69F9 File Offset: 0x001D4DF9
	public Mesh GetMesh()
	{
		this.RegenerateMesh();
		return this.mesh;
	}

	// Token: 0x04004125 RID: 16677
	protected float _clipLength;

	// Token: 0x04004126 RID: 16678
	protected List<MotionAnimationStep> _steps;

	// Token: 0x04004127 RID: 16679
	protected MotionAnimationStep _currentStep;

	// Token: 0x04004128 RID: 16680
	protected MotionAnimationStep _nextStep;

	// Token: 0x04004129 RID: 16681
	protected int _currentStepIndex;

	// Token: 0x0400412A RID: 16682
	protected MotionAnimationStep _interpolatedStep;

	// Token: 0x0400412B RID: 16683
	protected float _loopbackStepSearchedTimeStep;

	// Token: 0x0400412C RID: 16684
	protected MotionAnimationStep _loopbackStep;

	// Token: 0x0400412D RID: 16685
	protected int[] indices;

	// Token: 0x0400412E RID: 16686
	protected Vector3[] vertices;

	// Token: 0x0400412F RID: 16687
	protected Mesh mesh;

	// Token: 0x04004130 RID: 16688
	protected bool _meshDirty = true;
}
