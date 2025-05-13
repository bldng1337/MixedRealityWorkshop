


using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public abstract class Rune : MonoBehaviour
{

    public abstract void UpdateRune();
}


public class Energy
{
    public float value;
}

public class ConnectionSocket
{
    Rune m_Rune;
}