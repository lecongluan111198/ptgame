using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class BuildingMove : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("move");
        this.transform.parent.transform.parent.GetComponent<Building>()._SetDeselectedState();
    }
}
