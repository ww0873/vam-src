using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000D57 RID: 3415
public class ConfigurableJointReconnector : ScaleChangeReceiver
{
	// Token: 0x060068F8 RID: 26872 RVA: 0x002743BE File Offset: 0x002727BE
	public ConfigurableJointReconnector()
	{
	}

	// Token: 0x060068F9 RID: 26873 RVA: 0x002743CE File Offset: 0x002727CE
	public override void ScaleChanged(float scale)
	{
		base.ScaleChanged(scale);
		if (base.isActiveAndEnabled)
		{
			this.Reconnect();
		}
	}

	// Token: 0x17000F7F RID: 3967
	// (get) Token: 0x060068FA RID: 26874 RVA: 0x002743E8 File Offset: 0x002727E8
	// (set) Token: 0x060068FB RID: 26875 RVA: 0x00274402 File Offset: 0x00272802
	private Vector3 relativePosition
	{
		get
		{
			if (this.controlRelativePositionAndRotation)
			{
				return this.controlledRelativePosition;
			}
			return this.startingRelativePosition;
		}
		set
		{
			if (this.controlRelativePositionAndRotation)
			{
				this.controlledRelativePosition = value;
			}
			else
			{
				this.startingRelativePosition = value;
			}
		}
	}

	// Token: 0x17000F80 RID: 3968
	// (get) Token: 0x060068FC RID: 26876 RVA: 0x00274422 File Offset: 0x00272822
	// (set) Token: 0x060068FD RID: 26877 RVA: 0x0027443C File Offset: 0x0027283C
	private Quaternion relativeRotation
	{
		get
		{
			if (this.controlRelativePositionAndRotation)
			{
				return this.controlledRelativeRotation;
			}
			return this.startingRelativeRotation;
		}
		set
		{
			if (this.controlRelativePositionAndRotation)
			{
				this.controlledRelativeRotation = value;
			}
			else
			{
				this.startingRelativeRotation = value;
			}
		}
	}

	// Token: 0x17000F81 RID: 3969
	// (get) Token: 0x060068FE RID: 26878 RVA: 0x0027445C File Offset: 0x0027285C
	// (set) Token: 0x060068FF RID: 26879 RVA: 0x00274464 File Offset: 0x00272864
	public Vector3 controlledRelativePosition
	{
		get
		{
			return this._controlledRelativePosition;
		}
		set
		{
			if (this._controlledRelativePosition != value)
			{
				this._controlledRelativePosition = value;
				if (base.gameObject.activeInHierarchy && base.enabled)
				{
					this.Reconnect();
				}
			}
		}
	}

	// Token: 0x17000F82 RID: 3970
	// (get) Token: 0x06006900 RID: 26880 RVA: 0x0027449F File Offset: 0x0027289F
	// (set) Token: 0x06006901 RID: 26881 RVA: 0x002744A7 File Offset: 0x002728A7
	public Quaternion controlledRelativeRotation
	{
		get
		{
			return this._controlledRelativeRotation;
		}
		set
		{
			if (this._controlledRelativeRotation != value)
			{
				this._controlledRelativeRotation = value;
				if (base.gameObject.activeInHierarchy && base.enabled)
				{
					this.Reconnect();
				}
			}
		}
	}

	// Token: 0x06006902 RID: 26882 RVA: 0x002744E4 File Offset: 0x002728E4
	private IEnumerator RepeatingReset()
	{
		for (int i = 0; i < this.numResetFrames; i++)
		{
			this.ResetToOrigin();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006903 RID: 26883 RVA: 0x00274500 File Offset: 0x00272900
	private void ResetToOrigin()
	{
		Rigidbody connectedBody = this.joint.connectedBody;
		if (connectedBody != null)
		{
			base.transform.position = connectedBody.transform.localToWorldMatrix.MultiplyPoint3x4(this.relativePosition);
			base.transform.rotation = connectedBody.transform.rotation * this.relativeRotation;
		}
	}

	// Token: 0x06006904 RID: 26884 RVA: 0x0027456C File Offset: 0x0027296C
	public void Reconnect()
	{
		if (!this.wasInit)
		{
			this.wasInit = true;
			this.joint = base.GetComponent<ConfigurableJoint>();
			if (this.joint != null)
			{
				if (this.createJointOnInit && this.rigidBodyToConnect != null)
				{
					this.joint.connectedBody = null;
					base.transform.position = this.rigidBodyToConnect.transform.position;
					base.transform.rotation = this.rigidBodyToConnect.transform.rotation;
					this.joint.connectedBody = this.rigidBodyToConnect;
					this.startingRelativePosition = Vector3.zero;
					this.startingRelativeRotation = Quaternion.identity;
				}
				else
				{
					Rigidbody connectedBody = this.joint.connectedBody;
					if (connectedBody != null)
					{
						this.startingRelativePosition = connectedBody.transform.worldToLocalMatrix.MultiplyPoint3x4(base.transform.position);
						this.startingRelativeRotation = Quaternion.Inverse(connectedBody.transform.rotation) * base.transform.rotation;
					}
				}
			}
		}
		if (this.joint != null)
		{
			Vector3 position = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			Rigidbody connectedBody2 = this.joint.connectedBody;
			if (connectedBody2 != null)
			{
				this.joint.connectedBody = null;
				if (connectedBody2 != null)
				{
					base.transform.position = connectedBody2.transform.localToWorldMatrix.MultiplyPoint3x4(this.relativePosition);
					base.transform.rotation = connectedBody2.transform.rotation * this.relativeRotation;
				}
				this.joint.connectedBody = connectedBody2;
				if (this.resetRelativePositionAndRotationOnEnable)
				{
					base.StopAllCoroutines();
					base.StartCoroutine(this.RepeatingReset());
				}
				else
				{
					base.transform.position = position;
					base.transform.rotation = rotation;
				}
			}
		}
	}

	// Token: 0x06006905 RID: 26885 RVA: 0x00274780 File Offset: 0x00272B80
	private void OnEnable()
	{
		this.Reconnect();
	}

	// Token: 0x06006906 RID: 26886 RVA: 0x00274788 File Offset: 0x00272B88
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06006907 RID: 26887 RVA: 0x00274790 File Offset: 0x00272B90
	private void Awake()
	{
		this.Reconnect();
	}

	// Token: 0x040059C3 RID: 22979
	public bool resetRelativePositionAndRotationOnEnable;

	// Token: 0x040059C4 RID: 22980
	public bool createJointOnInit;

	// Token: 0x040059C5 RID: 22981
	public Rigidbody rigidBodyToConnect;

	// Token: 0x040059C6 RID: 22982
	public int numResetFrames = 10;

	// Token: 0x040059C7 RID: 22983
	private bool wasInit;

	// Token: 0x040059C8 RID: 22984
	private ConfigurableJoint joint;

	// Token: 0x040059C9 RID: 22985
	private Vector3 startingRelativePosition;

	// Token: 0x040059CA RID: 22986
	private Quaternion startingRelativeRotation;

	// Token: 0x040059CB RID: 22987
	public bool controlRelativePositionAndRotation;

	// Token: 0x040059CC RID: 22988
	protected Vector3 _controlledRelativePosition;

	// Token: 0x040059CD RID: 22989
	protected Quaternion _controlledRelativeRotation;

	// Token: 0x02001036 RID: 4150
	[CompilerGenerated]
	private sealed class <RepeatingReset>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007771 RID: 30577 RVA: 0x00274798 File Offset: 0x00272B98
		[DebuggerHidden]
		public <RepeatingReset>c__Iterator0()
		{
		}

		// Token: 0x06007772 RID: 30578 RVA: 0x002747A0 File Offset: 0x00272BA0
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				i = 0;
				break;
			case 1U:
				i++;
				break;
			default:
				return false;
			}
			if (i < this.numResetFrames)
			{
				base.ResetToOrigin();
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			this.$PC = -1;
			return false;
		}

		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x06007773 RID: 30579 RVA: 0x0027482E File Offset: 0x00272C2E
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x06007774 RID: 30580 RVA: 0x00274836 File Offset: 0x00272C36
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007775 RID: 30581 RVA: 0x0027483E File Offset: 0x00272C3E
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007776 RID: 30582 RVA: 0x0027484E File Offset: 0x00272C4E
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006B79 RID: 27513
		internal int <i>__1;

		// Token: 0x04006B7A RID: 27514
		internal ConfigurableJointReconnector $this;

		// Token: 0x04006B7B RID: 27515
		internal object $current;

		// Token: 0x04006B7C RID: 27516
		internal bool $disposing;

		// Token: 0x04006B7D RID: 27517
		internal int $PC;
	}
}
