using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家和敌人的基类
public class RobortController : MonoBehaviour
{
    public int hp=100;//生命值
    //判断是否存活
    public bool IsAlive(){
        return hp>0;
    }

    //受伤减少生命值  生命值不足时调用死亡方法
    public void GetDamge(int dmg){
        hp -=dmg;
        if(!IsAlive()){
            Die();
        }
    }

    //死亡方法
    public void Die(){
        Destroy(this.gameObject);
    }

    //射击子弹的虚类方法
    public virtual void OpenFire(){}
}
