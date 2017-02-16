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
            Commodity com1 = new Commodity();
            com1.Stock = 10;
            com1.DesiredStock = 10;
            com1.Type = CommodityType.food;
            com1.min = 0.8;
            com1.max = 1;
            Commodity com2 = new Commodity();
            com2.DesiredStock = 10;
            com2.Stock = 10;
            com2.Type = CommodityType.metal;
            com2.min = 0.8;
            com2.max = 1;
            Commodity com3 = new Commodity();
            com3.DesiredStock = 10;
            com3.Stock = 10;
            com3.Type = CommodityType.ore;
            com3.min = 0.8;
            com3.max = 1;
            Commodity com4 = new Commodity();
            com4.DesiredStock = 10;
            com4.Stock = 10;
            com4.Type = CommodityType.tools;
            com4.min = 0.8;
            com4.max = 1;
            Commodity com5 = new Commodity();
            com5.DesiredStock = 10;
            com5.Stock = 10;
            com5.Type = CommodityType.wood;
            com5.min = 0.8;
            com5.max = 1;
            startingResources.Add(com1); startingResources.Add(com2); startingResources.Add(com3); startingResources.Add(com4); startingResources.Add(com5);
            foreach (Occupation x in Enum.GetValues(typeof(Occupation)))
            {
                for (int i = 0; i < 10; i++)
                {
                    //make agents for each trade
                    Agent agent = new Agent();
                    agent.Job = x;
                    agent.Money = 500;
                    agent.Commodities = startingResources;
                    market.Agents.Add(agent);
                }
            }
            for (int i = 0; i < 100; i++)
            {
                //one day
                foreach (Agent a in market.Agents)
                {
                    DoJob(a);
                    CreateTickets(a, market);
                }
                market.ResolveTickets(day);
                market.MoveAgents();
                market.GetOccupationNumbers(day);
                day++;
            }
            return market;
        }
        static void CreateTickets(Agent agent, Market market)
        {
            Random rand = new Random();
            foreach (Commodity c in agent.Commodities)
            {
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
        static void DoJob(Agent agent)
        {
            Random ran = new Random();
            if (agent.Job == Occupation.farmer)
            {
                agent.Commodities.First(p => p.Type == CommodityType.food).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.tools).DesiredStock = 10;
                agent.Commodities.First(p => p.Type == CommodityType.wood).DesiredStock = 10;
                agent.Commodities.First(p => p.Type == CommodityType.ore).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.metal).DesiredStock = 0;
                if (agent.Commodities.First(p => p.Type == CommodityType.wood).Stock >= 1)
                {
                    if (agent.Commodities.First(p => p.Type == CommodityType.tools).Stock > 0)
                    {
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock += 4;
                        agent.Commodities.First(p => p.Type == CommodityType.wood).Stock--;
                        if (ran.Next(0, 9) == 1)
                            agent.Commodities.First(p => p.Type == CommodityType.tools).Stock--;
                    }
                    else
                    {
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock += 2;
                        agent.Commodities.First(p => p.Type == CommodityType.wood).Stock--;
                        if (ran.Next(0, 9) == 1)
                            agent.Commodities.First(p => p.Type == CommodityType.tools).Stock--;
                    }
                }
                else
                {
                    agent.Money -= 2;
                }
            }

            if (agent.Job == Occupation.miner)
            {
                agent.Commodities.First(p => p.Type == CommodityType.food).DesiredStock = 10;
                agent.Commodities.First(p => p.Type == CommodityType.ore).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.metal).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.tools).DesiredStock = 10;
                agent.Commodities.First(p => p.Type == CommodityType.wood).DesiredStock = 0;
                if (agent.Commodities.First(p => p.Type == CommodityType.food).Stock >= 1)
                {
                    if (agent.Commodities.First(p => p.Type == CommodityType.tools).Stock > 0)
                    {
                        agent.Commodities.First(p => p.Type == CommodityType.ore).Stock += 4;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
                        if (ran.Next(0, 9) == 1)
                            agent.Commodities.First(p => p.Type == CommodityType.tools).Stock--;
                    }
                    else
                    {
                        agent.Commodities.First(p => p.Type == CommodityType.ore).Stock += 2;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
                        if (ran.Next(0, 9) == 1)
                            agent.Commodities.First(p => p.Type == CommodityType.tools).Stock--;
                    }
                }
                else
                {
                    agent.Money -= 2;
                }
            }

            if (agent.Job == Occupation.refiner)
            {
                agent.Commodities.First(p => p.Type == CommodityType.metal).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.ore).DesiredStock = 10;
                agent.Commodities.First(p => p.Type == CommodityType.food).DesiredStock = 10;
                agent.Commodities.First(p => p.Type == CommodityType.tools).DesiredStock = 10;
                agent.Commodities.First(p => p.Type == CommodityType.wood).DesiredStock = 0;
                if (agent.Commodities.First(p => p.Type == CommodityType.food).Stock >= 1)
                {
                    if (agent.Commodities.First(p => p.Type == CommodityType.tools).Stock > 0)
                    {
                        agent.Commodities.Find(p => p.Type == CommodityType.metal).Stock += agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock;
                        agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock = 0;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
                        if (ran.Next(0, 9) == 1)
                            agent.Commodities.First(p => p.Type == CommodityType.tools).Stock--;
                    }
                    else
                    {
                        agent.Commodities.Find(p => p.Type == CommodityType.metal).Stock += agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock / 2;
                        agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock = 0;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
                        if (ran.Next(0, 9) == 1)
                            agent.Commodities.First(p => p.Type == CommodityType.tools).Stock--;
                    }
                }
                else
                {
                    agent.Money -= 2;
                }
            }

            if (agent.Job == Occupation.woodcutter)
            {
                agent.Commodities.First(p => p.Type == CommodityType.wood).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.food).DesiredStock = 10;
                agent.Commodities.First(p => p.Type == CommodityType.ore).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.metal).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.tools).DesiredStock = 10;
                if (agent.Commodities.First(p => p.Type == CommodityType.food).Stock >= 1)
                {
                    if (agent.Commodities.First(p => p.Type == CommodityType.tools).Stock > 0)
                    {
                        agent.Commodities.First(p => p.Type == CommodityType.wood).Stock += 2;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
                        if (ran.Next(0, 9) == 1)
                            agent.Commodities.First(p => p.Type == CommodityType.tools).Stock--;
                    }
                    else
                    {
                        agent.Commodities.First(p => p.Type == CommodityType.wood).Stock += 1;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
                        if (ran.Next(0, 9) == 1)
                            agent.Commodities.First(p => p.Type == CommodityType.tools).Stock--;
                    }
                }
                else
                {
                    agent.Money -= 2;
                }
            }

            if (agent.Job == Occupation.blacksmith)
            {
                agent.Commodities.First(p => p.Type == CommodityType.tools).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.metal).DesiredStock = 10;
                if (agent.Commodities.First(p => p.Type == CommodityType.food).Stock >= 1)
                {
                    if (agent.Commodities.First(p => p.Type == CommodityType.tools).Stock > 0)
                    {
                        agent.Commodities.Find(p => p.Type == CommodityType.metal).Stock += agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock;
                        agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock = 0;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
                        if (ran.Next(0, 9) == 1)
                            agent.Commodities.First(p => p.Type == CommodityType.tools).Stock--;
                    }
                    else
                    {
                        agent.Commodities.Find(p => p.Type == CommodityType.metal).Stock += agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock / 2;
                        agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock = 0;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
                        if (ran.Next(0, 9) == 1)
                            agent.Commodities.First(p => p.Type == CommodityType.tools).Stock--;
                    }
                }
                else
                {
                    agent.Money -= 2;
                }
            }
        }
    }
    public class OccupationData
    {
        public int day;
        public Occupation job;
        public int workers;
    }
    public class Market
    {
        public List<Ticket> Bids = new List<Ticket>();
        public List<Ticket> Asks = new List<Ticket>();
        public Dictionary<CommodityType, double> SupplybyDemand = new Dictionary<CommodityType, double>();
        public List<Agent> Agents = new List<Agent>();
        public List<Commodity> GraphData = new List<Commodity>();
        public List<OccupationData> OccupationD = new List<OccupationData>();
        public Market()
        {

        }
        public void GetOccupationNumbers(int day)
        {
            foreach(Occupation o in Enum.GetValues(typeof(Occupation)))
            {
                OccupationData od = new OccupationData();
                od.day = day;
                od.job = o;
                od.workers = Agents.Where(p => p.Job == o).Count();
                OccupationD.Add(od);
            }
        }
        public Occupation CommodityTypeToOccupation(CommodityType type)
        {
            if (type == CommodityType.food)
                return Occupation.farmer;
            else if (type == CommodityType.metal)
                return Occupation.refiner;
            else if (type == CommodityType.ore)
                return Occupation.miner;
            else if (type == CommodityType.tools)
                return Occupation.blacksmith;
            else 
                return Occupation.woodcutter;
        }
        public void MoveAgents()
        {
            foreach(Agent a in Agents)
            {
                if(a.Money < 0)
                {
                    CommodityType highestType = SupplybyDemand.OrderBy(p => p.Value).First().Key;
                    a.Job = CommodityTypeToOccupation(highestType);
                    a.Money = 10;
                }
            }
        }
        public void ResolveTickets(int now)
        {
            foreach (CommodityType c in Enum.GetValues(typeof(CommodityType)))
            {
                List<Ticket> tempBids = Bids.Where(p => p.commodity == c).OrderByDescending(p => p.Price).ToList();
                List<Ticket> tempAsks = Asks.Where(p => p.commodity == c).OrderBy(p => p.Price).ToList();
                double TodaySupply = 0;
                double TodayDemand = 0;
                foreach(Ticket bid in tempBids)
                {
                    TodayDemand += bid.Ideal;
                }
                foreach(Ticket ask in tempAsks)
                {
                    TodaySupply += ask.Ideal;
                }
                SupplybyDemand[c] = TodayDemand > 0? TodaySupply / TodayDemand : 1000000000;
                int trades = Math.Min(tempAsks.Count, Bids.Count);
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
                        bid.TicketsAgent.Money += clearingPrice * amountTraded;
                        ask.TicketsAgent.Money -= clearingPrice * amountTraded;
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
                       // t.TicketsAgent.RejectedBid(c);
                    }
                }
                if (tempAsks.Count > 0)
                {
                    foreach (Ticket t in tempAsks)
                    {
                       // if(TodaySupply < TodayDemand)
                          //  t.TicketsAgent.RejectedAsk(c);
                    }
                }
                double GuesstimatePrice = 0;
                foreach(Agent a in Agents)
                {
                    GuesstimatePrice += (a.Commodities.First(p => p.Type == c).min + a.Commodities.First(p => p.Type == c).max) / 2;
                }
                GuesstimatePrice = GuesstimatePrice / Agents.Count;
                Commodity tempCom = new Commodity();
                tempCom.Type = c;
                tempCom.max = GuesstimatePrice;
                this.GraphData.Add(tempCom);
            }
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
    public enum CommodityType
    {
        wood,
        food,
        ore,
        metal,
        tools
    }
    public enum Occupation
    {
        woodcutter,
        farmer,
        miner,
        refiner,
        blacksmith
    }
}
