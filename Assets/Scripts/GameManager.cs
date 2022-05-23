using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
     [SerializeField]
    float boardSizeX = 1, boardSizeZ = 1;
    [SerializeField]
    public int aiMaxClones = 1, aiDepth = 1;

    public AudioSource myFX;
    public AudioClip hoverFX;
    public AudioClip clickFX;


    [SerializeField]
    float offsetY = 2;
    BoardManager[] boards = new BoardManager[3];
    public Vector3[,,] coords;
    public PieceController[,,] pieces = new PieceController[3, 4, 4];
    HighlightController[,,] positionHighlights = new HighlightController[3, 4, 4];
    public bool playerTurn = true;
    [SerializeField]
    public UserManager player;
    [SerializeField]
    public GameObject playerPrefab, enemyPrefab, tileHighlightPrefab, boardPrefab, pauseMenuUI;

    public GameObject winGameUI, loseGameUI;

    public GameObject board1, board2, board3;

    public bool tiltOption;

    public int tiltPower = 3, SandsOfTime = 0, computerDifficulty = 0;


    public AIManager enemy;

    Camera mainCamera;
    List<Position> activeTiles;
    PieceController target;


    // Start is called before the first frame update
    void Start()
    {
        InitBoards();
        mainCamera = Camera.main;
        tiltOption = true;
    }

    void updateBoards() {
        coords = new Vector3[3, 4, 4];
        
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
    }

    public void InitBoards() {
       
        coords = new Vector3[3, 4, 4];
        boards[0] = board1.GetComponent<BoardManager>();
        boards[0].boardObject = board1;
        boards[1] = board2.GetComponent<BoardManager>();
        boards[1].boardObject = board2;
        boards[2] = board3.GetComponent<BoardManager>();
        boards[2].boardObject = board3;


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
                    var newInstanceHighlight = Instantiate(tileHighlightPrefab, coords[board, i, j], Quaternion.identity);
                    newInstanceHighlight.transform.parent = boards[board].boardObject.transform;
                    positionHighlights[board, i, j] = newInstanceHighlight.GetComponent<HighlightController>();
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
        var newInstance = Instantiate(isPlayer ? playerPrefab : enemyPrefab, GetCoords(p), Quaternion.identity);
        newInstance.transform.parent = boards[p.board].boardObject.transform;
        PieceController pc = newInstance.GetComponent<PieceController>();
        pc.pieceObject = newInstance;
        pc.position = p;
        pc.Init(boards[p.board]);
        pieces[pc.position.board, pc.position.x, pc.position.z] = pc;
        return pc;
    }

    public void DeletePiece(Position p, PieceController piece) {
        pieces[p.board, p.x, p.z] = null;
        if(piece.player) {
            boards[p.board].playerPieces.Remove(piece);
        }
        else {
            boards[p.board].enemyPieces.Remove(piece);
        }
        Destroy(piece.gameObject);
    }


    private float timeElapsed;
    private float computerThinkTimeDelay;

    void adjustNewSettings() {
         

        switch(computerDifficulty) {
            case 0:
            aiMaxClones=1;
            aiDepth = 1;
            computerThinkTimeDelay=3.5f;
            break;
            case 1:
            aiMaxClones=2;
            aiDepth = 2;
            computerThinkTimeDelay=1.5f;
            break;
            case 2:
            aiMaxClones=4;
            aiDepth = 3;
            computerThinkTimeDelay=0.5f;
            break;
        }
    }

    void checkWinCondition() {
        int enemyEmptyBoards = 0;
        int playerEmptyBoards = 0;
        foreach(var board in boards) {
            if(board.playerPieces.Count == 0) {
                playerEmptyBoards++;
            }
            if(board.enemyPieces.Count == 0) {
                enemyEmptyBoards++;
            }
        }
        if(playerEmptyBoards >= 2) {
            loseGameUI.SetActive(true);
            Time.timeScale = 0f;
            PauseMenu.GameIsPaused = !PauseMenu.GameIsPaused;
        }
        else if(enemyEmptyBoards >= 2) {
            
            winGameUI.SetActive(true);
            Time.timeScale = 0f;
            PauseMenu.GameIsPaused = !PauseMenu.GameIsPaused;
        }
    }
    void Update() {
        if (!PauseMenu.GameIsPaused) {
            adjustNewSettings();
            checkWinCondition();
            if(playerTurn) {
                // Rigid bodies always change position in space
                updateBoards();
            }
            else if (!playerTurn && Time.time - timeElapsed > Random.Range(computerThinkTimeDelay/3.5f,computerThinkTimeDelay))
            {
                timeElapsed = Time.time;
                State newState = enemy.AiMove(aiMaxClones,aiDepth).nextState;
                target = pieces[newState.move.from.board, newState.move.from.x, newState.move.from.z];
                target.MovePiece(newState.move.to);
                myFX.PlayOneShot(clickFX);
                playerTurn = !playerTurn;
                return;
            }
            if (Input.GetMouseButtonDown(0)) {
                timeElapsed = Time.time;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {  
                    //Select stage

                    if (hit.transform.tag == "Player" && playerTurn) {
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
                            myFX.PlayOneShot(hoverFX);
                        }
                        else {
                            target.target = false;
                            target = null;
                        }
                    } else if (hit.transform.tag == "Highlight") {
                        HighlightController.DisableAll();
                        target.MovePiece(hit.transform.gameObject.GetComponent<HighlightController>().position);
                        myFX.PlayOneShot(clickFX);
                        playerTurn = !playerTurn;
                    } 
                }
            }
        }
    }
}
