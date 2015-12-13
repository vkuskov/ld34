using UnityEngine;
using System.Collections;

public class Finisher : MonoBehaviour {
    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<LeafBackLink>() != null)
        {
            Debug.Log("===== GAME FINISHED ====");
        }
    }
}
