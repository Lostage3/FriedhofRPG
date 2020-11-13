using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class BossScript : MonoBehaviour
{
    [SerializeField] Transform target;

    public float lookRadius = 10f;	// Detection range for player
    float timeRemaining = 20;
    bool timerIsRunning = false;
    float time2 = 2;

    public Text GameText;


    const float locomotionAnimationSmoothTime = .1f;

    NavMeshAgent agent;
    Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
        agent.SetDestination(target.position);
        float speedPercent = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
            if (distance <= agent.stoppingDistance)
            {
                anim.SetTrigger("DownAttack");
                //anim.SetFloat("AttackSine", Mathf.Sin(Time.time));
                timerIsRunning = true;
                Debug.Log(timeRemaining);
            }
        FaceTarget();
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                anim.SetTrigger("DeathTrigger");
                time2 -= Time.deltaTime;
                Debug.Log("Death");
                timeRemaining = 0;
                timerIsRunning = false;
                if(time2 <= 0)
                {
                    Destroy(gameObject);
                    GameText.color = Color.green;
                    GameText.text = "Gewonnen";
                }
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
