using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    private int boardWidth = 6;
    private int boardHeight = 8;
    ReinforcePlate[,] Plates;

    private void Start()
    {
        Plates = new ReinforcePlate[boardHeight, boardWidth];
    }


}
