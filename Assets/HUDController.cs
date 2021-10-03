using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Mechanics
{
    public class HUDController : MonoBehaviour
    {
        private Image image;
        private List<GameObject> gunsList = new List<GameObject>();
        public GameObject imagePrefab;
        
        // TODO(grantcarlson): make this private.
        public GameObject ammoText;
        public GameObject staminaText;
        private GameObject newImage;
        public Image staminaBar, ammoBar;
        public GameObject timerText;
        public Sprite border;

        // Start is called before the first frame update

        private void Awake()
        {
            ammoText = GameObject.Find("AmmoText");
        }

        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateHUD(PlayerController player)
        {
            foreach(GameObject n in gunsList)
            {
                GameObject.Destroy(n);
            }
            gunsList.Clear(); 
           
            for(int i = 0; i<player.guns.Length; i++) {

                GameObject newImage = Instantiate(imagePrefab, (Vector2)transform.position, Quaternion.identity, transform);
                RectTransform rt = newImage.GetComponent<RectTransform>();
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 5, 75);
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -15 + (50 * i), 75);
                
                newImage.GetComponent<Image>().overrideSprite = player.guns[i].GetComponentInChildren<SpriteRenderer>().sprite;

                gunsList.Add(newImage);

            }

            GameObject newBorder = Instantiate(imagePrefab, (Vector2)transform.position, Quaternion.identity, transform);
            RectTransform rtb = newBorder.GetComponent<RectTransform>();
            rtb.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 5, 110);
            rtb.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -40 + (50 * player.gunIndex), 120);
            newBorder.GetComponent<Image>().overrideSprite = border;
            gunsList.Add(newBorder);
        }

        public void UpdateAmmo(int magazine, int magazineCap)
        {
            if (magazine == 0)
            {
                ammoText.GetComponent<Text>().color = Color.red;
            } else
            {
                ammoText.GetComponent<Text>().color = new Color32(255, 0, 255, 200);
            }
            ammoBar.fillAmount = (magazine + 0.0f) / magazineCap;

            ammoText.GetComponent<Text>().text = magazine + "/" + magazineCap;
        }

        public void UpdateStamina(int magazine, int magazineCap)
        {
            if (magazine == 0)
            {
                staminaText.GetComponent<Text>().color = Color.red;
            }
            else
            {
                staminaText.GetComponent<Text>().color = new Color32(255, 0, 255, 200);
            }
            staminaBar.fillAmount = (magazine + 0.0f) / magazineCap;
            staminaText.GetComponent<Text>().text = magazine + "/" + magazineCap;
        }

        public void UpdateTimer(float secsLeft)
        {
            timerText.GetComponent<Text>().color = new Color32(255, 0, 255, 200);
            timerText.GetComponent<Text>().text = ("" + (int) secsLeft);
        }
    }
}
