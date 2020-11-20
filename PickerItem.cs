using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dreamfora;
using Xamarin.Forms;

namespace NathanPicker
{
    public class PickerItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isChecked;


        private string _itemText;
        private string _imageSource;
        private string _currentSearch;

        private Color _textColor = Color.Default;
        private FontAttributes _fontAttributes = FontAttributes.None;
        private double _characterSpacing = 0;
        private double _fontSize = DFDesignMainPage.PickerTitleFontSize.Value;
        private string _fontFamily = null;


        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        public string ItemText
        {
            get => _itemText;
            set
            {
                _itemText = value;
                OnPropertyChanged();
            }
        }

        public string Imagesource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public string CurrentSearch
        {
            get => _currentSearch;
            set
            {
                _currentSearch = value;
                OnPropertyChanged();
            }
        }

        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                OnPropertyChanged();
            }
        }

        public FontAttributes FontAttributes
        {
            get => _fontAttributes;
            set
            {
                _fontAttributes = value;
                OnPropertyChanged();
            }
        }

        public double CharacterSpacing
        {
            get => _characterSpacing;
            set
            {
                _characterSpacing = value;
                OnPropertyChanged();
            }
        }

        public double FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged();
            }
        }

        public string FontFamily
        {
            get => _fontFamily;
            set
            {
                _fontFamily = value;
                OnPropertyChanged();
            }
        }

        public PickerItem()
        {
        }

        public PickerItem(string name)
        {
            _itemText = name;
        }
        public PickerItem(string name, string imageSource)
        {
            _itemText = name;
            _imageSource = imageSource;
        }
       

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}