using System;
using MVR;
using UnityEngine;

// Token: 0x02000C8E RID: 3214
public class DelayedSkinMeshRenderer : ObjectAllocator
{
	// Token: 0x060060F8 RID: 24824 RVA: 0x0024A6B1 File Offset: 0x00248AB1
	public DelayedSkinMeshRenderer()
	{
	}

	// Token: 0x060060F9 RID: 24825 RVA: 0x0024A6C0 File Offset: 0x00248AC0
	private void LateUpdate()
	{
		this.Init();
		if (this.skinnedMeshRenderer != null)
		{
			if (this.skinMesh == this.mesh1)
			{
				this.skinMesh = this.mesh2;
				if (this.delayCount == 1)
				{
					this.drawMesh = this.mesh1;
				}
				else
				{
					this.drawMesh = this.mesh3;
				}
			}
			else if (this.skinMesh == this.mesh2)
			{
				this.skinMesh = this.mesh3;
				if (this.delayCount == 1)
				{
					this.drawMesh = this.mesh2;
				}
				else
				{
					this.drawMesh = this.mesh1;
				}
			}
			else
			{
				this.skinMesh = this.mesh1;
				if (this.delayCount == 1)
				{
					this.drawMesh = this.mesh3;
				}
				else
				{
					this.drawMesh = this.mesh2;
				}
			}
			this.skinnedMeshRenderer.BakeMesh(this.skinMesh);
			for (int i = 0; i < this.skinnedMeshRenderer.materials.Length; i++)
			{
				Material material = this.skinnedMeshRenderer.materials[i];
				if (material != null)
				{
					Graphics.DrawMesh(this.drawMesh, this.lastMatrix, material, base.gameObject.layer, null, i, null, this.skinnedMeshRenderer.shadowCastingMode, this.skinnedMeshRenderer.receiveShadows);
				}
			}
		}
		this.lastMatrix = base.transform.localToWorldMatrix;
	}

	// Token: 0x060060FA RID: 24826 RVA: 0x0024A84C File Offset: 0x00248C4C
	private void Init()
	{
		if (!this.wasInit && this.skinnedMeshRenderer != null)
		{
			this.wasInit = true;
			this.lastMatrix = base.transform.localToWorldMatrix;
			this.skinnedMeshRenderer.enabled = false;
			this.mesh1 = UnityEngine.Object.Instantiate<Mesh>(this.skinnedMeshRenderer.sharedMesh);
			base.RegisterAllocatedObject(this.mesh1);
			this.mesh2 = UnityEngine.Object.Instantiate<Mesh>(this.skinnedMeshRenderer.sharedMesh);
			base.RegisterAllocatedObject(this.mesh2);
			this.mesh3 = UnityEngine.Object.Instantiate<Mesh>(this.skinnedMeshRenderer.sharedMesh);
			base.RegisterAllocatedObject(this.mesh3);
		}
	}

	// Token: 0x04005085 RID: 20613
	public SkinnedMeshRenderer skinnedMeshRenderer;

	// Token: 0x04005086 RID: 20614
	private Mesh mesh1;

	// Token: 0x04005087 RID: 20615
	private Mesh mesh2;

	// Token: 0x04005088 RID: 20616
	private Mesh mesh3;

	// Token: 0x04005089 RID: 20617
	private Mesh skinMesh;

	// Token: 0x0400508A RID: 20618
	private Mesh drawMesh;

	// Token: 0x0400508B RID: 20619
	protected Matrix4x4 lastMatrix;

	// Token: 0x0400508C RID: 20620
	public int delayCount = 1;

	// Token: 0x0400508D RID: 20621
	private bool wasInit;
}
