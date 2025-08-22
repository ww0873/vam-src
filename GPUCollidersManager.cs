using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Behaviours;
using GPUTools.Physics.Scripts.Kernels;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

// Token: 0x020009AE RID: 2478
public class GPUCollidersManager : MonoBehaviour
{
	// Token: 0x06003E71 RID: 15985 RVA: 0x0012CA92 File Offset: 0x0012AE92
	public GPUCollidersManager()
	{
	}

	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x06003E72 RID: 15986 RVA: 0x0012CABB File Offset: 0x0012AEBB
	public static GpuBuffer<GPLineSphereWithDelta> processedLineSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.processedLineSpheres;
		}
	}

	// Token: 0x17000743 RID: 1859
	// (get) Token: 0x06003E73 RID: 15987 RVA: 0x0012CAD9 File Offset: 0x0012AED9
	public static GpuBuffer<GPSphereWithDelta> processedSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.processedSpheres;
		}
	}

	// Token: 0x17000744 RID: 1860
	// (get) Token: 0x06003E74 RID: 15988 RVA: 0x0012CAF7 File Offset: 0x0012AEF7
	public static GpuBuffer<Vector4> planesBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.planes;
		}
	}

	// Token: 0x06003E75 RID: 15989 RVA: 0x0012CB15 File Offset: 0x0012AF15
	public static void RegisterLineSphereCollider(CapsuleLineSphereCollider lsc)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegLineSphereCollider(lsc);
		}
	}

	// Token: 0x06003E76 RID: 15990 RVA: 0x0012CB32 File Offset: 0x0012AF32
	public static void DeregisterLineSphereCollider(CapsuleLineSphereCollider lsc)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregLineSphereCollider(lsc);
		}
	}

	// Token: 0x06003E77 RID: 15991 RVA: 0x0012CB4F File Offset: 0x0012AF4F
	public static void RegisterSphereCollider(GpuSphereCollider sc)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegSphereCollider(sc);
		}
	}

	// Token: 0x06003E78 RID: 15992 RVA: 0x0012CB6C File Offset: 0x0012AF6C
	public static void DeregisterSphereCollider(GpuSphereCollider sc)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregSphereCollider(sc);
		}
	}

	// Token: 0x06003E79 RID: 15993 RVA: 0x0012CB89 File Offset: 0x0012AF89
	public static void RegisterPlaneCollider(PlaneCollider pc)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegPlaneCollider(pc);
		}
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x0012CBA6 File Offset: 0x0012AFA6
	public static void DeregisterPlaneCollider(PlaneCollider pc)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregPlaneCollider(pc);
		}
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x0012CBC3 File Offset: 0x0012AFC3
	public static void RegisterConsumer(GPUCollidersConsumer consumer)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegConsumer(consumer);
		}
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x0012CBE0 File Offset: 0x0012AFE0
	public static void DeregisterConsumer(GPUCollidersConsumer consumer)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregConsumer(consumer);
		}
	}

	// Token: 0x17000745 RID: 1861
	// (get) Token: 0x06003E7D RID: 15997 RVA: 0x0012CBFD File Offset: 0x0012AFFD
	public static GpuBuffer<GPGrabSphere> grabSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.grabSpheresBuff;
		}
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x0012CC1B File Offset: 0x0012B01B
	public static void RegisterGrabSphere(GpuGrabSphere gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegGrabSphere(gs);
		}
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x0012CC38 File Offset: 0x0012B038
	public static void DeregisterGrabSphere(GpuGrabSphere gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregGrabSphere(gs);
		}
	}

	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x06003E80 RID: 16000 RVA: 0x0012CC55 File Offset: 0x0012B055
	public static GpuBuffer<GPLineSphere> cutLineSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.cutCapsulesBuff;
		}
	}

	// Token: 0x06003E81 RID: 16001 RVA: 0x0012CC73 File Offset: 0x0012B073
	public static void RegisterCutCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegCutCapsule(gs);
		}
	}

	// Token: 0x06003E82 RID: 16002 RVA: 0x0012CC90 File Offset: 0x0012B090
	public static void DeregisterCutCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregCutCapsule(gs);
		}
	}

	// Token: 0x17000747 RID: 1863
	// (get) Token: 0x06003E83 RID: 16003 RVA: 0x0012CCAD File Offset: 0x0012B0AD
	public static GpuBuffer<GPLineSphere> growLineSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.growCapsulesBuff;
		}
	}

	// Token: 0x06003E84 RID: 16004 RVA: 0x0012CCCB File Offset: 0x0012B0CB
	public static void RegisterGrowCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegGrowCapsule(gs);
		}
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x0012CCE8 File Offset: 0x0012B0E8
	public static void DeregisterGrowCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregGrowCapsule(gs);
		}
	}

	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x06003E86 RID: 16006 RVA: 0x0012CD05 File Offset: 0x0012B105
	public static GpuBuffer<GPLineSphere> holdLineSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.holdCapsulesBuff;
		}
	}

	// Token: 0x06003E87 RID: 16007 RVA: 0x0012CD23 File Offset: 0x0012B123
	public static void RegisterHoldCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegHoldCapsule(gs);
		}
	}

	// Token: 0x06003E88 RID: 16008 RVA: 0x0012CD40 File Offset: 0x0012B140
	public static void DeregisterHoldCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregHoldCapsule(gs);
		}
	}

	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x06003E89 RID: 16009 RVA: 0x0012CD5D File Offset: 0x0012B15D
	public static GpuBuffer<GPLineSphereWithMatrixDelta> grabLineSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.grabCapsulesBuff;
		}
	}

	// Token: 0x06003E8A RID: 16010 RVA: 0x0012CD7B File Offset: 0x0012B17B
	public static void RegisterGrabCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegGrabCapsule(gs);
		}
	}

	// Token: 0x06003E8B RID: 16011 RVA: 0x0012CD98 File Offset: 0x0012B198
	public static void DeregisterGrabCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregGrabCapsule(gs);
		}
	}

	// Token: 0x1700074A RID: 1866
	// (get) Token: 0x06003E8C RID: 16012 RVA: 0x0012CDB5 File Offset: 0x0012B1B5
	public static GpuBuffer<GPLineSphere> pushLineSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.pushCapsulesBuff;
		}
	}

	// Token: 0x06003E8D RID: 16013 RVA: 0x0012CDD3 File Offset: 0x0012B1D3
	public static void RegisterPushCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegPushCapsule(gs);
		}
	}

	// Token: 0x06003E8E RID: 16014 RVA: 0x0012CDF0 File Offset: 0x0012B1F0
	public static void DeregisterPushCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregPushCapsule(gs);
		}
	}

	// Token: 0x1700074B RID: 1867
	// (get) Token: 0x06003E8F RID: 16015 RVA: 0x0012CE0D File Offset: 0x0012B20D
	public static GpuBuffer<GPLineSphere> pullLineSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.pullCapsulesBuff;
		}
	}

	// Token: 0x06003E90 RID: 16016 RVA: 0x0012CE2B File Offset: 0x0012B22B
	public static void RegisterPullCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegPullCapsule(gs);
		}
	}

	// Token: 0x06003E91 RID: 16017 RVA: 0x0012CE48 File Offset: 0x0012B248
	public static void DeregisterPullCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregPullCapsule(gs);
		}
	}

	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x06003E92 RID: 16018 RVA: 0x0012CE65 File Offset: 0x0012B265
	public static GpuBuffer<GPLineSphereWithDelta> brushLineSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.brushCapsulesBuff;
		}
	}

	// Token: 0x06003E93 RID: 16019 RVA: 0x0012CE83 File Offset: 0x0012B283
	public static void RegisterBrushCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegBrushCapsule(gs);
		}
	}

	// Token: 0x06003E94 RID: 16020 RVA: 0x0012CEA0 File Offset: 0x0012B2A0
	public static void DeregisterBrushCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregBrushCapsule(gs);
		}
	}

	// Token: 0x1700074D RID: 1869
	// (get) Token: 0x06003E95 RID: 16021 RVA: 0x0012CEBD File Offset: 0x0012B2BD
	public static GpuBuffer<GPLineSphere> rigidityIncreaseLineSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.rigidityIncreaseCapsulesBuff;
		}
	}

	// Token: 0x06003E96 RID: 16022 RVA: 0x0012CEDB File Offset: 0x0012B2DB
	public static void RegisterRigidityIncreaseCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegRigidityIncreaseCapsule(gs);
		}
	}

	// Token: 0x06003E97 RID: 16023 RVA: 0x0012CEF8 File Offset: 0x0012B2F8
	public static void DeregisterRigidityIncreaseCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregRigidityIncreaseCapsule(gs);
		}
	}

	// Token: 0x1700074E RID: 1870
	// (get) Token: 0x06003E98 RID: 16024 RVA: 0x0012CF15 File Offset: 0x0012B315
	public static GpuBuffer<GPLineSphere> rigidityDecreaseLineSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.rigidityDecreaseCapsulesBuff;
		}
	}

	// Token: 0x06003E99 RID: 16025 RVA: 0x0012CF33 File Offset: 0x0012B333
	public static void RegisterRigidityDecreaseCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegRigidityDecreaseCapsule(gs);
		}
	}

	// Token: 0x06003E9A RID: 16026 RVA: 0x0012CF50 File Offset: 0x0012B350
	public static void DeregisterRigidityDecreaseCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregRigidityDecreaseCapsule(gs);
		}
	}

	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x06003E9B RID: 16027 RVA: 0x0012CF6D File Offset: 0x0012B36D
	public static GpuBuffer<GPLineSphere> rigiditySetLineSpheresBuffer
	{
		get
		{
			if (GPUCollidersManager.singleton == null)
			{
				return null;
			}
			return GPUCollidersManager.singleton.rigiditySetCapsulesBuff;
		}
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x0012CF8B File Offset: 0x0012B38B
	public static void RegisterRigiditySetCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.RegRigiditySetCapsule(gs);
		}
	}

	// Token: 0x06003E9D RID: 16029 RVA: 0x0012CFA8 File Offset: 0x0012B3A8
	public static void DeregisterRigiditySetCapsule(GpuEditCapsule gs)
	{
		if (GPUCollidersManager.singleton != null)
		{
			GPUCollidersManager.singleton.DeregRigiditySetCapsule(gs);
		}
	}

	// Token: 0x06003E9E RID: 16030 RVA: 0x0012CFC8 File Offset: 0x0012B3C8
	protected void ComputeColliders()
	{
		if (this.consumers.Count > 0)
		{
			if (this.spheres == null || this.spheres.Count != this.sphereColliders.Count)
			{
				if (this.spheres != null)
				{
					this.spheres.Dispose();
				}
				if (this.processedSpheres != null)
				{
					this.processedSpheres.Dispose();
				}
				if (this.sphereColliders.Count > 0)
				{
					this.spheres = new GpuBuffer<GPSphere>(this.sphereColliders.Count, GPSphere.Size());
					this.processedSpheres = new GpuBuffer<GPSphereWithDelta>(this.sphereColliders.Count, GPSphereWithDelta.Size());
				}
				else
				{
					this.spheres = null;
					this.processedSpheres = null;
				}
				this.sphereCopyKernel.Spheres = this.spheres;
				this.sphereCopyKernel.ClearCacheAttributes();
				this.sphereProcessKernel.Spheres = this.spheres;
				this.sphereProcessKernel.ProcessedSpheres = this.processedSpheres;
				this.sphereProcessKernel.ClearCacheAttributes();
			}
			bool flag = false;
			if (this.oldSpheres == null || this.oldSpheres.Count != this.sphereColliders.Count)
			{
				flag = true;
				if (this.oldSpheres != null)
				{
					this.oldSpheres.Dispose();
				}
				if (this.sphereColliders.Count > 0)
				{
					this.oldSpheres = new GpuBuffer<GPSphere>(this.sphereColliders.Count, GPSphere.Size());
				}
				else
				{
					this.oldSpheres = null;
				}
				this.sphereCopyKernel.OldSpheres = this.oldSpheres;
				this.sphereCopyKernel.ClearCacheAttributes();
				this.sphereProcessKernel.OldSpheres = this.oldSpheres;
				this.sphereProcessKernel.ClearCacheAttributes();
			}
			if (this.spheres != null && this.oldSpheres != null)
			{
				GPSphere[] data = this.oldSpheres.Data;
				GPSphere[] data2 = this.spheres.Data;
				if (!flag)
				{
					this.sphereCopyKernel.Dispatch();
				}
				for (int i = 0; i < this.sphereColliders.Count; i++)
				{
					GpuSphereCollider gpuSphereCollider = this.sphereColliders[i];
					data2[i].Position = gpuSphereCollider.worldCenter;
					data2[i].Radius = gpuSphereCollider.worldRadius;
					data2[i].Friction = gpuSphereCollider.friction;
				}
				this.spheres.PushData();
				if (flag)
				{
					this.sphereCopyKernel.Dispatch();
				}
				this.sphereProcessKernel.Dispatch();
			}
			if (this.lineSpheres == null || this.lineSpheres.Count != this.lineSphereColliders.Count)
			{
				if (this.lineSpheres != null)
				{
					this.lineSpheres.Dispose();
				}
				if (this.processedLineSpheres != null)
				{
					this.processedLineSpheres.Dispose();
				}
				if (this.lineSphereColliders.Count > 0)
				{
					this.lineSpheres = new GpuBuffer<GPLineSphere>(this.lineSphereColliders.Count, GPLineSphere.Size());
					this.processedLineSpheres = new GpuBuffer<GPLineSphereWithDelta>(this.lineSphereColliders.Count, GPLineSphereWithDelta.Size());
				}
				else
				{
					this.lineSpheres = null;
					this.processedLineSpheres = null;
				}
				this.lineSphereCopyKernel.LineSpheres = this.lineSpheres;
				this.lineSphereCopyKernel.ClearCacheAttributes();
				this.lineSphereProcessKernel.LineSpheres = this.lineSpheres;
				this.lineSphereProcessKernel.ProcessedLineSpheres = GPUCollidersManager.processedLineSpheresBuffer;
				this.lineSphereProcessKernel.ClearCacheAttributes();
			}
			bool flag2 = false;
			if (this.oldLineSpheres == null || this.oldLineSpheres.Count != this.lineSphereColliders.Count)
			{
				flag2 = true;
				if (this.oldLineSpheres != null)
				{
					this.oldLineSpheres.Dispose();
				}
				if (this.lineSphereColliders.Count > 0)
				{
					this.oldLineSpheres = new GpuBuffer<GPLineSphere>(this.lineSphereColliders.Count, GPLineSphere.Size());
				}
				else
				{
					this.oldLineSpheres = null;
				}
				this.lineSphereCopyKernel.OldLineSpheres = this.oldLineSpheres;
				this.lineSphereCopyKernel.ClearCacheAttributes();
				this.lineSphereProcessKernel.OldLineSpheres = this.oldLineSpheres;
				this.lineSphereProcessKernel.ClearCacheAttributes();
			}
			if (this.lineSpheres != null && this.oldLineSpheres != null)
			{
				GPLineSphere[] data3 = this.oldLineSpheres.Data;
				GPLineSphere[] data4 = this.lineSpheres.Data;
				if (!flag2)
				{
					this.lineSphereCopyKernel.Dispatch();
				}
				for (int j = 0; j < this.lineSphereColliders.Count; j++)
				{
					CapsuleLineSphereCollider capsuleLineSphereCollider = this.lineSphereColliders[j];
					data4[j].PositionA = capsuleLineSphereCollider.WorldA;
					data4[j].PositionB = capsuleLineSphereCollider.WorldB;
					data4[j].RadiusA = capsuleLineSphereCollider.WorldRadiusA;
					data4[j].RadiusB = capsuleLineSphereCollider.WorldRadiusB;
					data4[j].Friction = capsuleLineSphereCollider.friction;
				}
				this.lineSpheres.PushData();
				if (flag2)
				{
					this.lineSphereCopyKernel.Dispatch();
				}
				this.lineSphereProcessKernel.Dispatch();
			}
			if (this.planes == null || this.planes.Count != this.planeColliders.Count)
			{
				if (this.planes != null)
				{
					this.planes.Dispose();
				}
				if (this.planeColliders.Count > 0)
				{
					this.planes = new GpuBuffer<Vector4>(this.planeColliders.Count, 16);
				}
				else
				{
					this.planes = null;
				}
			}
			if (this.planes != null)
			{
				Vector4[] data5 = this.planes.Data;
				for (int k = 0; k < this.planeColliders.Count; k++)
				{
					PlaneCollider planeCollider = this.planeColliders[k];
					data5[k] = planeCollider.GetWorldData();
				}
				this.planes.PushData();
			}
		}
	}

	// Token: 0x06003E9F RID: 16031 RVA: 0x0012D5D4 File Offset: 0x0012B9D4
	public void RegSphereCollider(GpuSphereCollider sphereCollider)
	{
		this.sphereColliders.Add(sphereCollider);
	}

	// Token: 0x06003EA0 RID: 16032 RVA: 0x0012D5E2 File Offset: 0x0012B9E2
	public void DeregSphereCollider(GpuSphereCollider sphereCollider)
	{
		this.sphereColliders.Remove(sphereCollider);
	}

	// Token: 0x06003EA1 RID: 16033 RVA: 0x0012D5F1 File Offset: 0x0012B9F1
	public void RegLineSphereCollider(CapsuleLineSphereCollider lineSphereCollider)
	{
		this.lineSphereColliders.Add(lineSphereCollider);
	}

	// Token: 0x06003EA2 RID: 16034 RVA: 0x0012D5FF File Offset: 0x0012B9FF
	public void DeregLineSphereCollider(CapsuleLineSphereCollider lineSphereCollider)
	{
		this.lineSphereColliders.Remove(lineSphereCollider);
	}

	// Token: 0x06003EA3 RID: 16035 RVA: 0x0012D60E File Offset: 0x0012BA0E
	public void RegPlaneCollider(PlaneCollider planeCollider)
	{
		this.planeColliders.Add(planeCollider);
	}

	// Token: 0x06003EA4 RID: 16036 RVA: 0x0012D61C File Offset: 0x0012BA1C
	public void DeregPlaneCollider(PlaneCollider planeCollider)
	{
		this.planeColliders.Remove(planeCollider);
	}

	// Token: 0x06003EA5 RID: 16037 RVA: 0x0012D62B File Offset: 0x0012BA2B
	public void RegConsumer(GPUCollidersConsumer consumer)
	{
		this.consumers.Add(consumer);
	}

	// Token: 0x06003EA6 RID: 16038 RVA: 0x0012D639 File Offset: 0x0012BA39
	public void DeregConsumer(GPUCollidersConsumer consumer)
	{
		this.consumers.Remove(consumer);
	}

	// Token: 0x06003EA7 RID: 16039 RVA: 0x0012D648 File Offset: 0x0012BA48
	protected void ComputeGrabSpheres()
	{
		if (this.grabSpheresBuff == null || this.grabSpheresBuff.Count != this.grabSpheres.Count)
		{
			if (this.grabSpheres.Count > 0)
			{
				if (this.grabSpheresBuff != null)
				{
					this.grabSpheresBuff.Dispose();
				}
				this.grabSpheresBuff = new GpuBuffer<GPGrabSphere>(this.grabSpheres.Count, GPGrabSphere.Size());
				this.usingNullGrabSpheresBuff = false;
			}
			else if (!this.usingNullGrabSpheresBuff)
			{
				if (this.grabSpheresBuff != null)
				{
					this.grabSpheresBuff.Dispose();
				}
				this.grabSpheresBuff = new GpuBuffer<GPGrabSphere>(1, GPGrabSphere.Size());
				this.usingNullGrabSpheresBuff = true;
				GPGrabSphere[] data = this.grabSpheresBuff.Data;
				data[0].ID = -1;
				data[0].Position = Vector3.zero;
				data[0].Radius = 0f;
				data[0].GrabbedThisFrame = 0;
				this.grabSpheresBuff.PushData();
			}
		}
		if (this.grabSpheres.Count > 0)
		{
			GPGrabSphere[] data2 = this.grabSpheresBuff.Data;
			this.usingNullGrabSpheresBuff = false;
			for (int i = 0; i < this.grabSpheres.Count; i++)
			{
				GpuGrabSphere gpuGrabSphere = this.grabSpheres[i];
				data2[i].ID = gpuGrabSphere.id;
				data2[i].Position = gpuGrabSphere.transform.position;
				data2[i].Radius = gpuGrabSphere.WorldRadius;
				data2[i].GrabbedThisFrame = gpuGrabSphere.enabledThisFrame;
				if (gpuGrabSphere.frameCountdown == 0)
				{
					gpuGrabSphere.enabledThisFrame = 0;
				}
				else
				{
					gpuGrabSphere.frameCountdown--;
				}
			}
			this.grabSpheresBuff.PushData();
		}
	}

	// Token: 0x06003EA8 RID: 16040 RVA: 0x0012D820 File Offset: 0x0012BC20
	public void RegGrabSphere(GpuGrabSphere gs)
	{
		while (this.grabSphereIDs.ContainsKey(this.uidcount))
		{
			this.uidcount++;
			if (this.uidcount > 10000000)
			{
				this.uidcount = 0;
			}
		}
		this.grabSphereIDs.Add(this.uidcount, true);
		gs.id = this.uidcount;
		gs.enabledThisFrame = 1;
		this.grabSpheres.Add(gs);
	}

	// Token: 0x06003EA9 RID: 16041 RVA: 0x0012D8A3 File Offset: 0x0012BCA3
	public void DeregGrabSphere(GpuGrabSphere gs)
	{
		this.grabSphereIDs.Remove(gs.id);
		this.grabSpheres.Remove(gs);
	}

	// Token: 0x06003EAA RID: 16042 RVA: 0x0012D8C4 File Offset: 0x0012BCC4
	protected GpuBuffer<GPLineSphere> ComputeEditCapsule(GpuBuffer<GPLineSphere> buffer, List<GpuEditCapsule> capsuleList)
	{
		GpuBuffer<GPLineSphere> gpuBuffer = buffer;
		if (gpuBuffer == null || gpuBuffer.Count != capsuleList.Count)
		{
			if (gpuBuffer != null)
			{
				gpuBuffer.Dispose();
			}
			if (capsuleList.Count > 0)
			{
				gpuBuffer = new GpuBuffer<GPLineSphere>(capsuleList.Count, GPLineSphere.Size());
			}
			else
			{
				gpuBuffer = null;
			}
		}
		if (capsuleList.Count > 0)
		{
			GPLineSphere[] data = gpuBuffer.Data;
			for (int i = 0; i < capsuleList.Count; i++)
			{
				GpuEditCapsule gpuEditCapsule = capsuleList[i];
				gpuEditCapsule.UpdateData();
				data[i].PositionA = gpuEditCapsule.WorldA;
				data[i].PositionB = gpuEditCapsule.WorldB;
				data[i].RadiusA = gpuEditCapsule.WorldRadiusA;
				data[i].RadiusB = gpuEditCapsule.WorldRadiusB;
				data[i].Friction = gpuEditCapsule.strength;
			}
			gpuBuffer.PushData();
		}
		return gpuBuffer;
	}

	// Token: 0x06003EAB RID: 16043 RVA: 0x0012D9B3 File Offset: 0x0012BDB3
	protected void ComputeCutCapsules()
	{
		this.cutCapsulesBuff = this.ComputeEditCapsule(this.cutCapsulesBuff, this.cutCapsules);
	}

	// Token: 0x06003EAC RID: 16044 RVA: 0x0012D9CD File Offset: 0x0012BDCD
	public void RegCutCapsule(GpuEditCapsule gs)
	{
		this.cutCapsules.Add(gs);
	}

	// Token: 0x06003EAD RID: 16045 RVA: 0x0012D9DB File Offset: 0x0012BDDB
	public void DeregCutCapsule(GpuEditCapsule gs)
	{
		this.cutCapsules.Remove(gs);
	}

	// Token: 0x06003EAE RID: 16046 RVA: 0x0012D9EA File Offset: 0x0012BDEA
	protected void ComputeGrowCapsules()
	{
		this.growCapsulesBuff = this.ComputeEditCapsule(this.growCapsulesBuff, this.growCapsules);
	}

	// Token: 0x06003EAF RID: 16047 RVA: 0x0012DA04 File Offset: 0x0012BE04
	public void RegGrowCapsule(GpuEditCapsule gs)
	{
		this.growCapsules.Add(gs);
	}

	// Token: 0x06003EB0 RID: 16048 RVA: 0x0012DA12 File Offset: 0x0012BE12
	public void DeregGrowCapsule(GpuEditCapsule gs)
	{
		this.growCapsules.Remove(gs);
	}

	// Token: 0x06003EB1 RID: 16049 RVA: 0x0012DA21 File Offset: 0x0012BE21
	protected void ComputeHoldCapsules()
	{
		this.holdCapsulesBuff = this.ComputeEditCapsule(this.holdCapsulesBuff, this.holdCapsules);
	}

	// Token: 0x06003EB2 RID: 16050 RVA: 0x0012DA3B File Offset: 0x0012BE3B
	public void RegHoldCapsule(GpuEditCapsule gs)
	{
		this.holdCapsules.Add(gs);
	}

	// Token: 0x06003EB3 RID: 16051 RVA: 0x0012DA49 File Offset: 0x0012BE49
	public void DeregHoldCapsule(GpuEditCapsule gs)
	{
		this.holdCapsules.Remove(gs);
	}

	// Token: 0x06003EB4 RID: 16052 RVA: 0x0012DA58 File Offset: 0x0012BE58
	protected void ComputeGrabCapsules()
	{
		if (this.grabCapsulesBuff == null || this.grabCapsulesBuff.Count != this.grabCapsules.Count)
		{
			if (this.grabCapsulesBuff != null)
			{
				this.grabCapsulesBuff.Dispose();
			}
			if (this.grabCapsules.Count > 0)
			{
				this.grabCapsulesBuff = new GpuBuffer<GPLineSphereWithMatrixDelta>(this.grabCapsules.Count, GPLineSphereWithMatrixDelta.Size());
			}
			else
			{
				this.grabCapsulesBuff = null;
			}
		}
		if (this.grabCapsules.Count > 0)
		{
			GPLineSphereWithMatrixDelta[] data = this.grabCapsulesBuff.Data;
			for (int i = 0; i < this.grabCapsules.Count; i++)
			{
				GpuGrabCapsule gpuGrabCapsule = this.grabCapsules[i] as GpuGrabCapsule;
				gpuGrabCapsule.UpdateData();
				data[i].PositionA = gpuGrabCapsule.WorldA;
				data[i].PositionB = gpuGrabCapsule.WorldB;
				data[i].RadiusA = gpuGrabCapsule.WorldRadiusA;
				data[i].RadiusB = gpuGrabCapsule.WorldRadiusB;
				data[i].Friction = gpuGrabCapsule.strength;
				data[i].ChangeMatrix = gpuGrabCapsule.changeMatrix;
			}
			this.grabCapsulesBuff.PushData();
		}
	}

	// Token: 0x06003EB5 RID: 16053 RVA: 0x0012DBA1 File Offset: 0x0012BFA1
	public void RegGrabCapsule(GpuEditCapsule gs)
	{
		this.grabCapsules.Add(gs);
	}

	// Token: 0x06003EB6 RID: 16054 RVA: 0x0012DBAF File Offset: 0x0012BFAF
	public void DeregGrabCapsule(GpuEditCapsule gs)
	{
		this.grabCapsules.Remove(gs);
	}

	// Token: 0x06003EB7 RID: 16055 RVA: 0x0012DBBE File Offset: 0x0012BFBE
	protected void ComputePushCapsules()
	{
		this.pushCapsulesBuff = this.ComputeEditCapsule(this.pushCapsulesBuff, this.pushCapsules);
	}

	// Token: 0x06003EB8 RID: 16056 RVA: 0x0012DBD8 File Offset: 0x0012BFD8
	public void RegPushCapsule(GpuEditCapsule gs)
	{
		this.pushCapsules.Add(gs);
	}

	// Token: 0x06003EB9 RID: 16057 RVA: 0x0012DBE6 File Offset: 0x0012BFE6
	public void DeregPushCapsule(GpuEditCapsule gs)
	{
		this.pushCapsules.Remove(gs);
	}

	// Token: 0x06003EBA RID: 16058 RVA: 0x0012DBF5 File Offset: 0x0012BFF5
	protected void ComputePullCapsules()
	{
		this.pullCapsulesBuff = this.ComputeEditCapsule(this.pullCapsulesBuff, this.pullCapsules);
	}

	// Token: 0x06003EBB RID: 16059 RVA: 0x0012DC0F File Offset: 0x0012C00F
	public void RegPullCapsule(GpuEditCapsule gs)
	{
		this.pullCapsules.Add(gs);
	}

	// Token: 0x06003EBC RID: 16060 RVA: 0x0012DC1D File Offset: 0x0012C01D
	public void DeregPullCapsule(GpuEditCapsule gs)
	{
		this.pullCapsules.Remove(gs);
	}

	// Token: 0x06003EBD RID: 16061 RVA: 0x0012DC2C File Offset: 0x0012C02C
	protected void ComputeBrushCapsules()
	{
		if (this.brushCapsulesBuff == null || this.brushCapsulesBuff.Count != this.brushCapsules.Count)
		{
			if (this.brushCapsulesBuff != null)
			{
				this.brushCapsulesBuff.Dispose();
			}
			if (this.brushCapsules.Count > 0)
			{
				this.brushCapsulesBuff = new GpuBuffer<GPLineSphereWithDelta>(this.brushCapsules.Count, GPLineSphereWithDelta.Size());
			}
			else
			{
				this.brushCapsulesBuff = null;
			}
		}
		if (this.brushCapsules.Count > 0)
		{
			GPLineSphereWithDelta[] data = this.brushCapsulesBuff.Data;
			for (int i = 0; i < this.brushCapsules.Count; i++)
			{
				GpuEditCapsule gpuEditCapsule = this.brushCapsules[i];
				gpuEditCapsule.UpdateData();
				Vector3 positionA = data[i].PositionA;
				Vector3 positionB = data[i].PositionB;
				data[i].PositionA = gpuEditCapsule.WorldA;
				data[i].PositionB = gpuEditCapsule.WorldB;
				data[i].DeltaA = data[i].PositionA - positionA;
				data[i].DeltaB = data[i].PositionB - positionB;
				data[i].Delta = (data[i].DeltaA + data[i].DeltaB) * 0.5f;
				data[i].RadiusA = gpuEditCapsule.WorldRadiusA;
				data[i].RadiusB = gpuEditCapsule.WorldRadiusB;
				data[i].Friction = gpuEditCapsule.strength;
			}
			this.brushCapsulesBuff.PushData();
		}
	}

	// Token: 0x06003EBE RID: 16062 RVA: 0x0012DDE9 File Offset: 0x0012C1E9
	public void RegBrushCapsule(GpuEditCapsule gs)
	{
		this.brushCapsules.Add(gs);
	}

	// Token: 0x06003EBF RID: 16063 RVA: 0x0012DDF7 File Offset: 0x0012C1F7
	public void DeregBrushCapsule(GpuEditCapsule gs)
	{
		this.brushCapsules.Remove(gs);
	}

	// Token: 0x06003EC0 RID: 16064 RVA: 0x0012DE06 File Offset: 0x0012C206
	protected void ComputeRigidityIncreaseCapsules()
	{
		this.rigidityIncreaseCapsulesBuff = this.ComputeEditCapsule(this.rigidityIncreaseCapsulesBuff, this.rigidityIncreaseCapsules);
	}

	// Token: 0x06003EC1 RID: 16065 RVA: 0x0012DE20 File Offset: 0x0012C220
	public void RegRigidityIncreaseCapsule(GpuEditCapsule gs)
	{
		this.rigidityIncreaseCapsules.Add(gs);
	}

	// Token: 0x06003EC2 RID: 16066 RVA: 0x0012DE2E File Offset: 0x0012C22E
	public void DeregRigidityIncreaseCapsule(GpuEditCapsule gs)
	{
		this.rigidityIncreaseCapsules.Remove(gs);
	}

	// Token: 0x06003EC3 RID: 16067 RVA: 0x0012DE3D File Offset: 0x0012C23D
	protected void ComputeRigidityDecreaseCapsules()
	{
		this.rigidityDecreaseCapsulesBuff = this.ComputeEditCapsule(this.rigidityDecreaseCapsulesBuff, this.rigidityDecreaseCapsules);
	}

	// Token: 0x06003EC4 RID: 16068 RVA: 0x0012DE57 File Offset: 0x0012C257
	public void RegRigidityDecreaseCapsule(GpuEditCapsule gs)
	{
		this.rigidityDecreaseCapsules.Add(gs);
	}

	// Token: 0x06003EC5 RID: 16069 RVA: 0x0012DE65 File Offset: 0x0012C265
	public void DeregRigidityDecreaseCapsule(GpuEditCapsule gs)
	{
		this.rigidityDecreaseCapsules.Remove(gs);
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x0012DE74 File Offset: 0x0012C274
	protected void ComputeRigiditySetCapsules()
	{
		this.rigiditySetCapsulesBuff = this.ComputeEditCapsule(this.rigiditySetCapsulesBuff, this.rigiditySetCapsules);
	}

	// Token: 0x06003EC7 RID: 16071 RVA: 0x0012DE8E File Offset: 0x0012C28E
	public void RegRigiditySetCapsule(GpuEditCapsule gs)
	{
		this.rigiditySetCapsules.Add(gs);
	}

	// Token: 0x06003EC8 RID: 16072 RVA: 0x0012DE9C File Offset: 0x0012C29C
	public void DeregRigiditySetCapsule(GpuEditCapsule gs)
	{
		this.rigiditySetCapsules.Remove(gs);
	}

	// Token: 0x06003EC9 RID: 16073 RVA: 0x0012DEAC File Offset: 0x0012C2AC
	public void Init()
	{
		if (!this._wasInit)
		{
			this._wasInit = true;
			GPUCollidersManager.singleton = this;
			this.sphereColliders = new List<GpuSphereCollider>();
			this.lineSphereColliders = new List<CapsuleLineSphereCollider>();
			this.planeColliders = new List<PlaneCollider>();
			this.lineSphereCopyKernel = new ParticleLineSphereCopyKernel();
			this.sphereCopyKernel = new ParticleSphereCopyKernel();
			this.lineSphereProcessKernel = new ParticleLineSphereProcessKernel();
			this.lineSphereProcessKernel.CollisionPrediction = new GpuValue<float>(0f);
			this.sphereProcessKernel = new ParticleSphereProcessKernel();
			this.sphereProcessKernel.CollisionPrediction = new GpuValue<float>(0f);
			this.consumers = new List<GPUCollidersConsumer>();
			this.grabSpheres = new List<GpuGrabSphere>();
			this.grabSphereIDs = new Dictionary<int, bool>();
			this.cutCapsules = new List<GpuEditCapsule>();
			this.growCapsules = new List<GpuEditCapsule>();
			this.pushCapsules = new List<GpuEditCapsule>();
			this.pullCapsules = new List<GpuEditCapsule>();
			this.brushCapsules = new List<GpuEditCapsule>();
			this.holdCapsules = new List<GpuEditCapsule>();
			this.grabCapsules = new List<GpuEditCapsule>();
			this.rigidityIncreaseCapsules = new List<GpuEditCapsule>();
			this.rigidityDecreaseCapsules = new List<GpuEditCapsule>();
			this.rigiditySetCapsules = new List<GpuEditCapsule>();
		}
	}

	// Token: 0x06003ECA RID: 16074 RVA: 0x0012DFD7 File Offset: 0x0012C3D7
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x06003ECB RID: 16075 RVA: 0x0012DFE0 File Offset: 0x0012C3E0
	private void FixedUpdate()
	{
		this.fixedDispatchCount++;
		if (Time.fixedDeltaTime > 0.02f)
		{
			this._collisionPrediction = 0f;
		}
		else
		{
			this._collisionPrediction = Mathf.Clamp01(1f - Time.timeScale);
		}
		this.sphereProcessKernel.CollisionPrediction.Value = this._collisionPrediction;
		this.lineSphereProcessKernel.CollisionPrediction.Value = this._collisionPrediction;
		this.ComputeColliders();
		this.ComputeGrabSpheres();
		this.ComputeCutCapsules();
		this.ComputeGrowCapsules();
		this.ComputeHoldCapsules();
		this.ComputeGrabCapsules();
		this.ComputePushCapsules();
		this.ComputePullCapsules();
		this.ComputeBrushCapsules();
		this.ComputeRigidityIncreaseCapsules();
		this.ComputeRigidityDecreaseCapsules();
		this.ComputeRigiditySetCapsules();
	}

	// Token: 0x06003ECC RID: 16076 RVA: 0x0012E0A4 File Offset: 0x0012C4A4
	private void Update()
	{
		this.fixedDispatchCount = 0;
	}

	// Token: 0x06003ECD RID: 16077 RVA: 0x0012E0B0 File Offset: 0x0012C4B0
	private void OnDestroy()
	{
		if (this.spheres != null)
		{
			this.spheres.Dispose();
		}
		if (this.oldSpheres != null)
		{
			this.oldSpheres.Dispose();
		}
		if (this.processedSpheres != null)
		{
			this.processedSpheres.Dispose();
		}
		if (this.lineSpheres != null)
		{
			this.lineSpheres.Dispose();
		}
		if (this.oldLineSpheres != null)
		{
			this.oldLineSpheres.Dispose();
		}
		if (this.processedLineSpheres != null)
		{
			this.processedLineSpheres.Dispose();
		}
		if (this.planes != null)
		{
			this.planes.Dispose();
		}
		if (this.grabSpheresBuff != null)
		{
			this.grabSpheresBuff.Dispose();
		}
		if (this.cutCapsulesBuff != null)
		{
			this.cutCapsulesBuff.Dispose();
		}
		if (this.growCapsulesBuff != null)
		{
			this.growCapsulesBuff.Dispose();
		}
		if (this.holdCapsulesBuff != null)
		{
			this.holdCapsulesBuff.Dispose();
		}
		if (this.grabCapsulesBuff != null)
		{
			this.grabCapsulesBuff.Dispose();
		}
		if (this.pushCapsulesBuff != null)
		{
			this.pushCapsulesBuff.Dispose();
		}
		if (this.pullCapsulesBuff != null)
		{
			this.pullCapsulesBuff.Dispose();
		}
		if (this.brushCapsulesBuff != null)
		{
			this.brushCapsulesBuff.Dispose();
		}
		if (this.rigidityIncreaseCapsulesBuff != null)
		{
			this.rigidityIncreaseCapsulesBuff.Dispose();
		}
		if (this.rigidityDecreaseCapsulesBuff != null)
		{
			this.rigidityDecreaseCapsulesBuff.Dispose();
		}
		if (this.rigiditySetCapsulesBuff != null)
		{
			this.rigiditySetCapsulesBuff.Dispose();
		}
	}

	// Token: 0x04002FAA RID: 12202
	public static GPUCollidersManager singleton;

	// Token: 0x04002FAB RID: 12203
	protected GpuBuffer<GPLineSphere> lineSpheres;

	// Token: 0x04002FAC RID: 12204
	protected GpuBuffer<GPLineSphere> oldLineSpheres;

	// Token: 0x04002FAD RID: 12205
	protected GpuBuffer<GPLineSphereWithDelta> processedLineSpheres;

	// Token: 0x04002FAE RID: 12206
	protected GpuBuffer<GPSphere> spheres;

	// Token: 0x04002FAF RID: 12207
	protected GpuBuffer<GPSphere> oldSpheres;

	// Token: 0x04002FB0 RID: 12208
	protected GpuBuffer<GPSphereWithDelta> processedSpheres;

	// Token: 0x04002FB1 RID: 12209
	protected GpuBuffer<Vector4> planes;

	// Token: 0x04002FB2 RID: 12210
	protected List<GPUCollidersConsumer> consumers;

	// Token: 0x04002FB3 RID: 12211
	protected List<GpuSphereCollider> sphereColliders;

	// Token: 0x04002FB4 RID: 12212
	protected List<CapsuleLineSphereCollider> lineSphereColliders;

	// Token: 0x04002FB5 RID: 12213
	protected List<PlaneCollider> planeColliders;

	// Token: 0x04002FB6 RID: 12214
	private ParticleLineSphereCopyKernel lineSphereCopyKernel;

	// Token: 0x04002FB7 RID: 12215
	private ParticleSphereCopyKernel sphereCopyKernel;

	// Token: 0x04002FB8 RID: 12216
	private ParticleLineSphereProcessKernel lineSphereProcessKernel;

	// Token: 0x04002FB9 RID: 12217
	private ParticleSphereProcessKernel sphereProcessKernel;

	// Token: 0x04002FBA RID: 12218
	protected bool usingNullGrabSpheresBuff;

	// Token: 0x04002FBB RID: 12219
	protected GpuBuffer<GPGrabSphere> grabSpheresBuff;

	// Token: 0x04002FBC RID: 12220
	protected List<GpuGrabSphere> grabSpheres;

	// Token: 0x04002FBD RID: 12221
	protected Dictionary<int, bool> grabSphereIDs;

	// Token: 0x04002FBE RID: 12222
	protected int uidcount;

	// Token: 0x04002FBF RID: 12223
	protected GpuBuffer<GPLineSphere> cutCapsulesBuff;

	// Token: 0x04002FC0 RID: 12224
	protected List<GpuEditCapsule> cutCapsules;

	// Token: 0x04002FC1 RID: 12225
	protected GpuBuffer<GPLineSphere> growCapsulesBuff;

	// Token: 0x04002FC2 RID: 12226
	protected List<GpuEditCapsule> growCapsules;

	// Token: 0x04002FC3 RID: 12227
	protected GpuBuffer<GPLineSphere> holdCapsulesBuff;

	// Token: 0x04002FC4 RID: 12228
	protected List<GpuEditCapsule> holdCapsules;

	// Token: 0x04002FC5 RID: 12229
	protected GpuBuffer<GPLineSphereWithMatrixDelta> grabCapsulesBuff;

	// Token: 0x04002FC6 RID: 12230
	protected List<GpuEditCapsule> grabCapsules;

	// Token: 0x04002FC7 RID: 12231
	protected GpuBuffer<GPLineSphere> pushCapsulesBuff;

	// Token: 0x04002FC8 RID: 12232
	protected List<GpuEditCapsule> pushCapsules;

	// Token: 0x04002FC9 RID: 12233
	protected GpuBuffer<GPLineSphere> pullCapsulesBuff;

	// Token: 0x04002FCA RID: 12234
	protected List<GpuEditCapsule> pullCapsules;

	// Token: 0x04002FCB RID: 12235
	protected GpuBuffer<GPLineSphereWithDelta> brushCapsulesBuff;

	// Token: 0x04002FCC RID: 12236
	protected List<GpuEditCapsule> brushCapsules;

	// Token: 0x04002FCD RID: 12237
	protected GpuBuffer<GPLineSphere> rigidityIncreaseCapsulesBuff;

	// Token: 0x04002FCE RID: 12238
	protected List<GpuEditCapsule> rigidityIncreaseCapsules;

	// Token: 0x04002FCF RID: 12239
	protected GpuBuffer<GPLineSphere> rigidityDecreaseCapsulesBuff;

	// Token: 0x04002FD0 RID: 12240
	protected List<GpuEditCapsule> rigidityDecreaseCapsules;

	// Token: 0x04002FD1 RID: 12241
	protected GpuBuffer<GPLineSphere> rigiditySetCapsulesBuff;

	// Token: 0x04002FD2 RID: 12242
	protected List<GpuEditCapsule> rigiditySetCapsules;

	// Token: 0x04002FD3 RID: 12243
	protected bool _wasInit;

	// Token: 0x04002FD4 RID: 12244
	public float fixedFrame1Prediction = 1.2f;

	// Token: 0x04002FD5 RID: 12245
	public float fixedFrame2Prediction = 1.2f;

	// Token: 0x04002FD6 RID: 12246
	public float fixedFrame3Prediction = 1.2f;

	// Token: 0x04002FD7 RID: 12247
	protected float _collisionPrediction;

	// Token: 0x04002FD8 RID: 12248
	protected int fixedDispatchCount;
}
