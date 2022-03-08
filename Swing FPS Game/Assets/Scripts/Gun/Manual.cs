using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Manual Gun", menuName = "Guns/Manual")]
public class Manual : Gun
{
    public override void OnLeftMouseDown(Transform cameraPos)
    {
        Fire(cameraPos);
    }
}
