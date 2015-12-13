using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
    public Vector3 direction;
    public float speed;

    private Vector3 normaizedDirection;

	void Start () {
        normaizedDirection = direction.normalized;
	}
	
	void FixedUpdate() {
        transform.position += normaizedDirection * speed * Time.fixedDeltaTime;
	}
}
