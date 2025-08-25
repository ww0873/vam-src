using System;
using System.Collections.Generic;
using Leap.Unity.Infix;
using UnityEngine;

namespace Leap.Unity.RuntimeGizmos
{
	// Token: 0x02000744 RID: 1860
	public class RuntimeGizmoDrawer
	{
		// Token: 0x06002D69 RID: 11625 RVA: 0x000F2160 File Offset: 0x000F0560
		public RuntimeGizmoDrawer()
		{
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06002D6A RID: 11626 RVA: 0x000F21E8 File Offset: 0x000F05E8
		// (set) Token: 0x06002D6B RID: 11627 RVA: 0x000F2208 File Offset: 0x000F0608
		public Shader gizmoShader
		{
			get
			{
				if (this._gizmoMaterial == null)
				{
					return null;
				}
				return this._gizmoMaterial.shader;
			}
			set
			{
				if (this._gizmoMaterial == null)
				{
					this._gizmoMaterial = new Material(value);
					this._gizmoMaterial.name = "Runtime Gizmo Material";
					this._gizmoMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				else
				{
					this._gizmoMaterial.shader = value;
				}
			}
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x000F2260 File Offset: 0x000F0660
		public void BeginGuard()
		{
			this._operationCountOnGuard = this._operations.Count;
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x000F2274 File Offset: 0x000F0674
		public void EndGuard()
		{
			bool flag = this._operations.Count > this._operationCountOnGuard;
			this._operationCountOnGuard = -1;
			if (flag)
			{
				Debug.LogError("New gizmos were drawn to the front buffer!  Make sure to never keep a reference to a Drawer, always get a new one every time you want to start drawing.");
			}
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000F22AC File Offset: 0x000F06AC
		public void RelativeTo(Transform transform)
		{
			this.matrix = transform.localToWorldMatrix;
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000F22BA File Offset: 0x000F06BA
		public void PushMatrix()
		{
			this._matrixStack.Push(this._currMatrix);
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x000F22CD File Offset: 0x000F06CD
		public void PopMatrix()
		{
			this.matrix = this._matrixStack.Pop();
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x000F22E0 File Offset: 0x000F06E0
		public void ResetMatrixAndColorState()
		{
			this.matrix = Matrix4x4.identity;
			this.color = Color.white;
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06002D72 RID: 11634 RVA: 0x000F22F8 File Offset: 0x000F06F8
		// (set) Token: 0x06002D73 RID: 11635 RVA: 0x000F2300 File Offset: 0x000F0700
		public Color color
		{
			get
			{
				return this._currColor;
			}
			set
			{
				if (this._currColor == value)
				{
					return;
				}
				this._currColor = value;
				this._operations.Add(RuntimeGizmoDrawer.OperationType.SetColor);
				this._colors.Add(this._currColor);
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06002D74 RID: 11636 RVA: 0x000F2338 File Offset: 0x000F0738
		// (set) Token: 0x06002D75 RID: 11637 RVA: 0x000F2340 File Offset: 0x000F0740
		public Matrix4x4 matrix
		{
			get
			{
				return this._currMatrix;
			}
			set
			{
				if (this._currMatrix == value)
				{
					return;
				}
				this._currMatrix = value;
				this._operations.Add(RuntimeGizmoDrawer.OperationType.SetMatrix);
				this._matrices.Add(this._currMatrix);
			}
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000F2378 File Offset: 0x000F0778
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix)
		{
			this.setWireMode(false);
			this.drawMeshInternal(mesh, matrix);
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x000F2389 File Offset: 0x000F0789
		public void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.DrawMesh(mesh, Matrix4x4.TRS(position, rotation, scale));
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000F239B File Offset: 0x000F079B
		public void DrawWireMesh(Mesh mesh, Matrix4x4 matrix)
		{
			this.setWireMode(true);
			this.drawMeshInternal(mesh, matrix);
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x000F23AC File Offset: 0x000F07AC
		public void DrawWireMesh(Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.DrawWireMesh(mesh, Matrix4x4.TRS(position, rotation, scale));
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x000F23BE File Offset: 0x000F07BE
		public void DrawLine(Vector3 a, Vector3 b)
		{
			this._operations.Add(RuntimeGizmoDrawer.OperationType.DrawLine);
			this._lines.Add(new RuntimeGizmoDrawer.Line(a, b));
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x000F23DE File Offset: 0x000F07DE
		public void DrawCube(Vector3 position, Vector3 size)
		{
			this.DrawMesh(this.cubeMesh, position, Quaternion.identity, size);
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x000F23F3 File Offset: 0x000F07F3
		public void DrawWireCube(Vector3 position, Vector3 size)
		{
			this.DrawWireMesh(this.wireCubeMesh, position, Quaternion.identity, size);
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x000F2408 File Offset: 0x000F0808
		public void DrawSphere(Vector3 center, float radius)
		{
			if (this.sphereMesh == null)
			{
				throw new InvalidOperationException("Cannot draw a sphere because the Runtime Gizmo Manager does not have a sphere mesh assigned!");
			}
			this.DrawMesh(this.sphereMesh, center, Quaternion.identity, Vector3.one * radius * 2f);
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x000F2458 File Offset: 0x000F0858
		public void DrawWireSphere(Pose pose, float radius, int numSegments = 32)
		{
			this._operations.Add(RuntimeGizmoDrawer.OperationType.DrawWireSphere);
			this._wireSpheres.Add(new RuntimeGizmoDrawer.WireSphere
			{
				pose = pose,
				radius = radius,
				numSegments = numSegments
			});
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x000F249D File Offset: 0x000F089D
		public void DrawWireSphere(Vector3 center, float radius, int numSegments = 32)
		{
			this.DrawWireSphere(new Pose(center, Quaternion.identity), radius, numSegments);
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x000F24B4 File Offset: 0x000F08B4
		public void DrawEllipsoid(Vector3 foci1, Vector3 foci2, float minorAxis)
		{
			this.PushMatrix();
			Vector3 pos = (foci1 + foci2) / 2f;
			Quaternion q = Quaternion.LookRotation(foci1 - foci2);
			float z = Mathf.Sqrt(Mathf.Pow(Vector3.Distance(foci1, foci2) / 2f, 2f) + Mathf.Pow(minorAxis / 2f, 2f)) * 2f;
			Vector3 s = new Vector3(minorAxis, minorAxis, z);
			this.matrix = Matrix4x4.TRS(pos, q, s);
			this.DrawWireSphere(Vector3.zero, 0.5f, 32);
			this.PopMatrix();
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x000F254C File Offset: 0x000F094C
		public void DrawWireCapsule(Vector3 start, Vector3 end, float radius)
		{
			Vector3 vector = (end - start).normalized * radius;
			Vector3 vector2 = Vector3.Slerp(vector, -vector, 0.5f);
			Vector3 vector3 = Vector3.Cross(vector, vector2).normalized * radius;
			float magnitude = (start - end).magnitude;
			this.DrawLineWireCircle(start, vector, radius, 8);
			this.DrawLineWireCircle(end, -vector, radius, 8);
			this.DrawLine(start + vector3, end + vector3);
			this.DrawLine(start - vector3, end - vector3);
			this.DrawLine(start + vector2, end + vector2);
			this.DrawLine(start - vector2, end - vector2);
			this.DrawWireArc(start, vector3, vector2, radius, 0.5f, 8);
			this.DrawWireArc(start, vector2, -vector3, radius, 0.5f, 8);
			this.DrawWireArc(end, vector3, -vector2, radius, 0.5f, 8);
			this.DrawWireArc(end, vector2, vector3, radius, 0.5f, 8);
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x000F265D File Offset: 0x000F0A5D
		private void DrawLineWireCircle(Vector3 center, Vector3 normal, float radius, int numCircleSegments = 16)
		{
			this.DrawWireArc(center, normal, Vector3.Slerp(normal, -normal, 0.5f), radius, 1f, numCircleSegments);
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x000F2680 File Offset: 0x000F0A80
		public void DrawWireArc(Vector3 center, Vector3 normal, Vector3 radialStartDirection, float radius, float fractionOfCircleToDraw, int numCircleSegments = 16)
		{
			normal = normal.normalized;
			Vector3 vector = radialStartDirection.normalized * radius;
			int num = (int)((float)numCircleSegments * fractionOfCircleToDraw);
			Quaternion rotation = Quaternion.AngleAxis(360f / (float)numCircleSegments, normal);
			for (int i = 0; i < num; i++)
			{
				Vector3 vector2 = rotation * vector;
				this.DrawLine(center + vector, center + vector2);
				vector = vector2;
			}
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x000F26F4 File Offset: 0x000F0AF4
		public void DrawColliders(GameObject gameObject, bool useWireframe = true, bool traverseHierarchy = true, bool drawTriggers = false)
		{
			this.PushMatrix();
			if (traverseHierarchy)
			{
				gameObject.GetComponentsInChildren<Collider>(this._colliderList);
			}
			else
			{
				gameObject.GetComponents<Collider>(this._colliderList);
			}
			for (int i = 0; i < this._colliderList.Count; i++)
			{
				Collider collider = this._colliderList[i];
				this.RelativeTo(collider.transform);
				if (!collider.isTrigger || drawTriggers)
				{
					this.DrawCollider(collider, true, true);
				}
			}
			this.PopMatrix();
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x000F2788 File Offset: 0x000F0B88
		public void DrawCollider(Collider collider, bool useWireframe = true, bool skipMatrixSetup = false)
		{
			if (!skipMatrixSetup)
			{
				this.PushMatrix();
				this.RelativeTo(collider.transform);
			}
			if (collider is BoxCollider)
			{
				BoxCollider boxCollider = collider as BoxCollider;
				if (useWireframe)
				{
					this.DrawWireCube(boxCollider.center, boxCollider.size);
				}
				else
				{
					this.DrawCube(boxCollider.center, boxCollider.size);
				}
			}
			else if (collider is SphereCollider)
			{
				SphereCollider sphereCollider = collider as SphereCollider;
				if (useWireframe)
				{
					this.DrawWireSphere(sphereCollider.center, sphereCollider.radius, 32);
				}
				else
				{
					this.DrawSphere(sphereCollider.center, sphereCollider.radius);
				}
			}
			else if (collider is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = collider as CapsuleCollider;
				if (useWireframe)
				{
					Vector3 a;
					switch (capsuleCollider.direction)
					{
					case 0:
						a = Vector3.right;
						goto IL_FF;
					case 1:
						a = Vector3.up;
						goto IL_FF;
					}
					a = Vector3.forward;
					IL_FF:
					this.DrawWireCapsule(capsuleCollider.center + a * (capsuleCollider.height / 2f - capsuleCollider.radius), capsuleCollider.center - a * (capsuleCollider.height / 2f - capsuleCollider.radius), capsuleCollider.radius);
				}
				else
				{
					Vector3 vector = Vector3.zero;
					vector += Vector3.one * capsuleCollider.radius * 2f;
					vector += new Vector3((float)((capsuleCollider.direction != 0) ? 0 : 1), (float)((capsuleCollider.direction != 1) ? 0 : 1), (float)((capsuleCollider.direction != 2) ? 0 : 1)) * (capsuleCollider.height - capsuleCollider.radius * 2f);
					this.DrawCube(capsuleCollider.center, vector);
				}
			}
			else if (collider is MeshCollider)
			{
				MeshCollider meshCollider = collider as MeshCollider;
				if (meshCollider.sharedMesh != null)
				{
					if (useWireframe)
					{
						this.DrawWireMesh(meshCollider.sharedMesh, Matrix4x4.identity);
					}
					else
					{
						this.DrawMesh(meshCollider.sharedMesh, Matrix4x4.identity);
					}
				}
			}
			if (!skipMatrixSetup)
			{
				this.PopMatrix();
			}
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000F29EC File Offset: 0x000F0DEC
		public void DrawPosition(Vector3 pos, Color lerpColor, float lerpCoeff, float? overrideScale = null)
		{
			float num;
			if (overrideScale != null)
			{
				num = overrideScale.Value;
			}
			else
			{
				num = 0.06f;
				Camera current = Camera.current;
				Vector4 v = this.matrix * pos;
				if (current != null)
				{
					float num2 = Vector3.Distance(v, current.transform.position);
					num *= num2;
				}
			}
			float d = num / 2f;
			float alpha = 0.6f;
			this.color = Color.red;
			if (lerpCoeff != 0f)
			{
				this.color = this.color.LerpHSV(lerpColor, lerpCoeff);
			}
			this.DrawLine(pos, pos + Vector3.right * d);
			this.color = Color.black.WithAlpha(alpha);
			if (lerpCoeff != 0f)
			{
				this.color = this.color.LerpHSV(lerpColor, lerpCoeff);
			}
			this.DrawLine(pos, pos - Vector3.right * d);
			this.color = Color.green;
			if (lerpCoeff != 0f)
			{
				this.color = this.color.LerpHSV(lerpColor, lerpCoeff);
			}
			this.DrawLine(pos, pos + Vector3.up * d);
			this.color = Color.black.WithAlpha(alpha);
			if (lerpCoeff != 0f)
			{
				this.color = this.color.LerpHSV(lerpColor, lerpCoeff);
			}
			this.DrawLine(pos, pos - Vector3.up * d);
			this.color = Color.blue;
			if (lerpCoeff != 0f)
			{
				this.color = this.color.LerpHSV(lerpColor, lerpCoeff);
			}
			this.DrawLine(pos, pos + Vector3.forward * d);
			this.color = Color.black.WithAlpha(alpha);
			if (lerpCoeff != 0f)
			{
				this.color = this.color.LerpHSV(lerpColor, lerpCoeff);
			}
			this.DrawLine(pos, pos - Vector3.forward * d);
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000F2C08 File Offset: 0x000F1008
		public void DrawPosition(Vector3 pos)
		{
			this.DrawPosition(pos, Color.white, 0f, null);
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000F2C2F File Offset: 0x000F102F
		public void DrawPosition(Vector3 pos, float overrideScale)
		{
			this.DrawPosition(pos, Color.white, 0f, new float?(overrideScale));
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x000F2C48 File Offset: 0x000F1048
		public void DrawRect(Transform frame, Rect rect)
		{
			this.PushMatrix();
			this.matrix = frame.localToWorldMatrix;
			this.DrawLine(rect.Corner00(), rect.Corner01());
			this.DrawLine(rect.Corner01(), rect.Corner11());
			this.DrawLine(rect.Corner11(), rect.Corner10());
			this.DrawLine(rect.Corner10(), rect.Corner00());
			this.PopMatrix();
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x000F2CB8 File Offset: 0x000F10B8
		public void ClearAllGizmos()
		{
			this._operations.Clear();
			this._matrices.Clear();
			this._colors.Clear();
			this._lines.Clear();
			this._wireSpheres.Clear();
			this._meshes.Clear();
			this._isInWireMode = false;
			this._currMatrix = Matrix4x4.identity;
			this._currColor = Color.white;
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000F2D24 File Offset: 0x000F1124
		public void DrawAllGizmosToScreen()
		{
			try
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = -1;
				this._currMatrix = Matrix4x4.identity;
				this._currColor = Color.white;
				GL.wireframe = false;
				for (int i = 0; i < this._operations.Count; i++)
				{
					RuntimeGizmoDrawer.OperationType operationType = this._operations[i];
					switch (operationType)
					{
					case RuntimeGizmoDrawer.OperationType.SetMatrix:
						this._currMatrix = this._matrices[num++];
						break;
					case RuntimeGizmoDrawer.OperationType.ToggleWireframe:
						GL.wireframe = !GL.wireframe;
						break;
					case RuntimeGizmoDrawer.OperationType.SetColor:
						this._currColor = this._colors[num2++];
						num6 = -1;
						break;
					case RuntimeGizmoDrawer.OperationType.DrawLine:
					{
						this.setPass(ref num6, true, null);
						GL.Begin(1);
						RuntimeGizmoDrawer.Line line = this._lines[num3++];
						GL.Vertex(this._currMatrix.MultiplyPoint(line.a));
						GL.Vertex(this._currMatrix.MultiplyPoint(line.b));
						GL.End();
						break;
					}
					case RuntimeGizmoDrawer.OperationType.DrawWireSphere:
					{
						this.setPass(ref num6, true, null);
						GL.Begin(1);
						RuntimeGizmoDrawer.WireSphere wireSphere = this._wireSpheres[num4++];
						this.drawWireSphereNow(wireSphere, ref num6);
						GL.End();
						break;
					}
					case RuntimeGizmoDrawer.OperationType.DrawMesh:
						if (GL.wireframe)
						{
							this.setPass(ref num6, true, null);
						}
						else
						{
							this.setPass(ref num6, false, null);
						}
						Graphics.DrawMeshNow(this._meshes[num5++], this._currMatrix * this._matrices[num++]);
						break;
					default:
						throw new InvalidOperationException("Unexpected operation type " + operationType);
					}
				}
			}
			finally
			{
				GL.wireframe = false;
			}
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000F2F48 File Offset: 0x000F1348
		private void drawLineNow(Vector3 a, Vector3 b)
		{
			GL.Vertex(this._currMatrix.MultiplyPoint(a));
			GL.Vertex(this._currMatrix.MultiplyPoint(b));
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x000F2F6C File Offset: 0x000F136C
		private void drawWireArcNow(Vector3 center, Vector3 normal, Vector3 radialStartDirection, float radius, float fractionOfCircleToDraw, int numCircleSegments = 16)
		{
			normal = normal.normalized;
			Vector3 vector = radialStartDirection.normalized * radius;
			int num = (int)((float)numCircleSegments * fractionOfCircleToDraw);
			Quaternion rotation = Quaternion.AngleAxis(360f / (float)numCircleSegments, normal);
			for (int i = 0; i < num; i++)
			{
				Vector3 vector2 = rotation * vector;
				this.drawLineNow(center + vector, center + vector2);
				vector = vector2;
			}
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x000F2FE0 File Offset: 0x000F13E0
		private void setCurrentPassColorIfNew(Color desiredColor, ref int curPass)
		{
			if (this._currColor != desiredColor)
			{
				this._currColor = desiredColor;
				this.setPass(ref curPass, true, null);
			}
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x000F3018 File Offset: 0x000F1418
		private void drawPlaneSoftenedWireArcNow(Vector3 position, Vector3 circleNormal, Vector3 radialStartDirection, float radius, Color inFrontOfPlaneColor, Color behindPlaneColor, Vector3 planeNormal, ref int curPass, float fractionOfCircleToDraw = 1f, int numCircleSegments = 16)
		{
			Color currColor = this._currColor;
			Vector3 b = planeNormal.Cross(circleNormal);
			Quaternion rotation = Quaternion.AngleAxis(360f / (float)numCircleSegments, circleNormal);
			Vector3 vector = radialStartDirection * radius;
			for (int i = 0; i < numCircleSegments + 1; i++)
			{
				Vector3 vector2 = rotation * vector;
				float num = vector.SignedAngle(b, circleNormal);
				float num2 = vector2.SignedAngle(b, circleNormal);
				bool flag = num < 0f;
				bool flag2 = num2 < 0f;
				if (flag != flag2)
				{
					Color value = Color.Lerp(inFrontOfPlaneColor, behindPlaneColor, 0.5f);
					GL.End();
					this.setPass(ref curPass, true, new Color?(value));
					GL.Begin(1);
				}
				else if (flag)
				{
					GL.End();
					this.setPass(ref curPass, true, new Color?(inFrontOfPlaneColor));
					GL.Begin(1);
				}
				else
				{
					GL.End();
					this.setPass(ref curPass, true, new Color?(behindPlaneColor));
					GL.Begin(1);
				}
				this.drawLineNow(vector, vector2);
				vector = vector2;
			}
			this._currColor = currColor;
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000F3134 File Offset: 0x000F1534
		private void drawWireSphereNow(RuntimeGizmoDrawer.WireSphere wireSphere, ref int curPass)
		{
			Vector3 position = wireSphere.pose.position;
			Quaternion rotation = wireSphere.pose.rotation;
			Vector3 b = this._currMatrix.MultiplyPoint3x4(position);
			Vector3 normalized = (Camera.current.transform.position - b).normalized;
			Vector3 vector = this._currMatrix.inverse.MultiplyVector(normalized);
			this.drawWireArcNow(position, vector, vector.Perpendicular(), wireSphere.radius, 1f, wireSphere.numSegments);
			Vector3 vector2 = rotation * Vector3.right;
			Vector3 vector3 = rotation * Vector3.up;
			Vector3 vector4 = rotation * Vector3.forward;
			this.drawPlaneSoftenedWireArcNow(position, vector3, vector2, wireSphere.radius, this._currColor, this._currColor.WithAlpha(this._currColor.a * 0.1f), vector, ref curPass, 1f, wireSphere.numSegments);
			this.drawPlaneSoftenedWireArcNow(position, vector4, vector3, wireSphere.radius, this._currColor, this._currColor.WithAlpha(this._currColor.a * 0.1f), vector, ref curPass, 1f, wireSphere.numSegments);
			this.drawPlaneSoftenedWireArcNow(position, vector2, vector4, wireSphere.radius, this._currColor, this._currColor.WithAlpha(this._currColor.a * 0.1f), vector, ref curPass, 1f, wireSphere.numSegments);
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x000F32B4 File Offset: 0x000F16B4
		private void setPass(ref int currPass, bool isUnlit, Color? desiredCurrColor = null)
		{
			bool flag = false;
			if (desiredCurrColor != null)
			{
				flag = (this._currColor != desiredCurrColor.Value);
				this._currColor = desiredCurrColor.Value;
			}
			int num;
			if (isUnlit)
			{
				if (this._currColor.a < 1f)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
			}
			else if (this._currColor.a < 1f)
			{
				num = 3;
			}
			else
			{
				num = 2;
			}
			if (currPass != num || flag)
			{
				currPass = num;
				this._gizmoMaterial.color = this._currColor;
				this._gizmoMaterial.SetPass(currPass);
			}
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000F3366 File Offset: 0x000F1766
		private void drawMeshInternal(Mesh mesh, Matrix4x4 matrix)
		{
			if (mesh == null)
			{
				throw new InvalidOperationException("Mesh cannot be null!");
			}
			this._operations.Add(RuntimeGizmoDrawer.OperationType.DrawMesh);
			this._meshes.Add(mesh);
			this._matrices.Add(matrix);
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000F33A3 File Offset: 0x000F17A3
		private void setWireMode(bool wireMode)
		{
			if (this._isInWireMode != wireMode)
			{
				this._operations.Add(RuntimeGizmoDrawer.OperationType.ToggleWireframe);
				this._isInWireMode = wireMode;
			}
		}

		// Token: 0x040023E6 RID: 9190
		public const int UNLIT_SOLID_PASS = 0;

		// Token: 0x040023E7 RID: 9191
		public const int UNLIT_TRANSPARENT_PASS = 1;

		// Token: 0x040023E8 RID: 9192
		public const int SHADED_SOLID_PASS = 2;

		// Token: 0x040023E9 RID: 9193
		public const int SHADED_TRANSPARENT_PASS = 3;

		// Token: 0x040023EA RID: 9194
		private List<RuntimeGizmoDrawer.OperationType> _operations = new List<RuntimeGizmoDrawer.OperationType>();

		// Token: 0x040023EB RID: 9195
		private List<Matrix4x4> _matrices = new List<Matrix4x4>();

		// Token: 0x040023EC RID: 9196
		private List<Color> _colors = new List<Color>();

		// Token: 0x040023ED RID: 9197
		private List<RuntimeGizmoDrawer.Line> _lines = new List<RuntimeGizmoDrawer.Line>();

		// Token: 0x040023EE RID: 9198
		private List<RuntimeGizmoDrawer.WireSphere> _wireSpheres = new List<RuntimeGizmoDrawer.WireSphere>();

		// Token: 0x040023EF RID: 9199
		private List<Mesh> _meshes = new List<Mesh>();

		// Token: 0x040023F0 RID: 9200
		private Color _currColor = Color.white;

		// Token: 0x040023F1 RID: 9201
		private Matrix4x4 _currMatrix = Matrix4x4.identity;

		// Token: 0x040023F2 RID: 9202
		private Stack<Matrix4x4> _matrixStack = new Stack<Matrix4x4>();

		// Token: 0x040023F3 RID: 9203
		private bool _isInWireMode;

		// Token: 0x040023F4 RID: 9204
		private Material _gizmoMaterial;

		// Token: 0x040023F5 RID: 9205
		private int _operationCountOnGuard = -1;

		// Token: 0x040023F6 RID: 9206
		public Mesh cubeMesh;

		// Token: 0x040023F7 RID: 9207
		public Mesh wireCubeMesh;

		// Token: 0x040023F8 RID: 9208
		public Mesh sphereMesh;

		// Token: 0x040023F9 RID: 9209
		public Mesh wireSphereMesh;

		// Token: 0x040023FA RID: 9210
		private List<Collider> _colliderList = new List<Collider>();

		// Token: 0x02000745 RID: 1861
		private enum OperationType
		{
			// Token: 0x040023FC RID: 9212
			SetMatrix,
			// Token: 0x040023FD RID: 9213
			ToggleWireframe,
			// Token: 0x040023FE RID: 9214
			SetColor,
			// Token: 0x040023FF RID: 9215
			DrawLine,
			// Token: 0x04002400 RID: 9216
			DrawWireSphere,
			// Token: 0x04002401 RID: 9217
			DrawMesh
		}

		// Token: 0x02000746 RID: 1862
		private struct Line
		{
			// Token: 0x06002D94 RID: 11668 RVA: 0x000F33C4 File Offset: 0x000F17C4
			public Line(Vector3 a, Vector3 b)
			{
				this.a = a;
				this.b = b;
			}

			// Token: 0x04002402 RID: 9218
			public Vector3 a;

			// Token: 0x04002403 RID: 9219
			public Vector3 b;
		}

		// Token: 0x02000747 RID: 1863
		private struct WireSphere
		{
			// Token: 0x04002404 RID: 9220
			public Pose pose;

			// Token: 0x04002405 RID: 9221
			public float radius;

			// Token: 0x04002406 RID: 9222
			public int numSegments;
		}
	}
}
