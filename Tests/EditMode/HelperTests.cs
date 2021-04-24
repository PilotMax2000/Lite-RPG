using System.Collections;
using System.Collections.Generic;
using LiteRPG.Helper;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class HelperTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ListIndexInLengthPasses()
        {
            List<int> test = new List<int>() {0};
            bool res = Helper.ListIndexExist(test, 0);
            Assert.AreEqual(true, res);
        }

        [Test]
        public void ListIndexOutOfLengthPasses()
        {
            List<int> test = new List<int>() {0};
            bool res = Helper.ListIndexExist(test, 1);
            Assert.AreEqual(false, res);
        }
    }
}