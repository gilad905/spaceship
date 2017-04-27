using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tiled2Unity;

[CustomTiledImporter]
public class CustomImporter_ssObject : ICustomTiledImporter
{
    const string OBJ_TYPE = "ssObject";
    static Dictionary<string, int> names;
    GameObject objects;

    public CustomImporter_ssObject()
    {
        this.objects = new GameObject("Objects_temp");
        this.objects.transform.position = new Vector3(0, 0, 0);

        GameObject objects = GameObject.Find("Objects");
        if (objects != null)
            GameObject.DestroyImmediate(objects);

        names = new Dictionary<string, int>();
    }

    public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
    {
        InstantiateTile(gameObject, props);
        GameObject.DestroyImmediate(gameObject);
    }

    public void CustomizePrefab(GameObject prefab)
    {
        objects.name = "Objects";

        foreach (TileObject tileObj in prefab.GetComponentsInChildren<TileObject>())
            InstantiateTile(tileObj.gameObject);

        SortChildrenByName(objects);

        Transform toDestroy = prefab.transform.Find("objects");
        if (toDestroy != null)
            GameObject.DestroyImmediate(toDestroy.gameObject);
        toDestroy = prefab.transform.Find("wall_objects");
        if (toDestroy != null)
            GameObject.DestroyImmediate(toDestroy.gameObject);
    }

    private void SortChildrenByName(GameObject parent)
    {
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < parent.transform.childCount; i++)
            children.Add(parent.transform.GetChild(i));
        children.Sort((c1, c2) => c1.name.CompareTo(c2.name));
        for (int i = 0; i < children.Count; i++)
            children[i].SetSiblingIndex(i);
    }

    //private void ClearChildren(GameObject parent)
    //{
    //    var children = new List<GameObject>();
    //    foreach (Transform child in parent.transform)
    //        children.Add(child.gameObject);
    //    children.ForEach(child => GameObject.DestroyImmediate(child));
    //}

    void InstantiateTile(GameObject oldObj)
    {
        InstantiateTile(oldObj, null);
    }

    void InstantiateTile(GameObject oldObj, IDictionary<string, string> props)
    {
        string name = OBJ_TYPE;
        if (props != null && props.ContainsKey("name"))
            name = props["name"];

        UnityEngine.Object resource = Resources.Load(name, typeof(GameObject));
        if (resource == null)
            resource = Resources.Load(OBJ_TYPE, typeof(GameObject));

        GameObject newObj = PrefabUtility.InstantiatePrefab(resource) as GameObject;
        newObj.name = SerialName(name);
        newObj.transform.parent = objects.transform;

        GameObject oldchild = oldObj.transform.GetChild(0).GetChild(0).gameObject;
        float objSize = PassMeshComponentes(newObj, oldchild);
        bool isTrigger = !(props == null || !props.ContainsKey("collide") || props["collide"] == "true");
        SetBoxCollider2D(newObj, oldchild, objSize, isTrigger);
        PassPositionRotation(newObj, oldObj, objSize);
    }

    void SetBoxCollider2D(GameObject newObj, GameObject oldObj, float objSize, bool isTrigger)
    {
        BoxCollider2D newCollider = newObj.GetComponent<BoxCollider2D>();
        newCollider.size = new Vector2(objSize, objSize);
        newCollider.offset = new Vector2(objSize / 2, 0 - (objSize / 2));
        newCollider.isTrigger = isTrigger;
    }

    float PassMeshComponentes(GameObject newObj, GameObject oldObj)
    {
        MeshRenderer oldMR = oldObj.GetComponent<MeshRenderer>();
        MeshRenderer newMR = newObj.GetComponent<MeshRenderer>();
        MeshFilter oldMF = oldObj.GetComponent<MeshFilter>();
        MeshFilter newMF = newObj.GetComponent<MeshFilter>();
        newMF.mesh = oldMF.sharedMesh;
        newMR.sortingLayerID = oldMR.sortingLayerID;
        newMR.material = oldObj.GetComponent<MeshRenderer>().sharedMaterial;
        float objSize = oldMF.sharedMesh.bounds.size.x;
        return objSize;
    }

    void PassPositionRotation(GameObject newObj, GameObject oldParent, float objSize)
    {
        newObj.transform.rotation = oldParent.transform.rotation;
        newObj.transform.position = oldParent.transform.position + oldParent.transform.GetChild(0).localPosition + oldParent.transform.GetChild(0).GetChild(0).localPosition;
        if (newObj.transform.rotation.z.ToString() == "0.7071068")
            newObj.transform.position += new Vector3(0 - objSize, 0 - objSize);
        else if (newObj.transform.rotation.z.ToString() == "-0.7071068")
            newObj.transform.position += new Vector3(objSize, 0 - objSize);
        else if (newObj.transform.rotation.z == 1 || newObj.transform.rotation.z == -1)
            newObj.transform.position += new Vector3(0, 0 - (objSize * 2));
    }

    string SerialName(string name)
    {
        if (!names.ContainsKey(name))
            names[name] = 1;
        else
            names[name]++;
        return name + names[name].ToString().PadLeft(2, '0');
    }

}