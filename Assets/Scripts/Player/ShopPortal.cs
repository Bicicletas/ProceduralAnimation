using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPortal : MonoBehaviour
{
    private GameObject shopPanel;
    [SerializeField] Transform parent;

    private Animator _portalAnimator;

    [SerializeField] float lifeTimeMinutes;
    public float currentTime = 0;

    bool timerIsOn = true;
    bool canUsePortal = true;

    private void Awake()
    {
        _portalAnimator = parent.gameObject.GetComponent<Animator>();

        currentTime = lifeTimeMinutes * 60;
    }

    private void Start()
    {
        shopPanel = ShopMenu.instance.shopPanel;
    }

    private void Update()
    {
        if (timerIsOn)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 5)
            {
                canUsePortal = false;

                this._portalAnimator.Play("Portal_destroy");

            }

            if (currentTime <= 0)
            {
                timerIsOn = false;
                Destroy(parent.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && canUsePortal)
        {
            PlayerController.canMove = false;
            Cursor.lockState = CursorLockMode.Confined;
            ShopMenu.instance.GetItemAmounts();
            shopPanel.SetActive(true);
        }
    }
}
