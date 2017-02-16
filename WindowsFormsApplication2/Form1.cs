using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1(Market market)
        {
            InitializeComponent();
            int type = 0;
            foreach (CommodityType c in Enum.GetValues(typeof(CommodityType)))
            {
                List<Commodity> CommodityData = market.GraphData.Where(p => p.Type == c).ToList();
                int x = 0;
                foreach(Commodity com in CommodityData)
                {
                    chart1.Series[type].Points.AddXY(x++, com.max);
                }
                type++;
            }
            type = 0;
            foreach (Occupation o in Enum.GetValues(typeof(Occupation)))
            {
                List<OccupationData> CommodityData = market.OccupationD.Where(p => p.job == o).ToList();
                int x = 0;
                foreach (OccupationData com in CommodityData)
                {
                    chart2.Series[type].Points.AddXY(com.day, com.workers);
                }
                type++;
            }
        }
    }
}
