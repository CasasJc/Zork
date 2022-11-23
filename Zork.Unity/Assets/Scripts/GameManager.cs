using UnityEngine;
using Zork.Common;
using Newtonsoft.Json;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI LocationText;
    [SerializeField]
    private TextMeshProUGUI ScoreText;
    [SerializeField]
    private TextMeshProUGUI MovesText;
    [SerializeField]
    private UnityInputService InputService;
    [SerializeField]
    private UnityOutputService OutputService;
    int moves = 0;
    int score = 0;
    private Game _game;
    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);
        _game.Player.LocationChanged += Player_LocationChanged;
        _game.Run(InputService, OutputService);
        LocationText.text = _game.Player.CurrentRoom.Name;
        _game.Player.MovesChanged += Player_MoveChanged;
        MovesText.text = "Moves: " + moves;
        _game.Player.ScoreChanged += Player_ScoreChanged;
        ScoreText.text = "Score: " + score;
    }
    private void Player_LocationChanged(object sender, Room location)
    {
        LocationText.text = location.Name;
    }
    private void Player_MoveChanged(object sender, int moves)
    {
        MovesText.text = $"Moves: {moves}";

    }
    private void Player_ScoreChanged(object sender, int score)
    {
        ScoreText.text = $"Score: {score}";

    }
    private void Start()
    {
        InputService.SetFocus();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            InputService.ProcessInput();
            InputService.SetFocus();
        }

       if(_game.IsRunning == false)
       {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

       }
    }
}
