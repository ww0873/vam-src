using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000D7D RID: 3453
public class InterpolateRigidbodies : MonoBehaviour
{
	// Token: 0x06006A50 RID: 27216 RVA: 0x002812FC File Offset: 0x0027F6FC
	public InterpolateRigidbodies()
	{
	}

	// Token: 0x17000F9D RID: 3997
	// (get) Token: 0x06006A51 RID: 27217 RVA: 0x00281304 File Offset: 0x0027F704
	// (set) Token: 0x06006A52 RID: 27218 RVA: 0x0028130C File Offset: 0x0027F70C
	public bool on
	{
		get
		{
			return this._on;
		}
		set
		{
			if (this._on != value)
			{
				if (value)
				{
					if (Application.isPlaying)
					{
						this._on = value;
						this.WalkAndSetInterpolate(base.transform);
					}
					else
					{
						Debug.LogWarning("Interpolation on rigidbodies should only be turned on at runtime to prevent major joint issues");
					}
				}
				else
				{
					this._on = value;
					this.WalkAndSetInterpolate(base.transform);
				}
			}
		}
	}

	// Token: 0x06006A53 RID: 27219 RVA: 0x00281370 File Offset: 0x0027F770
	private void WalkAndSetInterpolate(Transform t)
	{
		Rigidbody component = t.GetComponent<Rigidbody>();
		if (component != null)
		{
			if (this._on)
			{
				if (!component.isKinematic)
				{
					component.interpolation = RigidbodyInterpolation.Interpolate;
				}
			}
			else
			{
				component.interpolation = RigidbodyInterpolation.None;
			}
		}
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform t2 = (Transform)obj;
				this.WalkAndSetInterpolate(t2);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06006A54 RID: 27220 RVA: 0x0028140C File Offset: 0x0027F80C
	private void Start()
	{
		if (this.setOnStart)
		{
			this.on = true;
		}
	}

	// Token: 0x04005C5F RID: 23647
	[SerializeField]
	private bool _on;

	// Token: 0x04005C60 RID: 23648
	public bool setOnStart;
}
