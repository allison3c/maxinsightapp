using MaxInsight.Mobile;
using MaxInsight.Mobile.iOS;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ImageGesture), typeof(ImageGestureRenderer))]
namespace MaxInsight.Mobile.iOS
{
    public class ImageGestureRenderer : ViewRenderer<ImageGesture, ImageGestureView>
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="ImageGestureRenderer"/> class.
		/// </summary>
		public ImageGestureRenderer()
        {
        }

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<ImageGesture> e)
        {
            base.OnElementChanged(e);

            var imageGestureView = new ImageGestureView(e.NewElement.ItemsSource.ToString());
            Bind(e.NewElement);
            SetNativeControl(imageGestureView);

        }
        /// <summary>
        /// Binds the specified new element.
        /// </summary>
        /// <param name="newElement">The new element.</param>
        private void Bind(ImageGesture newElement)
        {
            if (newElement != null)
            {
                newElement.PropertyChanging += ElementPropertyChanging;
                newElement.PropertyChanged += ElementPropertyChanged;

            }
        }

        /// <summary>
        /// Elements the property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {

            }
        }
        /// <summary>
        /// Elements the property changing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangingEventArgs"/> instance containing the event data.</param>
        private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {

            }
        }
    }
}
