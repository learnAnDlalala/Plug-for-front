using Microsoft.AspNetCore.Mvc;
using ScrumPoker.Data;
using ScrumPoker.Data.Models;
using ScrumPoker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.Controllers
{
  /// <summary>
  /// Контроллер карт.
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class CardController:Controller
  {
    private readonly RoundCardService roundCardService;
    private readonly ModelContext db;
    public CardController(RoundCardService roundCard, ModelContext dbContext)
    {
      this.roundCardService = roundCard;
      this.db = dbContext;
    }

    [HttpPost]
    public async Task ChooseCard (RoundCard card)
    {
      await this.roundCardService.UserChoose(this.db,card);
      
    }
  }
}
