using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006CC RID: 1740
	public abstract class AbstractHoldDetector : Detector
	{
		// Token: 0x060029DF RID: 10719 RVA: 0x000E2724 File Offset: 0x000E0B24
		protected AbstractHoldDetector()
		{
		}

		// Token: 0x060029E0 RID: 10720
		protected abstract void ensureUpToDate();

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x060029E1 RID: 10721 RVA: 0x000E2799 File Offset: 0x000E0B99
		// (set) Token: 0x060029E2 RID: 10722 RVA: 0x000E27A1 File Offset: 0x000E0BA1
		public HandModelBase HandModel
		{
			get
			{
				return this._handModel;
			}
			set
			{
				this._handModel = value;
			}
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x000E27AC File Offset: 0x000E0BAC
		protected virtual void Awake()
		{
			if (base.GetComponent<HandModelBase>() != null && this.ControlsTransform)
			{
				Debug.LogWarning("Detector should not be control the HandModelBase's transform. Either attach it to its own transform or set ControlsTransform to false.");
			}
			if (this._handModel == null)
			{
				this._handModel = base.GetComponentInParent<HandModelBase>();
				if (this._handModel == null)
				{
					Debug.LogWarning("The HandModel field of Detector was unassigned and the detector has been disabled.");
					base.enabled = false;
				}
			}
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x000E281E File Offset: 0x000E0C1E
		protected virtual void Update()
		{
			this.ensureUpToDate();
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x060029E5 RID: 10725 RVA: 0x000E2826 File Offset: 0x000E0C26
		public virtual bool IsHolding
		{
			get
			{
				this.ensureUpToDate();
				return base.IsActive;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x060029E6 RID: 10726 RVA: 0x000E2834 File Offset: 0x000E0C34
		public virtual bool DidChangeFromLastFrame
		{
			get
			{
				this.ensureUpToDate();
				return this._didChange;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x060029E7 RID: 10727 RVA: 0x000E2842 File Offset: 0x000E0C42
		public virtual bool DidStartHold
		{
			get
			{
				this.ensureUpToDate();
				return this.DidChangeFromLastFrame && this.IsHolding;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x060029E8 RID: 10728 RVA: 0x000E285E File Offset: 0x000E0C5E
		public virtual bool DidRelease
		{
			get
			{
				this.ensureUpToDate();
				return this.DidChangeFromLastFrame && !this.IsHolding;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x060029E9 RID: 10729 RVA: 0x000E287D File Offset: 0x000E0C7D
		public float LastHoldTime
		{
			get
			{
				this.ensureUpToDate();
				return this._lastHoldTime;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x060029EA RID: 10730 RVA: 0x000E288B File Offset: 0x000E0C8B
		public float LastReleaseTime
		{
			get
			{
				this.ensureUpToDate();
				return this._lastReleaseTime;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060029EB RID: 10731 RVA: 0x000E2899 File Offset: 0x000E0C99
		public Vector3 Position
		{
			get
			{
				this.ensureUpToDate();
				return this._position;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060029EC RID: 10732 RVA: 0x000E28A7 File Offset: 0x000E0CA7
		public Vector3 LastActivePosition
		{
			get
			{
				return this._lastPosition;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060029ED RID: 10733 RVA: 0x000E28AF File Offset: 0x000E0CAF
		public Quaternion Rotation
		{
			get
			{
				this.ensureUpToDate();
				return this._rotation;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060029EE RID: 10734 RVA: 0x000E28BD File Offset: 0x000E0CBD
		public Quaternion LastActiveRotation
		{
			get
			{
				return this._lastRotation;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060029EF RID: 10735 RVA: 0x000E28C5 File Offset: 0x000E0CC5
		public Vector3 Direction
		{
			get
			{
				return this._direction;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060029F0 RID: 10736 RVA: 0x000E28CD File Offset: 0x000E0CCD
		public Vector3 LastActiveDirection
		{
			get
			{
				return this._lastDirection;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060029F1 RID: 10737 RVA: 0x000E28D5 File Offset: 0x000E0CD5
		public Vector3 Normal
		{
			get
			{
				return this._normal;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060029F2 RID: 10738 RVA: 0x000E28DD File Offset: 0x000E0CDD
		public Vector3 LastActiveNormal
		{
			get
			{
				return this._lastNormal;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060029F3 RID: 10739 RVA: 0x000E28E5 File Offset: 0x000E0CE5
		public float Distance
		{
			get
			{
				return this._distance;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060029F4 RID: 10740 RVA: 0x000E28ED File Offset: 0x000E0CED
		public float LastActiveDistance
		{
			get
			{
				return this._lastDistance;
			}
		}

		// Token: 0x060029F5 RID: 10741 RVA: 0x000E28F8 File Offset: 0x000E0CF8
		protected virtual void changeState(bool shouldBeActive)
		{
			bool isActive = base.IsActive;
			if (shouldBeActive)
			{
				this._lastHoldTime = Time.time;
				this.Activate();
			}
			else
			{
				this._lastReleaseTime = Time.time;
				this.Deactivate();
			}
			if (isActive != base.IsActive)
			{
				this._didChange = true;
			}
		}

		// Token: 0x0400220B RID: 8715
		[SerializeField]
		protected HandModelBase _handModel;

		// Token: 0x0400220C RID: 8716
		[Tooltip("Whether to change the transform of the parent object.")]
		public bool ControlsTransform = true;

		// Token: 0x0400220D RID: 8717
		[Tooltip("Draw this detector's Gizmos, if any. (Gizmos must be on in Unity edtor, too.)")]
		public bool ShowGizmos = true;

		// Token: 0x0400220E RID: 8718
		protected int _lastUpdateFrame = -1;

		// Token: 0x0400220F RID: 8719
		protected bool _didChange;

		// Token: 0x04002210 RID: 8720
		protected Vector3 _position;

		// Token: 0x04002211 RID: 8721
		protected Quaternion _rotation;

		// Token: 0x04002212 RID: 8722
		protected Vector3 _direction = Vector3.forward;

		// Token: 0x04002213 RID: 8723
		protected Vector3 _normal = Vector3.up;

		// Token: 0x04002214 RID: 8724
		protected float _distance;

		// Token: 0x04002215 RID: 8725
		protected float _lastHoldTime;

		// Token: 0x04002216 RID: 8726
		protected float _lastReleaseTime;

		// Token: 0x04002217 RID: 8727
		protected Vector3 _lastPosition = Vector3.zero;

		// Token: 0x04002218 RID: 8728
		protected Quaternion _lastRotation = Quaternion.identity;

		// Token: 0x04002219 RID: 8729
		protected Vector3 _lastDirection = Vector3.forward;

		// Token: 0x0400221A RID: 8730
		protected Vector3 _lastNormal = Vector3.up;

		// Token: 0x0400221B RID: 8731
		protected float _lastDistance = 1f;
	}
}
