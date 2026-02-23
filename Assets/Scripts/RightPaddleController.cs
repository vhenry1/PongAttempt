using UnityEngine;
using Unity.Netcode;

public class RightPaddle : PaddleController
{
    protected override float GetMovementInput()
    {
        if (IsOwner)
        {
            return Input.GetAxis("RightPaddle");
        }

        return 0f;
    }
}