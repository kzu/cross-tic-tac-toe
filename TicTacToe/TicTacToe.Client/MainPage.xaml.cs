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
            Board.Default.GameCompleted += OnGameCompleted;

            btnRow1Column1.Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 1, coordinateY: 1)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            btnRow1Column2.Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 1, coordinateY: 2)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            btnRow1Column3.Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 1, coordinateY: 3)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            btnRow2Column1.Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 2, coordinateY: 1)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            btnRow2Column2.Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 2, coordinateY: 2)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            btnRow2Column3.Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 2, coordinateY: 3)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            btnRow3Column1.Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 3, coordinateY: 1)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            btnRow3Column2.Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 3, coordinateY: 2)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
            btnRow3Column3.Clicked += async (sender, e) =>
            {
                await Board
                    .Default
                    .PlayAsync(coordinateX: 3, coordinateY: 3)
                    .ConfigureAwait(continueOnCapturedContext: false);
            };
        }

        void OnBoardReloaded (object sender, EventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    btnRow1Column1.Text = Board.Default.GetValue(coordinateX: 1, coordinateY: 1);
                    btnRow1Column2.Text = Board.Default.GetValue(coordinateX: 1, coordinateY: 2);
                    btnRow1Column3.Text = Board.Default.GetValue(coordinateX: 1, coordinateY: 3);
                    btnRow2Column1.Text = Board.Default.GetValue(coordinateX: 2, coordinateY: 1);
                    btnRow2Column2.Text = Board.Default.GetValue(coordinateX: 2, coordinateY: 2);
                    btnRow2Column3.Text = Board.Default.GetValue(coordinateX: 2, coordinateY: 3);
                    btnRow3Column1.Text = Board.Default.GetValue(coordinateX: 3, coordinateY: 1);
                    btnRow3Column2.Text = Board.Default.GetValue(coordinateX: 3, coordinateY: 2);
                    btnRow3Column3.Text = Board.Default.GetValue(coordinateX: 3, coordinateY: 3);
                });
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                //TODO: Handle exception
            }
        }

        void OnBoardReloaded(object sender, EventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    btnRow1Column1.Text = string.Empty;
                    btnRow1Column2.Text = string.Empty;
                    btnRow1Column3.Text = string.Empty;
                    btnRow2Column1.Text = string.Empty;
                    btnRow2Column2.Text = string.Empty;
                    btnRow2Column3.Text = string.Empty;
                    btnRow3Column1.Text = string.Empty;
                    btnRow3Column2.Text = string.Empty;
                    btnRow3Column3.Text = string.Empty;
                });
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                //TODO: Handle exception
            }
        }
    }
}
