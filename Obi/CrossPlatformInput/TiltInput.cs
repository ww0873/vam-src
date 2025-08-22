using System;
using UnityEngine;

namespace Obi.CrossPlatformInput
{
	// Token: 0x0200037C RID: 892
	public class TiltInput : MonoBehaviour
	{
		// Token: 0x06001654 RID: 5716 RVA: 0x0007DEC7 File Offset: 0x0007C2C7
		public TiltInput()
		{
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x0007DEDA File Offset: 0x0007C2DA
		private void OnEnable()
		{
			if (this.mapping.type == TiltInput.AxisMapping.MappingType.NamedAxis)
			{
				this.m_SteerAxis = new CrossPlatformInputManager.VirtualAxis(this.mapping.axisName);
				CrossPlatformInputManager.RegisterVirtualAxis(this.m_SteerAxis);
			}
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x0007DF10 File Offset: 0x0007C310
		private void Update()
		{
			float value = 0f;
			if (Input.acceleration != Vector3.zero)
			{
				TiltInput.AxisOptions axisOptions = this.tiltAroundAxis;
				if (axisOptions != TiltInput.AxisOptions.ForwardAxis)
				{
					if (axisOptions == TiltInput.AxisOptions.SidewaysAxis)
					{
						value = Mathf.Atan2(Input.acceleration.z, -Input.acceleration.y) * 57.29578f + this.centreAngleOffset;
					}
				}
				else
				{
					value = Mathf.Atan2(Input.acceleration.x, -Input.acceleration.y) * 57.29578f + this.centreAngleOffset;
				}
			}
			float num = Mathf.InverseLerp(-this.fullTiltAngle, this.fullTiltAngle, value) * 2f - 1f;
			switch (this.mapping.type)
			{
			case TiltInput.AxisMapping.MappingType.NamedAxis:
				this.m_SteerAxis.Update(num);
				break;
			case TiltInput.AxisMapping.MappingType.MousePositionX:
				CrossPlatformInputManager.SetVirtualMousePositionX(num * (float)Screen.width);
				break;
			case TiltInput.AxisMapping.MappingType.MousePositionY:
				CrossPlatformInputManager.SetVirtualMousePositionY(num * (float)Screen.width);
				break;
			case TiltInput.AxisMapping.MappingType.MousePositionZ:
				CrossPlatformInputManager.SetVirtualMousePositionZ(num * (float)Screen.width);
				break;
			}
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x0007E04D File Offset: 0x0007C44D
		private void OnDisable()
		{
			this.m_SteerAxis.Remove();
		}

		// Token: 0x04001271 RID: 4721
		public TiltInput.AxisMapping mapping;

		// Token: 0x04001272 RID: 4722
		public TiltInput.AxisOptions tiltAroundAxis;

		// Token: 0x04001273 RID: 4723
		public float fullTiltAngle = 25f;

		// Token: 0x04001274 RID: 4724
		public float centreAngleOffset;

		// Token: 0x04001275 RID: 4725
		private CrossPlatformInputManager.VirtualAxis m_SteerAxis;

		// Token: 0x0200037D RID: 893
		public enum AxisOptions
		{
			// Token: 0x04001277 RID: 4727
			ForwardAxis,
			// Token: 0x04001278 RID: 4728
			SidewaysAxis
		}

		// Token: 0x0200037E RID: 894
		[Serializable]
		public class AxisMapping
		{
			// Token: 0x06001658 RID: 5720 RVA: 0x0007E05A File Offset: 0x0007C45A
			public AxisMapping()
			{
			}

			// Token: 0x04001279 RID: 4729
			public TiltInput.AxisMapping.MappingType type;

			// Token: 0x0400127A RID: 4730
			public string axisName;

			// Token: 0x0200037F RID: 895
			public enum MappingType
			{
				// Token: 0x0400127C RID: 4732
				NamedAxis,
				// Token: 0x0400127D RID: 4733
				MousePositionX,
				// Token: 0x0400127E RID: 4734
				MousePositionY,
				// Token: 0x0400127F RID: 4735
				MousePositionZ
			}
		}
	}
}
