using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//子弹控制
public class Bullet : MonoBehaviour
{
    public int power = 10;//子弹威力
    //通过子弹的触发器确认当前碰到的物体
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<PlayerController>().GetDamge(power);
        }
        else if(other.gameObject.tag == "Enemy"){
            other.gameObject.GetComponent<EnemyController>().GetDamge(power);
        }else{
            Destroy(gameObject);
        }
        Destroy(gameObject);//碰到物体后销毁
    }
}
