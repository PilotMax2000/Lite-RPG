using System;
using System.Collections.Generic;

namespace LiteRPG.Helper
{
  public static class Helper
  {
    public static bool ListIndexExist<T>(List<T> list, int index)
    {
      return index >= 0 && index <= list.Count - 1;
    }
    
    public static bool IsIndexExist<T>(this IReadOnlyList<T> collection, int index)
    {
      return index >= 0 && index < collection.Count;
    }
  }
}