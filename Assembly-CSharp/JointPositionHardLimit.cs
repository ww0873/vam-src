using System;
using UnityEngine;

// Token: 0x02000D7E RID: 3454
public class JointPositionHardLimit : MonoBehaviour
{
	// Token: 0x06006A55 RID: 27221 RVA: 0x00281420 File Offset: 0x0027F820
	public JointPositionHardLimit()
	{
	}

	// Token: 0x06006A56 RID: 27222 RVA: 0x00281438 File Offset: 0x0027F838
	public void SetTargetPositionFromPercent()
	{
		if (this._percent < 0f)
		{
			this._currentTargetPosition = Vector3.Lerp(this._zeroTargetPosition, this._lowTargetPosition, -this._percent);
		}
		else
		{
			this._currentTargetPosition = Vector3.Lerp(this._zeroTargetPosition, this._highTargetPosition, this._percent);
		}
		if (this.cj != null && this.useOffsetPosition)
		{
			this.cj.connectedAnchor = this.startAnchor + this._currentTargetPosition;
			this._currentAnchor = this.cj.connectedAnchor;
		}
	}

	// Token: 0x17000F9E RID: 3998
	// (get) Token: 0x06006A57 RID: 27223 RVA: 0x002814DE File Offset: 0x0027F8DE
	// (set) Token: 0x06006A58 RID: 27224 RVA: 0x002814E6 File Offset: 0x0027F8E6
	public float percent
	{
		get
		{
			return this._percent;
		}
		set
		{
			if (this._percent != value)
			{
				this._percent = value;
				this.SetTargetPositionFromPercent();
			}
		}
	}

	// Token: 0x17000F9F RID: 3999
	// (get) Token: 0x06006A59 RID: 27225 RVA: 0x00281501 File Offset: 0x0027F901
	// (set) Token: 0x06006A5A RID: 27226 RVA: 0x00281509 File Offset: 0x0027F909
	public Vector3 lowTargetPosition
	{
		get
		{
			return this._lowTargetPosition;
		}
		set
		{
			if (this._lowTargetPosition != value)
			{
				this._lowTargetPosition = value;
				this.SetTargetPositionFromPercent();
			}
		}
	}

	// Token: 0x17000FA0 RID: 4000
	// (get) Token: 0x06006A5B RID: 27227 RVA: 0x00281529 File Offset: 0x0027F929
	// (set) Token: 0x06006A5C RID: 27228 RVA: 0x00281531 File Offset: 0x0027F931
	public Vector3 zeroTargetPosition
	{
		get
		{
			return this._zeroTargetPosition;
		}
		set
		{
			if (this._zeroTargetPosition != value)
			{
				this._zeroTargetPosition = value;
				this.SetTargetPositionFromPercent();
			}
		}
	}

	// Token: 0x17000FA1 RID: 4001
	// (get) Token: 0x06006A5D RID: 27229 RVA: 0x00281551 File Offset: 0x0027F951
	// (set) Token: 0x06006A5E RID: 27230 RVA: 0x00281559 File Offset: 0x0027F959
	public Vector3 highTargetPosition
	{
		get
		{
			return this._highTargetPosition;
		}
		set
		{
			if (this._highTargetPosition != value)
			{
				this._highTargetPosition = value;
				this.SetTargetPositionFromPercent();
			}
		}
	}

	// Token: 0x06006A5F RID: 27231 RVA: 0x0028157C File Offset: 0x0027F97C
	private void Start()
	{
		this.cj = base.GetComponent<ConfigurableJoint>();
		if (this.cj != null)
		{
			this.startAnchor = this.cj.connectedAnchor;
			this.cj.autoConfigureConnectedAnchor = false;
			this.startRotation = base.transform.localRotation;
		}
		this.SetTargetPositionFromPercent();
		this.DoUpdate();
	}

	// Token: 0x06006A60 RID: 27232 RVA: 0x002815E0 File Offset: 0x0027F9E0
	private void DoUpdate()
	{
		if (this.cj != null)
		{
			base.transform.localPosition = this.cj.connectedAnchor;
		}
	}

	// Token: 0x06006A61 RID: 27233 RVA: 0x00281609 File Offset: 0x0027FA09
	private void Update()
	{
		if (this.updateOn)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x06006A62 RID: 27234 RVA: 0x0028161C File Offset: 0x0027FA1C
	private void LateUpdate()
	{
		if (this.lateUpdateOn)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x06006A63 RID: 27235 RVA: 0x0028162F File Offset: 0x0027FA2F
	private void FixedUpdate()
	{
		if (this.fixedUpdateOn)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x04005C61 RID: 23649
	public bool lateUpdateOn;

	// Token: 0x04005C62 RID: 23650
	public bool updateOn = true;

	// Token: 0x04005C63 RID: 23651
	public bool fixedUpdateOn = true;

	// Token: 0x04005C64 RID: 23652
	public bool useOffsetPosition;

	// Token: 0x04005C65 RID: 23653
	private ConfigurableJoint cj;

	// Token: 0x04005C66 RID: 23654
	public Vector3 _currentAnchor;

	// Token: 0x04005C67 RID: 23655
	public Vector3 _currentTargetPosition;

	// Token: 0x04005C68 RID: 23656
	[SerializeField]
	private float _percent;

	// Token: 0x04005C69 RID: 23657
	[SerializeField]
	private Vector3 _lowTargetPosition;

	// Token: 0x04005C6A RID: 23658
	[SerializeField]
	private Vector3 _zeroTargetPosition;

	// Token: 0x04005C6B RID: 23659
	[SerializeField]
	private Vector3 _highTargetPosition;

	// Token: 0x04005C6C RID: 23660
	public Vector3 startAnchor;

	// Token: 0x04005C6D RID: 23661
	public Quaternion startRotation;
}
