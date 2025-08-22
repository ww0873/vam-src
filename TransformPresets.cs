using System;
using UnityEngine;

// Token: 0x02000E1A RID: 3610
public class TransformPresets : MonoBehaviour
{
	// Token: 0x06006F2A RID: 28458 RVA: 0x0029BCA8 File Offset: 0x0029A0A8
	public TransformPresets()
	{
	}

	// Token: 0x1700103F RID: 4159
	// (get) Token: 0x06006F2B RID: 28459 RVA: 0x0029BCB0 File Offset: 0x0029A0B0
	public Vector3[] positions
	{
		get
		{
			return this._positions;
		}
	}

	// Token: 0x17001040 RID: 4160
	// (get) Token: 0x06006F2C RID: 28460 RVA: 0x0029BCB8 File Offset: 0x0029A0B8
	public Quaternion[] rotations
	{
		get
		{
			return this._rotations;
		}
	}

	// Token: 0x17001041 RID: 4161
	// (get) Token: 0x06006F2D RID: 28461 RVA: 0x0029BCC0 File Offset: 0x0029A0C0
	public string[] labels
	{
		get
		{
			return this._labels;
		}
	}

	// Token: 0x17001042 RID: 4162
	// (get) Token: 0x06006F2E RID: 28462 RVA: 0x0029BCC8 File Offset: 0x0029A0C8
	// (set) Token: 0x06006F2F RID: 28463 RVA: 0x0029BCD0 File Offset: 0x0029A0D0
	public int numPresets
	{
		get
		{
			return this._numPresets;
		}
		set
		{
			if (this._numPresets != value && value > 0)
			{
				int numPresets = this._numPresets;
				this._numPresets = value;
				Vector3[] array = new Vector3[this._numPresets];
				Quaternion[] array2 = new Quaternion[this._numPresets];
				string[] array3 = new string[this._numPresets];
				for (int i = 0; i < this._numPresets; i++)
				{
					array2[i].x = 0f;
					array2[i].y = 0f;
					array2[i].z = 0f;
					array2[i].w = 1f;
				}
				for (int j = 0; j < numPresets; j++)
				{
					if (j == this._numPresets)
					{
						break;
					}
					array[j] = this._positions[j];
					array2[j] = this._rotations[j];
					array3[j] = this._labels[j];
				}
				this._positions = array;
				this._rotations = array2;
				this._labels = array3;
			}
		}
	}

	// Token: 0x06006F30 RID: 28464 RVA: 0x0029BE10 File Offset: 0x0029A210
	public void SetPresetFromTransform(int presetNum)
	{
		if (presetNum < this._numPresets && presetNum >= 0)
		{
			this._positions[presetNum] = base.transform.position;
			this._rotations[presetNum] = base.transform.rotation;
		}
	}

	// Token: 0x06006F31 RID: 28465 RVA: 0x0029BE68 File Offset: 0x0029A268
	public void SetTransformFromPreset(int presetNum)
	{
		if (presetNum < this._numPresets && presetNum >= 0)
		{
			base.transform.position = this._positions[presetNum];
			base.transform.rotation = this._rotations[presetNum];
		}
	}

	// Token: 0x0400605F RID: 24671
	[SerializeField]
	private Vector3[] _positions;

	// Token: 0x04006060 RID: 24672
	[SerializeField]
	private Quaternion[] _rotations;

	// Token: 0x04006061 RID: 24673
	[SerializeField]
	private string[] _labels;

	// Token: 0x04006062 RID: 24674
	[SerializeField]
	private int _numPresets;
}
