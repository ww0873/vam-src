using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.Constrains;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
	// Token: 0x02000A13 RID: 2579
	public class BuildPointJoints : BuildChainCommand
	{
		// Token: 0x06004161 RID: 16737 RVA: 0x00136F6A File Offset: 0x0013536A
		public BuildPointJoints(HairSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x00136F7C File Offset: 0x0013537C
		protected override void OnBuild()
		{
			GPPointJoint[] array = new GPPointJoint[this.settings.StandsSettings.Provider.GetVertices().Count];
			this.CreatePointJoints(array, false);
			if (this.settings.RuntimeData.PointJoints != null)
			{
				this.settings.RuntimeData.PointJoints.Dispose();
			}
			if (array.Length > 0)
			{
				this.settings.RuntimeData.PointJoints = new GpuBuffer<GPPointJoint>(array, GPPointJoint.Size());
			}
			else
			{
				this.settings.RuntimeData.PointJoints = null;
			}
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x00137018 File Offset: 0x00135418
		protected override void OnUpdateSettings()
		{
			if (this.settings.RuntimeData.PointJoints != null)
			{
				this.CreatePointJoints(this.settings.RuntimeData.PointJoints.Data, false);
				this.settings.RuntimeData.PointJoints.PushData();
			}
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x0013706C File Offset: 0x0013546C
		public void UpdateSettingsPreserveData()
		{
			this.settings.RuntimeData.PointJoints.PullData();
			this.CreatePointJoints(this.settings.RuntimeData.PointJoints.Data, true);
			this.settings.RuntimeData.PointJoints.PushData();
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x001370C0 File Offset: 0x001354C0
		public void RebuildFromGPUData()
		{
			this.settings.RuntimeData.PointJoints.PullData();
			GPPointJoint[] data = this.settings.RuntimeData.PointJoints.Data;
			List<Vector3> list = new List<Vector3>();
			List<float> list2 = new List<float>();
			for (int i = 0; i < data.Length; i++)
			{
				list.Add(data[i].Point);
				list2.Add(data[i].Rigidity);
			}
			this.settings.StandsSettings.Provider.SetVertices(list);
			if (this.settings.PhysicsSettings.UsePaintedRigidity)
			{
				this.settings.StandsSettings.Provider.SetRigidities(list2);
			}
			else
			{
				this.settings.StandsSettings.Provider.SetRigidities(null);
			}
			this.OnUpdateSettings();
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x001371A0 File Offset: 0x001355A0
		private void CreatePointJoints(GPPointJoint[] pointJoints, bool reuse = false)
		{
			List<Vector3> vertices = this.settings.StandsSettings.Provider.GetVertices();
			List<float> rigidities = this.settings.StandsSettings.Provider.GetRigidities();
			int segments = this.settings.StandsSettings.Segments;
			int[] hairRootToScalpMap = this.settings.StandsSettings.Provider.GetHairRootToScalpMap();
			Vector3 zero = Vector3.zero;
			bool usePaintedRigidity = this.settings.PhysicsSettings.UsePaintedRigidity;
			float rootRigidity = this.settings.PhysicsSettings.RootRigidity;
			float mainRigidity = this.settings.PhysicsSettings.MainRigidity;
			float tipRigidity = this.settings.PhysicsSettings.TipRigidity;
			float rigidityRolloffPower = this.settings.PhysicsSettings.RigidityRolloffPower;
			for (int i = 0; i < vertices.Count; i++)
			{
				Vector3 vector = vertices[i];
				int num = i / segments;
				int matrixId = hairRootToScalpMap[num];
				int num2 = i % segments;
				float num3;
				if (num2 == 0)
				{
					num3 = 1.1f;
				}
				else if (usePaintedRigidity && rigidities != null)
				{
					num3 = rigidities[i];
				}
				else if (num2 == 1)
				{
					num3 = rootRigidity;
				}
				else
				{
					float num4 = ((float)num2 - 1f) / (float)(segments - 2);
					float f = 1f - num4;
					float t = Mathf.Pow(f, rigidityRolloffPower);
					num3 = Mathf.Lerp(tipRigidity, mainRigidity, t);
				}
				num3 += this.JointAreaAdd(vector);
				if (reuse)
				{
					if (num2 == 0)
					{
						pointJoints[i].Rigidity = 1.1f;
					}
					else
					{
						pointJoints[i].Rigidity = Mathf.Clamp01(num3);
					}
				}
				else if (num2 == 0)
				{
					pointJoints[i] = new GPPointJoint(i, matrixId, vector, 1.1f);
				}
				else
				{
					pointJoints[i] = new GPPointJoint(i, matrixId, vector, Mathf.Clamp01(num3));
				}
			}
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x0013739C File Offset: 0x0013579C
		private float JointAreaAdd(Vector3 vertex)
		{
			float num = 0f;
			foreach (HairJointArea hairJointArea in this.settings.PhysicsSettings.JointAreas)
			{
				float magnitude = (vertex - hairJointArea.transform.localPosition).magnitude;
				if (magnitude < hairJointArea.Radius)
				{
					float num2 = (hairJointArea.Radius - magnitude) / hairJointArea.Radius;
					num += num2 * this.settings.PhysicsSettings.JointRigidity;
				}
			}
			return num;
		}

		// Token: 0x06004168 RID: 16744 RVA: 0x00137454 File Offset: 0x00135854
		protected override void OnDispose()
		{
			if (this.settings.RuntimeData.PointJoints != null)
			{
				this.settings.RuntimeData.PointJoints.Dispose();
			}
		}

		// Token: 0x0400310D RID: 12557
		private readonly HairSettings settings;
	}
}
