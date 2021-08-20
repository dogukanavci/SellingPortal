using AuctionAPI.Data;
using AuctionAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [Route("GetItems")]
        public JsonResult GetItems(BaseGetterObject baseGetterObject)
        {
            var result = new { TotalCount = _itemService.CountItems(baseGetterObject.SearchTerm), Items = _itemService.GetItems(baseGetterObject) };
            return new JsonResult(result);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            return new JsonResult(_itemService.GetItem(id));
        }
    }
}
