using System;
using Xamarin.Forms;

namespace TicTacToe.Client
{
    public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			this.InitializeComponent();

            Board.Default.Reloaded += OnBoardReloaded;

            GetButton("btnRow1Column1").Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 1, coordinateY: 1)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            GetButton("btnRow1Column2").Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 1, coordinateY: 2)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            GetButton("btnRow1Column3").Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 1, coordinateY: 3)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            GetButton("btnRow2Column1").Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 2, coordinateY: 1)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            GetButton("btnRow2Column2").Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 2, coordinateY: 2)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            GetButton("btnRow2Column3").Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 2, coordinateY: 3)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            GetButton("btnRow3Column1").Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 3, coordinateY: 1)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            GetButton("btnRow3Column2").Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 3, coordinateY: 2)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            GetButton("btnRow3Column3").Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 3, coordinateY: 3)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
        }

        void OnBoardReloaded (object sender, EventArgs e)
        {
            GetButton("btnRow1Column1").Text = Board.Default.GetValue(coordinateX: 1, coordinateY: 1);
            GetButton("btnRow1Column2").Text = Board.Default.GetValue(coordinateX: 1, coordinateY: 2);
            GetButton("btnRow1Column3").Text = Board.Default.GetValue(coordinateX: 1, coordinateY: 3);
            GetButton("btnRow2Column1").Text = Board.Default.GetValue(coordinateX: 2, coordinateY: 1);
            GetButton("btnRow2Column2").Text = Board.Default.GetValue(coordinateX: 2, coordinateY: 2);
            GetButton("btnRow2Column3").Text = Board.Default.GetValue(coordinateX: 2, coordinateY: 3);
            GetButton("btnRow3Column1").Text = Board.Default.GetValue(coordinateX: 3, coordinateY: 1);
            GetButton("btnRow3Column2").Text = Board.Default.GetValue(coordinateX: 3, coordinateY: 2);
            GetButton("btnRow3Column3").Text = Board.Default.GetValue(coordinateX: 3, coordinateY: 3);
        }

        Button GetButton(string name) => this.FindByName<Button>(name);
    }
}
