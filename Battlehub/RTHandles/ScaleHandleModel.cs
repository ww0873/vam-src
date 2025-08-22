using System;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x02000109 RID: 265
	public class ScaleHandleModel : BaseHandleModel
	{
		// Token: 0x06000663 RID: 1635 RVA: 0x00029410 File Offset: 0x00027810
		public ScaleHandleModel()
		{
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0002947C File Offset: 0x0002787C
		protected override void Awake()
		{
			base.Awake();
			this.m_b1x = this.m_armature.GetChild(0);
			this.m_b1y = this.m_armature.GetChild(1);
			this.m_b1z = this.m_armature.GetChild(2);
			this.m_b2x = this.m_armature.GetChild(3);
			this.m_b2y = this.m_armature.GetChild(4);
			this.m_b2z = this.m_armature.GetChild(5);
			this.m_b3x = this.m_armature.GetChild(6);
			this.m_b3y = this.m_armature.GetChild(7);
			this.m_b3z = this.m_armature.GetChild(8);
			this.m_b0 = this.m_armature.GetChild(9);
			Renderer component = this.m_model.GetComponent<Renderer>();
			this.m_materials = component.materials;
			component.sharedMaterials = this.m_materials;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00029568 File Offset: 0x00027968
		protected override void Start()
		{
			base.Start();
			this.UpdateTransforms();
			this.SetColors();
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0002957C File Offset: 0x0002797C
		public override void SetLock(LockObject lockObj)
		{
			base.SetLock(this.m_lockObj);
			this.SetColors();
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00029590 File Offset: 0x00027990
		public override void Select(RuntimeHandleAxis axis)
		{
			base.Select(axis);
			this.SetColors();
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x000295A0 File Offset: 0x000279A0
		private void SetDefaultColors()
		{
			if (this.m_lockObj.ScaleX)
			{
				this.m_materials[this.m_xMatIndex].color = this.m_disabledColor;
				this.m_materials[this.m_xArrowMatIndex].color = this.m_disabledColor;
			}
			else
			{
				this.m_materials[this.m_xMatIndex].color = this.m_xColor;
				this.m_materials[this.m_xArrowMatIndex].color = this.m_xColor;
			}
			if (this.m_lockObj.ScaleY)
			{
				this.m_materials[this.m_yMatIndex].color = this.m_disabledColor;
				this.m_materials[this.m_yArrowMatIndex].color = this.m_disabledColor;
			}
			else
			{
				this.m_materials[this.m_yMatIndex].color = this.m_yColor;
				this.m_materials[this.m_yArrowMatIndex].color = this.m_yColor;
			}
			if (this.m_lockObj.ScaleZ)
			{
				this.m_materials[this.m_zMatIndex].color = this.m_disabledColor;
			}
			else
			{
				this.m_materials[this.m_zMatIndex].color = this.m_zColor;
				this.m_materials[this.m_zArrowMatIndex].color = this.m_zColor;
			}
			if (this.m_lockObj.IsPositionLocked)
			{
				this.m_materials[this.m_xyzMatIndex].color = this.m_disabledColor;
			}
			else
			{
				this.m_materials[this.m_xyzMatIndex].color = this.m_altColor;
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0002973C File Offset: 0x00027B3C
		private void SetColors()
		{
			this.SetDefaultColors();
			RuntimeHandleAxis selectedAxis = this.m_selectedAxis;
			switch (selectedAxis)
			{
			case RuntimeHandleAxis.X:
				if (!this.m_lockObj.ScaleX)
				{
					this.m_materials[this.m_xArrowMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_xMatIndex].color = this.m_selectionColor;
				}
				break;
			case RuntimeHandleAxis.Y:
				if (!this.m_lockObj.ScaleY)
				{
					this.m_materials[this.m_yArrowMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_yMatIndex].color = this.m_selectionColor;
				}
				break;
			default:
				if (selectedAxis == RuntimeHandleAxis.Free)
				{
					this.m_materials[this.m_xyzMatIndex].color = this.m_selectionColor;
				}
				break;
			case RuntimeHandleAxis.Z:
				if (!this.m_lockObj.ScaleZ)
				{
					this.m_materials[this.m_zArrowMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_zMatIndex].color = this.m_selectionColor;
				}
				break;
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00029867 File Offset: 0x00027C67
		public override void SetScale(Vector3 scale)
		{
			base.SetScale(scale);
			this.m_scale = scale;
			this.UpdateTransforms();
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00029880 File Offset: 0x00027C80
		private void UpdateTransforms()
		{
			this.m_radius = Mathf.Max(0.01f, this.m_radius);
			Vector3 a = base.transform.rotation * Vector3.right * base.transform.localScale.x;
			Vector3 a2 = base.transform.rotation * Vector3.up * base.transform.localScale.y;
			Vector3 a3 = base.transform.rotation * Vector3.forward * base.transform.localScale.z;
			Vector3 position = base.transform.position;
			float num = this.m_radius / 0.05f;
			float d = this.m_arrowRadius / 0.1f;
			this.m_b0.localScale = Vector3.one * d * 2f;
			Transform b3z = this.m_b3z;
			Vector3 vector = Vector3.one * d;
			this.m_b3x.localScale = vector;
			vector = vector;
			this.m_b3y.localScale = vector;
			b3z.localScale = vector;
			this.m_b1x.position = position + a * this.m_arrowRadius;
			this.m_b1y.position = position + a2 * this.m_arrowRadius;
			this.m_b1z.position = position + a3 * this.m_arrowRadius;
			this.m_b2x.position = position + a * (this.m_length * this.m_scale.x - this.m_arrowRadius);
			this.m_b2y.position = position + a2 * (this.m_length * this.m_scale.y - this.m_arrowRadius);
			this.m_b2z.position = position + a3 * (this.m_length * this.m_scale.z - this.m_arrowRadius);
			Transform b2x = this.m_b2x;
			vector = new Vector3(1f, num, num);
			this.m_b1x.localScale = vector;
			b2x.localScale = vector;
			Transform b2y = this.m_b2y;
			vector = new Vector3(num, num, 1f);
			this.m_b1y.localScale = vector;
			b2y.localScale = vector;
			Transform b2z = this.m_b2z;
			vector = new Vector3(num, 1f, num);
			this.m_b1z.localScale = vector;
			b2z.localScale = vector;
			this.m_b3x.position = position + a * this.m_length * this.m_scale.x;
			this.m_b3y.position = position + a2 * this.m_length * this.m_scale.y;
			this.m_b3z.position = position + a3 * this.m_length * this.m_scale.z;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00029BAC File Offset: 0x00027FAC
		protected override void Update()
		{
			base.Update();
			if (this.m_prevRadius != this.m_radius || this.m_prevLength != this.m_length || this.m_prevArrowRadius != this.m_arrowRadius)
			{
				this.m_prevRadius = this.m_radius;
				this.m_prevLength = this.m_length;
				this.m_prevArrowRadius = this.m_arrowRadius;
				this.UpdateTransforms();
			}
		}

		// Token: 0x04000611 RID: 1553
		[SerializeField]
		private int m_xMatIndex;

		// Token: 0x04000612 RID: 1554
		[SerializeField]
		private int m_yMatIndex = 1;

		// Token: 0x04000613 RID: 1555
		[SerializeField]
		private int m_zMatIndex = 2;

		// Token: 0x04000614 RID: 1556
		[SerializeField]
		private int m_xArrowMatIndex = 3;

		// Token: 0x04000615 RID: 1557
		[SerializeField]
		private int m_yArrowMatIndex = 4;

		// Token: 0x04000616 RID: 1558
		[SerializeField]
		private int m_zArrowMatIndex = 5;

		// Token: 0x04000617 RID: 1559
		[SerializeField]
		private int m_xyzMatIndex = 6;

		// Token: 0x04000618 RID: 1560
		[SerializeField]
		private Transform m_armature;

		// Token: 0x04000619 RID: 1561
		[SerializeField]
		private Transform m_model;

		// Token: 0x0400061A RID: 1562
		private Transform m_b1x;

		// Token: 0x0400061B RID: 1563
		private Transform m_b2x;

		// Token: 0x0400061C RID: 1564
		private Transform m_b3x;

		// Token: 0x0400061D RID: 1565
		private Transform m_b1y;

		// Token: 0x0400061E RID: 1566
		private Transform m_b2y;

		// Token: 0x0400061F RID: 1567
		private Transform m_b3y;

		// Token: 0x04000620 RID: 1568
		private Transform m_b1z;

		// Token: 0x04000621 RID: 1569
		private Transform m_b2z;

		// Token: 0x04000622 RID: 1570
		private Transform m_b3z;

		// Token: 0x04000623 RID: 1571
		private Transform m_b0;

		// Token: 0x04000624 RID: 1572
		[SerializeField]
		private float m_radius = 0.05f;

		// Token: 0x04000625 RID: 1573
		[SerializeField]
		private float m_length = 1f;

		// Token: 0x04000626 RID: 1574
		[SerializeField]
		private float m_arrowRadius = 0.1f;

		// Token: 0x04000627 RID: 1575
		private const float DefaultRadius = 0.05f;

		// Token: 0x04000628 RID: 1576
		private const float DefaultLength = 1f;

		// Token: 0x04000629 RID: 1577
		private const float DefaultArrowRadius = 0.1f;

		// Token: 0x0400062A RID: 1578
		private Material[] m_materials;

		// Token: 0x0400062B RID: 1579
		private Vector3 m_scale = Vector3.one;

		// Token: 0x0400062C RID: 1580
		private float m_prevRadius;

		// Token: 0x0400062D RID: 1581
		private float m_prevLength;

		// Token: 0x0400062E RID: 1582
		private float m_prevArrowRadius;
	}
}
