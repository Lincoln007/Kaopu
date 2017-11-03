using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public static class SheetStatisticHelper
    {
        public static Dictionary<string, double> Statistic(List<Sheet> list, string title)
        {
            var SV = new List<SubstancsView>();
            foreach (var item in list)
            {
                if (item.Substancs_Views != null)
                {
                    SV.AddRange(item.Substancs_Views);
                }

            }
            var dict = new Dictionary<string, double>();
            var categorys = SV.GroupBy(e => e.Name).Select(e => e.Key).ToList();
            foreach (var name in categorys)
            {
                var sum = SV.Where(e => e.Name.ToLower() == name).Sum(e => e.Price);
                if (!dict.ContainsKey(title + name))
                {
                    dict.Add(title + name, sum);
                }
            }
            return dict;
        }

        public static Dictionary<string,Dictionary<string,double>> Statistic(List<Sheet> sheets)
        {
            var result = new Dictionary<string, Dictionary<string, double>>();
            var temp = Statistic(sheets.Where(e => e.Type == SheetType.Daily).ToList(), SheetType.Daily.GetDescription());
            result.Add(SheetType.Daily.GetDescription(), temp);
            temp = Statistic(sheets.Where(e => e.Type == SheetType.Transfer && e.Cost.HasValue && e.Cost.Value == Cost.RealPay).ToList(), SheetType.Transfer.GetDescription());
            result.Add(SheetType.Transfer.GetDescription(), temp);
            var errands = sheets.Where(e => e.Type == SheetType.Errand).ToList();
            temp = new Dictionary<string, double>() { { "出差报销差旅费", errands.Sum(e => e.Money) } };
            result.Add(SheetType.Errand.GetDescription(), temp);
            var receptions = sheets.Where(e => e.Type == SheetType.Reception).ToList();
            temp = new Dictionary<string, double>() { { "招待报销招待费", receptions.Sum(e => e.Money) } };
            result.Add(SheetType.Reception.GetDescription(), temp);
            return result;
        }
    }
}
