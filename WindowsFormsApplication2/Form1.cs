using Example1;
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
                try
                {
                    List<Commodity> CommodityData = market.GraphData.Where(p => p.Type == c).ToList();
                    int x = 0;
                    foreach (Commodity com in CommodityData)
                    {
                        chart1.Series[type].Points.AddXY(x++, com.max);
                    }
                    foreach (GraphData data in market.Data.Where(p => p.Type == c).ToList())
                    {

                        {
                            supply.Series[type].Points.AddXY(data.day, data.Supply);
                            demand.Series[type].Points.AddXY(data.day, data.Demand);
                            chart3.Series[type].Points.AddXY(data.day, data.AmountSold);
                            chart1.Series[type].Points.AddXY(data.day, data.Price);
                            double asdf = data.Price * Market.DailyProductionMinusIncome(Market.CommodityTypeToOccupation(data.Type), data.day) * (data.Demand > data.Supply ? 1 : data.Demand / data.Supply);

                            if (asdf > 4)
                                asdf = 4;
                            priceDemand.Series[type].Points.AddXY(data.day, asdf);
                        }
                    }
                    type++;
                }
                catch (Exception e)
                {

                }
            }
            type = 0;
            foreach (MarketData md in market.MarketData)
            {
                MarketMoney.Series[0].Points.AddXY(md.day, md.MarketMoney);
            }
            foreach (Occupation o in Enum.GetValues(typeof(Occupation)))
            {
                try
                {
                    List<OccupationData> CommodityData = market.OccupationD.Where(p => p.job == o).ToList();
                    int x = 0;
                    foreach (OccupationData com in CommodityData)
                    {
                        if (com.day == 4999)
                        {
                            int x222 = 5;
                        }
                        chart2.Series[type].Points.AddXY(com.day, com.workers);
                        agentmoney.Series[type].Points.AddXY(com.day, com.money / com.workers);
                        agentincome.Series[type].Points.AddXY(com.day, com.income / com.workers);
                        percentBought.Series[type].Points.AddXY(com.day, com.PercentBought);
                    }
                    type++;
                }
                catch(Exception e)
                {

                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
