using System;
using UnityEngine;

// Token: 0x02000300 RID: 768
public class CausticDecal : MonoBehaviour
{
	// Token: 0x0600122E RID: 4654 RVA: 0x000643C3 File Offset: 0x000627C3
	public CausticDecal()
	{
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x000643ED File Offset: 0x000627ED
	private void OnDrawGizmosSelected()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x00064410 File Offset: 0x00062810
	public Bounds GetBounds()
	{
		Vector3 lossyScale = base.transform.lossyScale;
		Vector3 vector = -lossyScale / 2f;
		Vector3 vector2 = lossyScale / 2f;
		Vector3[] array = new Vector3[]
		{
			new Vector3(vector.x, vector.y, vector.z),
			new Vector3(vector2.x, vector.y, vector.z),
			new Vector3(vector.x, vector2.y, vector.z),
			new Vector3(vector2.x, vector2.y, vector.z),
			new Vector3(vector.x, vector.y, vector2.z),
			new Vector3(vector2.x, vector.y, vector2.z),
			new Vector3(vector.x, vector2.y, vector2.z),
			new Vector3(vector2.x, vector2.y, vector2.z)
		};
		for (int i = 0; i < 8; i++)
		{
			array[i] = base.transform.TransformDirection(array[i]);
		}
		vector2 = (vector = array[0]);
		foreach (Vector3 rhs in array)
		{
			vector = Vector3.Min(vector, rhs);
			vector2 = Vector3.Max(vector2, rhs);
		}
		return new Bounds(base.transform.position, vector2 - vector);
	}

	// Token: 0x04000F76 RID: 3958
	public float maxAngle = 90f;

	// Token: 0x04000F77 RID: 3959
	public float pushDistance = 0.009f;

	// Token: 0x04000F78 RID: 3960
	public LayerMask affectedLayers = -1;
}
