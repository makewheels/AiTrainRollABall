using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class AiRollerAgent : Agent
{
    public Transform target;
    private Rigidbody rbody;

    public Text aiSocreText;
    private int aiSocre = 0;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();

    }

    public override void OnEpisodeBegin()
    {
        //初始化AI球的位置
        //只有AI球掉落的时候才重置到原点
        if (this.transform.position.y < 0.4f)
        {
            this.transform.position = new Vector3(0, 0.5f, 0);
            this.rbody.velocity = Vector3.zero;
            this.rbody.angularVelocity = Vector3.zero;
        }
        //初始化target的位置
        target.position = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //target球的坐标
        sensor.AddObservation(target.position);
        //AI球的坐标
        sensor.AddObservation(this.transform.position);
        //AI球的速度
        sensor.AddObservation(rbody.velocity.x);
        sensor.AddObservation(rbody.velocity.z);


    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //加力
        Vector3 force = Vector3.zero;
        force.x = vectorAction[0];
        force.z = vectorAction[1];
        rbody.AddForce(force*10);

        //判断出界
        if (this.transform.position.y < 0)
        {
            EndEpisode();
        }

        //计算距离
        float distance = Vector3.Distance(this.transform.position, target.position);
        //给奖励
        if (distance < 1.1f)
        {
            aiSocre++;
            aiSocreText.text = aiSocre + "";

            SetReward(1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
        
    }

}
