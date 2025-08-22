using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000964 RID: 2404
public class OVRGridCube : MonoBehaviour
{
	// Token: 0x06003BEB RID: 15339 RVA: 0x00121594 File Offset: 0x0011F994
	public OVRGridCube()
	{
	}

	// Token: 0x06003BEC RID: 15340 RVA: 0x001215CF File Offset: 0x0011F9CF
	private void Update()
	{
		this.UpdateCubeGrid();
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x001215D7 File Offset: 0x0011F9D7
	public void SetOVRCameraController(ref OVRCameraRig cameraController)
	{
		this.CameraController = cameraController;
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x001215E4 File Offset: 0x0011F9E4
	private void UpdateCubeGrid()
	{
		if (Input.GetKeyDown(this.GridKey))
		{
			if (!this.CubeGridOn)
			{
				this.CubeGridOn = true;
				Debug.LogWarning("CubeGrid ON");
				if (this.CubeGrid != null)
				{
					this.CubeGrid.SetActive(true);
				}
				else
				{
					this.CreateCubeGrid();
				}
			}
			else
			{
				this.CubeGridOn = false;
				Debug.LogWarning("CubeGrid OFF");
				if (this.CubeGrid != null)
				{
					this.CubeGrid.SetActive(false);
				}
			}
		}
		if (this.CubeGrid != null)
		{
			this.CubeSwitchColor = !OVRManager.tracker.isPositionTracked;
			if (this.CubeSwitchColor != this.CubeSwitchColorOld)
			{
				this.CubeGridSwitchColor(this.CubeSwitchColor);
			}
			this.CubeSwitchColorOld = this.CubeSwitchColor;
		}
	}

	// Token: 0x06003BEF RID: 15343 RVA: 0x001216C8 File Offset: 0x0011FAC8
	private void CreateCubeGrid()
	{
		Debug.LogWarning("Create CubeGrid");
		this.CubeGrid = new GameObject("CubeGrid");
		this.CubeGrid.layer = this.CameraController.gameObject.layer;
		for (int i = -this.gridSizeX; i <= this.gridSizeX; i++)
		{
			for (int j = -this.gridSizeY; j <= this.gridSizeY; j++)
			{
				for (int k = -this.gridSizeZ; k <= this.gridSizeZ; k++)
				{
					int num = 0;
					if ((i == 0 && j == 0) || (i == 0 && k == 0) || (j == 0 && k == 0))
					{
						if (i == 0 && j == 0 && k == 0)
						{
							num = 2;
						}
						else
						{
							num = 1;
						}
					}
					GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
					BoxCollider component = gameObject.GetComponent<BoxCollider>();
					component.enabled = false;
					gameObject.layer = this.CameraController.gameObject.layer;
					Renderer component2 = gameObject.GetComponent<Renderer>();
					component2.shadowCastingMode = ShadowCastingMode.Off;
					component2.receiveShadows = false;
					if (num == 0)
					{
						component2.material.color = Color.red;
					}
					else if (num == 1)
					{
						component2.material.color = Color.white;
					}
					else
					{
						component2.material.color = Color.yellow;
					}
					gameObject.transform.position = new Vector3((float)i * this.gridScale, (float)j * this.gridScale, (float)k * this.gridScale);
					float num2 = 0.7f;
					if (num == 1)
					{
						num2 = 1f;
					}
					if (num == 2)
					{
						num2 = 2f;
					}
					gameObject.transform.localScale = new Vector3(this.cubeScale * num2, this.cubeScale * num2, this.cubeScale * num2);
					gameObject.transform.parent = this.CubeGrid.transform;
				}
			}
		}
	}

	// Token: 0x06003BF0 RID: 15344 RVA: 0x001218C8 File Offset: 0x0011FCC8
	private void CubeGridSwitchColor(bool CubeSwitchColor)
	{
		Color color = Color.red;
		if (CubeSwitchColor)
		{
			color = Color.blue;
		}
		IEnumerator enumerator = this.CubeGrid.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				Material material = transform.GetComponent<Renderer>().material;
				if (material.color == Color.red || material.color == Color.blue)
				{
					material.color = color;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x04002DD2 RID: 11730
	public KeyCode GridKey = KeyCode.G;

	// Token: 0x04002DD3 RID: 11731
	private GameObject CubeGrid;

	// Token: 0x04002DD4 RID: 11732
	private bool CubeGridOn;

	// Token: 0x04002DD5 RID: 11733
	private bool CubeSwitchColorOld;

	// Token: 0x04002DD6 RID: 11734
	private bool CubeSwitchColor;

	// Token: 0x04002DD7 RID: 11735
	private int gridSizeX = 6;

	// Token: 0x04002DD8 RID: 11736
	private int gridSizeY = 4;

	// Token: 0x04002DD9 RID: 11737
	private int gridSizeZ = 6;

	// Token: 0x04002DDA RID: 11738
	private float gridScale = 0.3f;

	// Token: 0x04002DDB RID: 11739
	private float cubeScale = 0.03f;

	// Token: 0x04002DDC RID: 11740
	private OVRCameraRig CameraController;
}
