﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tiled2Unity.CustomTiledImporter]
public class CustomImporter_test : Tiled2Unity.ICustomTiledImporter
{
    public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props)
    {
        if (props.ContainsKey("name"))
        {
            ////// Remove old tile object
            ////Transform oldTileObject = gameObject.transform.Find("TileObject");
            ////if (oldTileObject != null)
            ////{
            ////    GameObject.DestroyImmediate(oldTileObject.gameObject);
            ////}

            //GameObject spawnInstance = (GameObject)Object.Instantiate(Resources.Load(props["name"]));
            ////spawnInstance.name = spawn.name;

            //spawnInstance.transform.parent = gameObject.transform;
            //spawnInstance.transform.localPosition = Vector3.zero;
        }
    }

    public void CustomizePrefab(GameObject prefab)
    {
        //Debug.Log("!");
    }
}