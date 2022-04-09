using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitBoards();
        mainCamera = Camera.main;
    }

    void InitBoards() {
        float offsetX = boardSizeX * boards[0].localScale.x / 4;
        float offsetZ = boardSizeZ * boards[0].localScale.z / 4;

        for(int board = 0; board < 3; board++) {
            positions[board, 0, 0] = new Vector3(boards[board].position.x - 3*offsetX/2, boards[board].position.y, boards[board].position.z + 3*offsetZ/2);
            for(int i = 0; i < 4; i++) {
                if(i != 0) {
                    positions[board, i, 0] = new Vector3(positions[board, i-1, 0].x, positions[board, i-1, 0].y, positions[board, i-1, 0].z - offsetZ);
                    print($"new position at {positions[board, i, 0]}");
                }
                for(int j = 1; j < 4; j++) {
                    positions[board, i, j] = new Vector3(positions[board, i, j-1].x + offsetX, positions[board, i, j-1].y, positions[board, i, j-1].z);
                    print($"new position at {positions[board, i, j]}");
                }
            }
        }

        for (int board = 0; board < 3; board++) {
            Vector3 playerPosition = positions[board, 0, 0];
            playerPosition.y += offsetY;
            GameObject playerPiece = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
            PieceController playerPiecePC = playerPiece.GetComponent<PieceController>();
            playerPiecePC.position = new Position(board, 0, 0);
            player.pieces.Add(playerPiecePC);

            Vector3 enemyPosition = positions[board, 3, 3];
            enemyPosition.y += offsetY;
            GameObject enemyPiece = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
            PieceController enemyPiecePC = enemyPiece.GetComponent<PieceController>();
            enemyPiecePC.position = new Position(board, 3, 3);
            enemy.pieces.Add(enemyPiecePC);
        }

        for(int board = 0; board < 3; board++) {
            Vector3 boardPosition = boards[board].position;
            boardPosition.y -= 1;
            boardHighlights[board] = Instantiate(boardHighlightPrefab, boardPosition, Quaternion.identity);
            boardHighlights[board].SetActive(false);
            for(int i = 0; i < 4; i++) {
                for(int j = 0; j < 4; j++) {
                    Vector3 position = positions[board, i, j];
                    position.y += 1.5f;
                    positionHighlights[board, i, j] = Instantiate(tileHighlightPrefab, position, Quaternion.identity);
                    positionHighlights[board, i, j].SetActive(false);
                }
            }
        }
    }

    void ChangeTurn() {
        playerTurn = !playerTurn;
        HighlightBoard(playerTurn ? player.focusBoard : enemy.focusBoard);
    }

    // Purely visual
    void HighlightBoard(int board) {
        boardHighlights[board].SetActive(!boardHighlights[board].activeSelf);
    }

    void HighlightPosition(Position p) {
        positionHighlights[p.board, p.x, p.z].SetActive(!positionHighlights[p.board, p.x, p.z].activeSelf);
    }

    // Finish the game when a player wins and the other loses
    void Finish() {

    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {  
                //Select stage
                if ((hit.transform.tag == "Player" && playerTurn) || (hit.transform.tag == "Enemy" && !playerTurn)) {
                    List<Position> moves = hit.transform.gameObject.GetComponent<PieceController>().GetMoves();
                    foreach (Position move in moves) {
                        HighlightPosition(move);
                    }
                } else if (hit.transform.tag == "Highlight") {
                    hit.transform.gameObject.SetActive(false);
                }
            }  
        }  
    }

    [SerializeField]
    float boardSizeX = 1, boardSizeZ = 1;
    [SerializeField]
    public int maxClones = 4;
    [SerializeField]
    float offsetY = 2;

    [SerializeField]
    Transform[] boards = new Transform[3];
    public Vector3[,,] positions = new Vector3[3, 4, 4];
    public PieceController[,,] tiles = new PieceController[3, 4, 4];
    GameObject[,,] positionHighlights = new GameObject[3, 4, 4];
    GameObject[] boardHighlights = new GameObject[3];
    bool playerTurn = true;
    [SerializeField]
    PlayerManager player, enemy;
    [SerializeField]
    GameObject playerPrefab, enemyPrefab, tileHighlightPrefab, boardHighlightPrefab;
    Camera mainCamera;
    List<Position> activeTiles;
}
