using System;
using Xamarin.Forms;
using Dreamfora;

namespace BottomSheet
{
    public class NathanBottomSheet : DFALayout
    {
        #region Declare

        protected readonly BottomPanContainer BottomPanContainer;
        protected readonly BoxView Background;

        #endregion Declare

        #region Constructor

        protected NathanBottomSheet()
        {
            IsVisible = false;

            BottomPanContainer = new BottomPanContainer();

            BottomPanContainer.PickerFullDownEvent += PickerFullDown;

            var backgroundGesture = new TapGestureRecognizer();
            backgroundGesture.Tapped += (sender, e) =>
            {
                if (IsVisible)
                {
                    InVisiblePicker();
                }
            };

            Background = new BoxView
            {
                Opacity = 0,
                BackgroundColor = Color.FromHex("#AAC2C2C2"),
                GestureRecognizers = {backgroundGesture}
            };


            Children.Add(Background);
            Children.Add(BottomPanContainer);
        }

        #endregion Constructor

        #region Methods

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            Background.Layout(DFDesignMainPage.fullScreenRectangle.RectangleInLayout(x, y));
            BottomPanContainer.Layout(PickerRectangle.RectangleInLayoutFromBottom(x, y, width, height));
        }

        public virtual void VisiblePicker()
        {
            if (IsVisible) return;
            IsVisible = true;

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

        public virtual void InVisiblePicker()
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
        }

        private void PickerFullDown(object sender, EventArgs e)
        {
            if (IsVisible)
            {
                InVisiblePicker();
            }
        }

        #endregion Methods

        private static readonly BindableProperty PickerRectangleProperty = BindableProperty.Create(
            nameof(PickerRectangle), typeof(DFDesignRectangleFromBottom375),
            typeof(NathanBottomSheet), DFDesignMainPage.bottomPanContainerRectangle,
            propertyChanged: OnPickerRectangleChanged);

        public DFDesignRectangleFromBottom375 PickerRectangle
        {
            get => (DFDesignRectangleFromBottom375) GetValue(PickerRectangleProperty);
            set => SetValue(PickerRectangleProperty, value);
        }

        private static void OnPickerRectangleChanged(object bindable, object oldValue, object newValue)
        {
            ((NathanBottomSheet) bindable).BottomPanContainer.PickerY = ((DFDesignRectangleFromBottom375) newValue).Y;
        }
    }


    public class BottomPanContainer : ContentView
    {
        #region Declare

        public readonly Frame BottomSheetFrame;

        public double _y;
        private bool _up, _down;
        private bool _fullDown;

        public readonly StackLayout FrameContentStackLayout;

        public event EventHandler PickerFullDownEvent; // 피커를 내렸을 때의 이벤트 핸들러

        public double PickerY = DFDesignMainPage.bottomPanContainerRectangle.Y; // 피커의 높이 값

        #endregion Declare


        #region Constructor

        public BottomPanContainer()
        {
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);


            FrameContentStackLayout = new StackLayout();

            BottomSheetFrame = new Frame
            {
                CornerRadius = (float) DFDesignMainPage.bottomSheetRadius.Value,
                HasShadow = false,
                Padding = new Thickness(0, 0, 0, 20),
                Content = FrameContentStackLayout
            };

            Content = BottomSheetFrame;
        }

        #endregion Constructor


        #region Function

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:

                    BottomSheetFrame.TranslationY = Math.Max(_y + e.TotalY, 0);

                    if (e.TotalY < 0)
                    {
                        _down = false;
                        _up = true;
                    }
                    else
                    {
                        _up = false;
                        _down = true;
                    }

                    _fullDown = e.TotalY >= PickerY / 3;

                    break;

                case GestureStatus.Completed:

                    _y = BottomSheetFrame.TranslationY;


                    if (_up)
                    {
                        BottomSheetFrame.TranslateTo(BottomSheetFrame.X, _y = 0, 250, Easing.CubicOut);
                    }

                    else if (_down)
                    {
                        if (_fullDown)
                        {
                            PickerFullDownEvent?.Invoke(this, EventArgs.Empty);
                        }
                        else // 완전히 내리지 않았을 경우 -> 올라감
                        {
                            BottomSheetFrame.TranslateTo(BottomSheetFrame.X, _y = 0, 250, Easing.CubicOut);
                        }
                    }

                    break;
                case GestureStatus.Started:
                    break;
                case GestureStatus.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion Function
    }
}