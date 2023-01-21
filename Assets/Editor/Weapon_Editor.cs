using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Weapon))]
public class Weapon_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields
 
        Weapon weaponScript = (Weapon)target;
 
        // draw checkbox for the bool
        if (weaponScript.isFiringProjectiles) // if bool is true, show other fields
        {
            weaponScript.projectile = EditorGUILayout.ObjectField("Projectile ", weaponScript.projectile, typeof(GameObject), true) as GameObject;
            //script.iField = EditorGUILayout.ObjectField("I Field", script.iField, typeof(InputField), true) as InputField;
            //script.Template = EditorGUILayout.ObjectField("Template", script.Template, typeof(GameObject), true) as GameObject;
        }
    }
}
#endif