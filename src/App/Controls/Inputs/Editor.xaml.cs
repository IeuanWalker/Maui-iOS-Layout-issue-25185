using System.Runtime.CompilerServices;
using App.Resources.Languages;
using CommunityToolkit.Maui.Behaviors;

namespace App.Controls.Inputs;

public partial class Editor : ContentView
{
	#region Properties

	public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(Editor), string.Empty, BindingMode.TwoWay, null, HandleBindingPropertyChangedDelegate);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(Editor), string.Empty, BindingMode.OneTime);

	public string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}

	public static readonly BindableProperty TextTransformProperty = BindableProperty.Create(nameof(TextTransform), typeof(TextTransform), typeof(Editor), TextTransform.Default, BindingMode.OneTime);

	public TextTransform TextTransform
	{
		get => (TextTransform)GetValue(TextTransformProperty);
		set => SetValue(TextTransformProperty, value);
	}

	public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(Editor), Keyboard.Default, BindingMode.OneTime, coerceValue: (_, v) => (Keyboard)v ?? Keyboard.Default);

	public Keyboard Keyboard
	{
		get => (Keyboard)GetValue(KeyboardProperty);
		set => SetValue(KeyboardProperty, value);
	}

	public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(Editor), int.MaxValue, BindingMode.OneTime);

	public int MaxLength
	{
		get => (int)GetValue(MaxLengthProperty);
		set => SetValue(MaxLengthProperty, value);
	}

	public static readonly BindableProperty ErrorProperty = BindableProperty.Create(nameof(Error), typeof(bool), typeof(Editor), false, BindingMode.TwoWay, null);

	public bool Error
	{
		get => (bool)GetValue(ErrorProperty);
		set => SetValue(ErrorProperty, value);
	}

	public static readonly BindableProperty ErrorsProperty = BindableProperty.Create(nameof(Errors), typeof(ICollection<string>), typeof(Editor), new List<string>(), BindingMode.TwoWay);

	public ICollection<string> Errors
	{
		get => (ICollection<string>)GetValue(ErrorsProperty);
		set => SetValue(ErrorsProperty, value);
	}

	public static readonly BindableProperty ShowCharacterCountProperty = BindableProperty.Create(nameof(ShowCharacterCount), typeof(bool), typeof(Editor), false, BindingMode.TwoWay);

	public bool ShowCharacterCount
	{
		get => (bool)GetValue(ShowCharacterCountProperty);
		set => SetValue(ShowCharacterCountProperty, value);
	}

	public static readonly BindableProperty AutoSizeProperty = BindableProperty.Create(nameof(AutoSize), typeof(EditorAutoSizeOption), typeof(Editor), EditorAutoSizeOption.Disabled, BindingMode.OneTime);

	public EditorAutoSizeOption AutoSize
	{
		get => (EditorAutoSizeOption)GetValue(AutoSizeProperty);
		set => SetValue(AutoSizeProperty, value);
	}

	#endregion Properties

	#region Events

	public event EventHandler? Completed;

	public event EventHandler? TextChanged;

	#endregion Events

	public Editor()
	{
		InitializeComponent();

		EditorField.Behaviors.Add(new UserStoppedTypingBehavior()
		{
			StoppedTypingTimeThreshold = 1000,
			MinimumLengthThreshold = 4,
			ShouldDismissKeyboardAutomatically = false,
			Command = new Command(OnUserStoppedTyping)
		});

		SemanticOrderView.ViewOrder = new List<View> { HiddenLabel, EditorField, LblCounterRepo, LblError };
	}

	void Handle_Focused(object sender, FocusEventArgs e)
	{
		if(string.IsNullOrEmpty(Text))
		{
			TransitionToTitle();
		}

		EditorFrame.SetDynamicResource(Border.StrokeProperty, Errors.Count > 0 ? "WarningColor" : "EntryBorderColour");
		HiddenLabel.SetDynamicResource(Label.TextColorProperty, "TextSecondaryColour");
	}

	void Handle_Unfocused(object sender, FocusEventArgs e)
	{
		if(string.IsNullOrEmpty(Text))
		{
			TransitionToPlaceholder();
		}

		EditorFrame.SetDynamicResource(Border.StrokeProperty, Errors.Count > 0 ? "WarningColor" : "UnfocusedColour");
		HiddenLabel.SetDynamicResource(Label.TextColorProperty, "TextPrimaryColour");
	}

	void Handle_OnCompleted(object sender, EventArgs e)
	{
		Completed?.Invoke(this, e);
	}

	void Handle_TextChanged(object sender, TextChangedEventArgs e)
	{
		TextChanged?.Invoke(this, e);
		CharacterLimitText();
	}

	void OnUserStoppedTyping()
	{
		if(ShowCharacterCount && MaxLength != int.MaxValue)
		{
			int characterRemaining = MaxLength - Text.Length;
			SemanticScreenReader.Default.Announce($"{characterRemaining} {AppResources.AccessibilityCharacterCount}");
		}
	}

	void TransitionToTitle()
	{
		HiddenLabel.Opacity = 0;
		HiddenLabel.IsVisible = true;

		Animation toTitleAnimation = new()
		{
			{ 0.5, 1, new Animation(_ => HiddenLabel.Opacity = _, 0, 1, Easing.Linear) },
			{ 0, 1, new Animation(_ => HiddenLabel.TranslationY = _, 75, 0, Easing.Linear) },
		};
		toTitleAnimation.Commit(this, nameof(toTitleAnimation), 16, 200, finished: (_, _) => EditorField.Placeholder = null);
	}

	void TransitionToPlaceholder()
	{
		Animation toPlaceholderAnimation = new()
		{
			{ 0, 0.7, new Animation(_ => HiddenLabel.Opacity = _, 1, 0, Easing.Linear) },
			{ 0, 1, new Animation(_ => HiddenLabel.TranslationY = _, 0, 75, Easing.Linear) },
		};
		toPlaceholderAnimation.Commit(this, nameof(toPlaceholderAnimation), 16, 200, finished: (_, _) =>
		{
			HiddenLabel.IsVisible = false;
			EditorField.Placeholder = Placeholder;
		});
	}

	void CharacterLimitText()
	{
		if(ShowCharacterCount)
		{
			SemanticProperties.SetDescription(LblCounterRepo, string.Format(AppResources.CharactersRemaining, Text?.Length ?? 0, MaxLength));

			LblCounterRepo.Text = $"{Text?.Length ?? 0} / {MaxLength}";
		}
	}

	static void HandleBindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
	{
		if(bindable is not Editor control)
		{
			return;
		}

		if(control.EditorField.IsFocused)
		{
			return;
		}

		if(!string.IsNullOrEmpty((string)newValue))
		{
			control.TransitionToTitle();
		}
		else
		{
			control.TransitionToPlaceholder();
		}
	}

	protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		base.OnPropertyChanged(propertyName);

		if(EditorFrame is null || EditorField is null)
		{
			return;
		}

		EditorField.IsEnabled = IsEnabled;

		if(propertyName == nameof(MaxLength))
		{
			UserStoppedTypingBehavior? be = EditorField.Behaviors.OfType<UserStoppedTypingBehavior>().FirstOrDefault();
			if(be is not null)
			{
				be.MinimumLengthThreshold = MaxLength / 2;
			}
		}

		if(Errors.Count > 0)
		{
			EditorFrame.SetDynamicResource(Border.StrokeProperty, "WarningColor");
		}
		else if(EditorField.IsFocused)
		{
			EditorFrame.SetDynamicResource(Border.StrokeProperty, "EntryBorderColour");
		}
		else
		{
			EditorFrame.SetDynamicResource(Border.StrokeProperty, "UnfocusedColour");
		}
	}

	void this_Loaded(object sender, EventArgs e)
	{
		EditorField.HeightRequest = AutoSize.Equals(EditorAutoSizeOption.Disabled) ? 180 : -1;
		CharacterLimitText();
	}
}