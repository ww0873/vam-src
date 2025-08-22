using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020008C6 RID: 2246
public class OVRBoundary
{
	// Token: 0x06003877 RID: 14455 RVA: 0x001134BD File Offset: 0x001118BD
	public OVRBoundary()
	{
	}

	// Token: 0x06003878 RID: 14456 RVA: 0x001134C5 File Offset: 0x001118C5
	public bool GetConfigured()
	{
		return OVRPlugin.GetBoundaryConfigured();
	}

	// Token: 0x06003879 RID: 14457 RVA: 0x001134CC File Offset: 0x001118CC
	public OVRBoundary.BoundaryTestResult TestNode(OVRBoundary.Node node, OVRBoundary.BoundaryType boundaryType)
	{
		OVRPlugin.BoundaryTestResult boundaryTestResult = OVRPlugin.TestBoundaryNode((OVRPlugin.Node)node, (OVRPlugin.BoundaryType)boundaryType);
		return new OVRBoundary.BoundaryTestResult
		{
			IsTriggering = (boundaryTestResult.IsTriggering == OVRPlugin.Bool.True),
			ClosestDistance = boundaryTestResult.ClosestDistance,
			ClosestPoint = boundaryTestResult.ClosestPoint.FromFlippedZVector3f(),
			ClosestPointNormal = boundaryTestResult.ClosestPointNormal.FromFlippedZVector3f()
		};
	}

	// Token: 0x0600387A RID: 14458 RVA: 0x00113534 File Offset: 0x00111934
	public OVRBoundary.BoundaryTestResult TestPoint(Vector3 point, OVRBoundary.BoundaryType boundaryType)
	{
		OVRPlugin.BoundaryTestResult boundaryTestResult = OVRPlugin.TestBoundaryPoint(point.ToFlippedZVector3f(), (OVRPlugin.BoundaryType)boundaryType);
		return new OVRBoundary.BoundaryTestResult
		{
			IsTriggering = (boundaryTestResult.IsTriggering == OVRPlugin.Bool.True),
			ClosestDistance = boundaryTestResult.ClosestDistance,
			ClosestPoint = boundaryTestResult.ClosestPoint.FromFlippedZVector3f(),
			ClosestPointNormal = boundaryTestResult.ClosestPointNormal.FromFlippedZVector3f()
		};
	}

	// Token: 0x0600387B RID: 14459 RVA: 0x001135A0 File Offset: 0x001119A0
	public void SetLookAndFeel(OVRBoundary.BoundaryLookAndFeel lookAndFeel)
	{
		OVRPlugin.BoundaryLookAndFeel boundaryLookAndFeel = new OVRPlugin.BoundaryLookAndFeel
		{
			Color = lookAndFeel.Color.ToColorf()
		};
		OVRPlugin.SetBoundaryLookAndFeel(boundaryLookAndFeel);
	}

	// Token: 0x0600387C RID: 14460 RVA: 0x001135D1 File Offset: 0x001119D1
	public void ResetLookAndFeel()
	{
		OVRPlugin.ResetBoundaryLookAndFeel();
	}

	// Token: 0x0600387D RID: 14461 RVA: 0x001135DC File Offset: 0x001119DC
	public Vector3[] GetGeometry(OVRBoundary.BoundaryType boundaryType)
	{
		int num = 0;
		if (OVRPlugin.GetBoundaryGeometry2((OVRPlugin.BoundaryType)boundaryType, IntPtr.Zero, ref num) && num > 0)
		{
			int num2 = num * OVRBoundary.cachedVector3fSize;
			if (OVRBoundary.cachedGeometryNativeBuffer.GetCapacity() < num2)
			{
				OVRBoundary.cachedGeometryNativeBuffer.Reset(num2);
			}
			int num3 = num * 3;
			if (OVRBoundary.cachedGeometryManagedBuffer.Length < num3)
			{
				OVRBoundary.cachedGeometryManagedBuffer = new float[num3];
			}
			if (OVRPlugin.GetBoundaryGeometry2((OVRPlugin.BoundaryType)boundaryType, OVRBoundary.cachedGeometryNativeBuffer.GetPointer(0), ref num))
			{
				Marshal.Copy(OVRBoundary.cachedGeometryNativeBuffer.GetPointer(0), OVRBoundary.cachedGeometryManagedBuffer, 0, num3);
				Vector3[] array = new Vector3[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = new OVRPlugin.Vector3f
					{
						x = OVRBoundary.cachedGeometryManagedBuffer[3 * i],
						y = OVRBoundary.cachedGeometryManagedBuffer[3 * i + 1],
						z = OVRBoundary.cachedGeometryManagedBuffer[3 * i + 2]
					}.FromFlippedZVector3f();
				}
				return array;
			}
		}
		return new Vector3[0];
	}

	// Token: 0x0600387E RID: 14462 RVA: 0x001136EA File Offset: 0x00111AEA
	public Vector3 GetDimensions(OVRBoundary.BoundaryType boundaryType)
	{
		return OVRPlugin.GetBoundaryDimensions((OVRPlugin.BoundaryType)boundaryType).FromVector3f();
	}

	// Token: 0x0600387F RID: 14463 RVA: 0x001136F7 File Offset: 0x00111AF7
	public bool GetVisible()
	{
		return OVRPlugin.GetBoundaryVisible();
	}

	// Token: 0x06003880 RID: 14464 RVA: 0x001136FE File Offset: 0x00111AFE
	public void SetVisible(bool value)
	{
		OVRPlugin.SetBoundaryVisible(value);
	}

	// Token: 0x06003881 RID: 14465 RVA: 0x00113707 File Offset: 0x00111B07
	// Note: this type is marked as 'beforefieldinit'.
	static OVRBoundary()
	{
	}

	// Token: 0x040029BE RID: 10686
	private static int cachedVector3fSize = Marshal.SizeOf(typeof(OVRPlugin.Vector3f));

	// Token: 0x040029BF RID: 10687
	private static OVRNativeBuffer cachedGeometryNativeBuffer = new OVRNativeBuffer(0);

	// Token: 0x040029C0 RID: 10688
	private static float[] cachedGeometryManagedBuffer = new float[0];

	// Token: 0x020008C7 RID: 2247
	public enum Node
	{
		// Token: 0x040029C2 RID: 10690
		HandLeft = 3,
		// Token: 0x040029C3 RID: 10691
		HandRight,
		// Token: 0x040029C4 RID: 10692
		Head = 9
	}

	// Token: 0x020008C8 RID: 2248
	public enum BoundaryType
	{
		// Token: 0x040029C6 RID: 10694
		OuterBoundary = 1,
		// Token: 0x040029C7 RID: 10695
		PlayArea = 256
	}

	// Token: 0x020008C9 RID: 2249
	public struct BoundaryTestResult
	{
		// Token: 0x040029C8 RID: 10696
		public bool IsTriggering;

		// Token: 0x040029C9 RID: 10697
		public float ClosestDistance;

		// Token: 0x040029CA RID: 10698
		public Vector3 ClosestPoint;

		// Token: 0x040029CB RID: 10699
		public Vector3 ClosestPointNormal;
	}

	// Token: 0x020008CA RID: 2250
	public struct BoundaryLookAndFeel
	{
		// Token: 0x040029CC RID: 10700
		public Color Color;
	}
}
