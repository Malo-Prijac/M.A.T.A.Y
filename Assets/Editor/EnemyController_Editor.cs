using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyController))]
public class EnemyController_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields
 
        EnemyController enemyControllerScript = (EnemyController)target;
 
        // draw checkbox for the bool
        if (enemyControllerScript.hasDifferentSlotForWeapon) // if bool is true, show other fields
        {
            enemyControllerScript.weaponSlotAttack = EditorGUILayout.ObjectField("Weapon Slot Attack ", enemyControllerScript.weaponSlotAttack, typeof(Transform), true) as Transform;
            enemyControllerScript.weaponSlotMovement = EditorGUILayout.ObjectField(">eapon Slot Movement ", enemyControllerScript.weaponSlotMovement, typeof(Transform), true) as Transform;
            //script.iField = EditorGUILayout.ObjectField("I Field", script.iField, typeof(InputField), true) as InputField;
            //script.Template = EditorGUILayout.ObjectField("Template", script.Template, typeof(GameObject), true) as GameObject;
        }
    }
}
#endif
