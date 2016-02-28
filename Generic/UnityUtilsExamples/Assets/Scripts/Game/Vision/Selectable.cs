namespace Vision
{
    /* public abstract class Selectable : ISelectable
     {
         private static Selectable selected;

         public virtual void selectThis()
         {
             if (selected != null)
                 selected.OnSelectEnd();
             selected = this;
             OnSelectStart();
         }

         public static void setSelectNull()
         {
             if (selected != null)
                 selected.OnSelectEnd();
             selected = null;
         }

         protected abstract void OnSelectStart();
         protected abstract void OnSelectEnd();
     }*/

    public class SelectionManger
    {
        private static ISelectable selected;

        public static void select(ISelectable selectable)
        {
            if (selected != null)
                selected.OnSelectEnd();
            selected = selectable;
            selectable.OnSelectStart();
        }

        public static void setSelectNull()
        {
            if (selected != null)
                selected.OnSelectEnd();
            selected = null;
        }

        public static ISelectable getCurrentSelected()
        {
            return selected;
        }
    }

    public interface ISelectable
    {
        void OnSelectStart();
        void OnSelectEnd();
    }
}
