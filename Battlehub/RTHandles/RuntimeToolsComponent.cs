using System;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x02000107 RID: 263
	public class RuntimeToolsComponent : MonoBehaviour
	{
		// Token: 0x06000652 RID: 1618 RVA: 0x00028CB2 File Offset: 0x000270B2
		public RuntimeToolsComponent()
		{
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00028CEA File Offset: 0x000270EA
		private void Awake()
		{
			UnityEditorToolsListener.ToolChanged += this.OnUnityEditorToolChanged;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00028CFD File Offset: 0x000270FD
		private void Start()
		{
			RuntimeTools.Current = RuntimeTool.Move;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00028D05 File Offset: 0x00027105
		private void OnDestroy()
		{
			UnityEditorToolsListener.ToolChanged -= this.OnUnityEditorToolChanged;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00028D18 File Offset: 0x00027118
		private void Update()
		{
			if (RuntimeTools.ActiveTool != null)
			{
				return;
			}
			bool flag = RuntimeEditorApplication.IsActiveWindow(RuntimeWindowType.GameView);
			if (!RuntimeTools.IsViewing && !flag)
			{
				if (InputController.GetKeyDown(this.ViewToolKey))
				{
					RuntimeTools.Current = RuntimeTool.View;
				}
				else if (InputController.GetKeyDown(this.MoveToolKey))
				{
					RuntimeTools.Current = RuntimeTool.Move;
				}
				else if (InputController.GetKeyDown(this.RotateToolKey))
				{
					RuntimeTools.Current = RuntimeTool.Rotate;
				}
				else if (InputController.GetKeyDown(this.ScaleToolKey))
				{
					RuntimeTools.Current = RuntimeTool.Scale;
				}
				if (InputController.GetKeyDown(this.PivotRotationKey))
				{
					if (RuntimeTools.PivotRotation == RuntimePivotRotation.Local)
					{
						RuntimeTools.PivotRotation = RuntimePivotRotation.Global;
					}
					else
					{
						RuntimeTools.PivotRotation = RuntimePivotRotation.Local;
					}
				}
				if (InputController.GetKeyDown(this.PivotModeKey) && !InputController.GetKey(KeyCode.LeftControl) && !InputController.GetKey(KeyCode.LeftShift))
				{
					if (RuntimeTools.PivotMode == RuntimePivotMode.Center)
					{
						RuntimeTools.PivotMode = RuntimePivotMode.Pivot;
					}
					else
					{
						RuntimeTools.PivotMode = RuntimePivotMode.Center;
					}
				}
			}
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00028E2D File Offset: 0x0002722D
		private void OnUnityEditorToolChanged()
		{
		}

		// Token: 0x04000603 RID: 1539
		public KeyCode ViewToolKey = KeyCode.Q;

		// Token: 0x04000604 RID: 1540
		public KeyCode MoveToolKey = KeyCode.W;

		// Token: 0x04000605 RID: 1541
		public KeyCode RotateToolKey = KeyCode.E;

		// Token: 0x04000606 RID: 1542
		public KeyCode ScaleToolKey = KeyCode.R;

		// Token: 0x04000607 RID: 1543
		public KeyCode PivotRotationKey = KeyCode.X;

		// Token: 0x04000608 RID: 1544
		public KeyCode PivotModeKey = KeyCode.Z;
	}
}
