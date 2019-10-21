﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.﻿

#pragma warning disable 0649 // For serialized fields
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class HandMeshSurface : FingerSurface
{
    [System.Serializable]
    private struct JointMap
    {
        [HideInInspector]
        public Transform Transform;
        public TrackedHandJoint Joint;
        public Vector3 WristPosOffset;
        public Vector3 WristRotOffset;
        public string Name;
    }

    [Header("Hand Objects")]
    [SerializeField]
    private GameObject leftHandObject;
    [SerializeField]
    private GameObject rightHandObject;
    [SerializeField]
    private Transform leftHandRoot;
    [SerializeField]
    private Transform rightHandRoot;

    [SerializeField]
    private JointMap[] leftJointMaps;
    [SerializeField]
    private JointMap[] rightJointMaps;
    [SerializeField]
    private Vector3 leftJointRotOffset;
    [SerializeField]
    private Vector3 rightJointRotOffset;

    protected override void Awake()
    {
        base.Awake();

        Transform[] leftHandTransforms = leftHandObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < leftJointMaps.Length; i++)
        {
            JointMap jm = leftJointMaps[i];
            foreach (Transform tr in leftHandTransforms)
            {
                if (tr.name == jm.Name)
                {
                    jm.Transform = tr;
                    break;
                }
            }
            leftJointMaps[i] = jm;
        }

        Transform[] rightHandTransforms = rightHandObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < rightJointMaps.Length; i++)
        {
            JointMap jm = rightJointMaps[i];
            foreach (Transform tr in rightHandTransforms)
            {
                if (tr.name == jm.Name)
                {
                    jm.Transform = tr;
                    break;
                }
            }
            rightJointMaps[i] = jm;
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        
        /*IMixedRealityInputSystem inputSystem = MixedRealityToolkit.Instance.GetService<IMixedRealityInputSystem>();
        IMixedRealityHandJointService handJointService = (inputSystem as MixedRealityInputSystem).GetDataProvider<IMixedRealityHandJointService>();

        if (handJointService.IsHandTracked(Handedness.Right))
        {
            rightHandObject.SetActive(true);

            Transform wristTransform = handJointService.RequestJointTransform(TrackedHandJoint.Wrist, Handedness.Right);

            foreach (JointMap jm in rightJointMaps)
            {
                if (jm.Joint == TrackedHandJoint.None)
                {
                    // Move it relative to the wrist
                    jm.Transform.position = wristTransform.TransformPoint(jm.WristPosOffset);
                    jm.Transform.rotation = wristTransform.rotation;
                    jm.Transform.Rotate(jm.WristRotOffset, Space.Self);
                }
                else
                {
                    Transform jointTransform = handJointService.RequestJointTransform(jm.Joint, Handedness.Right);
                    //jm.Transform.position = jointTransform.position;
                    jm.Transform.rotation = jointTransform.rotation * Quaternion.Euler(rightJointRotOffset);
                }
            }
        }
        else
        {
            rightHandObject.SetActive(false);
        }

        if (handJointService.IsHandTracked(Handedness.Left))
        {
            leftHandObject.SetActive(true);

            Transform wristTransform = handJointService.RequestJointTransform(TrackedHandJoint.Wrist, Handedness.Left);

            foreach (JointMap jm in leftJointMaps)
            {
                if (jm.Joint == TrackedHandJoint.None)
                {
                    // Move it relative to the wrist
                    jm.Transform.position = wristTransform.TransformPoint(jm.WristPosOffset);
                    jm.Transform.rotation = wristTransform.rotation;
                    jm.Transform.Rotate(jm.WristRotOffset, Space.Self);
                }
                else
                {
                    Transform jointTransform = handJointService.RequestJointTransform(jm.Joint, Handedness.Left);
                    //jm.Transform.position = jointTransform.position;
                    jm.Transform.rotation = jointTransform.rotation * Quaternion.Euler(leftJointRotOffset);
                }
            }
        }
        else
        {
            leftHandObject.SetActive(false);
        }*/
    }


#if UNITY_EDITOR
    [MenuItem("Surfaces/Generate Joint Map")]
    public static void PopuplateJointMap()
    {
        HandMeshSurface hms = GameObject.FindObjectOfType<HandMeshSurface>();

        if (hms == null)
            return;

        List<JointMap> jointMaps = new System.Collections.Generic.List<JointMap>(hms.leftJointMaps);
        for (int i = jointMaps.Count - 1; i>=0; i--)
        {
            if (string.IsNullOrEmpty(jointMaps[i].Name))
            {
                jointMaps.RemoveAt(i);
            }
        }
        hms.leftJointMaps = jointMaps.ToArray();

        jointMaps.Clear();
        jointMaps.AddRange(hms.rightJointMaps);
        for (int i = jointMaps.Count - 1; i >= 0; i--)
        {
            if (string.IsNullOrEmpty(jointMaps[i].Name))
            {
                jointMaps.RemoveAt(i);
            }
        }
        hms.rightJointMaps = jointMaps.ToArray();
    }
#endif
}
