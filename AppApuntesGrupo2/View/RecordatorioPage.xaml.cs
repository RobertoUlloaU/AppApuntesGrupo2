using AppApuntesGrupo2.ViewModel;

namespace AppApuntesGrupo2.View
{
    public partial class RecordatoriosPage : ContentPage
    {
        public RecordatoriosPage()
        {
            InitializeComponent();
            this.BindingContext = new ViewModel.RecordatoriosViewModel();
        }
    }
}
