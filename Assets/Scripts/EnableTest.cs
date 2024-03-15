using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTest : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable() {
        Debug.Log("ON");
    }

    private void OnDisable() {
        Debug.Log("OFF");
    }
}
