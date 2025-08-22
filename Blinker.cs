using System;
using UnityEngine;

// Token: 0x0200036B RID: 875
public class Blinker : MonoBehaviour
{
	// Token: 0x060015D7 RID: 5591 RVA: 0x0007CDB3 File Offset: 0x0007B1B3
	public Blinker()
	{
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x0007CDBB File Offset: 0x0007B1BB
	private void Awake()
	{
		this.rend = base.GetComponent<Renderer>();
		this.original = this.rend.material.color;
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x0007CDDF File Offset: 0x0007B1DF
	public void Blink()
	{
		this.rend.material.color = this.highlightColor;
	}

	// Token: 0x060015DA RID: 5594 RVA: 0x0007CDF8 File Offset: 0x0007B1F8
	private void LateUpdate()
	{
		this.rend.material.color += (this.original - this.rend.material.color) * Time.deltaTime * 5f;
	}

	// Token: 0x04001244 RID: 4676
	public Color highlightColor;

	// Token: 0x04001245 RID: 4677
	private Renderer rend;

	// Token: 0x04001246 RID: 4678
	private Color original;
}
