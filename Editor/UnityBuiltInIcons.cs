using UnityEditor;

namespace MikeSchweitzer.SelectionHistory.Editor
{
    public static class UnityBuiltInIcons
    {
        public static string FavoriteIconName { get; } = EditorGUIUtility.isProSkin
            ? "d_Favorite On Icon"
            : "d_Favorite Icon";

        public static string FavoriteEmptyIconName { get; } = EditorGUIUtility.isProSkin
            ? "d_Favorite Icon"
            : "d_Favorite On Icon";
        
        public static string RemoveIconName => "d_ol_minus";
        public static string SearchIconName => "d_Search Icon";
    }
}