using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGridLayout : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // Reference to the GridLayoutGroup component
    public Button cellPrefab; // Reference to the cell button prefab
    private List<Button> cellButtons; // List to hold references to the cell buttons

    void Start()
    {
        cellButtons = new List<Button>();
        InitializeGrid(3); // Example to initialize with 3x3 grid
    }

    public void InitializeGrid(int size)
    {
        // Clear existing buttons
        foreach (Button button in cellButtons)
        {
            Destroy(button.gameObject);
        }
        cellButtons.Clear();

        // Set GridLayoutGroup constraints
        gridLayoutGroup.constraintCount = size;

        // Adjust cell size based on grid size
        float panelWidth = gridLayoutGroup.GetComponent<RectTransform>().rect.width;
        float panelHeight = gridLayoutGroup.GetComponent<RectTransform>().rect.height;
        float cellWidth = panelWidth / size - gridLayoutGroup.spacing.x;
        float cellHeight = panelHeight / size - gridLayoutGroup.spacing.y;
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

        // Create buttons for each cell in the grid
        for (int i = 0; i < size * size; i++)
        {
            Button cellButton = Instantiate(cellPrefab, gridLayoutGroup.transform);
            cellButtons.Add(cellButton);
        }
    }
}
