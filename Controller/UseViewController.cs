using Microsoft.AspNetCore.Mvc;
using MigrationScripTest.Data;

namespace MigrationScripTest.Controller
{
    public class UseViewController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        //public UseViewController(DatabaseContext databaseContext) {
        //    _databaseContext = databaseContext;
        //}

        //[HttpGet("GetExpenseByTotalFromTable")]
        //public IActionResult GetExpenseByTotalTable()
        //{
        //    var items = _databaseContext
        //                   .ExpenseItems
        //                   .Select(x => new
        //                   {
        //                       id = x.Id,
        //                       name = x.Name,
        //                       category = x.Category,
        //                       total = x.History.Sum(r => r.Amount)
        //                   })
        //                   .OrderByDescending(x => x.total);
        //    return Ok(items);
        //}
        
        //[HttpGet("GetExpenseByTotalFromView")]
        //public IActionResult GetExpenseByTotalFromView()
        //{
        //    var items = _databaseContext
        //                   .ExpenseTotals
        //                   .Select(x => new
        //                   {
        //                       id = x.Id,
        //                       name = x.Name,
        //                       category = x.Category,
        //                       total = x.TotalAmount,
        //                       staticColumn = x.StaticColumn
        //                   })
        //                   .OrderByDescending(x => x.total);
        //    return Ok(items);
        //}
    }
}
