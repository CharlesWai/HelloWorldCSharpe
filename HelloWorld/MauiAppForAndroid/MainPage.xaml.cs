namespace MauiAppForAndroid
{
    public partial class MainPage : ContentPage
    {
        private IVibration vibrator;
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            vibrator = Vibration.Default;       
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            try
            {
                vibrator.Vibrate(TimeSpan.FromMilliseconds(1));
            }
            catch (Exception ex)
            {
                
            }
            
            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}