using System;
using UnityEngine;

namespace mset
{
	// Token: 0x02000332 RID: 818
	public class SkyAnchor : MonoBehaviour
	{
		// Token: 0x060013A4 RID: 5028 RVA: 0x00070630 File Offset: 0x0006EA30
		public SkyAnchor()
		{
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060013A5 RID: 5029 RVA: 0x0007066B File Offset: 0x0006EA6B
		public Sky CurrentSky
		{
			get
			{
				return this.Blender.CurrentSky;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060013A6 RID: 5030 RVA: 0x00070678 File Offset: 0x0006EA78
		public Sky PreviousSky
		{
			get
			{
				return this.Blender.PreviousSky;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x00070685 File Offset: 0x0006EA85
		// (set) Token: 0x060013A8 RID: 5032 RVA: 0x00070692 File Offset: 0x0006EA92
		public float BlendTime
		{
			get
			{
				return this.Blender.BlendTime;
			}
			set
			{
				this.Blender.BlendTime = value;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060013A9 RID: 5033 RVA: 0x000706A0 File Offset: 0x0006EAA0
		public bool IsStatic
		{
			get
			{
				return this.isStatic;
			}
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x000706A8 File Offset: 0x0006EAA8
		private void Start()
		{
			if (this.BindType != SkyAnchor.AnchorBindType.TargetSky)
			{
				base.GetComponent<Renderer>().SetPropertyBlock(new MaterialPropertyBlock());
				SkyManager skyManager = SkyManager.Get();
				skyManager.RegisterNewRenderer(base.GetComponent<Renderer>());
				skyManager.ApplyCorrectSky(base.GetComponent<Renderer>());
				this.BlendTime = skyManager.LocalBlendTime;
				if (this.Blender.CurrentSky)
				{
					this.Blender.SnapToSky(this.Blender.CurrentSky);
				}
				else
				{
					this.Blender.SnapToSky(skyManager.GlobalSky);
				}
			}
			this.materials = base.GetComponent<Renderer>().materials;
			this.LastPosition = base.transform.position;
			this.HasChanged = true;
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00070765 File Offset: 0x0006EB65
		private void OnEnable()
		{
			this.isStatic = base.gameObject.isStatic;
			this.ComputeCenter(ref this.CachedCenter);
			this.firstFrame = true;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0007078C File Offset: 0x0006EB8C
		private void LateUpdate()
		{
			if (this.BindType == SkyAnchor.AnchorBindType.TargetSky)
			{
				this.HasChanged = (this.AnchorSky != this.Blender.CurrentSky);
				if (this.AnchorSky != null)
				{
					this.CachedCenter = this.AnchorSky.transform.position;
				}
			}
			else if (this.BindType == SkyAnchor.AnchorBindType.TargetTransform)
			{
				if (this.AnchorTransform && (this.AnchorTransform.position.x != this.LastPosition.x || this.AnchorTransform.position.y != this.LastPosition.y || this.AnchorTransform.position.z != this.LastPosition.z))
				{
					this.HasChanged = true;
					this.LastPosition = this.AnchorTransform.position;
					this.CachedCenter.x = this.LastPosition.x;
					this.CachedCenter.y = this.LastPosition.y;
					this.CachedCenter.z = this.LastPosition.z;
				}
			}
			else if (!this.isStatic)
			{
				if (this.LastPosition.x != base.transform.position.x || this.LastPosition.y != base.transform.position.y || this.LastPosition.z != base.transform.position.z)
				{
					this.HasChanged = true;
					this.LastPosition = base.transform.position;
					this.ComputeCenter(ref this.CachedCenter);
				}
			}
			else
			{
				this.HasChanged = false;
			}
			this.HasChanged |= this.firstFrame;
			this.firstFrame = false;
			bool flag = this.Blender.IsBlending || this.Blender.WasBlending(Time.deltaTime);
			if (flag)
			{
				this.Apply();
			}
			else if (this.BindType == SkyAnchor.AnchorBindType.TargetSky)
			{
				if (this.HasChanged || this.Blender.CurrentSky.Dirty)
				{
					this.Apply();
				}
			}
			else if (this.HasLocalSky && (this.HasChanged || this.Blender.CurrentSky.Dirty))
			{
				this.Apply();
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00070A30 File Offset: 0x0006EE30
		public void UpdateMaterials()
		{
			this.materials = base.GetComponent<Renderer>().materials;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00070A44 File Offset: 0x0006EE44
		public void CleanUpMaterials()
		{
			if (this.materials != null)
			{
				foreach (Material obj in this.materials)
				{
					UnityEngine.Object.Destroy(obj);
				}
				this.materials = new Material[0];
			}
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00070A8D File Offset: 0x0006EE8D
		public void SnapToSky(Sky nusky)
		{
			if (nusky == null)
			{
				return;
			}
			if (this.BindType == SkyAnchor.AnchorBindType.TargetSky)
			{
				return;
			}
			this.Blender.SnapToSky(nusky);
			this.HasLocalSky = true;
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00070ABC File Offset: 0x0006EEBC
		public void BlendToSky(Sky nusky)
		{
			if (nusky == null)
			{
				return;
			}
			if (this.BindType == SkyAnchor.AnchorBindType.TargetSky)
			{
				return;
			}
			this.Blender.BlendToSky(nusky);
			this.HasLocalSky = true;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00070AEB File Offset: 0x0006EEEB
		public void SnapToGlobalSky(Sky nusky)
		{
			this.SnapToSky(nusky);
			this.HasLocalSky = false;
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00070AFB File Offset: 0x0006EEFB
		public void BlendToGlobalSky(Sky nusky)
		{
			if (this.HasLocalSky)
			{
				this.BlendToSky(nusky);
			}
			this.HasLocalSky = false;
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00070B18 File Offset: 0x0006EF18
		public void Apply()
		{
			if (this.BindType == SkyAnchor.AnchorBindType.TargetSky)
			{
				if (this.AnchorSky)
				{
					this.Blender.SnapToSky(this.AnchorSky);
				}
				else
				{
					this.Blender.SnapToSky(SkyManager.Get().GlobalSky);
				}
			}
			this.Blender.Apply(base.GetComponent<Renderer>(), this.materials);
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00070B83 File Offset: 0x0006EF83
		public void GetCenter(ref Vector3 _center)
		{
			_center.x = this.CachedCenter.x;
			_center.y = this.CachedCenter.y;
			_center.z = this.CachedCenter.z;
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00070BB8 File Offset: 0x0006EFB8
		private void ComputeCenter(ref Vector3 _center)
		{
			_center.x = base.transform.position.x;
			_center.y = base.transform.position.y;
			_center.z = base.transform.position.z;
			switch (this.BindType)
			{
			case SkyAnchor.AnchorBindType.Center:
				_center.x = base.GetComponent<Renderer>().bounds.center.x;
				_center.y = base.GetComponent<Renderer>().bounds.center.y;
				_center.z = base.GetComponent<Renderer>().bounds.center.z;
				break;
			case SkyAnchor.AnchorBindType.Offset:
			{
				Vector3 vector = base.transform.localToWorldMatrix.MultiplyPoint3x4(this.AnchorOffset);
				_center.x = vector.x;
				_center.y = vector.y;
				_center.z = vector.z;
				break;
			}
			case SkyAnchor.AnchorBindType.TargetTransform:
				if (this.AnchorTransform)
				{
					_center.x = this.AnchorTransform.position.x;
					_center.y = this.AnchorTransform.position.y;
					_center.z = this.AnchorTransform.position.z;
				}
				break;
			case SkyAnchor.AnchorBindType.TargetSky:
				if (this.AnchorSky)
				{
					_center.x = this.AnchorSky.transform.position.x;
					_center.y = this.AnchorSky.transform.position.y;
					_center.z = this.AnchorSky.transform.position.z;
				}
				break;
			}
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00070DBD File Offset: 0x0006F1BD
		private void OnDestroy()
		{
			this.CleanUpMaterials();
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00070DC5 File Offset: 0x0006F1C5
		private void OnApplicationQuit()
		{
			this.CleanUpMaterials();
		}

		// Token: 0x04001103 RID: 4355
		public SkyAnchor.AnchorBindType BindType;

		// Token: 0x04001104 RID: 4356
		public Transform AnchorTransform;

		// Token: 0x04001105 RID: 4357
		public Vector3 AnchorOffset = Vector3.zero;

		// Token: 0x04001106 RID: 4358
		public Sky AnchorSky;

		// Token: 0x04001107 RID: 4359
		public Vector3 CachedCenter = Vector3.zero;

		// Token: 0x04001108 RID: 4360
		public SkyApplicator CurrentApplicator;

		// Token: 0x04001109 RID: 4361
		private bool isStatic;

		// Token: 0x0400110A RID: 4362
		public bool HasLocalSky;

		// Token: 0x0400110B RID: 4363
		public bool HasChanged = true;

		// Token: 0x0400110C RID: 4364
		[SerializeField]
		private SkyBlender Blender = new SkyBlender();

		// Token: 0x0400110D RID: 4365
		private Vector3 LastPosition = Vector3.zero;

		// Token: 0x0400110E RID: 4366
		[NonSerialized]
		public Material[] materials;

		// Token: 0x0400110F RID: 4367
		private bool firstFrame;

		// Token: 0x02000333 RID: 819
		public enum AnchorBindType
		{
			// Token: 0x04001111 RID: 4369
			Center,
			// Token: 0x04001112 RID: 4370
			Offset,
			// Token: 0x04001113 RID: 4371
			TargetTransform,
			// Token: 0x04001114 RID: 4372
			TargetSky
		}
	}
}
