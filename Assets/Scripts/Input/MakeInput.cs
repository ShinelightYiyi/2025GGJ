using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

public class MakeInput : MonoBehaviour
{
    private MakeInput makeInput;

    [Header("Inhale Info")]
    [SerializeField]private float maxAir;
    private float currentAir;
    [SerializeField] private float proportion;//时间与空气之比
    [SerializeField]private float declineSpeed;//口腔空气减少速度，应该大于增加速度？
    private float time;
    private bool isInhaled;
    [SerializeField] private GameObject inhaleSprite;//两个半圆弧？

    [Header("Blow Info")]
    private float inhaledAir = 100;//吸入的空气
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private GameObject tool;
    private float distance;
    [SerializeField] private GameObject mouth;
    private float min;
    private float max;

    [Header("Bubble Info")]
    [SerializeField] private GameObject Blow;
    private float difficulty;
    private float weight;
    private float color;
    [SerializeField] Animator anim;

    private PrepareInput lastInput;
    private float[] floats;

    void Start()
    {
        makeInput = GetComponent<MakeInput>();
        lastInput =GameObject.Find("PrepareInput").GetComponent<PrepareInput>();
        isInhaled = false;//吸过没
        BubbleProperties();//获得weight,color
        Blow.SetActive(false);

        //将参数和这里的weight,color联系起来
    }
    void Update()
    {
        if (!isInhaled) InhaleRecord();//用ishaled来判断是否进入吹气
        else if (isInhaled)
        {
            BlowControl();
            ToolControl();
        }
    }

    void InhaleRecord()//记录吸气阶段的空气量
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            Debug.Log(currentAir);

            currentAir += (Time.deltaTime /proportion);
            SpriteScaled();

            if (currentAir > 60)//超过60递减
            {
                time += Time.deltaTime;
                currentAir -= time*0.001f/proportion;

            }
            else time = 0;

            if (currentAir >= maxAir)
            {   //强制退出吸气
                isInhaled = true;
                ChangePeriod();
            }

        }else if (Keyboard.current.spaceKey.wasReleasedThisFrame)//松开空格退出吸气
        {
            isInhaled = true;
            ChangePeriod();
            inhaledAir =currentAir;
        }
    }

    void BlowControl()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            Debug.Log(inhaledAir-currentAir);
            currentAir -= Time.deltaTime*.5f/proportion;
            if(currentAir <= 0)//没气就不准吹了
            {
                makeInput.enabled = false;
                return;
            }
            if (inhaledAir - currentAir > max)
            {
                //播放破裂动画
                inhaledAir -= max;
            }
        }else if (Keyboard.current.spaceKey.wasReleasedThisFrame &&inhaledAir-currentAir>min)
        {
            //播放动画
            anim.SetTrigger("Blow");
            inhaledAir = currentAir;
        } 
    }

    void ToolControl()
    {
        
        distance = Vector2.Distance(tool.transform.position, mouth.transform.position);
        if(Keyboard.current.wKey.isPressed)
        {
            tool.transform.position = new Vector3(tool.transform.position.x, tool.transform.position.y + moveSpeed, 0);
            tool.transform.localScale *= 0.995f;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            tool.transform.position = new Vector3(tool.transform.position.x, tool.transform.position.y - moveSpeed, 0);
            tool.transform.localScale *= 1.005f;
        }
    }//要做一个距离控制气体的

    void SpriteScaled()//准备写半圆弧缩放，变化速度直接用time操控，和proportion分离开
    {
        float pro=currentAir/maxAir;//表现大小
       inhaleSprite.transform.localScale=new Vector3(pro*.7f+.2f, pro * .7f + .2f, pro * .7f + .2f);
    }

    void BubbleProperties()
    {
        #region properties
        floats = lastInput.ratio;
        if (floats[0] > 50 && floats[0] < 65) difficulty = 0;
        else if (floats[0] > 65 && floats[0] < 75) difficulty = 1;
        else if (floats[0] > 75 && floats[0] < 85) difficulty = 2;

        if (floats[1] > 0 && floats[1] < 8) weight = 0;
        else if (floats[1] > 8 && floats[1] < 15) weight = 1;
        else if (floats[1] > 15 && floats[1] < 25) weight = 2;

        if (floats[2] > 0 && floats[2] < 8) color = 0;
        else if (floats[2] > 8 && floats[2] < 15) color = 1;
        else if (floats[2] > 15 && floats[2] < 25) color = 2;
        #endregion

        switch (difficulty)
        {
            case 0:
                min = 20;
                max = 25;
                break;
            case 1:
                min = 40;
                max = 50;
                break;
            case 2:
                min = 60;
                max = 70;
                break;
        }
    }

    void ChangePeriod()
    {
        GameObject.Find("Inhale").SetActive(false);//吸气结束关闭吸气的gameobject
        Blow.SetActive(true);

        time = 0;
    }

}
