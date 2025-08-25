using System;
using UnityEngine;

// Token: 0x02000302 RID: 770
public class CubeGenerator : MonoBehaviour
{
	// Token: 0x06001234 RID: 4660 RVA: 0x000647B7 File Offset: 0x00062BB7
	public CubeGenerator()
	{
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x000647BF File Offset: 0x00062BBF
	private void Start()
	{
		base.InvokeRepeating("UpdateCube", 1f, 2f);
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x000647D8 File Offset: 0x00062BD8
	private void UpdateCube()
	{
		Vector3 vector = base.transform.position;
		vector.y += 10f;
		vector.z -= 4f;
		vector += UnityEngine.Random.insideUnitSphere * 7f;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.cubes, vector, Quaternion.Euler((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360)));
		gameObject.AddComponent<Buoyancy>().Density = (float)UnityEngine.Random.Range(700, 850);
		gameObject.AddComponent<Rigidbody>().mass = (float)UnityEngine.Random.Range(100, 150);
		UnityEngine.Object.Destroy(gameObject, 30f);
	}

	// Token: 0x04000F7E RID: 3966
	public GameObject cubes;
}
