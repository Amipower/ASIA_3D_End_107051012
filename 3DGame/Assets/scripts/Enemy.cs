using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent nav;
    private Animator ani;
    private float timer;

    [Header("移動速度"), Range(0, 50)]
    public float speed = 4;
    [Header("停止距離"), Range(0, 50)]
    public float stopDistance = 2;
    [Header("攻擊冷卻時間"), Range(0, 50)]
    public float cd = 2f;

    private void Awake()
    {
        //取得身上的元件<代理器>
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        //尋找其他遊戲物件("物件名稱").變形元件
        player = GameObject.Find("髒比").transform;
        nav.speed = speed;
        nav.stoppingDistance = stopDistance;
    }

    private void Update()
    {
        Track();
        Attack();
    }
    private void Attack()
    {
        if (nav.remainingDistance < stopDistance)
        {
            timer += Time.deltaTime;
            Vector3 pos = player.position;
            pos.y = transform.position.y;

            transform.LookAt(pos);

            if (timer >= cd)
            {
                ani.SetTrigger("攻擊觸發");
                timer = 0;
            }    
        }
    }
    private void Track()
    {
        nav.SetDestination(player.position);
        ani.SetBool("跑步開關", nav.remainingDistance > stopDistance);
    }

}
