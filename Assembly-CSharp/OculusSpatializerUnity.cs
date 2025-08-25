using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x0200097C RID: 2428
public class OculusSpatializerUnity : MonoBehaviour
{
	// Token: 0x06003C91 RID: 15505 RVA: 0x001255A8 File Offset: 0x001239A8
	public OculusSpatializerUnity()
	{
	}

	// Token: 0x06003C92 RID: 15506 RVA: 0x00125676 File Offset: 0x00123A76
	private static Vector3 swapHandedness(Vector3 vec)
	{
		return new Vector3(vec.x, vec.y, -vec.z);
	}

	// Token: 0x06003C93 RID: 15507 RVA: 0x00125694 File Offset: 0x00123A94
	private static void AudioRaycast(Vector3 origin, Vector3 direction, out Vector3 point, out Vector3 normal, IntPtr data)
	{
		point = Vector3.zero;
		normal = Vector3.zero;
		RaycastHit raycastHit;
		if (Physics.Raycast(OculusSpatializerUnity.swapHandedness(origin), OculusSpatializerUnity.swapHandedness(direction), out raycastHit, 1000f, OculusSpatializerUnity.gLayerMask.value))
		{
			point = OculusSpatializerUnity.swapHandedness(raycastHit.point);
			normal = OculusSpatializerUnity.swapHandedness(raycastHit.normal);
		}
	}

	// Token: 0x06003C94 RID: 15508 RVA: 0x00125702 File Offset: 0x00123B02
	private void Start()
	{
		this._raycastCallback = new OculusSpatializerUnity.AudioRaycastCallback(OculusSpatializerUnity.AudioRaycast);
		OculusSpatializerUnity.OSP_Unity_AssignRayCastCallback(this._raycastCallback, IntPtr.Zero);
	}

	// Token: 0x06003C95 RID: 15509 RVA: 0x00125727 File Offset: 0x00123B27
	private void OnDestroy()
	{
		OculusSpatializerUnity.OSP_Unity_AssignRayCastCallback(IntPtr.Zero, IntPtr.Zero);
	}

	// Token: 0x06003C96 RID: 15510 RVA: 0x0012573C File Offset: 0x00123B3C
	private void Update()
	{
		if (this.dynamicReflectionsEnabled)
		{
			OculusSpatializerUnity.OSP_Unity_AssignRayCastCallback(this._raycastCallback, IntPtr.Zero);
		}
		else
		{
			OculusSpatializerUnity.OSP_Unity_AssignRayCastCallback(IntPtr.Zero, IntPtr.Zero);
		}
		OculusSpatializerUnity.OSP_Unity_SetDynamicRoomRaysPerSecond(this.raysPerSecond);
		OculusSpatializerUnity.OSP_Unity_SetDynamicRoomInterpSpeed(this.roomInterpSpeed);
		OculusSpatializerUnity.OSP_Unity_SetDynamicRoomMaxWallDistance(this.maxWallDistance);
		OculusSpatializerUnity.OSP_Unity_SetDynamicRoomRaysRayCacheSize(this.rayCacheSize);
		OculusSpatializerUnity.OSP_Unity_UseLegacyReverb(this.legacyReverb);
		OculusSpatializerUnity.gLayerMask = this.layerMask;
		OculusSpatializerUnity.OSP_Unity_UpdateRoomModel(1f);
		if (this.visualizeRoom)
		{
			if (!this.roomVisualizationInitialized)
			{
				this.inititalizeRoomVisualization();
				this.roomVisualizationInitialized = true;
			}
			Vector3 position;
			OculusSpatializerUnity.OSP_Unity_GetRoomDimensions(this.dims, this.coefs, out position);
			position.z *= -1f;
			Vector3 a = new Vector3(this.dims[0], this.dims[1], this.dims[2]);
			float sqrMagnitude = a.sqrMagnitude;
			if (!float.IsNaN(sqrMagnitude) && 0f < sqrMagnitude && sqrMagnitude < 1000000f)
			{
				base.transform.localScale = a * 0.999f;
			}
			base.transform.position = position;
			OculusSpatializerUnity.OSP_Unity_GetRaycastHits(this.points, this.normals, 2048);
			for (int i = 0; i < 2048; i++)
			{
				if (this.points[i] == Vector3.zero)
				{
					this.points[i].y = -10000f;
				}
				Vector3[] array = this.points;
				int num = i;
				array[num].z = array[num].z * -1f;
				Vector3[] array2 = this.normals;
				int num2 = i;
				array2[num2].z = array2[num2].z * -1f;
				this.particles[i].position = this.points[i] + this.normals[i] * this.particleOffset;
				if (this.normals[i] != Vector3.zero)
				{
					this.particles[i].rotation3D = Quaternion.LookRotation(this.normals[i]).eulerAngles;
				}
				this.particles[i].startSize = this.particleSize;
				this.particles[i].startColor = new Color(0.8156863f, 0.14901961f, 0.68235296f, 1f);
			}
			for (int j = 0; j < 6; j++)
			{
				Color value = Color.Lerp(Color.red, Color.green, this.coefs[j]);
				this.wallRenderer[j].material.SetColor("_TintColor", value);
			}
			this.sys.SetParticles(this.particles, this.particles.Length);
		}
	}

	// Token: 0x06003C97 RID: 15511 RVA: 0x00125A5C File Offset: 0x00123E5C
	private void inititalizeRoomVisualization()
	{
		Debug.Log("Oculus Audio dynamic room estimation visualization enabled");
		base.transform.position = Vector3.zero;
		this.sys = new GameObject("DecalManager")
		{
			transform = 
			{
				parent = base.transform
			}
		}.AddComponent<ParticleSystem>();
		ParticleSystem.MainModule main = this.sys.main;
		main.simulationSpace = ParticleSystemSimulationSpace.World;
		main.loop = false;
		main.playOnAwake = false;
		this.sys.emission.enabled = false;
		this.sys.shape.enabled = false;
		ParticleSystemRenderer component = this.sys.GetComponent<ParticleSystemRenderer>();
		component.renderMode = ParticleSystemRenderMode.Mesh;
		component.material.shader = Shader.Find("Particles/Additive");
		Texture2D texture2D = new Texture2D(64, 64);
		for (int i = 0; i < 32; i++)
		{
			for (int j = 0; j < 32; j++)
			{
				float num = (float)(32 - i);
				float num2 = (float)(32 - j);
				float num3 = Mathf.Sqrt(num * num + num2 * num2);
				float num4 = 2f * num3 / 32f;
				float a = (num3 >= 32f) ? 0f : Mathf.Clamp01(Mathf.Sin(6.2831855f * num4));
				Color color = new Color(1f, 1f, 1f, a);
				texture2D.SetPixel(i, j, color);
				texture2D.SetPixel(64 - i, j, color);
				texture2D.SetPixel(i, 64 - j, color);
				texture2D.SetPixel(64 - i, 64 - j, color);
			}
		}
		texture2D.Apply();
		component.material.mainTexture = texture2D;
		Mesh mesh = new Mesh();
		mesh.name = "ParticleQuad";
		mesh.vertices = new Vector3[]
		{
			new Vector3(-0.5f, -0.5f, 0f),
			new Vector3(0.5f, -0.5f, 0f),
			new Vector3(0.5f, 0.5f, 0f),
			new Vector3(-0.5f, 0.5f, 0f)
		};
		mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
		mesh.triangles = new int[]
		{
			0,
			1,
			2,
			0,
			2,
			3
		};
		mesh.RecalculateNormals();
		component.mesh = mesh;
		this.sys.Emit(2048);
		this.room = new GameObject("RoomVisualizer");
		this.room.transform.parent = base.transform;
		this.room.transform.localPosition = Vector3.zero;
		Texture2D texture2D2 = new Texture2D(32, 32);
		Color color2 = new Color(0f, 0f, 0f, 0f);
		for (int k = 0; k < 32; k++)
		{
			for (int l = 0; l < 32; l++)
			{
				texture2D2.SetPixel(k, l, color2);
			}
		}
		for (int m = 0; m < 32; m++)
		{
			Color color3 = Color.white * 0.125f;
			texture2D2.SetPixel(8, m, color3);
			texture2D2.SetPixel(m, 8, color3);
			texture2D2.SetPixel(24, m, color3);
			texture2D2.SetPixel(m, 24, color3);
			color3 *= 2f;
			texture2D2.SetPixel(16, m, color3);
			texture2D2.SetPixel(m, 16, color3);
			color3 *= 2f;
			texture2D2.SetPixel(0, m, color3);
			texture2D2.SetPixel(m, 0, color3);
		}
		texture2D2.Apply();
		for (int n = 0; n < 6; n++)
		{
			Mesh mesh2 = new Mesh();
			mesh2.name = "Plane" + n;
			Vector3[] array = new Vector3[4];
			int num5 = n / 2;
			int num6 = (n % 2 != 0) ? -1 : 1;
			for (int num7 = 0; num7 < 4; num7++)
			{
				array[num7][num5] = (float)num6 * 0.5f;
				array[num7][(num5 + 1) % 3] = 0.5f * (float)((num7 != 1 && num7 != 2) ? -1 : 1);
				array[num7][(num5 + 2) % 3] = 0.5f * (float)((num7 != 2 && num7 != 3) ? -1 : 1);
			}
			mesh2.vertices = array;
			mesh2.uv = new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f),
				new Vector2(1f, 0f)
			};
			mesh2.triangles = new int[]
			{
				0,
				1,
				2,
				0,
				2,
				3
			};
			mesh2.RecalculateNormals();
			GameObject gameObject = new GameObject("Wall_" + n);
			gameObject.AddComponent<MeshFilter>().mesh = mesh2;
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			this.wallRenderer[n] = meshRenderer;
			meshRenderer.material.shader = Shader.Find("Particles/Additive");
			meshRenderer.material.mainTexture = texture2D2;
			meshRenderer.material.mainTextureScale = new Vector2(8f, 8f);
			gameObject.transform.parent = this.room.transform;
			this.room.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x06003C98 RID: 15512
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int OSP_Unity_AssignRayCastCallback(MulticastDelegate callback, IntPtr data);

	// Token: 0x06003C99 RID: 15513
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int OSP_Unity_AssignRayCastCallback(IntPtr callback, IntPtr data);

	// Token: 0x06003C9A RID: 15514
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int OSP_Unity_SetDynamicRoomRaysPerSecond(int RaysPerSecond);

	// Token: 0x06003C9B RID: 15515
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int OSP_Unity_SetDynamicRoomInterpSpeed(float InterpSpeed);

	// Token: 0x06003C9C RID: 15516
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int OSP_Unity_SetDynamicRoomMaxWallDistance(float MaxWallDistance);

	// Token: 0x06003C9D RID: 15517
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int OSP_Unity_SetDynamicRoomRaysRayCacheSize(int RayCacheSize);

	// Token: 0x06003C9E RID: 15518
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int OSP_Unity_UpdateRoomModel(float wetLevel);

	// Token: 0x06003C9F RID: 15519
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int OSP_Unity_GetRoomDimensions(float[] roomDimensions, float[] reflectionsCoefs, out Vector3 position);

	// Token: 0x06003CA0 RID: 15520
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int OSP_Unity_GetRaycastHits(Vector3[] points, Vector3[] normals, int length);

	// Token: 0x06003CA1 RID: 15521
	[DllImport("AudioPluginOculusSpatializer")]
	private static extern int OSP_Unity_UseLegacyReverb(bool enable);

	// Token: 0x06003CA2 RID: 15522 RVA: 0x001260EE File Offset: 0x001244EE
	// Note: this type is marked as 'beforefieldinit'.
	static OculusSpatializerUnity()
	{
	}

	// Token: 0x04002E70 RID: 11888
	public LayerMask layerMask = -1;

	// Token: 0x04002E71 RID: 11889
	public bool visualizeRoom = true;

	// Token: 0x04002E72 RID: 11890
	private bool roomVisualizationInitialized;

	// Token: 0x04002E73 RID: 11891
	public int raysPerSecond = 256;

	// Token: 0x04002E74 RID: 11892
	public float roomInterpSpeed = 0.9f;

	// Token: 0x04002E75 RID: 11893
	public float maxWallDistance = 50f;

	// Token: 0x04002E76 RID: 11894
	public int rayCacheSize = 512;

	// Token: 0x04002E77 RID: 11895
	public bool dynamicReflectionsEnabled = true;

	// Token: 0x04002E78 RID: 11896
	public bool legacyReverb;

	// Token: 0x04002E79 RID: 11897
	private OculusSpatializerUnity.AudioRaycastCallback _raycastCallback;

	// Token: 0x04002E7A RID: 11898
	private float particleSize = 0.2f;

	// Token: 0x04002E7B RID: 11899
	private float particleOffset = 0.1f;

	// Token: 0x04002E7C RID: 11900
	private GameObject room;

	// Token: 0x04002E7D RID: 11901
	private Renderer[] wallRenderer = new Renderer[6];

	// Token: 0x04002E7E RID: 11902
	private float[] dims = new float[]
	{
		1f,
		1f,
		1f
	};

	// Token: 0x04002E7F RID: 11903
	private float[] coefs = new float[6];

	// Token: 0x04002E80 RID: 11904
	private const int HIT_COUNT = 2048;

	// Token: 0x04002E81 RID: 11905
	private Vector3[] points = new Vector3[2048];

	// Token: 0x04002E82 RID: 11906
	private Vector3[] normals = new Vector3[2048];

	// Token: 0x04002E83 RID: 11907
	private ParticleSystem sys;

	// Token: 0x04002E84 RID: 11908
	private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[2048];

	// Token: 0x04002E85 RID: 11909
	private static LayerMask gLayerMask = -1;

	// Token: 0x04002E86 RID: 11910
	private const string strOSP = "AudioPluginOculusSpatializer";

	// Token: 0x0200097D RID: 2429
	// (Invoke) Token: 0x06003CA4 RID: 15524
	public delegate void AudioRaycastCallback(Vector3 origin, Vector3 direction, out Vector3 point, out Vector3 normal, IntPtr data);
}
