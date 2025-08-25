using System;
using UnityEngine;

// Token: 0x02000D86 RID: 3462
public class ReportCenterOfMass : MonoBehaviour
{
	// Token: 0x06006AB5 RID: 27317 RVA: 0x00282F61 File Offset: 0x00281361
	public ReportCenterOfMass()
	{
	}

	// Token: 0x06006AB6 RID: 27318 RVA: 0x00282F69 File Offset: 0x00281369
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06006AB7 RID: 27319 RVA: 0x00282F78 File Offset: 0x00281378
	private void Update()
	{
		if (this.report && this.rb)
		{
			Debug.Log(base.name + " center of mass: " + this.rb.centerOfMass.ToString("F3"));
		}
	}

	// Token: 0x04005C97 RID: 23703
	public bool report;

	// Token: 0x04005C98 RID: 23704
	private Rigidbody rb;
}
