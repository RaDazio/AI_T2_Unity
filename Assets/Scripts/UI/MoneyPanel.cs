using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyPanel : MonoBehaviour
{
    public Sprite[] numbers;
    public GameObject cent;
    public GameObject dec;
    public GameObject uni;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateMoney(int money)
    {
        cent.GetComponent<Image>().sprite = numbers[money/100];
        dec.GetComponent<Image>().sprite = numbers[(money/10)%10];
        uni.GetComponent<Image>().sprite = numbers[money%10];
    }
}

