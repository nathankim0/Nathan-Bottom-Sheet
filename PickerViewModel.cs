using System.Collections.ObjectModel;

namespace NathanPicker
{
    public class PickerViewModel
    {
        public ObservableCollection<PickerItem> PickerItems { get; }

        public PickerViewModel()
        {
            PickerItems = new ObservableCollection<PickerItem>();
        }
    }
}