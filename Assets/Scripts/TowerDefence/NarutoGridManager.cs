using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NarutoGridManager : MonoBehaviour
{
    public Material myMaterial;
    private float waitTime = .5f;

    public List<NarutoGridNode> narutoNodeList;
    public List<List<NarutoGridNode>> narutoMatrix;
    //public List<NarutoGridNode> narutoRow1;
    public void InitNarutoMatrix()
    {
        //CERCA NEI GAME OBJ I NARUTO NODES, NON FARE MAI IN UPDATE
        narutoNodeList = new List<NarutoGridNode>(FindObjectsOfType<NarutoGridNode>());
        narutoNodeList.Sort((narutino1, narutino2) => narutino1.transform.position.z.CompareTo(narutino2.transform.position.z));

        int iRow = 0;
        foreach (NarutoGridNode narutino in narutoNodeList)
        {
            //se la lista non è inizializzata oppure se è vuota
            if (narutoMatrix == null || narutoMatrix.Count == 0)
            {
                //inizializzo al lista di liste
                narutoMatrix = new List<List<NarutoGridNode>>();
                //aggiungo la mia prima riga alla matrice
                narutoMatrix.Add(new List<NarutoGridNode>());
                narutoMatrix[iRow].Add(narutino);
            }
            else if (narutino.transform.position.z == narutoMatrix[iRow][0].transform.position.z)
            {
                //se la z è la stessa inserisco nella riga corrente
                narutoMatrix[iRow].Add(narutino);
            }
            else
            {
                //iRow++;
                narutoMatrix.Add(new List<NarutoGridNode>());
                narutoMatrix[++iRow].Add(narutino);

            }
        }
        foreach (List<NarutoGridNode> narutinoRow in narutoMatrix)

            narutinoRow.Sort((narutino1, narutino2) => narutino1.transform.position.x.CompareTo(narutino2.transform.position.x));

        for (int i_Row = 0; i_Row < narutoMatrix.Count; i_Row++)
        {
            for (int i_Column = 0; i_Column < narutoMatrix[i_Row].Count; i_Column++)
            {
                narutoMatrix[i_Row][i_Column].row = i_Row;
                narutoMatrix[i_Row][i_Column].column = i_Column;

                if (!narutoMatrix[i_Row][i_Column].isObstacle)
                {
                    if (i_Row < narutoMatrix.Count - 1 && !narutoMatrix[i_Row + 1][i_Column].isObstacle)
                        narutoMatrix[i_Row][i_Column].narutoTop = narutoMatrix[i_Row + 1][i_Column];

                    if (i_Row > 0 && !narutoMatrix[i_Row - 1][i_Column].isObstacle)
                        narutoMatrix[i_Row][i_Column].narutoBot = narutoMatrix[i_Row - 1][i_Column];

                    if (i_Column < narutoMatrix[i_Row].Count - 1 && !narutoMatrix[i_Row][i_Column + 1].isObstacle)
                        narutoMatrix[i_Row][i_Column].narutoRight = narutoMatrix[i_Row][i_Column + 1];

                    if (i_Column > 0 && !narutoMatrix[i_Row][i_Column - 1].isObstacle)
                        narutoMatrix[i_Row][i_Column].narutoLeft = narutoMatrix[i_Row][i_Column - 1];
                }
            }
        }

        Debug.Log("lista di liste fatta");

        Invoke("TestNarutoSearch",1f);
    }

    public NarutoGridNode testNarutoStart;
    public void TestNarutoSearch()
    {
        NarutoFindExitPlease(testNarutoStart);
    }

    //public List<NarutoGridNode>
    public List<NarutoGridNode> narutoPath, narutedNodes;
    public List<NarutoGridNode> NarutoFindExitPlease(NarutoGridNode startNode)
    {
        narutoPath=new List<NarutoGridNode>();
        narutedNodes.Add(startNode);

        //controllo se sono gia su un goal
        if(startNode.isGoal)
        {
            //ho finito
            narutoPath.Add(startNode);
            startNode.GetComponent<MeshRenderer>().material.DOColor(Color.red, .1f);

            return narutoPath;
        }
        //controllo attorno a me che non ci sia l'uscita
        foreach(NarutoGridNode node in startNode.narutoAllDirections)
        {
            if (node.isGoal)
            {
                narutoPath.Add(node);
                narutoPath.Add(startNode);
                node.GetComponent<MeshRenderer>().material.DOColor(Color.red, .1f);
                startNode.GetComponent<MeshRenderer>().material.DOColor(Color.red, .1f);

                return narutoPath;
            }
        }
        foreach (NarutoGridNode node in startNode.narutoAllDirections)
        {
            //mando un clone in ogni direzione
            if(NarutoCloneFindExit(node))
            {
                narutoPath.Add(startNode);
                startNode.GetComponent<MeshRenderer>().material.DOColor(Color.red, .1f);

                return narutoPath;
            }
        }
        return narutoPath;
    }

    public bool NarutoCloneFindExit(NarutoGridNode currentNode)
    {
        narutedNodes.Add(currentNode);

        foreach (NarutoGridNode node in currentNode.narutoAllDirections)
        {
            if (!narutedNodes.Contains(node))
            {
                if (node.isGoal)
                {
                    narutoPath.Add(node);
                    node.GetComponent<MeshRenderer>().material.DOColor(Color.red, .1f);

                    narutoPath.Add(currentNode);
                    currentNode.GetComponent<MeshRenderer>().material.DOColor(Color.red, .1f);

                    return true;
                }
            }
        }
        foreach (NarutoGridNode node in currentNode.narutoAllDirections)
        {
            if (!narutedNodes.Contains(node))
            {
                if (NarutoCloneFindExit(node))
                {
                    narutoPath.Add(currentNode);
                    currentNode.GetComponent<MeshRenderer>().material.DOColor(Color.red, .1f);
                    return true;
                }
            }
        }
        return false;
    }



    private void Awake()
    {
        myMaterial.color = Color.white;
        InitNarutoMatrix();
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ColorChangeCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //IEnumerator GridColorChange()
    //{
    //    myMaterial.DOColor(Color.red, .2f);
    //    yield return new WaitForSeconds(.5f);
    //}

    private IEnumerator ColorChangeCoroutine()
    {
        while (true)
        {
            myMaterial.DOColor(Color.green, waitTime);
            yield return new WaitForSeconds(waitTime);
            myMaterial.DOColor(Color.blue, waitTime);
            yield return new WaitForSeconds(waitTime);
            myMaterial.DOColor(Color.red, waitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

}
