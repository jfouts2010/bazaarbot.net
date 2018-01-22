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
            foreach (CommodityType ct in Enum.GetValues(typeof(CommodityType)))
            {
                Commodity c = new Commodity();
                c.DesiredStock = 1 * 50;
                c.max = 1;
                c.min = 0.8;
                c.Stock = 1;
                c.Type = ct;

                if (ct == CommodityType.Fish)
                {
                    c.min = 1.1;
                    c.max = 1.2;
                }
                if (ct == CommodityType.Wheat)
                {
                    c.min = 11.9;
                    c.max = 12;
                }
                if (ct == CommodityType.Timber)
                {
                    c.min = 11.9;
                    c.max = 12;
                }
              /*  if (ct == CommodityType.Grapes)
                {
                    c.min = 41;
                    c.max = 42;
                }
                if (ct == CommodityType.Cattle)
                {
                    c.min = 4.1;
                    c.max = 4.2;
                }
                if (ct == CommodityType.Pig)
                {
                    c.min = .9;
                    c.max = 1;
                }
                if (ct == CommodityType.FruitVegetables)
                {
                    c.min = 13.2;
                    c.max = 13.8;
                }
                if (ct == CommodityType.Nuts)
                {
                    c.min = 2.7;
                    c.max = 2.9;
                }*/
                startingResources.Add(c);
            }
           // startingResources.First(p => p.Type == CommodityType.Wheat).min = 4;
            //startingResources.First(p => p.Type == CommodityType.Wheat).min = 5;
            return startingResources;
        }

        public static void DoJob(Agent agent)
        {
            Random ran = new Random();
            if (agent.Commodities.First(p => p.Type == CommodityType.Fish).Stock > 0)
            {
                agent.Commodities.First(p => p.Type == CommodityType.Fish).Stock -= 1 * 50;
                agent.LastDayPercentBought += (double)1 / (Enum.GetValues(typeof(CommodityType))).Length;
            }
            if (agent.Commodities.First(p => p.Type == CommodityType.Wheat).Stock > 0)
            {
                agent.Commodities.First(p => p.Type == CommodityType.Wheat).Stock -= 1 * 50;
                agent.LastDayPercentBought += (double)1 / (Enum.GetValues(typeof(CommodityType))).Length;
            }
            if (agent.Commodities.First(p => p.Type == CommodityType.Timber).Stock > 0)
            {
                agent.Commodities.First(p => p.Type == CommodityType.Timber).Stock -= 1 * 50;
                agent.LastDayPercentBought += (double)1 / (Enum.GetValues(typeof(CommodityType))).Length;
            }
           /* if (agent.Commodities.First(p => p.Type == CommodityType.Grapes).Stock > 0)
            {
                agent.Commodities.First(p => p.Type == CommodityType.Grapes).Stock -= 1 * 50;
                agent.LastDayPercentBought += (double)1 / (Enum.GetValues(typeof(CommodityType))).Length;
            }
            if (agent.Commodities.First(p => p.Type == CommodityType.Cattle).Stock > 0)
            {
                agent.Commodities.First(p => p.Type == CommodityType.Cattle).Stock -= 1 * 50;
                agent.LastDayPercentBought += (double)1 / (Enum.GetValues(typeof(CommodityType))).Length;
            }
            if (agent.Commodities.First(p => p.Type == CommodityType.Pig).Stock > 0)
            {
                agent.Commodities.First(p => p.Type == CommodityType.Pig).Stock -= 1 * 50;
                agent.LastDayPercentBought += (double)1 / (Enum.GetValues(typeof(CommodityType))).Length;
            }
            if (agent.Commodities.First(p => p.Type == CommodityType.FruitVegetables).Stock > 0)
            {
                agent.Commodities.First(p => p.Type == CommodityType.FruitVegetables).Stock -= 1 * 50;
                agent.LastDayPercentBought += (double)1 / (Enum.GetValues(typeof(CommodityType))).Length;
            }
            if (agent.Commodities.First(p => p.Type == CommodityType.Nuts).Stock > 0)
            {
                agent.Commodities.First(p => p.Type == CommodityType.Nuts).Stock -= 1 * 50;
                agent.LastDayPercentBought += (double)1 / (Enum.GetValues(typeof(CommodityType))).Length;
            }*/
            if (agent.Job == Occupation.Farmer)
            {
                foreach(Commodity c in agent.Commodities)
                {
                    c.DesiredStock = 1;
                }
                agent.Commodities.First(p => p.Type == CommodityType.Wheat).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.Wheat).Stock += 1.4*2 * 50;
            }
            if (agent.Job == Occupation.Fisher)
            {
                foreach (Commodity c in agent.Commodities)
                {
                    c.DesiredStock = 1*50;
                }
                agent.Commodities.First(p => p.Type == CommodityType.Fish).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.Fish).Stock += 35.266 * 50;
            }
            if (agent.Job == Occupation.Woodworker)
            {
                foreach (Commodity c in agent.Commodities)
                {
                    c.DesiredStock = 1;
                }
                agent.Commodities.First(p => p.Type == CommodityType.Timber).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.Timber).Stock += 2.93 * 50;
            }
          /*  if (agent.Job == Occupation.GrapeFarmer)
            {
                foreach (Commodity c in agent.Commodities)
                {
                    c.DesiredStock = 1;
                }
                agent.Commodities.First(p => p.Type == CommodityType.Grapes).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.Grapes).Stock += 1.734625 * 50;
            }
            if (agent.Job == Occupation.CattleRancher)
            {
                foreach (Commodity c in agent.Commodities)
                {
                    c.DesiredStock = 1;
                }
                agent.Commodities.First(p => p.Type == CommodityType.Cattle).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.Cattle).Stock += 1.25*5 * 50;
            }
            if (agent.Job == Occupation.PigRancher)
            {
                foreach (Commodity c in agent.Commodities)
                {
                    c.DesiredStock = 1;
                }
                agent.Commodities.First(p => p.Type == CommodityType.Pig).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.Pig).Stock += 10.96*5 * 50;
            }
            if (agent.Job == Occupation.FruitVegFarmer)
            {
                foreach (Commodity c in agent.Commodities)
                {
                    c.DesiredStock = 1;
                }
                agent.Commodities.First(p => p.Type == CommodityType.FruitVegetables).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.FruitVegetables).Stock += 2.2149 * 50;
            }
            if (agent.Job == Occupation.NutFarmer)
            {
                foreach (Commodity c in agent.Commodities)
                {
                    c.DesiredStock = 1;
                }
                agent.Commodities.First(p => p.Type == CommodityType.Nuts).DesiredStock = 0;
                agent.Commodities.First(p => p.Type == CommodityType.Nuts).Stock += 9.041 * 50;
            }*/
           
        }

    }
    public enum CommodityType
    {
        Fish,
        Wheat,
        Timber,
       /* Grapes,
        Cattle,
        Pig,
        FruitVegetables,
        Nuts*/
    }
    public enum Occupation
    {
        Fisher,
        Farmer,
        Woodworker,
       /* GrapeFarmer,
        CattleRancher,
        PigRancher,
        FruitVegFarmer,
        NutFarmer*/
    }
}
