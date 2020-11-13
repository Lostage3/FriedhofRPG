using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SideCharKI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform enemy;

    float timeRemaining = 10;
    
   
    public Text GameText;

    public float lookRadius = 10f;

    const float locomotionAnimationSmoothTime = .1f;

    NavMeshAgent agent;
    Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        GameText.text = "Wir müssen über den Friedhof kommen, ohne entdeckt zu werden und dann den Spinnenboss besiegen. Bewegen kannst du dich mit WASD und angreifen mit linker Maustaste.";
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if(timeRemaining <= 0)
        {
            GameText.text = "";
            timeRemaining = 10000000000;
        }
        float distance = Vector3.Distance(enemy.position, transform.position);
        if(distance >= lookRadius)
        { 
        FaceTarget();
        agent.SetDestination(target.position);
        float speedPercent = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("SpeedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
        }

        else if (distance <= lookRadius)
        {
            
            FaceEnemy();
            anim.SetTrigger("Attack");
           
        }

    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    void FaceEnemy()
    {
        Vector3 direction = (enemy.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
