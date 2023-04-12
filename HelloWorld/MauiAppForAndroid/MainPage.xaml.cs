using Xamarin.Android;
using Xamarin.Kotlin;
using Android.OS;
using Android.Content;

namespace MauiAppForAndroid
{
    public partial class MainPage : ContentPage
    {
        private Vibrator vibrator;
        int count = 0;

        public MainPage()
        {
            this.BindingContext = this;
            InitializeComponent();
            vibrator = Vibrator.FromContext((Context)this.BindingContext);
            
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            vibrator.Vibrate(VibrationEffect.EffectClick);
            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}