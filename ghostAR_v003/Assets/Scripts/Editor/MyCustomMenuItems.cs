using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyCustomMenuItems : MonoBehaviour {

    [MenuItem("GameObject/Custom Items/World Space Button", false, 10)]
    static void CreateWorldSpaceButtonGameObject (MenuCommand menuCommand)
    {
        GameObject go = new GameObject("World Space Button");
        go.AddComponent<WorldSpaceButton>();

        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}
