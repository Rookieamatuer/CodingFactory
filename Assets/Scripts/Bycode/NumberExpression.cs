using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberExpression : Expression
{
    private int value;
    public override int Evaluate()
    {
        return value;
    }
}
