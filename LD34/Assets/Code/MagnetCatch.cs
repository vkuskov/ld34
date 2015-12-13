using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MagnetBackLink))]
[RequireComponent(typeof(Collider))]
public class MagnetCatch : MonoBehaviour {
    public LayerMask beltCollisionLayer;

    private Magnet magnet;

	void Start ()
    {
        magnet = GetComponent<MagnetBackLink>().link;	
	}

    void OnTriggerEnter(Collider collider)
    {
        Debug.LogFormat("Cllide with {0}", collider);
        ScrapBackLink scrapLink = collider.GetComponent<ScrapBackLink>();
        if (scrapLink != null)
        {
            Scrap scrap = scrapLink.link;
            magnet.PullUp(scrap);
        }
        else if (beltCollisionLayer == collider.gameObject.layer)
        {
            magnet.PullUp(null);
        }
    }
    	
}
