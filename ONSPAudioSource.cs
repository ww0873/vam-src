using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000980 RID: 2432
public class ONSPAudioSource : MonoBehaviour
{
	// Token: 0x06003CAD RID: 15533 RVA: 0x001262B2 File Offset: 0x001246B2
	public ONSPAudioSource()
	{
	}

	// Token: 0x06003CAE RID: 15534
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern void ONSP_GetGlobalRoomReflectionValues(ref bool reflOn, ref bool reverbOn, ref float width, ref float height, ref float length);

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x06003CAF RID: 15535 RVA: 0x001262D7 File Offset: 0x001246D7
	// (set) Token: 0x06003CB0 RID: 15536 RVA: 0x001262DF File Offset: 0x001246DF
	public bool EnableSpatialization
	{
		get
		{
			return this.enableSpatialization;
		}
		set
		{
			this.enableSpatialization = value;
		}
	}

	// Token: 0x170006B5 RID: 1717
	// (get) Token: 0x06003CB1 RID: 15537 RVA: 0x001262E8 File Offset: 0x001246E8
	// (set) Token: 0x06003CB2 RID: 15538 RVA: 0x001262F0 File Offset: 0x001246F0
	public float Gain
	{
		get
		{
			return this.gain;
		}
		set
		{
			this.gain = Mathf.Clamp(value, 0f, 24f);
		}
	}

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x06003CB3 RID: 15539 RVA: 0x00126308 File Offset: 0x00124708
	// (set) Token: 0x06003CB4 RID: 15540 RVA: 0x00126310 File Offset: 0x00124710
	public bool UseInvSqr
	{
		get
		{
			return this.useInvSqr;
		}
		set
		{
			this.useInvSqr = value;
		}
	}

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x06003CB5 RID: 15541 RVA: 0x00126319 File Offset: 0x00124719
	// (set) Token: 0x06003CB6 RID: 15542 RVA: 0x00126321 File Offset: 0x00124721
	public float Near
	{
		get
		{
			return this.near;
		}
		set
		{
			this.near = Mathf.Clamp(value, 0f, 1000000f);
		}
	}

	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x00126339 File Offset: 0x00124739
	// (set) Token: 0x06003CB8 RID: 15544 RVA: 0x00126341 File Offset: 0x00124741
	public float Far
	{
		get
		{
			return this.far;
		}
		set
		{
			this.far = Mathf.Clamp(value, 0f, 1000000f);
		}
	}

	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x06003CB9 RID: 15545 RVA: 0x00126359 File Offset: 0x00124759
	// (set) Token: 0x06003CBA RID: 15546 RVA: 0x00126361 File Offset: 0x00124761
	public float VolumetricRadius
	{
		get
		{
			return this.volumetricRadius;
		}
		set
		{
			this.volumetricRadius = Mathf.Clamp(value, 0f, 1000f);
		}
	}

	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x06003CBB RID: 15547 RVA: 0x00126379 File Offset: 0x00124779
	// (set) Token: 0x06003CBC RID: 15548 RVA: 0x00126381 File Offset: 0x00124781
	public bool EnableRfl
	{
		get
		{
			return this.enableRfl;
		}
		set
		{
			this.enableRfl = value;
		}
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x0012638C File Offset: 0x0012478C
	private void Awake()
	{
		AudioSource component = base.GetComponent<AudioSource>();
		this.SetParameters(ref component);
	}

	// Token: 0x06003CBE RID: 15550 RVA: 0x001263A8 File Offset: 0x001247A8
	private void Start()
	{
	}

	// Token: 0x06003CBF RID: 15551 RVA: 0x001263AC File Offset: 0x001247AC
	private void Update()
	{
		AudioSource component = base.GetComponent<AudioSource>();
		if (!Application.isPlaying || AudioListener.pause || !component.isPlaying || !component.isActiveAndEnabled)
		{
			component.spatialize = false;
			return;
		}
		this.SetParameters(ref component);
	}

	// Token: 0x06003CC0 RID: 15552 RVA: 0x001263FC File Offset: 0x001247FC
	public void SetParameters(ref AudioSource source)
	{
		source.spatialize = this.enableSpatialization;
		source.SetSpatializerFloat(0, this.gain);
		if (this.useInvSqr)
		{
			source.SetSpatializerFloat(1, 1f);
		}
		else
		{
			source.SetSpatializerFloat(1, 0f);
		}
		source.SetSpatializerFloat(2, this.near);
		source.SetSpatializerFloat(3, this.far);
		source.SetSpatializerFloat(4, this.volumetricRadius);
		if (this.enableRfl)
		{
			source.SetSpatializerFloat(5, 0f);
		}
		else
		{
			source.SetSpatializerFloat(5, 1f);
		}
	}

	// Token: 0x06003CC1 RID: 15553 RVA: 0x001264AC File Offset: 0x001248AC
	private void OnDrawGizmos()
	{
		if (ONSPAudioSource.RoomReflectionGizmoAS == null)
		{
			ONSPAudioSource.RoomReflectionGizmoAS = this;
		}
		Color color;
		color.r = 1f;
		color.g = 0.5f;
		color.b = 0f;
		color.a = 1f;
		Gizmos.color = color;
		Gizmos.DrawWireSphere(base.transform.position, this.Near);
		color.a = 0.1f;
		Gizmos.color = color;
		Gizmos.DrawSphere(base.transform.position, this.Near);
		color.r = 1f;
		color.g = 0f;
		color.b = 0f;
		color.a = 1f;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, this.Far);
		color.a = 0.1f;
		Gizmos.color = color;
		Gizmos.DrawSphere(base.transform.position, this.Far);
		color.r = 1f;
		color.g = 0f;
		color.b = 1f;
		color.a = 1f;
		Gizmos.color = color;
		Gizmos.DrawWireSphere(base.transform.position, this.VolumetricRadius);
		color.a = 0.1f;
		Gizmos.color = color;
		Gizmos.DrawSphere(base.transform.position, this.VolumetricRadius);
		if (ONSPAudioSource.RoomReflectionGizmoAS == this)
		{
			bool flag = false;
			bool flag2 = false;
			float x = 1f;
			float y = 1f;
			float z = 1f;
			ONSPAudioSource.ONSP_GetGlobalRoomReflectionValues(ref flag, ref flag2, ref x, ref y, ref z);
			if (Camera.main != null && flag)
			{
				if (flag2)
				{
					color = Color.white;
				}
				else
				{
					color = Color.cyan;
				}
				Gizmos.color = color;
				Gizmos.DrawWireCube(Camera.main.transform.position, new Vector3(x, y, z));
				color.a = 0.1f;
				Gizmos.color = color;
				Gizmos.DrawCube(Camera.main.transform.position, new Vector3(x, y, z));
			}
		}
	}

	// Token: 0x06003CC2 RID: 15554 RVA: 0x001266E7 File Offset: 0x00124AE7
	private void OnDestroy()
	{
		if (ONSPAudioSource.RoomReflectionGizmoAS == this)
		{
			ONSPAudioSource.RoomReflectionGizmoAS = null;
		}
	}

	// Token: 0x06003CC3 RID: 15555 RVA: 0x001266FF File Offset: 0x00124AFF
	// Note: this type is marked as 'beforefieldinit'.
	static ONSPAudioSource()
	{
	}

	// Token: 0x04002E93 RID: 11923
	public const string strONSPS = "AudioPluginOculusSpatializer";

	// Token: 0x04002E94 RID: 11924
	[SerializeField]
	private bool enableSpatialization = true;

	// Token: 0x04002E95 RID: 11925
	[SerializeField]
	private float gain;

	// Token: 0x04002E96 RID: 11926
	[SerializeField]
	private bool useInvSqr;

	// Token: 0x04002E97 RID: 11927
	[SerializeField]
	private float near = 1f;

	// Token: 0x04002E98 RID: 11928
	[SerializeField]
	private float far = 10f;

	// Token: 0x04002E99 RID: 11929
	[SerializeField]
	private float volumetricRadius;

	// Token: 0x04002E9A RID: 11930
	[SerializeField]
	private bool enableRfl;

	// Token: 0x04002E9B RID: 11931
	private static ONSPAudioSource RoomReflectionGizmoAS;
}
