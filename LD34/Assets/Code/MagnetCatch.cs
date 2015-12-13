using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MagnetBackLink))]
[RequireComponent(typeof(Collider))]
public class MagnetCatch : MonoBehaviour {
    public float catchShift = 2.0f;
    private Magnet magnet;

	void Start ()
    {
        magnet = GetComponent<MagnetBackLink>().link;	
	}

    void OnTriggerEnter(Collider collider)
    {
        ScrapBackLink scrapLink = collider.GetComponent<ScrapBackLink>();
        if (scrapLink != null)
        {
            Ray ray = new Ray(transform.position + Vector3.up * catchShift, Vector3.down);
            RaycastHit hit;
            collider.Raycast(ray, out hit, catchShift * 2.0f);
            Scrap scrap = scrapLink.link;
            Vector3 localPoint = scrap.transform.worldToLocalMatrix * hit.point;
            magnet.PullUp(scrap, localPoint);
        }
        else if (collider.GetComponent<BeltTrigger>() != null)
        {
            magnet.PullUp(null, Vector3.zero);
        }
    }
    	
}
