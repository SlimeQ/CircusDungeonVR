

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomTimingEvents))]
public class RandomTimingEventsEditor : Editor
{
    private RandomTimingEvents trigger;

    void OnEnable()
    {
        trigger = target as RandomTimingEvents;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("From value:", trigger.from.ToString());
        EditorGUILayout.LabelField("To value:", trigger.to.ToString());
        EditorGUILayout.MinMaxSlider(new GUIContent("Set timing range"), ref trigger.from, ref trigger.to, .001f, 35f);
        EditorGUILayout.LabelField("Offset from value:", trigger.offsetFrom.ToString());
        EditorGUILayout.LabelField("Offset to value:", trigger.offsetTo.ToString());
        EditorGUILayout.MinMaxSlider(new GUIContent("Set offset range"), ref trigger.offsetFrom, ref trigger.offsetTo, .001f, 35f);
        DrawDefaultInspector();
    }
}
