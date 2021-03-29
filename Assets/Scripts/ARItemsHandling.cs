using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARItemsHandling : MonoBehaviour
{
    public class ConfigItem
    {
        public string name;
        public GameObject loadedPrefab;

        public ConfigItem(string name)
        {
            this.name = name;
            Debug.Log("Prefab Path: " + this.PrefabPath());
            this.loadedPrefab = Resources.Load<GameObject>(this.PrefabPath());
            Debug.Log("loadedPrefab: " + this.loadedPrefab);
        }

        public string DirPath()
        {
            return "Items/" + this.name;
        }

        public string SpritePath()
        {
            return this.DirPath() + "/" + this.name;
        }

        public string PrefabPath()
        {
            return this.DirPath() + "/" + this.name;
        }

    }

    public GameObject SelectedItem;
    public Joystick RotationJoystick;
    //public Joystick VerticalRotationJoystick;
    public Joystick MovementJoystick;
    public Button DeleteButton;
    public Button ValidateButton;
    public CameraHandler cameraHandler;
    private ARRaycastManager _arRaycastManager;

    private List<GameObject> Items = new List<GameObject>();
    private List<ConfigItem> ConfigItems = new List<ConfigItem>();

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public PrefabJSON prefabDescription;

    private bool onTouchHold = false;

    private Vector2 touchPosition;

    public List<ConfigItem> GetConfigItems()
    {
        return this.ConfigItems;
    }

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();

        ConfigItems.Add(new ConfigItem("chair_1"));
        ConfigItems.Add(new ConfigItem("bed_1"));
        ConfigItems.Add(new ConfigItem("torchere_1"));
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsAnItemSelected() && Input.GetMouseButtonDown(0))
        {
            Ray ray = cameraHandler.GetCamera().ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                this.SelectItem(hitInfo.transform.gameObject);
            }
        }
        if (IsAnItemSelected())
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = cameraHandler.GetCamera().ScreenPointToRay(touch.position);
                    RaycastHit hitObject;

                    if (Physics.Raycast(ray, out hitObject, 25.0f))
                    {
                        if (hitObject.transform.gameObject == SelectedItem)
                        {
                            onTouchHold = true;
                        }
                    }
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    touchPosition = touch.position;
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    onTouchHold = false;
                }

            }

            if (onTouchHold)
            {
                if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon)) {
                    Pose hitPose = hits[0].pose;
                    SelectedItem.transform.position = hitPose.position;

                }
            }
        }
    }

    public bool IsAnItemSelected()
    {
        return this.SelectedItem != null;
    }

    ModelBehaviour GetModelBehaviour(GameObject item)
    {
        return item.GetComponent<ModelBehaviour>();
    }

    public GameObject FindItem(GameObject itemToFind)
    {
        return this.Items.Find(item => GetModelBehaviour(item).GetId() == GetModelBehaviour(itemToFind).GetId());
    }

    public void ActivateSelectedInterface(bool val)
    {
        if (ARSession.state == ARSessionState.Unsupported && MovementJoystick)
        {
            MovementJoystick.gameObject.SetActive(val);
        }

        if (RotationJoystick && DeleteButton && ValidateButton)
        {
            RotationJoystick.gameObject.SetActive(val);
            DeleteButton.gameObject.SetActive(val);
            ValidateButton.gameObject.SetActive(val);
        }
    }

    public void SelectItem(GameObject item)
    {
        GameObject found = this.FindItem(item);
        if (found)
        {
            Debug.Log("Item Found: " + found.ToString());
            if (SelectedItem)
            {
                ModelBehaviour model = this.GetModelBehaviour(SelectedItem);
                if (model)
                {
                    model.SetSelected(false);
                    model.RotationJoystick = null;
                    model.MovementJoystick = null;
                }
            }
            ModelBehaviour foundModel = this.GetModelBehaviour(found);
            if (foundModel)
            {
                this.SelectedItem = found;
                foundModel.SetSelected(true);
                foundModel.RotationJoystick = RotationJoystick;
                if (ARSession.state == ARSessionState.Unsupported && MovementJoystick)
                {
                    foundModel.MovementJoystick = MovementJoystick;
                }
                this.ActivateSelectedInterface(true);
            }
        }
    }

    public void UnSelectItem()
    {
        GetModelBehaviour(this.SelectedItem).SetSelected(false);
        this.SelectedItem = null;
        this.ActivateSelectedInterface(false);
    }

    public void DeleteSelectedItem()
    {
        GameObject toRemove = this.SelectedItem;
        this.UnSelectItem();
        this.Items.Remove(toRemove);
        Destroy(toRemove);
    }

    public void AddItem(GameObject objectModel, Vector3 position, Quaternion rotation)
    {
        EditNewObject(objectModel);

        GameObject spawnedObject = Instantiate(objectModel, position, rotation);
        spawnedObject.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
        if (ARSession.state == ARSessionState.Unsupported)
        {
            spawnedObject.transform.localScale = spawnedObject.transform.localScale * 100;
        }
        ModelBehaviour modelBehaviour = spawnedObject.AddComponent<ModelBehaviour>() as ModelBehaviour;
        modelBehaviour.cameraHandler = cameraHandler;
        this.Items.Add(spawnedObject);
        this.SelectItem(spawnedObject);
    }

    public void SetPrefabJSON(PrefabJSON prefab)
    {
        this.prefabDescription = prefab;
        Debug.Log(prefab.title + " -> " + prefab.appearances[0].color);
    }

    public void EditNewObject(GameObject prefab)
    {
        int childs = prefab.transform.childCount;
        
        GameObject feature_to_edit;
        string feature_name;
        string valColor;
        string valTexture;
        Color myColor;
        Material myMaterial;
        string path_to_material;

        for (int i = 0; i < childs; i++)
        {
            Appearance appearance = prefabDescription.appearances[i];
            feature_name = appearance.name;
            valColor = appearance.color;
            valTexture = appearance.texture;

            path_to_material = "Materials/" + valTexture;
            myMaterial = (Material)Resources.Load(path_to_material);

            myColor = Color.clear;
            if (valColor == "default") { valColor = "#FFFFFF"; }
            ColorUtility.TryParseHtmlString(valColor, out myColor);

            feature_to_edit = prefab.transform.Find(feature_name).gameObject;
            feature_to_edit.GetComponent<MeshRenderer>().sharedMaterial = myMaterial;
            feature_to_edit.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color", myColor);
        }
    }
}
