using AbstractGame;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumCore
{
    public class ChartViewModel
    {

        private ObservableCollection<KeyValuePair<string, int>> _data;
        public ObservableCollection<KeyValuePair<string, int>> Data
        {
            get { return _data; }
        }

        public List<KeyValuePair<string, int>> ValueList { get; set; }
        Client Client { get; set; }

        public ChartViewModel(Client client) {
            Client = client;
            _data = new ObservableCollection<KeyValuePair<string, int>>();
        }

        public ChartViewModel()
        {
            ValueList = new List<KeyValuePair<string, int>>();
            _data = new ObservableCollection<KeyValuePair<string, int>>();
        }

        public void DrawChartWithHeuristics(string gameName, FieldType colorOfPawn)
        {
            var values = Client.GetHeuristics(gameName, colorOfPawn);
            int i = 0;
            _data.Clear();
            foreach (var value in values)
            {
                _data.Add(new KeyValuePair<string, int>(i.ToString(), value));
                i++;
            }
        }

        public void DrawChartWithTakenPawns(string gameName, FieldType colorOfPawn)
        {
            var values = Client.GetTakenPawns(gameName, colorOfPawn);
            int i = 0;
            _data.Clear();
            foreach (var value in values)
            {
                _data.Add(new KeyValuePair<string, int>(i.ToString(), value));
                i++;
            }
        }
    }
}
