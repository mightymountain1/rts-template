using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UnitSelection : MonoBehaviour
{

    public RectTransform selectionBox;
    public LayerMask unitLayerMask;

    public  List<PlayerUnit> selectedUnits = new List<PlayerUnit>();



    private Vector2 startPos;

    // Components
    public Camera cam;
    private Player player;

    public Transform playerUnitparent;


    void Awake()
    {

        // get components
        //cam = Camera.main;
        player = GetComponent<Player>();



    }



    void Update()
    {

        ProcessPCTouch();


    }




    private void ProcessPCTouch()
    {


        // mouse down
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //
        {
            ToggleSelectionVisual(false);
            ToggleRangeVisual(false);
         //   ToggleUnitPanel(false);
            selectedUnits = new List<PlayerUnit>();
            ClearSelectedUnit();
            TrySelect(Input.mousePosition);
            startPos = Input.mousePosition;
        }

        // mouse up
        if (Input.GetMouseButtonUp(0))// && !EventSystem.current.IsPointerOverGameObject()
        {
            ReleaseSelectionBox();
        }

        // mouse held down
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())//
        {
            UpdateSelectionBox(Input.mousePosition);
        }


        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //   
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Ground")
                    {
                        // close other UI's

                        //UnitCommander.MyInstance.ClosePlayerBasePanel();
                        ClearSelectedUnit();
                      //  UIManager.MyInstance.multipleUnitsSelectedImage.SetActive(false);
                       // UnitCommander.MyInstance.CloseGateUI();


                        //    selectedUnits.Add(hit.collider.GetComponent<PlayerUnit>());

                        //if (selectedUnit)
                        //{
                        //    ClearSelectedUnit();
                        //    ClosePlayerBasePanel();

                        //}
                        //ClosePlayerBasePanel();
                        //// assign selected unit
                        //selectedUnit = hit.collider.GetComponent<PlayerUnit>();

                        //hasUnitSelected = true;
                        //selectedUnit.unitUICircle.gameObject.SetActive(true);
                        //selectedUnit.unitSelectionMarker.SetActive(true);

                    }
                }

            }
        }
    }

    // called when we click on a unit
    void TrySelect(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 9000, unitLayerMask))
        {
            PlayerUnit playerUnit = hit.collider.GetComponent<PlayerUnit>();


            if (hit.collider != null)
            {
                if (hit.collider.tag == "PlayerUnit" && !hit.collider.GetComponent<PlayerUnit>().isDead)
                {

                    if (HasUnitSelected())
                    {
                        ClearSelectedUnit();
                    }

                    // assign selected unit
                    //  selectedUnits.Add(playerUnit);
                    AddUnitsToSelection(playerUnit);
                    playerUnit.ToggleSelectionMarker(true);
                  //  UnitCommander.MyInstance.ClosePlayerBasePanel();
                  //  UnitCommander.MyInstance.CloseGateUI();
                  //  playerUnit.OpenCircleUI();
                    // playerUnit.unitUICircle.gameObject.SetActive(true);


                    //  hasUnitSelected = true;
                    //    selectedUnit.unitUICircle.gameObject.SetActive(true);
                    // selectedUnit.unitSelectionMarker.SetActive(true);

                }
            }


        }
    }

    public void AddUnitsToSelection(PlayerUnit playerUnit)
    {

        // ClearSelectedUnit();

        selectedUnits.Add(playerUnit);

        if (selectedUnits.Count == 1)
        {
           // UIManager.MyInstance.multipleUnitsSelectedImage.SetActive(false);
            //  playerUnit.unitUICircle.gameObject.SetActive(true);
          //  playerUnit.OpenCircleUI();
           // playerUnit.unitRangeMarker.gameObject.SetActive(true);
        }
        else if (selectedUnits.Count == 0)
        {
            ClearSelectedUnit();
        }
        else if (selectedUnits.Count > 1)
        {
          //  UIManager.MyInstance.multipleUnitsSelectedImage.SetActive(true);
            ClearSelectedUnit();
        }
    }


    public void ClearSelectedUnit()
    {

        foreach (PlayerUnit unit in selectedUnits)
        {
            // unit.unitUICircle.gameObject.SetActive(false);
       //     unit.CloseCircleUI();
        //    unit.unitRangeMarker.gameObject.SetActive(false);
        }
        //  ClearSelection();
    }

    public void ClearSelection()
    {
        selectedUnits = new List<PlayerUnit>();
    }



    // called when we are creating a selection box
    void UpdateSelectionBox(Vector2 curMousePos)
    {

        Vector2 curMousePos2 = Mouse3D.MyInstance.MyMousePos;

        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        // the Mathf.abs makes a negative number into a positive number
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);





    }

    void ReleaseSelectionBox()
    {

        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        foreach (PlayerUnit playerUnit in GameManager.MyInstance.playerUnits)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(playerUnit.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                // selectedUnits.Add(playerUnit);
                AddUnitsToSelection(playerUnit);
                playerUnit.ToggleSelectionMarker(true);
                //  Debug.Log(playerUnit.name);
            }
        }

    }


    // if unit has died then remove from selection list
    public void RemoveNullUnitsFromSelection()
    {
        for (int x = 0; x < selectedUnits.Count; x++)
        {
            if (selectedUnits[x] == null)
                selectedUnits.RemoveAt(x);
        }
    }




    void ToggleSelectionVisual(bool selected)
    {
        foreach (PlayerUnit playerUnit in selectedUnits)
        {
            playerUnit.ToggleSelectionMarker(selected);
        }
    }


    void ToggleRangeVisual(bool selected)
    {
        foreach (PlayerUnit playerUnit in selectedUnits)
        {
          //  playerUnit.ToggleRangeMarker(selected);
        }
    }


    // returns whether or not we're selecting a unit or units
    public bool HasUnitSelected()
    {
        return selectedUnits.Count > 0 ? true : false;
    }

    //public bool HasSquadSelected()
    //{
    //    return selectedSquadUnits.Count > 0 ? true : false;
    //}
    // turns the list into an array and returns it
    public Unit[] GetSelectedUnits()
    {
        return selectedUnits.ToArray();
    }
    //public Unit[] GetSelectedSquadUnits()
    //{
    //    return selectedSquadUnits.ToArray();
    //}


}
