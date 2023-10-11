using System;

public class RunCard
{
    public string CardName
    {
        get => _cardName;
        set
        {
            _cardName = value;
            ValueChanged?.Invoke(this, this);
        }
    }

    public int Hp
    {
        get => _hp;
        set
        {
            _hp = value;
            ValueChanged?.Invoke(this, this);
        }
    }

    public int Attack
    {
        get => _attack;
        set
        {
            _attack = value;
            ValueChanged?.Invoke(this, this);
        }
    }
    
    public Ability[] Abilities
    {
        get => _abilities;
        set
        {
            _abilities = value;
            ValueChanged?.Invoke(this, this);
        }
    }

    public readonly BaseCardScriptableObject baseCard;
    
    private int _attack;
    private Ability[] _abilities;
    private int _hp;
    private string _cardName;

    public RunCard(BaseCardScriptableObject heroCardDb)
    {
        _cardName = heroCardDb.cardName;
        _hp = heroCardDb.hp;
        _attack = heroCardDb.attack;
        _abilities = heroCardDb.abilities;
        baseCard = heroCardDb;
    }

    public event EventHandler<RunCard> ValueChanged;
}