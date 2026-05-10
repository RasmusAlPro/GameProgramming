using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunchEvents : MonoBehaviour
{
    public PlayerPunchHitbox punchHitbox;

    public void StartPunch()
    {   
        Debug.Log("StartPunch");
        punchHitbox.StartPunch();
    }

    public void EndPunch()
    {   
        Debug.Log("EndPunch");
        punchHitbox.EndPunch();
    }
}
