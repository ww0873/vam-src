using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Leap.Unity.Attributes;
using Leap.Unity.Query;
using UnityEngine;

namespace Leap.Unity.Attachments
{
	// Token: 0x02000666 RID: 1638
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	public class AttachmentHand : MonoBehaviour
	{
		// Token: 0x06002810 RID: 10256 RVA: 0x000DCE66 File Offset: 0x000DB266
		public AttachmentHand()
		{
			if (AttachmentHand.<>f__am$cache2 == null)
			{
				AttachmentHand.<>f__am$cache2 = new Action(AttachmentHand.<OnAttachmentPointsModified>m__2);
			}
			this.OnAttachmentPointsModified = AttachmentHand.<>f__am$cache2;
			base..ctor();
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06002811 RID: 10257 RVA: 0x000DCE91 File Offset: 0x000DB291
		public AttachmentHand.AttachmentPointsEnumerator points
		{
			get
			{
				return new AttachmentHand.AttachmentPointsEnumerator(this);
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06002812 RID: 10258 RVA: 0x000DCE99 File Offset: 0x000DB299
		// (set) Token: 0x06002813 RID: 10259 RVA: 0x000DCEA1 File Offset: 0x000DB2A1
		public Chirality chirality
		{
			get
			{
				return this._chirality;
			}
			set
			{
				this._chirality = value;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06002814 RID: 10260 RVA: 0x000DCEAA File Offset: 0x000DB2AA
		// (set) Token: 0x06002815 RID: 10261 RVA: 0x000DCEB2 File Offset: 0x000DB2B2
		public bool isTracked
		{
			get
			{
				return this._isTracked;
			}
			set
			{
				this._isTracked = value;
			}
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x000DCEBB File Offset: 0x000DB2BB
		private void OnValidate()
		{
			this.initializeAttachmentPointFlagConstants();
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000DCEC3 File Offset: 0x000DB2C3
		private void Awake()
		{
			this.initializeAttachmentPointFlagConstants();
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x000DCECB File Offset: 0x000DB2CB
		private void OnDestroy()
		{
			this._isBeingDestroyed = true;
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x000DCED4 File Offset: 0x000DB2D4
		public AttachmentPointBehaviour GetBehaviourForPoint(AttachmentPointFlags singlePoint)
		{
			AttachmentPointBehaviour result = null;
			switch (singlePoint)
			{
			case AttachmentPointFlags.None:
				break;
			default:
				if (singlePoint != AttachmentPointFlags.ThumbProximalJoint)
				{
					if (singlePoint != AttachmentPointFlags.ThumbDistalJoint)
					{
						if (singlePoint != AttachmentPointFlags.ThumbTip)
						{
							if (singlePoint != AttachmentPointFlags.IndexKnuckle)
							{
								if (singlePoint != AttachmentPointFlags.IndexMiddleJoint)
								{
									if (singlePoint != AttachmentPointFlags.IndexDistalJoint)
									{
										if (singlePoint != AttachmentPointFlags.IndexTip)
										{
											if (singlePoint != AttachmentPointFlags.MiddleKnuckle)
											{
												if (singlePoint != AttachmentPointFlags.MiddleMiddleJoint)
												{
													if (singlePoint != AttachmentPointFlags.MiddleDistalJoint)
													{
														if (singlePoint != AttachmentPointFlags.MiddleTip)
														{
															if (singlePoint != AttachmentPointFlags.RingKnuckle)
															{
																if (singlePoint != AttachmentPointFlags.RingMiddleJoint)
																{
																	if (singlePoint != AttachmentPointFlags.RingDistalJoint)
																	{
																		if (singlePoint != AttachmentPointFlags.RingTip)
																		{
																			if (singlePoint != AttachmentPointFlags.PinkyKnuckle)
																			{
																				if (singlePoint != AttachmentPointFlags.PinkyMiddleJoint)
																				{
																					if (singlePoint != AttachmentPointFlags.PinkyDistalJoint)
																					{
																						if (singlePoint == AttachmentPointFlags.PinkyTip)
																						{
																							result = this.pinkyTip;
																						}
																					}
																					else
																					{
																						result = this.pinkyDistalJoint;
																					}
																				}
																				else
																				{
																					result = this.pinkyMiddleJoint;
																				}
																			}
																			else
																			{
																				result = this.pinkyKnuckle;
																			}
																		}
																		else
																		{
																			result = this.ringTip;
																		}
																	}
																	else
																	{
																		result = this.ringDistalJoint;
																	}
																}
																else
																{
																	result = this.ringMiddleJoint;
																}
															}
															else
															{
																result = this.ringKnuckle;
															}
														}
														else
														{
															result = this.middleTip;
														}
													}
													else
													{
														result = this.middleDistalJoint;
													}
												}
												else
												{
													result = this.middleMiddleJoint;
												}
											}
											else
											{
												result = this.middleKnuckle;
											}
										}
										else
										{
											result = this.indexTip;
										}
									}
									else
									{
										result = this.indexDistalJoint;
									}
								}
								else
								{
									result = this.indexMiddleJoint;
								}
							}
							else
							{
								result = this.indexKnuckle;
							}
						}
						else
						{
							result = this.thumbTip;
						}
					}
					else
					{
						result = this.thumbDistalJoint;
					}
				}
				else
				{
					result = this.thumbProximalJoint;
				}
				break;
			case AttachmentPointFlags.Wrist:
				result = this.wrist;
				break;
			case AttachmentPointFlags.Palm:
				result = this.palm;
				break;
			}
			return result;
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000DD0C8 File Offset: 0x000DB4C8
		public void refreshAttachmentTransforms(AttachmentPointFlags points)
		{
			if (this._attachmentPointFlagConstants == null || this._attachmentPointFlagConstants.Length == 0)
			{
				this.initializeAttachmentPointFlagConstants();
			}
			bool flag = false;
			foreach (AttachmentPointFlags attachmentPointFlags in this._attachmentPointFlagConstants)
			{
				if (attachmentPointFlags != AttachmentPointFlags.None)
				{
					if ((!points.Contains(attachmentPointFlags) && this.GetBehaviourForPoint(attachmentPointFlags) != null) || (points.Contains(attachmentPointFlags) && this.GetBehaviourForPoint(attachmentPointFlags) == null))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.flattenAttachmentTransformHierarchy();
				foreach (AttachmentPointFlags attachmentPointFlags2 in this._attachmentPointFlagConstants)
				{
					if (attachmentPointFlags2 != AttachmentPointFlags.None)
					{
						if (points.Contains(attachmentPointFlags2))
						{
							this.ensureTransformExists(attachmentPointFlags2);
						}
						else
						{
							this.ensureTransformDoesNotExist(attachmentPointFlags2);
						}
					}
				}
				this.organizeAttachmentTransforms();
			}
			if (this._attachmentPointsDirty)
			{
				this.OnAttachmentPointsModified();
				this._attachmentPointsDirty = false;
			}
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000DD1E2 File Offset: 0x000DB5E2
		public void notifyPointBehaviourDeleted(AttachmentPointBehaviour point)
		{
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000DD1E4 File Offset: 0x000DB5E4
		private void initializeAttachmentPointFlagConstants()
		{
			Array values = Enum.GetValues(typeof(AttachmentPointFlags));
			if (this._attachmentPointFlagConstants == null || this._attachmentPointFlagConstants.Length == 0)
			{
				this._attachmentPointFlagConstants = new AttachmentPointFlags[values.Length];
			}
			int num = 0;
			IEnumerator enumerator = values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					int num2 = (int)obj;
					this._attachmentPointFlagConstants[num++] = (AttachmentPointFlags)num2;
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

		// Token: 0x0600281D RID: 10269 RVA: 0x000DD288 File Offset: 0x000DB688
		private void setBehaviourForPoint(AttachmentPointFlags singlePoint, AttachmentPointBehaviour behaviour)
		{
			switch (singlePoint)
			{
			case AttachmentPointFlags.None:
				break;
			default:
				if (singlePoint != AttachmentPointFlags.ThumbProximalJoint)
				{
					if (singlePoint != AttachmentPointFlags.ThumbDistalJoint)
					{
						if (singlePoint != AttachmentPointFlags.ThumbTip)
						{
							if (singlePoint != AttachmentPointFlags.IndexKnuckle)
							{
								if (singlePoint != AttachmentPointFlags.IndexMiddleJoint)
								{
									if (singlePoint != AttachmentPointFlags.IndexDistalJoint)
									{
										if (singlePoint != AttachmentPointFlags.IndexTip)
										{
											if (singlePoint != AttachmentPointFlags.MiddleKnuckle)
											{
												if (singlePoint != AttachmentPointFlags.MiddleMiddleJoint)
												{
													if (singlePoint != AttachmentPointFlags.MiddleDistalJoint)
													{
														if (singlePoint != AttachmentPointFlags.MiddleTip)
														{
															if (singlePoint != AttachmentPointFlags.RingKnuckle)
															{
																if (singlePoint != AttachmentPointFlags.RingMiddleJoint)
																{
																	if (singlePoint != AttachmentPointFlags.RingDistalJoint)
																	{
																		if (singlePoint != AttachmentPointFlags.RingTip)
																		{
																			if (singlePoint != AttachmentPointFlags.PinkyKnuckle)
																			{
																				if (singlePoint != AttachmentPointFlags.PinkyMiddleJoint)
																				{
																					if (singlePoint != AttachmentPointFlags.PinkyDistalJoint)
																					{
																						if (singlePoint == AttachmentPointFlags.PinkyTip)
																						{
																							this.pinkyTip = behaviour;
																						}
																					}
																					else
																					{
																						this.pinkyDistalJoint = behaviour;
																					}
																				}
																				else
																				{
																					this.pinkyMiddleJoint = behaviour;
																				}
																			}
																			else
																			{
																				this.pinkyKnuckle = behaviour;
																			}
																		}
																		else
																		{
																			this.ringTip = behaviour;
																		}
																	}
																	else
																	{
																		this.ringDistalJoint = behaviour;
																	}
																}
																else
																{
																	this.ringMiddleJoint = behaviour;
																}
															}
															else
															{
																this.ringKnuckle = behaviour;
															}
														}
														else
														{
															this.middleTip = behaviour;
														}
													}
													else
													{
														this.middleDistalJoint = behaviour;
													}
												}
												else
												{
													this.middleMiddleJoint = behaviour;
												}
											}
											else
											{
												this.middleKnuckle = behaviour;
											}
										}
										else
										{
											this.indexTip = behaviour;
										}
									}
									else
									{
										this.indexDistalJoint = behaviour;
									}
								}
								else
								{
									this.indexMiddleJoint = behaviour;
								}
							}
							else
							{
								this.indexKnuckle = behaviour;
							}
						}
						else
						{
							this.thumbTip = behaviour;
						}
					}
					else
					{
						this.thumbDistalJoint = behaviour;
					}
				}
				else
				{
					this.thumbProximalJoint = behaviour;
				}
				break;
			case AttachmentPointFlags.Wrist:
				this.wrist = behaviour;
				break;
			case AttachmentPointFlags.Palm:
				this.palm = behaviour;
				break;
			}
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x000DD47C File Offset: 0x000DB87C
		private void ensureTransformExists(AttachmentPointFlags singlePoint)
		{
			AttachmentHand.<ensureTransformExists>c__AnonStorey0 <ensureTransformExists>c__AnonStorey = new AttachmentHand.<ensureTransformExists>c__AnonStorey0();
			<ensureTransformExists>c__AnonStorey.singlePoint = singlePoint;
			if (!<ensureTransformExists>c__AnonStorey.singlePoint.IsSinglePoint())
			{
				Debug.LogError("Tried to ensure transform exists for singlePoint, but it contains more than one set flag.");
				return;
			}
			AttachmentPointBehaviour attachmentPointBehaviour = this.GetBehaviourForPoint(<ensureTransformExists>c__AnonStorey.singlePoint);
			if (attachmentPointBehaviour == null)
			{
				AttachmentPointBehaviour attachmentPointBehaviour2 = base.gameObject.GetComponentsInChildren<AttachmentPointBehaviour>().Query<AttachmentPointBehaviour>().FirstOrDefault(new Func<AttachmentPointBehaviour, bool>(<ensureTransformExists>c__AnonStorey.<>m__0));
				if (attachmentPointBehaviour2 == AttachmentPointFlags.None)
				{
					GameObject gameObject = new GameObject(Enum.GetName(typeof(AttachmentPointFlags), <ensureTransformExists>c__AnonStorey.singlePoint));
					attachmentPointBehaviour = gameObject.AddComponent<AttachmentPointBehaviour>();
				}
				else
				{
					attachmentPointBehaviour = attachmentPointBehaviour2;
				}
				attachmentPointBehaviour.attachmentPoint = <ensureTransformExists>c__AnonStorey.singlePoint;
				attachmentPointBehaviour.attachmentHand = this;
				this.setBehaviourForPoint(<ensureTransformExists>c__AnonStorey.singlePoint, attachmentPointBehaviour);
				AttachmentHand.SetTransformParent(attachmentPointBehaviour.transform, base.transform);
				this._attachmentPointsDirty = true;
			}
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x000DD55D File Offset: 0x000DB95D
		private static void SetTransformParent(Transform t, Transform parent)
		{
			t.parent = parent;
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000DD568 File Offset: 0x000DB968
		private void ensureTransformDoesNotExist(AttachmentPointFlags singlePoint)
		{
			if (!singlePoint.IsSinglePoint())
			{
				Debug.LogError("Tried to ensure transform exists for singlePoint, but it contains more than one set flag");
				return;
			}
			AttachmentPointBehaviour behaviourForPoint = this.GetBehaviourForPoint(singlePoint);
			if (behaviourForPoint != null)
			{
				InternalUtility.Destroy(behaviourForPoint.gameObject);
				this.setBehaviourForPoint(singlePoint, null);
				this._attachmentPointsDirty = true;
			}
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000DD5BC File Offset: 0x000DB9BC
		private void flattenAttachmentTransformHierarchy()
		{
			foreach (AttachmentPointBehaviour attachmentPointBehaviour in this.points)
			{
				AttachmentHand.SetTransformParent(attachmentPointBehaviour.transform, base.transform);
			}
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000DD604 File Offset: 0x000DBA04
		private void organizeAttachmentTransforms()
		{
			int num = 0;
			if (this.wrist != null)
			{
				this.wrist.transform.SetSiblingIndex(num++);
			}
			if (this.palm != null)
			{
				this.palm.transform.SetSiblingIndex(num++);
			}
			Transform transform = this.tryStackTransformHierarchy(new MonoBehaviour[]
			{
				this.thumbProximalJoint,
				this.thumbDistalJoint,
				this.thumbTip
			});
			if (transform != null)
			{
				transform.SetSiblingIndex(num++);
			}
			transform = this.tryStackTransformHierarchy(new MonoBehaviour[]
			{
				this.indexKnuckle,
				this.indexMiddleJoint,
				this.indexDistalJoint,
				this.indexTip
			});
			if (transform != null)
			{
				transform.SetSiblingIndex(num++);
			}
			transform = this.tryStackTransformHierarchy(new MonoBehaviour[]
			{
				this.middleKnuckle,
				this.middleMiddleJoint,
				this.middleDistalJoint,
				this.middleTip
			});
			if (transform != null)
			{
				transform.SetSiblingIndex(num++);
			}
			transform = this.tryStackTransformHierarchy(new MonoBehaviour[]
			{
				this.ringKnuckle,
				this.ringMiddleJoint,
				this.ringDistalJoint,
				this.ringTip
			});
			if (transform != null)
			{
				transform.SetSiblingIndex(num++);
			}
			transform = this.tryStackTransformHierarchy(new MonoBehaviour[]
			{
				this.pinkyKnuckle,
				this.pinkyMiddleJoint,
				this.pinkyDistalJoint,
				this.pinkyTip
			});
			if (transform != null)
			{
				transform.SetSiblingIndex(num++);
			}
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x000DD7C0 File Offset: 0x000DBBC0
		private Transform tryStackTransformHierarchy(params Transform[] transforms)
		{
			for (int i = 0; i < AttachmentHand.s_hierarchyTransformsBuffer.Length; i++)
			{
				AttachmentHand.s_hierarchyTransformsBuffer[i] = null;
			}
			int num = 0;
			Query<Transform> query = transforms.Query<Transform>();
			if (AttachmentHand.<>f__am$cache0 == null)
			{
				AttachmentHand.<>f__am$cache0 = new Func<Transform, bool>(AttachmentHand.<tryStackTransformHierarchy>m__0);
			}
			foreach (Transform transform in query.Where(AttachmentHand.<>f__am$cache0))
			{
				AttachmentHand.s_hierarchyTransformsBuffer[num++] = transform;
			}
			for (int j = num - 1; j > 0; j--)
			{
				AttachmentHand.SetTransformParent(AttachmentHand.s_hierarchyTransformsBuffer[j], AttachmentHand.s_hierarchyTransformsBuffer[j - 1]);
			}
			if (num > 0)
			{
				return AttachmentHand.s_hierarchyTransformsBuffer[0];
			}
			return null;
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x000DD8A8 File Offset: 0x000DBCA8
		private Transform tryStackTransformHierarchy(params MonoBehaviour[] monoBehaviours)
		{
			for (int i = 0; i < AttachmentHand.s_transformsBuffer.Length; i++)
			{
				AttachmentHand.s_transformsBuffer[i] = null;
			}
			int num = 0;
			Query<MonoBehaviour> query = monoBehaviours.Query<MonoBehaviour>();
			if (AttachmentHand.<>f__am$cache1 == null)
			{
				AttachmentHand.<>f__am$cache1 = new Func<MonoBehaviour, bool>(AttachmentHand.<tryStackTransformHierarchy>m__1);
			}
			foreach (MonoBehaviour monoBehaviour in query.Where(AttachmentHand.<>f__am$cache1))
			{
				AttachmentHand.s_transformsBuffer[num++] = monoBehaviour.transform;
			}
			return this.tryStackTransformHierarchy(AttachmentHand.s_transformsBuffer);
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x000DD964 File Offset: 0x000DBD64
		private static AttachmentPointFlags GetFlagFromFlagIdx(int pointIdx)
		{
			return (AttachmentPointFlags)(1 << pointIdx + 1);
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x000DD96E File Offset: 0x000DBD6E
		// Note: this type is marked as 'beforefieldinit'.
		static AttachmentHand()
		{
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x000DD986 File Offset: 0x000DBD86
		[CompilerGenerated]
		private static bool <tryStackTransformHierarchy>m__0(Transform t)
		{
			return t != null;
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x000DD98F File Offset: 0x000DBD8F
		[CompilerGenerated]
		private static bool <tryStackTransformHierarchy>m__1(MonoBehaviour b)
		{
			return b != null;
		}

		// Token: 0x06002829 RID: 10281 RVA: 0x000DD998 File Offset: 0x000DBD98
		[CompilerGenerated]
		private static void <OnAttachmentPointsModified>m__2()
		{
		}

		// Token: 0x0400215A RID: 8538
		public Action OnAttachmentPointsModified;

		// Token: 0x0400215B RID: 8539
		[HideInInspector]
		public AttachmentPointBehaviour wrist;

		// Token: 0x0400215C RID: 8540
		[HideInInspector]
		public AttachmentPointBehaviour palm;

		// Token: 0x0400215D RID: 8541
		[HideInInspector]
		public AttachmentPointBehaviour thumbProximalJoint;

		// Token: 0x0400215E RID: 8542
		[HideInInspector]
		public AttachmentPointBehaviour thumbDistalJoint;

		// Token: 0x0400215F RID: 8543
		[HideInInspector]
		public AttachmentPointBehaviour thumbTip;

		// Token: 0x04002160 RID: 8544
		[HideInInspector]
		public AttachmentPointBehaviour indexKnuckle;

		// Token: 0x04002161 RID: 8545
		[HideInInspector]
		public AttachmentPointBehaviour indexMiddleJoint;

		// Token: 0x04002162 RID: 8546
		[HideInInspector]
		public AttachmentPointBehaviour indexDistalJoint;

		// Token: 0x04002163 RID: 8547
		[HideInInspector]
		public AttachmentPointBehaviour indexTip;

		// Token: 0x04002164 RID: 8548
		[HideInInspector]
		public AttachmentPointBehaviour middleKnuckle;

		// Token: 0x04002165 RID: 8549
		[HideInInspector]
		public AttachmentPointBehaviour middleMiddleJoint;

		// Token: 0x04002166 RID: 8550
		[HideInInspector]
		public AttachmentPointBehaviour middleDistalJoint;

		// Token: 0x04002167 RID: 8551
		[HideInInspector]
		public AttachmentPointBehaviour middleTip;

		// Token: 0x04002168 RID: 8552
		[HideInInspector]
		public AttachmentPointBehaviour ringKnuckle;

		// Token: 0x04002169 RID: 8553
		[HideInInspector]
		public AttachmentPointBehaviour ringMiddleJoint;

		// Token: 0x0400216A RID: 8554
		[HideInInspector]
		public AttachmentPointBehaviour ringDistalJoint;

		// Token: 0x0400216B RID: 8555
		[HideInInspector]
		public AttachmentPointBehaviour ringTip;

		// Token: 0x0400216C RID: 8556
		[HideInInspector]
		public AttachmentPointBehaviour pinkyKnuckle;

		// Token: 0x0400216D RID: 8557
		[HideInInspector]
		public AttachmentPointBehaviour pinkyMiddleJoint;

		// Token: 0x0400216E RID: 8558
		[HideInInspector]
		public AttachmentPointBehaviour pinkyDistalJoint;

		// Token: 0x0400216F RID: 8559
		[HideInInspector]
		public AttachmentPointBehaviour pinkyTip;

		// Token: 0x04002170 RID: 8560
		private bool _attachmentPointsDirty;

		// Token: 0x04002171 RID: 8561
		[SerializeField]
		[Disable]
		private Chirality _chirality;

		// Token: 0x04002172 RID: 8562
		[SerializeField]
		[Disable]
		private bool _isTracked;

		// Token: 0x04002173 RID: 8563
		private bool _isBeingDestroyed;

		// Token: 0x04002174 RID: 8564
		private AttachmentPointFlags[] _attachmentPointFlagConstants;

		// Token: 0x04002175 RID: 8565
		private static Transform[] s_hierarchyTransformsBuffer = new Transform[4];

		// Token: 0x04002176 RID: 8566
		private static Transform[] s_transformsBuffer = new Transform[4];

		// Token: 0x04002177 RID: 8567
		[CompilerGenerated]
		private static Func<Transform, bool> <>f__am$cache0;

		// Token: 0x04002178 RID: 8568
		[CompilerGenerated]
		private static Func<MonoBehaviour, bool> <>f__am$cache1;

		// Token: 0x04002179 RID: 8569
		[CompilerGenerated]
		private static Action <>f__am$cache2;

		// Token: 0x02000667 RID: 1639
		public struct AttachmentPointsEnumerator
		{
			// Token: 0x0600282A RID: 10282 RVA: 0x000DD99C File Offset: 0x000DBD9C
			public AttachmentPointsEnumerator(AttachmentHand hand)
			{
				if (hand != null && hand._attachmentPointFlagConstants != null)
				{
					this._curIdx = -1;
					this._hand = hand;
					this._flagsCount = hand._attachmentPointFlagConstants.Length;
				}
				else
				{
					this._curIdx = -1;
					this._hand = null;
					this._flagsCount = 0;
				}
			}

			// Token: 0x0600282B RID: 10283 RVA: 0x000DD9F6 File Offset: 0x000DBDF6
			public AttachmentHand.AttachmentPointsEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x170004FA RID: 1274
			// (get) Token: 0x0600282C RID: 10284 RVA: 0x000DD9FE File Offset: 0x000DBDFE
			public AttachmentPointBehaviour Current
			{
				get
				{
					if (this._hand == null)
					{
						return null;
					}
					return this._hand.GetBehaviourForPoint(AttachmentHand.GetFlagFromFlagIdx(this._curIdx));
				}
			}

			// Token: 0x0600282D RID: 10285 RVA: 0x000DDA2C File Offset: 0x000DBE2C
			public bool MoveNext()
			{
				do
				{
					this._curIdx++;
				}
				while (this._curIdx < this._flagsCount && this._hand.GetBehaviourForPoint(AttachmentHand.GetFlagFromFlagIdx(this._curIdx)) == null);
				return this._curIdx < this._flagsCount;
			}

			// Token: 0x0400217A RID: 8570
			private int _curIdx;

			// Token: 0x0400217B RID: 8571
			private AttachmentHand _hand;

			// Token: 0x0400217C RID: 8572
			private int _flagsCount;
		}

		// Token: 0x02000F91 RID: 3985
		[CompilerGenerated]
		private sealed class <ensureTransformExists>c__AnonStorey0
		{
			// Token: 0x0600745D RID: 29789 RVA: 0x000DDA87 File Offset: 0x000DBE87
			public <ensureTransformExists>c__AnonStorey0()
			{
			}

			// Token: 0x0600745E RID: 29790 RVA: 0x000DDA8F File Offset: 0x000DBE8F
			internal bool <>m__0(AttachmentPointBehaviour p)
			{
				return p.attachmentPoint == this.singlePoint;
			}

			// Token: 0x0400686D RID: 26733
			internal AttachmentPointFlags singlePoint;
		}
	}
}
