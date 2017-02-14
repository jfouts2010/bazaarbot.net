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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            RunExample();
        }
        static void RunExample()
        {
            Market market = new Market();
            List<Commodity> startingResources = new List<Commodity>();
            Commodity com1 = new Commodity();
            com1.Stock = 10;
            com1.Type = CommodityType.food;
            com1.min = 0.8M;
            com1.max = 1;
            Commodity com2 = new Commodity();
            com2.Stock = 10;
            com2.Type = CommodityType.metal;
            com2.min = 0.8M;
            com2.max = 1;
            Commodity com3 = new Commodity();
            com3.Stock = 10;
            com3.Type = CommodityType.ore;
            com3.min = 0.8M;
            com3.max = 1;
            Commodity com4 = new Commodity();
            com4.Stock = 10;
            com4.Type = CommodityType.tools;
            com4.min = 0.8M;
            com4.max = 1;
            Commodity com5 = new Commodity();
            com5.Stock = 10;
            com5.Type = CommodityType.wood;
            com5.min = 0.8M;
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
            for (int i = 0; i < 10000; i++)
            {
                //one day
                foreach (Agent a in market.Agents)
                {
                    DoJob(a);
                    CreateTickets(a, market);
                }
                market.ResolveTickets();
            }
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
                    t.Price = c.min + ((decimal)rand.NextDouble() * (c.max - c.min));
                    t.limit = 0;
                    t.TicketsAgent = agent;
                    market.Bids.Add(t);
                }
                if (c.Stock < c.DesiredStock)
                {
                    //lets buy
                    Ticket t = new Ticket();
                    t.commodity = c.Type;
                    t.Ideal = c.Stock - c.DesiredStock;
                    t.Price = c.min + ((decimal)rand.NextDouble() * (c.max - c.min));
                    t.limit = 0;
                    t.TicketsAgent = agent;
                    market.Asks.Add(t);
                }
            }
        }
        static void DoJob(Agent agent)
        {
            Random ran = new Random();
            if (agent.Job == Occupation.farmer)
            {
                agent.Commodities.First(p => p.Type == CommodityType.food).DesiredStock = 0;
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
                agent.Commodities.First(p => p.Type == CommodityType.ore).DesiredStock = 0;
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
    public class Market
    {
        public List<Ticket> Bids = new List<Ticket>();
        public List<Ticket> Asks = new List<Ticket>();
        public List<Agent> Agents = new List<Agent>();
        public Market()
        {

        }
        public void ResolveTickets()
        {
            foreach (CommodityType c in Enum.GetValues(typeof(CommodityType)))
            {
                Bids = Bids.Where(p => p.commodity == c).OrderByDescending(p => p.Price).ToList();
                Asks = Asks.Where(p => p.commodity == c).OrderBy(p => p.Price).ToList();
                int trades = Math.Min(Asks.Count, Bids.Count);
                while (Bids.Count > 0 && Asks.Count > 0)
                {
                    Ticket bid = Bids.First();
                    Ticket ask = Asks.First();
                    decimal amountTraded = Math.Min(bid.Ideal, ask.Ideal);
                    decimal clearingPrice = (ask.Price + bid.Price) / 2;
                    if (amountTraded > 0)
                    {
                        bid.Ideal -= amountTraded;
                        ask.Ideal -= amountTraded;
                        bid.TicketsAgent.Commodities.First(p => p.Type == c).Stock += amountTraded;
                        ask.TicketsAgent.Commodities.First(p => p.Type == c).Stock -= amountTraded;
                        bid.TicketsAgent.Money += clearingPrice * amountTraded;
                        ask.TicketsAgent.Money -= clearingPrice * amountTraded;
                        //do something for buyer and seller confirming price is good
                    }
                    if (ask.Ideal == 0)
                        Asks.RemoveAt(0);
                    if (bid.Ideal == 0)
                        Bids.RemoveAt(0);
                }
                if (Bids.Count > 0)
                {
                    foreach (Ticket t in Bids)
                    {
                        //do something for each bid that is left over
                    }
                }
                if (Asks.Count > 0)
                {
                    foreach (Ticket t in Asks)
                    {
                        //do something for each ask that is left over
                    }
                }
            }
            Bids.Clear();
            Asks.Clear();
        }
    }
    public class Ticket
    {
        public decimal Price;
        public decimal Ideal;
        public decimal limit;
        public CommodityType commodity;
        public Agent TicketsAgent;
        public Ticket()
        {

        }
    }
    public class Agent
    {
        public List<Commodity> Commodities = new List<Commodity>();
        public decimal Money;
        public Occupation Job;
        public Agent()
        {

        }
    }
    public class Commodity
    {
        public CommodityType Type;
        public decimal Stock;
        public decimal DesiredStock;
        public decimal min;
        public decimal max;
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
