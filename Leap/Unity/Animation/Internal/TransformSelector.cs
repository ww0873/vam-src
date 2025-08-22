using System;
using UnityEngine;

namespace Leap.Unity.Animation.Internal
{
	// Token: 0x02000653 RID: 1619
	public struct TransformSelector
	{
		// Token: 0x060027A5 RID: 10149 RVA: 0x000DC1AF File Offset: 0x000DA5AF
		public TransformSelector(Transform target, Tween tween)
		{
			this._target = target;
			this._tween = tween;
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x000DC1BF File Offset: 0x000DA5BF
		public Tween Position(Vector3 a, Vector3 b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformPositionValueInterpolator>.Spawn().Init(a, b, this._target));
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x000DC1DE File Offset: 0x000DA5DE
		public Tween ToPosition(Vector3 b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformPositionValueInterpolator>.Spawn().Init(this._target.position, b, this._target));
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x000DC207 File Offset: 0x000DA607
		public Tween ByPosition(Vector3 delta)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformPositionValueInterpolator>.Spawn().Init(this._target.position, this._target.position + delta, this._target));
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x000DC240 File Offset: 0x000DA640
		public Tween Position(Transform a, Transform b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformPositionReferenceInterpolator>.Spawn().Init(a, b, this._target));
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x000DC25F File Offset: 0x000DA65F
		public Tween LocalPosition(Vector3 a, Vector3 b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalPositionValueInterpolator>.Spawn().Init(a, b, this._target));
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x000DC27E File Offset: 0x000DA67E
		public Tween ToLocalPosition(Vector3 b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalPositionValueInterpolator>.Spawn().Init(this._target.localPosition, b, this._target));
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x000DC2A7 File Offset: 0x000DA6A7
		public Tween ByLocalPosition(Vector3 delta)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalPositionValueInterpolator>.Spawn().Init(this._target.localPosition, this._target.localPosition + delta, this._target));
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x000DC2E0 File Offset: 0x000DA6E0
		public Tween LocalPosition(Transform a, Transform b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalPositionReferenceInterpolator>.Spawn().Init(a, b, this._target));
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x000DC2FF File Offset: 0x000DA6FF
		public Tween Rotation(Quaternion a, Quaternion b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformRotationValueInterpolator>.Spawn().Init(a, b, this._target));
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x000DC31E File Offset: 0x000DA71E
		public Tween ToRotation(Quaternion b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformRotationValueInterpolator>.Spawn().Init(this._target.rotation, b, this._target));
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x000DC347 File Offset: 0x000DA747
		public Tween ByRotation(Quaternion delta)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformRotationValueInterpolator>.Spawn().Init(this._target.rotation, this._target.rotation * delta, this._target));
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x000DC380 File Offset: 0x000DA780
		public Tween Rotation(Transform a, Transform b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformRotationReferenceInterpolator>.Spawn().Init(a, b, this._target));
		}

		// Token: 0x060027B2 RID: 10162 RVA: 0x000DC39F File Offset: 0x000DA79F
		public Tween LocalRotation(Quaternion a, Quaternion b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalRotationValueInterpolator>.Spawn().Init(a, b, this._target));
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x000DC3BE File Offset: 0x000DA7BE
		public Tween ToLocalRotation(Quaternion b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalRotationValueInterpolator>.Spawn().Init(this._target.localRotation, b, this._target));
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x000DC3E7 File Offset: 0x000DA7E7
		public Tween ByLocalRotation(Quaternion delta)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalRotationValueInterpolator>.Spawn().Init(this._target.localRotation, this._target.localRotation * delta, this._target));
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x000DC420 File Offset: 0x000DA820
		public Tween LocalRotation(Transform a, Transform b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalRotationReferenceInterpolator>.Spawn().Init(a, b, this._target));
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x000DC43F File Offset: 0x000DA83F
		public Tween LocalScale(Vector3 a, Vector3 b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalScaleValueInterpolator>.Spawn().Init(a, b, this._target));
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000DC45E File Offset: 0x000DA85E
		public Tween LocalScale(float a, float b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalScaleValueInterpolator>.Spawn().Init(Vector3.one * a, Vector3.one * b, this._target));
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x000DC491 File Offset: 0x000DA891
		public Tween ToLocalScale(Vector3 b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalScaleValueInterpolator>.Spawn().Init(this._target.localScale, b, this._target));
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000DC4BA File Offset: 0x000DA8BA
		public Tween ToLocalScale(float b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalScaleValueInterpolator>.Spawn().Init(this._target.localScale, Vector3.one * b, this._target));
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x000DC4ED File Offset: 0x000DA8ED
		public Tween ByLocalScale(float b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalScaleValueInterpolator>.Spawn().Init(this._target.localScale, this._target.localScale * b, this._target));
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x000DC526 File Offset: 0x000DA926
		public Tween LocalScale(Transform a, Transform b)
		{
			return this._tween.AddInterpolator(Pool<TransformSelector.TransformLocalScaleReferenceInterpolator>.Spawn().Init(a, b, this._target));
		}

		// Token: 0x0400213B RID: 8507
		private Transform _target;

		// Token: 0x0400213C RID: 8508
		private Tween _tween;

		// Token: 0x02000654 RID: 1620
		private class TransformPositionValueInterpolator : Vector3InterpolatorBase<Transform>
		{
			// Token: 0x060027BC RID: 10172 RVA: 0x000DC545 File Offset: 0x000DA945
			public TransformPositionValueInterpolator()
			{
			}

			// Token: 0x060027BD RID: 10173 RVA: 0x000DC54D File Offset: 0x000DA94D
			public override void Interpolate(float percent)
			{
				this._target.position = this._a + this._b * percent;
			}

			// Token: 0x060027BE RID: 10174 RVA: 0x000DC571 File Offset: 0x000DA971
			public override void Dispose()
			{
				this._target = null;
				Pool<TransformSelector.TransformPositionValueInterpolator>.Recycle(this);
			}

			// Token: 0x170004E1 RID: 1249
			// (get) Token: 0x060027BF RID: 10175 RVA: 0x000DC580 File Offset: 0x000DA980
			public override bool isValid
			{
				get
				{
					return this._target != null;
				}
			}
		}

		// Token: 0x02000655 RID: 1621
		private class TransformPositionReferenceInterpolator : InterpolatorBase<Transform, Transform>
		{
			// Token: 0x060027C0 RID: 10176 RVA: 0x000DC58E File Offset: 0x000DA98E
			public TransformPositionReferenceInterpolator()
			{
			}

			// Token: 0x170004E2 RID: 1250
			// (get) Token: 0x060027C1 RID: 10177 RVA: 0x000DC596 File Offset: 0x000DA996
			public override float length
			{
				get
				{
					return Vector3.Distance(this._a.position, this._b.position);
				}
			}

			// Token: 0x060027C2 RID: 10178 RVA: 0x000DC5B3 File Offset: 0x000DA9B3
			public override void Interpolate(float percent)
			{
				this._target.position = Vector3.Lerp(this._a.position, this._b.position, percent);
			}

			// Token: 0x060027C3 RID: 10179 RVA: 0x000DC5DC File Offset: 0x000DA9DC
			public override void Dispose()
			{
				this._target = null;
				Pool<TransformSelector.TransformPositionReferenceInterpolator>.Recycle(this);
			}

			// Token: 0x170004E3 RID: 1251
			// (get) Token: 0x060027C4 RID: 10180 RVA: 0x000DC5EB File Offset: 0x000DA9EB
			public override bool isValid
			{
				get
				{
					return this._target != null;
				}
			}
		}

		// Token: 0x02000656 RID: 1622
		private class TransformLocalPositionValueInterpolator : Vector3InterpolatorBase<Transform>
		{
			// Token: 0x060027C5 RID: 10181 RVA: 0x000DC5F9 File Offset: 0x000DA9F9
			public TransformLocalPositionValueInterpolator()
			{
			}

			// Token: 0x060027C6 RID: 10182 RVA: 0x000DC601 File Offset: 0x000DAA01
			public override void Interpolate(float percent)
			{
				this._target.localPosition = this._a + this._b * percent;
			}

			// Token: 0x060027C7 RID: 10183 RVA: 0x000DC625 File Offset: 0x000DAA25
			public override void Dispose()
			{
				this._target = null;
				Pool<TransformSelector.TransformLocalPositionValueInterpolator>.Recycle(this);
			}

			// Token: 0x170004E4 RID: 1252
			// (get) Token: 0x060027C8 RID: 10184 RVA: 0x000DC634 File Offset: 0x000DAA34
			public override bool isValid
			{
				get
				{
					return this._target != null;
				}
			}
		}

		// Token: 0x02000657 RID: 1623
		private class TransformLocalPositionReferenceInterpolator : InterpolatorBase<Transform, Transform>
		{
			// Token: 0x060027C9 RID: 10185 RVA: 0x000DC642 File Offset: 0x000DAA42
			public TransformLocalPositionReferenceInterpolator()
			{
			}

			// Token: 0x170004E5 RID: 1253
			// (get) Token: 0x060027CA RID: 10186 RVA: 0x000DC64A File Offset: 0x000DAA4A
			public override float length
			{
				get
				{
					return Vector3.Distance(this._a.localPosition, this._b.localPosition);
				}
			}

			// Token: 0x060027CB RID: 10187 RVA: 0x000DC667 File Offset: 0x000DAA67
			public override void Interpolate(float percent)
			{
				this._target.localPosition = Vector3.Lerp(this._a.localPosition, this._b.localPosition, percent);
			}

			// Token: 0x060027CC RID: 10188 RVA: 0x000DC690 File Offset: 0x000DAA90
			public override void Dispose()
			{
				this._target = null;
				Pool<TransformSelector.TransformLocalPositionReferenceInterpolator>.Recycle(this);
			}

			// Token: 0x170004E6 RID: 1254
			// (get) Token: 0x060027CD RID: 10189 RVA: 0x000DC69F File Offset: 0x000DAA9F
			public override bool isValid
			{
				get
				{
					return this._target != null;
				}
			}
		}

		// Token: 0x02000658 RID: 1624
		private class TransformRotationValueInterpolator : QuaternionInterpolatorBase<Transform>
		{
			// Token: 0x060027CE RID: 10190 RVA: 0x000DC6AD File Offset: 0x000DAAAD
			public TransformRotationValueInterpolator()
			{
			}

			// Token: 0x060027CF RID: 10191 RVA: 0x000DC6B5 File Offset: 0x000DAAB5
			public override void Interpolate(float percent)
			{
				this._target.rotation = Quaternion.Slerp(this._a, this._b, percent);
			}

			// Token: 0x060027D0 RID: 10192 RVA: 0x000DC6D4 File Offset: 0x000DAAD4
			public override void Dispose()
			{
				this._target = null;
				Pool<TransformSelector.TransformRotationValueInterpolator>.Recycle(this);
			}

			// Token: 0x170004E7 RID: 1255
			// (get) Token: 0x060027D1 RID: 10193 RVA: 0x000DC6E3 File Offset: 0x000DAAE3
			public override bool isValid
			{
				get
				{
					return this._target != null;
				}
			}
		}

		// Token: 0x02000659 RID: 1625
		private class TransformRotationReferenceInterpolator : InterpolatorBase<Transform, Transform>
		{
			// Token: 0x060027D2 RID: 10194 RVA: 0x000DC6F1 File Offset: 0x000DAAF1
			public TransformRotationReferenceInterpolator()
			{
			}

			// Token: 0x170004E8 RID: 1256
			// (get) Token: 0x060027D3 RID: 10195 RVA: 0x000DC6F9 File Offset: 0x000DAAF9
			public override float length
			{
				get
				{
					return Quaternion.Angle(this._a.rotation, this._b.rotation);
				}
			}

			// Token: 0x060027D4 RID: 10196 RVA: 0x000DC716 File Offset: 0x000DAB16
			public override void Interpolate(float percent)
			{
				this._target.rotation = Quaternion.Slerp(this._a.rotation, this._b.rotation, percent);
			}

			// Token: 0x060027D5 RID: 10197 RVA: 0x000DC73F File Offset: 0x000DAB3F
			public override void Dispose()
			{
				this._target = null;
				Pool<TransformSelector.TransformRotationReferenceInterpolator>.Recycle(this);
			}

			// Token: 0x170004E9 RID: 1257
			// (get) Token: 0x060027D6 RID: 10198 RVA: 0x000DC74E File Offset: 0x000DAB4E
			public override bool isValid
			{
				get
				{
					return this._target != null;
				}
			}
		}

		// Token: 0x0200065A RID: 1626
		private class TransformLocalRotationValueInterpolator : QuaternionInterpolatorBase<Transform>
		{
			// Token: 0x060027D7 RID: 10199 RVA: 0x000DC75C File Offset: 0x000DAB5C
			public TransformLocalRotationValueInterpolator()
			{
			}

			// Token: 0x060027D8 RID: 10200 RVA: 0x000DC764 File Offset: 0x000DAB64
			public override void Interpolate(float percent)
			{
				this._target.localRotation = Quaternion.Slerp(this._a, this._b, percent);
			}

			// Token: 0x060027D9 RID: 10201 RVA: 0x000DC783 File Offset: 0x000DAB83
			public override void Dispose()
			{
				this._target = null;
				Pool<TransformSelector.TransformLocalRotationValueInterpolator>.Recycle(this);
			}

			// Token: 0x170004EA RID: 1258
			// (get) Token: 0x060027DA RID: 10202 RVA: 0x000DC792 File Offset: 0x000DAB92
			public override bool isValid
			{
				get
				{
					return this._target != null;
				}
			}
		}

		// Token: 0x0200065B RID: 1627
		private class TransformLocalRotationReferenceInterpolator : InterpolatorBase<Transform, Transform>
		{
			// Token: 0x060027DB RID: 10203 RVA: 0x000DC7A0 File Offset: 0x000DABA0
			public TransformLocalRotationReferenceInterpolator()
			{
			}

			// Token: 0x170004EB RID: 1259
			// (get) Token: 0x060027DC RID: 10204 RVA: 0x000DC7A8 File Offset: 0x000DABA8
			public override float length
			{
				get
				{
					return Quaternion.Angle(this._a.localRotation, this._b.localRotation);
				}
			}

			// Token: 0x060027DD RID: 10205 RVA: 0x000DC7C5 File Offset: 0x000DABC5
			public override void Interpolate(float percent)
			{
				this._target.localRotation = Quaternion.Slerp(this._a.localRotation, this._b.localRotation, percent);
			}

			// Token: 0x060027DE RID: 10206 RVA: 0x000DC7EE File Offset: 0x000DABEE
			public override void Dispose()
			{
				this._target = null;
				Pool<TransformSelector.TransformLocalRotationReferenceInterpolator>.Recycle(this);
			}

			// Token: 0x170004EC RID: 1260
			// (get) Token: 0x060027DF RID: 10207 RVA: 0x000DC7FD File Offset: 0x000DABFD
			public override bool isValid
			{
				get
				{
					return this._target != null;
				}
			}
		}

		// Token: 0x0200065C RID: 1628
		private class TransformLocalScaleValueInterpolator : Vector3InterpolatorBase<Transform>
		{
			// Token: 0x060027E0 RID: 10208 RVA: 0x000DC80B File Offset: 0x000DAC0B
			public TransformLocalScaleValueInterpolator()
			{
			}

			// Token: 0x060027E1 RID: 10209 RVA: 0x000DC813 File Offset: 0x000DAC13
			public override void Interpolate(float percent)
			{
				this._target.localScale = this._a + this._b * percent;
			}

			// Token: 0x060027E2 RID: 10210 RVA: 0x000DC837 File Offset: 0x000DAC37
			public override void Dispose()
			{
				this._target = null;
				Pool<TransformSelector.TransformLocalScaleValueInterpolator>.Recycle(this);
			}

			// Token: 0x170004ED RID: 1261
			// (get) Token: 0x060027E3 RID: 10211 RVA: 0x000DC846 File Offset: 0x000DAC46
			public override bool isValid
			{
				get
				{
					return this._target != null;
				}
			}
		}

		// Token: 0x0200065D RID: 1629
		private class TransformLocalScaleReferenceInterpolator : InterpolatorBase<Transform, Transform>
		{
			// Token: 0x060027E4 RID: 10212 RVA: 0x000DC854 File Offset: 0x000DAC54
			public TransformLocalScaleReferenceInterpolator()
			{
			}

			// Token: 0x170004EE RID: 1262
			// (get) Token: 0x060027E5 RID: 10213 RVA: 0x000DC85C File Offset: 0x000DAC5C
			public override float length
			{
				get
				{
					return Quaternion.Angle(this._a.localRotation, this._b.localRotation);
				}
			}

			// Token: 0x060027E6 RID: 10214 RVA: 0x000DC879 File Offset: 0x000DAC79
			public override void Interpolate(float percent)
			{
				this._target.localScale = Vector3.Lerp(this._a.localScale, this._b.localScale, percent);
			}

			// Token: 0x060027E7 RID: 10215 RVA: 0x000DC8A2 File Offset: 0x000DACA2
			public override void Dispose()
			{
				this._target = null;
				Pool<TransformSelector.TransformLocalScaleReferenceInterpolator>.Recycle(this);
			}

			// Token: 0x170004EF RID: 1263
			// (get) Token: 0x060027E8 RID: 10216 RVA: 0x000DC8B1 File Offset: 0x000DACB1
			public override bool isValid
			{
				get
				{
					return this._target != null;
				}
			}
		}
	}
}
