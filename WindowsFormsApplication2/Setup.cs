using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2;

namespace Example1
{
    class Setup
    {
        public static List<Commodity> StartingResourcesBasic()
        {
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
            return startingResources;
        }

        public static void DoJob(Agent agent)
        {
            Random ran = new Random();
            if (agent.Job == Occupation.farmer)
            {
                agent.Commodities.First(p => p.Type == CommodityType.food).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.tools).DesiredStock = 1;
                agent.Commodities.First(p => p.Type == CommodityType.wood).DesiredStock = 10;
                agent.Commodities.First(p => p.Type == CommodityType.ore).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.metal).DesiredStock = 0;
                if (agent.Commodities.First(p => p.Type == CommodityType.wood).Stock >= 1)
                {
                    if (agent.Commodities.First(p => p.Type == CommodityType.tools).Stock >= 1)
                    {
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock += 4;
                        agent.Commodities.First(p => p.Type == CommodityType.wood).Stock--;
                        agent.Commodities.First(p => p.Type == CommodityType.tools).Stock -= 0.2;
                    }
                    else
                    {
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock += 2;
                        agent.Commodities.First(p => p.Type == CommodityType.wood).Stock--;
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
                agent.Commodities.First(p => p.Type == CommodityType.tools).DesiredStock = 1;
                agent.Commodities.First(p => p.Type == CommodityType.wood).DesiredStock = 0;
                if (agent.Commodities.First(p => p.Type == CommodityType.food).Stock >= 1)
                {
                    if (agent.Commodities.First(p => p.Type == CommodityType.tools).Stock >= 1)
                    {
                        agent.Commodities.First(p => p.Type == CommodityType.ore).Stock += 4;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
                        agent.Commodities.First(p => p.Type == CommodityType.tools).Stock -= 0.2;
                    }
                    else
                    {
                        agent.Commodities.First(p => p.Type == CommodityType.ore).Stock += 2;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
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
                if (agent.Commodities.Find(p => p.Type == CommodityType.metal).Stock >= 20)
                    agent.Commodities.First(p => p.Type == CommodityType.ore).DesiredStock = 0;
                else
                    agent.Commodities.First(p => p.Type == CommodityType.ore).DesiredStock = 5;
                agent.Commodities.First(p => p.Type == CommodityType.food).DesiredStock = 10;
                agent.Commodities.First(p => p.Type == CommodityType.tools).DesiredStock = 1;
                agent.Commodities.First(p => p.Type == CommodityType.wood).DesiredStock = 0;
                if (agent.Commodities.First(p => p.Type == CommodityType.food).Stock >= 1)
                {
                    if (agent.Commodities.First(p => p.Type == CommodityType.tools).Stock >= 1)
                    {
                        if (agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock > 0)
                        {
                            agent.Commodities.First(p => p.Type == CommodityType.tools).Stock -= 0.2;
                        }
                        agent.Commodities.Find(p => p.Type == CommodityType.metal).Stock += agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock;
                        agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock = 0;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;

                    }
                    else
                    {
                        if (agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock > 0)
                        {
                            agent.Commodities.Find(p => p.Type == CommodityType.metal).Stock += agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock / 2;
                            agent.Commodities.Find(p => p.Type == CommodityType.ore).Stock = 0;
                            agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
                        }
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
                agent.Commodities.First(p => p.Type == CommodityType.tools).DesiredStock = 1;
                if (agent.Commodities.First(p => p.Type == CommodityType.food).Stock >= 1)
                {
                    if (agent.Commodities.First(p => p.Type == CommodityType.tools).Stock >= 1)
                    {
                        agent.Commodities.First(p => p.Type == CommodityType.wood).Stock += 2;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
                        agent.Commodities.First(p => p.Type == CommodityType.tools).Stock -= 0.2;
                    }
                    else
                    {
                        agent.Commodities.First(p => p.Type == CommodityType.wood).Stock += 1;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
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
                agent.Commodities.First(p => p.Type == CommodityType.food).DesiredStock = 10;
                agent.Commodities.First(p => p.Type == CommodityType.wood).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.ore).DesiredStock = 0;
                if (agent.Commodities.Find(p => p.Type == CommodityType.tools).Stock >= 20)
                    agent.Commodities.First(p => p.Type == CommodityType.metal).DesiredStock = 0;
                else
                    agent.Commodities.First(p => p.Type == CommodityType.metal).DesiredStock = 5;
                if (agent.Commodities.First(p => p.Type == CommodityType.food).Stock >= 1)
                {
                    if (agent.Commodities.Find(p => p.Type == CommodityType.metal).Stock > 0)
                    {
                        agent.Commodities.Find(p => p.Type == CommodityType.tools).Stock += agent.Commodities.Find(p => p.Type == CommodityType.metal).Stock;
                        agent.Commodities.Find(p => p.Type == CommodityType.metal).Stock = 0;
                        agent.Commodities.First(p => p.Type == CommodityType.food).Stock--;
                    }
                }
                else
                {
                    agent.Money -= 2;
                }
            }
        }
       
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
