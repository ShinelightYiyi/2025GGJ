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
    private float time;
    private bool isInhaled;
    [SerializeField] private GameObject sprite;//两个半圆弧？

    [Header("Blow Info")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private GameObject tool;
    private float distance;
    [SerializeField] private GameObject mouth;

    [SerializeField] private ParticleSystem bubble;
    private bool isBlowing;

    private PrepareInput lastInput;

    void Start()
    {
        makeInput = GetComponent<MakeInput>();
        lastInput =GameObject.Find("PrepareInput").GetComponent<PrepareInput>();
        isInhaled = false;
        isBlowing = false;
        bubble.Stop();
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
            if (!isBlowing)
            {
                isBlowing=true;
                MakeBubble();
            }
            time += Time.deltaTime;
            currentAir = time /proportion;

            if (currentAir >= maxAir)
            {//强制退出吸气
                isInhaled = true;
                time = 0;
            }

        }else if (Keyboard.current.spaceKey.wasReleasedThisFrame)//松开空格退出吹气
        {
            isInhaled = true;
            time = 0;
        }
    }

    void BlowControl()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {

            time += Time.deltaTime;
            currentAir -= time /proportion;
            if(currentAir <= 0)//没气就不准吹了
            {
                makeInput.enabled = false;
            }
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
    }//控制工具移动并计算距离

    void SpriteScaled()//准备写半圆弧缩放，变化速度直接用time操控，和proportion分离开
    {

    }

    void MakeBubble()
    {
        if(isBlowing) bubble.Play();
        else bubble.Stop();
    }
}
