using AbstractGame;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumCore.Tablut
{
    public class ChartViewModel
    {

        private ObservableCollection<KeyValuePair<string, int>> _data;
        public ObservableCollection<KeyValuePair<string, int>> data
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

        public List<KeyValuePair<string, int>> MyValueList
        {
            get
            {
                return ValueList;
            }
        }

        public void UpdateDataForChart(string gameName)
        {
            var heuristics = Client.GetHeuristics(gameName, TablutFieldType.RED_PAWN);
            int i = 0;
            foreach (var heuristic in heuristics)
            {
                _data.Add(new KeyValuePair<string, int>(i.ToString(), heuristic));
                i++;
            }
        }
    }
}
