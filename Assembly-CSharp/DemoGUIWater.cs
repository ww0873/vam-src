using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000303 RID: 771
public class DemoGUIWater : MonoBehaviour
{
	// Token: 0x06001237 RID: 4663 RVA: 0x000648A4 File Offset: 0x00062CA4
	public DemoGUIWater()
	{
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x000648F5 File Offset: 0x00062CF5
	private void Start()
	{
		this.guiStyleHeader.fontSize = 18;
		this.guiStyleHeader.normal.textColor = new Color(1f, 0f, 0f);
		this.UpdateCurrentWater();
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x00064930 File Offset: 0x00062D30
	private void UpdateCurrentWater()
	{
		if (this.Boat != null)
		{
			this.Boat.SetActive(false);
			this.Boat.SetActive(true);
		}
		this.startSunIntencity = this.Sun.intensity;
		this.currentWater = GameObject.Find("Water");
		this.currentWaterMaterial = this.currentWater.GetComponent<Renderer>().material;
		this.refl = this.currentWaterMaterial.GetColor("_ReflectionColor").r;
		if (!this.IsMobileScene)
		{
			this.transparent = this.currentWaterMaterial.GetFloat("_DepthTransperent");
		}
		if (!this.IsMobileScene)
		{
			this.fadeBlend = this.currentWaterMaterial.GetFloat("_FadeDepth");
		}
		this.refraction = this.currentWaterMaterial.GetFloat("_Distortion");
		this.oldTextureScale = this.currentWaterMaterial.GetFloat("_TexturesScale");
		this.oldWaveScale = this.currentWaterMaterial.GetFloat("_WaveScale");
		GameObject gameObject = GameObject.Find("InfiniteWaterMesh");
		if (gameObject != null)
		{
			gameObject.GetComponent<Renderer>().material = this.currentWaterMaterial;
		}
		GameObject gameObject2 = GameObject.Find("ProjectorCausticScale");
		if (gameObject2 != null)
		{
			this.oldCausticScale = gameObject2.transform.localScale;
		}
		this.caustic = GameObject.Find("Caustic");
		if (this.IsMobileScene)
		{
			this.caustic.SetActive(true);
		}
		if (!this.IsMobileScene)
		{
			this.causticMaterial = this.caustic.GetComponent<Projector>().material;
		}
		this.waterDirection = this.currentWaterMaterial.GetVector("_Direction");
		if (!this.IsMobileScene)
		{
			this.foamDirection = this.currentWaterMaterial.GetVector("_FoamDirection");
		}
		if (!this.IsMobileScene)
		{
			this.causticDirection = this.causticMaterial.GetVector("_CausticDirection");
		}
		this.ABDirection = this.currentWaterMaterial.GetVector("_GDirectionAB");
		this.CDDirection = this.currentWaterMaterial.GetVector("_GDirectionCD");
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x00064B5B File Offset: 0x00062F5B
	private void OnGUI()
	{
		if (this.IsMobileScene)
		{
			this.GUIMobile();
		}
		else
		{
			this.GUIPC();
		}
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x00064B7C File Offset: 0x00062F7C
	private void GUIMobile()
	{
		if (this.currentWaterMaterial == null)
		{
			return;
		}
		if (GUI.Button(new Rect(10f, 35f, 150f, 40f), "On/Off Ripples"))
		{
			this.caustic.SetActive(true);
			this.water1.SetActive(!this.water1.activeSelf);
			this.water2.SetActive(!this.water2.activeSelf);
			this.caustic = GameObject.Find("Caustic");
			if (this.IsMobileScene)
			{
				this.caustic.SetActive(true);
			}
		}
		if (GUI.Button(new Rect(10f, 190f, 150f, 40f), "On/Off caustic"))
		{
			this.caustic.SetActive(!this.caustic.activeSelf);
		}
		GUIStyle guistyle = new GUIStyle();
		guistyle.normal.textColor = new Color(1f, 1f, 1f);
		this.angle = GUI.HorizontalSlider(new Rect(10f, 102f, 120f, 15f), this.angle, 30f, 240f);
		GUI.Label(new Rect(140f, 100f, 30f, 30f), "Day Time", guistyle);
		float value = Mathf.Sin((this.angle - 60f) / 50f);
		this.Sun.intensity = Mathf.Clamp01(value) * this.startSunIntencity + 0.05f;
		this.SunTransform.transform.rotation = Quaternion.Euler(0f, 0f, this.angle);
		this.refl = GUI.HorizontalSlider(new Rect(10f, 122f, 120f, 15f), this.refl, 0f, 1f);
		this.reflectionColor = new Color(this.refl, this.refl, this.refl, 1f);
		GUI.Label(new Rect(140f, 120f, 30f, 30f), "Reflection", guistyle);
		this.currentWaterMaterial.SetColor("_ReflectionColor", this.reflectionColor);
		this.refraction = GUI.HorizontalSlider(new Rect(10f, 142f, 120f, 15f), this.refraction, 0f, 700f);
		GUI.Label(new Rect(140f, 140f, 30f, 30f), "Distortion", guistyle);
		this.currentWaterMaterial.SetFloat("_Distortion", this.refraction);
		this.waterWaveScaleXZ = GUI.HorizontalSlider(new Rect(10f, 162f, 120f, 15f), this.waterWaveScaleXZ, 0.3f, 3f);
		GUI.Label(new Rect(140f, 160f, 30f, 30f), "Scale", guistyle);
		this.currentWaterMaterial.SetFloat("_WaveScale", this.oldWaveScale * this.waterWaveScaleXZ);
		this.currentWaterMaterial.SetFloat("_TexturesScale", this.oldTextureScale * this.waterWaveScaleXZ);
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x00064ED4 File Offset: 0x000632D4
	private void GUIPC()
	{
		if (this.currentWaterMaterial == null)
		{
			return;
		}
		if (GUI.Button(new Rect(10f, 35f, 150f, 40f), "Change Scene "))
		{
			int buildIndex = SceneManager.GetActiveScene().buildIndex;
			if (buildIndex == this.MaxScenes - 1)
			{
				SceneManager.LoadScene(0);
			}
			else
			{
				SceneManager.LoadScene(buildIndex + 1);
			}
			this.UpdateCurrentWater();
		}
		GUIStyle guistyle = new GUIStyle();
		guistyle.normal.textColor = new Color(1f, 1f, 1f);
		this.angle = GUI.HorizontalSlider(new Rect(10f, 102f, 120f, 15f), this.angle, 30f, 240f);
		GUI.Label(new Rect(140f, 100f, 30f, 30f), "Day Time", guistyle);
		float value = Mathf.Sin((this.angle - 60f) / 50f);
		this.Sun.intensity = Mathf.Clamp01(value) * this.startSunIntencity + 0.05f;
		this.SunTransform.transform.rotation = Quaternion.Euler(0f, 0f, this.angle);
		this.transparent = GUI.HorizontalSlider(new Rect(10f, 122f, 120f, 15f), this.transparent, 0f, 1f);
		GUI.Label(new Rect(140f, 120f, 30f, 30f), "Depth Transperent", guistyle);
		this.currentWaterMaterial.SetFloat("_DepthTransperent", this.transparent);
		this.fadeBlend = GUI.HorizontalSlider(new Rect(10f, 142f, 120f, 15f), this.fadeBlend, 0f, 1f);
		GUI.Label(new Rect(140f, 140f, 30f, 30f), "Fade Depth", guistyle);
		this.currentWaterMaterial.SetFloat("_FadeDepth", this.fadeBlend);
		this.refl = GUI.HorizontalSlider(new Rect(10f, 162f, 120f, 15f), this.refl, 0f, 1f);
		this.reflectionColor = new Color(this.refl, this.refl, this.refl, 1f);
		GUI.Label(new Rect(140f, 160f, 30f, 30f), "Reflection", guistyle);
		this.currentWaterMaterial.SetColor("_ReflectionColor", this.reflectionColor);
		this.refraction = GUI.HorizontalSlider(new Rect(10f, 182f, 120f, 15f), this.refraction, 0f, 700f);
		GUI.Label(new Rect(140f, 180f, 30f, 30f), "Distortion", guistyle);
		this.currentWaterMaterial.SetFloat("_Distortion", this.refraction);
		this.waterWaveScaleXZ = GUI.HorizontalSlider(new Rect(10f, 202f, 120f, 15f), this.waterWaveScaleXZ, 0.3f, 3f);
		GUI.Label(new Rect(140f, 200f, 30f, 30f), "Scale", guistyle);
		this.currentWaterMaterial.SetFloat("_WaveScale", this.oldWaveScale * this.waterWaveScaleXZ);
		this.currentWaterMaterial.SetFloat("_TexturesScale", this.oldTextureScale * this.waterWaveScaleXZ);
		GameObject gameObject = GameObject.Find("ProjectorCausticScale");
		Vector3 vector = this.oldCausticScale * this.waterWaveScaleXZ;
		if ((double)(gameObject.transform.localScale - vector).magnitude > 0.01)
		{
			gameObject.transform.localScale = vector;
			this.caustic.GetComponent<ProjectorMatrix>().UpdateMatrix();
		}
		this.direction = GUI.HorizontalSlider(new Rect(10f, 222f, 120f, 15f), this.direction, 1f, -1f);
		GUI.Label(new Rect(140f, 220f, 30f, 30f), "Direction", guistyle);
		this.currentWaterMaterial.SetVector("_Direction", this.waterDirection * this.direction);
		this.currentWaterMaterial.SetVector("_FoamDirection", this.foamDirection * this.direction);
		this.causticMaterial.SetVector("_CausticDirection", this.causticDirection * this.direction);
		this.currentWaterMaterial.SetVector("_GDirectionAB", this.ABDirection * this.direction);
		this.currentWaterMaterial.SetVector("_GDirectionCD", this.CDDirection * this.direction);
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x000653F9 File Offset: 0x000637F9
	private void OnDestroy()
	{
		if (!this.IsMobileScene)
		{
			this.causticMaterial.SetVector("_CausticDirection", this.causticDirection);
		}
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x0006541C File Offset: 0x0006381C
	private void OnSetColorMain(Color color)
	{
		this.currentWaterMaterial.SetColor("_Color", color);
	}

	// Token: 0x04000F7F RID: 3967
	public float UpdateInterval = 0.5f;

	// Token: 0x04000F80 RID: 3968
	public int MaxScenes = 2;

	// Token: 0x04000F81 RID: 3969
	public bool IsMobileScene;

	// Token: 0x04000F82 RID: 3970
	public Light Sun;

	// Token: 0x04000F83 RID: 3971
	public GameObject SunTransform;

	// Token: 0x04000F84 RID: 3972
	public GameObject Boat;

	// Token: 0x04000F85 RID: 3973
	public GameObject water1;

	// Token: 0x04000F86 RID: 3974
	public GameObject water2;

	// Token: 0x04000F87 RID: 3975
	public float angle = 130f;

	// Token: 0x04000F88 RID: 3976
	private bool canUpdateTestMaterial;

	// Token: 0x04000F89 RID: 3977
	private GameObject cam;

	// Token: 0x04000F8A RID: 3978
	private GUIStyle guiStyleHeader = new GUIStyle();

	// Token: 0x04000F8B RID: 3979
	private Material currentWaterMaterial;

	// Token: 0x04000F8C RID: 3980
	private Material causticMaterial;

	// Token: 0x04000F8D RID: 3981
	private GameObject currentWater;

	// Token: 0x04000F8E RID: 3982
	private float transparent;

	// Token: 0x04000F8F RID: 3983
	private float fadeBlend;

	// Token: 0x04000F90 RID: 3984
	private float refl;

	// Token: 0x04000F91 RID: 3985
	private float refraction;

	// Token: 0x04000F92 RID: 3986
	private float waterWaveScaleXZ = 1f;

	// Token: 0x04000F93 RID: 3987
	private Vector4 waterDirection;

	// Token: 0x04000F94 RID: 3988
	private Vector4 causticDirection;

	// Token: 0x04000F95 RID: 3989
	private Vector4 foamDirection;

	// Token: 0x04000F96 RID: 3990
	private Vector4 ABDirection;

	// Token: 0x04000F97 RID: 3991
	private Vector4 CDDirection;

	// Token: 0x04000F98 RID: 3992
	private float direction = 1f;

	// Token: 0x04000F99 RID: 3993
	private Color reflectionColor;

	// Token: 0x04000F9A RID: 3994
	private Vector3 oldCausticScale;

	// Token: 0x04000F9B RID: 3995
	private float oldTextureScale;

	// Token: 0x04000F9C RID: 3996
	private float oldWaveScale;

	// Token: 0x04000F9D RID: 3997
	private GameObject caustic;

	// Token: 0x04000F9E RID: 3998
	private float startSunIntencity;
}
