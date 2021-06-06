using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
    public int life = 100;
    public int maxLife = 100;
    public int ammo = 5;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("HUD_ARROW").GetComponent<ArrowPanel>().updateAmmo(ammo);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateLife(int totalLife)
    {
        if(totalLife<0)
        {
            life = 0;
        }
        else if(totalLife > maxLife)
        {
            life = maxLife;
        }
        else
        {
            life = totalLife;
        }
        updateLifeHUD();
    }

    public void UpdateAmmo(int newAmmo)
    {
        ammo = newAmmo;
        updateAmmoHUD();
    }

    private void updateLifeHUD()
    {
        GameObject.FindGameObjectWithTag("HUD_LIFE").GetComponent<LifePanel>().updateHealth(life);
    }
    
    private void updateAmmoHUD()
    {
        GameObject.FindGameObjectWithTag("HUD_ARROW").GetComponent<ArrowPanel>().updateAmmo(ammo);
    }

    public void updateScoreHUD(string score)
    {
        GameObject.FindGameObjectWithTag("HUD_SCORE").GetComponent<TextSetter>().setText("Score: " + score);
    }
    
    public void updateMoneyHUD(int money)
    {
        GameObject.FindGameObjectWithTag("HUD_MONEY").GetComponent<MoneyPanel>().updateMoney(money);
    }
}
