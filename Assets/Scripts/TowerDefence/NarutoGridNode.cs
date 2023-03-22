using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarutoGridNode : MonoBehaviour
{
    public bool isGoal = false;
    public bool isObstacle = false;
    public NarutoGridNode narutoLeft, narutoTop, narutoRight, narutoBot;
    public List<NarutoGridNode> narutoAllDirections;
    public int row, column;

    // Start is called before the first frame update
    void Start()
    {
        if (narutoLeft) //come dire (narutoBot !=null)
            narutoAllDirections.Add(narutoLeft);
        if (narutoBot) //come dire (narutoBot !=null)
            narutoAllDirections.Add(narutoBot); 
        if (narutoRight) //come dire (narutoBot !=null)
            narutoAllDirections.Add(narutoRight); 
        if (narutoTop) //come dire (narutoBot !=null)
            narutoAllDirections.Add(narutoTop); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
