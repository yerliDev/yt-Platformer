using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyYourself : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 1f);
    }

    
}
