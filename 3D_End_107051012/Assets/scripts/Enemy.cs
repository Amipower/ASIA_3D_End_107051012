using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("移動速度"), Range(0, 50)]
    public float speed = 4;
    [Header("停止距離"), Range(0, 50)]
    public float stopDistance = 2;
    [Header("攻擊冷卻時間"), Range(0, 50)]
    public float cd = 2f;
    [Header("攻擊中心點")]
    public Transform atkPoint;
    [Header("攻擊長度"), Range(0f, 5f)]
    public float atkLength;
    [Header("攻擊力"), Range(0, 500)]
    public float atk = 30;

    private Transform player;
    private NavMeshAgent nav;
    private Animator ani;
    private float timer;

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
    private void OnDrawGizmos()
    {
        //圖示.顏色=黃色
        Gizmos.color = Color.yellow;
        //圖示.繪製射線(中心點，方向)
        //(攻擊中心點的座標，攻擊中心點的前方*攻擊長度)
        Gizmos.DrawRay(atkPoint.position, atkPoint.forward * atkLength);
    }
    private RaycastHit hit;

    private float hp = 100;

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
                if (Physics.Raycast(atkPoint.position, atkPoint.forward, out hit, atkLength, 1 << 8))
                {
                    hit.collider.GetComponent<Player>().Damage(atk);
                }
            }    
        }
    }
    /// <summary>
    /// 受傷
    /// </summary>
    /// <param name="damage">接收的傷害值</param>
    public void Damage(float damage)
    {
        hp -= damage;
        ani.SetTrigger("受傷觸發");
        if (hp <= 0) Dead();
    }
    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {
        nav.isStopped = true;
        enabled = false;
        ani.SetBool("死亡觸發", true);
    }
    private void Track()
    {
        nav.SetDestination(player.position);
        ani.SetBool("跑步開關", nav.remainingDistance > stopDistance);
    }

}
