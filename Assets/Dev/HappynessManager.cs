using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappynessManager : MonoBehaviour
{
    [Range(0f, 100f)]
    public float happyness = 100f;

    public Slider slider;

    public List<float> instrumentsCasse = new List<float>();

    private void Start()
    {
        happyness = 100;
    }

    private void Update()
    {
        if(instrumentsCasse.Count == 0 && happyness < 100)
        {
            happyness += Time.deltaTime;
        }
        else if(instrumentsCasse.Count > 0)
        {
            happyness -= Time.deltaTime * instrumentsCasse.Count;
        }
        else if(happyness <= 0)
        {
            Debug.Log("Perdu");
        }

        slider.value = happyness;
    }
}
