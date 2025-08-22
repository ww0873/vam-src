using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020002EF RID: 751
public class CPUStarDataRenderer : BaseStarDataRenderer
{
	// Token: 0x060011AF RID: 4527 RVA: 0x0006189F File Offset: 0x0005FC9F
	public CPUStarDataRenderer()
	{
	}

	// Token: 0x060011B0 RID: 4528 RVA: 0x000618A8 File Offset: 0x0005FCA8
	public override IEnumerator ComputeStarData()
	{
		base.SendProgress(0f);
		Texture2D tex = new Texture2D((int)this.imageSize, (int)this.imageSize, TextureFormat.RGBAFloat, false);
		int tileSize = (int)this.imageSize / 2;
		List<StarPoint> starPoints = this.GenerateRandomStarsPoints(this.density, tileSize, tileSize);
		Vector2 origin = new Vector2(0f, (float)tileSize);
		base.SendProgress(0f);
		Vector2 rotationOrigin = new Vector2((float)tileSize, (float)tileSize);
		for (int yIndex = 0; yIndex < tileSize; yIndex++)
		{
			float yPercent = (float)yIndex / (float)(tileSize - 1);
			float yPosition = SphereUtility.PercentToHeight(yPercent);
			for (int i = 0; i < tileSize; i++)
			{
				float percent = (float)i / (float)(tileSize - 1);
				float radAngle = SphereUtility.PercentToRadAngle(percent);
				Vector3 spot = SphereUtility.SphericalToPoint(yPosition, radAngle);
				StarPoint starPoint = this.NearestStarPoint(spot, starPoints);
				Color color = new Color(starPoint.position.x, starPoint.position.y, starPoint.position.z, starPoint.noise);
				tex.SetPixel((int)origin.x + i, (int)origin.y + yIndex, color);
				float r;
				float g;
				SphereUtility.CalculateStarRotation(starPoint.position, out r, out g);
				Color color2 = new Color(r, g, 0f, 1f);
				tex.SetPixel((int)rotationOrigin.x + i, (int)rotationOrigin.y + yIndex, color2);
			}
			float totalProgress = (float)((yIndex + 1) * tileSize) / (float)(tileSize * tileSize);
			base.SendProgress(totalProgress);
			yield return null;
		}
		tex.Apply(false);
		base.SendCompletion(tex, true);
		yield break;
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x000618C4 File Offset: 0x0005FCC4
	private List<StarPoint> GenerateRandomStarsPoints(float density, int imageWidth, int imageHeight)
	{
		int num = Mathf.FloorToInt((float)imageWidth * (float)imageHeight * Mathf.Clamp(density, 0f, 1f));
		List<StarPoint> list = new List<StarPoint>(num + 1);
		for (int i = 0; i < num; i++)
		{
			Vector3 position = UnityEngine.Random.onUnitSphere * this.sphereRadius;
			StarPoint item = new StarPoint(position, UnityEngine.Random.Range(0.5f, 1f), 0f, 0f);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x00061944 File Offset: 0x0005FD44
	private StarPoint NearestStarPoint(Vector3 spot, List<StarPoint> starPoints)
	{
		StarPoint result = new StarPoint(Vector3.zero, 0f, 0f, 0f);
		if (starPoints == null)
		{
			return result;
		}
		float num = -1f;
		for (int i = 0; i < starPoints.Count; i++)
		{
			StarPoint starPoint = starPoints[i];
			float num2 = Vector3.Distance(spot, starPoint.position);
			if (num == -1f || num2 < num)
			{
				result = starPoint;
				num = num2;
			}
		}
		return result;
	}

	// Token: 0x02000EEF RID: 3823
	[CompilerGenerated]
	private sealed class <ComputeStarData>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007247 RID: 29255 RVA: 0x000619BF File Offset: 0x0005FDBF
		[DebuggerHidden]
		public <ComputeStarData>c__Iterator0()
		{
		}

		// Token: 0x06007248 RID: 29256 RVA: 0x000619C8 File Offset: 0x0005FDC8
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				base.SendProgress(0f);
				tex = new Texture2D((int)this.imageSize, (int)this.imageSize, TextureFormat.RGBAFloat, false);
				tileSize = (int)this.imageSize / 2;
				starPoints = base.GenerateRandomStarsPoints(this.density, tileSize, tileSize);
				origin = new Vector2(0f, (float)tileSize);
				base.SendProgress(0f);
				rotationOrigin = new Vector2((float)tileSize, (float)tileSize);
				yIndex = 0;
				break;
			case 1U:
				yIndex++;
				break;
			default:
				return false;
			}
			if (yIndex < tileSize)
			{
				yPercent = (float)yIndex / (float)(tileSize - 1);
				yPosition = SphereUtility.PercentToHeight(yPercent);
				for (int i = 0; i < tileSize; i++)
				{
					float percent = (float)i / (float)(tileSize - 1);
					float radAngle = SphereUtility.PercentToRadAngle(percent);
					Vector3 spot = SphereUtility.SphericalToPoint(yPosition, radAngle);
					StarPoint starPoint = base.NearestStarPoint(spot, starPoints);
					Color color = new Color(starPoint.position.x, starPoint.position.y, starPoint.position.z, starPoint.noise);
					tex.SetPixel((int)origin.x + i, (int)origin.y + yIndex, color);
					float r;
					float g;
					SphereUtility.CalculateStarRotation(starPoint.position, out r, out g);
					Color color2 = new Color(r, g, 0f, 1f);
					tex.SetPixel((int)rotationOrigin.x + i, (int)rotationOrigin.y + yIndex, color2);
				}
				totalProgress = (float)((yIndex + 1) * tileSize) / (float)(tileSize * tileSize);
				base.SendProgress(totalProgress);
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			tex.Apply(false);
			base.SendCompletion(tex, true);
			return false;
		}

		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x06007249 RID: 29257 RVA: 0x00061C7A File Offset: 0x0006007A
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x0600724A RID: 29258 RVA: 0x00061C82 File Offset: 0x00060082
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600724B RID: 29259 RVA: 0x00061C8A File Offset: 0x0006008A
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600724C RID: 29260 RVA: 0x00061C9A File Offset: 0x0006009A
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400662A RID: 26154
		internal Texture2D <tex>__0;

		// Token: 0x0400662B RID: 26155
		internal int <tileSize>__0;

		// Token: 0x0400662C RID: 26156
		internal List<StarPoint> <starPoints>__0;

		// Token: 0x0400662D RID: 26157
		internal Vector2 <origin>__0;

		// Token: 0x0400662E RID: 26158
		internal Vector2 <rotationOrigin>__0;

		// Token: 0x0400662F RID: 26159
		internal int <yIndex>__1;

		// Token: 0x04006630 RID: 26160
		internal float <yPercent>__2;

		// Token: 0x04006631 RID: 26161
		internal float <yPosition>__2;

		// Token: 0x04006632 RID: 26162
		internal float <totalProgress>__2;

		// Token: 0x04006633 RID: 26163
		internal CPUStarDataRenderer $this;

		// Token: 0x04006634 RID: 26164
		internal object $current;

		// Token: 0x04006635 RID: 26165
		internal bool $disposing;

		// Token: 0x04006636 RID: 26166
		internal int $PC;
	}
}
