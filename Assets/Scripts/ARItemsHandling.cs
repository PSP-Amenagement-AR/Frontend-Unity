using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>Class for manage the selection and the emergence of items in the screen.</summary>
public class ARItemsHandling : MonoBehaviour
{
    /// <summary>Class for load template and prefab objetc. Part of ARItemsHandling class.</summary>
    /// @see ARItemsHandling
    public class ConfigItem
    {
        /// <summary>Name of the prefab.</summary>
        public string name;
        /// <summary>Prefab object.</summary>
        public GameObject loadedPrefab;

        /// Constructor for ConfigItem class.
        /// @param name The name of the prefab to load.
        public ConfigItem(string name)
        {
            this.name = name;
            Debug.Log("Prefab Path: " + this.PrefabPath());
            this.loadedPrefab = Resources.Load<GameObject>(this.PrefabPath());
            Debug.Log("loadedPrefab: " + this.loadedPrefab);
        }

        /// Complete the path to directory.
        /// @returns The string containing the path.
        public string DirPath()
        {
            return "Items/" + this.name;
        }

        /// Complete the path to sprite file.
        /// @returns The string containing the path.
        public string SpritePath()
        {
            return this.DirPath() + "/" + this.name;
        }

        /// Complete the path to prefab file.
        /// @returns The string containing the path.
        public string PrefabPath()
        {
            return this.DirPath() + "/" + this.name;
        }

    }

    /// <summary>GameObject object for item selected in the stage.</summary>
    public GameObject SelectedItem;
    /// <summary>Joystick object for the rotation joystick.</summary>
    public Joystick RotationJoystick;
    public Joystick MovementJoystick;
    /// <summary>Button object for the delete button.</summary>
    public Button DeleteButton;
    /// <summary>Button object for the validation button.</summary>
    public Button ValidateButton;
    /// <summary>CameraHandler object for the camera in the stage.</summary>
    public CameraHandler cameraHandler;
    /// <summary>ARRaycastManager object for the detector of area in the stage.</summary>
    private ARRaycastManager _arRaycastManager;

    /// <summary>List of GameObject for list the items in the stage.</summary>
    private List<GameObject> Items = new List<GameObject>();
    /// <summary>List of ConfigItems objects.</summary>
    /// @see ConfigItems
    private List<ConfigItem> ConfigItems = new List<ConfigItem>();

    /// <summary>List of ARRaycastHit object for resume points and placements where the user hit the screen.</summary>
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    /// <summary>PrefabJSON object containing the description of a virtual furniture.</summary>
    public PrefabJSON prefabDescription;

    /// <summary>Boolean for detect when the user hold the pression on the screen. It's allow to detect the "drag and drop" process.</summary>
    private bool onTouchHold = false;

    /// <summary>Vector2 object containing the contact informations of the position touched by the user.</summary>
    private Vector2 touchPosition;

    /// Get the list of ConfigItems objects.
    /// @returns The list of ConfigItems objects.
    public List<ConfigItem> GetConfigItems()
    {
        return this.ConfigItems;
    }

    /// Function executed when the script is started.
    /// Load the prefab files in ConfigItems object and set in a list.
    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();

        ConfigItems.Add(new ConfigItem("chair_1"));
        ConfigItems.Add(new ConfigItem("bed_1"));
        ConfigItems.Add(new ConfigItem("torchere_1"));
    }

    /// Function executed once per frame.*
    /// Detect the area and manage the hit on the screen.
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

    /// <summary>Detect if an item is selected</summary>
    /// @returns The item selected.
    /// @note If no items are selected, no items are returned from the function.
    public bool IsAnItemSelected()
    {
        return this.SelectedItem != null;
    }

    /// Function which allow to get an item as ModelBehaviour object.
    /// @param item A GameObject item.
    /// @returns ModelBehavious object from an item.
    ModelBehaviour GetModelBehaviour(GameObject item)
    {
        return item.GetComponent<ModelBehaviour>();
    }

    /// Function for find an item.
    /// @param itemToFind The item to find.
    /// @returns The GameObject containing the item found.
    public GameObject FindItem(GameObject itemToFind)
    {
        return this.Items.Find(item => GetModelBehaviour(item).GetId() == GetModelBehaviour(itemToFind).GetId());
    }

    /// Activation of the interfac for manage a selected item in the stage.
    /// @param val A boolean indicating that an item is selected.
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

    /// Allow to select an item in the stage.
    /// @param item A GameObject containing the item to select.
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

    /// Function for unselect an item.
    public void UnSelectItem()
    {
        GetModelBehaviour(this.SelectedItem).SetSelected(false);
        this.SelectedItem = null;
        this.ActivateSelectedInterface(false);
    }

    /// Function for delete the selected item.
    public void DeleteSelectedItem()
    {
        GameObject toRemove = this.SelectedItem;
        this.UnSelectItem();
        this.Items.Remove(toRemove);
        Destroy(toRemove);
    }

    /// Function for add an item in the stage.
    /// @param objectModel The prefab of the object to add.
    /// @param position Vector3 object containing the position where the item will be added.
    /// @param rotation Quaternion object containing the rotation values of the item.
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

    /// Set the description of an item from a PrefabJSON object.
    /// @param prefab A PrefabJSON object containing the description of an item created.
    /// @see PrefabJSON
    public void SetPrefabJSON(PrefabJSON prefab)
    {
        this.prefabDescription = prefab;
        Debug.Log(prefab.title + " -> " + prefab.appearances[0].color);
    }

    /// Function allowing to edit a new item created in the stage.
    /// @param prefab GameObject containing the prefab to add in a new object.
    /// @note Load of the textures and colors for new item edition.
    /// @note If the color is set on "default" so the object will not have color but only the texture appearance.
    /// @attention The prefab customized is the same for all other items using this prefab. The prefab is overwrite whenever a new object is added to the scene.
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
