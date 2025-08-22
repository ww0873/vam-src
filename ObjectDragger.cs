using System;
using UnityEngine;

// Token: 0x02000388 RID: 904
public class ObjectDragger : MonoBehaviour
{
	// Token: 0x06001688 RID: 5768 RVA: 0x0007EA28 File Offset: 0x0007CE28
	public ObjectDragger()
	{
	}

	// Token: 0x06001689 RID: 5769 RVA: 0x0007EA30 File Offset: 0x0007CE30
	private void Awake()
	{
		this.body = base.gameObject.GetComponent<Rigidbody>();
		this.newPosition = base.transform.position;
	}

	// Token: 0x0600168A RID: 5770 RVA: 0x0007EA54 File Offset: 0x0007CE54
	private void OnMouseDown()
	{
		this.screenPoint = Camera.main.WorldToScreenPoint(base.gameObject.transform.position);
		this.offset = base.gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.screenPoint.z));
	}

	// Token: 0x0600168B RID: 5771 RVA: 0x0007EAD0 File Offset: 0x0007CED0
	private void OnMouseDrag()
	{
		this.dragged = true;
	}

	// Token: 0x0600168C RID: 5772 RVA: 0x0007EADC File Offset: 0x0007CEDC
	private void FixedUpdate()
	{
		if (this.dragged)
		{
			this.dragged = false;
			Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.screenPoint.z);
			this.newPosition = Camera.main.ScreenToWorldPoint(position) + this.offset;
			if (this.body != null)
			{
				this.body.velocity = (this.newPosition - base.transform.position) / Time.deltaTime;
			}
		}
	}

	// Token: 0x0600168D RID: 5773 RVA: 0x0007EB7F File Offset: 0x0007CF7F
	private void LateUpdate()
	{
		base.transform.position = this.newPosition;
	}

	// Token: 0x040012AC RID: 4780
	private Vector3 screenPoint;

	// Token: 0x040012AD RID: 4781
	private Vector3 offset;

	// Token: 0x040012AE RID: 4782
	private bool dragged;

	// Token: 0x040012AF RID: 4783
	private Vector3 newPosition;

	// Token: 0x040012B0 RID: 4784
	private Rigidbody body;
}
