using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    [Header("バンクロールの親オブジェクト")] 
    [SerializeField] private GameObject _bankrollParent;
    [Header("検索するオブジェクト範囲")] 
    [SerializeField] private float _checkRadius;

    [Header("デフォルトのバンクロール")] 
    [SerializeField] private GameObject _defultBankrollPrefab;
    private BankrollButton _bankrollButton;
    private GameObject _bankrollPrefab; //バンクロールのプレハブ
    private GameObject _bankrollPreview; //プレビュー用
    private bool _isPlacing;
    private Transform _bankrollTransform;
    private PlungerTrigger _plungerTrigger;
    private WarningUI _warningUI;
    private MoneyManager _moneyManager;


    private void Awake()
    {
        _plungerTrigger = FindObjectOfType<PlungerTrigger>();
        _warningUI = FindObjectOfType<WarningUI>();
        _moneyManager = FindObjectOfType<MoneyManager>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !_isPlacing)
        {
            DeleteBankroll();
        }
        
        
        if (_isPlacing && _bankrollPreview != null)
        {
            Vector3? placePosition = GetPlacementPosition();
            if (placePosition.HasValue)
            {
                _bankrollPreview.transform.position = placePosition.Value;
            }
        }
        
        // マウスを離したときに建設を確定
        if (Input.GetMouseButtonUp(0) && _isPlacing)
        {
            if (_plungerTrigger.IsBall) //ボールがスタート地点にいるかを判定
            {
                BankrollBase bankrollBase = _bankrollPrefab.GetComponent<BankrollBase>();
                int buildCost = bankrollBase.BuildCost;
                if (CheckBuildBankroll(buildCost))
                {
                    Vector3? placePosition = GetPlacementPosition();
                    GetBankrollPosition();
                    if (placePosition.HasValue && _bankrollTransform != null)
                    {
                        GameObject gameObject = Instantiate(_bankrollPrefab, _bankrollTransform.position, _bankrollTransform.rotation, _bankrollParent.transform);
                        //レイヤーの変更
                        gameObject.layer = 0;
                        Destroy(_bankrollTransform.gameObject);
                        //お金を減らす処理
                        UseMoney(buildCost);
                        //設置できる数を減らす処理
                        _bankrollButton.DecreaseCount();
                    }
                }
                else
                {
                    _warningUI.ShowWarningUI("お金が足りません");
                }
                
            }
            else
            {
                _warningUI.ShowWarningUI("ボールがスタート地点にありません");
            }
            
            Destroy(_bankrollPreview);
            _isPlacing = false;
        }
    }

    /// <summary>
    /// 設置するバンクロールの情報を受け取る処理
    /// </summary>
    /// <param name="bankrollButton"></param>
    public void SetBankroll(BankrollButton bankrollButton)
    {
        if (bankrollButton != null)
        {
            if (bankrollButton.BankrollCount > 0)
            {
                _bankrollButton = bankrollButton;
                StartPlacingBankroll();
            }
            else
            {
                _warningUI.ShowWarningUI("バンクロールを所持してません");
            }
        }
        
        Debug.Log("設置開始" + _bankrollButton.BankrollName);
    }

    //バンクロールの設置開始時に呼ばれる処理
    private void StartPlacingBankroll()
    {
        _isPlacing = true;
        _bankrollPreview = Instantiate(_bankrollButton.BankrollPreviewPrefab);
        _bankrollPrefab = _bankrollButton.BankrollPrefab;

    }
    
    //設置できるかを判定
    private Vector3? GetPlacementPosition()
    {
        // UIをクリックしている場合は無視
        if (EventSystem.current.IsPointerOverGameObject()) return null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return null;
    }

    //設置できる位置であるかの判定
    private void GetBankrollPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 hitPoint = hit.point;
            // 一定範囲内のオブジェクトを取得し、距離順にソート
            Collider[] colliders = Physics.OverlapSphere(hitPoint, _checkRadius);
            var sortedColliders = colliders
                .OrderBy(collider => Vector3.Distance(hitPoint, collider.transform.position))
                .ToArray();

            _bankrollTransform = null;
            foreach (var collider in sortedColliders)
            {
                if (collider.CompareTag("DefultBankroll"))
                {
                    _bankrollTransform = collider.gameObject.transform;
                    break;
                }
            }
        }
    }

    //バンクロールの撤去処理
    private void DeleteBankroll()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 hitPoint = hit.point;
            // 一定範囲内のオブジェクトを取得し、距離順にソート
            Collider[] colliders = Physics.OverlapSphere(hitPoint, _checkRadius);
            var sortedColliders = colliders
                .OrderBy(collider => Vector3.Distance(hitPoint, collider.transform.position))
                .ToArray();
            
            foreach (var collider in sortedColliders)
            {
                if (collider.CompareTag("Bankroll"))
                {
                    GameObject hitObject = collider.gameObject;
                    Instantiate(_defultBankrollPrefab, hitObject.transform.position, hitObject.transform.rotation, _bankrollParent.transform);
                    Destroy(hitObject);
                    break;
                }
            }
        }
    }
    
    // 設置時に呼ばれる処理
    public bool CheckBuildBankroll(int buildCost)
    {
        int currentMoney = _moneyManager.Money;
        if (currentMoney - buildCost >= 0)
        {
            return true; 
        }
        else
        {
            return false;
        }
    }
    
    // お金を減らす処理
    public void UseMoney(int buildCost)
    {
        _moneyManager.DecreaseMoney(buildCost);
    }
}
