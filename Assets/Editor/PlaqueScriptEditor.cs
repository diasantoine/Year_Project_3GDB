using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(plaqueScript)), CanEditMultipleObjects]
public class PlaqueScriptEditor : Editor
{
    public SerializedProperty
        type_Prop,
        activ_Prop,
        activTime_Prop,
        maxRessourceGot_Prop,
        regenRessource_Prop,
        SP_Prop,
        EmiRD_Prop;


    void OnEnable()
    {
        // Setup the SerializedProperties
        type_Prop = serializedObject.FindProperty("type");
        activTime_Prop = serializedObject.FindProperty("activTime");
        activ_Prop = serializedObject.FindProperty("activ");
        regenRessource_Prop = serializedObject.FindProperty("regenRessource");
        maxRessourceGot_Prop = serializedObject.FindProperty("maxRessourceGot");
        SP_Prop = serializedObject.FindProperty("SP");
        EmiRD_Prop = serializedObject.FindProperty("EmiRD");



    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(type_Prop);

        plaqueScript.Type tp = (plaqueScript.Type)type_Prop.intValue;

        switch (tp)
        {
            case plaqueScript.Type.NORMAL:
                break;
            case plaqueScript.Type.HOT:
                EditorGUILayout.PropertyField(SP_Prop, new GUIContent("SystemPlaque"));
                EditorGUILayout.PropertyField(activ_Prop, new GUIContent("Activ"));
                EditorGUILayout.PropertyField(activTime_Prop, new GUIContent("Activ Time"));
                EditorGUILayout.PropertyField(EmiRD_Prop, new GUIContent("Emission Renderer"));



                break;
            case plaqueScript.Type.COLD:
                EditorGUILayout.PropertyField(SP_Prop, new GUIContent("SystemPlaque"));
                EditorGUILayout.PropertyField(activ_Prop, new GUIContent("Activ"));
                EditorGUILayout.PropertyField(activTime_Prop, new GUIContent("Activ Time"));
                EditorGUILayout.PropertyField(EmiRD_Prop, new GUIContent("Emission Renderer"));

                break;
            case plaqueScript.Type.TOXIC:
                EditorGUILayout.PropertyField(regenRessource_Prop, new GUIContent("Regen Ressource"));
                EditorGUILayout.PropertyField(maxRessourceGot_Prop, new GUIContent("Max Ressource Got"));
                break;
            case plaqueScript.Type.PISTON:
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
