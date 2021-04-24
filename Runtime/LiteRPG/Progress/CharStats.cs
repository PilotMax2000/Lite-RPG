namespace LiteRPG.Progress
{
  public interface IMoneyStats
  {
    int Money { get; }
    void AddMoney(int amount);
  }

  public class CharStats : IMoneyStats
  {
    public int Money { get; private set; }

    public void AddMoney(int amount)
    {
      Money += amount;
      if (Money < 0) Money = 0;
    }
  }
}