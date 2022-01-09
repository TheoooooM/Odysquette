using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetStrawUI : MonoBehaviour {
    [Header("--- POSSIBLE STRAW")] 
    [SerializeField] private List<StrawSO> possibleStraw = new List<StrawSO>();
    [SerializeField] private List<string> strawDescList = new List<string>();
    [SerializeField] private List<Sprite> strawSprite = new List<Sprite>();
    [SerializeField] private GameObject shopCostItem = null;
    [SerializeField] private GameObject strawCanvasPrefab = null;
    [SerializeField] private GameObject transitionArrow = null;
    [SerializeField] private GameObject PressEGam = null;
    
    [Header("--- UI OBJECT")]
    [SerializeField] private TextMeshProUGUI strawName = null;
    [SerializeField] private TextMeshProUGUI strawDesc = null;
    [SerializeField] private SpriteRenderer strawSpriteRenderer = null;
    [SerializeField] private Items strawParentItems = null;
    [Space]
    [SerializeField] private Image damageValueImg = null;
    [SerializeField] private Image fireRateValueImg = null;
    [SerializeField] private List<Image> numberOfBulletValueImg = new List<Image>();


    private float maxDamage = 0;
    private float maxFireRate = 0;
    private int maxNumberOfBullet = 0;
    private GameObject actualStrawData = null;
    
    /// <summary>
    /// Set the data of the straw
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="fireRate"></param>
    /// <param name="numberBullet"></param>
    public void setData(GameManager.Straw straw, bool isShop, bool showSprite, bool showArrow = false) {
        GetMaxValue();
        
        strawName.text = straw.ToString() + " Straw";
        strawDesc.text = strawDescList[(int) straw];
        if (showSprite) {
            strawSpriteRenderer.sprite = strawSprite[(int) straw];
            strawParentItems.SetNewStraw(straw);
        }

        float damage = possibleStraw[(int) straw].damage;
        float fireRate = possibleStraw[(int) straw].timeValue;

        int numberOfBullet = straw switch {
            GameManager.Straw.basic => 1,
            GameManager.Straw.bubble => 4,
            GameManager.Straw.sniper => 1,
            GameManager.Straw.helix => 2,
            GameManager.Straw.tri => 3,
            GameManager.Straw.riffle => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(straw), straw, null)
        };


        damageValueImg.fillAmount = damage / maxDamage;
        fireRateValueImg.fillAmount = fireRate / maxFireRate;
        for (int i = 0; i < numberOfBulletValueImg.Count; i++) {
            numberOfBulletValueImg[i].enabled = numberOfBullet >= i + 1;
        }
        
        shopCostItem.SetActive(isShop);
        transitionArrow.SetActive(showArrow);
        PressEGam.SetActive(!showArrow);
    }

    /// <summary>
    /// Show the actual data of your straw
    /// </summary>
    public void ShowActualStrawData() {
        if(actualStrawData != null) Destroy(actualStrawData);
        
        actualStrawData = Instantiate(strawCanvasPrefab, transform.position - new Vector3(10, 0, 0), Quaternion.identity);
        actualStrawData.GetComponent<SetStrawUI>().setData(GameManager.Instance.actualStraw, false, false, true);
    }

    /// <summary>
    /// Destroy the actual Straw Data
    /// </summary>
    public void DestroyActualStrawData() {
        if(actualStrawData != null) Destroy(actualStrawData);

    }
    
    /// <summary>
    /// Get the max possible value
    /// </summary>
    private void GetMaxValue() {
        foreach (StrawSO straw in possibleStraw) {
            if (straw.damage > maxDamage) maxDamage = straw.damage;
            if (straw.timeValue > maxFireRate) maxFireRate = straw.timeValue;
            maxNumberOfBullet = 4;
        }
    }
}
