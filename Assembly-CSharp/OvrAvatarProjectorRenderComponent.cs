using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000792 RID: 1938
public class OvrAvatarProjectorRenderComponent : OvrAvatarRenderComponent
{
	// Token: 0x060031D2 RID: 12754 RVA: 0x00105397 File Offset: 0x00103797
	public OvrAvatarProjectorRenderComponent()
	{
	}

	// Token: 0x060031D3 RID: 12755 RVA: 0x001053A0 File Offset: 0x001037A0
	internal void InitializeProjectorRender(ovrAvatarRenderPart_ProjectorRender render, Shader shader, OvrAvatarRenderComponent target)
	{
		if (shader == null)
		{
			shader = Shader.Find("OvrAvatar/AvatarSurfaceShader");
		}
		this.material = base.CreateAvatarMaterial(base.gameObject.name + "_projector", shader);
		this.material.EnableKeyword("PROJECTOR_ON");
		Renderer component = target.GetComponent<Renderer>();
		if (component != null)
		{
			component.sharedMaterials = new List<Material>(component.sharedMaterials)
			{
				this.material
			}.ToArray();
		}
	}

	// Token: 0x060031D4 RID: 12756 RVA: 0x00105430 File Offset: 0x00103830
	internal void UpdateProjectorRender(OvrAvatarComponent component, ovrAvatarRenderPart_ProjectorRender render)
	{
		OvrAvatar.ConvertTransform(render.localTransform, base.transform);
		this.material.SetMatrix("_ProjectorWorldToLocal", base.transform.worldToLocalMatrix);
		component.UpdateAvatarMaterial(this.material, render.materialState);
	}

	// Token: 0x060031D5 RID: 12757 RVA: 0x00105480 File Offset: 0x00103880
	private void OnDrawGizmos()
	{
		Vector3 from = base.transform.localToWorldMatrix.MultiplyPoint(new Vector3(-1f, -1f, -1f));
		Vector3 vector = base.transform.localToWorldMatrix.MultiplyPoint(new Vector3(1f, -1f, -1f));
		Vector3 vector2 = base.transform.localToWorldMatrix.MultiplyPoint(new Vector3(-1f, 1f, -1f));
		Vector3 vector3 = base.transform.localToWorldMatrix.MultiplyPoint(new Vector3(1f, 1f, -1f));
		Vector3 vector4 = base.transform.localToWorldMatrix.MultiplyPoint(new Vector3(-1f, -1f, 1f));
		Vector3 vector5 = base.transform.localToWorldMatrix.MultiplyPoint(new Vector3(1f, -1f, 1f));
		Vector3 vector6 = base.transform.localToWorldMatrix.MultiplyPoint(new Vector3(-1f, 1f, 1f));
		Vector3 to = base.transform.localToWorldMatrix.MultiplyPoint(new Vector3(1f, 1f, 1f));
		Gizmos.color = Color.gray;
		Gizmos.DrawLine(from, vector);
		Gizmos.DrawLine(from, vector2);
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector, vector3);
		Gizmos.DrawLine(from, vector4);
		Gizmos.DrawLine(vector, vector5);
		Gizmos.DrawLine(vector2, vector6);
		Gizmos.DrawLine(vector3, to);
		Gizmos.DrawLine(vector4, vector5);
		Gizmos.DrawLine(vector4, vector6);
		Gizmos.DrawLine(vector6, to);
		Gizmos.DrawLine(vector5, to);
	}

	// Token: 0x040025AE RID: 9646
	private Material material;
}
