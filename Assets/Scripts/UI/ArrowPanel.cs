using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowPanel : MonoBehaviour
{
    public Sprite[] numbers;
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

    public void updateAmmo(int ammo)
    {
        dec.GetComponent<Image>().sprite = numbers[ammo/10];
        uni.GetComponent<Image>().sprite = numbers[ammo%10];
    }
}
