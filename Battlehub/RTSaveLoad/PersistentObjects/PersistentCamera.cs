using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Rendering;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200014A RID: 330
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCamera : PersistentBehaviour
	{
		// Token: 0x0600077A RID: 1914 RVA: 0x00032DB1 File Offset: 0x000311B1
		public PersistentCamera()
		{
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00032DBC File Offset: 0x000311BC
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Camera camera = (Camera)obj;
			camera.fieldOfView = this.fieldOfView;
			camera.nearClipPlane = this.nearClipPlane;
			camera.farClipPlane = this.farClipPlane;
			camera.renderingPath = (RenderingPath)this.renderingPath;
			camera.allowHDR = this.allowHDR;
			camera.forceIntoRenderTexture = this.forceIntoRenderTexture;
			camera.allowMSAA = this.allowMSAA;
			camera.orthographicSize = this.orthographicSize;
			camera.orthographic = this.orthographic;
			camera.opaqueSortMode = (OpaqueSortMode)this.opaqueSortMode;
			camera.transparencySortMode = (TransparencySortMode)this.transparencySortMode;
			camera.transparencySortAxis = this.transparencySortAxis;
			camera.depth = this.depth;
			camera.cullingMask = this.cullingMask;
			camera.eventMask = this.eventMask;
			camera.backgroundColor = this.backgroundColor;
			camera.rect = this.rect;
			camera.pixelRect = this.pixelRect;
			camera.targetTexture = (RenderTexture)objects.Get(this.targetTexture);
			camera.useJitteredProjectionMatrixForTransparentRendering = this.useJitteredProjectionMatrixForTransparentRendering;
			camera.clearFlags = (CameraClearFlags)this.clearFlags;
			camera.stereoSeparation = this.stereoSeparation;
			camera.stereoConvergence = this.stereoConvergence;
			camera.cameraType = (CameraType)this.cameraType;
			camera.stereoTargetEye = (StereoTargetEyeMask)this.stereoTargetEye;
			camera.targetDisplay = this.targetDisplay;
			camera.useOcclusionCulling = this.useOcclusionCulling;
			camera.layerCullDistances = this.layerCullDistances;
			camera.layerCullSpherical = this.layerCullSpherical;
			camera.depthTextureMode = (DepthTextureMode)this.depthTextureMode;
			camera.clearStencilAfterLightingPass = this.clearStencilAfterLightingPass;
			return camera;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00032F64 File Offset: 0x00031364
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Camera camera = (Camera)obj;
			this.fieldOfView = camera.fieldOfView;
			this.nearClipPlane = camera.nearClipPlane;
			this.farClipPlane = camera.farClipPlane;
			this.renderingPath = (uint)camera.renderingPath;
			this.allowHDR = camera.allowHDR;
			this.forceIntoRenderTexture = camera.forceIntoRenderTexture;
			this.allowMSAA = camera.allowMSAA;
			this.orthographicSize = camera.orthographicSize;
			this.orthographic = camera.orthographic;
			this.opaqueSortMode = (uint)camera.opaqueSortMode;
			this.transparencySortMode = (uint)camera.transparencySortMode;
			this.transparencySortAxis = camera.transparencySortAxis;
			this.depth = camera.depth;
			this.cullingMask = camera.cullingMask;
			this.eventMask = camera.eventMask;
			this.backgroundColor = camera.backgroundColor;
			this.rect = camera.rect;
			this.pixelRect = camera.pixelRect;
			this.targetTexture = camera.targetTexture.GetMappedInstanceID();
			this.useJitteredProjectionMatrixForTransparentRendering = camera.useJitteredProjectionMatrixForTransparentRendering;
			this.clearFlags = (uint)camera.clearFlags;
			this.stereoSeparation = camera.stereoSeparation;
			this.stereoConvergence = camera.stereoConvergence;
			this.cameraType = (uint)camera.cameraType;
			this.stereoTargetEye = (uint)camera.stereoTargetEye;
			this.targetDisplay = camera.targetDisplay;
			this.useOcclusionCulling = camera.useOcclusionCulling;
			this.layerCullDistances = camera.layerCullDistances;
			this.layerCullSpherical = camera.layerCullSpherical;
			this.depthTextureMode = (uint)camera.depthTextureMode;
			this.clearStencilAfterLightingPass = camera.clearStencilAfterLightingPass;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x000330FF File Offset: 0x000314FF
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.targetTexture, dependencies, objects, allowNulls);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0003311C File Offset: 0x0003151C
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Camera camera = (Camera)obj;
			base.AddDependency(camera.targetTexture, dependencies);
		}

		// Token: 0x040007EA RID: 2026
		public float fieldOfView;

		// Token: 0x040007EB RID: 2027
		public float nearClipPlane;

		// Token: 0x040007EC RID: 2028
		public float farClipPlane;

		// Token: 0x040007ED RID: 2029
		public uint renderingPath;

		// Token: 0x040007EE RID: 2030
		public bool allowHDR;

		// Token: 0x040007EF RID: 2031
		public bool forceIntoRenderTexture;

		// Token: 0x040007F0 RID: 2032
		public bool allowMSAA;

		// Token: 0x040007F1 RID: 2033
		public float orthographicSize;

		// Token: 0x040007F2 RID: 2034
		public bool orthographic;

		// Token: 0x040007F3 RID: 2035
		public uint opaqueSortMode;

		// Token: 0x040007F4 RID: 2036
		public uint transparencySortMode;

		// Token: 0x040007F5 RID: 2037
		public Vector3 transparencySortAxis;

		// Token: 0x040007F6 RID: 2038
		public float depth;

		// Token: 0x040007F7 RID: 2039
		public int cullingMask;

		// Token: 0x040007F8 RID: 2040
		public int eventMask;

		// Token: 0x040007F9 RID: 2041
		public Color backgroundColor;

		// Token: 0x040007FA RID: 2042
		public Rect rect;

		// Token: 0x040007FB RID: 2043
		public Rect pixelRect;

		// Token: 0x040007FC RID: 2044
		public long targetTexture;

		// Token: 0x040007FD RID: 2045
		public bool useJitteredProjectionMatrixForTransparentRendering;

		// Token: 0x040007FE RID: 2046
		public uint clearFlags;

		// Token: 0x040007FF RID: 2047
		public float stereoSeparation;

		// Token: 0x04000800 RID: 2048
		public float stereoConvergence;

		// Token: 0x04000801 RID: 2049
		public uint cameraType;

		// Token: 0x04000802 RID: 2050
		public bool stereoMirrorMode;

		// Token: 0x04000803 RID: 2051
		public uint stereoTargetEye;

		// Token: 0x04000804 RID: 2052
		public int targetDisplay;

		// Token: 0x04000805 RID: 2053
		public bool useOcclusionCulling;

		// Token: 0x04000806 RID: 2054
		public float[] layerCullDistances;

		// Token: 0x04000807 RID: 2055
		public bool layerCullSpherical;

		// Token: 0x04000808 RID: 2056
		public uint depthTextureMode;

		// Token: 0x04000809 RID: 2057
		public bool clearStencilAfterLightingPass;
	}
}
