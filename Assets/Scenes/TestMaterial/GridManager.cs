using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridManager : MonoBehaviour
{
    public int xDirectionLenght;
    public int yDirectionLenght;

    public GridElement spawningNode;

    public List<GridElement> gridNodesList = new List<GridElement>();

    void Start()
    {
        InitGrid();
    }

    public void InitGrid()
    {
        for (int j = 0; j < yDirectionLenght; j++)
        {
            for (int i = 0; i < xDirectionLenght; i++)
            {
                int xPos = i;
                int yPos = j;

                Instantiate(spawningNode, new Vector3(xPos+transform.position.x, yPos+transform.position.y, 0), Quaternion.identity, transform);

                gridNodesList.Add(spawningNode);
            }
        }
    }
}
