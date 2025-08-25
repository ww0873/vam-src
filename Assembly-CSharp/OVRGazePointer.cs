using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200095E RID: 2398
public class OVRGazePointer : MonoBehaviour
{
	// Token: 0x06003B83 RID: 15235 RVA: 0x0011F640 File Offset: 0x0011DA40
	public OVRGazePointer()
	{
	}

	// Token: 0x17000692 RID: 1682
	// (get) Token: 0x06003B84 RID: 15236 RVA: 0x0011F68D File Offset: 0x0011DA8D
	// (set) Token: 0x06003B85 RID: 15237 RVA: 0x0011F695 File Offset: 0x0011DA95
	public bool hidden
	{
		[CompilerGenerated]
		get
		{
			return this.<hidden>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<hidden>k__BackingField = value;
		}
	}

	// Token: 0x17000693 RID: 1683
	// (get) Token: 0x06003B86 RID: 15238 RVA: 0x0011F69E File Offset: 0x0011DA9E
	// (set) Token: 0x06003B87 RID: 15239 RVA: 0x0011F6A6 File Offset: 0x0011DAA6
	public float currentScale
	{
		[CompilerGenerated]
		get
		{
			return this.<currentScale>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<currentScale>k__BackingField = value;
		}
	}

	// Token: 0x17000694 RID: 1684
	// (get) Token: 0x06003B89 RID: 15241 RVA: 0x0011F6B8 File Offset: 0x0011DAB8
	// (set) Token: 0x06003B88 RID: 15240 RVA: 0x0011F6AF File Offset: 0x0011DAAF
	public Vector3 positionDelta
	{
		[CompilerGenerated]
		get
		{
			return this.<positionDelta>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<positionDelta>k__BackingField = value;
		}
	}

	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x06003B8A RID: 15242 RVA: 0x0011F6C0 File Offset: 0x0011DAC0
	public static OVRGazePointer instance
	{
		get
		{
			if (OVRGazePointer._instance == null)
			{
				UnityEngine.Debug.Log(string.Format("Instanciating GazePointer", 0));
				OVRGazePointer._instance = UnityEngine.Object.Instantiate<OVRGazePointer>((OVRGazePointer)Resources.Load("Prefabs/GazePointerRing", typeof(OVRGazePointer)));
			}
			return OVRGazePointer._instance;
		}
	}

	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x06003B8B RID: 15243 RVA: 0x0011F71C File Offset: 0x0011DB1C
	public float visibilityStrength
	{
		get
		{
			float a;
			if (this.hideByDefault)
			{
				a = Mathf.Clamp01(1f - (Time.time - this.lastShowRequestTime) / this.showTimeoutPeriod);
			}
			else
			{
				a = 1f;
			}
			float b = (this.lastHideRequestTime + this.hideTimeoutPeriod <= Time.time) ? 1f : ((!this.dimOnHideRequest) ? 0f : 0.1f);
			return Mathf.Min(a, b);
		}
	}

	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x06003B8C RID: 15244 RVA: 0x0011F7A1 File Offset: 0x0011DBA1
	// (set) Token: 0x06003B8D RID: 15245 RVA: 0x0011F7C8 File Offset: 0x0011DBC8
	public float SelectionProgress
	{
		get
		{
			return (!this.progressIndicator) ? 0f : this.progressIndicator.currentProgress;
		}
		set
		{
			if (this.progressIndicator)
			{
				this.progressIndicator.currentProgress = value;
			}
		}
	}

	// Token: 0x06003B8E RID: 15246 RVA: 0x0011F7E8 File Offset: 0x0011DBE8
	public void Awake()
	{
		this.currentScale = 1f;
		if (OVRGazePointer._instance != null && OVRGazePointer._instance != this)
		{
			base.enabled = false;
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		OVRGazePointer._instance = this;
		this.trailFollower = base.transform.Find("TrailFollower");
		this.progressIndicator = base.transform.GetComponent<OVRProgressIndicator>();
	}

	// Token: 0x06003B8F RID: 15247 RVA: 0x0011F85C File Offset: 0x0011DC5C
	private void Update()
	{
		if (this.rayTransform == null && Camera.main != null)
		{
			this.rayTransform = Camera.main.transform;
		}
		base.transform.position = this.rayTransform.position + this.rayTransform.forward * this.depth;
		if (this.visibilityStrength == 0f && !this.hidden)
		{
			this.Hide();
		}
		else if (this.visibilityStrength > 0f && this.hidden)
		{
			this.Show();
		}
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x0011F914 File Offset: 0x0011DD14
	public void SetPosition(Vector3 pos, Vector3 normal)
	{
		base.transform.position = pos;
		Quaternion rotation = base.transform.rotation;
		rotation.SetLookRotation(normal, this.rayTransform.up);
		base.transform.rotation = rotation;
		this.depth = (this.rayTransform.position - pos).magnitude;
		this.currentScale = this.depth * this.depthScaleMultiplier;
		base.transform.localScale = new Vector3(this.currentScale, this.currentScale, this.currentScale);
		this.positionSetsThisFrame++;
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x0011F9BA File Offset: 0x0011DDBA
	public void SetPosition(Vector3 pos)
	{
		this.SetPosition(pos, this.rayTransform.forward);
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x0011F9CE File Offset: 0x0011DDCE
	public float GetCurrentRadius()
	{
		return this.cursorRadius * this.currentScale;
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x0011F9E0 File Offset: 0x0011DDE0
	private void LateUpdate()
	{
		if (this.positionSetsThisFrame == 0)
		{
			Quaternion rotation = base.transform.rotation;
			rotation.SetLookRotation(this.rayTransform.forward, this.rayTransform.up);
			base.transform.rotation = rotation;
		}
		Quaternion rotation2 = this.trailFollower.rotation;
		rotation2.SetLookRotation(base.transform.rotation * new Vector3(0f, 0f, 1f), (this.lastPosition - base.transform.position).normalized);
		this.trailFollower.rotation = rotation2;
		this.positionDelta = base.transform.position - this.lastPosition;
		this.lastPosition = base.transform.position;
		this.positionSetsThisFrame = 0;
	}

	// Token: 0x06003B94 RID: 15252 RVA: 0x0011FAC2 File Offset: 0x0011DEC2
	public void RequestHide()
	{
		if (!this.dimOnHideRequest)
		{
			this.Hide();
		}
		this.lastHideRequestTime = Time.time;
	}

	// Token: 0x06003B95 RID: 15253 RVA: 0x0011FAE0 File Offset: 0x0011DEE0
	public void RequestShow()
	{
		this.Show();
		this.lastShowRequestTime = Time.time;
	}

	// Token: 0x06003B96 RID: 15254 RVA: 0x0011FAF4 File Offset: 0x0011DEF4
	private void Hide()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(false);
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
		if (base.GetComponent<Renderer>())
		{
			base.GetComponent<Renderer>().enabled = false;
		}
		this.hidden = true;
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x0011FB84 File Offset: 0x0011DF84
	private void Show()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(true);
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
		if (base.GetComponent<Renderer>())
		{
			base.GetComponent<Renderer>().enabled = true;
		}
		this.hidden = false;
	}

	// Token: 0x04002D73 RID: 11635
	private Transform trailFollower;

	// Token: 0x04002D74 RID: 11636
	[Tooltip("Should the pointer be hidden when not over interactive objects.")]
	public bool hideByDefault = true;

	// Token: 0x04002D75 RID: 11637
	[Tooltip("Time after leaving interactive object before pointer fades.")]
	public float showTimeoutPeriod = 1f;

	// Token: 0x04002D76 RID: 11638
	[Tooltip("Time after mouse pointer becoming inactive before pointer unfades.")]
	public float hideTimeoutPeriod = 0.1f;

	// Token: 0x04002D77 RID: 11639
	[Tooltip("Keep a faint version of the pointer visible while using a mouse")]
	public bool dimOnHideRequest = true;

	// Token: 0x04002D78 RID: 11640
	[Tooltip("Angular scale of pointer")]
	public float depthScaleMultiplier = 0.03f;

	// Token: 0x04002D79 RID: 11641
	public Transform rayTransform;

	// Token: 0x04002D7A RID: 11642
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <hidden>k__BackingField;

	// Token: 0x04002D7B RID: 11643
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private float <currentScale>k__BackingField;

	// Token: 0x04002D7C RID: 11644
	private float depth;

	// Token: 0x04002D7D RID: 11645
	private float hideUntilTime;

	// Token: 0x04002D7E RID: 11646
	private int positionSetsThisFrame;

	// Token: 0x04002D7F RID: 11647
	private Vector3 lastPosition;

	// Token: 0x04002D80 RID: 11648
	private float lastShowRequestTime;

	// Token: 0x04002D81 RID: 11649
	private float lastHideRequestTime;

	// Token: 0x04002D82 RID: 11650
	[Tooltip("Radius of the cursor. Used for preventing geometry intersections.")]
	public float cursorRadius = 1f;

	// Token: 0x04002D83 RID: 11651
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Vector3 <positionDelta>k__BackingField;

	// Token: 0x04002D84 RID: 11652
	private OVRProgressIndicator progressIndicator;

	// Token: 0x04002D85 RID: 11653
	private static OVRGazePointer _instance;
}
