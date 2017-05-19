using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    //private Items() { }
    //private static Items instance;
    //public static Items Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //            instance = new Items();
    //        return instance;
    //    }
    //}

    static Player playerCtrl = null;
    public List<SsItem> items = new List<SsItem>();

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerCtrl = player.GetComponent<Player>();

        items = new List<SsItem>()
        {
            new SsItem("Phaser", PhaserUse),
        };
    }

    private void PhaserUse()
    {
        playerCtrl.Shoot();
    }
}