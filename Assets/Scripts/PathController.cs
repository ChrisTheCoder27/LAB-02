using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField] public PathManager pathManager;

    List<Waypoint> thePath;
    Waypoint target;

    public float moveSpeed;
    public float rotateSpeed;

    public Animator animator;
    bool isWalking;

    void Start()
    {
        thePath = pathManager.GetPath();
        if (thePath != null && thePath.Count > 0)
        {
            // Set starting target to the first waypoint
            target = thePath[0];
        }

        isWalking = false;
        animator.SetBool("isWalking", isWalking);
    }

    void RotateTowardsTarget()
    {
        float stepSize = rotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
    void MoveForward()
    {
        float stepSize = moveSpeed * Time.deltaTime;
        float distanceToTarget = Vector3.Distance(transform.position, target.pos);
        if (distanceToTarget < stepSize)
        {
            return;
        }
        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            isWalking = !isWalking;
            animator.SetBool("isWalking", isWalking);
        }
        if (isWalking)
        {
            RotateTowardsTarget();
            MoveForward();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Switch to next target
        target = pathManager.GetNextTarget();
    }
}
