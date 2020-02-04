using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TetrisHandler : MonoBehaviour
{


    public bool isPlacing = false;
    public GameObject tetrisObj;
    private static TetrisHandler _instance;
    private bool isValidPlacement;

    public static TetrisHandler Instance { get { return _instance; } }

    public List<GameObject> airList = new List<GameObject>();
    public List<GameObject> fuelList = new List<GameObject>();
    public List<GameObject> metalList = new List<GameObject>();

    public GameObject airObject;
    public GameObject fuelObject;
    public GameObject metalObject;

    public List<TetrisPiece.Square> squares = new List<TetrisPiece.Square>();
    public List<BaseCard.resourceCache> resources = new List<BaseCard.resourceCache>();

    public Dictionary<TetrisPiece.PieceType, List<TetrisPiece.Square>> pieceDictionary = new Dictionary<TetrisPiece.PieceType, List<TetrisPiece.Square>>();
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        pieceDictionary.Add(TetrisPiece.PieceType.Dot, new List<TetrisPiece.Square>()
        {
            new TetrisPiece.Square() { xOffset = 0, yOffset = 0 }
        }
        );

        pieceDictionary.Add(TetrisPiece.PieceType.I, new List<TetrisPiece.Square>()
        {
            new TetrisPiece.Square() { xOffset = 0, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 0, yOffset = 1 },
            new TetrisPiece.Square() { xOffset = 0, yOffset = 2 },
            new TetrisPiece.Square() { xOffset = 0, yOffset = 3 },
        }
        );

        pieceDictionary.Add(TetrisPiece.PieceType.S, new List<TetrisPiece.Square>()
        {
            new TetrisPiece.Square() { xOffset = 0, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 1, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 1, yOffset = 1 },
            new TetrisPiece.Square() { xOffset = 2, yOffset = 1 },
        }
);

        pieceDictionary.Add(TetrisPiece.PieceType.Z, new List<TetrisPiece.Square>()
        {
            new TetrisPiece.Square() { xOffset = 0, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 1, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 1, yOffset = -1 },
            new TetrisPiece.Square() { xOffset = 2, yOffset = -1 },
        }
);
        pieceDictionary.Add(TetrisPiece.PieceType.O, new List<TetrisPiece.Square>()
        {
            new TetrisPiece.Square() { xOffset = 0, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 1, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 1, yOffset = 1 },
            new TetrisPiece.Square() { xOffset = 0, yOffset = 1 },
        }
);

        pieceDictionary.Add(TetrisPiece.PieceType.T, new List<TetrisPiece.Square>()
        {
            new TetrisPiece.Square() { xOffset = 0, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 1, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 2, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 1, yOffset = -1 },
        }
);
        pieceDictionary.Add(TetrisPiece.PieceType.L, new List<TetrisPiece.Square>()
        {
            new TetrisPiece.Square() { xOffset = 0, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 1, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 2, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 0, yOffset = -1 },
        }
);
        pieceDictionary.Add(TetrisPiece.PieceType.J, new List<TetrisPiece.Square>()
        {
            new TetrisPiece.Square() { xOffset = 0, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 1, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 2, yOffset = 0 },
            new TetrisPiece.Square() { xOffset = 2, yOffset = -1 },
        }
);

    }


    public void TakeDamage(TetrisPiece.ResourceType resourceType)
    {
        switch (resourceType)
        {

            case TetrisPiece.ResourceType.Air:
                {
                    {
                        if (TetrisHandler.Instance.airList.Count > 0)
                        {
                            int index = Random.Range(0, TetrisHandler.Instance.airList.Count);
                            GameObject temp = TetrisHandler.Instance.airList[index];
                            TetrisHandler.Instance.airList.RemoveAt(index);
                            Destroy(temp);
                        }
                        else if (TetrisHandler.Instance.airObject.transform.childCount > 0)
                        {
                            List<Transform> childs = TetrisHandler.Instance.airObject.GetComponentsInChildren<Transform>().ToList();
                            childs.Remove(TetrisHandler.Instance.airObject.transform);

                            List<Transform> truechilds = new List<Transform>();
                            foreach (Transform child in childs)
                            {
                                if (child.childCount < 1 && child.GetComponent<GridCube>().isDestroyed == false)
                                {
                                    truechilds.Add(child);
                                }
                            }

                            if(truechilds.Count < 1)
                            {
                                Debug.Log("YOU LOSE");
                                return;
                            }
                            
                            GameObject randomObject = (GameObject)((Transform)truechilds[Random.Range(0, truechilds.Count)]).gameObject;

                            randomObject.GetComponent<GridCube>().isDestroyed = true;
                        }
                    }
                }
                break;
            case TetrisPiece.ResourceType.Metal:
                {

                    {
                        if (TetrisHandler.Instance.metalList.Count > 0)
                        {
                            int index = Random.Range(0, TetrisHandler.Instance.metalList.Count);
                            GameObject temp = TetrisHandler.Instance.metalList[index];
                            TetrisHandler.Instance.metalList.RemoveAt(index);
                            Destroy(temp);
                        }
                        else if (TetrisHandler.Instance.metalObject.transform.childCount > 0)
                        {
                            List<Transform> childs = TetrisHandler.Instance.metalObject.GetComponentsInChildren<Transform>().ToList();
                            childs.Remove(TetrisHandler.Instance.metalObject.transform);

                            List<Transform> truechilds = new List<Transform>();
                            foreach (Transform child in childs)
                            {
                                if (child.childCount < 1 && child.GetComponent<GridCube>().isDestroyed == false)
                                {
                                    truechilds.Add(child);
                                }
                            }
                            if (truechilds.Count < 1)
                            {
                                Debug.Log("YOU LOSE");
                                return;
                            }

                            GameObject randomObject = (GameObject)((Transform)truechilds[Random.Range(0, truechilds.Count)]).gameObject;

                            randomObject.GetComponent<GridCube>().isDestroyed = true;
                        }
                    }
                }
                break;
            case TetrisPiece.ResourceType.Fuel:
                {

                    {
                        if (TetrisHandler.Instance.fuelList.Count > 0)
                        {
                            int index = Random.Range(0, TetrisHandler.Instance.fuelList.Count);
                            GameObject temp = TetrisHandler.Instance.fuelList[index];
                            TetrisHandler.Instance.fuelList.RemoveAt(index);
                            Destroy(temp);
                        }
                        else if (TetrisHandler.Instance.fuelObject.transform.childCount > 0)
                        {
                            List<Transform> childs = TetrisHandler.Instance.fuelObject.GetComponentsInChildren<Transform>().ToList();
                            childs.Remove(TetrisHandler.Instance.fuelObject.transform);

                            List<Transform> truechilds = new List<Transform>();
                            foreach (Transform child in childs)
                            {
                                if (child.childCount < 1 && child.GetComponent<GridCube>().isDestroyed == false)
                                {
                                    truechilds.Add(child);
                                }
                            }

                            if (truechilds.Count < 1)
                            {
                                Debug.Log("YOU LOSE");
                                return;
                            }

                            GameObject randomObject = (GameObject)((Transform)truechilds[Random.Range(0, truechilds.Count)]).gameObject;

                            randomObject.GetComponent<GridCube>().isDestroyed = true;
                        }
                    }
                }
                break;
            case TetrisPiece.ResourceType.Flexible:
                break;
        }
    }

    public void TrashCurrent()
    {
        if (resources.Count > 0)
        {
            Destroy(resources[0].gameObject);
            isPlacing = false;
            resources.Remove(resources[0]);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (resources.Count > 0 && !isPlacing)
        {
            isPlacing = true;
            tetrisObj = resources[0].gameObject;
            squares = resources[0].squares;

            for (int i = 0; i < squares.Count; i++)
            {
                tetrisObj.transform.GetChild(i).GetComponent<Renderer>().material.SetFloat("_Type", (int)squares[i].resourceType);
            }

        }

        if (isPlacing)
        {
            tetrisObj.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tetrisObj.transform.Translate(new Vector3(0, 0, 189));

            //Rotate piece
            if (Input.GetKeyDown(KeyCode.E))
            {
                tetrisObj.transform.Rotate(new Vector3(0, 0, -90));
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                tetrisObj.transform.Rotate(new Vector3(0, 0, 90));
            }

            if (Input.GetMouseButtonDown(0))
            {
                isValidPlacement = false;

                for (int i = 0; i < tetrisObj.transform.childCount; i++)
                {
                    TetrisPiece.ResourceType CubeType = squares[i].resourceType;

                    //raycast backwards to see if there is a grid square behind

                    Transform workingTransform = tetrisObj.transform.GetChild(i);

                    if (Physics.Raycast(workingTransform.position, new Vector3(0, 0, 1), out RaycastHit hit))
                    {

                        if (hit.collider.transform.parent.GetComponent<GridPiece>() && ((hit.collider.transform.gameObject.GetComponent<GridCube>().isDestroyed == false) || ( hit.collider.transform.gameObject.GetComponent<GridCube>().isDestroyed == true && tetrisObj.GetComponent<TetrisParent>().isRepair)))
                        {
                            TetrisPiece.ResourceType GridType = hit.collider.transform.parent.GetComponent<GridPiece>().resourceType;

                            //if gridtype == resourcetype, place.
                            //if gridtype != resourcetype, but grid space contains something, destroy both.
                            //if gridtype != resourcetype and grid is empty, destroy cube
                            //if none of the cubes are the right type, do not allow placement.

                            if (GridType == CubeType || CubeType == TetrisPiece.ResourceType.Flexible)
                            {
                                isValidPlacement = true;
                            }
                        }
                        else if (hit.collider.transform.GetComponent<TetrisTag>())
                        {
                            if (hit.collider.transform.GetComponent<TetrisTag>().ResourceType == CubeType)
                            {
                                isValidPlacement = true;
                            }
                        }
                    }
                    else
                    {
                        isValidPlacement = false;
                        return;
                    }
                }

                if (isValidPlacement)
                {
                    for (int i = 0; i < tetrisObj.transform.childCount; i++)
                    {
                        //raycast backwards to see if there is a grid square behind

                        Transform workingTransform = tetrisObj.transform.GetChild(i);

                        if (Physics.Raycast(workingTransform.position, new Vector3(0, 0, 1), out RaycastHit hit))
                        {
                            TetrisPiece.ResourceType CubeType = squares[i].resourceType;

                            if (hit.collider.transform.parent.GetComponent<GridPiece>())
                            {
                                TetrisPiece.ResourceType GridType = hit.collider.transform.parent.GetComponent<GridPiece>().resourceType;

                                //if gridtype == resourcetype, place.
                                if (GridType == CubeType || CubeType == TetrisPiece.ResourceType.Flexible)
                                {
                                    if (!hit.collider.transform.parent.GetComponent<GridPiece>())
                                    {
                                        Destroy(workingTransform.transform.gameObject);
                                    }
                                    else
                                    {
                                        if(tetrisObj.gameObject.GetComponent<TetrisParent>().isRepair)
                                        {
                                            Destroy(workingTransform.gameObject);

                                            Debug.Log("Setting is destroyed false");

                                            hit.collider.transform.gameObject.GetComponent<GridCube>().isDestroyed = false;
                                        }
                                        else
                                        {
                                            workingTransform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z - 1f);

                                            if (CubeType == TetrisPiece.ResourceType.Flexible)
                                            {
                                                squares[i].resourceType = GridType;
                                                workingTransform.GetComponent<Renderer>().material.SetFloat("_Type", (int)squares[i].resourceType);

                                            }

                                            if (GridType == TetrisPiece.ResourceType.Air)
                                            {
                                                airList.Add(workingTransform.gameObject);
                                            }
                                            else if (GridType == TetrisPiece.ResourceType.Fuel)
                                            {
                                                fuelList.Add(workingTransform.gameObject);

                                            }
                                            else if (GridType == TetrisPiece.ResourceType.Metal)
                                            {
                                                metalList.Add(workingTransform.gameObject);
                                            }
                                        }
                                    }
                                    //place it
                                }
                                else
                                {
                                    Destroy(workingTransform.gameObject);
                                }
                            }
                            else
                            {
                                Debug.Log("Hit resource for destruction");
                                Debug.Log(hit.collider.transform);

                                //if gridtype != resourcetype, but grid space contains something, destroy both.
                                if (!hit.collider.transform.parent.GetComponent<GridPiece>())
                                {
                                    Destroy(workingTransform.gameObject);
                                    if (hit.collider.GetComponent<TetrisTag>().ResourceType != CubeType)
                                    {


                                        if (airList.Contains(hit.collider.transform.gameObject))
                                        {
                                            airList.Remove(hit.collider.transform.gameObject);
                                        }
                                        else if (fuelList.Contains(hit.collider.transform.gameObject))
                                        {
                                            fuelList.Remove(hit.collider.transform.gameObject);
                                        }
                                        else if (metalList.Contains(hit.collider.transform.gameObject))
                                        {
                                            metalList.Remove(hit.collider.transform.gameObject);
                                        }
                                    }
                                    else // hit somethign with same resource type.
                                    {
                                        workingTransform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z - 1f);
                                    }
                                }
                                //if gridtype != resourcetype and grid is empty, destroy cube
                                else if (hit.collider.transform.parent.GetComponent<GridPiece>())
                                {
                                    Destroy(workingTransform.gameObject);
                                }
                            };
                        }
                    }

                    isPlacing = false;
                    if (resources.Count > 0)
                    {
                        resources.Remove(resources[0]);
                    }
                }

            }

        }
    }
}