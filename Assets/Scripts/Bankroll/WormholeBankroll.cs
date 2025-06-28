using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;

public class WormholeBankroll : BankrollBase
{
    BuildingPlacer building;
    [SerializeField] private List<int> BankrollList = new();
    GameObject Bankroll = default;

    private MoneyManager _moneyManager;

    public override void OnBankrollEffect(GameObject ballObject)
    {
        throw new System.NotImplementedException();
    }


    // Start is called before the first frame update
    void Start()
    {
        _moneyManager = GameObject.FindAnyObjectByType<MoneyManager>();
        building = GetComponent<BuildingPlacer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

}

