using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BankrollButton : MonoBehaviour
{
    [SerializeField] private string _bankrollName;
    public string BankrollName => _bankrollName;
    [SerializeField] private Text _bankrollNameText;
    private int _bankrollMoney;
    public int BankrollMoney => _bankrollMoney;
    [SerializeField] private Text _bankrollMoneyText;
    [SerializeField] private int _bankrollCount;
    public int BankrollCount => _bankrollCount;
    [SerializeField] private Text _bankrollCountText;
    [SerializeField] private Image _bankrollIconImage;
    [SerializeField] private Sprite _bankrollIconSprite;
    [SerializeField] private GameObject _bankrollPrefab;
    public GameObject BankrollPrefab => _bankrollPrefab;
    [SerializeField] private GameObject _bankrollPreviewPrefab;
    public GameObject BankrollPreviewPrefab => _bankrollPreviewPrefab;
    [SerializeField] private EventTrigger _bankrollButtonEvent;

    private BuildingPlacer _buildingPlace;
    // Start is called before the first frame update
    private void Start()
    {
        SetButton();
        _buildingPlace = GameObject.FindObjectOfType<BuildingPlacer>();
        if (_buildingPlace != null)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(_ => _buildingPlace.SetBankroll(gameObject.GetComponent<BankrollButton>()));
            _bankrollButtonEvent.triggers.Add(entry);
        }
    }

    /// <summary>
    /// バンクロールボタンの設定
    /// </summary>
    public void SetButton()
    {
        _bankrollNameText.text = _bankrollName;
        BankrollBase bankrollBase = null;
        if (_bankrollPrefab != null)
        {
            bankrollBase = _bankrollPrefab.GetComponent<BankrollBase>();
        }
        
        if (bankrollBase != null)
        {
            _bankrollMoney = bankrollBase.BuildCost;
            _bankrollMoneyText.text = _bankrollMoney.ToString();
        }
        _bankrollCountText.text = _bankrollCount.ToString();
        _bankrollIconImage.sprite = _bankrollIconSprite;
    }

    /// <summary>
    /// バンクロールの数を減らす
    /// </summary>
    public void DecreaseCount()
    {
        _bankrollCount--;
        _bankrollCountText.text = _bankrollCount.ToString();
    }

    private void OnValidate()
    {
        SetButton();
    }
}
