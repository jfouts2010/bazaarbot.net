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
            Dictionary<Occupation, int> StartNumber = new Dictionary<Occupation, int>();
            StartNumber.Add(Occupation.Fisher, 41);
            StartNumber.Add(Occupation.Farmer, 128);
            StartNumber.Add(Occupation.CattleRancher, 9);
            StartNumber.Add(Occupation.FruitVegFarmer, 4);
            StartNumber.Add(Occupation.GrapeFarmer, 118);
            StartNumber.Add(Occupation.NutFarmer, 7);
            StartNumber.Add(Occupation.PigRancher, 10);
            StartNumber.Add(Occupation.Woodworker, 83);
            Market data = RunExample(StartNumber);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(data));

        }
        static Market RunExample(Dictionary<Occupation, int> StartNumber)
        {
            int day = 1;
            Market market = new Market();
            List<Commodity> startingResources = new List<Commodity>();
            startingResources = Setup.StartingResourcesBasic();
            Random r = new Random();
            Dictionary<int, int> vales = new Dictionary<int, int>();

            //make agents for each trade
            foreach (Occupation x in Enum.GetValues(typeof(Occupation)))
            {
                for (int i = 0; i < StartNumber[x]; i++)
                {
                    Agent agent = new Agent();
                    agent.Job = x;
                    agent.Money = 500;
                    agent.Commodities = SetCommodities(startingResources);
                    market.Agents.Add(agent);
                }
            }
            //This is Daily Function
            for (int i = 0; i < 2000; i++)
            {
                //daily work for each agent
                foreach (Agent a in market.Agents)
                {
                    Setup.DoJob(a);
                    if (a.Job == Occupation.Woodworker && day > 600 && day < 700)
                    {
                        a.Commodities.First(p => p.Type == CommodityType.Timber).Stock = 0;
                    }
                    CreateTickets(a, market);
                }
                //end of day market work
                market.ResolveTickets(day);
                market.GetOccupationNumbers(day);
                market.MoveAgents(day);
                day++;
            }
            return market;
        }
        //Agent Creating Tickets to buy and sell his goods
        static void CreateTickets(Agent agent, Market market)
        {
            Random rand = new Random();
            double tempMoney = agent.Money;
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
                    if (tempMoney > 0)
                    {
                        Ticket t = new Ticket();
                        t.commodity = c.Type;
                        t.Ideal = c.DesiredStock - c.Stock;
                        t.Price = c.min + ((double)rand.NextDouble() * (c.max - c.min));
                        if (t.Ideal * t.Price > tempMoney)
                            t.Ideal = agent.Money / t.Price;
                        t.limit = 0;
                        t.TicketsAgent = agent;
                        market.Bids.Add(t);
                        tempMoney -= t.Ideal * t.Price;
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
        static public Occupation CommodityTypeToOccupation(CommodityType type)
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
        static public CommodityType OccuptationToCommodityType(Occupation occ)
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
        static public double DailyProductionMinusIncome(Occupation occ)
        {
            if (occ == Occupation.Farmer)
                return 1.8;
            if (occ == Occupation.Fisher)
                return 34.266;
            if (occ == Occupation.Woodworker)
                return 1.93;
            if (occ == Occupation.GrapeFarmer)
                return .734625;
            if (occ == Occupation.CattleRancher)
                return 1.25 * 5 - 1;
            if (occ == Occupation.PigRancher)
                return 10.96 * 5 - 1;
            if (occ == Occupation.FruitVegFarmer)
                return 1.2149;
            else
                return 8.041;
        }
        //move bad agents
        public void MoveAgents(int day)
        {
            Random r = new Random();
            //if the agent has less than 100 dollars, income over past 30 days < 0, and has lost money yesterdya, will attempt to move
            List<Agent> SwitchingAgents = Agents.Where(p => p.Money < 100 && p.Past30DayIncome <= 0 && p.MoneyDifferenceFromYesterday < 0).ToList();
            int switchedAgents = 0;
            foreach (Agent a in SwitchingAgents)
            {
                if (a.daysSinceMove > (30 + r.Next(0, 10)))
                {
                    List<GraphData> x = Data.Where(p => p.day == day).OrderByDescending(p => p.Price * DailyProductionMinusIncome(CommodityTypeToOccupation(p.Type)) * (p.Demand > p.Supply ? 1 : p.Demand / p.Supply)).ToList();
                    foreach (GraphData x2 in x)
                        if (a.Job != CommodityTypeToOccupation(x2.Type))
                        {
                            a.Job = CommodityTypeToOccupation(x2.Type);
                            switchedAgents++;
                            a.daysSinceMove = 0;
                            break;
                        }

                }
            }
            //these are agents that are loosing tons of money, but arent moving because they previously made tons of money
            foreach (Agent a2 in Agents)
            {
                if (a2.Past30DayIncome < 0 && a2.daysSinceMove > 30 + r.Next(0, 1000) && a2.MoneyDifferenceFromYesterday < 0)
                {
                    List<GraphData> x = Data.Where(p => p.day == day).OrderByDescending(p => p.Price * DailyProductionMinusIncome(CommodityTypeToOccupation(p.Type)) * (p.Demand > p.Supply ? 1 : p.Demand / p.Supply)).ToList();
                    foreach (GraphData x2 in x)
                        if (a2.Job != CommodityTypeToOccupation(x2.Type))
                        {
                            a2.Job = CommodityTypeToOccupation(x2.Type);
                            a2.daysSinceMove = 0;
                            switchedAgents++;
                            break;
                        }


                }
                a2.IncomeHistory.Add(a2.MoneyDifferenceFromYesterday);
                a2.Past30DayIncome = 0;
                a2.daysSinceMove++;
                for (int i = a2.IncomeHistory.Count > 30 ? a2.IncomeHistory.Count - 30 : 0; i < a2.IncomeHistory.Count; i++)
                {
                    a2.Past30DayIncome += a2.IncomeHistory[i];
                }
                a2.MoneyDifferenceFromYesterday = 0;
            }
        }
        //where trades occur
        public void ResolveTickets(int now)
        {
            double dailyTax = 0;
            foreach (CommodityType c in Enum.GetValues(typeof(CommodityType)))
            {
                List<Ticket> tempBids = Bids.Where(p => p.commodity == c).OrderByDescending(p => p.Price).ToList();
                List<Ticket> tempAsks = Asks.Where(p => p.commodity == c).OrderBy(p => p.Price).ToList();
                double TodaySupply = 0;
                double TodayDemand = 0;

                double HighestBid = 0;
                if (tempBids.Count > 0)
                    HighestBid = tempBids.First().Price;
                double LowestAsk = 0;
                if (tempAsks.Count > 0)
                    LowestAsk = tempAsks.First().Price;
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
                        ask.TicketsAgent.Money += clearingPrice * amountTraded;// * 0.95;
                        dailyTax += clearingPrice * amountTraded * 0.05;
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
                         t.TicketsAgent.RejectedBid(c, HighestBid);
                    }
                }
                if (tempAsks.Count > 0)
                {
                    foreach (Ticket t in tempAsks)
                    {
                         t.TicketsAgent.RejectedAsk(c, LowestAsk);
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
            Commodities.First(p => p.Type == c).min = (Commodities.First(p => p.Type == c).min + clearingPrice) / 2 * 1.01;
            Commodities.First(p => p.Type == c).max = (Commodities.First(p => p.Type == c).max + clearingPrice) / 2 * .99;
        }
        public void RejectedBid(CommodityType c, double HighestBid)
        {
            Commodities.First(p => p.Type == c).min = HighestBid * .99;
            Commodities.First(p => p.Type == c).max = HighestBid * 1.01;
            if (Commodities.First(p => p.Type == c).min > 9000)
                Commodities.First(p => p.Type == c).min = 9000;
            if (Commodities.First(p => p.Type == c).max > 10000)
                Commodities.First(p => p.Type == c).max = 10000;
        }
        public void RejectedAsk(CommodityType c, double LowestAsk)
        {
            Commodities.First(p => p.Type == c).min = LowestAsk * .98;
            Commodities.First(p => p.Type == c).max = LowestAsk * 1;
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
