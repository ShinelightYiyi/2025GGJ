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
    [SerializeField] private float proportion;//ʱ�������֮��
    private float time;
    private bool isInhaled;
    [SerializeField] private GameObject sprite;//������Բ����

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
        if (!isInhaled) InhaleRecord();//��ishaled���ж��Ƿ���봵��
        else if (isInhaled)
        {
            BlowControl();
            ToolControl();
        }
    }

    void InhaleRecord()//��¼�����׶εĿ�����
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
            {//ǿ���˳�����
                isInhaled = true;
                time = 0;
            }

        }else if (Keyboard.current.spaceKey.wasReleasedThisFrame)//�ɿ��ո��˳�����
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
            if(currentAir <= 0)//û���Ͳ�׼����
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
    }//���ƹ����ƶ����������

    void SpriteScaled()//׼��д��Բ�����ţ��仯�ٶ�ֱ����time�ٿأ���proportion���뿪
    {

    }

    void MakeBubble()
    {
        if(isBlowing) bubble.Play();
        else bubble.Stop();
    }
}
