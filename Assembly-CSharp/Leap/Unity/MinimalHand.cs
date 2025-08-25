using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000757 RID: 1879
	public class MinimalHand : HandModelBase
	{
		// Token: 0x06003057 RID: 12375 RVA: 0x000FB1C0 File Offset: 0x000F95C0
		public MinimalHand()
		{
		}

		// Token: 0x06003058 RID: 12376 RVA: 0x000FB1DE File Offset: 0x000F95DE
		public override bool SupportsEditorPersistence()
		{
			return true;
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06003059 RID: 12377 RVA: 0x000FB1E1 File Offset: 0x000F95E1
		// (set) Token: 0x0600305A RID: 12378 RVA: 0x000FB1E9 File Offset: 0x000F95E9
		public override Chirality Handedness
		{
			get
			{
				return this._handedness;
			}
			set
			{
				this._handedness = value;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x0600305B RID: 12379 RVA: 0x000FB1F2 File Offset: 0x000F95F2
		public override ModelType HandModelType
		{
			get
			{
				return ModelType.Graphics;
			}
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x000FB1F5 File Offset: 0x000F95F5
		public override void SetLeapHand(Hand hand)
		{
			this._hand = hand;
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x000FB1FE File Offset: 0x000F95FE
		public override Hand GetLeapHand()
		{
			return this._hand;
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x000FB208 File Offset: 0x000F9608
		public override void InitHand()
		{
			this._joints = new Transform[20];
			for (int i = 0; i < 20; i++)
			{
				this._joints[i] = this.createRenderer("Joint", this._jointMesh, this._jointScale, this._jointMat);
			}
			this._palm = this.createRenderer("Palm", this._palmMesh, this._palmScale, this._palmMat);
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x000FB280 File Offset: 0x000F9680
		public override void UpdateHand()
		{
			List<Finger> fingers = this._hand.Fingers;
			int num = 0;
			for (int i = 0; i < 5; i++)
			{
				Finger finger = fingers[i];
				for (int j = 0; j < 4; j++)
				{
					this._joints[num++].position = finger.Bone((Bone.BoneType)j).NextJoint.ToVector3();
				}
			}
			this._palm.position = this._hand.PalmPosition.ToVector3();
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x000FB30C File Offset: 0x000F970C
		private Transform createRenderer(string name, Mesh mesh, float scale, Material mat)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.AddComponent<MeshFilter>().mesh = mesh;
			gameObject.AddComponent<MeshRenderer>().sharedMaterial = mat;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localScale = Vector3.one * scale;
			return gameObject.transform;
		}

		// Token: 0x04002444 RID: 9284
		[SerializeField]
		private Chirality _handedness;

		// Token: 0x04002445 RID: 9285
		[SerializeField]
		private Mesh _palmMesh;

		// Token: 0x04002446 RID: 9286
		[SerializeField]
		private float _palmScale = 0.02f;

		// Token: 0x04002447 RID: 9287
		[SerializeField]
		private Material _palmMat;

		// Token: 0x04002448 RID: 9288
		[SerializeField]
		private Mesh _jointMesh;

		// Token: 0x04002449 RID: 9289
		[SerializeField]
		private float _jointScale = 0.01f;

		// Token: 0x0400244A RID: 9290
		[SerializeField]
		private Material _jointMat;

		// Token: 0x0400244B RID: 9291
		private Hand _hand;

		// Token: 0x0400244C RID: 9292
		private Transform _palm;

		// Token: 0x0400244D RID: 9293
		private Transform[] _joints;
	}
}
