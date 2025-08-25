using System;
using System.Threading;
using UnityEngine;

// Token: 0x02000313 RID: 787
public class WaterRipples : MonoBehaviour
{
	// Token: 0x06001283 RID: 4739 RVA: 0x00067670 File Offset: 0x00065A70
	public WaterRipples()
	{
	}

	// Token: 0x06001284 RID: 4740 RVA: 0x000676C8 File Offset: 0x00065AC8
	private void OnEnable()
	{
		this.canUpdate = true;
		Shader.EnableKeyword("ripples_on");
		Renderer component = base.GetComponent<Renderer>();
		this._GAmplitude = component.sharedMaterial.GetVector("_GAmplitude");
		this._GFrequency = component.sharedMaterial.GetVector("_GFrequency");
		this._GSteepness = component.sharedMaterial.GetVector("_GSteepness");
		this._GSpeed = component.sharedMaterial.GetVector("_GSpeed");
		this._GDirectionAB = component.sharedMaterial.GetVector("_GDirectionAB");
		this._GDirectionCD = component.sharedMaterial.GetVector("_GDirectionCD");
		this.t = base.transform;
		this.scaleBounds = base.GetComponent<MeshRenderer>().bounds.size;
		this.InitializeRipple();
		if (this.Multithreading)
		{
			this.thread = new Thread(new ThreadStart(this.UpdateRipples));
			this.thread.Start();
		}
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x000677CC File Offset: 0x00065BCC
	public Vector3 GetOffsetByPosition(Vector3 position)
	{
		Vector3 result = this.GerstnerOffset4(new Vector2(position.x, position.z), this._GSteepness, this._GAmplitude, this._GFrequency, this._GSpeed, this._GDirectionAB, this._GDirectionCD);
		result.y += this.GetTextureHeightByPosition(position.x, position.z);
		result.y += this.t.position.y;
		return result;
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x0006785C File Offset: 0x00065C5C
	public void CreateRippleByPosition(Vector3 position, float velocity)
	{
		position.x += this.scaleBounds.x / 2f - this.t.position.x;
		position.z += this.scaleBounds.z / 2f - this.t.position.z;
		position.x /= this.scaleBounds.x;
		position.z /= this.scaleBounds.z;
		position.x *= (float)this.DisplacementResolution;
		position.z *= (float)this.DisplacementResolution;
		this.SetRippleTexture((int)position.x, (int)position.z, velocity);
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x00067944 File Offset: 0x00065D44
	private void InitializeRipple()
	{
		this.inversedDamping = 1f - this.Damping;
		this.displacementTexture = new Texture2D(this.DisplacementResolution, this.DisplacementResolution, TextureFormat.RGBA32, false);
		this.displacementTexture.wrapMode = TextureWrapMode.Clamp;
		this.displacementTexture.filterMode = FilterMode.Bilinear;
		Shader.SetGlobalTexture("_WaterDisplacementTexture", this.displacementTexture);
		this.wavePoints = new Vector3[this.DisplacementResolution * this.DisplacementResolution];
		this.col = new Color[this.DisplacementResolution * this.DisplacementResolution];
		this.waveAcceleration = new Vector2[this.DisplacementResolution, this.DisplacementResolution];
		for (int i = 0; i < this.DisplacementResolution * this.DisplacementResolution; i++)
		{
			this.col[i] = new Color(0f, 0f, 0f);
			this.wavePoints[i] = new Vector3(0f, 0f);
		}
		for (int j = 0; j < this.DisplacementResolution; j++)
		{
			for (int k = 0; k < this.DisplacementResolution; k++)
			{
				this.waveAcceleration[j, k] = new Vector2(0f, 0f);
			}
		}
		if (this.CutOutTexture != null)
		{
			Texture2D texture2D = this.ScaleTexture(this.CutOutTexture, this.DisplacementResolution, this.DisplacementResolution);
			Color[] pixels = texture2D.GetPixels();
			this.cutOutTextureGray = new float[this.DisplacementResolution * this.DisplacementResolution];
			for (int l = 0; l < pixels.Length; l++)
			{
				this.cutOutTextureGray[l] = pixels[l].r * 0.299f + pixels[l].g * 0.587f + pixels[l].b * 0.114f;
			}
			this.cutOutTextureInitialized = true;
		}
	}

	// Token: 0x06001288 RID: 4744 RVA: 0x00067B50 File Offset: 0x00065F50
	private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
	{
		Texture2D texture2D = new Texture2D(source.width, source.height, TextureFormat.ARGB32, false);
		Color[] pixels = source.GetPixels();
		texture2D.SetPixels(pixels);
		TextureScale.Bilinear(texture2D, targetWidth, targetHeight);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x06001289 RID: 4745 RVA: 0x00067B90 File Offset: 0x00065F90
	private void UpdateRipples()
	{
		this.oldDateTime = DateTime.UtcNow;
		while (this.canUpdate)
		{
			this.threadDeltaTime = (DateTime.UtcNow - this.oldDateTime).TotalMilliseconds / 1000.0;
			this.oldDateTime = DateTime.UtcNow;
			int num = (int)((double)(1000f / (float)this.UpdateFPS) - this.threadDeltaTime);
			if (num > 0)
			{
				Thread.Sleep(num);
			}
			this.RippleTextureRecalculate();
		}
	}

	// Token: 0x0600128A RID: 4746 RVA: 0x00067C15 File Offset: 0x00066015
	private void FixedUpdate()
	{
		if (!this.Multithreading)
		{
			this.RippleTextureRecalculate();
		}
		this.displacementTexture.SetPixels(this.col);
		this.displacementTexture.Apply(false);
	}

	// Token: 0x0600128B RID: 4747 RVA: 0x00067C48 File Offset: 0x00066048
	private void Update()
	{
		this.movedObjPos = new Vector2(this.t.position.x, this.t.position.z);
	}

	// Token: 0x0600128C RID: 4748 RVA: 0x00067C88 File Offset: 0x00066088
	private void UpdateProjector()
	{
		int num = (int)((float)this.DisplacementResolution * this.movedObjPos.x / this.scaleBounds.x - this.projectorPosition.x);
		int num2 = (int)((float)this.DisplacementResolution * this.movedObjPos.y / this.scaleBounds.z - this.projectorPosition.y);
		this.projectorPosition.x = this.projectorPosition.x + (float)num;
		this.projectorPosition.y = this.projectorPosition.y + (float)num2;
		if (num == 0 && num2 == 0)
		{
			return;
		}
		if (num >= 0 && num2 >= 0)
		{
			for (int i = 1; i < this.DisplacementResolution; i++)
			{
				for (int j = 0; j < this.DisplacementResolution; j++)
				{
					if (i + num2 > 0 && i + num2 < this.DisplacementResolution && j + num > 0 && j + num < this.DisplacementResolution)
					{
						this.waveAcceleration[j, i] = this.waveAcceleration[j + num, i + num2];
						this.wavePoints[j + i * this.DisplacementResolution] = this.wavePoints[j + num + (i + num2) * this.DisplacementResolution];
					}
				}
			}
		}
		if (num >= 0 && num2 < 0)
		{
			for (int k = this.DisplacementResolution - 1; k >= 0; k--)
			{
				for (int l = 0; l < this.DisplacementResolution; l++)
				{
					if (k + num2 > 0 && k + num2 < this.DisplacementResolution && l + num > 0 && l + num < this.DisplacementResolution)
					{
						this.waveAcceleration[l, k] = this.waveAcceleration[l + num, k + num2];
						this.wavePoints[l + k * this.DisplacementResolution] = this.wavePoints[l + num + (k + num2) * this.DisplacementResolution];
					}
				}
			}
		}
		if (num < 0 && num2 >= 0)
		{
			for (int m = 0; m < this.DisplacementResolution; m++)
			{
				for (int n = this.DisplacementResolution - 1; n >= 0; n--)
				{
					if (m + num2 > 0 && m + num2 < this.DisplacementResolution && n + num > 0 && n + num < this.DisplacementResolution)
					{
						this.waveAcceleration[n, m] = this.waveAcceleration[n + num, m + num2];
						this.wavePoints[n + m * this.DisplacementResolution] = this.wavePoints[n + num + (m + num2) * this.DisplacementResolution];
					}
				}
			}
		}
		if (num < 0 && num2 < 0)
		{
			for (int num3 = this.DisplacementResolution - 1; num3 >= 0; num3--)
			{
				for (int num4 = this.DisplacementResolution - 1; num4 >= 0; num4--)
				{
					if (num3 + num2 > 0 && num3 + num2 < this.DisplacementResolution && num4 + num > 0 && num4 + num < this.DisplacementResolution)
					{
						this.waveAcceleration[num4, num3] = this.waveAcceleration[num4 + num, num3 + num2];
						this.wavePoints[num4 + num3 * this.DisplacementResolution] = this.wavePoints[num4 + num + (num3 + num2) * this.DisplacementResolution];
					}
				}
			}
		}
		Vector2 zero = Vector2.zero;
		for (int num5 = 0; num5 < this.DisplacementResolution; num5++)
		{
			this.waveAcceleration[0, num5] = zero;
			this.wavePoints[num5 * this.DisplacementResolution] = zero;
			this.waveAcceleration[this.DisplacementResolution - 1, num5] = zero;
			this.wavePoints[this.DisplacementResolution - 1 + num5 * this.DisplacementResolution] = zero;
			this.waveAcceleration[num5, 0] = zero;
			this.wavePoints[num5] = zero;
			this.waveAcceleration[num5, this.DisplacementResolution - 1] = zero;
			this.wavePoints[this.DisplacementResolution - 1 + num5] = zero;
		}
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x00068184 File Offset: 0x00066584
	private void OnDestroy()
	{
		this.canUpdate = false;
	}

	// Token: 0x0600128E RID: 4750 RVA: 0x0006818D File Offset: 0x0006658D
	private void OnDisable()
	{
		Shader.DisableKeyword("ripples_on");
		this.canUpdate = false;
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x000681A0 File Offset: 0x000665A0
	private void RippleTextureRecalculate()
	{
		if (this.UseProjectedWaves)
		{
			this.UpdateProjector();
		}
		int num = this.wavePoints.Length;
		int num2 = this.DisplacementResolution + 1;
		int num3 = this.DisplacementResolution - 2;
		int num4 = num - (this.DisplacementResolution + 1);
		for (int i = 0; i < num; i++)
		{
			if (i >= num2 && i < num4 && i % this.DisplacementResolution > 0)
			{
				int num5 = i % this.DisplacementResolution;
				int num6 = i / this.DisplacementResolution;
				float num7 = (this.wavePoints[i - 1].y + this.wavePoints[i + 1].y + this.wavePoints[i - this.DisplacementResolution].y + this.wavePoints[i + this.DisplacementResolution].y) / 4f;
				this.waveAcceleration[num5, num6].y += num7 - this.waveAcceleration[num5, num6].x;
			}
		}
		float num8 = this.Speed;
		if (!this.Multithreading)
		{
			num8 *= Time.fixedDeltaTime * (float)this.UpdateFPS;
		}
		for (int j = 0; j < this.DisplacementResolution; j++)
		{
			for (int k = 0; k < this.DisplacementResolution; k++)
			{
				this.waveAcceleration[k, j].x += this.waveAcceleration[k, j].y * num8;
				if (this.cutOutTextureInitialized)
				{
					this.waveAcceleration[k, j].x *= this.cutOutTextureGray[k + j * this.DisplacementResolution];
				}
				this.waveAcceleration[k, j].y *= this.inversedDamping;
				this.waveAcceleration[k, j].x *= this.inversedDamping;
				this.wavePoints[k + j * this.DisplacementResolution].y = this.waveAcceleration[k, j].x;
				if (!this.UseSmoothWaves)
				{
					float num9 = this.waveAcceleration[k, j].x * this.textureColorMultiplier;
					if (num9 >= 0f)
					{
						this.col[k + j * this.DisplacementResolution].r = num9;
					}
					else
					{
						this.col[k + j * this.DisplacementResolution].g = -num9;
					}
				}
			}
		}
		if (this.UseSmoothWaves)
		{
			for (int l = 2; l < num3; l++)
			{
				for (int m = 2; m < num3; m++)
				{
					float num9 = (this.wavePoints[m + l * this.DisplacementResolution - 2].y * 0.2f + this.wavePoints[m + l * this.DisplacementResolution - 1].y * 0.4f + this.wavePoints[m + l * this.DisplacementResolution].y * 0.6f + this.wavePoints[m + l * this.DisplacementResolution + 1].y * 0.4f + this.wavePoints[m + l * this.DisplacementResolution + 2].y * 0.2f) / 1.6f * this.textureColorMultiplier;
					if (num9 >= 0f)
					{
						this.col[m + l * this.DisplacementResolution].r = num9;
					}
					else
					{
						this.col[m + l * this.DisplacementResolution].g = -num9;
					}
				}
			}
			for (int n = 2; n < num3; n++)
			{
				for (int num10 = 2; num10 < num3; num10++)
				{
					float num9 = (this.wavePoints[num10 + n * this.DisplacementResolution - 2].y * 0.2f + this.wavePoints[num10 + n * this.DisplacementResolution - 1].y * 0.4f + this.wavePoints[num10 + n * this.DisplacementResolution].y * 0.6f + this.wavePoints[num10 + n * this.DisplacementResolution + 1].y * 0.4f + this.wavePoints[num10 + n * this.DisplacementResolution + 2].y * 0.2f) / 1.6f * this.textureColorMultiplier;
					if (num9 >= 0f)
					{
						this.col[num10 + n * this.DisplacementResolution].r = num9;
					}
					else
					{
						this.col[num10 + n * this.DisplacementResolution].g = -num9;
					}
				}
			}
		}
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x00068700 File Offset: 0x00066B00
	private void SetRippleTexture(int x, int y, float strength)
	{
		strength /= 100f;
		if (x >= 2 && x < this.DisplacementResolution - 2 && y >= 2 && y < this.DisplacementResolution - 2)
		{
			this.waveAcceleration[x, y].y -= strength;
			this.waveAcceleration[x + 1, y].y -= strength * 0.8f;
			this.waveAcceleration[x - 1, y].y -= strength * 0.8f;
			this.waveAcceleration[x, y + 1].y -= strength * 0.8f;
			this.waveAcceleration[x, y - 1].y -= strength * 0.8f;
			this.waveAcceleration[x + 1, y + 1].y -= strength * 0.7f;
			this.waveAcceleration[x + 1, y - 1].y -= strength * 0.7f;
			this.waveAcceleration[x - 1, y + 1].y -= strength * 0.7f;
			this.waveAcceleration[x - 1, y - 1].y -= strength * 0.7f;
			if (x >= 3 && x < this.DisplacementResolution - 3 && y >= 3 && y < this.DisplacementResolution - 3)
			{
				this.waveAcceleration[x + 2, y].y -= strength * 0.5f;
				this.waveAcceleration[x - 2, y].y -= strength * 0.5f;
				this.waveAcceleration[x, y + 2].y -= strength * 0.5f;
				this.waveAcceleration[x, y - 2].y -= strength * 0.5f;
			}
		}
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x00068924 File Offset: 0x00066D24
	private float GetTextureHeightByPosition(float x, float y)
	{
		x /= this.scaleBounds.x;
		y /= this.scaleBounds.y;
		x *= (float)this.DisplacementResolution;
		y *= (float)this.DisplacementResolution;
		if (x >= (float)this.DisplacementResolution || y >= (float)this.DisplacementResolution || x < 0f || y < 0f)
		{
			return 0f;
		}
		return this.waveAcceleration[(int)x, (int)y].x * this.textureColorMultiplier;
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x000689B8 File Offset: 0x00066DB8
	private Vector3 GerstnerOffset4(Vector2 xzVtx, Vector4 _GSteepness, Vector4 _GAmplitude, Vector4 _GFrequency, Vector4 _GSpeed, Vector4 _GDirectionAB, Vector4 _GDirectionCD)
	{
		Vector3 result = default(Vector3);
		float num = _GSteepness.x * _GAmplitude.x;
		float num2 = _GSteepness.y * _GAmplitude.y;
		Vector4 vector = new Vector4(num * _GDirectionAB.x, num * _GDirectionAB.y, num2 * _GDirectionAB.z, num2 * _GDirectionAB.w);
		Vector4 vector2 = new Vector4(_GSteepness.z * _GAmplitude.z * _GDirectionCD.x, _GSteepness.z * _GAmplitude.z * _GDirectionCD.y, _GSteepness.w * _GAmplitude.w * _GDirectionCD.z, _GSteepness.w * _GAmplitude.w * _GDirectionCD.w);
		float num3 = Vector2.Dot(new Vector2(_GDirectionAB.x, _GDirectionAB.y), xzVtx);
		float num4 = Vector2.Dot(new Vector2(_GDirectionAB.z, _GDirectionAB.w), xzVtx);
		float num5 = Vector2.Dot(new Vector2(_GDirectionCD.x, _GDirectionCD.y), xzVtx);
		float num6 = Vector2.Dot(new Vector2(_GDirectionCD.z, _GDirectionCD.w), xzVtx);
		Vector4 vector3 = new Vector4(num3 * _GFrequency.x, num4 * _GFrequency.y, num5 * _GFrequency.z, num6 * _GFrequency.w);
		Vector4 vector4 = new Vector4(Time.time * _GSpeed.x % 6.2831f, Time.time * _GSpeed.y % 6.2831f, Time.time * _GSpeed.z % 6.2831f, Time.time * _GSpeed.w % 6.2831f);
		Vector4 a = new Vector4(Mathf.Cos(vector3.x + vector4.x), Mathf.Cos(vector3.y + vector4.y), Mathf.Cos(vector3.z + vector4.z), Mathf.Cos(vector3.w + vector4.w));
		Vector4 a2 = new Vector4(Mathf.Sin(vector3.x + vector4.x), Mathf.Sin(vector3.y + vector4.y), Mathf.Sin(vector3.z + vector4.z), Mathf.Sin(vector3.w + vector4.w));
		result.x = Vector4.Dot(a, new Vector4(vector.x, vector.z, vector2.x, vector2.z));
		result.z = Vector4.Dot(a, new Vector4(vector.y, vector.w, vector2.y, vector2.w));
		result.y = Vector4.Dot(a2, _GAmplitude);
		return result;
	}

	// Token: 0x04000FFF RID: 4095
	[Range(20f, 200f)]
	public int UpdateFPS = 30;

	// Token: 0x04001000 RID: 4096
	public bool Multithreading = true;

	// Token: 0x04001001 RID: 4097
	public int DisplacementResolution = 128;

	// Token: 0x04001002 RID: 4098
	public float Damping = 0.005f;

	// Token: 0x04001003 RID: 4099
	[Range(0.0001f, 2f)]
	public float Speed = 1.5f;

	// Token: 0x04001004 RID: 4100
	public bool UseSmoothWaves;

	// Token: 0x04001005 RID: 4101
	public bool UseProjectedWaves;

	// Token: 0x04001006 RID: 4102
	public Texture2D CutOutTexture;

	// Token: 0x04001007 RID: 4103
	private Transform t;

	// Token: 0x04001008 RID: 4104
	private float textureColorMultiplier = 10f;

	// Token: 0x04001009 RID: 4105
	private Texture2D displacementTexture;

	// Token: 0x0400100A RID: 4106
	private Vector2[,] waveAcceleration;

	// Token: 0x0400100B RID: 4107
	private Color[] col;

	// Token: 0x0400100C RID: 4108
	private Vector3[] wavePoints;

	// Token: 0x0400100D RID: 4109
	private Vector3 scaleBounds;

	// Token: 0x0400100E RID: 4110
	private float inversedDamping;

	// Token: 0x0400100F RID: 4111
	private float[] cutOutTextureGray;

	// Token: 0x04001010 RID: 4112
	private bool cutOutTextureInitialized;

	// Token: 0x04001011 RID: 4113
	private Thread thread;

	// Token: 0x04001012 RID: 4114
	private bool canUpdate = true;

	// Token: 0x04001013 RID: 4115
	private double threadDeltaTime;

	// Token: 0x04001014 RID: 4116
	private DateTime oldDateTime;

	// Token: 0x04001015 RID: 4117
	private Vector2 movedObjPos;

	// Token: 0x04001016 RID: 4118
	private Vector2 projectorPosition;

	// Token: 0x04001017 RID: 4119
	private Vector4 _GAmplitude;

	// Token: 0x04001018 RID: 4120
	private Vector4 _GFrequency;

	// Token: 0x04001019 RID: 4121
	private Vector4 _GSteepness;

	// Token: 0x0400101A RID: 4122
	private Vector4 _GSpeed;

	// Token: 0x0400101B RID: 4123
	private Vector4 _GDirectionAB;

	// Token: 0x0400101C RID: 4124
	private Vector4 _GDirectionCD;
}
