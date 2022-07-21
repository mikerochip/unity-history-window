using UnityEditor;
using UnityEngine;

namespace SelectionHistory.Editor
{
    [FilePath("UserSettings/SelectionHistory/SelectionHistory.asset", FilePathAttribute.Location.ProjectFolder)]
    public class SelectionHistoryPreferences : ScriptableSingleton<SelectionHistoryPreferences>
    {
        public static int HistorySize
        {
            get => instance.historySize;
            set
            {
                instance.historySize = value;
                Save();
            }
        }
        public static bool AutoRemoveDestroyed
        {
            get => instance.autoRemoveDestroyed;
            set
            {
                instance.autoRemoveDestroyed = value;
                Save();
            }
        }
        public static bool AllowDuplicated
        {
            get => instance.allowDuplicated;
            set
            {
                instance.allowDuplicated = value;
                Save();
            }
        }
        public static bool ShowHierarchyObjects
        {
            get => instance.showHierarchyObjects;
            set
            {
                instance.showHierarchyObjects = value;
                Save();
            }
        }
        public static bool ShowUnloadedObjects
        {
            get => instance.showUnloadedObjects;
            set
            {
                instance.showUnloadedObjects = value;
                Save();
            }
        }
        public static bool ShowDestroyedObjects
        {
            get => instance.showDestroyedObjects;
            set
            {
                instance.showDestroyedObjects = value;
                Save();
            }
        }
        public static bool ShowFavoriteButton
        {
            get => instance.showFavoriteButton;
            set
            {
                instance.showFavoriteButton = value;
                Save();
            }
        }
        public static bool ShowProjectViewObjects
        {
            get => instance.showProjectViewObjects;
            set
            {
                instance.showProjectViewObjects = value;
                Save();
            }
        }
        public static bool DrawFavorites
        {
            get => instance.drawFavorites;
            set
            {
                instance.drawFavorites = value;
                Save();
            }
        }
        
        [SerializeField] private int historySize = 500;
        [SerializeField] private bool autoRemoveDestroyed = true;
        [SerializeField] private bool allowDuplicated;
        [SerializeField] private bool showHierarchyObjects = true;
        [SerializeField] private bool showUnloadedObjects = true;
        [SerializeField] private bool showDestroyedObjects;
        [SerializeField] private bool showFavoriteButton;
        [SerializeField] private bool showProjectViewObjects = true;
        [SerializeField] private bool drawFavorites = true;

        [SettingsProvider]
        public static SettingsProvider CreateSelectionHistorySettingsProvider() {
            var provider = new SettingsProvider("Selection History", SettingsScope.User) {
                label = "Selection History",
                guiHandler = (searchContext) => instance.OnSettingsGUI(searchContext),
            };
            return provider;
        }

        private void OnSettingsGUI(string searchContext)
        {
            historySize = EditorGUILayout.DelayedIntField("History Size", historySize);
            autoRemoveDestroyed = EditorGUILayout.Toggle("Auto Remove Destroyed", autoRemoveDestroyed);
            allowDuplicated = EditorGUILayout.Toggle("Allow Duplicated entries", allowDuplicated);
            showHierarchyObjects = EditorGUILayout.Toggle("Show Hierarchy objects", showHierarchyObjects);
            showProjectViewObjects = EditorGUILayout.Toggle("Show ProjectView objects", showProjectViewObjects);
            drawFavorites = EditorGUILayout.Toggle("Show Pin to favorites button", drawFavorites);
            
            if (GUI.changed)
                Save();
        }

        private static void Save()
        {
            instance.Save(saveAsText: true);
        }
    }
}
