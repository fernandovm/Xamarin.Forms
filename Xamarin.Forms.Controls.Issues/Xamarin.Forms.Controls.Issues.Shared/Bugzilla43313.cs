using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;

namespace Xamarin.Forms.Controls
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 43313, "Adding an item to ListView ItemSource has unexpected animation with different height rows and HasUnevenRows is true")]
	public class Bugzilla43313 : TestContentPage
	{
		int _i;
		ListView _listView;

		protected override void Init()
		{
			_i = 10;
			BindingContext = new _43313ViewModel();

			var btnAdd = new Button
			{
				Text = "Add item",
				WidthRequest = 100
			};
			btnAdd.Clicked += BtnAddOnClicked;

			var btnBottom = new Button
			{
				Text = "Scroll to end",
				WidthRequest = 100
			};
			btnBottom.Clicked += BtnBottomOnClicked;

			var btnPanel = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center,
				Children = { btnAdd, btnBottom }
			};

			_listView = new ListView
			{
				HasUnevenRows = true,
				BackgroundColor = Color.Transparent,
				VerticalOptions = LayoutOptions.FillAndExpand,
				ItemTemplate = new DataTemplate(() =>
				{
					var label = new Label { FontSize = 16, VerticalOptions = LayoutOptions.Center };
					label.SetBinding(Label.TextProperty, "Name");
					int height = 60 + new Random().Next(10, 100);
					return new ViewCell
					{
						Height = height,
						View = new StackLayout
						{
							Padding = new Thickness(0, 5, 0, 5),
							BackgroundColor = Color.Transparent,
							Children =
							{
								label
							}
						}
					};
				})
			};
			_listView.SetBinding(ListView.ItemsSourceProperty, new Binding("ListViewContent"));
			_listView.ItemTapped += (sender, e) => ((ListView)sender).SelectedItem = null;

			var instructions = new Label() { Text = "Tap the 'Add Item' button; a new item should be added to the bottom of the list and the list should scroll smoothly to display it. If the list scrolls back to the top before scrolling down to the new item, the test has failed." };

 			Content = new StackLayout
			{
				Padding = new Thickness(0, 40, 0, 0),
				Children =
				{
					instructions,
					btnPanel,
					_listView
				}
			};
		}

		//async
			void BtnAddOnClicked(object sender, EventArgs eventArgs)
		{
			string str = $"Item {_i++}";
			var item = new _43313Model { Name = str };
			(BindingContext as _43313ViewModel).ListViewContent.Add(item);

			//await Task.Delay(1500);

			_listView.ScrollTo(item, ScrollToPosition.End, true);
		}

		void BtnBottomOnClicked(object sender, EventArgs e)
		{
			_43313Model item = (BindingContext as _43313ViewModel).ListViewContent.Last();
			_listView.ScrollTo(item, ScrollToPosition.End, true);
		}

		[Preserve(AllMembers = true)]
		public class _43313Model
		{
			public string Name { get; set; }
		}

		[Preserve(AllMembers = true)]
		public class _43313ViewModel : INotifyPropertyChanged
		{
			ObservableCollection<_43313Model> listViewContent;

			public _43313ViewModel()
			{
				ListViewContent = new ObservableCollection<_43313Model>();
				listViewContent.Add(new _43313Model { Name = "Item 1" });
				listViewContent.Add(new _43313Model { Name = "Item 2" });
				listViewContent.Add(new _43313Model { Name = "Item 3" });
				listViewContent.Add(new _43313Model { Name = "Item 4" });
				listViewContent.Add(new _43313Model { Name = "Item 5" });
				listViewContent.Add(new _43313Model { Name = "Item 6" });
				listViewContent.Add(new _43313Model { Name = "Item 7" });
				listViewContent.Add(new _43313Model { Name = "Item 8" });
				listViewContent.Add(new _43313Model { Name = "Item 9" });
			}

			public ObservableCollection<_43313Model> ListViewContent
			{
				get { return listViewContent; }
				set
				{
					listViewContent = value;
					OnPropertyChanged();
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

			protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
			{
				PropertyChangedEventHandler handler = PropertyChanged;
				handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}