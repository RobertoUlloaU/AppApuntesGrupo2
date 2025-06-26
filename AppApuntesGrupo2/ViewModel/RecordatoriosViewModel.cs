using AppApuntesGrupo2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppApuntesGrupo2.ViewModel
{
    public class RecordatoriosViewModel : BaseViewModel
    {
        private string _nuevoTexto;
        private TimeSpan _nuevaFechaHora;

        private const string FileName = "recordatorios.json";
        private string FilePath => Path.Combine(FileSystem.AppDataDirectory, FileName);

        public ObservableCollection<Recordatorio> Recordatorios { get; set; } = new();

        public string NuevoTexto
        {
            get => _nuevoTexto;
            set => SetProperty(ref _nuevoTexto, value);
        }

        public TimeSpan NuevaFechaHora
        {
            get => _nuevaFechaHora;
            set => SetProperty(ref _nuevaFechaHora, value);
        }

        public ICommand AgregarCommand { get; }
        public ICommand EliminarCommand { get; }

        public RecordatoriosViewModel()
        {
            AgregarCommand = new Command(AgregarRecordatorio);
            EliminarCommand = new Command<Recordatorio>(EliminarRecordatorio);
            CargarRecordatorios();
        }

        private void AgregarRecordatorio()
        {
            if (string.IsNullOrWhiteSpace(NuevoTexto))
                return;

            Recordatorios.Add(new Recordatorio
            {
                Texto = NuevoTexto,
                FechaHora = NuevaFechaHora,
                Activo = true
            });

            GuardarRecordatorios();
            NuevoTexto = string.Empty;
            NuevaFechaHora = TimeSpan.Zero;
        }

        private void EliminarRecordatorio(Recordatorio recordatorio)
        {
            if (Recordatorios.Contains(recordatorio))
            {
                Recordatorios.Remove(recordatorio);
                GuardarRecordatorios();
            }
        }

        private async void CargarRecordatorios()
        {
            if (File.Exists(FilePath))
            {
                string json = await File.ReadAllTextAsync(FilePath);
                var lista = JsonSerializer.Deserialize<List<Recordatorio>>(json);
                if (lista != null)
                {
                    foreach (var rec in lista)
                        Recordatorios.Add(rec);
                }
            }
        }

        private async void GuardarRecordatorios()
        {
            string json = JsonSerializer.Serialize(Recordatorios.ToList());
            await File.WriteAllTextAsync(FilePath, json);
        }
    }
}
