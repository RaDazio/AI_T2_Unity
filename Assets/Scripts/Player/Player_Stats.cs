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
        if(Input.GetKeyUp(KeyCode.H) )
        {
            updateLife(-30);
        }
        if(Input.GetKeyUp(KeyCode.J) )
        {
            updateLife(20);
        }
        if(Input.GetKeyUp(KeyCode.L) )
        {
            Shoot();
        }
    }

    public void updateLife(int qtd)
    {
        if(life + qtd >= maxLife)
        {
            life = maxLife;
        }
        else if (life + qtd < 0)
        {
            life = 0;
        }
        else
        {
            life += qtd;
        }
        updateLifeHUD();
    }

    public void Shoot()
    {
        if(ammo == 0)
            return;
        else
            ammo--;
        updateAmmoHUD();
    }

    public void updateLifeHUD()
    {
        GameObject.FindGameObjectWithTag("HUD_LIFE").GetComponent<LifePanel>().updateHealth(life);
    }
    
    public void updateAmmoHUD()
    {
        GameObject.FindGameObjectWithTag("HUD_ARROW").GetComponent<ArrowPanel>().updateAmmo(ammo);
    }


}
