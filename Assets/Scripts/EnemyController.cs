using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//可以使用自动寻路功能
public class EnemyController : RobortController
{
    private NavMeshAgent agent;//保存导航代理体
    private Animator animator;//动画控制器
    private EnemySight enemySight;//寻找敌人
    private EnemySight weaponSight;//寻找武器
    private GameObject player;
    public WeaponBase weapon;


    public float patrolSpeed = 5;//巡逻速度
    public int wayPointIndex;//路径点序号
    public float patrolWaitTime = 2.0f;//巡逻等待时间
    public Transform patrolWayPointers;//路径点的父物体
    private float patrolTimer;//等待时间计时器

    public float chaseSpeed = 8;//追击速度
    public float chaseWaitTime = 5;//追击等待时间
    private float  chaseTimer;//追击计时器

    private void Awake() {
        enemySight = transform.Find("EnemySight").GetComponent<EnemySight>();
        weaponSight = transform.Find("WeaponSight").GetComponent<EnemySight>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
    }

    
    void Update()
    {
        //当玩家以及死亡时   不进行任何操作
        if(!PlayerController.GetInstancae().IsAlive()) return;
        //当玩家在武器视野内时开枪射击
        if(weaponSight.isPlayerInSight){
            Shoot();
        }
        //当玩家在敌人视野内时追击
        else if(enemySight.isPlayerInSight){
            Chase();
        }
        //都不在时巡逻
        else{
            Patrol();
        }
        animator.SetFloat("speed",agent.speed / chaseSpeed);
    }

    private void Shoot(){
        agent.isStopped = true;//射击时停止移动
        agent.speed = 0;
        //逐步旋转到当前玩家所在方向
        Vector3 lookatDir = player.transform.position;
        lookatDir.y = transform.position.y;
        Vector3 targetDir = lookatDir - transform.position;

        float step = 5 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward,targetDir,step,0);
        transform.rotation = Quaternion.LookRotation(newDir);

        animator.SetBool("shoot",true); 
    }

    //在动画播放时实现函数
    public override void OpenFire(){
        base.OpenFire();
        weapon.OpenFire(transform.forward);
    }

    private void Chase(){
        agent.isStopped = false;//开始移动
        agent.speed = chaseSpeed;
        Vector3 sightPos = enemySight.PlayerLastPos - transform.position;//移动方向为玩家最后一次出现位置与当前所在位置的向量差
        if(sightPos.sqrMagnitude > 4){
            agent.destination = enemySight.PlayerLastPos;//移动到玩家最后一次出现位置
            //当到达指定位置时停下观察  直到追击计时器满
            if(agent.remainingDistance <= agent.stoppingDistance ){
                chaseTimer += Time.deltaTime;
                if(chaseTimer > chaseWaitTime){
                    chaseTimer = 0;
                    agent.speed = 0;
                }
            }
            else{
                chaseTimer = 0;//计时器需要清零
            }
        }
    }

    private void Patrol(){
        agent.isStopped = false;
        agent.speed = patrolSpeed;
        if(agent.remainingDistance <= agent.stoppingDistance){
            patrolTimer += Time.deltaTime;
            //当巡逻计时满后 判断是否到达巡逻路径点
            if(patrolTimer > patrolWaitTime){
                //当巡逻路径点到最后一个时 重置路径点
                if(wayPointIndex == patrolWayPointers.childCount - 1){
                    wayPointIndex = 0;
                }
                else{
                    wayPointIndex++;
                }
                patrolTimer = 0;
            }  
        }
        //在巡逻途中时爷需要将计时器归零
        else{
            patrolTimer = 0;
        }
        agent.destination = patrolWayPointers.GetChild(wayPointIndex).position;//不断重置当前路径点
    }
}
