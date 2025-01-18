using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrepareInput : MonoBehaviour
{
    private PrepareInput prepareInput;

    [SerializeField]private Text text;
    public int numOfMaterial {  get; private set; }
    [SerializeField] protected RectTransform Square;//��պȡ�������ڳ�����

    public float[] ratio = new float[3];//��¼ÿ�����ϵĺ���

    public float sumoftime;
    private float time;
    private float sum;

    [SerializeField] private GameObject water0;//��ͬ���ϼ�����
    [SerializeField] private GameObject ganyou1;
    [SerializeField] private GameObject xijiejing2;
    private GameObject[] obj = new GameObject[3];

    void Start()
    {
        prepareInput = GetComponent<PrepareInput>();//��ȡ��Ҽ�������
        numOfMaterial = 0;//ͨ��0-2����պȡ��Һ��
        prepareInput.enabled = true;
        for(int i = 0; i < 3; i++)
        {
            switch(i)
            {
                case 0:
                    obj[i] = water0;
                    break;
                case 1:
                    obj[i] = ganyou1;
                    break;
                case 2:
                    obj[i]= xijiejing2;
                    break;
            }
        }
        BottleChoose(obj[numOfMaterial]);
    }
    void Update()
    {
        CheckInput();
        ChangeScene();
    }

    void CheckNum()
    {
        if (numOfMaterial < 0) numOfMaterial = 2;
        if (numOfMaterial > 2) numOfMaterial = 0;
        BottleChoose(obj[numOfMaterial]);
    }//������ִ���0-2֮��

    void CheckInput()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            RestorePosition(obj[numOfMaterial]);
            numOfMaterial--;
            CheckNum();
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            RestorePosition(obj[numOfMaterial]);
            numOfMaterial++;
            CheckNum();
        }

        if (Keyboard.current.jKey.isPressed)
        {
            time += Time.deltaTime;
            sum = (time / sumoftime) * 100;
            Square.sizeDelta = new Vector2(sum*2, 20);

            text.text = ((int)sum).ToString() + "/100";

            if (time >= sumoftime)
                prepareInput.enabled = false;

        }else if (Keyboard.current.jKey.wasReleasedThisFrame)
        {
            ratio[numOfMaterial] = sum;
            for(int i = 0; i < 2; i++)
            {
                if(i==numOfMaterial)
                    continue;
                else if (i != numOfMaterial)
                {
                    ratio[numOfMaterial]-=ratio[i];
                }
            }
        }
    }//����������жϣ�����ad��j

    void ChangeScene()
    {
        if(sum>=100)
        {
            SceneManager.LoadScene("Make");
            DontDestroyOnLoad(prepareInput);
        }
    }

    void BottleChoose(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + 3, 0);
    }

    void RestorePosition(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - 3, 0);
    }

}
