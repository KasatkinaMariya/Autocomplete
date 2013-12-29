using System;
using System.Collections.Generic;

namespace Autocompletter
{
    static class SortedLinkedListExtension
    {
        public static IEnumerable<TResult> Select<TElement, TResult>
            (this SortedLinkedList<TElement> source, Func<TElement,TResult> selector)
            where TElement : IComparable<TElement>
        {
            var selectedItems = new List<TResult>();

            foreach (TElement item in source)
                selectedItems.Add(selector(item));   

            return selectedItems;
        }
    }
}
