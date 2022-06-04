using System;

public class HealthSystem
{
    public event Action OnHealthChanged;
    public event Action OnMaxHealthChanged;
    public event Action OnHeal;
    public event Action OnDead;
    public event Action OnDamage;


    private int _maxHealth;


    public HealthSystem(int maxHealth)
    {
        _maxHealth = maxHealth;
        CurrentHealth = _maxHealth;
        MaxHealth = _maxHealth;
    }

    public int CurrentHealth { get; private set; }

    public int MaxHealth { get; private set; }
    
    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > _maxHealth) CurrentHealth = _maxHealth;
        
        OnHeal?.Invoke();
        OnHealthChanged?.Invoke();
    }
    
    public void HealComplete()
    {
        CurrentHealth = _maxHealth;
        
        OnHealthChanged?.Invoke();
        OnHeal?.Invoke();
    }

    public void SetHealthMax(int amount, bool recover = false)
    {
        _maxHealth = amount;

        if (recover)
        {
            CurrentHealth = _maxHealth;
            OnHealthChanged?.Invoke();
        }
        OnMaxHealthChanged?.Invoke();
    }

    public void Damage(int amount)
    {
        CurrentHealth -= amount;
        
        OnDamage?.Invoke();
        OnHealthChanged?.Invoke();
        
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
        
    }

    public float GetHealthNormalized()
    {
        return (float)CurrentHealth / MaxHealth;
    }
    public void Die() => OnDead?.Invoke();
    public bool IsDead() => CurrentHealth <= 0;
    


}
