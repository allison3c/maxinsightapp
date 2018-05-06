using System;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace MaxInsight.Mobile
{
	public class CustomCacheKeyFactory : ICacheKeyFactory
	{
		public string GetKey(ImageSource imageSource, object bindingContext)
		{
			if (imageSource == null)
				return null;

			string itemSuffix = string.Empty;
			var bindingItem = bindingContext as StandardPic;

			if (bindingItem != null)
				itemSuffix = bindingItem.PicName;

			// UriImageSource
			var uriImageSource = imageSource as UriImageSource;
			if (uriImageSource != null)
				return string.Format("{0}+myCustomUriSuffix+{1}", uriImageSource.Uri, itemSuffix);

			// FileImageSource
			var fileImageSource = imageSource as FileImageSource;
			if (fileImageSource != null)
				return string.Format("{0}+myCustomFileSuffix+{1}", fileImageSource.File, itemSuffix);

			// StreamImageSource
			var streamImageSource = imageSource as StreamImageSource;
			if (streamImageSource != null)
				return string.Format("{0}+myCustomStreamSuffix+{1}", streamImageSource.Stream.GetHashCode(), itemSuffix);

			throw new NotImplementedException("ImageSource type not supported");
		}
	}
}
