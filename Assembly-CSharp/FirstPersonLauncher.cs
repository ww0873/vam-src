using System;
using UnityEngine;

// Token: 0x02000385 RID: 901
public class FirstPersonLauncher : MonoBehaviour
{
	// Token: 0x0600167D RID: 5757 RVA: 0x0007E464 File Offset: 0x0007C864
	public FirstPersonLauncher()
	{
	}

	// Token: 0x0600167E RID: 5758 RVA: 0x0007E478 File Offset: 0x0007C878
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab, ray.origin, Quaternion.identity);
			Rigidbody component = gameObject.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.velocity = ray.direction * this.power;
			}
		}
	}

	// Token: 0x040012A0 RID: 4768
	public GameObject prefab;

	// Token: 0x040012A1 RID: 4769
	public float power = 2f;
}
