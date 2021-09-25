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
        private GameObject newImage;
        public Sprite border;

        // Start is called before the first frame update

        private void Awake()
        {
            
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
            Debug.Log("Updatin Hub");
            foreach(GameObject n in gunsList)
            {
                GameObject.Destroy(n);
            }
            gunsList.Clear(); 

            //player.guns[0].GetComponentInChildren<SpriteRenderer>().sprite
            //image.overrideSprite = player.guns[0].GetComponentInChildren<SpriteRenderer>().sprite;
           
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
    }
}