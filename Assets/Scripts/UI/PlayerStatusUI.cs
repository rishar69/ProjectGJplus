using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusUI : MonoBehaviour
{
    [Header("References")]
    public Health health;
    public Armor armor;
    public PlayerLevel playerLevel;
    public PlayerWeaponController weaponController;

    [Header("Health UI")]
    public Slider healthSlider;
    public TMP_Text healthText;

    [Header("Armor UI")]
    public Slider armorSlider;
    public TMP_Text armorText;

    [Header("XP UI")]
    public Slider xpSlider;
    public TMP_Text levelText;

    [Header("Weapon UI")]
    public Image weaponIcon;
    public TMP_Text weaponNameText;
    public TMP_Text ammoText;

    [Header("Score UI")]
    public TMP_Text scoreText;

    [Header("Smooth Settings")]
    public float smoothSpeed = 10f;

    private float healthTarget;
    private float armorTarget;
    private float xpTarget;

    void Start()
    {
        if (playerLevel == null) playerLevel = FindFirstObjectByType<PlayerLevel>();
        if (health == null) health = FindFirstObjectByType<Health>();
        if (armor == null) armor = FindFirstObjectByType<Armor>();
        if (weaponController == null) weaponController = FindFirstObjectByType<PlayerWeaponController>();

        // Setup sliders
        healthSlider.minValue = 0; healthSlider.maxValue = 1;
        armorSlider.minValue = 0; armorSlider.maxValue = 1;
        xpSlider.minValue = 0; xpSlider.maxValue = 1;

        // Initial values
        healthTarget = health.HealthPercent;
        armorTarget = armor.ArmorPercent;
        xpTarget = 0f;

        healthSlider.value = healthTarget;
        armorSlider.value = armorTarget;
        xpSlider.value = xpTarget;

        UpdateHealthText();
        UpdateArmorText();

        // Events
        health.OnHealthChanged.AddListener(OnHealthChanged);
        armor.OnArmorChanged.AddListener(OnArmorChanged);

        playerLevel.OnXPChanged += OnXPChanged;
        playerLevel.OnLevelUp += OnLevelUp;

        levelText.text = "LV " + playerLevel.level;

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.onScoreChanged += UpdateScoreUI;

        UpdateScoreUI(ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0);


        // Initialize weapon UI
        UpdateWeaponUI();
    }

    void Update()
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, healthTarget, Time.deltaTime * smoothSpeed);
        armorSlider.value = Mathf.Lerp(armorSlider.value, armorTarget, Time.deltaTime * smoothSpeed);
        xpSlider.value = Mathf.Lerp(xpSlider.value, xpTarget, Time.deltaTime * smoothSpeed);

        UpdateAmmoOnly();
    }

    private void UpdateScoreUI(int newScore)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + newScore;
    }

    private void OnHealthChanged(float percent)
    {
        healthTarget = percent;
        UpdateHealthText();
    }

    private void OnArmorChanged(float percent)
    {
        armorTarget = percent;
        UpdateArmorText();
    }

    private void OnXPChanged(int currentXP, int neededXP)
    {
        xpTarget = (float)currentXP / neededXP;
    }

    private void OnLevelUp(int newLevel)
    {
        levelText.text = "LV " + newLevel;
    }

    private void UpdateHealthText()
    {
        healthText.text = $"{Mathf.RoundToInt(health.CurrentHealth)} / {Mathf.RoundToInt(health.MaxHealth)}";
    }

    private void UpdateArmorText()
    {
        armorText.text = $"{Mathf.RoundToInt(armor.CurrentArmor)} / {Mathf.RoundToInt(armor.MaxArmor)}";
    }

    // ======================
    //     WEAPON UI
    // ======================
    public void UpdateWeaponUI()
    {
        if (weaponController == null || weaponController.weapons.Length == 0)
            return;

        Weapon w = weaponController.weapons[weaponController.CurrentWeaponIndex];
        if (w == null) return;

        if (weaponIcon != null)
            weaponIcon.sprite = w.WeaponIcon;

        if (weaponNameText != null)
            weaponNameText.text = w.weaponName;

        UpdateAmmoOnly();
    }

    private void UpdateAmmoOnly()
    {
        if (weaponController == null) return;
        if (weaponController.weapons == null || weaponController.weapons.Length == 0) return;

        Weapon w = weaponController.weapons[weaponController.CurrentWeaponIndex];
        if (w == null) return;

        ammoText.text = $"{w.currentAmmo} / {w.magazineSize}";
    }
}
