using System;
using UnityEngine;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002CC RID: 716
	public class TankShell : MonoBehaviour
	{
		// Token: 0x0600108E RID: 4238 RVA: 0x0005D293 File Offset: 0x0005B693
		public TankShell()
		{
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0005D2BC File Offset: 0x0005B6BC
		public bool Step()
		{
			if (this.hit)
			{
				return true;
			}
			if (Vector2.Distance(this.startPosition, base.transform.position) > 20f)
			{
				this.hit = true;
			}
			base.transform.Translate(this.heading * (this.speed * Time.deltaTime));
			return false;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0005D32A File Offset: 0x0005B72A
		public void Destroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0005D338 File Offset: 0x0005B738
		public void OnCollisionEnter2D(Collision2D collision)
		{
			Collider2D collider = collision.collider;
			if (collider.name == "DamagedWall")
			{
				UnityEngine.Object.Destroy(collider.gameObject);
				this.hit = true;
			}
			else if (collider.name == "Wall")
			{
				this.hit = true;
			}
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0005D394 File Offset: 0x0005B794
		public static TankShell Shoot(GameObject prefab, Vector2 startPosition, Vector2 heading)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, startPosition, Quaternion.identity);
			TankShell component = gameObject.GetComponent<TankShell>();
			if (component == null)
			{
				UnityEngine.Object.Destroy(gameObject);
				return null;
			}
			component.startPosition = startPosition;
			component.heading = heading;
			return component;
		}

		// Token: 0x04000EBE RID: 3774
		private Vector2 startPosition = Vector2.zero;

		// Token: 0x04000EBF RID: 3775
		private Vector2 heading = Vector2.zero;

		// Token: 0x04000EC0 RID: 3776
		private bool hit;

		// Token: 0x04000EC1 RID: 3777
		public float speed = 2f;
	}
}
