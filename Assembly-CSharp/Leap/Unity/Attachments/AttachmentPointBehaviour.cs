using System;
using Leap.Unity.Attributes;
using UnityEngine;

namespace Leap.Unity.Attachments
{
	// Token: 0x0200066A RID: 1642
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	public class AttachmentPointBehaviour : MonoBehaviour
	{
		// Token: 0x0600283F RID: 10303 RVA: 0x000DE02E File Offset: 0x000DC42E
		public AttachmentPointBehaviour()
		{
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x000DE036 File Offset: 0x000DC436
		private void OnValidate()
		{
			if (!this.attachmentPoint.IsSinglePoint() && this.attachmentPoint != AttachmentPointFlags.None)
			{
				Debug.LogError("AttachmentPointBehaviours should refer to a single attachmentPoint flag.", base.gameObject);
				this.attachmentPoint = AttachmentPointFlags.None;
			}
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x000DE06A File Offset: 0x000DC46A
		private void OnDestroy()
		{
			if (this.attachmentHand != null)
			{
				this.attachmentHand.notifyPointBehaviourDeleted(this);
			}
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x000DE089 File Offset: 0x000DC489
		public static implicit operator AttachmentPointFlags(AttachmentPointBehaviour p)
		{
			if (p == null)
			{
				return AttachmentPointFlags.None;
			}
			return p.attachmentPoint;
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x000DE0A0 File Offset: 0x000DC4A0
		public void SetTransformUsingHand(Hand hand)
		{
			if (hand == null)
			{
				return;
			}
			Vector3 zero = Vector3.zero;
			Quaternion identity = Quaternion.identity;
			AttachmentPointBehaviour.GetLeapHandPointData(hand, this.attachmentPoint, out zero, out identity);
			base.transform.position = zero;
			base.transform.rotation = identity;
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x000DE0E8 File Offset: 0x000DC4E8
		public static void GetLeapHandPointData(Hand hand, AttachmentPointFlags singlePoint, out Vector3 position, out Quaternion rotation)
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
			if (singlePoint != AttachmentPointFlags.None && !singlePoint.IsSinglePoint())
			{
				Debug.LogError("Cannot get attachment point data for an AttachmentPointFlags argument consisting of more than one set flag.");
				return;
			}
			switch (singlePoint)
			{
			case AttachmentPointFlags.None:
				return;
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
																							position = hand.Fingers[4].bones[3].NextJoint.ToVector3();
																							rotation = hand.Fingers[4].bones[3].Rotation.ToQuaternion();
																						}
																					}
																					else
																					{
																						position = hand.Fingers[4].bones[2].NextJoint.ToVector3();
																						rotation = hand.Fingers[4].bones[3].Rotation.ToQuaternion();
																					}
																				}
																				else
																				{
																					position = hand.Fingers[4].bones[1].NextJoint.ToVector3();
																					rotation = hand.Fingers[4].bones[2].Rotation.ToQuaternion();
																				}
																			}
																			else
																			{
																				position = hand.Fingers[4].bones[0].NextJoint.ToVector3();
																				rotation = hand.Fingers[4].bones[1].Rotation.ToQuaternion();
																			}
																		}
																		else
																		{
																			position = hand.Fingers[3].bones[3].NextJoint.ToVector3();
																			rotation = hand.Fingers[3].bones[3].Rotation.ToQuaternion();
																		}
																	}
																	else
																	{
																		position = hand.Fingers[3].bones[2].NextJoint.ToVector3();
																		rotation = hand.Fingers[3].bones[3].Rotation.ToQuaternion();
																	}
																}
																else
																{
																	position = hand.Fingers[3].bones[1].NextJoint.ToVector3();
																	rotation = hand.Fingers[3].bones[2].Rotation.ToQuaternion();
																}
															}
															else
															{
																position = hand.Fingers[3].bones[0].NextJoint.ToVector3();
																rotation = hand.Fingers[3].bones[1].Rotation.ToQuaternion();
															}
														}
														else
														{
															position = hand.Fingers[2].bones[3].NextJoint.ToVector3();
															rotation = hand.Fingers[2].bones[3].Rotation.ToQuaternion();
														}
													}
													else
													{
														position = hand.Fingers[2].bones[2].NextJoint.ToVector3();
														rotation = hand.Fingers[2].bones[3].Rotation.ToQuaternion();
													}
												}
												else
												{
													position = hand.Fingers[2].bones[1].NextJoint.ToVector3();
													rotation = hand.Fingers[2].bones[2].Rotation.ToQuaternion();
												}
											}
											else
											{
												position = hand.Fingers[2].bones[0].NextJoint.ToVector3();
												rotation = hand.Fingers[2].bones[1].Rotation.ToQuaternion();
											}
										}
										else
										{
											position = hand.Fingers[1].bones[3].NextJoint.ToVector3();
											rotation = hand.Fingers[1].bones[3].Rotation.ToQuaternion();
										}
									}
									else
									{
										position = hand.Fingers[1].bones[2].NextJoint.ToVector3();
										rotation = hand.Fingers[1].bones[3].Rotation.ToQuaternion();
									}
								}
								else
								{
									position = hand.Fingers[1].bones[1].NextJoint.ToVector3();
									rotation = hand.Fingers[1].bones[2].Rotation.ToQuaternion();
								}
							}
							else
							{
								position = hand.Fingers[1].bones[0].NextJoint.ToVector3();
								rotation = hand.Fingers[1].bones[1].Rotation.ToQuaternion();
							}
						}
						else
						{
							position = hand.Fingers[0].bones[3].NextJoint.ToVector3();
							rotation = hand.Fingers[0].bones[3].Rotation.ToQuaternion();
						}
					}
					else
					{
						position = hand.Fingers[0].bones[2].NextJoint.ToVector3();
						rotation = hand.Fingers[0].bones[3].Rotation.ToQuaternion();
					}
				}
				else
				{
					position = hand.Fingers[0].bones[1].NextJoint.ToVector3();
					rotation = hand.Fingers[0].bones[2].Rotation.ToQuaternion();
				}
				break;
			case AttachmentPointFlags.Wrist:
				position = hand.WristPosition.ToVector3();
				rotation = hand.Arm.Basis.rotation.ToQuaternion();
				break;
			case AttachmentPointFlags.Palm:
				position = hand.PalmPosition.ToVector3();
				rotation = hand.Basis.rotation.ToQuaternion();
				break;
			}
		}

		// Token: 0x04002183 RID: 8579
		[Tooltip("The AttachmentHand associated with this AttachmentPointBehaviour. AttachmentPointBehaviours should be beneath their AttachmentHand object in the hierarchy.")]
		[Disable]
		public AttachmentHand attachmentHand;

		// Token: 0x04002184 RID: 8580
		[Tooltip("To change which attachment points are available on an AttachmentHand, refer to the inspector for the parent AttachmentHands object.")]
		[Disable]
		public AttachmentPointFlags attachmentPoint;
	}
}
