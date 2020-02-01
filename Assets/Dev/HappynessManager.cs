using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappynessManager : MonoBehaviour
{
    [Range(0f, 100f)]
    public float happyness = 100f;

    public List<float> instrumentsCasse = new List<float>();
}
