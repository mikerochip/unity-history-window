using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace SelectionHistory.Editor
{
    public class FavoriteAssetsWindow : EditorWindow
    {
        [MenuItem("Window/Selection History/Favorites")]
        public static void OpenWindow()
        {
            var window = GetWindow<FavoriteAssetsWindow>();
            var titleContent = EditorGUIUtility.IconContent(UnityBuiltInIcons.favoriteIconName);
            titleContent.text = "Favorites";
            titleContent.tooltip = "Favorite assets window";
            window.titleContent = titleContent;
        }

        [MenuItem("Assets/Favorite Item")]
        [Shortcut("Selection History/Favorite Item", null, KeyCode.F, ShortcutModifiers.Shift | ShortcutModifiers.Alt)]
        public static void Favorite()
        { 
            FavoriteElements(Selection.objects);
        }

        private static void FavoriteElements(Object[] references)
        {
            foreach (var reference in references)
            {
                if (Favorites.IsFavorite(reference))
                    continue;
            
                if (Favorites.CanBeFavorite(Selection.activeObject))
                {
                    Favorites.AddFavorite(new Favorites.Favorite
                    {
                        reference = reference
                    });   
                }
            }
        }

        public StyleSheet styleSheet;

        public VisualTreeAsset favoriteElementTreeAsset;
        
        public void OnEnable()
        {
            Favorites.OnFavoritesUpdated += OnFavoritesUpdated;
            
            var root = rootVisualElement;
            root.styleSheets.Add(styleSheet);

            root.RegisterCallback<DragPerformEvent>(evt =>
            {
                DragAndDrop.AcceptDrag();
                FavoriteElements(DragAndDrop.objectReferences);
            });
            
            root.RegisterCallback<DragUpdatedEvent>(evt =>
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Move;
            });
            
            ReloadRoot();
        }

        private void OnFavoritesUpdated()
        {
            var root = rootVisualElement;
            root.Clear();
        
            ReloadRoot();
        }

        private void ReloadRoot()
        {
            var root = rootVisualElement;

            // var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/FavoriteElement.uxml");

            var scroll = new ScrollView(ScrollViewMode.Vertical);
            root.Add(scroll);

            for (var i = 0; i < Favorites.FavoritesList.Count; i++)
            {
                var assetReference = Favorites.FavoritesList[i].reference;

                if (assetReference == null)
                    continue;
                
                var elementTree = favoriteElementTreeAsset.CloneTree();

                var dragArea = elementTree.Q<VisualElement>("DragArea");
                if (dragArea != null)
                {
                    dragArea.RegisterCallback<MouseUpEvent>(evt =>
                    {
                        if (evt.button == 0)
                        {
                            Selection.activeObject = assetReference;
                        }
                        else
                        {
                            EditorGUIUtility.PingObject(assetReference);
                        }
                            
                        dragArea.userData = null;
                    });
                    dragArea.RegisterCallback<MouseDownEvent>(evt =>
                    {
                        if (evt.button == 0 && evt.modifiers.HasFlag(EventModifiers.Alt))
                        {
                            DragAndDrop.PrepareStartDrag();

                            var objectReferences = new[] {assetReference};
                            DragAndDrop.paths = new[]
                            {
                                AssetDatabase.GetAssetPath(assetReference)
                            };

                            DragAndDrop.objectReferences = objectReferences;
                            DragAndDrop.StartDrag(ObjectNames.GetDragAndDropTitle(assetReference));
                        }
                    });

                    dragArea.RegisterCallback<DragUpdatedEvent>(evt =>
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                    });
                }
                
                var icon = elementTree.Q<Image>("Icon");
                if (icon != null)
                {
                    icon.image = AssetPreview.GetMiniThumbnail(assetReference);
                }
                
                var removeIcon = elementTree.Q<Image>("RemoveIcon");
                if (removeIcon != null)
                {
                    // removeIcon.image = AssetPreview.GetMiniThumbnail(assetReference);
                    removeIcon.image = EditorGUIUtility.IconContent(UnityBuiltInIcons.removeIconName).image;
                    
                    removeIcon.RegisterCallback(delegate(MouseUpEvent e)
                    {
                        Favorites.RemoveFavorite(assetReference);
                    });
                }
                
                var label = elementTree.Q<Label>("Favorite");
                if (label != null)
                {
                    label.text = assetReference.name;
                }

                scroll.Add(elementTree);
            }

            var receiveDragArea = new VisualElement();
            receiveDragArea.style.flexGrow = 1;
            root.Add(receiveDragArea);
        }
    }
}