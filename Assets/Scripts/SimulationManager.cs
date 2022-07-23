using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour {
    [SerializeField] private GameField gameField;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int width, height;

    [SerializeField] private Vector2 simulationSpeedBoundaries;
    private float _simulationSpeed;
    private float _simulationTimer;

    // Start is called before the first frame update
    private void Start() {
        spriteRenderer.sprite = TextureGenerator.CreateSprite(width, height);

        gameField.Init(width, height);
        gameField.ChangeSprite(spriteRenderer);

        _simulationSpeed = simulationSpeedBoundaries.y;
    }

    // Update is called once per frame
    private void Update() {
        _simulationTimer += Time.deltaTime;
        if (_simulationTimer >= _simulationSpeed) {
            _simulationTimer = 0;
            Simulate();
        }

        HandleInput();
    }

    private void Simulate() {
        gameField.NextTurn();
        gameField.ChangeSprite(spriteRenderer);
    }

    private void HandleInput() {
        int x = (int)Input.mousePosition.x - 150;
        int y = (int)Input.mousePosition.y - 75;
        
        if (x < 0 || x >= width || y < 0 || y >= height) return;
        
        if (Input.GetMouseButtonDown(0)) {
            gameField.SetTileTo(x, y, TileType.Prey);
        } else if (Input.GetMouseButtonDown(1)) {
            gameField.SetTileTo(x, y, TileType.Predator);
        }
    }

    public void SetSimulationSpeed(Slider slider) {
        _simulationSpeed = Mathf.Lerp(simulationSpeedBoundaries.x, simulationSpeedBoundaries.y, slider.value);
    }
}