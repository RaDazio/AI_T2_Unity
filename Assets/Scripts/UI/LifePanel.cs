using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LifePanel : MonoBehaviour
{
    public Sprite EmptyHeart;
    public Sprite HalfHeart;
    public Sprite FullHeart;
    public Sprite LifeSprite;
    public Sprite[] NumbersSprite;

    public GameObject[] hearts;
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateHealth(int heathPercentage)
    {
        int heartRep = 100/hearts.Length;

        int fullHearts = heathPercentage/heartRep;
        int halfHearts = (heathPercentage%heartRep)/10;
        int emptyHearts = hearts.Length - fullHearts - halfHearts;

        for(int i = 0 ; i < fullHearts; i ++)
        {
            hearts[i].GetComponent<Image>().sprite = FullHeart;
        }

        for(int i = fullHearts ; i < fullHearts+halfHearts; i ++)
        {
            hearts[i].GetComponent<Image>().sprite = HalfHeart;
        }

        for(int i = fullHearts+halfHearts ; i < fullHearts+halfHearts+emptyHearts; i ++)
        {
            hearts[i].GetComponent<Image>().sprite = EmptyHeart;
        }
    }
}
