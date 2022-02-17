using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敌人的寻路逻辑
public class EnemySight : MonoBehaviour
{
    public float sight = 110f;//视野角度范围
    public bool isPlayerInSight;//玩家是否在视野范围内
    public Vector3 PlayerLastPos;//玩家消失位置
    public static Vector3 resetDir = Vector3.back;//重置方向向后
    private GameObject player;//玩家
    private SphereCollider sightSphere;//视野球
    // Start is called before the first frame update
    void Start()
    {
        //初始化
        player = GameObject.FindWithTag("Player");
        sightSphere = GetComponent<SphereCollider>();
        PlayerLastPos = resetDir;//初始方向为向后移动
    }

    //触发检测
    private void OnTriggerStay(Collider other) {
        if(other.gameObject == player){
            isPlayerInSight = false;
            Vector3 dir = other.transform.position - transform.position;//移动方向为自身位置与玩家的方向向量
            float angle = Vector3.Angle(dir,transform.forward);//记录移动方向向量与当前正方向的夹角角度
            //当夹角角度小于视野角度的一半时
            if(angle < sight * 0.5){
                RaycastHit hit;//记录射线检测的障碍物
                //射线检测 检测敌人与玩家之间是否有障碍物
                if(Physics.Raycast(transform.position+transform.up,dir.normalized,out hit,sightSphere.radius)){
                    //当检测出此方向上的物体是玩家时   向玩家移动
                    if(hit.collider.gameObject == player){
                        isPlayerInSight = true;
                        PlayerLastPos = player.transform.position;//记录位置
                    }
                }
            }
        }
    }

    //当玩家离开视野时
    private void OnTriggerExit(Collider other) {
        if(other.gameObject == player){
            isPlayerInSight = false;
        }
    }
}
