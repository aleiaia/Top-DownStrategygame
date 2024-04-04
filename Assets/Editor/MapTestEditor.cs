using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.GridMap;

[CustomEditor(typeof(MapTest)), CanEditMultipleObjects]
public class MapTestEditor : Editor
{
    MapTest mapTest;
    Editor settingsEditor;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate"))
        {
            mapTest.Generate();
        }

        DrawSettingsEditor(mapTest.GeneratorSettings.heighMapGeneratorSettings, mapTest.OnSettingsUpdated, ref mapTest.GeneratorSettings.heightMapGeneratorSettingsFoldout, ref settingsEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action OnSettingsUpadated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (mapTest.autoUpdateOnSettingsChange)
                        {
                            OnSettingsUpadated?.Invoke();
                        }
                    }
                }
            }
        }
    }
    
    private void OnEnable()
    {
        mapTest = (MapTest)target;
    }
}
