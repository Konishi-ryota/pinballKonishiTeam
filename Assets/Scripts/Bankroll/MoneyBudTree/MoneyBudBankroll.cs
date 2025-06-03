using UnityEngine;

public class MoneyBudBankroll : MoneyGainBankrollBase
{
    [Header("���������炤���߂̉�")]
    [SerializeField] public int _moneyGetCount = 10;
    [Header("���炤����")]
    [SerializeField] public int _getMoney = 1000;
    [Header("�؂ɐ�������܂ł̃��x��")]
    [SerializeField] public int _growthLevelCount = 10;
    [Header("�������Ȃ�؃o���N���[��")]
    [SerializeField] public GameObject _moneyTreeBankroll;
    private int _hitCount = 0;
    private int _growthLevel = 0;
    private BuildingPlacer _buildingPlacer;
    public override void OnBankrollHit(GameObject ballObject)
    {
        _hitCount++;
        Debug.Log(_hitCount);
        if (_hitCount >= _moneyGetCount)
        {
            GainMoney(_getMoney);
            _growthLevel++;
            _hitCount = 0;

            if (_growthLevel >= _growthLevelCount)
            {
                GrowingTree();
            }
        }

       

    }

    private void GrowingTree()
    {
        Debug.Log("����");
        if (_moneyTreeBankroll == null)
        {
            Debug.LogError("_moneyTreeBankroll Null");
            return;
        }
        GameObject newBankroll = Instantiate(_moneyTreeBankroll);
        newBankroll.transform.position = this.transform.position;
        newBankroll.transform.rotation = this.transform.rotation;
        _buildingPlacer = FindAnyObjectByType<BuildingPlacer>();
        newBankroll.transform.parent = _buildingPlacer.GetBankrollParent().transform;
        Destroy(this.gameObject);
    }



}
