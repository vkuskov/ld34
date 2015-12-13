using UnityEngine;
using System.Collections;

public class MagnetController : MonoBehaviour {

    public Magnet left;
    public Magnet right;
    public Tree tree;

	void Start()
    {
        left.tree = tree;
        right.tree = tree;
    }

	void Update ()
    {
        if (IsMagnetsAvailable())
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Debug.Log("Left!");
                left.Drop();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("Right!");
                right.Drop();
            }
        }
	}

    private bool IsMagnetsAvailable()
    {
        return !left.IsInWork && !right.IsInWork;
    }
}
