using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Critic.Test.Util
{
    public static class TestExtensions
    {
        public static void AreJsonEquals(this Assert assert,  object a, object b) {
            var jsonA = JsonConvert.SerializeObject(a, Formatting.None);
            var jsonB = JsonConvert.SerializeObject(b, Formatting.None);

            if (jsonA != jsonB)
                throw new AssertFailedException($"Objects are different => '{jsonA}' , '{jsonB}' ");
        }

        public static void ReplaceItem<T>(this List<T> list, T replacement, Func<T, object> predicate)
        {
            var oldItemIndex = list.FindIndex(u => predicate(u).ToString() == predicate(replacement).ToString());
            list[oldItemIndex] = replacement;
        }
    }
}
