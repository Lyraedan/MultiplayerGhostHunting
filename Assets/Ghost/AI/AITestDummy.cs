using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITestDummy : AI
{
    public override void OnAttack()
    {
    }

    public override void PerformAction()
    {
        StartCoroutine(Wander());
    }
}
