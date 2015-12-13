using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Tree : MonoBehaviour
{
    public float sizeInOneSide = 50.0f;
    public float searchStep = 1.0f;
    public float height = 50.0f;
    public LayerMask layerMask;

    private List<Leaf> leafs = new List<Leaf>();

    private class RaycastSorter : IComparer<RaycastHit>
    {
        public int Compare(RaycastHit x, RaycastHit y)
        {
            return (int)Mathf.Sign(x.distance - y.distance);
        }
    }

    public bool FindBestPlace(LeafData leafData, float x, out Vector3 positionInWorld)
    {
        x = Mathf.Round(x);
        float searchLeft = x;
        float searchRight = x;        
        while (searchLeft > -sizeInOneSide || searchRight < sizeInOneSide)
        {
            if (searchLeft > -sizeInOneSide)
            {
                if (CheckPlace(leafData, searchLeft, out positionInWorld))
                {
                    return true;
                }
                searchLeft -= searchStep;
            }
            if (searchRight < sizeInOneSide)
            {
                if (CheckPlace(leafData, searchRight, out positionInWorld))
                {
                    return true;
                }
                searchRight += searchStep;
            }
        }
        positionInWorld = Vector3.zero;
        return false;
    }

    private bool CheckPlace(LeafData leafData, float x, out Vector3 positionInWorld)
    {
        if (TraceLeaf(x, leafData, out positionInWorld))
        {
            return true;
        }
        return false;
    }

    private bool TraceLeaf(float x, LeafData data, out Vector3 positionInWorld)
    {
        Vector3 startPoint = new Vector3(x, height, 0) + data.mesh.bounds.center;
        startPoint = transform.TransformVector(startPoint);
        RaycastHit hit;
        if (Physics.BoxCast(startPoint, data.mesh.bounds.extents, Vector3.down, out hit, transform.rotation, height, layerMask))
        {
            LeafBackLink link = hit.collider.GetComponent<LeafBackLink>();
            if (link != null && link.link != null)
            {
                positionInWorld = hit.point;
                return true;
            }
        }
        positionInWorld = Vector3.zero;
        return false;
    }

    public void AddLeaf(LeafData data, Vector3 worldPosition)
    {
        Leaf leaf = Leaf.Create(data);
        leaf.transform.SetParent(transform);
        leaf.transform.position = worldPosition;
    }
}
