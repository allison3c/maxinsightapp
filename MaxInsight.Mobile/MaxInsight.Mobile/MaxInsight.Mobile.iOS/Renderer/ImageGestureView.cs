using System;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MaxInsight.Mobile.iOS
{
    public class ImageGestureView : UIView
    {
        /// <summary>
		/// The _page control
		/// </summary>
		private readonly UIPageControl _pageControl;

        /// <summary>
        /// The _scroller
        /// </summary>
        private readonly UIScrollView _scroller;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageGestureView"/> class.
        /// </summary>
        /// <param name="images">The images.</param>
        public ImageGestureView(string images)
            : this(default(CGRect), images)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageGestureView"/> class.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="images">The images.</param>
        public ImageGestureView(CGRect frame, string images = null)
            : base(frame)
        {
            //AutoresizingMask = UIViewAutoresizing.All;
            ContentMode = UIViewContentMode.ScaleToFill;

            FadeImages = true;
            BackgroundColor = UIColor.Black;
            Frame = new CGRect(UIScreen.MainScreen.Bounds.X,
                               UIScreen.MainScreen.Bounds.Y,
                               UIScreen.MainScreen.Bounds.Width,
                               UIScreen.MainScreen.Bounds.Height);

            //Frame = frame == default(CGRect) ? UIScreen.MainScreen.Bounds : frame;

            Images = images ?? "";

            _pageControl = new UIPageControl
            {
                AutoresizingMask = UIViewAutoresizing.All,
                ContentMode = UIViewContentMode.ScaleToFill
            };

            _pageControl.ValueChanged += (object sender, EventArgs e) => UpdateScrollPositionBasedOnPageControl();


            _scroller = new UIScrollView
            {
                AutoresizingMask = UIViewAutoresizing.All,
                ContentMode = UIViewContentMode.ScaleToFill,
                PagingEnabled = true,
                Bounces = false,
                ShowsHorizontalScrollIndicator = false,
                ShowsVerticalScrollIndicator = false
            };

            Add(_scroller);
            //Add(_pageControl);
        }

        /// <summary>
        /// Gets or sets a value indicating whether [fade images].
        /// </summary>
        /// <value><c>true</c> if [fade images]; otherwise, <c>false</c>.</value>
        public bool FadeImages { get; set; }

        /// <summary>
        /// Gets or sets the images.
        /// </summary>
        /// <value>The images.</value>
        public string Images { get; set; }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Images = "";
        }

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw(CGRect rect)
        {
            //TODO: need to remove observer if using the lambda?
            NSNotificationCenter.DefaultCenter.AddObserver(
                UIApplication.DidChangeStatusBarOrientationNotification,
                not =>
                {
                    var orientation = UIDevice.CurrentDevice.Orientation;
                    /***
                    if ((UIDeviceOrientation.LandscapeLeft == orientation || UIDeviceOrientation.LandscapeRight == orientation))
                    {
                        _scroller.ContentSize = new CGSize(Frame.Height * Images.Count - 1, Frame.Width);
                    }
                    else
                    {
                        _scroller.ContentSize = new CGSize(rect.Width * Images.Count - 1, rect.Height);
                    }
                    *****/
                    UpdateScrollPositionBasedOnPageControl();
                });
            //_pageControl.Frame = new CGRect(rect.Left, rect.Height - 40, rect.Width, 40);
            _scroller.Frame = new CGRect(rect.Left, rect.Top, rect.Width, rect.Height);

            AddImage(rect, 0, Images);
            _scroller.ContentSize = new CGSize(_scroller.Frame.Width * 0 - 1, _scroller.Frame.Height);
            _pageControl.Pages = 0;

            base.Draw(rect);
        }

        /// <summary>
        /// Adds the image.
        /// </summary>
        /// <param name="rect">The rect.</paramAddImage>
        /// <param name="position">The position.</param>
        /// <param name="im">The im.</param>
        private void AddImage(CGRect rect, nint position, string im)
        {
            var img = new UIImage();
            var isRemote = GestureHelpers.IsValidUrl(im);

            if (isRemote)
            {
                //dont await , fire and forget
                LoadImageAsync(position, im);
            }
            else
            {
                var fileName = im;
                if (NSFileManager.DefaultManager.FileExists(fileName))
                {
                    var url = new NSUrl(fileName, true);
                    using (var data = NSData.FromUrl(url))
                    {
                        img = UIImage.LoadFromData(data);
                    }
                }
                //img = UIImage.FromFile(im);
            }

            UIImageView imgView;
            nfloat x;
            nfloat y;
            nfloat widht;
            nfloat height;
            try
            {
                imgView = new UIImageView(img)
                {
                    AutoresizingMask = UIViewAutoresizing.All,
                    ContentMode = UIViewContentMode.ScaleToFill,
                    MultipleTouchEnabled = true, //add 16-12-09
                    UserInteractionEnabled = true //add 16-12-09
                };

                if (FadeImages)
                {
                    imgView.Alpha = 0;
                }

                //if first image is local, fade it in
                if (position == 0 && !isRemote)
                {
                    FadeImageViewIn(imgView);
                }

                //imgView.Frame = new CGRect(rect.Width * position, rect.Top, rect.Width, rect.Height);

                if (img.Size.Height > img.Size.Width)
                {
                    widht = img.Size.Width * rect.Height / img.Size.Height;
                    height = rect.Height;

                    x = (rect.Width - widht) / 2;
                    y = 0;
                }
                else
                {
                    widht = rect.Width;
                    height = img.Size.Height * rect.Width / img.Size.Width;
                    x = 0;
                    y = (rect.Height - height) / 2;
                }
                imgView.Frame = new CGRect(x, y, widht, height);

                //add 16-12-09
                imgView.AddGestureRecognizer(new UIPinchGestureRecognizer((UIPinchGestureRecognizer obj) =>
                {
                    if (obj.State == UIGestureRecognizerState.Began || obj.State == UIGestureRecognizerState.Changed)
                    {
                        obj.View.Transform *= CGAffineTransform.MakeScale(obj.Scale, obj.Scale);
                        obj.Scale = 1;
                    }
                }));

                imgView.AddGestureRecognizer(new UIPanGestureRecognizer((UIPanGestureRecognizer gestureRecognizer) =>
                {
                    AdjustAnchorPointForGestureRecognizer(gestureRecognizer);
                    var image = gestureRecognizer.View;
                    if (gestureRecognizer.State == UIGestureRecognizerState.Began || gestureRecognizer.State == UIGestureRecognizerState.Changed)
                    {
                        var translation = gestureRecognizer.TranslationInView(this);
                        image.Center = new CGPoint(image.Center.X + translation.X, image.Center.Y + translation.Y);
                    // Reset the gesture recognizer's translation to {0, 0} - the next callback will get a delta from the current position.
                    gestureRecognizer.SetTranslation(CGPoint.Empty, image);
                    }
                }));

                _scroller.AddSubview(imgView);
            }
            catch (Exception)
            {
            }
        }

        void AdjustAnchorPointForGestureRecognizer(UIGestureRecognizer gestureRecognizer)
        {
            if (gestureRecognizer.State == UIGestureRecognizerState.Began)
            {
                var image = gestureRecognizer.View;
                var locationInView = gestureRecognizer.LocationInView(image);
                var locationInSuperview = gestureRecognizer.LocationInView(image.Superview);

                image.Layer.AnchorPoint = new CGPoint(locationInView.X / image.Bounds.Size.Width, locationInView.Y / image.Bounds.Size.Height);
                image.Center = locationInSuperview;
            }
        }

        /// <summary>
        /// Loads the image asynchronous.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="url">The URL.</param>
        /// <returns>Task.</returns>
        private Task LoadImageAsync(nint position, string url)
        {
            return Task.Run(
                () =>
                {
                    var img = GestureHelpers.LoadFromUrl(url);

                    InvokeOnMainThread(
                        () =>
                        {
                            var imgView = _scroller.Subviews[position] as UIImageView;
                            if (_pageControl.CurrentPage == position && FadeImages)
                            {
                                FadeImageViewIn(imgView, img);
                            }
                            else
                            {
                                imgView.Image = img;
                            }
                        });
                });
        }

        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="imgView">The img view.</param>
        /// <param name="img">The img.</param>
        private void SetImage(UIImageView imgView, UIImage img)
        {
            if (img != null)
            {
                imgView.Image = img;
            }
            imgView.Alpha = 1;
        }

        /// <summary>
        /// Updates the scroll position based on page control.
        /// </summary>
        private void UpdateScrollPositionBasedOnPageControl()
        {
            var off = _pageControl.CurrentPage * _scroller.Frame.Width;
            _scroller.SetContentOffset(new CGPoint(off, 0), true);
        }

        /// <summary>
        /// Fades the image view in.
        /// </summary>
        /// <param name="imgView">The img view.</param>
        /// <param name="img">The img.</param>
        private void FadeImageViewIn(UIImageView imgView, UIImage img = null)
        {
            if (FadeImages)
            {
                Animate(0.3, 0, UIViewAnimationOptions.TransitionCrossDissolve, () => { SetImage(imgView, img); }, () => { });
            }
            else
            {
                SetImage(imgView, img);
            }
        }
    }

    /// <summary>
	/// Class GestureHelpers.
	/// </summary>
	public class GestureHelpers
    {
        /// <summary>
        /// Determines whether [is valid URL] [the specified URL string].
        /// </summary>
        /// <param name="urlString">The URL string.</param>
        /// <returns><c>true</c> if [is valid URL] [the specified URL string]; otherwise, <c>false</c>.</returns>
        public static bool IsValidUrl(string urlString)
        {
            Uri uri;
            return Uri.TryCreate(urlString, UriKind.Absolute, out uri)
                   && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeFtp
                       || uri.Scheme == Uri.UriSchemeMailto);
        }

        /// <summary>
        /// Loads from URL.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>UIImage.</returns>
        public static UIImage LoadFromUrl(string uri)
        {
            using (var url = new NSUrl(uri)) using (var data = NSData.FromUrl(url)) return UIImage.LoadFromData(data);
        }
    }
}
