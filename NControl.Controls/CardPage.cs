﻿using System;
using Xamarin.Forms;
using NControl.Abstractions;
using System.Threading.Tasks;

namespace NControl.Controls
{
	/// <summary>
	/// Card page. Based on custom transitions 
	/// </summary>
	public class CardPage: ContentPage
	{				
		#region Private Members

		/// <summary>
		/// The height of the requested.
		/// </summary>
		private double _requestedHeight;

		/// <summary>
		/// The width of the requested.
		/// </summary>
		private double _requestedWidth;

		/// <summary>
		/// The helper.
		/// </summary>
		private ICardPageHelper _platformHelper;

		/// <summary>
		/// The layout.
		/// </summary>
		private readonly RelativeLayout _layout;

		/// <summary>
		/// The overlay.
		/// </summary>
		private readonly BoxView _overlay;

		/// <summary>
		/// The overlay.
		/// </summary>
		private readonly NControlView _contentView;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.CardPage"/> class.
		/// </summary>
		public CardPage()
		{					
			_platformHelper = DependencyService.Get<ICardPageHelper> ();

			CardPadding = new Thickness (40, 100, 40, 200);
			BackgroundColor = Color.Transparent;

			NavigationPage.SetHasNavigationBar (this, false);
			NavigationPage.SetHasBackButton (this, false);

			_layout = new RelativeLayout ();

			base.Content = _layout;

			// Card 
			_contentView = new RoundCornerView {
				BackgroundColor = Color.White,
				CornerRadius = 4,
			};
				
			_overlay = new BoxView {
				BackgroundColor = Color.Black,
				Opacity = 0.0F,
			};

			_layout.Children.Add (_overlay, () => _layout.Bounds);
			_layout.Children.Add(_contentView, ()=> new Rectangle (
				CardPadding.Left,
				CardPadding.Top, 
				_layout.Width - (CardPadding.Right + CardPadding.Left), 
				_layout.Height - (CardPadding.Bottom + CardPadding.Top)));

			_contentView.TranslationY = _platformHelper.GetScreenSize().Height - (CardPadding.Top);
		}

		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			_overlay.FadeTo (0.2F);
			_contentView.TranslateTo (0.0, 0.0, 250, Easing.CubicInOut);
		}

		#region Properties

		/// <summary>
		/// Gets or sets the height of the requested.
		/// </summary>
		/// <value>The height of the requested.</value>
		public virtual double RequestedHeight 
		{			
			get { return _requestedHeight; }
			set 
			{
				var height = value;
				var padding = (_platformHelper.GetScreenSize().Height - height)/2;
				CardPadding = new Thickness (CardPadding.Left, padding, CardPadding.Right, padding);

				_requestedHeight = value;
				InvalidateMeasure ();
			}
		}

		/// <summary>
		/// Gets or sets the width of the requested.
		/// </summary>
		/// <value>The width of the requested.</value>
		public virtual double RequestedWidth 
		{			
			get { return _requestedWidth; }
			set 
			{
				var padding = (_platformHelper.GetScreenSize().Width - value)/2;
				CardPadding = new Thickness (padding, CardPadding.Top, padding, CardPadding.Bottom);

				_requestedWidth = value;
				InvalidateMeasure ();
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the View element representing the content of the Page.
		/// </summary>
		/// <value>The content.</value>
		public new View Content
		{
			get { return _contentView.Content; }
			set { _contentView.Content = value; }
		}

		#endregion

		#region Overridable Members

		/// <summary>
		/// Defines the insets/padding for the card
		/// </summary>
		/// <value>The card insets.</value>
		public Thickness CardPadding {
			get;
			set;
		}

		#endregion
	}
}

