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

    private List<Ray> testRays = new List<Ray>();
    private List<bool> testHits = new List<bool>();


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
        testRays.Clear();
        testHits.Clear();
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
        Leaf hit = TraceLeaf(x);
        if (hit != null)
        {
            if (CheckIfCanFit(hit, x, leafData))
            {
                leaf = hit;
                shift = x;
                return true;
            }
        }
        else
        {
            Debug.LogFormat("Place {0} failed", x);
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

    private bool CheckIfCanFit(Leaf parent, float x, LeafData leafData)
    {
        return true;
    }

    // Generate too musch garbadge but we're ok with that for now
    private Leaf TraceLeaf(float x)
    {
        Vector3 startPoint = new Vector3(x, height, 0);
        Ray ray = new Ray(startPoint, Vector3.down);
        testRays.Add(ray);
        startPoint = transform.TransformVector(startPoint);
        RaycastHit[] hits = Physics.RaycastAll(ray, height, layerMask);
        if (hits != null && hits.Length > 0)
        {
            testHits.Add(true);
            List<RaycastHit> sortedHits = new List<RaycastHit>(hits);
            sortedHits.Sort(new RaycastSorter());
            foreach (RaycastHit hit in sortedHits)
            {
                LeafBackLink link = hit.collider.GetComponent<LeafBackLink>();
                if (link != null)
                {
                    return link.link;
                }
            }
        }
        else
        {
            testHits.Add(false);
        }
        return null;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            float x = Random.Range(-50.0f, 50.0f);
            Leaf parent = null;
            float shift = 0;
            Debug.LogFormat("Try to find place {0}", x);
            if (FindBestPlace(rootLeaf, x, out parent, out shift))
            {
                Debug.LogFormat("Found! {0}", shift);
                parent.AddLeaf(Leaf.Create(rootLeaf), shift);
            }
            else
            {
                Debug.Log("Not found");
            }
        }
    }

    private void OnDrawGizmos()
    {
        UnityEngine.Assertions.Assert.AreEqual(testRays.Count, testHits.Count);
        for (int i = 0; i < testRays.Count; ++i)
        {
            Ray ray = testRays[i];
            Gizmos.color = testHits[i] ? Color.green : Color.red;
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * height);
        }
    }
}
