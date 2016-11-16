namespace Xamarin.Forms.MCS.FaceRecog
{
    public partial class RegisterPersonPage : ContentPage
    {
        public RegisterPersonPage()
        {
            InitializeComponent();
            BindingContext = new RegisterPersonViewModel();
        }
    }
}