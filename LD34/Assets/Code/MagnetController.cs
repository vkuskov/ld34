using UnityEngine;
using System.Collections;

public class MagnetController : MonoBehaviour {
    public float downSpeed = 50.0f;
    public float upSpeed = 20.0f;
    public float horizontalSpeed = 30.0f;

    public Magnet left;
    public Magnet right;
    public Tree tree;

	void Start()
    {
        SetupMagnet(left);
        SetupMagnet(right);
    }

    private void SetupMagnet(Magnet magnet)
    {
        magnet.tree = tree;
        magnet.downSpeed = downSpeed;
        magnet.upSpeed = upSpeed;
        magnet.horizontalSpeed = horizontalSpeed;
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
