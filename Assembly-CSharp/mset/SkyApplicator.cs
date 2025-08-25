using System;
using System.Collections.Generic;
using UnityEngine;

namespace mset
{
	// Token: 0x02000334 RID: 820
	[RequireComponent(typeof(Sky))]
	public class SkyApplicator : MonoBehaviour
	{
		// Token: 0x060013B8 RID: 5048 RVA: 0x00070DD0 File Offset: 0x0006F1D0
		public SkyApplicator()
		{
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x00070E27 File Offset: 0x0006F227
		// (set) Token: 0x060013BA RID: 5050 RVA: 0x00070E2F File Offset: 0x0006F22F
		public Bounds TriggerDimensions
		{
			get
			{
				return this.triggerDimensions;
			}
			set
			{
				this.HasChanged = true;
				this.triggerDimensions = value;
			}
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00070E3F File Offset: 0x0006F23F
		private void Awake()
		{
			this.TargetSky = base.GetComponent<Sky>();
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x00070E4D File Offset: 0x0006F24D
		private void Start()
		{
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00070E50 File Offset: 0x0006F250
		private void OnEnable()
		{
			base.gameObject.isStatic = true;
			base.transform.root.gameObject.isStatic = true;
			this.LastPosition = base.transform.position;
			if (this.ParentApplicator == null && base.transform.parent != null && base.transform.parent.GetComponent<SkyApplicator>() != null)
			{
				this.ParentApplicator = base.transform.parent.GetComponent<SkyApplicator>();
			}
			if (this.ParentApplicator != null)
			{
				this.ParentApplicator.Children.Add(this);
			}
			else
			{
				SkyManager skyManager = SkyManager.Get();
				if (skyManager != null)
				{
					skyManager.RegisterApplicator(this);
				}
			}
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00070F28 File Offset: 0x0006F328
		private void OnDisable()
		{
			if (this.ParentApplicator != null)
			{
				this.ParentApplicator.Children.Remove(this);
			}
			SkyManager skyManager = SkyManager.Get();
			if (skyManager)
			{
				skyManager.UnregisterApplicator(this, this.AffectedRenderers);
				this.AffectedRenderers.Clear();
			}
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00070F84 File Offset: 0x0006F384
		public void RemoveRenderer(Renderer rend)
		{
			if (this.AffectedRenderers.Contains(rend))
			{
				this.AffectedRenderers.Remove(rend);
				SkyAnchor component = rend.GetComponent<SkyAnchor>();
				if (component && component.CurrentApplicator == this)
				{
					component.CurrentApplicator = null;
				}
			}
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x00070FDC File Offset: 0x0006F3DC
		public void AddRenderer(Renderer rend)
		{
			SkyAnchor component = rend.GetComponent<SkyAnchor>();
			if (component != null)
			{
				if (component.CurrentApplicator != null)
				{
					component.CurrentApplicator.RemoveRenderer(rend);
				}
				component.CurrentApplicator = this;
			}
			this.AffectedRenderers.Add(rend);
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x00071030 File Offset: 0x0006F430
		public bool ApplyInside(Renderer rend)
		{
			if (this.TargetSky == null || !this.TriggerIsActive)
			{
				return false;
			}
			SkyAnchor component = rend.gameObject.GetComponent<SkyAnchor>();
			if (component && component.BindType == SkyAnchor.AnchorBindType.TargetSky && component.AnchorSky == this.TargetSky)
			{
				this.TargetSky.Apply(rend, 0);
				component.Apply();
				return true;
			}
			foreach (SkyApplicator skyApplicator in this.Children)
			{
				if (skyApplicator.ApplyInside(rend))
				{
					return true;
				}
			}
			Vector3 point = rend.bounds.center;
			if (component)
			{
				component.GetCenter(ref point);
			}
			point = base.transform.worldToLocalMatrix.MultiplyPoint(point);
			if (this.TriggerDimensions.Contains(point))
			{
				this.TargetSky.Apply(rend, 0);
				return true;
			}
			return false;
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x00071168 File Offset: 0x0006F568
		public bool RendererInside(Renderer rend)
		{
			SkyAnchor skyAnchor = rend.gameObject.GetComponent<SkyAnchor>();
			if (skyAnchor && skyAnchor.BindType == SkyAnchor.AnchorBindType.TargetSky && skyAnchor.AnchorSky == this.TargetSky)
			{
				this.AddRenderer(rend);
				skyAnchor.Apply();
				return true;
			}
			if (!this.TriggerIsActive)
			{
				return false;
			}
			foreach (SkyApplicator skyApplicator in this.Children)
			{
				if (skyApplicator.RendererInside(rend))
				{
					return true;
				}
			}
			if (skyAnchor == null)
			{
				skyAnchor = (rend.gameObject.AddComponent(typeof(SkyAnchor)) as SkyAnchor);
			}
			skyAnchor.GetCenter(ref this._center);
			this._center = base.transform.worldToLocalMatrix.MultiplyPoint(this._center);
			if (this.TriggerDimensions.Contains(this._center))
			{
				if (!this.AffectedRenderers.Contains(rend))
				{
					this.AddRenderer(rend);
					if (!skyAnchor.HasLocalSky)
					{
						skyAnchor.SnapToSky(SkyManager.Get().GlobalSky);
					}
					skyAnchor.BlendToSky(this.TargetSky);
				}
				return true;
			}
			this.RemoveRenderer(rend);
			return false;
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x000712E0 File Offset: 0x0006F6E0
		private void LateUpdate()
		{
			if (this.TargetSky.Dirty)
			{
				foreach (Renderer renderer in this.AffectedRenderers)
				{
					if (!(renderer == null))
					{
						this.TargetSky.Apply(renderer, 0);
					}
				}
				this.TargetSky.Dirty = false;
			}
			if (base.transform.position != this.LastPosition)
			{
				this.HasChanged = true;
			}
		}

		// Token: 0x04001115 RID: 4373
		public Sky TargetSky;

		// Token: 0x04001116 RID: 4374
		public bool TriggerIsActive = true;

		// Token: 0x04001117 RID: 4375
		[SerializeField]
		private Bounds triggerDimensions = new Bounds(Vector3.zero, Vector3.one);

		// Token: 0x04001118 RID: 4376
		public bool HasChanged = true;

		// Token: 0x04001119 RID: 4377
		public SkyApplicator ParentApplicator;

		// Token: 0x0400111A RID: 4378
		public List<SkyApplicator> Children = new List<SkyApplicator>();

		// Token: 0x0400111B RID: 4379
		private HashSet<Renderer> AffectedRenderers = new HashSet<Renderer>();

		// Token: 0x0400111C RID: 4380
		private Vector3 LastPosition = Vector3.zero;

		// Token: 0x0400111D RID: 4381
		private Vector3 _center;
	}
}
