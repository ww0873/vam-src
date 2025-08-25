using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Weelco.VRInput
{
	// Token: 0x02000592 RID: 1426
	public abstract class VRInputModule : BaseInputModule
	{
		// Token: 0x060023D3 RID: 9171 RVA: 0x000CF1C8 File Offset: 0x000CD5C8
		protected VRInputModule()
		{
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060023D4 RID: 9172 RVA: 0x000CF1D0 File Offset: 0x000CD5D0
		public static VRInputModule instance
		{
			get
			{
				return VRInputModule._instance;
			}
		}

		// Token: 0x060023D5 RID: 9173
		public abstract void AddController(IUIPointer controller);

		// Token: 0x060023D6 RID: 9174
		public abstract void RemoveController(IUIPointer controller);

		// Token: 0x060023D7 RID: 9175 RVA: 0x000CF1D7 File Offset: 0x000CD5D7
		protected override void Awake()
		{
			base.Awake();
			if (VRInputModule._instance != null)
			{
				Debug.LogWarning("Trying to instantiate multiple VRInputModule::" + this);
				UnityEngine.Object.DestroyImmediate(base.gameObject);
			}
			VRInputModule._instance = this;
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x000CF210 File Offset: 0x000CD610
		protected override void Start()
		{
			base.Start();
			this.UICamera = new GameObject("DummyCamera").AddComponent<Camera>();
			this.UICamera.clearFlags = CameraClearFlags.Nothing;
			this.UICamera.enabled = false;
			this.UICamera.fieldOfView = 5f;
			this.UICamera.nearClipPlane = 0.01f;
			Canvas[] array = Resources.FindObjectsOfTypeAll<Canvas>();
			foreach (Canvas canvas in array)
			{
				canvas.worldCamera = this.UICamera;
			}
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x000CF29C File Offset: 0x000CD69C
		protected void UpdateCameraPosition(IUIPointer controller)
		{
			this.UICamera.transform.position = controller.target.transform.position;
			this.UICamera.transform.rotation = controller.target.transform.rotation;
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x000CF2E9 File Offset: 0x000CD6E9
		protected Vector2 GetCameraSize()
		{
			return new Vector2((float)this.UICamera.pixelWidth, (float)this.UICamera.pixelHeight);
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x000CF308 File Offset: 0x000CD708
		protected void ClearSelection()
		{
			if (base.eventSystem.currentSelectedGameObject)
			{
				base.eventSystem.SetSelectedGameObject(null);
			}
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x000CF32B File Offset: 0x000CD72B
		// Note: this type is marked as 'beforefieldinit'.
		static VRInputModule()
		{
		}

		// Token: 0x04001E32 RID: 7730
		private static VRInputModule _instance;

		// Token: 0x04001E33 RID: 7731
		private Camera UICamera;
	}
}
