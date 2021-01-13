using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class add_dynamic_bone : MonoBehaviour
{
    [Tooltip(
        "Lock your inspector-panel, select the bones you want to add the script to then drag and drop them onto this text.")]
    public GameObject[] m_bonesToAddDynamicScriptTo;

    [Tooltip("Internal physics simulation rate.")]
    public float m_UpdateRate = 60.0f;

    public DynamicBone.UpdateMode m_UpdateMode = DynamicBone.UpdateMode.AnimatePhysics;

    [Tooltip("How much the bones slowed down.")]
    [Range(0, 1)]
    public float m_Damping = 0.1f;

    public AnimationCurve m_DampingDistrib = null;

    [Tooltip("How much the force applied to return each bone to original orientation.")]
    [Range(0, 1)]
    public float m_Elasticity = 0.03f;

    public AnimationCurve m_ElasticityDistrib = null;

    [Tooltip("How much bone's original orientation are preserved.")]
    [Range(0, 1)]
    public float m_Stiffness = 0;

    public AnimationCurve m_StiffnessDistrib = null;

    [Tooltip("How much character's position change is ignored in physics simulation.")]
    [Range(0, 1)]
    public float m_Inert = 0.75f;

    public AnimationCurve m_InertDistrib = null;

    [Tooltip("How much the bones slowed down when collide.")]
    public float m_Friction = 0;

    public AnimationCurve m_FrictionDistrib = null;

    [Tooltip("Each bone can be a sphere to collide with colliders. Radius describe sphere's size.")]
    public float m_Radius = 0.01f;

    public AnimationCurve m_RadiusDistrib = null;

    [Tooltip("If End Length is not zero, an extra bone is generated at the end of transform hierarchy.")]
    public float m_EndLength = 0;

    [Tooltip("If End Offset is not zero, an extra bone is generated at the end of transform hierarchy.")]
    public Vector3 m_EndOffset = Vector3.zero;

    [Tooltip("The force apply to bones. Partial force apply to character's initial pose is cancelled out.")]
    public Vector3 m_Gravity = new Vector3(0, -0.002f, 0);

    [Tooltip("The force apply to bones.")] public Vector3 m_Force = Vector3.zero;

    [Tooltip("Collider objects interact with the bones.")]
    public List<DynamicBoneColliderBase> m_Colliders = null;

    [Tooltip("Bones exclude from physics simulation.")]
    public List<Transform> m_Exclusions = null;

    [Tooltip("Constrain bones to move on specified plane.")]
    public DynamicBone.FreezeAxis m_FreezeAxis = DynamicBone.FreezeAxis.None;

    [Tooltip("Disable physics simulation automatically if character is far from camera or player.")]
    public bool m_DistantDisable = false;

    public Transform m_ReferenceObject = null;
    public float m_DistanceToObject = 20;

    [Space(10)] public bool CheckWhenReady;

    [ContextMenu("Click here to add the script(s) to the bone(s)")]

    // Start is called before the first frame update
    private void AddScript()
    {
        if (CheckWhenReady)
        {
            if (m_bonesToAddDynamicScriptTo.Length == 0)
                Debug.LogError("You didn't populate the array of bones!! (Add the gameobjects to the value called Bones To Add Dynamic Script To)");
            else
                for (int i = 0; i < m_bonesToAddDynamicScriptTo.Length; i++)
                {
                    DynamicBone script = m_bonesToAddDynamicScriptTo[i].AddComponent<DynamicBone>();
                    script.m_Root = m_bonesToAddDynamicScriptTo[i].transform;
                    script.m_UpdateRate = m_UpdateRate;
                    script.m_UpdateMode = m_UpdateMode;
                    script.m_Damping = m_Damping;
                    script.m_DampingDistrib = m_DampingDistrib;
                    script.m_Elasticity = m_Elasticity;
                    script.m_ElasticityDistrib = m_ElasticityDistrib;
                    script.m_Stiffness = m_Stiffness;
                    script.m_StiffnessDistrib = m_StiffnessDistrib;
                    script.m_Inert = m_Inert;
                    script.m_InertDistrib = m_InertDistrib;
                    script.m_Friction = m_Friction;
                    script.m_FrictionDistrib = m_FrictionDistrib;
                    script.m_Radius = m_Radius;
                    script.m_RadiusDistrib = m_RadiusDistrib;
                    script.m_EndLength = m_EndLength;
                    script.m_EndOffset = m_EndOffset;
                    script.m_Gravity = m_Gravity;
                    script.m_Force = m_Force;
                    script.m_Colliders = m_Colliders;
                    script.m_Exclusions = m_Exclusions;
                    script.m_FreezeAxis = m_FreezeAxis;
                    script.m_DistantDisable = m_DistantDisable;
                    script.m_ReferenceObject = m_ReferenceObject;
                    script.m_DistanceToObject = m_DistanceToObject;
                }
        }
        else
            Debug.LogError("Are you sure you are ready to add the scripts? (Checkmark the box that says Check When Ready at the bottom of the script)");
    }

    [ContextMenu("Click here to remove the script(s) from the bone(s)")]
    private void RemoveScripts()
    {
        if (m_bonesToAddDynamicScriptTo.Length == 0)
            Debug.LogError("You didn't populate the array of bones!! (Add the gameobjects to the value called Bones To Add Dynamic Script To)");
        else
            for (int i = 0; i < m_bonesToAddDynamicScriptTo.Length; i++)
            {
                DestroyImmediate(m_bonesToAddDynamicScriptTo[i].GetComponent<DynamicBone>());
            }
    }
}
