using UnityEngine;

public class MoneyBudBankroll : MoneyGainBankrollBase
{
    [Header("お金をもらうための回数")]
    [SerializeField] private int _moneyGetCount = 10;
    [Header("もらうお金")]
    [SerializeField] private int _getMoney = 1000;
    [Header("木に成長するまでのレベル")]
    [SerializeField] private int _growthLevelCount = 10;
    [Header("お金がなる木バンクロール")]
    [SerializeField] private GameObject _moneyTreeBankroll;
    private int _hitCount = 0;
    private int _growthLevel = 0;
    private BuildingPlacer _buildingPlacer;
    public override void OnBankrollHit(GameObject ballObject)
    {
        _hitCount++;
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
