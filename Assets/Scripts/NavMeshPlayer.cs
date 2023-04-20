using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshPlayer : MonoBehaviour
{

    NavMeshAgent myAgent;
    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            myAgent.speed = 3.5f;
            SetDestinationToMousePosition();
        }
        if(Input.GetMouseButtonUp(0))
        {
            myAgent.speed = 3.5f;
        }
        if (Input.GetMouseButton(0))
        {
            myAgent.speed = 20f;
            SetDestinationToMousePosition();
        }
    }

    void SetDestinationToMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            myAgent.SetDestination(hit.point);
        }
    }

}
