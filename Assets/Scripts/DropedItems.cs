using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedItems : MonoBehaviour
{
    public GameObject weaponPrefab;//切换武器预制体
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player" ){
            PlayerController.GetInstancae().AddWeapon(weaponPrefab);
            Destroy(gameObject);
        }
        
    }
}
