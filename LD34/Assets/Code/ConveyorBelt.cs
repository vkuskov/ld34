using UnityEngine;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour {
    public float moveSpeed = 2.0f;
    public float minScrapCD = 1.0f;
    public float maxScrapCD = 1.5f;

    public LeafData[] possibleScrap;

    public Transform spawnPoint;
    public Transform scrapRoot;

    private LeafData nextScrap;
    private float scrapCD = 0;

	void Update ()
    {
        if (scrapCD <= 0)
        {
            AddMoreScrap();
        }
        else
        {
            scrapCD -= Time.deltaTime;
        }
	}

    private void AddMoreScrap()
    {
        if (nextScrap == null)
        {
            nextScrap = possibleScrap[Random.Range(0, possibleScrap.Length)];
        }
        Bounds bounds = nextScrap.mesh.bounds;
        // We need to rotate bounding box there. But Unity3D doesn't have function for this and I don't want to do it.
        if (!Physics.CheckBox(nextScrap.mesh.bounds.center + spawnPoint.transform.position, nextScrap.mesh.bounds.extents * 3.0f))
        {
            Scrap scrap = Scrap.Create(nextScrap);
            Mover mover = scrap.gameObject.AddComponent<Mover>();
            mover.direction = Vector3.right;
            mover.speed = moveSpeed;
            scrap.transform.SetParent(scrapRoot);
            scrap.transform.position = spawnPoint.transform.position;
            scrapCD = Random.Range(minScrapCD, maxScrapCD);
            nextScrap = null;
        }
    }
}
