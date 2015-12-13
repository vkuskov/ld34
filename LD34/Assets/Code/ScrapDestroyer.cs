using UnityEngine;
using System.Collections;

public class ScrapDestroyer : MonoBehaviour {
    void OnTriggerEnter(Collider collider)
    {
        ScrapBackLink scrap = collider.GetComponent<ScrapBackLink>();
        if (scrap != null)
        {
            Destroy(scrap.link.gameObject);
        }
    }
}
