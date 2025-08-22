using System;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x02000102 RID: 258
	public static class RuntimeHandles
	{
		// Token: 0x06000607 RID: 1543 RVA: 0x000249BC File Offset: 0x00022DBC
		static RuntimeHandles()
		{
			RuntimeHandles.LinesMaterial.color = Color.white;
			RuntimeHandles.LinesMaterial.enableInstancing = true;
			RuntimeHandles.LinesMaterialZTest = new Material(Shader.Find("Battlehub/RTHandles/VertexColor"));
			RuntimeHandles.LinesMaterialZTest.color = Color.white;
			RuntimeHandles.LinesMaterialZTest.SetFloat("_ZTest", 4f);
			RuntimeHandles.LinesMaterialZTest.enableInstancing = true;
			RuntimeHandles.LinesClipMaterial = new Material(Shader.Find("Battlehub/RTHandles/VertexColorClip"));
			RuntimeHandles.LinesClipMaterial.color = Color.white;
			RuntimeHandles.LinesClipMaterial.enableInstancing = true;
			RuntimeHandles.LinesBillboardMaterial = new Material(Shader.Find("Battlehub/RTHandles/VertexColorBillboard"));
			RuntimeHandles.LinesBillboardMaterial.color = Color.white;
			RuntimeHandles.LinesBillboardMaterial.enableInstancing = true;
			RuntimeHandles.ShapesMaterial = new Material(Shader.Find("Battlehub/RTHandles/Shape"));
			RuntimeHandles.ShapesMaterial.color = Color.white;
			RuntimeHandles.ShapesMaterial.enableInstancing = true;
			RuntimeHandles.ShapesMaterialZTest = new Material(Shader.Find("Battlehub/RTHandles/Shape"));
			RuntimeHandles.ShapesMaterialZTest.color = new Color(1f, 1f, 1f, 0f);
			RuntimeHandles.ShapesMaterialZTest.SetFloat("_ZTest", 4f);
			RuntimeHandles.ShapesMaterialZTest.SetFloat("_ZWrite", 1f);
			RuntimeHandles.ShapesMaterialZTest.enableInstancing = true;
			RuntimeHandles.ShapesMaterialZTestOffset = new Material(Shader.Find("Battlehub/RTHandles/Shape"));
			RuntimeHandles.ShapesMaterialZTestOffset.color = new Color(1f, 1f, 1f, 1f);
			RuntimeHandles.ShapesMaterialZTestOffset.SetFloat("_ZTest", 4f);
			RuntimeHandles.ShapesMaterialZTestOffset.SetFloat("_ZWrite", 1f);
			RuntimeHandles.ShapesMaterialZTestOffset.SetFloat("_OFactors", -1f);
			RuntimeHandles.ShapesMaterialZTestOffset.SetFloat("_OUnits", -1f);
			RuntimeHandles.ShapesMaterialZTestOffset.enableInstancing = true;
			RuntimeHandles.ShapesMaterialZTest2 = new Material(Shader.Find("Battlehub/RTHandles/Shape"));
			RuntimeHandles.ShapesMaterialZTest2.color = new Color(1f, 1f, 1f, 0f);
			RuntimeHandles.ShapesMaterialZTest2.SetFloat("_ZTest", 4f);
			RuntimeHandles.ShapesMaterialZTest2.SetFloat("_ZWrite", 1f);
			RuntimeHandles.ShapesMaterialZTest2.enableInstancing = true;
			RuntimeHandles.ShapesMaterialZTest3 = new Material(Shader.Find("Battlehub/RTHandles/Shape"));
			RuntimeHandles.ShapesMaterialZTest3.color = new Color(1f, 1f, 1f, 0f);
			RuntimeHandles.ShapesMaterialZTest3.SetFloat("_ZTest", 4f);
			RuntimeHandles.ShapesMaterialZTest3.SetFloat("_ZWrite", 1f);
			RuntimeHandles.ShapesMaterialZTest3.enableInstancing = true;
			RuntimeHandles.ShapesMaterialZTest4 = new Material(Shader.Find("Battlehub/RTHandles/Shape"));
			RuntimeHandles.ShapesMaterialZTest4.color = new Color(1f, 1f, 1f, 0f);
			RuntimeHandles.ShapesMaterialZTest4.SetFloat("_ZTest", 4f);
			RuntimeHandles.ShapesMaterialZTest4.SetFloat("_ZWrite", 1f);
			RuntimeHandles.ShapesMaterialZTest4.enableInstancing = true;
			RuntimeHandles.XMaterial = new Material(Shader.Find("Battlehub/RTCommon/Billboard"));
			RuntimeHandles.XMaterial.color = Color.white;
			RuntimeHandles.XMaterial.mainTexture = Resources.Load<Texture>("Battlehub.RuntimeHandles.x");
			RuntimeHandles.XMaterial.enableInstancing = true;
			RuntimeHandles.YMaterial = new Material(Shader.Find("Battlehub/RTCommon/Billboard"));
			RuntimeHandles.YMaterial.color = Color.white;
			RuntimeHandles.YMaterial.mainTexture = Resources.Load<Texture>("Battlehub.RuntimeHandles.y");
			RuntimeHandles.YMaterial.enableInstancing = true;
			RuntimeHandles.ZMaterial = new Material(Shader.Find("Battlehub/RTCommon/Billboard"));
			RuntimeHandles.ZMaterial.color = Color.white;
			RuntimeHandles.ZMaterial.mainTexture = Resources.Load<Texture>("Battlehub.RuntimeHandles.z");
			RuntimeHandles.ZMaterial.enableInstancing = true;
			RuntimeHandles.GridMaterial = new Material(Shader.Find("Battlehub/RTHandles/Grid"));
			RuntimeHandles.GridMaterial.color = Color.white;
			RuntimeHandles.GridMaterial.SetFloat("_ZTest", 1f);
			RuntimeHandles.GridMaterial.enableInstancing = true;
			Mesh mesh = RuntimeHandles.CreateConeMesh(RTHColors.SelectionColor, 1f);
			Mesh mesh2 = RuntimeHandles.CreateConeMesh(RTHColors.DisabledColor, 1f);
			CombineInstance combineInstance = default(CombineInstance);
			combineInstance.mesh = mesh;
			combineInstance.transform = Matrix4x4.TRS(Vector3.up * 1f, Quaternion.identity, Vector3.one);
			RuntimeHandles.SelectionArrowY = new Mesh();
			RuntimeHandles.SelectionArrowY.CombineMeshes(new CombineInstance[]
			{
				combineInstance
			}, true);
			RuntimeHandles.SelectionArrowY.RecalculateNormals();
			combineInstance.mesh = mesh2;
			combineInstance.transform = Matrix4x4.TRS(Vector3.up * 1f, Quaternion.identity, Vector3.one);
			RuntimeHandles.DisabledArrowY = new Mesh();
			RuntimeHandles.DisabledArrowY.CombineMeshes(new CombineInstance[]
			{
				combineInstance
			}, true);
			RuntimeHandles.DisabledArrowY.RecalculateNormals();
			combineInstance.mesh = RuntimeHandles.CreateConeMesh(RTHColors.YColor, 1f);
			combineInstance.transform = Matrix4x4.TRS(Vector3.up * 1f, Quaternion.identity, Vector3.one);
			RuntimeHandles.ArrowY = new Mesh();
			RuntimeHandles.ArrowY.CombineMeshes(new CombineInstance[]
			{
				combineInstance
			}, true);
			RuntimeHandles.ArrowY.RecalculateNormals();
			CombineInstance combineInstance2 = default(CombineInstance);
			combineInstance2.mesh = mesh;
			combineInstance2.transform = Matrix4x4.TRS(Vector3.right * 1f, Quaternion.AngleAxis(-90f, Vector3.forward), Vector3.one);
			RuntimeHandles.SelectionArrowX = new Mesh();
			RuntimeHandles.SelectionArrowX.CombineMeshes(new CombineInstance[]
			{
				combineInstance2
			}, true);
			RuntimeHandles.SelectionArrowX.RecalculateNormals();
			combineInstance2.mesh = mesh2;
			combineInstance2.transform = Matrix4x4.TRS(Vector3.right * 1f, Quaternion.AngleAxis(-90f, Vector3.forward), Vector3.one);
			RuntimeHandles.DisabledArrowX = new Mesh();
			RuntimeHandles.DisabledArrowX.CombineMeshes(new CombineInstance[]
			{
				combineInstance2
			}, true);
			RuntimeHandles.DisabledArrowX.RecalculateNormals();
			combineInstance2.mesh = RuntimeHandles.CreateConeMesh(RTHColors.XColor, 1f);
			combineInstance2.transform = Matrix4x4.TRS(Vector3.right * 1f, Quaternion.AngleAxis(-90f, Vector3.forward), Vector3.one);
			RuntimeHandles.ArrowX = new Mesh();
			RuntimeHandles.ArrowX.CombineMeshes(new CombineInstance[]
			{
				combineInstance2
			}, true);
			RuntimeHandles.ArrowX.RecalculateNormals();
			CombineInstance combineInstance3 = default(CombineInstance);
			combineInstance3.mesh = mesh;
			combineInstance3.transform = Matrix4x4.TRS(Vector3.forward * 1f, Quaternion.AngleAxis(90f, Vector3.right), Vector3.one);
			RuntimeHandles.SelectionArrowZ = new Mesh();
			RuntimeHandles.SelectionArrowZ.CombineMeshes(new CombineInstance[]
			{
				combineInstance3
			}, true);
			RuntimeHandles.SelectionArrowZ.RecalculateNormals();
			combineInstance3.mesh = mesh2;
			combineInstance3.transform = Matrix4x4.TRS(Vector3.forward * 1f, Quaternion.AngleAxis(90f, Vector3.right), Vector3.one);
			RuntimeHandles.DisabledArrowZ = new Mesh();
			RuntimeHandles.DisabledArrowZ.CombineMeshes(new CombineInstance[]
			{
				combineInstance3
			}, true);
			RuntimeHandles.DisabledArrowZ.RecalculateNormals();
			combineInstance3.mesh = RuntimeHandles.CreateConeMesh(RTHColors.ZColor, 1f);
			combineInstance3.transform = Matrix4x4.TRS(Vector3.forward * 1f, Quaternion.AngleAxis(90f, Vector3.right), Vector3.one);
			RuntimeHandles.ArrowZ = new Mesh();
			RuntimeHandles.ArrowZ.CombineMeshes(new CombineInstance[]
			{
				combineInstance3
			}, true);
			RuntimeHandles.ArrowZ.RecalculateNormals();
			combineInstance.mesh = RuntimeHandles.CreateConeMesh(RTHColors.YColor, 1f);
			combineInstance2.mesh = RuntimeHandles.CreateConeMesh(RTHColors.XColor, 1f);
			combineInstance3.mesh = RuntimeHandles.CreateConeMesh(RTHColors.ZColor, 1f);
			RuntimeHandles.Arrows = new Mesh();
			RuntimeHandles.Arrows.CombineMeshes(new CombineInstance[]
			{
				combineInstance,
				combineInstance2,
				combineInstance3
			}, true);
			RuntimeHandles.Arrows.RecalculateNormals();
			RuntimeHandles.SelectionCube = RuntimeGraphics.CreateCubeMesh(RTHColors.SelectionColor, Vector3.zero, 1f, 0.1f, 0.1f, 0.1f);
			RuntimeHandles.DisabledCube = RuntimeGraphics.CreateCubeMesh(RTHColors.DisabledColor, Vector3.zero, 1f, 0.1f, 0.1f, 0.1f);
			RuntimeHandles.CubeX = RuntimeGraphics.CreateCubeMesh(RTHColors.XColor, Vector3.zero, 1f, 0.1f, 0.1f, 0.1f);
			RuntimeHandles.CubeY = RuntimeGraphics.CreateCubeMesh(RTHColors.YColor, Vector3.zero, 1f, 0.1f, 0.1f, 0.1f);
			RuntimeHandles.CubeZ = RuntimeGraphics.CreateCubeMesh(RTHColors.ZColor, Vector3.zero, 1f, 0.1f, 0.1f, 0.1f);
			RuntimeHandles.CubeUniform = RuntimeGraphics.CreateCubeMesh(RTHColors.AltColor, Vector3.zero, 1f, 0.1f, 0.1f, 0.1f);
			RuntimeHandles.SceneGizmoSelectedAxis = RuntimeHandles.CreateSceneGizmoHalfAxis(RTHColors.SelectionColor, Quaternion.AngleAxis(90f, Vector3.right));
			RuntimeHandles.SceneGizmoXAxis = RuntimeHandles.CreateSceneGizmoAxis(RTHColors.XColor, RTHColors.AltColor, Quaternion.AngleAxis(-90f, Vector3.forward));
			RuntimeHandles.SceneGizmoYAxis = RuntimeHandles.CreateSceneGizmoAxis(RTHColors.YColor, RTHColors.AltColor, Quaternion.identity);
			RuntimeHandles.SceneGizmoZAxis = RuntimeHandles.CreateSceneGizmoAxis(RTHColors.ZColor, RTHColors.AltColor, Quaternion.AngleAxis(90f, Vector3.right));
			RuntimeHandles.SceneGizmoCube = RuntimeGraphics.CreateCubeMesh(RTHColors.AltColor, Vector3.zero, 1f, 1f, 1f, 1f);
			RuntimeHandles.SceneGizmoSelectedCube = RuntimeGraphics.CreateCubeMesh(RTHColors.SelectionColor, Vector3.zero, 1f, 1f, 1f, 1f);
			RuntimeHandles.SceneGizmoQuad = RuntimeGraphics.CreateQuadMesh(1f, 1f);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000254D0 File Offset: 0x000238D0
		private static Mesh CreateConeMesh(Color color, float scale)
		{
			int num = 12;
			float num2 = 0.2f;
			num2 *= scale;
			Vector3[] array = new Vector3[num * 3 + 1];
			int[] array2 = new int[num * 6];
			Color[] array3 = new Color[array.Length];
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i] = color;
			}
			float num3 = num2 / 2.6f;
			float num4 = num2;
			float num5 = 6.2831855f / (float)num;
			float y = -num4;
			array[array.Length - 1] = new Vector3(0f, -num4, 0f);
			for (int j = 0; j < num; j++)
			{
				float f = (float)j * num5;
				float x = Mathf.Cos(f) * num3;
				float z = Mathf.Sin(f) * num3;
				array[j] = new Vector3(x, y, z);
				array[num + j] = new Vector3(0f, 0.01f, 0f);
				array[2 * num + j] = array[j];
			}
			for (int k = 0; k < num; k++)
			{
				array2[k * 6] = k;
				array2[k * 6 + 1] = num + k;
				array2[k * 6 + 2] = (k + 1) % num;
				array2[k * 6 + 3] = array.Length - 1;
				array2[k * 6 + 4] = 2 * num + k;
				array2[k * 6 + 5] = 2 * num + (k + 1) % num;
			}
			return new Mesh
			{
				name = "Cone",
				vertices = array,
				triangles = array2,
				colors = array3
			};
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00025690 File Offset: 0x00023A90
		private static Mesh CreateSceneGizmoHalfAxis(Color color, Quaternion rotation)
		{
			Mesh mesh = RuntimeHandles.CreateConeMesh(color, 1f);
			CombineInstance combineInstance = default(CombineInstance);
			combineInstance.mesh = mesh;
			combineInstance.transform = Matrix4x4.TRS(Vector3.up * 0.1f, Quaternion.AngleAxis(180f, Vector3.right), Vector3.one);
			Mesh mesh2 = new Mesh();
			mesh2.CombineMeshes(new CombineInstance[]
			{
				combineInstance
			}, true);
			CombineInstance combineInstance2 = default(CombineInstance);
			combineInstance2.mesh = mesh2;
			combineInstance2.transform = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
			mesh2 = new Mesh();
			mesh2.CombineMeshes(new CombineInstance[]
			{
				combineInstance2
			}, true);
			mesh2.RecalculateNormals();
			return mesh2;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00025758 File Offset: 0x00023B58
		private static Mesh CreateSceneGizmoAxis(Color axisColor, Color altColor, Quaternion rotation)
		{
			Mesh mesh = RuntimeHandles.CreateConeMesh(axisColor, 1f);
			Mesh mesh2 = RuntimeHandles.CreateConeMesh(altColor, 1f);
			CombineInstance combineInstance = default(CombineInstance);
			combineInstance.mesh = mesh;
			combineInstance.transform = Matrix4x4.TRS(Vector3.up * 0.1f, Quaternion.AngleAxis(180f, Vector3.right), Vector3.one);
			CombineInstance combineInstance2 = default(CombineInstance);
			combineInstance2.mesh = mesh2;
			combineInstance2.transform = Matrix4x4.TRS(Vector3.down * 0.1f, Quaternion.identity, Vector3.one);
			Mesh mesh3 = new Mesh();
			mesh3.CombineMeshes(new CombineInstance[]
			{
				combineInstance,
				combineInstance2
			}, true);
			CombineInstance combineInstance3 = default(CombineInstance);
			combineInstance3.mesh = mesh3;
			combineInstance3.transform = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
			mesh3 = new Mesh();
			mesh3.CombineMeshes(new CombineInstance[]
			{
				combineInstance3
			}, true);
			mesh3.RecalculateNormals();
			return mesh3;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00025874 File Offset: 0x00023C74
		public static float GetScreenScale(Vector3 position, Camera camera)
		{
			float num = (float)camera.pixelHeight;
			if (camera.orthographic)
			{
				return camera.orthographicSize * 2f / num * 90f;
			}
			Transform transform = camera.transform;
			float num2 = Vector3.Dot(position - transform.position, transform.forward);
			float num3 = 2f * num2 * Mathf.Tan(camera.fieldOfView * 0.5f * 0.017453292f);
			return num3 / num * 90f;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x000258F4 File Offset: 0x00023CF4
		private static void DoAxes(Vector3 position, Matrix4x4 transform, RuntimeHandleAxis selectedAxis, bool xLocked, bool yLocked, bool zLocked)
		{
			Vector3 vector = Vector3.right * 1f;
			Vector3 vector2 = Vector3.up * 1f;
			Vector3 vector3 = Vector3.forward * 1f;
			vector = transform.MultiplyVector(vector);
			vector2 = transform.MultiplyVector(vector2);
			vector3 = transform.MultiplyVector(vector3);
			if (xLocked)
			{
				GL.Color(RTHColors.DisabledColor);
			}
			else
			{
				GL.Color(((selectedAxis & RuntimeHandleAxis.X) != RuntimeHandleAxis.None) ? RTHColors.SelectionColor : RTHColors.XColor);
			}
			GL.Vertex(position);
			GL.Vertex(position + vector);
			if (yLocked)
			{
				GL.Color(RTHColors.DisabledColor);
			}
			else
			{
				GL.Color(((selectedAxis & RuntimeHandleAxis.Y) != RuntimeHandleAxis.None) ? RTHColors.SelectionColor : RTHColors.YColor);
			}
			GL.Vertex(position);
			GL.Vertex(position + vector2);
			if (zLocked)
			{
				GL.Color(RTHColors.DisabledColor);
			}
			else
			{
				GL.Color(((selectedAxis & RuntimeHandleAxis.Z) != RuntimeHandleAxis.None) ? RTHColors.SelectionColor : RTHColors.ZColor);
			}
			GL.Vertex(position);
			GL.Vertex(position + vector3);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00025A38 File Offset: 0x00023E38
		public static void DoPositionHandle(Vector3 position, Quaternion rotation, RuntimeHandleAxis selectedAxis = RuntimeHandleAxis.None, bool snapMode = false, LockObject lockObject = null)
		{
			float screenScale = RuntimeHandles.GetScreenScale(position, Camera.current);
			Matrix4x4 matrix4x = Matrix4x4.TRS(position, rotation, new Vector3(screenScale, screenScale, screenScale));
			RuntimeHandles.LinesMaterial.SetPass(0);
			GL.Begin(1);
			bool flag = lockObject != null && lockObject.PositionX;
			bool flag2 = lockObject != null && lockObject.PositionY;
			bool flag3 = lockObject != null && lockObject.PositionZ;
			RuntimeHandles.DoAxes(position, matrix4x, selectedAxis, flag, flag2, flag3);
			Vector3 vector = Vector3.right * 0.2f;
			Vector3 vector2 = Vector3.up * 0.2f;
			Vector3 vector3 = Vector3.forward * 0.2f;
			if (snapMode)
			{
				GL.End();
				RuntimeHandles.LinesBillboardMaterial.SetPass(0);
				GL.PushMatrix();
				GL.MultMatrix(matrix4x);
				GL.Begin(1);
				if (selectedAxis == RuntimeHandleAxis.Snap)
				{
					GL.Color(RTHColors.SelectionColor);
				}
				else
				{
					GL.Color(RTHColors.AltColor);
				}
				float num = 0.1f;
				Vector3 v = new Vector3(num, num, 0f);
				Vector3 v2 = new Vector3(num, -num, 0f);
				Vector3 v3 = new Vector3(-num, -num, 0f);
				Vector3 v4 = new Vector3(-num, num, 0f);
				GL.Vertex(v);
				GL.Vertex(v2);
				GL.Vertex(v2);
				GL.Vertex(v3);
				GL.Vertex(v3);
				GL.Vertex(v4);
				GL.Vertex(v4);
				GL.Vertex(v);
				GL.End();
				GL.PopMatrix();
			}
			else
			{
				Camera current = Camera.current;
				Vector3 lhs = matrix4x.inverse.MultiplyVector(current.transform.position - position);
				float num2 = Mathf.Sign(Vector3.Dot(lhs, vector)) * 1f;
				float num3 = Mathf.Sign(Vector3.Dot(lhs, vector2)) * 1f;
				float num4 = Mathf.Sign(Vector3.Dot(lhs, vector3)) * 1f;
				vector.x *= num2;
				vector2.y *= num3;
				vector3.z *= num4;
				Vector3 vector4 = vector + vector2;
				Vector3 vector5 = vector + vector3;
				Vector3 vector6 = vector2 + vector3;
				vector = matrix4x.MultiplyPoint(vector);
				vector2 = matrix4x.MultiplyPoint(vector2);
				vector3 = matrix4x.MultiplyPoint(vector3);
				vector4 = matrix4x.MultiplyPoint(vector4);
				vector5 = matrix4x.MultiplyPoint(vector5);
				vector6 = matrix4x.MultiplyPoint(vector6);
				if (!flag && !flag3)
				{
					GL.Color((selectedAxis == RuntimeHandleAxis.XZ) ? RTHColors.SelectionColor : RTHColors.YColor);
					GL.Vertex(position);
					GL.Vertex(vector3);
					GL.Vertex(vector3);
					GL.Vertex(vector5);
					GL.Vertex(vector5);
					GL.Vertex(vector);
					GL.Vertex(vector);
					GL.Vertex(position);
				}
				if (!flag && !flag2)
				{
					GL.Color((selectedAxis == RuntimeHandleAxis.XY) ? RTHColors.SelectionColor : RTHColors.ZColor);
					GL.Vertex(position);
					GL.Vertex(vector2);
					GL.Vertex(vector2);
					GL.Vertex(vector4);
					GL.Vertex(vector4);
					GL.Vertex(vector);
					GL.Vertex(vector);
					GL.Vertex(position);
				}
				if (!flag2 && !flag3)
				{
					GL.Color((selectedAxis == RuntimeHandleAxis.YZ) ? RTHColors.SelectionColor : RTHColors.XColor);
					GL.Vertex(position);
					GL.Vertex(vector2);
					GL.Vertex(vector2);
					GL.Vertex(vector6);
					GL.Vertex(vector6);
					GL.Vertex(vector3);
					GL.Vertex(vector3);
					GL.Vertex(position);
				}
				GL.End();
				GL.Begin(7);
				if (!flag && !flag3)
				{
					GL.Color(RTHColors.YColorTransparent);
					GL.Vertex(position);
					GL.Vertex(vector3);
					GL.Vertex(vector5);
					GL.Vertex(vector);
				}
				if (!flag && !flag2)
				{
					GL.Color(RTHColors.ZColorTransparent);
					GL.Vertex(position);
					GL.Vertex(vector2);
					GL.Vertex(vector4);
					GL.Vertex(vector);
				}
				if (!flag2 && !flag3)
				{
					GL.Color(RTHColors.XColorTransparent);
					GL.Vertex(position);
					GL.Vertex(vector2);
					GL.Vertex(vector6);
					GL.Vertex(vector3);
				}
				GL.End();
			}
			RuntimeHandles.ShapesMaterial.SetPass(0);
			if (!flag && !flag2 && !flag3)
			{
				Graphics.DrawMeshNow(RuntimeHandles.Arrows, matrix4x);
				if ((selectedAxis & RuntimeHandleAxis.X) != RuntimeHandleAxis.None)
				{
					Graphics.DrawMeshNow(RuntimeHandles.SelectionArrowX, matrix4x);
				}
				if ((selectedAxis & RuntimeHandleAxis.Y) != RuntimeHandleAxis.None)
				{
					Graphics.DrawMeshNow(RuntimeHandles.SelectionArrowY, matrix4x);
				}
				if ((selectedAxis & RuntimeHandleAxis.Z) != RuntimeHandleAxis.None)
				{
					Graphics.DrawMeshNow(RuntimeHandles.SelectionArrowZ, matrix4x);
				}
			}
			else
			{
				if (flag)
				{
					Graphics.DrawMeshNow(RuntimeHandles.DisabledArrowX, matrix4x);
				}
				else if ((selectedAxis & RuntimeHandleAxis.X) != RuntimeHandleAxis.None)
				{
					Graphics.DrawMeshNow(RuntimeHandles.SelectionArrowX, matrix4x);
				}
				else
				{
					Graphics.DrawMeshNow(RuntimeHandles.ArrowX, matrix4x);
				}
				if (flag2)
				{
					Graphics.DrawMeshNow(RuntimeHandles.DisabledArrowY, matrix4x);
				}
				else if ((selectedAxis & RuntimeHandleAxis.Y) != RuntimeHandleAxis.None)
				{
					Graphics.DrawMeshNow(RuntimeHandles.SelectionArrowY, matrix4x);
				}
				else
				{
					Graphics.DrawMeshNow(RuntimeHandles.ArrowY, matrix4x);
				}
				if (flag3)
				{
					Graphics.DrawMeshNow(RuntimeHandles.DisabledArrowZ, matrix4x);
				}
				else if ((selectedAxis & RuntimeHandleAxis.Z) != RuntimeHandleAxis.None)
				{
					Graphics.DrawMeshNow(RuntimeHandles.SelectionArrowZ, matrix4x);
				}
				else
				{
					Graphics.DrawMeshNow(RuntimeHandles.ArrowZ, matrix4x);
				}
			}
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00025FC0 File Offset: 0x000243C0
		public static void DoRotationHandle(Quaternion rotation, Vector3 position, RuntimeHandleAxis selectedAxis = RuntimeHandleAxis.None, LockObject lockObject = null)
		{
			float screenScale = RuntimeHandles.GetScreenScale(position, Camera.current);
			float num = 1f;
			Vector3 s = new Vector3(screenScale, screenScale, screenScale);
			Matrix4x4 transform = Matrix4x4.TRS(Vector3.zero, rotation * Quaternion.AngleAxis(-90f, Vector3.up), Vector3.one);
			Matrix4x4 transform2 = Matrix4x4.TRS(Vector3.zero, rotation * Quaternion.AngleAxis(-90f, Vector3.right), Vector3.one);
			Matrix4x4 transform3 = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
			Matrix4x4 m = Matrix4x4.TRS(position, Quaternion.identity, s);
			bool flag = lockObject != null && lockObject.RotationX;
			bool flag2 = lockObject != null && lockObject.RotationY;
			bool flag3 = lockObject != null && lockObject.RotationZ;
			bool flag4 = lockObject != null && lockObject.RotationScreen;
			RuntimeHandles.LinesClipMaterial.SetPass(0);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GL.Begin(1);
			if (flag)
			{
				GL.Color(RTHColors.DisabledColor);
			}
			else
			{
				GL.Color((selectedAxis == RuntimeHandleAxis.X) ? RTHColors.SelectionColor : RTHColors.XColor);
			}
			RuntimeGraphics.DrawCircleGL(transform, num, 64);
			if (flag2)
			{
				GL.Color(RTHColors.DisabledColor);
			}
			else
			{
				GL.Color((selectedAxis == RuntimeHandleAxis.Y) ? RTHColors.SelectionColor : RTHColors.YColor);
			}
			RuntimeGraphics.DrawCircleGL(transform2, num, 64);
			if (flag3)
			{
				GL.Color(RTHColors.DisabledColor);
			}
			else
			{
				GL.Color((selectedAxis == RuntimeHandleAxis.Z) ? RTHColors.SelectionColor : RTHColors.ZColor);
			}
			RuntimeGraphics.DrawCircleGL(transform3, num, 64);
			GL.End();
			GL.PopMatrix();
			RuntimeHandles.LinesBillboardMaterial.SetPass(0);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GL.Begin(1);
			if (flag && flag2 && flag3)
			{
				GL.Color(RTHColors.DisabledColor);
			}
			else
			{
				GL.Color((selectedAxis == RuntimeHandleAxis.Free) ? RTHColors.SelectionColor : RTHColors.AltColor);
			}
			RuntimeGraphics.DrawCircleGL(Matrix4x4.identity, num, 64);
			if (flag4)
			{
				GL.Color(RTHColors.DisabledColor);
			}
			else
			{
				GL.Color((selectedAxis == RuntimeHandleAxis.Screen) ? RTHColors.SelectionColor : RTHColors.AltColor);
			}
			RuntimeGraphics.DrawCircleGL(Matrix4x4.identity, num * 1.1f, 64);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0002625C File Offset: 0x0002465C
		public static void DoScaleHandle(Vector3 scale, Vector3 position, Quaternion rotation, RuntimeHandleAxis selectedAxis = RuntimeHandleAxis.None, LockObject lockObject = null)
		{
			float screenScale = RuntimeHandles.GetScreenScale(position, Camera.current);
			Matrix4x4 transform = Matrix4x4.TRS(position, rotation, scale * screenScale);
			RuntimeHandles.LinesMaterial.SetPass(0);
			bool flag = lockObject != null && lockObject.ScaleX;
			bool flag2 = lockObject != null && lockObject.ScaleY;
			bool flag3 = lockObject != null && lockObject.ScaleZ;
			GL.Begin(1);
			RuntimeHandles.DoAxes(position, transform, selectedAxis, flag, flag2, flag3);
			GL.End();
			Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, rotation, scale);
			RuntimeHandles.ShapesMaterial.SetPass(0);
			Vector3 vector = new Vector3(screenScale, screenScale, screenScale);
			Vector3 b = matrix4x.MultiplyVector(Vector3.right) * screenScale * 1f;
			Vector3 b2 = matrix4x.MultiplyVector(Vector3.up) * screenScale * 1f;
			Vector3 b3 = matrix4x.MultiplyVector(Vector3.forward) * screenScale * 1f;
			if (selectedAxis == RuntimeHandleAxis.X)
			{
				Graphics.DrawMeshNow((!flag) ? RuntimeHandles.SelectionCube : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b, rotation, vector));
				Graphics.DrawMeshNow((!flag2) ? RuntimeHandles.CubeY : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b2, rotation, vector));
				Graphics.DrawMeshNow((!flag3) ? RuntimeHandles.CubeZ : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b3, rotation, vector));
				Graphics.DrawMeshNow((!flag || !flag2 || !flag3) ? RuntimeHandles.CubeUniform : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position, rotation, vector * 1.35f));
			}
			else if (selectedAxis == RuntimeHandleAxis.Y)
			{
				Graphics.DrawMeshNow((!flag) ? RuntimeHandles.CubeX : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b, rotation, vector));
				Graphics.DrawMeshNow((!flag2) ? RuntimeHandles.SelectionCube : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b2, rotation, vector));
				Graphics.DrawMeshNow((!flag3) ? RuntimeHandles.CubeZ : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b3, rotation, vector));
				Graphics.DrawMeshNow((!flag || !flag2 || !flag3) ? RuntimeHandles.CubeUniform : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position, rotation, vector * 1.35f));
			}
			else if (selectedAxis == RuntimeHandleAxis.Z)
			{
				Graphics.DrawMeshNow((!flag) ? RuntimeHandles.CubeX : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b, rotation, vector));
				Graphics.DrawMeshNow((!flag2) ? RuntimeHandles.CubeY : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b2, rotation, vector));
				Graphics.DrawMeshNow((!flag3) ? RuntimeHandles.SelectionCube : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b3, rotation, vector));
				Graphics.DrawMeshNow((!flag || !flag2 || !flag3) ? RuntimeHandles.CubeUniform : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position, rotation, vector * 1.35f));
			}
			else if (selectedAxis == RuntimeHandleAxis.Free)
			{
				Graphics.DrawMeshNow((!flag) ? RuntimeHandles.CubeX : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b, rotation, vector));
				Graphics.DrawMeshNow((!flag2) ? RuntimeHandles.CubeY : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b2, rotation, vector));
				Graphics.DrawMeshNow((!flag3) ? RuntimeHandles.CubeZ : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b3, rotation, vector));
				Graphics.DrawMeshNow((!flag || !flag2 || !flag3) ? RuntimeHandles.SelectionCube : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position, rotation, vector * 1.35f));
			}
			else
			{
				Graphics.DrawMeshNow((!flag) ? RuntimeHandles.CubeX : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b, rotation, vector));
				Graphics.DrawMeshNow((!flag2) ? RuntimeHandles.CubeY : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b2, rotation, vector));
				Graphics.DrawMeshNow((!flag3) ? RuntimeHandles.CubeZ : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position + b3, rotation, vector));
				Graphics.DrawMeshNow((!flag || !flag2 || !flag3) ? RuntimeHandles.CubeUniform : RuntimeHandles.DisabledCube, Matrix4x4.TRS(position, rotation, vector * 1.35f));
			}
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0002672C File Offset: 0x00024B2C
		public static void DoSceneGizmo(Vector3 position, Quaternion rotation, Vector3 selection, float gizmoScale, float xAlpha = 1f, float yAlpha = 1f, float zAlpha = 1f)
		{
			float num = RuntimeHandles.GetScreenScale(position, Camera.current) * gizmoScale;
			Vector3 vector = new Vector3(num, num, num);
			float billboardOffset = 0.4f;
			if (Camera.current.orthographic)
			{
				billboardOffset = 0.42f;
			}
			if (selection != Vector3.zero)
			{
				if (selection == Vector3.one)
				{
					RuntimeHandles.ShapesMaterialZTestOffset.SetPass(0);
					Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoSelectedCube, Matrix4x4.TRS(position, rotation, vector * 0.15f));
				}
				else if ((xAlpha == 1f || xAlpha == 0f) && (yAlpha == 1f || yAlpha == 0f) && (zAlpha == 1f || zAlpha == 0f))
				{
					RuntimeHandles.ShapesMaterialZTestOffset.SetPass(0);
					Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoSelectedAxis, Matrix4x4.TRS(position, rotation * Quaternion.LookRotation(selection, Vector3.up), vector));
				}
			}
			RuntimeHandles.ShapesMaterialZTest.SetPass(0);
			RuntimeHandles.ShapesMaterialZTest.color = Color.white;
			Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoCube, Matrix4x4.TRS(position, rotation, vector * 0.15f));
			if (xAlpha == 1f && yAlpha == 1f && zAlpha == 1f)
			{
				Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoXAxis, Matrix4x4.TRS(position, rotation, vector));
				Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoYAxis, Matrix4x4.TRS(position, rotation, vector));
				Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoZAxis, Matrix4x4.TRS(position, rotation, vector));
			}
			else if (xAlpha < 1f)
			{
				RuntimeHandles.ShapesMaterialZTest3.SetPass(0);
				RuntimeHandles.ShapesMaterialZTest3.color = new Color(1f, 1f, 1f, yAlpha);
				Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoYAxis, Matrix4x4.TRS(position, rotation, vector));
				RuntimeHandles.ShapesMaterialZTest4.SetPass(0);
				RuntimeHandles.ShapesMaterialZTest4.color = new Color(1f, 1f, 1f, zAlpha);
				Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoZAxis, Matrix4x4.TRS(position, rotation, vector));
				RuntimeHandles.ShapesMaterialZTest2.SetPass(0);
				RuntimeHandles.ShapesMaterialZTest2.color = new Color(1f, 1f, 1f, xAlpha);
				Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoXAxis, Matrix4x4.TRS(position, rotation, vector));
				RuntimeHandles.XMaterial.SetPass(0);
			}
			else if (yAlpha < 1f)
			{
				RuntimeHandles.ShapesMaterialZTest4.SetPass(0);
				RuntimeHandles.ShapesMaterialZTest4.color = new Color(1f, 1f, 1f, zAlpha);
				Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoZAxis, Matrix4x4.TRS(position, rotation, vector));
				RuntimeHandles.ShapesMaterialZTest2.SetPass(0);
				RuntimeHandles.ShapesMaterialZTest2.color = new Color(1f, 1f, 1f, xAlpha);
				Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoXAxis, Matrix4x4.TRS(position, rotation, vector));
				RuntimeHandles.ShapesMaterialZTest3.SetPass(0);
				RuntimeHandles.ShapesMaterialZTest3.color = new Color(1f, 1f, 1f, yAlpha);
				Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoYAxis, Matrix4x4.TRS(position, rotation, vector));
			}
			else
			{
				RuntimeHandles.ShapesMaterialZTest2.SetPass(0);
				RuntimeHandles.ShapesMaterialZTest2.color = new Color(1f, 1f, 1f, xAlpha);
				Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoXAxis, Matrix4x4.TRS(position, rotation, vector));
				RuntimeHandles.ShapesMaterialZTest3.SetPass(0);
				RuntimeHandles.ShapesMaterialZTest3.color = new Color(1f, 1f, 1f, yAlpha);
				Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoYAxis, Matrix4x4.TRS(position, rotation, vector));
				RuntimeHandles.ShapesMaterialZTest4.SetPass(0);
				RuntimeHandles.ShapesMaterialZTest4.color = new Color(1f, 1f, 1f, zAlpha);
				Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoZAxis, Matrix4x4.TRS(position, rotation, vector));
			}
			RuntimeHandles.XMaterial.SetPass(0);
			RuntimeHandles.XMaterial.color = new Color(1f, 1f, 1f, xAlpha);
			RuntimeHandles.DragSceneGizmoAxis(position, rotation, Vector3.right, gizmoScale, 0.125f, billboardOffset, num);
			RuntimeHandles.YMaterial.SetPass(0);
			RuntimeHandles.YMaterial.color = new Color(1f, 1f, 1f, yAlpha);
			RuntimeHandles.DragSceneGizmoAxis(position, rotation, Vector3.up, gizmoScale, 0.125f, billboardOffset, num);
			RuntimeHandles.ZMaterial.SetPass(0);
			RuntimeHandles.ZMaterial.color = new Color(1f, 1f, 1f, zAlpha);
			RuntimeHandles.DragSceneGizmoAxis(position, rotation, Vector3.forward, gizmoScale, 0.125f, billboardOffset, num);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00026BE4 File Offset: 0x00024FE4
		private static void DragSceneGizmoAxis(Vector3 position, Quaternion rotation, Vector3 axis, float gizmoScale, float billboardScale, float billboardOffset, float sScale)
		{
			Vector3 vector = Vector3.Reflect(Camera.current.transform.forward, axis) * 0.1f;
			float num = Vector3.Dot(Camera.current.transform.forward, axis);
			if (num > 0f)
			{
				if (Camera.current.orthographic)
				{
					vector += axis * num * 0.4f;
				}
				else
				{
					vector = axis * num * 0.7f;
				}
			}
			else if (Camera.current.orthographic)
			{
				vector -= axis * num * 0.1f;
			}
			else
			{
				vector = Vector3.zero;
			}
			Vector3 vector2 = position + (axis + vector) * billboardOffset * sScale;
			float num2 = RuntimeHandles.GetScreenScale(vector2, Camera.current) * gizmoScale;
			Vector3 a = new Vector3(num2, num2, num2);
			Graphics.DrawMeshNow(RuntimeHandles.SceneGizmoQuad, Matrix4x4.TRS(vector2, rotation, a * billboardScale));
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00026CF8 File Offset: 0x000250F8
		public static float GetGridFarPlane()
		{
			float y = Camera.current.transform.position.y;
			float num = RuntimeHandles.CountOfDigits(y);
			float num2 = Mathf.Pow(10f, num - 1f);
			return num2 * 150f;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00026D40 File Offset: 0x00025140
		public static void DrawGrid(Vector3 gridOffset, float camOffset = 0f)
		{
			float num = Mathf.Abs(camOffset);
			num = Mathf.Max(1f, num);
			float num2 = RuntimeHandles.CountOfDigits(num);
			float num3 = Mathf.Pow(10f, num2 - 1f);
			float num4 = Mathf.Pow(10f, num2);
			float num5 = Mathf.Pow(10f, num2 + 1f);
			float alpha = 1f - (num - num3) / (num4 - num3);
			float alpha2 = (num * 10f - num4) / (num5 - num4);
			Vector3 position = Camera.current.transform.position;
			RuntimeHandles.DrawGrid(position, gridOffset, num3, alpha, num * 20f);
			RuntimeHandles.DrawGrid(position, gridOffset, num4, alpha2, num * 20f);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00026DF0 File Offset: 0x000251F0
		private static void DrawGrid(Vector3 cameraPosition, Vector3 gridOffset, float spacing, float alpha, float fadeDisance)
		{
			cameraPosition.y = gridOffset.y;
			gridOffset.y = 0f;
			RuntimeHandles.GridMaterial.SetFloat("_FadeDistance", fadeDisance);
			RuntimeHandles.GridMaterial.SetPass(0);
			GL.Begin(1);
			GL.Color(new Color(1f, 1f, 1f, 0.1f * alpha));
			cameraPosition.x = Mathf.Floor(cameraPosition.x / spacing) * spacing;
			cameraPosition.z = Mathf.Floor(cameraPosition.z / spacing) * spacing;
			for (int i = -150; i < 150; i++)
			{
				GL.Vertex(gridOffset + cameraPosition + new Vector3((float)i * spacing, 0f, -150f * spacing));
				GL.Vertex(gridOffset + cameraPosition + new Vector3((float)i * spacing, 0f, 150f * spacing));
				GL.Vertex(gridOffset + cameraPosition + new Vector3(-150f * spacing, 0f, (float)i * spacing));
				GL.Vertex(gridOffset + cameraPosition + new Vector3(150f * spacing, 0f, (float)i * spacing));
			}
			GL.End();
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00026F40 File Offset: 0x00025340
		public static void DrawBoundRay(ref Bounds bounds, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			RuntimeHandles.LinesMaterialZTest.SetPass(0);
			Matrix4x4 m = Matrix4x4.TRS(position, rotation, scale);
			Vector3 position2 = m.MultiplyPoint(bounds.center);
			float screenScale = RuntimeHandles.GetScreenScale(position2, Camera.current);
			float num = 10f * screenScale;
			Vector3 vector = bounds.center;
			Vector3 a = bounds.center + new Vector3(0f, -num, 0f);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GL.Begin(1);
			GL.Color(RTHColors.RaysColor);
			int num2 = 100;
			Vector3 vector2 = a - vector;
			vector2 /= (float)num2;
			for (int i = 0; i < num2; i++)
			{
				vector += vector2;
				GL.Vertex(vector);
				GL.Vertex(vector + vector2);
				vector += vector2;
			}
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00027034 File Offset: 0x00025434
		public static void DrawBounds(ref Bounds bounds, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			RuntimeHandles.LinesMaterialZTest.SetPass(0);
			Matrix4x4 m = Matrix4x4.TRS(position, rotation, scale);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GL.Begin(1);
			GL.Color(RTHColors.BoundsColor);
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = -1; j <= 1; j += 2)
				{
					for (int k = -1; k <= 1; k += 2)
					{
						Vector3 vector = bounds.center + new Vector3(bounds.extents.x * (float)i, bounds.extents.y * (float)j, bounds.extents.z * (float)k);
						Vector3 position2 = m.MultiplyPoint(vector);
						float d = Mathf.Max(RuntimeHandles.GetScreenScale(position2, Camera.current), 0.1f);
						Vector3 vector2 = Vector3.one * 0.2f * d;
						Vector3 sizeX = new Vector3(Mathf.Min(vector2.x / Mathf.Abs(scale.x), bounds.extents.x), 0f, 0f);
						Vector3 sizeY = new Vector3(0f, Mathf.Min(vector2.y / Mathf.Abs(scale.y), bounds.extents.y), 0f);
						Vector3 sizeZ = new Vector3(0f, 0f, Mathf.Min(vector2.z / Mathf.Abs(scale.z), bounds.extents.z));
						RuntimeHandles.DrawCorner(vector, sizeX, sizeY, sizeZ, new Vector3((float)(-1 * i), (float)(-1 * j), (float)(-1 * k)));
					}
				}
			}
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00027204 File Offset: 0x00025604
		private static void DrawCorner(Vector3 p, Vector3 sizeX, Vector3 sizeY, Vector3 sizeZ, Vector3 s)
		{
			GL.Vertex(p);
			GL.Vertex(p + sizeX * s.x);
			GL.Vertex(p);
			GL.Vertex(p + sizeY * s.y);
			GL.Vertex(p);
			GL.Vertex(p + sizeZ * s.z);
			GL.Vertex(p);
			GL.Vertex(p + sizeX * s.x);
			GL.Vertex(p);
			GL.Vertex(p + sizeY * s.y);
			GL.Vertex(p);
			GL.Vertex(p + sizeZ * s.z);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x000272C5 File Offset: 0x000256C5
		public static float CountOfDigits(float number)
		{
			return (number != 0f) ? Mathf.Ceil(Mathf.Log10(Mathf.Abs(number) + 0.5f)) : 1f;
		}

		// Token: 0x040005B4 RID: 1460
		public const float HandleScale = 1f;

		// Token: 0x040005B5 RID: 1461
		private static readonly Mesh Arrows;

		// Token: 0x040005B6 RID: 1462
		private static readonly Mesh ArrowY;

		// Token: 0x040005B7 RID: 1463
		private static readonly Mesh ArrowX;

		// Token: 0x040005B8 RID: 1464
		private static readonly Mesh ArrowZ;

		// Token: 0x040005B9 RID: 1465
		private static readonly Mesh SelectionArrowY;

		// Token: 0x040005BA RID: 1466
		private static readonly Mesh SelectionArrowX;

		// Token: 0x040005BB RID: 1467
		private static readonly Mesh SelectionArrowZ;

		// Token: 0x040005BC RID: 1468
		private static readonly Mesh DisabledArrowY;

		// Token: 0x040005BD RID: 1469
		private static readonly Mesh DisabledArrowX;

		// Token: 0x040005BE RID: 1470
		private static readonly Mesh DisabledArrowZ;

		// Token: 0x040005BF RID: 1471
		private static readonly Mesh SelectionCube;

		// Token: 0x040005C0 RID: 1472
		private static readonly Mesh DisabledCube;

		// Token: 0x040005C1 RID: 1473
		private static readonly Mesh CubeX;

		// Token: 0x040005C2 RID: 1474
		private static readonly Mesh CubeY;

		// Token: 0x040005C3 RID: 1475
		private static readonly Mesh CubeZ;

		// Token: 0x040005C4 RID: 1476
		private static readonly Mesh CubeUniform;

		// Token: 0x040005C5 RID: 1477
		private static readonly Mesh SceneGizmoSelectedAxis;

		// Token: 0x040005C6 RID: 1478
		private static readonly Mesh SceneGizmoXAxis;

		// Token: 0x040005C7 RID: 1479
		private static readonly Mesh SceneGizmoYAxis;

		// Token: 0x040005C8 RID: 1480
		private static readonly Mesh SceneGizmoZAxis;

		// Token: 0x040005C9 RID: 1481
		private static readonly Mesh SceneGizmoCube;

		// Token: 0x040005CA RID: 1482
		private static readonly Mesh SceneGizmoSelectedCube;

		// Token: 0x040005CB RID: 1483
		private static readonly Mesh SceneGizmoQuad;

		// Token: 0x040005CC RID: 1484
		private static readonly Material ShapesMaterialZTest;

		// Token: 0x040005CD RID: 1485
		private static readonly Material ShapesMaterialZTest2;

		// Token: 0x040005CE RID: 1486
		private static readonly Material ShapesMaterialZTest3;

		// Token: 0x040005CF RID: 1487
		private static readonly Material ShapesMaterialZTest4;

		// Token: 0x040005D0 RID: 1488
		private static readonly Material ShapesMaterialZTestOffset;

		// Token: 0x040005D1 RID: 1489
		private static readonly Material ShapesMaterial;

		// Token: 0x040005D2 RID: 1490
		private static readonly Material LinesMaterial = new Material(Shader.Find("Battlehub/RTHandles/VertexColor"));

		// Token: 0x040005D3 RID: 1491
		private static readonly Material LinesMaterialZTest;

		// Token: 0x040005D4 RID: 1492
		private static readonly Material LinesClipMaterial;

		// Token: 0x040005D5 RID: 1493
		private static readonly Material LinesBillboardMaterial;

		// Token: 0x040005D6 RID: 1494
		private static readonly Material XMaterial;

		// Token: 0x040005D7 RID: 1495
		private static readonly Material YMaterial;

		// Token: 0x040005D8 RID: 1496
		private static readonly Material ZMaterial;

		// Token: 0x040005D9 RID: 1497
		private static readonly Material GridMaterial;
	}
}
