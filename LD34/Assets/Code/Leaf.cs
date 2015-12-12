using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
public class Leaf : MonoBehaviour
{
    private LeafData sourceData;
    private List<Leaf> leafs;

    private void Start()
    {
        leafs = new List<Leaf>();
    }

    public void AddLeaf(Leaf leaf, float x)
    {
        leafs.Add(leaf);
        leaf.transform.SetParent(transform);
        leaf.transform.localPosition = new Vector3(x, sourceData.height, 0); ;
        leaf.name = string.Format("Leaf #{0}", leafs.Count);
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

    private void OnDrawGizmos()
    {
        
        for (int i = 0; i < leafs.Count; ++i)
        {
            if (leafs[i] != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, leafs[i].transform.position);
            }
        }
    }

}
