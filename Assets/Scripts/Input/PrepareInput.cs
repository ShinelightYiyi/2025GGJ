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
    [SerializeField] protected RectTransform Square;//将蘸取量体现在长条里

    public float[] ratio = new float[3];//记录每个材料的含量

    public float sumoftime;
    private float time;
    private float sum;

    [SerializeField] private GameObject water0;//不同材料及其编号
    [SerializeField] private GameObject ganyou1;
    [SerializeField] private GameObject xijiejing2;
    private GameObject[] obj = new GameObject[3];

    void Start()
    {
        prepareInput = GetComponent<PrepareInput>();//获取玩家键盘输入
        numOfMaterial = 0;//通过0-2控制蘸取的液体
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
    }//检测数字处于0-2之间

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
    }//对输入进行判断，包括ad与j

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
