using Example1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApplication2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Market data = RunExample();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(data));

        }
        static Market RunExample()
        {
            int day = 1;
            Market market = new Market();
            List<Commodity> startingResources = new List<Commodity>();
            startingResources = Setup.StartingResourcesBasic();
            Random r = new Random();
            Dictionary<int, int> vales = new Dictionary<int, int>();

            foreach (Occupation x in Enum.GetValues(typeof(Occupation)))
            {
                for (int i = 0; i < 50; i++)
                {
                   
                    //make agents for each trade
                    Agent agent = new Agent();
                    agent.Job = x;
                    agent.Money = 500;
                    agent.Commodities = SetCommodities(startingResources);
                    market.Agents.Add(agent);
                }
            }
            for (int i = 0; i < 600; i++)
            {
                if (day == 900)
                {
                    int x = 5;
                }
                //one day
                foreach (Agent a in market.Agents)
                {
                   
                    Setup.DoJob(a);
                    CreateTickets(a, market);
                }
                market.ResolveTickets(day);

                market.GetOccupationNumbers(day);
                market.MoveAgents(day);
                day++;
            }
            return market;
        }
        static void CreateTickets(Agent agent, Market market)
        {
            Random rand = new Random();
            foreach (Commodity c in agent.Commodities)
            {
                if (c.Stock > 200)
                    c.Stock = 200;
                if (c.Stock > c.DesiredStock)
                {
                    //lets sell
                    Ticket t = new Ticket();
                    t.commodity = c.Type;
                    t.Ideal = c.Stock - c.DesiredStock;
                    t.Price = c.min + ((double)rand.NextDouble() * (c.max - c.min));
                    t.limit = 0;
                    t.TicketsAgent = agent;
                    market.Asks.Add(t);
                }
                if (c.Stock < c.DesiredStock)
                {
                    //lets buy
                    if (agent.Money > 0)
                    {
                        Ticket t = new Ticket();
                        t.commodity = c.Type;
                        t.Ideal = c.DesiredStock - c.Stock;
                        t.Price = c.min + ((double)rand.NextDouble() * (c.max - c.min));
                        if (t.Ideal * t.Price > agent.Money)
                            t.Ideal = agent.Money / t.Price;
                        t.limit = 0;
                        t.TicketsAgent = agent;
                        market.Bids.Add(t);
                    }
                }
            }
        }
        static List<Commodity> SetCommodities(List<Commodity> startingCom)
        {
            List<Commodity> newCom = new List<Commodity>();
            foreach (Commodity c in startingCom)
            {
                newCom.Add(new Commodity() { DesiredStock = c.DesiredStock, max = c.max, min = c.min, Stock = c.Stock, Type = c.Type });
            }
            return newCom;
        }
       
    }
    public class OccupationData
    {
        public int day;
        public Occupation job;
        public int workers;
        public double money;
        public double income;
    }
    public class GraphData
    {
        public int day { get; set; }
        public double Supply { get; set; }
        public double Demand { get; set; }
        public double AmountSold { get; set; }
        public CommodityType Type { get; set; }
        public double Price { get; set; }
        public double workers { get; set; }
    }
    public class MarketData
    {
        public int day { get; set; }
        public double MarketMoney { get; set; }
        public double TaxIncome { get; set; }
    }
    public class Market
    {
        public List<Ticket> Bids = new List<Ticket>();
        public List<Ticket> Asks = new List<Ticket>();
        public Dictionary<CommodityType, double> SupplybyDemand = new Dictionary<CommodityType, double>();
        public List<Agent> Agents = new List<Agent>();
        public List<GraphData> Data = new List<WindowsFormsApplication2.GraphData>();
        public List<Commodity> GraphData = new List<Commodity>();
        public double MarketMoney;
        public List<MarketData> MarketData = new List<WindowsFormsApplication2.MarketData>();
        public List<OccupationData> OccupationD = new List<OccupationData>();
        public double LastDayHighestBid;
        public double LastDayLowestAsk;
        public Market()
        {

        }
        public void GetOccupationNumbers(int day)
        {
            foreach (Occupation o in Enum.GetValues(typeof(Occupation)))
            {
                OccupationData od = new OccupationData();
                od.day = day;
                od.job = o;
                od.workers = Agents.Where(p => p.Job == o).Count();
                foreach (Agent a in Agents.Where(p => p.Job == o))
                {
                    od.money += a.Money;
                    od.income += a.MoneyDifferenceFromYesterday;
                }
                OccupationD.Add(od);
            }
        }
        public Occupation CommodityTypeToOccupation(CommodityType type)
        {
            if (type == CommodityType.Wheat)
                return Occupation.Farmer;
            else if (type == CommodityType.Fish)
                return Occupation.Fisher;
            else if (type == CommodityType.Timber)
                return Occupation.Woodworker;
            else if (type == CommodityType.Grapes)
                return Occupation.GrapeFarmer;
            else if (type == CommodityType.Cattle)
                return Occupation.CattleRancher;
            else if (type == CommodityType.Pig)
                return Occupation.PigRancher;
            else if (type == CommodityType.FruitVegetables)
                return Occupation.FruitVegFarmer;
            else
                return Occupation.NutFarmer;
        }
        public CommodityType OccuptationToCommodityType(Occupation occ)
        {
            if (occ == Occupation.Fisher)
                return CommodityType.Fish;
            else if (occ == Occupation.Farmer)
                return CommodityType.Wheat;
            else if (occ == Occupation.Woodworker)
                return CommodityType.Timber;
            else if (occ == Occupation.GrapeFarmer)
                return CommodityType.Grapes;
            else if (occ == Occupation.CattleRancher)
                return CommodityType.Cattle;
            else if (occ == Occupation.PigRancher)
                return CommodityType.Pig;
            else if (occ == Occupation.FruitVegFarmer)
                return CommodityType.FruitVegetables;
            else
                return CommodityType.Nuts;
        }
        public void MoveAgents(int day)
        {
            Random r = new Random();
            foreach (Agent a in Agents)
            {
                if (a.Money < 100 && a.Past30DayIncome <= 0 && a.daysSinceMove > (15 + r.Next(0, 15)))
                {
                    List<GraphData> x = Data.Where(p => p.day == day).OrderByDescending(p => p.Price * p.Demand / (p.workers + 1)).ToList();
                    foreach (GraphData x2 in x)
                        if (a.Job != CommodityTypeToOccupation(x2.Type))
                        {

                            a.Job = CommodityTypeToOccupation(x2.Type);
                            if (a.Money < 0)
                            {
                                a.Money += 10;
                                MarketMoney -= 10;
                                //a.Commodities.First(p => p.Type == CommodityType.Wheat).Stock += 3;
                            }
                          
                            a.daysSinceMove = 0;
                            break;
                        }
                }
              /*  else if (a.Past30DayIncome < 0 && a.daysSinceMove > r.Next(0, 1000))
                {
                    List<GraphData> x = Data.Where(p => p.day == day).OrderByDescending(p => p.Price * p.Demand / (p.workers + 1)).ToList();
                    if (a.Job != CommodityTypeToOccupation(x.First().Type))
                    {

                        a.Job = CommodityTypeToOccupation(x.First().Type);
                        if (a.Money < 0)
                        {
                            a.Money = 10;
                            MarketMoney -= 15;
                            a.Commodities.First(p => p.Type == CommodityType.food).Stock += 3;
                        }

                        a.daysSinceMove = 0;
                    }
                }*/
                a.IncomeHistory.Add(a.MoneyDifferenceFromYesterday);
                a.Past30DayIncome = 0;
                a.daysSinceMove++;
                for (int i = a.IncomeHistory.Count > 30 ? a.IncomeHistory.Count - 30 : 0; i < a.IncomeHistory.Count; i++)
                {
                    a.Past30DayIncome += a.IncomeHistory[i];
                }
                a.MoneyDifferenceFromYesterday = 0;
            }
        }
        public void ResolveTickets(int now)
        {
            double dailyTax = 0;
            foreach (CommodityType c in Enum.GetValues(typeof(CommodityType)))
            {
                List<Ticket> tempBids = Bids.Where(p => p.commodity == c).OrderByDescending(p => p.Price).ToList();
                List<Ticket> tempAsks = Asks.Where(p => p.commodity == c).OrderBy(p => p.Price).ToList();
                double TodaySupply = 0;
                double TodayDemand = 0;
                foreach (Ticket bid in tempBids)
                {
                    TodayDemand += bid.Ideal;
                }
                foreach (Ticket ask in tempAsks)
                {
                    TodaySupply += ask.Ideal;
                }

                SupplybyDemand[c] = TodayDemand > 0 ? TodaySupply / TodayDemand : 1000000000;
                int trades = Math.Min(tempAsks.Count, tempBids.Count);
                double amountSold = 0;
                while (tempBids.Count > 0 && tempAsks.Count > 0)
                {
                    Ticket bid = tempBids.First();
                    Ticket ask = tempAsks.First();
                    double amountTraded = Math.Min(bid.Ideal, ask.Ideal);
                    double clearingPrice = (ask.Price + bid.Price) / 2;
                    if (amountTraded > 0)
                    {
                        bid.Ideal -= amountTraded;
                        ask.Ideal -= amountTraded;
                        bid.TicketsAgent.Commodities.First(p => p.Type == c).Stock += amountTraded;
                        ask.TicketsAgent.Commodities.First(p => p.Type == c).Stock -= amountTraded;
                        amountSold += amountTraded;
                        bid.TicketsAgent.Money -= clearingPrice * amountTraded;
                        ask.TicketsAgent.Money += clearingPrice * amountTraded * 0.9;
                        dailyTax += clearingPrice * amountTraded * 0.1;
                        bid.TicketsAgent.MoneyDifferenceFromYesterday -= clearingPrice * amountTraded;
                        ask.TicketsAgent.MoneyDifferenceFromYesterday += clearingPrice * amountTraded;
                        ask.TicketsAgent.AcceptedDeal(clearingPrice, amountTraded, c);
                        bid.TicketsAgent.AcceptedDeal(clearingPrice, amountTraded, c);
                    }
                    if (ask.Ideal == 0)
                        tempAsks.RemoveAt(0);
                    if (bid.Ideal == 0)
                        tempBids.RemoveAt(0);
                }

                if (tempBids.Count > 0)
                {
                    foreach (Ticket t in tempBids)
                    {
                        t.TicketsAgent.RejectedBid(c);
                    }
                }
                if (tempAsks.Count > 0)
                {
                    foreach (Ticket t in tempAsks)
                    {
                       /* if (TodaySupply < TodayDemand)
                            t.TicketsAgent.RejectedAsk(c);*/
                    }
                }
                double GuesstimatePrice = 0;
                foreach (Agent a in Agents)
                {
                    GuesstimatePrice += (a.Commodities.First(p => p.Type == c).min + a.Commodities.First(p => p.Type == c).max) / 2;
                }
                int CommodityWorkers = Agents.Where(p => p.Job == CommodityTypeToOccupation(c)).Count();
                GuesstimatePrice = GuesstimatePrice / Agents.Count;
                Data.Add(new GraphData() { day = now, Supply = TodaySupply, Demand = TodayDemand, Type = c, AmountSold = amountSold, Price = GuesstimatePrice, workers = CommodityWorkers });
            }
            MarketMoney += dailyTax;
            MarketData.Add(new WindowsFormsApplication2.MarketData() { day = now, TaxIncome = dailyTax, MarketMoney = MarketMoney });
            Bids.Clear();
            Asks.Clear();
        }
    }
    public class Ticket
    {
        public double Price;
        public double Ideal;
        public double limit;
        public CommodityType commodity;
        public Agent TicketsAgent;
        public Ticket()
        {

        }
    }
    public class Agent
    {
        public List<Commodity> Commodities = new List<Commodity>();
        public double Money;
        public Occupation Job;
        public int daysSinceMove;
        public double MoneyDifferenceFromYesterday;
        public List<double> IncomeHistory = new List<double>();
        public double Past30DayIncome;
        public Agent()
        {

        }
        public void AcceptedDeal(double clearingPrice, double quantTraded, CommodityType c)
        {
            Commodities.First(p => p.Type == c).min = (Commodities.First(p => p.Type == c).min + clearingPrice * .9) / 2;
            Commodities.First(p => p.Type == c).max = (Commodities.First(p => p.Type == c).max + clearingPrice * 1.1) / 2;
        }
        public void RejectedBid(CommodityType c)
        {
            Commodities.First(p => p.Type == c).max = Commodities.First(p => p.Type == c).max * 1.01;
            Commodities.First(p => p.Type == c).min = Commodities.First(p => p.Type == c).min * 1.01;
            if (Commodities.First(p => p.Type == c).min > 9000)
                Commodities.First(p => p.Type == c).min = 9000;
            if (Commodities.First(p => p.Type == c).max > 10000)
                Commodities.First(p => p.Type == c).max = 10000;
        }
        public void RejectedAsk(CommodityType c)
        {
            Commodities.First(p => p.Type == c).max = Commodities.First(p => p.Type == c).max * .99;
            Commodities.First(p => p.Type == c).min = Commodities.First(p => p.Type == c).min * .99;
            if (Commodities.First(p => p.Type == c).min < 0.1)
                Commodities.First(p => p.Type == c).min = 0.1;
            if (Commodities.First(p => p.Type == c).max < 0.15)
                Commodities.First(p => p.Type == c).max = 0.15;
        }
    }
    public class Commodity
    {
        public CommodityType Type;
        public double Stock;
        public double DesiredStock;
        public double min;
        public double max;
    }
   
}
