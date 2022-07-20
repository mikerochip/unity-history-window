using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SelectionHistory.Editor
{
    [FilePath("UserSettings/Gemserk/Favorites.asset", FilePathAttribute.Location.ProjectFolder)]
    public class Favorites : ScriptableSingleton<Favorites>
    {
        [Serializable]
        public class Favorite
        {
            public Object reference;
        }
    
        public static event Action OnFavoritesUpdated;
        
        public static List<Favorite> FavoritesList => instance.favoritesList;

        [SerializeField] private List<Favorite> favoritesList = new List<Favorite>();

        public static void AddFavorite(Favorite favorite)
        {
            instance.favoritesList.Add(favorite);
            Save();
            OnFavoritesUpdated?.Invoke();
        }

        public static bool IsFavorite(Object reference)
        {
            return instance.favoritesList.Count(f => f.reference == reference) > 0;
        }

        public static void RemoveFavorite(Object reference)
        {
            instance.favoritesList.RemoveAll(f => f.reference == reference);
            Save();
            OnFavoritesUpdated?.Invoke();
        }

        public static bool CanBeFavorite(Object reference)
        {
            if (reference is GameObject go)
            {
                return go.scene == null;
            }
            return true;
        }

        private void OnEnable()
        {
            var removed = favoritesList.RemoveAll(f => f.reference == null);
            if (removed > 0)
                Save();
        }

        private static void Save()
        {
            instance.Save(saveAsText: true);
        }
    }
}