using System.Collections;
using Xamarin.Forms;

namespace MaxInsight.Mobile
{
    public class ImageGesture : View
    {
        public ImageGesture()
        {
        }

        /// <summary>
        /// The items source property
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(string),
                                    typeof(ImageGesture), null,
                                    BindingMode.OneWay, null, null, null, null);

        /// <summary>
        /// The item template property
        /// </summary>
        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create("ItemTemplate", typeof(DataTemplate),
                                    typeof(ImageGesture), null,
                                    BindingMode.OneWay, null, null, null, null);



        // Properties
        //
        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public string ItemsSource
        {
            get
            {
                return (string)base.GetValue(ImageGallery.ItemsSourceProperty);
            }
            set
            {
                base.SetValue(ImageGallery.ItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the item template.
        /// </summary>
        /// <value>The item template.</value>
        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)base.GetValue(ImageGallery.ItemTemplateProperty);
            }
            set
            {
                base.SetValue(ImageGallery.ItemTemplateProperty, value);
            }
        }
    }
}
