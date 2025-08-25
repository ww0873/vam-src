using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace VikingCrewTools.UI
{
	// Token: 0x0200056D RID: 1389
	public class SpeechBubbleBehaviour : MonoBehaviour
	{
		// Token: 0x06002332 RID: 9010 RVA: 0x000C88BB File Offset: 0x000C6CBB
		public SpeechBubbleBehaviour()
		{
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06002333 RID: 9011 RVA: 0x000C88CE File Offset: 0x000C6CCE
		public int Iteration
		{
			get
			{
				return this._iteration;
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06002334 RID: 9012 RVA: 0x000C88D6 File Offset: 0x000C6CD6
		// (set) Token: 0x06002335 RID: 9013 RVA: 0x000C88DE File Offset: 0x000C6CDE
		public Camera Cam
		{
			get
			{
				return this._cam;
			}
			set
			{
				this._cam = value;
			}
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x000C88E8 File Offset: 0x000C6CE8
		protected void Update()
		{
			this._timeToLive -= Time.unscaledDeltaTime;
			if (0f < this._timeToLive && this._timeToLive < 1f)
			{
				this._image.color = new Color(this._image.color.r, this._image.color.g, this._image.color.b, this._timeToLive);
				this._text.color = new Color(this._text.color.r, this._text.color.g, this._text.color.b, this._timeToLive);
			}
			if (this._timeToLive <= 0f)
			{
				this.Clear();
			}
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x000C89E0 File Offset: 0x000C6DE0
		protected void LateUpdate()
		{
			if (this._objectToFollow != null)
			{
				base.transform.position = this._objectToFollow.position + this._offset;
			}
			base.transform.rotation = this._cam.transform.rotation;
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x000C8A3A File Offset: 0x000C6E3A
		public void Clear()
		{
			base.gameObject.SetActive(false);
			this._iteration++;
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x000C8A56 File Offset: 0x000C6E56
		public void UpdateText(string text, float newTimeToLive)
		{
			this._text.text = text;
			this._timeToLive = newTimeToLive;
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x000C8A6C File Offset: 0x000C6E6C
		public void Setup(Vector3 position, string text, float timeToLive, Color color, Camera cam)
		{
			this.Setup(text, timeToLive, color, cam);
			base.transform.position = position;
			base.transform.rotation = this._cam.transform.rotation;
			this._objectToFollow = null;
			this._offset = Vector3.zero;
			if (timeToLive > 0f)
			{
				base.gameObject.SetActive(true);
			}
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x000C8AD8 File Offset: 0x000C6ED8
		public void Setup(Transform objectToFollow, Vector3 offset, string text, float timeToLive, Color color, Camera cam)
		{
			this.Setup(text, timeToLive, color, cam);
			this._objectToFollow = objectToFollow;
			base.transform.position = objectToFollow.position + offset;
			base.transform.rotation = this._cam.transform.rotation;
			this._offset = offset;
			if (timeToLive > 0f)
			{
				base.gameObject.SetActive(true);
			}
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x000C8B4C File Offset: 0x000C6F4C
		private void Setup(string text, float timeToLive, Color color, Camera cam)
		{
			if (cam)
			{
				this._cam = cam;
			}
			else
			{
				this._cam = Camera.main;
			}
			this._timeToLive = timeToLive;
			this._text.text = text;
			this._image.color = color;
			this._text.color = new Color(this._text.color.r, this._text.color.g, this._text.color.b, 1f);
		}

		// Token: 0x04001D29 RID: 7465
		private float _timeToLive = 1f;

		// Token: 0x04001D2A RID: 7466
		private Transform _objectToFollow;

		// Token: 0x04001D2B RID: 7467
		private Vector3 _offset;

		// Token: 0x04001D2C RID: 7468
		[FormerlySerializedAs("text")]
		[SerializeField]
		private Text _text;

		// Token: 0x04001D2D RID: 7469
		[FormerlySerializedAs("image")]
		[SerializeField]
		private Image _image;

		// Token: 0x04001D2E RID: 7470
		private int _iteration;

		// Token: 0x04001D2F RID: 7471
		private Camera _cam;
	}
}
