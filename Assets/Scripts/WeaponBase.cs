using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public Sprite img_weapon;//武器图标
    public Transform muzzle;//枪口位置
    public GameObject bulletPrefab;//子弹预制体
    public int bulletNum;//武器子弹数
    public float bulletSpeed = 12;//子弹速度

    //开火函数  参数为子弹方向(由鼠标方向决定)
    public void OpenFire(Vector3 dir){
        if(bulletNum > 0){
            var bullet = GameObject.Instantiate(bulletPrefab,muzzle.position,Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = dir * bulletSpeed;
            bulletNum--;
        }
    }

}
