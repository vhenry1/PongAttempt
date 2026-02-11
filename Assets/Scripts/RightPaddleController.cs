using UnityEngine;

public class RightPaddle : PaddleController
{
    protected override float GetMovementInput()
    {
        return Input.GetAxis("RightPaddle");
    }
}