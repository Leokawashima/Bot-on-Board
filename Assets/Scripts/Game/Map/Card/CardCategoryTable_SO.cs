using System;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(menuName = "BoB/Card/Category")]
    public class CardCategoryTable_SO : ScriptableObject
    {
        public Category[] Table;

        [Serializable]
        public class Category
        {
            public string Name;
        }
    }
}