using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace WhereUAt.Ninja.Mobile
{
    public class StatusPage : ContentPage
    {
        private StatusPageViewModel viewModel;

        public StatusPage()
        {
            viewModel = new StatusPageViewModel();

            Label header = new Label
            {
                Text = "Status",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            TextCell storedRequestsTextCell = new TextCell
            {
                Text = "Number of Requests Stored:",
                Detail = "With Detail Text",
            };

            storedRequestsTextCell.SetBinding(TextCell.DetailProperty, "StoredRequestsCount");
            storedRequestsTextCell.BindingContext = viewModel;

            TextCell lastSentTextCell = new TextCell
            {
                Text = "Last Location Successfully Sent:",
                Detail = "",
            };

            lastSentTextCell.SetBinding(TextCell.DetailProperty, "LastSentDateTime");
            lastSentTextCell.BindingContext = viewModel;

            TableView tableView = new TableView
            {
                Root = new TableRoot {
                        new TableSection("") {
                            storedRequestsTextCell,
                            lastSentTextCell
                        }
                },
                Intent = TableIntent.Settings
            };

            Content = new StackLayout
            {
                Children = {
                    header,
                    tableView
                }
            };

        }

        

    }
}
