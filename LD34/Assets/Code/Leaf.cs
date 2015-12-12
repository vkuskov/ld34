using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
public class Leaf : MonoBehaviour
{
    private LeafData sourceData;
    private List<Leaf> leafs;
    private List<Vector3> worldPoints;

    private void Start()
    {
        worldPoints = new List<Vector3>(sourceData.childPoints);
        leafs = new List<Leaf>(worldPoints.Count);
        for (int i = 0; i < worldPoints.Count; ++i) // want something better here
        {
            leafs.Add(null);
        }
    }

    public void AddLeaf(Leaf leaf, int pointIndex)
    {
        if (leafs[pointIndex] != null)
        {
            Debug.LogWarningFormat("Leaf index {0} already has element", pointIndex);
            return;
        }
        leafs[pointIndex] = leaf;
        leaf.transform.SetParent(transform);
        leaf.transform.localPosition = sourceData.childPoints[pointIndex];
        leaf.name = string.Format("Leaf #{0}", pointIndex);
    }

    private void UpdatePointsInWorld()
    {
        for (int i = 0; i < worldPoints.Count; ++i)
        {
            worldPoints[i] = transform.TransformVector(sourceData.childPoints[i]);
        }
    }

    private void Update()
    {
        UpdatePointsInWorld();
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
        Leaf leaf = go.AddComponent<Leaf>();
        leaf.sourceData = data;
        meshGO.transform.rotation = Quaternion.Euler(-90.0f, 90, 0);
        return leaf;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < worldPoints.Count; ++i)
        {
            if (leafs[i] != null)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.blue;
            }
            Gizmos.DrawLine(transform.position, worldPoints[i]);
        }
    }
}
