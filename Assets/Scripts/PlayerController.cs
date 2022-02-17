using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//继承自玩家和敌人的基类
//因为玩家只有一个  设置单例模式
public class PlayerController : RobortController
{
    static PlayerController Instance;
    public static PlayerController GetInstancae(){
        return Instance;
    }
    private void Awake() {
        Instance = this;
    }

    //玩家属性
    public List<WeaponBase> weapons;//玩家的武器背包
    public Transform hand;//武器挂载点
    public float speed = 10;//玩家的移动速度

    private WeaponBase currentWeapon;//当前武器
    private int currentWeaponIndex = 0;//当前武器在背包中的序号(默认为0) 即第一个武器

    //组件的调用
    private Animator animator;
    private CharacterController controller;

    void Start()
    {
        //初始化
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        currentWeapon = weapons[currentWeaponIndex];
    }


    void Update()
    {
        //玩家的移动
        var camera = Camera.main.transform;//摄像机的当前位置
        //玩家正向移动方向为   摄像机正向向量在地面上的投影的方向
        var forward = Vector3.ProjectOnPlane(camera.forward,Vector3.up);
        var right = Vector3.ProjectOnPlane(camera.right,Vector3.up);
        //玩家的移动方向
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        var move = forward*v + right*h;
        //使用CharacterController的Move方法进行移动  使用方向向量的规范化数值
        controller.Move(move.normalized*speed*Time.deltaTime);
        //使用controller的当前速度属性除一跑步时的最高速度 即可得到动画在速度起来后切换为跑步动画
        animator.SetFloat("Speed",controller.velocity.magnitude / speed);
        //结合鼠标的移动来改变玩家的移动方向
        var rotate = GetAimPoint();
        RotateToTarget(rotate);


        //玩家开火
        if(Input.GetButton("Fire1")){
            Shoot();
        }

        //玩家武器切换
        float f = Input.GetAxis("Mouse ScrollWheel");//获取鼠标滑轮值
        if(f > 0 ){
            NextWeapon(1);
        }
        else if(f < 0){
            NextWeapon(-1);
        }


        //更新玩家信息
        UIManager.GetInstance().UpdateHPUI(hp);
        UIManager.GetInstance().UpdateWeaponUI(currentWeapon.img_weapon,currentWeapon.bulletNum);
    }

    //获得目标点和Player位置的方向向量
    public Vector3 GetAimPoint(){
        //鼠标从屏幕投射一条射线
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;//地面射线点
        //射线检测
        if(Physics.Raycast(cameraRay,out floorHit,100.0f,LayerMask.GetMask("Floor"))){
            Vector3 playerToMouse = floorHit.point - transform.position;//使用地面点到玩家位置的向量
            playerToMouse.y = 0;//将向量的y设为0 即为方向移动向量
            return playerToMouse; 
        }
        //射线检测失败返回0向量
        return Vector3.zero;
    }

    //角色转向函数
    public void RotateToTarget(Vector3 rotate){
        transform.LookAt(rotate + transform.position);
    }

    //射击动画播放函数
    public void Shoot(){
        //当现在正在运行的动画状态为第1层时(底层为0)  且状态名为Idle时
        if(animator.GetCurrentAnimatorStateInfo(1).IsName("Idle")){
            animator.SetBool("shoot",true);
        }
    }

    //射击函数
    public override void OpenFire()
    {
        base.OpenFire();
        Debug.Log("Fire");
        currentWeapon.OpenFire(transform.forward);//以人物的正面反向为子弹射击方向
    }

    //切换武器函数
    public void NextWeapon(int step){
        int index = (currentWeaponIndex + step + weapons.Count) % weapons.Count;//因为存在负数所以需要这样设计
        currentWeapon.gameObject.SetActive(false);//将当前武器隐藏
        currentWeapon = weapons[index]; 
        currentWeapon.gameObject.SetActive(true);//切换武器显示
        currentWeaponIndex = index;//切换武器的序号值设为当前武器
    }

    //拾取武器函数
    public void AddWeapon(GameObject addWeapon){
        //在武器背包中查看是否已经拥有当前武器  如果已经拥有则加子弹数
        for(int i=0;i<weapons.Count;i++){
            if(weapons[i].gameObject.name == addWeapon.name){
                currentWeapon.bulletNum+=30;
                return;
            }
        }
        var newWeapon = GameObject.Instantiate(addWeapon,hand);//将拾取的武器初始化到手上
        newWeapon.name = addWeapon.name;//设置武器的名字
        newWeapon.transform.localRotation = currentWeapon.transform.localRotation;//设置武器的旋转
        weapons.Add(newWeapon.GetComponent<WeaponBase>());//将新武器加入武器背包
        NextWeapon(weapons.Count - 1 - currentWeaponIndex);//将新武器设为当前武器
    }
}
