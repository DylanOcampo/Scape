using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardMenu : MonoBehaviour
{
    public int Identifier;
    private float startingPosition;
    private Tween CardUp;
    public bool CanPlay = true;

    public GameObject Button;
    public bool Top;


    private void Start()
    {
        startingPosition = transform.position.y;
    }

    private void Setup_MouseEnter()
    {
        CardUp = transform.DOMoveY(startingPosition + 100, .5f, true);
        CardUp.Pause();
        CardUp.SetAutoKill(false);
    }

    public void OnMouseEnter()
    {
        if (CanPlay)
        {
            Top = true;
            if (CardUp == null)
            {
                Setup_MouseEnter();

                CardUp.Play();
            }
            else
            {
                CardUp.Restart();
            }
        }

    }

    public void ForceRestart()
    {
        if (Top)
        {
            Top = false;
            CardUp.PlayBackwards();
        }
    }

    public void OnMouseExit()
    {
        if (CanPlay)
        {
            if (startingPosition != transform.position.y)
            {
                Top = false;
                CardUp.PlayBackwards();
            }
        }

    }

    public void OnClickCard()
    {
        CanPlay = false;
        MainMenuManager.instance.CardMenuSelect(Identifier);
    }


}
