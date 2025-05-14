using System.Text;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    private int boardWidth = 6;
    private int boardHeight = 8;
    ReinforcePlate[,] Plates;

    //char grayeChar = 'бр';
    //char blueChar = 'бс';
    //char redChar = 'б▄';

    StringBuilder debugString = new StringBuilder();

    private readonly Vector2Int[] directions = new[]
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    private void Start()
    {
        
    }

    public void InitializeBoard()
    {
        Plates = new ReinforcePlate[boardHeight, boardWidth];
    }

}
