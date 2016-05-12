using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class AggregationManager:ManagerBase
    {
        public Aggregation Gain(HttpContextBase context,int year,int month)
        {
            var aggregation = new Aggregation()
            {
                Year = year,
                Month = month,
                Gathers=GetGathers(context),
                EvaluationCollects=GetCollects(context,Company.Evaluation),
                ProjectionCollects=GetCollects(context,Company.Projection)
            };

            return aggregation;
        }

        private Gather GetGather(HttpContextBase context,Company company)
        {
            double val = .0;
            var name = company.ToString();
            return new Gather()
            {
                Company = company,
                Income = double.TryParse(context.Request.Form["IncomeS" + name].ToString(), out val) ? val : .0,
                MarginIncome = double.TryParse(context.Request.Form["MarginIncome" + name].ToString(), out val) ? val : .0,
                Pay = double.TryParse(context.Request.Form["Pay" + name].ToString(), out val) ? val : .0,
                MarginPay = double.TryParse(context.Request.Form["MarginPay" + name].ToString(), out val) ? val : .0,
                Transfer = double.TryParse(context.Request.Form["Transfer" + name].ToString(), out val) ? val : .0,
                Petty = double.TryParse(context.Request.Form["Petty" + name].ToString(), out val) ? val : .0
            };
        }

        public List<Gather> GetGathers(HttpContextBase context)
        {
            var list = new List<Gather>();
            foreach(Company company in Enum.GetValues(typeof(Company)))
            {
                list.Add(GetGather(context, company));
            }
            return list;
        }

        public List<Collect> GetCollects(HttpContextBase context,Company company)
        {
            var name = company.ToString();
            var summargsString = context.Request.Form["Summary" + name];
            var expensesString = context.Request.Form["Expenses" + name];
            var incomesString = context.Request.Form["Income" + name];
            var list = new List<Collect>();
            if (!string.IsNullOrEmpty(summargsString)&& !string.IsNullOrEmpty(expensesString)&& !string.IsNullOrEmpty(incomesString))
            {
                var summarys = summargsString.Split(',');
                var expenses = expensesString.Split(',');
                var incomes = incomesString.Split(',');
                var count = summarys.Count();
                double val = .0;
                for(var i = 0; i < count; i++)
                {
                    list.Add(new Collect()
                    {
                        Summary = summarys[i],
                        Expenses = double.TryParse(expenses[i], out val) ? val : .0,
                        Income = double.TryParse(incomes[i], out val) ? val : .0,
                        Company = company
                    });
                }
            }
            return list;
        }

        public void Update(Aggregation aggregation)
        {
            using (var db = GetDbContext())
            {
                
            }
        }

      
    }
}
