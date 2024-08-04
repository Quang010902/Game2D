using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }
    [SerializeField] Text textCoin;
    public void SetCoin(int coin)
    {
        textCoin.text = coin.ToString();
    }
}
