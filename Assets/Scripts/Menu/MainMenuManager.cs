using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager _instance;

    public static MainMenuManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MainMenuManager>();
            }
            return _instance;
        }
    }



    public Transform Ani_Card_Start, Ani_Card_End, Ani_Menu_Start, Ani_Menu_End;
    public GameObject Card, MenuCards;

    public List<CardMenu> CardButtons = new List<CardMenu>();
    public Tween Menu_Ani;
    public Tween[] Card_Ani;

    private void Setup_Card_Menu()
    {
        Menu_Ani = MenuCards.transform.DOMove(Ani_Menu_End.position, .5f);
        Menu_Ani.OnPlay(() => OnStart_CardMenu());
        Menu_Ani.Pause();
        Menu_Ani.SetAutoKill(false);
        Menu_Ani.OnComplete(() => MenuCards.transform.DOMove(Ani_Menu_Start.position, .25f).OnComplete(() => Callback_CardMenu()));
    }

    private void OnStart_CardMenu()
    {
        foreach (var item in CardButtons)
        {
            item.CanPlay = false;
        }
    }

    private void Callback_CardMenu()
    {
        foreach (var item in CardButtons)
        {
            item.CanPlay = true;
        }


    }

    public void Play_Card_Menu()
    {
        if (Menu_Ani == null)
        {
            Setup_Card_Menu();
            Menu_Ani.Play();
        }
        else
        {
            Menu_Ani.Restart();
        }
    }



    public void CardMenuSelect(int option)
    {
        Play_Card_Menu();
    }



}
