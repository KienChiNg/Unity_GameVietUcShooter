using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(UIEffectComponent))]
public class UIEffectControllerEditor : Editor
{
    public string[] effectOptions = new string[] { "Translate", "Rotate", "Scale" };
    public string[] axisOptions = new string[] { "axisX" , "axisY" , "axisZ" };
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIEffectComponent controller = target as UIEffectComponent;

        List<Effect> lerps = controller.effect.effects;

        for (int i = 0; i < lerps.Count; i ++)
        {
            lerps[i].type = (EffectType)EditorGUILayout.Popup("Effect Type", (int)lerps[i].type, effectOptions);
            switch(lerps[i].type)
            {
                case EffectType.Translate:
                    lerps[i] = Vector3LerpEditor(lerps[i]);
                    break;

                case EffectType.RotateAroundAxis:
                    lerps[i] = RotateAroundAxis(lerps[i]);
                    break;

                case EffectType.Scale:
                    lerps[i] = Vector3LerpEditor(lerps[i]);
                    break;
            }

            if(GUILayout.Button("Remove effect", new GUILayoutOption[] { GUILayout.Width(100) } ))
            {
                lerps.Remove(lerps[i]);
            }

            EditorGUILayout.Separator();
        }

        if(GUILayout.Button("Add effect"))
        {
            lerps.Add(new Effect());
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(controller);
            EditorSceneManager.MarkSceneDirty(controller.gameObject.scene);
        }
    }

    public Effect Vector3LerpEditor(Effect effect)
    {

        effect.v3Start = EditorGUILayout.Vector3Field("Begin", effect.v3Start);
        effect.v3End = EditorGUILayout.Vector3Field("Destination", effect.v3End);
        effect.duration = EditorGUILayout.FloatField("Duration", effect.duration);
        effect.delay = EditorGUILayout.FloatField("Delay", effect.delay);
        return effect;
    }

    public Effect RotateAroundAxis(Effect effect)
    {
        effect.rotateAxis = (Axis)EditorGUILayout.Popup("Rotate Axis", (int)effect.rotateAxis, axisOptions);
        effect.rotateTimes = EditorGUILayout.IntField("Rotate times", effect.rotateTimes);
        effect.isClockwise = EditorGUILayout.Toggle("Clockwise", effect.isClockwise);
        effect.duration = EditorGUILayout.FloatField("Duration", effect.duration);
        effect.delay = EditorGUILayout.FloatField("Delay", effect.delay);
        return effect;
    }

}
