using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class GameCache<T> where T : MonoBehaviour {
    private readonly HashSet<T> cache;

    internal GameCache(){
        cache = new HashSet<T>();
    }

    internal void Add(T item) {
        cache.Add(item);
    }

    internal void Remove(T item) {
        cache.Remove(item);
    }

    internal bool TryFetchDisabledItem(out T disabledItem) {

        return TryFetchByCondition(out disabledItem, ItemIsDisabled);

        #region Local_Function

#pragma warning disable IDE0062 // Make local function 'static'
        bool ItemIsDisabled(T item) {
#pragma warning restore IDE0062 // Make local function 'static'
            return !item.gameObject.activeInHierarchy;
        }

        #endregion
    }

    internal bool TryFetchByCondition(out T outItem, Func<T, bool> condition) {
        outItem = null;

        foreach (var item in cache) {
            if (condition(item)) {
                outItem = item;
                break;
            }
        }

        return outItem != null;
    }

    internal void Foreach(Action<T> action) {
        foreach (var item in cache) {
            action(item);
        }
    }
}
