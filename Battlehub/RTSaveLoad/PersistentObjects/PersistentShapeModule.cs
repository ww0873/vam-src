using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000120 RID: 288
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentShapeModule : PersistentData
	{
		// Token: 0x060006E2 RID: 1762 RVA: 0x0002F962 File Offset: 0x0002DD62
		public PersistentShapeModule()
		{
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0002F96C File Offset: 0x0002DD6C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.ShapeModule shapeModule = (ParticleSystem.ShapeModule)obj;
			shapeModule.enabled = this.enabled;
			shapeModule.shapeType = (ParticleSystemShapeType)this.shapeType;
			shapeModule.randomDirectionAmount = this.randomDirectionAmount;
			shapeModule.sphericalDirectionAmount = this.sphericalDirectionAmount;
			shapeModule.alignToDirection = this.alignToDirection;
			shapeModule.radius = this.radius;
			shapeModule.radiusMode = (ParticleSystemShapeMultiModeValue)this.radiusMode;
			shapeModule.radiusSpread = this.radiusSpread;
			shapeModule.radiusSpeed = base.Write<ParticleSystem.MinMaxCurve>(shapeModule.radiusSpeed, this.radiusSpeed, objects);
			shapeModule.radiusSpeedMultiplier = this.radiusSpeedMultiplier;
			shapeModule.angle = this.angle;
			shapeModule.length = this.length;
			shapeModule.scale = this.box;
			shapeModule.meshShapeType = (ParticleSystemMeshShapeType)this.meshShapeType;
			shapeModule.mesh = (Mesh)objects.Get(this.mesh);
			shapeModule.meshRenderer = (MeshRenderer)objects.Get(this.meshRenderer);
			shapeModule.skinnedMeshRenderer = (SkinnedMeshRenderer)objects.Get(this.skinnedMeshRenderer);
			shapeModule.useMeshMaterialIndex = this.useMeshMaterialIndex;
			shapeModule.meshMaterialIndex = this.meshMaterialIndex;
			shapeModule.useMeshColors = this.useMeshColors;
			shapeModule.normalOffset = this.normalOffset;
			shapeModule.scale = this.scale;
			shapeModule.arc = this.arc;
			shapeModule.arcMode = (ParticleSystemShapeMultiModeValue)this.arcMode;
			shapeModule.arcSpread = this.arcSpread;
			shapeModule.arcSpeed = base.Write<ParticleSystem.MinMaxCurve>(shapeModule.arcSpeed, this.arcSpeed, objects);
			shapeModule.arcSpeedMultiplier = this.arcSpeedMultiplier;
			return shapeModule;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0002FB34 File Offset: 0x0002DF34
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.ShapeModule shapeModule = (ParticleSystem.ShapeModule)obj;
			this.enabled = shapeModule.enabled;
			this.shapeType = (uint)shapeModule.shapeType;
			this.randomDirectionAmount = shapeModule.randomDirectionAmount;
			this.sphericalDirectionAmount = shapeModule.sphericalDirectionAmount;
			this.alignToDirection = shapeModule.alignToDirection;
			this.radius = shapeModule.radius;
			this.radiusMode = (uint)shapeModule.radiusMode;
			this.radiusSpread = shapeModule.radiusSpread;
			this.radiusSpeed = base.Read<PersistentMinMaxCurve>(this.radiusSpeed, shapeModule.radiusSpeed);
			this.radiusSpeedMultiplier = shapeModule.radiusSpeedMultiplier;
			this.angle = shapeModule.angle;
			this.length = shapeModule.length;
			this.box = shapeModule.scale;
			this.meshShapeType = (uint)shapeModule.meshShapeType;
			this.mesh = shapeModule.mesh.GetMappedInstanceID();
			this.meshRenderer = shapeModule.meshRenderer.GetMappedInstanceID();
			this.skinnedMeshRenderer = shapeModule.skinnedMeshRenderer.GetMappedInstanceID();
			this.useMeshMaterialIndex = shapeModule.useMeshMaterialIndex;
			this.meshMaterialIndex = shapeModule.meshMaterialIndex;
			this.useMeshColors = shapeModule.useMeshColors;
			this.normalOffset = shapeModule.normalOffset;
			this.scale = shapeModule.scale;
			this.arc = shapeModule.arc;
			this.arcMode = (uint)shapeModule.arcMode;
			this.arcSpread = shapeModule.arcSpread;
			this.arcSpeed = base.Read<PersistentMinMaxCurve>(this.arcSpeed, shapeModule.arcSpeed);
			this.arcSpeedMultiplier = shapeModule.arcSpeedMultiplier;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0002FCE8 File Offset: 0x0002E0E8
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.radiusSpeed, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.mesh, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.meshRenderer, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.skinnedMeshRenderer, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.arcSpeed, dependencies, objects, allowNulls);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0002FD4C File Offset: 0x0002E14C
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.ShapeModule shapeModule = (ParticleSystem.ShapeModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.radiusSpeed, shapeModule.radiusSpeed, dependencies);
			base.AddDependency(shapeModule.mesh, dependencies);
			base.AddDependency(shapeModule.meshRenderer, dependencies);
			base.AddDependency(shapeModule.skinnedMeshRenderer, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.arcSpeed, shapeModule.arcSpeed, dependencies);
		}

		// Token: 0x0400070A RID: 1802
		public bool enabled;

		// Token: 0x0400070B RID: 1803
		public uint shapeType;

		// Token: 0x0400070C RID: 1804
		public float randomDirectionAmount;

		// Token: 0x0400070D RID: 1805
		public float sphericalDirectionAmount;

		// Token: 0x0400070E RID: 1806
		public bool alignToDirection;

		// Token: 0x0400070F RID: 1807
		public float radius;

		// Token: 0x04000710 RID: 1808
		public uint radiusMode;

		// Token: 0x04000711 RID: 1809
		public float radiusSpread;

		// Token: 0x04000712 RID: 1810
		public PersistentMinMaxCurve radiusSpeed;

		// Token: 0x04000713 RID: 1811
		public float radiusSpeedMultiplier;

		// Token: 0x04000714 RID: 1812
		public float angle;

		// Token: 0x04000715 RID: 1813
		public float length;

		// Token: 0x04000716 RID: 1814
		public Vector3 box;

		// Token: 0x04000717 RID: 1815
		public uint meshShapeType;

		// Token: 0x04000718 RID: 1816
		public long mesh;

		// Token: 0x04000719 RID: 1817
		public long meshRenderer;

		// Token: 0x0400071A RID: 1818
		public long skinnedMeshRenderer;

		// Token: 0x0400071B RID: 1819
		public bool useMeshMaterialIndex;

		// Token: 0x0400071C RID: 1820
		public int meshMaterialIndex;

		// Token: 0x0400071D RID: 1821
		public bool useMeshColors;

		// Token: 0x0400071E RID: 1822
		public float normalOffset;

		// Token: 0x0400071F RID: 1823
		public float meshScale;

		// Token: 0x04000720 RID: 1824
		public float arc;

		// Token: 0x04000721 RID: 1825
		public uint arcMode;

		// Token: 0x04000722 RID: 1826
		public float arcSpread;

		// Token: 0x04000723 RID: 1827
		public PersistentMinMaxCurve arcSpeed;

		// Token: 0x04000724 RID: 1828
		public float arcSpeedMultiplier;

		// Token: 0x04000725 RID: 1829
		public Vector3 scale;
	}
}
