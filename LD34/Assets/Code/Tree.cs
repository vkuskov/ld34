using UnityEngine;
using System.Collections.Generic;



public class Tree : MonoBehaviour
{
    public LeafData rootLeaf;

    private Leaf root;

    private bool hack = false;

    private void Start()
    {
        root = Leaf.Create(rootLeaf);
        root.transform.SetParent(transform);
        root.transform.localPosition = Vector3.zero;
        root.name = "Tree Root";
    }

    private void Update()
    {
        if (!hack)
        {
            Leaf test = Leaf.Create(rootLeaf);
            root.AddLeaf(test, 0);
            hack = true;
        }
    }

}
