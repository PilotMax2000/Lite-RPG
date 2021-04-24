using System.Collections.Generic;

namespace LiteRPG.Helper
{
  public class Helper
  {
    public static bool ListIndexExist<T>(List<T> list, int index)
    {
      return index >= 0 && index <= list.Count - 1;
    }
  }
}