using Xamarin.Forms;
using BottomSheet;


namespace NathanPicker
{
    public class NathanMemo : NathanBottomSheet
    {
        private readonly Editor editor;
        public NathanMemo()
        {
            editor = new Editor { Margin=50, BackgroundColor=Color.Silver};
            BottomPanContainer.FrameContentStackLayout.Children.Add(editor);
        }
    }
}
