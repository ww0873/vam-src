using System;
using UnityEngine;

// Token: 0x02000D55 RID: 3413
public class CollisionDebug : MonoBehaviour
{
	// Token: 0x060068F0 RID: 26864 RVA: 0x00274235 File Offset: 0x00272635
	public CollisionDebug()
	{
	}

	// Token: 0x060068F1 RID: 26865 RVA: 0x00274240 File Offset: 0x00272640
	private void OnCollisionEnter(Collision c)
	{
		if (this.on)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Collision between RB ",
				base.transform.name,
				" and collider ",
				c.collider.name,
				" in RB ",
				c.rigidbody.name
			}));
			foreach (ContactPoint contactPoint in c.contacts)
			{
				MyDebug.DrawWireCube(contactPoint.point, 0.01f, Color.red);
			}
		}
	}

	// Token: 0x060068F2 RID: 26866 RVA: 0x002742E8 File Offset: 0x002726E8
	private void OnCollisionStay(Collision c)
	{
		if (this.on)
		{
			foreach (ContactPoint contactPoint in c.contacts)
			{
				MyDebug.DrawWireCube(contactPoint.point, 0.01f, Color.red);
			}
		}
	}

	// Token: 0x060068F3 RID: 26867 RVA: 0x0027433E File Offset: 0x0027273E
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060068F4 RID: 26868 RVA: 0x0027434C File Offset: 0x0027274C
	private void Update()
	{
		this.isDetectingCollisions = this.rb.detectCollisions;
	}

	// Token: 0x040059BF RID: 22975
	public bool on;

	// Token: 0x040059C0 RID: 22976
	public bool isDetectingCollisions;

	// Token: 0x040059C1 RID: 22977
	private Rigidbody rb;
}
