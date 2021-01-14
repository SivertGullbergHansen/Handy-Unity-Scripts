using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRM;
using Object = UnityEngine.Object;

// Script written by Sivert Gullberg Hansen
// Version 1.2 [14-Jan-21]
// Made for Dynamic Bone Version 1.2.2
// https://github.com/SivertGullbergHansen
// https://sivert.xyz
public class add_dynamic_bone : EditorWindow
{
    private bool ready = false;

    [MenuItem("Tools/Sivert/Dynamic Bone Adder")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        add_dynamic_bone window = (add_dynamic_bone)EditorWindow.GetWindow(typeof(add_dynamic_bone));
        window.titleContent = new GUIContent("Dynamic Bone Adder");
        window.Show();
    }

    private Vector2 scrollPosition = Vector2.zero;

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);
        GUIStyle style = new GUIStyle(EditorStyles.miniButton);
        style.fixedHeight = 40;
        style.hover.textColor = new Color(1, .25f, .25f);
        style.normal.textColor = Color.red;

        GUILayout.Space(6);

        // Script Settings
        GUILayout.Label("Dynamic Bone Adder Settings", EditorStyles.boldLabel);

        // Gameobject Array
        ScriptableObject scriptableObj = this;
        SerializedObject serialObj = new SerializedObject(scriptableObj);
        SerializedProperty serialProp = serialObj.FindProperty("m_bonesToAddDynamicScriptTo");
        EditorGUILayout.PropertyField(serialProp, true);
        serialObj.ApplyModifiedProperties();

        GUILayout.Space(12);

        // Dynamic Bone Settings
        GUILayout.Label("Dynamic Bone Settings", EditorStyles.boldLabel);
        m_UpdateRate = EditorGUILayout.FloatField(new GUIContent("Update Rate", "Internal physics simulation rate."), m_UpdateRate);
        m_UpdateMode = (DynamicBone.UpdateMode)EditorGUILayout.EnumFlagsField("Update Mode", m_UpdateMode);
        m_Damping = EditorGUILayout.FloatField(new GUIContent("Damping", "How much the bones slowed down."), m_Damping);
        m_DampingDistrib = EditorGUILayout.CurveField("Damping Distribution", m_DampingDistrib);
        m_Elasticity = EditorGUILayout.FloatField(new GUIContent("Elasticity", "How much the force applied to return each bone to original orientation."), m_Elasticity);
        m_ElasticityDistrib = EditorGUILayout.CurveField("Elasticity Distribution", m_ElasticityDistrib);
        m_Stiffness = EditorGUILayout.FloatField(new GUIContent("Stiffness", "How much bone's original orientation are preserved."), m_Stiffness);
        m_StiffnessDistrib = EditorGUILayout.CurveField("Stiffness Distribution", m_StiffnessDistrib);
        m_Inert = EditorGUILayout.FloatField(new GUIContent("Inert", "How much character's position change is ignored in physics simulation."), m_Inert);
        m_InertDistrib = EditorGUILayout.CurveField("Inert Distribution", m_InertDistrib);
        m_Friction = EditorGUILayout.FloatField(new GUIContent("Friction", "How much the bones slowed down when collide."), m_Friction);
        m_FrictionDistrib = EditorGUILayout.CurveField("Friction Distribution", m_FrictionDistrib);
        m_Radius = EditorGUILayout.FloatField(new GUIContent("Radius", "Each bone can be a sphere to collide with colliders. Radius describe sphere's size."), m_Radius);
        m_RadiusDistrib = EditorGUILayout.CurveField("Radius Distribution", m_RadiusDistrib);
        m_EndLength = EditorGUILayout.FloatField(new GUIContent("End Length", "If End Length is not zero, an extra bone is generated at the end of transform hierarchy."), m_EndLength);
        m_EndOffset = EditorGUILayout.Vector3Field(new GUIContent("End Offset", "The force apply to bones. Partial force apply to character's initial pose is cancelled out."), m_EndOffset);
        m_Gravity = EditorGUILayout.Vector3Field(new GUIContent("Gravity", "The force apply to bones. Partial force apply to character's initial pose is cancelled out."), m_Gravity);
        m_Force = EditorGUILayout.Vector3Field(new GUIContent("Force", "The force apply to bones."), m_Force);

        // Collider Array
        ScriptableObject scriptableObj2 = this;
        SerializedObject serialObj2 = new SerializedObject(scriptableObj2);
        SerializedProperty serialProp2 = serialObj2.FindProperty("m_Colliders");
        EditorGUILayout.PropertyField(serialProp2, true);
        serialObj2.ApplyModifiedProperties();

        // Exclusion Array
        ScriptableObject scriptableObj3 = this;
        SerializedObject serialObj3 = new SerializedObject(scriptableObj3);
        SerializedProperty serialProp3 = serialObj3.FindProperty("m_Exclusions");
        EditorGUILayout.PropertyField(serialProp3, true);
        serialObj3.ApplyModifiedProperties();

        m_FreezeAxis = (DynamicBone.FreezeAxis)EditorGUILayout.EnumFlagsField(new GUIContent("Freeze Axis", "Constrain bones to move on specified plane."), m_FreezeAxis);
        m_DistantDisable = EditorGUILayout.Toggle(new GUIContent("Distant Disable", "Disable physics simulation automatically if character is far from camera or player."), m_DistantDisable);
        m_ReferenceObject = EditorGUILayout.ObjectField(new GUIContent("Reference Object", ""), m_ReferenceObject, typeof(Transform), true) as Transform;

        GUILayout.Space(12);

        // Check if the user has added any bones to the list
        if (m_bonesToAddDynamicScriptTo != null)
        {
            if (m_bonesToAddDynamicScriptTo.Length == 0)
                ready = false;
            else
                ready = true;
        }

        #region Buttons

        EditorGUI.BeginDisabledGroup(ready == false);
        // Buttons

        if (GUILayout.Button(new GUIContent("Add script to bone(s)", "Adds the dynamic bone script to the selected bones and gives the dynamic bone script the same values as this script. If dynamic bone script already exists on target then update values of that script to be equal to our values."), GUILayout.Height(40)))
        {
            AddScript();
        }

        GUILayout.Space(6);

        if (GUILayout.Button(new GUIContent("Update bone(s)", "Changes the values of the dynamic bone script on every selected bone to be equal to this scripts settings."), GUILayout.Height(40)))
        {
            UpdateScripts();
        }

        GUILayout.Space(6);

        if (GUILayout.Button(new GUIContent("Copy settings from bone(s)", "Copies the settings from the first bone added to the list and changes this scripts current settings to be equal."), GUILayout.Height(40)))
        {
            CopySettings();
        }

        GUILayout.Space(40);

        if (GUILayout.Button(new GUIContent("Remove script from bone(s)", "Removes the dynamic bone script from the selected bones."), style, GUILayout.Height(40)))
        {
            RemoveScripts();
        }
        EditorGUI.EndDisabledGroup();

        #endregion Buttons

        GUILayout.EndScrollView();
    }

    #region variables

    [Tooltip("Select the bones you want to add the script to then drag and drop them onto this text.")]
    public GameObject[] m_bonesToAddDynamicScriptTo;

    [Tooltip("Internal physics simulation rate.")]
    public float m_UpdateRate = 60.0f;

    public DynamicBone.UpdateMode m_UpdateMode = DynamicBone.UpdateMode.Default;

    [Tooltip("How much the bones slowed down.")]
    [Range(0, 1)]
    public float m_Damping = 0.15f;

    public AnimationCurve m_DampingDistrib = AnimationCurve.Constant(0, 1, 1);

    [Tooltip("How much the force applied to return each bone to original orientation.")]
    [Range(0, 1)]
    public float m_Elasticity = 0.03f;

    public AnimationCurve m_ElasticityDistrib = AnimationCurve.Constant(0, 1, 1);

    [Tooltip("How much bone's original orientation are preserved.")]
    [Range(0, 1)]
    public float m_Stiffness = 0.3f;

    public AnimationCurve m_StiffnessDistrib = AnimationCurve.Constant(0, 1, 1);

    [Tooltip("How much character's position change is ignored in physics simulation.")]
    [Range(0, 1)]
    public float m_Inert = 0;

    public AnimationCurve m_InertDistrib = AnimationCurve.Constant(0, 1, 1);

    [Tooltip("How much the bones slowed down when collide.")]
    public float m_Friction = 0;

    public AnimationCurve m_FrictionDistrib = AnimationCurve.Constant(0, 1, 1);

    [Tooltip("Each bone can be a sphere to collide with colliders. Radius describe sphere's size.")]
    public float m_Radius = 0.0001f;

    public AnimationCurve m_RadiusDistrib = AnimationCurve.Constant(0, 1, 1);

    [Tooltip("If End Length is not zero, an extra bone is generated at the end of transform hierarchy.")]
    public float m_EndLength = 0;

    [Tooltip("If End Offset is not zero, an extra bone is generated at the end of transform hierarchy.")]
    public Vector3 m_EndOffset = Vector3.zero;

    [Tooltip("The force apply to bones. Partial force apply to character's initial pose is cancelled out.")]
    public Vector3 m_Gravity = Vector3.zero;

    [Tooltip("The force apply to bones.")]
    public Vector3 m_Force = Vector3.zero;

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

    #endregion variables

    #region Methods

    private void AddScript()
    {
        if (m_bonesToAddDynamicScriptTo.Length == 0)
            Debug.LogError("You didn't populate the array of bones!! (Add the gameobjects to the value called Bones To Add Dynamic Script To)");
        else
            for (int i = 0; i < m_bonesToAddDynamicScriptTo.Length; i++)
            {
                if (!m_bonesToAddDynamicScriptTo[i].GetComponent<DynamicBone>())
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
                else
                {
                    UpdateScripts();
                }
            }
    }

    private void UpdateScripts()
    {
        if (m_bonesToAddDynamicScriptTo.Length == 0)
            Debug.LogError("You didn't populate the array of bones!! (Add the gameobjects to the value called Bones To Add Dynamic Script To)");
        else
        {
            for (int i = 0; i < m_bonesToAddDynamicScriptTo.Length; i++)
            {
                DynamicBone script = m_bonesToAddDynamicScriptTo[i].GetComponent<DynamicBone>();
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
    }

    private void CopySettings()
    {
        if (m_bonesToAddDynamicScriptTo.Length == 0)
            Debug.LogError("You didn't populate the array of bones!! (Add the gameobjects to the value called Bones To Add Dynamic Script To)");
        else
        {
            DynamicBone script = m_bonesToAddDynamicScriptTo[0].GetComponent<DynamicBone>();
            m_UpdateRate = script.m_UpdateRate;
            m_UpdateMode = script.m_UpdateMode;
            m_Damping = script.m_Damping;
            m_DampingDistrib = script.m_DampingDistrib;
            m_Elasticity = script.m_Elasticity;
            m_ElasticityDistrib = script.m_ElasticityDistrib;
            m_Stiffness = script.m_Stiffness;
            m_StiffnessDistrib = script.m_StiffnessDistrib;
            m_Inert = script.m_Inert;
            m_InertDistrib = script.m_InertDistrib;
            m_Friction = script.m_Friction;
            m_FrictionDistrib = script.m_FrictionDistrib;
            m_Radius = script.m_Radius;
            m_RadiusDistrib = script.m_RadiusDistrib;
            m_EndLength = script.m_EndLength;
            m_EndOffset = script.m_EndOffset;
            m_Gravity = script.m_Gravity;
            m_Force = script.m_Force;
            m_Colliders = script.m_Colliders;
            m_Exclusions = script.m_Exclusions;
            m_FreezeAxis = script.m_FreezeAxis;
            m_DistantDisable = script.m_DistantDisable;
            m_ReferenceObject = script.m_ReferenceObject;
            m_DistanceToObject = script.m_DistanceToObject;
        }
    }

    private void RemoveScripts()
    {
        if (m_bonesToAddDynamicScriptTo.Length == 0)
            Debug.LogError("You didn't populate the array of bones!! (Add the gameobjects to the value called Bones To Add Dynamic Script To)");
        else
            for (int i = 0; i < m_bonesToAddDynamicScriptTo.Length; i++)
            {
                if (m_bonesToAddDynamicScriptTo[i].GetComponent<DynamicBone>())
                    DestroyImmediate(m_bonesToAddDynamicScriptTo[i].GetComponent<DynamicBone>());
            }
    }

    #endregion Methods
}
