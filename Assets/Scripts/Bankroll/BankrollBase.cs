using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class BankrollBase : MonoBehaviour
{
    [Header("設置時のコスト")]
    [SerializeField] protected int _buildCost;

    [Header("１度あたると消えるか")] 
    [SerializeField] protected bool _isOneTimeOnly;

    [Header("デフォルトのバンクロール")] 
    [SerializeField] protected GameObject _defultBankrollPrefab;
    [Header("バンクロールの親オブジェクトの名前")] 
    [SerializeField] private string _bankrollParentName = "BankrollParent";
    [Header("バンクロールの親オブジェクトの名前")] 
    [SerializeField] private string _defultBankrollPrefabName = "DefultBankroll";
    
    public int BuildCost => _buildCost;
    protected Rigidbody _ballRigidbody;
    protected Vector3 _hitPos;
    protected GameObject _bankrollParent;


    private void Awake()
    {
        FindBankrollParentObject();
    }
    
    protected void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            _ballRigidbody = other.rigidbody;
            _hitPos = other.contacts[0].normal;
            SoundManager.Instance.PlaySE(SESoundData.SE.Hit); //音を鳴らす処理
            OnBankrollEffect(other.gameObject);
            
            //１度当たったらデフォルトのバンクロールに変更
            if (_isOneTimeOnly)
            {
                Instantiate(_defultBankrollPrefab, this.transform.position, this.transform.rotation, _bankrollParent.transform);
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// バンクロールにボールが当たった時に呼び出したい処理
    /// </summary>
    public abstract void OnBankrollEffect(GameObject ballObject);
    
    /// <summary>
    /// バンクロールの親オブジェクトを設定する関数
    /// </summary>
    private void FindBankrollParentObject()
    {
        _bankrollParent = GameObject.Find(_bankrollParentName);
        if (_bankrollParent == null)
        {
            Debug.LogWarning($"[Scene] GameObject '{_bankrollParentName}' not found!");
        }
    }

    
    /// <summary>
    /// デフォルトのバンクロールを設定する関数
    /// </summary>
    public void FindDefultBankrollPrefab()
    {
#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets($"t:GameObject {_defultBankrollPrefabName}");

        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            _defultBankrollPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Debug.Log($"[Assets] Found Prefab: {_defultBankrollPrefab.name} at {path}");
        }
        else
        {
            Debug.LogWarning($"[Assets] Prefab '{_defultBankrollPrefabName}' not found!");
        }
#endif
    }
}
