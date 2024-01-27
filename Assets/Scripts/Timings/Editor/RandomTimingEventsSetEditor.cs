

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomTimingEventsSet))]
public class RandomTimingEventsSetEditor : Editor
{
    private RandomTimingEventsSet trigger;

    void OnEnable()
    {
        trigger = target as RandomTimingEventsSet;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("From value:", trigger.from.ToString());
        EditorGUILayout.LabelField("To value:", trigger.to.ToString());
        EditorGUILayout.MinMaxSlider(new GUIContent("Set timing range"), ref trigger.from, ref trigger.to, .001f, 50f);
        EditorGUILayout.LabelField("Offset from value:", trigger.offsetFrom.ToString());
        EditorGUILayout.LabelField("Offset to value:", trigger.offsetTo.ToString());
        EditorGUILayout.MinMaxSlider(new GUIContent("Set offset range"), ref trigger.offsetFrom, ref trigger.offsetTo, .001f, 35f);
        DrawDefaultInspector();
    }
}
