using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Tiled2Unity.CustomTiledImporter]
public class CustomImporter_ssObject : Tiled2Unity.ICustomTiledImporter
{
    const string OBJ_TYPE = "SsObject";
    const string NEW_PREF = "_new";
    static Dictionary<string, int> names = new Dictionary<string, int>();
    static Transform objects = GameObject.Find("Objects").transform;
    static Transform originalParent = null;

    public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
    {
        if (originalParent == null)
            originalParent = gameObject.transform.parent;
        InstantiateTileObject(gameObject, props);
    }

    UnityEngine.Object InstantiateTileObject(GameObject gameObject, IDictionary<string, string> props)
    {
        GameObject newObj = null;

        string name = OBJ_TYPE;
        if (props.ContainsKey("name"))
            name = props["name"];

        UnityEngine.Object resource = Resources.Load(name, typeof(GameObject));
        if (resource == null)
            resource = Resources.Load(OBJ_TYPE, typeof(GameObject));

        newObj = PrefabUtility.InstantiatePrefab(resource) as GameObject;
        newObj.name = SerialName(name ?? OBJ_TYPE) + NEW_PREF;
        //newObj.tag = OBJ_TYPE;
        InheritProperties(newObj, gameObject);

        return newObj;
    }

    void InheritProperties(GameObject newObj, GameObject gameObject)
    {
        newObj.transform.parent = objects.transform;
        newObj.transform.position = gameObject.transform.position;
        BoxCollider2D newCollider = newObj.GetComponent<BoxCollider2D>();
        if (newCollider != null)
        {
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            newCollider.offset = collider.offset;
            newCollider.size = collider.size;
        }
    }

    string SerialName(string name)
    {
        if (!names.ContainsKey(name))
            names[name] = 1;
        else
            names[name]++;
        return name + names[name];
    }

    void RotateTiles()
    {
        for (int i = objects.childCount - 1; i >= 0; i--)
        {
            GameObject aChild = objects.GetChild(i).gameObject;
            if (aChild.name.EndsWith(NEW_PREF))
                aChild.name = aChild.name.Remove(aChild.name.Length - NEW_PREF.Length);
            else
                GameObject.DestroyImmediate(aChild);
        }
    }

    public void CustomizePrefab(GameObject prefab)
    {
        RotateTiles();
        if (originalParent != null)
            GameObject.DestroyImmediate(originalParent.gameObject);
    }
}