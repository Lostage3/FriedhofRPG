using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EnemyKIzPos : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Collider targetCollider;
    [SerializeField] float zWalkDistance = 30f;

    public Text GameText;

    Vector3 wayPoint1;
    Vector3 wayPoint2;

    const float locomotionAnimationSmoothTime = .1f;
    float speedPercent;

    NavMeshAgent agent;
    Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        wayPoint1 = transform.position;
        wayPoint2 = new Vector3(transform.position.x, transform.position.y, transform.position.z +zWalkDistance);
        agent.SetDestination(wayPoint2);
    }

    private void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= agent.stoppingDistance)
        {
            SceneManager.LoadScene("Friedhof", LoadSceneMode.Single);
        }

        speedPercent = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);

        if (transform.position.z >= wayPoint2.z - 2f)
        {
            agent.SetDestination(wayPoint1);
        }
        else if (transform.position.z <= wayPoint1.z + 2f)
        {
            agent.SetDestination(wayPoint2);
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other == targetCollider)
        {
            GameText.color = Color.red;
            GameText.text = "Was macht ihr hier? Runter vom Friedhof";
            Debug.Log("Was macht ihr hier? Runter vom Friedhof");
            agent.SetDestination(target.position);
            anim.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
            FaceTarget();
            
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
