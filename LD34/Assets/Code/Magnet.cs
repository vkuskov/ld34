using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {
    public float downSpeed = 50.0f;
    public float upSpeed = 20.0f;
    public float horizontalSpeed = 30.0f;
    [HideInInspector]
    public Tree tree;

    private Scrap scrap;
    private Vector3 startPoisition;

    private enum State
    {
        Stay,
        MoveDown,
        MoveUp,
        MoveToBack,
        MoveToPosition,
        DropOnTree,
        MoveFromTree,
        MoveToSourcePosition,
        MoveToFront,
    }

    private State state = State.Stay;

    private Leaf targetLeaf;
    private float targetLeafShift;
    private Vector3 targetWorldPosition;

    public bool IsInWork
    {
        get
        {
            return state == State.MoveDown || state == State.MoveUp || state == State.MoveToBack || state == State.MoveToPosition || state == State.DropOnTree;
        }
    }

    void Start()
    {
        startPoisition = transform.position;
    }

    // Big and shitty state machine
    void FixedUpdate()
    {
        Vector3 direction;
        Vector3 horizontalPosition = new Vector3(targetWorldPosition.x, transform.position.y, targetWorldPosition.z);
        Vector3 horizontalBackPosition = new Vector3(startPoisition.x, transform.position.y, targetWorldPosition.z);
        switch (state)
        {
            case State.MoveDown:
                transform.position += Vector3.down * Time.fixedDeltaTime * downSpeed;
                break;
            case State.MoveUp:
                transform.position += Vector3.up * Time.fixedDeltaTime * upSpeed;
                if (transform.localPosition.y >= 0)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
                    if (scrap != null)
                    {
                        if (tree.FindBestPlace(scrap.Data, transform.position.x, out targetLeaf, out targetLeafShift))
                        {
                            state = State.MoveToBack;
                            // We should find better way to do it
                            Vector3 positionInLeafSpace = new Vector3(targetLeafShift, scrap.Data.height, 0);
                            targetWorldPosition = targetLeaf.transform.TransformVector(positionInLeafSpace);
                        }
                        else
                        {
                            state = State.Stay;
                            Destroy(scrap.gameObject);
                            scrap = null;
                        }                        
                    }
                    else
                    {
                        state = State.Stay;
                    }                    
                }
                break;
            case State.MoveToBack:
                transform.position += Vector3.forward * Time.fixedDeltaTime * horizontalSpeed;
                if (transform.position.z >= tree.transform.position.z)
                {
                    state = State.MoveToPosition;
                    transform.position = new Vector3(transform.position.x, transform.position.y, tree.transform.position.z);                    
                }
                break;
            case State.MoveToPosition:
                direction = horizontalPosition - transform.position;
                if (direction.sqrMagnitude <= 1.0f)
                {
                    transform.position = horizontalPosition;
                    state = State.DropOnTree;
                }
                else
                {
                    direction.Normalize();
                    transform.position += direction * Time.fixedDeltaTime * horizontalSpeed;
                }
                break;
            case State.DropOnTree:
                direction = targetWorldPosition - transform.position;
                if (direction.sqrMagnitude <= 1.0f)
                {
                    transform.position = targetWorldPosition;
                    state = State.MoveFromTree;
                    targetLeaf.AddLeaf(Leaf.Create(scrap.Data), targetLeafShift);
                    Destroy(scrap.gameObject);
                    scrap = null;
                }
                else
                {
                    transform.position += Vector3.down * Time.fixedDeltaTime * downSpeed;
                }
                break;
            case State.MoveFromTree:
                transform.position += Vector3.up * Time.fixedDeltaTime * upSpeed;
                if (transform.localPosition.y >= 0)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
                    state = State.MoveToSourcePosition;
                }
                break;
            case State.MoveToSourcePosition:
                direction = horizontalBackPosition - transform.position;
                if (direction.sqrMagnitude <= 1.0f)
                {
                    transform.position = horizontalBackPosition;
                    state = State.MoveToFront;
                }
                else
                {
                    direction.Normalize();
                    transform.position += direction * Time.fixedDeltaTime * horizontalSpeed;
                }
                break;
            case State.MoveToFront:
                direction = startPoisition - transform.position;
                if (direction.sqrMagnitude <= 1.0f)
                {
                    transform.position = startPoisition;
                    state = State.Stay;
                }
                else
                {
                    direction.Normalize();
                    transform.position += direction * Time.fixedDeltaTime * horizontalSpeed;
                }
                break;

        }
    }

    public void Drop()
    {
        if (state == State.Stay)
        {
            state = State.MoveDown;
        }
    }

    public void PullUp(Scrap scrap)
    {
        if (state == State.MoveDown)
        {
            state = State.MoveUp;
            if (scrap != null)
            {
                scrap.GetComponent<Mover>().enabled = false;
                scrap.transform.SetParent(transform, true);
                this.scrap = scrap;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetWorldPosition, 1.0f);
    }
}
