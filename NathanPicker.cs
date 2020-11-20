using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections;
using Dreamfora;
using System.Linq;
using System.Text.RegularExpressions;
using BottomSheet;

namespace NathanPicker
{
    public class NathanPicker : NathanBottomSheet, INathanPicker
    {
        #region Declare

        private PickerViewModel PickerViewModel { get; set; }

        private readonly HeaderLayout _headerLayout; // 상단 헤더부분 (손잡이 + 타이틀)
        private readonly CollectionView _myCollectionView;

        #endregion Declare


        #region Constructor

        public NathanPicker()
        {
            BindingContext = new PickerItem();

            IsVisible = false;

            PickerViewModel = new PickerViewModel();

            _headerLayout = new HeaderLayout();

            _headerLayout.TapGestureRecognizerSearchImage.Tapped += (s, e) =>
            {
                _myCollectionView.ScrollTo(0);

                _headerLayout.TitleImage.IsVisible = false;
                _headerLayout.TitleLabel.IsVisible = false;
                _headerLayout.SearchImage.IsVisible = false;

                _headerLayout.PickerSearchBar.IsVisible = true;
                _headerLayout.BackImage.IsVisible = true;

                new Animation(d => _headerLayout.PickerSearchBar.Opacity = d, 0.5, 1).Commit(_headerLayout.PickerSearchBar,
                    "SearchBar Animation", 60, 300,
                    Easing.CubicOut);
            };

            _headerLayout.TapGestureRecognizerBackImage.Tapped += (s, e) =>
            {
                _myCollectionView.ScrollTo(0);

                _headerLayout.SearchImage.IsVisible = true;
                _headerLayout.PickerSearchBar.Text = "";

                _headerLayout.TitleImage.IsVisible = true;
                _headerLayout.TitleLabel.IsVisible = true;

                _headerLayout.PickerSearchBar.IsVisible = false;
                _headerLayout.BackImage.IsVisible = false;
            };

            _headerLayout.PickerSearchBar.TextChanged += OnPickerSearchBarTextChanged;


            var emptyViewLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Fill,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = DFDesignMainPage.PickerTitleFontSize.Value
            };
            emptyViewLabel.SetBinding(Label.TextProperty, "CurrentSearch");

            _myCollectionView = new CollectionView
            {
                ItemsSource = PickerViewModel.PickerItems,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                SelectionMode = SelectionMode.Single,

                ItemTemplate = new DataTemplate(() => new CollectionCellLayout()),

                EmptyView = new StackLayout
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Padding = new Thickness(DFDesignMainPage.emptyViewSidePadding.Value, 0,
                        DFDesignMainPage.emptyViewSidePadding.Value, 0),
                    Children =
                    {
                        emptyViewLabel
                    }
                }
            };
            _myCollectionView.SelectionChanged += OnCollectionsViewItemSelected;


            BottomPanContainer.FrameContentStackLayout.Children.Add(_headerLayout);
            BottomPanContainer.FrameContentStackLayout.Children.Add(_myCollectionView);

            Init();


            var backgroundGesture = new TapGestureRecognizer();
            backgroundGesture.Tapped += (s, e) =>
            {
                if (IsVisible)
                {
                    InVisiblePicker();
                }
            };
        }

        #endregion Constructor

        #region Methods

        private void Init()
        {
            PickerRectangle = DFDesignMainPage.bottomPanContainerRectangle;
            HasIcon = true;
            SortType = SortType.None;
            Title = "";
            TitleColor = Color.Default;
            TitleFontSize = DFDesignMainPage.PickerTitleFontSize.Value;
            TitleImageSource = "";
            TextColor = Color.Default;
            FontAttributes = FontAttributes.None;
            CharacterSpacing = 0;
            SelectedIndex = -1;
            FontSize = DFDesignMainPage.PickerFontSize.Value;
            FontFamily = null;
            SearchEnabled = true;
        }

        public override void VisiblePicker()
        {
            if (IsVisible) return;
            IsVisible = true;

            if (PickerViewModel.PickerItems.Count <= 0)
            {
                SearchEnabled = false;
            }

            if (SelectedIndex > -1 && SelectedIndex < PickerViewModel.PickerItems.Count)
            {
                Console.WriteLine("**** ChangeSelectedItemColor()");
                ChangeSelectedItemColor();
            }

            new Animation
            {
                {0, 0.5, new Animation(v => Background.Opacity = v)},
                {
                    0, 1, new Animation(v => BottomPanContainer.BottomSheetFrame.TranslationY = v,
                        PickerRectangle.Y,
                        BottomPanContainer._y = 0,
                        Easing.CubicOut)
                }
            }.Commit(this, "VisiblePicker", 10, 400);
        }

        public override void InVisiblePicker()
        {
            new Animation
            {
                {0, 0.5, new Animation(v => Background.Opacity = v, 1, 0)},
                {
                    0, 1, new Animation(v => BottomPanContainer.BottomSheetFrame.TranslationY = v,
                        BottomPanContainer._y,
                        BottomPanContainer._y = PickerRectangle.Y,
                        Easing.CubicOut)
                }
            }.Commit(this, "InVisiblePicker", 10, 400, null, (v, c) => IsVisible = false, () => false);

            if (SearchEnabled)
            {
                _headerLayout.SearchImage.IsVisible = true;
                _headerLayout.PickerSearchBar.Text = "";

                _headerLayout.TitleImage.IsVisible = true;
                _headerLayout.TitleLabel.IsVisible = true;

                _headerLayout.PickerSearchBar.IsVisible = false;
                _headerLayout.BackImage.IsVisible = false;
            }

            _myCollectionView.ScrollTo(0);
        }


        /// <summary>
        /// Dreamfora에 적용 시, 변경 필요.
        /// </summary>
        private void ChangeSelectedItemColor()
        {
            PickerViewModel.PickerItems.ToList().ForEach(x =>
            {
                x.TextColor = TextColor;
                x.IsChecked = false;
            });

            PickerViewModel.PickerItems.ToList().ElementAt(SelectedIndex).TextColor = Color.FromRgb(140, 78, 218);
            PickerViewModel.PickerItems.ToList().ElementAt(SelectedIndex).IsChecked = true;
        }

        public void AddItem(string text)
        {
            PickerViewModel.PickerItems.Add(new PickerItem(text));

            SetItemOptionsDefault();
        }

        public void AddItem(string text, string imageSource)
        {
            PickerViewModel.PickerItems.Add(new PickerItem(text, imageSource));

            SetItemOptionsDefault();
        }

        public void AddItem(PickerItem pickerItem)
        {
            PickerViewModel.PickerItems.Add(pickerItem);

            SetItemOptionsDefault();
        }


        private void SetItemOptionsDefault()
        {
            PickerViewModel.PickerItems.ToList().ForEach(x => x.FontAttributes = FontAttributes);
            PickerViewModel.PickerItems.ToList().ForEach(x => x.CharacterSpacing = CharacterSpacing);
            PickerViewModel.PickerItems.ToList().ForEach(x => x.FontSize = FontSize);
            PickerViewModel.PickerItems.ToList().ForEach(x => x.FontFamily = FontFamily);
        }

        public void DeleteItem(int index)
        {
            if (index >= PickerViewModel.PickerItems.Count || index < 0 || PickerViewModel.PickerItems.Count == 0)
            {
                return;
            }

            PickerViewModel.PickerItems.RemoveAt(index);
        }

        public string GetItemText(int index)
        {
            if (index >= PickerViewModel.PickerItems.Count || index < 0 || PickerViewModel.PickerItems.Count == 0)
            {
                return "";
            }

            return PickerViewModel.PickerItems[index].ItemText;
        }

        private void OnPickerSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            _myCollectionView.ScrollTo(0);
            _myCollectionView.ItemsSource = GetSearchResults(e.NewTextValue);
            ((PickerItem) (_myCollectionView.BindingContext)).CurrentSearch =
                "\"" + e.NewTextValue + "\"\r\n" + "was not found.";
        }

        private void OnCollectionsViewItemSelected(object sender, SelectionChangedEventArgs e)
        {
            SelectedIndex = PickerViewModel.PickerItems.IndexOf((PickerItem) e.CurrentSelection.FirstOrDefault());
            ChangeSelectedItemColor();
            InVisiblePicker();
        }

        private IEnumerable<PickerItem> GetSearchResults(string queryString)
        {
            // 띄어쓰기 제거, 대문자 -> 소문자
            // \s는 공백문자를 나타냄.
            // ?? 앞에 값이 null이면 뒤에 있는 "" 대입

            var normalizedQuery = Regex.Replace(queryString?.ToLower() ?? "", @"\s", "");

            return PickerViewModel.PickerItems
                .Where(f => Regex.Replace(f.ItemText.ToLowerInvariant(), @"\s", "")
                    .Contains(normalizedQuery)).ToList();
        }

        #endregion Methods


        #region Event

        public event EventHandler SelectedIndexChanged;

        #endregion Event


        #region Fields

        private static readonly BindableProperty HasIconProperty = BindableProperty.Create(nameof(HasIcon),
            typeof(bool),
            typeof(NathanPicker), true, propertyChanged: OnHasIconChanged);

        private static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
            typeof(IList), typeof(NathanPicker), propertyChanged: OnItemsSourceChanged);

        private static readonly BindableProperty SortTypeProperty = BindableProperty.Create(nameof(SortType),
            typeof(SortType), typeof(NathanPicker), SortType.None, propertyChanged: OnSortTypeChanged);

        private static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string),
            typeof(NathanPicker), "", propertyChanged: OnTitleChanged);

        private static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor),
            typeof(Color), typeof(NathanPicker), Color.Default, propertyChanged: OnTitleColorChanged);

        private static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize),
            typeof(double),
            typeof(NathanPicker), DFDesignMainPage.PickerTitleFontSize.Value, propertyChanged: OnTitleFontSizeChanged);

        private static readonly BindableProperty TitleImageProperty = BindableProperty.Create(nameof(TitleImageSource),
            typeof(string), typeof(NathanPicker), "", propertyChanged: OnTitleImageChanged);

        private static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(NathanPicker), Color.Default,
                propertyChanged: OnTextColorChanged);

        private static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(NathanPicker),
                FontAttributes.None,
                propertyChanged: OnFontAttributesChanged);

        private static readonly BindableProperty CharacterSpacingProperty =
            BindableProperty.Create(nameof(CharacterSpacing), typeof(double), typeof(NathanPicker), 0.0,
                propertyChanged: OnCharacterSpacingChanged);

        private static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(NathanPicker),
                DFDesignMainPage.PickerTitleFontSize.Value,
                propertyChanged: OnFontSizeChanged);

        private static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(NathanPicker), null,
                propertyChanged: OnFontFamilyChanged);

        private static readonly BindableProperty SearchEnabledProperty =
            BindableProperty.Create(nameof(SearchEnabled), typeof(bool), typeof(NathanPicker), true,
                propertyChanged: OnSearchEnabledChanged);

        private static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex),
            typeof(int), typeof(BottomPanContainer), -1, propertyChanged: OnSelectedIndexChanged);

        #endregion Fields

        #region Properties

        public bool HasIcon
        {
            get => (bool) GetValue(HasIconProperty);
            set => SetValue(HasIconProperty, value);
        }

        public IList ItemsSource
        {
            get => (IList) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public SortType SortType
        {
            get => (SortType)GetValue(SortTypeProperty);
            set => SetValue(SortTypeProperty, value);
        }

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public Color TitleColor
        {
            get => (Color) GetValue(TitleColorProperty);
            set => SetValue(TitleColorProperty, value);
        }

        public double TitleFontSize
        {
            get => (double) GetValue(TitleFontSizeProperty);
            set => SetValue(TitleFontSizeProperty, value);
        }

        public string TitleImageSource
        {
            get => (string) GetValue(TitleImageProperty);
            set => SetValue(TitleImageProperty, value);
        }

        public Color TextColor
        {
            get => (Color) GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public FontAttributes FontAttributes
        {
            get => (FontAttributes) GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }

        public double CharacterSpacing
        {
            get => (double) GetValue(CharacterSpacingProperty);
            set => SetValue(CharacterSpacingProperty, value);
        }

        public double FontSize
        {
            get => (double) GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public string FontFamily
        {
            get => (string) GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public bool SearchEnabled
        {
            get => (bool) GetValue(SearchEnabledProperty);
            set => SetValue(SearchEnabledProperty, value);
        }

        public int SelectedIndex
        {
            get => (int) GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        #endregion Properties

        #region Property Changed Methods

        private static void OnHasIconChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var check = (bool) newValue;

            if (check) return;

            ((NathanPicker) bindable)._myCollectionView.BackgroundColor =
                Color.FromRgb(247, 247, 249);
            ((NathanPicker) bindable)._myCollectionView.ItemTemplate = new DataTemplate(() =>
                new NonIconCollectionCellLayout
                {
                    Padding = new Thickness(0, DFDesignMainPage.collectionTextTopBottomPadding.Value, 0,
                        DFDesignMainPage.collectionTextTopBottomPadding.Value)
                });
            ((NathanPicker) bindable)._myCollectionView.ItemsSource =
                ((NathanPicker) bindable).PickerViewModel.PickerItems;
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var t = (List<PickerItem>) newValue;

            ((NathanPicker)bindable).PickerViewModel.PickerItems.Clear();

            t.ForEach(x =>
            {
                ((NathanPicker)bindable).PickerViewModel.PickerItems.Add(x);
            });
        }

        private static void OnSortTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            switch ((SortType)newValue)
            {
                case SortType.None:
                    break;
                case SortType.Ascending:
                    ((NathanPicker)bindable).ItemsSource = (((NathanPicker)bindable).ItemsSource as List<PickerItem>).OrderBy(x => x.ItemText).ToList() ;
                    break;
                case SortType.Descending:
                    ((NathanPicker)bindable).ItemsSource = (((NathanPicker)bindable).ItemsSource as List<PickerItem>).OrderByDescending(x => x.ItemText).ToList();
                    break;
            }
        }

        private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((NathanPicker) bindable)._headerLayout.TitleLabel.Text = (string) newValue;
        }

        private static void OnTitleColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((NathanPicker) bindable)._headerLayout.TitleLabel.TextColor = (Color) newValue;
        }

        private static void OnTitleFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((NathanPicker) bindable)._headerLayout.TitleLabel.FontSize = (double) newValue;
        }

        private static void OnTitleImageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((NathanPicker) bindable)._headerLayout.TitleImage.Source = (string) newValue;
        }

        private static void OnTextColorChanged(object bindable, object oldValue, object newValue)
        {
            var color = (Color) newValue;
            ((NathanPicker) bindable).PickerViewModel.PickerItems.ToList().ForEach(x => x.TextColor = color);
        }

        private static void OnFontAttributesChanged(object bindable, object oldValue, object newValue)
        {
            var fontAttributes = (FontAttributes) newValue;
            ((NathanPicker) bindable).PickerViewModel.PickerItems.ToList()
                .ForEach(x => x.FontAttributes = fontAttributes);
        }

        private static void OnCharacterSpacingChanged(object bindable, object oldValue, object newValue)
        {
            var characterSpacing = (double) newValue;
            ((NathanPicker) bindable).PickerViewModel.PickerItems.ToList()
                .ForEach(x => x.CharacterSpacing = characterSpacing);
        }

        private static void OnFontSizeChanged(object bindable, object oldValue, object newValue)
        {
            var fontSize = (double) newValue;
            ((NathanPicker) bindable).PickerViewModel.PickerItems.ToList().ForEach(x => x.FontSize = fontSize);
        }

        private static void OnFontFamilyChanged(object bindable, object oldValue, object newValue)
        {
            var fontFamily = (string) newValue;
            ((NathanPicker) bindable).PickerViewModel.PickerItems.ToList().ForEach(x => x.FontFamily = fontFamily);
        }

        private static void OnSearchEnabledChanged(object bindable, object oldValue, object newValue)
        {
            ((NathanPicker) bindable)._headerLayout.SearchImage.IsVisible = (bool) newValue;
        }

        private static void OnSelectedIndexChanged(object bindable, object oldValue, object newValue)
        {
            ((NathanPicker) bindable).SelectedIndexChanged?.Invoke(bindable, EventArgs.Empty);
        }

        #endregion Property Changed Methods
    }

    public class CollectionCellLayout : DFALayout
    {
        private readonly Image _checkImage;
        private readonly Image _iconImage;
        protected readonly Label TextLabel;

        public CollectionCellLayout()
        {
            Padding = new Thickness(0, DFDesignMainPage.collectionTextTopBottomPadding.Value, 0,
                DFDesignMainPage.collectionTextTopBottomPadding.Value);

            _checkImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "CheckImage"
            };
            _checkImage.SetBinding(Image.IsVisibleProperty, "IsChecked");

            _iconImage = new Image
            {
                Aspect = Aspect.AspectFit
            };
            _iconImage.SetBinding(Image.SourceProperty, "Imagesource");


            TextLabel = new Label
            {
                LineBreakMode = LineBreakMode.TailTruncation,
            };
            TextLabel.SetBinding(Label.TextProperty, "ItemText");
            TextLabel.SetBinding(Label.FontAttributesProperty, "FontAttributes");
            TextLabel.SetBinding(Label.TextColorProperty, "TextColor");
            TextLabel.SetBinding(Label.CharacterSpacingProperty, "CharacterSpacing");
            TextLabel.SetBinding(Label.FontSizeProperty, "FontSize");
            TextLabel.SetBinding(Label.FontFamilyProperty, "FontFamily");

            var triggerTrue = new DataTrigger(typeof(CollectionCellLayout))
            {
                Value = true,
                Binding = new Binding() {Path = "IsChecked"}
            };
            var setterTrue = new Setter
            {
                Property = BackgroundColorProperty,
                Value = Color.FromRgb(247, 247, 249)
            };
            triggerTrue.Setters.Clear();
            triggerTrue.Setters.Add(setterTrue);
            this.Triggers.Add(triggerTrue);

            Children.Add(_checkImage);
            Children.Add(_iconImage);
            Children.Add(TextLabel);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(TextLabel.Measure(widthConstraint, heightConstraint).Request,
                TextLabel.Measure(widthConstraint, heightConstraint).Minimum);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            _checkImage.Layout(
                DFDesignMainPage.collectionCellCheckImageRectangle.RectangleInLayout(x, y, width, height));
            _iconImage.Layout(DFDesignMainPage.collectionCellImageRectangle.RectangleInLayout(x, y, width, height));
            TextLabel.Layout(DFDesignMainPage.collectionCellLabelRectangle.RectangleInLayout(x, y, width, height));
        }
    }

    public class NonIconCollectionCellLayout : CollectionCellLayout
    {
        public NonIconCollectionCellLayout()
        {
            Children.Add(TextLabel);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            TextLabel.Layout(
                DFDesignMainPage.collectionNonIconCellLabelRectangle.RectangleInLayout(x, y, width, height));
        }
    }

    public class HeaderLayout : DFALayout
    {
        private readonly BoxView _handle;

        public readonly Image TitleImage;
        public readonly Label TitleLabel;
        public readonly Image SearchImage;

        public readonly Image BackImage;
        public readonly Entry PickerSearchBar;


        public readonly TapGestureRecognizer TapGestureRecognizerSearchImage;
        public readonly TapGestureRecognizer TapGestureRecognizerBackImage;


        public HeaderLayout()
        {
            TapGestureRecognizerSearchImage = new TapGestureRecognizer();
            TapGestureRecognizerBackImage = new TapGestureRecognizer();

            _handle = new BoxView
            {
                CornerRadius = DFDesignMainPage.handleRadius.Value,
                BackgroundColor = Color.FromRgb(204, 204, 204),
            };

            TitleImage = new Image
            {
                Aspect = Aspect.AspectFit,
            };

            TitleLabel = new Label
            {
                FontSize = DFDesignMainPage.PickerTitleFontSize.Value,
            };

            SearchImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "iconsSearch",
                GestureRecognizers = {TapGestureRecognizerSearchImage}
            };

            BackImage = new Image
            {
                IsVisible = false,
                Aspect = Aspect.AspectFit,
                Source = "iconback",
                GestureRecognizers = {TapGestureRecognizerBackImage}
            };

            PickerSearchBar = new Entry
            {
                IsVisible = false,
                Placeholder = "Search...",
                BackgroundColor = Color.FromRgb(246, 248, 251),
                FontSize = DFDesignMainPage.PickerTitleFontSize.Value,
                ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                Margin = 0,
            };

            Children.Add(_handle);

            Children.Add(TitleImage);
            Children.Add(TitleLabel);
            Children.Add(SearchImage);

            Children.Add(BackImage);
            Children.Add(PickerSearchBar);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            Size minSize = new Size();
            Size maxSize = new Size();

            minSize.Width = maxSize.Width = DFDesignMainPage.fullScreenRectangle.Width;

            minSize.Height = maxSize.Height = DFDesignMainPage.headerHeight.Value;

            return new SizeRequest(maxSize, minSize);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            _handle.Layout(DFDesignMainPage.HandleRectangle.RectangleInLayout(x, y, width, height));

            TitleImage.Layout(DFDesignMainPage.TitleImageRectangle.RectangleInLayout(x, y, width, height));
            TitleLabel.Layout(DFDesignMainPage.TitleLabelRectangle.RectangleInLayout(x, y, width, height));
            SearchImage.Layout(DFDesignMainPage.SearchImageRectangle.RectangleInLayout(x, y, width, height));

            BackImage.Layout(DFDesignMainPage.BackImageRectangle.RectangleInLayout(x, y, width, height));
            PickerSearchBar.Layout(DFDesignMainPage.PickerSearchBarRectangle.RectangleInLayout(x, y, width, height));
        }
    }
}