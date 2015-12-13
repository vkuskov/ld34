using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Tree : MonoBehaviour
{
    public LeafData rootLeaf;
    public float sizeInOneSide = 50.0f;
    public float searchStep = 1.0f;
    public float height = 50.0f;
    public LayerMask layerMask;

    private Leaf root;

    private class RaycastSorter : IComparer<RaycastHit>
    {
        public int Compare(RaycastHit x, RaycastHit y)
        {
            return (int)Mathf.Sign(x.distance - y.distance);
        }
    }

    private void Start()
    {
        root = Leaf.Create(rootLeaf);
        root.transform.SetParent(transform);
        root.transform.localPosition = Vector3.zero;
        root.name = "Tree Root";
    }

    public bool FindBestPlace(LeafData leafData, float x, out Leaf leaf, out float shift)
    {
        // We can do "right" thing, implement recursive search and then check if we can fit next leaf.
        // But we already have all data in physics engine that we will use anyway.
        // So we need to raytrace tree from top and easily find best element!
        x = Mathf.Round(x);
        float searchLeft = x;
        float searchRight = x;        
        while (searchLeft > -sizeInOneSide || searchRight < sizeInOneSide)
        {
            if (searchLeft > -sizeInOneSide)
            {
                if (CheckPlace(leafData, searchLeft, out leaf, out shift))
                {
                    shift = ToLocalSpace(leaf, shift);
                    return true;
                }
                searchLeft -= searchStep;
            }
            if (searchRight < sizeInOneSide)
            {
                if (CheckPlace(leafData, searchRight, out leaf, out shift))
                {
                    shift = ToLocalSpace(leaf, shift);
                    return true;
                }
                searchRight += searchStep;
            }
        }
        leaf = null;
        shift = 0;
        return false;
    }

    private bool CheckPlace(LeafData leafData, float x, out Leaf leaf, out float shift)
    {
        Leaf hit = TraceLeaf(x, leafData);
        if (hit != null)
        {
            leaf = hit;
            shift = x;
            return true;
        }
        leaf = null;
        shift = 0;
        return false;
    }

    private float ToLocalSpace(Leaf leaf, float x)
    {
        Vector3 rootLeafWorldPosition = leaf.transform.position;
        Vector3 leafInTreeSpace = transform.worldToLocalMatrix * rootLeafWorldPosition;
        return x - leafInTreeSpace.x;
    }

    private Leaf TraceLeaf(float x, LeafData data)
    {
        Vector3 startPoint = new Vector3(x, height, 0) + data.mesh.bounds.center;
        startPoint = transform.TransformVector(startPoint);
        RaycastHit hit;
        if (Physics.BoxCast(startPoint, data.mesh.bounds.extents, Vector3.down, out hit, transform.rotation, height, layerMask))
        {
            LeafBackLink link = hit.collider.GetComponent<LeafBackLink>();
            if (link != null && link.link != null)
            {
                return link.link;
            }
        }
        return null;
    }

    private void Update()
    {
        /*if (Input.anyKeyDown)
        {
            float x = Random.Range(-50.0f, 50.0f);
            Leaf parent = null;
            float shift = 0;
            if (FindBestPlace(rootLeaf, x, out parent, out shift))
            {
                parent.AddLeaf(Leaf.Create(rootLeaf), shift);
            }
        }*/
    }

    private void OnDrawGizmos()
    {
    }
}
