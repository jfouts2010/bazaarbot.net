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
            com1.mean = 1;
            Commodity com2 = new Commodity();
            com2.Stock = 10;
            com2.Type = CommodityType.metal;
            com2.mean = 1;
            Commodity com3 = new Commodity();
            com3.Stock = 10;
            com3.Type = CommodityType.ore;
            com3.mean = 1;
            Commodity com4 = new Commodity();
            com4.Stock = 10;
            com4.Type = CommodityType.tools;
            com4.mean = 1;
            Commodity com5 = new Commodity();
            com5.Stock = 10;
            com5.Type = CommodityType.wood;
            com5.mean = 1;
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
                        if(ran.Next(0, 9) == 1)
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
    }
    public class Ticket
    {
        public decimal Price;
        public decimal Ideal;
        public decimal Min;
        public decimal Max;
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
        public decimal mean;
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
