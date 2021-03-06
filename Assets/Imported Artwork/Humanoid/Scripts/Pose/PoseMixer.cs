﻿using Passer.Humanoid.Tracking;
using System.Collections.Generic;
using UnityEngine;

namespace Passer.Humanoid {

    [System.Serializable]
    public class PoseMixer {
        public List<MixedPose> mixedPoses = new List<MixedPose>();
        public int currentPoseIx;
        public Pose detectedPose;

        // New version without currentPose
        public void SetPoseValue(int newCurrentPoseIx, float value = 1) {
            if (currentPoseIx != newCurrentPoseIx) {
                currentPoseIx = newCurrentPoseIx;

                for (int i = 0; i < mixedPoses.Count; i++) {
                    MixedPose mixedPose = mixedPoses[i];
                    if (mixedPose.value > 0 && i != currentPoseIx)
                        mixedPose.previousValue = mixedPose.value;
                    else
                        mixedPose.previousValue = 0;
                }
            }

            MixedPose currentPose = mixedPoses[currentPoseIx];
            if (currentPose != null) {
                currentPose.value = value;

                float invValue = 1 - value;
                foreach (MixedPose mixedPose in mixedPoses) {
                    if (mixedPose.previousValue != 0)
                        mixedPose.value = invValue * mixedPose.previousValue;
                }
            }

        }

        public void SetPoseValue(Pose newCurrentPose, float value = 1) {
            int poseIx = GetPoseIx(newCurrentPose);
            SetPoseValue(poseIx, value);
        }

        public virtual MixedPose Add() {
            MixedPose newMixedPose = new MixedPose() {
                pose = ScriptableObject.CreateInstance<Pose>()
            };
            mixedPoses.Add(newMixedPose);
            return newMixedPose;
        }

        public MixedPose Add(Pose _pose) {
            MixedPose foundMixedPose = mixedPoses.Find(mixedPose => mixedPose.pose == _pose);
            if (foundMixedPose != null)
                return foundMixedPose;
            MixedPose newMixedPose = new MixedPose() {
                pose = _pose
            };
            mixedPoses.Add(newMixedPose);
            return newMixedPose;
        }

        public int GetPoseIx(Pose _pose) {
            int ix = mixedPoses.FindIndex(mixedPose => mixedPose.pose == _pose);
            if (ix != -1)
                return ix;
            MixedPose newMixedPose = new MixedPose() {
                pose = _pose
            };
            mixedPoses.Add(newMixedPose);
            // position is last position;
            return mixedPoses.Count - 1;
        }
        public MixedPose GetEditedPose() {
            foreach (MixedPose mixedPose in mixedPoses) {
                if (mixedPose.pose != null && mixedPose.isEdited)
                    return mixedPose;
            }
            return null;
        }

        public void ShowPose(HumanoidControl humanoid) {
            foreach (MixedPose mixedPose in mixedPoses) {
                if (mixedPose != null && mixedPose.pose != null)
                    mixedPose.pose.Show(humanoid, mixedPose.value);
            }
        }
        public void ShowPose(HumanoidControl humanoid, Side showSide) {
            int bestPose = -1;
            float bestScore = -1;
            ResetAffectedBones(humanoid, showSide);
            int i = 0;
            foreach (MixedPose mixedPose in mixedPoses) {
                if (mixedPose != null && mixedPose.pose != null)
                    mixedPose.pose.ShowAdditive(humanoid, showSide, mixedPose.value);
                mixedPose.UpdateScore(humanoid, showSide);
                if (mixedPose.score > bestScore) {
                    bestPose = i;
                    bestScore = mixedPose.score;
                    detectedPose = mixedPose.pose;
                }
                i++;
            }
            if (bestPose != -1)
                currentPoseIx = bestPose;
        }

        public void ResetAffectedBones(HumanoidControl humanoid, Side showSide) {
            // This will not reset the bones when no mixed pose is active
            // We do want this, because otherwise the hand is not reset....

            //bool anyActive = false;
            //foreach (MixedPose mixedPose in mixedPoses)
            //    if (mixedPose.value > 0)
            //        anyActive = true;

            //if (!anyActive)
            //    return;

            foreach (MixedPose mixedPose in mixedPoses) {
                if (mixedPose != null && mixedPose.pose != null && mixedPose.pose.bonePoses != null) {

                    foreach (BonePose bonePose in mixedPose.pose.bonePoses) {
                        HumanoidTarget.TargetedBone targetedBone = humanoid.GetBone(showSide, bonePose.boneRef.sideBoneId);
                        if (targetedBone == null || targetedBone.target.transform == null)
                            continue;

                        if (bonePose.setTranslation)
                            targetedBone.target.transform.position = targetedBone.TargetBasePosition();
                        if (bonePose.setRotation)
                            targetedBone.target.transform.rotation = targetedBone.TargetBaseRotation();
                        if (bonePose.setRotation)
                            targetedBone.target.transform.localScale = Vector3.one;
                    }

                }
            }
        }

        public void Cleanup() {
            mixedPoses.RemoveAll(mixedPose => mixedPose.pose == null);
        }

        public string[] GetPoseNames() {
            string[] poseNames = new string[mixedPoses.Count];//.Length];
            for (int i = 0; i < poseNames.Length; i++) {
                if (mixedPoses[i].pose == null)
                    poseNames[i] = "";
                else
                    poseNames[i] = mixedPoses[i].pose.name;
            }
            return poseNames;
        }
    }

    [System.Serializable]
    public class MixedPose {
        /// <summary>
        /// The pose itself
        /// </summary>
        public Pose pose;
        /// <summary>
        /// The current value of the pose
        /// </summary>
        public float value;
        /// <summary>
        /// The value of the pose when it became the previous pose
        /// </summary>
        public float previousValue;
        /// <summary>
        /// Pose is in editing mode
        /// </summary>
        public bool isEdited;
        /// <summary> Score indicating how much the current pose matched this pose</summary>
        public float score;

        public void UpdateScore(HumanoidControl humanoid, Side side) {
            if (pose == null)
                return;

            score = pose.GetScore(humanoid, side);
        }
    }
}

