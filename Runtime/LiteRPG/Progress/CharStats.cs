namespace LiteRPG.Progress
{
  public interface IMoneyProgress
  {
    int Money { get; }
    void AddMoney(int amount);
  }

  public class TestGameProgress : IMoneyProgress
  {
    public int Money { get; private set; }

    public void AddMoney(int amount)
    {
      Money += amount;
      if (Money < 0) Money = 0;
    }
  }
}