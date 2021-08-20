using AuctionAPI.Data;
using AuctionAPI.Models;
using Microsoft.AspNetCore.Http;
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
    public class BidderController : ControllerBase
    {
        private IBidderService _bidderService;
        public BidderController(IBidderService bidderService)
        {
            _bidderService = bidderService;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(_bidderService.GetBidders());
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            return new JsonResult(_bidderService.GetBidder(id));
        }
    }
}
