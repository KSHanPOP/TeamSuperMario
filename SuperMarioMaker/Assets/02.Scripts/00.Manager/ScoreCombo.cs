using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCombo : MonoBehaviour
{
    public int[] scoreCombo;

    public static ScoreCombo Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
