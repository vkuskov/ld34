using System;
using UnityEngine;
using System.Collections.Generic;

public class Leaf : MonoBehaviour
{
    private LeafData sourceData;

    public LeafData Data
    {
        get
        {
            return sourceData;
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public static Leaf Create(LeafData data)
    {
        if (data == null)
        {
            throw new ArgumentNullException("data");
        }
        GameObject go = new GameObject();
        GameObject meshGO = new GameObject();
        meshGO.transform.SetParent(go.transform);
        meshGO.name = "Mesh";
        MeshFilter meshFilter = meshGO.AddComponent<MeshFilter>();
        meshFilter.mesh = data.mesh;        
        MeshRenderer meshRenderer = meshGO.AddComponent<MeshRenderer>();
        meshRenderer.material = data.material;
        MeshCollider collider = meshGO.AddComponent<MeshCollider>();
        collider.convex = true;
        meshGO.layer = LayerMask.NameToLayer("Leafs");
        Leaf leaf = go.AddComponent<Leaf>();
        leaf.sourceData = data;
        meshGO.transform.rotation = Quaternion.Euler(-90.0f, 90, 0);
        LeafBackLink backLink = meshGO.AddComponent<LeafBackLink>();
        backLink.link = leaf;
        return leaf;
    }

}
