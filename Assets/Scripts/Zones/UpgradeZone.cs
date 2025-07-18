using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeZone : MonoBehaviour
{
    [Header("Tick Settings")]
    [SerializeField] float percentagePerTick;
    [SerializeField] float tickDelay;
    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI zoneTxt;

    [Header("Cost Settings")]
    [SerializeField] int stackSlotsPerLevel;
    [SerializeField] int baseCostperTick = 1;
    [SerializeField] float costMultiplierPerLevel;

    public bool isBuying = false;
    public int costPerTick;
    public int currentLevel = 1;
    [SerializeField] float currentProgress;
    Coroutine routine;

    private void Start()
    {
        costPerTick = baseCostperTick;
        zoneTxt.text = $"Subir\n nível \n $ {costPerTick} ";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isBuying)
        {
            isBuying = true;
            routine = StartCoroutine(BuyPercentagePerTickRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") 
        {
            isBuying = false;
            StopCoroutine(routine);
        }
    }

    IEnumerator BuyPercentagePerTickRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        bool hasMoney = MoneyManager.Instance.CheckMoney(costPerTick);

        while (hasMoney)
        {
            hasMoney = MoneyManager.Instance.ChargeMoney(costPerTick);
            currentProgress += percentagePerTick;
            if (currentProgress < 100)
            {
                progressBar.fillAmount = Mathf.Clamp01(currentProgress / 100);
            }
            else
            {
                int overLevel = 0;
                if (currentProgress > 100)
                {
                    overLevel = (int)currentProgress - 100;
                }
                LevelUp(overLevel);
            }
            yield return new WaitForSeconds(tickDelay);
        }
    }

    void LevelUp(int overLevel)
    {
        PlayerStacker stacker = GameObject.FindAnyObjectByType<PlayerStacker>();
        stacker.IncreaseStackListMax(stackSlotsPerLevel);
        currentProgress = overLevel;
        progressBar.fillAmount = Mathf.Clamp01(currentProgress / 100);
        currentLevel++;
        costPerTick = (int)(baseCostperTick * (currentLevel * costMultiplierPerLevel));
        zoneTxt.text = $"Subir\n nível \n $ {costPerTick} ";
    }
}
