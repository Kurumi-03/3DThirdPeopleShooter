using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//使用单例模式方便调用
public class UIManager : MonoBehaviour
{
    private static UIManager Instance;
    public static UIManager GetInstance(){
        return Instance;
    }
    // Start is called before the first frame update
    private void Awake() {
        Instance = this;
    }

    public Image weaponImage;
    public Text bulletNum;
    public Text hpNum;

    public void UpdateWeaponUI(Sprite weaponSprite,int bullet_Num){
        weaponImage.sprite = weaponSprite;
        bulletNum.text = bullet_Num.ToString();
    }
    public void UpdateHPUI(int hp_num){
        hpNum.text = hp_num.ToString();
    }

}
