using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Card> test = new List<Card>();
    public Player testP;

    public void OnClick_Test()
    {
        testP.SetCardHand(test.ToArray());
    }
}
