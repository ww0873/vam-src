using System;
using System.Collections;
using UnityEngine;

namespace Battlehub.Utils
{
	// Token: 0x020002A1 RID: 673
	public static class GameObjectExtensions
	{
		// Token: 0x06000FE5 RID: 4069 RVA: 0x0005ADEC File Offset: 0x000591EC
		public static bool IsPrefab(this GameObject go)
		{
			if (Application.isEditor && !Application.isPlaying)
			{
				throw new InvalidOperationException("Does not work in edit mode");
			}
			return go.scene.buildIndex < 0;
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0005AE2C File Offset: 0x0005922C
		public static Bounds CalculateBounds(this GameObject g)
		{
			Transform transform = g.transform;
			Renderer componentInChildren = transform.GetComponentInChildren<Renderer>(true);
			if (componentInChildren)
			{
				Bounds bounds = componentInChildren.bounds;
				if (bounds.size == Vector3.zero && bounds.center != componentInChildren.transform.position)
				{
					bounds = GameObjectExtensions.TransformBounds(componentInChildren.transform.localToWorldMatrix, bounds);
				}
				GameObjectExtensions.CalculateBounds(transform, ref bounds);
				if (bounds.extents == Vector3.zero)
				{
					bounds.extents = new Vector3(0.5f, 0.5f, 0.5f);
				}
				return bounds;
			}
			return new Bounds(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0005AEF8 File Offset: 0x000592F8
		private static void CalculateBounds(Transform t, ref Bounds totalBounds)
		{
			IEnumerator enumerator = t.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					Renderer component = transform.GetComponent<Renderer>();
					if (component)
					{
						Bounds bounds = component.bounds;
						if (bounds.size == Vector3.zero && bounds.center != component.transform.position)
						{
							bounds = GameObjectExtensions.TransformBounds(component.transform.localToWorldMatrix, bounds);
						}
						totalBounds.Encapsulate(bounds.min);
						totalBounds.Encapsulate(bounds.max);
					}
					GameObjectExtensions.CalculateBounds(transform, ref totalBounds);
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
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0005AFD4 File Offset: 0x000593D4
		public static Bounds TransformBounds(Matrix4x4 matrix, Bounds bounds)
		{
			Vector3 center = matrix.MultiplyPoint(bounds.center);
			Vector3 extents = bounds.extents;
			Vector3 vector = matrix.MultiplyVector(new Vector3(extents.x, 0f, 0f));
			Vector3 vector2 = matrix.MultiplyVector(new Vector3(0f, extents.y, 0f));
			Vector3 vector3 = matrix.MultiplyVector(new Vector3(0f, 0f, extents.z));
			extents.x = Mathf.Abs(vector.x) + Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x);
			extents.y = Mathf.Abs(vector.y) + Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y);
			extents.z = Mathf.Abs(vector.z) + Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z);
			return new Bounds
			{
				center = center,
				extents = extents
			};
		}
	}
}
