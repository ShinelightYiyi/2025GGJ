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
    [SerializeField]private float declineSpeed;//��ǻ���������ٶȣ�Ӧ�ô��������ٶȣ�
    private float time;
    private bool isInhaled;
    [SerializeField] private GameObject inhaleSprite;//������Բ����

    [Header("Blow Info")]
    private float inhaledAir = 100;//����Ŀ���
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
        isInhaled = false;//����û
        BubbleProperties();//���weight,color
        Blow.SetActive(false);

        //�������������weight,color��ϵ����
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
            Debug.Log(currentAir);

            currentAir += (Time.deltaTime /proportion);
            SpriteScaled();

            if (currentAir > 60)//����60�ݼ�
            {
                time += Time.deltaTime;
                currentAir -= time*0.001f/proportion;

            }
            else time = 0;

            if (currentAir >= maxAir)
            {   //ǿ���˳�����
                isInhaled = true;
                ChangePeriod();
            }

        }else if (Keyboard.current.spaceKey.wasReleasedThisFrame)//�ɿ��ո��˳�����
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
            if(currentAir <= 0)//û���Ͳ�׼����
            {
                makeInput.enabled = false;
                return;
            }
            if (inhaledAir - currentAir > max)
            {
                //�������Ѷ���
                inhaledAir -= max;
            }
        }else if (Keyboard.current.spaceKey.wasReleasedThisFrame &&inhaledAir-currentAir>min)
        {
            //���Ŷ���
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
    }//Ҫ��һ��������������

    void SpriteScaled()//׼��д��Բ�����ţ��仯�ٶ�ֱ����time�ٿأ���proportion���뿪
    {
        float pro=currentAir/maxAir;//���ִ�С
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
        GameObject.Find("Inhale").SetActive(false);//���������ر�������gameobject
        Blow.SetActive(true);

        time = 0;
    }

}
