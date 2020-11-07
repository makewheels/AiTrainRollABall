using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserBall : MonoBehaviour
{
    public Transform target;
    private Rigidbody rbody;
    public DynamicJoystick joystick;

    public Text userSocreText;
    private int userSocre=0;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //加力
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        Vector3 force = Vector3.zero;
        force.x = horizontal;
        force.z = vertical;
        rbody.AddForce(force * 10);

        //判断出界
        if (this.transform.position.y < 0.4f)
        {
            this.transform.position = new Vector3(0, 0.5f, 0);
            rbody.velocity = new Vector3(0, 0, 0);
            //rbody.angularVelocity=new Vector3(0, 0, 0);
            rbody.MovePosition(new Vector3(0, 0.5f, 0));
        }

        //计算距离
        float distance = Vector3.Distance(this.transform.position, target.position);
        //给奖励
        if (distance < 1.1f)
        {
            userSocre++;
            userSocreText.text = userSocre+"";
            //初始化target的位置
            target.position = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
        }
    }
}
