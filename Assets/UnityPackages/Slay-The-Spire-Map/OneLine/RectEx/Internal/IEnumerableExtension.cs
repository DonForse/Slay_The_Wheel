using System;
using System.Collections.Generic;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.RectEx.Internal {
    public static class IEnumerableExtension {
        public static IEnumerable<TResult> Merge<TFirst, TSecond, TResult> (this IEnumerable<TFirst> first,
                                                                            IEnumerable<TSecond> second,
                                                                            Func<TFirst, TSecond, TResult> selector){
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext()){
                yield return selector(firstEnumerator.Current, secondEnumerator.Current);
            }
        }
    }
}
