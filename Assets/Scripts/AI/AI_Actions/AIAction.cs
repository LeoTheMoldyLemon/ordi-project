using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction : MonoBehaviour
{
    public Movement movement;
    public abstract void Do();
    public abstract bool Stuck();
}
