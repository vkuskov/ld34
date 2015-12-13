using UnityEngine;
using System.Collections;

public class Scrap : MonoBehaviour {
    private LeafData sourceData;

    public LeafData Data
    {
        get { return sourceData; }
    }

    public static Scrap Create(LeafData data)
    {
        GameObject rootGO = new GameObject();
        Scrap scrap = rootGO.AddComponent<Scrap>();
        scrap.sourceData = data;
        GameObject meshGO = new GameObject();
        meshGO.transform.SetParent(rootGO.transform);
        meshGO.transform.rotation = Quaternion.Euler(-90.0f, 90, 0);
        MeshFilter meshFilter = meshGO.AddComponent<MeshFilter>();
        meshFilter.mesh = data.mesh;
        MeshRenderer meshRenderer = meshGO.AddComponent<MeshRenderer>();
        meshRenderer.material = data.material;
        MeshCollider collider = meshGO.AddComponent<MeshCollider>();
        collider.convex = true;
        collider.isTrigger = true;
        Rigidbody body = meshGO.AddComponent<Rigidbody>();
        body.isKinematic = true;
        meshGO.layer = LayerMask.NameToLayer("Scrap");
        ScrapBackLink link = meshGO.AddComponent<ScrapBackLink>();
        link.link = scrap;
        meshGO.name = "Mesh";
        rootGO.name = "Scrap";
        return scrap;
    }

}
