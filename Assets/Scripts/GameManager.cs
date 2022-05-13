using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitBoards();
        mainCamera = Camera.main;
        winmsg.SetActive(false);
    }

    void InitBoards() {
        for(int i = 0; i < 3; i++) {
            int x = i * 6 - 6;

            BoardManager newBoard = Instantiate(boardPrefab, new Vector3(x, 0, 0), Quaternion.identity).GetComponent<BoardManager>();
            boards[i] = newBoard;
        }


        float offsetX = boardSizeX * boards[0].transform.localScale.x / 4;
        float offsetZ = boardSizeZ * boards[0].transform.localScale.z / 4;

        for(int board = 0; board < 3; board++) {
            coords[board, 0, 0] = new Vector3(boards[board].transform.position.x - 3*offsetX/2, boards[board].transform.position.y + offsetY, boards[board].transform.position.z + 3*offsetZ/2);
            for(int i = 0; i < 4; i++) {
                if(i != 0) {
                    coords[board, i, 0] = new Vector3(coords[board, i-1, 0].x, coords[board, i-1, 0].y, coords[board, i-1, 0].z - offsetZ);
                }
                for(int j = 1; j < 4; j++) {
                    coords[board, i, j] = new Vector3(coords[board, i, j-1].x + offsetX, coords[board, i, j-1].y, coords[board, i, j-1].z);
                }
            }
        }

        for (int board = 0; board < 3; board++) {
            AddPiece(new Position(board, 0, 0), true);
            AddPiece(new Position(board, 3, 3), false);
        }

        for(int board = 0; board < 3; board++) {
            Vector3 boardPosition = boards[board].transform.position;
            boardPosition.y -= 1;
            for(int i = 0; i < 4; i++) {
                for(int j = 0; j < 4; j++) {
                    positionHighlights[board, i, j] = Instantiate(tileHighlightPrefab, coords[board, i, j], Quaternion.identity).GetComponent<HighlightController>();
                    positionHighlights[board, i, j].gameObject.SetActive(false);
                    positionHighlights[board, i, j].position = new Position(board, i, j);
                }
            }
        }
    }

    void ChangeTurn() {
        playerTurn = !playerTurn;
    }

    void HighlightPosition(Position p) {
        positionHighlights[p.board, p.x, p.z].gameObject.SetActive(!positionHighlights[p.board, p.x, p.z].gameObject.activeSelf);
    }

    // Finish the game when a player wins and the other loses
    void Finish(bool player) {
        TextMeshPro text = winmsg.GetComponent<TextMeshPro>();
        if(player) {
            text.color = Color.blue;
            text.text = "You won!";
        } else {
            text.color = Color.red;
            text.text = "You lose!";
        }
        winmsg.SetActive(true);
        Time.timeScale = 0;
    }

    public PieceController GetPiece(Position p) {
        return pieces[p.board, p.x, p.z];
    }

    public HighlightController GetHighlight(Position p) {
        return positionHighlights[p.board, p.x, p.z];
    }

    public Vector3 GetCoords(Position p) {
        return coords[p.board, p.x, p.z];
    }
    
    public PieceController AddPiece(Position p, bool isPlayer) {
        PieceController pc = Instantiate(isPlayer ? playerPrefab : enemyPrefab, GetCoords(p), Quaternion.identity).GetComponent<PieceController>();
        pc.position = p;
        pc.Init(boards[p.board]);
        pieces[pc.position.board, pc.position.x, pc.position.z] = pc;
        return pc;
    }

    public void DeletePiece(Position p, PieceController piece) {
        pieces[p.board, p.x, p.z] = null;
        if(piece.player) boards[p.board].playerPieces.Remove(piece);
        else boards[p.board].enemyPieces.Remove(piece);
        Destroy(piece.gameObject);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {  
                //Select stage
                if ((hit.transform.tag == "Player" && playerTurn) || (hit.transform.tag == "Enemy" && !playerTurn)) {
                    PieceController pc = hit.transform.gameObject.GetComponent<PieceController>();
                    HighlightController.DisableAll();
                    if(!pc.target){
                        if(target) target.target = false;
                        target = pc;
                        target.target = true;
                        List<Position> moves = pc.GetMoves();
                        foreach (Position move in moves) {
                            HighlightPosition(move);
                        }
                    } else {
                        target.target = false;
                        target = null;
                    }
                } else if (hit.transform.tag == "Highlight") {
                    HighlightController.DisableAll();
                    target.MovePiece(hit.transform.gameObject.GetComponent<HighlightController>().position);
                    playerTurn = !playerTurn;
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
    BoardManager[] boards = new BoardManager[3];
    public Vector3[,,] coords = new Vector3[3, 4, 4];
    public PieceController[,,] pieces = new PieceController[3, 4, 4];
    HighlightController[,,] positionHighlights = new HighlightController[3, 4, 4];
    public bool playerTurn = true;
    [SerializeField]
    public UserManager player, enemy;
    [SerializeField]
    public GameObject playerPrefab, enemyPrefab, tileHighlightPrefab, boardPrefab, winmsg;
    Camera mainCamera;
    List<Position> activeTiles;
    PieceController target;
}
