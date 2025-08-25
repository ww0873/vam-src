using System;
using UnityEngine;

// Token: 0x0200075A RID: 1882
public class TestScript : MonoBehaviour
{
	// Token: 0x06003072 RID: 12402 RVA: 0x000FBF35 File Offset: 0x000FA335
	public TestScript()
	{
	}

	// Token: 0x06003073 RID: 12403 RVA: 0x000FBF3D File Offset: 0x000FA33D
	private void Start()
	{
	}

	// Token: 0x06003074 RID: 12404 RVA: 0x000FBF40 File Offset: 0x000FA340
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.testSound1.PlaySoundAt(base.transform.position, 0f, 1f, 1f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.testSound2.PlaySoundAt(new Vector3(5f, 0f, 0f), 0f, 1f, 1f);
		}
	}

	// Token: 0x0400245B RID: 9307
	[InspectorNote("Sound Setup", "Press '1' to play testSound1 and '2' to play testSound2")]
	public SoundFXRef testSound1;

	// Token: 0x0400245C RID: 9308
	public SoundFXRef testSound2;
}
