using System;
using UnityEngine;

// Token: 0x02000BCF RID: 3023
public class DebugMatrix4x4 : MonoBehaviour
{
	// Token: 0x060055E0 RID: 21984 RVA: 0x001F65B4 File Offset: 0x001F49B4
	public DebugMatrix4x4()
	{
	}

	// Token: 0x060055E1 RID: 21985 RVA: 0x001F65BC File Offset: 0x001F49BC
	private void Start()
	{
	}

	// Token: 0x060055E2 RID: 21986 RVA: 0x001F65C0 File Offset: 0x001F49C0
	private void Update()
	{
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		string text = string.Empty;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				text = text + localToWorldMatrix[i, j].ToString("F4") + ":";
			}
			text += "\n";
		}
		Debug.Log("Matrix is\n" + text);
		Debug.Log("Quaternion is \n" + base.transform.localRotation.ToString("F2"));
	}
}
