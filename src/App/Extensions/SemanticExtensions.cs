#if __IOS__ || MACCATALYST

using UIKit;
using PlatformView = UIKit.UIView;

#elif __ANDROID__

using Android.Views;
using Android.Views.Accessibility;
using PlatformView = Android.Views.View;

#endif

namespace App.Extensions;

public static class SemanticExtensions
{
	public static void SetSemanticFocusForcefully(this IView element)
	{
		if(element?.Handler?.PlatformView is not PlatformView platformView)
		{
			throw new NullReferenceException("Can't access view from a null handler");
		}

#if __ANDROID__
		platformView.SendAccessibilityEvent(EventTypes.ViewAccessibilityFocused);
#elif __IOS__ || MACCATALYST
		UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, platformView);
#endif
	}

	public static void HideForAccessibility(this IView view)
	{
#if ANDROID
		if(view.Handler?.PlatformView is Android.Views.View nativeView)
		{
			// Screen reader
			nativeView.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;

			// Keyboard navigation
			((ViewGroup)nativeView).DescendantFocusability = DescendantFocusability.BlockDescendants;
		}
#endif

#if IOS
		if(view.Handler?.PlatformView is UIView nativeView)
		{
			// Screen reader
			nativeView.AccessibilityElementsHidden = true;

			// Keyboard navigation
			nativeView.UserInteractionEnabled = false;
		}
#endif
	}

	public static void ShowForAccessibility(this IView view)
	{
#if ANDROID
		if(view.Handler?.PlatformView is Android.Views.View nativeView)
		{
			// Screen reader
			nativeView.ImportantForAccessibility = ImportantForAccessibility.Auto;

			// Keyboard navigation
			((ViewGroup)nativeView).DescendantFocusability = DescendantFocusability.AfterDescendants;
		}
#endif

#if IOS
		if(view.Handler?.PlatformView is UIView nativeView)
		{
			// Screen reader
			nativeView.AccessibilityElementsHidden = false;

			// Keyboard navigation
			nativeView.UserInteractionEnabled = true;
		}
#endif
	}
}