using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Vision
{
    /// <summary>
    /// keeps track of what the player is trying to select.
    /// </summary>
    public class SelectionManger
    {
        private static List<ISelectable> selected = new List<ISelectable>();

        public static void select(ISelectable selectable)
        {
            foreach (ISelectable s in selected)
                s.OnSelectEnd();
            selected.Clear();
            selected.Add(selectable);
            selectable.OnSelectStart();
        }


        public static void addSelected(ISelectable selectable)
        {
            selected.Add(selectable);
            selectable.OnSelectStart();
        }

        public static void setSelectNull()
        {
            foreach (ISelectable s in selected)
                s.OnSelectEnd();
            selected.Clear();
        }

        public static ReadOnlyCollection<ISelectable> getCurrentSelected()
        {
            return selected.AsReadOnly();
        }
        
        public static void addTarget(object ogg)
        {
            foreach (ISelectable sel in selected)
            {
                sel.reciveTarget(ogg);
            }
        }
    }

    /// <summary>
    /// a class that implement this interface can be selected.
    /// the two function will be called.
    /// </summary>
    public interface ISelectable
    {
        void OnSelectStart();
        void OnSelectEnd();
        bool selectionPersingTroughScenes();
        void reciveTarget(object ogg);

    }
}
