using UnityEngine;
using UnityEngine.Events;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField, Min(0)] private int money;

    [System.Serializable]
    public class MoneyChangedEvent : UnityEvent<int> { }

    public MoneyChangedEvent onMoneyChanged;

    public int Money => money;

    public void AddMoney(int amount)
    {
        if (amount <= 0)
            return;

        money += amount;
        onMoneyChanged?.Invoke(money);
    }
}
