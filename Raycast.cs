using System;
using UnityEngine;

// Token: 0x020002BA RID: 698
public class Raycast : MonoBehaviour
{
	// Token: 0x06001049 RID: 4169 RVA: 0x0005BCAA File Offset: 0x0005A0AA
	public Raycast()
	{
	}

	// Token: 0x0600104A RID: 4170 RVA: 0x0005BCB4 File Offset: 0x0005A0B4
	private void Update()
	{
		RaycastHit raycastHit;
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, this.range))
		{
			if (raycastHit.transform.tag == "Lever")
			{
				raycastHit.transform.gameObject.GetComponent<LeverControll>().turn();
			}
			else if (raycastHit.transform.tag == "Button")
			{
				raycastHit.transform.gameObject.GetComponent<ButtonControl>().turn();
			}
			else if (raycastHit.transform.tag == "Code")
			{
			}
		}
	}

	// Token: 0x04000E76 RID: 3702
	public float range;
}
