using UnityEngine;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour {
    public float moveSpeed = 2.0f;
    public float minScrapCD = 1.0f;
    public float maxScrapCD = 1.5f;

    public LeafData[] possibleScrap;

    public Transform spawnPoint;
    public Transform scrapRoot;

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
        Scrap scrap = Scrap.Create(possibleScrap[Random.Range(0, possibleScrap.Length)]);
        Mover mover = scrap.gameObject.AddComponent<Mover>();
        mover.direction = Vector3.right;
        mover.speed = moveSpeed;
        scrap.transform.SetParent(scrapRoot);
        scrap.transform.position = spawnPoint.transform.position;
        scrapCD = Random.Range(minScrapCD, maxScrapCD);
    }
}
