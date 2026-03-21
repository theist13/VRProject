using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    public void OnUpdateMoney(int money)
    {
        moneyText.text = $"{money} $";
    }
}
