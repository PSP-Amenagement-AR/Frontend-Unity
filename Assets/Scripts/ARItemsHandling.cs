using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

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

    private List<GameObject> Items = new List<GameObject>();
    private List<ConfigItem> ConfigItems = new List<ConfigItem>();

    public List<ConfigItem> GetConfigItems()
    {
        return this.ConfigItems;
    }

    private void Awake()
    {
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
        if (MovementJoystick && RotationJoystick && DeleteButton && ValidateButton)
        {
            //MovementJoystick.gameObject.SetActive(val);
            RotationJoystick.gameObject.SetActive(val);
            MovementJoystick.gameObject.SetActive(val);
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
                foundModel.MovementJoystick = MovementJoystick;
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
        GameObject spawnedObject = Instantiate(objectModel, position, rotation);
        spawnedObject.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
        if (ARSession.state == ARSessionState.Unsupported)
        {
            spawnedObject.transform.localScale = spawnedObject.transform.localScale * 100;
        }
        _ = spawnedObject.AddComponent<ModelBehaviour>() as ModelBehaviour;
        this.Items.Add(spawnedObject);
        this.SelectItem(spawnedObject);
    }
}
