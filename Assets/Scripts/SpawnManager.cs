using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Flask> flasks;
    [SerializeField] private List<Color> colors;
    [SerializeField] private Ball ballPrefab;

    [SerializeField] private int countBalls;

    private void FillFlaskS()
    {
        List<Ball> balls = new List<Ball>(countBalls * colors.Count);

        //1. рандомно создавать и помещать в balls, чтобы линейно потом распределять по склянкам
        //2. линейно заполнить в balls, чтобы потом рандомно распределять по склянкам
    }
} 
