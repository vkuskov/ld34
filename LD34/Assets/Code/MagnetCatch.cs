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
            Scrap scrap = scrapLink.link;
            magnet.PullUp(scrap);
        }
        else if (collider.GetComponent<BeltTrigger>() != null)
        {
            magnet.PullUp(null);
        }
    }
    	
}
